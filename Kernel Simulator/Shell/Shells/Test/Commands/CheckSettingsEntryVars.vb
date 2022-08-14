
'    Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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

Imports KS.Misc.Configuration

Namespace Shell.Shells.Test.Commands
    Class Test_CheckSettingsEntryVarsCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim Results As Dictionary(Of String, Boolean) = CheckSettingsVariables()
            Dim NotFound As New List(Of String)

            'Go through each and every result
            For Each Variable As String In Results.Keys
                Dim IsFound As Boolean = Results(Variable)
                If Not IsFound Then
                    NotFound.Add(Variable)
                End If
            Next

            'Warn if not found
            If NotFound.Count > 0 Then
                Write(DoTranslation("These configuration entries have invalid variables or enumerations and need to be fixed:"), True, ColTypes.Warning)
                WriteList(NotFound)
            End If
        End Sub

    End Class
End Namespace
