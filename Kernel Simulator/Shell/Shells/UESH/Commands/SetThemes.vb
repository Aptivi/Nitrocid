
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

Imports KS.Kernel.Configuration
Imports KS.Files.Querying

Namespace Shell.Shells.UESH.Commands
    Class SetThemesCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            If ColoredShell Then
                'Try to apply the theme
                Dim ThemePath As String = NeutralizePath(ListArgs(0))
                If FileExists(ThemePath) Then
                    ApplyThemeFromFile(ThemePath)
                Else
                    ApplyThemeFromResources(ListArgs(0))
                End If

                'Save it to configuration
                CreateConfig()
            Else
                Write(DoTranslation("Colors are not available. Turn on colored shell in the kernel config."), True, ColTypes.Neutral)
            End If
        End Sub

        Public Overrides Sub HelpHelper()
            Write("<Theme>: ThemeName.json, " + String.Join(", ", Themes.Themes.Keys), True, ColTypes.Neutral)
        End Sub

    End Class
End Namespace
