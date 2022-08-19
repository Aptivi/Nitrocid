
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

Imports KS.Misc.Settings

Namespace Shell.Shells.UESH.Commands
    ''' <summary>
    ''' Lets you change kernel settings
    ''' </summary>
    ''' <remarks>
    ''' This command starts up the Settings application, which allows you to change the kernel settings available to you. It's the successor to the defunct Kernel Simulator Configuration Tool application, and is native to the kernel.
    ''' <br></br>
    ''' It starts with the list of sections to start from. Once the user selects one, they'll be greeted with various options that are configurable. When they choose one, they'll be able to change the setting there.
    ''' <br></br>
    ''' If you just want to try out a setting without saving to the configuration file, you can change a setting and exit it immediately. It only survives the current session until you decide to save the changes to the configuration file.
    ''' <br></br>
    ''' Some settings allow you to specify a string, a number, or by the usage of another API, like the ColorWheel() tool.
    ''' <br></br>
    ''' In the string or long string values, if you used the /clear value, it will blank out the value. In some settings, if you just pressed ENTER, it'll use the same value that the kernel uses at the moment.
    ''' <br></br>
    ''' We've made sure that this application is user-friendly.
    ''' <br></br>
    ''' For the screensaver and splashes, refer to the command usage below.
    ''' <br></br>
    ''' <list type="table">
    ''' <listheader>
    ''' <term>Switches</term>
    ''' <description>Description</description>
    ''' </listheader>
    ''' <item>
    ''' <term>-saver</term>
    ''' <description>Opens the screensaver settings</description>
    ''' </item>
    ''' <item>
    ''' <term>-splash</term>
    ''' <description>Opens the splash settings</description>
    ''' </item>
    ''' </list>
    ''' <br></br>
    ''' The user must have at least the administrative privileges before they can run the below commands.
    ''' </remarks>
    Class SettingsCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
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
