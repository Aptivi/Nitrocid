
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

        Try
            If cmd = "help" Then
                IMAPShowHelp()
            ElseIf cmd = "exit" Then
                ExitRequested = True
            ElseIf cmd = "list" Then
                If FullArgsL(0).IsNumeric Then
                    Dim Page As Integer
                    If FullArgsL(0) = "" Then Page = 1 Else Page = FullArgsL(0)
                    If Page <= 0 Then
                        W(DoTranslation("Page may not be negative or zero.", currentLang), True, ColTypes.Err)
                        Exit Sub
                    End If
                    Dim MsgsLimitForPg As Integer = 10
                    Dim FirstIndex As Integer = (MsgsLimitForPg * Page) - 10
                    Dim LastIndex As Integer = (MsgsLimitForPg * Page) - 1
                    Dim MaxMessagesIndex As Integer = IMAP_Messages.Count - 1
                    For i As Integer = FirstIndex To LastIndex
                        If Not i > MaxMessagesIndex Then
                            Dim Msg As MimeMessage = IMAP_Client.Inbox.GetMessage(IMAP_Messages(i))
                            W("- [{0}] {1}: ", False, ColTypes.HelpCmd, i + 1, Msg.From) : W("{0}", True, ColTypes.HelpDef, Msg.Subject)
                        End If
                    Next
                Else
                    W(DoTranslation("Page is not a numeric value.", currentLang), True, ColTypes.Err)
                End If
            ElseIf cmd = "read" Then
                If FullArgsL(0).IsNumeric Then
                    Dim Message As Integer = FullArgsL(0) - 1
                    Dim MaxMessagesIndex As Integer = IMAP_Messages.Count - 1
                    If Message < 0 Then
                        W(DoTranslation("Message number may not be negative or zero.", currentLang), True, ColTypes.Err)
                        Exit Sub
                    ElseIf Message > MaxMessagesIndex Then
                        W(DoTranslation("Message specified is not found.", currentLang), True, ColTypes.Err)
                        Exit Sub
                    End If

                    'Get message
                    Dim Msg As MimeMessage = IMAP_Client.Inbox.GetMessage(IMAP_Messages(Message))

                    'Prepare view
                    Console.WriteLine()

                    'Print all the addresses that sent the mail
                    For Each Address As InternetAddress In Msg.From
                        W(DoTranslation("- From {0}", currentLang), True, ColTypes.HelpCmd, Address.ToString)
                    Next

                    'Print all the addresses that received the mail
                    For Each Address As InternetAddress In Msg.To
                        W(DoTranslation("- To {0}", currentLang), True, ColTypes.HelpCmd, Address.ToString)
                    Next

                    'Print the date and time when the user received the mail
                    W(DoTranslation("- Sent at {0} in {1}", currentLang), True, ColTypes.HelpCmd, RenderTime(Msg.Date.DateTime), RenderDate(Msg.Date.DateTime))

                    'Prepare subject
                    Console.WriteLine()
                    W($"- {Msg.Subject}", False, ColTypes.HelpCmd)

                    'Write a sign after the subject if attachments are found
                    If Msg.Attachments.Count > 0 Then
                        W(" - [*]", True, ColTypes.HelpCmd)
                    Else
                        Console.WriteLine()
                    End If

                    'Prepare body
                    Console.WriteLine()
                    W(Msg.GetTextBody(Text.TextFormat.Plain), True, ColTypes.HelpDef)
                    Console.WriteLine()

                    'Populate attachments
                    If Msg.Attachments.Count > 0 Then
                        W(DoTranslation("Attachments:", currentLang), True, ColTypes.Neutral)
                        For Each Attachment As MimeEntity In Msg.Attachments
                            If TypeOf Attachment Is MessagePart Then
                                W($"- {Attachment.ContentDisposition?.FileName}", True, ColTypes.Neutral)
                            Else
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
