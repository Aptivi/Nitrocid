
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
    Public TextEdit_CommandThread As New Thread(AddressOf TextEdit_ParseCommand) With {.Name = "Text Edit Command Thread"}

    Sub TextEdit_ParseCommand(ByVal CommandText As String)
        Try
            'Indicator if required arguments are provided
            Dim RequiredArgumentsProvided As Boolean = True

            'Get the index of the first space
            Dim index As Integer = CommandText.IndexOf(" ")
            If index = -1 Then index = CommandText.Length
            Wdbg("I", "Index: {0}", index)

            'Get the String Of arguments
            Dim strArgs As String = CommandText.Substring(index)
            Wdbg("I", "Prototype strArgs: {0}", strArgs)
            If Not index = CommandText.Length Then strArgs = strArgs.Substring(1)
            Wdbg("I", "Finished strArgs: {0}", strArgs)

            'Separate between command and arguments specified
            Dim Command As String = CommandText.Split(" ")(0)
            Dim Arguments() As String
            Dim TStream As New MemoryStream(Encoding.Default.GetBytes(strArgs))
            Dim Parser As New TextFieldParser(TStream) With {
                .Delimiters = {" "},
                .HasFieldsEnclosedInQuotes = True,
                .TrimWhiteSpace = False
            }
            Arguments = Parser.ReadFields
            If Arguments IsNot Nothing Then
                For i As Integer = 0 To Arguments.Length - 1
                    Arguments(i).Replace("""", "")
                Next
                RequiredArgumentsProvided = Arguments?.Length >= TextEdit_Commands(Command).MinimumArguments
            ElseIf TextEdit_Commands(Command).ArgumentsRequired And Arguments Is Nothing Then
                RequiredArgumentsProvided = False
            End If

            'Try to parse command
            Select Case Command
                Case "print"
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
                                W("- {0}: ", False, ColTypes.ListEntry, LineNumber)
                                W(Line, True, ColTypes.ListValue)
                            Else
                                W(DoTranslation("The specified line number may not be larger than the last file line number."), True, ColTypes.Error)
                            End If
                        Else
                            W(DoTranslation("Specified line number {0} is not a valid number."), True, ColTypes.Error, Arguments(0))
                            Wdbg("E", "{0} is not a numeric value.", Arguments(0))
                        End If
                    Else
                        For Each Line As String In TextEdit_FileLines
                            Wdbg("I", "Line number: {0} ({1})", LineNumber, Line)
                            W("- {0}: ", False, ColTypes.ListEntry, LineNumber)
                            W(Line, True, ColTypes.ListValue)
                            LineNumber += 1
                        Next
                    End If
                Case "addline"
                    If RequiredArgumentsProvided Then
                        TextEdit_AddNewLine(strArgs)
                    End If
                Case "delline"
                    If RequiredArgumentsProvided Then
                        If IsNumeric(Arguments(0)) Then
                            If CInt(Arguments(0)) <= TextEdit_FileLines.Count Then
                                TextEdit_RemoveLine(Arguments(0))
                                W(DoTranslation("Removed line."), True, ColTypes.Neutral)
                            Else
                                W(DoTranslation("The specified line number may not be larger than the last file line number."), True, ColTypes.Error)
                            End If
                        Else
                            W(DoTranslation("Specified line number {0} is not a valid number."), True, ColTypes.Error, Arguments(0))
                            Wdbg("E", "{0} is not a numeric value.", Arguments(0))
                        End If
                    End If
                Case "replace"
                    If RequiredArgumentsProvided Then
                        TextEdit_Replace(Arguments(0), Arguments(1))
                        W(DoTranslation("String replaced."), True, ColTypes.Neutral)
                    End If
                Case "replaceinline"
                    If RequiredArgumentsProvided Then
                        If Arguments(2).IsNumeric Then
                            If CInt(Arguments(2)) <= TextEdit_FileLines.Count Then
                                TextEdit_Replace(Arguments(0), Arguments(1), Arguments(2))
                                W(DoTranslation("String replaced."), True, ColTypes.Neutral)
                            Else
                                W(DoTranslation("The specified line number may not be larger than the last file line number."), True, ColTypes.Error)
                            End If
                        Else
                            W(DoTranslation("Specified line number {0} is not a valid number."), True, ColTypes.Error, Arguments(2))
                            Wdbg("E", "{0} is not a numeric value.", Arguments(2))
                        End If
                    End If
                Case "delword"
                    If RequiredArgumentsProvided Then
                        If IsNumeric(Arguments(1)) Then
                            If CInt(Arguments(1)) <= TextEdit_FileLines.Count Then
                                TextEdit_DeleteWord(Arguments(0), Arguments(1))
                                W(DoTranslation("Word deleted."), True, ColTypes.Neutral)
                            Else
                                W(DoTranslation("The specified line number may not be larger than the last file line number."), True, ColTypes.Error)
                            End If
                        Else
                            W(DoTranslation("Specified line number {0} is not a valid number."), True, ColTypes.Error, Arguments(1))
                            Wdbg("E", "{0} is not a numeric value.", Arguments(1))
                        End If
                    End If
                Case "delcharnum"
                    If RequiredArgumentsProvided Then
                        If IsNumeric(Arguments(1)) And IsNumeric(Arguments(0)) Then
                            If CInt(Arguments(1)) <= TextEdit_FileLines.Count Then
                                TextEdit_DeleteChar(Arguments(0), Arguments(1))
                                W(DoTranslation("Character deleted."), True, ColTypes.Neutral)
                            Else
                                W(DoTranslation("The specified line number may not be larger than the last file line number."), True, ColTypes.Error)
                            End If
                        Else
                            W(DoTranslation("One or both of the numbers are not numeric."), True, ColTypes.Error)
                            Wdbg("E", "{0} and {1} are not numeric values.", Arguments(0), Arguments(1))
                        End If
                    End If
                Case "querychar"
                    If RequiredArgumentsProvided Then
                        If IsNumeric(Arguments(1)) Then
                            If CInt(Arguments(1)) <= TextEdit_FileLines.Count Then
                                Dim QueriedChars As Dictionary(Of Integer, String) = TextEdit_QueryChar(Arguments(0), Arguments(1))
                                For Each CharIndex As Integer In QueriedChars.Keys
                                    W("- {0}: ", False, ColTypes.ListEntry, CharIndex)
                                    W("{0} ({1})", True, ColTypes.ListValue, Arguments(0), QueriedChars(CharIndex))
                                Next
                            Else
                                W(DoTranslation("The specified line number may not be larger than the last file line number."), True, ColTypes.Error)
                            End If
                        ElseIf Arguments(1).ToLower = "all" Then
                            Dim QueriedChars As Dictionary(Of Integer, Dictionary(Of Integer, String)) = TextEdit_QueryChar(Arguments(0))
                            For Each LineIndex As Integer In QueriedChars.Keys
                                For Each CharIndex As Integer In QueriedChars(LineIndex).Keys
                                    W("- {0}:{1}: ", False, ColTypes.ListEntry, LineIndex, CharIndex)
                                    W("{0} ({1})", True, ColTypes.ListValue, Arguments(0), TextEdit_FileLines(LineIndex))
                                Next
                            Next
                        End If
                    End If
                Case "queryword"
                    If RequiredArgumentsProvided Then
                        If IsNumeric(Arguments(1)) Then
                            If CInt(Arguments(1)) <= TextEdit_FileLines.Count Then
                                Dim QueriedChars As Dictionary(Of Integer, String) = TextEdit_QueryWord(Arguments(0), Arguments(1))
                                For Each WordIndex As Integer In QueriedChars.Keys
                                    W("- {0}: ", False, ColTypes.ListEntry, WordIndex)
                                    W("{0} ({1})", True, ColTypes.ListValue, Arguments(0), TextEdit_FileLines(Arguments(1)))
                                Next
                            Else
                                W(DoTranslation("The specified line number may not be larger than the last file line number."), True, ColTypes.Error)
                            End If
                        ElseIf Arguments(1).ToLower = "all" Then
                            Dim QueriedWords As Dictionary(Of Integer, Dictionary(Of Integer, String)) = TextEdit_QueryWord(Arguments(0))
                            For Each LineIndex As Integer In QueriedWords.Keys
                                For Each WordIndex As Integer In QueriedWords(LineIndex).Keys
                                    W("- {0}:{1}: ", False, ColTypes.ListEntry, LineIndex, WordIndex)
                                    W("{0} ({1})", True, ColTypes.ListValue, Arguments(0), TextEdit_FileLines(LineIndex))
                                Next
                            Next
                        End If
                    End If
                Case "clear"
                    TextEdit_FileLines.Clear()
                Case "save"
                    TextEdit_SaveTextFile(False)
                Case "exit"
                    TextEdit_SaveTextFile(True)
                    TextEdit_Exiting = True
                Case "exitnosave"
                    TextEdit_Exiting = True
                Case "help"
                    If Arguments?.Length > 0 Then
                        Wdbg("I", "Requested help for {0}", Arguments(0))
                        TextEdit_GetHelp(Arguments(0))
                    Else
                        Wdbg("I", "Requested help for all commands")
                        TextEdit_GetHelp()
                    End If
            End Select

            'See if the command is done (passed all required arguments)
            If TextEdit_Commands(Command).ArgumentsRequired And Not RequiredArgumentsProvided Then
                W(DoTranslation("Required arguments are not passed to command {0}"), True, ColTypes.Error, Command)
                Wdbg("E", "Passed arguments were not enough to run command {0}. Arguments passed: {1}", Command, Arguments?.Length)
                TextEdit_GetHelp(Command)
            End If
        Catch taex As ThreadAbortException
            Exit Sub
        Catch ex As Exception
            W(DoTranslation("Error trying to run command: {0}"), True, ColTypes.Error, ex.Message)
            Wdbg("E", "Error running command {0}: {1}", CommandText.Split(" ")(0), ex.Message)
            WStkTrc(ex)
            EventManager.RaiseTextCommandError(CommandText, ex)
        End Try
    End Sub

    Sub EditorCancelCommand(sender As Object, e As ConsoleCancelEventArgs)
        If e.SpecialKey = ConsoleSpecialKey.ControlC Then
            Console.WriteLine()
            DefConsoleOut = Console.Out
            Console.SetOut(StreamWriter.Null)
            e.Cancel = True
            TextEdit_CommandThread.Abort()
        End If
    End Sub

End Module
