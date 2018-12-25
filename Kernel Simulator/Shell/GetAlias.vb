
'    Kernel Simulator  Copyright (C) 2018  EoflaOE
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

Module GetAlias

    Sub ExecuteAlias(ByVal aliascmd As String)

        Dim actualCmd As String = aliases(aliascmd) + strcommand.Replace(aliascmd, "")
        GetCommand.ExecuteCommand(actualCmd)

    End Sub

    Public Sub ListAliases()
        If (aliases.Count <> 0) Then
            Dim aliasValues As New ArrayList
            Dim aliasKeys As New ArrayList
            For Each a As String In aliases.Values
                aliasValues.Add(a)
            Next
            For Each a As String In aliases.Keys
                aliasKeys.Add(a)
            Next
            For a As Integer = 0 To aliasKeys.Count - 1
                W("{0}: ", "helpCmd", aliasKeys(a))
                Wln(aliasValues(a), "helpDef")
            Next
        End If

    End Sub

End Module
