
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

Module ArgumentPrompt

    'Variables
    Public EnteredArguments As New List(Of String)
    Public AvailableArgs() As String = {"quiet", "cmdinject", "debug", "maintenance", "safe", "testInteractive"}

    ''' <summary>
    ''' Prompts user for arguments
    ''' </summary>
    ''' <param name="InjMode">Argument injection mode (usually by "arginj" command)</param>
    Sub PromptArgs(Optional ByVal InjMode As Boolean = False)
        'Checks if the arguments are injected
        Dim AnswerArgs As String = ""

        'Shows available arguments
        W(DoTranslation("Available arguments: {0}") + vbNewLine +
          DoTranslation("'q' to quit."), True, ColTypes.Neutral, String.Join(", ", AvailableArgs))
        While Not AnswerArgs = "q"
            'Prompts for the arguments
            W(DoTranslation("Arguments ('help' for help): "), False, ColTypes.Input, String.Join(", ", AvailableArgs))
            AnswerArgs = Console.ReadLine()

            'Add an argument to the entered arguments list
            If AnswerArgs <> "q" Then
                For Each AnswerArg As String In AnswerArgs.Split(","c)
                    If AnswerArg.Contains("help") Then
                        W(DoTranslation("Separate boot arguments with commas without spaces, for example, 'motd,gpuprobe'") + vbNewLine +
                          DoTranslation("Separate commands on 'cmdinject' with colons with spaces, for example, 'cmdinject setthemes Hacker : beep 1024 0.5'") + vbNewLine +
                          DoTranslation("Note that the 'debug' argument does not fully cover the kernel."), True, ColTypes.Neutral)
                        Exit For
                    Else
                        EnteredArguments.Add(AnswerArg)
                    End If
                Next
            Else
                If InjMode Then
                    argsInjected = True
                    EventManager.RaiseArgumentsInjected(EnteredArguments)
                    W(DoTranslation("Injected arguments will be scheduled to run at next reboot."), True, ColTypes.Neutral)
                Else
                    W(DoTranslation("Starting the kernel with:") + " {0}", True, ColTypes.Neutral, String.Join(", ", EnteredArguments))
                    ParseArguments()
                End If
            End If
        End While
    End Sub

End Module
