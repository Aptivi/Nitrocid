
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

    Public TShellCmds As String() = {"print", "printf", "printd", "printdf", "testevent", "probehw", "garbage", "panic", "panicf", "translate", "places", "loadmods",
                                     "debug", "rdebug", "testmd5", "testsha256", "testsha1", "testregexp", "colortest", "colortruetest", "sendnot", "dcalend",
                                     "listcodepages", "help", "exit", "shutdown", "testsha512"}
    Public TEST_ExitFlag As Boolean
    Public TEST_ShutdownFlag As Boolean

    ''' <summary>
    ''' Initiates the test shell
    ''' </summary>
    Sub InitTShell()
        Dim FullCmd As String
        AddHandler Console.CancelKeyPress, AddressOf TCancelCommand

        While Not TEST_ExitFlag
            If Not IsNothing(DefConsoleOut) Then
                Console.SetOut(DefConsoleOut)
            End If
            W("(t)> ", False, ColTypes.Input)
            FullCmd = Console.ReadLine
            Try
                If Not (FullCmd = Nothing Or FullCmd?.StartsWith(" ") = True) Then
                    Wdbg("I", "Command: {0}", FullCmd)
                    If TShellCmds.Contains(FullCmd.Split(" ")(0)) Then
                        TStartCommandThread = New Thread(AddressOf TParseCommand)
                        TStartCommandThread.Start(FullCmd)
                        TStartCommandThread.Join()
                    Else
                        W(DoTranslation("Command {0} not found. See the ""help"" command for the list of commands.", currentLang), True, ColTypes.Err, FullCmd.Split(" ")(0))
                    End If
                Else
                    Console.WriteLine()
                    Thread.Sleep(30) 'This is to fix race condition between test shell initialization and starting the event handler thread
                End If
            Catch ex As Exception
                W(DoTranslation("Error in unit testing: {0}"), True, ColTypes.Err, ex.Message)
                Wdbg("E", "Error: {0}", ex.Message)
                WStkTrc(ex)
            End Try
        End While
    End Sub

End Module
