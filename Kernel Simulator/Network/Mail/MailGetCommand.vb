
'    Kernel Simulator  Copyright (C) 2018-2021  EoflaOE
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
Imports Microsoft.VisualBasic.FileIO
Imports MimeKit

Module MailGetCommand

    Public MailStartCommandThread As New Thread(AddressOf Mail_ExecuteCommand) With {.Name = "Mail Command Thread"}

    ''' <summary>
    ''' Parses and executes the specified command with arguments
    ''' </summary>
    ''' <param name="Parameters">Two strings. The first one is the command string, and the other is the arguments string.</param>
    Sub Mail_ExecuteCommand(ByVal Parameters As String()) 'This is converted to String() to ensure compatibility with Threading.Thread.
        Dim cmd As String = Parameters(0)
        Dim args As String = Parameters(1)
        Dim RequiredArgsProvided As Boolean = True
        Dim FullArgsLQ() As String
        Dim TStream As New MemoryStream(Encoding.Default.GetBytes(args))
        Dim Parser As New TextFieldParser(TStream) With {
            .Delimiters = {" "},
            .HasFieldsEnclosedInQuotes = True,
            .TrimWhiteSpace = False
        }
        FullArgsLQ = Parser.ReadFields
        If FullArgsLQ IsNot Nothing Then
            For i As Integer = 0 To FullArgsLQ.Length - 1
                FullArgsLQ(i).Replace("""", "")
            Next
            RequiredArgsProvided = FullArgsLQ?.Length >= MailCommands(cmd).MinimumArguments
        ElseIf MailCommands(cmd).ArgumentsRequired And FullArgsLQ Is Nothing Then
            RequiredArgsProvided = False
        End If
        Wdbg("I", "Arguments with enclosed quotes count: {0}", FullArgsLQ?.Count)

        Try
            Select Case cmd
                Case "cd"
                    If RequiredArgsProvided Then
                        MailChangeDirectory(FullArgsLQ(0))
                    End If
                Case "list"
                    If FullArgsLQ?.Count > 0 Then
                        Wdbg("I", "Page is numeric? {0}", FullArgsLQ(0).IsNumeric)
                        If FullArgsLQ(0).IsNumeric Then
                            W(MailListMessages(FullArgsLQ(0)), False, ColTypes.Neutral)
                        Else
                            W(DoTranslation("Page is not a numeric value."), True, ColTypes.Error)
                        End If
                    Else
                        W(MailListMessages(1), False, ColTypes.Neutral)
                    End If
                Case "lsdirs"
                    W(MailListDirectories, False, ColTypes.Neutral)
                Case "read"
                    If RequiredArgsProvided Then
                        Wdbg("I", "Message number is numeric? {0}", FullArgsLQ(0).IsNumeric)
                        If FullArgsLQ(0).IsNumeric Then
                            MailPrintMessage(FullArgsLQ(0))
                        Else
                            W(DoTranslation("Message number is not a numeric value."), True, ColTypes.Error)
                        End If
                    End If
                Case "readenc"
                    If RequiredArgsProvided Then
                        Wdbg("I", "Message number is numeric? {0}", FullArgsLQ(0).IsNumeric)
                        If FullArgsLQ(0).IsNumeric Then
                            MailPrintMessage(FullArgsLQ(0), True)
                        Else
                            W(DoTranslation("Message number is not a numeric value."), True, ColTypes.Error)
                        End If
                    End If
                Case "send", "sendenc"
                    Dim Receiver, Subject As String
                    Dim Body As New BodyBuilder

                    'Prompt for receiver e-mail address
                    W(DoTranslation("Enter recipient mail address:") + " ", False, ColTypes.Input)
                    Receiver = Console.ReadLine
                    Wdbg("I", "Recipient: {0}", Receiver)

                    'Check for mail format
                    If Receiver.Contains("@") And Receiver.Substring(Receiver.IndexOf("@")).Contains(".") Then
                        Wdbg("I", "Mail format satisfied. Contains ""@"" and contains ""."" in the second part after the ""@"" symbol.")

                        'Prompt for subject
                        W(DoTranslation("Enter the subject:") + " ", False, ColTypes.Input)
                        Subject = Console.ReadLine
                        Wdbg("I", "Subject: {0} ({1} chars)", Subject, Subject.Length)

                        'Prompt for body
                        W(DoTranslation("Enter your message below. Write ""EOF"" to confirm."), True, ColTypes.Input)
                        Dim BodyLine As String = ""
                        While Not BodyLine.ToUpper = "EOF"
                            BodyLine = Console.ReadLine
                            If Not BodyLine.ToUpper = "EOF" Then
                                Wdbg("I", "Body line: {0} ({1} chars)", BodyLine, BodyLine.Length)
                                Body.TextBody += BodyLine + vbNewLine
                                Wdbg("I", "Body length: {0} chars", Body.TextBody.Length)
                            End If
                        End While

                        W(DoTranslation("Enter file paths to attachments. Press ENTER on a blank path to confirm."), True, ColTypes.Neutral)
                        Dim PathLine As String = " "
                        While Not PathLine = ""
                            W("> ", False, ColTypes.Input)
                            PathLine = Console.ReadLine
                            If Not PathLine = "" Then
                                PathLine = NeutralizePath(PathLine)
                                Wdbg("I", "Path line: {0} ({1} chars)", PathLine, PathLine.Length)
                                If File.Exists(PathLine) Then
                                    Body.Attachments.Add(PathLine)
                                End If
                            End If
                        End While

                        'Send the message
                        W(DoTranslation("Sending message..."), True, ColTypes.Neutral)
                        If cmd = "sendenc" Then
                            If MailSendEncryptedMessage(Receiver, Subject, Body.ToMessageBody) Then
                                Wdbg("I", "Message sent.")
                                W(DoTranslation("Message sent."), True, ColTypes.Neutral)
                            Else
                                Wdbg("E", "See debug output to find what's wrong.")
                                W(DoTranslation("Error sending message."), True, ColTypes.Error)
                            End If
                        Else
                            If MailSendMessage(Receiver, Subject, Body.ToMessageBody) Then
                                Wdbg("I", "Message sent.")
                                W(DoTranslation("Message sent."), True, ColTypes.Neutral)
                            Else
                                Wdbg("E", "See debug output to find what's wrong.")
                                W(DoTranslation("Error sending message."), True, ColTypes.Error)
                            End If
                        End If
                    Else
                        Wdbg("E", "Mail format unsatisfied. " + Receiver)
                        W(DoTranslation("Invalid e-mail address. Make sure you've written the address correctly and that it matches the format of the example shown:") + " john.s@example.com", True, ColTypes.Error)
                    End If
                Case "rm"
                    If RequiredArgsProvided Then
                        Wdbg("I", "Message number is numeric? {0}", FullArgsLQ(0).IsNumeric)
                        If FullArgsLQ(0).IsNumeric Then
                            MailRemoveMessage(FullArgsLQ(0))
                        Else
                            W(DoTranslation("Message number is not a numeric value."), True, ColTypes.Error)
                        End If
                    End If
                Case "rmall"
                    If RequiredArgsProvided Then
                        If MailRemoveAllBySender(FullArgsLQ(0)) Then
                            W(DoTranslation("All mail made by {0} are removed successfully."), True, ColTypes.Neutral, FullArgsLQ(0))
                        Else
                            W(DoTranslation("Failed to remove all mail made by {0}."), True, ColTypes.Neutral, FullArgsLQ(0))
                        End If
                    End If
                Case "mv"
                    If RequiredArgsProvided Then
                        Wdbg("I", "Message number is numeric? {0}", FullArgsLQ(0).IsNumeric)
                        If FullArgsLQ(0).IsNumeric Then
                            MailMoveMessage(FullArgsLQ(0), FullArgsLQ(1))
                        Else
                            W(DoTranslation("Message number is not a numeric value."), True, ColTypes.Error)
                        End If
                    End If
                Case "mvall"
                    If RequiredArgsProvided Then
                        If MailMoveAllBySender(FullArgsLQ(0), FullArgsLQ(1)) Then
                            W(DoTranslation("All mail made by {0} are moved successfully."), True, ColTypes.Neutral, FullArgsLQ(0))
                        Else
                            W(DoTranslation("Failed to move all mail made by {0}."), True, ColTypes.Neutral, FullArgsLQ(0))
                        End If
                    End If
                Case "mkdir"
                    If RequiredArgsProvided Then
                        CreateMailDirectory(FullArgsLQ(0))
                    End If
                Case "rmdir"
                    If RequiredArgsProvided Then
                        DeleteMailDirectory(FullArgsLQ(0))
                    End If
                Case "ren"
                    If RequiredArgsProvided Then
                        RenameMailDirectory(FullArgsLQ(0), FullArgsLQ(1))
                    End If
                Case "help"
                    If FullArgsLQ?.Length > 0 Then
                        IMAPShowHelp(FullArgsLQ(0))
                    Else
                        IMAPShowHelp()
                    End If
                Case "exit"
                    ExitRequested = True
                    W(DoTranslation("Do you want to keep connected?") + " <y/n> ", False, ColTypes.Input)
                    Dim Answer As Char = Console.ReadKey.KeyChar
                    Console.WriteLine()
                    If Answer = "y" Then
                        KeepAlive = True
                    ElseIf Answer = "n" Then
                        KeepAlive = False
                    Else
                        W(DoTranslation("Invalid choice. Assuming no..."), True, ColTypes.Input)
                    End If
            End Select

            If MailCommands(cmd).ArgumentsRequired And Not RequiredArgsProvided Then
                W(DoTranslation("Required arguments are not passed to command {0}"), True, ColTypes.Error, cmd)
                Wdbg("E", "Passed arguments were not enough to run command {0}. Arguments passed: {1}", cmd, FullArgsLQ?.Count)
                IMAPShowHelp(cmd)
            End If
        Catch ex As Exception
            EventManager.RaiseIMAPCommandError(cmd + " " + args, ex)
            W(DoTranslation("Error executing mail command: {0}"), True, ColTypes.Error, ex.Message)
            WStkTrc(ex)
        End Try
    End Sub

    Sub MailCancelCommand(sender As Object, e As ConsoleCancelEventArgs)
        If e.SpecialKey = ConsoleSpecialKey.ControlC Then
            Console.WriteLine()
            DefConsoleOut = Console.Out
            Console.SetOut(StreamWriter.Null)
            e.Cancel = True
            MailStartCommandThread.Abort()
        End If
    End Sub

End Module
