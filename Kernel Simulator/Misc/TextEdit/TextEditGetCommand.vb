
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

Public Module TextEditGetCommand

    'Variables
    Public TextEdit_CommandThread As New Thread(AddressOf TextEdit_ParseCommand) With {.Name = "Text Edit Command Thread"}

    Sub TextEdit_ParseCommand(ByVal requestedCommand As String)
        Try
            'Variables
            Dim Command As String
            Dim RequiredArgumentsProvided As Boolean = True

            '1. Get the index of the first space (Used for step 3)
            Dim index As Integer = requestedCommand.IndexOf(" ")
            If index = -1 Then index = requestedCommand.Length
            Wdbg("I", "Index: {0}", index)

            '2. Split the requested command string into words
            Dim words() As String = requestedCommand.Split({" "c})
            For i As Integer = 0 To words.Length - 1
                Wdbg("I", "Word {0}: {1}", i + 1, words(i))
            Next
            Command = words(0)

            '3. Get the string of arguments
            Dim strArgs As String = requestedCommand.Substring(index)
            Wdbg("I", "Prototype strArgs: {0}", strArgs)
            If Not index = requestedCommand.Length Then strArgs = strArgs.Substring(1)
            Wdbg("I", "Finished strArgs: {0}", strArgs)

            '4. Split the arguments with enclosed quotes and set the required boolean variable
            Dim eqargs() As String = strArgs.SplitEncloseDoubleQuotes(" ")
            If eqargs IsNot Nothing Then
                RequiredArgumentsProvided = eqargs?.Length >= TextEdit_Commands(Command).MinimumArguments
            ElseIf TextEdit_Commands(Command).ArgumentsRequired And eqargs Is Nothing Then
                RequiredArgumentsProvided = False
            End If

            '4a. Debug: get all arguments from eqargs()
            If eqargs IsNot Nothing Then Wdbg("I", "Arguments parsed from eqargs(): " + String.Join(", ", eqargs))

            '5. Check to see if a requested command is obsolete
            If TextEdit_Commands(Command).Obsolete Then
                Wdbg("I", "The command requested {0} is obsolete", Command)
                W(DoTranslation("This command is obsolete and will be removed in a future release."), True, ColTypes.Neutral)
            End If

            '6. Execute a command
            Select Case Command
                Case "print"
                    Dim LineNumber As Integer = 1
                    If eqargs?.Length > 0 Then
                        Wdbg("I", "Line number provided: {0}", eqargs(0))
                        Wdbg("I", "Is it numeric? {0}", eqargs(0).IsNumeric)
                        If eqargs(0).IsNumeric Then
                            LineNumber = eqargs(0)
                            Wdbg("I", "File lines: {0}", TextEdit_FileLines.Count)
                            If CInt(eqargs(0)) <= TextEdit_FileLines.Count Then
                                Dim Line As String = TextEdit_FileLines(LineNumber - 1)
                                Wdbg("I", "Line number: {0} ({1})", LineNumber, Line)
                                W("- {0}: ", False, ColTypes.ListEntry, LineNumber)
                                W(Line, True, ColTypes.ListValue)
                            Else
                                W(DoTranslation("The specified line number may not be larger than the last file line number."), True, ColTypes.Error)
                            End If
                        Else
                            W(DoTranslation("Specified line number {0} is not a valid number."), True, ColTypes.Error, eqargs(0))
                            Wdbg("E", "{0} is not a numeric value.", eqargs(0))
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
                        If IsNumeric(eqargs(0)) Then
                            If CInt(eqargs(0)) <= TextEdit_FileLines.Count Then
                                TextEdit_RemoveLine(eqargs(0))
                                W(DoTranslation("Removed line."), True, ColTypes.Neutral)
                            Else
                                W(DoTranslation("The specified line number may not be larger than the last file line number."), True, ColTypes.Error)
                            End If
                        Else
                            W(DoTranslation("Specified line number {0} is not a valid number."), True, ColTypes.Error, eqargs(0))
                            Wdbg("E", "{0} is not a numeric value.", eqargs(0))
                        End If
                    End If
                Case "replace"
                    If RequiredArgumentsProvided Then
                        TextEdit_Replace(eqargs(0), eqargs(1))
                        W(DoTranslation("String replaced."), True, ColTypes.Neutral)
                    End If
                Case "replaceinline"
                    If RequiredArgumentsProvided Then
                        If eqargs(2).IsNumeric Then
                            If CInt(eqargs(2)) <= TextEdit_FileLines.Count Then
                                TextEdit_Replace(eqargs(0), eqargs(1), eqargs(2))
                                W(DoTranslation("String replaced."), True, ColTypes.Neutral)
                            Else
                                W(DoTranslation("The specified line number may not be larger than the last file line number."), True, ColTypes.Error)
                            End If
                        Else
                            W(DoTranslation("Specified line number {0} is not a valid number."), True, ColTypes.Error, eqargs(2))
                            Wdbg("E", "{0} is not a numeric value.", eqargs(2))
                        End If
                    End If
                Case "delword"
                    If RequiredArgumentsProvided Then
                        If IsNumeric(eqargs(1)) Then
                            If CInt(eqargs(1)) <= TextEdit_FileLines.Count Then
                                TextEdit_DeleteWord(eqargs(0), eqargs(1))
                                W(DoTranslation("Word deleted."), True, ColTypes.Neutral)
                            Else
                                W(DoTranslation("The specified line number may not be larger than the last file line number."), True, ColTypes.Error)
                            End If
                        Else
                            W(DoTranslation("Specified line number {0} is not a valid number."), True, ColTypes.Error, eqargs(1))
                            Wdbg("E", "{0} is not a numeric value.", eqargs(1))
                        End If
                    End If
                Case "delcharnum"
                    If RequiredArgumentsProvided Then
                        If IsNumeric(eqargs(1)) And IsNumeric(eqargs(0)) Then
                            If CInt(eqargs(1)) <= TextEdit_FileLines.Count Then
                                TextEdit_DeleteChar(eqargs(0), eqargs(1))
                                W(DoTranslation("Character deleted."), True, ColTypes.Neutral)
                            Else
                                W(DoTranslation("The specified line number may not be larger than the last file line number."), True, ColTypes.Error)
                            End If
                        Else
                            W(DoTranslation("One or both of the numbers are not numeric."), True, ColTypes.Error)
                            Wdbg("E", "{0} and {1} are not numeric values.", eqargs(0), eqargs(1))
                        End If
                    End If
                Case "querychar"
                    If RequiredArgumentsProvided Then
                        If IsNumeric(eqargs(1)) Then
                            If CInt(eqargs(1)) <= TextEdit_FileLines.Count Then
                                Dim QueriedChars As Dictionary(Of Integer, String) = TextEdit_QueryChar(eqargs(0), eqargs(1))
                                For Each CharIndex As Integer In QueriedChars.Keys
                                    W("- {0}: ", False, ColTypes.ListEntry, CharIndex)
                                    W("{0} ({1})", True, ColTypes.ListValue, eqargs(0), QueriedChars(CharIndex))
                                Next
                            Else
                                W(DoTranslation("The specified line number may not be larger than the last file line number."), True, ColTypes.Error)
                            End If
                        ElseIf eqargs(1).ToLower = "all" Then
                            Dim QueriedChars As Dictionary(Of Integer, Dictionary(Of Integer, String)) = TextEdit_QueryChar(eqargs(0))
                            For Each LineIndex As Integer In QueriedChars.Keys
                                For Each CharIndex As Integer In QueriedChars(LineIndex).Keys
                                    W("- {0}:{1}: ", False, ColTypes.ListEntry, LineIndex, CharIndex)
                                    W("{0} ({1})", True, ColTypes.ListValue, eqargs(0), TextEdit_FileLines(LineIndex))
                                Next
                            Next
                        End If
                    End If
                Case "queryword"
                    If RequiredArgumentsProvided Then
                        If IsNumeric(eqargs(1)) Then
                            If CInt(eqargs(1)) <= TextEdit_FileLines.Count Then
                                Dim QueriedChars As Dictionary(Of Integer, String) = TextEdit_QueryWord(eqargs(0), eqargs(1))
                                For Each WordIndex As Integer In QueriedChars.Keys
                                    W("- {0}: ", False, ColTypes.ListEntry, WordIndex)
                                    W("{0} ({1})", True, ColTypes.ListValue, eqargs(0), TextEdit_FileLines(eqargs(1)))
                                Next
                            Else
                                W(DoTranslation("The specified line number may not be larger than the last file line number."), True, ColTypes.Error)
                            End If
                        ElseIf eqargs(1).ToLower = "all" Then
                            Dim QueriedWords As Dictionary(Of Integer, Dictionary(Of Integer, String)) = TextEdit_QueryWord(eqargs(0))
                            For Each LineIndex As Integer In QueriedWords.Keys
                                For Each WordIndex As Integer In QueriedWords(LineIndex).Keys
                                    W("- {0}:{1}: ", False, ColTypes.ListEntry, LineIndex, WordIndex)
                                    W("{0} ({1})", True, ColTypes.ListValue, eqargs(0), TextEdit_FileLines(LineIndex))
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
                    If eqargs?.Length > 0 Then
                        Wdbg("I", "Requested help for {0}", eqargs(0))
                        ShowHelp(eqargs(0), ShellCommandType.TextShell)
                    Else
                        Wdbg("I", "Requested help for all commands")
                        ShowHelp(ShellCommandType.TextShell)
                    End If
            End Select

            'See if the command is done (passed all required arguments)
            If TextEdit_Commands(Command).ArgumentsRequired And Not RequiredArgumentsProvided Then
                W(DoTranslation("Required arguments are not passed to command {0}"), True, ColTypes.Error, Command)
                Wdbg("E", "Passed arguments were not enough to run command {0}. Arguments passed: {1}", Command, eqargs?.Length)
                ShowHelp(Command, ShellCommandType.TextShell)
            End If
        Catch taex As ThreadAbortException
            Exit Sub
        Catch ex As Exception
            W(DoTranslation("Error trying to run command: {0}"), True, ColTypes.Error, ex.Message)
            Wdbg("E", "Error running command {0}: {1}", requestedCommand.Split(" ")(0), ex.Message)
            WStkTrc(ex)
            EventManager.RaiseTextCommandError(requestedCommand, ex)
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
