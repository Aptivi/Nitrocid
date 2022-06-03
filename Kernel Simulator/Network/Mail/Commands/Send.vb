
'    Kernel Simulator  Copyright (C) 2018-2022  EoflaOE
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
Imports KS.Network.Mail.Transfer

Namespace Network.Mail.Commands
    Class Mail_SendCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim Receiver, Subject As String
            Dim Body As New BodyBuilder

            'Prompt for receiver e-mail address
            Write(DoTranslation("Enter recipient mail address:") + " ", False, ColTypes.Input)
            Receiver = ReadLine()
            Wdbg(DebugLevel.I, "Recipient: {0}", Receiver)

            'Check for mail format
            If Receiver.Contains("@") And Receiver.Substring(Receiver.IndexOf("@")).Contains(".") Then
                Wdbg(DebugLevel.I, "Mail format satisfied. Contains ""@"" and contains ""."" in the second part after the ""@"" symbol.")

                'Prompt for subject
                Write(DoTranslation("Enter the subject:") + " ", False, ColTypes.Input)
                Subject = ReadLine()
                Wdbg(DebugLevel.I, "Subject: {0} ({1} chars)", Subject, Subject.Length)

                'Prompt for body
                Write(DoTranslation("Enter your message below. Write ""EOF"" to confirm."), True, ColTypes.Input)
                Dim BodyLine As String = ""
                While Not BodyLine.ToUpper = "EOF"
                    BodyLine = ReadLine()
                    If Not BodyLine.ToUpper = "EOF" Then
                        Wdbg(DebugLevel.I, "Body line: {0} ({1} chars)", BodyLine, BodyLine.Length)
                        Body.TextBody += BodyLine + NewLine
                        Wdbg(DebugLevel.I, "Body length: {0} chars", Body.TextBody.Length)
                    End If
                End While

                Write(DoTranslation("Enter file paths to attachments. Press ENTER on a blank path to confirm."), True, ColTypes.Neutral)
                Dim PathLine As String = " "
                While Not PathLine = ""
                    Write("> ", False, ColTypes.Input)
                    PathLine = ReadLine()
                    If Not PathLine = "" Then
                        PathLine = NeutralizePath(PathLine)
                        Wdbg(DebugLevel.I, "Path line: {0} ({1} chars)", PathLine, PathLine.Length)
                        If FileExists(PathLine) Then
                            Body.Attachments.Add(PathLine)
                        End If
                    End If
                End While

                'Send the message
                Write(DoTranslation("Sending message..."), True, ColTypes.Progress)
                If MailSendMessage(Receiver, Subject, Body.ToMessageBody) Then
                    Wdbg(DebugLevel.I, "Message sent.")
                    Write(DoTranslation("Message sent."), True, ColTypes.Success)
                Else
                    Wdbg(DebugLevel.E, "See debug output to find what's wrong.")
                    Write(DoTranslation("Error sending message."), True, ColTypes.Error)
                End If
            ElseIf ReadLineReboot.ReadLine.ReadRanToCompletion Then
                Wdbg(DebugLevel.E, "Mail format unsatisfied." + Receiver)
                Write(DoTranslation("Invalid e-mail address. Make sure you've written the address correctly and that it matches the format of the example shown:") + " john.s@example.com", True, ColTypes.Error)
            End If
        End Sub

    End Class
End Namespace