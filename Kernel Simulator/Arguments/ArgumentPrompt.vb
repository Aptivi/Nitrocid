
'    Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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

Imports KS.Arguments.ArgumentBase

Namespace Arguments
    Public Module ArgumentPrompt

        'Variables
        Friend EnteredArguments As New List(Of String)

        ''' <summary>
        ''' Prompts user for arguments
        ''' </summary>
        ''' <param name="InjMode">Argument injection mode (usually by "arginj" command)</param>
        Sub PromptArgs(Optional InjMode As Boolean = False)
            'Checks if the arguments are injected
            Dim AnswerArgs As String = ""

            'Shows available arguments
            Write(DoTranslation("Available kernel arguments:"), True, ColTypes.ListTitle)
            ShowArgsHelp(ArgumentType.KernelArgs)
            Console.WriteLine()
            Write("* " + DoTranslation("Press ""q"" if you're done."), True, ColTypes.Tip)
            Write("* " + DoTranslation("Multiple kernel arguments can be separated with commas without spaces, for example:") + " ""debug,safe""", True, ColTypes.Tip)
            Write("* " + DoTranslation("Multiple injected commands can be separated with colons with spaces, for example:") + " cmdinject ""beep 100 500 : echo Hello!""", True, ColTypes.Tip)

            'Prompts for the arguments
            While Not AnswerArgs = "q"
                Write(">> ", False, ColTypes.Input)
                AnswerArgs = ReadLine()

                'Add an argument to the entered arguments list
                If AnswerArgs <> "q" Then
                    For Each AnswerArg As String In AnswerArgs.Split(","c)
                        If AvailableArgs.ContainsKey(AnswerArg.Split(" "c)(0)) Then
                            EnteredArguments.Add(AnswerArg)
                        ElseIf Not String.IsNullOrWhiteSpace(AnswerArg.Split(" "c)(0)) Then
                            Write(DoTranslation("The requested argument {0} is not found."), True, ColTypes.Error, AnswerArg.Split(" "c)(0))
                        End If
                    Next
                Else
                    If InjMode Then
                        ArgsInjected = True
                        KernelEventManager.RaiseArgumentsInjected(EnteredArguments)
                        Write(DoTranslation("Injected arguments will be scheduled to run at next reboot."), True, ColTypes.Neutral)
                    ElseIf EnteredArguments.Count <> 0 Then
                        Write(DoTranslation("Starting the kernel with:") + " {0}", True, ColTypes.Neutral, String.Join(", ", EnteredArguments))
                        ParseArguments(EnteredArguments, ArgumentType.KernelArgs)
                    End If
                End If
            End While
        End Sub

    End Module
End Namespace