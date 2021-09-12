
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
Imports MimeKit

Class Mail_SendCommand
    Inherits CommandExecutor
    Implements ICommand

    Public Overrides Sub Execute(StringArgs As String, ListArgs() As String) Implements ICommand.Execute
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
            If MailSendMessage(Receiver, Subject, Body.ToMessageBody) Then
                Wdbg(DebugLevel.I, "Message sent.")
                W(DoTranslation("Message sent."), True, ColTypes.Success)
            Else
                Wdbg(DebugLevel.E, "See debug output to find what's wrong.")
                W(DoTranslation("Error sending message."), True, ColTypes.Error)
            End If
        Else
            Wdbg(DebugLevel.E, "Mail format unsatisfied. " + Receiver)
            W(DoTranslation("Invalid e-mail address. Make sure you've written the address correctly and that it matches the format of the example shown:") + " john.s@example.com", True, ColTypes.Error)
        End If
    End Sub

End Class