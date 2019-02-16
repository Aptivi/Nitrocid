
'    Kernel Simulator  Copyright (C) 2018-2019  EoflaOE
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
    Public forbidden As String() = {"alias"}

    Public Sub manageAlias(ByVal mode As String, ByVal aliasTBA As String, Optional ByVal cmd As String = "")

        If (mode = "add") Then
            If (aliasTBA = cmd) Then
                Wln(DoTranslation("Alias can't be the same name as a command.", currentLang), "neutralText")
                Wdbg("({0} = {1}) = true", aliasTBA, cmd)
            ElseIf Not (availableCommands.Contains(cmd)) Then
                Wln(DoTranslation("Command not found to alias to {0}.", currentLang), "neutralText", aliasTBA)
                Wdbg("availableCmds.Cont({0}) = false | No aliasing", cmd)
            ElseIf (forbidden.Contains(cmd)) Then
                Wln(DoTranslation("Aliasing {0} to {1} is forbidden completely.", currentLang), "neutralText", cmd, aliasTBA)
                Wdbg("forbid.Cont({0}) = true | No aliasing", cmd)
            Else
                Wdbg("({0} = {1}) = false", aliasTBA, cmd)
                aliases.Add(aliasTBA, cmd)
                Wln(DoTranslation("You can now run ""{0}"" as a command: ""{1}"".", currentLang), "neutralText", aliasTBA, cmd)
            End If
        ElseIf (mode = "rem") Then
            If (aliases.ContainsKey(aliasTBA)) Then
                cmd = aliases(aliasTBA)
                Wdbg("aliases({0}) is found", aliasTBA)
                aliases.Remove(aliasTBA)
                Wln(DoTranslation("You can no longer use ""{0}"" as a command ""{1}"".", currentLang), "neutralText", aliasTBA, cmd)
            Else
                Wdbg("aliases({0}) is not found", aliasTBA)
                Wln(DoTranslation("Alias {0} is not found to be removed.", currentLang), "neutralText", aliasTBA)
            End If
        Else
            Wln(DoTranslation("Invalid mode {0}.", currentLang), "neutralText", mode)
        End If

    End Sub

End Module
