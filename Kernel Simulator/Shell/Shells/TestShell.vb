
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

Imports System.Threading
Imports KS.TestShell

Namespace Shell.Shells
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
            Console.WriteLine()
            WriteSeparator(DoTranslation("Welcome to Test Shell!"), True)

            'Actual shell logic
            While Not Bail
                If DefConsoleOut IsNot Nothing Then
                    Console.SetOut(DefConsoleOut)
                End If

                'Write the custom prompt style for the test shell
                Wdbg(DebugLevel.I, "Test_PromptStyle = {0}", Test_PromptStyle)
                If Test_PromptStyle = "" Then
                    Write("(t)> ", False, ColTypes.Input)
                Else
                    Dim ParsedPromptStyle As String = ProbePlaces(Test_PromptStyle)
                    ParsedPromptStyle.ConvertVTSequences
                    Write(ParsedPromptStyle, False, ColTypes.Gray)
                End If

                'Parse the command
                Dim FullCmd As String = Console.ReadLine
                Try
                    GetLine(FullCmd, False, "", ShellType.TestShell)
                Catch taex As ThreadAbortException
                    CancelRequested = False
                    Bail = True
                Catch ex As Exception
                    Write(DoTranslation("Error in test shell: {0}"), True, ColTypes.Error, ex.Message)
                    Wdbg(DebugLevel.E, "Error: {0}", ex.Message)
                    WStkTrc(ex)
                End Try
            End While
        End Sub

    End Class
End Namespace
