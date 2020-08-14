
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

Imports MailKit.Net.Imap
Imports MailKit.Net.Smtp

Module MailLogin

    'Variables
    Public IMAP_Client As New ImapClient()
    Public SMTP_Client As New SmtpClient()
    Friend Mail_Authentication As New NetworkCredential()

    ''' <summary>
    ''' Prompts user to enter username or e-mail address
    ''' </summary>
    Sub PromptUser()
        'Username or mail address
        W(DoTranslation("Enter username or mail address: ", currentLang), False, ColTypes.Input)
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
        W(DoTranslation("Enter password: ", currentLang), False, ColTypes.Input)
        Mail_Authentication.Password = ReadLineNoInput("*")
        Console.WriteLine()

        'IMAP Server address and port
        W(DoTranslation("Enter IMAP server address and port (<address> or <address>:[port]): ", currentLang), False, ColTypes.Input)
        Dim IMAP_Address As String = Console.ReadLine
        Dim IMAP_Port As Integer = 0
        Wdbg("I", "IMAP Server: ""{0}""", IMAP_Address)

        'If the address is <address>:[port]
        If IMAP_Address.Contains(":") Then
            Wdbg("I", "Found colon in address. Separating...", Username)
            IMAP_Port = CInt(IMAP_Address.Substring(IMAP_Address.IndexOf(":") + 1))
            IMAP_Address = IMAP_Address.Remove(IMAP_Address.IndexOf(":"))
            Wdbg("I", "Final address: {0}, Final port: {1}", IMAP_Address, IMAP_Port)
        End If

        'SMTP Server address and port
        W(DoTranslation("Enter SMTP server address and port (<address> or <address>:[port]): ", currentLang), False, ColTypes.Input)
        Dim SMTP_Address As String = Console.ReadLine
        Dim SMTP_Port As Integer = 587
        Wdbg("I", "SMTP Server: ""{0}""", SMTP_Address)

        'If the address is <address>:[port]
        If SMTP_Address.Contains(":") Then
            Wdbg("I", "Found colon in address. Separating...", Username)
            SMTP_Port = CInt(SMTP_Address.Substring(SMTP_Address.IndexOf(":") + 1))
            SMTP_Address = SMTP_Address.Remove(SMTP_Address.IndexOf(":"))
            Wdbg("I", "Final address: {0}, Final port: {1}", SMTP_Address, SMTP_Port)
        End If

        'Try to connect
        Mail_Authentication.Domain = IMAP_Address
        ConnectShell(IMAP_Address, IMAP_Port, SMTP_Address, SMTP_Port)
    End Sub

    ''' <summary>
    ''' Tries to connect to specified address and port with specified credentials
    ''' </summary>
    ''' <param name="Address">An IP address of the IMAP server</param>
    ''' <param name="Port">A port of the IMAP server</param>
    ''' <param name="SmtpAddress">An IP address of the SMTP server</param>
    ''' <param name="SmtpPort">A port of the SMTP server</param>
    Sub ConnectShell(ByVal Address As String, ByVal Port As Integer, ByVal SmtpAddress As String, ByVal SmtpPort As Integer)
        Try
            'IMAP Connection
            W(DoTranslation("Connecting to {0}...", currentLang), True, ColTypes.Neutral, Address)
            Wdbg("I", "Connecting to IMAP Server {0}:{1} with SSL...", Address, Port)
            IMAP_Client.Connect(Address, Port, MailKit.Security.SecureSocketOptions.SslOnConnect)

            'SMTP Connection
            W(DoTranslation("Connecting to {0}...", currentLang), True, ColTypes.Neutral, SmtpAddress)
            Wdbg("I", "Connecting to SMTP Server {0}:{1} with SSL...", Address, Port)
            SMTP_Client.Connect(SmtpAddress, SmtpPort, MailKit.Security.SecureSocketOptions.StartTls)

            'IMAP Authentication
            W(DoTranslation("Authenticating...", currentLang), True, ColTypes.Neutral)
            Wdbg("I", "Authenticating {0} to IMAP server {1}...", Mail_Authentication.UserName, Address)
            IMAP_Client.Authenticate(Mail_Authentication)

            'SMTP Authentication
            Wdbg("I", "Authenticating {0} to SMTP server {1}...", Mail_Authentication.UserName, SmtpAddress)
            SMTP_Client.Authenticate(Mail_Authentication)

            'Initialize shell
            Wdbg("I", "Authentication succeeded. Opening shell...")
            OpenShell(Address)
        Catch ex As Exception
            W(DoTranslation("Error while connecting to {0}: {1}", currentLang), True, ColTypes.Err, Address, ex.Message)
            WStkTrc(ex)
            IMAP_Client.Disconnect(True)
            SMTP_Client.Disconnect(True)
        End Try
    End Sub

End Module
