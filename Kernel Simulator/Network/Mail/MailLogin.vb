
'    Kernel Simulator  Copyright (C) 2018-2022  EoflaOE
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

Imports MailKit
Imports MailKit.Net.Imap
Imports MailKit.Net.Smtp
Imports MailKit.Net.Pop3
Imports MimeKit.Cryptography
Imports Addresstigator
Imports KS.Network.Mail.PGP

Namespace Network.Mail
    Module MailLogin

        'Variables
        Public IMAP_Client As New ImapClient()
        Public SMTP_Client As New SmtpClient()
        Friend Mail_Authentication As New NetworkCredential()
        Public Mail_UserPromptStyle As String = ""
        Public Mail_PassPromptStyle As String = ""
        Public Mail_IMAPPromptStyle As String = ""
        Public Mail_SMTPPromptStyle As String = ""
        Public Mail_POP3PromptStyle As String = ""
        Public Mail_GPGPromptStyle As String = ""
        Public Mail_Debug As Boolean
        Public Mail_AutoDetectServer As Boolean = True

#If POP3Feature Then
        Public POP3_Client As Pop3Client
#End If

        ''' <summary>
        ''' Mail server type
        ''' </summary>
        Public Enum ServerType
            ''' <summary>
            ''' The IMAP server
            ''' </summary>
            IMAP
            ''' <summary>
            ''' The SMTP server
            ''' </summary>
            SMTP
#If POP3Feature Then
            ''' <summary>
            ''' The POP3 server
            ''' </summary>
            POP3
#End If
        End Enum

        ''' <summary>
        ''' Prompts user to enter username or e-mail address
        ''' </summary>
        Sub PromptUser()
            'Username or mail address
            If Not String.IsNullOrWhiteSpace(Mail_UserPromptStyle) Then
                Write(ProbePlaces(Mail_UserPromptStyle), False, ColTypes.Input)
            Else
                Write(DoTranslation("Enter username or mail address: "), False, ColTypes.Input)
            End If

            'Try to get the username or e-mail address from the input
            Dim InputMailAddress As String = ReadLine()
            If Not String.IsNullOrWhiteSpace(Mail_UserPromptStyle) And ReadLineReboot.ReadLine.ReadRanToCompletion Then
                PromptPassword(InputMailAddress)
            End If
        End Sub

        ''' <summary>
        ''' Prompts user to enter password
        ''' </summary>
        ''' <param name="Username">Specified username</param>
        Sub PromptPassword(Username As String)
            'Password
            Wdbg(DebugLevel.I, "Username: {0}", Username)
            Mail_Authentication.UserName = Username
            If Not String.IsNullOrWhiteSpace(Mail_PassPromptStyle) Then
                Write(ProbePlaces(Mail_PassPromptStyle), False, ColTypes.Input)
            Else
                Write(DoTranslation("Enter password: "), False, ColTypes.Input)
            End If
            Mail_Authentication.Password = ReadLineNoInput()

            Dim DynamicAddressIMAP As String = ServerDetect(Username, ServerType.IMAP)
            Dim DynamicAddressSMTP As String = ServerDetect(Username, ServerType.SMTP)
#If POP3Feature Then
            Dim DynamicAddressPOP3 As String = ServerDetect(Username, ServerType.POP3)
#Else
            Dim DynamicAddressPOP3 As String = ""
#End If

            If DynamicAddressIMAP <> "" And (DynamicAddressSMTP <> "" Or DynamicAddressPOP3 <> "") And Mail_AutoDetectServer Then
                ParseAddresses(DynamicAddressIMAP, 0, DynamicAddressSMTP, 0, DynamicAddressPOP3, 0)
            Else
                PromptServer()
            End If
        End Sub

        ''' <summary>
        ''' Prompts for server
        ''' </summary>
        Sub PromptServer()
            Dim IMAP_Address As String
            Dim IMAP_Port As Integer
            Dim SMTP_Address As String = ""
            Dim SMTP_Port As Integer
            Dim POP3_Address As String = ""
            Dim POP3_Port As Integer = 587
            'IMAP server address and port
            If Not String.IsNullOrWhiteSpace(Mail_IMAPPromptStyle) Then
                Write(ProbePlaces(Mail_IMAPPromptStyle), False, ColTypes.Input)
            Else
                Write(DoTranslation("Enter IMAP server address and port (<address> or <address>:[port]): "), False, ColTypes.Input)
            End If
            IMAP_Address = ReadLine(False)
            Wdbg(DebugLevel.I, "IMAP Server: ""{0}""", IMAP_Address)

            'SMTP/POP3 server address and port
            If Not Mail_UsePop3 Then
                If Not String.IsNullOrWhiteSpace(Mail_SMTPPromptStyle) Then
                    Write(ProbePlaces(Mail_SMTPPromptStyle), False, ColTypes.Input)
                Else
                    Write(DoTranslation("Enter SMTP server address and port (<address> or <address>:[port]): "), False, ColTypes.Input)
                End If
                SMTP_Address = ReadLine(False)
                SMTP_Port = 587
                Wdbg(DebugLevel.I, "SMTP Server: ""{0}""", SMTP_Address)
            Else
#If POP3Feature Then
                If Not String.IsNullOrWhiteSpace(Mail_POP3PromptStyle) Then
                    Write(ProbePlaces(Mail_POP3PromptStyle), False, ColTypes.Input)
                Else
                    Write(DoTranslation("Enter POP3 server address and port (<address> or <address>:[port]): "), False, ColTypes.Input)
                End If
                POP3_Address = ReadLine(False)
                POP3_Port = 995
                Wdbg(DebugLevel.I, "POP3 Server: ""{0}""", POP3_Address)
#Else
                Throw New PlatformNotSupportedException(DoTranslation("POP3 mail is disabled. If you really want POP3 mail, re-compile the application with POP3 support."))
#End If
            End If

            'Parse addresses to connect
            ParseAddresses(IMAP_Address, IMAP_Port, SMTP_Address, SMTP_Port, POP3_Address, POP3_Port)
        End Sub

        Sub ParseAddresses(IMAP_Address As String, IMAP_Port As Integer, SMTP_Address As String, SMTP_Port As Integer, POP3_Address As String, POP3_Port As Integer)
            'If the address is <address>:[port]
            If IMAP_Address.Contains(":") Then
                Wdbg(DebugLevel.I, "Found colon in address. Separating...", Mail_Authentication.UserName)
                IMAP_Port = CInt(IMAP_Address.Substring(IMAP_Address.IndexOf(":") + 1))
                IMAP_Address = IMAP_Address.Remove(IMAP_Address.IndexOf(":"))
                Wdbg(DebugLevel.I, "Final address: {0}, Final port: {1}", IMAP_Address, IMAP_Port)
            End If

            'If the address is <address>:[port]
            If SMTP_Address.Contains(":") Then
                Wdbg(DebugLevel.I, "Found colon in address. Separating...", Mail_Authentication.UserName)
                SMTP_Port = CInt(SMTP_Address.Substring(SMTP_Address.IndexOf(":") + 1))
                SMTP_Address = SMTP_Address.Remove(SMTP_Address.IndexOf(":"))
                Wdbg(DebugLevel.I, "Final address: {0}, Final port: {1}", SMTP_Address, SMTP_Port)
            End If

            'If the address is <address>:[port]
#If POP3Feature Then
            If POP3_Address.Contains(":") Then
                Wdbg(DebugLevel.I, "Found colon in address. Separating...", Mail_Authentication.UserName)
                POP3_Port = CInt(POP3_Address.Substring(POP3_Address.IndexOf(":") + 1))
                POP3_Address = POP3_Address.Remove(POP3_Address.IndexOf(":"))
                Wdbg(DebugLevel.I, "Final address: {0}, Final port: {1}", POP3_Address, POP3_Port)
            End If
#End If

            'Try to connect
            Mail_Authentication.Domain = IMAP_Address
            ConnectShell(IMAP_Address, IMAP_Port, SMTP_Address, SMTP_Port, POP3_Address, POP3_Port)
        End Sub

        ''' <summary>
        ''' Detects servers based on dictionary
        ''' </summary>
        ''' <param name="Address">E-mail address</param>
        ''' <returns>Server address. Otherwise, null.</returns>
        Public Function ServerDetect(Address As String, Type As ServerType) As String
            'Get the mail server dynamically
            Dim DynamicConfiguration As ClientConfig = Tools.GetIspConfig(Address)
            Dim ReturnedMailAddress As String = ""
            Dim ReturnedMailPort As Integer
            Select Case Type
                Case ServerType.IMAP
                    Dim ImapServers = DynamicConfiguration.EmailProvider.IncomingServer.Select(Function(x) x).Where(Function(x) x.Type = "imap")
                    If ImapServers.Count > 0 Then
                        Dim ImapServer As IncomingServer = ImapServers(0)
                        ReturnedMailAddress = ImapServer.Hostname
                        ReturnedMailPort = ImapServer.Port
                    End If
#If POP3Feature Then
                Case ServerType.POP3
                    Dim Pop3Servers = DynamicConfiguration.EmailProvider.IncomingServer.Select(Function(x) x).Where(Function(x) x.Type = "pop3")
                    If Pop3Servers.Count > 0 Then
                        Dim Pop3Server As IncomingServer = Pop3Servers(0)
                        ReturnedMailAddress = Pop3Server.Hostname
                        ReturnedMailPort = Pop3Server.Port
                    End If
#End If
                Case ServerType.SMTP
                    Dim SmtpServer As OutgoingServer = DynamicConfiguration.EmailProvider.OutgoingServer
                    ReturnedMailAddress = SmtpServer?.Hostname
                    ReturnedMailPort = SmtpServer.Port
                Case Else
                    Return ""
            End Select
            Return $"{ReturnedMailAddress}:{ReturnedMailPort}"
        End Function

        ''' <summary>
        ''' Tries to connect to specified address and port with specified credentials
        ''' </summary>
        ''' <param name="Address">An IP address of the IMAP server</param>
        ''' <param name="Port">A port of the IMAP server</param>
        ''' <param name="SmtpAddress">An IP address of the SMTP server</param>
        ''' <param name="SmtpPort">A port of the SMTP server</param>
        ''' <param name="POP3_Address">An IP address of the POP3 server</param>
        ''' <param name="POP3_Port">A port of the POP3 server</param>
        Sub ConnectShell(Address As String, Port As Integer, SmtpAddress As String, SmtpPort As Integer, POP3_Address As String, POP3_Port As Integer)
            Try
                'Register the context and initialize the loggers if debug mode is on
                If DebugMode And Mail_Debug Then
                    IMAP_Client = New ImapClient(New ProtocolLogger(HomePath + "/ImapDebug.log") With {.LogTimestamps = True, .RedactSecrets = True, .ClientPrefix = "KS:  ", .ServerPrefix = "SRV: "})
                    SMTP_Client = New SmtpClient(New ProtocolLogger(HomePath + "/SmtpDebug.log") With {.LogTimestamps = True, .RedactSecrets = True, .ClientPrefix = "KS:  ", .ServerPrefix = "SRV: "})
#If POP3Feature Then
                    POP3_Client = New Pop3Client(New ProtocolLogger(HomePath + "/Pop3Debug.log") With {.LogTimestamps = True, .RedactSecrets = True, .ClientPrefix = "KS:  ", .ServerPrefix = "SRV: "})
#End If
                End If
                CryptographyContext.Register(GetType(PGPContext))

                'IMAP Connection
                Write(DoTranslation("Connecting to {0}..."), True, ColTypes.Neutral, Address)
                Wdbg(DebugLevel.I, "Connecting to IMAP Server {0}:{1} with SSL...", Address, Port)
                IMAP_Client.Connect(Address, Port, Security.SecureSocketOptions.SslOnConnect)
                AddHandler IMAP_Client.WebAlert, AddressOf HandleWebAlert

                'SMTP/POP3 Connection
                If Not Mail_UsePop3 Then
                    Write(DoTranslation("Connecting to {0}..."), True, ColTypes.Neutral, SmtpAddress)
                    Wdbg(DebugLevel.I, "Connecting to SMTP Server {0}:{1} with SSL...", Address, Port)
                    SMTP_Client.Connect(SmtpAddress, SmtpPort, Security.SecureSocketOptions.StartTlsWhenAvailable)
                Else
#If POP3Feature Then
                    Write(DoTranslation("Connecting to {0}..."), True, ColTypes.Neutral, POP3_Address)
                    Wdbg(DebugLevel.I, "Connecting to POP3 Server {0}:{1} with SSL...", Address, Port)
                    POP3_Client.Connect(POP3_Address, POP3_Port, Security.SecureSocketOptions.SslOnConnect)
#Else
                    Throw New PlatformNotSupportedException(DoTranslation("POP3 mail is disabled. If you really want POP3 mail, re-compile the application with POP3 support."))
#End If
                End If

                'IMAP Authentication
                Write(DoTranslation("Authenticating..."), True, ColTypes.Neutral)
                Wdbg(DebugLevel.I, "Authenticating {0} to IMAP server {1}...", Mail_Authentication.UserName, Address)
                IMAP_Client.Authenticate(Mail_Authentication)

                'SMTP/POP3 Authentication
                If Not Mail_UsePop3 Then
                    Wdbg(DebugLevel.I, "Authenticating {0} to SMTP server {1}...", Mail_Authentication.UserName, SmtpAddress)
                    SMTP_Client.Authenticate(Mail_Authentication)
                Else
#If POP3Feature Then
                    Wdbg(DebugLevel.I, "Authenticating {0} to POP3 server {1}...", Mail_Authentication.UserName, POP3_Address)
                    POP3_Client.Authenticate(Mail_Authentication)
#Else
                    Throw New PlatformNotSupportedException(DoTranslation("POP3 mail is disabled. If you really want POP3 mail, re-compile the application with POP3 support."))
#End If
                End If
                RemoveHandler IMAP_Client.WebAlert, AddressOf HandleWebAlert

                'Initialize shell
                Wdbg(DebugLevel.I, "Authentication succeeded. Opening shell...")
                StartShell(ShellType.MailShell)
            Catch ex As Exception
                Write(DoTranslation("Error while connecting to {0}: {1}"), True, ColTypes.Error, Address, ex.Message)
                WStkTrc(ex)
                IMAP_Client.Disconnect(True)
                SMTP_Client.Disconnect(True)
#If POP3Feature Then
                POP3_Client.Disconnect(True)
#End If
            End Try
        End Sub

    End Module
End Namespace
