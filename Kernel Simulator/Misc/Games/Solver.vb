
'    Kernel Simulator  Copyright (C) 2018-2020  EoflaOE
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

Module Solver

    ''' <summary>
    ''' Initializes the game
    ''' </summary>
    Sub InitializeSolver()
        Dim RandomDriver As New Random
        Dim RandomExpression As String
        Dim UserEvaluated As String = ""
        Dim Operations() As String = {"+", "-", "*", "/"}
        Wdbg("I", "Initialized expressions.")
        W(DoTranslation("Write ""GetMeOut"" to exit.", currentLang), True, ColTypes.Neutral)
        While True
            RandomExpression = CStr(RandomDriver.Next(1000)) + Operations.ElementAt(RandomDriver.Next(Operations.Count)) + CStr(RandomDriver.Next(1000))
            Wdbg("I", "Expression to be solved: {0}", RandomExpression)
            W(RandomExpression, True, ColTypes.Input)
            UserEvaluated = ReadLineNoInput("")
            Wdbg("I", "Evaluated: {0}", UserEvaluated)
            If UserEvaluated = "GetMeOut" Then
                Wdbg("I", "Exit")
                Exit Sub
            ElseIf CDbl(UserEvaluated) = New DataTable().Compute(RandomExpression, Nothing) Then
                Wdbg("I", "Expression is {0} and equals {1}", UserEvaluated, New DataTable().Compute(RandomExpression, Nothing))
                W(DoTranslation("Solved perfectly!", currentLang), True, ColTypes.Neutral)
            Else
                Wdbg("I", "Expression is {0} and equals {1}", UserEvaluated, New DataTable().Compute(RandomExpression, Nothing))
                W(DoTranslation("Solved incorrectly.", currentLang), True, ColTypes.Neutral)
            End If
            UserEvaluated = ""
        End While
    End Sub

End Module
