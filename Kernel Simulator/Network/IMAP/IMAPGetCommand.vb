
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

Imports MimeKit

Module IMAPGetCommand

    Sub IMAP_ExecuteCommand(ByVal cmd As String, ByVal args As String)
        Dim FullArgsL As List(Of String) = args.Split(" ").ToList
        Wdbg("I", "Arguments count: {0}", FullArgsL.Count)

        Try
            If cmd = "help" Then
                IMAPShowHelp()
            ElseIf cmd = "exit" Then
                ExitRequested = True
            ElseIf cmd = "list" Then
                Wdbg("I", "Page is numeric? {0}", FullArgsL(0).IsNumeric)
                If FullArgsL(0).IsNumeric Then
                    Dim Page As Integer
                    If FullArgsL(0) = "" Then Page = 1 Else Page = FullArgsL(0)
                    Wdbg("I", "Page number {0}", Page)
                    If Page <= 0 Then
                        Wdbg("E", "Trying to access page 0 or less than 0.")
                        W(DoTranslation("Page may not be negative or zero.", currentLang), True, ColTypes.Err)
                        Exit Sub
                    End If
                    Dim MsgsLimitForPg As Integer = 10
                    Dim FirstIndex As Integer = (MsgsLimitForPg * Page) - 10
                    Dim LastIndex As Integer = (MsgsLimitForPg * Page) - 1
                    Dim MaxMessagesIndex As Integer = IMAP_Messages.Count - 1
                    Wdbg("I", "10 messages shown in each page. First message number in page {0} is {1} and last message number in page {0} is {2}", MsgsLimitForPg, FirstIndex, LastIndex)
                    For i As Integer = FirstIndex To LastIndex
                        If Not i > MaxMessagesIndex Then
                            Wdbg("I", "Getting message {0}...", i)
                            Dim Msg As MimeMessage = IMAP_Client.Inbox.GetMessage(IMAP_Messages(i))
                            Dim MsgFrom As String = Msg.From.ToString
                            Dim MsgSubject As String = Msg.Subject
                            Wdbg("I", "From {0}: {1}", MsgFrom, MsgSubject)
                            W("- [{0}] {1}: ", False, ColTypes.HelpCmd, i + 1, Msg.From) : W("{0}", True, ColTypes.HelpDef, Msg.Subject)
                        Else
                            Wdbg("W", "Reached max message limit. Message number {0}", i)
                        End If
                    Next
                Else
                    W(DoTranslation("Page is not a numeric value.", currentLang), True, ColTypes.Err)
                End If
            ElseIf cmd = "read" Then
                Wdbg("I", "Message number is numeric? {0}", FullArgsL(0).IsNumeric)
                If FullArgsL(0).IsNumeric Then
                    Dim Message As Integer = FullArgsL(0) - 1
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

                    'Get message
                    Wdbg("I", "Getting message...")
                    Dim Msg As MimeMessage = IMAP_Client.Inbox.GetMessage(IMAP_Messages(Message))

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
                    W(Msg.GetTextBody(Text.TextFormat.Plain), True, ColTypes.HelpDef)
                    Console.WriteLine()

                    'Populate attachments
                    If Msg.Attachments.Count > 0 Then
                        W(DoTranslation("Attachments:", currentLang), True, ColTypes.Neutral)
                        For Each Attachment As MimeEntity In Msg.Attachments
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
                Else
                    W(DoTranslation("Message number is not a numeric value.", currentLang), True, ColTypes.Err)
                End If
            End If
        Catch ex As Exception
            W(DoTranslation("Error executing IMAP command: {0}", currentLang), True, ColTypes.Err, ex.Message)
            WStkTrc(ex)
        End Try
    End Sub

End Module
