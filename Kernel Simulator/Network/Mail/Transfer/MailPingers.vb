
'    Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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

Imports System.Threading
Imports KS.Shell.Shells.Mail

Namespace Network.Mail.Transfer
    Module MailPingers

        ''' <summary>
        ''' [IMAP] Tries to keep the connection going
        ''' </summary>
        Sub IMAPKeepConnection()
            'Every 30 seconds, send a ping to IMAP server
            While IMAP_Client.IsConnected
                Thread.Sleep(Mail_ImapPingInterval)
                If IMAP_Client.IsConnected Then
                    SyncLock IMAP_Client.SyncRoot
                        IMAP_Client.NoOp()
                    End SyncLock
                    PopulateMessages()
                Else
                    Wdbg(DebugLevel.W, "Connection state is inconsistent. Stopping IMAPKeepConnection()...")
                    Thread.CurrentThread.Interrupt()
                End If
            End While
        End Sub

        ''' <summary>
        ''' [SMTP] Tries to keep the connection going
        ''' </summary>
        Sub SMTPKeepConnection()
            'Every 30 seconds, send a ping to SMTP server
            While SMTP_Client.IsConnected
                Thread.Sleep(Mail_SmtpPingInterval)
                If SMTP_Client.IsConnected Then
                    SyncLock SMTP_Client.SyncRoot
                        SMTP_Client.NoOp()
                    End SyncLock
                Else
                    Wdbg(DebugLevel.W, "Connection state is inconsistent. Stopping SMTPKeepConnection()...")
                    Thread.CurrentThread.Interrupt()
                End If
            End While
        End Sub

    End Module
End Namespace

