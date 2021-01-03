
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
    Public TextEdit_Commands As String() = {"help", "exit", "print", "addline", "delline", "replace", "replaceinline", "delword", "delcharnum"}
    Public TextEdit_ModCommands As New ArrayList
    Public TextEdit_FileStream As FileStream
    Public TextEdit_FileLines As List(Of String)
    Public TextEdit_AutoSave As New Thread(AddressOf TextEdit_HandleAutoSaveTextFile)

    Public Sub InitializeTextShell(ByVal FilePath As String)
        'Add handler for text editor shell
        AddHandler Console.CancelKeyPress, AddressOf EditorCancelCommand
        RemoveHandler Console.CancelKeyPress, AddressOf CancelCommand

        While Not TextEdit_Exiting
            'Open file if not open
            If IsNothing(TextEdit_FileStream) Then
                Wdbg("W", "File not open yet. Trying to open {0}...", FilePath)
                If Not TextEdit_OpenTextFile(FilePath) Then
                    W(DoTranslation("Failed to open file. Exiting shell...", currentLang), True, ColTypes.Err)
                    Exit While
                End If
                TextEdit_AutoSave.Start()
            End If

            'Prepare for prompt
            If Not IsNothing(DefConsoleOut) Then
                Console.SetOut(DefConsoleOut)
            End If
            W("[", False, ColTypes.Gray) : W("{0}", False, ColTypes.UserName, Path.GetFileName(FilePath)) : W("] > ", False, ColTypes.Gray)
            SetInputColor()

            'Prompt for command
            EventManager.RaiseTextShellInitialized()
            Dim WrittenCommand As String = Console.ReadLine

            'Check to see if the command doesn't start with spaces or if the command is nothing
            Wdbg("I", "Starts with spaces: {0}, Is Nothing: {1}, Is Blank {2}", WrittenCommand.StartsWith(" "), IsNothing(WrittenCommand), WrittenCommand = "")
            If Not (WrittenCommand = Nothing Or WrittenCommand?.StartsWith(" ") = True) Then
                Wdbg("I", "Checking command {0} for existence.", WrittenCommand.Split(" ")(0))
                If TextEdit_Commands.Contains(WrittenCommand.Split(" ")(0)) Then
                    Wdbg("I", "Command {0} found in the list of {1} commands.", WrittenCommand.Split(" ")(0), TextEdit_Commands.Length)
                    TextEdit_CommandThread = New Thread(AddressOf TextEdit_ParseCommand)
                    EventManager.RaiseTextPreExecuteCommand(WrittenCommand)
                    Wdbg("I", "Made new thread. Starting with argument {0}...", WrittenCommand)
                    TextEdit_CommandThread.Start(WrittenCommand)
                    TextEdit_CommandThread.Join()
                    EventManager.RaiseTextPostExecuteCommand(WrittenCommand)
                ElseIf TextEdit_ModCommands.Contains(WrittenCommand.Split(" ")(0)) Then
                    Wdbg("I", "Mod command {0} executing...", WrittenCommand.Split(" ")(0))
                    EventManager.RaiseTextPreExecuteCommand(WrittenCommand)
                    ExecuteModCommand(WrittenCommand)
                    EventManager.RaiseTextPostExecuteCommand(WrittenCommand)
                Else
                    W(DoTranslation("The specified text editor command is not found.", currentLang), True, ColTypes.Err)
                    Wdbg("E", "Command {0} not found in the list of {1} commands.", WrittenCommand.Split(" ")(0), TextEdit_Commands.Length)
                End If
            End If

            'When pressing CTRL+C on shell after command execution, it can generate another prompt without making newline, so fix this.
            If IsNothing(strcommand) Then
                Console.WriteLine()
                Thread.Sleep(30) 'This is to fix race condition between shell initialization and starting the event handler thread
            End If
        End While

        'Close file
        TextEdit_CloseTextFile()
        TextEdit_AutoSave.Abort()
        TextEdit_AutoSave = New Thread(AddressOf TextEdit_HandleAutoSaveTextFile)

        'Remove handler for text editor shell
        AddHandler Console.CancelKeyPress, AddressOf CancelCommand
        RemoveHandler Console.CancelKeyPress, AddressOf EditorCancelCommand
        TextEdit_Exiting = False
    End Sub

    Sub EditorCancelCommand(sender As Object, e As ConsoleCancelEventArgs)
        If e.SpecialKey = ConsoleSpecialKey.ControlC Then
            DefConsoleOut = Console.Out
            Console.SetOut(StreamWriter.Null)
            e.Cancel = True
            TextEdit_CommandThread.Abort()
        End If
    End Sub

End Module
