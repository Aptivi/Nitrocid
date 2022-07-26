
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

Imports KS.Files.Folders
Imports KS.Shell.ShellBase.Shells
Imports ReadLineReboot

Namespace Shell.ShellBase
    Public Class CommandAutoComplete
        Implements IAutoCompleteHandler

        Private ReadOnly ShellTypeToAutocomplete As ShellType = ShellType.Shell

        Public Property Separators As Char() = {" "c} Implements IAutoCompleteHandler.Separators

        Public Function GetSuggestions(text As String, index As Integer) As String() Implements IAutoCompleteHandler.GetSuggestions
            If ShellStack.Count > 0 Then
                If String.IsNullOrEmpty(text) Then
                    Return GetCommands(ShellTypeToAutocomplete).Keys.ToArray()
                Else
                    'TODO: We currently only support file listing. Extend command base to support CommandArgumentInfo containing
                    'command argument and autocomplete information.
                    Return CreateList(CurrentDir, True).Select(Function(x) x.Name).ToArray()
                End If
            Else
                Return Nothing
            End If
        End Function

        Protected Friend Sub New(Optional ShellType As ShellType = ShellType.Shell)
            ShellTypeToAutocomplete = ShellType
        End Sub

    End Class
End Namespace
