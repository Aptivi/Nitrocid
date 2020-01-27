
'    Kernel Simulator  Copyright (C) 2018-2020  EoflaOE
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

Module FTPTools

    Public Sub TryToConnect(ByVal address As String)
        If connected = True Then
            W(DoTranslation("You should disconnect from server before connecting to another server", currentLang), True, ColTypes.Neutral)
        Else
            Try
                'Create an FTP stream to connect to
                Dim FtpHost As String = address.Replace("ftps://", "").Replace(address.Substring(address.LastIndexOf(":")), "")
                Dim FtpPort As String = address.Replace("ftps://", "").Replace(FtpHost + ":", "")

                'Check to see if no port is provided by client
                If FtpHost = FtpPort Then
                    FtpPort = 0 'Used for detecting of SSL is being used or not dynamically on connection
                End If

                'Make a new FTP client object instance
                ClientFTP = New FtpClient With {
                    .Host = FtpHost,
                    .Port = FtpPort,
                    .RetryAttempts = 3,
                    .EncryptionMode = FtpEncryptionMode.Explicit
                }

                'Add handler for SSL validation
                AddHandler ClientFTP.ValidateCertificate, New FtpSslValidation(AddressOf TryToValidate)

                'Prompt for username
                W(DoTranslation("Username for {0}: ", currentLang), False, ColTypes.Input, address)
                user = Console.ReadLine()
                If user = "" Then
                    Wdbg("W", "User is not provided. Fallback to ""anonymous""")
                    user = "anonymous"
                End If

                'Prompt for password
                W(DoTranslation("Password for {0}: ", currentLang), False, ColTypes.Input, user)

                'Get input
                pass = ReadLineNoInput()
                Console.WriteLine()

                'Set up credentials
                ClientFTP.Credentials = New NetworkCredential(user, pass)

                'Prepare profiles
                Dim profiles As List(Of FtpProfile) = ClientFTP.AutoDetect(False)
                Dim profsel As New FtpProfile
                Wdbg("I", "Profile count: {0}", profiles.Count)
                If profiles.Count > 1 Then 'More than one profile
                    W(DoTranslation("More than one profile found. Select one:", currentLang) + vbNewLine, True, ColTypes.Neutral)
                    For i As Integer = 1 To profiles.Count - 1
                        W($"{i}: {profiles(i).Host}, {profiles(i).Credentials.UserName}, {profiles(i).DataConnection.ToString}, {profiles(i).Encoding.EncodingName}, {profiles(i).Encryption.ToString}, {profiles(i).Protocols.ToString}", True, ColTypes.Neutral)
                    Next
                    Dim profanswer As Char
                    Dim profanswered As Boolean
                    While profanswered
                        profanswer = Console.ReadKey(True).KeyChar
                        Wdbg("I", "Selection: {0}", profanswer)
                        If IsNumeric(profanswer) Then
                            Try
                                Wdbg("I", "Profile selected")
                                profsel = profiles(Val(profanswer))
                                profanswered = True
                            Catch ex As Exception
                                Wdbg("I", "Profile invalid")
                                W(DoTranslation("Invalid profile selection.", currentLang) + vbNewLine, True, ColTypes.Neutral)
                                WStkTrc(ex)
                            End Try
                        End If
                    End While
                Else
                    profsel = profiles(0) 'Select first profile
                End If

                'Connect
                W(DoTranslation("Trying to connect to {0} with profile {1}...", currentLang), True, ColTypes.Neutral, address, profiles.IndexOf(profsel))
                Wdbg("I", "Connecting to {0} with {1}...", address, profiles.IndexOf(profsel))
                ClientFTP.Connect(profsel)

                'Show that it's connected
                W(DoTranslation("Connected to {0}", currentLang), True, ColTypes.Neutral, address)
                Wdbg("I", "Connected.")
                connected = True

                'Prepare to print current FTP directory
                currentremoteDir = ClientFTP.GetWorkingDirectory
                ftpsite = ClientFTP.Host
            Catch ex As Exception
                Wdbg("W", "Error connecting to {0}: {1}", address, ex.Message)
                WStkTrc(ex)
                If DebugMode = True Then
                    W(DoTranslation("Error when trying to connect to {0}: {1}", currentLang) + vbNewLine +
                      DoTranslation("Stack Trace: {2}", currentLang), True, ColTypes.Neutral, address, ex.Message, ex.StackTrace)
                Else
                    W(DoTranslation("Error when trying to connect to {0}: {1}", currentLang), True, ColTypes.Neutral, address, ex.Message)
                End If
            End Try
        End If
    End Sub

    Public Sub TryToValidate(control As FtpClient, e As FtpSslValidationEventArgs)
        Wdbg("I", "Certificate checks")
        If e.PolicyErrors = Net.Security.SslPolicyErrors.None Then
            Wdbg("I", "Certificate accepted.")
            Wdbg("I", e.Certificate.GetRawCertDataString)
            e.Accept = True
        End If
        Wdbg("W", $"Certificate error is {e.PolicyErrors.ToString}")
    End Sub

End Module

Class FTPTracer
    Inherits TraceListener 'Both Write and WriteLine do exactly the same thing, which is writing to a debugger.
    Public Overloads Overrides Sub Write(ByVal Message As String)
        Wdbg("I", Message)
    End Sub
    Public Overloads Overrides Sub WriteLine(ByVal Message As String)
        Wdbg("I", Message)
    End Sub
End Class