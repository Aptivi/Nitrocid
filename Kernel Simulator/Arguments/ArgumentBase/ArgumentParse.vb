
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

Imports KS.Arguments.KernelArguments

Namespace Arguments.ArgumentBase
    Public Module ArgumentParse

        Public ReadOnly AvailableArgs As New Dictionary(Of String, ArgumentInfo) From {
            {"quiet", New ArgumentInfo("quiet", ArgumentType.KernelArgs, "Starts the kernel quietly", "", False, 0, New QuietArgument)},
            {"cmdinject", New ArgumentInfo("cmdinject", ArgumentType.KernelArgs, "Injects a command to start up in the next login", "[commands]", False, 0, New CmdInjectArgument)},
            {"debug", New ArgumentInfo("debug", ArgumentType.KernelArgs, "Enables debug mode", "", False, 0, New DebugArgument)},
            {"maintenance", New ArgumentInfo("maintenance", ArgumentType.KernelArgs, "Like safe mode, but also disables multi-user and some customization", "", False, 0, New MaintenanceArgument)},
            {"safe", New ArgumentInfo("safe", ArgumentType.KernelArgs, "Starts the kernel in safe mode, disabling all mods", "", False, 0, New SafeArgument)},
            {"testInteractive", New ArgumentInfo("testInteractive", ArgumentType.KernelArgs, "Opens a test shell", "", False, 0, New TestInteractiveArgument)}
        }

        ''' <summary>
        ''' Parses specified arguments
        ''' </summary>
        ''' <param name="ArgumentsInput">Input Arguments</param>
        ''' <param name="ArgumentType">Argument type</param>
        Public Sub ParseArguments(ArgumentsInput As List(Of String), ArgumentType As ArgumentType)
            'Check for the arguments written by the user
            Try
                'Select the argument dictionary
                Dim Arguments As Dictionary(Of String, ArgumentInfo) = AvailableArgs
                Select Case ArgumentType
                    Case ArgumentType.PreBootCommandLineArgs
                        Arguments = AvailablePreBootCMDLineArgs
                    Case ArgumentType.CommandLineArgs
                        Arguments = AvailableCMDLineArgs
                End Select

                'Parse them now
                For i As Integer = 0 To ArgumentsInput.Count - 1
                    Dim Argument As String = ArgumentsInput(i)
                    If Arguments.ContainsKey(Argument) Then
                        'Variables
                        Dim ArgumentInfo As New ProvidedArgumentArgumentsInfo(Argument, ArgumentType)
                        Dim FullArgs() As String = ArgumentInfo.FullArgumentsList
                        Dim Args() As String = ArgumentInfo.ArgumentsList
                        Dim Switches() As String = ArgumentInfo.SwitchesList
                        Dim strArgs As String = ArgumentInfo.ArgumentsText
                        Dim RequiredArgumentsProvided As Boolean = ArgumentInfo.RequiredArgumentsProvided

                        'If there are enough arguments provided, execute. Otherwise, fail with not enough arguments.
                        If (Arguments(Argument).ArgumentsRequired And RequiredArgumentsProvided) Or Not Arguments(Argument).ArgumentsRequired Then
                            Dim ArgumentBase As ArgumentExecutor = Arguments(Argument).ArgumentBase
                            ArgumentBase.Execute(strArgs, FullArgs, Args, Switches)
                        Else
                            Wdbg(DebugLevel.W, "User hasn't provided enough arguments for {0}", Argument)
                            Write(DoTranslation("There was not enough arguments."), True, ColTypes.Neutral)
                        End If
                    End If
                Next
            Catch ex As Exception
                KernelError(KernelErrorLevel.U, True, 5, DoTranslation("Unrecoverable error in argument:") + " " + ex.Message, ex)
            End Try
        End Sub

    End Module
End Namespace