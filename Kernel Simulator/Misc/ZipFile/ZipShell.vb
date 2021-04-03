
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
Imports System.IO.Compression
Imports System.Threading

Module ZipShell

    'Variables
    Public ZipShell_Exiting As Boolean
    Public ZipShell_Commands As String() = {"help", "exit", "list", "get", "chdir", "chadir"}
    Public ZipShell_ModCommands As New ArrayList
    Public ZipShell_FileStream As FileStream
    Public ZipShell_ZipArchive As ZipArchive
    Public ZipShell_CurrentDirectory As String
    Public ZipShell_CurrentArchiveDirectory As String

    ''' <summary>
    ''' Initializes the ZIP shell
    ''' </summary>
    ''' <param name="ZipFile">A ZIP file. We recommend you to use <see cref="NeutralizePath(String)"></see> to neutralize path.</param>
    Public Sub InitializeZipShell(ByVal ZipFile As String)
        'Add handler for text editor shell
        AddHandler Console.CancelKeyPress, AddressOf EditorCancelCommand
        RemoveHandler Console.CancelKeyPress, AddressOf CancelCommand
        ZipShell_CurrentDirectory = CurrDir

        While Not ZipShell_Exiting
            'Open file if not open
            If ZipShell_FileStream Is Nothing Then ZipShell_FileStream = New FileStream(ZipFile, FileMode.Open)
            If ZipShell_ZipArchive Is Nothing Then ZipShell_ZipArchive = New ZipArchive(ZipShell_FileStream, ZipArchiveMode.Update, False)

            'Prepare for prompt
            If Not IsNothing(DefConsoleOut) Then
                Console.SetOut(DefConsoleOut)
            End If
            W("[", False, ColTypes.Gray) : W("{0}@{1}", False, ColTypes.UserName, ZipShell_CurrentArchiveDirectory, Path.GetFileName(ZipFile)) : W("] > ", False, ColTypes.Gray)
            SetInputColor()

            'Prompt for command
            EventManager.RaiseZipShellInitialized()
            Dim WrittenCommand As String = Console.ReadLine

            'Check to see if the command doesn't start with spaces or if the command is nothing
            Wdbg("I", "Starts with spaces: {0}, Is Nothing: {1}, Is Blank {2}", WrittenCommand.StartsWith(" "), IsNothing(WrittenCommand), WrittenCommand = "")
            If Not (WrittenCommand = Nothing Or WrittenCommand?.StartsWith(" ") = True) Then
                Wdbg("I", "Checking command {0} for existence.", WrittenCommand.Split(" ")(0))
                If ZipShell_Commands.Contains(WrittenCommand.Split(" ")(0)) Then
                    Wdbg("I", "Command {0} found in the list of {1} commands.", WrittenCommand.Split(" ")(0), ZipShell_Commands.Length)
                    ZipShell_CommandThread = New Thread(AddressOf ZipShell_ParseCommand)
                    EventManager.RaiseTextPreExecuteCommand(WrittenCommand)
                    Wdbg("I", "Made new thread. Starting with argument {0}...", WrittenCommand)
                    ZipShell_CommandThread.Start(WrittenCommand)
                    ZipShell_CommandThread.Join()
                    EventManager.RaiseTextPostExecuteCommand(WrittenCommand)
                ElseIf ZipShell_ModCommands.Contains(WrittenCommand.Split(" ")(0)) Then
                    Wdbg("I", "Mod command {0} executing...", WrittenCommand.Split(" ")(0))
                    EventManager.RaiseTextPreExecuteCommand(WrittenCommand)
                    ExecuteModCommand(WrittenCommand)
                    EventManager.RaiseTextPostExecuteCommand(WrittenCommand)
                Else
                    W(DoTranslation("The specified ZIP shell command is not found."), True, ColTypes.Err)
                    Wdbg("E", "Command {0} not found in the list of {1} commands.", WrittenCommand.Split(" ")(0), ZipShell_Commands.Length)
                End If
            End If

            'When pressing CTRL+C on shell after command execution, it can generate another prompt without making newline, so fix this.
            If IsNothing(strcommand) Then
                Console.WriteLine()
                Thread.Sleep(30) 'This is to fix race condition between shell initialization and starting the event handler thread
            End If
        End While

        'Close file stream
        ZipShell_ZipArchive.Dispose()
        ZipShell_CurrentDirectory = ""
        ZipShell_CurrentArchiveDirectory = ""

        'Remove handler for text editor shell
        AddHandler Console.CancelKeyPress, AddressOf CancelCommand
        RemoveHandler Console.CancelKeyPress, AddressOf EditorCancelCommand
        ZipShell_Exiting = False
    End Sub

    Sub ZipShellCancelCommand(sender As Object, e As ConsoleCancelEventArgs)
        If e.SpecialKey = ConsoleSpecialKey.ControlC Then
            DefConsoleOut = Console.Out
            Console.SetOut(StreamWriter.Null)
            e.Cancel = True
            ZipShell_CommandThread.Abort()
        End If
    End Sub

End Module
