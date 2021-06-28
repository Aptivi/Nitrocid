
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

Imports MailKit.Net.Imap
Imports MailKit.Net.Smtp
Imports MimeKit.Cryptography

Module MailLogin

    'Variables
    Public IMAP_Client As New ImapClient()
    Public SMTP_Client As New SmtpClient()
    Friend Mail_Authentication As New NetworkCredential()

    ''' <summary>
    ''' Mail server type
    ''' </summary>
    Public Enum ServerType
        IMAP
        SMTP
    End Enum

    ''' <summary>
    ''' Prompts user to enter username or e-mail address
    ''' </summary>
    Sub PromptUser()
        'Username or mail address
        W(DoTranslation("Enter username or mail address: "), False, ColTypes.Input)
        PromptPassword(Console.ReadLine)
    End Sub

    ''' <summary>
    ''' Prompts user to enter password
    ''' </summary>
    ''' <param name="Username">Specified username</param>
    Sub PromptPassword(ByVal Username As String)
        'Password
        Wdbg("I", "Username: {0}", Username)
        Mail_Authentication.UserName = Username
        W(DoTranslation("Enter password: "), False, ColTypes.Input)
        Mail_Authentication.Password = ReadLineNoInput("*")
        Console.WriteLine()
        Dim DynamicAddressIMAP As String = ServerDetect(Username, ServerType.IMAP)
        Dim DynamicAddressSMTP As String = ServerDetect(Username, ServerType.SMTP)
        If DynamicAddressIMAP <> "" And DynamicAddressSMTP <> "" Then
            ParseAddresses(DynamicAddressIMAP, 0, DynamicAddressSMTP, 0)
        Else
            PromptServer()
        End If
    End Sub

    ''' <summary>
    ''' Prompts for server
    ''' </summary>
    Sub PromptServer()
        'IMAP Server address and port
        W(DoTranslation("Enter IMAP server address and port (<address> or <address>:[port]): "), False, ColTypes.Input)
        Dim IMAP_Address As String = Console.ReadLine
        Dim IMAP_Port As Integer = 0
        Wdbg("I", "IMAP Server: ""{0}""", IMAP_Address)
        W(DoTranslation("Enter SMTP server address and port (<address> or <address>:[port]): "), False, ColTypes.Input)
        Dim SMTP_Address As String = Console.ReadLine
        Dim SMTP_Port As Integer = 587
        Wdbg("I", "SMTP Server: ""{0}""", SMTP_Address)
        ParseAddresses(IMAP_Address, IMAP_Port, SMTP_Address, SMTP_Port)
    End Sub

    Sub ParseAddresses(ByVal IMAP_Address As String, ByVal IMAP_Port As Integer, ByVal SMTP_Address As String, ByVal SMTP_Port As Integer)
        'If the address is <address>:[port]
        If IMAP_Address.Contains(":") Then
            Wdbg("I", "Found colon in address. Separating...", Mail_Authentication.UserName)
            IMAP_Port = CInt(IMAP_Address.Substring(IMAP_Address.IndexOf(":") + 1))
            IMAP_Address = IMAP_Address.Remove(IMAP_Address.IndexOf(":"))
            Wdbg("I", "Final address: {0}, Final port: {1}", IMAP_Address, IMAP_Port)
        End If

        'If the address is <address>:[port]
        If SMTP_Address.Contains(":") Then
            Wdbg("I", "Found colon in address. Separating...", Mail_Authentication.UserName)
            SMTP_Port = CInt(SMTP_Address.Substring(SMTP_Address.IndexOf(":") + 1))
            SMTP_Address = SMTP_Address.Remove(SMTP_Address.IndexOf(":"))
            Wdbg("I", "Final address: {0}, Final port: {1}", SMTP_Address, SMTP_Port)
        End If

        'Try to connect
        Mail_Authentication.Domain = IMAP_Address
        ConnectShell(IMAP_Address, IMAP_Port, SMTP_Address, SMTP_Port)
    End Sub

    ''' <summary>
    ''' Detects servers based on dictionary
    ''' </summary>
    ''' <param name="Address">E-mail address</param>
    ''' <returns>Server address. Otherwise, null.</returns>
    Public Function ServerDetect(ByVal Address As String, ByVal Type As ServerType) As String
        If Address.EndsWith("@gmail.com") Or Address.EndsWith("@googlemail.com") Then
            If Type = ServerType.IMAP Then
                Return "imap.gmail.com"
            ElseIf Type = ServerType.SMTP Then
                Return "smtp.gmail.com:587"
            Else
                Return ""
            End If
        ElseIf Address.EndsWith("@aol.com") Then
            If Type = ServerType.IMAP Then
                Return "imap.aol.com"
            ElseIf Type = ServerType.SMTP Then
                Return "smtp.aol.com:587"
            Else
                Return ""
            End If
        ElseIf Address.EndsWith("@outlook.com") Or Address.EndsWith("@hotmail.com") Then
            If Type = ServerType.IMAP Then
                Return "imap-mail.outlook.com"
            ElseIf Type = ServerType.SMTP Then
                Return "smtp-mail.outlook.com:587"
            Else
                Return ""
            End If
        ElseIf Address.EndsWith("@office365.com") Then
            If Type = ServerType.IMAP Then
                Return "outlook.office365.com"
            ElseIf Type = ServerType.SMTP Then
                Return "smtp.office365.com:587"
            Else
                Return ""
            End If
        ElseIf Address.EndsWith("@bt.com") Then
            If Type = ServerType.IMAP Then
                Return "imap4.btconnect.com"
            ElseIf Type = ServerType.SMTP Then
                Return "smtp.btconnect.com:25"
            Else
                Return ""
            End If
        ElseIf Address.EndsWith("@yahoo.com") Then
            If Type = ServerType.IMAP Then
                Return "imap.mail.yahoo.com"
            ElseIf Type = ServerType.SMTP Then
                Return "smtp.mail.yahoo.com"
            Else
                Return ""
            End If
        ElseIf Address.EndsWith("@yahoo.co.uk") Then
            If Type = ServerType.IMAP Then
                Return "imap.mail.yahoo.co.uk"
            ElseIf Type = ServerType.SMTP Then
                Return "smtp.mail.yahoo.co.uk"
            Else
                Return ""
            End If
        ElseIf Address.EndsWith("@yahoo.au") Then
            If Type = ServerType.IMAP Then
                Return "imap.mail.yahoo.au"
            ElseIf Type = ServerType.SMTP Then
                Return "smtp.mail.yahoo.au"
            Else
                Return ""
            End If
        ElseIf Address.EndsWith("@verizon.net") Then
            If Type = ServerType.IMAP Then
                Return "incoming.verizon.net"
            ElseIf Type = ServerType.SMTP Then
                Return "outgoing.verizon.net:587"
            Else
                Return ""
            End If
        ElseIf Address.EndsWith("@att.com") Then
            If Type = ServerType.IMAP Then
                Return "imap.att.yahoo.com"
            ElseIf Type = ServerType.SMTP Then
                Return "smtp.att.yahoo.com"
            Else
                Return ""
            End If
        ElseIf Address.EndsWith("@o2online.de") Then
            If Type = ServerType.IMAP Then
                Return "imap.o2online.de"
            ElseIf Type = ServerType.SMTP Then
                Return "mail.o2online.de"
            Else
                Return ""
            End If
        ElseIf Address.EndsWith("@t-online.de") Then
            If Type = ServerType.IMAP Then
                Return "secureimap.t-online.de"
            ElseIf Type = ServerType.SMTP Then
                Return "securesmtp.t-online.de:587"
            Else
                Return ""
            End If
        ElseIf Address.EndsWith("@1und1.de") Then
            If Type = ServerType.IMAP Then
                Return "imap.1und1.de"
            ElseIf Type = ServerType.SMTP Then
                Return "smtp.1und1.de:587"
            Else
                Return ""
            End If
        ElseIf Address.EndsWith("@ionos.com") Then
            If Type = ServerType.IMAP Then
                Return "imap.1and1.com"
            ElseIf Type = ServerType.SMTP Then
                Return "smtp.1and1.com:587"
            Else
                Return ""
            End If
        ElseIf Address.EndsWith("@zoho.com") Then
            If Type = ServerType.IMAP Then
                Return "imap.zoho.com"
            ElseIf Type = ServerType.SMTP Then
                Return "smtp.zoho.com"
            Else
                Return ""
            End If
        ElseIf Address.EndsWith("@ntlworld.com") Then
            If Type = ServerType.IMAP Then
                Return "imap.ntlworld.com"
            ElseIf Type = ServerType.SMTP Then
                Return "smtp.ntlworld.com"
            Else
                Return ""
            End If
        ElseIf Address.EndsWith("@mail.com") Then
            If Type = ServerType.IMAP Then
                Return "imap.mail.com"
            ElseIf Type = ServerType.SMTP Then
                Return "smtp.mail.com:587"
            Else
                Return ""
            End If
        ElseIf Address.EndsWith("@fastmail.fm") Then
            If Type = ServerType.IMAP Then
                Return "imap.fastmail.com"
            ElseIf Type = ServerType.SMTP Then
                Return "smtp.fastmail.com:587"
            Else
                Return ""
            End If
        ElseIf Address.EndsWith("@gmx.com") Then
            If Type = ServerType.IMAP Then
                Return "imap.gmx.com"
            ElseIf Type = ServerType.SMTP Then
                Return "smtp.gmx.com"
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
    Sub ConnectShell(ByVal Address As String, ByVal Port As Integer, ByVal SmtpAddress As String, ByVal SmtpPort As Integer)
        Try
            CryptographyContext.Register(GetType(PGPContext))

            'IMAP Connection
            W(DoTranslation("Connecting to {0}..."), True, ColTypes.Neutral, Address)
            Wdbg("I", "Connecting to IMAP Server {0}:{1} with SSL...", Address, Port)
            IMAP_Client.Connect(Address, Port, MailKit.Security.SecureSocketOptions.SslOnConnect)
            AddHandler IMAP_Client.WebAlert, AddressOf HandleWebAlert

            'SMTP Connection
            W(DoTranslation("Connecting to {0}..."), True, ColTypes.Neutral, SmtpAddress)
            Wdbg("I", "Connecting to SMTP Server {0}:{1} with SSL...", Address, Port)
            SMTP_Client.Connect(SmtpAddress, SmtpPort, MailKit.Security.SecureSocketOptions.StartTls)

            'IMAP Authentication
            W(DoTranslation("Authenticating..."), True, ColTypes.Neutral)
            Wdbg("I", "Authenticating {0} to IMAP server {1}...", Mail_Authentication.UserName, Address)
            IMAP_Client.Authenticate(Mail_Authentication)

            'SMTP Authentication
            Wdbg("I", "Authenticating {0} to SMTP server {1}...", Mail_Authentication.UserName, SmtpAddress)
            SMTP_Client.Authenticate(Mail_Authentication)
            RemoveHandler IMAP_Client.WebAlert, AddressOf HandleWebAlert

            'Initialize shell
            Wdbg("I", "Authentication succeeded. Opening shell...")
            OpenMailShell(Address)
        Catch ex As Exception
            W(DoTranslation("Error while connecting to {0}: {1}"), True, ColTypes.Error, Address, ex.Message)
            WStkTrc(ex)
            IMAP_Client.Disconnect(True)
            SMTP_Client.Disconnect(True)
        End Try
    End Sub

End Module
