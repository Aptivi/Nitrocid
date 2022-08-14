
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

Imports MailKit
Imports MailKit.Net.Imap
Imports KS.Misc.Notifications
Imports KS.Shell.Shells.Mail

Namespace Network.Mail
    Public Module MailHandlers

        ''' <summary>
        ''' Initializes the CountChanged handlers. Currently, it only supports inbox.
        ''' </summary>
        Public Sub InitializeHandlers()
            AddHandler IMAP_Client.Inbox.CountChanged, AddressOf OnCountChanged
        End Sub

        ''' <summary>
        ''' Releases the CountChanged handlers. Currently, it only supports inbox.
        ''' </summary>
        Public Sub ReleaseHandlers()
            RemoveHandler IMAP_Client.Inbox.CountChanged, AddressOf OnCountChanged
        End Sub

        ''' <summary>
        ''' Handles WebAlert sent by Gmail
        ''' </summary>
        Sub HandleWebAlert(sender As Object, e As WebAlertEventArgs)
            Wdbg(DebugLevel.I, "WebAlert URI: {0}", e.WebUri.AbsoluteUri)
            Write(e.Message, True, ColTypes.Warning)
            Write(DoTranslation("Opening URL... Make sure to follow the steps shown on the screen."), True, ColTypes.Neutral)
            Process.Start(e.WebUri.AbsoluteUri).WaitForExit()
        End Sub

        ''' <summary>
        ''' Executed when the CountChanged event is fired.
        ''' </summary>
        ''' <param name="Sender">A folder</param>
        ''' <param name="e">Event arguments</param>
        Sub OnCountChanged(Sender As Object, e As EventArgs)
            Dim Folder As ImapFolder = Sender
            If Folder.Count > IMAP_Messages.Count Then
                Dim NewMessagesCount As Integer = Folder.Count - IMAP_Messages.Count
                NotifySend(New Notification(DoTranslation("{0} new messages arrived in inbox.").FormatString(NewMessagesCount),
                                            DoTranslation("Open ""mail"" to see them."),
                                            NotifPriority.Medium, NotifType.Normal))
            End If
        End Sub

    End Module
End Namespace
