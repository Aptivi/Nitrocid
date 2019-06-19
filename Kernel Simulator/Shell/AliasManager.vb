
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
    Private aliastreamr As New IO.StreamReader(paths("Home") + "/aliases.csv")

    'Initializing and Saving
    Public Sub InitAliases()
        'Get all aliases from file
        While Not aliastreamr.EndOfStream
            Dim line As String = aliastreamr.ReadLine
            If Not aliases.ContainsKey(line.Remove(line.IndexOf(","c))) Then
                aliases.Add(line.Remove(line.IndexOf(","c)), line.Substring(line.IndexOf(" "c) + 1))
            End If
        End While
        aliastreamr.BaseStream.Seek(0, IO.SeekOrigin.Begin)
    End Sub
    Public Sub SaveAliases()
        'Save all aliases to file
        Dim aliast As New List(Of String)
        For i As Integer = 0 To aliases.Count - 1
            aliast.Add(aliases.Keys(i) + ", " + aliases.Values(i))
        Next

        'Close the read stream, write all the lines, then open the read stream again
        aliastreamr.Close()
        IO.File.WriteAllLines(paths("Home") + "/aliases.csv", aliast)
        aliastreamr = New IO.StreamReader(paths("Home") + "/aliases.csv")
    End Sub

    'Management and Execution
    Public Sub ManageAlias(ByVal mode As String, ByVal aliasTBA As String, Optional ByVal cmd As String = "")
        If (mode = "add") Then
            'User tries to add an alias.
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
            'user tries to remove an alias
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

        'Save all aliases
        SaveAliases()
    End Sub
    Sub ExecuteAlias(ByVal aliascmd As String)
        'If this sub worked properly, consider putting it to Shell directly
        Dim actualCmd As String = strcommand.Replace(aliascmd, aliases(aliascmd))
        GetCommand.ExecuteCommand(actualCmd)
    End Sub

End Module
