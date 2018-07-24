
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

Module HelpSystem

    Sub ShowHelp(Optional ByVal command As String = "")

        If (command = "") Then

            'TODO: Color the help system. (0.0.5)
            'TODO: Modification Parser for mods with commands (Help, Commands, and everything).
            If (simHelp = False) Then
                'It is messy. It will actually be converted to dictionary in 0.0.5.
                Wln("- adduser: {0}" + vbNewLine + "- alias: {42}" + "- annoying-sound (Alias: beep): {1}" + vbNewLine + "- arginj: {2}" + vbNewLine + "- calc: {3}" + vbNewLine + _
                    "- cdir (Alias: currentdir): {4}" + vbNewLine + "- changedir (Aliases: chdir, cd): {5}" + vbNewLine + "- chmal: {43}" + vbNewLine + "- chhostname: {6}" + vbNewLine + _
                    "- chmotd: {7}" + vbNewLine + "- choice: {8}" + vbNewLine + "- chpwd: {9}" + vbNewLine + "- chusrname: {10}" + vbNewLine + "- cls: {11}" + vbNewLine + _
                    "- debuglog: {12}" + vbNewLine + "- disco: {13}" + vbNewLine + "- echo: {14}" + vbNewLine + "- help: {15}" + vbNewLine + "- hwprobe: {16}" + vbNewLine + _
                    "- list (Alias: ls): {17}" + vbNewLine + "- logout: {18}" + vbNewLine + "- lsdrivers: {19}" + vbNewLine + "- lsnet: {20}" + vbNewLine + _
                    "- lsnettree: {21}" + vbNewLine + "- md (Alias: mkdir): {22}" + vbNewLine + "- netinfo: {23}" + vbNewLine + "- panicsim: {24}" + vbNewLine + _
                    "- perm: {25}" + vbNewLine + "- ping: {26}" + vbNewLine + "- read: {27}" + vbNewLine + "- reboot: {28}" + vbNewLine + "- reloadconfig: {29}" + vbNewLine + _
                    "- rd (Alias: rmdir): {30}" + vbNewLine + "- rmuser: {31}" + vbNewLine + "- scical: {32}" + vbNewLine + "- setcolors: {33}" + vbNewLine + _
                    "- setthemes: {34}" + vbNewLine + "- showmotd: {35}" + vbNewLine + "- showmal: {44}" + vbNewLine + "- showtd: {36}" + vbNewLine + "- showtdzone: {37}" + vbNewLine + "- shutdown: {38}" + vbNewLine + _
                    "- sysinfo: {39}" + vbNewLine + "- unitconv: {40}" + vbNewLine + "- version: {41}" + vbNewLine + vbNewLine + _
                    "* You can use multiple commands at the same time using the colon between commands." + vbNewLine + _
                    "* Pre-defined aliases will be removed in 0.0.5." + vbNewLine + _
                    "* Aliases will be shown in ""showaliases"" on 0.0.5.", "neutralText", _
                    adduserCmdDesc, annoyingSoundCmdDesc, argInjCmdDesc, calcCmdDesc, currentDirCmdDesc, changeDirCmdDesc, chHostNameCmdDesc, chMotdCmdDesc, _
                    choiceCmdDesc, chPwdCmdDesc, chUsrNameCmdDesc, clsCmdDesc, debugLogCmdDesc, discoCmdDesc, echoCmdDesc, helpCmdDesc, hwProbeCmdDesc, listCmdDesc, _
                    logoutCmdDesc, lsDriversCmdDesc, lsnetCmdDesc, lsNetByTreeCmdDesc, makeDirectoryCmdDesc, netInfoCmdDesc, panicSimCmdDesc, permCmdDesc, pingCmdDesc, _
                    readCmdDesc, rebootCmdDesc, reloadConfigCmdDesc, rmDirCmdDesc, rmUserCmdDesc, sciCalCmdDesc, setColorsCmdDesc, setThemesCmdDesc, showMotdCmdDesc, _
                    showTdCmdDesc, showTdZoneCmdDesc, shutdownCmdDesc, sysInfoCmdDesc, unitConvCmdDesc, versionCmdDesc, aliasCmdDesc, chMalCmdDesc, showMalCmdDesc)
            Else
                Wln(String.Join(", ", availableCommands), "neutralText")
            End If

        ElseIf (command = "adduser") Then

            Wln("Usage: adduser <userName> [password] [confirm]" + vbNewLine + _
                "       adduser: to be prompted about new username and password. (deprecated)", "neutralText")

        ElseIf (command = "alias") Then

            Wln("Usage: alias <rem/add> <alias> <cmd>", "neutralText")

        ElseIf (command = "annoying-sound") Or (command = "beep") Then

            Wln("Usage: annoying-sound/beep <Frequency:Hz> <Time:Seconds>" + vbNewLine + _
                "       annoying-sound/beep: to be prompted about beeping. (deprecated)", "neutralText")

        ElseIf (command = "arginj") Then

            Wln("Usage: arginj [Arguments separated by commas]" + vbNewLine + _
                "       arginj: to be prompted about boot arguments. (deprecated)", "neutralText")

        ElseIf (command = "calc") Then

            Wln("Usage: calc <expression> ...", "neutralText")

        ElseIf (command = "cdir") Or (command = "currentdir") Then

            Wln("Usage: cdir/currentdir: to get current directory", "neutralText")

        ElseIf (command = "changedir") Or (command = "chdir") Or (command = "cd") Then

            Wln("Usage: chdir/changedir/cd <directory> OR ..", "neutralText")

        ElseIf (command = "chhostname") Then

            Wln("Usage: chhostname <HostName>" + vbNewLine + _
                "       chhostname: to be prompted about changing host name. (deprecated)", "neutralText")

        ElseIf (command = "chmotd") Then

            Wln("Usage: chmotd <Message>", "neutralText")

        ElseIf (command = "chmal") Then

            Wln("Usage: chmal <Message>", "neutralText")

        ElseIf (command = "choice") Then

            Wln("Usage: choice <Question> <sets>" + vbNewLine + _
                "       choice: to be prompted about choices. (deprecated)", "neutralText")

        ElseIf (command = "chpwd") Then

            Wln("Usage: chpwd: to be prompted about changing passwords. (deprecated)", "neutralText")

        ElseIf (command = "chusrname") Then

            Wln("Usage: chusrname <oldUserName> <newUserName>" + vbNewLine + _
                "       chusrname: to be prompted about changing usernames. (deprecated)", "neutralText")

        ElseIf (command = "cls") Then

            Wln("Usage: cls: to clear screen.", "neutralText")

        ElseIf (command = "debuglog") Then

            Wln("Usage: debuglog: Shows you debug logs so you can send the log to us.", "neutralText")

        ElseIf (command = "disco") Then

            Wln("Usage: disco: to get a disco effect on the console. True color support will come with GUI console.", "neutralText")

        ElseIf (command = "echo") Then

            Wln("Usage: echo <text>" + vbNewLine + _
                "       echo: to be prompted about text printing. (deprecated)", "neutralText")

        ElseIf (command = "hwprobe") Then

            Wln("Usage: hwprobe: Probes hardware (Only works when the hardware is not probed and hwprobe is not executed).", "neutralText")

        ElseIf (command.Contains("ls") Or command.Contains("list")) Then

            If (command = "ls" Or command = "list") Then
                Wln("Usage: ls/list [oneDirectory]" + vbNewLine + _
                    "       ls/list: to get current directory.", "neutralText")
            ElseIf (command = "lsdrivers") Then
                Wln("Usage: lsdrivers: Lists probed drivers." + vbNewLine + _
                    "       Friends of lsdrivers: sysinfo, version", "neutralText")
            ElseIf (command = "lsnet") Then
                Wln("Usage: lsnet: Lists network information, as well as every computer connected to a network." + vbNewLine + _
                    "       Friends of lsnet: lsnettree", "neutralText")
            ElseIf (command = "lsnettree") Then
                Wln("Usage: lsnettree: Lists network information, as well as every computer connected to a network, in a tree form." + vbNewLine + _
                    "       Friends of lsnettree: lsnet", "neutralText")
            End If

        ElseIf (command = "logout") Then

            Wln("Usage: logout: Logs you out of the user." + vbNewLine + _
                "       Friends of logout: reboot, shutdown", "neutralText")

        ElseIf (command = "mkdir") Or (command = "md") Then

            Wln("Usage: md/mkdir <anything>", "neutralText")

        ElseIf (command = "netinfo") Then

            Wln("Usage: netinfo: Get every network information", "neutralText")

        ElseIf (command = "panicsim") Then

            Wln("Usage: panicsim <message> [S/F/D/[C]/U] [RebootTime:Seconds]" + vbNewLine + _
                "       panicsim: to be prompted about panic simulator options. (deprecated)", "neutralText")

        ElseIf (command = "perm") Then

            Wln("Usage: perm <userName> <Admin/Disabled> <Allow/Disallow>" + vbNewLine + _
                "       perm: to be prompted about permission setting. (deprecated)", "neutralText")

        ElseIf (command = "ping") Then

            Wln("Usage: ping <Address> [repeatTimes]" + vbNewLine + _
                "       ping: to get prompted about writing address. (deprecated)", "neutralText")

        ElseIf (command = "rmdir") Or (command = "rd") Then

            Wln("Usage: rd/rmdir <directory>", "neutralText")

        ElseIf (command = "read") Then

            Wln("Usage: read <file>" + vbNewLine + _
                "       read: to get prompted about reading file contents. (deprecated)", "neutralText")

        ElseIf (command = "reboot") Then

            Wln("Usage: reboot: Restarts your simulated computer." + vbNewLine + _
                "       Friends of reboot: shutdown, logout", "neutralText")

        ElseIf (command = "reloadconfig") Then

            Wln("Usage: reloadconfig: Reloads the configuration that is changed by the end-user or by tool." + vbNewLine + _
                "       Colors doesn't require a restart, but most of the settings require you to restart.", "neutralText")

        ElseIf (command = "rmuser") Then

            Wln("Usage: rmuser <Username>" + vbNewLine + _
                "       rmuser: to get prompted about removing usernames.", "neutralText")

        ElseIf (command = "scical") Then

            Wln("Usage: scical <expression1|pi|e> <+|-|*|/|%> <expression2|pi|e> ..." + vbNewLine + _
                "       scical <sqrt|tan|sin|cos> <number>", "neutralText")

        ElseIf (command = "setcolors") Then

            Wln("Usage: setcolors <inputColor/def> <licenseColor/def> <contKernelErrorColor/def> <uncontKernelErrorColor/def> <hostNameShellColor/def> <userNameShellColor/def> <backgroundColor/def> <neutralTextColor/def>" + vbNewLine + _
                "       setcolors: to get prompted about setting colors. (deprecated)" + vbNewLine + _
                "       Friends of setcolors: setthemes", "neutralText")

        ElseIf (command = "setthemes") Then

            Wln("Usage: setthemes <Theme>" + vbNewLine + _
                "       setthemes: to get prompted about setting themes. (deprecated)" + vbNewLine + _
                "       Friends of setthemes: setcolors", "neutralText")

        ElseIf (command = "showmotd") Then

            Wln("Usage: showmotd: Shows your current Message Of The Day.", "neutralText")

        ElseIf (command = "showtd") Then

            Wln("Usage: showtd: Shows the date and time.", "neutralText")

        ElseIf (command = "showtdzone") Then

            Wln("Usage: showtdzone: Shows the date and time in zones.", "neutralText")

        ElseIf (command = "shutdown") Then

            Wln("Usage: shutdown: Shuts down your simulated computer." + vbNewLine + _
                "       Friends of shutdown: reboot, logout", "neutralText")

        ElseIf (command = "sysinfo") Then

            Wln("Usage: sysinfo: Shows system information and versions." + vbNewLine + _
                "       Friends of sysinfo: lsdrivers, version", "neutralText")

        ElseIf (command = "unitconv") Then

            Wln("Usage: unitconv <sourceUnit> <targetUnit> <value>" + vbNewLine + _
                "Units: B, KB, MB, GB, TB, Bits, Octal, Binary, Decimal, Hexadecimal, mm, cm, m, km, Fahrenheit, Celsius, Kelvin, " + _
                "Reaumur, Romer, Delisle, Rankine, j, kj, m/s, km/h, cm/ms, Kilograms, Grams, Tons, Kilotons, Megatons, kn, n, Hz, kHz, MHz, " + _
                "GHz, Number (source only), Money (target only), Percent (target only), Centivolts, Volts, Kilovolts, Watts, Kilowatts, " + _
                "Milliliters, Liters, Kiloliters, Gallons, Ounces, Feet, Inches, Yards and Miles.", "neutralText")

        ElseIf (command = "version") Then

            Wln("Usage: version: Shows kernel version." + vbNewLine + _
                "       Friends of version: lsdrivers, sysinfo", "neutralText")

        End If

    End Sub

End Module
