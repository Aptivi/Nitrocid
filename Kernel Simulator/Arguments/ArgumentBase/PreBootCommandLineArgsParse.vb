
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

Imports KS.Arguments.PreBootCommandLineArguments

Namespace Arguments.ArgumentBase
    Module PreBootCommandLineArgsParse

        Public AvailablePreBootCMDLineArgs As New Dictionary(Of String, ArgumentInfo) From {{"reset", New ArgumentInfo("reset", ArgumentType.PreBootCommandLineArgs, "Resets the kernel to the factory settings", "", False, 0, New PreBootCommandLine_ResetArgument)},
                                                                                        {"bypasssizedetection", New ArgumentInfo("bypasssizedetection", ArgumentType.PreBootCommandLineArgs, "Bypasses the console size detection", "", False, 0, New PreBootCommandLine_BypassSizeDetectionArgument)},
                                                                                        {"linuxcompatibility", New ArgumentInfo("linuxcompatibility", ArgumentType.PreBootCommandLineArgs, "Boots Kernel Simulator in Linux compatibility mode (only for Windows consoles other than Command Prompt)", "", False, 0, New PreBootCommandLine_LinuxCompatibility)}}

        ''' <summary>
        ''' Parses the pre-boot command line arguments
        ''' </summary>
        ''' <param name="Args">Arguments</param>
        Sub ParsePreBootCMDArguments(Args As String())
            Try
                If Args.Length <> 0 Then
                    For Each Argument As String In Args
                        If AvailablePreBootCMDLineArgs.Keys.Contains(Argument) Then
                            'Variables
                            Dim ArgumentInfo As New ProvidedArgumentArgumentsInfo(Argument, ArgumentType.PreBootCommandLineArgs)
                            Dim FullArgs() As String = ArgumentInfo.FullArgumentsList
                            Dim ArgsList() As String = ArgumentInfo.ArgumentsList
                            Dim Switches() As String = ArgumentInfo.SwitchesList
                            Dim strArgs As String = ArgumentInfo.ArgumentsText
                            Dim RequiredArgumentsProvided As Boolean = ArgumentInfo.RequiredArgumentsProvided

                            'If there are enough arguments provided, execute. Otherwise, fail with not enough arguments.
                            If (AvailablePreBootCMDLineArgs(Argument).ArgumentsRequired And RequiredArgumentsProvided) Or Not AvailablePreBootCMDLineArgs(Argument).ArgumentsRequired Then
                                Dim ArgumentBase As ArgumentExecutor = AvailablePreBootCMDLineArgs(Argument).ArgumentBase
                                ArgumentBase.Execute(strArgs, FullArgs, Args, Switches)
                            Else
                                Wdbg(DebugLevel.W, "User hasn't provided enough arguments for {0}", Argument)
                                Write(DoTranslation("There was not enough arguments."), True, ColTypes.Neutral)
                            End If
                        End If
                    Next
                End If
            Catch ex As Exception
                Write(DoTranslation("Error while parsing pre-boot command-line arguments: {0}") + vbNewLine + "{1}", True, ColTypes.Error, ex.Message, ex.StackTrace)
            End Try
        End Sub

    End Module
End Namespace