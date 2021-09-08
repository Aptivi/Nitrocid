
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

Public Module UESHVariables

    Public ShellVariables As New Dictionary(Of String, String)

    ''' <summary>
    ''' Initializes a $variable
    ''' </summary>
    ''' <param name="var">A $variable</param>
    Public Sub InitializeVariable(var As String)
        If Not ShellVariables.ContainsKey(var) Then
            ShellVariables.Add(var, "")
            Wdbg("I", "Initialized variable {0}", var)
        End If
    End Sub

    ''' <summary>
    ''' Gets a value of a $variable on command line
    ''' </summary>
    ''' <param name="var">A $variable</param>
    ''' <param name="cmd">A command line in script</param>
    ''' <returns>A command line in script that has a value of $variable</returns>
    Function GetVariableCommand(var As String, cmd As String) As String
        If Not Commands(cmd.Split(" ")(0)).SettingVariable Then
            Dim newcmd As String = cmd.Replace(var, ShellVariables(var))
            Wdbg("I", "Replaced variable {0} with their values. Result: {1}", var, newcmd)
            Return newcmd
        End If
        Return cmd
    End Function

    ''' <summary>
    ''' Gets a value of a $variable
    ''' </summary>
    ''' <param name="var">A $variable</param>
    ''' <returns>A value of $variable, or an empty string if not found</returns>
    Public Function GetVariable(var As String) As String
        Try
            Return ShellVariables(var)
        Catch ex As Exception
            Wdbg("E", "Error getting variable {0}: {1}", var, ex.Message)
        End Try
        Return ""
    End Function

    ''' <summary>
    ''' Sets a $variable
    ''' </summary>
    ''' <param name="var">A $variable</param>
    ''' <param name="value">A value to set to $variable</param>
    Public Function SetVariable(var As String, value As String) As Boolean
        Try
            If Not ShellVariables.ContainsKey(var) Then InitializeVariable(var)
            ShellVariables(var) = value
            Wdbg("I", "Set variable {0} to {1}", var, value)
            Return True
        Catch ex As Exception
            Wdbg("E", "Error setting variable {0}: {1}", var, ex.Message)
        End Try
        Return False
    End Function

End Module
