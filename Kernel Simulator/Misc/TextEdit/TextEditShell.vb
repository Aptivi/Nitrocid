
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

Public Module TextEditShell

    'Variables
    Public TextEdit_Exiting As Boolean
    Public ReadOnly TextEdit_Commands As New Dictionary(Of String, CommandInfo) From {{"addline", New CommandInfo("addline", ShellCommandType.TextShell, "Adds a new line with text at the end of the file", "<text>", True, 1, New TextEdit_AddLineCommand)},
                                                                                      {"clear", New CommandInfo("clear", ShellCommandType.TextShell, "Clears the text file", "", False, 0, New TextEdit_ClearCommand)},
                                                                                      {"delcharnum", New CommandInfo("delcharnum", ShellCommandType.TextShell, "Deletes a character from character number in specified line", "<charnumber> <linenumber>", True, 2, New TextEdit_DelCharNumCommand)},
                                                                                      {"delline", New CommandInfo("delline", ShellCommandType.TextShell, "Removes the specified line number", "<linenumber> [linenumber2]", True, 1, New TextEdit_DelLineCommand)},
                                                                                      {"delword", New CommandInfo("delword", ShellCommandType.TextShell, "Deletes a word or phrase from line number", """<word/phrase>"" <linenumber> [linenumber2]", True, 2, New TextEdit_DelWordCommand)},
                                                                                      {"exit", New CommandInfo("exit", ShellCommandType.TextShell, "Exits the text editor and save unsaved changes", "", False, 0, New TextEdit_ExitCommand)},
                                                                                      {"exitnosave", New CommandInfo("exitnosave", ShellCommandType.TextShell, "Exits the text editor", "", False, 0, New TextEdit_ExitNoSaveCommand)},
                                                                                      {"help", New CommandInfo("help", ShellCommandType.TextShell, "Lists available commands", "[command]", False, 0, New TextEdit_HelpCommand)},
                                                                                      {"print", New CommandInfo("print", ShellCommandType.TextShell, "Prints the contents of the file with line numbers to the console", "[linenumber] [linenumber2]", False, 0, New TextEdit_PrintCommand)},
                                                                                      {"querychar", New CommandInfo("querychar", ShellCommandType.TextShell, "Queries a character in a specified line or all lines", "<char> <linenumber/all> [linenumber2]", True, 2, New TextEdit_QueryCharCommand)},
                                                                                      {"queryword", New CommandInfo("queryword", ShellCommandType.TextShell, "Queries a word in a specified line or all lines", """<word/phrase>"" <linenumber/all> [linenumber2]", True, 2, New TextEdit_QueryWordCommand)},
                                                                                      {"replace", New CommandInfo("replace", ShellCommandType.TextShell, "Replaces a word or phrase with another one", """<word/phrase>"" ""<word/phrase>""", True, 2, New TextEdit_ReplaceCommand)},
                                                                                      {"replaceinline", New CommandInfo("replaceinline", ShellCommandType.TextShell, "Replaces a word or phrase with another one in a line", """<word/phrase>"" ""<word/phrase>"" <linenumber> [linenumber2]", True, 3, New TextEdit_ReplaceInlineCommand)},
                                                                                      {"save", New CommandInfo("save", ShellCommandType.TextShell, "Saves the file", "", False, 0, New TextEdit_SaveCommand)}}
    Public TextEdit_ModCommands As New ArrayList
    Public TextEdit_FileStream As FileStream
    Public TextEdit_FileLines As List(Of String)
    Friend TextEdit_FileLinesOrig As List(Of String)
    Public TextEdit_AutoSave As New Thread(AddressOf TextEdit_HandleAutoSaveTextFile) With {.Name = "Text Edit Autosave Thread"}
    Public TextEdit_AutoSaveFlag As Boolean = True
    Public TextEdit_AutoSaveInterval As Integer = 60
    Public TextEdit_PromptStyle As String = ""

    Public Sub InitializeTextShell(FilePath As String)
        'Add handler for text editor shell
        SwitchCancellationHandler(ShellCommandType.TextShell)

        While Not TextEdit_Exiting
            'Open file if not open
            If TextEdit_FileStream Is Nothing Then
                Wdbg(DebugLevel.W, "File not open yet. Trying to open {0}...", FilePath)
                If Not TextEdit_OpenTextFile(FilePath) Then
                    W(DoTranslation("Failed to open file. Exiting shell..."), True, ColTypes.Error)
                    Exit While
                End If
                TextEdit_AutoSave.Start()
            End If

            'Prepare for prompt
            If DefConsoleOut IsNot Nothing Then
                Console.SetOut(DefConsoleOut)
            End If
            Wdbg(DebugLevel.I, "TextEdit_PromptStyle = {0}", TextEdit_PromptStyle)
            If TextEdit_PromptStyle = "" Then
                W("[", False, ColTypes.Gray) : W("{0}{1}", False, ColTypes.UserName, Path.GetFileName(FilePath), If(TextEdit_WasTextEdited(), "*", "")) : W("] > ", False, ColTypes.Gray)
            Else
                Dim ParsedPromptStyle As String = ProbePlaces(TextEdit_PromptStyle)
                ParsedPromptStyle.ConvertVTSequences
                W(ParsedPromptStyle, False, ColTypes.Gray)
            End If
            SetInputColor()

            'Prompt for command
            EventManager.RaiseTextShellInitialized()
            Dim WrittenCommand As String = Console.ReadLine

            'Check to see if the command doesn't start with spaces or if the command is nothing
            Wdbg(DebugLevel.I, "Starts with spaces: {0}, Is Nothing: {1}, Is Blank {2}", WrittenCommand?.StartsWith(" "), WrittenCommand Is Nothing, WrittenCommand = "")
            If Not (WrittenCommand = Nothing Or WrittenCommand?.StartsWithAnyOf({" ", "#"}) = True) Then
                Dim Command As String = WrittenCommand.SplitEncloseDoubleQuotes(" ")(0)
                Wdbg(DebugLevel.I, "Checking command {0} for existence.", Command)
                If TextEdit_Commands.ContainsKey(Command) Then
                    Wdbg(DebugLevel.I, "Command {0} found in the list of {1} commands.", Command, TextEdit_Commands.Count)
                    Dim Params As New ExecuteCommandThreadParameters(WrittenCommand, ShellCommandType.TextShell, Nothing)
                    TextEdit_CommandThread = New Thread(AddressOf ExecuteCommand) With {.Name = "Text Edit Command Thread"}
                    EventManager.RaiseTextPreExecuteCommand(WrittenCommand)
                    Wdbg(DebugLevel.I, "Made new thread. Starting with argument {0}...", WrittenCommand)
                    TextEdit_CommandThread.Start(Params)
                    TextEdit_CommandThread.Join()
                    EventManager.RaiseTextPostExecuteCommand(WrittenCommand)
                ElseIf TextEdit_ModCommands.Contains(Command) Then
                    Wdbg(DebugLevel.I, "Mod command {0} executing...", Command)
                    ExecuteModCommand(WrittenCommand)
                ElseIf TextShellAliases.Keys.Contains(Command) Then
                    Wdbg(DebugLevel.I, "Text shell alias command found.")
                    WrittenCommand = WrittenCommand.Replace($"""{Command}""", Command)
                    ExecuteTextAlias(WrittenCommand)
                Else
                    W(DoTranslation("The specified text editor command is not found."), True, ColTypes.Error)
                    Wdbg(DebugLevel.E, "Command {0} not found in the list of {1} commands.", Command, TextEdit_Commands.Count)
                End If
            End If

            'This is to fix race condition between shell initialization and starting the event handler thread
            If WrittenCommand Is Nothing Then
                Thread.Sleep(30)
            End If
        End While

        'Close file
        TextEdit_CloseTextFile()
        TextEdit_AutoSave.Abort()
        TextEdit_AutoSave = New Thread(AddressOf TextEdit_HandleAutoSaveTextFile) With {.Name = "Text Edit Autosave Thread"}

        'Remove handler for text editor shell
        SwitchCancellationHandler(LastShellType)
        TextEdit_Exiting = False
    End Sub

End Module
