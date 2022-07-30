
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

Imports KS.Hardware

Namespace Shell.Commands
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
                Write(DoTranslation("Kernel Version:") + " ", False, ColTypes.ListEntry) : Write(KernelVersion, True, ColTypes.ListValue)
                Write(DoTranslation("Debug Mode:") + " ", False, ColTypes.ListEntry) : Write(DebugMode.ToString, True, ColTypes.ListValue)
                Write(DoTranslation("Colored Shell:") + " ", False, ColTypes.ListEntry) : Write(ColoredShell.ToString, True, ColTypes.ListValue)
                Write(DoTranslation("Arguments on Boot:") + " ", False, ColTypes.ListEntry) : Write(ArgsOnBoot.ToString, True, ColTypes.ListValue)
                Write(DoTranslation("Help command simplified:") + " ", False, ColTypes.ListEntry) : Write(SimHelp.ToString, True, ColTypes.ListValue)
                Write(DoTranslation("MOTD on Login:") + " ", False, ColTypes.ListEntry) : Write(ShowMOTD.ToString, True, ColTypes.ListValue)
                Write(DoTranslation("Time/Date on corner:") + " ", False, ColTypes.ListEntry) : Write(CornerTimeDate.ToString, True, ColTypes.ListValue)
                Console.WriteLine()
            End If

            If ShowHardwareInfo Then
                'Hardware section
                WriteSeparator(DoTranslation("Hardware settings"), True)
                ListHardware()
                Write(DoTranslation("Use ""hwinfo"" for extended information about hardware."), True, ColTypes.Tip)
                Console.WriteLine()
            End If

            If ShowUserInfo Then
                'User section
                WriteSeparator(DoTranslation("User settings"), True)
                Write(DoTranslation("Current user name:") + " ", False, ColTypes.ListEntry) : Write(CurrentUser.Username, True, ColTypes.ListValue)
                Write(DoTranslation("Current host name:") + " ", False, ColTypes.ListEntry) : Write(HostName, True, ColTypes.ListValue)
                Write(DoTranslation("Available usernames:") + " ", False, ColTypes.ListEntry) : Write(String.Join(", ", ListAllUsers), True, ColTypes.ListValue)
                Console.WriteLine()
            End If

            If ShowMessageOfTheDay Then
                'Show MOTD
                WriteSeparator("MOTD", True)
                Write(ProbePlaces(MOTDMessage), True, ColTypes.Neutral)
            End If

            If ShowMal Then
                'Show MAL
                WriteSeparator("MAL", True)
                Write(ProbePlaces(MAL), True, ColTypes.Neutral)
            End If
        End Sub

        Public Overrides Sub HelpHelper()
            Write(DoTranslation("This command has the below switches that change how it works:"), True, ColTypes.Neutral)
            Write("  -s: ", False, ColTypes.ListEntry) : Write(DoTranslation("Shows the system information"), True, ColTypes.ListValue)
            Write("  -h: ", False, ColTypes.ListEntry) : Write(DoTranslation("Shows the hardware information"), True, ColTypes.ListValue)
            Write("  -u: ", False, ColTypes.ListEntry) : Write(DoTranslation("Shows the user information"), True, ColTypes.ListValue)
            Write("  -m: ", False, ColTypes.ListEntry) : Write(DoTranslation("Shows the message of the day"), True, ColTypes.ListValue)
            Write("  -l: ", False, ColTypes.ListEntry) : Write(DoTranslation("Shows the message of the day after login"), True, ColTypes.ListValue)
            Write("  -a: ", False, ColTypes.ListEntry) : Write(DoTranslation("Shows all information"), True, ColTypes.ListValue)
        End Sub

    End Class
End Namespace