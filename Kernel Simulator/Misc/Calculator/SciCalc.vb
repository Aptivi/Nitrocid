
'    Kernel Simulator  Copyright (C) 2018-2019  EoflaOE
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

Public Module SciCalc

    Public Sub ExpressionCalculate(ByVal sciMode As Boolean, ByVal ParamArray exps() As Object)

        Try
            If sciMode = False Then
                If exps.Count >= 3 Then
                    Dim expressions As New List(Of String)
                    Dim ops As New List(Of String)
                    Dim finalExp As String = ""
                    Dim numOps As Integer = 0
                    For i As Integer = 0 To exps.Count - 1 Step 2
                        If exps(i) = "pi" Then 'Add a "pi" = 3.14 constant value to our expression
                            expressions.Add(Math.PI)
                        ElseIf exps(i) = "e" Then 'Add an euler constant value to our expression
                            expressions.Add(Math.E)
                        Else
                            expressions.Add(exps(i))
                        End If
                    Next
                    For i As Integer = 1 To exps.Count - 1 Step 2
                        ops.Add(exps(i))
                    Next
                    For i As Integer = 0 To expressions.Count - 1
                        finalExp += expressions(i) + " "
                        If i <> expressions.Count - 1 Then
                            finalExp += ops(numOps) + " "
                            numOps += 1
                        End If
                    Next
                    Dim finalRes = New DataTable().Compute(finalExp, Nothing)
                    Wln("{0}= {1}", "neutralText", finalExp, FormatNumber(finalRes, 2))
                Else
                    ShowHelp("scical")
                End If
            Else
                If exps.Count = 2 Then
                    Dim finalRes
                    If exps(0) = "sqrt" Then 'Square root of a number
                        finalRes = Math.Sqrt(exps(1))
                    ElseIf exps(0) = "tan" Then 'Tangent of a number
                        finalRes = Math.Tan(exps(1))
                    ElseIf exps(0) = "sin" Then 'A sine of a number
                        finalRes = Math.Sin(exps(1))
                    ElseIf exps(0) = "cos" Then 'A cosine of a number
                        finalRes = Math.Cos(exps(1))
                    Else
                        ShowHelp("scical")
                        Exit Sub
                    End If
                    Wln(DoTranslation("{0} of {1} = {2}", currentLang), "neutralText", exps(0), exps(1), FormatNumber(finalRes, 2))
                Else
                    ShowHelp("scical")
                End If
            End If
        Catch ex As DivideByZeroException
            Wln(DoTranslation("Attempt to divide by zero is not allowed.", currentLang), "neutralText")
            If DebugMode = True Then
                Wln(ex.StackTrace, "neutralText") : Wdbg(ex.StackTrace, True)
            End If
        Catch ex As OverflowException
            Wln(DoTranslation("There has been a suspected attempt at calculating that resulted in an overflow.", currentLang), "neutralText")
            If DebugMode = True Then
                Wln("Overflow " + ex.StackTrace, "neutralText") : Wdbg("Overflow " + ex.StackTrace, True)
            End If
        Catch ex As Exception
            Wln(DoTranslation("There is an error while calculating: {0}", currentLang), "neutralText", ex.Message)
            If DebugMode = True Then
                Wln(ex.StackTrace, "neutralText") : Wdbg(ex.StackTrace, True)
            End If
        End Try

    End Sub

End Module
