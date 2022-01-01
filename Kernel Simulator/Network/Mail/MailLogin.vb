
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

Module MailLogin

    'Variables
    Public IMAP_Client As New ImapClient()
    Public SMTP_Client As New SmtpClient()
    Public POP3_Client As New Pop3Client()
    Friend Mail_Authentication As New NetworkCredential()
    Public Mail_UserPromptStyle As String = ""
    Public Mail_PassPromptStyle As String = ""
    Public Mail_IMAPPromptStyle As String = ""
    Public Mail_SMTPPromptStyle As String = ""
    Public Mail_POP3PromptStyle As String = ""
    Public Mail_GPGPromptStyle As String = ""
    Public Mail_Debug As Boolean
    Public Mail_AutoDetectServer As Boolean = True

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
        ''' <summary>
        ''' The POP3 server
        ''' </summary>
        POP3
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
        PromptPassword(Console.ReadLine)
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
        Mail_Authentication.Password = ReadLineNoInput("*")
        Console.WriteLine()
        Dim DynamicAddressIMAP As String = ServerDetect(Username, ServerType.IMAP)
        Dim DynamicAddressSMTP As String = ServerDetect(Username, ServerType.SMTP)
        Dim DynamicAddressPOP3 As String = ServerDetect(Username, ServerType.POP3)
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
        IMAP_Address = Console.ReadLine
        Wdbg(DebugLevel.I, "IMAP Server: ""{0}""", IMAP_Address)

        'SMTP/POP3 server address and port
        If Not Mail_UsePop3 Then
            If Not String.IsNullOrWhiteSpace(Mail_SMTPPromptStyle) Then
                Write(ProbePlaces(Mail_SMTPPromptStyle), False, ColTypes.Input)
            Else
                Write(DoTranslation("Enter SMTP server address and port (<address> or <address>:[port]): "), False, ColTypes.Input)
            End If
            SMTP_Address = Console.ReadLine
            SMTP_Port = 587
            Wdbg(DebugLevel.I, "SMTP Server: ""{0}""", SMTP_Address)
        Else
            If Not String.IsNullOrWhiteSpace(Mail_POP3PromptStyle) Then
                Write(ProbePlaces(Mail_POP3PromptStyle), False, ColTypes.Input)
            Else
                Write(DoTranslation("Enter POP3 server address and port (<address> or <address>:[port]): "), False, ColTypes.Input)
            End If
            POP3_Address = Console.ReadLine
            POP3_Port = 995
            Wdbg(DebugLevel.I, "POP3 Server: ""{0}""", POP3_Address)
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
        If POP3_Address.Contains(":") Then
            Wdbg(DebugLevel.I, "Found colon in address. Separating...", Mail_Authentication.UserName)
            POP3_Port = CInt(POP3_Address.Substring(POP3_Address.IndexOf(":") + 1))
            POP3_Address = POP3_Address.Remove(POP3_Address.IndexOf(":"))
            Wdbg(DebugLevel.I, "Final address: {0}, Final port: {1}", POP3_Address, POP3_Port)
        End If

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
        If Address.EndsWith("@gmail.com") Or Address.EndsWith("@googlemail.com") Then
            If Type = ServerType.IMAP Then
                Return "imap.gmail.com"
            ElseIf Type = ServerType.SMTP Then
                Return "smtp.gmail.com:587"
            ElseIf Type = ServerType.POP3 Then
                Return "pop.gmail.com:995"
            Else
                Return ""
            End If
        ElseIf Address.EndsWith("@aol.com") Then
            If Type = ServerType.IMAP Then
                Return "imap.aol.com"
            ElseIf Type = ServerType.SMTP Then
                Return "smtp.aol.com:465"
            ElseIf Type = ServerType.POP3 Then
                Return "pop.aol.com:995"
            Else
                Return ""
            End If
        ElseIf Address.EndsWith("@outlook.com") Or Address.EndsWith("@hotmail.com") Then
            If Type = ServerType.IMAP Then
                Return "imap-mail.outlook.com"
            ElseIf Type = ServerType.SMTP Then
                Return "smtp-mail.outlook.com:587"
            ElseIf Type = ServerType.POP3 Then
                Return "pop3.live.com:995"
            Else
                Return ""
            End If
        ElseIf Address.EndsWith("@office365.com") Then
            If Type = ServerType.IMAP Then
                Return "outlook.office365.com"
            ElseIf Type = ServerType.SMTP Then
                Return "smtp.office365.com:587"
            ElseIf Type = ServerType.POP3 Then
                Return "outlook.office365.com:995"
            Else
                Return ""
            End If
        ElseIf Address.EndsWith("@btinternet.com") Then
            If Type = ServerType.IMAP Then
                Return "mail.btinternet.com:993"
            ElseIf Type = ServerType.SMTP Then
                Return "mail.btinternet.com:465"
            ElseIf Type = ServerType.POP3 Then
                Return "mail.btinternet.com:995"
            Else
                Return ""
            End If
        ElseIf Address.EndsWith("@yahoo.com") Then
            If Type = ServerType.IMAP Then
                Return "imap.mail.yahoo.com"
            ElseIf Type = ServerType.SMTP Then
                Return "smtp.mail.yahoo.com"
            ElseIf Type = ServerType.POP3 Then
                Return "pop.mail.yahoo.com:995"
            Else
                Return ""
            End If
        ElseIf Address.EndsWith("@yahoo.co.uk") Then
            If Type = ServerType.IMAP Then
                Return "imap.mail.yahoo.co.uk"
            ElseIf Type = ServerType.SMTP Then
                Return "smtp.mail.yahoo.co.uk"
            ElseIf Type = ServerType.POP3 Then
                Return "pop.mail.yahoo.co.uk:995"
            Else
                Return ""
            End If
        ElseIf Address.EndsWith("@yahoo.au") Then
            If Type = ServerType.IMAP Then
                Return "imap.mail.yahoo.au"
            ElseIf Type = ServerType.SMTP Then
                Return "smtp.mail.yahoo.au"
            ElseIf Type = ServerType.POP3 Then
                Return "pop.mail.yahoo.au:995"
            Else
                Return ""
            End If
        ElseIf Address.EndsWith("@verizon.net") Then
            If Type = ServerType.IMAP Then
                Return "imap.aol.com"
            ElseIf Type = ServerType.SMTP Then
                Return "smtp.verizon.net:465"
            ElseIf Type = ServerType.POP3 Then
                Return "pop.verizon.net:995"
            Else
                Return ""
            End If
        ElseIf Address.EndsWith("@att.net") Then
            If Type = ServerType.IMAP Then
                Return "imap.mail.att.net"
            ElseIf Type = ServerType.SMTP Then
                Return "outbound.att.net:465"
            ElseIf Type = ServerType.POP3 Then
                Return "inbound.att.net:995"
            Else
                Return ""
            End If
        ElseIf Address.EndsWith("@o2online.de") Then
            If Type = ServerType.IMAP Then
                Return "mail.o2online.de"
            ElseIf Type = ServerType.SMTP Then
                Return "smtp.o2online.de"
            ElseIf Type = ServerType.POP3 Then
                Return "pop.o2online.de:995"
            Else
                Return ""
            End If
        ElseIf Address.EndsWith("@t-online.de") Then
            If Type = ServerType.IMAP Then
                Return "secureimap.t-online.de"
            ElseIf Type = ServerType.SMTP Then
                Return "securesmtp.t-online.de:465"
            ElseIf Type = ServerType.POP3 Then
                Return "securepop.t-online.de:995"
            Else
                Return ""
            End If
        ElseIf Address.EndsWith("@1und1.de") Then
            If Type = ServerType.IMAP Then
                Return "imap.1und1.de"
            ElseIf Type = ServerType.SMTP Then
                Return "smtp.1und1.de:587"
            ElseIf Type = ServerType.POP3 Then
                Return "pop.1und1.de:995"
            Else
                Return ""
            End If
        ElseIf Address.EndsWith("@ionos.com") Then
            If Type = ServerType.IMAP Then
                Return "imap.ionos.com"
            ElseIf Type = ServerType.SMTP Then
                Return "smtp.ionos.com:587"
            ElseIf Type = ServerType.POP3 Then
                Return "pop.ionos.com:995"
            Else
                Return ""
            End If
        ElseIf Address.EndsWith("@zoho.com") Then
            If Type = ServerType.IMAP Then
                Return "imap.zoho.com"
            ElseIf Type = ServerType.SMTP Then
                Return "smtp.zoho.com"
            ElseIf Type = ServerType.POP3 Then
                Return "pop.zoho.com:995"
            Else
                Return ""
            End If
        ElseIf Address.EndsWith("@ntlworld.com") Then
            If Type = ServerType.IMAP Then
                Return "imap.virginmedia.com"
            ElseIf Type = ServerType.SMTP Then
                Return "smtp.virginmedia.com"
            ElseIf Type = ServerType.POP3 Then
                Return "pop3.virginmedia.com:995"
            Else
                Return ""
            End If
        ElseIf Address.EndsWith("@mail.com") Then
            If Type = ServerType.IMAP Then
                Return "imap.mail.com"
            ElseIf Type = ServerType.SMTP Then
                Return "smtp.mail.com:587"
            ElseIf Type = ServerType.POP3 Then
                Return "pop.mail.com:995"
            Else
                Return ""
            End If
        ElseIf Address.EndsWith("@fastmail.fm") Then
            If Type = ServerType.IMAP Then
                Return "imap.fastmail.com"
            ElseIf Type = ServerType.SMTP Then
                Return "smtp.fastmail.com:587"
            ElseIf Type = ServerType.POP3 Then
                Return "pop.fastmail.com:995"
            Else
                Return ""
            End If
        ElseIf Address.EndsWith("@gmx.com") Then
            If Type = ServerType.IMAP Then
                Return "imap.gmx.com"
            ElseIf Type = ServerType.SMTP Then
                Return "smtp.gmx.com"
            ElseIf Type = ServerType.POP3 Then
                Return "pop.gmx.com:995"
            Else
                Return ""
            End If
        End If
        Return ""
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
                POP3_Client = New Pop3Client(New ProtocolLogger(HomePath + "/Pop3Debug.log") With {.LogTimestamps = True, .RedactSecrets = True, .ClientPrefix = "KS:  ", .ServerPrefix = "SRV: "})
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
                Write(DoTranslation("Connecting to {0}..."), True, ColTypes.Neutral, POP3_Address)
                Wdbg(DebugLevel.I, "Connecting to POP3 Server {0}:{1} with SSL...", Address, Port)
                POP3_Client.Connect(POP3_Address, POP3_Port, Security.SecureSocketOptions.SslOnConnect)
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
                Wdbg(DebugLevel.I, "Authenticating {0} to POP3 server {1}...", Mail_Authentication.UserName, POP3_Address)
                POP3_Client.Authenticate(Mail_Authentication)
            End If
            RemoveHandler IMAP_Client.WebAlert, AddressOf HandleWebAlert

            'Initialize shell
            Wdbg(DebugLevel.I, "Authentication succeeded. Opening shell...")
            OpenMailShell(Address)
        Catch ex As Exception
            Write(DoTranslation("Error while connecting to {0}: {1}"), True, ColTypes.Error, Address, ex.Message)
            WStkTrc(ex)
            IMAP_Client.Disconnect(True)
            SMTP_Client.Disconnect(True)
            POP3_Client.Disconnect(True)
        End Try
    End Sub

End Module
