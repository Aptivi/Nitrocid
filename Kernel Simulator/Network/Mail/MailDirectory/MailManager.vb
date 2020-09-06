
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

Imports System.Text
Imports MailKit
Imports MimeKit

Public Module MailManager

    ''' <summary>
    ''' Lists messages
    ''' </summary>
    ''' <param name="PageNum">Page number</param>
    ''' <returns>String list of messages</returns>
    ''' <exception cref="ArgumentException"></exception>
    Public Function MailListMessages(ByVal PageNum As Integer) As String
        If PageNum <= 0 Then PageNum = 1
        Wdbg("I", "Page number {0}", PageNum)
        If PageNum <= 0 Then
            Wdbg("E", "Trying to access page 0 or less than 0.")
            Throw New ArgumentException(DoTranslation("Page may not be negative or zero.", currentLang))
            Return ""
        End If

        Dim EntryBuilder As New StringBuilder
        Dim MsgsLimitForPg As Integer = 10
        Dim FirstIndex As Integer = (MsgsLimitForPg * PageNum) - 10
        Dim LastIndex As Integer = (MsgsLimitForPg * PageNum) - 1
        Dim MaxMessagesIndex As Integer = IMAP_Messages.Count - 1
        Wdbg("I", "10 messages shown in each page. First message number in page {0} is {1} and last message number in page {0} is {2}", MsgsLimitForPg, FirstIndex, LastIndex)
        For i As Integer = FirstIndex To LastIndex
            If Not i > MaxMessagesIndex Then
                Wdbg("I", "Getting message {0}...", i)
                SyncLock IMAP_Client.SyncRoot
                    Dim Msg As MimeMessage
                    If Not IMAP_CurrentDirectory = "" And Not IMAP_CurrentDirectory = "Inbox" Then
                        Dim Dir As MailFolder = OpenFolder(IMAP_CurrentDirectory)
                        Msg = Dir.GetMessage(IMAP_Messages(i))
                    Else
                        Msg = IMAP_Client.Inbox.GetMessage(IMAP_Messages(i))
                    End If
                    Dim MsgFrom As String = Msg.From.ToString
                    Dim MsgSubject As String = Msg.Subject
                    Wdbg("I", "From {0}: {1}", MsgFrom, MsgSubject)
                    EntryBuilder.AppendLine($"- [{i + 1}] {Msg.From}: {Msg.Subject}")
                End SyncLock
            Else
                Wdbg("W", "Reached max message limit. Message number {0}", i)
            End If
        Next
        Return EntryBuilder.ToString
    End Function

    ''' <summary>
    ''' Removes a message
    ''' </summary>
    ''' <param name="MsgNumber">Message number</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    Public Function MailRemoveMessage(ByVal MsgNumber As Integer) As Boolean
        Dim Message As Integer = MsgNumber - 1
        Dim MaxMessagesIndex As Integer = IMAP_Messages.Count - 1
        Wdbg("I", "Message number {0}", Message)
        If Message < 0 Then
            Wdbg("E", "Trying to remove message 0 or less than 0.")
            Throw New ArgumentException(DoTranslation("Message number may not be negative or zero.", currentLang))
            Return False
        ElseIf Message > MaxMessagesIndex Then
            Wdbg("E", "Message {0} not in list. It was larger than MaxMessagesIndex ({1})", Message, MaxMessagesIndex)
            Throw New EventsAndExceptions.MailException(DoTranslation("Message specified is not found.", currentLang))
            Return False
        End If

        SyncLock IMAP_Client.SyncRoot
            If Not IMAP_CurrentDirectory = "" And Not IMAP_CurrentDirectory = "Inbox" Then
                'Remove message
                Dim Dir As MailFolder = OpenFolder(IMAP_CurrentDirectory)
                Wdbg("I", "Opened {0}. Removing {1}...", IMAP_CurrentDirectory, MsgNumber)
                Dir.AddFlags(Message, MessageFlags.Deleted, True)
                Wdbg("I", "Removed.")
                Dir.Expunge()
            Else
                'Remove message
                Wdbg("I", "Removing {0}...", MsgNumber)
                IMAP_Client.Inbox.AddFlags(Message, MessageFlags.Deleted, True)
                Wdbg("I", "Removed.")
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
    Public Function MailRemoveAllBySender(ByVal Sender As String) As Boolean
        Wdbg("I", "All mail by {0} will be removed.", Sender)
        Dim DeletedMsgNumber As Integer = 1
        Dim SteppedMsgNumber As Integer = 0
        For i As Integer = 0 To IMAP_Messages.Count
            Try
                SyncLock IMAP_Client.SyncRoot
                    Dim MessageId As UniqueId = IMAP_Messages(i)
                    Dim Msg As MimeMessage
                    If Not IMAP_CurrentDirectory = "" And Not IMAP_CurrentDirectory = "Inbox" Then
                        Dim Dir As MailFolder = OpenFolder(IMAP_CurrentDirectory)
                        Msg = Dir.GetMessage(MessageId)
                    Else
                        Msg = IMAP_Client.Inbox.GetMessage(MessageId)
                    End If
                    SteppedMsgNumber += 1

                    For Each address In Msg.From
                        If address.Name = Sender Then
                            If Not IMAP_CurrentDirectory = "" And Not IMAP_CurrentDirectory = "Inbox" Then
                                Dim Dir As MailFolder = OpenFolder(IMAP_CurrentDirectory)

                                'Remove message
                                Wdbg("I", "Opened {0}. Removing {1}...", IMAP_CurrentDirectory, Sender)
                                Dir.AddFlags(MessageId, MessageFlags.Deleted, True)
                                Wdbg("I", "Removed.")
                                Dir.Expunge()
                                Wdbg("I", "Message {0} from {1} deleted from {2}. {3} messages remaining to parse.", DeletedMsgNumber, Sender, IMAP_CurrentDirectory, IMAP_Messages.Count - SteppedMsgNumber)
                                W(DoTranslation("Message {0} from {1} deleted from {2}. {3} messages remaining to parse.", currentLang), True, ColTypes.Neutral, DeletedMsgNumber, Sender, IMAP_CurrentDirectory, IMAP_Messages.Count - SteppedMsgNumber)
                            Else
                                'Remove message
                                Wdbg("I", "Removing {0}...", Sender)
                                IMAP_Client.Inbox.AddFlags(MessageId, MessageFlags.Deleted, True)
                                Wdbg("I", "Removed.")
                                IMAP_Client.Inbox.Expunge()
                                Wdbg("I", "Message {0} from {1} deleted from inbox. {2} messages remaining to parse.", DeletedMsgNumber, Sender, IMAP_Messages.Count - SteppedMsgNumber)
                                W(DoTranslation("Message {0} from {1} deleted from inbox. {2} messages remaining to parse.", currentLang), True, ColTypes.Neutral, DeletedMsgNumber, Sender, IMAP_Messages.Count - SteppedMsgNumber)
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
