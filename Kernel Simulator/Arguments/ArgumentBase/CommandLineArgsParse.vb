
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

Imports KS.Arguments.CommandLineArguments

Namespace Arguments.ArgumentBase
    Module CommandLineArgsParse

        Public AvailableCMDLineArgs As New Dictionary(Of String, ArgumentInfo) From {{"testInteractive", New ArgumentInfo("testInteractive", ArgumentType.CommandLineArgs, "Opens a test shell", "", False, 0, New CommandLine_TestInteractiveArgument)},
                                                                                     {"debug", New ArgumentInfo("debug", ArgumentType.CommandLineArgs, "Enables debug mode", "", False, 0, New CommandLine_DebugArgument)},
                                                                                     {"args", New ArgumentInfo("args", ArgumentType.CommandLineArgs, "Prompts for arguments", "", False, 0, New CommandLine_ArgsArgument)},
                                                                                     {"help", New ArgumentInfo("help", ArgumentType.CommandLineArgs, "Help page", "", False, 0, New CommandLine_HelpArgument)}}

        ''' <summary>
        ''' Parses the command line arguments
        ''' </summary>
        ''' <param name="Args">Arguments</param>
        Sub ParseCMDArguments(Args As String())
            Try
                If Args.Length <> 0 Then
                    For Each Argument As String In Args
                        If AvailableCMDLineArgs.Keys.Contains(Argument) Then
                            'Variables
                            Dim ArgumentInfo As New ProvidedArgumentArgumentsInfo(Argument, ArgumentType.CommandLineArgs)
                            Dim FullArgs() As String = ArgumentInfo.FullArgumentsList
                            Dim ArgsList() As String = ArgumentInfo.ArgumentsList
                            Dim Switches() As String = ArgumentInfo.SwitchesList
                            Dim strArgs As String = ArgumentInfo.ArgumentsText
                            Dim RequiredArgumentsProvided As Boolean = ArgumentInfo.RequiredArgumentsProvided

                            'If there are enough arguments provided, execute. Otherwise, fail with not enough arguments.
                            If (AvailableCMDLineArgs(Argument).ArgumentsRequired And RequiredArgumentsProvided) Or Not AvailableCMDLineArgs(Argument).ArgumentsRequired Then
                                Dim ArgumentBase As ArgumentExecutor = AvailableCMDLineArgs(Argument).ArgumentBase
                                ArgumentBase.Execute(strArgs, FullArgs, Args, Switches)
                            Else
                                Wdbg(DebugLevel.W, "User hasn't provided enough arguments for {0}", Argument)
                                Write(DoTranslation("There was not enough arguments."), True, ColTypes.Neutral)
                            End If
                        ElseIf Not AvailablePreBootCMDLineArgs.Keys.Contains(Argument) Then
                            Write(DoTranslation("Command line argument {0} not found."), True, ColTypes.Error, Argument)
                        End If
                    Next
                End If
            Catch ex As Exception
                Write(DoTranslation("Error while parsing real command-line arguments: {0}") + vbNewLine + "{1}", True, ColTypes.Error, ex.Message, ex.StackTrace)
                If Args.Contains("testMod") Then Environment.Exit(1)
            End Try
        End Sub

    End Module
End Namespace