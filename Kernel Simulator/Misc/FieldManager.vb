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

Imports System.Reflection

Public Module FieldManager

    ''' <summary>
    ''' Sets the value of a variable to the new value dynamically
    ''' </summary>
    ''' <param name="Variable">Variable name. Use operator NameOf to get name.</param>
    ''' <param name="VariableValue">New value of variable</param>
    ''' <param name="VariableType">Variable type</param>
    Public Sub SetValue(ByVal Variable As String, ByVal VariableValue As Object, ByVal VariableType As Type)
        'Get field for specified variable
        Dim TargetField As FieldInfo = GetField(Variable, VariableType)

        'Set the variable if found
        If Not IsNothing(TargetField) Then
            'The "obj" description says this: "The object whose field value will be set."
            'Apparently, SetValue works on modules if you specify a variable name as an object (first argument). Not only classes.
            'Unfortunately, there are no examples on the MSDN that showcase such situations; classes are being used.
            Wdbg("I", "Got field {0}. Setting to {1}...", TargetField.Name, VariableValue)
            TargetField.SetValue(Variable, VariableValue)
        Else
            'Variable not found on any of the "flag" modules.
            Wdbg("I", "Field {0} not found.", Variable)
            W(DoTranslation("Variable {0} is not found on any of the modules."), True, ColTypes.Err, Variable)
        End If
    End Sub

    ''' <summary>
    ''' Gets the value of a variable dynamically 
    ''' </summary>
    ''' <param name="Variable">Variable name. Use operator NameOf to get name.</param>
    ''' <param name="VariableType">Variable type</param>
    ''' <returns>Value of a variable</returns>
    Public Function GetValue(ByVal Variable As String, ByVal VariableType As Type) As Object
        'Get field for specified variable
        Dim TargetField As FieldInfo = GetField(Variable, VariableType)

        'Get the variable if found
        If Not IsNothing(TargetField) Then
            'The "obj" description says this: "The object whose field value will be returned."
            'Apparently, GetValue works on modules if you specify a variable name as an object (first argument). Not only classes.
            'Unfortunately, there are no examples on the MSDN that showcase such situations; classes are being used.
            Wdbg("I", "Got field {0}.", TargetField.Name)
            Return TargetField.GetValue(Variable)
        Else
            'Variable not found on any of the "flag" modules.
            Wdbg("I", "Field {0} not found.", Variable)
            W(DoTranslation("Variable {0} is not found on any of the modules."), True, ColTypes.Err, Variable)
            Return Nothing
        End If
    End Function

    ''' <summary>
    ''' Gets a field from variable name
    ''' </summary>
    ''' <param name="Variable">Variable name. Use operator NameOf to get name.</param>
    ''' <param name="Type">Variable type</param>
    ''' <returns>Field information</returns>
    Public Function GetField(ByVal Variable As String, ByVal Type As Type) As FieldInfo
        'Get fields of specified type
        Dim Field As FieldInfo = Type.GetField(Variable)

        'Check if any of them contains the specified variable
        If Not IsNothing(Field) Then
            Return Field
        End If
    End Function

End Module
