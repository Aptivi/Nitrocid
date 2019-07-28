
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
                                                              {"cdbglog", DoTranslation("Deletes everything in debug log", currentLang)},
                                                              {"chdir", DoTranslation("Changes directory", currentLang)},
                                                              {"chhostname", DoTranslation("Changes host name (Admins only)", currentLang)},
                                                              {"chlang", DoTranslation("Changes language", currentLang)},
                                                              {"chmal", DoTranslation("Changes MAL, the MOTD After Login (Admins only)", currentLang)},
                                                              {"chmotd", DoTranslation("Changes MOTD, the Message Of The Day (Admins only)", currentLang)},
                                                              {"chpwd", DoTranslation("Changes password for current user", currentLang)},
                                                              {"chusrname", DoTranslation("Changes user name (Admins Only)", currentLang)},
                                                              {"cls", DoTranslation("Clears the screen", currentLang)},
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
                                                              {"setcolors", DoTranslation("Sets up kernel colors", currentLang)},
                                                              {"setsaver", DoTranslation("Sets up kernel screensavers", currentLang)},
                                                              {"setthemes", DoTranslation("Sets up kernel themes", currentLang)},
                                                              {"showaliases", DoTranslation("Shows aliases", currentLang)},
                                                              {"showmal", DoTranslation("Shows MOTD after login set by user or kernel", currentLang)},
                                                              {"showmotd", DoTranslation("Shows message of the day set by user or kernel", currentLang)},
                                                              {"showtd", DoTranslation("Shows date and time", currentLang)},
                                                              {"showtdzone", DoTranslation("Shows date and time in zones", currentLang)},
                                                              {"shutdown", DoTranslation("The kernel will be shut down", currentLang)},
                                                              {"sses", DoTranslation("Gets SSE versions", currentLang)},
                                                              {"sysinfo", DoTranslation("System information", currentLang)},
                                                              {"useddeps", DoTranslation("Shows used open source dependencies", currentLang)},
                                                              {"usermanual", DoTranslation("A very descriptive user manual.", currentLang)}}
    End Sub

    Public Sub ShowHelp(Optional ByVal command As String = "")

        Dim wholesslist As String() = IO.Directory.GetFiles(paths("Mods"), "*SS.m", IO.SearchOption.TopDirectoryOnly)
        If command = "" Then

            If simHelp = False Then
                For Each cmd As String In definitions.Keys
                    W("- {0}: ", False, "helpCmd", cmd) : W("{0}", True, "helpDef", definitions(cmd))
                Next
                For Each cmd As String In moddefs.Keys
                    W("- {0}: ", False, "helpCmd", cmd) : W("{0}", True, "helpDef", moddefs(cmd))
                Next
                For Each cmd As String In aliases.Keys
                    W("- {0}: ", False, "helpCmd", cmd) : W("{0}", True, "helpDef", definitions(aliases(cmd)))
                Next
                W(DoTranslation("* You can use multiple commands using the colon between commands.", currentLang), True, "neutralText")
            Else
                For Each cmd As String In aliases.Keys
                    W("{0}({1}), ", False, "helpCmd", cmd, aliases(cmd))
                Next
                For Each cmd As String In moddefs.Keys
                    W("{0}, ", False, "helpCmd", cmd)
                Next
                W(String.Join(", ", availableCommands), True, "helpCmd")
            End If

        ElseIf command = "adduser" Then

            W(DoTranslation("Usage:", currentLang) + " adduser <userName> [password] [confirm]", True, "neutralText")

        ElseIf command = "alias" Then

            W(DoTranslation("Usage:", currentLang) + " alias <rem/add> <alias> <cmd>", True, "neutralText")

        ElseIf command = "arginj" Then

            W(DoTranslation("Usage:", currentLang) + " arginj [Arguments separated by spaces]" + vbNewLine +
                "       " + DoTranslation("where arguments will be {0}", currentLang), True, "neutralText", String.Join(", ", AvailableArgs))

        ElseIf command = "cdbglog" Then

            W(DoTranslation("Usage:", currentLang) + " cdbglog: " + DoTranslation("Deletes everything in debug log", currentLang), True, "neutralText")

        ElseIf command = "chdir" Then

            W(DoTranslation("Usage:", currentLang) + " chdir <directory/..>", True, "neutralText")

        ElseIf command = "chhostname" Then

            W(DoTranslation("Usage:", currentLang) + " chhostname <HostName>", True, "neutralText")

        ElseIf command = "chlang" Then

            W(DoTranslation("Usage:", currentLang) + " chlang <language>" + vbNewLine +
                "<language>: " + String.Join("/", availableLangs), True, "neutralText")

        ElseIf command = "chmotd" Then

            W(DoTranslation("Usage:", currentLang) + " chmotd <Message>", True, "neutralText")

        ElseIf command = "chmal" Then

            W(DoTranslation("Usage:", currentLang) + " chmal <Message>", True, "neutralText")

        ElseIf command = "chpwd" Then

            W(DoTranslation("Usage:", currentLang) + " chpwd <Username> <UserPass> <newPass> <confirm>", True, "neutralText")

        ElseIf command = "chusrname" Then

            W(DoTranslation("Usage:", currentLang) + " chusrname <oldUserName> <newUserName>", True, "neutralText")

        ElseIf command = "cls" Then

            W(DoTranslation("Usage:", currentLang) + " cls: " + DoTranslation("to clear screen.", currentLang), True, "neutralText")

        ElseIf command = "currency" Then

            W(DoTranslation("Usage:", currentLang) + " currency <SourceCountry3Letters> <DestCountry3Letters> <AmountOfMoney>", True, "neutralText")

        ElseIf command = "debuglog" Then

            W(DoTranslation("Usage:", currentLang) + " debuglog: " + DoTranslation("Shows you debug logs so you can send the log to us.", currentLang), True, "neutralText")

        ElseIf command = "ftp" Then

            W(DoTranslation("Usage:", currentLang) + " ftp: " + DoTranslation("Initializes the FTP shell.", currentLang), True, "neutralText")

        ElseIf command = "list" Then
            W(DoTranslation("Usage:", currentLang) + " list [oneDirectory]" + vbNewLine +
                "       list: " + DoTranslation("to get current directory.", currentLang), True, "neutralText")

        ElseIf command = "lscomp" Then
            W(DoTranslation("Usage:", currentLang) + " lscomp: " + DoTranslation("Lists network information, as well as every computer.", currentLang) + vbNewLine +
                "       " + DoTranslation("Friends of", currentLang) + " lscomp: lsnet, lsnettree", True, "neutralText")

        ElseIf command = "lsnet" Then
            W(DoTranslation("Usage:", currentLang) + " lsnet: " + DoTranslation("Lists network information, as well as every computer connected to a network.", currentLang) + vbNewLine +
                "       " + DoTranslation("Friends of", currentLang) + " lsnet: lscomp, lsnettree", True, "neutralText")

        ElseIf command = "lsnettree" Then
            W(DoTranslation("Usage:", currentLang) + " lsnettree: " + DoTranslation("Lists network information, as well as every computer connected to a network, in a tree form.", currentLang) + vbNewLine +
                "       " + DoTranslation("Friends of", currentLang) + " lsnettree: lscomp, lsnet", True, "neutralText")

        ElseIf command = "reloadsaver" Then

            W(DoTranslation("Usage:", currentLang) + " reloadsaver <modNameSS.m>" + vbNewLine +
                "       " + DoTranslation("where modnameSS.m will be", currentLang) + " {0}", True, "neutralText", String.Join(", ", wholesslist))

        ElseIf command = "lockscreen" Then

            W(DoTranslation("Usage:", currentLang) + " lockscreen: " + DoTranslation("Locks your screen with the password.", currentLang) + vbNewLine +
                "       " + DoTranslation("Friends of", currentLang) + " lockscreen: savescreen", True, "neutralText")

        ElseIf command = "logout" Then

            W(DoTranslation("Usage:", currentLang) + " logout: " + DoTranslation("Logs you out of the user.", currentLang) + vbNewLine +
                "       " + DoTranslation("Friends of", currentLang) + " logout: reboot, shutdown", True, "neutralText")

        ElseIf command = "md" Then

            W(DoTranslation("Usage:", currentLang) + " md <anything>", True, "neutralText")

        ElseIf command = "netinfo" Then

            W(DoTranslation("Usage:", currentLang) + " netinfo: " + DoTranslation("Get every network information", currentLang), True, "neutralText")

        ElseIf command = "noaliases" Then

            W(DoTranslation("Usage:", currentLang) + " noaliases: " + DoTranslation("Shows forbidden list of aliases", currentLang), True, "neutralText")

        ElseIf command = "perm" Then

            W(DoTranslation("Usage:", currentLang) + " perm <userName> <Admin/Disabled> <Allow/Disallow>", True, "neutralText")

        ElseIf command = "ping" Then

            W(DoTranslation("Usage:", currentLang) + " ping <Address> [repeatTimes]", True, "neutralText")

        ElseIf command = "rd" Then

            W(DoTranslation("Usage:", currentLang) + " rd <directory>", True, "neutralText")

        ElseIf command = "read" Then

            W(DoTranslation("Usage:", currentLang) + " read <file>", True, "neutralText")

        ElseIf command = "reboot" Then

            W(DoTranslation("Usage:", currentLang) + " reboot: " + DoTranslation("Restarts your simulated computer.", currentLang) + vbNewLine +
                "       " + DoTranslation("Friends of", currentLang) + " reboot: shutdown, logout", True, "neutralText")

        ElseIf command = "reloadconfig" Then

            W(DoTranslation("Usage:", currentLang) + " reloadconfig: " + DoTranslation("Reloads the configuration that is changed by the end-user or by tool.", currentLang) + vbNewLine +
                "       " + DoTranslation("Colors doesn't require a restart, but most of the settings require you to restart.", currentLang), True, "neutralText")

        ElseIf command = "rmuser" Then

            W(DoTranslation("Usage:", currentLang) + " rmuser <Username>", True, "neutralText")

        ElseIf command = "savescreen" Then

            W(DoTranslation("Usage:", currentLang) + " savescreen: " + DoTranslation("shows you a selected screensaver, while protecting your screen from burn outs.", currentLang) + vbNewLine +
                "       " + DoTranslation("Friends of", currentLang) + " savescreen: lockscreen", True, "neutralText")

        ElseIf command = "setcolors" Then

            W(DoTranslation("Usage:", currentLang) + " setcolors <inputColor/def/RESET> <licenseColor/def/RESET> <contKernelErrorColor/def/RESET> <uncontKernelErrorColor/def/RESET> <hostNameShellColor/def/RESET> <userNameShellColor/def/RESET> <backgroundColor/def/RESET> <neutralTextColor/def/RESET> <cmdListColor/def/RESET> <cmdDefColor/def/RESET>" + vbNewLine +
                "       " + DoTranslation("Friends of", currentLang) + " setcolors: setthemes", True, "neutralText")

        ElseIf command = "setsaver" Then

            W(DoTranslation("Usage:", currentLang) + " setsaver <modNameSS.m/matrix/disco/colorMix>" + vbNewLine +
                "       " + DoTranslation("where modnameSS.m will be", currentLang) + " {0}", True, "neutralText", String.Join(", ", wholesslist))

        ElseIf command = "setthemes" Then

            W(DoTranslation("Usage:", currentLang) + " setthemes <Theme>" + vbNewLine +
                "       " + DoTranslation("Friends of", currentLang) + " setthemes: setcolors", True, "neutralText")

        ElseIf command = "showaliases" Then

            W(DoTranslation("Usage:", currentLang) + " showaliases: " + DoTranslation("Shows aliases", currentLang), True, "neutralText")

        ElseIf command = "showmotd" Then

            W(DoTranslation("Usage:", currentLang) + " showmotd: " + DoTranslation("Shows your current Message Of The Day.", currentLang), True, "neutralText")

        ElseIf command = "showtd" Then

            W(DoTranslation("Usage:", currentLang) + " showtd: " + DoTranslation("Shows the date and time.", currentLang), True, "neutralText")

        ElseIf command = "showtdzone" Then

            W(DoTranslation("Usage:", currentLang) + " showtdzone: " + DoTranslation("Shows the date and time in zones.", currentLang), True, "neutralText")

        ElseIf command = "shutdown" Then

            W(DoTranslation("Usage:", currentLang) + " shutdown: " + DoTranslation("Shuts down your simulated computer.", currentLang) + vbNewLine +
                "       " + DoTranslation("Friends of", currentLang) + " shutdown: reboot, logout", True, "neutralText")

        ElseIf command = "sses" Then

            W(DoTranslation("Usage:", currentLang) + " sses: " + DoTranslation("Gets SSE versions", currentLang), True, "neutralText")

        ElseIf command = "sysinfo" Then

            W(DoTranslation("Usage:", currentLang) + " sysinfo: " + DoTranslation("Shows system information and versions.", currentLang) + vbNewLine +
                "       " + DoTranslation("Friends of", currentLang) + " sysinfo: version", True, "neutralText")

        ElseIf command = "useddeps" Then

            W(DoTranslation("Usage:", currentLang) + " useddeps: " + DoTranslation("Shows open source libraries used", currentLang), True, "neutralText")

        ElseIf command = "usermanual" Then

            W(DoTranslation("Usage:", currentLang) + " usermanual <sectionWord>", True, "neutralText")

        End If

    End Sub

End Module
