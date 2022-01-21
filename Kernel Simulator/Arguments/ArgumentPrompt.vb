
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

Imports KS.Arguments.ArgumentBase
Imports KS.Arguments.KernelArguments

Namespace Arguments
    Module ArgumentPrompt

        'Variables
        Public EnteredArguments As New List(Of String)
        Public AvailableArgs As New Dictionary(Of String, ArgumentInfo) From {{"quiet", New ArgumentInfo("quiet", ArgumentType.KernelArgs, "Starts the kernel quietly", "", False, 0, New QuietArgument)},
                                                                          {"cmdinject", New ArgumentInfo("cmdinject", ArgumentType.KernelArgs, "Injects a command to start up in the next login", "[commands]", False, 0, New CmdInjectArgument)},
                                                                          {"debug", New ArgumentInfo("debug", ArgumentType.KernelArgs, "Enables debug mode", "", False, 0, New DebugArgument)},
                                                                          {"maintenance", New ArgumentInfo("maintenance", ArgumentType.KernelArgs, "Like safe mode, but also disables multi-user and some customization", "", False, 0, New MaintenanceArgument)},
                                                                          {"safe", New ArgumentInfo("safe", ArgumentType.KernelArgs, "Starts the kernel in safe mode, disabling all mods", "", False, 0, New SafeArgument)},
                                                                          {"testInteractive", New ArgumentInfo("testInteractive", ArgumentType.KernelArgs, "Opens a test shell", "", False, 0, New TestInteractiveArgument)}}

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
                AnswerArgs = Console.ReadLine()

                'Add an argument to the entered arguments list
                If AnswerArgs <> "q" Then
                    For Each AnswerArg As String In AnswerArgs.Split(","c)
                        If AvailableArgs.Keys.Contains(AnswerArg.Split(" "c)(0)) Then
                            EnteredArguments.Add(AnswerArg)
                        ElseIf Not String.IsNullOrWhiteSpace(AnswerArg.Split(" "c)(0)) Then
                            Write(DoTranslation("The requested argument {0} is not found."), True, ColTypes.Error, AnswerArg.Split(" "c)(0))
                        End If
                    Next
                Else
                    If InjMode Then
                        ArgsInjected = True
                        Kernel.Kernel.KernelEventManager.RaiseArgumentsInjected(EnteredArguments)
                        Write(DoTranslation("Injected arguments will be scheduled to run at next reboot."), True, ColTypes.Neutral)
                    ElseIf EnteredArguments.Count <> 0 Then
                        Write(DoTranslation("Starting the kernel with:") + " {0}", True, ColTypes.Neutral, String.Join(", ", EnteredArguments))
                        ParseArguments()
                    End If
                End If
            End While
        End Sub

    End Module
End Namespace