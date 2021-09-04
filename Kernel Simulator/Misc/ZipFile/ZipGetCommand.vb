
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

Public Module ZipGetCommand

    'Variables
    Public ZipShell_CommandThread As New Thread(AddressOf ZipShell_ParseCommand) With {.Name = "ZIP Shell Command Thread"}

    Sub ZipShell_ParseCommand(ByVal requestedCommand As String)
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
                RequiredArgumentsProvided = eqargs?.Length >= ZipShell_Commands(Command).MinimumArguments
            ElseIf ZipShell_Commands(Command).ArgumentsRequired And eqargs Is Nothing Then
                RequiredArgumentsProvided = False
            End If

            '4a. Debug: get all arguments from eqargs()
            If eqargs IsNot Nothing Then Wdbg("I", "Arguments parsed from eqargs(): " + String.Join(", ", eqargs))

            '5. Check to see if a requested command is obsolete
            If ZipShell_Commands(Command).Obsolete Then
                Wdbg("I", "The command requested {0} is obsolete", Command)
                W(DoTranslation("This command is obsolete and will be removed in a future release."), True, ColTypes.Neutral)
            End If

            '6. Execute a command
            Select Case Command
                Case "list"
                    Dim Entries As List(Of ZipArchiveEntry)
                    If eqargs?.Length > 0 Then
                        Wdbg("I", "Listing entries with {0} as target directory", eqargs(0))
                        Entries = ListZipEntries(eqargs(0))
                    Else
                        Wdbg("I", "Listing entries with current directory as target directory")
                        Entries = ListZipEntries(ZipShell_CurrentArchiveDirectory)
                    End If
                    For Each Entry As ZipArchiveEntry In Entries
                        W("- {0}: ", False, ColTypes.ListEntry, Entry.FullName)
                        If Not Entry.Name = "" Then 'Entry is a file
                            W("{0} ({1})", True, ColTypes.ListValue, Entry.CompressedLength.FileSizeToString, Entry.Length.FileSizeToString)
                        Else
                            Console.WriteLine()
                        End If
                    Next
                Case "get"
                    If RequiredArgumentsProvided Then
                        Dim Where As String = ""
                        Dim Absolute As Boolean
                        If eqargs?.Length > 1 Then
                            If Not eqargs(1) = "-absolute" Then Where = NeutralizePath(eqargs(1))
                            If eqargs?.Contains("-absolute") Then
                                Absolute = True
                            End If
                        End If
                        ExtractZipFileEntry(eqargs(0), Where, Absolute)
                    End If
                Case "chdir"
                    If RequiredArgumentsProvided Then
                        If Not ChangeWorkingZipLocalDirectory(eqargs(0)) Then
                            W(DoTranslation("Directory {0} doesn't exist"), True, ColTypes.Error, eqargs(0))
                        End If
                    End If
                Case "chadir"
                    If RequiredArgumentsProvided Then
                        If Not ChangeWorkingArchiveDirectory(eqargs(0)) Then
                            W(DoTranslation("Archive directory {0} doesn't exist"), True, ColTypes.Error, eqargs(0))
                        End If
                    End If
                Case "cdir"
                    W(ZipShell_CurrentDirectory, True, ColTypes.Neutral)
                Case "exit"
                    ZipShell_Exiting = True
                Case "help"
                    If eqargs?.Length > 0 Then
                        Wdbg("I", "Requested help for {0}", eqargs(0))
                        ShowHelp(eqargs(0), ShellCommandType.ZIPShell)
                    Else
                        Wdbg("I", "Requested help for all commands")
                        ShowHelp(ShellCommandType.ZIPShell)
                    End If
            End Select

            'See if the command is done (passed all required arguments)
            If ZipShell_Commands(Command).ArgumentsRequired And Not RequiredArgumentsProvided Then
                W(DoTranslation("Required arguments are not passed to command {0}"), True, ColTypes.Error, Command)
                Wdbg("E", "Passed arguments were not enough to run command {0}. Arguments passed: {1}", Command, eqargs?.Length)
                ShowHelp(Command, ShellCommandType.ZIPShell)
            End If
        Catch taex As ThreadAbortException
            Exit Sub
        Catch ex As Exception
            W(DoTranslation("Error trying to run command: {0}"), True, ColTypes.Error, ex.Message)
            Wdbg("E", "Error running command {0}: {1}", requestedCommand.Split(" ")(0), ex.Message)
            WStkTrc(ex)
            EventManager.RaiseZipCommandError(requestedCommand, ex)
        End Try
    End Sub

    Sub ZipShellCancelCommand(sender As Object, e As ConsoleCancelEventArgs)
        If e.SpecialKey = ConsoleSpecialKey.ControlC Then
            Console.WriteLine()
            DefConsoleOut = Console.Out
            Console.SetOut(StreamWriter.Null)
            e.Cancel = True
            ZipShell_CommandThread.Abort()
        End If
    End Sub

End Module
