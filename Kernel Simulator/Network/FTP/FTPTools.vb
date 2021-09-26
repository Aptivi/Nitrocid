
'    Kernel Simulator  Copyright (C) 2018-2021  EoflaOE
'
'    This file is part of Kernel Simulator
'
'    Kernel Simulator is free software: you can redistribute it and/or modify
'    it under the terms of the GNU General Public License as published by
'    the Free Software Foundation, either version 3 of the License, or
'    (at your option) any later version.
'
'    Kernel Simulator is distributed in the hope that it will be useful,
'    but WITHOUT ANY WARRANTY; without even the implied warranty of
'    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'    GNU General Public License for more details.
'
'    You should have received a copy of the GNU General Public License
'    along with this program.  If not, see <https://www.gnu.org/licenses/>.

Imports System.IO
Imports System.Net.Security
Imports Newtonsoft.Json.Linq

Public Module FTPTools

    ''' <summary>
    ''' Prompts user for a password
    ''' </summary>
    ''' <param name="user">A user name</param>
    ''' <param name="Address">A host address</param>
    ''' <param name="Port">A port for the address</param>
    Public Sub PromptForPassword(user As String, Optional Address As String = "", Optional Port As Integer = 0, Optional EncryptionMode As FtpEncryptionMode = FtpEncryptionMode.Explicit)
        'Make a new FTP client object instance (Used in case logging in using speed dial)
        If ClientFTP Is Nothing Then
            ClientFTP = New FtpClient With {
                            .Host = Address,
                            .Port = Port,
                            .RetryAttempts = 3,
                            .EncryptionMode = EncryptionMode
                        }
        End If

        'Prompt for password
        If Not String.IsNullOrWhiteSpace(FtpPassPromptStyle) Then
            W(ProbePlaces(FtpPassPromptStyle), False, ColTypes.Input, user)
        Else
            W(DoTranslation("Password for {0}: "), False, ColTypes.Input, user)
        End If

        'Get input
        FtpPass = ReadLineNoInput("*")
        Console.WriteLine()

        'Set up credentials
        ClientFTP.Credentials = New NetworkCredential(user, FtpPass)

        'Connect to FTP
        ConnectFTP()
    End Sub

    ''' <summary>
    ''' Tries to connect to the FTP server
    ''' </summary>
    ''' <param name="address">An FTP server. You may specify it like "[address]" or "[address]:[port]"</param>
    Public Sub TryToConnect(address As String)
        If FtpConnected = True Then
            W(DoTranslation("You should disconnect from server before connecting to another server"), True, ColTypes.Error)
        Else
            Try
                'Create an FTP stream to connect to
                Dim FtpHost As String = address.Replace("ftpes://", "").Replace("ftps://", "").Replace("ftp://", "").Replace(address.Substring(address.LastIndexOf(":")), "")
                Dim FtpPort As String = address.Replace("ftpes://", "").Replace("ftps://", "").Replace("ftp://", "").Replace(FtpHost + ":", "")

                'Check to see if no port is provided by client
                If FtpHost = FtpPort Then
                    FtpPort = 0 'Used for detecting of SSL is being used or not dynamically on connection
                End If

                'Make a new FTP client object instance
                ClientFTP = New FtpClient With {
                    .Host = FtpHost,
                    .Port = FtpPort,
                    .RetryAttempts = 3,
                    .EncryptionMode = FtpEncryptionMode.Auto
                }

                'Add handler for SSL validation
                If FtpTryToValidateCertificate Then AddHandler ClientFTP.ValidateCertificate, New FtpSslValidation(AddressOf TryToValidate)

                'Prompt for username
                If Not String.IsNullOrWhiteSpace(FtpUserPromptStyle) Then
                    W(ProbePlaces(FtpUserPromptStyle), False, ColTypes.Input, address)
                Else
                    W(DoTranslation("Username for {0}: "), False, ColTypes.Input, address)
                End If
                FtpUser = Console.ReadLine()
                If FtpUser = "" Then
                    Wdbg(DebugLevel.W, "User is not provided. Fallback to ""anonymous""")
                    FtpUser = "anonymous"
                End If

                PromptForPassword(FtpUser)
            Catch ex As Exception
                Wdbg(DebugLevel.W, "Error connecting to {0}: {1}", address, ex.Message)
                WStkTrc(ex)
                If DebugMode = True Then
                    W(DoTranslation("Error when trying to connect to {0}: {1}") + vbNewLine +
                      DoTranslation("Stack Trace: {2}"), True, ColTypes.Error, address, ex.Message, ex.StackTrace)
                Else
                    W(DoTranslation("Error when trying to connect to {0}: {1}"), True, ColTypes.Error, address, ex.Message)
                End If
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Tries to connect to the FTP server.
    ''' </summary>
    Private Sub ConnectFTP()
        'Prepare profiles
        W(DoTranslation("Preparing profiles... It could take several minutes..."), True, ColTypes.Neutral)
        Dim profiles As List(Of FtpProfile) = ClientFTP.AutoDetect(FTPFirstProfileOnly)
        Dim profsel As New FtpProfile
        Wdbg(DebugLevel.I, "Profile count: {0}", profiles.Count)
        If profiles.Count > 1 Then 'More than one profile
            If FtpUseFirstProfile Then
                profsel = profiles(0)
            Else
                W(DoTranslation("More than one profile found. Select one:") + vbNewLine + vbNewLine +
                              "#, " + DoTranslation("Host Name, Username, Data Type, Encoding, Encryption, Protocols"), True, ColTypes.Neutral)
                For i As Integer = 0 To profiles.Count - 1
                    W($"{i + 1}) {profiles(i).Host}, {profiles(i).Credentials.UserName}, {profiles(i).DataConnection}, {profiles(i).Encoding.EncodingName}, {profiles(i).Encryption}, {profiles(i).Protocols}", True, ColTypes.Option)
                Next
                Dim profanswer As Char
                Dim profanswered As Boolean
                While Not profanswered
                    W(vbNewLine + ">> ", False, ColTypes.Input)
                    profanswer = Console.ReadLine
                    Wdbg(DebugLevel.I, "Selection: {0}", profanswer)
                    If IsNumeric(profanswer) Then
                        Try
                            Wdbg(DebugLevel.I, "Profile selected")
                            profsel = profiles(Val(profanswer) - 1)
                            profanswered = True
                        Catch ex As Exception
                            Wdbg(DebugLevel.I, "Profile invalid")
                            W(DoTranslation("Invalid profile selection.") + vbNewLine, True, ColTypes.Error)
                            WStkTrc(ex)
                        End Try
                    End If
                End While
            End If
        ElseIf profiles.Count = 1 Then
            profsel = profiles(0) 'Select first profile
        Else 'Failed trying to get profiles
            W(DoTranslation("Error when trying to connect to {0}: Connection timeout or lost connection"), True, ColTypes.Error, ClientFTP.Host)
            Exit Sub
        End If

        'Connect
        W(DoTranslation("Trying to connect to {0} with profile {1}..."), True, ColTypes.Neutral, ClientFTP.Host, profiles.IndexOf(profsel))
        Wdbg(DebugLevel.I, "Connecting to {0} with {1}...", ClientFTP.Host, profiles.IndexOf(profsel))
        ClientFTP.Connect(profsel)

        'Show that it's connected
        W(DoTranslation("Connected to {0}"), True, ColTypes.Success, ClientFTP.Host)
        Wdbg(DebugLevel.I, "Connected.")
        FtpConnected = True

        'If MOTD exists, show it
        If FtpShowMotd Then
            If ClientFTP.FileExists("welcome.msg") Then
                W(FTPDownloadToString("welcome.msg"), True, ColTypes.Banner)
            ElseIf ClientFTP.FileExists(".message") Then
                W(FTPDownloadToString(".message"), True, ColTypes.Banner)
            End If
        End If

        'Prepare to print current FTP directory
        FtpCurrentRemoteDir = ClientFTP.GetWorkingDirectory
        Wdbg(DebugLevel.I, "Working directory: {0}", FtpCurrentRemoteDir)
        FtpSite = ClientFTP.Host
        FtpUser = ClientFTP.Credentials.UserName

        'Write connection information to Speed Dial file if it doesn't exist there
        Dim SpeedDialEntries As Dictionary(Of String, JToken) = ListSpeedDialEntries(SpeedDialType.FTP)
        Wdbg(DebugLevel.I, "Speed dial length: {0}", SpeedDialEntries.Count)
        If SpeedDialEntries.ContainsKey(FtpSite) Then
            Wdbg(DebugLevel.I, "Site already there.")
            Exit Sub
        Else
            'Speed dial format is below:
            'Site,Port,Username,Encryption
            If FtpNewConnectionsToSpeedDial Then AddEntryToSpeedDial(FtpSite, ClientFTP.Port, FtpUser, SpeedDialType.FTP, ClientFTP.EncryptionMode)
        End If
    End Sub

    ''' <summary>
    ''' Tries to validate certificate
    ''' </summary>
    Public Sub TryToValidate(control As FtpClient, e As FtpSslValidationEventArgs)
        Wdbg(DebugLevel.I, "Certificate checks")
        If e.PolicyErrors = SslPolicyErrors.None Then
            Wdbg(DebugLevel.I, "Certificate accepted.")
            Wdbg(DebugLevel.I, e.Certificate.GetRawCertDataString)
            e.Accept = True
        Else
            Wdbg(DebugLevel.W, $"Certificate error is {e.PolicyErrors}")
            W(DoTranslation("During certificate validation, there are certificate errors. It might be the first time you've connected to the server or the certificate might have been expired. Here's an error:"), True, ColTypes.Error)
            W("- {0}", True, ColTypes.Error, e.PolicyErrors.ToString)
            If FtpAlwaysAcceptInvalidCerts Then
                Wdbg(DebugLevel.W, "Certificate accepted, although there are errors.")
                Wdbg(DebugLevel.I, e.Certificate.GetRawCertDataString)
                e.Accept = True
            Else
                Dim Answer As String = ""
                Do Until Answer.ToLower = "y" Or Answer.ToLower = "n"
                    W(DoTranslation("Are you sure that you want to connect?") + " (y/n) ", False, ColTypes.Question)
                    SetConsoleColor(New Color(InputColor))
                    Answer = Console.ReadKey.KeyChar
                    Console.WriteLine()
                    Wdbg(DebugLevel.I, $"Answer is {Answer}")
                    If Answer.ToLower = "y" Then
                        Wdbg(DebugLevel.W, "Certificate accepted, although there are errors.")
                        Wdbg(DebugLevel.I, e.Certificate.GetRawCertDataString)
                        e.Accept = True
                    ElseIf Answer.ToLower <> "n" Then
                        Wdbg(DebugLevel.W, "Invalid answer.")
                        W(DoTranslation("Invalid answer. Please try again."), True, ColTypes.Error)
                    End If
                Loop
            End If
        End If
    End Sub

    ''' <summary>
    ''' Opens speed dial prompt
    ''' </summary>
    Sub QuickConnect()
        If File.Exists(GetKernelPath(KernelPathType.FTPSpeedDial)) Then
            Dim SpeedDialLines As Dictionary(Of String, JToken) = ListSpeedDialEntries(SpeedDialType.FTP)
            Wdbg(DebugLevel.I, "Speed dial length: {0}", SpeedDialLines.Count)
            Dim Counter As Integer = 1
            Dim Answer As String
            Dim Answering As Boolean = True
            If Not SpeedDialLines.Count = 0 Then
                W(DoTranslation("Select an address to connect to:") + vbNewLine, True, ColTypes.Neutral)
                For Each SpeedDialAddress As String In SpeedDialLines.Keys
                    Wdbg(DebugLevel.I, "Speed dial address: {0}", SpeedDialAddress)
                    W("{0}) {1}, {2}, {3}, {4}", True, ColTypes.Option, Counter, SpeedDialAddress, SpeedDialLines(SpeedDialAddress)("Port"), SpeedDialLines(SpeedDialAddress)("User"), SpeedDialLines(SpeedDialAddress)("FTP Encryption Mode"))
                    Counter += 1
                Next
                Console.WriteLine()
                While Answering
                    W(">> ", False, ColTypes.Input)
                    Answer = Console.ReadLine
                    Wdbg(DebugLevel.I, "Response: {0}", Answer)
                    If IsNumeric(Answer) Then
                        Wdbg(DebugLevel.I, "Response is numeric. IsNumeric(Answer) returned true. Checking to see if in-bounds...")
                        Dim AnswerInt As Integer = Answer
                        If AnswerInt <= SpeedDialLines.Count Then
                            Answering = False
                            Wdbg(DebugLevel.I, "Response is in-bounds. Connecting...")
                            Dim ChosenSpeedDialAddress As String = SpeedDialLines.Keys(AnswerInt - 1)
                            Wdbg(DebugLevel.I, "Chosen connection: {0}", ChosenSpeedDialAddress)
                            Dim Address As String = ChosenSpeedDialAddress
                            Dim Port As String = SpeedDialLines(ChosenSpeedDialAddress)("Port")
                            Dim Username As String = SpeedDialLines(ChosenSpeedDialAddress)("User")
                            Dim Encryption As FtpEncryptionMode = [Enum].Parse(GetType(FtpEncryptionMode), SpeedDialLines(ChosenSpeedDialAddress)("FTP Encryption Mode"))
                            Wdbg(DebugLevel.I, "Address: {0}, Port: {1}, Username: {2}, Encryption: {3}", Address, Port, Username, Encryption)
                            PromptForPassword(Username, Address, Port, Encryption)
                        Else
                            Wdbg(DebugLevel.I, "Response is out-of-bounds. Retrying...")
                            W(DoTranslation("The selection is out of range. Select between 1-{0}. Try again."), True, ColTypes.Error, SpeedDialLines.Count)
                        End If
                    Else
                        Wdbg(DebugLevel.W, "Response isn't numeric. IsNumeric(Answer) returned false.")
                        W(DoTranslation("The selection is not a number. Try again."), True, ColTypes.Error)
                    End If
                End While
            Else
                Wdbg(DebugLevel.E, "Speed dial is empty. Lines count is 0.")
                W(DoTranslation("Speed dial is empty. Connect to a server to add an address to it."), True, ColTypes.Error)
            End If
        Else
            Wdbg(DebugLevel.E, "File doesn't exist.")
            W(DoTranslation("Speed dial doesn't exist. Connect to a server to add an address to it."), True, ColTypes.Error)
        End If
    End Sub

End Module

Class FTPTracer
    Inherits TraceListener 'Both Write and WriteLine do exactly the same thing, which is writing to a debugger.

    ''' <summary>
    ''' Writes any message that the tracer has received to the debugger.
    ''' </summary>
    ''' <param name="Message">A message</param>
    Public Overloads Overrides Sub Write(Message As String)
        Wdbg(DebugLevel.I, Message)
    End Sub

    ''' <summary>
    ''' Writes any message that the tracer has received to the debugger. Please note that this does exactly as Write() since the debugger only supports writing with newlines.
    ''' </summary>
    ''' <param name="Message">A message</param>
    Public Overloads Overrides Sub WriteLine(Message As String)
        Wdbg(DebugLevel.I, Message)
    End Sub
End Class