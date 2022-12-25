// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using TermRead.Reader;

namespace KS.Kernel
{
    /// <summary>
    /// Kernel flags module
    /// </summary>
    public static class Flags
    {

        // Variables
        /// <summary>
        /// Toggle Debugging mode
        /// </summary>
        public static bool DebugMode;
        /// <summary>
        /// Maintenance Mode
        /// </summary>
        public static bool Maintenance;
        /// <summary>
        /// Clear Screen On Log-in
        /// </summary>
        public static bool ClearOnLogin;
        /// <summary>
        /// Show MOTD on log-in
        /// </summary>
        public static bool ShowMOTD = true;
        /// <summary>
        /// Show MAL on log-in
        /// </summary>
        public static bool ShowMAL = true;
        /// <summary>
        /// Simplified Help Command
        /// </summary>
        public static bool SimHelp;
        /// <summary>
        /// Probe slots
        /// </summary>
        public static bool SlotProbe = true;
        /// <summary>
        /// Probe the hardware quietly. This overrides the <see cref="VerboseHardwareProbe"/> flag.
        /// </summary>
        public static bool QuietHardwareProbe;
        /// <summary>
        /// Show Time/Date on corner
        /// </summary>
        public static bool CornerTimeDate;
        /// <summary>
        /// A signal when user logs out.
        /// </summary>
        public static bool LogoutRequested;
        /// <summary>
        /// Reboot requested
        /// </summary>
        public static bool RebootRequested;
        /// <summary>
        /// Log username for FTP
        /// </summary>
        public static bool FTPLoggerUsername;
        /// <summary>
        /// Log IP address for FTP
        /// </summary>
        public static bool FTPLoggerIP;
        /// <summary>
        /// Only first profile will be returned
        /// </summary>
        public static bool FTPFirstProfileOnly;
        /// <summary>
        /// Whether safe mode is enabled
        /// </summary>
        public static bool SafeMode;
        /// <summary>
        /// Whether or not to parse whole directory for size
        /// </summary>
        public static bool FullParseMode;
        /// <summary>
        /// When the screensaver timer has reached
        /// </summary>
        public static bool ScrnTimeReached;
        /// <summary>
        /// Enable marquee on startup
        /// </summary>
        public static bool StartScroll = true;
        /// <summary>
        /// Whether or not to render time and dates short or long
        /// </summary>
        public static bool LongTimeDate = true;
        /// <summary>
        /// Whether or not to detect if the user is logged in
        /// </summary>
        public static bool LoggedIn;
        /// <summary>
        /// Whether or not to show available usernames on login
        /// </summary>
        public static bool ShowAvailableUsers = true;
        /// <summary>
        /// Whether or not to show hidden files
        /// </summary>
        public static bool HiddenFiles;
        /// <summary>
        /// Whether or not to check for updates on startup
        /// </summary>
        public static bool CheckUpdateStart = true;
        /// <summary>
        /// Change culture when changing language
        /// </summary>
        public static bool LangChangeCulture;
        /// <summary>
        /// Shows the progress bar while downloading using the {Down|Up}load{File|String}() API.
        /// </summary>
        public static bool ShowProgress = true;
        /// <summary>
        /// Records remote debug chat to debug log
        /// </summary>
        public static bool RecordChatToDebugLog = true;
        /// <summary>
        /// Wraps the list outputs
        /// </summary>
        public static bool WrapListOutputs;
        /// <summary>
        /// Ensures that all hardware will be probed
        /// </summary>
        public static bool FullHardwareProbe;
        /// <summary>
        /// Makes the hardware prober a bit talkative
        /// </summary>
        public static bool VerboseHardwareProbe;
        /// <summary>
        /// Draws the border around the notification
        /// </summary>
        public static bool DrawBorderNotification;
        /// <summary>
        /// Whether to show the app information on boot
        /// </summary>
        public static bool ShowAppInfoOnBoot = true;
        /// <summary>
        /// Show how much time a stage took on boot
        /// </summary>
        public static bool ShowStageFinishTimes;
        /// <summary>
        /// Whether to start the kernel mods on boot
        /// </summary>
        public static bool StartKernelMods = true;
        /// <summary>
        /// Whether to show the current time before login
        /// </summary>
        public static bool ShowCurrentTimeBeforeLogin = true;
        /// <summary>
        /// Whether to notify the user about minor boot faults
        /// </summary>
        public static bool NotifyFaultsBoot = true;
        /// <summary>
        /// Whether to show more information about adapters
        /// </summary>
        public static bool ExtensiveAdapterInformation = true;
        /// <summary>
        /// Whether to print general network information
        /// </summary>
        public static bool GeneralNetworkInformation = true;
        /// <summary>
        /// Whether to start the default color wheel in true color mode
        /// </summary>
        public static bool ColorWheelTrueColor = true;
        /// <summary>
        /// Whether to suppress the unauthorized messages while listing directory contents
        /// </summary>
        public static bool SuppressUnauthorizedMessages = true;
        /// <summary>
        /// Print the line numbers while listing file contents
        /// </summary>
        public static bool PrintLineNumbers;
        /// <summary>
        /// Whether to let the user choose which user they want to sign in or write it themselves
        /// </summary>
        public static bool ChooseUser;
        /// <summary>
        /// Whether to print the stack trace on kernel error
        /// </summary>
        public static bool ShowStackTraceOnKernelError;
        /// <summary>
        /// Deletes all events and/or reminders before saving them using saveall
        /// </summary>
        public static bool SaveEventsRemindersDestructively;
        /// <summary>
        /// Automatically downloads the kernel updates and notifies the user
        /// </summary>
        public static bool AutoDownloadUpdate = true;
        /// <summary>
        /// Enables event debugging
        /// </summary>
        public static bool EventDebug;
        /// <summary>
        /// Enable the stylish splash screen in place of the regular verbose boot messages
        /// </summary>
        public static bool EnableSplash = true;
        /// <summary>
        /// When there is a remote debug connection error, notify the user
        /// </summary>
        public static bool NotifyOnRemoteDebugConnectionError = true;
        /// <summary>
        /// Enables the Figlet font for the timer
        /// </summary>
        public static bool EnableFigletTimer;
        /// <summary>
        /// Shows how many commands available in help for shells
        /// </summary>
        public static bool ShowCommandsCount;
        /// <summary>
        /// Shows how many shell commands available in help for shells
        /// </summary>
        public static bool ShowShellCommandsCount = true;
        /// <summary>
        /// Shows how many mod commands available in help for shells
        /// </summary>
        public static bool ShowModCommandsCount = true;
        /// <summary>
        /// Shows how many aliases available in help for shells
        /// </summary>
        public static bool ShowShellAliasesCount = true;
        /// <summary>
        /// Whether to simulate a situation where there is no APM available. If enabled, it shows the "It's now safe to
        /// turn off your computer" text.
        /// </summary>
        public static bool SimulateNoAPM;
        /// <summary>
        /// Sets the console background color using the VT sequence if true.
        /// </summary>
        public static bool SetBackground = true;
        /// <summary>
        /// Whether the input history is enabled
        /// </summary>
        public static bool InputHistoryEnabled
        {
            get
            {
                return TermReaderSettings.HistoryEnabled;
            }
            set
            {
                TermReaderSettings.HistoryEnabled = value;
            }
        }
        /// <summary>
        /// Enables the scroll bar in selection screens
        /// </summary>
        public static bool EnableScrollBarInSelection = true;
        /// <summary>
        /// Beeps on shutdown (to restore the way of 0.0.1's shutdown)
        /// </summary>
        public static bool BeepOnShutdown;
        /// <summary>
        /// Delay on shutdown (to restore the way of 0.0.1's shutdown)
        /// </summary>
        public static bool DelayOnShutdown;
        /// <summary>
        /// Does your console support true color?
        /// </summary>
        public static bool ConsoleSupportsTrueColor = true;

        // Private flags
        /// <summary>
        /// Notifies user as soon as the kernel finished booting if there is an error reading configuration.
        /// </summary>
        internal static bool NotifyConfigError;
        /// <summary>
        /// Notifies user as soon as the kernel finished booting if there is an error downloading debugging data.
        /// </summary>
        internal static bool NotifyDebugDownloadError;
        /// <summary>
        /// Notifies user as soon as the kernel finished booting if network is not available while downloading debugging data.
        /// </summary>
        internal static bool NotifyDebugDownloadNetworkUnavailable;
        /// <summary>
        /// When the command cancel is requested
        /// </summary>
        internal static bool CancelRequested;
        /// <summary>
        /// Show MOTD every <see cref="Users.Login.Login.LoginPrompt()"/> call if false. Otherwise, shows it only once.
        /// </summary>
        internal static bool ShowMOTDOnceFlag = true;
        /// <summary>
        /// The kernel has errored
        /// </summary>
        internal static bool KernelErrored;
        /// <summary>
        /// Notifies user as soon as the kernel finished booting if there was a kernel error in the previous boot
        /// </summary>
        internal static bool NotifyKernelError;
        /// <summary>
        /// Notifies the kernel to be quiet
        /// </summary>
        internal static bool QuietKernel;
        /// <summary>
        /// Checking for the console size (minimum req. 80x24)
        /// </summary>
        internal static bool CheckingForConsoleSize = true;
        /// <summary>
        /// Is the cancellation handler installed?
        /// </summary>
        internal static bool CancellationHandlerInstalled;
        /// <summary>
        /// If the kernel is shut down, exit from main entry point gracefully.
        /// </summary>
        internal static bool KernelShutdown;
        /// <summary>
        /// If this is true, prompts for console support
        /// </summary>
        internal static bool FirstTime;
        internal static bool DoNotDisturb;

    }
}
