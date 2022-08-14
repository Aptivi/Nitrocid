
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

Imports Newtonsoft.Json.Linq

Namespace Shell.Shells.Test.Commands
    Class Test_CheckLocalLinesCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim EnglishJson As JToken = JToken.Parse(My.Resources.eng)
            Dim LanguageJson As JToken
            For Each LanguageName As String In Languages.Languages.Keys
                LanguageJson = JToken.Parse(My.Resources.ResourceManager.GetString(LanguageName.Replace("-", "_")))
                If LanguageJson.Count <> EnglishJson.Count Then
                    Write(DoTranslation("Line mismatch in") + " {0}: {1} <> {2}", True, ColTypes.Warning, LanguageName, LanguageJson.Count, EnglishJson.Count)
                End If
            Next
        End Sub

    End Class
End Namespace
