
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

Module ArgumentParse

    'Variables
    Public argcommands As String
    Public argcmds() As String

    ''' <summary>
    ''' Parses specified arguments
    ''' </summary>
    Public Sub ParseArguments()

        'Check for the arguments written by the user
        Try
            For i As Integer = 0 To EnteredArguments.Count - 1
                Dim indexArg As Integer = EnteredArguments(i).IndexOf(" ")
                If indexArg = -1 Then
                    indexArg = EnteredArguments(i).Length
                    EnteredArguments(i) = EnteredArguments(i).Substring(0, indexArg)
                End If
                If AvailableArgs.Contains(EnteredArguments(i).Substring(0, indexArg)) Then
                    If EnteredArguments(i) = "quiet" Then

                        DefConsoleOut = Console.Out
                        Console.SetOut(IO.StreamWriter.Null)

                    ElseIf EnteredArguments(i).Contains("cmdinject") Then

                        'Command Injector argument
                        If EnteredArguments(i) = "cmdinject" Then
                            W(DoTranslation("Available commands: {0}") + vbNewLine +
                              DoTranslation("Write command: "), False, ColTypes.Input, String.Join(", ", Commands.Keys))
                            argcmds = Console.ReadLine().Split({" : "}, StringSplitOptions.RemoveEmptyEntries)
                            argcommands = String.Join(", ", argcmds)
                            If argcommands <> "q" Then
                                CommandFlag = True
                            Else
                                W(DoTranslation("Command injection has been cancelled."), True, ColTypes.Neutral)
                            End If
                        Else
                            argcmds = EnteredArguments(i).Substring(10).Split({" : "}, StringSplitOptions.RemoveEmptyEntries)
                            argcommands = String.Join(", ", argcmds)
                            CommandFlag = True
                        End If

                    ElseIf EnteredArguments(i) = "debug" Then

                        DebugMode = True

                    ElseIf EnteredArguments(i) = "maintenance" Then

                        maintenance = True

                    ElseIf EnteredArguments(i) = "safe" Then

                        SafeMode = True

                    ElseIf EnteredArguments(i) = "testInteractive" Then

                        InitTShell()
                        If Test_ShutdownFlag Then Environment.Exit(0)

                    End If
                Else
                    W(DoTranslation("bargs: The requested argument {0} is not found."), True, ColTypes.Error, EnteredArguments(i).Substring(0, indexArg))
                End If
            Next
        Catch ex As Exception
            KernelError("U", True, 5, DoTranslation("bargs: Unrecoverable error in argument: ") + ex.Message, ex)
        End Try

    End Sub

End Module
