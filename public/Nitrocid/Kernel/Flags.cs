// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.Kernel.Configuration;

namespace KS.Kernel
{
    /// <summary>
    /// Kernel flags module
    /// </summary>
    public static class Flags
    {

        /// <summary>
        /// Clear Screen On Log-in
        /// </summary>
        public static bool ClearOnLogin =>
            Config.MainConfig.ClearOnLogin;
        /// <summary>
        /// Show MOTD on log-in
        /// </summary>
        public static bool ShowMOTD =>
            Config.MainConfig.ShowMOTD;
        /// <summary>
        /// Show MAL on log-in
        /// </summary>
        public static bool ShowMAL =>
            Config.MainConfig.ShowMAL;
        /// <summary>
        /// Simplified Help Command
        /// </summary>
        public static bool SimHelp =>
            Config.MainConfig.SimHelp;
        /// <summary>
        /// Probe the hardware quietly. This overrides the <see cref="VerboseHardwareProbe"/> flag.
        /// </summary>
        public static bool QuietHardwareProbe =>
            Config.MainConfig.QuietHardwareProbe;
        /// <summary>
        /// Show Time/Date on corner
        /// </summary>
        public static bool CornerTimeDate =>
            Config.MainConfig.CornerTimeDate;
        /// <summary>
        /// Log username for FTP
        /// </summary>
        public static bool FTPLoggerUsername =>
            Config.MainConfig.FTPLoggerUsername;
        /// <summary>
        /// Log IP address for FTP
        /// </summary>
        public static bool FTPLoggerIP =>
            Config.MainConfig.FTPLoggerIP;
        /// <summary>
        /// Only first profile will be returned
        /// </summary>
        public static bool FTPFirstProfileOnly =>
            Config.MainConfig.FTPFirstProfileOnly;
        /// <summary>
        /// Whether or not to parse whole directory for size
        /// </summary>
        public static bool FullParseMode =>
            Config.MainConfig.FullParseMode;
        /// <summary>
        /// Enable marquee on startup
        /// </summary>
        public static bool StartScroll =>
            Config.MainConfig.StartScroll;
        /// <summary>
        /// Whether or not to render time and dates short or long
        /// </summary>
        public static bool LongTimeDate =>
            Config.MainConfig.LongTimeDate;
        /// <summary>
        /// Whether or not to show available usernames on login
        /// </summary>
        public static bool ShowAvailableUsers =>
            Config.MainConfig.ShowAvailableUsers;
        /// <summary>
        /// Whether or not to show hidden files
        /// </summary>
        public static bool HiddenFiles =>
            Config.MainConfig.HiddenFiles;
        /// <summary>
        /// Whether or not to check for updates on startup
        /// </summary>
        public static bool CheckUpdateStart =>
            Config.MainConfig.CheckUpdateStart;
        /// <summary>
        /// Change culture when changing language
        /// </summary>
        public static bool LangChangeCulture =>
            Config.MainConfig.LangChangeCulture;
        /// <summary>
        /// Shows the progress bar while downloading using the {Down|Up}load{File|String}() API.
        /// </summary>
        public static bool ShowProgress =>
            Config.MainConfig.ShowProgress;
        /// <summary>
        /// Records remote debug chat to debug log
        /// </summary>
        public static bool RecordChatToDebugLog =>
            Config.MainConfig.RecordChatToDebugLog;
        /// <summary>
        /// Wraps the list outputs
        /// </summary>
        public static bool WrapListOutputs =>
            Config.MainConfig.WrapListOutputs;
        /// <summary>
        /// Ensures that all hardware will be probed
        /// </summary>
        public static bool FullHardwareProbe =>
            Config.MainConfig.FullHardwareProbe;
        /// <summary>
        /// Makes the hardware prober a bit talkative
        /// </summary>
        public static bool VerboseHardwareProbe =>
            Config.MainConfig.VerboseHardwareProbe;
        /// <summary>
        /// Draws the border around the notification
        /// </summary>
        public static bool DrawBorderNotification =>
            Config.MainConfig.DrawBorderNotification;
        /// <summary>
        /// Whether to show the app information on boot
        /// </summary>
        public static bool ShowAppInfoOnBoot =>
            Config.MainConfig.ShowAppInfoOnBoot;
        /// <summary>
        /// Show how much time a stage took on boot
        /// </summary>
        public static bool ShowStageFinishTimes =>
            Config.MainConfig.ShowStageFinishTimes;
        /// <summary>
        /// Whether to start the kernel mods on boot
        /// </summary>
        public static bool StartKernelMods =>
            Config.MainConfig.StartKernelMods;
        /// <summary>
        /// Whether to show the current time before login
        /// </summary>
        public static bool ShowCurrentTimeBeforeLogin =>
            Config.MainConfig.ShowCurrentTimeBeforeLogin;
        /// <summary>
        /// Whether to notify the user about minor boot faults
        /// </summary>
        public static bool NotifyFaultsBoot =>
            Config.MainConfig.NotifyFaultsBoot;
        /// <summary>
        /// Whether to suppress the unauthorized messages while listing directory contents
        /// </summary>
        public static bool SuppressUnauthorizedMessages =>
            Config.MainConfig.SuppressUnauthorizedMessages;
        /// <summary>
        /// Print the line numbers while listing file contents
        /// </summary>
        public static bool PrintLineNumbers =>
            Config.MainConfig.PrintLineNumbers;
        /// <summary>
        /// Whether to use the modern way to present log-on screen or the classic way (write your username)
        /// </summary>
        public static bool ModernLogon =>
            Config.MainConfig.ModernLogon;
        /// <summary>
        /// Whether to print the stack trace on kernel error
        /// </summary>
        public static bool ShowStackTraceOnKernelError =>
            Config.MainConfig.ShowStackTraceOnKernelError;
        /// <summary>
        /// Deletes all events and/or reminders before saving them using saveall
        /// </summary>
        public static bool SaveEventsRemindersDestructively =>
            Config.MainConfig.SaveEventsRemindersDestructively;
        /// <summary>
        /// Automatically downloads the kernel updates and notifies the user
        /// </summary>
        public static bool AutoDownloadUpdate =>
            Config.MainConfig.AutoDownloadUpdate;
        /// <summary>
        /// Enables event debugging
        /// </summary>
        public static bool EventDebug =>
            Config.MainConfig.EventDebug;
        /// <summary>
        /// Enable the stylish splash screen in place of the regular verbose boot messages
        /// </summary>
        public static bool EnableSplash =>
            Config.MainConfig.EnableSplash;
        /// <summary>
        /// When there is a remote debug connection error, notify the user
        /// </summary>
        public static bool NotifyOnRemoteDebugConnectionError =>
            Config.MainConfig.NotifyOnRemoteDebugConnectionError;
        /// <summary>
        /// Enables the Figlet font for the timer
        /// </summary>
        public static bool EnableFigletTimer =>
            Config.MainConfig.EnableFigletTimer;
        /// <summary>
        /// Shows how many commands available in help for shells
        /// </summary>
        public static bool ShowCommandsCount =>
            Config.MainConfig.ShowCommandsCount;
        /// <summary>
        /// Shows how many shell commands available in help for shells
        /// </summary>
        public static bool ShowShellCommandsCount =>
            Config.MainConfig.ShowShellCommandsCount;
        /// <summary>
        /// Shows how many mod commands available in help for shells
        /// </summary>
        public static bool ShowModCommandsCount =>
            Config.MainConfig.ShowModCommandsCount;
        /// <summary>
        /// Shows how many aliases available in help for shells
        /// </summary>
        public static bool ShowShellAliasesCount =>
            Config.MainConfig.ShowShellAliasesCount;
        /// <summary>
        /// Whether to simulate a situation where there is no APM available. If enabled, it shows the "It's now safe to
        /// turn off your computer" text.
        /// </summary>
        public static bool SimulateNoAPM =>
            Config.MainConfig.SimulateNoAPM;
        /// <summary>
        /// Enables the scroll bar in selection screens
        /// </summary>
        public static bool EnableScrollBarInSelection =>
            Config.MainConfig.EnableScrollBarInSelection;
        /// <summary>
        /// Beeps on shutdown (to restore the way of 0.0.1's shutdown)
        /// </summary>
        public static bool BeepOnShutdown =>
            Config.MainConfig.BeepOnShutdown;
        /// <summary>
        /// Delay on shutdown (to restore the way of 0.0.1's shutdown)
        /// </summary>
        public static bool DelayOnShutdown =>
            Config.MainConfig.DelayOnShutdown;
        /// <summary>
        /// Does your console support true color?
        /// </summary>
        public static bool ConsoleSupportsTrueColor =>
            Config.MainConfig.ConsoleSupportsTrueColor;
        /// <summary>
        /// Whether the input history is enabled
        /// </summary>
        public static bool InputHistoryEnabled =>
            Config.MainConfig.InputHistoryEnabled;
        /// <summary>
        /// Show tips on log-in
        /// </summary>
        public static bool ShowTip =>
            Config.MainConfig.ShowTip;

        // Private flags
        internal static bool NotifyConfigError;
        internal static bool NotifyDebugDownloadError;
        internal static bool NotifyDebugDownloadNetworkUnavailable;
        internal static bool CancelRequested;
        internal static bool ShowMOTDOnceFlag = true;
        internal static bool KernelErrored;
        internal static bool NotifyKernelError;
        internal static bool QuietKernel;
        internal static bool CheckingForConsoleSize = true;
        internal static bool KernelShutdown;
        internal static bool FirstTime;
        internal static bool DoNotDisturb;
        internal static bool ScrnTimeReached;
        internal static bool LoggedIn;
        internal static bool UseAltBuffer = true;
        internal static bool HasSetAltBuffer;
        internal static bool LogoutRequested;
        internal static bool RebootRequested;
        internal static bool DebugMode;
        internal static bool SafeMode;
        internal static bool Maintenance;
        internal static bool TalkativePreboot;
        internal static bool SetBufferSize = true;

    }
}
