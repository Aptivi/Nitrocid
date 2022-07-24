
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

Imports KS.Files.Querying

Namespace Scripting
    Public Module UESHOperators

        ''' <summary>
        ''' Checks to see if the two UESH variables are equal
        ''' </summary>
        ''' <param name="FirstVariable">The first $variable</param>
        ''' <param name="SecondVariable">The second $variable</param>
        ''' <returns>True if the two UESH variables are equal. False if otherwise.</returns>
        Public Function UESHVariableEqual(FirstVariable As String, SecondVariable As String) As Boolean
            Dim Satisfied As Boolean
            Wdbg(DebugLevel.I, "Querying {0} and {1} for equality...", FirstVariable, SecondVariable)
            Dim FirstVarValue As String = GetVariable(FirstVariable)
            Wdbg(DebugLevel.I, "Got value of {0}: {1}...", FirstVariable, FirstVarValue)
            Dim SecondVarValue As String = GetVariable(SecondVariable)
            Wdbg(DebugLevel.I, "Got value of {0}: {1}...", SecondVariable, SecondVarValue)
            Satisfied = FirstVarValue = SecondVarValue
            Wdbg(DebugLevel.I, "Satisfied: {0}", Satisfied)
            Return Satisfied
        End Function

        ''' <summary>
        ''' Checks to see if the two UESH variables are different
        ''' </summary>
        ''' <param name="FirstVariable">The first $variable</param>
        ''' <param name="SecondVariable">The second $variable</param>
        ''' <returns>True if the two UESH variables are different. False if otherwise.</returns>
        Public Function UESHVariableNotEqual(FirstVariable As String, SecondVariable As String) As Boolean
            Dim Satisfied As Boolean
            Wdbg(DebugLevel.I, "Querying {0} and {1} for inequality...", FirstVariable, SecondVariable)
            Dim FirstVarValue As String = GetVariable(FirstVariable)
            Wdbg(DebugLevel.I, "Got value of {0}: {1}...", FirstVariable, FirstVarValue)
            Dim SecondVarValue As String = GetVariable(SecondVariable)
            Wdbg(DebugLevel.I, "Got value of {0}: {1}...", SecondVariable, SecondVarValue)
            Satisfied = FirstVarValue <> SecondVarValue
            Wdbg(DebugLevel.I, "Satisfied: {0}", Satisfied)
            Return Satisfied
        End Function

        ''' <summary>
        ''' Checks to see if one of the two UESH variables is less than the other
        ''' </summary>
        ''' <param name="FirstVariable">The first $variable</param>
        ''' <param name="SecondVariable">The second $variable</param>
        ''' <returns>True if the one of the two UESH variables is less than the other. False if otherwise.</returns>
        Public Function UESHVariableLessThan(FirstVariable As String, SecondVariable As String) As Boolean
            Dim Satisfied As Boolean
            Wdbg(DebugLevel.I, "Querying {0} and {1} for inequality...", FirstVariable, SecondVariable)
            Dim FirstVarValue As String = GetVariable(FirstVariable)
            Wdbg(DebugLevel.I, "Got value of {0}: {1}...", FirstVariable, FirstVarValue)
            Dim SecondVarValue As String = GetVariable(SecondVariable)
            Wdbg(DebugLevel.I, "Got value of {0}: {1}...", SecondVariable, SecondVarValue)
            Dim FirstVarInt As Long = Long.Parse(FirstVarValue)
            Dim SecondVarInt As Long = Long.Parse(SecondVarValue)
            Satisfied = FirstVarInt < SecondVarInt
            Wdbg(DebugLevel.I, "Satisfied: {0}", Satisfied)
            Return Satisfied
        End Function

        ''' <summary>
        ''' Checks to see if one of the two UESH variables is greater than the other
        ''' </summary>
        ''' <param name="FirstVariable">The first $variable</param>
        ''' <param name="SecondVariable">The second $variable</param>
        ''' <returns>True if the one of the two UESH variables is greater than the other. False if otherwise.</returns>
        Public Function UESHVariableGreaterThan(FirstVariable As String, SecondVariable As String) As Boolean
            Dim Satisfied As Boolean
            Wdbg(DebugLevel.I, "Querying {0} and {1} for inequality...", FirstVariable, SecondVariable)
            Dim FirstVarValue As String = GetVariable(FirstVariable)
            Wdbg(DebugLevel.I, "Got value of {0}: {1}...", FirstVariable, FirstVarValue)
            Dim SecondVarValue As String = GetVariable(SecondVariable)
            Wdbg(DebugLevel.I, "Got value of {0}: {1}...", SecondVariable, SecondVarValue)
            Dim FirstVarInt As Long = Long.Parse(FirstVarValue)
            Dim SecondVarInt As Long = Long.Parse(SecondVarValue)
            Satisfied = FirstVarInt > SecondVarInt
            Wdbg(DebugLevel.I, "Satisfied: {0}", Satisfied)
            Return Satisfied
        End Function

        ''' <summary>
        ''' Checks to see if one of the two UESH variables is less than the other or equal to each other
        ''' </summary>
        ''' <param name="FirstVariable">The first $variable</param>
        ''' <param name="SecondVariable">The second $variable</param>
        ''' <returns>True if the one of the two UESH variables is less than the other or equal to each other. False if otherwise.</returns>
        Public Function UESHVariableLessThanOrEqual(FirstVariable As String, SecondVariable As String) As Boolean
            Dim Satisfied As Boolean
            Wdbg(DebugLevel.I, "Querying {0} and {1} for inequality...", FirstVariable, SecondVariable)
            Dim FirstVarValue As String = GetVariable(FirstVariable)
            Wdbg(DebugLevel.I, "Got value of {0}: {1}...", FirstVariable, FirstVarValue)
            Dim SecondVarValue As String = GetVariable(SecondVariable)
            Wdbg(DebugLevel.I, "Got value of {0}: {1}...", SecondVariable, SecondVarValue)
            Dim FirstVarInt As Long = Long.Parse(FirstVarValue)
            Dim SecondVarInt As Long = Long.Parse(SecondVarValue)
            Satisfied = FirstVarInt <= SecondVarInt
            Wdbg(DebugLevel.I, "Satisfied: {0}", Satisfied)
            Return Satisfied
        End Function

        ''' <summary>
        ''' Checks to see if one of the two UESH variables is greater than the other or equal to each other
        ''' </summary>
        ''' <param name="FirstVariable">The first $variable</param>
        ''' <param name="SecondVariable">The second $variable</param>
        ''' <returns>True if the one of the two UESH variables is greater than the other or equal to each other. False if otherwise.</returns>
        Public Function UESHVariableGreaterThanOrEqual(FirstVariable As String, SecondVariable As String) As Boolean
            Dim Satisfied As Boolean
            Wdbg(DebugLevel.I, "Querying {0} and {1} for inequality...", FirstVariable, SecondVariable)
            Dim FirstVarValue As String = GetVariable(FirstVariable)
            Wdbg(DebugLevel.I, "Got value of {0}: {1}...", FirstVariable, FirstVarValue)
            Dim SecondVarValue As String = GetVariable(SecondVariable)
            Wdbg(DebugLevel.I, "Got value of {0}: {1}...", SecondVariable, SecondVarValue)
            Dim FirstVarInt As Long = Long.Parse(FirstVarValue)
            Dim SecondVarInt As Long = Long.Parse(SecondVarValue)
            Satisfied = FirstVarInt >= SecondVarInt
            Wdbg(DebugLevel.I, "Satisfied: {0}", Satisfied)
            Return Satisfied
        End Function

        ''' <summary>
        ''' Checks to see if the value of the UESH variable is a file path and exists
        ''' </summary>
        ''' <param name="Variable">The $variable</param>
        ''' <returns>True if the file exists. False if otherwise.</returns>
        Public Function UESHVariableFileExists(Variable As String) As Boolean
            Dim Satisfied As Boolean
            Wdbg(DebugLevel.I, "Querying {0} for file existence...", Variable)
            Dim VarValue As String = GetVariable(Variable)
            Wdbg(DebugLevel.I, "Got value of {0}: {1}...", Variable, VarValue)
            Satisfied = FileExists(VarValue, True)
            Wdbg(DebugLevel.I, "Satisfied: {0}", Satisfied)
            Return Satisfied
        End Function

        ''' <summary>
        ''' Checks to see if the value of the UESH variable is a file path and doesn't exist
        ''' </summary>
        ''' <param name="Variable">The $variable</param>
        ''' <returns>True if the file doesn't exist. False if otherwise.</returns>
        Public Function UESHVariableFileDoesNotExist(Variable As String) As Boolean
            Dim Satisfied As Boolean
            Wdbg(DebugLevel.I, "Querying {0} for file existence...", Variable)
            Dim VarValue As String = GetVariable(Variable)
            Wdbg(DebugLevel.I, "Got value of {0}: {1}...", Variable, VarValue)
            Satisfied = Not FileExists(VarValue, True)
            Wdbg(DebugLevel.I, "Satisfied: {0}", Satisfied)
            Return Satisfied
        End Function

        ''' <summary>
        ''' Checks to see if the value of the UESH variable is a directory path and exists
        ''' </summary>
        ''' <param name="Variable">The $variable</param>
        ''' <returns>True if the directory exists. False if otherwise.</returns>
        Public Function UESHVariableDirectoryExists(Variable As String) As Boolean
            Dim Satisfied As Boolean
            Wdbg(DebugLevel.I, "Querying {0} for directory existence...", Variable)
            Dim VarValue As String = GetVariable(Variable)
            Wdbg(DebugLevel.I, "Got value of {0}: {1}...", Variable, VarValue)
            Satisfied = FolderExists(VarValue, True)
            Wdbg(DebugLevel.I, "Satisfied: {0}", Satisfied)
            Return Satisfied
        End Function

        ''' <summary>
        ''' Checks to see if the value of the UESH variable is a directory path and doesn't exist
        ''' </summary>
        ''' <param name="Variable">The $variable</param>
        ''' <returns>True if the directory doesn't exist. False if otherwise.</returns>
        Public Function UESHVariableDirectoryDoesNotExist(Variable As String) As Boolean
            Dim Satisfied As Boolean
            Wdbg(DebugLevel.I, "Querying {0} for directory existence...", Variable)
            Dim VarValue As String = GetVariable(Variable)
            Wdbg(DebugLevel.I, "Got value of {0}: {1}...", Variable, VarValue)
            Satisfied = Not FolderExists(VarValue, True)
            Wdbg(DebugLevel.I, "Satisfied: {0}", Satisfied)
            Return Satisfied
        End Function

        ''' <summary>
        ''' Checks to see if a UESH variable contains an expression
        ''' </summary>
        ''' <param name="FirstVariable">The first $variable</param>
        ''' <param name="SecondVariable">The second $variable</param>
        ''' <returns>True if satisfied. False if otherwise.</returns>
        Public Function UESHVariableContains(FirstVariable As String, SecondVariable As String) As Boolean
            Dim Satisfied As Boolean
            Wdbg(DebugLevel.I, "Querying {0} and {1}...", FirstVariable, SecondVariable)
            Dim FirstVarValue As String = GetVariable(FirstVariable)
            Wdbg(DebugLevel.I, "Got value of {0}: {1}...", FirstVariable, FirstVarValue)
            Dim SecondVarValue As String = GetVariable(SecondVariable)
            Wdbg(DebugLevel.I, "Got value of {0}: {1}...", SecondVariable, SecondVarValue)
            Satisfied = FirstVarValue.Contains(SecondVarValue)
            Wdbg(DebugLevel.I, "Satisfied: {0}", Satisfied)
            Return Satisfied
        End Function

        ''' <summary>
        ''' Checks to see if a UESH variable doesn't contain an expression
        ''' </summary>
        ''' <param name="FirstVariable">The first $variable</param>
        ''' <param name="SecondVariable">The second $variable</param>
        ''' <returns>True if satisfied. False if otherwise.</returns>
        Public Function UESHVariableNotContains(FirstVariable As String, SecondVariable As String) As Boolean
            Dim Satisfied As Boolean
            Wdbg(DebugLevel.I, "Querying {0} and {1}...", FirstVariable, SecondVariable)
            Dim FirstVarValue As String = GetVariable(FirstVariable)
            Wdbg(DebugLevel.I, "Got value of {0}: {1}...", FirstVariable, FirstVarValue)
            Dim SecondVarValue As String = GetVariable(SecondVariable)
            Wdbg(DebugLevel.I, "Got value of {0}: {1}...", SecondVariable, SecondVarValue)
            Satisfied = Not FirstVarValue.Contains(SecondVarValue)
            Wdbg(DebugLevel.I, "Satisfied: {0}", Satisfied)
            Return Satisfied
        End Function

        ''' <summary>
        ''' Checks to see if the value of the UESH variable is a valid file path
        ''' </summary>
        ''' <param name="Variable">The $variable</param>
        ''' <returns>True if valid. False if otherwise.</returns>
        Public Function UESHVariableValidPath(Variable As String) As Boolean
            Dim Satisfied As Boolean
            Wdbg(DebugLevel.I, "Querying {0}...", Variable)
            Dim VarValue As String = GetVariable(Variable)
            Wdbg(DebugLevel.I, "Got value of {0}: {1}...", Variable, VarValue)
            Satisfied = TryParsePath(VarValue)
            Wdbg(DebugLevel.I, "Satisfied: {0}", Satisfied)
            Return Satisfied
        End Function

        ''' <summary>
        ''' Checks to see if the value of the UESH variable is not a valid file path
        ''' </summary>
        ''' <param name="Variable">The $variable</param>
        ''' <returns>True if invalid. False if otherwise.</returns>
        Public Function UESHVariableInvalidPath(Variable As String) As Boolean
            Dim Satisfied As Boolean
            Wdbg(DebugLevel.I, "Querying {0}...", Variable)
            Dim VarValue As String = GetVariable(Variable)
            Wdbg(DebugLevel.I, "Got value of {0}: {1}...", Variable, VarValue)
            Satisfied = Not TryParsePath(VarValue)
            Wdbg(DebugLevel.I, "Satisfied: {0}", Satisfied)
            Return Satisfied
        End Function

        ''' <summary>
        ''' Checks to see if the value of the UESH variable is a valid file name
        ''' </summary>
        ''' <param name="Variable">The $variable</param>
        ''' <returns>True if valid. False if otherwise.</returns>
        Public Function UESHVariableValidFileName(Variable As String) As Boolean
            Dim Satisfied As Boolean
            Wdbg(DebugLevel.I, "Querying {0}...", Variable)
            Dim VarValue As String = GetVariable(Variable)
            Wdbg(DebugLevel.I, "Got value of {0}: {1}...", Variable, VarValue)
            Satisfied = TryParseFileName(VarValue)
            Wdbg(DebugLevel.I, "Satisfied: {0}", Satisfied)
            Return Satisfied
        End Function

        ''' <summary>
        ''' Checks to see if the value of the UESH variable is not a valid file name
        ''' </summary>
        ''' <param name="Variable">The $variable</param>
        ''' <returns>True if invalid. False if otherwise.</returns>
        Public Function UESHVariableInvalidFileName(Variable As String) As Boolean
            Dim Satisfied As Boolean
            Wdbg(DebugLevel.I, "Querying {0}...", Variable)
            Dim VarValue As String = GetVariable(Variable)
            Wdbg(DebugLevel.I, "Got value of {0}: {1}...", Variable, VarValue)
            Satisfied = Not TryParseFileName(VarValue)
            Wdbg(DebugLevel.I, "Satisfied: {0}", Satisfied)
            Return Satisfied
        End Function

    End Module
End Namespace
