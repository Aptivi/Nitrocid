
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
Imports System.Threading
Imports MailKit
Imports MimeKit
Imports MimeKit.Text

Module MailGetCommand

    Public MailStartCommandThread As New Thread(AddressOf Mail_ExecuteCommand)

    ''' <summary>
    ''' Parses and executes the specified command with arguments
    ''' </summary>
    ''' <param name="Parameters">Two strings. The first one is the command string, and the other is the arguments string.</param>
    Sub Mail_ExecuteCommand(ByVal Parameters As String()) 'This is converted to String() to ensure compatibility with Threading.Thread.
        Dim cmd As String = Parameters(0)
        Dim args As String = Parameters(1)
        Dim FullArgsL As List(Of String) = args.Split({" "}, StringSplitOptions.RemoveEmptyEntries).ToList
        Dim RequiredArgsProvided As Boolean
        Wdbg("I", "Arguments count: {0}", FullArgsL.Count)

        Try
            If cmd = "help" Then
                RequiredArgsProvided = True
                IMAPShowHelp()
            ElseIf cmd = "cd" Then
                If FullArgsL.Count > 0 Then
                    RequiredArgsProvided = True
                    MailChangeDirectory(FullArgsL(0))
                End If
            ElseIf cmd = "exit" Then
                RequiredArgsProvided = True
                ExitRequested = True
                W(DoTranslation("Do you want to keep connected?", currentLang) + " <y/n> ", False, ColTypes.Input)
                Dim Answer As Char = Console.ReadKey.KeyChar
                Console.WriteLine()
                If Answer = "y" Then
                    KeepAlive = True
                ElseIf Answer = "n" Then
                    KeepAlive = False
                Else
                    W(DoTranslation("Invalid choice. Assuming no...", currentLang), True, ColTypes.Input)
                End If
            ElseIf cmd = "list" Then
                If FullArgsL.Count > 0 Then
                    RequiredArgsProvided = True
                    Wdbg("I", "Page is numeric? {0}", FullArgsL(0).IsNumeric)
                    If FullArgsL(0).IsNumeric Then
                        W(MailListMessages(FullArgsL(0)), False, ColTypes.Neutral)
                    Else
                        W(DoTranslation("Page is not a numeric value.", currentLang), True, ColTypes.Err)
                    End If
                End If
            ElseIf cmd = "lsdirs" Then
                RequiredArgsProvided = True
                W(MailListDirectories, False, ColTypes.Neutral)
            ElseIf cmd = "read" Then
                If FullArgsL.Count > 0 Then
                    RequiredArgsProvided = True
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
                        End SyncLock
                    Else
                        W(DoTranslation("Message number is not a numeric value.", currentLang), True, ColTypes.Err)
                    End If
                End If
            ElseIf cmd = "send" Then
                RequiredArgsProvided = True
                Dim Receiver, Subject As String
                Dim Body As New StringBuilder

                'Prompt for receiver e-mail address
                W(DoTranslation("Enter recipient mail address:", currentLang) + " ", False, ColTypes.Input)
                Receiver = Console.ReadLine
                Wdbg("I", "Recipient: {0}", Receiver)

                'Check for mail format
                If Receiver.Contains("@") And Receiver.Substring(Receiver.IndexOf("@")).Contains(".") Then
                    Wdbg("I", "Mail format satisfied. Contains ""@"" and contains ""."" in the second part after the ""@"" symbol.")

                    'Prompt for subject
                    W(DoTranslation("Enter the subject:", currentLang) + " ", False, ColTypes.Input)
                    Subject = Console.ReadLine
                    Wdbg("I", "Subject: {0} ({1} chars)", Subject, Subject.Length)

                    'Prompt for body
                    W(DoTranslation("Enter your message below. Write ""EOF"" to confirm.", currentLang), True, ColTypes.Input)
                    Dim BodyLine As String = ""
                    While Not BodyLine.ToUpper = "EOF"
                        BodyLine = Console.ReadLine
                        If Not BodyLine.ToUpper = "EOF" Then
                            Wdbg("I", "Body line: {0} ({1} chars)", BodyLine, BodyLine.Length)
                            Body.AppendLine(BodyLine)
                            Wdbg("I", "Body length: {0} chars", Body.Length)
                        End If
                    End While

                    'Send the message
                    SyncLock SMTP_Client.SyncRoot
                        Try
                            W(DoTranslation("Sending message...", currentLang), True, ColTypes.Neutral)
                            Dim FinalMessage As New MimeMessage
                            FinalMessage.From.Add(MailboxAddress.Parse(Mail_Authentication.UserName))
                            Wdbg("I", "Added sender to FinalMessage.From.")
                            FinalMessage.To.Add(MailboxAddress.Parse(Receiver))
                            Wdbg("I", "Added address to FinalMessage.To.")
                            FinalMessage.Subject = Subject
                            Wdbg("I", "Added subject to FinalMessage.Subject.")
                            FinalMessage.Body = New TextPart(TextFormat.Plain) With {.Text = Body.ToString}
                            Wdbg("I", "Added body to FinalMessage.Body (plain text). Sending message...")
                            SMTP_Client.Send(FinalMessage)
                            W(DoTranslation("Message sent.", currentLang), True, ColTypes.Neutral)
                        Catch ex As Exception
                            W(DoTranslation("Error sending message: {0}", currentLang), True, ColTypes.Err, ex.Message)
                            Wdbg("E", "Failed to send message: {0}", ex.Message)
                            WStkTrc(ex)
                        End Try
                    End SyncLock
                Else
                    Wdbg("E", "Mail format unsatisfied.")
                    W(DoTranslation("Invalid e-mail address. Make sure you've written the address correctly and that it matches the format of the example shown:", currentLang) + " john.s@example.com", True, ColTypes.Err)
                End If
            ElseIf cmd = "rm" Then
                If FullArgsL.Count > 0 Then
                    RequiredArgsProvided = True
                    Wdbg("I", "Message number is numeric? {0}", FullArgsL(0).IsNumeric)
                    If FullArgsL(0).IsNumeric Then
                        MailRemoveMessage(FullArgsL(0))
                    Else
                        W(DoTranslation("Message number is not a numeric value.", currentLang), True, ColTypes.Err)
                    End If
                End If
            ElseIf cmd = "rmall" Then
                If FullArgsL.Count > 0 Then
                    RequiredArgsProvided = True
                    Wdbg("I", "All mail by {0} will be removed.", FullArgsL(0))
                    Dim DeletedMsgNumber As Integer = 1
                    Dim SteppedMsgNumber As Integer = 0
                    For Each MessageId As UniqueId In IMAP_Messages
                        SyncLock IMAP_Client.SyncRoot
                            Dim Msg As MimeMessage
                            If Not IMAP_CurrentDirectory = "" And Not IMAP_CurrentDirectory = "Inbox" Then
                                Dim Dir As MailFolder = OpenFolder(IMAP_CurrentDirectory)
                                Msg = Dir.GetMessage(MessageId)
                            Else
                                Msg = IMAP_Client.Inbox.GetMessage(MessageId)
                            End If
                            SteppedMsgNumber += 1

                            For Each address In Msg.From
                                If address.Name = FullArgsL(0) Then
                                    If Not IMAP_CurrentDirectory = "" And Not IMAP_CurrentDirectory = "Inbox" Then
                                        Dim Dir As MailFolder = OpenFolder(IMAP_CurrentDirectory)

                                        'Remove message
                                        Wdbg("I", "Opened {0}. Removing {1}...", IMAP_CurrentDirectory, FullArgsL(0))
                                        Dir.AddFlags(MessageId, MessageFlags.Deleted, True)
                                        Wdbg("I", "Removed.")
                                        Dir.Expunge()
                                        W(DoTranslation("Message {0} from {1} deleted from {2}. {3} messages remaining to parse.", currentLang), True, ColTypes.Neutral, DeletedMsgNumber, FullArgsL(0), IMAP_CurrentDirectory, IMAP_Messages.Count - SteppedMsgNumber)
                                    Else
                                        'Remove message
                                        Wdbg("I", "Removing {0}...", FullArgsL(0))
                                        IMAP_Client.Inbox.AddFlags(MessageId, MessageFlags.Deleted, True)
                                        Wdbg("I", "Removed.")
                                        IMAP_Client.Inbox.Expunge()
                                        W(DoTranslation("Message {0} from {1} deleted from inbox. {2} messages remaining to parse.", currentLang), True, ColTypes.Neutral, DeletedMsgNumber, FullArgsL(0), IMAP_Messages.Count - SteppedMsgNumber)
                                    End If
                                    DeletedMsgNumber += 1
                                End If
                            Next
                        End SyncLock
                    Next
                End If
            End If

            If Not RequiredArgsProvided Then
                W(DoTranslation("Required arguments are not passed to command {0}", currentLang), True, ColTypes.Err, cmd)
                Wdbg("E", "Passed arguments were not enough to run command {0}. Arguments passed: {1}", cmd, FullArgsL.Count)
                IMAPShowHelp(cmd)
            End If
        Catch ex As Exception
            EventManager.RaiseIMAPCommandError()
            W(DoTranslation("Error executing mail command: {0}", currentLang), True, ColTypes.Err, ex.Message)
            WStkTrc(ex)
        End Try
    End Sub

    Sub MailCancelCommand(sender As Object, e As ConsoleCancelEventArgs)
        If e.SpecialKey = ConsoleSpecialKey.ControlC Then
            DefConsoleOut = Console.Out
            Console.SetOut(StreamWriter.Null)
            e.Cancel = True
            MailStartCommandThread.Abort()
        End If
    End Sub

End Module
