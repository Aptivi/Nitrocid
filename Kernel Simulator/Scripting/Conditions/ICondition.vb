
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

Namespace Scripting.Conditions
    Public Interface ICondition

        ''' <summary>
        ''' Specifies the condition name
        ''' </summary>
        ReadOnly Property ConditionName As String
        ''' <summary>
        ''' Specifies where the condition should be located. Beware that it starts from 1.
        ''' </summary>
        ReadOnly Property ConditionPosition As Integer
        ''' <summary>
        ''' How many arguments are required (counting the condition itself)? Beware that it starts from 1.
        ''' </summary>
        ReadOnly Property ConditionRequiredArguments As Integer
        ''' <summary>
        ''' Checks whether the condition is satisfied
        ''' </summary>
        Function IsConditionSatisfied(FirstVariable As String, SecondVariable As String) As Boolean
        ''' <summary>
        ''' Checks whether the condition is satisfiedfor more than two variables
        ''' </summary>
        Function IsConditionSatisfied(Variables As String()) As Boolean

    End Interface
End Namespace