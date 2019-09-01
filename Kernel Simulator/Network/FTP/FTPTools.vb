
'    Kernel Simulator  Copyright (C) 2018-2019  EoflaOE
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
                If FtpHost = FtpPort Then
                    FtpPort = 0
                End If
                ClientFTP = New FtpClient With {
                    .Host = FtpHost,
                    .Port = FtpPort,
                    .RetryAttempts = 3,
                    .EncryptionMode = FtpEncryptionMode.Explicit
                }

                'Prompt for username and for password
                AddHandler ClientFTP.ValidateCertificate, New FtpSslValidation(AddressOf TryToValidate)
                W(DoTranslation("Username for {0}: ", currentLang), False, ColTypes.Input, address)
                user = Console.ReadLine()
                If user = "" Then user = "anonymous"
                W(DoTranslation("Password for {0}: ", currentLang), False, ColTypes.Input, user)

                'Get input
                While True
                    Dim character As Char = Console.ReadKey(True).KeyChar
                    If character = vbCr Or character = vbLf Then
                        Console.WriteLine()
                        Exit While
                    Else
                        pass += character
                    End If
                End While

                'Set up credentials
                ClientFTP.Credentials = New NetworkCredential(user, pass)

                'Prepare profiles
                Dim profiles As List(Of FtpProfile) = ClientFTP.AutoDetect
                Dim profsel As FtpProfile
                If profiles.Count > 1 Then 'More than one profile
                    W(DoTranslation("More than one profile found. Select one:", currentLang) + vbNewLine, True, ColTypes.Neutral)
                    For i As Integer = 1 To profiles.Count - 1
                        W($"{i}: {profiles(i).Host}, {profiles(i).Credentials.UserName}, {profiles(i).DataConnection.ToString}, {profiles(i).Encoding.EncodingName}, {profiles(i).Encryption.ToString}, {profiles(i).Protocols.ToString}", True, ColTypes.Neutral)
                    Next
                Else
                    profsel = profiles(0) 'Select first profile
                End If

                'Connect
                ClientFTP.Connect(profsel)

                'Show that it's connected
                W(DoTranslation("Connected to {0}", currentLang), True, ColTypes.Neutral, address)
                connected = True

                'Prepare to print current FTP directory
                currentremoteDir = ClientFTP.GetWorkingDirectory
                ftpsite = ClientFTP.Host
            Catch ex As Exception
                Wdbg("Error connecting to {0}: {1}", address, ex.Message)
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
        Wdbg("Certificate checks")
        If e.PolicyErrors = Net.Security.SslPolicyErrors.None Then
            Wdbg("Certificate accepted.")
            Wdbg(e.Certificate.GetRawCertDataString)
            e.Accept = True
        End If
        Wdbg($"Certificate error is {e.PolicyErrors.ToString}")
    End Sub

End Module
