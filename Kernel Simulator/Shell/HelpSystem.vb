
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

Public Module HelpSystem

    'This dictionary is the definitions for commands.
    Public definitions As New Dictionary(Of String, String) From {{"adduser", "Adds users (Only admins can access this command)"}, _
                                                                  {"alias", "Adds aliases to commands (Only admins can access this command)"}, _
                                                                  {"arginj", "Injects arguments to the kernel (reboot required, admins only)"}, _
                                                                  {"calc", "Simple calculator"}, _
                                                                  {"cdir", "Shows current directory"}, _
                                                                  {"changedir", "Changes directory"}, _
                                                                  {"chhostname", "Changes host name (Admins only)"}, _
                                                                  {"chmal", "Changes MAL, the MOTD After Login (Admins only)"}, _
                                                                  {"chmotd", "Changes MOTD, the Message Of The Day (Admins only)"}, _
                                                                  {"chpwd", "Changes password for current user"}, _
                                                                  {"chusrname", "Changes user name (Admins Only)"}, _
                                                                  {"choice", "Makes user choices"}, _
                                                                  {"cls", "Clears the screen"}, _
                                                                  {"debuglog", "Shows debug logs (Admins Only)"}, _
                                                                  {"echo", "Writes a text into a console"}, _
                                                                  {"ftp", "Use an FTP shell to interact with servers"}, _
                                                                  {"help", "Help page"}, _
                                                                  {"hwprobe", "Probe hardware manually (One time in 'nohwprobe' kernel)"}, _
                                                                  {"list", "List file/folder contents in current folder"}, _
                                                                  {"loadsaver", "Loads screensaver file in %USERPROFILE%\KSMods"}, _
                                                                  {"lockscreen", "Locks your screen with a password"}, _
                                                                  {"logout", "Logs you out"}, _
                                                                  {"lscomp", "Lists all network devices who is online (and offline in domains)"}, _
                                                                  {"lsnet", "Lists all network addresses on host"}, _
                                                                  {"lsnettree", "Lists all network addresses on host using the tree"}, _
                                                                  {"md", "Creates a directory"}, _
                                                                  {"netinfo", "Lists information about all available interfaces"}, _
                                                                  {"noaliases", "Shows forbidden list of aliases"}, _
                                                                  {"panicsim", "Kernel Panic Simulator (real)"}, _
                                                                  {"perm", "Manage permissions for users (Only admins can access this command)"}, _
                                                                  {"ping", "Check to see if specified address is available"}, _
                                                                  {"read", "Writes file contents to the console"}, _
                                                                  {"reboot", "Restarts your computer (WARNING: No syncing, because it is not a final kernel)"}, _
                                                                  {"reloadconfig", "Reloads configuration file that is edited."}, _
                                                                  {"rd", "Removes a directory"}, _
                                                                  {"rmuser", "Removes a user from the list (Admins Only)"}, _
                                                                  {"savescreen", "Saves your screen from burn outs"}, _
                                                                  {"scical", "Scientific calculator. The unit converter is separated to another command"}, _
                                                                  {"setcolors", "Sets up kernel colors"}, _
                                                                  {"setsaver", "Sets up kernel screensavers"}, _
                                                                  {"setthemes", "Sets up kernel themes"}, _
                                                                  {"showaliases", "Shows aliases"}, _
                                                                  {"showmal", "Shows MOTD after login set by user or kernel"}, _
                                                                  {"showmotd", "Shows message of the day set by user or kernel"}, _
                                                                  {"showtd", "Shows date and time"}, _
                                                                  {"showtdzone", "Shows date and time in zones"}, _
                                                                  {"shutdown", "The kernel will be shut down"}, _
                                                                  {"sysinfo", "System information"}, _
                                                                  {"unitconv", "Unit converter that is separated from scicalc."}, _
                                                                  {"useddeps", "Shows used open source dependencies"}, _
                                                                  {"usermanual", "A very descriptive user manual."}}
    Public moddefs As New Dictionary(Of String, String)

    Public Sub ShowHelp(Optional ByVal command As String = "")

        Dim wholesslist As String()
        If (EnvironmentOSType.Contains("Unix")) Then
            wholesslist = IO.Directory.GetFiles(Environ("HOME") + "/KSMods", "*SS.m", IO.SearchOption.TopDirectoryOnly)
        Else
            wholesslist = IO.Directory.GetFiles(Environ("USERPROFILE") + "\KSMods", "*SS.m", IO.SearchOption.TopDirectoryOnly)
        End If
        If (command = "") Then

            If (simHelp = False) Then
                For Each cmd As String In definitions.Keys
                    W("- {0}: ", "helpCmd", cmd) : Wln("{0}", "helpDef", definitions(cmd))
                Next
                For Each cmd As String In moddefs.Keys
                    W("- {0}: ", "helpCmd", cmd) : Wln("{0}", "helpDef", moddefs(cmd))
                Next
                Wln("* You can use multiple commands using the colon between commands.", "neutralText")
            Else
                Wln(String.Join(", ", availableCommands), "neutralText")
            End If

        ElseIf (command = "adduser") Then

            Wln("Usage: adduser <userName> [password] [confirm]", "neutralText")

        ElseIf (command = "alias") Then

            Wln("Usage: alias <rem/add> <alias> <cmd>", "neutralText")

        ElseIf (command = "arginj") Then

            Wln("Usage: arginj [Arguments separated by commas]" + vbNewLine + _
                "       where arguments will be {0}", "neutralText", String.Join(", ", AvailableArgs))

        ElseIf (command = "calc") Then

            Wln("Usage: calc <expression> ...", "neutralText")

        ElseIf (command = "cdir") Then

            Wln("Usage: cdir: to get current directory", "neutralText")

        ElseIf (command = "chdir") Then

            Wln("Usage: chdir <directory> OR ..", "neutralText")

        ElseIf (command = "chhostname") Then

            Wln("Usage: chhostname <HostName>", "neutralText")

        ElseIf (command = "chmotd") Then

            Wln("Usage: chmotd <Message>", "neutralText")

        ElseIf (command = "chmal") Then

            Wln("Usage: chmal <Message>", "neutralText")

        ElseIf (command = "choice") Then

            Wln("Usage: choice <Question> <sets>", "neutralText")

        ElseIf (command = "chpwd") Then

            Wln("Usage: chpwd: <Username> <UserPass> <newPass> <confirm>", "neutralText")

        ElseIf (command = "chusrname") Then

            Wln("Usage: chusrname <oldUserName> <newUserName>", "neutralText")

        ElseIf (command = "cls") Then

            Wln("Usage: cls: to clear screen.", "neutralText")

        ElseIf (command = "debuglog") Then

            Wln("Usage: debuglog: Shows you debug logs so you can send the log to us.", "neutralText")

        ElseIf (command = "echo") Then

            Wln("Usage: echo <text>", "neutralText")

        ElseIf (command = "ftp") Then

            Wln("Usage: ftp: Initializes the FTP shell.", "neutralText")

        ElseIf (command = "hwprobe") Then

            Wln("Usage: hwprobe: Probes hardware (Only works when the hardware is not probed and hwprobe is not executed).", "neutralText")

        ElseIf (command = "list") Then
            Wln("Usage: list [oneDirectory]" + vbNewLine + _
                "       list: to get current directory.", "neutralText")

        ElseIf (command = "lsnet") Then
            Wln("Usage: lsnet: Lists network information, as well as every computer connected to a network." + vbNewLine + _
                "       Friends of lsnet: lsnettree", "neutralText")

        ElseIf (command = "lsnettree") Then
            Wln("Usage: lsnettree: Lists network information, as well as every computer connected to a network, in a tree form." + vbNewLine + _
                "       Friends of lsnettree: lsnet", "neutralText")

        ElseIf (command = "loadsaver") Then

            Wln("Usage: loadsaver <modNameSS.m>" + vbNewLine + _
                "       where modnameSS.m will be {0}", "neutralText", String.Join(", ", wholesslist))

        ElseIf (command = "lockscreen") Then

            Wln("Usage: lockscreen: Locks your screen with the password." + vbNewLine + _
                "       Friends of lockscreen: savescreen", "neutralText")

        ElseIf (command = "logout") Then

            Wln("Usage: logout: Logs you out of the user." + vbNewLine + _
                "       Friends of logout: reboot, shutdown", "neutralText")

        ElseIf (command = "md") Then

            Wln("Usage: md <anything>", "neutralText")

        ElseIf (command = "netinfo") Then

            Wln("Usage: netinfo: Get every network information", "neutralText")

        ElseIf (command = "noaliases") Then

            Wln("Usage: noaliases: SHows forbidden list of aliases", "neutralText")

        ElseIf (command = "panicsim") Then

            Wln("Usage: panicsim <message> [S/F/D/[C]/U] [RebootTime:Seconds]", "neutralText")

        ElseIf (command = "perm") Then

            Wln("Usage: perm <userName> <Admin/Disabled> <Allow/Disallow>", "neutralText")

        ElseIf (command = "ping") Then

            Wln("Usage: ping <Address> [repeatTimes]", "neutralText")

        ElseIf (command = "rd") Then

            Wln("Usage: rd <directory>", "neutralText")

        ElseIf (command = "read") Then

            Wln("Usage: read <file>", "neutralText")

        ElseIf (command = "reboot") Then

            Wln("Usage: reboot: Restarts your simulated computer." + vbNewLine + _
                "       Friends of reboot: shutdown, logout", "neutralText")

        ElseIf (command = "reloadconfig") Then

            Wln("Usage: reloadconfig: Reloads the configuration that is changed by the end-user or by tool." + vbNewLine + _
                "       Colors doesn't require a restart, but most of the settings require you to restart.", "neutralText")

        ElseIf (command = "rmuser") Then

            Wln("Usage: rmuser <Username>", "neutralText")

        ElseIf (command = "savescreen") Then

            Wln("Usage: savescreen: shows you a selected screensaver, while protecting your screen from burn outs." + vbNewLine + _
                "       Friends of savescreen: lockscreen", "neutralText")

        ElseIf (command = "scical") Then

            Wln("Usage: scical <expression1|pi|e> <+|-|*|/|%> <expression2|pi|e> ..." + vbNewLine + _
                "       scical <sqrt|tan|sin|cos> <number>", "neutralText")

        ElseIf (command = "setcolors") Then

            Wln("Usage: setcolors <inputColor/def> <licenseColor/def> <contKernelErrorColor/def> <uncontKernelErrorColor/def> <hostNameShellColor/def> <userNameShellColor/def> <backgroundColor/def> <neutralTextColor/def> <cmdListColor/def> <cmdDefColor/def>" + vbNewLine + _
                "       Friends of setcolors: setthemes", "neutralText")

        ElseIf (command = "setsaver") Then

            Wln("Usage: setsaver <modNameSS.m/matrix/disco/colorMix>" + vbNewLine + _
                "       where modnameSS.m will be {0}", "neutralText", String.Join(", ", wholesslist))

        ElseIf (command = "setthemes") Then

            Wln("Usage: setthemes <Theme>" + vbNewLine + _
                "       Friends of setthemes: setcolors", "neutralText")

        ElseIf (command = "showaliases") Then

            Wln("Usage: showaliases: Shows aliases", "neutralText")

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
                "       Friends of sysinfo: version", "neutralText")

        ElseIf (command = "unitconv") Then

            Wln("Usage: unitconv <sourceUnit> <targetUnit> <value>" + vbNewLine + _
                "Units: B, KB, MB, GB, TB, Bits, Octal, Binary, Decimal, Hexadecimal, mm, cm, m, km, Fahrenheit, Celsius, Kelvin, " + _
                "Reaumur, Romer, Delisle, Rankine, j, kj, m/s, km/h, cm/ms, Kilograms, Grams, Tons, Kilotons, Megatons, kn, n, Hz, kHz, MHz, " + _
                "GHz, Number (source only), Money (target only), Percent (target only), Centivolts, Volts, Kilovolts, Watts, Kilowatts, " + _
                "Milliliters, Liters, Kiloliters, Gallons, Ounces, Feet, Inches, Yards and Miles.", "neutralText")

        ElseIf (command = "useddeps") Then

            Wln("Usage: useddeps: Shows open source libraries used", "neutralText")

        ElseIf (command = "usermanual") Then

            Wln("Usage: usermanual <sectionWord>" + vbNewLine + _
                "       Work in progress.", "neutralText")

        End If

    End Sub

End Module
