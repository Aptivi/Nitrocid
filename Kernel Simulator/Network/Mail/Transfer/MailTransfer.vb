
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

Imports System.IO
Imports System.Text
Imports MailKit
Imports MailKit.Search
Imports MimeKit
Imports MimeKit.Cryptography
Imports MimeKit.Text
Imports KS.Shell.Shells.Mail
Imports KS.Network.Mail.Directory
Imports KS.Network.Mail.PGP
Imports KS.TimeDate

Namespace Network.Mail.Transfer
    Public Module MailTransfer

        ''' <summary>
        ''' Prints content of message to console
        ''' </summary>
        ''' <param name="MessageNum">Message number</param>
        Public Sub MailPrintMessage(MessageNum As Integer, Optional Decrypt As Boolean = False)
            Dim Message As Integer = MessageNum - 1
            Dim MaxMessagesIndex As Integer = IMAP_Messages.Count - 1
            Wdbg(DebugLevel.I, "Message number {0}", Message)
            If Message < 0 Then
                Wdbg(DebugLevel.E, "Trying to access message 0 or less than 0.")
                Write(DoTranslation("Message number may not be negative or zero."), True, ColTypes.Error)
                Exit Sub
            ElseIf Message > MaxMessagesIndex Then
                Wdbg(DebugLevel.E, "Message {0} not in list. It was larger than MaxMessagesIndex ({1})", Message, MaxMessagesIndex)
                Write(DoTranslation("Message specified is not found."), True, ColTypes.Error)
                Exit Sub
            End If

            SyncLock IMAP_Client.SyncRoot
                'Get message
                Wdbg(DebugLevel.I, "Getting message...")
                Dim Msg As MimeMessage
                If Not IMAP_CurrentDirectory = "" And Not IMAP_CurrentDirectory = "Inbox" Then
                    Dim Dir As MailFolder = OpenFolder(IMAP_CurrentDirectory)
                    Msg = Dir.GetMessage(IMAP_Messages(Message), Nothing, Mail_Progress)
                Else
                    Msg = IMAP_Client.Inbox.GetMessage(IMAP_Messages(Message), Nothing, Mail_Progress)
                End If

                'Prepare view
                Console.WriteLine()

                'Print all the addresses that sent the mail
                Wdbg(DebugLevel.I, "{0} senders.", Msg.From.Count)
                For Each Address As InternetAddress In Msg.From
                    Wdbg(DebugLevel.I, "Address: {0} ({1})", Address.Name, Address.Encoding.EncodingName)
                    Write(DoTranslation("- From {0}"), True, ColTypes.ListEntry, Address.ToString)
                Next

                'Print all the addresses that received the mail
                Wdbg(DebugLevel.I, "{0} receivers.", Msg.To.Count)
                For Each Address As InternetAddress In Msg.To
                    Wdbg(DebugLevel.I, "Address: {0} ({1})", Address.Name, Address.Encoding.EncodingName)
                    Write(DoTranslation("- To {0}"), True, ColTypes.ListEntry, Address.ToString)
                Next

                'Print the date and time when the user received the mail
                Wdbg(DebugLevel.I, "Rendering time and date of {0}.", Msg.Date.DateTime.ToString)
                Write(DoTranslation("- Sent at {0} in {1}"), True, ColTypes.ListEntry, RenderTime(Msg.Date.DateTime), RenderDate(Msg.Date.DateTime))

                'Prepare subject
                Console.WriteLine()
                Wdbg(DebugLevel.I, "Subject length: {0}, {1}", Msg.Subject.Length, Msg.Subject)
                Write($"- {Msg.Subject}", False, ColTypes.ListEntry)

                'Write a sign after the subject if attachments are found
                Wdbg(DebugLevel.I, "Attachments count: {0}", Msg.Attachments.Count)
                If Msg.Attachments.Count > 0 Then
                    Write(" - [*]", True, ColTypes.ListEntry)
                Else
                    Console.WriteLine()
                End If

                'Prepare body
                Console.WriteLine()
                Wdbg(DebugLevel.I, "Displaying body...")
                Dim DecryptedMessage As Dictionary(Of String, MimeEntity)
                Wdbg(DebugLevel.I, "To decrypt: {0}", Decrypt)
                If Decrypt Then
                    DecryptedMessage = DecryptMessage(Msg)
                    Wdbg(DebugLevel.I, "Decrypted messages length: {0}", DecryptedMessage.Count)
                    Dim DecryptedEntity As MimeEntity = DecryptedMessage("Body")
                    Dim DecryptedStream As New MemoryStream
                    Wdbg(DebugLevel.I, $"Decrypted message type: {If(TypeOf DecryptedEntity Is Multipart, "Multipart", "Singlepart")}")
                    If TypeOf DecryptedEntity Is Multipart Then
                        Dim MultiEntity As Multipart = CType(DecryptedEntity, Multipart)
                        Wdbg(DebugLevel.I, $"Decrypted message entity is {If(MultiEntity IsNot Nothing, "multipart", "nothing")}")
                        If MultiEntity IsNot Nothing Then
                            For EntityNumber As Integer = 0 To MultiEntity.Count - 1
                                Wdbg(DebugLevel.I, $"Entity number {EntityNumber} is {If(MultiEntity(EntityNumber).IsAttachment, "an attachment", "not an attachment")}")
                                If Not MultiEntity(EntityNumber).IsAttachment Then
                                    MultiEntity(EntityNumber).WriteTo(DecryptedStream, True)
                                    Wdbg(DebugLevel.I, "Written {0} bytes to stream.", DecryptedStream.Length)
                                    DecryptedStream.Position = 0
                                    Dim DecryptedByte(DecryptedStream.Length) As Byte
                                    DecryptedStream.Read(DecryptedByte, 0, DecryptedStream.Length)
                                    Wdbg(DebugLevel.I, "Written {0} bytes to buffer.", DecryptedByte.Length)
                                    Write(Encoding.Default.GetString(DecryptedByte), True, ColTypes.ListValue)
                                End If
                            Next
                        End If
                    Else
                        DecryptedEntity.WriteTo(DecryptedStream, True)
                        Wdbg(DebugLevel.I, "Written {0} bytes to stream.", DecryptedStream.Length)
                        DecryptedStream.Position = 0
                        Dim DecryptedByte(DecryptedStream.Length) As Byte
                        DecryptedStream.Read(DecryptedByte, 0, DecryptedStream.Length)
                        Wdbg(DebugLevel.I, "Written {0} bytes to buffer.", DecryptedByte.Length)
                        Write(Encoding.Default.GetString(DecryptedByte), True, ColTypes.ListValue)
                    End If
                Else
                    Write(Msg.GetTextBody(Mail_TextFormat), True, ColTypes.ListValue)
                End If
                Console.WriteLine()

                'Populate attachments
#Disable Warning BC42104
                If Msg.Attachments.Count > 0 Then
                    Write(DoTranslation("Attachments:"), True, ColTypes.Neutral)
                    Dim AttachmentEntities As New List(Of MimeEntity)
                    If Decrypt Then
                        Wdbg(DebugLevel.I, "Parsing attachments...")
                        For DecryptedEntityNumber As Integer = 0 To DecryptedMessage.Count - 1
                            Wdbg(DebugLevel.I, "Is entity number {0} an attachment? {1}", DecryptedEntityNumber, DecryptedMessage.Keys(DecryptedEntityNumber).Contains("Attachment"))
                            Wdbg(DebugLevel.I, "Is entity number {0} a body that is a multipart? {1}", DecryptedEntityNumber, DecryptedMessage.Keys(DecryptedEntityNumber) = "Body" And TypeOf DecryptedMessage("Body") Is Multipart)
                            If DecryptedMessage.Keys(DecryptedEntityNumber).Contains("Attachment") Then
                                Wdbg(DebugLevel.I, "Adding entity {0} to attachment entities...", DecryptedEntityNumber)
                                AttachmentEntities.Add(DecryptedMessage.Values(DecryptedEntityNumber))
                            ElseIf DecryptedMessage.Keys(DecryptedEntityNumber) = "Body" And TypeOf DecryptedMessage("Body") Is Multipart Then
                                Dim MultiEntity As Multipart = CType(DecryptedMessage("Body"), Multipart)
                                Wdbg(DebugLevel.I, $"Decrypted message entity is {If(MultiEntity IsNot Nothing, "multipart", "nothing")}")
                                If MultiEntity IsNot Nothing Then
                                    Wdbg(DebugLevel.I, "{0} entities found.", MultiEntity.Count)
                                    For EntityNumber As Integer = 0 To MultiEntity.Count - 1
                                        Wdbg(DebugLevel.I, $"Entity number {EntityNumber} is {If(MultiEntity(EntityNumber).IsAttachment, "an attachment", "not an attachment")}")
                                        If MultiEntity(EntityNumber).IsAttachment Then
                                            Wdbg(DebugLevel.I, "Adding entity {0} to attachment list...", EntityNumber)
                                            AttachmentEntities.Add(MultiEntity(EntityNumber))
                                        End If
                                    Next
                                End If
                            End If
                        Next
                    Else
                        AttachmentEntities = Msg.Attachments
                    End If
                    For Each Attachment As MimeEntity In AttachmentEntities
                        Wdbg(DebugLevel.I, "Attachment ID: {0}", Attachment.ContentId)
                        If TypeOf Attachment Is MessagePart Then
                            Wdbg(DebugLevel.I, "Attachment is a message.")
                            Write($"- {Attachment.ContentDisposition?.FileName}", True, ColTypes.Neutral)
                        Else
                            Wdbg(DebugLevel.I, "Attachment is a file.")
                            Dim AttachmentPart As MimePart = Attachment
                            Write($"- {AttachmentPart.FileName}", True, ColTypes.Neutral)
                        End If
                    Next
                End If
#Enable Warning BC42104
            End SyncLock
        End Sub

        ''' <summary>
        ''' Decrypts a message
        ''' </summary>
        ''' <param name="Text">Text part</param>
        ''' <returns>A decrypted message, or null if unsuccessful.</returns>
        Public Function DecryptMessage(Text As MimeMessage) As Dictionary(Of String, MimeEntity)
            Dim EncryptedDict As New Dictionary(Of String, MimeEntity)
            Wdbg(DebugLevel.I, $"Encrypted message type: {If(TypeOf Text.Body Is MultipartEncrypted, "Multipart", "Singlepart")}")
            If TypeOf Text.Body Is MultipartEncrypted Then
                Dim Encrypted = CType(Text.Body, MultipartEncrypted)
                Wdbg(DebugLevel.I, $"Message type: {If(Encrypted IsNot Nothing, "MultipartEncrypted", "Nothing")}")
                Wdbg(DebugLevel.I, "Decrypting...")
                EncryptedDict.Add("Body", Encrypted.Decrypt(New PGPContext))
            Else
                Wdbg(DebugLevel.W, "Trying to decrypt plain text. Returning body...")
                EncryptedDict.Add("Body", Text.Body)
            End If
            Dim AttachmentNumber As Integer = 1
            For Each TextAttachment As MimeEntity In Text.Attachments
                Wdbg(DebugLevel.I, "Attachment number {0}", AttachmentNumber)
                Wdbg(DebugLevel.I, $"Encrypted attachment type: {If(TypeOf TextAttachment Is MultipartEncrypted, "Multipart", "Singlepart")}")
                If TypeOf TextAttachment Is MultipartEncrypted Then
                    Dim Encrypted = CType(TextAttachment, MultipartEncrypted)
                    Wdbg(DebugLevel.I, $"Attachment type: {If(Encrypted IsNot Nothing, "MultipartEncrypted", "Nothing")}")
                    Wdbg(DebugLevel.I, "Decrypting...")
                    EncryptedDict.Add("Attachment " & AttachmentNumber, Encrypted.Decrypt(New PGPContext))
                Else
                    Wdbg(DebugLevel.W, "Trying to decrypt plain attachment. Returning body...")
                    EncryptedDict.Add("Attachment " & AttachmentNumber, TextAttachment)
                End If
                AttachmentNumber += 1
            Next
            Return EncryptedDict
        End Function

        ''' <summary>
        ''' Sends a message
        ''' </summary>
        ''' <param name="Recipient">Recipient name</param>
        ''' <param name="Subject">Subject</param>
        ''' <param name="Body">Body (only text. See <see cref="MailSendMessage(String, String, MimeEntity)"/> for more.)</param>
        ''' <returns>True if successful; False if unsuccessful.</returns>
        Public Function MailSendMessage(Recipient As String, Subject As String, Body As String) As String
            'Construct a message
            Dim FinalMessage As New MimeMessage
            FinalMessage.From.Add(MailboxAddress.Parse(Mail_Authentication.UserName))
            Wdbg(DebugLevel.I, "Added sender to FinalMessage.From.")
            FinalMessage.To.Add(MailboxAddress.Parse(Recipient))
            Wdbg(DebugLevel.I, "Added address to FinalMessage.To.")
            FinalMessage.Subject = Subject
            Wdbg(DebugLevel.I, "Added subject to FinalMessage.Subject.")
            FinalMessage.Body = New TextPart(TextFormat.Plain) With {.Text = Body.ToString}
            Wdbg(DebugLevel.I, "Added body to FinalMessage.Body (plain text). Sending message...")

            'Send the message
            SyncLock SMTP_Client.SyncRoot
                Try
                    SMTP_Client.Send(FinalMessage, Nothing, Mail_Progress)
                    Return True
                Catch ex As Exception
                    Wdbg(DebugLevel.E, "Failed to send message: {0}", ex.Message)
                    WStkTrc(ex)
                End Try
                Return False
            End SyncLock
        End Function

        ''' <summary>
        ''' Sends a message with advanced features like attachments
        ''' </summary>
        ''' <param name="Recipient">Recipient name</param>
        ''' <param name="Subject">Subject</param>
        ''' <param name="Body">Body</param>
        ''' <returns>True if successful; False if unsuccessful.</returns>
        Public Function MailSendMessage(Recipient As String, Subject As String, Body As MimeEntity) As String
            'Construct a message
            Dim FinalMessage As New MimeMessage
            FinalMessage.From.Add(MailboxAddress.Parse(Mail_Authentication.UserName))
            Wdbg(DebugLevel.I, "Added sender to FinalMessage.From.")
            FinalMessage.To.Add(MailboxAddress.Parse(Recipient))
            Wdbg(DebugLevel.I, "Added address to FinalMessage.To.")
            FinalMessage.Subject = Subject
            Wdbg(DebugLevel.I, "Added subject to FinalMessage.Subject.")
            FinalMessage.Body = Body
            Wdbg(DebugLevel.I, "Added body to FinalMessage.Body (plain text). Sending message...")

            'Send the message
            SyncLock SMTP_Client.SyncRoot
                Try
                    SMTP_Client.Send(FinalMessage, Nothing, Mail_Progress)
                    Return True
                Catch ex As Exception
                    Wdbg(DebugLevel.E, "Failed to send message: {0}", ex.Message)
                    WStkTrc(ex)
                End Try
                Return False
            End SyncLock
        End Function

        ''' <summary>
        ''' Sends an encrypted message with advanced features like attachments
        ''' </summary>
        ''' <param name="Recipient">Recipient name</param>
        ''' <param name="Subject">Subject</param>
        ''' <param name="Body">Body</param>
        ''' <returns>True if successful; False if unsuccessful.</returns>
        Public Function MailSendEncryptedMessage(Recipient As String, Subject As String, Body As MimeEntity) As String
            'Construct a message
            Dim FinalMessage As New MimeMessage
            FinalMessage.From.Add(MailboxAddress.Parse(Mail_Authentication.UserName))
            Wdbg(DebugLevel.I, "Added sender to FinalMessage.From.")
            FinalMessage.To.Add(MailboxAddress.Parse(Recipient))
            Wdbg(DebugLevel.I, "Added address to FinalMessage.To.")
            FinalMessage.Subject = Subject
            Wdbg(DebugLevel.I, "Added subject to FinalMessage.Subject.")
            FinalMessage.Body = MultipartEncrypted.Encrypt(New PGPContext, FinalMessage.To.Mailboxes, Body)
            Wdbg(DebugLevel.I, "Added body to FinalMessage.Body (plain text). Sending message...")

            'Send the message
            SyncLock SMTP_Client.SyncRoot
                Try
                    SMTP_Client.Send(FinalMessage, Nothing, Mail_Progress)
                    Return True
                Catch ex As Exception
                    Wdbg(DebugLevel.E, "Failed to send message: {0}", ex.Message)
                    WStkTrc(ex)
                End Try
                Return False
            End SyncLock
        End Function

        ''' <summary>
        ''' Populates e-mail messages
        ''' </summary>
        Public Sub PopulateMessages()
            If IMAP_Client.IsConnected Then
                SyncLock IMAP_Client.SyncRoot
                    If IMAP_CurrentDirectory = "" Or IMAP_CurrentDirectory = "Inbox" Then
                        IMAP_Client.Inbox.Open(FolderAccess.ReadWrite)
                        Wdbg(DebugLevel.I, "Opened inbox")
                        IMAP_Messages = IMAP_Client.Inbox.Search(SearchQuery.All).Reverse
                        Wdbg(DebugLevel.I, "Messages count: {0} messages", IMAP_Messages.LongCount)
                    Else
                        Dim Folder As MailFolder = OpenFolder(IMAP_CurrentDirectory)
                        Wdbg(DebugLevel.I, "Opened {0}", IMAP_CurrentDirectory)
                        IMAP_Messages = Folder.Search(SearchQuery.All).Reverse
                        Wdbg(DebugLevel.I, "Messages count: {0} messages", IMAP_Messages.LongCount)
                    End If
                End SyncLock
            End If
        End Sub

    End Module
End Namespace

