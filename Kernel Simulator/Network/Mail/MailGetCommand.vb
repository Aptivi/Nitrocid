
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
Imports System.Threading
Imports MimeKit

Module MailGetCommand

    Public MailStartCommandThread As New Thread(AddressOf Mail_ExecuteCommand) With {.Name = "Mail Command Thread"}

    ''' <summary>
    ''' Parses and executes the specified mail command
    ''' </summary>
    ''' <param name="requestedCommand">A command. It may come with arguments</param>
    Sub Mail_ExecuteCommand(requestedCommand As String)
        'Variables
        Dim ArgumentInfo As New ProvidedCommandArgumentsInfo(requestedCommand, ShellCommandType.MailShell)
        Dim Command As String = ArgumentInfo.Command
        Dim eqargs() As String = ArgumentInfo.ArgumentsList
        Dim strArgs As String = ArgumentInfo.ArgumentsText
        Dim RequiredArgumentsProvided As Boolean = ArgumentInfo.RequiredArgumentsProvided

        '5. Check to see if a requested command is obsolete
        If MailCommands(Command).Obsolete Then
            Wdbg(DebugLevel.I, "The command requested {0} is obsolete", Command)
            W(DoTranslation("This command is obsolete and will be removed in a future release."), True, ColTypes.Neutral)
        End If

        '6. Execute a command
        Try
            Select Case words(0)
                Case "cd"
                    If RequiredArgumentsProvided Then
                        MailChangeDirectory(eqargs(0))
                    End If
                Case "list"
                    If eqargs?.Length > 0 Then
                        Wdbg(DebugLevel.I, "Page is numeric? {0}", eqargs(0).IsNumeric)
                        If eqargs(0).IsNumeric Then
                            W(MailListMessages(eqargs(0)), False, ColTypes.Neutral)
                        Else
                            W(DoTranslation("Page is not a numeric value."), True, ColTypes.Error)
                        End If
                    Else
                        W(MailListMessages(1), False, ColTypes.Neutral)
                    End If
                Case "lsdirs"
                    W(MailListDirectories, False, ColTypes.Neutral)
                Case "read"
                    If RequiredArgumentsProvided Then
                        Wdbg(DebugLevel.I, "Message number is numeric? {0}", eqargs(0).IsNumeric)
                        If eqargs(0).IsNumeric Then
                            MailPrintMessage(eqargs(0))
                        Else
                            W(DoTranslation("Message number is not a numeric value."), True, ColTypes.Error)
                        End If
                    End If
                Case "readenc"
                    If RequiredArgumentsProvided Then
                        Wdbg(DebugLevel.I, "Message number is numeric? {0}", eqargs(0).IsNumeric)
                        If eqargs(0).IsNumeric Then
                            MailPrintMessage(eqargs(0), True)
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
                    Wdbg(DebugLevel.I, "Recipient: {0}", Receiver)

                    'Check for mail format
                    If Receiver.Contains("@") And Receiver.Substring(Receiver.IndexOf("@")).Contains(".") Then
                        Wdbg(DebugLevel.I, "Mail format satisfied. Contains ""@"" and contains ""."" in the second part after the ""@"" symbol.")

                        'Prompt for subject
                        W(DoTranslation("Enter the subject:") + " ", False, ColTypes.Input)
                        Subject = Console.ReadLine
                        Wdbg(DebugLevel.I, "Subject: {0} ({1} chars)", Subject, Subject.Length)

                        'Prompt for body
                        W(DoTranslation("Enter your message below. Write ""EOF"" to confirm."), True, ColTypes.Input)
                        Dim BodyLine As String = ""
                        While Not BodyLine.ToUpper = "EOF"
                            BodyLine = Console.ReadLine
                            If Not BodyLine.ToUpper = "EOF" Then
                                Wdbg(DebugLevel.I, "Body line: {0} ({1} chars)", BodyLine, BodyLine.Length)
                                Body.TextBody += BodyLine + vbNewLine
                                Wdbg(DebugLevel.I, "Body length: {0} chars", Body.TextBody.Length)
                            End If
                        End While

                        W(DoTranslation("Enter file paths to attachments. Press ENTER on a blank path to confirm."), True, ColTypes.Neutral)
                        Dim PathLine As String = " "
                        While Not PathLine = ""
                            W("> ", False, ColTypes.Input)
                            PathLine = Console.ReadLine
                            If Not PathLine = "" Then
                                PathLine = NeutralizePath(PathLine)
                                Wdbg(DebugLevel.I, "Path line: {0} ({1} chars)", PathLine, PathLine.Length)
                                If File.Exists(PathLine) Then
                                    Body.Attachments.Add(PathLine)
                                End If
                            End If
                        End While

                        'Send the message
                        W(DoTranslation("Sending message..."), True, ColTypes.Neutral)
                        If requestedCommand = "sendenc" Then
                            If MailSendEncryptedMessage(Receiver, Subject, Body.ToMessageBody) Then
                                Wdbg(DebugLevel.I, "Message sent.")
                                W(DoTranslation("Message sent."), True, ColTypes.Neutral)
                            Else
                                Wdbg(DebugLevel.E, "See debug output to find what's wrong.")
                                W(DoTranslation("Error sending message."), True, ColTypes.Error)
                            End If
                        Else
                            If MailSendMessage(Receiver, Subject, Body.ToMessageBody) Then
                                Wdbg(DebugLevel.I, "Message sent.")
                                W(DoTranslation("Message sent."), True, ColTypes.Neutral)
                            Else
                                Wdbg(DebugLevel.E, "See debug output to find what's wrong.")
                                W(DoTranslation("Error sending message."), True, ColTypes.Error)
                            End If
                        End If
                    Else
                        Wdbg(DebugLevel.E, "Mail format unsatisfied. " + Receiver)
                        W(DoTranslation("Invalid e-mail address. Make sure you've written the address correctly and that it matches the format of the example shown:") + " john.s@example.com", True, ColTypes.Error)
                    End If
                Case "rm"
                    If RequiredArgumentsProvided Then
                        Wdbg(DebugLevel.I, "Message number is numeric? {0}", eqargs(0).IsNumeric)
                        If eqargs(0).IsNumeric Then
                            MailRemoveMessage(eqargs(0))
                        Else
                            W(DoTranslation("Message number is not a numeric value."), True, ColTypes.Error)
                        End If
                    End If
                Case "rmall"
                    If RequiredArgumentsProvided Then
                        If MailRemoveAllBySender(eqargs(0)) Then
                            W(DoTranslation("All mail made by {0} are removed successfully."), True, ColTypes.Neutral, eqargs(0))
                        Else
                            W(DoTranslation("Failed to remove all mail made by {0}."), True, ColTypes.Neutral, eqargs(0))
                        End If
                    End If
                Case "mv"
                    If RequiredArgumentsProvided Then
                        Wdbg(DebugLevel.I, "Message number is numeric? {0}", eqargs(0).IsNumeric)
                        If eqargs(0).IsNumeric Then
                            MailMoveMessage(eqargs(0), eqargs(1))
                        Else
                            W(DoTranslation("Message number is not a numeric value."), True, ColTypes.Error)
                        End If
                    End If
                Case "mvall"
                    If RequiredArgumentsProvided Then
                        If MailMoveAllBySender(eqargs(0), eqargs(1)) Then
                            W(DoTranslation("All mail made by {0} are moved successfully."), True, ColTypes.Neutral, eqargs(0))
                        Else
                            W(DoTranslation("Failed to move all mail made by {0}."), True, ColTypes.Neutral, eqargs(0))
                        End If
                    End If
                Case "mkdir"
                    If RequiredArgumentsProvided Then
                        CreateMailDirectory(eqargs(0))
                    End If
                Case "rmdir"
                    If RequiredArgumentsProvided Then
                        DeleteMailDirectory(eqargs(0))
                    End If
                Case "ren"
                    If RequiredArgumentsProvided Then
                        RenameMailDirectory(eqargs(0), eqargs(1))
                    End If
                Case "help"
                    If eqargs?.Length > 0 Then
                        ShowHelp(eqargs(0), ShellCommandType.MailShell)
                    Else
                        ShowHelp(ShellCommandType.MailShell)
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

            If MailCommands(words(0)).ArgumentsRequired And Not RequiredArgumentsProvided Then
                W(DoTranslation("Required arguments are not passed to command {0}"), True, ColTypes.Error, words(0))
                Wdbg(DebugLevel.E, "Passed arguments were not enough to run command {0}. Arguments passed: {1}", words(0), eqargs?.Length)
                ShowHelp(words(0), ShellCommandType.MailShell)
            End If
        Catch taex As ThreadAbortException
            Exit Sub
        Catch ex As Exception
            EventManager.RaiseIMAPCommandError(requestedCommand, ex)
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
