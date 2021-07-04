
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

Public Module Flags

    'Variables
    ''' <summary>
    ''' Trigger double panic
    ''' </summary>
    Public StopPanicAndGoToDoublePanic As Boolean
    ''' <summary>
    ''' Toggle Debugging mode
    ''' </summary>
    Public DebugMode As Boolean
    ''' <summary>
    ''' Flag for log-in
    ''' </summary>
    Public LoginFlag As Boolean
    ''' <summary>
    ''' A signal for command kernel argument
    ''' </summary>
    Public CommandFlag As Boolean
    ''' <summary>
    ''' A flag for checking for an argument on reboot
    ''' </summary>
    Public argsInjected As Boolean
    ''' <summary>
    ''' Set Root Password boolean
    ''' </summary>
    Public setRootPasswd As Boolean
    ''' <summary>
    ''' Set Root Password to any password
    ''' </summary>
    Public RootPasswd As String = "toor"
    ''' <summary>
    ''' Maintenance Mode
    ''' </summary>
    Public maintenance As Boolean
    ''' <summary>
    ''' Arguments On Boot
    ''' </summary>
    Public argsOnBoot As Boolean
    ''' <summary>
    ''' Clear Screen On Log-in
    ''' </summary>
    Public clsOnLogin As Boolean
    ''' <summary>
    ''' Show MOTD on log-in
    ''' </summary>
    Public showMOTD As Boolean = True
    ''' <summary>
    ''' Simplified Help Command
    ''' </summary>
    Public simHelp As Boolean
    ''' <summary>
    ''' Probe slots
    ''' </summary>
    Public slotProbe As Boolean = True
    ''' <summary>
    ''' Probe quietly
    ''' </summary>
    Public quietProbe As Boolean
    ''' <summary>
    ''' Show Time/Date on corner
    ''' </summary>
    Public CornerTD As Boolean
    ''' <summary>
    ''' Instance checking
    ''' </summary>
    Public instanceChecked As Boolean
    ''' <summary>
    ''' A signal when user logs out.
    ''' </summary>
    Public LogoutRequested As Boolean
    ''' <summary>
    ''' Reboot requested
    ''' </summary>
    Public RebootRequested As Boolean
    ''' <summary>
    ''' Log username for FTP
    ''' </summary>
    Public FTPLoggerUsername As Boolean
    ''' <summary>
    ''' Log IP address for FTP
    ''' </summary>
    Public FTPLoggerIP As Boolean
    ''' <summary>
    ''' Only first profile will be returned
    ''' </summary>
    Public FTPFirstProfileOnly As Boolean
    ''' <summary>
    ''' Whether safe mode is enabled
    ''' </summary>
    Public SafeMode As Boolean
    ''' <summary>
    ''' Whether or not to parse whole directory for size
    ''' </summary>
    Public FullParseMode As Boolean
    ''' <summary>
    ''' When the screensaver timer has reached
    ''' </summary>
    Public ScrnTimeReached As Boolean
    ''' <summary>
    ''' Enable marquee on startup
    ''' </summary>
    Public StartScroll As Boolean = True
    ''' <summary>
    ''' Whether or not to render time and dates short or long
    ''' </summary>
    Public LongTimeDate As Boolean = True
    ''' <summary>
    ''' Whether or not to detect if the user is logged in
    ''' </summary>
    Public LoggedIn As Boolean
    ''' <summary>
    ''' Whether or not to show available usernames on login
    ''' </summary>
    Public ShowAvailableUsers As Boolean = True
    ''' <summary>
    ''' Whether or not to show hidden files
    ''' </summary>
    Public HiddenFiles As Boolean
    ''' <summary>
    ''' Whether or not to check for updates on startup
    ''' </summary>
    Public CheckUpdateStart As Boolean = True
    ''' <summary>
    ''' Change culture when changing language
    ''' </summary>
    Public LangChangeCulture As Boolean
    ''' <summary>
    ''' Shows the progress bar while downloading using the {Down|Up}load{File|String}() API.
    ''' </summary>
    Public ShowProgress As Boolean = True
    ''' <summary>
    ''' Records remote debug chat to debug log
    ''' </summary>
    Public RecordChatToDebugLog As Boolean = True
    ''' <summary>
    ''' Wraps the list outputs
    ''' </summary>
    Public WrapListOutputs As Boolean
    ''' <summary>
    ''' Ensures that all hardware will be probed
    ''' </summary>
    Public FullProbe As Boolean = True

    'Private flags
    ''' <summary>
    ''' Notifies user as soon as the kernel finished booting if there is an error reading configuration.
    ''' </summary>
    Friend NotifyConfigError As Boolean
    ''' <summary>
    ''' Notifies user as soon as the kernel finished booting if there is an error downloading debugging data.
    ''' </summary>
    Friend NotifyDebugDownloadError As Boolean
    ''' <summary>
    ''' When the command cancel is requested
    ''' </summary>
    Friend CancelRequested As Boolean
    ''' <summary>
    ''' Show MOTD every LoginPrompt() call
    ''' </summary>
    Friend ShowMOTDOnceFlag As Boolean = True

End Module
