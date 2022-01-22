
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

Namespace Misc.Games
    Public Module Solver

        Public SolverMinimumNumber As Integer = 0
        Public SolverMaximumNumber As Integer = 1000
        Public SolverShowInput As Boolean

        ''' <summary>
        ''' Initializes the game
        ''' </summary>
        Sub InitializeSolver()
            Dim RandomDriver As New Random
            Dim RandomExpression As String
            Dim UserEvaluated As String
            Dim Operations() As String = {"+", "-", "*", "/"}
            Write(DoTranslation("Press CTRL+C to exit."), True, ColTypes.Neutral)
            Wdbg(DebugLevel.I, "Initialized expressions.")
            While True
                RandomExpression = CStr(RandomDriver.Next(SolverMinimumNumber, SolverMaximumNumber)) + Operations.ElementAt(RandomDriver.Next(Operations.Length)) + CStr(RandomDriver.Next(SolverMinimumNumber, SolverMaximumNumber))
                Wdbg(DebugLevel.I, "Expression to be solved: {0}", RandomExpression)
                Write(RandomExpression, True, ColTypes.Input)
                UserEvaluated = If(SolverShowInput, Console.ReadLine(), ReadLineNoInput(""))
                Wdbg(DebugLevel.I, "Evaluated: {0}", UserEvaluated)
                If CDbl(UserEvaluated) = New DataTable().Compute(RandomExpression, Nothing) Then
                    Wdbg(DebugLevel.I, "Expression is {0} and equals {1}", UserEvaluated, New DataTable().Compute(RandomExpression, Nothing))
                    Write(DoTranslation("Solved perfectly!"), True, ColTypes.Neutral)
                Else
                    Wdbg(DebugLevel.I, "Expression is {0} and equals {1}", UserEvaluated, New DataTable().Compute(RandomExpression, Nothing))
                    Write(DoTranslation("Solved incorrectly."), True, ColTypes.Neutral)
                End If
            End While
        End Sub

    End Module
End Namespace