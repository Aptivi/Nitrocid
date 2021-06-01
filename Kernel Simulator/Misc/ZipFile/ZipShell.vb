
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
    Public ZipShell_Commands As New Dictionary(Of String, CommandInfo) From {{"cdir", New CommandInfo("cdir", ShellCommandType.ZIPShell, False, 0)},
                                                                             {"chdir", New CommandInfo("chdir", ShellCommandType.ZIPShell, True, 1)},
                                                                             {"chadir", New CommandInfo("chadir", ShellCommandType.ZIPShell, True, 1)},
                                                                             {"exit", New CommandInfo("exit", ShellCommandType.ZIPShell, False, 0)},
                                                                             {"get", New CommandInfo("get", ShellCommandType.ZIPShell, True, 1)},
                                                                             {"help", New CommandInfo("help", ShellCommandType.ZIPShell, False, 0)},
                                                                             {"list", New CommandInfo("list", ShellCommandType.ZIPShell, False, 0)}}
    Public ZipShell_ModCommands As New ArrayList
    Public ZipShell_FileStream As FileStream
    Public ZipShell_ZipArchive As ZipArchive
    Public ZipShell_CurrentDirectory As String
    Public ZipShell_CurrentArchiveDirectory As String

    ''' <summary>
    ''' Initializes the ZIP shell
    ''' </summary>
    ''' <param name="ZipFile">A ZIP file. We recommend you to use <see cref="NeutralizePath(String, Boolean)"></see> to neutralize path.</param>
    Public Sub InitializeZipShell(ByVal ZipFile As String)
        'Add handler for ZIP shell
        AddHandler Console.CancelKeyPress, AddressOf ZipShellCancelCommand
        RemoveHandler Console.CancelKeyPress, AddressOf CancelCommand
        ZipShell_CurrentDirectory = CurrDir

        While Not ZipShell_Exiting
            'Open file if not open
            If ZipShell_FileStream Is Nothing Then ZipShell_FileStream = New FileStream(ZipFile, FileMode.Open)
            If ZipShell_ZipArchive Is Nothing Then ZipShell_ZipArchive = New ZipArchive(ZipShell_FileStream, ZipArchiveMode.Update, False)

            'Prepare for prompt
            If DefConsoleOut IsNot Nothing Then
                Console.SetOut(DefConsoleOut)
            End If
            W("[", False, ColTypes.Gray) : W("{0}@{1}", False, ColTypes.UserName, ZipShell_CurrentArchiveDirectory, Path.GetFileName(ZipFile)) : W("] > ", False, ColTypes.Gray)
            SetInputColor()

            'Prompt for command
            EventManager.RaiseZipShellInitialized()
            Dim WrittenCommand As String = Console.ReadLine

            'Check to see if the command doesn't start with spaces or if the command is nothing
            Wdbg("I", "Starts with spaces: {0}, Is Nothing: {1}, Is Blank {2}", WrittenCommand?.StartsWith(" "), WrittenCommand Is Nothing, WrittenCommand = "")
            If Not (WrittenCommand = Nothing Or WrittenCommand?.StartsWith(" ") = True) Then
                Wdbg("I", "Checking command {0} for existence.", WrittenCommand.Split(" ")(0))
                If ZipShell_Commands.ContainsKey(WrittenCommand.Split(" ")(0)) Then
                    Wdbg("I", "Command {0} found in the list of {1} commands.", WrittenCommand.Split(" ")(0), ZipShell_Commands.Count)
                    ZipShell_CommandThread = New Thread(AddressOf ZipShell_ParseCommand) With {.Name = "ZIP Shell Command Thread"}
                    EventManager.RaiseZipPreExecuteCommand(WrittenCommand)
                    Wdbg("I", "Made new thread. Starting with argument {0}...", WrittenCommand)
                    ZipShell_CommandThread.Start(WrittenCommand)
                    ZipShell_CommandThread.Join()
                    EventManager.RaiseZipPostExecuteCommand(WrittenCommand)
                ElseIf ZipShell_ModCommands.Contains(WrittenCommand.Split(" ")(0)) Then
                    Wdbg("I", "Mod command {0} executing...", WrittenCommand.Split(" ")(0))
                    EventManager.RaiseZipPreExecuteCommand(WrittenCommand)
                    ExecuteModCommand(WrittenCommand)
                    EventManager.RaiseZipPostExecuteCommand(WrittenCommand)
                ElseIf ZIPShellAliases.Keys.Contains(WrittenCommand.Split(" ")(0)) Then
                    Wdbg("I", "ZIP shell alias command found.")
                    ExecuteZIPAlias(WrittenCommand)
                Else
                    W(DoTranslation("The specified ZIP shell command is not found."), True, ColTypes.Error)
                    Wdbg("E", "Command {0} not found in the list of {1} commands.", WrittenCommand.Split(" ")(0), ZipShell_Commands.Count)
                End If
            End If

            'This is to fix race condition between shell initialization and starting the event handler thread
            If WrittenCommand Is Nothing Then
                Thread.Sleep(30)
            End If
        End While

        'Close file stream
        ZipShell_ZipArchive.Dispose()
        ZipShell_CurrentDirectory = ""
        ZipShell_CurrentArchiveDirectory = ""
        ZipShell_ZipArchive = Nothing
        ZipShell_FileStream = Nothing

        'Remove handler for ZIP shell
        AddHandler Console.CancelKeyPress, AddressOf CancelCommand
        RemoveHandler Console.CancelKeyPress, AddressOf ZipShellCancelCommand
        ZipShell_Exiting = False
    End Sub

    ''' <summary>
    ''' Executes the ZIP shell alias
    ''' </summary>
    ''' <param name="aliascmd">Aliased command with arguments</param>
    Sub ExecuteZIPAlias(ByVal aliascmd As String)
        Dim FirstWordCmd As String = aliascmd.Split(" "c)(0)
        Dim actualCmd As String = aliascmd.Replace(FirstWordCmd, ZIPShellAliases(FirstWordCmd))
        Wdbg("I", "Actual command: {0}", actualCmd)
        ZipShell_CommandThread = New Thread(AddressOf ZipShell_ParseCommand) With {.Name = "ZIP Shell Command Thread"}
        ZipShell_CommandThread.Start(actualCmd)
        ZipShell_CommandThread.Join()
    End Sub

End Module
