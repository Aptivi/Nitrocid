
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

Module HelpDefinitions

    Public adduserCmdDesc As String = "Adds users (Only admins can access this command)"
    Public aliasCmdDesc As String = "Adds aliases to commands (Only admins can access this command)"
    Public annoyingSoundCmdDesc As String = "Console will beep in Hz and time in milliseconds"
    Public argInjCmdDesc As String = "Injects arguments to the kernel (reboot required, admins only)"
    Public calcCmdDesc As String = "Simple calculator (No prompt)"
    Public currentDirCmdDesc As String = "Shows current directory"
    Public changeDirCmdDesc As String = "Changes directory"
    Public chHostNameCmdDesc As String = "Changes host name (Admins only)"
    Public chMalCmdDesc As String = "Changes MAL, the MOTD After Login (Admins only)"
    Public chMotdCmdDesc As String = "Changes MOTD, the Message Of The Day (Admins only)"
    Public choiceCmdDesc As String = "Makes user choices"
    Public chPwdCmdDesc As String = "Changes password for current user"
    Public chUsrNameCmdDesc As String = "Changes user name (Admins Only)"
    Public clsCmdDesc As String = "Clears the screen"
    Public debugLogCmdDesc As String = "Shows debug logs (Admins Only)"
    Public discoCmdDesc As String = "A disco effect! (press ENTER to quit)"
    Public echoCmdDesc As String = "Writes a text into a console"
    Public helpCmdDesc As String = "Help page"
    Public hwProbeCmdDesc As String = "Probe hardware manually (One time in 'nohwprobe' kernel)"
    Public listCmdDesc As String = "List file/folder contents in current folder"
    Public logoutCmdDesc As String = "Logs you out"
    Public lsDriversCmdDesc As String = "Lists drivers that is recognized by the kernel."
    Public lsnetCmdDesc As String = "Lists all network addresses on host"
    Public lsNetByTreeCmdDesc As String = "Lists all network addresses on host using the tree"
    Public makeDirectoryCmdDesc As String = "Creates a directory (No prompt)"
    Public netInfoCmdDesc As String = "Lists information about all available interfaces"
    Public panicSimCmdDesc As String = "Kernel Panic Simulator (real)"
    Public permCmdDesc As String = "Manage permissions for users (Only admins can access this command)"
    Public pingCmdDesc As String = "Check to see if specified address is available"
    Public readCmdDesc As String = "Writes file contents to the console"
    Public rebootCmdDesc As String = "Restarts your computer (WARNING: No syncing, because it is not a final kernel)"
    Public reloadConfigCmdDesc As String = "Reloads configuration file that is edited."
    Public rmDirCmdDesc As String = "Removes a directory (No prompt)"
    Public rmUserCmdDesc As String = "Removes a user from the list (Admins Only)"
    Public sciCalCmdDesc As String = "Scientific calculator. The unit converter is separated to another command (No prompt)"
    Public setColorsCmdDesc As String = "Sets up kernel colors"
    Public setThemesCmdDesc As String = "Sets up kernel cthemes"
    Public showMalCmdDesc As String = "Shows MOTD after login set by user or kernel"
    Public showMotdCmdDesc As String = "Shows message of the day set by user or kernel"
    Public showTdCmdDesc As String = "Shows date and time"
    Public showTdZoneCmdDesc As String = "Shows date and time in zones"
    Public shutdownCmdDesc As String = "The kernel will be shut down"
    Public sysInfoCmdDesc As String = "System information"
    Public unitConvCmdDesc As String = "Unit converter that is separated from scicalc."
    Public versionCmdDesc As String = "Shows kernel version"

End Module
