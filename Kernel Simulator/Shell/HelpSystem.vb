
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

            'TODO: Convert (All Commands) help system from plain text to dictionaries
            If (simHelp = False) Then
                Wln("Help commands:" + vbNewLine + vbNewLine + _
                                                     "adduser: Adds users (Only admins can access this command)" + vbNewLine + _
                                                     "annoying-sound (Alias: beep): Console will beep in Hz and time in milliseconds" + vbNewLine + _
                                                     "arginj: Injects arguments to the kernel (reboot required, admins only)" + vbNewLine + _
                                                     "calc: Simple calculator (No prompt)" + vbNewLine + _
                                                     "cdir (Alias: currentdir): Shows current directory" + vbNewLine + _
                                                     "changedir (Aliases: chdir, cd): Changes directory" + vbNewLine + _
                                                     "chhostname: Changes host name (Admins only)" + vbNewLine + _
                                                     "chmotd: Changes MOTD, the Message Of The Day (Admins only)" + vbNewLine + _
                                                     "choice: Makes user choices" + vbNewLine + _
                                                     "chpwd: Changes password for current user" + vbNewLine + _
                                                     "chusrname: Changes user name (Admins Only)" + vbNewLine + _
                                                     "cls: Clears the screen" + vbNewLine + _
                                                     "debuglog: Shows debug logs (Admins Only)" + vbNewLine + _
                                                     "disco: A disco effect! (press ENTER to quit)" + vbNewLine + _
                                                     "echo: Writes a text into a console" + vbNewLine + _
                                                     "future-eyes-destroyer (Alias: fed): Like disco, but black/white version." + vbNewLine + _
                                                     "help: Help page" + vbNewLine + _
                                                     "hwprobe: Probe hardware manually (One time in 'nohwprobe' kernel)" + vbNewLine + _
                                                     "list (Alias: ls): List file/folder contents in current folder" + vbNewLine + _
                                                     "logout: Logs you out." + vbNewLine + _
                                                     "lsdrivers: Lists drivers that is recognized by the kernel." + vbNewLine + _
                                                     "lsnet: Lists all network addresses on host" + vbNewLine + _
                                                     "lsnettree: Lists all network addresses on host using the tree" + vbNewLine + _
                                                     "md (Alias: mkdir): Creates a directory (No prompt)" + vbNewLine + _
                                                     "netinfo: Lists information about all available interfaces" + vbNewLine + _
                                                     "panicsim: Kernel Panic Simulator (real)" + vbNewLine + _
                                                     "perm: Manage permissions for users (Only admins can access this command)" + vbNewLine + _
                                                     "ping: Check to see if specified address is available" + vbNewLine + _
                                                     "read: Writes file contents to the console" + vbNewLine + _
                                                     "reboot: Restarts your computer (WARNING: No syncing, because it is not a final kernel)" + vbNewLine + _
                                                     "reloadconfig: Reloads configuration file that is edited." + vbNewLine + _
                                                     "rd (Alias: rmdir): Removes a directory (No prompt)" + vbNewLine + _
                                                     "rmuser: Removes a user from the list (Admins Only)" + vbNewLine + _
                                                     "scical: Scientific calculator. The unit converter is separated to another command (No prompt)" + vbNewLine + _
                                                     "setcolors: Sets up kernel colors" + vbNewLine + _
                                                     "setthemes: Sets up kernel themes" + vbNewLine + _
                                                     "showmotd: Shows message of the day set by user or kernel" + vbNewLine + _
                                                     "showtd: Shows date and time" + vbNewLine + _
                                                     "shutdown: The kernel will be shut down" + vbNewLine + _
                                                     "sysinfo: System information" + vbNewLine + _
                                                     "unitconv: Unit converter that is separated from scicalc." + vbNewLine + _
                                                     "version: Shows kernel version", "neutralText")
            Else
                Wln(String.Join(", ", availableCommands), "neutralText")
            End If

        ElseIf (command.Contains("adduser")) Then

            Wln("Usage: adduser <userName> [password] [confirm]" + vbNewLine + _
                "       adduser: to be prompted about new username and password", "neutralText")

        ElseIf (command.Contains("annoying-sound") Or command.Contains("beep")) Then

            Wln("Usage: annoying-sound/beep <Frequency:Hz> <Time:Seconds>" + vbNewLine + _
                "       annoying-sound/beep: to be prompted about beeping.", "neutralText")

        ElseIf (command.Contains("arginj")) Then

            Wln("Usage: arginj [Arguments separated by commas]" + vbNewLine + _
                "       arginj: to be prompted about boot arguments.", "neutralText")

        ElseIf (command.Contains("calc")) Then

            Wln("Usage: calc <expression> ...", "neutralText")

        ElseIf (command.Contains("cdir") Or command.Contains("currentdir")) Then

            Wln("Usage: cdir/currentdir: to get current directory", "neutralText")

        ElseIf (command.Contains("changedir") Or command.Contains("chdir") Or command.StartsWith("cd")) Then

            Wln("Usage: chdir/changedir/cd <directory> OR ..", "neutralText")

        ElseIf (command.Contains("chhostname")) Then

            Wln("Usage: chhostname <HostName>" + vbNewLine + _
                "       chhostname: to be prompted about changing host name.", "neutralText")

        ElseIf (command.Contains("chmotd")) Then

            Wln("Usage: chmotd <Message>", "neutralText")

        ElseIf (command.Contains("choice")) Then

            Wln("Usage: choice <Question> <sets>" + vbNewLine + _
                "       choice: to be prompted about choices.", "neutralText")

        ElseIf (command.Contains("chpwd")) Then

            Wln("Usage: chpwd: to be prompted about changing passwords.", "neutralText")

        ElseIf (command.Contains("chusrname")) Then

            Wln("Usage: chusrname <oldUserName> <newUserName>" + vbNewLine + _
                "       chusrname: to be prompted about changing usernames.", "neutralText")

        ElseIf (command.Contains("cls")) Then

            Wln("Usage: cls: to clear screen.", "neutralText")

        ElseIf (command.Contains("debuglog")) Then

            Wln("Usage: debuglog: Shows you debug logs so you can send the log to us.", "neutralText")

        ElseIf (command.Contains("disco")) Then

            Wln("Usage: disco: to get a disco effect on the console. True color support will come with GUI console.", "neutralText")

        ElseIf (command.Contains("echo")) Then

            Wln("Usage: echo <text>" + vbNewLine + _
                "       echo: to be prompted about text printing.", "neutralText")

        ElseIf (command.Contains("fed") Or command.Contains("future-eyes-destroyer")) Then

            Wln("Usage: fed/future-eyes-destroyer: It will be removed in the future. Simulates a monochrome disco.", "neutralText")

        ElseIf (command.Contains("hwprobe")) Then

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

        ElseIf (command.Contains("logout")) Then

            Wln("Usage: logout: Logs you out of the user." + vbNewLine + _
                "       Friends of logout: reboot, shutdown", "neutralText")

        ElseIf (command.Contains("mkdir") Or command.Contains("md")) Then

            Wln("Usage: md/mkdir <anything>", "neutralText")

        ElseIf (command.Contains("netinfo")) Then

            Wln("Usage: netinfo: Get every network information", "neutralText")

        ElseIf (command.Contains("panicsim")) Then

            Wln("Usage: panicsim <message> [S/F/D/[C]/U] [RebootTime:Seconds]" + vbNewLine + _
                "       panicsim: to be prompted about panic simulator options.", "neutralText")

        ElseIf (command.Contains("perm")) Then

            Wln("Usage: perm <userName> <Admin/Disabled> <Allow/Disallow>" + vbNewLine + _
                "       perm: to be prompted about permission setting.", "neutralText")

        ElseIf (command.Contains("ping")) Then

            Wln("Usage: ping <Address> [repeatTimes]" + vbNewLine + _
                "       ping: to get prompted about writing address.", "neutralText")

        ElseIf (command.Contains("rmdir") Or command.Contains("rd")) Then

            Wln("Usage: rd/rmdir <directory>", "neutralText")

        ElseIf (command.Contains("read")) Then

            Wln("Usage: read <file>" + vbNewLine + _
                "       read: to get prompted about reading file contents.", "neutralText")

        ElseIf (command.Contains("reboot")) Then

            Wln("Usage: reboot: Restarts your simulated computer." + vbNewLine + _
                "       Friends of reboot: shutdown, logout", "neutralText")

        ElseIf (command.Contains("reloadconfig")) Then

            Wln("Usage: reloadconfig: Reloads the configuration that is changed by the end-user or by tool." + vbNewLine + _
                "       Colors doesn't require a restart, but most of the settings require you to restart.", "neutralText")

        ElseIf (command.Contains("rmuser")) Then

            Wln("Usage: rmuser <Username>" + vbNewLine + _
                "       rmuser: to get prompted about removing usernames.", "neutralText")

        ElseIf (command.Contains("scical")) Then

            Wln("Usage: scical <expression1|pi|e> <+|-|*|/|%> <expression2|pi|e> ..." + vbNewLine + _
                "       scical <sqrt|tan|sin|cos> <number>", "neutralText")

        ElseIf (command.Contains("setcolors")) Then

            Wln("Usage: setcolors <inputColor/def> <licenseColor/def> <contKernelErrorColor/def> <uncontKernelErrorColor/def> <hostNameShellColor/def> <userNameShellColor/def> <backgroundColor/def> <neutralTextColor/def>" + vbNewLine + _
                "       setcolors: to get prompted about setting colors." + vbNewLine + _
                "       Friends of setcolors: setthemes", "neutralText")

        ElseIf (command.Contains("setthemes")) Then

            Wln("Usage: setthemes <Theme>" + vbNewLine + _
                "       setthemes: to get prompted about setting themes." + vbNewLine + _
                "       Friends of setthemes: setcolors", "neutralText")

        ElseIf (command.Contains("showmotd")) Then

            Wln("Usage: showmotd: Shows your current Message Of The Day.", "neutralText")

        ElseIf (command.Contains("showtd")) Then

            Wln("Usage: showtd: Shows the date and time.", "neutralText")

        ElseIf (command.Contains("shutdown")) Then

            Wln("Usage: shutdown: Shuts down your simulated computer." + vbNewLine + _
                "       Friends of shutdown: reboot, logout", "neutralText")

        ElseIf (command.Contains("sysinfo")) Then

            Wln("Usage: sysinfo: Shows system information and versions." + vbNewLine + _
                "       Friends of sysinfo: lsdrivers, version", "neutralText")

        ElseIf (command.Contains("unitconv")) Then

            Wln("Usage: unitconv <sourceUnit> <targetUnit> <value>" + vbNewLine + _
                "Units: B, KB, MB, GB, TB, Bits, Octal, Binary, Decimal, Hexadecimal, mm, cm, m, km, Fahrenheit, Celsius, Kelvin, " + _
                "j, kj, m/s, km/h, cm/ms, Kilograms, Grams, Tons, Kilotons, Megatons, kn, n, Hz, kHz, MHz, GHz, Number (source only), " + _
                "Money (target only), Percent (target only), Centivolts, Volts, Kilovolts, Watts, Kilowatts, Milliliters, Liters, " + _
                "Kiloliters, Gallons, Ounces, Feet, Inches, Yards and Miles.", "neutralText")

        ElseIf (command.Contains("version")) Then

            Wln("Usage: version: Shows kernel version." + vbNewLine + _
                "       Friends of version: lsdrivers, sysinfo", "neutralText")

        End If

    End Sub

End Module
