
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

Public Module HelpSystem

    'This dictionary is the definitions for commands.
    Public definitions As Dictionary(Of String, String)
    Public moddefs As New Dictionary(Of String, String)

    Public Sub InitHelp()
        definitions = New Dictionary(Of String, String) From {{"adduser", DoTranslation("Adds users (Only admins can access this command)", currentLang)},
                                                             {"alias", DoTranslation("Adds aliases to commands (Only admins can access this command)", currentLang)},
                                                             {"arginj", DoTranslation("Injects arguments to the kernel (reboot required, admins only)", currentLang)},
                                                             {"calc", DoTranslation("Simple calculator", currentLang)},
                                                             {"chdir", DoTranslation("Changes directory", currentLang)},
                                                             {"chhostname", DoTranslation("Changes host name (Admins only)", currentLang)},
                                                             {"chmal", DoTranslation("Changes MAL, the MOTD After Login (Admins only)", currentLang)},
                                                             {"chmotd", DoTranslation("Changes MOTD, the Message Of The Day (Admins only)", currentLang)},
                                                             {"chpwd", DoTranslation("Changes password for current user", currentLang)},
                                                             {"chusrname", DoTranslation("Changes user name (Admins Only)", currentLang)},
                                                             {"cls", DoTranslation("Clears the screen", currentLang)},
                                                             {"currency", DoTranslation("Convert amount of money from country to country", currentLang)},
                                                             {"debuglog", DoTranslation("Shows debug logs (Admins Only)", currentLang)},
                                                             {"ftp", DoTranslation("Use an FTP shell to interact with servers", currentLang)},
                                                             {"help", DoTranslation("Help page", currentLang)},
                                                             {"list", DoTranslation("List file/folder contents in current folder", currentLang)},
                                                             {"lockscreen", DoTranslation("Locks your screen with a password", currentLang)},
                                                             {"logout", DoTranslation("Logs you out", currentLang)},
                                                             {"lscomp", DoTranslation("Lists all network devices who is online (and offline in domains)", currentLang)},
                                                             {"lsnet", DoTranslation("Lists all network addresses on host", currentLang)},
                                                             {"lsnettree", DoTranslation("Lists all network addresses on host using the tree", currentLang)},
                                                             {"md", DoTranslation("Creates a directory", currentLang)},
                                                             {"netinfo", DoTranslation("Lists information about all available interfaces", currentLang)},
                                                             {"noaliases", DoTranslation("Shows forbidden list of aliases", currentLang)},
                                                             {"perm", DoTranslation("Manage permissions for users (Only admins can access this command)", currentLang)},
                                                             {"ping", DoTranslation("Check to see if specified address is available", currentLang)},
                                                             {"read", DoTranslation("Writes file contents to the console", currentLang)},
                                                             {"reboot", DoTranslation("Restarts your computer (WARNING: No syncing, because it is not a final kernel)", currentLang)},
                                                             {"reloadconfig", DoTranslation("Reloads configuration file that is edited.", currentLang)},
                                                             {"reloadsaver", DoTranslation("Reloads screensaver file in %USERPROFILE%\KSMods", currentLang)},
                                                             {"rd", DoTranslation("Removes a directory", currentLang)},
                                                             {"rmuser", DoTranslation("Removes a user from the list (Admins Only)", currentLang)},
                                                             {"savescreen", DoTranslation("Saves your screen from burn outs", currentLang)},
                                                             {"scical", DoTranslation("Scientific calculator. The unit converter is separated to another command", currentLang)},
                                                             {"setcolors", DoTranslation("Sets up kernel colors", currentLang)},
                                                             {"setsaver", DoTranslation("Sets up kernel screensavers", currentLang)},
                                                             {"setthemes", DoTranslation("Sets up kernel themes", currentLang)},
                                                             {"showaliases", DoTranslation("Shows aliases", currentLang)},
                                                             {"showmal", DoTranslation("Shows MOTD after login set by user or kernel", currentLang)},
                                                             {"showmotd", DoTranslation("Shows message of the day set by user or kernel", currentLang)},
                                                             {"showtd", DoTranslation("Shows date and time", currentLang)},
                                                             {"showtdzone", DoTranslation("Shows date and time in zones", currentLang)},
                                                             {"shutdown", DoTranslation("The kernel will be shut down", currentLang)},
                                                             {"sysinfo", DoTranslation("System information", currentLang)},
                                                             {"unitconv", DoTranslation("Unit converter that is separated from scicalc.", currentLang)},
                                                             {"useddeps", DoTranslation("Shows used open source dependencies", currentLang)},
                                                             {"usermanual", DoTranslation("A very descriptive user manual.", currentLang)}}
    End Sub

    Public Sub ShowHelp(Optional ByVal command As String = "")

        Dim wholesslist As String() = IO.Directory.GetFiles(paths("Mods"), "*SS.m", IO.SearchOption.TopDirectoryOnly)
        If (command = "") Then

            If (simHelp = False) Then
                For Each cmd As String In definitions.Keys
                    W("- {0}: ", "helpCmd", cmd) : Wln("{0}", "helpDef", definitions(cmd))
                Next
                For Each cmd As String In moddefs.Keys
                    W("- {0}: ", "helpCmd", cmd) : Wln("{0}", "helpDef", moddefs(cmd))
                Next
                For Each cmd As String In aliases.Keys
                    W("- {0}: ", "helpCmd", cmd) : Wln("{0}", "helpDef", definitions(aliases(cmd)))
                Next
                Wln(DoTranslation("* You can use multiple commands using the colon between commands.", currentLang), "neutralText")
            Else
                For Each cmd As String In aliases.Keys
                    W("{0}({1}), ", "helpCmd", cmd, aliases(cmd))
                Next
                For Each cmd As String In moddefs.Keys
                    W("{0}, ", "helpCmd", cmd)
                Next
                Wln(String.Join(", ", availableCommands), "neutralText")
            End If

        ElseIf (command = "adduser") Then

            Wln(DoTranslation("Usage:", currentLang) + " adduser <userName> [password] [confirm]", "neutralText")

        ElseIf (command = "alias") Then

            Wln(DoTranslation("Usage:", currentLang) + " alias <rem/add> <alias> <cmd>", "neutralText")

        ElseIf (command = "arginj") Then

            Wln(DoTranslation("Usage:", currentLang) + " arginj [Arguments separated by spaces]" + vbNewLine +
                "       " + DoTranslation("where arguments will be {0}", currentLang), "neutralText", String.Join(", ", AvailableArgs))

        ElseIf (command = "calc") Then

            Wln(DoTranslation("Usage:", currentLang) + " calc <expression> ...", "neutralText")

        ElseIf (command = "chdir") Then

            Wln(DoTranslation("Usage:", currentLang) + " chdir <directory/..>", "neutralText")

        ElseIf (command = "chhostname") Then

            Wln(DoTranslation("Usage:", currentLang) + " chhostname <HostName>", "neutralText")

        ElseIf (command = "chmotd") Then

            Wln(DoTranslation("Usage:", currentLang) + " chmotd <Message>", "neutralText")

        ElseIf (command = "chmal") Then

            Wln(DoTranslation("Usage:", currentLang) + " chmal <Message>", "neutralText")

        ElseIf (command = "chpwd") Then

            Wln(DoTranslation("Usage:", currentLang) + " chpwd <Username> <UserPass> <newPass> <confirm>", "neutralText")

        ElseIf (command = "chusrname") Then

            Wln(DoTranslation("Usage:", currentLang) + " chusrname <oldUserName> <newUserName>", "neutralText")

        ElseIf (command = "cls") Then

            Wln(DoTranslation("Usage:", currentLang) + " cls: " + DoTranslation("to clear screen.", currentLang), "neutralText")

        ElseIf (command = "currency") Then

            Wln(DoTranslation("Usage:", currentLang) + " currency <SourceCountry3Letters> <DestCountry3Letters> <AmountOfMoney>" + vbNewLine +
                DoTranslation("Currencies:", currentLang) + vbNewLine, "neutralText")
            For Each CInfo As CurrencyInfo In GetAllCurrencies()
                Wln("- {0}({2}): {1}", "neutralText", CInfo.CountryName, CInfo.CurrencyName, CInfo.ID)
            Next

        ElseIf (command = "debuglog") Then

            Wln(DoTranslation("Usage:", currentLang) + " debuglog: " + DoTranslation("Shows you debug logs so you can send the log to us.", currentLang), "neutralText")

        ElseIf (command = "ftp") Then

            Wln(DoTranslation("Usage:", currentLang) + " ftp: " + DoTranslation("Initializes the FTP shell.", currentLang), "neutralText")

        ElseIf (command = "list") Then
            Wln(DoTranslation("Usage:", currentLang) + " list [oneDirectory]" + vbNewLine +
                "       list: " + DoTranslation("to get current directory.", currentLang), "neutralText")

        ElseIf (command = "lscomp") Then
            Wln(DoTranslation("Usage:", currentLang) + " lscomp: " + DoTranslation("Lists network information, as well as every computer.", currentLang) + vbNewLine +
                "       " + DoTranslation("Friends of", currentLang) + " lscomp: lsnet, lsnettree", "neutralText")

        ElseIf (command = "lsnet") Then
            Wln(DoTranslation("Usage:", currentLang) + " lsnet: " + DoTranslation("Lists network information, as well as every computer connected to a network.", currentLang) + vbNewLine +
                "       " + DoTranslation("Friends of", currentLang) + " lsnet: lscomp, lsnettree", "neutralText")

        ElseIf (command = "lsnettree") Then
            Wln(DoTranslation("Usage:", currentLang) + " lsnettree: " + DoTranslation("Lists network information, as well as every computer connected to a network, in a tree form.", currentLang) + vbNewLine +
                "       " + DoTranslation("Friends of", currentLang) + " lsnettree: lscomp, lsnet", "neutralText")

        ElseIf (command = "reloadsaver") Then

            Wln(DoTranslation("Usage:", currentLang) + " reloadsaver <modNameSS.m>" + vbNewLine +
                "       " + DoTranslation("where modnameSS.m will be", currentLang) + " {0}", "neutralText", String.Join(", ", wholesslist))

        ElseIf (command = "lockscreen") Then

            Wln(DoTranslation("Usage:", currentLang) + " lockscreen: " + DoTranslation("Locks your screen with the password.", currentLang) + vbNewLine +
                "       " + DoTranslation("Friends of", currentLang) + " lockscreen: savescreen", "neutralText")

        ElseIf (command = "logout") Then

            Wln(DoTranslation("Usage:", currentLang) + " logout: " + DoTranslation("Logs you out of the user.", currentLang) + vbNewLine +
                "       " + DoTranslation("Friends of", currentLang) + " logout: reboot, shutdown", "neutralText")

        ElseIf (command = "md") Then

            Wln(DoTranslation("Usage:", currentLang) + " md <anything>", "neutralText")

        ElseIf (command = "netinfo") Then

            Wln(DoTranslation("Usage:", currentLang) + " netinfo: " + DoTranslation("Get every network information", currentLang), "neutralText")

        ElseIf (command = "noaliases") Then

            Wln(DoTranslation("Usage:", currentLang) + " noaliases: " + DoTranslation("Shows forbidden list of aliases", currentLang), "neutralText")

        ElseIf (command = "perm") Then

            Wln(DoTranslation("Usage:", currentLang) + " perm <userName> <Admin/Disabled> <Allow/Disallow>", "neutralText")

        ElseIf (command = "ping") Then

            Wln(DoTranslation("Usage:", currentLang) + " ping <Address> [repeatTimes]", "neutralText")

        ElseIf (command = "rd") Then

            Wln(DoTranslation("Usage:", currentLang) + " rd <directory>", "neutralText")

        ElseIf (command = "read") Then

            Wln(DoTranslation("Usage:", currentLang) + " read <file>", "neutralText")

        ElseIf (command = "reboot") Then

            Wln(DoTranslation("Usage:", currentLang) + " reboot: " + DoTranslation("Restarts your simulated computer.", currentLang) + vbNewLine +
                "       " + DoTranslation("Friends of", currentLang) + " reboot: shutdown, logout", "neutralText")

        ElseIf (command = "reloadconfig") Then

            Wln(DoTranslation("Usage:", currentLang) + " reloadconfig: " + DoTranslation("Reloads the configuration that is changed by the end-user or by tool.", currentLang) + vbNewLine +
                "       " + DoTranslation("Colors doesn't require a restart, but most of the settings require you to restart.", currentLang), "neutralText")

        ElseIf (command = "rmuser") Then

            Wln(DoTranslation("Usage:", currentLang) + " rmuser <Username>", "neutralText")

        ElseIf (command = "savescreen") Then

            Wln(DoTranslation("Usage:", currentLang) + " savescreen: " + DoTranslation("shows you a selected screensaver, while protecting your screen from burn outs.", currentLang) + vbNewLine +
                "       " + DoTranslation("Friends of", currentLang) + " savescreen: lockscreen", "neutralText")

        ElseIf (command = "scical") Then

            Wln(DoTranslation("Usage:", currentLang) + " scical <expression1|pi|e> <+|-|*|/|%> <expression2|pi|e> ..." + vbNewLine +
                "       scical <sqrt|tan|sin|cos> <number>", "neutralText")

        ElseIf (command = "setcolors") Then

            Wln(DoTranslation("Usage:", currentLang) + " setcolors <inputColor/def/RESET> <licenseColor/def/RESET> <contKernelErrorColor/def/RESET> <uncontKernelErrorColor/def/RESET> <hostNameShellColor/def/RESET> <userNameShellColor/def/RESET> <backgroundColor/def/RESET> <neutralTextColor/def/RESET> <cmdListColor/def/RESET> <cmdDefColor/def/RESET>" + vbNewLine +
                "       " + DoTranslation("Friends of", currentLang) + " setcolors: setthemes", "neutralText")

        ElseIf (command = "setsaver") Then

            Wln(DoTranslation("Usage:", currentLang) + " setsaver <modNameSS.m/matrix/disco/colorMix>" + vbNewLine +
                "       " + DoTranslation("where modnameSS.m will be", currentLang) + " {0}", "neutralText", String.Join(", ", wholesslist))

        ElseIf (command = "setthemes") Then

            Wln(DoTranslation("Usage:", currentLang) + " setthemes <Theme>" + vbNewLine +
                "       " + DoTranslation("Friends of", currentLang) + " setthemes: setcolors", "neutralText")

        ElseIf (command = "showaliases") Then

            Wln(DoTranslation("Usage:", currentLang) + " showaliases: " + DoTranslation("Shows aliases", currentLang), "neutralText")

        ElseIf (command = "showmotd") Then

            Wln(DoTranslation("Usage:", currentLang) + " showmotd: " + DoTranslation("Shows your current Message Of The Day.", currentLang), "neutralText")

        ElseIf (command = "showtd") Then

            Wln(DoTranslation("Usage:", currentLang) + " showtd: " + DoTranslation("Shows the date and time.", currentLang), "neutralText")

        ElseIf (command = "showtdzone") Then

            Wln(DoTranslation("Usage:", currentLang) + " showtdzone: " + DoTranslation("Shows the date and time in zones.", currentLang), "neutralText")

        ElseIf (command = "shutdown") Then

            Wln(DoTranslation("Usage:", currentLang) + " shutdown: " + DoTranslation("Shuts down your simulated computer.", currentLang) + vbNewLine +
                "       " + DoTranslation("Friends of", currentLang) + " shutdown: reboot, logout", "neutralText")

        ElseIf (command = "sysinfo") Then

            Wln(DoTranslation("Usage:", currentLang) + " sysinfo: " + DoTranslation("Shows system information and versions.", currentLang) + vbNewLine +
                "       " + DoTranslation("Friends of", currentLang) + " sysinfo: version", "neutralText")

        ElseIf (command = "unitconv") Then

            Wln(DoTranslation("Usage:", currentLang) + " unitconv <sourceUnit> <targetUnit> <value>" + vbNewLine +
                "Units: B, KB, MB, GB, TB, Bits, Octal, Binary, Decimal, Hexadecimal, mm, cm, m, km, Fahrenheit, Celsius, Kelvin, " +
                "Reaumur, Romer, Delisle, Rankine, j, kj, m/s, km/h, cm/ms, Kilograms, Grams, Tons, Kilotons, Megatons, kn, n, Hz, kHz, MHz, " +
                "GHz, Number (source only), Money (target only), Percent (target only), Centivolts, Volts, Kilovolts, Watts, Kilowatts, " +
                "Milliliters, Liters, Kiloliters, Gallons, Ounces, Feet, Inches, Yards and Miles.", "neutralText")

        ElseIf (command = "useddeps") Then

            Wln(DoTranslation("Usage:", currentLang) + " useddeps: " + DoTranslation("Shows open source libraries used", currentLang), "neutralText")

        ElseIf (command = "usermanual") Then

            Wln(DoTranslation("Usage:", currentLang) + " usermanual <sectionWord>", "neutralText")

        End If

    End Sub

End Module
