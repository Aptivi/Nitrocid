
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

    ''' <summary>
    ''' Parses specified arguments
    ''' </summary>
    Public Sub ParseArguments()
        'Check for the arguments written by the user
        Try
            For i As Integer = 0 To EnteredArguments.Count - 1
                Dim Argument As String = EnteredArguments(i)
                If AvailableArgs.Keys.Contains(Argument) Then
                    'Variables
                    Dim ArgumentInfo As New ProvidedArgumentArgumentsInfo(Argument, ArgumentType.KernelArgs)
                    Dim FullArgs() As String = ArgumentInfo.FullArgumentsList
                    Dim Args() As String = ArgumentInfo.ArgumentsList
                    Dim Switches() As String = ArgumentInfo.SwitchesList
                    Dim strArgs As String = ArgumentInfo.ArgumentsText
                    Dim RequiredArgumentsProvided As Boolean = ArgumentInfo.RequiredArgumentsProvided

                    'If there are enough arguments provided, execute. Otherwise, fail with not enough arguments.
                    If (AvailableArgs(Argument).ArgumentsRequired And RequiredArgumentsProvided) Or Not AvailableArgs(Argument).ArgumentsRequired Then
                        Dim ArgumentBase As ArgumentExecutor = AvailableArgs(Argument).ArgumentBase
                        ArgumentBase.Execute(strArgs, FullArgs, Args, Switches)
                    Else
                        Wdbg(DebugLevel.W, "User hasn't provided enough arguments for {0}", Argument)
                        Write(DoTranslation("There was not enough arguments."), True, ColTypes.Neutral)
                    End If
                Else
                    Write(DoTranslation("The requested argument {0} is not found."), True, ColTypes.Error, Argument)
                End If
            Next
        Catch ex As Exception
            KernelError(KernelErrorLevel.U, True, 5, DoTranslation("Unrecoverable error in argument:") + " " + ex.Message, ex)
        End Try
    End Sub

End Module
