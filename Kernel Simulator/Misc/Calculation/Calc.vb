
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

Public Module Calc

    ''' <summary>
    ''' Try to calculate an expression
    ''' </summary>
    ''' <param name="Expression">An expression</param>
    ''' <returns>A dictionary of results and success/failure</returns>
    ''' <remarks>Obsolete in favor of string evaluator.</remarks>
    <Obsolete>
    Public Function DoCalc(ByVal Expression As String) As Dictionary(Of Double, Boolean)
        Dim Calculated As Boolean
        Dim Result As Double
        Dim DictCheck As New Dictionary(Of Double, Boolean) 'Key: Result / Value: If True, then success. Else, failure.
        Try
            Wdbg("I", "Expression: {0}", Expression)
            Result = New DataTable().Compute(Expression, Nothing)
            Wdbg("I", "Result: {0}", Result)
            Calculated = True
        Catch ex As Exception
            Wdbg("E", "Error while evaluating expression: {0}", ex.Message)
            WStkTrc(ex)
        End Try
        DictCheck.Add(Result, Calculated)
        Return DictCheck
    End Function

End Module
