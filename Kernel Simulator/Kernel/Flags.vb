
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

Namespace Kernel
    Public Module Flags

        'Variables
        ''' <summary>
        ''' Toggle Debugging mode
        ''' </summary>
        Public DebugMode As Boolean
        ''' <summary>
        ''' A flag for checking for an argument on reboot
        ''' </summary>
        Public ArgsInjected As Boolean
        ''' <summary>
        ''' Maintenance Mode
        ''' </summary>
        Public Maintenance As Boolean
        ''' <summary>
        ''' Arguments On Boot
        ''' </summary>
        Public ArgsOnBoot As Boolean
        ''' <summary>
        ''' Clear Screen On Log-in
        ''' </summary>
        Public ClearOnLogin As Boolean
        ''' <summary>
        ''' Show MOTD on log-in
        ''' </summary>
        Public ShowMOTD As Boolean = True
        ''' <summary>
        ''' Show MAL on log-in
        ''' </summary>
        Public ShowMAL As Boolean = True
        ''' <summary>
        ''' Simplified Help Command
        ''' </summary>
        Public SimHelp As Boolean
        ''' <summary>
        ''' Probe slots
        ''' </summary>
        Public SlotProbe As Boolean = True
        ''' <summary>
        ''' Probe the hardware quietly. This overrides the <see cref="VerboseHardwareProbe"/> flag.
        ''' </summary>
        Public QuietHardwareProbe As Boolean
        ''' <summary>
        ''' Show Time/Date on corner
        ''' </summary>
        Public CornerTimeDate As Boolean
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
        Public FullHardwareProbe As Boolean
        ''' <summary>
        ''' Makes the hardware prober a bit talkative
        ''' </summary>
        Public VerboseHardwareProbe As Boolean
        ''' <summary>
        ''' Draws the border around the notification
        ''' </summary>
        Public DrawBorderNotification As Boolean
        ''' <summary>
        ''' Whether to show the app information on boot
        ''' </summary>
        Public ShowAppInfoOnBoot As Boolean = True
        ''' <summary>
        ''' Whether to parse the command-line arguments on boot
        ''' </summary>
        Public ParseCommandLineArguments As Boolean = True
        ''' <summary>
        ''' Show how much time a stage took on boot
        ''' </summary>
        Public ShowStageFinishTimes As Boolean
        ''' <summary>
        ''' Whether to start the kernel mods on boot
        ''' </summary>
        Public StartKernelMods As Boolean = True
        ''' <summary>
        ''' Whether to show the current time before login
        ''' </summary>
        Public ShowCurrentTimeBeforeLogin As Boolean = True
        ''' <summary>
        ''' Whether to notify the user about minor boot faults
        ''' </summary>
        Public NotifyFaultsBoot As Boolean = True
        ''' <summary>
        ''' Whether to show more information about adapters
        ''' </summary>
        Public ExtensiveAdapterInformation As Boolean = True
        ''' <summary>
        ''' Whether to print general network information
        ''' </summary>
        Public GeneralNetworkInformation As Boolean = True
        ''' <summary>
        ''' Whether to start the default color wheel in true color mode
        ''' </summary>
        Public ColorWheelTrueColor As Boolean = True
        ''' <summary>
        ''' Whether to suppress the unauthorized messages while listing directory contents
        ''' </summary>
        Public SuppressUnauthorizedMessages As Boolean = True
        ''' <summary>
        ''' Print the line numbers while listing file contents
        ''' </summary>
        Public PrintLineNumbers As Boolean
        ''' <summary>
        ''' Whether to let the user choose which user they want to sign in or write it themselves
        ''' </summary>
        Public ChooseUser As Boolean
        ''' <summary>
        ''' Whether to print the stack trace on kernel error
        ''' </summary>
        Public ShowStackTraceOnKernelError As Boolean
        ''' <summary>
        ''' Whether to check the debug log to see if it exceeds the quota
        ''' </summary>
        Public CheckDebugQuota As Boolean = True
        ''' <summary>
        ''' Deletes all events and/or reminders before saving them using saveall
        ''' </summary>
        Public SaveEventsRemindersDestructively As Boolean
        ''' <summary>
        ''' Automatically downloads the kernel updates and notifies the user
        ''' </summary>
        Public AutoDownloadUpdate As Boolean = True
        ''' <summary>
        ''' Enables event debugging
        ''' </summary>
        Public EventDebug As Boolean
        ''' <summary>
        ''' Use the new welcome style for the kernel (uses the Figlet text rendering)
        ''' </summary>
        Public NewWelcomeStyle As Boolean = True
        ''' <summary>
        ''' Enable the stylish splash screen in place of the regular verbose boot messages
        ''' </summary>
        Public EnableSplash As Boolean = True
        ''' <summary>
        ''' When there is a remote debug connection error, notify the user
        ''' </summary>
        Public NotifyOnRemoteDebugConnectionError As Boolean = True
        ''' <summary>
        ''' Enables the Figlet font for the timer
        ''' </summary>
        Public EnableFigletTimer As Boolean
        ''' <summary>
        ''' Shows how many commands available in help for shells
        ''' </summary>
        Public ShowCommandsCount As Boolean
        ''' <summary>
        ''' Shows how many shell commands available in help for shells
        ''' </summary>
        Public ShowShellCommandsCount As Boolean = True
        ''' <summary>
        ''' Shows how many mod commands available in help for shells
        ''' </summary>
        Public ShowModCommandsCount As Boolean = True
        ''' <summary>
        ''' Shows how many aliases available in help for shells
        ''' </summary>
        Public ShowShellAliasesCount As Boolean = True
        ''' <summary>
        ''' Whether to simulate a situation where there is no APM available. If enabled, it shows the "It's now safe to
        ''' turn off your computer" text.
        ''' </summary>
        Public SimulateNoAPM As Boolean
        ''' <summary>
        ''' Sets the console background color using the VT sequence if true.
        ''' </summary>
        Public SetBackground As Boolean = True
        ''' <summary>
        ''' Whether the input history is enabled
        ''' </summary>
        Public Property InputHistoryEnabled As Boolean
            Get
                Return ReadLineReboot.ReadLine.HistoryEnabled
            End Get
            Set
                ReadLineReboot.ReadLine.HistoryEnabled = Value
            End Set
        End Property
        ''' <summary>
        ''' Whether the clipboard and yanking (CTRL + Y) is enabled
        ''' </summary>
        Public Property InputClipboardEnabled As Boolean
            Get
                Return ReadLineReboot.ReadLine.ClipboardEnabled
            End Get
            Set
                ReadLineReboot.ReadLine.ClipboardEnabled = Value
            End Set
        End Property
        ''' <summary>
        ''' Whether the undo function is enabled
        ''' </summary>
        Public Property InputUndoEnabled As Boolean
            Get
                Return ReadLineReboot.ReadLine.UndoEnabled
            End Get
            Set
                ReadLineReboot.ReadLine.UndoEnabled = Value
            End Set
        End Property

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
        ''' Notifies user as soon as the kernel finished booting if network is not available while downloading debugging data.
        ''' </summary>
        Friend NotifyDebugDownloadNetworkUnavailable As Boolean
        ''' <summary>
        ''' When the command cancel is requested
        ''' </summary>
        Friend CancelRequested As Boolean
        ''' <summary>
        ''' Show MOTD every <see cref="LoginPrompt()"/> call
        ''' </summary>
        Friend ShowMOTDOnceFlag As Boolean = True
        ''' <summary>
        ''' The kernel has errored
        ''' </summary>
        Friend KernelErrored As Boolean
        ''' <summary>
        ''' Notifies user as soon as the kernel finished booting if there was a kernel error in the previous boot
        ''' </summary>
        Friend NotifyKernelError As Boolean
        ''' <summary>
        ''' Notifies the kernel to be quiet
        ''' </summary>
        Friend QuietKernel As Boolean
        ''' <summary>
        ''' Checking for the console size (minimum req. 80x24)
        ''' </summary>
        Friend CheckingForConsoleSize As Boolean = True
        ''' <summary>
        ''' Is the cancellation handler installed?
        ''' </summary>
        Friend CancellationHandlerInstalled As Boolean
        ''' <summary>
        ''' If the kernel is shut down, exit from main entry point gracefully.
        ''' </summary>
        Friend KernelShutdown As Boolean

    End Module
End Namespace
