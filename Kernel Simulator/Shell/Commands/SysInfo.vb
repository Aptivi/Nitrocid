
'    Kernel Simulator  Copyright (C) 2018-2021  EoflaOE
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

Class SysInfoCommand
    Inherits CommandExecutor
    Implements ICommand

    Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
        Dim ShowSystemInfo, ShowHardwareInfo, ShowUserInfo, ShowMessageOfTheDay, ShowMal As Boolean
        If ListSwitchesOnly.Contains("-s") Then ShowSystemInfo = True
        If ListSwitchesOnly.Contains("-h") Then ShowHardwareInfo = True
        If ListSwitchesOnly.Contains("-u") Then ShowUserInfo = True
        If ListSwitchesOnly.Contains("-m") Then ShowMessageOfTheDay = True
        If ListSwitchesOnly.Contains("-l") Then ShowMal = True
        If ListSwitchesOnly.Contains("-a") Or ListSwitchesOnly.Length = 0 Then
            ShowSystemInfo = True
            ShowHardwareInfo = True
            ShowUserInfo = True
            ShowMessageOfTheDay = True
            ShowMal = True
        End If

        If ShowSystemInfo Then
            'Kernel section
            WriteSeparator(DoTranslation("Kernel settings"), True)
            W(DoTranslation("Kernel Version:") + " ", False, ColTypes.ListEntry) : W(KernelVersion, True, ColTypes.ListValue)
            W(DoTranslation("Debug Mode:") + " ", False, ColTypes.ListEntry) : W(DebugMode, True, ColTypes.ListValue)
            W(DoTranslation("Colored Shell:") + " ", False, ColTypes.ListEntry) : W(ColoredShell, True, ColTypes.ListValue)
            W(DoTranslation("Arguments on Boot:") + " ", False, ColTypes.ListEntry) : W(ArgsOnBoot, True, ColTypes.ListValue)
            W(DoTranslation("Help command simplified:") + " ", False, ColTypes.ListEntry) : W(SimHelp, True, ColTypes.ListValue)
            W(DoTranslation("MOTD on Login:") + " ", False, ColTypes.ListEntry) : W(ShowMOTD, True, ColTypes.ListValue)
            W(DoTranslation("Time/Date on corner:") + " ", False, ColTypes.ListEntry) : W(CornerTimeDate, True, ColTypes.ListValue)
            Console.WriteLine()
        End If

        If ShowHardwareInfo Then
            'Hardware section
            WriteSeparator(DoTranslation("Hardware settings"), True)
            ListHardware()
            W(DoTranslation("Use ""hwinfo"" for extended information about hardware."), True, ColTypes.Tip)
            Console.WriteLine()
        End If

        If ShowUserInfo Then
            'User section
            WriteSeparator(DoTranslation("User settings"), True)
            W(DoTranslation("Current user name:") + " ", False, ColTypes.ListEntry) : W(CurrentUser.Username, True, ColTypes.ListValue)
            W(DoTranslation("Current host name:") + " ", False, ColTypes.ListEntry) : W(HostName, True, ColTypes.ListValue)
            W(DoTranslation("Available usernames:") + " ", False, ColTypes.ListEntry) : W(String.Join(", ", Users.Keys), True, ColTypes.ListValue)
            Console.WriteLine()
        End If

        If ShowMessageOfTheDay Then
            'Show MOTD
            WriteSeparator("MOTD", True)
            W(ProbePlaces(MOTDMessage), True, ColTypes.Neutral)
        End If

        If ShowMal Then
            'Show MAL
            WriteSeparator("MAL", True)
            W(ProbePlaces(MAL), True, ColTypes.Neutral)
        End If
    End Sub

End Class