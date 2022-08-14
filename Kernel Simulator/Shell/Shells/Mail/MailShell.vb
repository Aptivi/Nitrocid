
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

Imports KS.Network.Mail.Transfer
Imports KS.Network.Mail
Imports KS.Shell.Prompts

Namespace Shell.Shells.Mail
    Public Class MailShell
        Inherits ShellExecutor
        Implements IShell

        Public Overrides ReadOnly Property ShellType As ShellType Implements IShell.ShellType
            Get
                Return ShellType.MailShell
            End Get
        End Property

        Public Overrides Property Bail As Boolean Implements IShell.Bail

        Public Overrides Sub InitializeShell(ParamArray ShellArgs() As Object) Implements IShell.InitializeShell
            'Send ping to keep the connection alive
            Dim IMAP_NoOp As New KernelThread("IMAP Keep Connection", False, AddressOf IMAPKeepConnection)
            IMAP_NoOp.Start()
            Wdbg(DebugLevel.I, "Made new thread about IMAPKeepConnection()")
            Dim SMTP_NoOp As New KernelThread("SMTP Keep Connection", False, AddressOf SMTPKeepConnection)
            SMTP_NoOp.Start()
            Wdbg(DebugLevel.I, "Made new thread about SMTPKeepConnection()")
            KernelEventManager.RaiseIMAPShellInitialized()

            While Not Bail
                'Populate messages
                PopulateMessages()
                If Mail_NotifyNewMail Then InitializeHandlers()

                'See UESHShell.vb for more info
                SyncLock GetCancelSyncLock(ShellType)
                    'Initialize prompt
                    If DefConsoleOut IsNot Nothing Then
                        Console.SetOut(DefConsoleOut)
                    End If
                    WriteShellPrompt(ShellType)
                End SyncLock

                'Listen for a command
                Dim cmd As String = ReadLine()
                If Not (cmd = Nothing Or cmd?.StartsWithAnyOf({" ", "#"})) Then
                    KernelEventManager.RaiseIMAPPreExecuteCommand(cmd)
                    GetLine(cmd, "", ShellType.MailShell)
                    KernelEventManager.RaiseIMAPPostExecuteCommand(cmd)
                End If
            End While

            'Disconnect the session
            IMAP_CurrentDirectory = "Inbox"
            If KeepAlive Then
                Wdbg(DebugLevel.W, "Exit requested, but not disconnecting.")
            Else
                Wdbg(DebugLevel.W, "Exit requested. Disconnecting host...")
                If Mail_NotifyNewMail Then ReleaseHandlers()
                IMAP_Client.Disconnect(True)
                SMTP_Client.Disconnect(True)
            End If
        End Sub

    End Class
End Namespace
