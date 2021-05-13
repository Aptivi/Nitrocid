
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
    Public Test_Commands As New Dictionary(Of String, CommandInfo) From {{"print", New CommandInfo("print", ShellCommandType.TestShell, True, 3, False, False, False, False)},
                                                                         {"printf", New CommandInfo("printf", ShellCommandType.TestShell, True, 4, False, False, False, False)},
                                                                         {"printd", New CommandInfo("printd", ShellCommandType.TestShell, True, 1, False, False, False, False)},
                                                                         {"printdf", New CommandInfo("printdf", ShellCommandType.TestShell, True, 2, False, False, False, False)},
                                                                         {"testevent", New CommandInfo("testevent", ShellCommandType.TestShell, True, 1, False, False, False, False)},
                                                                         {"probehw", New CommandInfo("probehw", ShellCommandType.TestShell, False, 0, False, False, False, False)},
                                                                         {"garbage", New CommandInfo("garbage", ShellCommandType.TestShell, False, 0, False, False, False, False)},
                                                                         {"panic", New CommandInfo("panic", ShellCommandType.TestShell, True, 4, False, False, False, False)},
                                                                         {"panicf", New CommandInfo("panicf", ShellCommandType.TestShell, True, 5, False, False, False, False)},
                                                                         {"translate", New CommandInfo("translate", ShellCommandType.TestShell, True, 2, False, False, False, False)},
                                                                         {"places", New CommandInfo("places", ShellCommandType.TestShell, True, 1, False, False, False, False)},
                                                                         {"testsha512", New CommandInfo("testsha512", ShellCommandType.TestShell, True, 1, False, False, False, False)},
                                                                         {"testsha256", New CommandInfo("testsha256", ShellCommandType.TestShell, True, 1, False, False, False, False)},
                                                                         {"testsha1", New CommandInfo("testsha1", ShellCommandType.TestShell, True, 1, False, False, False, False)},
                                                                         {"testmd5", New CommandInfo("testmd5", ShellCommandType.TestShell, True, 1, False, False, False, False)},
                                                                         {"testregexp", New CommandInfo("testregexp", ShellCommandType.TestShell, True, 2, False, False, False, False)},
                                                                         {"loadmods", New CommandInfo("loadmods", ShellCommandType.TestShell, True, 1, False, False, False, False)},
                                                                         {"debug", New CommandInfo("debug", ShellCommandType.TestShell, True, 1, False, False, False, False)},
                                                                         {"rdebug", New CommandInfo("rdebug", ShellCommandType.TestShell, True, 1, False, False, False, False)},
                                                                         {"colortest", New CommandInfo("colortest", ShellCommandType.TestShell, True, 1, False, False, False, False)},
                                                                         {"colortruetest", New CommandInfo("colortruetest", ShellCommandType.TestShell, True, 1, False, False, False, False)},
                                                                         {"sendnot", New CommandInfo("sendnot", ShellCommandType.TestShell, True, 3, False, False, False, False)},
                                                                         {"dcalend", New CommandInfo("dcalend", ShellCommandType.TestShell, True, 1, False, False, False, False)},
                                                                         {"listcodepages", New CommandInfo("listcodepages", ShellCommandType.TestShell, False, 0, False, False, False, False)},
                                                                         {"lscompilervars", New CommandInfo("lscompilervars", ShellCommandType.TestShell, False, 0, False, False, False, False)},
                                                                         {"testlistwriterstr", New CommandInfo("testlistwriterstr", ShellCommandType.TestShell, False, 0, False, False, False, False)},
                                                                         {"testlistwriterint", New CommandInfo("testlistwriterint", ShellCommandType.TestShell, False, 0, False, False, False, False)},
                                                                         {"testlistwriterchar", New CommandInfo("testlistwriterchar", ShellCommandType.TestShell, False, 0, False, False, False, False)},
                                                                         {"lscultures", New CommandInfo("lscultures", ShellCommandType.TestShell, False, 0, False, False, False, False)},
                                                                         {"help", New CommandInfo("help", ShellCommandType.TestShell, False, 0, False, False, False, False)},
                                                                         {"exit", New CommandInfo("exit", ShellCommandType.TestShell, False, 0, False, False, False, False)},
                                                                         {"shutdown", New CommandInfo("shutdown", ShellCommandType.TestShell, False, 0, False, False, False, False)}}
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
                    If Test_Commands.ContainsKey(FullCmd.Split(" ")(0)) Then
                        TStartCommandThread = New Thread(AddressOf TParseCommand) With {.Name = "Test Shell Command Thread"}
                        TStartCommandThread.Start(FullCmd)
                        TStartCommandThread.Join()
                    ElseIf Test_ModCommands.Contains(FullCmd.Split(" ")(0)) Then
                        Wdbg("I", "Mod command found.")
                        ExecuteModCommand(FullCmd)
                    Else
                        W(DoTranslation("Command {0} not found. See the ""help"" command for the list of commands."), True, ColTypes.Err, FullCmd.Split(" ")(0))
                    End If
                Else
                    Thread.Sleep(30) 'This is to fix race condition between test shell initialization and starting the event handler thread
                End If
            Catch ex As Exception
                W(DoTranslation("Error in test shell: {0}"), True, ColTypes.Err, ex.Message)
                Wdbg("E", "Error: {0}", ex.Message)
                WStkTrc(ex)
            End Try
        End While
    End Sub

End Module
