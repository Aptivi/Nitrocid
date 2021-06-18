
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
    Public ReadOnly TextEdit_Commands As New Dictionary(Of String, CommandInfo) From {{"addline", New CommandInfo("addline", ShellCommandType.TextShell, DoTranslation("Adds a new line with text at the end of the file"), True, 1)},
                                                                                      {"clear", New CommandInfo("clear", ShellCommandType.TextShell, DoTranslation("Clears the text file"), False, 0)},
                                                                                      {"delcharnum", New CommandInfo("delcharnum", ShellCommandType.TextShell, DoTranslation("Deletes a character from character number in specified line"), True, 2)},
                                                                                      {"delline", New CommandInfo("delline", ShellCommandType.TextShell, DoTranslation("Removes the specified line number"), True, 1)},
                                                                                      {"delword", New CommandInfo("delword", ShellCommandType.TextShell, DoTranslation("Deletes a word or phrase from line number"), True, 2)},
                                                                                      {"exit", New CommandInfo("exit", ShellCommandType.TextShell, DoTranslation("Exits the text editor and save unsaved changes"), False, 0)},
                                                                                      {"exitnosave", New CommandInfo("exitnosave", ShellCommandType.TextShell, DoTranslation("Exits the text editor"), False, 0)},
                                                                                      {"help", New CommandInfo("help", ShellCommandType.TextShell, DoTranslation("Lists available commands"), False, 0)},
                                                                                      {"print", New CommandInfo("print", ShellCommandType.TextShell, DoTranslation("Prints the contents of the file with line numbers to the console"), False, 0)},
                                                                                      {"querychar", New CommandInfo("querychar", ShellCommandType.TextShell, DoTranslation("Queries a character in a specified line or all lines"), True, 2)},
                                                                                      {"queryword", New CommandInfo("queryword", ShellCommandType.TextShell, DoTranslation("Queries a word in a specified line or all lines"), True, 2)},
                                                                                      {"replace", New CommandInfo("replace", ShellCommandType.TextShell, DoTranslation("Replaces a word or phrase with another one"), True, 2)},
                                                                                      {"replaceinline", New CommandInfo("replaceinline", ShellCommandType.TextShell, DoTranslation("Replaces a word or phrase with another one in a line"), True, 3)},
                                                                                      {"save", New CommandInfo("save", ShellCommandType.TextShell, DoTranslation("Saves the file"), False, 0)}}
    Public TextEdit_ModCommands As New ArrayList
    Public TextEdit_FileStream As FileStream
    Public TextEdit_FileLines As List(Of String)
    Friend TextEdit_FileLinesOrig As List(Of String)
    Public TextEdit_AutoSave As New Thread(AddressOf TextEdit_HandleAutoSaveTextFile) With {.Name = "Text Edit Autosave Thread"}
    Public TextEdit_AutoSaveFlag As Boolean = True
    Public TextEdit_AutoSaveInterval As Integer = 60

    Public Sub InitializeTextShell(ByVal FilePath As String)
        'Add handler for text editor shell
        AddHandler Console.CancelKeyPress, AddressOf EditorCancelCommand
        RemoveHandler Console.CancelKeyPress, AddressOf CancelCommand

        While Not TextEdit_Exiting
            'Open file if not open
            If TextEdit_FileStream Is Nothing Then
                Wdbg("W", "File not open yet. Trying to open {0}...", FilePath)
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
            W("[", False, ColTypes.Gray) : W("{0}{1}", False, ColTypes.UserName, Path.GetFileName(FilePath), If(TextEdit_WasTextEdited(), "*", "")) : W("] > ", False, ColTypes.Gray)
            SetInputColor()

            'Prompt for command
            EventManager.RaiseTextShellInitialized()
            Dim WrittenCommand As String = Console.ReadLine

            'Check to see if the command doesn't start with spaces or if the command is nothing
            Wdbg("I", "Starts with spaces: {0}, Is Nothing: {1}, Is Blank {2}", WrittenCommand?.StartsWith(" "), WrittenCommand Is Nothing, WrittenCommand = "")
            If Not (WrittenCommand = Nothing Or WrittenCommand?.StartsWithAnyOf({" ", "#"}) = True) Then
                Dim Command As String = WrittenCommand.SplitEncloseDoubleQuotes(" ")(0)
                Wdbg("I", "Checking command {0} for existence.", Command)
                If TextEdit_Commands.ContainsKey(Command) Then
                    Wdbg("I", "Command {0} found in the list of {1} commands.", Command, TextEdit_Commands.Count)
                    TextEdit_CommandThread = New Thread(AddressOf TextEdit_ParseCommand) With {.Name = "Text Edit Command Thread"}
                    EventManager.RaiseTextPreExecuteCommand(WrittenCommand)
                    Wdbg("I", "Made new thread. Starting with argument {0}...", WrittenCommand)
                    TextEdit_CommandThread.Start(WrittenCommand)
                    TextEdit_CommandThread.Join()
                    EventManager.RaiseTextPostExecuteCommand(WrittenCommand)
                ElseIf TextEdit_ModCommands.Contains(Command) Then
                    Wdbg("I", "Mod command {0} executing...", Command)
                    ExecuteModCommand(WrittenCommand)
                ElseIf TextShellAliases.Keys.Contains(Command) Then
                    Wdbg("I", "Text shell alias command found.")
                    WrittenCommand = WrittenCommand.Replace($"""{Command}""", Command)
                    ExecuteTextAlias(WrittenCommand)
                Else
                    W(DoTranslation("The specified text editor command is not found."), True, ColTypes.Error)
                    Wdbg("E", "Command {0} not found in the list of {1} commands.", Command, TextEdit_Commands.Count)
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
        AddHandler Console.CancelKeyPress, AddressOf CancelCommand
        RemoveHandler Console.CancelKeyPress, AddressOf EditorCancelCommand
        TextEdit_Exiting = False
    End Sub

    ''' <summary>
    ''' Executes the text editor shell alias
    ''' </summary>
    ''' <param name="aliascmd">Aliased command with arguments</param>
    Sub ExecuteTextAlias(ByVal aliascmd As String)
        Dim FirstWordCmd As String = aliascmd.SplitEncloseDoubleQuotes(" ")(0)
        Dim actualCmd As String = aliascmd.Replace(FirstWordCmd, TextShellAliases(FirstWordCmd))
        Wdbg("I", "Actual command: {0}", actualCmd)
        TextEdit_CommandThread = New Thread(AddressOf TextEdit_ParseCommand) With {.Name = "Text Edit Command Thread"}
        TextEdit_CommandThread.Start(actualCmd)
        TextEdit_CommandThread.Join()
    End Sub

End Module
