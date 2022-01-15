
'    Kernel Simulator  Copyright (C) 2018-2022  EoflaOE
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

Public Class TestShell
    Inherits ShellExecutor
    Implements IShell

    Public Overrides ReadOnly Property ShellType As ShellType Implements IShell.ShellType
        Get
            Return ShellType.TestShell
        End Get
    End Property

    Public Overrides Property Bail As Boolean Implements IShell.Bail

    Public Overrides Sub InitializeShell(ParamArray ShellArgs() As Object) Implements IShell.InitializeShell
        'Show the welcome message
        SwitchCancellationHandler(ShellType.TestShell)
        Console.WriteLine()
        WriteSeparator(DoTranslation("Welcome to Test Shell!"), True)

        'Actual shell logic
        While Not Bail
            SyncLock TestCancelSync
                If DefConsoleOut IsNot Nothing Then
                    Console.SetOut(DefConsoleOut)
                End If
                Wdbg(DebugLevel.I, "Test_PromptStyle = {0}", Test_PromptStyle)
                If Test_PromptStyle = "" Then
                    Write("(t)> ", False, ColTypes.Input)
                Else
                    Dim ParsedPromptStyle As String = ProbePlaces(Test_PromptStyle)
                    ParsedPromptStyle.ConvertVTSequences
                    Write(ParsedPromptStyle, False, ColTypes.Gray)
                End If
                Dim FullCmd As String = Console.ReadLine
                Try
                    If Not (FullCmd = Nothing Or FullCmd?.StartsWithAnyOf({" ", "#"}) = True) Then
                        Wdbg(DebugLevel.I, "Command: {0}", FullCmd)
                        Dim Command As String = FullCmd.SplitEncloseDoubleQuotes(" ")(0)
                        If Test_Commands.ContainsKey(Command) Then
                            Dim Params As New ExecuteCommandThreadParameters(FullCmd, ShellType.TestShell, Nothing)
                            TStartCommandThread = New Thread(AddressOf ExecuteCommand) With {.Name = "Test Shell Command Thread"}
                            TStartCommandThread.Start(Params)
                            TStartCommandThread.Join()
                        ElseIf Test_ModCommands.Contains(Command) Then
                            Wdbg(DebugLevel.I, "Mod command found.")
                            ExecuteModCommand(FullCmd)
                        ElseIf TestShellAliases.Keys.Contains(Command) Then
                            Wdbg(DebugLevel.I, "Test shell alias command found.")
                            FullCmd = FullCmd.Replace($"""{Command}""", Command)
                            ExecuteTestAlias(FullCmd)
                        Else
                            Write(DoTranslation("Command {0} not found. See the ""help"" command for the list of commands."), True, ColTypes.Error, Command)
                        End If
                    End If
                Catch ex As Exception
                    Write(DoTranslation("Error in test shell: {0}"), True, ColTypes.Error, ex.Message)
                    Wdbg(DebugLevel.E, "Error: {0}", ex.Message)
                    WStkTrc(ex)
                End Try
            End SyncLock
        End While

        'Restore the cancellation handler
        SwitchCancellationHandler(LastShellType)
    End Sub

End Class
