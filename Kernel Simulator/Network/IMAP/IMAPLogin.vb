
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

Module IMAPLogin

    'Variables
    Public IMAP_Client As New ImapClient()
    Friend IMAP_Authentication As New NetworkCredential()

    Sub PromptUser()
        'Username or mail address
        W(DoTranslation("Enter username or mail address: ", currentLang), False, ColTypes.Input)
        PromptPassword(Console.ReadLine)
    End Sub

    Sub PromptPassword(ByVal Username As String)
        'Password
        IMAP_Authentication.UserName = Username
        W(DoTranslation("Enter password: ", currentLang), False, ColTypes.Input)
        IMAP_Authentication.Password = ReadLineNoInput("*")
        Console.WriteLine()

        'Server address and port
        W(DoTranslation("Enter server address and port (<address> or <address>:[port]): ", currentLang), False, ColTypes.Input)
        Dim IMAP_Address As String = Console.ReadLine
        Dim IMAP_Port As Integer = 0

        'If the address is <address>:[port]
        If IMAP_Address.Contains(":") Then
            IMAP_Address = IMAP_Address.Remove(IMAP_Address.IndexOf(":"))
            IMAP_Port = CInt(IMAP_Address.Substring(IMAP_Address.IndexOf(":") + 1))
        End If

        'Try to connect
        ConnectShell(IMAP_Address, IMAP_Port)
    End Sub

    Sub ConnectShell(ByVal Address As String, ByVal Port As Integer)
        Try
            'Connection
            W(DoTranslation("Connecting to {0}...", currentLang), True, ColTypes.Neutral, Address)
            IMAP_Client.Connect(Address, Port, MailKit.Security.SecureSocketOptions.SslOnConnect)

            'Authentication
            W(DoTranslation("Authenticating...", currentLang), True, ColTypes.Neutral)
            IMAP_Client.Authenticate(IMAP_Authentication)

            'Initialize shell
            OpenShell(Address)
        Catch ex As Exception
            W(DoTranslation("Error while connecting to {0}: {1}", currentLang), True, ColTypes.Err, Address, ex.Message)
            WStkTrc(ex)
            IMAP_Client.Disconnect(True)
        End Try
    End Sub

End Module
