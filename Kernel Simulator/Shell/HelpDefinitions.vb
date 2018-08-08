
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

Public Module HelpDefinitions

    'This dictionary is the definitions for commands.
    Public definitions As New Dictionary(Of String, String) From {{"adduser", "Adds users (Only admins can access this command)"}, _
                                                                  {"alias", "Adds aliases to commands (Only admins can access this command)"}, _
                                                                  {"arginj", "Injects arguments to the kernel (reboot required, admins only)"}, _
                                                                  {"beep", "Console will beep in Hz and time in milliseconds"}, _
                                                                  {"calc", "Simple calculator (No prompt)"}, _
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
                                                                  {"disco", "A disco effect! (press ENTER to quit - Disco command will be removed and replaced with a screensaver in 0.0.4.12)"}, _
                                                                  {"echo", "Writes a text into a console"}, _
                                                                  {"help", "Help page"}, _
                                                                  {"hwprobe", "Probe hardware manually (One time in 'nohwprobe' kernel)"}, _
                                                                  {"list", "List file/folder contents in current folder"}, _
                                                                  {"loadsaver", "Loads screensaver file in %USERPROFILE%\KSMods"}, _
                                                                  {"lockscreen", "Locks your screen with a password"}, _
                                                                  {"logout", "Logs you out"}, _
                                                                  {"lscomp", "Lists all network devices who is online (and offline in domains)"}, _
                                                                  {"lsnet", "Lists all network addresses on host"}, _
                                                                  {"lsnettree", "Lists all network addresses on host using the tree"}, _
                                                                  {"md", "Creates a directory (No prompt)"}, _
                                                                  {"netinfo", "Lists information about all available interfaces"}, _
                                                                  {"panicsim", "Kernel Panic Simulator (real)"}, _
                                                                  {"perm", "Manage permissions for users (Only admins can access this command)"}, _
                                                                  {"ping", "Check to see if specified address is available"}, _
                                                                  {"read", "Writes file contents to the console"}, _
                                                                  {"reboot", "Restarts your computer (WARNING: No syncing, because it is not a final kernel)"}, _
                                                                  {"reloadconfig", "Reloads configuration file that is edited."}, _
                                                                  {"rd", "Removes a directory (No prompt)"}, _
                                                                  {"rmuser", "Removes a user from the list (Admins Only)"}, _
                                                                  {"savescreen", "Saves your screen from burn outs"}, _
                                                                  {"scical", "Scientific calculator. The unit converter is separated to another command (No prompt)"}, _
                                                                  {"setcolors", "Sets up kernel colors"}, _
                                                                  {"setsaver", "Sets up kernel screensavers"}, _
                                                                  {"setthemes", "Sets up kernel themes"}, _
                                                                  {"showmal", "Shows MOTD after login set by user or kernel"}, _
                                                                  {"showmotd", "Shows message of the day set by user or kernel"}, _
                                                                  {"showtd", "Shows date and time"}, _
                                                                  {"showtdzone", "Shows date and time in zones"}, _
                                                                  {"shutdown", "The kernel will be shut down"}, _
                                                                  {"sysinfo", "System information"}, _
                                                                  {"unitconv", "Unit converter that is separated from scicalc."}, _
                                                                  {"version", "Shows kernel version"}}

End Module
