
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

using ColorSeq;
using ColorSeq.Accessibility;
using Extensification.StringExts;
using FluentFTP;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Files;
using KS.Files.Folders;
using KS.Files.Querying;
using KS.Kernel.Debugging.RemoteDebug;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Calendar;
using KS.Misc.Games;
using KS.Misc.Notifications;
using KS.Misc.Screensaver;
using KS.Misc.Timers;
using KS.Misc.Writers.FancyWriters.Tools;
using KS.Misc.Writers.MiscWriters;
using KS.Network.Base;
using KS.Network.RPC;
using KS.Shell.Shells.FTP;
using KS.Shell.Shells.Hex;
using KS.Shell.Shells.Mail;
using KS.Shell.Shells.RSS;
using KS.Shell.Shells.Text;
using ManagedWeatherMap.Core;
using MimeKit.Text;
using Newtonsoft.Json;
using System;
using TermRead.Reader;
using static KS.ConsoleBase.Inputs.Styles.ChoiceStyle;
using static KS.Misc.Games.SpeedPress;
using ColorTools = KS.ConsoleBase.Colors.ColorTools;

namespace KS.Kernel.Configuration.Instances
{
    public class KernelMainConfig
    {
        private static string _CurrentDirectory = Paths.HomePath;

        public bool Maintenance { get; set; }
        public bool CheckUpdateStart { get; set; } = true;
        public string CustomBanner { get => WelcomeMessage.GetCustomBanner(); set => WelcomeMessage.customBanner = value; }
        public bool LangChangeCulture { get; set; }
        public string CurrentLanguage { get; set; } = "eng";
        public string CurrentCultStr { get; set; } = "en-US";
        public bool ShowAppInfoOnBoot { get; set; } = true;
        public bool ShowStageFinishTimes { get; set; }
        public bool StartKernelMods { get; set; } = true;
        public bool ShowCurrentTimeBeforeLogin { get; set; } = true;
        public bool NotifyFaultsBoot { get; set; } = true;
        public bool ShowStackTraceOnKernelError { get; set; }
        public bool AutoDownloadUpdate { get; set; } = true;
        public bool EventDebug { get; set; }
        public bool EnableSplash { get; set; } = true;
        public string SplashName { get; set; } = "Dots";
        public string BannerFigletFont
        {
            get => KernelTools.bannerFigletFont;
            set => KernelTools.bannerFigletFont = FigletTools.FigletFonts.ContainsKey(value) ? value : "Banner";
        }
        public bool SimulateNoAPM { get; set; }
        public bool SetBackground { get; set; } = true;
        public bool ColorBlind
        {
            get => ColorSeq.ColorTools.EnableColorTransformation;
            set => ColorSeq.ColorTools.EnableColorTransformation = value;
        }
        public int BlindnessDeficiency
        {
            get => (int)ColorSeq.ColorTools.ColorDeficiency;
            set => ColorSeq.ColorTools.ColorDeficiency = (Deficiency)value;
        }
        public double BlindnessSeverity
        {
            get => ColorSeq.ColorTools.ColorDeficiencySeverity;
            set => ColorSeq.ColorTools.ColorDeficiencySeverity = value;
        }
        public bool BeepOnShutdown { get; set; }
        public bool DelayOnShutdown { get; set; }
        public bool ColorBlindSimple
        {
            get => ColorSeq.ColorTools.EnableSimpleColorTransformation;
            set => ColorSeq.ColorTools.EnableSimpleColorTransformation = value;
        }
        public bool ConsoleSupportsTrueColor { get; set; } = true;
        public int AltCalendar { get; set; } = (int)CalendarTypes.Hijri;
        public bool EnableAltCalendar { get; set; }
        public bool StartCustomScreensavers { get; set; } = true;
        public string UserNameShellColor
        {
            get => ColorTools.GetColor(KernelColorType.UserNameShell).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.UserNameShell, new ColorSeq.Color(value));
        }
        public string HostNameShellColor
        {
            get => ColorTools.GetColor(KernelColorType.HostNameShell).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.HostNameShell, new ColorSeq.Color(value));
        }
        public string ContinuableKernelErrorColor
        {
            get => ColorTools.GetColor(KernelColorType.ContKernelError).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.ContKernelError, new ColorSeq.Color(value));
        }
        public string UncontinuableKernelErrorColor
        {
            get => ColorTools.GetColor(KernelColorType.UncontKernelError).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.UncontKernelError, new ColorSeq.Color(value));
        }
        public string TextColor
        {
            get => ColorTools.GetColor(KernelColorType.NeutralText).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.NeutralText, new ColorSeq.Color(value));
        }
        public string LicenseColor
        {
            get => ColorTools.GetColor(KernelColorType.License).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.License, new ColorSeq.Color(value));
        }
        public string BackgroundColor
        {
            get => ColorTools.GetColor(KernelColorType.Background).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.Background, new ColorSeq.Color(value));
        }
        public string InputColor
        {
            get => ColorTools.GetColor(KernelColorType.Input).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.Input, new ColorSeq.Color(value));
        }
        public string ListEntryColor
        {
            get => ColorTools.GetColor(KernelColorType.ListEntry).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.ListEntry, new ColorSeq.Color(value));
        }
        public string ListValueColor
        {
            get => ColorTools.GetColor(KernelColorType.ListValue).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.ListValue, new ColorSeq.Color(value));
        }
        public string KernelStageColor
        {
            get => ColorTools.GetColor(KernelColorType.Stage).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.Stage, new ColorSeq.Color(value));
        }
        public string ErrorTextColor
        {
            get => ColorTools.GetColor(KernelColorType.Error).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.Error, new ColorSeq.Color(value));
        }
        public string WarningTextColor
        {
            get => ColorTools.GetColor(KernelColorType.Warning).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.Warning, new ColorSeq.Color(value));
        }
        public string OptionColor
        {
            get => ColorTools.GetColor(KernelColorType.Option).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.Option, new ColorSeq.Color(value));
        }
        public string BannerColor
        {
            get => ColorTools.GetColor(KernelColorType.Banner).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.Banner, new ColorSeq.Color(value));
        }
        public string NotificationTitleColor
        {
            get => ColorTools.GetColor(KernelColorType.NotificationTitle).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.NotificationTitle, new ColorSeq.Color(value));
        }
        public string NotificationDescriptionColor
        {
            get => ColorTools.GetColor(KernelColorType.NotificationDescription).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.NotificationDescription, new ColorSeq.Color(value));
        }
        public string NotificationProgressColor
        {
            get => ColorTools.GetColor(KernelColorType.NotificationProgress).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.NotificationProgress, new ColorSeq.Color(value));
        }
        public string NotificationFailureColor
        {
            get => ColorTools.GetColor(KernelColorType.NotificationFailure).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.NotificationFailure, new ColorSeq.Color(value));
        }
        public string QuestionColor
        {
            get => ColorTools.GetColor(KernelColorType.Question).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.Question, new ColorSeq.Color(value));
        }
        public string SuccessColor
        {
            get => ColorTools.GetColor(KernelColorType.Success).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.Success, new ColorSeq.Color(value));
        }
        public string UserDollarColor
        {
            get => ColorTools.GetColor(KernelColorType.UserDollar).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.UserDollar, new ColorSeq.Color(value));
        }
        public string TipColor
        {
            get => ColorTools.GetColor(KernelColorType.Tip).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.Tip, new ColorSeq.Color(value));
        }
        public string SeparatorTextColor
        {
            get => ColorTools.GetColor(KernelColorType.SeparatorText).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.SeparatorText, new ColorSeq.Color(value));
        }
        public string SeparatorColor
        {
            get => ColorTools.GetColor(KernelColorType.Separator).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.Separator, new ColorSeq.Color(value));
        }
        public string ListTitleColor
        {
            get => ColorTools.GetColor(KernelColorType.ListTitle).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.ListTitle, new ColorSeq.Color(value));
        }
        public string DevelopmentWarningColor
        {
            get => ColorTools.GetColor(KernelColorType.DevelopmentWarning).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.DevelopmentWarning, new ColorSeq.Color(value));
        }
        public string StageTimeColor
        {
            get => ColorTools.GetColor(KernelColorType.StageTime).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.StageTime, new ColorSeq.Color(value));
        }
        public string ProgressColor
        {
            get => ColorTools.GetColor(KernelColorType.Progress).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.Progress, new ColorSeq.Color(value));
        }
        public string BackOptionColor
        {
            get => ColorTools.GetColor(KernelColorType.BackOption).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.BackOption, new ColorSeq.Color(value));
        }
        public string LowPriorityBorderColor
        {
            get => ColorTools.GetColor(KernelColorType.LowPriorityBorder).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.LowPriorityBorder, new ColorSeq.Color(value));
        }
        public string MediumPriorityBorderColor
        {
            get => ColorTools.GetColor(KernelColorType.MediumPriorityBorder).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.MediumPriorityBorder, new ColorSeq.Color(value));
        }
        public string HighPriorityBorderColor
        {
            get => ColorTools.GetColor(KernelColorType.HighPriorityBorder).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.HighPriorityBorder, new ColorSeq.Color(value));
        }
        public string TableSeparatorColor
        {
            get => ColorTools.GetColor(KernelColorType.TableSeparator).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.TableSeparator, new ColorSeq.Color(value));
        }
        public string TableHeaderColor
        {
            get => ColorTools.GetColor(KernelColorType.TableHeader).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.TableHeader, new ColorSeq.Color(value));
        }
        public string TableValueColor
        {
            get => ColorTools.GetColor(KernelColorType.TableValue).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.TableValue, new ColorSeq.Color(value));
        }
        public string SelectedOptionColor
        {
            get => ColorTools.GetColor(KernelColorType.SelectedOption).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.SelectedOption, new ColorSeq.Color(value));
        }
        public string AlternativeOptionColor
        {
            get => ColorTools.GetColor(KernelColorType.AlternativeOption).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.AlternativeOption, new ColorSeq.Color(value));
        }
        public string WeekendDayColor
        {
            get => ColorTools.GetColor(KernelColorType.WeekendDay).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.WeekendDay, new ColorSeq.Color(value));
        }
        public string EventDayColor
        {
            get => ColorTools.GetColor(KernelColorType.EventDay).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.EventDay, new ColorSeq.Color(value));
        }
        public string TableTitleColor
        {
            get => ColorTools.GetColor(KernelColorType.TableTitle).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.TableTitle, new ColorSeq.Color(value));
        }
        public string TodayDayColor
        {
            get => ColorTools.GetColor(KernelColorType.TodayDay).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.TodayDay, new ColorSeq.Color(value));
        }
        public bool QuietHardwareProbe { get; set; }
        public bool FullHardwareProbe { get; set; }
        public bool VerboseHardwareProbe { get; set; }
        public bool ShowMOTD { get; set; } = true;
        public bool ClearOnLogin { get; set; }
        public string HostName { get; set; } = "kernel";
        public bool ShowAvailableUsers { get; set; } = true;
        public string MotdFilePath { get; set; } = Paths.GetKernelPath(KernelPathType.MOTD);
        public string MalFilePath { get; set; } = Paths.GetKernelPath(KernelPathType.MAL);
        public string UsernamePrompt { get; set; } = "";
        public string PasswordPrompt { get; set; } = "";
        public bool ShowMAL { get; set; } = true;
        public bool IncludeAnonymous { get; set; }
        public bool IncludeDisabled { get; set; }
        public bool ModernLogon { get; set; } = true;
        public bool SimHelp { get; set; }
        public string CurrentDir
        {
            get
            {
                return _CurrentDirectory;
            }
            set
            {
                Filesystem.ThrowOnInvalidPath(value);
                value = Filesystem.NeutralizePath(value);
                if (Checking.FolderExists(value))
                {
                    _CurrentDirectory = value;
                }
                else
                {
                    throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("Directory {0} not found").FormatString(value));
                }
            }
        }
        public string PathsToLookup { get; set; } = Environment.GetEnvironmentVariable("PATH");
        public string PromptPreset { get; set; } = "Default";
        public string FTPPromptPreset { get; set; } = "Default";
        public string MailPromptPreset { get; set; } = "Default";
        public string SFTPPromptPreset { get; set; } = "Default";
        public string RSSPromptPreset { get; set; } = "Default";
        public string TextEditPromptPreset { get; set; } = "Default";
        public string JSONShellPromptPreset { get; set; } = "Default";
        public string HexEditPromptPreset { get; set; } = "Default";
        public string HTTPShellPromptPreset { get; set; } = "Default";
        public string ArchiveShellPromptPreset { get; set; } = "Default";
        public string AdminShellPromptPreset { get; set; } = "Default";
        public int DefaultChoiceOutputType { get; set; } = (int)ChoiceOutputType.Modern;
        public int SortMode { get; set; } = (int)FilesystemSortOptions.FullName;
        public int SortDirection { get; set; } = (int)FilesystemSortDirection.Ascending;
        public bool HiddenFiles { get; set; }
        public bool FullParseMode { get; set; }
        public bool ShowFilesystemProgress { get; set; } = true;
        public bool ShowFileDetailsList { get; set; } = true;
        public bool SuppressUnauthorizedMessages { get; set; } = true;
        public bool PrintLineNumbers { get; set; }
        public bool SortList { get; set; } = true;
        public bool ShowTotalSizeInList { get; set; }
        public int DebugPort
        {
            get => RemoteDebugger.debugPort;
            set => RemoteDebugger.debugPort = value < 0 ? 3014 : value;
        }
        public int DownloadRetries
        {
            get => NetworkTools.downloadRetries;
            set => NetworkTools.downloadRetries = value < 0 ? 3 : value;
        }
        public int UploadRetries
        {
            get => NetworkTools.uploadRetries;
            set => NetworkTools.uploadRetries = value < 0 ? 3 : value;
        }
        public bool ShowProgress { get; set; } = true;
        public bool FTPLoggerUsername { get; set; }
        public bool FTPLoggerIP { get; set; }
        public bool FTPFirstProfileOnly { get; set; }
        public bool ShowPreview { get; set; }
        public bool RecordChatToDebugLog { get; set; } = true;
        public bool SSHBanner { get; set; }
        public bool RPCEnabled { get; set; } = true;
        public int RPCPort
        {
            get => RemoteProcedure.rpcPort;
            set => RemoteProcedure.rpcPort = value < 0 ? 12345 : value;
        }
        public bool FtpShowDetailsInList { get; set; } = true;
        public string FtpUserPromptStyle { get; set; } = "";
        public string FtpPassPromptStyle { get; set; } = "";
        public bool FtpUseFirstProfile { get; set; }
        public bool FtpNewConnectionsToSpeedDial { get; set; } = true;
        public bool FtpTryToValidateCertificate { get; set; } = true;
        public bool FtpShowMotd { get; set; } = true;
        public bool FtpAlwaysAcceptInvalidCerts { get; set; }
        public string Mail_UserPromptStyle { get; set; } = "";
        public string Mail_PassPromptStyle { get; set; } = "";
        public string Mail_IMAPPromptStyle { get; set; } = "";
        public string Mail_SMTPPromptStyle { get; set; } = "";
        public bool Mail_AutoDetectServer { get; set; } = true;
        public bool Mail_Debug { get; set; }
        public bool Mail_NotifyNewMail { get; set; } = true;
        public string Mail_GPGPromptStyle { get; set; } = "";
        public int Mail_ImapPingInterval
        {
            get => MailShellCommon.imapPingInterval;
            set => MailShellCommon.imapPingInterval = value < 0 ? 30000 : value;
        }
        public int Mail_SmtpPingInterval
        {
            get => MailShellCommon.smtpPingInterval;
            set => MailShellCommon.smtpPingInterval = value < 0 ? 30000 : value;
        }
        public int Mail_TextFormat { get; set; } = (int)TextFormat.Plain;
        public bool RDebugAutoStart { get; set; } = true;
        public string RDebugMessageFormat { get; set; } = "";
        public string RSSFeedUrlPromptStyle { get; set; } = "";
        public bool RSSRefreshFeeds { get; set; } = true;
        public int RSSRefreshInterval
        {
            get => RSSShellCommon.refreshInterval;
            set => RSSShellCommon.refreshInterval = value < 0 ? 60000 : value;
        }
        public bool SFTPShowDetailsInList { get; set; } = true;
        public string SFTPUserPromptStyle { get; set; } = "";
        public bool SFTPNewConnectionsToSpeedDial { get; set; } = true;
        public int PingTimeout
        {
            get => NetworkTools.pingTimeout;
            set => NetworkTools.pingTimeout = value < 0 ? 60000 : value;
        }
        public string DownloadPercentagePrint { get; set; } = "";
        public string UploadPercentagePrint { get; set; } = "";
        public bool FtpRecursiveHashing { get; set; }
        public int Mail_MaxMessagesInPage
        {
            get => MailShellCommon.maxMessagesInPage;
            set => MailShellCommon.maxMessagesInPage = value < 0 ? 10 : value;
        }
        public bool Mail_ShowProgress { get; set; } = true;
        public string Mail_ProgressStyle { get; set; } = "";
        public string Mail_ProgressStyleSingle { get; set; } = "";
        public bool DownloadNotificationProvoke { get; set; }
        public bool UploadNotificationProvoke { get; set; }
        public int RSSFetchTimeout
        {
            get => RSSShellCommon.fetchTimeout;
            set => RSSShellCommon.fetchTimeout = value < 0 ? 60000 : value;
        }
        public int FtpVerifyRetryAttempts
        {
            get => FTPShellCommon.verifyRetryAttempts;
            set => FTPShellCommon.verifyRetryAttempts = value < 0 ? 3 : value;
        }
        public int FtpConnectTimeout
        {
            get => FTPShellCommon.connectTimeout;
            set => FTPShellCommon.connectTimeout = value < 0 ? 15000 : value;
        }
        public int FtpDataConnectTimeout
        {
            get => FTPShellCommon.dataConnectTimeout;
            set => FTPShellCommon.dataConnectTimeout = value < 0 ? 15000 : value;
        }
        public int FtpProtocolVersions { get; set; } = (int)FtpIpVersion.ANY;
        public bool NotifyOnRemoteDebugConnectionError { get; set; } = true;
        public string DefaultSaverName
        {
            get => Screensaver.defSaverName;
            set => Screensaver.defSaverName = Screensaver.Screensavers.ContainsKey(value) ? value : "plain";
        }
        public int ScreenTimeout
        {
            get => Screensaver.scrnTimeout;
            set => Screensaver.scrnTimeout = value < 0 ? 300000 : value;
        }
        public bool ScreensaverDebug { get; set; }
        public bool PasswordLock { get; set; } = true;
        public bool CornerTimeDate { get; set; }
        public bool StartScroll { get; set; } = true;
        public bool LongTimeDate { get; set; } = true;
        public int PreferredUnit { get; set; } = (int)UnitMeasurement.Metric;
        public bool TextEdit_AutoSaveFlag { get; set; } = true;
        public int TextEdit_AutoSaveInterval
        {
            get => TextEditShellCommon.autoSaveInterval;
            set => TextEditShellCommon.autoSaveInterval = value < 0 ? 60 : value;
        }
        public bool HexEdit_AutoSaveFlag { get; set; } = true;
        public int HexEdit_AutoSaveInterval
        {
            get => HexEditShellCommon.autoSaveInterval;
            set => HexEditShellCommon.autoSaveInterval = value < 0 ? 60 : value;
        }
        public bool WrapListOutputs { get; set; }
        public bool DrawBorderNotification { get; set; }
        public string BlacklistedModsString { get; set; } = "";
        public int SolverMinimumNumber
        {
            get => Solver.minimumNumber;
            set => Solver.minimumNumber = value < 0 ? 0 : value;
        }
        public int SolverMaximumNumber
        {
            get => Solver.maximumNumber;
            set => Solver.maximumNumber = value < 0 ? 1000 : value;
        }
        public bool SolverShowInput { get; set; }
        public string NotifyUpperLeftCornerChar
        {
            get => NotificationManager.notifyUpperLeftCornerChar;
            set => NotificationManager.notifyUpperLeftCornerChar = string.IsNullOrEmpty(value) ? "╔" : value[0].ToString();
        }
        public string NotifyUpperRightCornerChar
        {
            get => NotificationManager.notifyUpperRightCornerChar;
            set => NotificationManager.notifyUpperRightCornerChar = string.IsNullOrEmpty(value) ? "╗" : value[0].ToString();
        }
        public string NotifyLowerLeftCornerChar
        {
            get => NotificationManager.notifyLowerLeftCornerChar;
            set => NotificationManager.notifyLowerLeftCornerChar = string.IsNullOrEmpty(value) ? "╚" : value[0].ToString();
        }
        public string NotifyLowerRightCornerChar
        {
            get => NotificationManager.notifyLowerRightCornerChar;
            set => NotificationManager.notifyLowerRightCornerChar = string.IsNullOrEmpty(value) ? "╝" : value[0].ToString();
        }
        public string NotifyUpperFrameChar
        {
            get => NotificationManager.notifyUpperFrameChar;
            set => NotificationManager.notifyUpperFrameChar = string.IsNullOrEmpty(value) ? "═" : value[0].ToString();
        }
        public string NotifyLowerFrameChar
        {
            get => NotificationManager.notifyLowerFrameChar;
            set => NotificationManager.notifyLowerFrameChar = string.IsNullOrEmpty(value) ? "═" : value[0].ToString();
        }
        public string NotifyLeftFrameChar
        {
            get => NotificationManager.notifyLeftFrameChar;
            set => NotificationManager.notifyLeftFrameChar = string.IsNullOrEmpty(value) ? "║" : value[0].ToString();
        }
        public string NotifyRightFrameChar
        {
            get => NotificationManager.notifyRightFrameChar;
            set => NotificationManager.notifyRightFrameChar = string.IsNullOrEmpty(value) ? "║" : value[0].ToString();
        }
        public int SpeedPressCurrentDifficulty { get; set; } = (int)SpeedPressDifficulty.Medium;
        public int SpeedPressTimeout
        {
            get => speedPressTimeout;
            set => speedPressTimeout = value < 0 ? 3000 : value;
        }
        public bool ShowHeadlineOnLogin { get; set; }
        public string RssHeadlineUrl { get; set; } = "https://www.techrepublic.com/rssfeeds/articles/";
        public bool SaveEventsRemindersDestructively { get; set; }
        public int JsonShell_Formatting { get; set; } = (int)Formatting.Indented;
        public bool EnableFigletTimer { get; set; }
        public string TimerFigletFont
        {
            get => TimerScreen.timerFigletFont;
            set => TimerScreen.timerFigletFont = FigletTools.FigletFonts.ContainsKey(value) ? value : "Small";
        }
        public bool ShowCommandsCount { get; set; }
        public bool ShowShellCommandsCount { get; set; } = true;
        public bool ShowModCommandsCount { get; set; } = true;
        public bool ShowShellAliasesCount { get; set; } = true;
        public string CurrentMask
        {
            get => Input.currentMask;
            set => Input.currentMask = string.IsNullOrEmpty(value) ? "*" : value[0].ToString();
        }
        public string ProgressUpperLeftCornerChar
        {
            get => ProgressTools.progressUpperLeftCornerChar;
            set => ProgressTools.progressUpperLeftCornerChar = string.IsNullOrEmpty(value) ? "╔" : value[0].ToString();
        }
        public string ProgressUpperRightCornerChar
        {
            get => ProgressTools.progressUpperRightCornerChar;
            set => ProgressTools.progressUpperRightCornerChar = string.IsNullOrEmpty(value) ? "╗" : value[0].ToString();
        }
        public string ProgressLowerLeftCornerChar
        {
            get => ProgressTools.progressLowerLeftCornerChar;
            set => ProgressTools.progressLowerLeftCornerChar = string.IsNullOrEmpty(value) ? "╚" : value[0].ToString();
        }
        public string ProgressLowerRightCornerChar
        {
            get => ProgressTools.progressLowerRightCornerChar;
            set => ProgressTools.progressLowerRightCornerChar = string.IsNullOrEmpty(value) ? "╝" : value[0].ToString();
        }
        public string ProgressUpperFrameChar
        {
            get => ProgressTools.progressUpperFrameChar;
            set => ProgressTools.progressUpperFrameChar = string.IsNullOrEmpty(value) ? "═" : value[0].ToString();
        }
        public string ProgressLowerFrameChar
        {
            get => ProgressTools.progressLowerFrameChar;
            set => ProgressTools.progressLowerFrameChar = string.IsNullOrEmpty(value) ? "═" : value[0].ToString();
        }
        public string ProgressLeftFrameChar
        {
            get => ProgressTools.progressLeftFrameChar;
            set => ProgressTools.progressLeftFrameChar = string.IsNullOrEmpty(value) ? "║" : value[0].ToString();
        }
        public string ProgressRightFrameChar
        {
            get => ProgressTools.progressRightFrameChar;
            set => ProgressTools.progressRightFrameChar = string.IsNullOrEmpty(value) ? "║" : value[0].ToString();
        }
        public bool InputHistoryEnabled
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
        public bool MeteorUsePowerLine { get; set; } = true;
        public int MeteorSpeed
        {
            get => MeteorShooter.meteorSpeed;
            set => MeteorShooter.meteorSpeed = value < 0 ? 10 : value;
        }
        public bool EnableScrollBarInSelection { get; set; } = true;
        public bool DoNotDisturb
        {
            get => Flags.DoNotDisturb;
            set => Flags.DoNotDisturb = value;
        }
        public string BorderUpperLeftCornerChar
        {
            get
            {
                return BorderTools._borderUpperLeftCornerChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╔";
                BorderTools._borderUpperLeftCornerChar = value;
            }
        }
        public string BorderUpperRightCornerChar
        {
            get
            {
                return BorderTools._borderUpperRightCornerChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╗";
                BorderTools._borderUpperRightCornerChar = value;
            }
        }
        public string BorderLowerLeftCornerChar
        {
            get
            {
                return BorderTools._borderLowerLeftCornerChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╚";
                BorderTools._borderLowerLeftCornerChar = value;
            }
        }
        public string BorderLowerRightCornerChar
        {
            get
            {
                return BorderTools._borderLowerRightCornerChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╝";
                BorderTools._borderLowerRightCornerChar = value;
            }
        }
        public string BorderUpperFrameChar
        {
            get
            {
                return BorderTools._borderUpperFrameChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "═";
                BorderTools._borderUpperFrameChar = value;
            }
        }
        public string BorderLowerFrameChar
        {
            get
            {
                return BorderTools._borderLowerFrameChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "═";
                BorderTools._borderLowerFrameChar = value;
            }
        }
        public string BorderLeftFrameChar
        {
            get
            {
                return BorderTools._borderLeftFrameChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "║";
                BorderTools._borderLeftFrameChar = value;
            }
        }
        public string BorderRightFrameChar
        {
            get
            {
                return BorderTools._borderRightFrameChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "║";
                BorderTools._borderRightFrameChar = value;
            }
        }
        public string FileManagerBackgroundColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.DarkBlue)).PlainSequence;
        public string FileManagerForegroundColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.Yellow)).PlainSequence;
        public string FileManagerPaneBackgroundColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.Blue3)).PlainSequence;
        public string FileManagerPaneSeparatorColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.DarkGreen_005f00)).PlainSequence;
        public string FileManagerPaneSelectedSeparatorColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.Green3_00d700)).PlainSequence;
        public string FileManagerPaneSelectedFileForeColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.Yellow)).PlainSequence;
        public string FileManagerPaneSelectedFileBackColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.DarkBlue)).PlainSequence;
        public string FileManagerPaneFileForeColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.DarkYellow)).PlainSequence;
        public string FileManagerPaneFileBackColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.Blue3)).PlainSequence;
        public string FileManagerOptionBackgroundColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.DarkCyan)).PlainSequence;
        public string FileManagerOptionForegroundColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.Black)).PlainSequence;
        public string FileManagerKeyBindingOptionColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.Cyan)).PlainSequence;
        public string FileManagerBoxBackgroundColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.Red)).PlainSequence;
        public string FileManagerBoxForegroundColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.White)).PlainSequence;
        public string TaskManagerBackgroundColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.DarkBlue)).PlainSequence;
        public string TaskManagerForegroundColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.Yellow)).PlainSequence;
        public string TaskManagerPaneBackgroundColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.Blue3)).PlainSequence;
        public string TaskManagerPaneSeparatorColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.DarkGreen_005f00)).PlainSequence;
        public string TaskManagerPaneSelectedTaskForeColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.Yellow)).PlainSequence;
        public string TaskManagerPaneSelectedTaskBackColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.DarkBlue)).PlainSequence;
        public string TaskManagerPaneTaskForeColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.DarkYellow)).PlainSequence;
        public string TaskManagerPaneTaskBackColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.Blue3)).PlainSequence;
        public string TaskManagerOptionBackgroundColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.DarkCyan)).PlainSequence;
        public string TaskManagerOptionForegroundColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.Black)).PlainSequence;
        public string TaskManagerKeyBindingOptionColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.Cyan)).PlainSequence;
    }
}
