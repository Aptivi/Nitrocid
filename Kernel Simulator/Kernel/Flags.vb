
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

Module Flags

    'Variables: Normal environment
    Public TimeDateIsSet As Boolean                             'To fix a bug after reboot
    Public StopPanicAndGoToDoublePanic As Boolean               'Double panic mode in kernel error
    Public DebugMode As Boolean                                 'Toggle Debugging mode
    Public LoginFlag As Boolean                                 'Flag for log-in
    Public CommandFlag As Boolean                               'A signal for command kernel argument
    Public argsFlag As Boolean                                  'A flag for checking for an argument later
    Public argsInjected As Boolean                              'A flag for checking for an argument on reboot
    Public setRootPasswd As Boolean                             'Set Root Password boolean
    Public RootPasswd As String = ""                            'Set Root Password to any password
    Public maintenance As Boolean                               'Maintenance Mode
    Public argsOnBoot As Boolean                                'Arguments On Boot
    Public clsOnLogin As Boolean                                'Clear Screen On Log-in
    Public showMOTD As Boolean = True                           'Show MOTD on log-in
    Public simHelp As Boolean                                   'Simplified Help Command
    Public slotProbe As Boolean = True                          'Probe slots
    Public quietProbe As Boolean                                'Probe quietly
    Public CornerTD As Boolean                                  'Show Time/Date on corner
    Public instanceChecked As Boolean                           'Instance checking
    Public LogoutRequested As Boolean                           'A signal when user logs out.
    Public RebootRequested As Boolean                           'Reboot requested
    Public FTPLoggerUsername As Boolean                         'Log username for FTP
    Public FTPLoggerIP As Boolean                               'Log IP address for FTP
    Public FTPFirstProfileOnly As Boolean                       'Only first profile will be returned
    Public SafeMode As Boolean                                  'Whether safe mode is enabled
    Public FullParseMode As Boolean                             'Whether or not to parse whole directory for size
    Public ScrnTimeReached As Boolean                           'When the screensaver timer has reached
    Public StartScroll As Boolean = True                        'Enable marquee on startup
    Public LongTimeDate As Boolean = True                       'Whether or not to render time and dates short or long
    Public LoggedIn As Boolean                                  'Whether or not to detect if the user is logged in
    Public ShowAvailableUsers As Boolean = True                 'Whether or not to show available usernames on login
    Public HiddenFiles As Boolean                               'Whether or not to show hidden files
    Public CheckUpdateStart As Boolean = True                   'Whether or not to check for updates on startup
    Public LangChangeCulture As Boolean                         'Change culture when changing language
    Public ShowProgress As Boolean = True                       'Shows the progress bar while downloading using "get" command.
    Public RecordChatToDebugLog As Boolean = True               'Records remote debug chat to debug log

    'Private flags
    Friend NotifyConfigError As Boolean                         'Notifies user as soon as the kernel finished booting if there is an error reading configuration.
    Friend CancelRequested As Boolean                           'When the command cancel is requested

End Module
