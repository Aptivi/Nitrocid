
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

Public Module AliasManager

    Public aliases As New Dictionary(Of String, String)

    Public Sub manageAlias(ByVal mode As String, ByVal aliasTBA As String, ByVal cmd As String)

        If (mode = "add") Then
            If (aliasTBA = cmd) Then
                Wln("Alias can't be the same name as a command.", "neutralText")
                Wdbg("({0} = {1}) = true", True, aliasTBA, cmd)
            ElseIf Not (availableCommands.Contains(cmd)) Then
                Wln("Command not found to alias to {0}.", "neutralText", aliasTBA)
                Wdbg("availableCmds.Cont({0}) = false | No aliasing", True, cmd)
            Else
                Wdbg("({0} = {1}) = false", True, aliasTBA, cmd)
                aliases.Add(aliasTBA, cmd)
                Wln("You can now run ""{0}"" as a command: ""{1}"".", "neutralText", aliasTBA, cmd)
            End If
        ElseIf (mode = "rem") Then
            If (aliases.ContainsKey(aliasTBA)) Then
                Wdbg("aliases({0}) is found", True, aliasTBA)
                aliases.Remove(aliasTBA)
                Wln("You can no longer use ""{0}"" as a command ""{1}"".", "neutralText", aliasTBA, cmd)
            Else
                Wdbg("aliases({0}) is not found", True, aliasTBA)
                Wln("Alias {0} is not found to be removed.", "neutralText", aliasTBA)
            End If
        Else
            Wln("Invalid mode {0}.", "neutralText", mode)
        End If

    End Sub

End Module
