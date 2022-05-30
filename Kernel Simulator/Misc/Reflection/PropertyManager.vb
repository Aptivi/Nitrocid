
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

Imports System.Reflection

Namespace Misc.Reflection
    Public Module PropertyManager

        ''' <summary>
        ''' Gets the value of a property in the type of a variable dynamically
        ''' </summary>
        ''' <param name="Variable">Variable name. Use operator NameOf to get name.</param>
        ''' <param name="Property">Property name from within the variable type</param>
        ''' <returns>Value of a property</returns>
        Public Function GetPropertyValueInVariable(Variable As String, [Property] As String) As Object
            Return GetPropertyValueInVariable(Variable, [Property], Nothing)
        End Function

        ''' <summary>
        ''' Gets the value of a property in the type of a variable dynamically
        ''' </summary>
        ''' <param name="Variable">Variable name. Use operator NameOf to get name.</param>
        ''' <param name="Property">Property name from within the variable type</param>
        ''' <param name="VariableType">Variable type</param>
        ''' <returns>Value of a property</returns>
        Public Function GetPropertyValueInVariable(Variable As String, [Property] As String, VariableType As Type) As Object
            'Get field for specified variable
            Dim TargetField As FieldInfo
            If VariableType IsNot Nothing Then
                TargetField = GetField(Variable, VariableType)
            Else
                TargetField = GetField(Variable)
            End If

            'Get the variable if found
            If TargetField IsNot Nothing Then
                'Now, get the property
                Wdbg(DebugLevel.I, "Got field {0}.", TargetField.Name)
                Dim TargetProperty As PropertyInfo = TargetField.FieldType.GetProperty([Property])
                Dim TargetValue As Object
                If VariableType IsNot Nothing Then
                    TargetValue = GetValue(Variable, VariableType)
                Else
                    TargetValue = GetValue(Variable)
                End If

                'Get the property value if found
                If TargetProperty IsNot Nothing Then
                    Return TargetProperty.GetValue(TargetValue)
                Else
                    'Property not found on any of the "flag" modules.
                    Wdbg(DebugLevel.I, "Property {0} not found.", [Property])
                    Throw New Exceptions.NoSuchReflectionPropertyException(DoTranslation("Property {0} is not found on any of the modules."), [Property])
                End If
            Else
                'Variable not found on any of the "flag" modules.
                Wdbg(DebugLevel.I, "Field {0} not found.", Variable)
                Throw New Exceptions.NoSuchReflectionVariableException(DoTranslation("Variable {0} is not found on any of the modules."), Variable)
            End If
        End Function

        ''' <summary>
        ''' Gets the properties from the type dynamically
        ''' </summary>
        ''' <param name="VariableType">Variable type</param>
        ''' <returns>Dictionary containing all properties</returns>
        Public Function GetProperties(VariableType As Type) As Dictionary(Of String, Object)
            'Get field for specified variable
            Dim Properties As PropertyInfo() = VariableType.GetProperties()
            Dim PropertyDict As New Dictionary(Of String, Object)

            'Get the properties and get their values
            For Each VarProperty As PropertyInfo In Properties
                Dim PropertyValue As Object = VarProperty.GetValue(VariableType)
                PropertyDict.Add(VarProperty.Name, PropertyValue)
            Next
            Return PropertyDict
        End Function

        ''' <summary>
        ''' Gets the properties from the type without evaluation
        ''' </summary>
        ''' <param name="VariableType">Variable type</param>
        ''' <returns>Dictionary containing all properties</returns>
        Public Function GetPropertiesNoEvaluation(VariableType As Type) As Dictionary(Of String, Type)
            'Get field for specified variable
            Dim Properties As PropertyInfo() = VariableType.GetProperties()
            Dim PropertyDict As New Dictionary(Of String, Type)

            'Get the properties and get their values
            For Each VarProperty As PropertyInfo In Properties
                PropertyDict.Add(VarProperty.Name, VarProperty.PropertyType)
            Next
            Return PropertyDict
        End Function

    End Module
End Namespace
