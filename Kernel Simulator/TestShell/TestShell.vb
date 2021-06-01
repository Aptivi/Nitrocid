
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

Imports System.Threading

Module TestShell

    Public Test_ModCommands As New ArrayList
    Public Test_Commands As New Dictionary(Of String, CommandInfo) From {{"print", New CommandInfo("print", ShellCommandType.TestShell, True, 3)},
                                                                         {"printf", New CommandInfo("printf", ShellCommandType.TestShell, True, 4)},
                                                                         {"printd", New CommandInfo("printd", ShellCommandType.TestShell, True, 1)},
                                                                         {"printdf", New CommandInfo("printdf", ShellCommandType.TestShell, True, 2)},
                                                                         {"testevent", New CommandInfo("testevent", ShellCommandType.TestShell, True, 1)},
                                                                         {"probehw", New CommandInfo("probehw", ShellCommandType.TestShell, False, 0)},
                                                                         {"garbage", New CommandInfo("garbage", ShellCommandType.TestShell, False, 0)},
                                                                         {"panic", New CommandInfo("panic", ShellCommandType.TestShell, True, 4)},
                                                                         {"panicf", New CommandInfo("panicf", ShellCommandType.TestShell, True, 5)},
                                                                         {"translate", New CommandInfo("translate", ShellCommandType.TestShell, True, 2)},
                                                                         {"places", New CommandInfo("places", ShellCommandType.TestShell, True, 1)},
                                                                         {"testsha512", New CommandInfo("testsha512", ShellCommandType.TestShell, True, 1)},
                                                                         {"testsha256", New CommandInfo("testsha256", ShellCommandType.TestShell, True, 1)},
                                                                         {"testsha1", New CommandInfo("testsha1", ShellCommandType.TestShell, True, 1)},
                                                                         {"testmd5", New CommandInfo("testmd5", ShellCommandType.TestShell, True, 1)},
                                                                         {"testregexp", New CommandInfo("testregexp", ShellCommandType.TestShell, True, 2)},
                                                                         {"loadmods", New CommandInfo("loadmods", ShellCommandType.TestShell, True, 1)},
                                                                         {"debug", New CommandInfo("debug", ShellCommandType.TestShell, True, 1)},
                                                                         {"rdebug", New CommandInfo("rdebug", ShellCommandType.TestShell, True, 1)},
                                                                         {"colortest", New CommandInfo("colortest", ShellCommandType.TestShell, True, 1)},
                                                                         {"colortruetest", New CommandInfo("colortruetest", ShellCommandType.TestShell, True, 1)},
                                                                         {"sendnot", New CommandInfo("sendnot", ShellCommandType.TestShell, True, 3)},
                                                                         {"dcalend", New CommandInfo("dcalend", ShellCommandType.TestShell, True, 1)},
                                                                         {"listcodepages", New CommandInfo("listcodepages", ShellCommandType.TestShell, False, 0)},
                                                                         {"lscompilervars", New CommandInfo("lscompilervars", ShellCommandType.TestShell, False, 0)},
                                                                         {"testdictwriterstr", New CommandInfo("testdictwriterstr", ShellCommandType.TestShell, False, 0)},
                                                                         {"testdictwriterint", New CommandInfo("testdictwriterint", ShellCommandType.TestShell, False, 0)},
                                                                         {"testdictwriterchar", New CommandInfo("testdictwriterchar", ShellCommandType.TestShell, False, 0)},
                                                                         {"testlistwriterstr", New CommandInfo("testlistwriterstr", ShellCommandType.TestShell, False, 0)},
                                                                         {"testlistwriterint", New CommandInfo("testlistwriterint", ShellCommandType.TestShell, False, 0)},
                                                                         {"testlistwriterchar", New CommandInfo("testlistwriterchar", ShellCommandType.TestShell, False, 0)},
                                                                         {"lscultures", New CommandInfo("lscultures", ShellCommandType.TestShell, False, 0)},
                                                                         {"getcustomsaversetting", New CommandInfo("getcustomsaversetting", ShellCommandType.TestShell, True, 2)},
                                                                         {"setcustomsaversetting", New CommandInfo("setcustomsaversetting", ShellCommandType.TestShell, True, 3)},
                                                                         {"help", New CommandInfo("help", ShellCommandType.TestShell, False, 0)},
                                                                         {"exit", New CommandInfo("exit", ShellCommandType.TestShell, False, 0)},
                                                                         {"shutdown", New CommandInfo("shutdown", ShellCommandType.TestShell, False, 0)}}
    Public Test_ExitFlag As Boolean
    Public Test_ShutdownFlag As Boolean

    ''' <summary>
    ''' Initiates the test shell
    ''' </summary>
    Sub InitTShell()
        Dim FullCmd As String
        AddHandler Console.CancelKeyPress, AddressOf TCancelCommand

        While Not Test_ExitFlag
            If Not IsNothing(DefConsoleOut) Then
                Console.SetOut(DefConsoleOut)
            End If
            W("(t)> ", False, ColTypes.Input)
            FullCmd = Console.ReadLine
            Try
                If Not (FullCmd = Nothing Or FullCmd?.StartsWith(" ") = True) Then
                    Wdbg("I", "Command: {0}", FullCmd)
                    Dim Command As String = FullCmd.Split(" ")(0)
                    If Test_Commands.ContainsKey(Command) Then
                        TStartCommandThread = New Thread(AddressOf TParseCommand) With {.Name = "Test Shell Command Thread"}
                        TStartCommandThread.Start(FullCmd)
                        TStartCommandThread.Join()
                    ElseIf Test_ModCommands.Contains(Command) Then
                        Wdbg("I", "Mod command found.")
                        ExecuteModCommand(FullCmd)
                    ElseIf TestShellAliases.Keys.Contains(Command) Then
                        Wdbg("I", "Test shell alias command found.")
                        ExecuteTestAlias(FullCmd)
                    Else
                        W(DoTranslation("Command {0} not found. See the ""help"" command for the list of commands."), True, ColTypes.Error, Command)
                    End If
                Else
                    Thread.Sleep(30) 'This is to fix race condition between test shell initialization and starting the event handler thread
                End If
            Catch ex As Exception
                W(DoTranslation("Error in test shell: {0}"), True, ColTypes.Error, ex.Message)
                Wdbg("E", "Error: {0}", ex.Message)
                WStkTrc(ex)
            End Try
        End While
    End Sub

    ''' <summary>
    ''' Executes the test shell alias
    ''' </summary>
    ''' <param name="aliascmd">Aliased command with arguments</param>
    Sub ExecuteTestAlias(ByVal aliascmd As String)
        Dim FirstWordCmd As String = aliascmd.Split(" "c)(0)
        Dim actualCmd As String = aliascmd.Replace(FirstWordCmd, TestShellAliases(FirstWordCmd))
        Wdbg("I", "Actual command: {0}", actualCmd)
        TStartCommandThread = New Thread(AddressOf TParseCommand) With {.Name = "Test Shell Command Thread"}
        TStartCommandThread.Start(actualCmd)
        TStartCommandThread.Join()
    End Sub

End Module
