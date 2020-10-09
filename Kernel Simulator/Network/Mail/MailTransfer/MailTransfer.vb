
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

Imports System.IO
Imports System.Text
Imports MailKit
Imports MimeKit
Imports MimeKit.Cryptography
Imports MimeKit.Text

Public Module MailTransfer

    ''' <summary>
    ''' Prints content of message to console
    ''' </summary>
    ''' <param name="MessageNum">Message number</param>
    Public Sub MailPrintMessage(ByVal MessageNum As Integer, Optional ByVal Decrypt As Boolean = False)
        Dim Message As Integer = MessageNum - 1
        Dim MaxMessagesIndex As Integer = IMAP_Messages.Count - 1
        Wdbg("I", "Message number {0}", Message)
        If Message < 0 Then
            Wdbg("E", "Trying to access message 0 or less than 0.")
            W(DoTranslation("Message number may not be negative or zero.", currentLang), True, ColTypes.Err)
            Exit Sub
        ElseIf Message > MaxMessagesIndex Then
            Wdbg("E", "Message {0} not in list. It was larger than MaxMessagesIndex ({1})", Message, MaxMessagesIndex)
            W(DoTranslation("Message specified is not found.", currentLang), True, ColTypes.Err)
            Exit Sub
        End If

        SyncLock IMAP_Client.SyncRoot
            'Get message
            Wdbg("I", "Getting message...")
            Dim Msg As MimeMessage
            If Not IMAP_CurrentDirectory = "" And Not IMAP_CurrentDirectory = "Inbox" Then
                Dim Dir As MailFolder = OpenFolder(IMAP_CurrentDirectory)
                Msg = Dir.GetMessage(IMAP_Messages(Message))
            Else
                Msg = IMAP_Client.Inbox.GetMessage(IMAP_Messages(Message))
            End If

            'Prepare view
            Console.WriteLine()

            'Print all the addresses that sent the mail
            Wdbg("I", "{0} senders.", Msg.From.Count)
            For Each Address As InternetAddress In Msg.From
                Wdbg("I", "Address: {0} ({1})", Address.Name, Address.Encoding.EncodingName)
                W(DoTranslation("- From {0}", currentLang), True, ColTypes.HelpCmd, Address.ToString)
            Next

            'Print all the addresses that received the mail
            Wdbg("I", "{0} receivers.", Msg.To.Count)
            For Each Address As InternetAddress In Msg.To
                Wdbg("I", "Address: {0} ({1})", Address.Name, Address.Encoding.EncodingName)
                W(DoTranslation("- To {0}", currentLang), True, ColTypes.HelpCmd, Address.ToString)
            Next

            'Print the date and time when the user received the mail
            Wdbg("I", "Rendering time and date of {0}.", Msg.Date.DateTime.ToString)
            W(DoTranslation("- Sent at {0} in {1}", currentLang), True, ColTypes.HelpCmd, RenderTime(Msg.Date.DateTime), RenderDate(Msg.Date.DateTime))

            'Prepare subject
            Console.WriteLine()
            Wdbg("I", "Subject length: {0}, {1}", Msg.Subject.Length, Msg.Subject)
            W($"- {Msg.Subject}", False, ColTypes.HelpCmd)

            'Write a sign after the subject if attachments are found
            Wdbg("I", "Attachments count: {0}", Msg.Attachments.Count)
            If Msg.Attachments.Count > 0 Then
                W(" - [*]", True, ColTypes.HelpCmd)
            Else
                Console.WriteLine()
            End If

            'Prepare body
            Console.WriteLine()
            Wdbg("I", "Displaying body...")
            Dim DecryptedMessage As Dictionary(Of String, MimeEntity)
            If Decrypt Then
                DecryptedMessage = DecryptMessage(Msg)
                Dim DecryptedEntity As MimeEntity = DecryptedMessage("Body")
                Dim DecryptedStream As New MemoryStream
                If TypeOf DecryptedEntity Is Multipart Then
                    Dim MultiEntity As Multipart = CType(DecryptedEntity, Multipart)
                    If Not IsNothing(MultiEntity) Then
                        For EntityNumber As Integer = 0 To MultiEntity.Count - 1
                            If Not MultiEntity(EntityNumber).IsAttachment Then
                                MultiEntity(EntityNumber).WriteTo(DecryptedStream, True)
                                DecryptedStream.Position = 0
                                Dim DecryptedByte(DecryptedStream.Length) As Byte
                                DecryptedStream.Read(DecryptedByte, 0, DecryptedStream.Length)
                                W(Encoding.Default.GetString(DecryptedByte), True, ColTypes.HelpDef)
                            End If
                        Next
                    End If
                Else
                    DecryptedEntity.WriteTo(DecryptedStream, True)
                    DecryptedStream.Position = 0
                    Dim DecryptedByte(DecryptedStream.Length) As Byte
                    DecryptedStream.Read(DecryptedByte, 0, DecryptedStream.Length)
                    W(Encoding.Default.GetString(DecryptedByte), True, ColTypes.HelpDef)
                End If
            Else
                W(Msg.GetTextBody(TextFormat.Plain), True, ColTypes.HelpDef)
            End If
            Console.WriteLine()

            'Populate attachments
#Disable Warning BC42104
            If Msg.Attachments.Count > 0 Then
                W(DoTranslation("Attachments:", currentLang), True, ColTypes.Neutral)
                Dim AttachmentEntities As New List(Of MimeEntity)
                If Decrypt Then
                    For DecryptedEntityNumber As Integer = 0 To DecryptedMessage.Count - 1
                        If DecryptedMessage.Keys(DecryptedEntityNumber).Contains("Attachment") Then
                            AttachmentEntities.Add(DecryptedMessage.Values(DecryptedEntityNumber))
                        ElseIf DecryptedMessage.Keys(DecryptedEntityNumber) = "Body" And TypeOf DecryptedMessage("Body") Is Multipart Then
                            Dim MultiEntity As Multipart = CType(DecryptedMessage("Body"), Multipart)
                            If Not IsNothing(MultiEntity) Then
                                For EntityNumber As Integer = 0 To MultiEntity.Count - 1
                                    If MultiEntity(EntityNumber).IsAttachment Then
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
                    Wdbg("I", "Attachment ID: {0}", Attachment.ContentId)
                    If TypeOf Attachment Is MessagePart Then
                        Wdbg("I", "Attachment is a message.")
                        W($"- {Attachment.ContentDisposition?.FileName}", True, ColTypes.Neutral)
                    Else
                        Wdbg("I", "Attachment is a file.")
                        Dim AttachmentPart As MimePart = Attachment
                        W($"- {AttachmentPart.FileName}", True, ColTypes.Neutral)
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
    Public Function DecryptMessage(ByVal Text As MimeMessage) As Dictionary(Of String, MimeEntity)
        Dim EncryptedDict As New Dictionary(Of String, MimeEntity)
        If TypeOf Text.Body Is MultipartEncrypted Then
            Dim Encrypted = CType(Text.Body, MultipartEncrypted)
            EncryptedDict.Add("Body", Encrypted.Decrypt(New PGPContext))
        Else
            EncryptedDict.Add("Body", Text.Body)
        End If
        Dim AttachmentNumber As Integer = 1
        For Each TextAttachment As MimeEntity In Text.Attachments
            If TypeOf TextAttachment Is MultipartEncrypted Then
                Dim Encrypted = CType(TextAttachment, MultipartEncrypted)
                EncryptedDict.Add("Attachment " & AttachmentNumber, Encrypted.Decrypt(New PGPContext))
            Else
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
    Public Function MailSendMessage(ByVal Recipient As String, ByVal Subject As String, ByVal Body As String) As String
        SyncLock SMTP_Client.SyncRoot
            Try
                Dim FinalMessage As New MimeMessage
                FinalMessage.From.Add(MailboxAddress.Parse(Mail_Authentication.UserName))
                Wdbg("I", "Added sender to FinalMessage.From.")
                FinalMessage.To.Add(MailboxAddress.Parse(Recipient))
                Wdbg("I", "Added address to FinalMessage.To.")
                FinalMessage.Subject = Subject
                Wdbg("I", "Added subject to FinalMessage.Subject.")
                FinalMessage.Body = New TextPart(TextFormat.Plain) With {.Text = Body.ToString}
                Wdbg("I", "Added body to FinalMessage.Body (plain text). Sending message...")
                SMTP_Client.Send(FinalMessage)
                Return True
            Catch ex As Exception
                Wdbg("E", "Failed to send message: {0}", ex.Message)
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
    Public Function MailSendMessage(ByVal Recipient As String, ByVal Subject As String, ByVal Body As MimeEntity) As String
        SyncLock SMTP_Client.SyncRoot
            Try
                Dim FinalMessage As New MimeMessage
                FinalMessage.From.Add(MailboxAddress.Parse(Mail_Authentication.UserName))
                Wdbg("I", "Added sender to FinalMessage.From.")
                FinalMessage.To.Add(MailboxAddress.Parse(Recipient))
                Wdbg("I", "Added address to FinalMessage.To.")
                FinalMessage.Subject = Subject
                Wdbg("I", "Added subject to FinalMessage.Subject.")
                FinalMessage.Body = Body
                Wdbg("I", "Added body to FinalMessage.Body (plain text). Sending message...")
                SMTP_Client.Send(FinalMessage)
                Return True
            Catch ex As Exception
                Wdbg("E", "Failed to send message: {0}", ex.Message)
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
    Public Function MailSendEncryptedMessage(ByVal Recipient As String, ByVal Subject As String, ByVal Body As MimeEntity) As String
        SyncLock SMTP_Client.SyncRoot
            Try
                Dim FinalMessage As New MimeMessage
                FinalMessage.From.Add(MailboxAddress.Parse(Mail_Authentication.UserName))
                Wdbg("I", "Added sender to FinalMessage.From.")
                FinalMessage.To.Add(MailboxAddress.Parse(Recipient))
                Wdbg("I", "Added address to FinalMessage.To.")
                FinalMessage.Subject = Subject
                Wdbg("I", "Added subject to FinalMessage.Subject.")
                FinalMessage.Body = MultipartEncrypted.Encrypt(New PGPContext, FinalMessage.To.Mailboxes, Body)
                Wdbg("I", "Added body to FinalMessage.Body (plain text). Sending message...")
                SMTP_Client.Send(FinalMessage)
                Return True
            Catch ex As Exception
                Wdbg("E", "Failed to send message: {0}", ex.Message)
                WStkTrc(ex)
            End Try
            Return False
        End SyncLock
    End Function

End Module
