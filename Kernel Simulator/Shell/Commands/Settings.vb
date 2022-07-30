
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

Namespace Shell.Commands
    Class SettingsCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim SettingsType As SettingsType = SettingsType.Normal
            If ListSwitchesOnly.Length > 0 Then
                If ListSwitchesOnly(0) = "-saver" Then SettingsType = SettingsType.Screensaver
                If ListSwitchesOnly(0) = "-splash" Then SettingsType = SettingsType.Splash
            End If
            OpenMainPage(SettingsType)
        End Sub

        Public Overrides Sub HelpHelper()
            Write(DoTranslation("This command has the below switches that change how it works:"), True, ColTypes.Neutral)
            Write("  -saver: ", False, ColTypes.ListEntry) : Write(DoTranslation("Opens the screensaver settings"), True, ColTypes.ListValue)
            Write("  -splash: ", False, ColTypes.ListEntry) : Write(DoTranslation("Opens the splash settings"), True, ColTypes.ListValue)
        End Sub

    End Class
End Namespace