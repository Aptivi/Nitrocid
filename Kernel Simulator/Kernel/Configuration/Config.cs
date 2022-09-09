
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

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using ColorSeq;
using Extensification.StringExts;
using FluentFTP;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.ConsoleBase.Inputs.Styles;
using KS.ConsoleBase.Themes;
using KS.Files;
using KS.Files.Folders;
using KS.Files.Querying;
using KS.Kernel.Debugging;
using KS.Kernel.Debugging.RemoteDebug;
using KS.Languages;
using KS.ManPages;
using KS.Misc;
using KS.Misc.Games;
using KS.Misc.Notifications;
using KS.Misc.Probers.Motd;
using KS.Misc.Reflection;
using KS.Misc.Screensaver;
using KS.Misc.Screensaver.Displays;
using KS.Misc.Settings;
using KS.Misc.Splash;
using KS.Misc.Timers;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.FancyWriters.Tools;
using KS.Misc.Writers.MiscWriters;
using KS.Modifications;
using KS.Network;
using KS.Network.Mail;
using KS.Network.Mail.Directory;
using KS.Network.RPC;
using KS.Network.RSS;
using KS.Network.SSH;
using KS.Network.Transfer;
using KS.Shell.Prompts;
using KS.Shell.ShellBase.Shells;
using KS.Shell.Shells.FTP;
using KS.Shell.Shells.Hex;
using KS.Shell.Shells.Json;
using KS.Shell.Shells.Mail;
using KS.Shell.Shells.RSS;
using KS.Shell.Shells.SFTP;
using KS.Shell.Shells.Text;
using KS.Users;
using ManagedWeatherMap.Core;
using MimeKit.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static System.Collections.Specialized.BitVector32;

namespace KS.Kernel.Configuration
{
    /// <summary>
    /// Configuration module
    /// </summary>
    public static class Config
    {

        /// <summary>
        /// Base config token to be loaded each kernel startup.
        /// </summary>
        internal static JObject ConfigToken;
        /// <summary>
        /// Fallback configuration
        /// </summary>
        internal readonly static JObject PristineConfigToken = GetNewConfigObject();

        /// <summary>
        /// Config category enumeration
        /// </summary>
        public enum ConfigCategory
        {
            /// <summary>
            /// All general kernel settings, mainly for maintaining the kernel.
            /// </summary>
            General,
            /// <summary>
            /// Color settings
            /// </summary>
            Colors,
            /// <summary>
            /// Hardware settings
            /// </summary>
            Hardware,
            /// <summary>
            /// Login settings
            /// </summary>
            Login,
            /// <summary>
            /// Shell settings
            /// </summary>
            Shell,
            /// <summary>
            /// Filesystem settings
            /// </summary>
            Filesystem,
            /// <summary>
            /// Network settings
            /// </summary>
            Network,
            /// <summary>
            /// Screensaver settings
            /// </summary>
            Screensaver,
            /// <summary>
            /// Miscellaneous settings
            /// </summary>
            Misc
        }

        /// <summary>
        /// Creates a new JSON object containing the kernel settings of all kinds
        /// </summary>
        /// <returns>A pristine config object</returns>
        public static JObject GetNewConfigObject()
        {
            var ConfigurationObject = new JObject();

            // The General Section
            var GeneralConfig = new JObject()
            {
                { "Prompt for Arguments on Boot", Flags.ArgsOnBoot },
                { "Maintenance Mode", Flags.Maintenance },
                { "Check for Updates on Startup", Flags.CheckUpdateStart },
                { "Custom Startup Banner", WelcomeMessage.CustomBanner },
                { "Change Culture when Switching Languages", Flags.LangChangeCulture },
                { "Language", LanguageManager.CurrentLanguage },
                { "Culture", CultureManager.CurrentCult.Name },
                { "Show app information during boot", Flags.ShowAppInfoOnBoot },
                { "Parse command-line arguments", Flags.ParseCommandLineArguments },
                { "Show stage finish times", Flags.ShowStageFinishTimes },
                { "Start kernel modifications on boot", Flags.StartKernelMods },
                { "Show current time before login", Flags.ShowCurrentTimeBeforeLogin },
                { "Notify for any fault during boot", Flags.NotifyFaultsBoot },
                { "Show stack trace on kernel error", Flags.ShowStackTraceOnKernelError },
                { "Check debug quota", Flags.CheckDebugQuota },
                { "Automatically download updates", Flags.AutoDownloadUpdate },
                { "Enable event debugging", Flags.EventDebug },
                { "New welcome banner", Flags.NewWelcomeStyle },
                { "Stylish splash screen", Flags.EnableSplash },
                { "Splash name", SplashManager.SplashName },
                { "Banner figlet font", KernelTools.BannerFigletFont },
                { "Simulate No APM Mode", Flags.SimulateNoAPM },
                { "Set console background color", Flags.SetBackground }
            };
            ConfigurationObject.Add("General", GeneralConfig);

            // The Colors Section
            var ColorConfig = new JObject()
            {
                { "User Name Shell Color", ColorTools.UserNameShellColor.PlainSequenceEnclosed },
                { "Host Name Shell Color", ColorTools.HostNameShellColor.PlainSequenceEnclosed },
                { "Continuable Kernel Error Color", ColorTools.ContKernelErrorColor.PlainSequenceEnclosed },
                { "Uncontinuable Kernel Error Color", ColorTools.UncontKernelErrorColor.PlainSequenceEnclosed },
                { "Text Color", ColorTools.NeutralTextColor.PlainSequenceEnclosed },
                { "License Color", ColorTools.LicenseColor.PlainSequenceEnclosed },
                { "Background Color", ColorTools.BackgroundColor.PlainSequenceEnclosed },
                { "Input Color", ColorTools.InputColor.PlainSequenceEnclosed },
                { "List Entry Color", ColorTools.ListEntryColor.PlainSequenceEnclosed },
                { "List Value Color", ColorTools.ListValueColor.PlainSequenceEnclosed },
                { "Kernel Stage Color", ColorTools.StageColor.PlainSequenceEnclosed },
                { "Error Text Color", ColorTools.ErrorColor.PlainSequenceEnclosed },
                { "Warning Text Color", ColorTools.WarningColor.PlainSequenceEnclosed },
                { "Option Color", ColorTools.OptionColor.PlainSequenceEnclosed },
                { "Banner Color", ColorTools.BannerColor.PlainSequenceEnclosed },
                { "Notification Title Color", ColorTools.NotificationTitleColor.PlainSequenceEnclosed },
                { "Notification Description Color", ColorTools.NotificationDescriptionColor.PlainSequenceEnclosed },
                { "Notification Progress Color", ColorTools.NotificationProgressColor.PlainSequenceEnclosed },
                { "Notification Failure Color", ColorTools.NotificationFailureColor.PlainSequenceEnclosed },
                { "Question Color", ColorTools.QuestionColor.PlainSequenceEnclosed },
                { "Success Color", ColorTools.SuccessColor.PlainSequenceEnclosed },
                { "User Dollar Color", ColorTools.UserDollarColor.PlainSequenceEnclosed },
                { "Tip Color", ColorTools.TipColor.PlainSequenceEnclosed },
                { "Separator Text Color", ColorTools.SeparatorTextColor.PlainSequenceEnclosed },
                { "Separator Color", ColorTools.SeparatorColor.PlainSequenceEnclosed },
                { "List Title Color", ColorTools.ListTitleColor.PlainSequenceEnclosed },
                { "Development Warning Color", ColorTools.DevelopmentWarningColor.PlainSequenceEnclosed },
                { "Stage Time Color", ColorTools.StageTimeColor.PlainSequenceEnclosed },
                { "Progress Color", ColorTools.ProgressColor.PlainSequenceEnclosed },
                { "Back Option Color", ColorTools.BackOptionColor.PlainSequenceEnclosed },
                { "Low Priority Border Color", ColorTools.LowPriorityBorderColor.PlainSequenceEnclosed },
                { "Medium Priority Border Color", ColorTools.MediumPriorityBorderColor.PlainSequenceEnclosed },
                { "High Priority Border Color", ColorTools.HighPriorityBorderColor.PlainSequenceEnclosed },
                { "Table Separator Color", ColorTools.TableSeparatorColor.PlainSequenceEnclosed },
                { "Table Header Color", ColorTools.TableHeaderColor.PlainSequenceEnclosed },
                { "Table Value Color", ColorTools.TableValueColor.PlainSequenceEnclosed },
                { "Selected Option Color", ColorTools.SelectedOptionColor.PlainSequenceEnclosed },
                { "Alternative Option Color", ColorTools.AlternativeOptionColor.PlainSequenceEnclosed }
            };
            ConfigurationObject.Add("Colors", ColorConfig);

            // The Hardware Section
            var HardwareConfig = new JObject()
            {
                { "Quiet Probe", Flags.QuietHardwareProbe },
                { "Full Probe", Flags.FullHardwareProbe },
                { "Verbose Probe", Flags.VerboseHardwareProbe }
            };
            ConfigurationObject.Add("Hardware", HardwareConfig);

            // The Login Section
            var LoginConfig = new JObject()
            {
                { "Show MOTD on Log-in", Flags.ShowMOTD },
                { "Clear Screen on Log-in", Flags.ClearOnLogin },
                { "Host Name", NetworkTools.HostName },
                { "Show available usernames", Flags.ShowAvailableUsers },
                { "MOTD Path", MotdParse.MotdFilePath },
                { "MAL Path", MalParse.MalFilePath },
                { "Username prompt style", Login.Login.UsernamePrompt },
                { "Password prompt style", Login.Login.PasswordPrompt },
                { "Show MAL on Log-in", Flags.ShowMAL },
                { "Include anonymous users", UserManagement.IncludeAnonymous },
                { "Include disabled users", UserManagement.IncludeDisabled }
            };
            ConfigurationObject.Add("Login", LoginConfig);

            // The Shell Section
            var ShellConfig = new JObject()
            {
                { "Colored Shell", Shell.Shell.ColoredShell },
                { "Simplified Help Command", Flags.SimHelp },
                { "Current Directory", CurrentDirectory.CurrentDir },
                { "Lookup Directories", Shell.Shell.PathsToLookup.EncloseByDoubleQuotes() },
                { "Prompt Preset", PromptPresetManager.UESHShellCurrentPreset.PresetName },
                { "FTP Prompt Preset", PromptPresetManager.FTPShellCurrentPreset.PresetName },
                { "Mail Prompt Preset", PromptPresetManager.MailShellCurrentPreset.PresetName },
                { "SFTP Prompt Preset", PromptPresetManager.SFTPShellCurrentPreset.PresetName },
                { "RSS Prompt Preset", PromptPresetManager.RSSShellCurrentPreset.PresetName },
                { "Text Edit Prompt Preset", PromptPresetManager.TextShellCurrentPreset.PresetName },
                { "Test Shell Prompt Preset", PromptPresetManager.TestShellCurrentPreset.PresetName },
                { "JSON Shell Prompt Preset", PromptPresetManager.JsonShellCurrentPreset.PresetName },
                { "Hex Edit Prompt Preset", PromptPresetManager.HexShellCurrentPreset.PresetName },
                { "HTTP Shell Prompt Preset", PromptPresetManager.HTTPShellCurrentPreset.PresetName },
                { "Archive Shell Prompt Preset", PromptPresetManager.ArchiveShellCurrentPreset.PresetName },
                { "Start color wheel in true color mode", Flags.ColorWheelTrueColor },
                { "Default choice output type", ChoiceStyle.DefaultChoiceOutputType.ToString() }
            };
            ConfigurationObject.Add("Shell", ShellConfig);

            // The Filesystem Section
            var FilesystemConfig = new JObject()
            {
                { "Filesystem sort mode", Listing.SortMode.ToString() },
                { "Filesystem sort direction", Listing.SortDirection.ToString() },
                { "Debug Size Quota in Bytes", DebugManager.DebugQuota },
                { "Show Hidden Files", Flags.HiddenFiles },
                { "Size parse mode", Flags.FullParseMode },
                { "Show progress on filesystem operations", Filesystem.ShowFilesystemProgress },
                { "Show file details in list", Listing.ShowFileDetailsList },
                { "Suppress unauthorized messages", Flags.SuppressUnauthorizedMessages },
                { "Print line numbers on printing file contents", Flags.PrintLineNumbers },
                { "Sort the list", Listing.SortList },
                { "Show total size in list", Listing.ShowTotalSizeInList }
            };
            ConfigurationObject.Add("Filesystem", FilesystemConfig);

            // The Network Section
            var NetworkConfig = new JObject()
            {
                { "Debug Port", RemoteDebugger.DebugPort },
                { "Download Retry Times", NetworkTools.DownloadRetries },
                { "Upload Retry Times", NetworkTools.UploadRetries },
                { "Show progress bar while downloading or uploading from \"get\" or \"put\" command", Flags.ShowProgress },
                { "Log FTP username", Flags.FTPLoggerUsername },
                { "Log FTP IP address", Flags.FTPLoggerIP },
                { "Return only first FTP profile", Flags.FTPFirstProfileOnly },
                { "Show mail message preview", MailManager.ShowPreview },
                { "Record chat to debug log", Flags.RecordChatToDebugLog },
                { "Show SSH banner", SSH.SSHBanner },
                { "Enable RPC", RemoteProcedure.RPCEnabled },
                { "RPC Port", RemoteProcedure.RPCPort },
                { "Show file details in FTP list", FTPShellCommon.FtpShowDetailsInList },
                { "Username prompt style for FTP", FTPShellCommon.FtpUserPromptStyle },
                { "Password prompt style for FTP", FTPShellCommon.FtpPassPromptStyle },
                { "Use first FTP profile", FTPShellCommon.FtpUseFirstProfile },
                { "Add new connections to FTP speed dial", FTPShellCommon.FtpNewConnectionsToSpeedDial },
                { "Try to validate secure FTP certificates", FTPShellCommon.FtpTryToValidateCertificate },
                { "Show FTP MOTD on connection", FTPShellCommon.FtpShowMotd },
                { "Always accept invalid FTP certificates", FTPShellCommon.FtpAlwaysAcceptInvalidCerts },
                { "Username prompt style for mail", MailLogin.Mail_UserPromptStyle },
                { "Password prompt style for mail", MailLogin.Mail_PassPromptStyle },
                { "IMAP prompt style for mail", MailLogin.Mail_IMAPPromptStyle },
                { "SMTP prompt style for mail", MailLogin.Mail_SMTPPromptStyle },
                { "Automatically detect mail server", MailLogin.Mail_AutoDetectServer },
                { "Enable mail debug", MailLogin.Mail_Debug },
                { "Notify for new mail messages", MailShellCommon.Mail_NotifyNewMail },
                { "GPG password prompt style for mail", MailLogin.Mail_GPGPromptStyle },
                { "Send IMAP ping interval", MailShellCommon.Mail_ImapPingInterval },
                { "Send SMTP ping interval", MailShellCommon.Mail_SmtpPingInterval },
                { "Mail text format", MailShellCommon.Mail_TextFormat.ToString() },
                { "Automatically start remote debug on startup", RemoteDebugger.RDebugAutoStart },
                { "Remote debug message format", RemoteDebugger.RDebugMessageFormat },
                { "RSS feed URL prompt style", RSSShellCommon.RSSFeedUrlPromptStyle },
                { "Auto refresh RSS feed", RSSShellCommon.RSSRefreshFeeds },
                { "Auto refresh RSS feed interval", RSSShellCommon.RSSRefreshInterval },
                { "Show file details in SFTP list", SFTPShellCommon.SFTPShowDetailsInList },
                { "Username prompt style for SFTP", SFTPShellCommon.SFTPUserPromptStyle },
                { "Add new connections to SFTP speed dial", SFTPShellCommon.SFTPNewConnectionsToSpeedDial },
                { "Ping timeout", NetworkTools.PingTimeout },
                { "Show extensive adapter info", Flags.ExtensiveAdapterInformation },
                { "Show general network information", Flags.GeneralNetworkInformation },
                { "Download percentage text", NetworkTransfer.DownloadPercentagePrint },
                { "Upload percentage text", NetworkTransfer.UploadPercentagePrint },
                { "Recursive hashing for FTP", FTPShellCommon.FtpRecursiveHashing },
                { "Maximum number of e-mails in one page", MailShellCommon.Mail_MaxMessagesInPage },
                { "Show mail transfer progress", MailShellCommon.Mail_ShowProgress },
                { "Mail transfer progress", MailShellCommon.Mail_ProgressStyle },
                { "Mail transfer progress (single)", MailShellCommon.Mail_ProgressStyleSingle },
                { "Show notification for download progress", NetworkTransfer.DownloadNotificationProvoke },
                { "Show notification for upload progress", NetworkTransfer.UploadNotificationProvoke },
                { "RSS feed fetch timeout", RSSShellCommon.RSSFetchTimeout },
                { "Verify retry attempts for FTP transmission", FTPShellCommon.FtpVerifyRetryAttempts },
                { "FTP connection timeout", FTPShellCommon.FtpConnectTimeout },
                { "FTP data connection timeout", FTPShellCommon.FtpDataConnectTimeout },
                { "FTP IP versions", FTPShellCommon.FtpProtocolVersions.ToString() },
                { "Notify on remote debug connection error", Flags.NotifyOnRemoteDebugConnectionError }
            };
            ConfigurationObject.Add("Network", NetworkConfig);

            // The Screensaver Section
            var ScreensaverConfig = new JObject()
            {
                { "Screensaver", Screensaver.DefSaverName },
                { "Screensaver Timeout in ms", Screensaver.ScrnTimeout },
                { "Enable screensaver debugging", Screensaver.ScreensaverDebug },
                { "Ask for password after locking", Screensaver.PasswordLock }
            };

            // ColorMix config json object
            var ColorMixConfig = new JObject()
            {
                { "Activate 255 colors", ColorMixSettings.ColorMix255Colors },
                { "Activate true colors", ColorMixSettings.ColorMixTrueColor },
                { "Delay in Milliseconds", ColorMixSettings.ColorMixDelay },
                { "Background color", new Color(ColorMixSettings.ColorMixBackgroundColor).Type == ColorType.TrueColor ? ColorMixSettings.ColorMixBackgroundColor.EncloseByDoubleQuotes() : ColorMixSettings.ColorMixBackgroundColor },
                { "Minimum red color level", ColorMixSettings.ColorMixMinimumRedColorLevel },
                { "Minimum green color level", ColorMixSettings.ColorMixMinimumGreenColorLevel },
                { "Minimum blue color level", ColorMixSettings.ColorMixMinimumBlueColorLevel },
                { "Minimum color level", ColorMixSettings.ColorMixMinimumColorLevel },
                { "Maximum red color level", ColorMixSettings.ColorMixMaximumRedColorLevel },
                { "Maximum green color level", ColorMixSettings.ColorMixMaximumGreenColorLevel },
                { "Maximum blue color level", ColorMixSettings.ColorMixMaximumBlueColorLevel },
                { "Maximum color level", ColorMixSettings.ColorMixMaximumColorLevel }
            };
            ScreensaverConfig.Add("ColorMix", ColorMixConfig);

            // Disco config json object
            var DiscoConfig = new JObject()
            {
                { "Activate 255 colors", DiscoSettings.Disco255Colors },
                { "Activate true colors", DiscoSettings.DiscoTrueColor },
                { "Delay in Milliseconds", DiscoSettings.DiscoDelay },
                { "Use Beats Per Minute", DiscoSettings.DiscoUseBeatsPerMinute },
                { "Cycle colors", DiscoSettings.DiscoCycleColors },
                { "Enable Black and White Mode", DiscoSettings.DiscoEnableFedMode },
                { "Minimum red color level", DiscoSettings.DiscoMinimumRedColorLevel },
                { "Minimum green color level", DiscoSettings.DiscoMinimumGreenColorLevel },
                { "Minimum blue color level", DiscoSettings.DiscoMinimumBlueColorLevel },
                { "Minimum color level", DiscoSettings.DiscoMinimumColorLevel },
                { "Maximum red color level", DiscoSettings.DiscoMaximumRedColorLevel },
                { "Maximum green color level", DiscoSettings.DiscoMaximumGreenColorLevel },
                { "Maximum blue color level", DiscoSettings.DiscoMaximumBlueColorLevel },
                { "Maximum color level", DiscoSettings.DiscoMaximumColorLevel }
            };
            ScreensaverConfig.Add("Disco", DiscoConfig);

            // GlitterColor config json object
            var GlitterColorConfig = new JObject()
            {
                { "Activate 255 colors", GlitterColorSettings.GlitterColor255Colors },
                { "Activate true colors", GlitterColorSettings.GlitterColorTrueColor },
                { "Delay in Milliseconds", GlitterColorSettings.GlitterColorDelay },
                { "Minimum red color level", GlitterColorSettings.GlitterColorMinimumRedColorLevel },
                { "Minimum green color level", GlitterColorSettings.GlitterColorMinimumGreenColorLevel },
                { "Minimum blue color level", GlitterColorSettings.GlitterColorMinimumBlueColorLevel },
                { "Minimum color level", GlitterColorSettings.GlitterColorMinimumColorLevel },
                { "Maximum red color level", GlitterColorSettings.GlitterColorMaximumRedColorLevel },
                { "Maximum green color level", GlitterColorSettings.GlitterColorMaximumGreenColorLevel },
                { "Maximum blue color level", GlitterColorSettings.GlitterColorMaximumBlueColorLevel },
                { "Maximum color level", GlitterColorSettings.GlitterColorMaximumColorLevel }
            };
            ScreensaverConfig.Add("GlitterColor", GlitterColorConfig);

            // Lines config json object
            var LinesConfig = new JObject()
            {
                { "Activate 255 colors", LinesSettings.Lines255Colors },
                { "Activate true colors", LinesSettings.LinesTrueColor },
                { "Delay in Milliseconds", LinesSettings.LinesDelay },
                { "Line character", LinesSettings.LinesLineChar },
                { "Background color", new Color(LinesSettings.LinesBackgroundColor).Type == ColorType.TrueColor ? LinesSettings.LinesBackgroundColor.EncloseByDoubleQuotes() : LinesSettings.LinesBackgroundColor },
                { "Minimum red color level", LinesSettings.LinesMinimumRedColorLevel },
                { "Minimum green color level", LinesSettings.LinesMinimumGreenColorLevel },
                { "Minimum blue color level", LinesSettings.LinesMinimumBlueColorLevel },
                { "Minimum color level", LinesSettings.LinesMinimumColorLevel },
                { "Maximum red color level", LinesSettings.LinesMaximumRedColorLevel },
                { "Maximum green color level", LinesSettings.LinesMaximumGreenColorLevel },
                { "Maximum blue color level", LinesSettings.LinesMaximumBlueColorLevel },
                { "Maximum color level", LinesSettings.LinesMaximumColorLevel }
            };
            ScreensaverConfig.Add("Lines", LinesConfig);

            // Dissolve config json object
            var DissolveConfig = new JObject()
            {
                { "Activate 255 colors", DissolveSettings.Dissolve255Colors },
                { "Activate true colors", DissolveSettings.DissolveTrueColor },
                { "Background color", new Color(DissolveSettings.DissolveBackgroundColor).Type == ColorType.TrueColor ? DissolveSettings.DissolveBackgroundColor.EncloseByDoubleQuotes() : DissolveSettings.DissolveBackgroundColor },
                { "Minimum red color level", DissolveSettings.DissolveMinimumRedColorLevel },
                { "Minimum green color level", DissolveSettings.DissolveMinimumGreenColorLevel },
                { "Minimum blue color level", DissolveSettings.DissolveMinimumBlueColorLevel },
                { "Minimum color level", DissolveSettings.DissolveMinimumColorLevel },
                { "Maximum red color level", DissolveSettings.DissolveMaximumRedColorLevel },
                { "Maximum green color level", DissolveSettings.DissolveMaximumGreenColorLevel },
                { "Maximum blue color level", DissolveSettings.DissolveMaximumBlueColorLevel },
                { "Maximum color level", DissolveSettings.DissolveMaximumColorLevel }
            };
            ScreensaverConfig.Add("Dissolve", DissolveConfig);

            // BouncingBlock config json object
            var BouncingBlockConfig = new JObject()
            {
                { "Activate 255 colors", BouncingBlockSettings.BouncingBlock255Colors },
                { "Activate true colors", BouncingBlockSettings.BouncingBlockTrueColor },
                { "Delay in Milliseconds", BouncingBlockSettings.BouncingBlockDelay },
                { "Background color", new Color(BouncingBlockSettings.BouncingBlockBackgroundColor).Type == ColorType.TrueColor ? BouncingBlockSettings.BouncingBlockBackgroundColor.EncloseByDoubleQuotes() : BouncingBlockSettings.BouncingBlockBackgroundColor },
                { "Foreground color", new Color(BouncingBlockSettings.BouncingBlockForegroundColor).Type == ColorType.TrueColor ? BouncingBlockSettings.BouncingBlockForegroundColor.EncloseByDoubleQuotes() : BouncingBlockSettings.BouncingBlockForegroundColor },
                { "Minimum red color level", BouncingBlockSettings.BouncingBlockMinimumRedColorLevel },
                { "Minimum green color level", BouncingBlockSettings.BouncingBlockMinimumGreenColorLevel },
                { "Minimum blue color level", BouncingBlockSettings.BouncingBlockMinimumBlueColorLevel },
                { "Minimum color level", BouncingBlockSettings.BouncingBlockMinimumColorLevel },
                { "Maximum red color level", BouncingBlockSettings.BouncingBlockMaximumRedColorLevel },
                { "Maximum green color level", BouncingBlockSettings.BouncingBlockMaximumGreenColorLevel },
                { "Maximum blue color level", BouncingBlockSettings.BouncingBlockMaximumBlueColorLevel },
                { "Maximum color level", BouncingBlockSettings.BouncingBlockMaximumColorLevel }
            };
            ScreensaverConfig.Add("BouncingBlock", BouncingBlockConfig);

            // ProgressClock config json object
            var ProgressClockConfig = new JObject()
            {
                { "Activate 255 colors", ProgressClockSettings.ProgressClock255Colors },
                { "Activate true colors", ProgressClockSettings.ProgressClockTrueColor },
                { "Cycle colors", ProgressClockSettings.ProgressClockCycleColors },
                { "Ticks to change color", ProgressClockSettings.ProgressClockCycleColorsTicks },
                { "Color of Seconds Bar", ProgressClockSettings.ProgressClockSecondsProgressColor },
                { "Color of Minutes Bar", ProgressClockSettings.ProgressClockMinutesProgressColor },
                { "Color of Hours Bar", ProgressClockSettings.ProgressClockHoursProgressColor },
                { "Color of Information", ProgressClockSettings.ProgressClockProgressColor },
                { "Delay in Milliseconds", ProgressClockSettings.ProgressClockDelay },
                { "Upper left corner character for hours bar", ProgressClockSettings.ProgressClockUpperLeftCornerCharHours },
                { "Upper left corner character for minutes bar", ProgressClockSettings.ProgressClockUpperLeftCornerCharMinutes },
                { "Upper left corner character for seconds bar", ProgressClockSettings.ProgressClockUpperLeftCornerCharSeconds },
                { "Upper right corner character for hours bar", ProgressClockSettings.ProgressClockUpperRightCornerCharHours },
                { "Upper right corner character for minutes bar", ProgressClockSettings.ProgressClockUpperRightCornerCharMinutes },
                { "Upper right corner character for seconds bar", ProgressClockSettings.ProgressClockUpperRightCornerCharSeconds },
                { "Lower left corner character for hours bar", ProgressClockSettings.ProgressClockLowerRightCornerCharHours },
                { "Lower left corner character for minutes bar", ProgressClockSettings.ProgressClockLowerLeftCornerCharMinutes },
                { "Lower left corner character for seconds bar", ProgressClockSettings.ProgressClockLowerLeftCornerCharSeconds },
                { "Lower right corner character for hours bar", ProgressClockSettings.ProgressClockLowerRightCornerCharHours },
                { "Lower right corner character for minutes bar", ProgressClockSettings.ProgressClockLowerRightCornerCharMinutes },
                { "Lower right corner character for seconds bar", ProgressClockSettings.ProgressClockLowerRightCornerCharSeconds },
                { "Upper frame character for hours bar", ProgressClockSettings.ProgressClockUpperFrameCharHours },
                { "Upper frame character for minutes bar", ProgressClockSettings.ProgressClockUpperFrameCharMinutes },
                { "Upper frame character for seconds bar", ProgressClockSettings.ProgressClockUpperFrameCharSeconds },
                { "Lower frame character for hours bar", ProgressClockSettings.ProgressClockLowerFrameCharHours },
                { "Lower frame character for minutes bar", ProgressClockSettings.ProgressClockLowerFrameCharMinutes },
                { "Lower frame character for seconds bar", ProgressClockSettings.ProgressClockLowerFrameCharSeconds },
                { "Left frame character for hours bar", ProgressClockSettings.ProgressClockLeftFrameCharHours },
                { "Left frame character for minutes bar", ProgressClockSettings.ProgressClockLeftFrameCharMinutes },
                { "Left frame character for seconds bar", ProgressClockSettings.ProgressClockLeftFrameCharSeconds },
                { "Right frame character for hours bar", ProgressClockSettings.ProgressClockRightFrameCharHours },
                { "Right frame character for minutes bar", ProgressClockSettings.ProgressClockRightFrameCharMinutes },
                { "Right frame character for seconds bar", ProgressClockSettings.ProgressClockRightFrameCharSeconds },
                { "Information text for hours", ProgressClockSettings.ProgressClockInfoTextHours },
                { "Information text for minutes", ProgressClockSettings.ProgressClockInfoTextMinutes },
                { "Information text for seconds", ProgressClockSettings.ProgressClockInfoTextSeconds },
                { "Minimum red color level for hours", ProgressClockSettings.ProgressClockMinimumRedColorLevelHours },
                { "Minimum green color level for hours", ProgressClockSettings.ProgressClockMinimumGreenColorLevelHours },
                { "Minimum blue color level for hours", ProgressClockSettings.ProgressClockMinimumBlueColorLevelHours },
                { "Minimum color level for hours", ProgressClockSettings.ProgressClockMinimumColorLevelHours },
                { "Maximum red color level for hours", ProgressClockSettings.ProgressClockMaximumRedColorLevelHours },
                { "Maximum green color level for hours", ProgressClockSettings.ProgressClockMaximumGreenColorLevelHours },
                { "Maximum blue color level for hours", ProgressClockSettings.ProgressClockMaximumBlueColorLevelHours },
                { "Maximum color level for hours", ProgressClockSettings.ProgressClockMaximumColorLevelHours },
                { "Minimum red color level for minutes", ProgressClockSettings.ProgressClockMinimumRedColorLevelMinutes },
                { "Minimum green color level for minutes", ProgressClockSettings.ProgressClockMinimumGreenColorLevelMinutes },
                { "Minimum blue color level for minutes", ProgressClockSettings.ProgressClockMinimumBlueColorLevelMinutes },
                { "Minimum color level for minutes", ProgressClockSettings.ProgressClockMinimumColorLevelMinutes },
                { "Maximum red color level for minutes", ProgressClockSettings.ProgressClockMaximumRedColorLevelMinutes },
                { "Maximum green color level for minutes", ProgressClockSettings.ProgressClockMaximumGreenColorLevelMinutes },
                { "Maximum blue color level for minutes", ProgressClockSettings.ProgressClockMaximumBlueColorLevelMinutes },
                { "Maximum color level for minutes", ProgressClockSettings.ProgressClockMaximumColorLevelMinutes },
                { "Minimum red color level for seconds", ProgressClockSettings.ProgressClockMinimumRedColorLevelSeconds },
                { "Minimum green color level for seconds", ProgressClockSettings.ProgressClockMinimumGreenColorLevelSeconds },
                { "Minimum blue color level for seconds", ProgressClockSettings.ProgressClockMinimumBlueColorLevelSeconds },
                { "Minimum color level for seconds", ProgressClockSettings.ProgressClockMinimumColorLevelSeconds },
                { "Maximum red color level for seconds", ProgressClockSettings.ProgressClockMaximumRedColorLevelSeconds },
                { "Maximum green color level for seconds", ProgressClockSettings.ProgressClockMaximumGreenColorLevelSeconds },
                { "Maximum blue color level for seconds", ProgressClockSettings.ProgressClockMaximumBlueColorLevelSeconds },
                { "Maximum color level for seconds", ProgressClockSettings.ProgressClockMaximumColorLevelSeconds },
                { "Minimum red color level", ProgressClockSettings.ProgressClockMinimumRedColorLevel },
                { "Minimum green color level", ProgressClockSettings.ProgressClockMinimumGreenColorLevel },
                { "Minimum blue color level", ProgressClockSettings.ProgressClockMinimumBlueColorLevel },
                { "Minimum color level", ProgressClockSettings.ProgressClockMinimumColorLevel },
                { "Maximum red color level", ProgressClockSettings.ProgressClockMaximumRedColorLevel },
                { "Maximum green color level", ProgressClockSettings.ProgressClockMaximumGreenColorLevel },
                { "Maximum blue color level", ProgressClockSettings.ProgressClockMaximumBlueColorLevel },
                { "Maximum color level", ProgressClockSettings.ProgressClockMaximumColorLevel }
            };
            ScreensaverConfig.Add("ProgressClock", ProgressClockConfig);

            // Lighter config json object
            var LighterConfig = new JObject()
            {
                { "Activate 255 colors", LighterSettings.Lighter255Colors },
                { "Activate true colors", LighterSettings.LighterTrueColor },
                { "Delay in Milliseconds", LighterSettings.LighterDelay },
                { "Max Positions Count", LighterSettings.LighterMaxPositions },
                { "Background color", new Color(LighterSettings.LighterBackgroundColor).Type == ColorType.TrueColor ? LighterSettings.LighterBackgroundColor.EncloseByDoubleQuotes() : LighterSettings.LighterBackgroundColor },
                { "Minimum red color level", LighterSettings.LighterMinimumRedColorLevel },
                { "Minimum green color level", LighterSettings.LighterMinimumGreenColorLevel },
                { "Minimum blue color level", LighterSettings.LighterMinimumBlueColorLevel },
                { "Minimum color level", LighterSettings.LighterMinimumColorLevel },
                { "Maximum red color level", LighterSettings.LighterMaximumRedColorLevel },
                { "Maximum green color level", LighterSettings.LighterMaximumGreenColorLevel },
                { "Maximum blue color level", LighterSettings.LighterMaximumBlueColorLevel },
                { "Maximum color level", LighterSettings.LighterMaximumColorLevel }
            };
            ScreensaverConfig.Add("Lighter", LighterConfig);

            // Wipe config json object
            var WipeConfig = new JObject()
            {
                { "Activate 255 colors", WipeSettings.Wipe255Colors },
                { "Activate true colors", WipeSettings.WipeTrueColor },
                { "Delay in Milliseconds", WipeSettings.WipeDelay },
                { "Wipes to change direction", WipeSettings.WipeWipesNeededToChangeDirection },
                { "Background color", new Color(WipeSettings.WipeBackgroundColor).Type == ColorType.TrueColor ? WipeSettings.WipeBackgroundColor.EncloseByDoubleQuotes() : WipeSettings.WipeBackgroundColor },
                { "Minimum red color level", WipeSettings.WipeMinimumRedColorLevel },
                { "Minimum green color level", WipeSettings.WipeMinimumGreenColorLevel },
                { "Minimum blue color level", WipeSettings.WipeMinimumBlueColorLevel },
                { "Minimum color level", WipeSettings.WipeMinimumColorLevel },
                { "Maximum red color level", WipeSettings.WipeMaximumRedColorLevel },
                { "Maximum green color level", WipeSettings.WipeMaximumGreenColorLevel },
                { "Maximum blue color level", WipeSettings.WipeMaximumBlueColorLevel },
                { "Maximum color level", WipeSettings.WipeMaximumColorLevel }
            };
            ScreensaverConfig.Add("Wipe", WipeConfig);

            // Matrix config json object
            var MatrixConfig = new JObject()
            {
                { "Delay in Milliseconds", MatrixSettings.MatrixDelay }
            };
            ScreensaverConfig.Add("Matrix", MatrixConfig);

            // GlitterMatrix config json object
            var GlitterMatrixConfig = new JObject()
            {
                { "Delay in Milliseconds", GlitterMatrixSettings.GlitterMatrixDelay },
                { "Background color", new Color(GlitterMatrixSettings.GlitterMatrixBackgroundColor).Type == ColorType.TrueColor ? GlitterMatrixSettings.GlitterMatrixBackgroundColor.EncloseByDoubleQuotes() : GlitterMatrixSettings.GlitterMatrixBackgroundColor },
                { "Foreground color", new Color(GlitterMatrixSettings.GlitterMatrixForegroundColor).Type == ColorType.TrueColor ? GlitterMatrixSettings.GlitterMatrixForegroundColor.EncloseByDoubleQuotes() : GlitterMatrixSettings.GlitterMatrixForegroundColor }
            };
            ScreensaverConfig.Add("GlitterMatrix", GlitterMatrixConfig);

            // BouncingText config json object
            var BouncingTextConfig = new JObject()
            {
                { "Activate 255 colors", BouncingTextSettings.BouncingText255Colors },
                { "Activate true colors", BouncingTextSettings.BouncingTextTrueColor },
                { "Delay in Milliseconds", BouncingTextSettings.BouncingTextDelay },
                { "Text shown", BouncingTextSettings.BouncingTextWrite },
                { "Background color", new Color(BouncingTextSettings.BouncingTextBackgroundColor).Type == ColorType.TrueColor ? BouncingTextSettings.BouncingTextBackgroundColor.EncloseByDoubleQuotes() : BouncingTextSettings.BouncingTextBackgroundColor },
                { "Foreground color", new Color(BouncingTextSettings.BouncingTextForegroundColor).Type == ColorType.TrueColor ? BouncingTextSettings.BouncingTextForegroundColor.EncloseByDoubleQuotes() : BouncingTextSettings.BouncingTextForegroundColor },
                { "Minimum red color level", BouncingTextSettings.BouncingTextMinimumRedColorLevel },
                { "Minimum green color level", BouncingTextSettings.BouncingTextMinimumGreenColorLevel },
                { "Minimum blue color level", BouncingTextSettings.BouncingTextMinimumBlueColorLevel },
                { "Minimum color level", BouncingTextSettings.BouncingTextMinimumColorLevel },
                { "Maximum red color level", BouncingTextSettings.BouncingTextMaximumRedColorLevel },
                { "Maximum green color level", BouncingTextSettings.BouncingTextMaximumGreenColorLevel },
                { "Maximum blue color level", BouncingTextSettings.BouncingTextMaximumBlueColorLevel },
                { "Maximum color level", BouncingTextSettings.BouncingTextMaximumColorLevel }
            };
            ScreensaverConfig.Add("BouncingText", BouncingTextConfig);

            // Fader config json object
            var FaderConfig = new JObject()
            {
                { "Delay in Milliseconds", FaderSettings.FaderDelay },
                { "Fade Out Delay in Milliseconds", FaderSettings.FaderFadeOutDelay },
                { "Text shown", FaderSettings.FaderWrite },
                { "Max Fade Steps", FaderSettings.FaderMaxSteps },
                { "Background color", new Color(FaderSettings.FaderBackgroundColor).Type == ColorType.TrueColor ? FaderSettings.FaderBackgroundColor.EncloseByDoubleQuotes() : FaderSettings.FaderBackgroundColor },
                { "Minimum red color level", FaderSettings.FaderMinimumRedColorLevel },
                { "Minimum green color level", FaderSettings.FaderMinimumGreenColorLevel },
                { "Minimum blue color level", FaderSettings.FaderMinimumBlueColorLevel },
                { "Maximum red color level", FaderSettings.FaderMaximumRedColorLevel },
                { "Maximum green color level", FaderSettings.FaderMaximumGreenColorLevel },
                { "Maximum blue color level", FaderSettings.FaderMaximumBlueColorLevel }
            };
            ScreensaverConfig.Add("Fader", FaderConfig);

            // FaderBack config json object
            var FaderBackConfig = new JObject()
            {
                { "Delay in Milliseconds", FaderBackSettings.FaderBackDelay },
                { "Fade Out Delay in Milliseconds", FaderBackSettings.FaderBackFadeOutDelay },
                { "Max Fade Steps", FaderBackSettings.FaderBackMaxSteps },
                { "Minimum red color level", FaderBackSettings.FaderBackMinimumRedColorLevel },
                { "Minimum green color level", FaderBackSettings.FaderBackMinimumGreenColorLevel },
                { "Minimum blue color level", FaderBackSettings.FaderBackMinimumBlueColorLevel },
                { "Maximum red color level", FaderBackSettings.FaderBackMaximumRedColorLevel },
                { "Maximum green color level", FaderBackSettings.FaderBackMaximumGreenColorLevel },
                { "Maximum blue color level", FaderBackSettings.FaderBackMaximumBlueColorLevel }
            };
            ScreensaverConfig.Add("FaderBack", FaderBackConfig);

            // BeatFader config json object
            var BeatFaderConfig = new JObject()
            {
                { "Activate 255 colors", BeatFaderSettings.BeatFader255Colors },
                { "Activate true colors", BeatFaderSettings.BeatFaderTrueColor },
                { "Delay in Beats Per Minute", BeatFaderSettings.BeatFaderDelay },
                { "Cycle colors", BeatFaderSettings.BeatFaderCycleColors },
                { "Beat color", BeatFaderSettings.BeatFaderBeatColor },
                { "Max Fade Steps", BeatFaderSettings.BeatFaderMaxSteps },
                { "Minimum red color level", BeatFaderSettings.BeatFaderMinimumRedColorLevel },
                { "Minimum green color level", BeatFaderSettings.BeatFaderMinimumGreenColorLevel },
                { "Minimum blue color level", BeatFaderSettings.BeatFaderMinimumBlueColorLevel },
                { "Minimum color level", BeatFaderSettings.BeatFaderMinimumColorLevel },
                { "Maximum red color level", BeatFaderSettings.BeatFaderMaximumRedColorLevel },
                { "Maximum green color level", BeatFaderSettings.BeatFaderMaximumGreenColorLevel },
                { "Maximum blue color level", BeatFaderSettings.BeatFaderMaximumBlueColorLevel },
                { "Maximum color level", BeatFaderSettings.BeatFaderMaximumColorLevel }
            };
            ScreensaverConfig.Add("BeatFader", BeatFaderConfig);

            // Typo config json object
            var TypoConfig = new JObject()
            {
                { "Delay in Milliseconds", TypoSettings.TypoDelay },
                { "Write Again Delay in Milliseconds", TypoSettings.TypoWriteAgainDelay },
                { "Text shown", TypoSettings.TypoWrite },
                { "Minimum writing speed in WPM", TypoSettings.TypoWritingSpeedMin },
                { "Maximum writing speed in WPM", TypoSettings.TypoWritingSpeedMax },
                { "Probability of typo in percent", TypoSettings.TypoMissStrikePossibility },
                { "Probability of miss in percent", TypoSettings.TypoMissPossibility },
                { "Text color", new Color(TypoSettings.TypoTextColor).Type == ColorType.TrueColor ? TypoSettings.TypoTextColor.EncloseByDoubleQuotes() : TypoSettings.TypoTextColor }
            };
            ScreensaverConfig.Add("Typo", TypoConfig);

            // Marquee config json object
            var MarqueeConfig = new JObject()
            {
                { "Activate 255 colors", MarqueeSettings.Marquee255Colors },
                { "Activate true colors", MarqueeSettings.MarqueeTrueColor },
                { "Delay in Milliseconds", MarqueeSettings.MarqueeDelay },
                { "Text shown", MarqueeSettings.MarqueeWrite },
                { "Always centered", MarqueeSettings.MarqueeAlwaysCentered },
                { "Use Console API", MarqueeSettings.MarqueeUseConsoleAPI },
                { "Background color", new Color(MarqueeSettings.MarqueeBackgroundColor).Type == ColorType.TrueColor ? MarqueeSettings.MarqueeBackgroundColor.EncloseByDoubleQuotes() : MarqueeSettings.MarqueeBackgroundColor },
                { "Minimum red color level", MarqueeSettings.MarqueeMinimumRedColorLevel },
                { "Minimum green color level", MarqueeSettings.MarqueeMinimumGreenColorLevel },
                { "Minimum blue color level", MarqueeSettings.MarqueeMinimumBlueColorLevel },
                { "Minimum color level", MarqueeSettings.MarqueeMinimumColorLevel },
                { "Maximum red color level", MarqueeSettings.MarqueeMaximumRedColorLevel },
                { "Maximum green color level", MarqueeSettings.MarqueeMaximumGreenColorLevel },
                { "Maximum blue color level", MarqueeSettings.MarqueeMaximumBlueColorLevel },
                { "Maximum color level", MarqueeSettings.MarqueeMaximumColorLevel }
            };
            ScreensaverConfig.Add("Marquee", MarqueeConfig);

            // Linotypo config json object
            var LinotypoConfig = new JObject()
            {
                { "Delay in Milliseconds", LinotypoSettings.LinotypoDelay },
                { "New Screen Delay in Milliseconds", LinotypoSettings.LinotypoNewScreenDelay },
                { "Text shown", LinotypoSettings.LinotypoWrite },
                { "Minimum writing speed in WPM", LinotypoSettings.LinotypoWritingSpeedMin },
                { "Maximum writing speed in WPM", LinotypoSettings.LinotypoWritingSpeedMax },
                { "Probability of typo in percent", LinotypoSettings.LinotypoMissStrikePossibility },
                { "Column Count", LinotypoSettings.LinotypoTextColumns },
                { "Line Fill Threshold", LinotypoSettings.LinotypoEtaoinThreshold },
                { "Line Fill Capping Probability in percent", LinotypoSettings.LinotypoEtaoinCappingPossibility },
                { "Line Fill Type", LinotypoSettings.LinotypoEtaoinType.ToString() },
                { "Probability of miss in percent", LinotypoSettings.LinotypoMissPossibility },
                { "Text color", new Color(LinotypoSettings.LinotypoTextColor).Type == ColorType.TrueColor ? LinotypoSettings.LinotypoTextColor.EncloseByDoubleQuotes() : LinotypoSettings.LinotypoTextColor }
            };
            ScreensaverConfig.Add("Linotypo", LinotypoConfig);

            // Typewriter config json object
            var TypewriterConfig = new JObject()
            {
                { "Delay in Milliseconds", TypewriterSettings.TypewriterDelay },
                { "New Screen Delay in Milliseconds", TypewriterSettings.TypewriterNewScreenDelay },
                { "Text shown", TypewriterSettings.TypewriterWrite },
                { "Minimum writing speed in WPM", TypewriterSettings.TypewriterWritingSpeedMin },
                { "Maximum writing speed in WPM", TypewriterSettings.TypewriterWritingSpeedMax },
                { "Text color", new Color(TypewriterSettings.TypewriterTextColor).Type == ColorType.TrueColor ? TypewriterSettings.TypewriterTextColor.EncloseByDoubleQuotes() : TypewriterSettings.TypewriterTextColor }
            };
            ScreensaverConfig.Add("Typewriter", TypewriterConfig);

            // FlashColor config json object
            var FlashColorConfig = new JObject()
            {
                { "Activate 255 colors", FlashColorSettings.FlashColor255Colors },
                { "Activate true colors", FlashColorSettings.FlashColorTrueColor },
                { "Delay in Milliseconds", FlashColorSettings.FlashColorDelay },
                { "Background color", new Color(FlashColorSettings.FlashColorBackgroundColor).Type == ColorType.TrueColor ? FlashColorSettings.FlashColorBackgroundColor.EncloseByDoubleQuotes() : FlashColorSettings.FlashColorBackgroundColor },
                { "Minimum red color level", FlashColorSettings.FlashColorMinimumRedColorLevel },
                { "Minimum green color level", FlashColorSettings.FlashColorMinimumGreenColorLevel },
                { "Minimum blue color level", FlashColorSettings.FlashColorMinimumBlueColorLevel },
                { "Minimum color level", FlashColorSettings.FlashColorMinimumColorLevel },
                { "Maximum red color level", FlashColorSettings.FlashColorMaximumRedColorLevel },
                { "Maximum green color level", FlashColorSettings.FlashColorMaximumGreenColorLevel },
                { "Maximum blue color level", FlashColorSettings.FlashColorMaximumBlueColorLevel },
                { "Maximum color level", FlashColorSettings.FlashColorMaximumColorLevel }
            };
            ScreensaverConfig.Add("FlashColor", FlashColorConfig);

            // SpotWrite config json object
            var SpotWriteonfig = new JObject()
            {
                { "Delay in Milliseconds", SpotWriteSettings.SpotWriteDelay },
                { "New Screen Delay in Milliseconds", SpotWriteSettings.SpotWriteNewScreenDelay },
                { "Text shown", SpotWriteSettings.SpotWriteWrite },
                { "Text color", SpotWriteSettings.SpotWriteTextColor }
            };
            ScreensaverConfig.Add("SpotWrite", SpotWriteonfig);

            // Ramp config json object
            var RampConfig = new JObject()
            {
                { "Activate 255 colors", RampSettings.Ramp255Colors },
                { "Activate true colors", RampSettings.RampTrueColor },
                { "Delay in Milliseconds", RampSettings.RampDelay },
                { "Next ramp interval", RampSettings.RampDelay },
                { "Upper left corner character for ramp bar", RampSettings.RampUpperLeftCornerChar },
                { "Upper right corner character for ramp bar", RampSettings.RampUpperRightCornerChar },
                { "Lower left corner character for ramp bar", RampSettings.RampLowerLeftCornerChar },
                { "Lower right corner character for ramp bar", RampSettings.RampLowerRightCornerChar },
                { "Upper frame character for ramp bar", RampSettings.RampUpperFrameChar },
                { "Lower frame character for ramp bar", RampSettings.RampLowerFrameChar },
                { "Left frame character for ramp bar", RampSettings.RampLeftFrameChar },
                { "Right frame character for ramp bar", RampSettings.RampRightFrameChar },
                { "Minimum red color level for start color", RampSettings.RampMinimumRedColorLevelStart },
                { "Minimum green color level for start color", RampSettings.RampMinimumGreenColorLevelStart },
                { "Minimum blue color level for start color", RampSettings.RampMinimumBlueColorLevelStart },
                { "Minimum color level for start color", RampSettings.RampMinimumColorLevelStart },
                { "Maximum red color level for start color", RampSettings.RampMaximumRedColorLevelStart },
                { "Maximum green color level for start color", RampSettings.RampMaximumGreenColorLevelStart },
                { "Maximum blue color level for start color", RampSettings.RampMaximumBlueColorLevelStart },
                { "Maximum color level for start color", RampSettings.RampMaximumColorLevelStart },
                { "Minimum red color level for end color", RampSettings.RampMinimumRedColorLevelEnd },
                { "Minimum green color level for end color", RampSettings.RampMinimumGreenColorLevelEnd },
                { "Minimum blue color level for end color", RampSettings.RampMinimumBlueColorLevelEnd },
                { "Minimum color level for end color", RampSettings.RampMinimumColorLevelEnd },
                { "Maximum red color level for end color", RampSettings.RampMaximumRedColorLevelEnd },
                { "Maximum green color level for end color", RampSettings.RampMaximumGreenColorLevelEnd },
                { "Maximum blue color level for end color", RampSettings.RampMaximumBlueColorLevelEnd },
                { "Maximum color level for end color", RampSettings.RampMaximumColorLevelEnd },
                { "Upper left corner color for ramp bar", RampSettings.RampUpperLeftCornerColor },
                { "Upper right corner color for ramp bar", RampSettings.RampUpperRightCornerColor },
                { "Lower left corner color for ramp bar", RampSettings.RampLowerLeftCornerColor },
                { "Lower right corner color for ramp bar", RampSettings.RampLowerRightCornerColor },
                { "Upper frame color for ramp bar", RampSettings.RampUpperFrameColor },
                { "Lower frame color for ramp bar", RampSettings.RampLowerFrameColor },
                { "Left frame color for ramp bar", RampSettings.RampLeftFrameColor },
                { "Right frame color for ramp bar", RampSettings.RampRightFrameColor },
                { "Use border colors for ramp bar", RampSettings.RampUseBorderColors }
            };
            ScreensaverConfig.Add("Ramp", RampConfig);

            // StackBox config json object
            var StackBoxConfig = new JObject()
            {
                { "Activate 255 colors", StackBoxSettings.StackBox255Colors },
                { "Activate true colors", StackBoxSettings.StackBoxTrueColor },
                { "Delay in Milliseconds", StackBoxSettings.StackBoxDelay },
                { "Minimum red color level", StackBoxSettings.StackBoxMinimumRedColorLevel },
                { "Minimum green color level", StackBoxSettings.StackBoxMinimumGreenColorLevel },
                { "Minimum blue color level", StackBoxSettings.StackBoxMinimumBlueColorLevel },
                { "Minimum color level", StackBoxSettings.StackBoxMinimumColorLevel },
                { "Maximum red color level", StackBoxSettings.StackBoxMaximumRedColorLevel },
                { "Maximum green color level", StackBoxSettings.StackBoxMaximumGreenColorLevel },
                { "Maximum blue color level", StackBoxSettings.StackBoxMaximumBlueColorLevel },
                { "Maximum color level", StackBoxSettings.StackBoxMaximumColorLevel },
                { "Fill the boxes", StackBoxSettings.StackBoxFill }
            };
            ScreensaverConfig.Add("StackBox", StackBoxConfig);

            // Snaker config json object
            var SnakerConfig = new JObject()
            {
                { "Activate 255 colors", SnakerSettings.Snaker255Colors },
                { "Activate true colors", SnakerSettings.SnakerTrueColor },
                { "Delay in Milliseconds", SnakerSettings.SnakerDelay },
                { "Stage delay in milliseconds", SnakerSettings.SnakerStageDelay },
                { "Minimum red color level", SnakerSettings.SnakerMinimumRedColorLevel },
                { "Minimum green color level", SnakerSettings.SnakerMinimumGreenColorLevel },
                { "Minimum blue color level", SnakerSettings.SnakerMinimumBlueColorLevel },
                { "Minimum color level", SnakerSettings.SnakerMinimumColorLevel },
                { "Maximum red color level", SnakerSettings.SnakerMaximumRedColorLevel },
                { "Maximum green color level", SnakerSettings.SnakerMaximumGreenColorLevel },
                { "Maximum blue color level", SnakerSettings.SnakerMaximumBlueColorLevel },
                { "Maximum color level", SnakerSettings.SnakerMaximumColorLevel }
            };
            ScreensaverConfig.Add("Snaker", SnakerConfig);

            // BarRot config json object
            var BarRotConfig = new JObject()
            {
                { "Activate 255 colors", BarRotSettings.BarRot255Colors },
                { "Activate true colors", BarRotSettings.BarRotTrueColor },
                { "Delay in Milliseconds", BarRotSettings.BarRotDelay },
                { "Next ramp rot interval", BarRotSettings.BarRotNextRampDelay },
                { "Upper left corner character for ramp bar", BarRotSettings.BarRotUpperLeftCornerChar },
                { "Upper right corner character for ramp bar", BarRotSettings.BarRotUpperRightCornerChar },
                { "Lower left corner character for ramp bar", BarRotSettings.BarRotLowerLeftCornerChar },
                { "Lower right corner character for ramp bar", BarRotSettings.BarRotLowerRightCornerChar },
                { "Upper frame character for ramp bar", BarRotSettings.BarRotUpperFrameChar },
                { "Lower frame character for ramp bar", BarRotSettings.BarRotLowerFrameChar },
                { "Left frame character for ramp bar", BarRotSettings.BarRotLeftFrameChar },
                { "Right frame character for ramp bar", BarRotSettings.BarRotRightFrameChar },
                { "Minimum red color level for start color", BarRotSettings.BarRotMinimumRedColorLevelStart },
                { "Minimum green color level for start color", BarRotSettings.BarRotMinimumGreenColorLevelStart },
                { "Minimum blue color level for start color", BarRotSettings.BarRotMinimumBlueColorLevelStart },
                { "Maximum red color level for start color", BarRotSettings.BarRotMaximumRedColorLevelStart },
                { "Maximum green color level for start color", BarRotSettings.BarRotMaximumGreenColorLevelStart },
                { "Maximum blue color level for start color", BarRotSettings.BarRotMaximumBlueColorLevelStart },
                { "Minimum red color level for end color", BarRotSettings.BarRotMinimumRedColorLevelEnd },
                { "Minimum green color level for end color", BarRotSettings.BarRotMinimumGreenColorLevelEnd },
                { "Minimum blue color level for end color", BarRotSettings.BarRotMinimumBlueColorLevelEnd },
                { "Maximum red color level for end color", BarRotSettings.BarRotMaximumRedColorLevelEnd },
                { "Maximum green color level for end color", BarRotSettings.BarRotMaximumGreenColorLevelEnd },
                { "Maximum blue color level for end color", BarRotSettings.BarRotMaximumBlueColorLevelEnd },
                { "Upper left corner color for ramp bar", BarRotSettings.BarRotUpperLeftCornerColor },
                { "Upper right corner color for ramp bar", BarRotSettings.BarRotUpperRightCornerColor },
                { "Lower left corner color for ramp bar", BarRotSettings.BarRotLowerLeftCornerColor },
                { "Lower right corner color for ramp bar", BarRotSettings.BarRotLowerRightCornerColor },
                { "Upper frame color for ramp bar", BarRotSettings.BarRotUpperFrameColor },
                { "Lower frame color for ramp bar", BarRotSettings.BarRotLowerFrameColor },
                { "Left frame color for ramp bar", BarRotSettings.BarRotLeftFrameColor },
                { "Right frame color for ramp bar", BarRotSettings.BarRotRightFrameColor },
                { "Use border colors for ramp bar", BarRotSettings.BarRotUseBorderColors }
            };
            ScreensaverConfig.Add("BarRot", BarRotConfig);

            // Fireworks config json object
            var FireworksConfig = new JObject()
            {
                { "Activate 255 colors", FireworksSettings.Fireworks255Colors },
                { "Activate true colors", FireworksSettings.FireworksTrueColor },
                { "Delay in Milliseconds", FireworksSettings.FireworksDelay },
                { "Firework explosion radius", FireworksSettings.FireworksRadius },
                { "Minimum red color level", FireworksSettings.FireworksMinimumRedColorLevel },
                { "Minimum green color level", FireworksSettings.FireworksMinimumGreenColorLevel },
                { "Minimum blue color level", FireworksSettings.FireworksMinimumBlueColorLevel },
                { "Minimum color level", FireworksSettings.FireworksMinimumColorLevel },
                { "Maximum red color level", FireworksSettings.FireworksMaximumRedColorLevel },
                { "Maximum green color level", FireworksSettings.FireworksMaximumGreenColorLevel },
                { "Maximum blue color level", FireworksSettings.FireworksMaximumBlueColorLevel },
                { "Maximum color level", FireworksSettings.FireworksMaximumColorLevel }
            };
            ScreensaverConfig.Add("Fireworks", FireworksConfig);

            // Figlet config json object
            var FigletConfig = new JObject()
            {
                { "Activate 255 colors", FigletSettings.Figlet255Colors },
                { "Activate true colors", FigletSettings.FigletTrueColor },
                { "Delay in Milliseconds", FigletSettings.FigletDelay },
                { "Text shown", FigletSettings.FigletText },
                { "Figlet font", FigletSettings.FigletFont },
                { "Minimum red color level", FigletSettings.FigletMinimumRedColorLevel },
                { "Minimum green color level", FigletSettings.FigletMinimumGreenColorLevel },
                { "Minimum blue color level", FigletSettings.FigletMinimumBlueColorLevel },
                { "Minimum color level", FigletSettings.FigletMinimumColorLevel },
                { "Maximum red color level", FigletSettings.FigletMaximumRedColorLevel },
                { "Maximum green color level", FigletSettings.FigletMaximumGreenColorLevel },
                { "Maximum blue color level", FigletSettings.FigletMaximumBlueColorLevel },
                { "Maximum color level", FigletSettings.FigletMaximumColorLevel }
            };
            ScreensaverConfig.Add("Figlet", FigletConfig);

            // FlashText config json object
            var FlashTextConfig = new JObject()
            {
                { "Activate 255 colors", FlashTextSettings.FlashText255Colors },
                { "Activate true colors", FlashTextSettings.FlashTextTrueColor },
                { "Delay in Milliseconds", FlashTextSettings.FlashTextDelay },
                { "Text shown", FlashTextSettings.FlashTextWrite },
                { "Background color", new Color(FlashTextSettings.FlashTextBackgroundColor).Type == ColorType.TrueColor ? FlashTextSettings.FlashTextBackgroundColor.EncloseByDoubleQuotes() : FlashTextSettings.FlashTextBackgroundColor },
                { "Minimum red color level", FlashTextSettings.FlashTextMinimumRedColorLevel },
                { "Minimum green color level", FlashTextSettings.FlashTextMinimumGreenColorLevel },
                { "Minimum blue color level", FlashTextSettings.FlashTextMinimumBlueColorLevel },
                { "Minimum color level", FlashTextSettings.FlashTextMinimumColorLevel },
                { "Maximum red color level", FlashTextSettings.FlashTextMaximumRedColorLevel },
                { "Maximum green color level", FlashTextSettings.FlashTextMaximumGreenColorLevel },
                { "Maximum blue color level", FlashTextSettings.FlashTextMaximumBlueColorLevel },
                { "Maximum color level", FlashTextSettings.FlashTextMaximumColorLevel }
            };
            ScreensaverConfig.Add("FlashText", FlashTextConfig);

            // Noise config json object
            var NoiseConfig = new JObject()
            {
                { "New Screen Delay in Milliseconds", NoiseSettings.NoiseNewScreenDelay },
                { "Noise density", NoiseSettings.NoiseDensity }
            };
            ScreensaverConfig.Add("Noise", NoiseConfig);

            // PersonLookup config json object
            var PersonLookupConfig = new JObject()
            {
                { "Delay in Milliseconds", PersonLookupSettings.PersonLookupDelay },
                { "New Screen Delay in Milliseconds", PersonLookupSettings.PersonLookupLookedUpDelay },
                { "Minimum names count", PersonLookupSettings.PersonLookupMinimumNames },
                { "Maximum names count", PersonLookupSettings.PersonLookupMaximumNames },
                { "Minimum age years count", PersonLookupSettings.PersonLookupMinimumAgeYears },
                { "Maximum age years count", PersonLookupSettings.PersonLookupMaximumAgeYears }
            };
            ScreensaverConfig.Add("PersonLookup", PersonLookupConfig);

            // DateAndTime config json object
            var DateAndTimeConfig = new JObject()
            {
                { "Activate 255 colors", DateAndTimeSettings.DateAndTime255Colors },
                { "Activate true colors", DateAndTimeSettings.DateAndTimeTrueColor },
                { "Delay in Milliseconds", DateAndTimeSettings.DateAndTimeDelay },
                { "Minimum red color level", DateAndTimeSettings.DateAndTimeMinimumRedColorLevel },
                { "Minimum green color level", DateAndTimeSettings.DateAndTimeMinimumGreenColorLevel },
                { "Minimum blue color level", DateAndTimeSettings.DateAndTimeMinimumBlueColorLevel },
                { "Minimum color level", DateAndTimeSettings.DateAndTimeMinimumColorLevel },
                { "Maximum red color level", DateAndTimeSettings.DateAndTimeMaximumRedColorLevel },
                { "Maximum green color level", DateAndTimeSettings.DateAndTimeMaximumGreenColorLevel },
                { "Maximum blue color level", DateAndTimeSettings.DateAndTimeMaximumBlueColorLevel },
                { "Maximum color level", DateAndTimeSettings.DateAndTimeMaximumColorLevel }
            };
            ScreensaverConfig.Add("DateAndTime", DateAndTimeConfig);

            // Glitch config json object
            var GlitchConfig = new JObject()
            {
                { "Delay in Milliseconds", GlitchSettings.GlitchDelay },
                { "Glitch density", GlitchSettings.GlitchDensity }
            };
            ScreensaverConfig.Add("Glitch", GlitchConfig);

            // FallingLine config json object
            var FallingLineConfig = new JObject()
            {
                { "Activate 255 colors", FallingLineSettings.FallingLine255Colors },
                { "Activate true colors", FallingLineSettings.FallingLineTrueColor },
                { "Delay in Milliseconds", FallingLineSettings.FallingLineDelay },
                { "Max Fade Steps", FallingLineSettings.FallingLineMaxSteps },
                { "Minimum red color level", FallingLineSettings.FallingLineMinimumRedColorLevel },
                { "Minimum green color level", FallingLineSettings.FallingLineMinimumGreenColorLevel },
                { "Minimum blue color level", FallingLineSettings.FallingLineMinimumBlueColorLevel },
                { "Minimum color level", FallingLineSettings.FallingLineMinimumColorLevel },
                { "Maximum red color level", FallingLineSettings.FallingLineMaximumRedColorLevel },
                { "Maximum green color level", FallingLineSettings.FallingLineMaximumGreenColorLevel },
                { "Maximum blue color level", FallingLineSettings.FallingLineMaximumBlueColorLevel },
                { "Maximum color level", FallingLineSettings.FallingLineMaximumColorLevel }
            };
            ScreensaverConfig.Add("FallingLine", FallingLineConfig);

            // Indeterminate config json object
            var IndeterminateConfig = new JObject()
            {
                { "Activate 255 colors", IndeterminateSettings.Indeterminate255Colors },
                { "Activate true colors", IndeterminateSettings.IndeterminateTrueColor },
                { "Delay in Milliseconds", IndeterminateSettings.IndeterminateDelay },
                { "Upper left corner character for ramp bar", IndeterminateSettings.IndeterminateUpperLeftCornerChar },
                { "Upper right corner character for ramp bar", IndeterminateSettings.IndeterminateUpperRightCornerChar },
                { "Lower left corner character for ramp bar", IndeterminateSettings.IndeterminateLowerLeftCornerChar },
                { "Lower right corner character for ramp bar", IndeterminateSettings.IndeterminateLowerRightCornerChar },
                { "Upper frame character for ramp bar", IndeterminateSettings.IndeterminateUpperFrameChar },
                { "Lower frame character for ramp bar", IndeterminateSettings.IndeterminateLowerFrameChar },
                { "Left frame character for ramp bar", IndeterminateSettings.IndeterminateLeftFrameChar },
                { "Right frame character for ramp bar", IndeterminateSettings.IndeterminateRightFrameChar },
                { "Minimum red color level", IndeterminateSettings.IndeterminateMinimumRedColorLevel },
                { "Minimum green color level", IndeterminateSettings.IndeterminateMinimumGreenColorLevel },
                { "Minimum blue color level", IndeterminateSettings.IndeterminateMinimumBlueColorLevel },
                { "Minimum color level", IndeterminateSettings.IndeterminateMinimumColorLevel },
                { "Maximum red color level", IndeterminateSettings.IndeterminateMaximumRedColorLevel },
                { "Maximum green color level", IndeterminateSettings.IndeterminateMaximumGreenColorLevel },
                { "Maximum blue color level", IndeterminateSettings.IndeterminateMaximumBlueColorLevel },
                { "Maximum color level", IndeterminateSettings.IndeterminateMaximumColorLevel },
                { "Upper left corner color for ramp bar", IndeterminateSettings.IndeterminateUpperLeftCornerColor },
                { "Upper right corner color for ramp bar", IndeterminateSettings.IndeterminateUpperRightCornerColor },
                { "Lower left corner color for ramp bar", IndeterminateSettings.IndeterminateLowerLeftCornerColor },
                { "Lower right corner color for ramp bar", IndeterminateSettings.IndeterminateLowerRightCornerColor },
                { "Upper frame color for ramp bar", IndeterminateSettings.IndeterminateUpperFrameColor },
                { "Lower frame color for ramp bar", IndeterminateSettings.IndeterminateLowerFrameColor },
                { "Left frame color for ramp bar", IndeterminateSettings.IndeterminateLeftFrameColor },
                { "Right frame color for ramp bar", IndeterminateSettings.IndeterminateRightFrameColor },
                { "Use border colors for ramp bar", IndeterminateSettings.IndeterminateUseBorderColors }
            };
            ScreensaverConfig.Add("Indeterminate", IndeterminateConfig);

            // Pulse config json object
            var PulseConfig = new JObject()
            {
                { "Delay in Milliseconds", PulseSettings.PulseDelay },
                { "Max Fade Steps", PulseSettings.PulseMaxSteps },
                { "Minimum red color level", PulseSettings.PulseMinimumRedColorLevel },
                { "Minimum green color level", PulseSettings.PulseMinimumGreenColorLevel },
                { "Minimum blue color level", PulseSettings.PulseMinimumBlueColorLevel },
                { "Maximum red color level", PulseSettings.PulseMaximumRedColorLevel },
                { "Maximum green color level", PulseSettings.PulseMaximumGreenColorLevel },
                { "Maximum blue color level", PulseSettings.PulseMaximumBlueColorLevel }
            };
            ScreensaverConfig.Add("Pulse", PulseConfig);

            // BeatPulse config json object
            var BeatPulseConfig = new JObject()
            {
                { "Activate 255 colors", BeatPulseSettings.BeatPulse255Colors },
                { "Activate true colors", BeatPulseSettings.BeatPulseTrueColor },
                { "Delay in Beats Per Minute", BeatPulseSettings.BeatPulseDelay },
                { "Cycle colors", BeatPulseSettings.BeatPulseCycleColors },
                { "Beat color", BeatPulseSettings.BeatPulseBeatColor },
                { "Max Fade Steps", BeatPulseSettings.BeatPulseMaxSteps },
                { "Minimum red color level", BeatPulseSettings.BeatPulseMinimumRedColorLevel },
                { "Minimum green color level", BeatPulseSettings.BeatPulseMinimumGreenColorLevel },
                { "Minimum blue color level", BeatPulseSettings.BeatPulseMinimumBlueColorLevel },
                { "Minimum color level", BeatPulseSettings.BeatPulseMinimumColorLevel },
                { "Maximum red color level", BeatPulseSettings.BeatPulseMaximumRedColorLevel },
                { "Maximum green color level", BeatPulseSettings.BeatPulseMaximumGreenColorLevel },
                { "Maximum blue color level", BeatPulseSettings.BeatPulseMaximumBlueColorLevel },
                { "Maximum color level", BeatPulseSettings.BeatPulseMaximumColorLevel }
            };
            ScreensaverConfig.Add("BeatPulse", BeatPulseConfig);

            // EdgePulse config json object
            var EdgePulseConfig = new JObject()
            {
                { "Delay in Milliseconds", EdgePulseSettings.EdgePulseDelay },
                { "Max Fade Steps", EdgePulseSettings.EdgePulseMaxSteps },
                { "Minimum red color level", EdgePulseSettings.EdgePulseMinimumRedColorLevel },
                { "Minimum green color level", EdgePulseSettings.EdgePulseMinimumGreenColorLevel },
                { "Minimum blue color level", EdgePulseSettings.EdgePulseMinimumBlueColorLevel },
                { "Maximum red color level", EdgePulseSettings.EdgePulseMaximumRedColorLevel },
                { "Maximum green color level", EdgePulseSettings.EdgePulseMaximumGreenColorLevel },
                { "Maximum blue color level", EdgePulseSettings.EdgePulseMaximumBlueColorLevel }
            };
            ScreensaverConfig.Add("EdgePulse", EdgePulseConfig);

            // BeatEdgePulse config json object
            var BeatEdgePulseConfig = new JObject()
            {
                { "Activate 255 colors", BeatEdgePulseSettings.BeatEdgePulse255Colors },
                { "Activate true colors", BeatEdgePulseSettings.BeatEdgePulseTrueColor },
                { "Delay in Beats Per Minute", BeatEdgePulseSettings.BeatEdgePulseDelay },
                { "Cycle colors", BeatEdgePulseSettings.BeatEdgePulseCycleColors },
                { "Beat color", BeatEdgePulseSettings.BeatEdgePulseBeatColor },
                { "Max Fade Steps", BeatEdgePulseSettings.BeatEdgePulseMaxSteps },
                { "Minimum red color level", BeatEdgePulseSettings.BeatEdgePulseMinimumRedColorLevel },
                { "Minimum green color level", BeatEdgePulseSettings.BeatEdgePulseMinimumGreenColorLevel },
                { "Minimum blue color level", BeatEdgePulseSettings.BeatEdgePulseMinimumBlueColorLevel },
                { "Minimum color level", BeatEdgePulseSettings.BeatEdgePulseMinimumColorLevel },
                { "Maximum red color level", BeatEdgePulseSettings.BeatEdgePulseMaximumRedColorLevel },
                { "Maximum green color level", BeatEdgePulseSettings.BeatEdgePulseMaximumGreenColorLevel },
                { "Maximum blue color level", BeatEdgePulseSettings.BeatEdgePulseMaximumBlueColorLevel },
                { "Maximum color level", BeatEdgePulseSettings.BeatEdgePulseMaximumColorLevel }
            };
            ScreensaverConfig.Add("BeatEdgePulse", BeatEdgePulseConfig);

            // GradientRot config json object
            var GradientRotConfig = new JObject()
            {
                { "Delay in Milliseconds", GradientRotSettings.GradientRotDelay },
                { "Next gradient rot interval", GradientRotSettings.GradientRotNextRampDelay },
                { "Minimum red color level for start color", GradientRotSettings.GradientRotMinimumRedColorLevelStart },
                { "Minimum green color level for start color", GradientRotSettings.GradientRotMinimumGreenColorLevelStart },
                { "Minimum blue color level for start color", GradientRotSettings.GradientRotMinimumBlueColorLevelStart },
                { "Maximum red color level for start color", GradientRotSettings.GradientRotMaximumRedColorLevelStart },
                { "Maximum green color level for start color", GradientRotSettings.GradientRotMaximumGreenColorLevelStart },
                { "Maximum blue color level for start color", GradientRotSettings.GradientRotMaximumBlueColorLevelStart },
                { "Minimum red color level for end color", GradientRotSettings.GradientRotMinimumRedColorLevelEnd },
                { "Minimum green color level for end color", GradientRotSettings.GradientRotMinimumGreenColorLevelEnd },
                { "Minimum blue color level for end color", GradientRotSettings.GradientRotMinimumBlueColorLevelEnd },
                { "Maximum red color level for end color", GradientRotSettings.GradientRotMaximumRedColorLevelEnd },
                { "Maximum green color level for end color", GradientRotSettings.GradientRotMaximumGreenColorLevelEnd },
                { "Maximum blue color level for end color", GradientRotSettings.GradientRotMaximumBlueColorLevelEnd }
            };
            ScreensaverConfig.Add("GradientRot", GradientRotConfig);

            // Gradient config json object
            var GradientConfig = new JObject()
            {
                { "Next gradient rot interval", GradientSettings.GradientNextRampDelay },
                { "Minimum red color level for start color", GradientSettings.GradientMinimumRedColorLevelStart },
                { "Minimum green color level for start color", GradientSettings.GradientMinimumGreenColorLevelStart },
                { "Minimum blue color level for start color", GradientSettings.GradientMinimumBlueColorLevelStart },
                { "Maximum red color level for start color", GradientSettings.GradientMaximumRedColorLevelStart },
                { "Maximum green color level for start color", GradientSettings.GradientMaximumGreenColorLevelStart },
                { "Maximum blue color level for start color", GradientSettings.GradientMaximumBlueColorLevelStart },
                { "Minimum red color level for end color", GradientSettings.GradientMinimumRedColorLevelEnd },
                { "Minimum green color level for end color", GradientSettings.GradientMinimumGreenColorLevelEnd },
                { "Minimum blue color level for end color", GradientSettings.GradientMinimumBlueColorLevelEnd },
                { "Maximum red color level for end color", GradientSettings.GradientMaximumRedColorLevelEnd },
                { "Maximum green color level for end color", GradientSettings.GradientMaximumGreenColorLevelEnd },
                { "Maximum blue color level for end color", GradientSettings.GradientMaximumBlueColorLevelEnd }
            };
            ScreensaverConfig.Add("Gradient", GradientConfig);

            // Lightspeed config json object
            var LightspeedConfig = new JObject()
            {
                { "Cycle colors", LightspeedSettings.LightspeedCycleColors },
                { "Minimum red color level", LightspeedSettings.LightspeedMinimumRedColorLevel },
                { "Minimum green color level", LightspeedSettings.LightspeedMinimumGreenColorLevel },
                { "Minimum blue color level", LightspeedSettings.LightspeedMinimumBlueColorLevel },
                { "Minimum color level", LightspeedSettings.LightspeedMinimumColorLevel },
                { "Maximum red color level", LightspeedSettings.LightspeedMaximumRedColorLevel },
                { "Maximum green color level", LightspeedSettings.LightspeedMaximumGreenColorLevel },
                { "Maximum blue color level", LightspeedSettings.LightspeedMaximumBlueColorLevel },
                { "Maximum color level", LightspeedSettings.LightspeedMaximumColorLevel }
            };
            ScreensaverConfig.Add("Lightspeed", LightspeedConfig);

            // Starfield config json object
            var StarfieldConfig = new JObject()
            {
                { "Delay in Milliseconds", StarfieldSettings.StarfieldDelay }
            };
            ScreensaverConfig.Add("Starfield", StarfieldConfig);

            // Siren config json object
            var SirenConfig = new JObject()
            {
                { "Delay in Milliseconds", SirenSettings.SirenDelay },
                { "Siren style", SirenSettings.SirenStyle }
            };
            ScreensaverConfig.Add("Siren", SirenConfig);

            // Spin config json object
            var SpinConfig = new JObject()
            {
                { "Delay in Milliseconds", SpinSettings.SpinDelay }
            };
            ScreensaverConfig.Add("Spin", SpinConfig);

            // Add a screensaver config json object to Screensaver section
            ConfigurationObject.Add("Screensaver", ScreensaverConfig);

            // The Splash Section
            var SplashConfig = new JObject();

            // Simple config json object
            var SplashSimpleConfig = new JObject()
            {
                { "Progress text location", SplashSettings.SimpleProgressTextLocation.ToString() }
            };
            SplashConfig.Add("Simple", SplashSimpleConfig);

            // Progress config json object
            var SplashProgressConfig = new JObject()
            {
                { "Progress bar color", SplashSettings.ProgressProgressColor },
                { "Progress text location", SplashSettings.ProgressProgressTextLocation.ToString() }
            };
            SplashConfig.Add("Progress", SplashProgressConfig);

            // PowerLineProgress config json object
            var SplashPowerLineProgressConfig = new JObject()
            {
                { "Progress bar color", SplashSettings.PowerLineProgressProgressColor },
                { "Progress text location", SplashSettings.PowerLineProgressProgressTextLocation.ToString() }
            };
            SplashConfig.Add("PowerLineProgress", SplashPowerLineProgressConfig);

            // Add a splash config json object to Splash section
            ConfigurationObject.Add("Splash", SplashConfig);

            // Misc Section
            var MiscConfig = new JObject()
            {
                { "Show Time/Date on Upper Right Corner", Flags.CornerTimeDate },
                { "Marquee on startup", Flags.StartScroll },
                { "Long Time and Date", Flags.LongTimeDate },
                { "Preferred Unit for Temperature", Misc.Forecast.Forecast.PreferredUnit.ToString() },
                { "Enable text editor autosave", TextEditShellCommon.TextEdit_AutoSaveFlag },
                { "Text editor autosave interval", TextEditShellCommon.TextEdit_AutoSaveInterval },
                { "Enable hex editor autosave", HexEditShellCommon.HexEdit_AutoSaveFlag },
                { "Hex editor autosave interval", HexEditShellCommon.HexEdit_AutoSaveInterval },
                { "Wrap list outputs", Flags.WrapListOutputs },
                { "Draw notification border", Flags.DrawBorderNotification },
                { "Blacklisted mods", ModManager.BlacklistedModsString },
                { "Solver minimum number", Solver.SolverMinimumNumber },
                { "Solver maximum number", Solver.SolverMaximumNumber },
                { "Solver show input", Solver.SolverShowInput },
                { "Upper left corner character for notification border", Notifications.NotifyUpperLeftCornerChar },
                { "Upper right corner character for notification border", Notifications.NotifyUpperRightCornerChar },
                { "Lower left corner character for notification border", Notifications.NotifyLowerLeftCornerChar },
                { "Lower right corner character for notification border", Notifications.NotifyLowerRightCornerChar },
                { "Upper frame character for notification border", Notifications.NotifyUpperFrameChar },
                { "Lower frame character for notification border", Notifications.NotifyLowerFrameChar },
                { "Left frame character for notification border", Notifications.NotifyLeftFrameChar },
                { "Right frame character for notification border", Notifications.NotifyRightFrameChar },
                { "Manual page information style", PageViewer.ManpageInfoStyle },
                { "Default difficulty for SpeedPress", SpeedPress.SpeedPressCurrentDifficulty.ToString() },
                { "Keypress timeout for SpeedPress", SpeedPress.SpeedPressTimeout },
                { "Show latest RSS headline on login", RSSTools.ShowHeadlineOnLogin },
                { "RSS headline URL", RSSTools.RssHeadlineUrl },
                { "Save all events and/or reminders destructively", Flags.SaveEventsRemindersDestructively },
                { "Upper left corner character for RGB color wheel", ColorWheelOpen.WheelUpperLeftCornerChar },
                { "Upper right corner character for RGB color wheel", ColorWheelOpen.WheelUpperRightCornerChar },
                { "Lower left corner character for RGB color wheel", ColorWheelOpen.WheelLowerLeftCornerChar },
                { "Lower right corner character for RGB color wheel", ColorWheelOpen.WheelLowerRightCornerChar },
                { "Upper frame character for RGB color wheel", ColorWheelOpen.WheelUpperFrameChar },
                { "Lower frame character for RGB color wheel", ColorWheelOpen.WheelLowerFrameChar },
                { "Left frame character for RGB color wheel", ColorWheelOpen.WheelLeftFrameChar },
                { "Right frame character for RGB color wheel", ColorWheelOpen.WheelRightFrameChar },
                { "Default JSON formatting for JSON shell", JsonShellCommon.JsonShell_Formatting.ToString() },
                { "Enable Figlet for timer", Flags.EnableFigletTimer },
                { "Figlet font for timer", TimerScreen.TimerFigletFont },
                { "Show the commands count on help", Flags.ShowCommandsCount },
                { "Show the shell commands count on help", Flags.ShowShellCommandsCount },
                { "Show the mod commands count on help", Flags.ShowModCommandsCount },
                { "Show the aliases count on help", Flags.ShowShellAliasesCount },
                { "Password mask character", Input.CurrentMask },
                { "Upper left corner character for progress bars", ProgressTools.ProgressUpperLeftCornerChar },
                { "Upper right corner character for progress bars", ProgressTools.ProgressUpperRightCornerChar },
                { "Lower left corner character for progress bars", ProgressTools.ProgressLowerLeftCornerChar },
                { "Lower right corner character for progress bars", ProgressTools.ProgressLowerRightCornerChar },
                { "Upper frame character for progress bars", ProgressTools.ProgressUpperFrameChar },
                { "Lower frame character for progress bars", ProgressTools.ProgressLowerFrameChar },
                { "Left frame character for progress bars", ProgressTools.ProgressLeftFrameChar },
                { "Right frame character for progress bars", ProgressTools.ProgressRightFrameChar },
                { "Users count for love or hate comments", LoveHateRespond.LoveOrHateUsersCount },
                { "Input history enabled", Flags.InputHistoryEnabled },
                { "Input clipboard enabled", Flags.InputClipboardEnabled },
                { "Input undo enabled", Flags.InputUndoEnabled },
                { "Use PowerLine for rendering spaceship", MeteorShooter.MeteorUsePowerLine },
                { "Meteor game speed", MeteorShooter.MeteorSpeed }
            };
            ConfigurationObject.Add("Misc", MiscConfig);
            return ConfigurationObject;
        }

        /// <summary>
        /// Creates the kernel configuration file
        /// </summary>
        /// <exception cref="Exceptions.ConfigException"></exception>
        public static void CreateConfig() => CreateConfig(Paths.GetKernelPath(KernelPathType.Configuration));

        /// <summary>
        /// Creates the kernel configuration file with custom path
        /// </summary>
        /// <exception cref="Exceptions.ConfigException"></exception>
        public static void CreateConfig(string ConfigPath)
        {
            if (Flags.SafeMode)
                return;

            Filesystem.ThrowOnInvalidPath(ConfigPath);
            var ConfigurationObject = GetNewConfigObject();

            // Save Config
            File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(ConfigurationObject, Formatting.Indented));
            Kernel.KernelEventManager.RaiseConfigSaved();
        }

        /// <summary>
        /// Creates the kernel configuration file
        /// </summary>
        /// <returns>True if successful; False if unsuccessful.</returns>
        /// <exception cref="Exceptions.ConfigException"></exception>
        public static bool TryCreateConfig() => TryCreateConfig(Paths.GetKernelPath(KernelPathType.Configuration));

        /// <summary>
        /// Creates the kernel configuration file with custom path
        /// </summary>
        /// <returns>True if successful; False if unsuccessful.</returns>
        /// <exception cref="Exceptions.ConfigException"></exception>
        public static bool TryCreateConfig(string ConfigPath) => TryCreateConfig(JObject.Parse(File.ReadAllText(ConfigPath)));

        /// <summary>
        /// Creates the kernel configuration file with custom path
        /// </summary>
        /// <returns>True if successful; False if unsuccessful.</returns>
        /// <exception cref="Exceptions.ConfigException"></exception>
        public static bool TryCreateConfig(JToken ConfigToken)
        {
            try
            {
                CreateConfig((string)ConfigToken);
                return true;
            }
            catch (Exception ex)
            {
                Kernel.KernelEventManager.RaiseConfigSaveError(ex);
                DebugWriter.WriteDebugStackTrace(ex);
                return false;
            }
        }

        /// <summary>
        /// Configures the kernel according to the kernel failsafe configuration
        /// </summary>
        /// <exception cref="Exceptions.ConfigException"></exception>
        public static void ReadFailsafeConfig()
        {
            if (Flags.OptInToNewConfigReader)
                ReadConfigNew(PristineConfigToken, true);
            else
                ReadConfig(PristineConfigToken, true);
        }

        /// <summary>
        /// Configures the kernel according to the kernel configuration file
        /// </summary>
        /// <exception cref="Exceptions.ConfigException"></exception>
        public static void ReadConfig()
        {
            if (Flags.OptInToNewConfigReader)
                ReadConfigNew(Paths.GetKernelPath(KernelPathType.Configuration));
            else
                ReadConfig(Paths.GetKernelPath(KernelPathType.Configuration));
        }

        /// <summary>
        /// Configures the kernel according to the custom kernel configuration file
        /// </summary>
        public static void ReadConfig(string ConfigPath)
        {
            Filesystem.ThrowOnInvalidPath(ConfigPath);
            if (Flags.OptInToNewConfigReader)
                ReadConfigNew(JObject.Parse(File.ReadAllText(ConfigPath)));
            else
                ReadConfig(JObject.Parse(File.ReadAllText(ConfigPath)));
        }

        /// <summary>
        /// Configures the kernel according to the custom kernel configuration file (new)
        /// </summary>
        /// <exception cref="Exceptions.ConfigException"></exception>
        public static void ReadConfigNew(JToken ConfigToken, bool Force = false)
        {
            if (Flags.SafeMode & !Force)
                return;

            // Load config token
            Config.ConfigToken = (JObject)ConfigToken;
            DebugWriter.WriteDebug(DebugLevel.I, "Config loaded with {0} sections", ConfigToken.Count());

            // Parse config metadata
            JToken ConfigMetadata = JToken.Parse(Properties.Resources.Resources.SettingsEntries);
            JToken ScreensaverConfigMetadata = JToken.Parse(Properties.Resources.Resources.ScreensaverSettingsEntries);
            JToken SplashConfigMetadata = JToken.Parse(Properties.Resources.Resources.SplashSettingsEntries);
            JToken[] Metadatas = new[] { ConfigMetadata, ScreensaverConfigMetadata, SplashConfigMetadata };

            // Load configuration values
            foreach (JToken metadata in Metadatas)
            {
                // Get the max sections
                int MaxSections = metadata.Count();
                for (int SectionIndex = 0; SectionIndex <= MaxSections - 1; SectionIndex++)
                {
                    // Get the section property and fetch metadata information from section
                    JProperty Section = (JProperty)metadata.ToList()[SectionIndex];
                    var SectionTokenGeneral = metadata[Section.Name];
                    var SectionToken = SectionTokenGeneral["Keys"];

                    // Get config token from path
                    var SectionTokenPath = SectionTokenGeneral["Path"];
                    var ConfigTokenFromPath = ConfigToken.SelectToken((string)SectionTokenPath);

                    // Count the options
                    int MaxOptions = SectionToken.Count();
                    for (int OptionIndex = 0; OptionIndex <= MaxOptions - 1; OptionIndex++)
                    {
                        // Get the setting token and fetch information
                        var Setting = SectionToken[OptionIndex];
                        string VariableKeyName = (string)Setting["Name"];
                        string Variable = (string)Setting["Variable"];

                        // Get variable value and type
                        SettingsKeyType VariableType = (SettingsKeyType)Convert.ToInt32(Enum.Parse(typeof(SettingsKeyType), (string)Setting["Type"]));
                        object VariableValue;
                        if (VariableType == SettingsKeyType.SColor)
                        {
                            VariableValue = new Color(((string)ConfigTokenFromPath[VariableKeyName]).ReleaseDoubleQuotes());

                            // Setting entry is color, but the variable could be either String or Color.
                            if ((FieldManager.CheckField(Variable) && FieldManager.GetField(Variable).FieldType == typeof(string)) ||
                                (PropertyManager.CheckProperty(Variable) && PropertyManager.GetProperty(Variable).PropertyType == typeof(string)))
                            {
                                // We're dealing with the field or the property which takes color but is a string containing plain sequence
                                VariableValue = ((Color)VariableValue).PlainSequence;
                            }
                        }
                        else if (VariableType == SettingsKeyType.SSelection)
                        {
                            bool SelectionEnum = (bool)(Setting["IsEnumeration"] ?? false);
                            string SelectionEnumAssembly = (string)Setting["EnumerationAssembly"];
                            bool SelectionEnumInternal = (bool)(Setting["EnumerationInternal"] ?? false);
                            if (SelectionEnum)
                            {
                                if (SelectionEnumInternal)
                                {
                                    // Apparently, we need to have a full assembly name for getting types.
                                    Type enumType = Type.GetType("KS." + Setting["Enumeration"].ToString() + ", " + Assembly.GetExecutingAssembly().FullName);
                                    VariableValue = Enum.Parse(enumType, ((string)ConfigTokenFromPath[VariableKeyName]).ReleaseDoubleQuotes());
                                }
                                else
                                {
                                    Type enumType = Type.GetType(Setting["Enumeration"].ToString() + ", " + SelectionEnumAssembly);
                                    VariableValue = Enum.Parse(enumType, ((string)ConfigTokenFromPath[VariableKeyName]).ReleaseDoubleQuotes());
                                }
                            }
                            else
                            {
                                VariableValue = ConfigTokenFromPath[VariableKeyName].ToObject<dynamic>();
                            }
                        }
                        else
                            VariableValue = ConfigTokenFromPath[VariableKeyName].ToObject<dynamic>();

                        // Check to see if the value is numeric
                        if (VariableValue is int or long)
                        {
                            if (Convert.ToInt64(VariableValue) <= int.MaxValue)
                                VariableValue = int.Parse(Convert.ToString(VariableValue));
                            else if (Convert.ToInt64(VariableValue) <= long.MaxValue)
                                VariableValue = long.Parse(Convert.ToString(VariableValue));
                        }

                        // Now, set the value
                        if (FieldManager.CheckField(Variable))
                        {
                            // We're dealing with the field
                            FieldManager.SetValue(Variable, VariableValue, true);
                        }
                        else if (PropertyManager.CheckProperty(Variable))
                        {
                            // We're dealing with the property
                            PropertyManager.SetPropertyValue(Variable, VariableValue);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Configures the kernel according to the custom kernel configuration file
        /// </summary>
        /// <exception cref="Exceptions.ConfigException"></exception>
        public static void ReadConfig(JToken ConfigToken, bool Force = false)
        {
            if (Flags.SafeMode & !Force)
                return;

            // Parse configuration.
            // NOTE: Question marks between parentheses are for nullable types.
            Config.ConfigToken = (JObject)ConfigToken;
            DebugWriter.WriteDebug(DebugLevel.I, "Config loaded with {0} sections", ConfigToken.Count());

            // ----------------------------- Important configuration -----------------------------
            // Set background color flag
            Flags.SetBackground = (bool)ConfigToken["General"]["Set console background color"];

            // Language
            Flags.LangChangeCulture = (bool)ConfigToken["General"]["Change Culture when Switching Languages"];
            if (Flags.LangChangeCulture)
            {
                CultureManager.CurrentCult = new CultureInfo((ConfigToken["General"]["Culture"] != null) ? ConfigToken["General"]["Culture"].ToString() : "en-US");
            }
            LanguageManager.SetLang((string)ConfigToken["General"]["Language"] ?? "eng");

            // Colored Shell
            bool UncoloredDetected = ConfigToken["Shell"]["Colored Shell"] != null && !ConfigToken["Shell"]["Colored Shell"].ToObject<bool>();
            if (UncoloredDetected)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Detected uncolored shell. Removing colors...");
                ThemeTools.ApplyThemeFromResources("LinuxUncolored");
                Shell.Shell.ColoredShell = false;
            }

            // ----------------------------- General configuration -----------------------------
            // Colors Section
            DebugWriter.WriteDebug(DebugLevel.I, "Loading colors...");
            if (Shell.Shell.ColoredShell)
            {
                // We use New Color() to parse entered color. This is to ensure that the kernel can use the correct VT sequence.
                ColorTools.UserNameShellColor = new Color(ConfigToken["Colors"]["User Name Shell Color"].ToString());
                ColorTools.HostNameShellColor = new Color(ConfigToken["Colors"]["Host Name Shell Color"].ToString());
                ColorTools.ContKernelErrorColor = new Color(ConfigToken["Colors"]["Continuable Kernel Error Color"].ToString());
                ColorTools.UncontKernelErrorColor = new Color(ConfigToken["Colors"]["Uncontinuable Kernel Error Color"].ToString());
                ColorTools.NeutralTextColor = new Color(ConfigToken["Colors"]["Text Color"].ToString());
                ColorTools.LicenseColor = new Color(ConfigToken["Colors"]["License Color"].ToString());
                ColorTools.BackgroundColor = new Color(ConfigToken["Colors"]["Background Color"].ToString());
                ColorTools.InputColor = new Color(ConfigToken["Colors"]["Input Color"].ToString());
                ColorTools.ListEntryColor = new Color(ConfigToken["Colors"]["List Entry Color"].ToString());
                ColorTools.ListValueColor = new Color(ConfigToken["Colors"]["List Value Color"].ToString());
                ColorTools.StageColor = new Color(ConfigToken["Colors"]["Kernel Stage Color"].ToString());
                ColorTools.ErrorColor = new Color(ConfigToken["Colors"]["Error Text Color"].ToString());
                ColorTools.WarningColor = new Color(ConfigToken["Colors"]["Warning Text Color"].ToString());
                ColorTools.OptionColor = new Color(ConfigToken["Colors"]["Option Color"].ToString());
                ColorTools.BannerColor = new Color(ConfigToken["Colors"]["Banner Color"].ToString());
                ColorTools.NotificationTitleColor = new Color(ConfigToken["Colors"]["Notification Title Color"].ToString());
                ColorTools.NotificationDescriptionColor = new Color(ConfigToken["Colors"]["Notification Description Color"].ToString());
                ColorTools.NotificationProgressColor = new Color(ConfigToken["Colors"]["Notification Progress Color"].ToString());
                ColorTools.NotificationFailureColor = new Color(ConfigToken["Colors"]["Notification Failure Color"].ToString());
                ColorTools.QuestionColor = new Color(ConfigToken["Colors"]["Question Color"].ToString());
                ColorTools.SuccessColor = new Color(ConfigToken["Colors"]["Success Color"].ToString());
                ColorTools.UserDollarColor = new Color(ConfigToken["Colors"]["User Dollar Color"].ToString());
                ColorTools.TipColor = new Color(ConfigToken["Colors"]["Tip Color"].ToString());
                ColorTools.SeparatorTextColor = new Color(ConfigToken["Colors"]["Separator Text Color"].ToString());
                ColorTools.SeparatorColor = new Color(ConfigToken["Colors"]["Separator Color"].ToString());
                ColorTools.ListTitleColor = new Color(ConfigToken["Colors"]["List Title Color"].ToString());
                ColorTools.DevelopmentWarningColor = new Color(ConfigToken["Colors"]["Development Warning Color"].ToString());
                ColorTools.StageTimeColor = new Color(ConfigToken["Colors"]["Stage Time Color"].ToString());
                ColorTools.ProgressColor = new Color(ConfigToken["Colors"]["Progress Color"].ToString());
                ColorTools.BackOptionColor = new Color(ConfigToken["Colors"]["Back Option Color"].ToString());
                ColorTools.LowPriorityBorderColor = new Color(ConfigToken["Colors"]["Low Priority Border Color"].ToString());
                ColorTools.MediumPriorityBorderColor = new Color(ConfigToken["Colors"]["Medium Priority Border Color"].ToString());
                ColorTools.HighPriorityBorderColor = new Color(ConfigToken["Colors"]["High Priority Border Color"].ToString());
                ColorTools.TableSeparatorColor = new Color(ConfigToken["Colors"]["Table Separator Color"].ToString());
                ColorTools.TableHeaderColor = new Color(ConfigToken["Colors"]["Table Header Color"].ToString());
                ColorTools.TableValueColor = new Color(ConfigToken["Colors"]["Table Value Color"].ToString());
                ColorTools.SelectedOptionColor = new Color(ConfigToken["Colors"]["Selected Option Color"].ToString());
                ColorTools.AlternativeOptionColor = new Color(ConfigToken["Colors"]["Alternative Option Color"].ToString());
            }

            // General Section
            DebugWriter.WriteDebug(DebugLevel.I, "Parsing general section...");
            Flags.Maintenance = (bool)ConfigToken["General"]["Maintenance Mode"];
            Flags.ArgsOnBoot = (bool)ConfigToken["General"]["Prompt for Arguments on Boot"];
            Flags.CheckUpdateStart = (bool)ConfigToken["General"]["Check for Updates on Startup"];
            if (!string.IsNullOrWhiteSpace((string)ConfigToken["General"]["Custom Startup Banner"]))
                WelcomeMessage.CustomBanner = (string)ConfigToken["General"]["Custom Startup Banner"];
            Flags.ShowAppInfoOnBoot = (bool)ConfigToken["General"]["Show app information during boot"];
            Flags.ParseCommandLineArguments = (bool)ConfigToken["General"]["Parse command-line arguments"];
            Flags.ShowStageFinishTimes = (bool)ConfigToken["General"]["Show stage finish times"];
            Flags.StartKernelMods = (bool)ConfigToken["General"]["Start kernel modifications on boot"];
            Flags.ShowCurrentTimeBeforeLogin = (bool)ConfigToken["General"]["Show current time before login"];
            Flags.NotifyFaultsBoot = (bool)ConfigToken["General"]["Notify for any fault during boot"];
            Flags.ShowStackTraceOnKernelError = (bool)ConfigToken["General"]["Show stack trace on kernel error"];
            Flags.CheckDebugQuota = (bool)ConfigToken["General"]["Check debug quota"];
            Flags.AutoDownloadUpdate = (bool)ConfigToken["General"]["Automatically download updates"];
            Flags.EventDebug = (bool)ConfigToken["General"]["Enable event debugging"];
            Flags.NewWelcomeStyle = (bool)ConfigToken["General"]["New welcome banner"];
            Flags.EnableSplash = (bool)ConfigToken["General"]["Stylish splash screen"];
            SplashManager.SplashName = (string)ConfigToken["General"]["Splash name"] ?? "Simple";
            KernelTools.BannerFigletFont = (string)ConfigToken["General"]["Banner figlet font"] ?? "Banner";
            Flags.SimulateNoAPM = (bool)ConfigToken["General"]["Simulate No APM Mode"];

            // Login Section
            DebugWriter.WriteDebug(DebugLevel.I, "Parsing login section...");
            Flags.ClearOnLogin = (bool)ConfigToken["Login"]["Clear Screen on Log-in"];
            Flags.ShowMOTD = (bool)ConfigToken["Login"]["Show MOTD on Log-in"];
            Flags.ShowAvailableUsers = (bool)ConfigToken["Login"]["Show available usernames"];
            if (!string.IsNullOrWhiteSpace((string)ConfigToken["Login"]["Host Name"]))
                NetworkTools.HostName = (string)ConfigToken["Login"]["Host Name"];
            if (!string.IsNullOrWhiteSpace((string)ConfigToken["Login"]["MOTD Path"]) & Parsing.TryParsePath((string)ConfigToken["Login"]["MOTD Path"]))
                MotdParse.MotdFilePath = (string)ConfigToken["Login"]["MOTD Path"];
            if (!string.IsNullOrWhiteSpace((string)ConfigToken["Login"]["MAL Path"]) & Parsing.TryParsePath((string)ConfigToken["Login"]["MAL Path"]))
                MalParse.MalFilePath = (string)ConfigToken["Login"]["MAL Path"];
            Login.Login.UsernamePrompt = (string)ConfigToken["Login"]["Username prompt style"] ?? "";
            Login.Login.PasswordPrompt = (string)ConfigToken["Login"]["Password prompt style"] ?? "";
            Flags.ShowMAL = (bool)ConfigToken["Login"]["Show MAL on Log-in"];
            UserManagement.IncludeAnonymous = (bool)ConfigToken["Login"]["Include anonymous users"];
            UserManagement.IncludeDisabled = (bool)ConfigToken["Login"]["Include disabled users"];

            // Shell Section
            DebugWriter.WriteDebug(DebugLevel.I, "Parsing shell section...");
            Flags.SimHelp = (bool)ConfigToken["Shell"]["Simplified Help Command"];
            CurrentDirectory.CurrentDir = (string)ConfigToken["Shell"]["Current Directory"] ?? Paths.HomePath;
            Shell.Shell.PathsToLookup = !string.IsNullOrEmpty((string)ConfigToken["Shell"]["Lookup Directories"]) ? ConfigToken["Shell"]["Lookup Directories"].ToString().ReleaseDoubleQuotes() : Environment.GetEnvironmentVariable("PATH");
            PromptPresetManager.SetPreset((string)ConfigToken["Shell"]["Prompt Preset"] ?? "Default", ShellType.Shell, false);
            PromptPresetManager.SetPreset((string)ConfigToken["Shell"]["FTP Prompt Preset"] ?? "Default", ShellType.FTPShell, false);
            PromptPresetManager.SetPreset((string)ConfigToken["Shell"]["Mail Prompt Preset"] ?? "Default", ShellType.MailShell, false);
            PromptPresetManager.SetPreset((string)ConfigToken["Shell"]["SFTP Prompt Preset"] ?? "Default", ShellType.SFTPShell, false);
            PromptPresetManager.SetPreset((string)ConfigToken["Shell"]["RSS Prompt Preset"] ?? "Default", ShellType.RSSShell, false);
            PromptPresetManager.SetPreset((string)ConfigToken["Shell"]["Text Edit Prompt Preset"] ?? "Default", ShellType.TextShell, false);
            PromptPresetManager.SetPreset((string)ConfigToken["Shell"]["Test Shell Prompt Preset"] ?? "Default", ShellType.TestShell, false);
            PromptPresetManager.SetPreset((string)ConfigToken["Shell"]["JSON Shell Prompt Preset"] ?? "Default", ShellType.JsonShell, false);
            PromptPresetManager.SetPreset((string)ConfigToken["Shell"]["Hex Edit Prompt Preset"] ?? "Default", ShellType.HexShell, false);
            PromptPresetManager.SetPreset((string)ConfigToken["Shell"]["HTTP Shell Prompt Preset"] ?? "Default", ShellType.HTTPShell, false);
            PromptPresetManager.SetPreset((string)ConfigToken["Shell"]["Archive Shell Prompt Preset"] ?? "Default", ShellType.ArchiveShell, false);
            Flags.ColorWheelTrueColor = (bool)ConfigToken["Shell"]["Start color wheel in true color mode"];
            ChoiceStyle.DefaultChoiceOutputType = (ConfigToken["Shell"]["Default choice output type"] != null) ? (Enum.TryParse((string)ConfigToken["Shell"]["Default choice output type"], out ChoiceStyle.DefaultChoiceOutputType) ? ChoiceStyle.DefaultChoiceOutputType : ChoiceStyle.ChoiceOutputType.Modern) : ChoiceStyle.ChoiceOutputType.Modern;

            // Filesystem Section
            DebugWriter.WriteDebug(DebugLevel.I, "Parsing filesystem section...");
            DebugManager.DebugQuota = (double)(int.TryParse((string)ConfigToken["Filesystem"]["Debug Size Quota in Bytes"], out _) ? (int)ConfigToken["Filesystem"]["Debug Size Quota in Bytes"] : 1073741824);
            Flags.FullParseMode = (bool)ConfigToken["Filesystem"]["Size parse mode"];
            Flags.HiddenFiles = (bool)ConfigToken["Filesystem"]["Show Hidden Files"];
            Listing.SortMode = (ConfigToken["Filesystem"]["Filesystem sort mode"] != null) ? (Enum.TryParse((string)ConfigToken["Filesystem"]["Filesystem sort mode"], out Listing.SortMode) ? Listing.SortMode : FilesystemSortOptions.FullName) : FilesystemSortOptions.FullName;
            Listing.SortDirection = (ConfigToken["Filesystem"]["Filesystem sort direction"] != null) ? (Enum.TryParse((string)ConfigToken["Filesystem"]["Filesystem sort direction"], out Listing.SortDirection) ? Listing.SortDirection : FilesystemSortDirection.Ascending) : FilesystemSortDirection.Ascending;
            Filesystem.ShowFilesystemProgress = (bool)ConfigToken["Filesystem"]["Show progress on filesystem operations"];
            Listing.ShowFileDetailsList = (bool)ConfigToken["Filesystem"]["Show file details in list"];
            Flags.SuppressUnauthorizedMessages = (bool)ConfigToken["Filesystem"]["Suppress unauthorized messages"];
            Flags.PrintLineNumbers = (bool)ConfigToken["Filesystem"]["Print line numbers on printing file contents"];
            Listing.SortList = (bool)ConfigToken["Filesystem"]["Sort the list"];
            Listing.ShowTotalSizeInList = (bool)ConfigToken["Filesystem"]["Show total size in list"];

            // Hardware Section
            DebugWriter.WriteDebug(DebugLevel.I, "Parsing hardware section...");
            Flags.QuietHardwareProbe = (bool)ConfigToken["Hardware"]["Quiet Probe"];
            Flags.FullHardwareProbe = (bool)ConfigToken["Hardware"]["Full Probe"];
            Flags.VerboseHardwareProbe = (bool)ConfigToken["Hardware"]["Verbose Probe"];

            // Network Section
            DebugWriter.WriteDebug(DebugLevel.I, "Parsing network section...");
            RemoteDebugger.DebugPort = int.TryParse((string)ConfigToken["Network"]["Debug Port"], out _) ? (int)ConfigToken["Network"]["Debug Port"] : 3014;
            NetworkTools.DownloadRetries = int.TryParse((string)ConfigToken["Network"]["Download Retry Times"], out _) ? (int)ConfigToken["Network"]["Download Retry Times"] : 3;
            NetworkTools.UploadRetries = int.TryParse((string)ConfigToken["Network"]["Upload Retry Times"], out _) ? (int)ConfigToken["Network"]["Upload Retry Times"] : 3;
            Flags.ShowProgress = (bool)ConfigToken["Network"]["Show progress bar while downloading or uploading from \"get\" or \"put\" command"];
            Flags.FTPLoggerUsername = (bool)ConfigToken["Network"]["Log FTP username"];
            Flags.FTPLoggerIP = (bool)ConfigToken["Network"]["Log FTP IP address"];
            Flags.FTPFirstProfileOnly = (bool)ConfigToken["Network"]["Return only first FTP profile"];
            MailManager.ShowPreview = (bool)ConfigToken["Network"]["Show mail message preview"];
            Flags.RecordChatToDebugLog = (bool)ConfigToken["Network"]["Record chat to debug log"];
            SSH.SSHBanner = (bool)ConfigToken["Network"]["Show SSH banner"];
            RemoteProcedure.RPCEnabled = (bool)ConfigToken["Network"]["Enable RPC"];
            RemoteProcedure.RPCPort = int.TryParse((string)ConfigToken["Network"]["RPC Port"], out _) ? (int)ConfigToken["Network"]["RPC Port"] : 12345;
            FTPShellCommon.FtpShowDetailsInList = (bool)ConfigToken["Network"]["Show file details in FTP list"];
            FTPShellCommon.FtpUserPromptStyle = (string)ConfigToken["Network"]["Username prompt style for FTP"] ?? "";
            FTPShellCommon.FtpPassPromptStyle = (string)ConfigToken["Network"]["Password prompt style for FTP"] ?? "";
            FTPShellCommon.FtpUseFirstProfile = (bool)ConfigToken["Network"]["Use first FTP profile"];
            FTPShellCommon.FtpNewConnectionsToSpeedDial = (bool)ConfigToken["Network"]["Add new connections to FTP speed dial"];
            FTPShellCommon.FtpTryToValidateCertificate = (bool)ConfigToken["Network"]["Try to validate secure FTP certificates"];
            FTPShellCommon.FtpShowMotd = (bool)ConfigToken["Network"]["Show FTP MOTD on connection"];
            FTPShellCommon.FtpAlwaysAcceptInvalidCerts = (bool)ConfigToken["Network"]["Always accept invalid FTP certificates"];
            MailLogin.Mail_UserPromptStyle = (string)ConfigToken["Network"]["Username prompt style for mail"] ?? "";
            MailLogin.Mail_PassPromptStyle = (string)ConfigToken["Network"]["Password prompt style for mail"] ?? "";
            MailLogin.Mail_IMAPPromptStyle = (string)ConfigToken["Network"]["IMAP prompt style for mail"] ?? "";
            MailLogin.Mail_SMTPPromptStyle = (string)ConfigToken["Network"]["SMTP prompt style for mail"] ?? "";
            MailLogin.Mail_AutoDetectServer = (bool)ConfigToken["Network"]["Automatically detect mail server"];
            MailLogin.Mail_Debug = (bool)ConfigToken["Network"]["Enable mail debug"];
            MailShellCommon.Mail_NotifyNewMail = (bool)ConfigToken["Network"]["Notify for new mail messages"];
            MailLogin.Mail_GPGPromptStyle = (string)ConfigToken["Network"]["GPG password prompt style for mail"] ?? "";
            MailShellCommon.Mail_ImapPingInterval = int.TryParse((string)ConfigToken["Network"]["Send IMAP ping interval"], out _) ? (int)ConfigToken["Network"]["Send IMAP ping interval"] : 30000;
            MailShellCommon.Mail_SmtpPingInterval = int.TryParse((string)ConfigToken["Network"]["Send SMTP ping interval"], out _) ? (int)ConfigToken["Network"]["Send SMTP ping interval"] : 30000;
            MailShellCommon.Mail_TextFormat = (ConfigToken["Network"]["Mail text format"] != null) ? (Enum.TryParse((string)ConfigToken["Network"]["Mail text format"], out MailShellCommon.Mail_TextFormat) ? MailShellCommon.Mail_TextFormat : TextFormat.Plain) : TextFormat.Plain;
            RemoteDebugger.RDebugAutoStart = (bool)ConfigToken["Network"]["Automatically start remote debug on startup"];
            RemoteDebugger.RDebugMessageFormat = (string)ConfigToken["Network"]["Remote debug message format"] ?? "";
            RSSShellCommon.RSSFeedUrlPromptStyle = (string)ConfigToken["Network"]["RSS feed URL prompt style"] ?? "";
            RSSShellCommon.RSSRefreshFeeds = (bool)ConfigToken["Network"]["Auto refresh RSS feed"];
            RSSShellCommon.RSSRefreshInterval = int.TryParse((string)ConfigToken["Network"]["Auto refresh RSS feed interval"], out _) ? (int)ConfigToken["Network"]["Auto refresh RSS feed interval"] : 60000;
            SFTPShellCommon.SFTPShowDetailsInList = (bool)ConfigToken["Network"]["Show file details in SFTP list"];
            SFTPShellCommon.SFTPUserPromptStyle = (string)ConfigToken["Network"]["Username prompt style for SFTP"] ?? "";
            SFTPShellCommon.SFTPNewConnectionsToSpeedDial = (bool)ConfigToken["Network"]["Add new connections to SFTP speed dial"];
            NetworkTools.PingTimeout = int.TryParse((string)ConfigToken["Network"]["Ping timeout"], out _) ? (int)ConfigToken["Network"]["Ping timeout"] : 60000;
            Flags.ExtensiveAdapterInformation = (bool)ConfigToken["Network"]["Show extensive adapter info"];
            Flags.GeneralNetworkInformation = (bool)ConfigToken["Network"]["Show general network information"];
            NetworkTransfer.DownloadPercentagePrint = (string)ConfigToken["Network"]["Download percentage text"] ?? "";
            NetworkTransfer.UploadPercentagePrint = (string)ConfigToken["Network"]["Upload percentage text"] ?? "";
            FTPShellCommon.FtpRecursiveHashing = (bool)ConfigToken["Network"]["Recursive hashing for FTP"];
            MailShellCommon.Mail_MaxMessagesInPage = int.TryParse((string)ConfigToken["Network"]["Maximum number of e-mails in one page"], out _) ? (int)ConfigToken["Network"]["Maximum number of e-mails in one page"] : 10;
            MailShellCommon.Mail_ShowProgress = (bool)ConfigToken["Network"]["Show mail transfer progress"];
            MailShellCommon.Mail_ProgressStyle = (string)ConfigToken["Network"]["Mail transfer progress"] ?? "";
            MailShellCommon.Mail_ProgressStyleSingle = (string)ConfigToken["Network"]["Mail transfer progress (single)"] ?? "";
            NetworkTransfer.DownloadNotificationProvoke = (bool)ConfigToken["Network"]["Show notification for download progress"];
            NetworkTransfer.UploadNotificationProvoke = (bool)ConfigToken["Network"]["Show notification for upload progress"];
            RSSShellCommon.RSSFetchTimeout = int.TryParse((string)ConfigToken["Network"]["RSS feed fetch timeout"], out _) ? (int)ConfigToken["Network"]["RSS feed fetch timeout"] : 60000;
            FTPShellCommon.FtpVerifyRetryAttempts = int.TryParse((string)ConfigToken["Network"]["Verify retry attempts for FTP transmission"], out _) ? (int)ConfigToken["Network"]["Verify retry attempts for FTP transmission"] : 3;
            FTPShellCommon.FtpConnectTimeout = int.TryParse((string)ConfigToken["Network"]["FTP connection timeout"], out _) ? (int)ConfigToken["Network"]["FTP connection timeout"] : 15000;
            FTPShellCommon.FtpDataConnectTimeout = int.TryParse((string)ConfigToken["Network"]["FTP data connection timeout"], out _) ? (int)ConfigToken["Network"]["FTP data connection timeout"] : 15000;
            FTPShellCommon.FtpProtocolVersions = (ConfigToken["Network"]["FTP IP versions"] != null) ? (Enum.TryParse((string)ConfigToken["Network"]["FTP IP versions"], out FTPShellCommon.FtpProtocolVersions) ? FTPShellCommon.FtpProtocolVersions : FtpIpVersion.ANY) : FtpIpVersion.ANY;
            Flags.NotifyOnRemoteDebugConnectionError = (bool)ConfigToken["Network"]["Notify on remote debug connection error"];

            // Screensaver Section
            Screensaver.DefSaverName = (string)ConfigToken["Screensaver"]["Screensaver"] ?? "matrix";
            Screensaver.ScrnTimeout = int.TryParse((string)ConfigToken["Screensaver"]["Screensaver Timeout in ms"], out _) ? (int)ConfigToken["Screensaver"]["Screensaver Timeout in ms"] : 300000;
            Screensaver.ScreensaverDebug = (bool)ConfigToken["Screensaver"]["Enable screensaver debugging"];
            Screensaver.PasswordLock = (bool)ConfigToken["Screensaver"]["Ask for password after locking"];

            // Screensaver-specific settings go below:
            // > ColorMix
            ColorMixSettings.ColorMix255Colors = (bool)ConfigToken["Screensaver"]["ColorMix"]["Activate 255 colors"];
            ColorMixSettings.ColorMixTrueColor = (bool)ConfigToken["Screensaver"]["ColorMix"]["Activate true colors"];
            ColorMixSettings.ColorMixDelay = int.TryParse((string)ConfigToken["Screensaver"]["ColorMix"]["Delay in Milliseconds"], out _) ? (int)ConfigToken["Screensaver"]["ColorMix"]["Delay in Milliseconds"] : 1;
            ColorMixSettings.ColorMixBackgroundColor = new Color(ConfigToken["Screensaver"]["ColorMix"]["Background color"].ToString()).PlainSequence;
            ColorMixSettings.ColorMixMinimumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["ColorMix"]["Minimum red color level"], out _) ? (int)ConfigToken["Screensaver"]["ColorMix"]["Minimum red color level"] : 0;
            ColorMixSettings.ColorMixMinimumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["ColorMix"]["Minimum green color level"], out _) ? (int)ConfigToken["Screensaver"]["ColorMix"]["Minimum green color level"] : 0;
            ColorMixSettings.ColorMixMinimumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["ColorMix"]["Minimum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["ColorMix"]["Minimum blue color level"] : 0;
            ColorMixSettings.ColorMixMinimumColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["ColorMix"]["Minimum color level"], out _) ? (int)ConfigToken["Screensaver"]["ColorMix"]["Minimum color level"] : 0;
            ColorMixSettings.ColorMixMaximumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["ColorMix"]["Maximum red color level"], out _) ? (int)ConfigToken["Screensaver"]["ColorMix"]["Maximum red color level"] : 255;
            ColorMixSettings.ColorMixMaximumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["ColorMix"]["Maximum green color level"], out _) ? (int)ConfigToken["Screensaver"]["ColorMix"]["Maximum green color level"] : 255;
            ColorMixSettings.ColorMixMaximumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["ColorMix"]["Maximum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["ColorMix"]["Maximum blue color level"] : 255;
            ColorMixSettings.ColorMixMaximumColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["ColorMix"]["Maximum color level"], out _) ? (int)ConfigToken["Screensaver"]["ColorMix"]["Maximum color level"] : 255;

            // > Disco
            DiscoSettings.Disco255Colors = (bool)ConfigToken["Screensaver"]["Disco"]["Activate 255 colors"];
            DiscoSettings.DiscoTrueColor = (bool)ConfigToken["Screensaver"]["Disco"]["Activate true colors"];
            DiscoSettings.DiscoCycleColors = (bool)ConfigToken["Screensaver"]["Disco"]["Cycle colors"];
            DiscoSettings.DiscoDelay = int.TryParse((string)ConfigToken["Screensaver"]["Disco"]["Delay in Milliseconds"], out _) ? (int)ConfigToken["Screensaver"]["Disco"]["Delay in Milliseconds"] : 100;
            DiscoSettings.DiscoUseBeatsPerMinute = (bool)ConfigToken["Screensaver"]["Disco"]["Use Beats Per Minute"];
            DiscoSettings.DiscoEnableFedMode = (bool)ConfigToken["Screensaver"]["Disco"]["Enable Black and White Mode"];
            DiscoSettings.DiscoMinimumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Disco"]["Minimum red color level"], out _) ? (int)ConfigToken["Screensaver"]["Disco"]["Minimum red color level"] : 0;
            DiscoSettings.DiscoMinimumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Disco"]["Minimum green color level"], out _) ? (int)ConfigToken["Screensaver"]["Disco"]["Minimum green color level"] : 0;
            DiscoSettings.DiscoMinimumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Disco"]["Minimum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["Disco"]["Minimum blue color level"] : 0;
            DiscoSettings.DiscoMinimumColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Disco"]["Minimum color level"], out _) ? (int)ConfigToken["Screensaver"]["Disco"]["Minimum color level"] : 0;
            DiscoSettings.DiscoMaximumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Disco"]["Maximum red color level"], out _) ? (int)ConfigToken["Screensaver"]["Disco"]["Maximum red color level"] : 255;
            DiscoSettings.DiscoMaximumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Disco"]["Maximum green color level"], out _) ? (int)ConfigToken["Screensaver"]["Disco"]["Maximum green color level"] : 255;
            DiscoSettings.DiscoMaximumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Disco"]["Maximum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["Disco"]["Maximum blue color level"] : 255;
            DiscoSettings.DiscoMaximumColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Disco"]["Maximum color level"], out _) ? (int)ConfigToken["Screensaver"]["Disco"]["Maximum color level"] : 255;

            // > GlitterColor
            GlitterColorSettings.GlitterColor255Colors = (bool)ConfigToken["Screensaver"]["GlitterColor"]["Activate 255 colors"];
            GlitterColorSettings.GlitterColorTrueColor = (bool)ConfigToken["Screensaver"]["GlitterColor"]["Activate true colors"];
            GlitterColorSettings.GlitterColorDelay = int.TryParse((string)ConfigToken["Screensaver"]["GlitterColor"]["Delay in Milliseconds"], out _) ? (int)ConfigToken["Screensaver"]["GlitterColor"]["Delay in Milliseconds"] : 1;
            GlitterColorSettings.GlitterColorMinimumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["GlitterColor"]["Minimum red color level"], out _) ? (int)ConfigToken["Screensaver"]["GlitterColor"]["Minimum red color level"] : 0;
            GlitterColorSettings.GlitterColorMinimumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["GlitterColor"]["Minimum green color level"], out _) ? (int)ConfigToken["Screensaver"]["GlitterColor"]["Minimum green color level"] : 0;
            GlitterColorSettings.GlitterColorMinimumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["GlitterColor"]["Minimum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["GlitterColor"]["Minimum blue color level"] : 0;
            GlitterColorSettings.GlitterColorMinimumColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["GlitterColor"]["Minimum color level"], out _) ? (int)ConfigToken["Screensaver"]["GlitterColor"]["Minimum color level"] : 0;
            GlitterColorSettings.GlitterColorMaximumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["GlitterColor"]["Maximum red color level"], out _) ? (int)ConfigToken["Screensaver"]["GlitterColor"]["Maximum red color level"] : 255;
            GlitterColorSettings.GlitterColorMaximumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["GlitterColor"]["Maximum green color level"], out _) ? (int)ConfigToken["Screensaver"]["GlitterColor"]["Maximum green color level"] : 255;
            GlitterColorSettings.GlitterColorMaximumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["GlitterColor"]["Maximum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["GlitterColor"]["Maximum blue color level"] : 255;
            GlitterColorSettings.GlitterColorMaximumColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["GlitterColor"]["Maximum color level"], out _) ? (int)ConfigToken["Screensaver"]["GlitterColor"]["Maximum color level"] : 255;

            // > GlitterMatrix
            GlitterMatrixSettings.GlitterMatrixDelay = int.TryParse((string)ConfigToken["Screensaver"]["GlitterMatrix"]["Delay in Milliseconds"], out _) ? (int)ConfigToken["Screensaver"]["GlitterMatrix"]["Delay in Milliseconds"] : 1;
            GlitterMatrixSettings.GlitterMatrixBackgroundColor = new Color((string)ConfigToken["Screensaver"]["GlitterMatrix"]["Background color"]).PlainSequence;
            GlitterMatrixSettings.GlitterMatrixForegroundColor = new Color((string)ConfigToken["Screensaver"]["GlitterMatrix"]["Foreground color"]).PlainSequence;

            // > Lines
            LinesSettings.Lines255Colors = (bool)ConfigToken["Screensaver"]["Lines"]["Activate 255 colors"];
            LinesSettings.LinesTrueColor = (bool)ConfigToken["Screensaver"]["Lines"]["Activate true colors"];
            LinesSettings.LinesDelay = int.TryParse((string)ConfigToken["Screensaver"]["Lines"]["Delay in Milliseconds"], out _) ? (int)ConfigToken["Screensaver"]["Lines"]["Delay in Milliseconds"] : 500;
            LinesSettings.LinesLineChar = (string)ConfigToken["Screensaver"]["Lines"]["Line character"] ?? "-";
            LinesSettings.LinesBackgroundColor = new Color((string)ConfigToken["Screensaver"]["Lines"]["Background color"]).PlainSequence;
            LinesSettings.LinesMinimumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Lines"]["Minimum red color level"], out _) ? (int)ConfigToken["Screensaver"]["Lines"]["Minimum red color level"] : 0;
            LinesSettings.LinesMinimumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Lines"]["Minimum green color level"], out _) ? (int)ConfigToken["Screensaver"]["Lines"]["Minimum green color level"] : 0;
            LinesSettings.LinesMinimumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Lines"]["Minimum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["Lines"]["Minimum blue color level"] : 0;
            LinesSettings.LinesMinimumColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Lines"]["Minimum color level"], out _) ? (int)ConfigToken["Screensaver"]["Lines"]["Minimum color level"] : 0;
            LinesSettings.LinesMaximumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Lines"]["Maximum red color level"], out _) ? (int)ConfigToken["Screensaver"]["Lines"]["Maximum red color level"] : 255;
            LinesSettings.LinesMaximumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Lines"]["Maximum green color level"], out _) ? (int)ConfigToken["Screensaver"]["Lines"]["Maximum green color level"] : 255;
            LinesSettings.LinesMaximumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Lines"]["Maximum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["Lines"]["Maximum blue color level"] : 255;
            LinesSettings.LinesMaximumColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Lines"]["Maximum color level"], out _) ? (int)ConfigToken["Screensaver"]["Lines"]["Maximum color level"] : 255;

            // > Dissolve
            DissolveSettings.Dissolve255Colors = (bool)ConfigToken["Screensaver"]["Dissolve"]["Activate 255 colors"];
            DissolveSettings.DissolveTrueColor = (bool)ConfigToken["Screensaver"]["Dissolve"]["Activate true colors"];
            DissolveSettings.DissolveBackgroundColor = new Color((string)ConfigToken["Screensaver"]["Dissolve"]["Background color"]).PlainSequence;
            DissolveSettings.DissolveMinimumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Dissolve"]["Minimum red color level"], out _) ? (int)ConfigToken["Screensaver"]["Dissolve"]["Minimum red color level"] : 0;
            DissolveSettings.DissolveMinimumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Dissolve"]["Minimum green color level"], out _) ? (int)ConfigToken["Screensaver"]["Dissolve"]["Minimum green color level"] : 0;
            DissolveSettings.DissolveMinimumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Dissolve"]["Minimum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["Dissolve"]["Minimum blue color level"] : 0;
            DissolveSettings.DissolveMinimumColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Dissolve"]["Minimum color level"], out _) ? (int)ConfigToken["Screensaver"]["Dissolve"]["Minimum color level"] : 0;
            DissolveSettings.DissolveMaximumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Dissolve"]["Maximum red color level"], out _) ? (int)ConfigToken["Screensaver"]["Dissolve"]["Maximum red color level"] : 255;
            DissolveSettings.DissolveMaximumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Dissolve"]["Maximum green color level"], out _) ? (int)ConfigToken["Screensaver"]["Dissolve"]["Maximum green color level"] : 255;
            DissolveSettings.DissolveMaximumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Dissolve"]["Maximum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["Dissolve"]["Maximum blue color level"] : 255;
            DissolveSettings.DissolveMaximumColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Dissolve"]["Maximum color level"], out _) ? (int)ConfigToken["Screensaver"]["Dissolve"]["Maximum color level"] : 255;

            // > BouncingBlock
            BouncingBlockSettings.BouncingBlock255Colors = (bool)ConfigToken["Screensaver"]["BouncingBlock"]["Activate 255 colors"];
            BouncingBlockSettings.BouncingBlockTrueColor = (bool)ConfigToken["Screensaver"]["BouncingBlock"]["Activate true colors"];
            BouncingBlockSettings.BouncingBlockDelay = int.TryParse((string)ConfigToken["Screensaver"]["BouncingBlock"]["Delay in Milliseconds"], out _) ? (int)ConfigToken["Screensaver"]["BouncingBlock"]["Delay in Milliseconds"] : 10;
            BouncingBlockSettings.BouncingBlockBackgroundColor = new Color((string)ConfigToken["Screensaver"]["BouncingBlock"]["Background color"]).PlainSequence;
            BouncingBlockSettings.BouncingBlockForegroundColor = new Color((string)ConfigToken["Screensaver"]["BouncingBlock"]["Foreground color"]).PlainSequence;
            BouncingBlockSettings.BouncingBlockMinimumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["BouncingBlock"]["Minimum red color level"], out _) ? (int)ConfigToken["Screensaver"]["BouncingBlock"]["Minimum red color level"] : 0;
            BouncingBlockSettings.BouncingBlockMinimumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["BouncingBlock"]["Minimum green color level"], out _) ? (int)ConfigToken["Screensaver"]["BouncingBlock"]["Minimum green color level"] : 0;
            BouncingBlockSettings.BouncingBlockMinimumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["BouncingBlock"]["Minimum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["BouncingBlock"]["Minimum blue color level"] : 0;
            BouncingBlockSettings.BouncingBlockMinimumColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["BouncingBlock"]["Minimum color level"], out _) ? (int)ConfigToken["Screensaver"]["BouncingBlock"]["Minimum color level"] : 0;
            BouncingBlockSettings.BouncingBlockMaximumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["BouncingBlock"]["Maximum red color level"], out _) ? (int)ConfigToken["Screensaver"]["BouncingBlock"]["Maximum red color level"] : 255;
            BouncingBlockSettings.BouncingBlockMaximumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["BouncingBlock"]["Maximum green color level"], out _) ? (int)ConfigToken["Screensaver"]["BouncingBlock"]["Maximum green color level"] : 255;
            BouncingBlockSettings.BouncingBlockMaximumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["BouncingBlock"]["Maximum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["BouncingBlock"]["Maximum blue color level"] : 255;
            BouncingBlockSettings.BouncingBlockMaximumColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["BouncingBlock"]["Maximum color level"], out _) ? (int)ConfigToken["Screensaver"]["BouncingBlock"]["Maximum color level"] : 255;

            // > BouncingText
            BouncingTextSettings.BouncingText255Colors = (bool)ConfigToken["Screensaver"]["BouncingText"]["Activate 255 colors"];
            BouncingTextSettings.BouncingTextTrueColor = (bool)ConfigToken["Screensaver"]["BouncingText"]["Activate true colors"];
            BouncingTextSettings.BouncingTextDelay = int.TryParse((string)ConfigToken["Screensaver"]["BouncingText"]["Delay in Milliseconds"], out _) ? (int)ConfigToken["Screensaver"]["BouncingText"]["Delay in Milliseconds"] : 10;
            BouncingTextSettings.BouncingTextWrite = (string)ConfigToken["Screensaver"]["BouncingText"]["Text shown"] ?? "Kernel Simulator";
            BouncingTextSettings.BouncingTextBackgroundColor = new Color((string)ConfigToken["Screensaver"]["BouncingText"]["Background color"]).PlainSequence;
            BouncingTextSettings.BouncingTextForegroundColor = new Color((string)ConfigToken["Screensaver"]["BouncingText"]["Foreground color"]).PlainSequence;
            BouncingTextSettings.BouncingTextMinimumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["BouncingText"]["Minimum red color level"], out _) ? (int)ConfigToken["Screensaver"]["BouncingText"]["Minimum red color level"] : 0;
            BouncingTextSettings.BouncingTextMinimumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["BouncingText"]["Minimum green color level"], out _) ? (int)ConfigToken["Screensaver"]["BouncingText"]["Minimum green color level"] : 0;
            BouncingTextSettings.BouncingTextMinimumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["BouncingText"]["Minimum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["BouncingText"]["Minimum blue color level"] : 0;
            BouncingTextSettings.BouncingTextMinimumColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["BouncingText"]["Minimum color level"], out _) ? (int)ConfigToken["Screensaver"]["BouncingText"]["Minimum color level"] : 0;
            BouncingTextSettings.BouncingTextMaximumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["BouncingText"]["Maximum red color level"], out _) ? (int)ConfigToken["Screensaver"]["BouncingText"]["Maximum red color level"] : 255;
            BouncingTextSettings.BouncingTextMaximumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["BouncingText"]["Maximum green color level"], out _) ? (int)ConfigToken["Screensaver"]["BouncingText"]["Maximum green color level"] : 255;
            BouncingTextSettings.BouncingTextMaximumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["BouncingText"]["Maximum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["BouncingText"]["Maximum blue color level"] : 255;
            BouncingTextSettings.BouncingTextMaximumColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["BouncingText"]["Maximum color level"], out _) ? (int)ConfigToken["Screensaver"]["BouncingText"]["Maximum color level"] : 255;

            // > ProgressClock
            ProgressClockSettings.ProgressClock255Colors = (bool)ConfigToken["Screensaver"]["ProgressClock"]["Activate 255 colors"];
            ProgressClockSettings.ProgressClockTrueColor = (bool)ConfigToken["Screensaver"]["ProgressClock"]["Activate true colors"];
            ProgressClockSettings.ProgressClockCycleColors = (bool)ConfigToken["Screensaver"]["ProgressClock"]["Cycle colors"];
            ProgressClockSettings.ProgressClockSecondsProgressColor = (string)ConfigToken["Screensaver"]["ProgressClock"]["Color of Seconds Bar"];
            ProgressClockSettings.ProgressClockMinutesProgressColor = (string)ConfigToken["Screensaver"]["ProgressClock"]["Color of Minutes Bar"];
            ProgressClockSettings.ProgressClockHoursProgressColor = (string)ConfigToken["Screensaver"]["ProgressClock"]["Color of Hours Bar"];
            ProgressClockSettings.ProgressClockProgressColor = (string)ConfigToken["Screensaver"]["ProgressClock"]["Color of Information"];
            ProgressClockSettings.ProgressClockCycleColorsTicks = int.TryParse((string)ConfigToken["Screensaver"]["ProgressClock"]["Ticks to change color"], out _) ? (int)ConfigToken["Screensaver"]["ProgressClock"]["Ticks to change color"] : 20;
            ProgressClockSettings.ProgressClockDelay = int.TryParse((string)ConfigToken["Screensaver"]["ProgressClock"]["Delay in Milliseconds"], out _) ? (int)ConfigToken["Screensaver"]["ProgressClock"]["Delay in Milliseconds"] : 500;
            ProgressClockSettings.ProgressClockUpperLeftCornerCharHours = (string)ConfigToken["Screensaver"]["ProgressClock"]["Upper left corner character for hours bar"] ?? "╔";
            ProgressClockSettings.ProgressClockUpperLeftCornerCharMinutes = (string)ConfigToken["Screensaver"]["ProgressClock"]["Upper left corner character for minutes bar"] ?? "╔";
            ProgressClockSettings.ProgressClockUpperLeftCornerCharSeconds = (string)ConfigToken["Screensaver"]["ProgressClock"]["Upper left corner character for seconds bar"] ?? "╔";
            ProgressClockSettings.ProgressClockUpperRightCornerCharHours = (string)ConfigToken["Screensaver"]["ProgressClock"]["Upper right corner character for hours bar"] ?? "╗";
            ProgressClockSettings.ProgressClockUpperRightCornerCharMinutes = (string)ConfigToken["Screensaver"]["ProgressClock"]["Upper right corner character for minutes bar"] ?? "╗";
            ProgressClockSettings.ProgressClockUpperRightCornerCharSeconds = (string)ConfigToken["Screensaver"]["ProgressClock"]["Upper right corner character for seconds bar"] ?? "╗";
            ProgressClockSettings.ProgressClockLowerLeftCornerCharHours = (string)ConfigToken["Screensaver"]["ProgressClock"]["Lower left corner character for hours bar"] ?? "╚";
            ProgressClockSettings.ProgressClockLowerLeftCornerCharMinutes = (string)ConfigToken["Screensaver"]["ProgressClock"]["Lower left corner character for minutes bar"] ?? "╚";
            ProgressClockSettings.ProgressClockLowerLeftCornerCharSeconds = (string)ConfigToken["Screensaver"]["ProgressClock"]["Lower left corner character for seconds bar"] ?? "╚";
            ProgressClockSettings.ProgressClockLowerRightCornerCharHours = (string)ConfigToken["Screensaver"]["ProgressClock"]["Lower right corner character for hours bar"] ?? "╝";
            ProgressClockSettings.ProgressClockLowerRightCornerCharMinutes = (string)ConfigToken["Screensaver"]["ProgressClock"]["Lower right corner character for minutes bar"] ?? "╝";
            ProgressClockSettings.ProgressClockLowerRightCornerCharSeconds = (string)ConfigToken["Screensaver"]["ProgressClock"]["Lower right corner character for seconds bar"] ?? "╝";
            ProgressClockSettings.ProgressClockUpperFrameCharHours = (string)ConfigToken["Screensaver"]["ProgressClock"]["Upper frame character for hours bar"] ?? "═";
            ProgressClockSettings.ProgressClockUpperFrameCharMinutes = (string)ConfigToken["Screensaver"]["ProgressClock"]["Upper frame character for minutes bar"] ?? "═";
            ProgressClockSettings.ProgressClockUpperFrameCharSeconds = (string)ConfigToken["Screensaver"]["ProgressClock"]["Upper frame character for seconds bar"] ?? "═";
            ProgressClockSettings.ProgressClockLowerFrameCharHours = (string)ConfigToken["Screensaver"]["ProgressClock"]["Lower frame character for hours bar"] ?? "═";
            ProgressClockSettings.ProgressClockLowerFrameCharMinutes = (string)ConfigToken["Screensaver"]["ProgressClock"]["Lower frame character for minutes bar"] ?? "═";
            ProgressClockSettings.ProgressClockLowerFrameCharSeconds = (string)ConfigToken["Screensaver"]["ProgressClock"]["Lower frame character for seconds bar"] ?? "═";
            ProgressClockSettings.ProgressClockLeftFrameCharHours = (string)ConfigToken["Screensaver"]["ProgressClock"]["Left frame character for hours bar"] ?? "║";
            ProgressClockSettings.ProgressClockLeftFrameCharMinutes = (string)ConfigToken["Screensaver"]["ProgressClock"]["Left frame character for minutes bar"] ?? "║";
            ProgressClockSettings.ProgressClockLeftFrameCharSeconds = (string)ConfigToken["Screensaver"]["ProgressClock"]["Left frame character for seconds bar"] ?? "║";
            ProgressClockSettings.ProgressClockRightFrameCharHours = (string)ConfigToken["Screensaver"]["ProgressClock"]["Right frame character for hours bar"] ?? "║";
            ProgressClockSettings.ProgressClockRightFrameCharMinutes = (string)ConfigToken["Screensaver"]["ProgressClock"]["Right frame character for minutes bar"] ?? "║";
            ProgressClockSettings.ProgressClockRightFrameCharSeconds = (string)ConfigToken["Screensaver"]["ProgressClock"]["Right frame character for seconds bar"] ?? "║";
            ProgressClockSettings.ProgressClockInfoTextHours = (string)ConfigToken["Screensaver"]["ProgressClock"]["Information text for hours"] ?? "";
            ProgressClockSettings.ProgressClockInfoTextMinutes = (string)ConfigToken["Screensaver"]["ProgressClock"]["Information text for minutes"] ?? "";
            ProgressClockSettings.ProgressClockInfoTextSeconds = (string)ConfigToken["Screensaver"]["ProgressClock"]["Information text for seconds"] ?? "";
            ProgressClockSettings.ProgressClockMinimumRedColorLevelHours = int.TryParse((string)ConfigToken["Screensaver"]["ProgressClock"]["Minimum red color level for hours"], out _) ? (int)ConfigToken["Screensaver"]["ProgressClock"]["Minimum red color level for hours"] : 0;
            ProgressClockSettings.ProgressClockMinimumGreenColorLevelHours = int.TryParse((string)ConfigToken["Screensaver"]["ProgressClock"]["Minimum green color level for hours"], out _) ? (int)ConfigToken["Screensaver"]["ProgressClock"]["Minimum green color level for hours"] : 0;
            ProgressClockSettings.ProgressClockMinimumBlueColorLevelHours = int.TryParse((string)ConfigToken["Screensaver"]["ProgressClock"]["Minimum blue color level for hours"], out _) ? (int)ConfigToken["Screensaver"]["ProgressClock"]["Minimum blue color level for hours"] : 0;
            ProgressClockSettings.ProgressClockMinimumColorLevelHours = int.TryParse((string)ConfigToken["Screensaver"]["ProgressClock"]["Minimum color level for hours"], out _) ? (int)ConfigToken["Screensaver"]["ProgressClock"]["Minimum color level for hours"] : 0;
            ProgressClockSettings.ProgressClockMaximumRedColorLevelHours = int.TryParse((string)ConfigToken["Screensaver"]["ProgressClock"]["Maximum red color level for hours"], out _) ? (int)ConfigToken["Screensaver"]["ProgressClock"]["Maximum red color level for hours"] : 255;
            ProgressClockSettings.ProgressClockMaximumGreenColorLevelHours = int.TryParse((string)ConfigToken["Screensaver"]["ProgressClock"]["Maximum green color level for hours"], out _) ? (int)ConfigToken["Screensaver"]["ProgressClock"]["Maximum green color level for hours"] : 255;
            ProgressClockSettings.ProgressClockMaximumBlueColorLevelHours = int.TryParse((string)ConfigToken["Screensaver"]["ProgressClock"]["Maximum blue color level for hours"], out _) ? (int)ConfigToken["Screensaver"]["ProgressClock"]["Maximum blue color level for hours"] : 255;
            ProgressClockSettings.ProgressClockMaximumColorLevelHours = int.TryParse((string)ConfigToken["Screensaver"]["ProgressClock"]["Maximum color level for hours"], out _) ? (int)ConfigToken["Screensaver"]["ProgressClock"]["Maximum color level for hours"] : 255;
            ProgressClockSettings.ProgressClockMinimumRedColorLevelMinutes = int.TryParse((string)ConfigToken["Screensaver"]["ProgressClock"]["Minimum red color level for minutes"], out _) ? (int)ConfigToken["Screensaver"]["ProgressClock"]["Minimum red color level for minutes"] : 0;
            ProgressClockSettings.ProgressClockMinimumGreenColorLevelMinutes = int.TryParse((string)ConfigToken["Screensaver"]["ProgressClock"]["Minimum green color level for minutes"], out _) ? (int)ConfigToken["Screensaver"]["ProgressClock"]["Minimum green color level for minutes"] : 0;
            ProgressClockSettings.ProgressClockMinimumBlueColorLevelMinutes = int.TryParse((string)ConfigToken["Screensaver"]["ProgressClock"]["Minimum blue color level for minutes"], out _) ? (int)ConfigToken["Screensaver"]["ProgressClock"]["Minimum blue color level for minutes"] : 0;
            ProgressClockSettings.ProgressClockMinimumColorLevelMinutes = int.TryParse((string)ConfigToken["Screensaver"]["ProgressClock"]["Minimum color level for minutes"], out _) ? (int)ConfigToken["Screensaver"]["ProgressClock"]["Minimum color level for minutes"] : 0;
            ProgressClockSettings.ProgressClockMaximumRedColorLevelMinutes = int.TryParse((string)ConfigToken["Screensaver"]["ProgressClock"]["Maximum red color level for minutes"], out _) ? (int)ConfigToken["Screensaver"]["ProgressClock"]["Maximum red color level for minutes"] : 255;
            ProgressClockSettings.ProgressClockMaximumGreenColorLevelMinutes = int.TryParse((string)ConfigToken["Screensaver"]["ProgressClock"]["Maximum green color level for minutes"], out _) ? (int)ConfigToken["Screensaver"]["ProgressClock"]["Maximum green color level for minutes"] : 255;
            ProgressClockSettings.ProgressClockMaximumBlueColorLevelMinutes = int.TryParse((string)ConfigToken["Screensaver"]["ProgressClock"]["Maximum blue color level for minutes"], out _) ? (int)ConfigToken["Screensaver"]["ProgressClock"]["Maximum blue color level for minutes"] : 255;
            ProgressClockSettings.ProgressClockMaximumColorLevelMinutes = int.TryParse((string)ConfigToken["Screensaver"]["ProgressClock"]["Maximum color level for minutes"], out _) ? (int)ConfigToken["Screensaver"]["ProgressClock"]["Maximum color level for minutes"] : 255;
            ProgressClockSettings.ProgressClockMinimumRedColorLevelSeconds = int.TryParse((string)ConfigToken["Screensaver"]["ProgressClock"]["Minimum red color level for seconds"], out _) ? (int)ConfigToken["Screensaver"]["ProgressClock"]["Minimum red color level for seconds"] : 0;
            ProgressClockSettings.ProgressClockMinimumGreenColorLevelSeconds = int.TryParse((string)ConfigToken["Screensaver"]["ProgressClock"]["Minimum green color level for seconds"], out _) ? (int)ConfigToken["Screensaver"]["ProgressClock"]["Minimum green color level for seconds"] : 0;
            ProgressClockSettings.ProgressClockMinimumBlueColorLevelSeconds = int.TryParse((string)ConfigToken["Screensaver"]["ProgressClock"]["Minimum blue color level for seconds"], out _) ? (int)ConfigToken["Screensaver"]["ProgressClock"]["Minimum blue color level for seconds"] : 0;
            ProgressClockSettings.ProgressClockMinimumColorLevelSeconds = int.TryParse((string)ConfigToken["Screensaver"]["ProgressClock"]["Minimum color level for seconds"], out _) ? (int)ConfigToken["Screensaver"]["ProgressClock"]["Minimum color level for seconds"] : 0;
            ProgressClockSettings.ProgressClockMaximumRedColorLevelSeconds = int.TryParse((string)ConfigToken["Screensaver"]["ProgressClock"]["Maximum red color level for seconds"], out _) ? (int)ConfigToken["Screensaver"]["ProgressClock"]["Maximum red color level for seconds"] : 255;
            ProgressClockSettings.ProgressClockMaximumGreenColorLevelSeconds = int.TryParse((string)ConfigToken["Screensaver"]["ProgressClock"]["Maximum green color level for seconds"], out _) ? (int)ConfigToken["Screensaver"]["ProgressClock"]["Maximum green color level for seconds"] : 255;
            ProgressClockSettings.ProgressClockMaximumBlueColorLevelSeconds = int.TryParse((string)ConfigToken["Screensaver"]["ProgressClock"]["Maximum blue color level for seconds"], out _) ? (int)ConfigToken["Screensaver"]["ProgressClock"]["Maximum blue color level for seconds"] : 255;
            ProgressClockSettings.ProgressClockMaximumColorLevelSeconds = int.TryParse((string)ConfigToken["Screensaver"]["ProgressClock"]["Maximum color level for seconds"], out _) ? (int)ConfigToken["Screensaver"]["ProgressClock"]["Maximum color level for seconds"] : 255;
            ProgressClockSettings.ProgressClockMinimumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["ProgressClock"]["Minimum red color level"], out _) ? (int)ConfigToken["Screensaver"]["ProgressClock"]["Minimum red color level"] : 0;
            ProgressClockSettings.ProgressClockMinimumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["ProgressClock"]["Minimum green color level"], out _) ? (int)ConfigToken["Screensaver"]["ProgressClock"]["Minimum green color level"] : 0;
            ProgressClockSettings.ProgressClockMinimumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["ProgressClock"]["Minimum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["ProgressClock"]["Minimum blue color level"] : 0;
            ProgressClockSettings.ProgressClockMinimumColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["ProgressClock"]["Minimum color level"], out _) ? (int)ConfigToken["Screensaver"]["ProgressClock"]["Minimum color level"] : 0;
            ProgressClockSettings.ProgressClockMaximumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["ProgressClock"]["Maximum red color level"], out _) ? (int)ConfigToken["Screensaver"]["ProgressClock"]["Maximum red color level"] : 255;
            ProgressClockSettings.ProgressClockMaximumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["ProgressClock"]["Maximum green color level"], out _) ? (int)ConfigToken["Screensaver"]["ProgressClock"]["Maximum green color level"] : 255;
            ProgressClockSettings.ProgressClockMaximumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["ProgressClock"]["Maximum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["ProgressClock"]["Maximum blue color level"] : 255;
            ProgressClockSettings.ProgressClockMaximumColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["ProgressClock"]["Maximum color level"], out _) ? (int)ConfigToken["Screensaver"]["ProgressClock"]["Maximum color level"] : 255;

            // > Lighter
            LighterSettings.Lighter255Colors = (bool)ConfigToken["Screensaver"]["Lighter"]["Activate 255 colors"];
            LighterSettings.LighterTrueColor = (bool)ConfigToken["Screensaver"]["Lighter"]["Activate true colors"];
            LighterSettings.LighterDelay = int.TryParse((string)ConfigToken["Screensaver"]["Lighter"]["Delay in Milliseconds"], out _) ? (int)ConfigToken["Screensaver"]["Lighter"]["Delay in Milliseconds"] : 100;
            LighterSettings.LighterMaxPositions = int.TryParse((string)ConfigToken["Screensaver"]["Lighter"]["Max Positions Count"], out _) ? (int)ConfigToken["Screensaver"]["Lighter"]["Max Positions Count"] : 10;
            LighterSettings.LighterBackgroundColor = new Color((string)ConfigToken["Screensaver"]["Lighter"]["Background color"]).PlainSequence;
            LighterSettings.LighterMinimumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Lighter"]["Minimum red color level"], out _) ? (int)ConfigToken["Screensaver"]["Lighter"]["Minimum red color level"] : 0;
            LighterSettings.LighterMinimumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Lighter"]["Minimum green color level"], out _) ? (int)ConfigToken["Screensaver"]["Lighter"]["Minimum green color level"] : 0;
            LighterSettings.LighterMinimumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Lighter"]["Minimum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["Lighter"]["Minimum blue color level"] : 0;
            LighterSettings.LighterMinimumColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Lighter"]["Minimum color level"], out _) ? (int)ConfigToken["Screensaver"]["Lighter"]["Minimum color level"] : 0;
            LighterSettings.LighterMaximumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Lighter"]["Maximum red color level"], out _) ? (int)ConfigToken["Screensaver"]["Lighter"]["Maximum red color level"] : 255;
            LighterSettings.LighterMaximumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Lighter"]["Maximum green color level"], out _) ? (int)ConfigToken["Screensaver"]["Lighter"]["Maximum green color level"] : 255;
            LighterSettings.LighterMaximumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Lighter"]["Maximum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["Lighter"]["Maximum blue color level"] : 255;
            LighterSettings.LighterMaximumColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Lighter"]["Maximum color level"], out _) ? (int)ConfigToken["Screensaver"]["Lighter"]["Maximum color level"] : 255;

            // > Wipe
            WipeSettings.Wipe255Colors = (bool)ConfigToken["Screensaver"]["Wipe"]["Activate 255 colors"];
            WipeSettings.WipeTrueColor = (bool)ConfigToken["Screensaver"]["Wipe"]["Activate true colors"];
            WipeSettings.WipeDelay = int.TryParse((string)ConfigToken["Screensaver"]["Wipe"]["Delay in Milliseconds"], out _) ? (int)ConfigToken["Screensaver"]["Wipe"]["Delay in Milliseconds"] : 10;
            WipeSettings.WipeWipesNeededToChangeDirection = int.TryParse((string)ConfigToken["Screensaver"]["Wipe"]["Wipes to change direction"], out _) ? (int)ConfigToken["Screensaver"]["Wipe"]["Wipes to change direction"] : 10;
            WipeSettings.WipeBackgroundColor = new Color((string)ConfigToken["Screensaver"]["Wipe"]["Background color"]).PlainSequence;
            WipeSettings.WipeMinimumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Wipe"]["Minimum red color level"], out _) ? (int)ConfigToken["Screensaver"]["Wipe"]["Minimum red color level"] : 0;
            WipeSettings.WipeMinimumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Wipe"]["Minimum green color level"], out _) ? (int)ConfigToken["Screensaver"]["Wipe"]["Minimum green color level"] : 0;
            WipeSettings.WipeMinimumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Wipe"]["Minimum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["Wipe"]["Minimum blue color level"] : 0;
            WipeSettings.WipeMinimumColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Wipe"]["Minimum color level"], out _) ? (int)ConfigToken["Screensaver"]["Wipe"]["Minimum color level"] : 0;
            WipeSettings.WipeMaximumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Wipe"]["Maximum red color level"], out _) ? (int)ConfigToken["Screensaver"]["Wipe"]["Maximum red color level"] : 255;
            WipeSettings.WipeMaximumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Wipe"]["Maximum green color level"], out _) ? (int)ConfigToken["Screensaver"]["Wipe"]["Maximum green color level"] : 255;
            WipeSettings.WipeMaximumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Wipe"]["Maximum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["Wipe"]["Maximum blue color level"] : 255;
            WipeSettings.WipeMaximumColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Wipe"]["Maximum color level"], out _) ? (int)ConfigToken["Screensaver"]["Wipe"]["Maximum color level"] : 255;

            // > Fader
            FaderSettings.FaderDelay = int.TryParse((string)ConfigToken["Screensaver"]["Fader"]["Delay in Milliseconds"], out _) ? (int)ConfigToken["Screensaver"]["Fader"]["Delay in Milliseconds"] : 50;
            FaderSettings.FaderFadeOutDelay = int.TryParse((string)ConfigToken["Screensaver"]["Fader"]["Fade Out Delay in Milliseconds"], out _) ? (int)ConfigToken["Screensaver"]["Fader"]["Fade Out Delay in Milliseconds"] : 3000;
            FaderSettings.FaderWrite = (string)ConfigToken["Screensaver"]["Fader"]["Text shown"] ?? "Kernel Simulator";
            FaderSettings.FaderMaxSteps = int.TryParse((string)ConfigToken["Screensaver"]["Fader"]["Max Fade Steps"], out _) ? (int)ConfigToken["Screensaver"]["Fader"]["Max Fade Steps"] : 25;
            FaderSettings.FaderBackgroundColor = new Color((string)ConfigToken["Screensaver"]["Fader"]["Background color"]).PlainSequence;
            FaderSettings.FaderMinimumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Fader"]["Minimum red color level"], out _) ? (int)ConfigToken["Screensaver"]["Fader"]["Minimum red color level"] : 0;
            FaderSettings.FaderMinimumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Fader"]["Minimum green color level"], out _) ? (int)ConfigToken["Screensaver"]["Fader"]["Minimum green color level"] : 0;
            FaderSettings.FaderMinimumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Fader"]["Minimum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["Fader"]["Minimum blue color level"] : 0;
            FaderSettings.FaderMaximumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Fader"]["Maximum red color level"], out _) ? (int)ConfigToken["Screensaver"]["Fader"]["Maximum red color level"] : 255;
            FaderSettings.FaderMaximumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Fader"]["Maximum green color level"], out _) ? (int)ConfigToken["Screensaver"]["Fader"]["Maximum green color level"] : 255;
            FaderSettings.FaderMaximumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Fader"]["Maximum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["Fader"]["Maximum blue color level"] : 255;

            // > FaderBack
            FaderBackSettings.FaderBackDelay = int.TryParse((string)ConfigToken["Screensaver"]["FaderBack"]["Delay in Milliseconds"], out _) ? (int)ConfigToken["Screensaver"]["FaderBack"]["Delay in Milliseconds"] : 50;
            FaderBackSettings.FaderBackFadeOutDelay = int.TryParse((string)ConfigToken["Screensaver"]["FaderBack"]["Fade Out Delay in Milliseconds"], out _) ? (int)ConfigToken["Screensaver"]["FaderBack"]["Fade Out Delay in Milliseconds"] : 3000;
            FaderBackSettings.FaderBackMaxSteps = int.TryParse((string)ConfigToken["Screensaver"]["FaderBack"]["Max Fade Steps"], out _) ? (int)ConfigToken["Screensaver"]["FaderBack"]["Max Fade Steps"] : 25;
            FaderBackSettings.FaderBackMinimumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["FaderBack"]["Minimum red color level"], out _) ? (int)ConfigToken["Screensaver"]["FaderBack"]["Minimum red color level"] : 0;
            FaderBackSettings.FaderBackMinimumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["FaderBack"]["Minimum green color level"], out _) ? (int)ConfigToken["Screensaver"]["FaderBack"]["Minimum green color level"] : 0;
            FaderBackSettings.FaderBackMinimumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["FaderBack"]["Minimum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["FaderBack"]["Minimum blue color level"] : 0;
            FaderBackSettings.FaderBackMaximumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["FaderBack"]["Maximum red color level"], out _) ? (int)ConfigToken["Screensaver"]["FaderBack"]["Maximum red color level"] : 255;
            FaderBackSettings.FaderBackMaximumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["FaderBack"]["Maximum green color level"], out _) ? (int)ConfigToken["Screensaver"]["FaderBack"]["Maximum green color level"] : 255;
            FaderBackSettings.FaderBackMaximumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["FaderBack"]["Maximum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["FaderBack"]["Maximum blue color level"] : 255;

            // > BeatFader
            BeatFaderSettings.BeatFader255Colors = (bool)ConfigToken["Screensaver"]["BeatFader"]["Activate 255 colors"];
            BeatFaderSettings.BeatFaderTrueColor = (bool)ConfigToken["Screensaver"]["BeatFader"]["Activate true colors"];
            BeatFaderSettings.BeatFaderCycleColors = (bool)ConfigToken["Screensaver"]["BeatFader"]["Cycle colors"];
            BeatFaderSettings.BeatFaderBeatColor = (string)ConfigToken["Screensaver"]["BeatFader"]["Beat color"];
            BeatFaderSettings.BeatFaderDelay = int.TryParse((string)ConfigToken["Screensaver"]["BeatFader"]["Delay in Beats Per Minute"], out _) ? (int)ConfigToken["Screensaver"]["BeatFader"]["Delay in Beats Per Minute"] : 120;
            BeatFaderSettings.BeatFaderMaxSteps = int.TryParse((string)ConfigToken["Screensaver"]["BeatFader"]["Max Fade Steps"], out _) ? (int)ConfigToken["Screensaver"]["BeatFader"]["Max Fade Steps"] : 25;
            BeatFaderSettings.BeatFaderMinimumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["BeatFader"]["Minimum red color level"], out _) ? (int)ConfigToken["Screensaver"]["BeatFader"]["Minimum red color level"] : 0;
            BeatFaderSettings.BeatFaderMinimumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["BeatFader"]["Minimum green color level"], out _) ? (int)ConfigToken["Screensaver"]["BeatFader"]["Minimum green color level"] : 0;
            BeatFaderSettings.BeatFaderMinimumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["BeatFader"]["Minimum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["BeatFader"]["Minimum blue color level"] : 0;
            BeatFaderSettings.BeatFaderMinimumColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["BeatFader"]["Minimum color level"], out _) ? (int)ConfigToken["Screensaver"]["BeatFader"]["Minimum color level"] : 0;
            BeatFaderSettings.BeatFaderMaximumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["BeatFader"]["Maximum red color level"], out _) ? (int)ConfigToken["Screensaver"]["BeatFader"]["Maximum red color level"] : 255;
            BeatFaderSettings.BeatFaderMaximumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["BeatFader"]["Maximum green color level"], out _) ? (int)ConfigToken["Screensaver"]["BeatFader"]["Maximum green color level"] : 255;
            BeatFaderSettings.BeatFaderMaximumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["BeatFader"]["Maximum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["BeatFader"]["Maximum blue color level"] : 255;
            BeatFaderSettings.BeatFaderMaximumColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["BeatFader"]["Maximum color level"], out _) ? (int)ConfigToken["Screensaver"]["BeatFader"]["Maximum color level"] : 255;

            // > Typo
            TypoSettings.TypoDelay = int.TryParse((string)ConfigToken["Screensaver"]["Typo"]["Delay in Milliseconds"], out _) ? (int)ConfigToken["Screensaver"]["Typo"]["Delay in Milliseconds"] : 50;
            TypoSettings.TypoWriteAgainDelay = int.TryParse((string)ConfigToken["Screensaver"]["Typo"]["Write Again Delay in Milliseconds"], out _) ? (int)ConfigToken["Screensaver"]["Typo"]["Write Again Delay in Milliseconds"] : 3000;
            TypoSettings.TypoWrite = (string)ConfigToken["Screensaver"]["Typo"]["Text shown"] ?? "Kernel Simulator";
            TypoSettings.TypoWritingSpeedMin = int.TryParse((string)ConfigToken["Screensaver"]["Typo"]["Minimum writing speed in WPM"], out _) ? (int)ConfigToken["Screensaver"]["Typo"]["Minimum writing speed in WPM"] : 50;
            TypoSettings.TypoWritingSpeedMax = int.TryParse((string)ConfigToken["Screensaver"]["Typo"]["Maximum writing speed in WPM"], out _) ? (int)ConfigToken["Screensaver"]["Typo"]["Maximum writing speed in WPM"] : 80;
            TypoSettings.TypoMissStrikePossibility = int.TryParse((string)ConfigToken["Screensaver"]["Typo"]["Probability of typo in percent"], out _) ? (int)ConfigToken["Screensaver"]["Typo"]["Probability of typo in percent"] : 20;
            TypoSettings.TypoMissPossibility = int.TryParse((string)ConfigToken["Screensaver"]["Typo"]["Probability of miss in percent"], out _) ? (int)ConfigToken["Screensaver"]["Typo"]["Probability of miss in percent"] : 10;
            TypoSettings.TypoTextColor = new Color((string)ConfigToken["Screensaver"]["Typo"]["Text color"]).PlainSequence;

            // > Marquee
            MarqueeSettings.Marquee255Colors = (bool)ConfigToken["Screensaver"]["Marquee"]["Activate 255 colors"];
            MarqueeSettings.MarqueeTrueColor = (bool)ConfigToken["Screensaver"]["Marquee"]["Activate true colors"];
            MarqueeSettings.MarqueeWrite = (string)ConfigToken["Screensaver"]["Marquee"]["Text shown"] ?? "Kernel Simulator";
            MarqueeSettings.MarqueeDelay = int.TryParse((string)ConfigToken["Screensaver"]["Marquee"]["Delay in Milliseconds"], out _) ? (int)ConfigToken["Screensaver"]["Marquee"]["Delay in Milliseconds"] : 10;
            MarqueeSettings.MarqueeAlwaysCentered = (bool)ConfigToken["Screensaver"]["Marquee"]["Always centered"];
            MarqueeSettings.MarqueeUseConsoleAPI = (bool)ConfigToken["Screensaver"]["Marquee"]["Use Console API"];
            MarqueeSettings.MarqueeMinimumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Marquee"]["Minimum red color level"], out _) ? (int)ConfigToken["Screensaver"]["Marquee"]["Minimum red color level"] : 0;
            MarqueeSettings.MarqueeMinimumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Marquee"]["Minimum green color level"], out _) ? (int)ConfigToken["Screensaver"]["Marquee"]["Minimum green color level"] : 0;
            MarqueeSettings.MarqueeMinimumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Marquee"]["Minimum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["Marquee"]["Minimum blue color level"] : 0;
            MarqueeSettings.MarqueeMinimumColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Marquee"]["Minimum color level"], out _) ? (int)ConfigToken["Screensaver"]["Marquee"]["Minimum color level"] : 0;
            MarqueeSettings.MarqueeMaximumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Marquee"]["Maximum red color level"], out _) ? (int)ConfigToken["Screensaver"]["Marquee"]["Maximum red color level"] : 255;
            MarqueeSettings.MarqueeMaximumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Marquee"]["Maximum green color level"], out _) ? (int)ConfigToken["Screensaver"]["Marquee"]["Maximum green color level"] : 255;
            MarqueeSettings.MarqueeMaximumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Marquee"]["Maximum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["Marquee"]["Maximum blue color level"] : 255;
            MarqueeSettings.MarqueeMaximumColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Marquee"]["Maximum color level"], out _) ? (int)ConfigToken["Screensaver"]["Marquee"]["Maximum color level"] : 255;
            MarqueeSettings.MarqueeBackgroundColor = new Color((string)ConfigToken["Screensaver"]["Marquee"]["Background color"]).PlainSequence;

            // > Matrix
            MatrixSettings.MatrixDelay = int.TryParse((string)ConfigToken["Screensaver"]["Matrix"]["Delay in Milliseconds"], out _) ? (int)ConfigToken["Screensaver"]["Matrix"]["Delay in Milliseconds"] : 1;

            // > Linotypo
            bool tryGetEtaoinType()
            {
                var ret = Enum.TryParse((string)ConfigToken["Screensaver"]["Linotypo"]["Line Fill Type"], out LinotypoSettings.FillType argresult);
                LinotypoSettings.LinotypoEtaoinType = argresult;
                return ret;
            }
            LinotypoSettings.LinotypoDelay = int.TryParse((string)ConfigToken["Screensaver"]["Linotypo"]["Delay in Milliseconds"], out _) ? (int)ConfigToken["Screensaver"]["Linotypo"]["Delay in Milliseconds"] : 50;
            LinotypoSettings.LinotypoNewScreenDelay = int.TryParse((string)ConfigToken["Screensaver"]["Linotypo"]["New Screen Delay in Milliseconds"], out _) ? (int)ConfigToken["Screensaver"]["Linotypo"]["New Screen Delay in Milliseconds"] : 3000;
            LinotypoSettings.LinotypoWrite = (string)ConfigToken["Screensaver"]["Linotypo"]["Text shown"] ?? "Kernel Simulator";
            LinotypoSettings.LinotypoWritingSpeedMin = int.TryParse((string)ConfigToken["Screensaver"]["Linotypo"]["Minimum writing speed in WPM"], out _) ? (int)ConfigToken["Screensaver"]["Linotypo"]["Minimum writing speed in WPM"] : 50;
            LinotypoSettings.LinotypoWritingSpeedMax = int.TryParse((string)ConfigToken["Screensaver"]["Linotypo"]["Maximum writing speed in WPM"], out _) ? (int)ConfigToken["Screensaver"]["Linotypo"]["Maximum writing speed in WPM"] : 80;
            LinotypoSettings.LinotypoMissStrikePossibility = int.TryParse((string)ConfigToken["Screensaver"]["Linotypo"]["Probability of typo in percent"], out _) ? (int)ConfigToken["Screensaver"]["Linotypo"]["Probability of typo in percent"] : 1;
            LinotypoSettings.LinotypoTextColumns = int.TryParse((string)ConfigToken["Screensaver"]["Linotypo"]["Column Count"], out _) ? (int)ConfigToken["Screensaver"]["Linotypo"]["Column Count"] : 3;
            LinotypoSettings.LinotypoEtaoinThreshold = int.TryParse((string)ConfigToken["Screensaver"]["Linotypo"]["Line Fill Threshold"], out _) ? (int)ConfigToken["Screensaver"]["Linotypo"]["Line Fill Threshold"] : 5;
            LinotypoSettings.LinotypoEtaoinCappingPossibility = int.TryParse((string)ConfigToken["Screensaver"]["Linotypo"]["Line Fill Capping Probability in percent"], out _) ? (int)ConfigToken["Screensaver"]["Linotypo"]["Line Fill Capping Probability in percent"] : 5;
            LinotypoSettings.LinotypoEtaoinType = ConfigToken["Screensaver"]["Linotypo"]["Line Fill Type"] != null ? (tryGetEtaoinType() ? LinotypoSettings.LinotypoEtaoinType : LinotypoSettings.FillType.EtaoinPattern) : LinotypoSettings.FillType.EtaoinPattern;
            LinotypoSettings.LinotypoMissPossibility = int.TryParse((string)ConfigToken["Screensaver"]["Linotypo"]["Probability of miss in percent"], out _) ? (int)ConfigToken["Screensaver"]["Linotypo"]["Probability of miss in percent"] : 10;
            LinotypoSettings.LinotypoTextColor = new Color((string)ConfigToken["Screensaver"]["Linotypo"]["Text color"]).PlainSequence;

            // > Typewriter
            TypewriterSettings.TypewriterDelay = int.TryParse((string)ConfigToken["Screensaver"]["Typewriter"]["Delay in Milliseconds"], out _) ? (int)ConfigToken["Screensaver"]["Typewriter"]["Delay in Milliseconds"] : 50;
            TypewriterSettings.TypewriterNewScreenDelay = int.TryParse((string)ConfigToken["Screensaver"]["Typewriter"]["New Screen Delay in Milliseconds"], out _) ? (int)ConfigToken["Screensaver"]["Typewriter"]["New Screen Delay in Milliseconds"] : 3000;
            TypewriterSettings.TypewriterWrite = (string)ConfigToken["Screensaver"]["Typewriter"]["Text shown"] ?? "Kernel Simulator";
            TypewriterSettings.TypewriterWritingSpeedMin = int.TryParse((string)ConfigToken["Screensaver"]["Typewriter"]["Minimum writing speed in WPM"], out _) ? (int)ConfigToken["Screensaver"]["Typewriter"]["Minimum writing speed in WPM"] : 50;
            TypewriterSettings.TypewriterWritingSpeedMax = int.TryParse((string)ConfigToken["Screensaver"]["Typewriter"]["Maximum writing speed in WPM"], out _) ? (int)ConfigToken["Screensaver"]["Typewriter"]["Maximum writing speed in WPM"] : 80;
            TypewriterSettings.TypewriterTextColor = new Color((string)ConfigToken["Screensaver"]["Typewriter"]["Text color"]).PlainSequence;

            // > FlashColor
            FlashColorSettings.FlashColor255Colors = (bool)ConfigToken["Screensaver"]["FlashColor"]["Activate 255 colors"];
            FlashColorSettings.FlashColorTrueColor = (bool)ConfigToken["Screensaver"]["FlashColor"]["Activate true colors"];
            FlashColorSettings.FlashColorDelay = int.TryParse((string)ConfigToken["Screensaver"]["FlashColor"]["Delay in Milliseconds"], out _) ? (int)ConfigToken["Screensaver"]["FlashColor"]["Delay in Milliseconds"] : 1;
            FlashColorSettings.FlashColorBackgroundColor = new Color((string)ConfigToken["Screensaver"]["FlashColor"]["Background color"]).PlainSequence;
            FlashColorSettings.FlashColorMinimumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["FlashColor"]["Minimum red color level"], out _) ? (int)ConfigToken["Screensaver"]["FlashColor"]["Minimum red color level"] : 0;
            FlashColorSettings.FlashColorMinimumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["FlashColor"]["Minimum green color level"], out _) ? (int)ConfigToken["Screensaver"]["FlashColor"]["Minimum green color level"] : 0;
            FlashColorSettings.FlashColorMinimumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["FlashColor"]["Minimum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["FlashColor"]["Minimum blue color level"] : 0;
            FlashColorSettings.FlashColorMinimumColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["FlashColor"]["Minimum color level"], out _) ? (int)ConfigToken["Screensaver"]["FlashColor"]["Minimum color level"] : 0;
            FlashColorSettings.FlashColorMaximumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["FlashColor"]["Maximum red color level"], out _) ? (int)ConfigToken["Screensaver"]["FlashColor"]["Maximum red color level"] : 255;
            FlashColorSettings.FlashColorMaximumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["FlashColor"]["Maximum green color level"], out _) ? (int)ConfigToken["Screensaver"]["FlashColor"]["Maximum green color level"] : 255;
            FlashColorSettings.FlashColorMaximumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["FlashColor"]["Maximum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["FlashColor"]["Maximum blue color level"] : 255;
            FlashColorSettings.FlashColorMaximumColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["FlashColor"]["Maximum color level"], out _) ? (int)ConfigToken["Screensaver"]["FlashColor"]["Maximum color level"] : 255;

            // > SpotWrite
            SpotWriteSettings.SpotWriteDelay = int.TryParse((string)ConfigToken["Screensaver"]["SpotWrite"]["Delay in Milliseconds"], out _) ? (int)ConfigToken["Screensaver"]["SpotWrite"]["Delay in Milliseconds"] : 50;
            SpotWriteSettings.SpotWriteNewScreenDelay = int.TryParse((string)ConfigToken["Screensaver"]["SpotWrite"]["New Screen Delay in Milliseconds"], out _) ? (int)ConfigToken["Screensaver"]["SpotWrite"]["New Screen Delay in Milliseconds"] : 3000;
            SpotWriteSettings.SpotWriteWrite = (string)ConfigToken["Screensaver"]["SpotWrite"]["Text shown"] ?? "Kernel Simulator";
            SpotWriteSettings.SpotWriteTextColor = new Color((string)ConfigToken["Screensaver"]["SpotWrite"]["Text color"]).PlainSequence;

            // > Ramp
            RampSettings.Ramp255Colors = (bool)ConfigToken["Screensaver"]["Ramp"]["Activate 255 colors"];
            RampSettings.RampTrueColor = (bool)ConfigToken["Screensaver"]["Ramp"]["Activate true colors"];
            RampSettings.RampDelay = int.TryParse((string)ConfigToken["Screensaver"]["Ramp"]["Delay in Milliseconds"], out _) ? (int)ConfigToken["Screensaver"]["Ramp"]["Delay in Milliseconds"] : 20;
            RampSettings.RampNextRampDelay = int.TryParse((string)ConfigToken["Screensaver"]["Ramp"]["Next ramp interval"], out _) ? (int)ConfigToken["Screensaver"]["Ramp"]["Next ramp interval"] : 250;
            RampSettings.RampUpperLeftCornerChar = (string)(ConfigToken["Screensaver"]["Ramp"]["Upper left corner character for ramp bar"] ?? "╔");
            RampSettings.RampUpperRightCornerChar = (string)(ConfigToken["Screensaver"]["Ramp"]["Upper right corner character for ramp bar"] ?? "╗");
            RampSettings.RampLowerLeftCornerChar = (string)(ConfigToken["Screensaver"]["Ramp"]["Lower left corner character for ramp bar"] ?? "╚");
            RampSettings.RampLowerRightCornerChar = (string)(ConfigToken["Screensaver"]["Ramp"]["Lower right corner character for ramp bar"] ?? "╝");
            RampSettings.RampUpperFrameChar = (string)(ConfigToken["Screensaver"]["Ramp"]["Upper frame character for ramp bar"] ?? "═");
            RampSettings.RampLowerFrameChar = (string)(ConfigToken["Screensaver"]["Ramp"]["Lower frame character for ramp bar"] ?? "═");
            RampSettings.RampLeftFrameChar = (string)(ConfigToken["Screensaver"]["Ramp"]["Left frame character for ramp bar"] ?? "║");
            RampSettings.RampRightFrameChar = (string)(ConfigToken["Screensaver"]["Ramp"]["Right frame character for ramp bar"] ?? "║");
            RampSettings.RampMinimumRedColorLevelStart = int.TryParse((string)ConfigToken["Screensaver"]["Ramp"]["Minimum red color level for start color"], out _) ? (int)ConfigToken["Screensaver"]["Ramp"]["Minimum red color level for start color"] : 0;
            RampSettings.RampMinimumGreenColorLevelStart = int.TryParse((string)ConfigToken["Screensaver"]["Ramp"]["Minimum green color level for start color"], out _) ? (int)ConfigToken["Screensaver"]["Ramp"]["Minimum green color level for start color"] : 0;
            RampSettings.RampMinimumBlueColorLevelStart = int.TryParse((string)ConfigToken["Screensaver"]["Ramp"]["Minimum blue color level for start color"], out _) ? (int)ConfigToken["Screensaver"]["Ramp"]["Minimum blue color level for start color"] : 0;
            RampSettings.RampMinimumColorLevelStart = int.TryParse((string)ConfigToken["Screensaver"]["Ramp"]["Minimum color level for start color"], out _) ? (int)ConfigToken["Screensaver"]["Ramp"]["Minimum color level for start color"] : 0;
            RampSettings.RampMaximumRedColorLevelStart = int.TryParse((string)ConfigToken["Screensaver"]["Ramp"]["Maximum red color level for start color"], out _) ? (int)ConfigToken["Screensaver"]["Ramp"]["Maximum red color level for start color"] : 255;
            RampSettings.RampMaximumGreenColorLevelStart = int.TryParse((string)ConfigToken["Screensaver"]["Ramp"]["Maximum green color level for start color"], out _) ? (int)ConfigToken["Screensaver"]["Ramp"]["Maximum green color level for start color"] : 255;
            RampSettings.RampMaximumBlueColorLevelStart = int.TryParse((string)ConfigToken["Screensaver"]["Ramp"]["Maximum blue color level for start color"], out _) ? (int)ConfigToken["Screensaver"]["Ramp"]["Maximum blue color level for start color"] : 255;
            RampSettings.RampMaximumColorLevelStart = int.TryParse((string)ConfigToken["Screensaver"]["Ramp"]["Maximum color level for start color"], out _) ? (int)ConfigToken["Screensaver"]["Ramp"]["Maximum color level for start color"] : 255;
            RampSettings.RampMinimumRedColorLevelEnd = int.TryParse((string)ConfigToken["Screensaver"]["Ramp"]["Minimum red color level for end color"], out _) ? (int)ConfigToken["Screensaver"]["Ramp"]["Minimum red color level for end color"] : 0;
            RampSettings.RampMinimumGreenColorLevelEnd = int.TryParse((string)ConfigToken["Screensaver"]["Ramp"]["Minimum green color level for end color"], out _) ? (int)ConfigToken["Screensaver"]["Ramp"]["Minimum green color level for end color"] : 0;
            RampSettings.RampMinimumBlueColorLevelEnd = int.TryParse((string)ConfigToken["Screensaver"]["Ramp"]["Minimum blue color level for end color"], out _) ? (int)ConfigToken["Screensaver"]["Ramp"]["Minimum blue color level for end color"] : 0;
            RampSettings.RampMinimumColorLevelEnd = int.TryParse((string)ConfigToken["Screensaver"]["Ramp"]["Minimum color level for end color"], out _) ? (int)ConfigToken["Screensaver"]["Ramp"]["Minimum color level for end color"] : 0;
            RampSettings.RampMaximumRedColorLevelEnd = int.TryParse((string)ConfigToken["Screensaver"]["Ramp"]["Maximum red color level for end color"], out _) ? (int)ConfigToken["Screensaver"]["Ramp"]["Maximum red color level for end color"] : 255;
            RampSettings.RampMaximumGreenColorLevelEnd = int.TryParse((string)ConfigToken["Screensaver"]["Ramp"]["Maximum green color level for end color"], out _) ? (int)ConfigToken["Screensaver"]["Ramp"]["Maximum green color level for end color"] : 255;
            RampSettings.RampMaximumBlueColorLevelEnd = int.TryParse((string)ConfigToken["Screensaver"]["Ramp"]["Maximum blue color level for end color"], out _) ? (int)ConfigToken["Screensaver"]["Ramp"]["Maximum blue color level for end color"] : 255;
            RampSettings.RampMaximumColorLevelEnd = int.TryParse((string)ConfigToken["Screensaver"]["Ramp"]["Maximum color level for end color"], out _) ? (int)ConfigToken["Screensaver"]["Ramp"]["Maximum color level for end color"] : 255;
            RampSettings.RampUpperLeftCornerColor = new Color((string)ConfigToken["Screensaver"]["Ramp"]["Upper left corner color for ramp bar"]).PlainSequence;
            RampSettings.RampUpperRightCornerColor = new Color((string)ConfigToken["Screensaver"]["Ramp"]["Upper right corner color for ramp bar"]).PlainSequence;
            RampSettings.RampLowerLeftCornerColor = new Color((string)ConfigToken["Screensaver"]["Ramp"]["Lower left corner color for ramp bar"]).PlainSequence;
            RampSettings.RampLowerRightCornerColor = new Color((string)ConfigToken["Screensaver"]["Ramp"]["Lower right corner color for ramp bar"]).PlainSequence;
            RampSettings.RampUpperFrameColor = new Color((string)ConfigToken["Screensaver"]["Ramp"]["Upper frame color for ramp bar"]).PlainSequence;
            RampSettings.RampLowerFrameColor = new Color((string)ConfigToken["Screensaver"]["Ramp"]["Lower frame color for ramp bar"]).PlainSequence;
            RampSettings.RampLeftFrameColor = new Color((string)ConfigToken["Screensaver"]["Ramp"]["Left frame color for ramp bar"]).PlainSequence;
            RampSettings.RampRightFrameColor = new Color((string)ConfigToken["Screensaver"]["Ramp"]["Right frame color for ramp bar"]).PlainSequence;
            RampSettings.RampUseBorderColors = (bool)ConfigToken["Screensaver"]["Ramp"]["Use border colors for ramp bar"];

            // > StackBox
            StackBoxSettings.StackBox255Colors = (bool)ConfigToken["Screensaver"]["StackBox"]["Activate 255 colors"];
            StackBoxSettings.StackBoxTrueColor = (bool)ConfigToken["Screensaver"]["StackBox"]["Activate true colors"];
            StackBoxSettings.StackBoxDelay = int.TryParse((string)ConfigToken["Screensaver"]["StackBox"]["Delay in Milliseconds"], out _) ? (int)ConfigToken["Screensaver"]["StackBox"]["Delay in Milliseconds"] : 10;
            StackBoxSettings.StackBoxMinimumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["StackBox"]["Minimum red color level"], out _) ? (int)ConfigToken["Screensaver"]["StackBox"]["Minimum red color level"] : 0;
            StackBoxSettings.StackBoxMinimumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["StackBox"]["Minimum green color level"], out _) ? (int)ConfigToken["Screensaver"]["StackBox"]["Minimum green color level"] : 0;
            StackBoxSettings.StackBoxMinimumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["StackBox"]["Minimum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["StackBox"]["Minimum blue color level"] : 0;
            StackBoxSettings.StackBoxMinimumColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["StackBox"]["Minimum color level"], out _) ? (int)ConfigToken["Screensaver"]["StackBox"]["Minimum color level"] : 0;
            StackBoxSettings.StackBoxMaximumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["StackBox"]["Maximum red color level"], out _) ? (int)ConfigToken["Screensaver"]["StackBox"]["Maximum red color level"] : 255;
            StackBoxSettings.StackBoxMaximumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["StackBox"]["Maximum green color level"], out _) ? (int)ConfigToken["Screensaver"]["StackBox"]["Maximum green color level"] : 255;
            StackBoxSettings.StackBoxMaximumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["StackBox"]["Maximum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["StackBox"]["Maximum blue color level"] : 255;
            StackBoxSettings.StackBoxMaximumColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["StackBox"]["Maximum color level"], out _) ? (int)ConfigToken["Screensaver"]["StackBox"]["Maximum color level"] : 255;
            StackBoxSettings.StackBoxFill = (bool)ConfigToken["Screensaver"]["StackBox"]["Fill the boxes"];

            // > Snaker
            SnakerSettings.Snaker255Colors = (bool)ConfigToken["Screensaver"]["Snaker"]["Activate 255 colors"];
            SnakerSettings.SnakerTrueColor = (bool)ConfigToken["Screensaver"]["Snaker"]["Activate true colors"];
            SnakerSettings.SnakerDelay = int.TryParse((string)ConfigToken["Screensaver"]["Snaker"]["Delay in Milliseconds"], out _) ? (int)ConfigToken["Screensaver"]["Snaker"]["Delay in Milliseconds"] : 100;
            SnakerSettings.SnakerStageDelay = int.TryParse((string)ConfigToken["Screensaver"]["Snaker"]["Stage delay in milliseconds"], out _) ? (int)ConfigToken["Screensaver"]["Snaker"]["Stage delay in milliseconds"] : 5000;
            SnakerSettings.SnakerMinimumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Snaker"]["Minimum red color level"], out _) ? (int)ConfigToken["Screensaver"]["Snaker"]["Minimum red color level"] : 0;
            SnakerSettings.SnakerMinimumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Snaker"]["Minimum green color level"], out _) ? (int)ConfigToken["Screensaver"]["Snaker"]["Minimum green color level"] : 0;
            SnakerSettings.SnakerMinimumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Snaker"]["Minimum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["Snaker"]["Minimum blue color level"] : 0;
            SnakerSettings.SnakerMinimumColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Snaker"]["Minimum color level"], out _) ? (int)ConfigToken["Screensaver"]["Snaker"]["Minimum color level"] : 0;
            SnakerSettings.SnakerMaximumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Snaker"]["Maximum red color level"], out _) ? (int)ConfigToken["Screensaver"]["Snaker"]["Maximum red color level"] : 255;
            SnakerSettings.SnakerMaximumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Snaker"]["Maximum green color level"], out _) ? (int)ConfigToken["Screensaver"]["Snaker"]["Maximum green color level"] : 255;
            SnakerSettings.SnakerMaximumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Snaker"]["Maximum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["Snaker"]["Maximum blue color level"] : 255;
            SnakerSettings.SnakerMaximumColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Snaker"]["Maximum color level"], out _) ? (int)ConfigToken["Screensaver"]["Snaker"]["Maximum color level"] : 255;

            // > BarRot
            BarRotSettings.BarRot255Colors = (bool)ConfigToken["Screensaver"]["BarRot"]["Activate 255 colors"];
            BarRotSettings.BarRotTrueColor = (bool)ConfigToken["Screensaver"]["BarRot"]["Activate true colors"];
            BarRotSettings.BarRotDelay = int.TryParse((string)ConfigToken["Screensaver"]["BarRot"]["Delay in Milliseconds"], out _) ? (int)ConfigToken["Screensaver"]["BarRot"]["Delay in Milliseconds"] : 10;
            BarRotSettings.BarRotNextRampDelay = int.TryParse((string)ConfigToken["Screensaver"]["BarRot"]["Next ramp rot interval"], out _) ? (int)ConfigToken["Screensaver"]["BarRot"]["Next ramp rot interval"] : 250;
            BarRotSettings.BarRotUpperLeftCornerChar = (string)(ConfigToken["Screensaver"]["BarRot"]["Upper left corner character for ramp bar"] ?? "╔");
            BarRotSettings.BarRotUpperRightCornerChar = (string)(ConfigToken["Screensaver"]["BarRot"]["Upper right corner character for ramp bar"] ?? "╗");
            BarRotSettings.BarRotLowerLeftCornerChar = (string)(ConfigToken["Screensaver"]["BarRot"]["Lower left corner character for ramp bar"] ?? "╚");
            BarRotSettings.BarRotLowerRightCornerChar = (string)(ConfigToken["Screensaver"]["BarRot"]["Lower right corner character for ramp bar"] ?? "╝");
            BarRotSettings.BarRotUpperFrameChar = (string)(ConfigToken["Screensaver"]["BarRot"]["Upper frame character for ramp bar"] ?? "═");
            BarRotSettings.BarRotLowerFrameChar = (string)(ConfigToken["Screensaver"]["BarRot"]["Lower frame character for ramp bar"] ?? "═");
            BarRotSettings.BarRotLeftFrameChar = (string)(ConfigToken["Screensaver"]["BarRot"]["Left frame character for ramp bar"] ?? "║");
            BarRotSettings.BarRotRightFrameChar = (string)(ConfigToken["Screensaver"]["BarRot"]["Right frame character for ramp bar"] ?? "║");
            BarRotSettings.BarRotMinimumRedColorLevelStart = int.TryParse((string)ConfigToken["Screensaver"]["BarRot"]["Minimum red color level for start color"], out _) ? (int)ConfigToken["Screensaver"]["BarRot"]["Minimum red color level for start color"] : 0;
            BarRotSettings.BarRotMinimumGreenColorLevelStart = int.TryParse((string)ConfigToken["Screensaver"]["BarRot"]["Minimum green color level for start color"], out _) ? (int)ConfigToken["Screensaver"]["BarRot"]["Minimum green color level for start color"] : 0;
            BarRotSettings.BarRotMinimumBlueColorLevelStart = int.TryParse((string)ConfigToken["Screensaver"]["BarRot"]["Minimum blue color level for start color"], out _) ? (int)ConfigToken["Screensaver"]["BarRot"]["Minimum blue color level for start color"] : 0;
            BarRotSettings.BarRotMaximumRedColorLevelStart = int.TryParse((string)ConfigToken["Screensaver"]["BarRot"]["Maximum red color level for start color"], out _) ? (int)ConfigToken["Screensaver"]["BarRot"]["Maximum red color level for start color"] : 255;
            BarRotSettings.BarRotMaximumGreenColorLevelStart = int.TryParse((string)ConfigToken["Screensaver"]["BarRot"]["Maximum green color level for start color"], out _) ? (int)ConfigToken["Screensaver"]["BarRot"]["Maximum green color level for start color"] : 255;
            BarRotSettings.BarRotMaximumBlueColorLevelStart = int.TryParse((string)ConfigToken["Screensaver"]["BarRot"]["Maximum blue color level for start color"], out _) ? (int)ConfigToken["Screensaver"]["BarRot"]["Maximum blue color level for start color"] : 255;
            BarRotSettings.BarRotMinimumRedColorLevelEnd = int.TryParse((string)ConfigToken["Screensaver"]["BarRot"]["Minimum red color level for end color"], out _) ? (int)ConfigToken["Screensaver"]["BarRot"]["Minimum red color level for end color"] : 0;
            BarRotSettings.BarRotMinimumGreenColorLevelEnd = int.TryParse((string)ConfigToken["Screensaver"]["BarRot"]["Minimum green color level for end color"], out _) ? (int)ConfigToken["Screensaver"]["BarRot"]["Minimum green color level for end color"] : 0;
            BarRotSettings.BarRotMinimumBlueColorLevelEnd = int.TryParse((string)ConfigToken["Screensaver"]["BarRot"]["Minimum blue color level for end color"], out _) ? (int)ConfigToken["Screensaver"]["BarRot"]["Minimum blue color level for end color"] : 0;
            BarRotSettings.BarRotMaximumRedColorLevelEnd = int.TryParse((string)ConfigToken["Screensaver"]["BarRot"]["Maximum red color level for end color"], out _) ? (int)ConfigToken["Screensaver"]["BarRot"]["Maximum red color level for end color"] : 255;
            BarRotSettings.BarRotMaximumGreenColorLevelEnd = int.TryParse((string)ConfigToken["Screensaver"]["BarRot"]["Maximum green color level for end color"], out _) ? (int)ConfigToken["Screensaver"]["BarRot"]["Maximum green color level for end color"] : 255;
            BarRotSettings.BarRotMaximumBlueColorLevelEnd = int.TryParse((string)ConfigToken["Screensaver"]["BarRot"]["Maximum blue color level for end color"], out _) ? (int)ConfigToken["Screensaver"]["BarRot"]["Maximum blue color level for end color"] : 255;
            BarRotSettings.BarRotUpperLeftCornerColor = new Color((string)ConfigToken["Screensaver"]["BarRot"]["Upper left corner color for ramp bar"]).PlainSequence;
            BarRotSettings.BarRotUpperRightCornerColor = new Color((string)ConfigToken["Screensaver"]["BarRot"]["Upper right corner color for ramp bar"]).PlainSequence;
            BarRotSettings.BarRotLowerLeftCornerColor = new Color((string)ConfigToken["Screensaver"]["BarRot"]["Lower left corner color for ramp bar"]).PlainSequence;
            BarRotSettings.BarRotLowerRightCornerColor = new Color((string)ConfigToken["Screensaver"]["BarRot"]["Lower right corner color for ramp bar"]).PlainSequence;
            BarRotSettings.BarRotUpperFrameColor = new Color((string)ConfigToken["Screensaver"]["BarRot"]["Upper frame color for ramp bar"]).PlainSequence;
            BarRotSettings.BarRotLowerFrameColor = new Color((string)ConfigToken["Screensaver"]["BarRot"]["Lower frame color for ramp bar"]).PlainSequence;
            BarRotSettings.BarRotLeftFrameColor = new Color((string)ConfigToken["Screensaver"]["BarRot"]["Left frame color for ramp bar"]).PlainSequence;
            BarRotSettings.BarRotRightFrameColor = new Color((string)ConfigToken["Screensaver"]["BarRot"]["Right frame color for ramp bar"]).PlainSequence;
            BarRotSettings.BarRotUseBorderColors = (bool)ConfigToken["Screensaver"]["BarRot"]["Use border colors for ramp bar"];

            // > Fireworks
            FireworksSettings.Fireworks255Colors = (bool)ConfigToken["Screensaver"]["Fireworks"]["Activate 255 colors"];
            FireworksSettings.FireworksTrueColor = (bool)ConfigToken["Screensaver"]["Fireworks"]["Activate true colors"];
            FireworksSettings.FireworksDelay = int.TryParse((string)ConfigToken["Screensaver"]["Fireworks"]["Delay in Milliseconds"], out _) ? (int)ConfigToken["Screensaver"]["Fireworks"]["Delay in Milliseconds"] : 10;
            FireworksSettings.FireworksRadius = int.TryParse((string)ConfigToken["Screensaver"]["Fireworks"]["Firework explosion radius"], out _) ? (int)ConfigToken["Screensaver"]["Fireworks"]["Firework explosion radius"] : 5;
            FireworksSettings.FireworksMinimumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Fireworks"]["Minimum red color level"], out _) ? (int)ConfigToken["Screensaver"]["Fireworks"]["Minimum red color level"] : 0;
            FireworksSettings.FireworksMinimumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Fireworks"]["Minimum green color level"], out _) ? (int)ConfigToken["Screensaver"]["Fireworks"]["Minimum green color level"] : 0;
            FireworksSettings.FireworksMinimumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Fireworks"]["Minimum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["Fireworks"]["Minimum blue color level"] : 0;
            FireworksSettings.FireworksMinimumColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Fireworks"]["Minimum color level"], out _) ? (int)ConfigToken["Screensaver"]["Fireworks"]["Minimum color level"] : 0;
            FireworksSettings.FireworksMaximumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Fireworks"]["Maximum red color level"], out _) ? (int)ConfigToken["Screensaver"]["Fireworks"]["Maximum red color level"] : 255;
            FireworksSettings.FireworksMaximumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Fireworks"]["Maximum green color level"], out _) ? (int)ConfigToken["Screensaver"]["Fireworks"]["Maximum green color level"] : 255;
            FireworksSettings.FireworksMaximumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Fireworks"]["Maximum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["Fireworks"]["Maximum blue color level"] : 255;
            FireworksSettings.FireworksMaximumColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Fireworks"]["Maximum color level"], out _) ? (int)ConfigToken["Screensaver"]["Fireworks"]["Maximum color level"] : 255;

            // > Figlet
            FigletSettings.Figlet255Colors = (bool)ConfigToken["Screensaver"]["Figlet"]["Activate 255 colors"];
            FigletSettings.FigletTrueColor = (bool)ConfigToken["Screensaver"]["Figlet"]["Activate true colors"];
            FigletSettings.FigletDelay = int.TryParse((string)ConfigToken["Screensaver"]["Figlet"]["Delay in Milliseconds"], out _) ? (int)ConfigToken["Screensaver"]["Figlet"]["Delay in Milliseconds"] : 10;
            FigletSettings.FigletText = (string)ConfigToken["Screensaver"]["Figlet"]["Text shown"] ?? "Kernel Simulator";
            FigletSettings.FigletFont = (string)(ConfigToken["Screensaver"]["Figlet"]["Figlet font"] ?? "Small");
            FigletSettings.FigletMinimumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Figlet"]["Minimum red color level"], out _) ? (int)ConfigToken["Screensaver"]["Figlet"]["Minimum red color level"] : 0;
            FigletSettings.FigletMinimumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Figlet"]["Minimum green color level"], out _) ? (int)ConfigToken["Screensaver"]["Figlet"]["Minimum green color level"] : 0;
            FigletSettings.FigletMinimumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Figlet"]["Minimum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["Figlet"]["Minimum blue color level"] : 0;
            FigletSettings.FigletMinimumColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Figlet"]["Minimum color level"], out _) ? (int)ConfigToken["Screensaver"]["Figlet"]["Minimum color level"] : 0;
            FigletSettings.FigletMaximumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Figlet"]["Maximum red color level"], out _) ? (int)ConfigToken["Screensaver"]["Figlet"]["Maximum red color level"] : 255;
            FigletSettings.FigletMaximumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Figlet"]["Maximum green color level"], out _) ? (int)ConfigToken["Screensaver"]["Figlet"]["Maximum green color level"] : 255;
            FigletSettings.FigletMaximumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Figlet"]["Maximum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["Figlet"]["Maximum blue color level"] : 255;
            FigletSettings.FigletMaximumColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Figlet"]["Maximum color level"], out _) ? (int)ConfigToken["Screensaver"]["Figlet"]["Maximum color level"] : 255;

            // > FlashText
            FlashTextSettings.FlashText255Colors = (bool)ConfigToken["Screensaver"]["FlashText"]["Activate 255 colors"];
            FlashTextSettings.FlashTextTrueColor = (bool)ConfigToken["Screensaver"]["FlashText"]["Activate true colors"];
            FlashTextSettings.FlashTextDelay = int.TryParse((string)ConfigToken["Screensaver"]["FlashText"]["Delay in Milliseconds"], out _) ? (int)ConfigToken["Screensaver"]["FlashText"]["Delay in Milliseconds"] : 10;
            FlashTextSettings.FlashTextWrite = (string)ConfigToken["Screensaver"]["FlashText"]["Text shown"] ?? "Kernel Simulator";
            FlashTextSettings.FlashTextBackgroundColor = new Color((string)ConfigToken["Screensaver"]["FlashText"]["Background color"]).PlainSequence;
            FlashTextSettings.FlashTextMinimumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["FlashText"]["Minimum red color level"], out _) ? (int)ConfigToken["Screensaver"]["FlashText"]["Minimum red color level"] : 0;
            FlashTextSettings.FlashTextMinimumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["FlashText"]["Minimum green color level"], out _) ? (int)ConfigToken["Screensaver"]["FlashText"]["Minimum green color level"] : 0;
            FlashTextSettings.FlashTextMinimumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["FlashText"]["Minimum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["FlashText"]["Minimum blue color level"] : 0;
            FlashTextSettings.FlashTextMinimumColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["FlashText"]["Minimum color level"], out _) ? (int)ConfigToken["Screensaver"]["FlashText"]["Minimum color level"] : 0;
            FlashTextSettings.FlashTextMaximumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["FlashText"]["Maximum red color level"], out _) ? (int)ConfigToken["Screensaver"]["FlashText"]["Maximum red color level"] : 255;
            FlashTextSettings.FlashTextMaximumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["FlashText"]["Maximum green color level"], out _) ? (int)ConfigToken["Screensaver"]["FlashText"]["Maximum green color level"] : 255;
            FlashTextSettings.FlashTextMaximumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["FlashText"]["Maximum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["FlashText"]["Maximum blue color level"] : 255;
            FlashTextSettings.FlashTextMaximumColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["FlashText"]["Maximum color level"], out _) ? (int)ConfigToken["Screensaver"]["FlashText"]["Maximum color level"] : 255;

            // > Noise
            NoiseSettings.NoiseNewScreenDelay = int.TryParse((string)ConfigToken["Screensaver"]["Noise"]["New Screen Delay in Milliseconds"], out _) ? (int)ConfigToken["Screensaver"]["Noise"]["New Screen Delay in Milliseconds"] : 5000;
            NoiseSettings.NoiseDensity = int.TryParse((string)ConfigToken["Screensaver"]["Noise"]["Noise density"], out _) ? (int)ConfigToken["Screensaver"]["Noise"]["Noise density"] : 40;

            // > PersonLookup
            PersonLookupSettings.PersonLookupDelay = int.TryParse((string)ConfigToken["Screensaver"]["PersonLookup"]["Delay in Milliseconds"], out _) ? (int)ConfigToken["Screensaver"]["PersonLookup"]["Delay in Milliseconds"] : 75;
            PersonLookupSettings.PersonLookupLookedUpDelay = int.TryParse((string)ConfigToken["Screensaver"]["PersonLookup"]["New Screen Delay in Milliseconds"], out _) ? (int)ConfigToken["Screensaver"]["PersonLookup"]["New Screen Delay in Milliseconds"] : 10000;
            PersonLookupSettings.PersonLookupMinimumNames = int.TryParse((string)ConfigToken["Screensaver"]["PersonLookup"]["Minimum names count"], out _) ? (int)ConfigToken["Screensaver"]["PersonLookup"]["Minimum names count"] : 10;
            PersonLookupSettings.PersonLookupMaximumNames = int.TryParse((string)ConfigToken["Screensaver"]["PersonLookup"]["Maximum names count"], out _) ? (int)ConfigToken["Screensaver"]["PersonLookup"]["Maximum names count"] : 100;
            PersonLookupSettings.PersonLookupMinimumAgeYears = int.TryParse((string)ConfigToken["Screensaver"]["PersonLookup"]["Minimum age years count"], out _) ? (int)ConfigToken["Screensaver"]["PersonLookup"]["Minimum age years count"] : 18;
            PersonLookupSettings.PersonLookupMaximumAgeYears = int.TryParse((string)ConfigToken["Screensaver"]["PersonLookup"]["Maximum age years count"], out _) ? (int)ConfigToken["Screensaver"]["PersonLookup"]["Maximum age years count"] : 100;

            // > DateAndTime
            DateAndTimeSettings.DateAndTime255Colors = (bool)ConfigToken["Screensaver"]["DateAndTime"]["Activate 255 colors"];
            DateAndTimeSettings.DateAndTimeTrueColor = (bool)ConfigToken["Screensaver"]["DateAndTime"]["Activate true colors"];
            DateAndTimeSettings.DateAndTimeDelay = int.TryParse((string)ConfigToken["Screensaver"]["DateAndTime"]["Delay in Milliseconds"], out _) ? (int)ConfigToken["Screensaver"]["DateAndTime"]["Delay in Milliseconds"] : 1000;
            DateAndTimeSettings.DateAndTimeMinimumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["DateAndTime"]["Minimum red color level"], out _) ? (int)ConfigToken["Screensaver"]["DateAndTime"]["Minimum red color level"] : 0;
            DateAndTimeSettings.DateAndTimeMinimumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["DateAndTime"]["Minimum green color level"], out _) ? (int)ConfigToken["Screensaver"]["DateAndTime"]["Minimum green color level"] : 0;
            DateAndTimeSettings.DateAndTimeMinimumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["DateAndTime"]["Minimum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["DateAndTime"]["Minimum blue color level"] : 0;
            DateAndTimeSettings.DateAndTimeMinimumColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["DateAndTime"]["Minimum color level"], out _) ? (int)ConfigToken["Screensaver"]["DateAndTime"]["Minimum color level"] : 0;
            DateAndTimeSettings.DateAndTimeMaximumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["DateAndTime"]["Maximum red color level"], out _) ? (int)ConfigToken["Screensaver"]["DateAndTime"]["Maximum red color level"] : 255;
            DateAndTimeSettings.DateAndTimeMaximumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["DateAndTime"]["Maximum green color level"], out _) ? (int)ConfigToken["Screensaver"]["DateAndTime"]["Maximum green color level"] : 255;
            DateAndTimeSettings.DateAndTimeMaximumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["DateAndTime"]["Maximum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["DateAndTime"]["Maximum blue color level"] : 255;
            DateAndTimeSettings.DateAndTimeMaximumColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["DateAndTime"]["Maximum color level"], out _) ? (int)ConfigToken["Screensaver"]["DateAndTime"]["Maximum color level"] : 255;

            // > Glitch
            GlitchSettings.GlitchDelay = int.TryParse((string)ConfigToken["Screensaver"]["Glitch"]["Delay in Milliseconds"], out _) ? (int)ConfigToken["Screensaver"]["Glitch"]["Delay in Milliseconds"] : 10;
            GlitchSettings.GlitchDensity = int.TryParse((string)ConfigToken["Screensaver"]["Glitch"]["Glitch density"], out _) ? (int)ConfigToken["Screensaver"]["Glitch"]["Glitch density"] : 40;

            // > Indeterminate
            IndeterminateSettings.Indeterminate255Colors = (bool)ConfigToken["Screensaver"]["Indeterminate"]["Activate 255 colors"];
            IndeterminateSettings.IndeterminateTrueColor = (bool)ConfigToken["Screensaver"]["Indeterminate"]["Activate true colors"];
            IndeterminateSettings.IndeterminateDelay = int.TryParse((string)ConfigToken["Screensaver"]["Indeterminate"]["Delay in Milliseconds"], out _) ? (int)ConfigToken["Screensaver"]["Indeterminate"]["Delay in Milliseconds"] : 20;
            IndeterminateSettings.IndeterminateUpperLeftCornerChar = (string)(ConfigToken["Screensaver"]["Indeterminate"]["Upper left corner character for ramp bar"] ?? "╔");
            IndeterminateSettings.IndeterminateUpperRightCornerChar = (string)(ConfigToken["Screensaver"]["Indeterminate"]["Upper right corner character for ramp bar"] ?? "╗");
            IndeterminateSettings.IndeterminateLowerLeftCornerChar = (string)(ConfigToken["Screensaver"]["Indeterminate"]["Lower left corner character for ramp bar"] ?? "╚");
            IndeterminateSettings.IndeterminateLowerRightCornerChar = (string)(ConfigToken["Screensaver"]["Indeterminate"]["Lower right corner character for ramp bar"] ?? "╝");
            IndeterminateSettings.IndeterminateUpperFrameChar = (string)(ConfigToken["Screensaver"]["Indeterminate"]["Upper frame character for ramp bar"] ?? "═");
            IndeterminateSettings.IndeterminateLowerFrameChar = (string)(ConfigToken["Screensaver"]["Indeterminate"]["Lower frame character for ramp bar"] ?? "═");
            IndeterminateSettings.IndeterminateLeftFrameChar = (string)(ConfigToken["Screensaver"]["Indeterminate"]["Left frame character for ramp bar"] ?? "║");
            IndeterminateSettings.IndeterminateRightFrameChar = (string)(ConfigToken["Screensaver"]["Indeterminate"]["Right frame character for ramp bar"] ?? "║");
            IndeterminateSettings.IndeterminateMaximumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Indeterminate"]["Minimum red color level"], out _) ? (int)ConfigToken["Screensaver"]["Indeterminate"]["Minimum red color level"] : 0;
            IndeterminateSettings.IndeterminateMinimumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Indeterminate"]["Minimum green color level"], out _) ? (int)ConfigToken["Screensaver"]["Indeterminate"]["Minimum green color level"] : 0;
            IndeterminateSettings.IndeterminateMinimumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Indeterminate"]["Minimum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["Indeterminate"]["Minimum blue color level"] : 0;
            IndeterminateSettings.IndeterminateMinimumColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Indeterminate"]["Minimum color level"], out _) ? (int)ConfigToken["Screensaver"]["Indeterminate"]["Minimum color level"] : 0;
            IndeterminateSettings.IndeterminateMaximumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Indeterminate"]["Maximum red color level"], out _) ? (int)ConfigToken["Screensaver"]["Indeterminate"]["Maximum red color level"] : 255;
            IndeterminateSettings.IndeterminateMaximumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Indeterminate"]["Maximum green color level"], out _) ? (int)ConfigToken["Screensaver"]["Indeterminate"]["Maximum green color level"] : 255;
            IndeterminateSettings.IndeterminateMaximumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Indeterminate"]["Maximum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["Indeterminate"]["Maximum blue color level"] : 255;
            IndeterminateSettings.IndeterminateMaximumColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Indeterminate"]["Maximum color level"], out _) ? (int)ConfigToken["Screensaver"]["Indeterminate"]["Maximum color level"] : 255;
            IndeterminateSettings.IndeterminateUpperLeftCornerColor = new Color((string)ConfigToken["Screensaver"]["Indeterminate"]["Upper left corner color for ramp bar"]).PlainSequence;
            IndeterminateSettings.IndeterminateUpperRightCornerColor = new Color((string)ConfigToken["Screensaver"]["Indeterminate"]["Upper right corner color for ramp bar"]).PlainSequence;
            IndeterminateSettings.IndeterminateLowerLeftCornerColor = new Color((string)ConfigToken["Screensaver"]["Indeterminate"]["Lower left corner color for ramp bar"]).PlainSequence;
            IndeterminateSettings.IndeterminateLowerRightCornerColor = new Color((string)ConfigToken["Screensaver"]["Indeterminate"]["Lower right corner color for ramp bar"]).PlainSequence;
            IndeterminateSettings.IndeterminateUpperFrameColor = new Color((string)ConfigToken["Screensaver"]["Indeterminate"]["Upper frame color for ramp bar"]).PlainSequence;
            IndeterminateSettings.IndeterminateLowerFrameColor = new Color((string)ConfigToken["Screensaver"]["Indeterminate"]["Lower frame color for ramp bar"]).PlainSequence;
            IndeterminateSettings.IndeterminateLeftFrameColor = new Color((string)ConfigToken["Screensaver"]["Indeterminate"]["Left frame color for ramp bar"]).PlainSequence;
            IndeterminateSettings.IndeterminateRightFrameColor = new Color((string)ConfigToken["Screensaver"]["Indeterminate"]["Right frame color for ramp bar"]).PlainSequence;
            IndeterminateSettings.IndeterminateUseBorderColors = (bool)ConfigToken["Screensaver"]["Indeterminate"]["Use border colors for ramp bar"];

            // > Pulse
            PulseSettings.PulseDelay = int.TryParse((string)ConfigToken["Screensaver"]["Pulse"]["Delay in Milliseconds"], out _) ? (int)ConfigToken["Screensaver"]["Pulse"]["Delay in Milliseconds"] : 50;
            PulseSettings.PulseMaxSteps = int.TryParse((string)ConfigToken["Screensaver"]["Pulse"]["Max Fade Steps"], out _) ? (int)ConfigToken["Screensaver"]["Pulse"]["Max Fade Steps"] : 25;
            PulseSettings.PulseMinimumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Pulse"]["Minimum red color level"], out _) ? (int)ConfigToken["Screensaver"]["Pulse"]["Minimum red color level"] : 0;
            PulseSettings.PulseMinimumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Pulse"]["Minimum green color level"], out _) ? (int)ConfigToken["Screensaver"]["Pulse"]["Minimum green color level"] : 0;
            PulseSettings.PulseMinimumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Pulse"]["Minimum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["Pulse"]["Minimum blue color level"] : 0;
            PulseSettings.PulseMaximumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Pulse"]["Maximum red color level"], out _) ? (int)ConfigToken["Screensaver"]["Pulse"]["Maximum red color level"] : 255;
            PulseSettings.PulseMaximumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Pulse"]["Maximum green color level"], out _) ? (int)ConfigToken["Screensaver"]["Pulse"]["Maximum green color level"] : 255;
            PulseSettings.PulseMaximumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Pulse"]["Maximum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["Pulse"]["Maximum blue color level"] : 255;

            // > BeatPulse
            BeatPulseSettings.BeatPulse255Colors = (bool)ConfigToken["Screensaver"]["BeatPulse"]["Activate 255 colors"];
            BeatPulseSettings.BeatPulseTrueColor = (bool)ConfigToken["Screensaver"]["BeatPulse"]["Activate true colors"];
            BeatPulseSettings.BeatPulseCycleColors = (bool)ConfigToken["Screensaver"]["BeatPulse"]["Cycle colors"];
            BeatPulseSettings.BeatPulseBeatColor = (string)(ConfigToken["Screensaver"]["BeatPulse"]["Beat color"] ?? 17);
            BeatPulseSettings.BeatPulseDelay = int.TryParse((string)ConfigToken["Screensaver"]["BeatPulse"]["Delay in Beats Per Minute"], out _) ? (int)ConfigToken["Screensaver"]["BeatPulse"]["Delay in Beats Per Minute"] : 120;
            BeatPulseSettings.BeatPulseMaxSteps = int.TryParse((string)ConfigToken["Screensaver"]["BeatPulse"]["Max Fade Steps"], out _) ? (int)ConfigToken["Screensaver"]["BeatPulse"]["Max Fade Steps"] : 25;
            BeatPulseSettings.BeatPulseMinimumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["BeatPulse"]["Delay in Milliseconds"], out _) ? (int)ConfigToken["Screensaver"]["BeatPulse"]["Delay in Milliseconds"] : 1000;
            BeatPulseSettings.BeatPulseMinimumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["BeatPulse"]["Minimum red color level"], out _) ? (int)ConfigToken["Screensaver"]["BeatPulse"]["Minimum red color level"] : 0;
            BeatPulseSettings.BeatPulseMinimumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["BeatPulse"]["Minimum green color level"], out _) ? (int)ConfigToken["Screensaver"]["BeatPulse"]["Minimum green color level"] : 0;
            BeatPulseSettings.BeatPulseMinimumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["BeatPulse"]["Minimum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["BeatPulse"]["Minimum blue color level"] : 0;
            BeatPulseSettings.BeatPulseMinimumColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["BeatPulse"]["Minimum color level"], out _) ? (int)ConfigToken["Screensaver"]["BeatPulse"]["Minimum color level"] : 0;
            BeatPulseSettings.BeatPulseMaximumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["BeatPulse"]["Maximum red color level"], out _) ? (int)ConfigToken["Screensaver"]["BeatPulse"]["Maximum red color level"] : 255;
            BeatPulseSettings.BeatPulseMaximumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["BeatPulse"]["Maximum green color level"], out _) ? (int)ConfigToken["Screensaver"]["BeatPulse"]["Maximum green color level"] : 255;
            BeatPulseSettings.BeatPulseMaximumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["BeatPulse"]["Maximum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["BeatPulse"]["Maximum blue color level"] : 255;
            BeatPulseSettings.BeatPulseMaximumColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["BeatPulse"]["Maximum color level"], out _) ? (int)ConfigToken["Screensaver"]["BeatPulse"]["Maximum color level"] : 255;

            // > EdgePulse
            EdgePulseSettings.EdgePulseDelay = int.TryParse((string)ConfigToken["Screensaver"]["EdgePulse"]["Delay in Milliseconds"], out _) ? (int)ConfigToken["Screensaver"]["EdgePulse"]["Delay in Milliseconds"] : 50;
            EdgePulseSettings.EdgePulseMaxSteps = int.TryParse((string)ConfigToken["Screensaver"]["EdgePulse"]["Max Fade Steps"], out _) ? (int)ConfigToken["Screensaver"]["EdgePulse"]["Max Fade Steps"] : 25;
            EdgePulseSettings.EdgePulseMinimumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["EdgePulse"]["Minimum red color level"], out _) ? (int)ConfigToken["Screensaver"]["EdgePulse"]["Minimum red color level"] : 0;
            EdgePulseSettings.EdgePulseMinimumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["EdgePulse"]["Minimum green color level"], out _) ? (int)ConfigToken["Screensaver"]["EdgePulse"]["Minimum green color level"] : 0;
            EdgePulseSettings.EdgePulseMinimumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["EdgePulse"]["Minimum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["EdgePulse"]["Minimum blue color level"] : 0;
            EdgePulseSettings.EdgePulseMaximumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["EdgePulse"]["Maximum red color level"], out _) ? (int)ConfigToken["Screensaver"]["EdgePulse"]["Maximum red color level"] : 255;
            EdgePulseSettings.EdgePulseMaximumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["EdgePulse"]["Maximum green color level"], out _) ? (int)ConfigToken["Screensaver"]["EdgePulse"]["Maximum green color level"] : 255;
            EdgePulseSettings.EdgePulseMaximumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["EdgePulse"]["Maximum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["EdgePulse"]["Maximum blue color level"] : 255;

            // > BeatEdgePulse
            BeatEdgePulseSettings.BeatEdgePulse255Colors = (bool)ConfigToken["Screensaver"]["BeatEdgePulse"]["Activate 255 colors"];
            BeatEdgePulseSettings.BeatEdgePulseTrueColor = (bool)ConfigToken["Screensaver"]["BeatEdgePulse"]["Activate true colors"];
            BeatEdgePulseSettings.BeatEdgePulseCycleColors = (bool)ConfigToken["Screensaver"]["BeatEdgePulse"]["Cycle colors"];
            BeatEdgePulseSettings.BeatEdgePulseBeatColor = (string)(ConfigToken["Screensaver"]["BeatEdgePulse"]["Beat color"] ?? 17);
            BeatEdgePulseSettings.BeatEdgePulseDelay = int.TryParse((string)ConfigToken["Screensaver"]["BeatEdgePulse"]["Delay in Beats Per Minute"], out _) ? (int)ConfigToken["Screensaver"]["BeatEdgePulse"]["Delay in Beats Per Minute"] : 120;
            BeatEdgePulseSettings.BeatEdgePulseMaxSteps = int.TryParse((string)ConfigToken["Screensaver"]["BeatEdgePulse"]["Max Fade Steps"], out _) ? (int)ConfigToken["Screensaver"]["BeatEdgePulse"]["Max Fade Steps"] : 25;
            BeatEdgePulseSettings.BeatEdgePulseMinimumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["BeatEdgePulse"]["Delay in Milliseconds"], out _) ? (int)ConfigToken["Screensaver"]["BeatEdgePulse"]["Delay in Milliseconds"] : 1000;
            BeatEdgePulseSettings.BeatEdgePulseMinimumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["BeatEdgePulse"]["Minimum red color level"], out _) ? (int)ConfigToken["Screensaver"]["BeatEdgePulse"]["Minimum red color level"] : 0;
            BeatEdgePulseSettings.BeatEdgePulseMinimumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["BeatEdgePulse"]["Minimum green color level"], out _) ? (int)ConfigToken["Screensaver"]["BeatEdgePulse"]["Minimum green color level"] : 0;
            BeatEdgePulseSettings.BeatEdgePulseMinimumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["BeatEdgePulse"]["Minimum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["BeatEdgePulse"]["Minimum blue color level"] : 0;
            BeatEdgePulseSettings.BeatEdgePulseMinimumColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["BeatEdgePulse"]["Minimum color level"], out _) ? (int)ConfigToken["Screensaver"]["BeatEdgePulse"]["Minimum color level"] : 0;
            BeatEdgePulseSettings.BeatEdgePulseMaximumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["BeatEdgePulse"]["Maximum red color level"], out _) ? (int)ConfigToken["Screensaver"]["BeatEdgePulse"]["Maximum red color level"] : 255;
            BeatEdgePulseSettings.BeatEdgePulseMaximumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["BeatEdgePulse"]["Maximum green color level"], out _) ? (int)ConfigToken["Screensaver"]["BeatEdgePulse"]["Maximum green color level"] : 255;
            BeatEdgePulseSettings.BeatEdgePulseMaximumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["BeatEdgePulse"]["Maximum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["BeatEdgePulse"]["Maximum blue color level"] : 255;
            BeatEdgePulseSettings.BeatEdgePulseMaximumColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["BeatEdgePulse"]["Maximum color level"], out _) ? (int)ConfigToken["Screensaver"]["BeatEdgePulse"]["Maximum color level"] : 255;

            // > GradientRot
            GradientRotSettings.GradientRotDelay = int.TryParse((string)ConfigToken["Screensaver"]["GradientRot"]["Delay in Milliseconds"], out _) ? (int)ConfigToken["Screensaver"]["GradientRot"]["Delay in Milliseconds"] : 10;
            GradientRotSettings.GradientRotNextRampDelay = int.TryParse((string)ConfigToken["Screensaver"]["GradientRot"]["Next ramp rot interval"], out _) ? (int)ConfigToken["Screensaver"]["GradientRot"]["Next ramp rot interval"] : 250;
            GradientRotSettings.GradientRotMinimumRedColorLevelStart = int.TryParse((string)ConfigToken["Screensaver"]["GradientRot"]["Minimum red color level for start color"], out _) ? (int)ConfigToken["Screensaver"]["GradientRot"]["Minimum red color level for start color"] : 0;
            GradientRotSettings.GradientRotMinimumGreenColorLevelStart = int.TryParse((string)ConfigToken["Screensaver"]["GradientRot"]["Minimum green color level for start color"], out _) ? (int)ConfigToken["Screensaver"]["GradientRot"]["Minimum green color level for start color"] : 0;
            GradientRotSettings.GradientRotMinimumBlueColorLevelStart = int.TryParse((string)ConfigToken["Screensaver"]["GradientRot"]["Minimum blue color level for start color"], out _) ? (int)ConfigToken["Screensaver"]["GradientRot"]["Minimum blue color level for start color"] : 0;
            GradientRotSettings.GradientRotMaximumRedColorLevelStart = int.TryParse((string)ConfigToken["Screensaver"]["GradientRot"]["Maximum red color level for start color"], out _) ? (int)ConfigToken["Screensaver"]["GradientRot"]["Maximum red color level for start color"] : 255;
            GradientRotSettings.GradientRotMaximumGreenColorLevelStart = int.TryParse((string)ConfigToken["Screensaver"]["GradientRot"]["Maximum green color level for start color"], out _) ? (int)ConfigToken["Screensaver"]["GradientRot"]["Maximum green color level for start color"] : 255;
            GradientRotSettings.GradientRotMaximumBlueColorLevelStart = int.TryParse((string)ConfigToken["Screensaver"]["GradientRot"]["Maximum blue color level for start color"], out _) ? (int)ConfigToken["Screensaver"]["GradientRot"]["Maximum blue color level for start color"] : 255;
            GradientRotSettings.GradientRotMinimumRedColorLevelEnd = int.TryParse((string)ConfigToken["Screensaver"]["GradientRot"]["Minimum red color level for end color"], out _) ? (int)ConfigToken["Screensaver"]["GradientRot"]["Minimum red color level for end color"] : 0;
            GradientRotSettings.GradientRotMinimumGreenColorLevelEnd = int.TryParse((string)ConfigToken["Screensaver"]["GradientRot"]["Minimum green color level for end color"], out _) ? (int)ConfigToken["Screensaver"]["GradientRot"]["Minimum green color level for end color"] : 0;
            GradientRotSettings.GradientRotMinimumBlueColorLevelEnd = int.TryParse((string)ConfigToken["Screensaver"]["GradientRot"]["Minimum blue color level for end color"], out _) ? (int)ConfigToken["Screensaver"]["GradientRot"]["Minimum blue color level for end color"] : 0;
            GradientRotSettings.GradientRotMaximumRedColorLevelEnd = int.TryParse((string)ConfigToken["Screensaver"]["GradientRot"]["Maximum red color level for end color"], out _) ? (int)ConfigToken["Screensaver"]["GradientRot"]["Maximum red color level for end color"] : 255;
            GradientRotSettings.GradientRotMaximumGreenColorLevelEnd = int.TryParse((string)ConfigToken["Screensaver"]["GradientRot"]["Maximum green color level for end color"], out _) ? (int)ConfigToken["Screensaver"]["GradientRot"]["Maximum green color level for end color"] : 255;
            GradientRotSettings.GradientRotMaximumBlueColorLevelEnd = int.TryParse((string)ConfigToken["Screensaver"]["GradientRot"]["Maximum blue color level for end color"], out _) ? (int)ConfigToken["Screensaver"]["GradientRot"]["Maximum blue color level for end color"] : 255;

            // > Gradient
            GradientSettings.GradientNextRampDelay = int.TryParse((string)ConfigToken["Screensaver"]["Gradient"]["Next ramp rot interval"], out _) ? (int)ConfigToken["Screensaver"]["Gradient"]["Next ramp rot interval"] : 250;
            GradientSettings.GradientMinimumRedColorLevelStart = int.TryParse((string)ConfigToken["Screensaver"]["Gradient"]["Minimum red color level for start color"], out _) ? (int)ConfigToken["Screensaver"]["Gradient"]["Minimum red color level for start color"] : 0;
            GradientSettings.GradientMinimumGreenColorLevelStart = int.TryParse((string)ConfigToken["Screensaver"]["Gradient"]["Minimum green color level for start color"], out _) ? (int)ConfigToken["Screensaver"]["Gradient"]["Minimum green color level for start color"] : 0;
            GradientSettings.GradientMinimumBlueColorLevelStart = int.TryParse((string)ConfigToken["Screensaver"]["Gradient"]["Minimum blue color level for start color"], out _) ? (int)ConfigToken["Screensaver"]["Gradient"]["Minimum blue color level for start color"] : 0;
            GradientSettings.GradientMaximumRedColorLevelStart = int.TryParse((string)ConfigToken["Screensaver"]["Gradient"]["Maximum red color level for start color"], out _) ? (int)ConfigToken["Screensaver"]["Gradient"]["Maximum red color level for start color"] : 255;
            GradientSettings.GradientMaximumGreenColorLevelStart = int.TryParse((string)ConfigToken["Screensaver"]["Gradient"]["Maximum green color level for start color"], out _) ? (int)ConfigToken["Screensaver"]["Gradient"]["Maximum green color level for start color"] : 255;
            GradientSettings.GradientMaximumBlueColorLevelStart = int.TryParse((string)ConfigToken["Screensaver"]["Gradient"]["Maximum blue color level for start color"], out _) ? (int)ConfigToken["Screensaver"]["Gradient"]["Maximum blue color level for start color"] : 255;
            GradientSettings.GradientMinimumRedColorLevelEnd = int.TryParse((string)ConfigToken["Screensaver"]["Gradient"]["Minimum red color level for end color"], out _) ? (int)ConfigToken["Screensaver"]["Gradient"]["Minimum red color level for end color"] : 0;
            GradientSettings.GradientMinimumGreenColorLevelEnd = int.TryParse((string)ConfigToken["Screensaver"]["Gradient"]["Minimum green color level for end color"], out _) ? (int)ConfigToken["Screensaver"]["Gradient"]["Minimum green color level for end color"] : 0;
            GradientSettings.GradientMinimumBlueColorLevelEnd = int.TryParse((string)ConfigToken["Screensaver"]["Gradient"]["Minimum blue color level for end color"], out _) ? (int)ConfigToken["Screensaver"]["Gradient"]["Minimum blue color level for end color"] : 0;
            GradientSettings.GradientMaximumRedColorLevelEnd = int.TryParse((string)ConfigToken["Screensaver"]["Gradient"]["Maximum red color level for end color"], out _) ? (int)ConfigToken["Screensaver"]["Gradient"]["Maximum red color level for end color"] : 255;
            GradientSettings.GradientMaximumGreenColorLevelEnd = int.TryParse((string)ConfigToken["Screensaver"]["Gradient"]["Maximum green color level for end color"], out _) ? (int)ConfigToken["Screensaver"]["Gradient"]["Maximum green color level for end color"] : 255;
            GradientSettings.GradientMaximumBlueColorLevelEnd = int.TryParse((string)ConfigToken["Screensaver"]["Gradient"]["Maximum blue color level for end color"], out _) ? (int)ConfigToken["Screensaver"]["Gradient"]["Maximum blue color level for end color"] : 255;

            // > Lightspeed
            LightspeedSettings.LightspeedCycleColors = (bool)ConfigToken["Screensaver"]["Lightspeed"]["Cycle colors"];
            LightspeedSettings.LightspeedMinimumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Lightspeed"]["Minimum red color level"], out _) ? (int)ConfigToken["Screensaver"]["Lightspeed"]["Minimum red color level"] : 0;
            LightspeedSettings.LightspeedMinimumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Lightspeed"]["Minimum green color level"], out _) ? (int)ConfigToken["Screensaver"]["Lightspeed"]["Minimum green color level"] : 0;
            LightspeedSettings.LightspeedMinimumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Lightspeed"]["Minimum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["Lightspeed"]["Minimum blue color level"] : 0;
            LightspeedSettings.LightspeedMinimumColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Lightspeed"]["Minimum color level"], out _) ? (int)ConfigToken["Screensaver"]["Lightspeed"]["Minimum color level"] : 0;
            LightspeedSettings.LightspeedMaximumRedColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Lightspeed"]["Maximum red color level"], out _) ? (int)ConfigToken["Screensaver"]["Lightspeed"]["Maximum red color level"] : 255;
            LightspeedSettings.LightspeedMaximumGreenColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Lightspeed"]["Maximum green color level"], out _) ? (int)ConfigToken["Screensaver"]["Lightspeed"]["Maximum green color level"] : 255;
            LightspeedSettings.LightspeedMaximumBlueColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Lightspeed"]["Maximum blue color level"], out _) ? (int)ConfigToken["Screensaver"]["Lightspeed"]["Maximum blue color level"] : 255;
            LightspeedSettings.LightspeedMaximumColorLevel = int.TryParse((string)ConfigToken["Screensaver"]["Lightspeed"]["Maximum color level"], out _) ? (int)ConfigToken["Screensaver"]["Lightspeed"]["Maximum color level"] : 255;

            // > Starfield
            StarfieldSettings.StarfieldDelay = int.TryParse((string)ConfigToken["Screensaver"]["Starfield"]["Delay in Milliseconds"], out _) ? (int)ConfigToken["Screensaver"]["Starfield"]["Delay in Milliseconds"] : 10;

            // > Siren
            SirenSettings.SirenDelay = int.TryParse((string)ConfigToken["Screensaver"]["Siren"]["Delay in Milliseconds"], out _) ? (int)ConfigToken["Screensaver"]["Siren"]["Delay in Milliseconds"] : 10;
            SirenSettings.SirenStyle = (string)(ConfigToken["Screensaver"]["Siren"]["Siren style"] ?? "Cop");

            // > Spin
            SpinSettings.SpinDelay = int.TryParse((string)ConfigToken["Screensaver"]["Spin"]["Delay in Milliseconds"], out _) ? (int)ConfigToken["Screensaver"]["Spin"]["Delay in Milliseconds"] : 10;

            // Splash Section - Splash-specific settings go below:
            // > Simple
            SplashSettings.SimpleProgressTextLocation = ConfigToken["Splash"]["Simple"]["Progress text location"] != null ? Enum.TryParse((string)ConfigToken["Splash"]["Simple"]["Progress text location"], out SplashSettings.SimpleProgressTextLocation) ? SplashSettings.SimpleProgressTextLocation : TextLocation.Top : TextLocation.Top;

            // > Progress
            SplashSettings.ProgressProgressColor = new Color((ConfigToken["Splash"]["Progress"]["Progress bar color"].ToString()) ?? ColorTools.ProgressColor.PlainSequence).PlainSequence;
            SplashSettings.ProgressProgressTextLocation = ConfigToken["Splash"]["Progress"]["Progress text location"] != null ? Enum.TryParse((string)ConfigToken["Splash"]["Progress"]["Progress text location"], out SplashSettings.ProgressProgressTextLocation) ? SplashSettings.ProgressProgressTextLocation : TextLocation.Top : TextLocation.Top;

            // > PowerLineProgress
            SplashSettings.PowerLineProgressProgressColor = new Color((ConfigToken["Splash"]["PowerLineProgress"]["Progress bar color"].ToString()) ?? ColorTools.ProgressColor.PlainSequence).PlainSequence;
            SplashSettings.PowerLineProgressProgressTextLocation = ConfigToken["Splash"]["PowerLineProgress"]["Progress text location"] != null ? Enum.TryParse((string)ConfigToken["Splash"]["PowerLineProgress"]["Progress text location"], out SplashSettings.PowerLineProgressProgressTextLocation) ? SplashSettings.PowerLineProgressProgressTextLocation : TextLocation.Top : TextLocation.Top;

            // Misc Section
            DebugWriter.WriteDebug(DebugLevel.I, "Parsing misc section...");
            Flags.CornerTimeDate = (bool)ConfigToken["Misc"]["Show Time/Date on Upper Right Corner"];
            Flags.StartScroll = (bool)ConfigToken["Misc"]["Marquee on startup"];
            Flags.LongTimeDate = (bool)ConfigToken["Misc"]["Long Time and Date"];
            Misc.Forecast.Forecast.PreferredUnit = ConfigToken["Misc"]["Preferred Unit for Temperature"] != null ? Enum.TryParse((string)ConfigToken["Misc"]["Preferred Unit for Temperature"], out Misc.Forecast.Forecast.PreferredUnit) ? Misc.Forecast.Forecast.PreferredUnit : UnitMeasurement.Metric : UnitMeasurement.Metric;
            TextEditShellCommon.TextEdit_AutoSaveFlag = (bool)ConfigToken["Misc"]["Enable text editor autosave"];
            TextEditShellCommon.TextEdit_AutoSaveInterval = int.TryParse((string)ConfigToken["Misc"]["Text editor autosave interval"], out _) ? (int)ConfigToken["Misc"]["Text editor autosave interval"] : 60;
            HexEditShellCommon.HexEdit_AutoSaveFlag = (bool)ConfigToken["Misc"]["Enable hex editor autosave"];
            HexEditShellCommon.HexEdit_AutoSaveInterval = int.TryParse((string)ConfigToken["Misc"]["Hex editor autosave interval"], out _) ? (int)ConfigToken["Misc"]["Hex editor autosave interval"] : 60;
            Flags.WrapListOutputs = (bool)ConfigToken["Misc"]["Wrap list outputs"];
            Flags.DrawBorderNotification = (bool)ConfigToken["Misc"]["Draw notification border"];
            ModManager.BlacklistedModsString = (string)ConfigToken["Misc"]["Blacklisted mods"] ?? "";
            Solver.SolverMinimumNumber = int.TryParse((string)ConfigToken["Misc"]["Solver minimum number"], out _) ? (int)ConfigToken["Misc"]["Solver minimum number"] : 1000;
            Solver.SolverMaximumNumber = int.TryParse((string)ConfigToken["Misc"]["Solver maximum number"], out _) ? (int)ConfigToken["Misc"]["Solver maximum number"] : 1000;
            Solver.SolverShowInput = (bool)ConfigToken["Misc"]["Solver show input"];
            Notifications.NotifyUpperLeftCornerChar = (string)(ConfigToken["Misc"]["Upper left corner character for notification border"] ?? "╔");
            Notifications.NotifyUpperRightCornerChar = (string)(ConfigToken["Misc"]["Upper right corner character for notification border"] ?? "╗");
            Notifications.NotifyLowerLeftCornerChar = (string)(ConfigToken["Misc"]["Lower left corner character for notification border"] ?? "╚");
            Notifications.NotifyLowerRightCornerChar = (string)(ConfigToken["Misc"]["Lower right corner character for notification border"] ?? "╝");
            Notifications.NotifyUpperFrameChar = (string)(ConfigToken["Misc"]["Upper frame character for notification border"] ?? "═");
            Notifications.NotifyLowerFrameChar = (string)(ConfigToken["Misc"]["Lower frame character for notification border"] ?? "═");
            Notifications.NotifyLeftFrameChar = (string)(ConfigToken["Misc"]["Left frame character for notification border"] ?? "║");
            Notifications.NotifyRightFrameChar = (string)(ConfigToken["Misc"]["Right frame character for notification border"] ?? "║");
            PageViewer.ManpageInfoStyle = (string)ConfigToken["Misc"]["Manual page information style"] ?? "";
            SpeedPress.SpeedPressCurrentDifficulty = ConfigToken["Misc"]["Default difficulty for SpeedPress"] != null ? Enum.TryParse((string)ConfigToken["Misc"]["Default difficulty for SpeedPress"], out SpeedPress.SpeedPressCurrentDifficulty) ? SpeedPress.SpeedPressCurrentDifficulty : SpeedPress.SpeedPressDifficulty.Medium : SpeedPress.SpeedPressDifficulty.Medium;
            SpeedPress.SpeedPressTimeout = int.TryParse((string)ConfigToken["Misc"]["Keypress timeout for SpeedPress"], out _) ? (int)ConfigToken["Misc"]["Keypress timeout for SpeedPress"] : 3000;
            RSSTools.ShowHeadlineOnLogin = (bool)ConfigToken["Misc"]["Show latest RSS headline on login"];
            RSSTools.RssHeadlineUrl = (string)ConfigToken["Misc"]["RSS headline URL"] ?? "https://www.techrepublic.com/rssfeeds/articles/";
            Flags.SaveEventsRemindersDestructively = (bool)ConfigToken["Misc"]["Save all events and/or reminders destructively"];
            ColorWheelOpen.WheelUpperLeftCornerChar = (string)(ConfigToken["Misc"]["Upper left corner character for RGB color wheel"] ?? "╔");
            ColorWheelOpen.WheelUpperRightCornerChar = (string)(ConfigToken["Misc"]["Upper right corner character for RGB color wheel"] ?? "╗");
            ColorWheelOpen.WheelLowerLeftCornerChar = (string)(ConfigToken["Misc"]["Lower left corner character for RGB color wheel"] ?? "╚");
            ColorWheelOpen.WheelLowerRightCornerChar = (string)(ConfigToken["Misc"]["Lower right corner character for RGB color wheel"] ?? "╝");
            ColorWheelOpen.WheelUpperFrameChar = (string)(ConfigToken["Misc"]["Upper frame character for RGB color wheel"] ?? "═");
            ColorWheelOpen.WheelLowerFrameChar = (string)(ConfigToken["Misc"]["Lower frame character for RGB color wheel"] ?? "═");
            ColorWheelOpen.WheelLeftFrameChar = (string)(ConfigToken["Misc"]["Left frame character for RGB color wheel"] ?? "║");
            ColorWheelOpen.WheelRightFrameChar = (string)(ConfigToken["Misc"]["Right frame character for RGB color wheel"] ?? "║");
            JsonShellCommon.JsonShell_Formatting = ConfigToken["Misc"]["Default JSON formatting for JSON shell"] != null ? Enum.TryParse((string)ConfigToken["Misc"]["Default JSON formatting for JSON shell"], out JsonShellCommon.JsonShell_Formatting) ? JsonShellCommon.JsonShell_Formatting : Formatting.Indented : Formatting.Indented;
            Flags.EnableFigletTimer = (bool)ConfigToken["Misc"]["Enable Figlet for timer"];
            TimerScreen.TimerFigletFont = (string)(ConfigToken["Misc"]["Figlet font for timer"] ?? "Small");
            Flags.ShowCommandsCount = (bool)ConfigToken["Misc"]["Show the commands count on help"];
            Flags.ShowShellCommandsCount = (bool)ConfigToken["Misc"]["Show the shell commands count on help"];
            Flags.ShowModCommandsCount = (bool)ConfigToken["Misc"]["Show the mod commands count on help"];
            Flags.ShowShellAliasesCount = (bool)ConfigToken["Misc"]["Show the aliases count on help"];
            Input.CurrentMask = (string)(ConfigToken["Misc"]["Password mask character"] ?? '*');
            ProgressTools.ProgressUpperLeftCornerChar = (string)(ConfigToken["Misc"]["Upper left corner character for progress bars"] ?? "╔");
            ProgressTools.ProgressUpperRightCornerChar = (string)(ConfigToken["Misc"]["Upper right corner character for progress bars"] ?? "╗");
            ProgressTools.ProgressLowerLeftCornerChar = (string)(ConfigToken["Misc"]["Lower left corner character for progress bars"] ?? "╚");
            ProgressTools.ProgressLowerRightCornerChar = (string)(ConfigToken["Misc"]["Lower right corner character for progress bars"] ?? "╝");
            ProgressTools.ProgressUpperFrameChar = (string)(ConfigToken["Misc"]["Upper frame character for progress bars"] ?? "═");
            ProgressTools.ProgressLowerFrameChar = (string)(ConfigToken["Misc"]["Lower frame character for progress bars"] ?? "═");
            ProgressTools.ProgressLeftFrameChar = (string)(ConfigToken["Misc"]["Left frame character for progress bars"] ?? "║");
            ProgressTools.ProgressRightFrameChar = (string)(ConfigToken["Misc"]["Right frame character for progress bars"] ?? "║");
            LoveHateRespond.LoveOrHateUsersCount = int.TryParse((string)ConfigToken["Misc"]["Users count for love or hate comments"], out _) ? (int)ConfigToken["Misc"]["Users count for love or hate comments"] : 20;
            Flags.InputHistoryEnabled = (bool)ConfigToken["Misc"]["Input history enabled"];
            Flags.InputClipboardEnabled = (bool)ConfigToken["Misc"]["Input clipboard enabled"];
            Flags.InputUndoEnabled = (bool)ConfigToken["Misc"]["Input undo enabled"];
            MeteorShooter.MeteorUsePowerLine = (bool)ConfigToken["Misc"]["Use PowerLine for rendering spaceship"];
            MeteorShooter.MeteorSpeed = int.TryParse((string)ConfigToken["Misc"]["Meteor game speed"], out _) ? (int)ConfigToken["Misc"]["Meteor game speed"] : 10;

            // Check to see if the config needs fixes
            ConfigTools.RepairConfig();

            // Raise event
            Kernel.KernelEventManager.RaiseConfigRead();
        }

        /// <summary>
        /// Configures the kernel according to the kernel configuration file
        /// </summary>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="Exceptions.ConfigException"></exception>
        public static bool TryReadConfig() => TryReadConfig(Paths.GetKernelPath(KernelPathType.Configuration));

        /// <summary>
        /// Configures the kernel according to the custom kernel configuration file
        /// </summary>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TryReadConfig(string ConfigPath)
        {
            Filesystem.ThrowOnInvalidPath(ConfigPath);
            return TryReadConfig(JObject.Parse(File.ReadAllText(ConfigPath)));
        }

        /// <summary>
        /// Configures the kernel according to the custom kernel configuration file
        /// </summary>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="Exceptions.ConfigException"></exception>
        public static bool TryReadConfig(JToken ConfigToken)
        {
            try
            {
                if (Flags.OptInToNewConfigReader)
                    ReadConfigNew(ConfigToken);
                else
                    ReadConfig(ConfigToken);
                return true;
            }
            catch (Exception ex)
            {
                Kernel.KernelEventManager.RaiseConfigReadError(ex);
                DebugWriter.WriteDebugStackTrace(ex);
                if (!SplashReport.KernelBooted)
                {
                    Notifications.NotifySend(new Notification(Translate.DoTranslation("Error loading settings"), Translate.DoTranslation("There is an error while loading settings. You may need to check the settings file."), Notifications.NotifPriority.Medium, Notifications.NotifType.Normal));
                }
                DebugWriter.WriteDebug(DebugLevel.E, "Error trying to read config: {0}", ex.Message);
                throw new Exceptions.ConfigException(Translate.DoTranslation("There is an error trying to read configuration: {0}."), ex, ex.Message);
            }
        }

        /// <summary>
        /// Main loader for configuration file
        /// </summary>
        public static void InitializeConfig()
        {
            // Make a config file if not found
            if (!Checking.FileExists(Paths.GetKernelPath(KernelPathType.Configuration)))
            {
                DebugWriter.WriteDebug(DebugLevel.E, "No config file found. Creating...");
                CreateConfig();
            }

            // Load and read config
            try
            {
                TryReadConfig();
            }
            catch (Exceptions.ConfigException cex)
            {
                TextWriterColor.Write(cex.Message, true, ColorTools.ColTypes.Error);
                DebugWriter.WriteDebugStackTrace(cex);
                TextWriterColor.Write(Translate.DoTranslation("Trying to fix configuration..."), true, ColorTools.ColTypes.Error);
                ConfigTools.RepairConfig();
            }
        }

    }
}
