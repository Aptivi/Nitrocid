
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
Imports MimeKit

Namespace Network.Mail.Directory
    Public Module MailManager

        Public ShowPreview As Boolean

        ''' <summary>
        ''' Lists messages
        ''' </summary>
        ''' <param name="PageNum">Page number</param>
        ''' <exception cref="ArgumentException"></exception>
        Public Sub MailListMessages(PageNum As Integer)
            MailListMessages(PageNum, Mail_MaxMessagesInPage)
        End Sub

        ''' <summary>
        ''' Lists messages
        ''' </summary>
        ''' <param name="PageNum">Page number</param>
        ''' <param name="MessagesInPage">Max messages in one page</param>
        ''' <exception cref="ArgumentException"></exception>
        Public Sub MailListMessages(PageNum As Integer, MessagesInPage As Integer)
            'Sanity checks for the page number
            If PageNum <= 0 Then PageNum = 1
            Wdbg(DebugLevel.I, "Page number {0}", PageNum)

            Dim MsgsLimitForPg As Integer = MessagesInPage
            Dim FirstIndex As Integer = (MsgsLimitForPg * PageNum) - 10
            Dim LastIndex As Integer = (MsgsLimitForPg * PageNum) - 1
            Dim MaxMessagesIndex As Integer = IMAP_Messages.Count - 1
            Wdbg(DebugLevel.I, "10 messages shown in each page. First message number in page {0} is {1} and last message number in page {0} is {2}", MsgsLimitForPg, FirstIndex, LastIndex)
            For i As Integer = FirstIndex To LastIndex
                If Not i > MaxMessagesIndex Then
                    Dim MsgFrom As String = ""
                    Dim MsgSubject As String = ""
                    Dim MsgPreview As String = ""

                    'Getting information about the message is vital to display them.
                    Wdbg(DebugLevel.I, "Getting message {0}...", i)
                    SyncLock IMAP_Client.SyncRoot
                        Dim Msg As MimeMessage
                        If Not IMAP_CurrentDirectory = "" And Not IMAP_CurrentDirectory = "Inbox" Then
                            Dim Dir As MailFolder = OpenFolder(IMAP_CurrentDirectory)
                            Msg = Dir.GetMessage(IMAP_Messages(i), Nothing, Mail_Progress)
                        Else
                            Msg = IMAP_Client.Inbox.GetMessage(IMAP_Messages(i), Nothing, Mail_Progress)
                        End If
                        MsgFrom = Msg.From.ToString
                        MsgSubject = Msg.Subject
                        MsgPreview = If(Msg.GetTextBody(Text.TextFormat.Text), "").Truncate(200)
                    End SyncLock
                    Wdbg(DebugLevel.I, "From {0}: {1}", MsgFrom, MsgSubject)

                    'Display them now.
                    Write($"- [{i + 1}/{MaxMessagesIndex + 1}] {MsgFrom}: ", False, ColTypes.ListEntry) : Write(MsgSubject, True, ColTypes.ListValue)
                    If ShowPreview And Not String.IsNullOrWhiteSpace(MsgPreview) Then
                        'TODO: For more efficient preview, use the PREVIEW extension as documented in RFC-8970 (https://tools.ietf.org/html/rfc8970). However,
                        '      this is impossible at this time because no server and no client support this extension. It supports the LAZY modifier. It only
                        '      displays 200 character long body.
                        '      Concept: Msg.Preview(LazyMode:=True)
                        Write(MsgPreview, True, ColTypes.ListValue)
                    End If
                Else
                    Wdbg(DebugLevel.W, "Reached max message limit. Message number {0}", i)
                End If
            Next
        End Sub

        ''' <summary>
        ''' Removes a message
        ''' </summary>
        ''' <param name="MsgNumber">Message number</param>
        ''' <returns>True if successful; False if unsuccessful</returns>
        ''' <exception cref="ArgumentException"></exception>
        ''' <exception cref="Exceptions.MailException"></exception>
        Public Function MailRemoveMessage(MsgNumber As Integer) As Boolean
            Dim Message As Integer = MsgNumber - 1
            Dim MaxMessagesIndex As Integer = IMAP_Messages.Count - 1
            Wdbg(DebugLevel.I, "Message number {0}", Message)
            If Message < 0 Then
                Wdbg(DebugLevel.E, "Trying to remove message 0 or less than 0.")
                Throw New ArgumentException(DoTranslation("Message number may not be negative or zero."))
                Return False
            ElseIf Message > MaxMessagesIndex Then
                Wdbg(DebugLevel.E, "Message {0} not in list. It was larger than MaxMessagesIndex ({1})", Message, MaxMessagesIndex)
                Throw New Exceptions.MailException(DoTranslation("Message specified is not found."))
                Return False
            End If

            SyncLock IMAP_Client.SyncRoot
                If Not IMAP_CurrentDirectory = "" And Not IMAP_CurrentDirectory = "Inbox" Then
                    'Remove message
                    Dim Dir As MailFolder = OpenFolder(IMAP_CurrentDirectory)
                    Wdbg(DebugLevel.I, "Opened {0}. Removing {1}...", IMAP_CurrentDirectory, MsgNumber)
                    Dir.Store(IMAP_Messages(Message), New StoreFlagsRequest(StoreAction.Add, MessageFlags.Deleted))
                    Wdbg(DebugLevel.I, "Removed.")
                    Dir.Expunge()
                Else
                    'Remove message
                    IMAP_Client.Inbox.Open(FolderAccess.ReadWrite)
                    Wdbg(DebugLevel.I, "Removing {0}...", MsgNumber)
                    IMAP_Client.Inbox.Store(IMAP_Messages(Message), New StoreFlagsRequest(StoreAction.Add, MessageFlags.Deleted))
                    Wdbg(DebugLevel.I, "Removed.")
                    IMAP_Client.Inbox.Expunge()
                End If
            End SyncLock
            Return True
        End Function

        ''' <summary>
        ''' Removes all mail that the specified sender has sent
        ''' </summary>
        ''' <param name="Sender">The sender name</param>
        ''' <returns>True if successful; False if unsuccessful</returns>
        Public Function MailRemoveAllBySender(Sender As String) As Boolean
            Wdbg(DebugLevel.I, "All mail by {0} will be removed.", Sender)
            Dim DeletedMsgNumber As Integer = 1
            Dim SteppedMsgNumber As Integer = 0
            For i As Integer = 0 To IMAP_Messages.Count
                Try
                    SyncLock IMAP_Client.SyncRoot
                        Dim MessageId As UniqueId = IMAP_Messages(i)
                        Dim Msg As MimeMessage
                        If Not IMAP_CurrentDirectory = "" And Not IMAP_CurrentDirectory = "Inbox" Then
                            Dim Dir As MailFolder = OpenFolder(IMAP_CurrentDirectory)
                            Msg = Dir.GetMessage(MessageId, Nothing, Mail_Progress)
                        Else
                            Msg = IMAP_Client.Inbox.GetMessage(MessageId, Nothing, Mail_Progress)
                        End If
                        SteppedMsgNumber += 1

                        For Each address In Msg.From
                            If address.Name = Sender Then
                                If Not IMAP_CurrentDirectory = "" And Not IMAP_CurrentDirectory = "Inbox" Then
                                    Dim Dir As MailFolder = OpenFolder(IMAP_CurrentDirectory)

                                    'Remove message
                                    Wdbg(DebugLevel.I, "Opened {0}. Removing {1}...", IMAP_CurrentDirectory, Sender)
                                    Dir.Store(MessageId, New StoreFlagsRequest(StoreAction.Add, MessageFlags.Deleted))
                                    Wdbg(DebugLevel.I, "Removed.")
                                    Dir.Expunge()
                                    Wdbg(DebugLevel.I, "Message {0} from {1} deleted from {2}. {3} messages remaining to parse.", DeletedMsgNumber, Sender, IMAP_CurrentDirectory, IMAP_Messages.Count - SteppedMsgNumber)
                                    Write(DoTranslation("Message {0} from {1} deleted from {2}. {3} messages remaining to parse."), True, ColTypes.Neutral, DeletedMsgNumber, Sender, IMAP_CurrentDirectory, IMAP_Messages.Count - SteppedMsgNumber)
                                Else
                                    'Remove message
                                    IMAP_Client.Inbox.Open(FolderAccess.ReadWrite)
                                    Wdbg(DebugLevel.I, "Removing {0}...", Sender)
                                    IMAP_Client.Inbox.Store(MessageId, New StoreFlagsRequest(StoreAction.Add, MessageFlags.Deleted))
                                    Wdbg(DebugLevel.I, "Removed.")
                                    IMAP_Client.Inbox.Expunge()
                                    Wdbg(DebugLevel.I, "Message {0} from {1} deleted from inbox. {2} messages remaining to parse.", DeletedMsgNumber, Sender, IMAP_Messages.Count - SteppedMsgNumber)
                                    Write(DoTranslation("Message {0} from {1} deleted from inbox. {2} messages remaining to parse."), True, ColTypes.Neutral, DeletedMsgNumber, Sender, IMAP_Messages.Count - SteppedMsgNumber)
                                End If
                                DeletedMsgNumber += 1
                            End If
                        Next
                    End SyncLock
                Catch ex As Exception
                    WStkTrc(ex)
                    Return False
                End Try
            Next
            Return True
        End Function

        ''' <summary>
        ''' Moves a message
        ''' </summary>
        ''' <param name="MsgNumber">Message number</param>
        ''' <param name="TargetFolder">Target folder</param>
        ''' <returns>True if successful; False if unsuccessful</returns>
        ''' <exception cref="ArgumentException"></exception>
        ''' <exception cref="Exceptions.MailException"></exception>
        Public Function MailMoveMessage(MsgNumber As Integer, TargetFolder As String) As Boolean
            Dim Message As Integer = MsgNumber - 1
            Dim MaxMessagesIndex As Integer = IMAP_Messages.Count - 1
            Wdbg(DebugLevel.I, "Message number {0}", Message)
            If Message < 0 Then
                Wdbg(DebugLevel.E, "Trying to move message 0 or less than 0.")
                Throw New ArgumentException(DoTranslation("Message number may not be negative or zero."))
                Return False
            ElseIf Message > MaxMessagesIndex Then
                Wdbg(DebugLevel.E, "Message {0} not in list. It was larger than MaxMessagesIndex ({1})", Message, MaxMessagesIndex)
                Throw New Exceptions.MailException(DoTranslation("Message specified is not found."))
                Return False
            End If

            SyncLock IMAP_Client.SyncRoot
                If Not IMAP_CurrentDirectory = "" And Not IMAP_CurrentDirectory = "Inbox" Then
                    'Move message
                    Dim Dir As MailFolder = OpenFolder(IMAP_CurrentDirectory)
                    Dim TargetF As MailFolder = OpenFolder(TargetFolder)
                    Wdbg(DebugLevel.I, "Opened {0}. Moving {1}...", IMAP_CurrentDirectory, MsgNumber)
                    Dir.MoveTo(IMAP_Messages(Message), TargetF)
                    Wdbg(DebugLevel.I, "Moved.")
                Else
                    'Move message
                    Dim TargetF As MailFolder = OpenFolder(TargetFolder)
                    Wdbg(DebugLevel.I, "Moving {0}...", MsgNumber)
                    IMAP_Client.Inbox.Open(FolderAccess.ReadWrite)
                    IMAP_Client.Inbox.MoveTo(IMAP_Messages(Message), TargetF)
                    Wdbg(DebugLevel.I, "Moved.")
                End If
            End SyncLock
            Return True
        End Function

        ''' <summary>
        ''' Moves all mail that the specified sender has sent
        ''' </summary>
        ''' <param name="Sender">The sender name</param>
        ''' <param name="TargetFolder">Target folder</param>
        ''' <returns>True if successful; False if unsuccessful</returns>
        Public Function MailMoveAllBySender(Sender As String, TargetFolder As String) As Boolean
            Wdbg(DebugLevel.I, "All mail by {0} will be moved.", Sender)
            Dim DeletedMsgNumber As Integer = 1
            Dim SteppedMsgNumber As Integer = 0
            For i As Integer = 0 To IMAP_Messages.Count
                Try
                    SyncLock IMAP_Client.SyncRoot
                        Dim MessageId As UniqueId = IMAP_Messages(i)
                        Dim Msg As MimeMessage
                        If Not IMAP_CurrentDirectory = "" And Not IMAP_CurrentDirectory = "Inbox" Then
                            Dim Dir As MailFolder = OpenFolder(IMAP_CurrentDirectory)
                            Msg = Dir.GetMessage(MessageId, Nothing, Mail_Progress)
                        Else
                            Msg = IMAP_Client.Inbox.GetMessage(MessageId, Nothing, Mail_Progress)
                        End If
                        SteppedMsgNumber += 1

                        For Each address In Msg.From
                            If address.Name = Sender Then
                                If Not IMAP_CurrentDirectory = "" And Not IMAP_CurrentDirectory = "Inbox" Then
                                    Dim Dir As MailFolder = OpenFolder(IMAP_CurrentDirectory)
                                    Dim TargetF As MailFolder = OpenFolder(TargetFolder)
                                    'Remove message
                                    Wdbg(DebugLevel.I, "Opened {0}. Moving {1}...", IMAP_CurrentDirectory, Sender)
                                    Dir.MoveTo(MessageId, TargetF)
                                    Wdbg(DebugLevel.I, "Moved.")
                                    Wdbg(DebugLevel.I, "Message {0} from {1} moved from {2}. {3} messages remaining to parse.", DeletedMsgNumber, Sender, IMAP_CurrentDirectory, IMAP_Messages.Count - SteppedMsgNumber)
                                    Write(DoTranslation("Message {0} from {1} moved from {2}. {3} messages remaining to parse."), True, ColTypes.Neutral, DeletedMsgNumber, Sender, IMAP_CurrentDirectory, IMAP_Messages.Count - SteppedMsgNumber)
                                Else
                                    'Remove message
                                    Dim TargetF As MailFolder = OpenFolder(TargetFolder)
                                    Wdbg(DebugLevel.I, "Moving {0}...", Sender)
                                    IMAP_Client.Inbox.Open(FolderAccess.ReadWrite)
                                    IMAP_Client.Inbox.MoveTo(MessageId, TargetF)
                                    Wdbg(DebugLevel.I, "Moved.")
                                    Wdbg(DebugLevel.I, "Message {0} from {1} moved. {2} messages remaining to parse.", DeletedMsgNumber, Sender, IMAP_Messages.Count - SteppedMsgNumber)
                                    Write(DoTranslation("Message {0} from {1} moved. {2} messages remaining to parse."), True, ColTypes.Neutral, DeletedMsgNumber, Sender, IMAP_Messages.Count - SteppedMsgNumber)
                                End If
                                DeletedMsgNumber += 1
                            End If
                        Next
                    End SyncLock
                Catch ex As Exception
                    WStkTrc(ex)
                    Return False
                End Try
            Next
            Return True
        End Function

    End Module
End Namespace
