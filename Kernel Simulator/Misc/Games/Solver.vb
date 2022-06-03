
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

            'Show tip to exit
            Write(DoTranslation("Press ""q"" to exit."), True, ColTypes.Neutral)
            Wdbg(DebugLevel.I, "Initialized expressions.")
            While True
                'Populate the numbers
                Dim FirstNumber As Integer = RandomDriver.Next(SolverMinimumNumber, SolverMaximumNumber)
                Dim OperationIndex As Integer = RandomDriver.Next(Operations.Length)
                Dim SecondNumber As Integer = RandomDriver.Next(SolverMinimumNumber, SolverMaximumNumber)

                'Generate the expression
                RandomExpression = CStr(FirstNumber) + Operations.ElementAt(OperationIndex) + CStr(SecondNumber)
                Wdbg(DebugLevel.I, "Expression to be solved: {0}", RandomExpression)
                Write(RandomExpression, True, ColTypes.Input)

                'Wait for response
                UserEvaluated = If(SolverShowInput, ReadLine(), ReadLineNoInput(""))
                Wdbg(DebugLevel.I, "Evaluated: {0}", UserEvaluated)

                'Check to see if the user has entered the correct answer
                Dim UserEvaluatedNumber As Double
                Dim EvaluatedNumber As Double = New DataTable().Compute(RandomExpression, Nothing)
                If Double.TryParse(UserEvaluated, UserEvaluatedNumber) Then
                    If UserEvaluatedNumber = EvaluatedNumber Then
                        Wdbg(DebugLevel.I, "Expression is {0} and equals {1}", UserEvaluated, EvaluatedNumber)
                        Write(DoTranslation("Solved perfectly!"), True, ColTypes.Neutral)
                    Else
                        Wdbg(DebugLevel.I, "Expression is {0} and equals {1}", UserEvaluated, EvaluatedNumber)
                        Write(DoTranslation("Solved incorrectly."), True, ColTypes.Neutral)
                    End If
                ElseIf UserEvaluated = "q" Then
                    Wdbg(DebugLevel.W, "User requested exit.")
                    Exit While
                ElseIf ReadLineReboot.ReadLine.ReadRanToCompletion Then
                    Wdbg(DebugLevel.E, "User evaluated ""{0}"". However, it's not numeric.", UserEvaluated)
                    Write(DoTranslation("You can only write the numbers."), True, ColTypes.Error)
                End If
            End While
        End Sub

    End Module
End Namespace