
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

Namespace Scripting.Conditions.Types
    Public Class EqualsCondition
        Inherits BaseCondition
        Implements ICondition

        Public Overrides ReadOnly Property ConditionName As String Implements ICondition.ConditionName
            Get
                Return "eq"
            End Get
        End Property

        Public Overrides ReadOnly Property ConditionPosition As Integer = 2 Implements ICondition.ConditionPosition

        Public Overrides ReadOnly Property ConditionRequiredArguments As Integer = 3 Implements ICondition.ConditionRequiredArguments

        Public Overrides Function IsConditionSatisfied(FirstVariable As String, SecondVariable As String) As Boolean Implements ICondition.IsConditionSatisfied
            Return UESHVariableEqual(FirstVariable, SecondVariable)
        End Function

    End Class
End Namespace