
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
Imports Newtonsoft.Json.Linq

Public Module SFTPTools

    ''' <summary>
    ''' Tries to connect to the FTP server
    ''' </summary>
    ''' <param name="address">An FTP server. You may specify it like "[address]" or "[address]:[port]"</param>
    Public Sub SFTPTryToConnect(address As String)
        If SFTPConnected = True Then
            W(DoTranslation("You should disconnect from server before connecting to another server"), True, ColTypes.Error)
        Else
            Try
                'Create an SFTP stream to connect to
                Dim SftpHost As String = address.Replace("sftp://", "").Replace(address.Substring(address.LastIndexOf(":")), "")
                Dim SftpPort As String = address.Replace("sftp://", "").Replace(SftpHost + ":", "")

                'Check to see if no port is provided by client
                If SftpHost = SftpPort Then
                    SftpPort = 22
                End If

                'Prompt for username
                If Not String.IsNullOrWhiteSpace(SFTPUserPromptStyle) Then
                    W(ProbePlaces(SFTPUserPromptStyle), False, ColTypes.Input, address)
                Else
                    W(DoTranslation("Username for {0}: "), False, ColTypes.Input, address)
                End If
                SFTPUser = Console.ReadLine()
                If SFTPUser = "" Then
                    Wdbg(DebugLevel.W, "User is not provided. Fallback to ""anonymous""")
                    SFTPUser = "anonymous"
                End If

                'Make a new instance of SftpClient
                ClientSFTP = New SftpClient(PromptConnectionInfo(SftpHost, SftpPort, SFTPUser))

                'Connect to SFTP
                ConnectSFTP()
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
    ''' Tries to connect to the SFTP server.
    ''' </summary>
    Private Sub ConnectSFTP()
        'Connect
        W(DoTranslation("Trying to connect to {0}..."), True, ColTypes.Neutral, ClientSFTP.ConnectionInfo.Host)
        Wdbg(DebugLevel.I, "Connecting to {0} with {1}...", ClientSFTP.ConnectionInfo.Host)
        ClientSFTP.Connect()

        'Show that it's connected
        W(DoTranslation("Connected to {0}"), True, ColTypes.Neutral, ClientSFTP.ConnectionInfo.Host)
        Wdbg(DebugLevel.I, "Connected.")
        SFTPConnected = True

        'Prepare to print current SFTP directory
        SFTPCurrentRemoteDir = ClientSFTP.WorkingDirectory
        Wdbg(DebugLevel.I, "Working directory: {0}", SFTPCurrentRemoteDir)
        SFTPSite = ClientSFTP.ConnectionInfo.Host
        SFTPUser = ClientSFTP.ConnectionInfo.Username

        'Write connection information to Speed Dial file if it doesn't exist there
        Dim SpeedDialEntries As Dictionary(Of String, JToken) = ListSpeedDialEntries(SpeedDialType.SFTP)
        Wdbg(DebugLevel.I, "Speed dial length: {0}", SpeedDialEntries.Count)
        If SpeedDialEntries.ContainsKey(SFTPSite) Then
            Wdbg(DebugLevel.I, "Site already there.")
            Exit Sub
        Else
            'Speed dial format is below:
            'Site,Port,Username
            If SFTPNewConnectionsToSpeedDial Then AddEntryToSpeedDial(SFTPSite, ClientSFTP.ConnectionInfo.Port, SFTPUser, SpeedDialType.SFTP)
        End If
    End Sub

    ''' <summary>
    ''' Opens speed dial prompt
    ''' </summary>
    Sub SFTPQuickConnect()
        If File.Exists(GetKernelPath(KernelPathType.SFTPSpeedDial)) Then
            Dim SpeedDialLines As Dictionary(Of String, JToken) = ListSpeedDialEntries(SpeedDialType.SFTP)
            Wdbg(DebugLevel.I, "Speed dial length: {0}", SpeedDialLines.Count)
            Dim Counter As Integer = 1
            Dim Answer As String
            Dim Answering As Boolean = True
            If Not SpeedDialLines.Count = 0 Then
                W(DoTranslation("Select an address to connect to:") + vbNewLine, True, ColTypes.Neutral)
                For Each SpeedDialAddress As String In SpeedDialLines.Keys
                    Wdbg(DebugLevel.I, "Speed dial address: {0}", SpeedDialAddress)
                    W("{0}) {1}, {2}, {3}", True, ColTypes.Option, Counter, SpeedDialAddress, SpeedDialLines(SpeedDialAddress)("Port"), SpeedDialLines(SpeedDialAddress)("User"))
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
                            Wdbg(DebugLevel.I, "Address: {0}, Port: {1}, Username: {2}", Address, Port, Username)
                            ClientSFTP = New SftpClient(PromptConnectionInfo(Address, Port, Username))
                            ConnectSFTP()
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