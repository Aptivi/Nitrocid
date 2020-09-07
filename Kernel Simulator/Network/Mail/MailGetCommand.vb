
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
Imports Microsoft.VisualBasic.FileIO

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
        Dim FullArgsLQ() As String
        Dim TStream As New MemoryStream(Encoding.Default.GetBytes(args))
        Dim Parser As New TextFieldParser(TStream) With {
            .Delimiters = {" "},
            .HasFieldsEnclosedInQuotes = True
        }
        FullArgsLQ = Parser.ReadFields
        If Not FullArgsLQ Is Nothing Then
            For i As Integer = 0 To FullArgsLQ.Length - 1
                FullArgsLQ(i).Replace("""", "")
            Next
        End If
        Dim RequiredArgsProvided As Boolean
        Wdbg("I", "Arguments count: {0}", FullArgsL.Count)
        Wdbg("I", "Arguments with enclosed quotes count: {0}", FullArgsLQ?.Count)

        Try
            If cmd = "help" Then
                RequiredArgsProvided = True
                IMAPShowHelp()
            ElseIf cmd = "cd" Then
                If FullArgsLQ?.Count > 0 Then
                    RequiredArgsProvided = True
                    MailChangeDirectory(FullArgsLQ(0))
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
                        MailPrintMessage(FullArgsL(0))
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
                    W(DoTranslation("Sending message...", currentLang), True, ColTypes.Neutral)
                    If MailSendMessage(Receiver, Subject, BodyLine) Then
                        Wdbg("I", "Message sent.")
                        W(DoTranslation("Message sent.", currentLang), True, ColTypes.Neutral)
                    Else
                        Wdbg("E", "See debug output to find what's wrong.")
                        W(DoTranslation("Error sending message.", currentLang), True, ColTypes.Err)
                    End If
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
                    If MailRemoveAllBySender(FullArgsLQ(0)) Then
                        W(DoTranslation("All mail made by {0} are removed successfully.", currentLang), True, ColTypes.Neutral, FullArgsLQ(0))
                    Else
                        W(DoTranslation("Failed to remove all mail made by {0}.", currentLang), True, ColTypes.Neutral, FullArgsLQ(0))
                    End If
                End If
            ElseIf cmd = "mv" Then
                If FullArgsL.Count > 0 Then
                    RequiredArgsProvided = True
                    Wdbg("I", "Message number is numeric? {0}", FullArgsL(0).IsNumeric)
                    If FullArgsL(0).IsNumeric Then
                        MailMoveMessage(FullArgsL(0), FullArgsLQ(1))
                    Else
                        W(DoTranslation("Message number is not a numeric value.", currentLang), True, ColTypes.Err)
                    End If
                End If
            ElseIf cmd = "mvall" Then
                If FullArgsL.Count > 0 Then
                    RequiredArgsProvided = True
                    If MailMoveAllBySender(FullArgsLQ(0), FullArgsLQ(1)) Then
                        W(DoTranslation("All mail made by {0} are moved successfully.", currentLang), True, ColTypes.Neutral, FullArgsLQ(0))
                    Else
                        W(DoTranslation("Failed to move all mail made by {0}.", currentLang), True, ColTypes.Neutral, FullArgsLQ(0))
                    End If
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
