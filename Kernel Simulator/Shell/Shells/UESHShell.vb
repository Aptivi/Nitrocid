
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

Public Class UESHShell
    Inherits ShellExecutor
    Implements IShell

    Public Overrides ReadOnly Property ShellType As ShellCommandType Implements IShell.ShellType
        Get
            Return ShellCommandType.Shell
        End Get
    End Property

    Public Overrides Property Bail As Boolean Implements IShell.Bail

    Public Overrides Sub InitializeShell(ParamArray ShellArgs() As Object) Implements IShell.InitializeShell
        'Let CTRL+C cancel running command
        SwitchCancellationHandler(ShellCommandType.Shell)

        While Not Bail
            SyncLock CancelSync
                If LogoutRequested Then
                    Wdbg(DebugLevel.I, "Requested log out: {0}", LogoutRequested)
                    LogoutRequested = False
                    LoggedIn = False
                    Bail = True
                ElseIf Not InSaver Then
                    Try
                        'Try to probe injected commands
                        Wdbg(DebugLevel.I, "Probing injected commands using GetLine(True)...")
                        GetLine(True, "")

                        'Enable cursor (We put it here to avoid repeated "CursorVisible = True" statements in different command codes)
                        Console.CursorVisible = True

                        'Write a prompt
                        If DefConsoleOut IsNot Nothing Then
                            Console.SetOut(DefConsoleOut)
                        End If
                        CommandPromptWrite()
                        DisposeAll()

                        'Set an input color
                        Wdbg(DebugLevel.I, "ColoredShell is {0}", ColoredShell)
                        SetInputColor()

                        'Wait for command
                        Wdbg(DebugLevel.I, "Waiting for command")
                        KernelEventManager.RaiseShellInitialized()
                        Dim strcommand As String = Console.ReadLine()

                        If Not InSaver Then
                            'Fire event of PreRaiseCommand
                            KernelEventManager.RaisePreExecuteCommand(strcommand)

                            'Check for a type of command
                            If Not (strcommand = Nothing Or strcommand?.StartsWith(" ") = True) Then
                                Dim Done As Boolean = False
                                Dim Commands As String() = strcommand.Split({" : "}, StringSplitOptions.RemoveEmptyEntries)
                                For Each Command As String In Commands
                                    Dim Parts As String() = Command.SplitEncloseDoubleQuotes(" ")
                                    Wdbg(DebugLevel.I, "Mod commands probing started with {0} from {1}", Command, strcommand)
                                    If ModCommands.Contains(Parts(0)) Then
                                        Done = True
                                        Wdbg(DebugLevel.I, "Mod command: {0}", Parts(0))
                                        ExecuteModCommand(Command)
                                    End If
                                    Wdbg(DebugLevel.I, "Aliases probing started with {0} from {1}", Command, strcommand)
                                    If Aliases.Keys.Contains(Parts(0)) Then
                                        Done = True
                                        Wdbg(DebugLevel.I, "Alias: {0}", Parts(0))
                                        ExecuteAlias(Command)
                                    End If
                                    If Done = False Then
                                        Wdbg(DebugLevel.I, "Executing built-in command")
                                        GetLine(False, Command)
                                    End If
                                Next

                                'Fire an event of PostExecuteCommand
                                KernelEventManager.RaisePostExecuteCommand(strcommand)
                            End If
                        End If
                    Catch ex As Exception
                        WStkTrc(ex)
                        Write(DoTranslation("There was an error in the shell.") + vbNewLine + "Error {0}: {1}", True, ColTypes.Error, ex.GetType.FullName, ex.Message)
                        Continue While
                    End Try
                End If
            End SyncLock
        End While
    End Sub

End Class
