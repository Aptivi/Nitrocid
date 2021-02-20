
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

Public Module TextEditGetCommand

    'Variables
    Public TextEdit_CommandThread As New Thread(AddressOf TextEdit_ParseCommand)

    Sub TextEdit_ParseCommand(ByVal CommandText As String)
        Try
            'Indicator if command finished
            Dim CommandDone As Boolean

            'Separate between command and arguments specified
            Dim Command As String = CommandText.Split(" ")(0)
            Dim Arguments() As String
            Dim TStream As New MemoryStream(Encoding.Default.GetBytes(CommandText))
            Dim Parser As New TextFieldParser(TStream) With {
                .Delimiters = {" "},
                .HasFieldsEnclosedInQuotes = True
            }
            Arguments = Parser.ReadFields
            If Arguments IsNot Nothing Then
                For i As Integer = 0 To Arguments.Length - 1
                    Arguments(i).Replace("""", "")
                Next
            End If

            'Remove first entry from array
            Dim ArgumentsList As List(Of String) = Arguments.ToList
            ArgumentsList.Remove(Command)
            Arguments = ArgumentsList.ToArray

            'Try to parse command
            If Command = "help" Then
                If Arguments?.Length > 0 Then
                    Wdbg("I", "Requested help for {0}", Arguments(0))
                    TextEdit_GetHelp(Arguments(0))
                Else
                    Wdbg("I", "Requested help for all commands")
                    TextEdit_GetHelp()
                End If
                CommandDone = True
            ElseIf Command = "exit" Then
                CommandDone = True
                TextEdit_SaveTextFile(True)
                TextEdit_Exiting = True
            ElseIf Command = "exitnosave" Then
                CommandDone = True
                TextEdit_Exiting = True
            ElseIf Command = "save" Then
                CommandDone = True
                TextEdit_SaveTextFile(False)
            ElseIf Command = "print" Then
                Dim LineNumber As Integer = 1
                If Arguments?.Length > 0 Then
                    Wdbg("I", "Line number provided: {0}", Arguments(0))
                    Wdbg("I", "Is it numeric? {0}", Arguments(0).IsNumeric)
                    If Arguments(0).IsNumeric Then
                        LineNumber = Arguments(0)
                        Wdbg("I", "File lines: {0}", TextEdit_FileLines.Count)
                        If CInt(Arguments(0)) <= TextEdit_FileLines.Count Then
                            Dim Line As String = TextEdit_FileLines(LineNumber - 1)
                            Wdbg("I", "Line number: {0} ({1})", LineNumber, Line)
                            W("- {0}: ", False, ColTypes.HelpCmd, LineNumber)
                            W(Line, True, ColTypes.HelpDef)
                        Else
                            W(DoTranslation("The specified line number may not be larger than the last file line number.", currentLang), True, ColTypes.Err)
                        End If
                    Else
                        W(DoTranslation("Specified line number {0} is not a valid number.", currentLang), True, ColTypes.Err, Arguments(0))
                        Wdbg("E", "{0} is not a numeric value.", Arguments(0))
                    End If
                Else
                    For Each Line As String In TextEdit_FileLines
                        Wdbg("I", "Line number: {0} ({1})", LineNumber, Line)
                        W("- {0}: ", False, ColTypes.HelpCmd, LineNumber)
                        W(Line, True, ColTypes.HelpDef)
                        LineNumber += 1
                    Next
                End If
                CommandDone = True
            ElseIf Command = "addline" Then
                If Arguments?.Length > 0 Then
                    Dim NewLineContent As String = Arguments.Join(" ")
                    Wdbg("I", "Prototype new line content: {0}", NewLineContent)
                    If NewLineContent.StartsWith("""") And NewLineContent.EndsWith("""") Then
                        NewLineContent = NewLineContent.Remove(0, 1)
                        Wdbg("I", "Stage 1 new line content: {0}", NewLineContent)
                        NewLineContent = NewLineContent.Remove(NewLineContent.Length - 1, 1)
                        Wdbg("I", "Stage 2 new line content: {0}", NewLineContent)
                    End If
                    Wdbg("I", "Final new line content: {0}", NewLineContent)
                    TextEdit_FileLines.Add(NewLineContent)
                    CommandDone = True
                End If
            ElseIf Command = "delline" Then
                If Arguments?.Length > 0 Then
                    CommandDone = True
                    Wdbg("I", "Is argument numeric: {0}", IsNumeric(Arguments(0)))
                    If IsNumeric(Arguments(0)) Then
                        Dim LineIndex As Integer = Arguments(0) - 1
                        Wdbg("I", "Got line index: {0}", LineIndex)
                        Wdbg("I", "Old file lines: {0}", TextEdit_FileLines.Count)
                        If CInt(Arguments(0)) <= TextEdit_FileLines.Count Then
                            TextEdit_FileLines.RemoveAt(LineIndex)
                            Wdbg("I", "New file lines: {0}", TextEdit_FileLines.Count)
                            W(DoTranslation("Removed line.", currentLang), True, ColTypes.Neutral)
                        Else
                            W(DoTranslation("The specified line number may not be larger than the last file line number.", currentLang), True, ColTypes.Err)
                        End If
                    Else
                        W(DoTranslation("Specified line number {0} is not a valid number.", currentLang), True, ColTypes.Err, Arguments(0))
                        Wdbg("E", "{0} is not a numeric value.", Arguments(0))
                    End If
                End If
            ElseIf Command = "replace" Then
                If Arguments?.Length > 1 Then
                    CommandDone = True
                    Wdbg("I", "Source: {0}, Target: {1}", Arguments(0), Arguments(1))
                    For LineIndex As Integer = 0 To TextEdit_FileLines.Count - 1
                        Wdbg("I", "Replacing ""{0}"" with ""{1}"" in line {2}", Arguments(0), Arguments(1), LineIndex + 1)
                        TextEdit_FileLines(LineIndex) = TextEdit_FileLines(LineIndex).Replace(Arguments(0), Arguments(1))
                    Next
                End If
            ElseIf Command = "replaceinline" Then
                If Arguments?.Length > 2 Then
                    CommandDone = True
                    Wdbg("I", "Source: {0}, Target: {1}, Line Number: {2}", Arguments(0), Arguments(1), Arguments(2))
                    Wdbg("I", "File lines: {0}", TextEdit_FileLines.Count)
                    If Arguments(2).IsNumeric Then
                        Dim LineIndex As Long = Arguments(2) - 1
                        If CInt(Arguments(2)) <= TextEdit_FileLines.Count Then
                            Wdbg("I", "Replacing ""{0}"" with ""{1}"" in line {2}", Arguments(0), Arguments(1), LineIndex + 1)
                            TextEdit_FileLines(LineIndex) = TextEdit_FileLines(LineIndex).Replace(Arguments(0), Arguments(1))
                        Else
                            W(DoTranslation("The specified line number may not be larger than the last file line number.", currentLang), True, ColTypes.Err)
                        End If
                    Else
                        W(DoTranslation("Specified line number {0} is not a valid number.", currentLang), True, ColTypes.Err, Arguments(2))
                        Wdbg("E", "{0} is not a numeric value.", Arguments(2))
                    End If
                End If
            ElseIf Command = "delword" Then
                If Arguments?.Length > 1 Then
                    CommandDone = True
                    Wdbg("I", "Word/Phrase: {0}, Line: {1} ({2})", Arguments(0), Arguments(1), IsNumeric(Arguments(1)))
                    If IsNumeric(Arguments(1)) Then
                        Dim LineIndex As Integer = Arguments(1) - 1
                        Wdbg("I", "Got line index: {0}", LineIndex)
                        Wdbg("I", "File lines: {0}", TextEdit_FileLines.Count)
                        If CInt(Arguments(1)) <= TextEdit_FileLines.Count Then
                            TextEdit_FileLines(LineIndex) = TextEdit_FileLines(LineIndex).Replace(Arguments(0), "")
                            Wdbg("I", "Removed {0}. Result: {1}", LineIndex, TextEdit_FileLines.Count)
                        Else
                            W(DoTranslation("The specified line number may not be larger than the last file line number.", currentLang), True, ColTypes.Err)
                        End If
                    Else
                        W(DoTranslation("Specified line number {0} is not a valid number.", currentLang), True, ColTypes.Err, Arguments(1))
                        Wdbg("E", "{0} is not a numeric value.", Arguments(1))
                    End If
                End If
            ElseIf Command = "delcharnum" Then
                If Arguments?.Length > 1 Then
                    CommandDone = True
                    Wdbg("I", "Char number: {0} ({1}), Line: {2} ({3})", Arguments(0), IsNumeric(Arguments(0)), Arguments(1), IsNumeric(Arguments(1)))
                    If IsNumeric(Arguments(1)) Then
                        Dim LineIndex As Integer = Arguments(1) - 1
                        Dim CharIndex As Integer = Arguments(0) - 1
                        Wdbg("I", "Got line index: {0}", LineIndex)
                        Wdbg("I", "Got char index: {0}", CharIndex)
                        Wdbg("I", "File lines: {0}", TextEdit_FileLines.Count)
                        If CInt(Arguments(1)) <= TextEdit_FileLines.Count Then
                            TextEdit_FileLines(LineIndex) = TextEdit_FileLines(LineIndex).Remove(CharIndex, 1)
                            Wdbg("I", "Removed {0}. Result: {1}", LineIndex, TextEdit_FileLines(LineIndex))
                        Else
                            W(DoTranslation("The specified line number may not be larger than the last file line number.", currentLang), True, ColTypes.Err)
                        End If
                    Else
                        W(DoTranslation("Specified line number {0} is not a valid number.", currentLang), True, ColTypes.Err, Arguments(1))
                        Wdbg("E", "{0} is not a numeric value.", Arguments(1))
                    End If
                End If
            ElseIf Command = "querychar" Then
                If Arguments?.Length > 1 Then
                    CommandDone = True
                    Wdbg("I", "Char: {0}, Line: {1} ({2})", Arguments(0), Arguments(1), IsNumeric(Arguments(1)))
                    If IsNumeric(Arguments(1)) Then
                        Dim LineIndex As Integer = Arguments(1) - 1
                        Wdbg("I", "Got line index: {0}", LineIndex)
                        Wdbg("I", "File lines: {0}", TextEdit_FileLines.Count)
                        If CInt(Arguments(1)) <= TextEdit_FileLines.Count Then
                            For CharIndex As Integer = 0 To TextEdit_FileLines(Arguments(1)).Length - 1
                                If TextEdit_FileLines(Arguments(1))(CharIndex) = Arguments(0) Then
                                    W("- {0}: ", False, ColTypes.HelpCmd, CharIndex)
                                    W("{0} ({1})", True, ColTypes.HelpDef, Arguments(0), TextEdit_FileLines(Arguments(1)))
                                End If
                            Next
                        Else
                            W(DoTranslation("The specified line number may not be larger than the last file line number.", currentLang), True, ColTypes.Err)
                        End If
                    ElseIf Arguments(1).ToLower = "all" Then
                        Wdbg("I", "File lines: {0}", TextEdit_FileLines.Count)
                        For LineIndex As Integer = 0 To TextEdit_FileLines.Count - 1
                            For CharIndex As Integer = 0 To TextEdit_FileLines(LineIndex).Length - 1
                                If TextEdit_FileLines(LineIndex)(CharIndex) = Arguments(0) Then
                                    W("- {0}:{1}: ", False, ColTypes.HelpCmd, LineIndex, CharIndex)
                                    W("{0} ({1})", True, ColTypes.HelpDef, Arguments(0), TextEdit_FileLines(LineIndex))
                                End If
                            Next
                        Next
                    End If
                End If
            ElseIf Command = "clear" Then
                TextEdit_FileLines.Clear()
            End If

            'See if the command is done (passed all required arguments)
            If Not CommandDone Then
                W(DoTranslation("Required arguments are not passed to command {0}", currentLang), True, ColTypes.Err, Command)
                Wdbg("E", "Passed arguments were not enough to run command {0}. Arguments passed: {1}", Command, Arguments.Length)
                TextEdit_GetHelp(Command)
            End If
        Catch ex As Exception
            W(DoTranslation("Error trying to run command: {0}", currentLang), True, ColTypes.Err, ex.Message)
            Wdbg("E", "Error running command {0}: {1}", CommandText.Split(" ")(0), ex.Message)
            WStkTrc(ex)
            EventManager.RaiseTextCommandError(CommandText, ex)
        End Try
    End Sub

End Module
