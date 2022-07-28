
'    Kernel Simulator  Copyright (C) 2018-2022  EoflaOE
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

Imports System.Globalization
Imports System.IO
Imports ManagedWeatherMap.Core
Imports KS.ConsoleBase.Inputs.Styles
Imports KS.Files.Folders
Imports KS.Files.Querying
Imports KS.ManPages
Imports KS.Misc.Forecast
Imports KS.Misc.Games
Imports KS.Misc.Editors.JsonShell
Imports KS.Misc.Notifications
Imports KS.Misc.RarFile
Imports KS.Misc.Screensaver
Imports KS.Misc.Screensaver.Displays
Imports KS.Misc.Splash
Imports KS.Misc.Editors.TextEdit
Imports KS.Misc.Editors.HexEdit
Imports KS.Misc.Timers
Imports KS.Misc.Writers.FancyWriters.Tools
Imports KS.Misc.Writers.MiscWriters
Imports KS.Misc.ZipFile
Imports KS.Modifications
Imports KS.Network
Imports KS.Network.FTP
Imports KS.Network.HTTP
Imports KS.Network.Mail
Imports KS.Network.Mail.Directory
Imports KS.Network.RemoteDebug
Imports KS.Network.RPC
Imports KS.Network.RSS
Imports KS.Network.SFTP
Imports KS.Network.SSH
Imports KS.Network.Transfer
Imports KS.TestShell
Imports MimeKit.Text
Imports Newtonsoft.Json.Linq

Namespace Misc.Configuration
    Public Module Config

        ''' <summary>
        ''' Base config token to be loaded each kernel startup.
        ''' </summary>
        Friend ConfigToken As JObject

        ''' <summary>
        ''' Config category enumeration
        ''' </summary>
        Public Enum ConfigCategory
            ''' <summary>
            ''' All general kernel settings, mainly for maintaining the kernel.
            ''' </summary>
            General
            ''' <summary>
            ''' Color settings
            ''' </summary>
            Colors
            ''' <summary>
            ''' Hardware settings
            ''' </summary>
            Hardware
            ''' <summary>
            ''' Login settings
            ''' </summary>
            Login
            ''' <summary>
            ''' Shell settings
            ''' </summary>
            Shell
            ''' <summary>
            ''' Filesystem settings
            ''' </summary>
            Filesystem
            ''' <summary>
            ''' Network settings
            ''' </summary>
            Network
            ''' <summary>
            ''' Screensaver settings
            ''' </summary>
            Screensaver
            ''' <summary>
            ''' Miscellaneous settings
            ''' </summary>
            Misc
        End Enum

        ''' <summary>
        ''' Creates a new JSON object containing the kernel settings of all kinds
        ''' </summary>
        ''' <returns>A pristine config object</returns>
        Public Function GetNewConfigObject() As JObject
            Dim ConfigurationObject As New JObject

            'The General Section
            Dim GeneralConfig As New JObject From {
                    {"Prompt for Arguments on Boot", ArgsOnBoot},
                    {"Maintenance Mode", Maintenance},
                    {"Check for Updates on Startup", CheckUpdateStart},
                    {"Custom Startup Banner", CustomBanner},
                    {"Change Culture when Switching Languages", LangChangeCulture},
                    {"Language", CurrentLanguage},
                    {"Culture", CurrentCult.Name},
                    {"Show app information during boot", ShowAppInfoOnBoot},
                    {"Parse command-line arguments", ParseCommandLineArguments},
                    {"Show stage finish times", ShowStageFinishTimes},
                    {"Start kernel modifications on boot", StartKernelMods},
                    {"Show current time before login", ShowCurrentTimeBeforeLogin},
                    {"Notify for any fault during boot", NotifyFaultsBoot},
                    {"Show stack trace on kernel error", ShowStackTraceOnKernelError},
                    {"Check debug quota", CheckDebugQuota},
                    {"Automatically download updates", AutoDownloadUpdate},
                    {"Enable event debugging", EventDebug},
                    {"New welcome banner", NewWelcomeStyle},
                    {"Stylish splash screen", EnableSplash},
                    {"Splash name", SplashName},
                    {"Banner figlet font", BannerFigletFont},
                    {"Simulate No APM Mode", SimulateNoAPM}
                }
            ConfigurationObject.Add("General", GeneralConfig)

            'The Colors Section
            Dim ColorConfig As New JObject From {
                    {"User Name Shell Color", UserNameShellColor.PlainSequenceEnclosed},
                    {"Host Name Shell Color", HostNameShellColor.PlainSequenceEnclosed},
                    {"Continuable Kernel Error Color", ContKernelErrorColor.PlainSequenceEnclosed},
                    {"Uncontinuable Kernel Error Color", UncontKernelErrorColor.PlainSequenceEnclosed},
                    {"Text Color", NeutralTextColor.PlainSequenceEnclosed},
                    {"License Color", LicenseColor.PlainSequenceEnclosed},
                    {"Background Color", BackgroundColor.PlainSequenceEnclosed},
                    {"Input Color", InputColor.PlainSequenceEnclosed},
                    {"List Entry Color", ListEntryColor.PlainSequenceEnclosed},
                    {"List Value Color", ListValueColor.PlainSequenceEnclosed},
                    {"Kernel Stage Color", StageColor.PlainSequenceEnclosed},
                    {"Error Text Color", ErrorColor.PlainSequenceEnclosed},
                    {"Warning Text Color", WarningColor.PlainSequenceEnclosed},
                    {"Option Color", OptionColor.PlainSequenceEnclosed},
                    {"Banner Color", BannerColor.PlainSequenceEnclosed},
                    {"Notification Title Color", NotificationTitleColor.PlainSequenceEnclosed},
                    {"Notification Description Color", NotificationDescriptionColor.PlainSequenceEnclosed},
                    {"Notification Progress Color", NotificationProgressColor.PlainSequenceEnclosed},
                    {"Notification Failure Color", NotificationFailureColor.PlainSequenceEnclosed},
                    {"Question Color", QuestionColor.PlainSequenceEnclosed},
                    {"Success Color", SuccessColor.PlainSequenceEnclosed},
                    {"User Dollar Color", UserDollarColor.PlainSequenceEnclosed},
                    {"Tip Color", TipColor.PlainSequenceEnclosed},
                    {"Separator Text Color", SeparatorTextColor.PlainSequenceEnclosed},
                    {"Separator Color", SeparatorColor.PlainSequenceEnclosed},
                    {"List Title Color", ListTitleColor.PlainSequenceEnclosed},
                    {"Development Warning Color", DevelopmentWarningColor.PlainSequenceEnclosed},
                    {"Stage Time Color", StageTimeColor.PlainSequenceEnclosed},
                    {"Progress Color", ColorTools.ProgressColor.PlainSequenceEnclosed},
                    {"Back Option Color", BackOptionColor.PlainSequenceEnclosed},
                    {"Low Priority Border Color", LowPriorityBorderColor.PlainSequenceEnclosed},
                    {"Medium Priority Border Color", MediumPriorityBorderColor.PlainSequenceEnclosed},
                    {"High Priority Border Color", HighPriorityBorderColor.PlainSequenceEnclosed},
                    {"Table Separator Color", TableSeparatorColor.PlainSequenceEnclosed},
                    {"Table Header Color", TableHeaderColor.PlainSequenceEnclosed},
                    {"Table Value Color", TableValueColor.PlainSequenceEnclosed},
                    {"Selected Option Color", SelectedOptionColor.PlainSequenceEnclosed},
                    {"Alternative Option Color", AlternativeOptionColor.PlainSequenceEnclosed}
                }
            ConfigurationObject.Add("Colors", ColorConfig)

            'The Hardware Section
            Dim HardwareConfig As New JObject From {
                    {"Quiet Probe", QuietHardwareProbe},
                    {"Full Probe", FullHardwareProbe},
                    {"Verbose Probe", VerboseHardwareProbe}
                }
            ConfigurationObject.Add("Hardware", HardwareConfig)

            'The Login Section
            Dim LoginConfig As New JObject From {
                    {"Show MOTD on Log-in", ShowMOTD},
                    {"Clear Screen on Log-in", ClearOnLogin},
                    {"Host Name", HostName},
                    {"Show available usernames", ShowAvailableUsers},
                    {"MOTD Path", MOTDFilePath},
                    {"MAL Path", MALFilePath},
                    {"Username prompt style", UsernamePrompt},
                    {"Password prompt style", PasswordPrompt},
                    {"Show MAL on Log-in", ShowMAL},
                    {"Include anonymous users", IncludeAnonymous},
                    {"Include disabled users", IncludeDisabled}
                }
            ConfigurationObject.Add("Login", LoginConfig)

            'The Shell Section
            Dim ShellConfig As New JObject From {
                    {"Colored Shell", ColoredShell},
                    {"Simplified Help Command", SimHelp},
                    {"Current Directory", CurrentDir},
                    {"Lookup Directories", PathsToLookup.EncloseByDoubleQuotes},
                    {"Prompt Style", ShellPromptStyle},
                    {"FTP Prompt Style", FTPShellPromptStyle},
                    {"Mail Prompt Style", MailShellPromptStyle},
                    {"SFTP Prompt Style", SFTPShellPromptStyle},
                    {"RSS Prompt Style", RSSShellPromptStyle},
                    {"Text Edit Prompt Style", TextEdit_PromptStyle},
                    {"Zip Shell Prompt Style", ZipShell_PromptStyle},
                    {"Test Shell Prompt Style", Test_PromptStyle},
                    {"JSON Shell Prompt Style", JsonShell_PromptStyle},
                    {"Hex Edit Prompt Style", HexEdit_PromptStyle},
                    {"HTTP Shell Prompt Style", HTTPShellPromptStyle},
                    {"RAR Shell Prompt Style", RarShell_PromptStyle},
                    {"Probe injected commands", ProbeInjectedCommands},
                    {"Start color wheel in true color mode", ColorWheelTrueColor},
                    {"Default choice output type", DefaultChoiceOutputType}
                }
            ConfigurationObject.Add("Shell", ShellConfig)

            'The Filesystem Section
            Dim FilesystemConfig As New JObject From {
                    {"Filesystem sort mode", SortMode.ToString},
                    {"Filesystem sort direction", SortDirection.ToString},
                    {"Debug Size Quota in Bytes", DebugQuota},
                    {"Show Hidden Files", HiddenFiles},
                    {"Size parse mode", FullParseMode},
                    {"Show progress on filesystem operations", ShowFilesystemProgress},
                    {"Show file details in list", ShowFileDetailsList},
                    {"Suppress unauthorized messages", SuppressUnauthorizedMessages},
                    {"Print line numbers on printing file contents", PrintLineNumbers},
                    {"Sort the list", SortList},
                    {"Show total size in list", ShowTotalSizeInList}
                }
            ConfigurationObject.Add("Filesystem", FilesystemConfig)

            'The Network Section
            Dim NetworkConfig As New JObject From {
                    {"Debug Port", DebugPort},
                    {"Download Retry Times", DownloadRetries},
                    {"Upload Retry Times", UploadRetries},
                    {"Show progress bar while downloading or uploading from ""get"" or ""put"" command", ShowProgress},
                    {"Log FTP username", FTPLoggerUsername},
                    {"Log FTP IP address", FTPLoggerIP},
                    {"Return only first FTP profile", FTPFirstProfileOnly},
                    {"Show mail message preview", ShowPreview},
                    {"Record chat to debug log", RecordChatToDebugLog},
                    {"Show SSH banner", SSHBanner},
                    {"Enable RPC", RPCEnabled},
                    {"RPC Port", RPCPort},
                    {"Show file details in FTP list", FtpShowDetailsInList},
                    {"Username prompt style for FTP", FtpUserPromptStyle},
                    {"Password prompt style for FTP", FtpPassPromptStyle},
                    {"Use first FTP profile", FtpUseFirstProfile},
                    {"Add new connections to FTP speed dial", FtpNewConnectionsToSpeedDial},
                    {"Try to validate secure FTP certificates", FtpTryToValidateCertificate},
                    {"Show FTP MOTD on connection", FtpShowMotd},
                    {"Always accept invalid FTP certificates", FtpAlwaysAcceptInvalidCerts},
                    {"Username prompt style for mail", Mail_UserPromptStyle},
                    {"Password prompt style for mail", Mail_PassPromptStyle},
                    {"IMAP prompt style for mail", Mail_IMAPPromptStyle},
                    {"SMTP prompt style for mail", Mail_SMTPPromptStyle},
                    {"Automatically detect mail server", Mail_AutoDetectServer},
                    {"Enable mail debug", Mail_Debug},
                    {"Notify for new mail messages", Mail_NotifyNewMail},
                    {"GPG password prompt style for mail", Mail_GPGPromptStyle},
                    {"Send IMAP ping interval", Mail_ImapPingInterval},
                    {"Send SMTP ping interval", Mail_SmtpPingInterval},
                    {"Mail text format", Mail_TextFormat.ToString},
                    {"Automatically start remote debug on startup", RDebugAutoStart},
                    {"Remote debug message format", RDebugMessageFormat},
                    {"RSS feed URL prompt style", RSSFeedUrlPromptStyle},
                    {"Auto refresh RSS feed", RSSRefreshFeeds},
                    {"Auto refresh RSS feed interval", RSSRefreshInterval},
                    {"Show file details in SFTP list", SFTPShowDetailsInList},
                    {"Username prompt style for SFTP", SFTPUserPromptStyle},
                    {"Add new connections to SFTP speed dial", SFTPNewConnectionsToSpeedDial},
                    {"Ping timeout", PingTimeout},
                    {"Show extensive adapter info", ExtensiveAdapterInformation},
                    {"Show general network information", GeneralNetworkInformation},
                    {"Download percentage text", DownloadPercentagePrint},
                    {"Upload percentage text", UploadPercentagePrint},
                    {"Recursive hashing for FTP", FtpRecursiveHashing},
                    {"Maximum number of e-mails in one page", Mail_MaxMessagesInPage},
                    {"Show mail transfer progress", Mail_ShowProgress},
                    {"Mail transfer progress", Mail_ProgressStyle},
                    {"Mail transfer progress (single)", Mail_ProgressStyleSingle},
                    {"Show notification for download progress", DownloadNotificationProvoke},
                    {"Show notification for upload progress", UploadNotificationProvoke},
                    {"RSS feed fetch timeout", RSSFetchTimeout},
                    {"Verify retry attempts for FTP transmission", FtpVerifyRetryAttempts},
                    {"FTP connection timeout", FtpConnectTimeout},
                    {"FTP data connection timeout", FtpDataConnectTimeout},
                    {"FTP IP versions", FtpProtocolVersions},
                    {"Notify on remote debug connection error", NotifyOnRemoteDebugConnectionError}
                }
            ConfigurationObject.Add("Network", NetworkConfig)

            'The Screensaver Section
            Dim ScreensaverConfig As New JObject From {
                    {"Screensaver", DefSaverName},
                    {"Screensaver Timeout in ms", ScrnTimeout},
                    {"Enable screensaver debugging", ScreensaverDebug},
                    {"Ask for password after locking", PasswordLock}
                }

            'ColorMix config json object
            Dim ColorMixConfig As New JObject From {
                    {"Activate 255 Color Mode", ColorMix255Colors},
                    {"Activate True Color Mode", ColorMixTrueColor},
                    {"Delay in Milliseconds", ColorMixDelay},
                    {"Background color", If(New Color(ColorMixBackgroundColor).Type = ColorType.TrueColor, ColorMixBackgroundColor.EncloseByDoubleQuotes, ColorMixBackgroundColor)},
                    {"Minimum red color level", ColorMixMinimumRedColorLevel},
                    {"Minimum green color level", ColorMixMinimumGreenColorLevel},
                    {"Minimum blue color level", ColorMixMinimumBlueColorLevel},
                    {"Minimum color level", ColorMixMinimumColorLevel},
                    {"Maximum red color level", ColorMixMaximumRedColorLevel},
                    {"Maximum green color level", ColorMixMaximumGreenColorLevel},
                    {"Maximum blue color level", ColorMixMaximumBlueColorLevel},
                    {"Maximum color level", ColorMixMaximumColorLevel}
                }
            ScreensaverConfig.Add("ColorMix", ColorMixConfig)

            'Disco config json object
            Dim DiscoConfig As New JObject From {
                    {"Activate 255 Color Mode", Disco255Colors},
                    {"Activate True Color Mode", DiscoTrueColor},
                    {"Delay in Milliseconds", DiscoDelay},
                    {"Use Beats Per Minute", DiscoUseBeatsPerMinute},
                    {"Cycle Colors", DiscoCycleColors},
                    {"Enable Black and White Mode", DiscoEnableFedMode},
                    {"Minimum red color level", DiscoMinimumRedColorLevel},
                    {"Minimum green color level", DiscoMinimumGreenColorLevel},
                    {"Minimum blue color level", DiscoMinimumBlueColorLevel},
                    {"Minimum color level", DiscoMinimumColorLevel},
                    {"Maximum red color level", DiscoMaximumRedColorLevel},
                    {"Maximum green color level", DiscoMaximumGreenColorLevel},
                    {"Maximum blue color level", DiscoMaximumBlueColorLevel},
                    {"Maximum color level", DiscoMaximumColorLevel}
                }
            ScreensaverConfig.Add("Disco", DiscoConfig)

            'GlitterColor config json object
            Dim GlitterColorConfig As New JObject From {
                    {"Activate 255 Color Mode", GlitterColor255Colors},
                    {"Activate True Color Mode", GlitterColorTrueColor},
                    {"Delay in Milliseconds", GlitterColorDelay},
                    {"Minimum red color level", GlitterColorMinimumRedColorLevel},
                    {"Minimum green color level", GlitterColorMinimumGreenColorLevel},
                    {"Minimum blue color level", GlitterColorMinimumBlueColorLevel},
                    {"Minimum color level", GlitterColorMinimumColorLevel},
                    {"Maximum red color level", GlitterColorMaximumRedColorLevel},
                    {"Maximum green color level", GlitterColorMaximumGreenColorLevel},
                    {"Maximum blue color level", GlitterColorMaximumBlueColorLevel},
                    {"Maximum color level", GlitterColorMaximumColorLevel}
                }
            ScreensaverConfig.Add("GlitterColor", GlitterColorConfig)

            'Lines config json object
            Dim LinesConfig As New JObject From {
                    {"Activate 255 Color Mode", Lines255Colors},
                    {"Activate True Color Mode", LinesTrueColor},
                    {"Delay in Milliseconds", LinesDelay},
                    {"Line character", LinesLineChar},
                    {"Background color", If(New Color(LinesBackgroundColor).Type = ColorType.TrueColor, LinesBackgroundColor.EncloseByDoubleQuotes, LinesBackgroundColor)},
                    {"Minimum red color level", LinesMinimumRedColorLevel},
                    {"Minimum green color level", LinesMinimumGreenColorLevel},
                    {"Minimum blue color level", LinesMinimumBlueColorLevel},
                    {"Minimum color level", LinesMinimumColorLevel},
                    {"Maximum red color level", LinesMaximumRedColorLevel},
                    {"Maximum green color level", LinesMaximumGreenColorLevel},
                    {"Maximum blue color level", LinesMaximumBlueColorLevel},
                    {"Maximum color level", LinesMaximumColorLevel}
                }
            ScreensaverConfig.Add("Lines", LinesConfig)

            'Dissolve config json object
            Dim DissolveConfig As New JObject From {
                    {"Activate 255 Color Mode", Dissolve255Colors},
                    {"Activate True Color Mode", DissolveTrueColor},
                    {"Background color", If(New Color(DissolveBackgroundColor).Type = ColorType.TrueColor, DissolveBackgroundColor.EncloseByDoubleQuotes, DissolveBackgroundColor)},
                    {"Minimum red color level", DissolveMinimumRedColorLevel},
                    {"Minimum green color level", DissolveMinimumGreenColorLevel},
                    {"Minimum blue color level", DissolveMinimumBlueColorLevel},
                    {"Minimum color level", DissolveMinimumColorLevel},
                    {"Maximum red color level", DissolveMaximumRedColorLevel},
                    {"Maximum green color level", DissolveMaximumGreenColorLevel},
                    {"Maximum blue color level", DissolveMaximumBlueColorLevel},
                    {"Maximum color level", DissolveMaximumColorLevel}
                }
            ScreensaverConfig.Add("Dissolve", DissolveConfig)

            'BouncingBlock config json object
            Dim BouncingBlockConfig As New JObject From {
                    {"Activate 255 Color Mode", BouncingBlock255Colors},
                    {"Activate True Color Mode", BouncingBlockTrueColor},
                    {"Delay in Milliseconds", BouncingBlockDelay},
                    {"Background color", If(New Color(BouncingBlockBackgroundColor).Type = ColorType.TrueColor, BouncingBlockBackgroundColor.EncloseByDoubleQuotes, BouncingBlockBackgroundColor)},
                    {"Foreground color", If(New Color(BouncingBlockForegroundColor).Type = ColorType.TrueColor, BouncingBlockForegroundColor.EncloseByDoubleQuotes, BouncingBlockForegroundColor)},
                    {"Minimum red color level", BouncingBlockMinimumRedColorLevel},
                    {"Minimum green color level", BouncingBlockMinimumGreenColorLevel},
                    {"Minimum blue color level", BouncingBlockMinimumBlueColorLevel},
                    {"Minimum color level", BouncingBlockMinimumColorLevel},
                    {"Maximum red color level", BouncingBlockMaximumRedColorLevel},
                    {"Maximum green color level", BouncingBlockMaximumGreenColorLevel},
                    {"Maximum blue color level", BouncingBlockMaximumBlueColorLevel},
                    {"Maximum color level", BouncingBlockMaximumColorLevel}
                }
            ScreensaverConfig.Add("BouncingBlock", BouncingBlockConfig)

            'ProgressClock config json object
            Dim ProgressClockConfig As New JObject From {
                    {"Activate 255 Color Mode", ProgressClock255Colors},
                    {"Activate True Color Mode", ProgressClockTrueColor},
                    {"Cycle Colors", ProgressClockCycleColors},
                    {"Ticks to change color", ProgressClockCycleColorsTicks},
                    {"Color of Seconds Bar", ProgressClockSecondsProgressColor},
                    {"Color of Minutes Bar", ProgressClockMinutesProgressColor},
                    {"Color of Hours Bar", ProgressClockHoursProgressColor},
                    {"Color of Information", ProgressClockProgressColor},
                    {"Delay in Milliseconds", ProgressClockDelay},
                    {"Upper left corner character for hours bar", ProgressClockUpperLeftCornerCharHours},
                    {"Upper left corner character for minutes bar", ProgressClockUpperLeftCornerCharMinutes},
                    {"Upper left corner character for seconds bar", ProgressClockUpperLeftCornerCharSeconds},
                    {"Upper right corner character for hours bar", ProgressClockUpperRightCornerCharHours},
                    {"Upper right corner character for minutes bar", ProgressClockUpperRightCornerCharMinutes},
                    {"Upper right corner character for seconds bar", ProgressClockUpperRightCornerCharSeconds},
                    {"Lower left corner character for hours bar", ProgressClockLowerRightCornerCharHours},
                    {"Lower left corner character for minutes bar", ProgressClockLowerLeftCornerCharMinutes},
                    {"Lower left corner character for seconds bar", ProgressClockLowerLeftCornerCharSeconds},
                    {"Lower right corner character for hours bar", ProgressClockLowerRightCornerCharHours},
                    {"Lower right corner character for minutes bar", ProgressClockLowerRightCornerCharMinutes},
                    {"Lower right corner character for seconds bar", ProgressClockLowerRightCornerCharSeconds},
                    {"Upper frame character for hours bar", ProgressClockUpperFrameCharHours},
                    {"Upper frame character for minutes bar", ProgressClockUpperFrameCharMinutes},
                    {"Upper frame character for seconds bar", ProgressClockUpperFrameCharSeconds},
                    {"Lower frame character for hours bar", ProgressClockLowerFrameCharHours},
                    {"Lower frame character for minutes bar", ProgressClockLowerFrameCharMinutes},
                    {"Lower frame character for seconds bar", ProgressClockLowerFrameCharSeconds},
                    {"Left frame character for hours bar", ProgressClockLeftFrameCharHours},
                    {"Left frame character for minutes bar", ProgressClockLeftFrameCharMinutes},
                    {"Left frame character for seconds bar", ProgressClockLeftFrameCharSeconds},
                    {"Right frame character for hours bar", ProgressClockRightFrameCharHours},
                    {"Right frame character for minutes bar", ProgressClockRightFrameCharMinutes},
                    {"Right frame character for seconds bar", ProgressClockRightFrameCharSeconds},
                    {"Information text for hours", ProgressClockInfoTextHours},
                    {"Information text for minutes", ProgressClockInfoTextMinutes},
                    {"Information text for seconds", ProgressClockInfoTextSeconds},
                    {"Minimum red color level for hours", ProgressClockMinimumRedColorLevelHours},
                    {"Minimum green color level for hours", ProgressClockMinimumGreenColorLevelHours},
                    {"Minimum blue color level for hours", ProgressClockMinimumBlueColorLevelHours},
                    {"Minimum color level for hours", ProgressClockMinimumColorLevelHours},
                    {"Maximum red color level for hours", ProgressClockMaximumRedColorLevelHours},
                    {"Maximum green color level for hours", ProgressClockMaximumGreenColorLevelHours},
                    {"Maximum blue color level for hours", ProgressClockMaximumBlueColorLevelHours},
                    {"Maximum color level for hours", ProgressClockMaximumColorLevelHours},
                    {"Minimum red color level for minutes", ProgressClockMinimumRedColorLevelMinutes},
                    {"Minimum green color level for minutes", ProgressClockMinimumGreenColorLevelMinutes},
                    {"Minimum blue color level for minutes", ProgressClockMinimumBlueColorLevelMinutes},
                    {"Minimum color level for minutes", ProgressClockMinimumColorLevelMinutes},
                    {"Maximum red color level for minutes", ProgressClockMaximumRedColorLevelMinutes},
                    {"Maximum green color level for minutes", ProgressClockMaximumGreenColorLevelMinutes},
                    {"Maximum blue color level for minutes", ProgressClockMaximumBlueColorLevelMinutes},
                    {"Maximum color level for minutes", ProgressClockMaximumColorLevelMinutes},
                    {"Minimum red color level for seconds", ProgressClockMinimumRedColorLevelSeconds},
                    {"Minimum green color level for seconds", ProgressClockMinimumGreenColorLevelSeconds},
                    {"Minimum blue color level for seconds", ProgressClockMinimumBlueColorLevelSeconds},
                    {"Minimum color level for seconds", ProgressClockMinimumColorLevelSeconds},
                    {"Maximum red color level for seconds", ProgressClockMaximumRedColorLevelSeconds},
                    {"Maximum green color level for seconds", ProgressClockMaximumGreenColorLevelSeconds},
                    {"Maximum blue color level for seconds", ProgressClockMaximumBlueColorLevelSeconds},
                    {"Maximum color level for seconds", ProgressClockMaximumColorLevelSeconds},
                    {"Minimum red color level", ProgressClockMinimumRedColorLevel},
                    {"Minimum green color level", ProgressClockMinimumGreenColorLevel},
                    {"Minimum blue color level", ProgressClockMinimumBlueColorLevel},
                    {"Minimum color level", ProgressClockMinimumColorLevel},
                    {"Maximum red color level", ProgressClockMaximumRedColorLevel},
                    {"Maximum green color level", ProgressClockMaximumGreenColorLevel},
                    {"Maximum blue color level", ProgressClockMaximumBlueColorLevel},
                    {"Maximum color level", ProgressClockMaximumColorLevel}
                }
            ScreensaverConfig.Add("ProgressClock", ProgressClockConfig)

            'Lighter config json object
            Dim LighterConfig As New JObject From {
                    {"Activate 255 Color Mode", Lighter255Colors},
                    {"Activate True Color Mode", LighterTrueColor},
                    {"Delay in Milliseconds", LighterDelay},
                    {"Max Positions Count", LighterMaxPositions},
                    {"Background color", If(New Color(LighterBackgroundColor).Type = ColorType.TrueColor, LighterBackgroundColor.EncloseByDoubleQuotes, LighterBackgroundColor)},
                    {"Minimum red color level", LighterMinimumRedColorLevel},
                    {"Minimum green color level", LighterMinimumGreenColorLevel},
                    {"Minimum blue color level", LighterMinimumBlueColorLevel},
                    {"Minimum color level", LighterMinimumColorLevel},
                    {"Maximum red color level", LighterMaximumRedColorLevel},
                    {"Maximum green color level", LighterMaximumGreenColorLevel},
                    {"Maximum blue color level", LighterMaximumBlueColorLevel},
                    {"Maximum color level", LighterMaximumColorLevel}
                }
            ScreensaverConfig.Add("Lighter", LighterConfig)

            'Wipe config json object
            Dim WipeConfig As New JObject From {
                    {"Activate 255 Color Mode", Wipe255Colors},
                    {"Activate True Color Mode", WipeTrueColor},
                    {"Delay in Milliseconds", WipeDelay},
                    {"Wipes to change direction", WipeWipesNeededToChangeDirection},
                    {"Background color", If(New Color(WipeBackgroundColor).Type = ColorType.TrueColor, WipeBackgroundColor.EncloseByDoubleQuotes, WipeBackgroundColor)},
                    {"Minimum red color level", WipeMinimumRedColorLevel},
                    {"Minimum green color level", WipeMinimumGreenColorLevel},
                    {"Minimum blue color level", WipeMinimumBlueColorLevel},
                    {"Minimum color level", WipeMinimumColorLevel},
                    {"Maximum red color level", WipeMaximumRedColorLevel},
                    {"Maximum green color level", WipeMaximumGreenColorLevel},
                    {"Maximum blue color level", WipeMaximumBlueColorLevel},
                    {"Maximum color level", WipeMaximumColorLevel}
                }
            ScreensaverConfig.Add("Wipe", WipeConfig)

            'Matrix config json object
            Dim MatrixConfig As New JObject From {
                    {"Delay in Milliseconds", MatrixDelay}
                }
            ScreensaverConfig.Add("Matrix", MatrixConfig)

            'GlitterMatrix config json object
            Dim GlitterMatrixConfig As New JObject From {
                    {"Delay in Milliseconds", GlitterMatrixDelay},
                    {"Background color", If(New Color(GlitterMatrixBackgroundColor).Type = ColorType.TrueColor, GlitterMatrixBackgroundColor.EncloseByDoubleQuotes, GlitterMatrixBackgroundColor)},
                    {"Foreground color", If(New Color(GlitterMatrixForegroundColor).Type = ColorType.TrueColor, GlitterMatrixForegroundColor.EncloseByDoubleQuotes, GlitterMatrixForegroundColor)}
                }
            ScreensaverConfig.Add("GlitterMatrix", GlitterMatrixConfig)

            'BouncingText config json object
            Dim BouncingTextConfig As New JObject From {
                    {"Activate 255 Color Mode", BouncingText255Colors},
                    {"Activate True Color Mode", BouncingTextTrueColor},
                    {"Delay in Milliseconds", BouncingTextDelay},
                    {"Text Shown", BouncingTextWrite},
                    {"Background color", If(New Color(BouncingTextBackgroundColor).Type = ColorType.TrueColor, BouncingTextBackgroundColor.EncloseByDoubleQuotes, BouncingTextBackgroundColor)},
                    {"Foreground color", If(New Color(BouncingTextForegroundColor).Type = ColorType.TrueColor, BouncingTextForegroundColor.EncloseByDoubleQuotes, BouncingTextForegroundColor)},
                    {"Minimum red color level", BouncingTextMinimumRedColorLevel},
                    {"Minimum green color level", BouncingTextMinimumGreenColorLevel},
                    {"Minimum blue color level", BouncingTextMinimumBlueColorLevel},
                    {"Minimum color level", BouncingTextMinimumColorLevel},
                    {"Maximum red color level", BouncingTextMaximumRedColorLevel},
                    {"Maximum green color level", BouncingTextMaximumGreenColorLevel},
                    {"Maximum blue color level", BouncingTextMaximumBlueColorLevel},
                    {"Maximum color level", BouncingTextMaximumColorLevel}
                }
            ScreensaverConfig.Add("BouncingText", BouncingTextConfig)

            'Fader config json object
            Dim FaderConfig As New JObject From {
                    {"Delay in Milliseconds", FaderDelay},
                    {"Fade Out Delay in Milliseconds", FaderFadeOutDelay},
                    {"Text Shown", FaderWrite},
                    {"Max Fade Steps", FaderMaxSteps},
                    {"Background color", If(New Color(FaderBackgroundColor).Type = ColorType.TrueColor, FaderBackgroundColor.EncloseByDoubleQuotes, FaderBackgroundColor)},
                    {"Minimum red color level", FaderMinimumRedColorLevel},
                    {"Minimum green color level", FaderMinimumGreenColorLevel},
                    {"Minimum blue color level", FaderMinimumBlueColorLevel},
                    {"Maximum red color level", FaderMaximumRedColorLevel},
                    {"Maximum green color level", FaderMaximumGreenColorLevel},
                    {"Maximum blue color level", FaderMaximumBlueColorLevel}
                }
            ScreensaverConfig.Add("Fader", FaderConfig)

            'FaderBack config json object
            Dim FaderBackConfig As New JObject From {
                    {"Delay in Milliseconds", FaderBackDelay},
                    {"Fade Out Delay in Milliseconds", FaderBackFadeOutDelay},
                    {"Max Fade Steps", FaderBackMaxSteps},
                    {"Minimum red color level", FaderBackMinimumRedColorLevel},
                    {"Minimum green color level", FaderBackMinimumGreenColorLevel},
                    {"Minimum blue color level", FaderBackMinimumBlueColorLevel},
                    {"Maximum red color level", FaderBackMaximumRedColorLevel},
                    {"Maximum green color level", FaderBackMaximumGreenColorLevel},
                    {"Maximum blue color level", FaderBackMaximumBlueColorLevel}
                }
            ScreensaverConfig.Add("FaderBack", FaderBackConfig)

            'BeatFader config json object
            Dim BeatFaderConfig As New JObject From {
                    {"Activate 255 Color Mode", BeatFader255Colors},
                    {"Activate True Color Mode", BeatFaderTrueColor},
                    {"Delay in Beats Per Minute", BeatFaderDelay},
                    {"Cycle Colors", BeatFaderCycleColors},
                    {"Beat Color", BeatFaderBeatColor},
                    {"Max Fade Steps", BeatFaderMaxSteps},
                    {"Minimum red color level", BeatFaderMinimumRedColorLevel},
                    {"Minimum green color level", BeatFaderMinimumGreenColorLevel},
                    {"Minimum blue color level", BeatFaderMinimumBlueColorLevel},
                    {"Minimum color level", BeatFaderMinimumColorLevel},
                    {"Maximum red color level", BeatFaderMaximumRedColorLevel},
                    {"Maximum green color level", BeatFaderMaximumGreenColorLevel},
                    {"Maximum blue color level", BeatFaderMaximumBlueColorLevel},
                    {"Maximum color level", BeatFaderMaximumColorLevel}
                }
            ScreensaverConfig.Add("BeatFader", BeatFaderConfig)

            'Typo config json object
            Dim TypoConfig As New JObject From {
                    {"Delay in Milliseconds", TypoDelay},
                    {"Write Again Delay in Milliseconds", TypoWriteAgainDelay},
                    {"Text Shown", TypoWrite},
                    {"Minimum writing speed in WPM", TypoWritingSpeedMin},
                    {"Maximum writing speed in WPM", TypoWritingSpeedMax},
                    {"Probability of typo in percent", TypoMissStrikePossibility},
                    {"Probability of miss in percent", TypoMissPossibility},
                    {"Text color", If(New Color(TypoTextColor).Type = ColorType.TrueColor, TypoTextColor.EncloseByDoubleQuotes, TypoTextColor)}
                }
            ScreensaverConfig.Add("Typo", TypoConfig)

            'Marquee config json object
            Dim MarqueeConfig As New JObject From {
                    {"Activate 255 Color Mode", Marquee255Colors},
                    {"Activate True Color Mode", MarqueeTrueColor},
                    {"Delay in Milliseconds", MarqueeDelay},
                    {"Text Shown", MarqueeWrite},
                    {"Always Centered", MarqueeAlwaysCentered},
                    {"Use Console API", MarqueeUseConsoleAPI},
                    {"Background color", If(New Color(MarqueeBackgroundColor).Type = ColorType.TrueColor, MarqueeBackgroundColor.EncloseByDoubleQuotes, MarqueeBackgroundColor)},
                    {"Minimum red color level", MarqueeMinimumRedColorLevel},
                    {"Minimum green color level", MarqueeMinimumGreenColorLevel},
                    {"Minimum blue color level", MarqueeMinimumBlueColorLevel},
                    {"Minimum color level", MarqueeMinimumColorLevel},
                    {"Maximum red color level", MarqueeMaximumRedColorLevel},
                    {"Maximum green color level", MarqueeMaximumGreenColorLevel},
                    {"Maximum blue color level", MarqueeMaximumBlueColorLevel},
                    {"Maximum color level", MarqueeMaximumColorLevel}
                }
            ScreensaverConfig.Add("Marquee", MarqueeConfig)

            'Linotypo config json object
            Dim LinotypoConfig As New JObject From {
                    {"Delay in Milliseconds", LinotypoDelay},
                    {"New Screen Delay in Milliseconds", LinotypoNewScreenDelay},
                    {"Text Shown", LinotypoWrite},
                    {"Minimum writing speed in WPM", LinotypoWritingSpeedMin},
                    {"Maximum writing speed in WPM", LinotypoWritingSpeedMax},
                    {"Probability of typo in percent", LinotypoMissStrikePossibility},
                    {"Column Count", LinotypoTextColumns},
                    {"Line Fill Threshold", LinotypoEtaoinThreshold},
                    {"Line Fill Capping Probability in percent", LinotypoEtaoinCappingPossibility},
                    {"Line Fill Type", LinotypoEtaoinType},
                    {"Probability of miss in percent", LinotypoMissPossibility},
                    {"Text color", If(New Color(LinotypoTextColor).Type = ColorType.TrueColor, LinotypoTextColor.EncloseByDoubleQuotes, LinotypoTextColor)}
                }
            ScreensaverConfig.Add("Linotypo", LinotypoConfig)

            'Typewriter config json object
            Dim TypewriterConfig As New JObject From {
                    {"Delay in Milliseconds", TypewriterDelay},
                    {"New Screen Delay in Milliseconds", TypewriterNewScreenDelay},
                    {"Text Shown", TypewriterWrite},
                    {"Minimum writing speed in WPM", TypewriterWritingSpeedMin},
                    {"Maximum writing speed in WPM", TypewriterWritingSpeedMax},
                    {"Text color", If(New Color(TypewriterTextColor).Type = ColorType.TrueColor, TypewriterTextColor.EncloseByDoubleQuotes, TypewriterTextColor)}
                }
            ScreensaverConfig.Add("Typewriter", TypewriterConfig)

            'FlashColor config json object
            Dim FlashColorConfig As New JObject From {
                    {"Activate 255 Color Mode", FlashColor255Colors},
                    {"Activate True Color Mode", FlashColorTrueColor},
                    {"Delay in Milliseconds", FlashColorDelay},
                    {"Background color", If(New Color(FlashColorBackgroundColor).Type = ColorType.TrueColor, FlashColorBackgroundColor.EncloseByDoubleQuotes, FlashColorBackgroundColor)},
                    {"Minimum red color level", FlashColorMinimumRedColorLevel},
                    {"Minimum green color level", FlashColorMinimumGreenColorLevel},
                    {"Minimum blue color level", FlashColorMinimumBlueColorLevel},
                    {"Minimum color level", FlashColorMinimumColorLevel},
                    {"Maximum red color level", FlashColorMaximumRedColorLevel},
                    {"Maximum green color level", FlashColorMaximumGreenColorLevel},
                    {"Maximum blue color level", FlashColorMaximumBlueColorLevel},
                    {"Maximum color level", FlashColorMaximumColorLevel}
                }
            ScreensaverConfig.Add("FlashColor", FlashColorConfig)

            'SpotWrite config json object
            Dim SpotWriteonfig As New JObject From {
                    {"Delay in Milliseconds", SpotWriteDelay},
                    {"New Screen Delay in Milliseconds", SpotWriteNewScreenDelay},
                    {"Text Shown", SpotWriteWrite},
                    {"Text color", SpotWriteTextColor}
                }
            ScreensaverConfig.Add("SpotWrite", SpotWriteonfig)

            'Ramp config json object
            Dim RampConfig As New JObject From {
                    {"Activate 255 Color Mode", Ramp255Colors},
                    {"Activate True Color Mode", RampTrueColor},
                    {"Delay in Milliseconds", RampDelay},
                    {"Next ramp interval", RampDelay},
                    {"Upper left corner character for ramp bar", RampUpperLeftCornerChar},
                    {"Upper right corner character for ramp bar", RampUpperRightCornerChar},
                    {"Lower left corner character for ramp bar", RampLowerLeftCornerChar},
                    {"Lower right corner character for ramp bar", RampLowerRightCornerChar},
                    {"Upper frame character for ramp bar", RampUpperFrameChar},
                    {"Lower frame character for ramp bar", RampLowerFrameChar},
                    {"Left frame character for ramp bar", RampLeftFrameChar},
                    {"Right frame character for ramp bar", RampRightFrameChar},
                    {"Minimum red color level for start color", RampMinimumRedColorLevelStart},
                    {"Minimum green color level for start color", RampMinimumGreenColorLevelStart},
                    {"Minimum blue color level for start color", RampMinimumBlueColorLevelStart},
                    {"Minimum color level for start color", RampMinimumColorLevelStart},
                    {"Maximum red color level for start color", RampMaximumRedColorLevelStart},
                    {"Maximum green color level for start color", RampMaximumGreenColorLevelStart},
                    {"Maximum blue color level for start color", RampMaximumBlueColorLevelStart},
                    {"Maximum color level for start color", RampMaximumColorLevelStart},
                    {"Minimum red color level for end color", RampMinimumRedColorLevelEnd},
                    {"Minimum green color level for end color", RampMinimumGreenColorLevelEnd},
                    {"Minimum blue color level for end color", RampMinimumBlueColorLevelEnd},
                    {"Minimum color level for end color", RampMinimumColorLevelEnd},
                    {"Maximum red color level for end color", RampMaximumRedColorLevelEnd},
                    {"Maximum green color level for end color", RampMaximumGreenColorLevelEnd},
                    {"Maximum blue color level for end color", RampMaximumBlueColorLevelEnd},
                    {"Maximum color level for end color", RampMaximumColorLevelEnd},
                    {"Upper left corner color for ramp bar", RampUpperLeftCornerColor},
                    {"Upper right corner color for ramp bar", RampUpperRightCornerColor},
                    {"Lower left corner color for ramp bar", RampLowerLeftCornerColor},
                    {"Lower right corner color for ramp bar", RampLowerRightCornerColor},
                    {"Upper frame color for ramp bar", RampUpperFrameColor},
                    {"Lower frame color for ramp bar", RampLowerFrameColor},
                    {"Left frame color for ramp bar", RampLeftFrameColor},
                    {"Right frame color for ramp bar", RampRightFrameColor},
                    {"Use border colors for ramp bar", RampUseBorderColors}
                }
            ScreensaverConfig.Add("Ramp", RampConfig)

            'StackBox config json object
            Dim StackBoxConfig As New JObject From {
                    {"Activate 255 Color Mode", StackBox255Colors},
                    {"Activate True Color Mode", StackBoxTrueColor},
                    {"Delay in Milliseconds", StackBoxDelay},
                    {"Minimum red color level", StackBoxMinimumRedColorLevel},
                    {"Minimum green color level", StackBoxMinimumGreenColorLevel},
                    {"Minimum blue color level", StackBoxMinimumBlueColorLevel},
                    {"Minimum color level", StackBoxMinimumColorLevel},
                    {"Maximum red color level", StackBoxMaximumRedColorLevel},
                    {"Maximum green color level", StackBoxMaximumGreenColorLevel},
                    {"Maximum blue color level", StackBoxMaximumBlueColorLevel},
                    {"Maximum color level", StackBoxMaximumColorLevel},
                    {"Fill the boxes", StackBoxFill}
                }
            ScreensaverConfig.Add("StackBox", StackBoxConfig)

            'Snaker config json object
            Dim SnakerConfig As New JObject From {
                    {"Activate 255 Color Mode", Snaker255Colors},
                    {"Activate True Color Mode", SnakerTrueColor},
                    {"Delay in Milliseconds", SnakerDelay},
                    {"Stage delay in milliseconds", SnakerStageDelay},
                    {"Minimum red color level", SnakerMinimumRedColorLevel},
                    {"Minimum green color level", SnakerMinimumGreenColorLevel},
                    {"Minimum blue color level", SnakerMinimumBlueColorLevel},
                    {"Minimum color level", SnakerMinimumColorLevel},
                    {"Maximum red color level", SnakerMaximumRedColorLevel},
                    {"Maximum green color level", SnakerMaximumGreenColorLevel},
                    {"Maximum blue color level", SnakerMaximumBlueColorLevel},
                    {"Maximum color level", SnakerMaximumColorLevel}
                }
            ScreensaverConfig.Add("Snaker", SnakerConfig)

            'BarRot config json object
            Dim BarRotConfig As New JObject From {
                    {"Activate 255 Color Mode", BarRot255Colors},
                    {"Activate True Color Mode", BarRotTrueColor},
                    {"Delay in Milliseconds", BarRotDelay},
                    {"Next ramp rot interval", BarRotNextRampDelay},
                    {"Upper left corner character for ramp bar", BarRotUpperLeftCornerChar},
                    {"Upper right corner character for ramp bar", BarRotUpperRightCornerChar},
                    {"Lower left corner character for ramp bar", BarRotLowerLeftCornerChar},
                    {"Lower right corner character for ramp bar", BarRotLowerRightCornerChar},
                    {"Upper frame character for ramp bar", BarRotUpperFrameChar},
                    {"Lower frame character for ramp bar", BarRotLowerFrameChar},
                    {"Left frame character for ramp bar", BarRotLeftFrameChar},
                    {"Right frame character for ramp bar", BarRotRightFrameChar},
                    {"Minimum red color level for start color", BarRotMinimumRedColorLevelStart},
                    {"Minimum green color level for start color", BarRotMinimumGreenColorLevelStart},
                    {"Minimum blue color level for start color", BarRotMinimumBlueColorLevelStart},
                    {"Maximum red color level for start color", BarRotMaximumRedColorLevelStart},
                    {"Maximum green color level for start color", BarRotMaximumGreenColorLevelStart},
                    {"Maximum blue color level for start color", BarRotMaximumBlueColorLevelStart},
                    {"Minimum red color level for end color", BarRotMinimumRedColorLevelEnd},
                    {"Minimum green color level for end color", BarRotMinimumGreenColorLevelEnd},
                    {"Minimum blue color level for end color", BarRotMinimumBlueColorLevelEnd},
                    {"Maximum red color level for end color", BarRotMaximumRedColorLevelEnd},
                    {"Maximum green color level for end color", BarRotMaximumGreenColorLevelEnd},
                    {"Maximum blue color level for end color", BarRotMaximumBlueColorLevelEnd},
                    {"Upper left corner color for ramp bar", BarRotUpperLeftCornerColor},
                    {"Upper right corner color for ramp bar", BarRotUpperRightCornerColor},
                    {"Lower left corner color for ramp bar", BarRotLowerLeftCornerColor},
                    {"Lower right corner color for ramp bar", BarRotLowerRightCornerColor},
                    {"Upper frame color for ramp bar", BarRotUpperFrameColor},
                    {"Lower frame color for ramp bar", BarRotLowerFrameColor},
                    {"Left frame color for ramp bar", BarRotLeftFrameColor},
                    {"Right frame color for ramp bar", BarRotRightFrameColor},
                    {"Use border colors for ramp bar", BarRotUseBorderColors}
                }
            ScreensaverConfig.Add("BarRot", BarRotConfig)

            'Fireworks config json object
            Dim FireworksConfig As New JObject From {
                    {"Activate 255 Color Mode", Fireworks255Colors},
                    {"Activate True Color Mode", FireworksTrueColor},
                    {"Delay in Milliseconds", FireworksDelay},
                    {"Firework explosion radius", FireworksRadius},
                    {"Minimum red color level", FireworksMinimumRedColorLevel},
                    {"Minimum green color level", FireworksMinimumGreenColorLevel},
                    {"Minimum blue color level", FireworksMinimumBlueColorLevel},
                    {"Minimum color level", FireworksMinimumColorLevel},
                    {"Maximum red color level", FireworksMaximumRedColorLevel},
                    {"Maximum green color level", FireworksMaximumGreenColorLevel},
                    {"Maximum blue color level", FireworksMaximumBlueColorLevel},
                    {"Maximum color level", FireworksMaximumColorLevel}
                }
            ScreensaverConfig.Add("Fireworks", FireworksConfig)

            'Figlet config json object
            Dim FigletConfig As New JObject From {
                    {"Activate 255 Color Mode", Figlet255Colors},
                    {"Activate True Color Mode", FigletTrueColor},
                    {"Delay in Milliseconds", FigletDelay},
                    {"Text Shown", FigletText},
                    {"Figlet font", FigletFont},
                    {"Minimum red color level", FigletMinimumRedColorLevel},
                    {"Minimum green color level", FigletMinimumGreenColorLevel},
                    {"Minimum blue color level", FigletMinimumBlueColorLevel},
                    {"Minimum color level", FigletMinimumColorLevel},
                    {"Maximum red color level", FigletMaximumRedColorLevel},
                    {"Maximum green color level", FigletMaximumGreenColorLevel},
                    {"Maximum blue color level", FigletMaximumBlueColorLevel},
                    {"Maximum color level", FigletMaximumColorLevel}
                }
            ScreensaverConfig.Add("Figlet", FigletConfig)

            'FlashText config json object
            Dim FlashTextConfig As New JObject From {
                    {"Activate 255 Color Mode", FlashText255Colors},
                    {"Activate True Color Mode", FlashTextTrueColor},
                    {"Delay in Milliseconds", FlashTextDelay},
                    {"Text Shown", FlashTextWrite},
                    {"Background color", If(New Color(FlashTextBackgroundColor).Type = ColorType.TrueColor, FlashTextBackgroundColor.EncloseByDoubleQuotes, FlashTextBackgroundColor)},
                    {"Minimum red color level", FlashTextMinimumRedColorLevel},
                    {"Minimum green color level", FlashTextMinimumGreenColorLevel},
                    {"Minimum blue color level", FlashTextMinimumBlueColorLevel},
                    {"Minimum color level", FlashTextMinimumColorLevel},
                    {"Maximum red color level", FlashTextMaximumRedColorLevel},
                    {"Maximum green color level", FlashTextMaximumGreenColorLevel},
                    {"Maximum blue color level", FlashTextMaximumBlueColorLevel},
                    {"Maximum color level", FlashTextMaximumColorLevel}
                }
            ScreensaverConfig.Add("FlashText", FlashTextConfig)

            'Noise config json object
            Dim NoiseConfig As New JObject From {
                    {"New Screen Delay in Milliseconds", NoiseNewScreenDelay},
                    {"Noise density", NoiseDensity}
                }
            ScreensaverConfig.Add("Noise", NoiseConfig)

            'PersonLookup config json object
            Dim PersonLookupConfig As New JObject From {
                    {"Delay in Milliseconds", PersonLookupDelay},
                    {"New Screen Delay in Milliseconds", PersonLookupLookedUpDelay},
                    {"Minimum names count", PersonLookupMinimumNames},
                    {"Maximum names count", PersonLookupMaximumNames},
                    {"Minimum age years count", PersonLookupMinimumAgeYears},
                    {"Maximum age years count", PersonLookupMaximumAgeYears}
                }
            ScreensaverConfig.Add("PersonLookup", PersonLookupConfig)

            'DateAndTime config json object
            Dim DateAndTimeConfig As New JObject From {
                    {"Activate 255 Color Mode", DateAndTime255Colors},
                    {"Activate True Color Mode", DateAndTimeTrueColor},
                    {"Delay in Milliseconds", DateAndTimeDelay},
                    {"Minimum red color level", DateAndTimeMinimumRedColorLevel},
                    {"Minimum green color level", DateAndTimeMinimumGreenColorLevel},
                    {"Minimum blue color level", DateAndTimeMinimumBlueColorLevel},
                    {"Minimum color level", DateAndTimeMinimumColorLevel},
                    {"Maximum red color level", DateAndTimeMaximumRedColorLevel},
                    {"Maximum green color level", DateAndTimeMaximumGreenColorLevel},
                    {"Maximum blue color level", DateAndTimeMaximumBlueColorLevel},
                    {"Maximum color level", DateAndTimeMaximumColorLevel}
                }
            ScreensaverConfig.Add("DateAndTime", DateAndTimeConfig)

            'Glitch config json object
            Dim GlitchConfig As New JObject From {
                    {"Delay in Milliseconds", GlitchDelay},
                    {"Glitch density", GlitchDensity}
                }
            ScreensaverConfig.Add("Glitch", GlitchConfig)

            'Indeterminate config json object
            Dim IndeterminateConfig As New JObject From {
                    {"Activate 255 Color Mode", Indeterminate255Colors},
                    {"Activate True Color Mode", IndeterminateTrueColor},
                    {"Delay in Milliseconds", IndeterminateDelay},
                    {"Upper left corner character for ramp bar", IndeterminateUpperLeftCornerChar},
                    {"Upper right corner character for ramp bar", IndeterminateUpperRightCornerChar},
                    {"Lower left corner character for ramp bar", IndeterminateLowerLeftCornerChar},
                    {"Lower right corner character for ramp bar", IndeterminateLowerRightCornerChar},
                    {"Upper frame character for ramp bar", IndeterminateUpperFrameChar},
                    {"Lower frame character for ramp bar", IndeterminateLowerFrameChar},
                    {"Left frame character for ramp bar", IndeterminateLeftFrameChar},
                    {"Right frame character for ramp bar", IndeterminateRightFrameChar},
                    {"Minimum red color level", IndeterminateMinimumRedColorLevel},
                    {"Minimum green color level", IndeterminateMinimumGreenColorLevel},
                    {"Minimum blue color level", IndeterminateMinimumBlueColorLevel},
                    {"Minimum color level", IndeterminateMinimumColorLevel},
                    {"Maximum red color level", IndeterminateMaximumRedColorLevel},
                    {"Maximum green color level", IndeterminateMaximumGreenColorLevel},
                    {"Maximum blue color level", IndeterminateMaximumBlueColorLevel},
                    {"Maximum color level", IndeterminateMaximumColorLevel},
                    {"Upper left corner color for ramp bar", IndeterminateUpperLeftCornerColor},
                    {"Upper right corner color for ramp bar", IndeterminateUpperRightCornerColor},
                    {"Lower left corner color for ramp bar", IndeterminateLowerLeftCornerColor},
                    {"Lower right corner color for ramp bar", IndeterminateLowerRightCornerColor},
                    {"Upper frame color for ramp bar", IndeterminateUpperFrameColor},
                    {"Lower frame color for ramp bar", IndeterminateLowerFrameColor},
                    {"Left frame color for ramp bar", IndeterminateLeftFrameColor},
                    {"Right frame color for ramp bar", IndeterminateRightFrameColor},
                    {"Use border colors for ramp bar", IndeterminateUseBorderColors}
                }
            ScreensaverConfig.Add("Indeterminate", IndeterminateConfig)

            'Pulse config json object
            Dim PulseConfig As New JObject From {
                    {"Delay in Milliseconds", PulseDelay},
                    {"Max Fade Steps", PulseMaxSteps},
                    {"Minimum red color level", PulseMinimumRedColorLevel},
                    {"Minimum green color level", PulseMinimumGreenColorLevel},
                    {"Minimum blue color level", PulseMinimumBlueColorLevel},
                    {"Maximum red color level", PulseMaximumRedColorLevel},
                    {"Maximum green color level", PulseMaximumGreenColorLevel},
                    {"Maximum blue color level", PulseMaximumBlueColorLevel}
                }
            ScreensaverConfig.Add("Pulse", PulseConfig)

            'BeatPulse config json object
            Dim BeatPulseConfig As New JObject From {
                    {"Activate 255 Color Mode", BeatPulse255Colors},
                    {"Activate True Color Mode", BeatPulseTrueColor},
                    {"Delay in Beats Per Minute", BeatPulseDelay},
                    {"Cycle Colors", BeatPulseCycleColors},
                    {"Beat Color", BeatPulseBeatColor},
                    {"Max Fade Steps", BeatPulseMaxSteps},
                    {"Minimum red color level", BeatPulseMinimumRedColorLevel},
                    {"Minimum green color level", BeatPulseMinimumGreenColorLevel},
                    {"Minimum blue color level", BeatPulseMinimumBlueColorLevel},
                    {"Minimum color level", BeatPulseMinimumColorLevel},
                    {"Maximum red color level", BeatPulseMaximumRedColorLevel},
                    {"Maximum green color level", BeatPulseMaximumGreenColorLevel},
                    {"Maximum blue color level", BeatPulseMaximumBlueColorLevel},
                    {"Maximum color level", BeatPulseMaximumColorLevel}
                }
            ScreensaverConfig.Add("BeatPulse", BeatPulseConfig)

            'EdgePulse config json object
            Dim EdgePulseConfig As New JObject From {
                    {"Delay in Milliseconds", EdgePulseDelay},
                    {"Max Fade Steps", EdgePulseMaxSteps},
                    {"Minimum red color level", EdgePulseMinimumRedColorLevel},
                    {"Minimum green color level", EdgePulseMinimumGreenColorLevel},
                    {"Minimum blue color level", EdgePulseMinimumBlueColorLevel},
                    {"Maximum red color level", EdgePulseMaximumRedColorLevel},
                    {"Maximum green color level", EdgePulseMaximumGreenColorLevel},
                    {"Maximum blue color level", EdgePulseMaximumBlueColorLevel}
                }
            ScreensaverConfig.Add("EdgePulse", EdgePulseConfig)

            'BeatEdgePulse config json object
            Dim BeatEdgePulseConfig As New JObject From {
                    {"Activate 255 Color Mode", BeatEdgePulse255Colors},
                    {"Activate True Color Mode", BeatEdgePulseTrueColor},
                    {"Delay in Beats Per Minute", BeatEdgePulseDelay},
                    {"Cycle Colors", BeatEdgePulseCycleColors},
                    {"Beat Color", BeatEdgePulseBeatColor},
                    {"Max Fade Steps", BeatEdgePulseMaxSteps},
                    {"Minimum red color level", BeatEdgePulseMinimumRedColorLevel},
                    {"Minimum green color level", BeatEdgePulseMinimumGreenColorLevel},
                    {"Minimum blue color level", BeatEdgePulseMinimumBlueColorLevel},
                    {"Minimum color level", BeatEdgePulseMinimumColorLevel},
                    {"Maximum red color level", BeatEdgePulseMaximumRedColorLevel},
                    {"Maximum green color level", BeatEdgePulseMaximumGreenColorLevel},
                    {"Maximum blue color level", BeatEdgePulseMaximumBlueColorLevel},
                    {"Maximum color level", BeatEdgePulseMaximumColorLevel}
                }
            ScreensaverConfig.Add("BeatEdgePulse", BeatEdgePulseConfig)

            'Add a screensaver config json object to Screensaver section
            ConfigurationObject.Add("Screensaver", ScreensaverConfig)

            'The Splash Section
            Dim SplashConfig As New JObject()

            'Simple config json object
            Dim SplashSimpleConfig As New JObject From {
                    {"Progress text location", SimpleProgressTextLocation}
                }
            SplashConfig.Add("Simple", SplashSimpleConfig)

            'Progress config json object
            Dim SplashProgressConfig As New JObject From {
                    {"Progress bar color", ProgressProgressColor},
                    {"Progress text location", ProgressProgressTextLocation}
                }
            SplashConfig.Add("Progress", SplashProgressConfig)

            'Add a splash config json object to Splash section
            ConfigurationObject.Add("Splash", SplashConfig)

            'Misc Section
            Dim MiscConfig As New JObject From {
                    {"Show Time/Date on Upper Right Corner", CornerTimeDate},
                    {"Marquee on startup", StartScroll},
                    {"Long Time and Date", LongTimeDate},
                    {"Preferred Unit for Temperature", PreferredUnit},
                    {"Enable text editor autosave", TextEdit_AutoSaveFlag},
                    {"Text editor autosave interval", TextEdit_AutoSaveInterval},
                    {"Wrap list outputs", WrapListOutputs},
                    {"Draw notification border", DrawBorderNotification},
                    {"Blacklisted mods", BlacklistedModsString},
                    {"Solver minimum number", SolverMinimumNumber},
                    {"Solver maximum number", SolverMaximumNumber},
                    {"Solver show input", SolverShowInput},
                    {"Upper left corner character for notification border", NotifyUpperLeftCornerChar},
                    {"Upper right corner character for notification border", NotifyUpperRightCornerChar},
                    {"Lower left corner character for notification border", NotifyLowerLeftCornerChar},
                    {"Lower right corner character for notification border", NotifyLowerRightCornerChar},
                    {"Upper frame character for notification border", NotifyUpperFrameChar},
                    {"Lower frame character for notification border", NotifyLowerFrameChar},
                    {"Left frame character for notification border", NotifyLeftFrameChar},
                    {"Right frame character for notification border", NotifyRightFrameChar},
                    {"Manual page information style", ManpageInfoStyle},
                    {"Default difficulty for SpeedPress", SpeedPressCurrentDifficulty},
                    {"Keypress timeout for SpeedPress", SpeedPressTimeout},
                    {"Show latest RSS headline on login", ShowHeadlineOnLogin},
                    {"RSS headline URL", RssHeadlineUrl},
                    {"Save all events and/or reminders destructively", SaveEventsRemindersDestructively},
                    {"Upper left corner character for RGB color wheel", WheelUpperLeftCornerChar},
                    {"Upper right corner character for RGB color wheel", WheelUpperRightCornerChar},
                    {"Lower left corner character for RGB color wheel", WheelLowerLeftCornerChar},
                    {"Lower right corner character for RGB color wheel", WheelLowerRightCornerChar},
                    {"Upper frame character for RGB color wheel", WheelUpperFrameChar},
                    {"Lower frame character for RGB color wheel", WheelLowerFrameChar},
                    {"Left frame character for RGB color wheel", WheelLeftFrameChar},
                    {"Right frame character for RGB color wheel", WheelRightFrameChar},
                    {"Default JSON formatting for JSON shell", JsonShell_Formatting},
                    {"Enable Figlet for timer", EnableFigletTimer},
                    {"Figlet font for timer", TimerFigletFont},
                    {"Show the commands count on help", ShowCommandsCount},
                    {"Show the shell commands count on help", ShowShellCommandsCount},
                    {"Show the mod commands count on help", ShowModCommandsCount},
                    {"Show the aliases count on help", ShowShellAliasesCount},
                    {"Password mask character", CurrentMask},
                    {"Upper left corner character for progress bars", ProgressUpperLeftCornerChar},
                    {"Upper right corner character for progress bars", ProgressUpperRightCornerChar},
                    {"Lower left corner character for progress bars", ProgressLowerLeftCornerChar},
                    {"Lower right corner character for progress bars", ProgressLowerRightCornerChar},
                    {"Upper frame character for progress bars", ProgressUpperFrameChar},
                    {"Lower frame character for progress bars", ProgressLowerFrameChar},
                    {"Left frame character for progress bars", ProgressLeftFrameChar},
                    {"Right frame character for progress bars", ProgressRightFrameChar},
                    {"Users count for love or hate comments", LoveOrHateUsersCount},
                    {"Input history enabled", InputHistoryEnabled},
                    {"Input clipboard enabled", InputClipboardEnabled},
                    {"Input undo enabled", InputUndoEnabled}
                }
            ConfigurationObject.Add("Misc", MiscConfig)
            Return ConfigurationObject
        End Function

        ''' <summary>
        ''' Creates the kernel configuration file
        ''' </summary>
        ''' <exception cref="Exceptions.ConfigException"></exception>
        Public Sub CreateConfig()
            CreateConfig(GetKernelPath(KernelPathType.Configuration))
        End Sub

        ''' <summary>
        ''' Creates the kernel configuration file with custom path
        ''' </summary>
        ''' <exception cref="Exceptions.ConfigException"></exception>
        Sub CreateConfig(ConfigPath As String)
            ThrowOnInvalidPath(ConfigPath)
            Dim ConfigurationObject As JObject = GetNewConfigObject()

            'Save Config
            File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(ConfigurationObject, Formatting.Indented))
            KernelEventManager.RaiseConfigSaved()
        End Sub

        ''' <summary>
        ''' Creates the kernel configuration file
        ''' </summary>
        ''' <returns>True if successful; False if unsuccessful.</returns>
        ''' <exception cref="Exceptions.ConfigException"></exception>
        Public Function TryCreateConfig() As Boolean
            Return TryCreateConfig(GetKernelPath(KernelPathType.Configuration))
        End Function

        ''' <summary>
        ''' Creates the kernel configuration file with custom path
        ''' </summary>
        ''' <returns>True if successful; False if unsuccessful.</returns>
        ''' <exception cref="Exceptions.ConfigException"></exception>
        Function TryCreateConfig(ConfigPath As String) As Boolean
            Try
                CreateConfig(ConfigPath)
                Return True
            Catch ex As Exception
                KernelEventManager.RaiseConfigSaveError(ex)
                WStkTrc(ex)
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Configures the kernel according to the kernel configuration file
        ''' </summary>
        ''' <exception cref="Exceptions.ConfigException"></exception>
        Public Sub ReadConfig()
            ReadConfig(GetKernelPath(KernelPathType.Configuration))
        End Sub

        ''' <summary>
        ''' Configures the kernel according to the kernel configuration file
        ''' </summary>
        ''' <returns>True if successful; False if unsuccessful</returns>
        ''' <exception cref="Exceptions.ConfigException"></exception>
        Public Function TryReadConfig() As Boolean
            Return TryReadConfig(GetKernelPath(KernelPathType.Configuration))
        End Function

        ''' <summary>
        ''' Configures the kernel according to the custom kernel configuration file
        ''' </summary>
        ''' <exception cref="Exceptions.ConfigException"></exception>
        Sub ReadConfig(ConfigPath As String)
            'Parse configuration. NOTE: Question marks between parentheses are for nullable types.
            ThrowOnInvalidPath(ConfigPath)
            InitializeConfigToken(ConfigPath)
            Wdbg(DebugLevel.I, "Config loaded with {0} sections", ConfigToken.Count)

            '----------------------------- Important configuration -----------------------------
            'Language
            LangChangeCulture = If(ConfigToken("General")?("Change Culture when Switching Languages"), False)
            If LangChangeCulture Then CurrentCult = New CultureInfo(If(ConfigToken("General")?("Culture") IsNot Nothing, ConfigToken("General")("Culture").ToString, "en-US"))
            SetLang(If(ConfigToken("General")?("Language"), "eng"))

            'Colored Shell
            Dim UncoloredDetected As Boolean = ConfigToken("Shell")?("Colored Shell") IsNot Nothing AndAlso Not ConfigToken("Shell")("Colored Shell").ToObject(Of Boolean)
            If UncoloredDetected Then
                Wdbg(DebugLevel.W, "Detected uncolored shell. Removing colors...")
                ApplyThemeFromResources("LinuxUncolored")
                ColoredShell = False
            End If

            '----------------------------- General configuration -----------------------------
            'Colors Section
            Wdbg(DebugLevel.I, "Loading colors...")
            If ColoredShell Then
                'We use New Color() to parse entered color. This is to ensure that the kernel can use the correct VT sequence.
                UserNameShellColor = New Color(If(ConfigToken("Colors")?("User Name Shell Color"), ConsoleColors.Green).ToString)
                HostNameShellColor = New Color(If(ConfigToken("Colors")?("Host Name Shell Color"), ConsoleColors.DarkGreen).ToString)
                ContKernelErrorColor = New Color(If(ConfigToken("Colors")?("Continuable Kernel Error Color"), ConsoleColors.Yellow).ToString)
                UncontKernelErrorColor = New Color(If(ConfigToken("Colors")?("Uncontinuable Kernel Error Color"), ConsoleColors.Red).ToString)
                NeutralTextColor = New Color(If(ConfigToken("Colors")?("Text Color"), ConsoleColors.Gray).ToString)
                LicenseColor = New Color(If(ConfigToken("Colors")?("License Color"), ConsoleColors.White).ToString)
                BackgroundColor = New Color(If(ConfigToken("Colors")?("Background Color"), ConsoleColors.Black).ToString)
                InputColor = New Color(If(ConfigToken("Colors")?("Input Color"), ConsoleColors.White).ToString)
                ListEntryColor = New Color(If(ConfigToken("Colors")?("List Entry Color"), ConsoleColors.DarkYellow).ToString)
                ListValueColor = New Color(If(ConfigToken("Colors")?("List Value Color"), ConsoleColors.DarkGray).ToString)
                StageColor = New Color(If(ConfigToken("Colors")?("Kernel Stage Color"), ConsoleColors.Green).ToString)
                ErrorColor = New Color(If(ConfigToken("Colors")?("Error Text Color"), ConsoleColors.Red).ToString)
                WarningColor = New Color(If(ConfigToken("Colors")?("Warning Text Color"), ConsoleColors.Yellow).ToString)
                OptionColor = New Color(If(ConfigToken("Colors")?("Option Color"), ConsoleColors.DarkYellow).ToString)
                BannerColor = New Color(If(ConfigToken("Colors")?("Banner Color"), ConsoleColors.Green).ToString)
                NotificationTitleColor = New Color(If(ConfigToken("Colors")?("Notification Title Color"), ConsoleColors.White).ToString)
                NotificationDescriptionColor = New Color(If(ConfigToken("Colors")?("Notification Description Color"), ConsoleColors.Gray).ToString)
                NotificationProgressColor = New Color(If(ConfigToken("Colors")?("Notification Progress Color"), ConsoleColors.DarkYellow).ToString)
                NotificationFailureColor = New Color(If(ConfigToken("Colors")?("Notification Failure Color"), ConsoleColors.Red).ToString)
                QuestionColor = New Color(If(ConfigToken("Colors")?("Question Color"), ConsoleColors.Yellow).ToString)
                SuccessColor = New Color(If(ConfigToken("Colors")?("Success Color"), ConsoleColors.Green).ToString)
                UserDollarColor = New Color(If(ConfigToken("Colors")?("User Dollar Color"), ConsoleColors.Gray).ToString)
                TipColor = New Color(If(ConfigToken("Colors")?("Tip Color"), ConsoleColors.Gray).ToString)
                SeparatorTextColor = New Color(If(ConfigToken("Colors")?("Separator Text Color"), ConsoleColors.White).ToString)
                SeparatorColor = New Color(If(ConfigToken("Colors")?("Separator Color"), ConsoleColors.Gray).ToString)
                ListTitleColor = New Color(If(ConfigToken("Colors")?("List Title Color"), ConsoleColors.White).ToString)
                DevelopmentWarningColor = New Color(If(ConfigToken("Colors")?("Development Warning Color"), ConsoleColors.Yellow).ToString)
                StageTimeColor = New Color(If(ConfigToken("Colors")?("Stage Time Color"), ConsoleColors.Gray).ToString)
                ColorTools.ProgressColor = New Color(If(ConfigToken("Colors")?("Progress Color"), ConsoleColors.DarkYellow).ToString)
                BackOptionColor = New Color(If(ConfigToken("Colors")?("Back Option Color"), ConsoleColors.DarkRed).ToString)
                LowPriorityBorderColor = New Color(If(ConfigToken("Colors")?("Low Priority Border Color"), ConsoleColors.White).ToString)
                MediumPriorityBorderColor = New Color(If(ConfigToken("Colors")?("Medium Priority Border Color"), ConsoleColors.Yellow).ToString)
                HighPriorityBorderColor = New Color(If(ConfigToken("Colors")?("High Priority Border Color"), ConsoleColors.Red).ToString)
                TableSeparatorColor = New Color(If(ConfigToken("Colors")?("Table Separator Color"), ConsoleColors.DarkGray).ToString)
                TableHeaderColor = New Color(If(ConfigToken("Colors")?("Table Header Color"), ConsoleColors.White).ToString)
                TableValueColor = New Color(If(ConfigToken("Colors")?("Table Value Color"), ConsoleColors.Gray).ToString)
                SelectedOptionColor = New Color(If(ConfigToken("Colors")?("Selected Option Color"), ConsoleColors.Yellow).ToString)
                AlternativeOptionColor = New Color(If(ConfigToken("Colors")?("Alternative Option Color"), ConsoleColors.DarkGreen).ToString)
                LoadBack()
            End If

            'General Section
            Wdbg(DebugLevel.I, "Parsing general section...")
            Maintenance = If(ConfigToken("General")?("Maintenance Mode"), False)
            ArgsOnBoot = If(ConfigToken("General")?("Prompt for Arguments on Boot"), False)
            CheckUpdateStart = If(ConfigToken("General")?("Check for Updates on Startup"), True)
            If Not String.IsNullOrWhiteSpace(ConfigToken("General")?("Custom Startup Banner")) Then CustomBanner = ConfigToken("General")?("Custom Startup Banner")
            ShowAppInfoOnBoot = If(ConfigToken("General")?("Show app information during boot"), True)
            ParseCommandLineArguments = If(ConfigToken("General")?("Parse command-line arguments"), True)
            ShowStageFinishTimes = If(ConfigToken("General")?("Show stage finish times"), False)
            StartKernelMods = If(ConfigToken("General")?("Start kernel modifications on boot"), True)
            ShowCurrentTimeBeforeLogin = If(ConfigToken("General")?("Show current time before login"), True)
            NotifyFaultsBoot = If(ConfigToken("General")?("Notify for any fault during boot"), True)
            ShowStackTraceOnKernelError = If(ConfigToken("General")?("Show stack trace on kernel error"), False)
            CheckDebugQuota = If(ConfigToken("General")?("Check debug quota"), True)
            AutoDownloadUpdate = If(ConfigToken("General")?("Automatically download updates"), True)
            EventDebug = If(ConfigToken("General")?("Enable event debugging"), False)
            NewWelcomeStyle = If(ConfigToken("General")?("New welcome banner"), True)
            EnableSplash = If(ConfigToken("General")?("Stylish splash screen"), True)
            SplashName = If(ConfigToken("General")?("Splash name"), "Simple")
            BannerFigletFont = If(ConfigToken("General")?("Banner figlet font"), "Banner")
            SimulateNoAPM = If(ConfigToken("General")?("Simulate No APM Mode"), False)

            'Login Section
            Wdbg(DebugLevel.I, "Parsing login section...")
            ClearOnLogin = If(ConfigToken("Login")?("Clear Screen on Log-in"), False)
            ShowMOTD = If(ConfigToken("Login")?("Show MOTD on Log-in"), True)
            ShowAvailableUsers = If(ConfigToken("Login")?("Show available usernames"), True)
            If Not String.IsNullOrWhiteSpace(ConfigToken("Login")?("Host Name")) Then HostName = ConfigToken("Login")?("Host Name")
            If Not String.IsNullOrWhiteSpace(ConfigToken("Login")?("MOTD Path")) And TryParsePath(ConfigToken("Login")?("MOTD Path")) Then MOTDFilePath = ConfigToken("Login")?("MOTD Path")
            If Not String.IsNullOrWhiteSpace(ConfigToken("Login")?("MAL Path")) And TryParsePath(ConfigToken("Login")?("MAL Path")) Then MALFilePath = ConfigToken("Login")?("MAL Path")
            UsernamePrompt = If(ConfigToken("Login")?("Username prompt style"), "")
            PasswordPrompt = If(ConfigToken("Login")?("Password prompt style"), "")
            ShowMAL = If(ConfigToken("Login")?("Show MAL on Log-in"), True)
            IncludeAnonymous = If(ConfigToken("Login")?("Include anonymous users"), False)
            IncludeDisabled = If(ConfigToken("Login")?("Include disabled users"), False)

            'Shell Section
            Wdbg(DebugLevel.I, "Parsing shell section...")
            SimHelp = If(ConfigToken("Shell")?("Simplified Help Command"), False)
            CurrentDir = If(ConfigToken("Shell")?("Current Directory"), HomePath)
            PathsToLookup = If(Not String.IsNullOrEmpty(ConfigToken("Shell")?("Lookup Directories")), ConfigToken("Shell")?("Lookup Directories").ToString.ReleaseDoubleQuotes, Environment.GetEnvironmentVariable("PATH"))
            ShellPromptStyle = If(ConfigToken("Shell")?("Prompt Style"), "")
            FTPShellPromptStyle = If(ConfigToken("Shell")?("FTP Prompt Style"), "")
            MailShellPromptStyle = If(ConfigToken("Shell")?("Mail Prompt Style"), "")
            SFTPShellPromptStyle = If(ConfigToken("Shell")?("SFTP Prompt Style"), "")
            RSSShellPromptStyle = If(ConfigToken("Shell")?("RSS Prompt Style"), "")
            TextEdit_PromptStyle = If(ConfigToken("Shell")?("Text Edit Prompt Style"), "")
            ZipShell_PromptStyle = If(ConfigToken("Shell")?("Zip Shell Prompt Style"), "")
            Test_PromptStyle = If(ConfigToken("Shell")?("Test Shell Prompt Style"), "")
            JsonShell_PromptStyle = If(ConfigToken("Shell")?("JSON Shell Prompt Style"), "")
            HexEdit_PromptStyle = If(ConfigToken("Shell")?("Hex Edit Prompt Style"), "")
            HTTPShellPromptStyle = If(ConfigToken("Shell")?("HTTP Shell Prompt Style"), "")
            RarShell_PromptStyle = If(ConfigToken("Shell")?("RAR Shell Prompt Style"), "")
            ProbeInjectedCommands = If(ConfigToken("Shell")?("Probe injected commands"), True)
            ColorWheelTrueColor = If(ConfigToken("Shell")?("Start color wheel in true color mode"), True)
            DefaultChoiceOutputType = If(ConfigToken("Shell")?("Default choice output type") IsNot Nothing, If([Enum].TryParse(ConfigToken("Shell")?("Default choice output type"), DefaultChoiceOutputType), DefaultChoiceOutputType, ChoiceOutputType.Modern), ChoiceOutputType.Modern)

            'Filesystem Section
            Wdbg(DebugLevel.I, "Parsing filesystem section...")
            DebugQuota = If(Integer.TryParse(ConfigToken("Filesystem")?("Debug Size Quota in Bytes"), 0), ConfigToken("Filesystem")?("Debug Size Quota in Bytes"), 1073741824)
            FullParseMode = If(ConfigToken("Filesystem")?("Size parse mode"), False)
            HiddenFiles = If(ConfigToken("Filesystem")?("Show Hidden Files"), False)
            SortMode = If(ConfigToken("Filesystem")?("Filesystem sort mode") IsNot Nothing, If([Enum].TryParse(ConfigToken("Filesystem")?("Filesystem sort mode"), SortMode), SortMode, FilesystemSortOptions.FullName), FilesystemSortOptions.FullName)
            SortDirection = If(ConfigToken("Filesystem")?("Filesystem sort direction") IsNot Nothing, If([Enum].TryParse(ConfigToken("Filesystem")?("Filesystem sort direction"), SortDirection), SortDirection, FilesystemSortDirection.Ascending), FilesystemSortDirection.Ascending)
            ShowFilesystemProgress = If(ConfigToken("Filesystem")?("Show progress on filesystem operations"), True)
            ShowFileDetailsList = If(ConfigToken("Filesystem")?("Show file details in list"), True)
            SuppressUnauthorizedMessages = If(ConfigToken("Filesystem")?("Suppress unauthorized messages"), True)
            PrintLineNumbers = If(ConfigToken("Filesystem")?("Print line numbers on printing file contents"), False)
            SortList = If(ConfigToken("Filesystem")?("Sort the list"), True)
            ShowTotalSizeInList = If(ConfigToken("Filesystem")?("Show total size in list"), False)

            'Hardware Section
            Wdbg(DebugLevel.I, "Parsing hardware section...")
            QuietHardwareProbe = If(ConfigToken("Hardware")?("Quiet Probe"), False)
            FullHardwareProbe = If(ConfigToken("Hardware")?("Full Probe"), True)
            VerboseHardwareProbe = If(ConfigToken("Hardware")?("Verbose Probe"), False)

            'Network Section
            Wdbg(DebugLevel.I, "Parsing network section...")
            DebugPort = If(Integer.TryParse(ConfigToken("Network")?("Debug Port"), 0), ConfigToken("Network")?("Debug Port"), 3014)
            DownloadRetries = If(Integer.TryParse(ConfigToken("Network")?("Download Retry Times"), 0), ConfigToken("Network")?("Download Retry Times"), 3)
            UploadRetries = If(Integer.TryParse(ConfigToken("Network")?("Upload Retry Times"), 0), ConfigToken("Network")?("Upload Retry Times"), 3)
            ShowProgress = If(ConfigToken("Network")?("Show progress bar while downloading or uploading from ""get"" or ""put"" command"), True)
            FTPLoggerUsername = If(ConfigToken("Network")?("Log FTP username"), False)
            FTPLoggerIP = If(ConfigToken("Network")?("Log FTP IP address"), False)
            FTPFirstProfileOnly = If(ConfigToken("Network")?("Return only first FTP profile"), False)
            ShowPreview = If(ConfigToken("Network")?("Show mail message preview"), False)
            RecordChatToDebugLog = If(ConfigToken("Network")?("Record chat to debug log"), True)
            SSHBanner = If(ConfigToken("Network")?("Show SSH banner"), False)
            RPCEnabled = If(ConfigToken("Network")?("Enable RPC"), True)
            RPCPort = If(Integer.TryParse(ConfigToken("Network")?("RPC Port"), 0), ConfigToken("Network")?("RPC Port"), 12345)
            FtpShowDetailsInList = If(ConfigToken("Network")?("Show file details in FTP list"), True)
            FtpUserPromptStyle = If(ConfigToken("Network")?("Username prompt style for FTP"), "")
            FtpPassPromptStyle = If(ConfigToken("Network")?("Password prompt style for FTP"), "")
            FtpUseFirstProfile = If(ConfigToken("Network")?("Use first FTP profile"), True)
            FtpNewConnectionsToSpeedDial = If(ConfigToken("Network")?("Add new connections to FTP speed dial"), True)
            FtpTryToValidateCertificate = If(ConfigToken("Network")?("Try to validate secure FTP certificates"), True)
            FtpShowMotd = If(ConfigToken("Network")?("Show FTP MOTD on connection"), True)
            FtpAlwaysAcceptInvalidCerts = If(ConfigToken("Network")?("Always accept invalid FTP certificates"), True)
            Mail_UserPromptStyle = If(ConfigToken("Network")?("Username prompt style for mail"), "")
            Mail_PassPromptStyle = If(ConfigToken("Network")?("Password prompt style for mail"), "")
            Mail_IMAPPromptStyle = If(ConfigToken("Network")?("IMAP prompt style for mail"), "")
            Mail_SMTPPromptStyle = If(ConfigToken("Network")?("SMTP prompt style for mail"), "")
            Mail_AutoDetectServer = If(ConfigToken("Network")?("Automatically detect mail server"), True)
            Mail_Debug = If(ConfigToken("Network")?("Enable mail debug"), False)
            Mail_NotifyNewMail = If(ConfigToken("Network")?("Notify for new mail messages"), True)
            Mail_GPGPromptStyle = If(ConfigToken("Network")?("GPG password prompt style for mail"), True)
            Mail_ImapPingInterval = If(Integer.TryParse(ConfigToken("Network")?("Send IMAP ping interval"), 0), ConfigToken("Network")?("Send IMAP ping interval"), 30000)
            Mail_SmtpPingInterval = If(Integer.TryParse(ConfigToken("Network")?("Send SMTP ping interval"), 0), ConfigToken("Network")?("Send SMTP ping interval"), 30000)
            Mail_TextFormat = If(ConfigToken("Network")?("Mail text format") IsNot Nothing, If([Enum].TryParse(ConfigToken("Network")?("Mail text format"), Mail_TextFormat), Mail_TextFormat, TextFormat.Plain), TextFormat.Plain)
            RDebugAutoStart = If(ConfigToken("Network")?("Automatically start remote debug on startup"), True)
            RDebugMessageFormat = If(ConfigToken("Network")?("Remote debug message format"), "")
            RSSFeedUrlPromptStyle = If(ConfigToken("Network")?("RSS feed URL prompt style"), "")
            RSSRefreshFeeds = If(ConfigToken("Network")?("Auto refresh RSS feed"), True)
            RSSRefreshInterval = If(Integer.TryParse(ConfigToken("Network")?("Auto refresh RSS feed interval"), 0), ConfigToken("Network")?("Auto refresh RSS feed interval"), 60000)
            SFTPShowDetailsInList = If(ConfigToken("Network")?("Show file details in SFTP list"), True)
            SFTPUserPromptStyle = If(ConfigToken("Network")?("Username prompt style for SFTP"), "")
            SFTPNewConnectionsToSpeedDial = If(ConfigToken("Network")?("Add new connections to SFTP speed dial"), True)
            PingTimeout = If(Integer.TryParse(ConfigToken("Network")?("Ping timeout"), 0), ConfigToken("Network")?("Ping timeout"), 60000)
            ExtensiveAdapterInformation = If(ConfigToken("Network")?("Show extensive adapter info"), True)
            GeneralNetworkInformation = If(ConfigToken("Network")?("Show general network information"), True)
            DownloadPercentagePrint = If(ConfigToken("Network")?("Download percentage text"), "")
            UploadPercentagePrint = If(ConfigToken("Network")?("Upload percentage text"), "")
            FtpRecursiveHashing = If(ConfigToken("Network")?("Recursive hashing for FTP"), False)
            Mail_MaxMessagesInPage = If(Integer.TryParse(ConfigToken("Network")?("Maximum number of e-mails in one page"), 0), ConfigToken("Network")?("Maximum number of e-mails in one page"), 10)
            Mail_ShowProgress = If(ConfigToken("Network")?("Show mail transfer progress"), False)
            Mail_ProgressStyle = If(ConfigToken("Network")?("Mail transfer progress"), "")
            Mail_ProgressStyleSingle = If(ConfigToken("Network")?("Mail transfer progress (single)"), "")
            DownloadNotificationProvoke = If(ConfigToken("Network")?("Show notification for download progress"), False)
            UploadNotificationProvoke = If(ConfigToken("Network")?("Show notification for upload progress"), False)
            RSSFetchTimeout = If(Integer.TryParse(ConfigToken("Network")?("RSS feed fetch timeout"), 0), ConfigToken("Network")?("RSS feed fetch timeout"), 60000)
            FtpVerifyRetryAttempts = If(Integer.TryParse(ConfigToken("Network")?("Verify retry attempts for FTP transmission"), 0), ConfigToken("Network")?("Verify retry attempts for FTP transmission"), 3)
            FtpConnectTimeout = If(Integer.TryParse(ConfigToken("Network")?("FTP connection timeout"), 0), ConfigToken("Network")?("FTP connection timeout"), 15000)
            FtpDataConnectTimeout = If(Integer.TryParse(ConfigToken("Network")?("FTP data connection timeout"), 0), ConfigToken("Network")?("FTP data connection timeout"), 15000)
            FtpProtocolVersions = If(ConfigToken("Network")?("FTP IP versions") IsNot Nothing, If([Enum].TryParse(ConfigToken("Network")?("FTP IP versions"), FtpProtocolVersions), FtpProtocolVersions, FtpIpVersion.ANY), FtpIpVersion.ANY)
            NotifyOnRemoteDebugConnectionError = If(ConfigToken("Network")?("Notify on remote debug connection error"), True)

            'Screensaver Section
            DefSaverName = If(ConfigToken("Screensaver")?("Screensaver"), "matrix")
            ScrnTimeout = If(Integer.TryParse(ConfigToken("Screensaver")?("Screensaver Timeout in ms"), 0), ConfigToken("Screensaver")?("Screensaver Timeout in ms"), 300000)
            ScreensaverDebug = If(ConfigToken("Screensaver")?("Enable screensaver debugging"), False)
            PasswordLock = If(ConfigToken("Screensaver")?("Ask for password after locking"), True)

            'Screensaver-specific settings go below:
            '> ColorMix
            ColorMix255Colors = If(ConfigToken("Screensaver")?("ColorMix")?("Activate 255 Color Mode"), False)
            ColorMixTrueColor = If(ConfigToken("Screensaver")?("ColorMix")?("Activate True Color Mode"), True)
            ColorMixDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("ColorMix")?("Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("ColorMix")?("Delay in Milliseconds"), 1)
            ColorMixBackgroundColor = New Color(If(ConfigToken("Screensaver")?("ColorMix")?("Background color"), ConsoleColors.Red).ToString).PlainSequence
            ColorMixMinimumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("ColorMix")?("Minimum red color level"), 0), ConfigToken("Screensaver")?("ColorMix")?("Minimum red color level"), 0)
            ColorMixMinimumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("ColorMix")?("Minimum green color level"), 0), ConfigToken("Screensaver")?("ColorMix")?("Minimum green color level"), 0)
            ColorMixMinimumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("ColorMix")?("Minimum blue color level"), 0), ConfigToken("Screensaver")?("ColorMix")?("Minimum blue color level"), 0)
            ColorMixMinimumColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("ColorMix")?("Minimum color level"), 0), ConfigToken("Screensaver")?("ColorMix")?("Minimum color level"), 0)
            ColorMixMaximumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("ColorMix")?("Maximum red color level"), 0), ConfigToken("Screensaver")?("ColorMix")?("Maximum red color level"), 255)
            ColorMixMaximumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("ColorMix")?("Maximum green color level"), 0), ConfigToken("Screensaver")?("ColorMix")?("Maximum green color level"), 255)
            ColorMixMaximumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("ColorMix")?("Maximum blue color level"), 0), ConfigToken("Screensaver")?("ColorMix")?("Maximum blue color level"), 255)
            ColorMixMaximumColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("ColorMix")?("Maximum color level"), 0), ConfigToken("Screensaver")?("ColorMix")?("Maximum color level"), 255)

            '> Disco
            Disco255Colors = If(ConfigToken("Screensaver")?("Disco")?("Activate 255 Color Mode"), False)
            DiscoTrueColor = If(ConfigToken("Screensaver")?("Disco")?("Activate True Color Mode"), True)
            DiscoCycleColors = If(ConfigToken("Screensaver")?("Disco")?("Cycle Colors"), False)
            DiscoDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("Disco")?("Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("Disco")?("Delay in Milliseconds"), 100)
            DiscoUseBeatsPerMinute = If(ConfigToken("Screensaver")?("Disco")?("Use Beats Per Minute"), False)
            DiscoEnableFedMode = If(ConfigToken("Screensaver")?("Disco")?("Enable Black and White Mode"), False)
            DiscoMinimumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Disco")?("Minimum red color level"), 0), ConfigToken("Screensaver")?("Disco")?("Minimum red color level"), 0)
            DiscoMinimumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Disco")?("Minimum green color level"), 0), ConfigToken("Screensaver")?("Disco")?("Minimum green color level"), 0)
            DiscoMinimumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Disco")?("Minimum blue color level"), 0), ConfigToken("Screensaver")?("Disco")?("Minimum blue color level"), 0)
            DiscoMinimumColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Disco")?("Minimum color level"), 0), ConfigToken("Screensaver")?("Disco")?("Minimum color level"), 0)
            DiscoMaximumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Disco")?("Maximum red color level"), 0), ConfigToken("Screensaver")?("Disco")?("Maximum red color level"), 255)
            DiscoMaximumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Disco")?("Maximum green color level"), 0), ConfigToken("Screensaver")?("Disco")?("Maximum green color level"), 255)
            DiscoMaximumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Disco")?("Maximum blue color level"), 0), ConfigToken("Screensaver")?("Disco")?("Maximum blue color level"), 255)
            DiscoMaximumColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Disco")?("Maximum color level"), 0), ConfigToken("Screensaver")?("Disco")?("Maximum color level"), 255)

            '> GlitterColor
            GlitterColor255Colors = If(ConfigToken("Screensaver")?("GlitterColor")?("Activate 255 Color Mode"), False)
            GlitterColorTrueColor = If(ConfigToken("Screensaver")?("GlitterColor")?("Activate True Color Mode"), True)
            GlitterColorDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("GlitterColor")?("Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("GlitterColor")?("Delay in Milliseconds"), 1)
            GlitterColorMinimumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("GlitterColor")?("Minimum red color level"), 0), ConfigToken("Screensaver")?("GlitterColor")?("Minimum red color level"), 0)
            GlitterColorMinimumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("GlitterColor")?("Minimum green color level"), 0), ConfigToken("Screensaver")?("GlitterColor")?("Minimum green color level"), 0)
            GlitterColorMinimumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("GlitterColor")?("Minimum blue color level"), 0), ConfigToken("Screensaver")?("GlitterColor")?("Minimum blue color level"), 0)
            GlitterColorMinimumColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("GlitterColor")?("Minimum color level"), 0), ConfigToken("Screensaver")?("GlitterColor")?("Minimum color level"), 0)
            GlitterColorMaximumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("GlitterColor")?("Maximum red color level"), 0), ConfigToken("Screensaver")?("GlitterColor")?("Maximum red color level"), 255)
            GlitterColorMaximumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("GlitterColor")?("Maximum green color level"), 0), ConfigToken("Screensaver")?("GlitterColor")?("Maximum green color level"), 255)
            GlitterColorMaximumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("GlitterColor")?("Maximum blue color level"), 0), ConfigToken("Screensaver")?("GlitterColor")?("Maximum blue color level"), 255)
            GlitterColorMaximumColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("GlitterColor")?("Maximum color level"), 0), ConfigToken("Screensaver")?("GlitterColor")?("Maximum color level"), 255)

            '> GlitterMatrix
            GlitterMatrixDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("GlitterMatrix")?("Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("GlitterMatrix")?("Delay in Milliseconds"), 1)
            GlitterMatrixBackgroundColor = New Color(If(ConfigToken("Screensaver")?("GlitterMatrix")?("Background color"), ConsoleColors.Black).ToString).PlainSequence
            GlitterMatrixForegroundColor = New Color(If(ConfigToken("Screensaver")?("GlitterMatrix")?("Foreground color"), ConsoleColors.Green).ToString).PlainSequence

            '> Lines
            Lines255Colors = If(ConfigToken("Screensaver")?("Lines")?("Activate 255 Color Mode"), False)
            LinesTrueColor = If(ConfigToken("Screensaver")?("Lines")?("Activate True Color Mode"), True)
            LinesDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("Lines")?("Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("Lines")?("Delay in Milliseconds"), 500)
            LinesLineChar = If(ConfigToken("Screensaver")?("Lines")?("Line character"), "-")
            LinesBackgroundColor = New Color(If(ConfigToken("Screensaver")?("Lines")?("Background color"), ConsoleColors.Black).ToString).PlainSequence
            LinesMinimumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Lines")?("Minimum red color level"), 0), ConfigToken("Screensaver")?("Lines")?("Minimum red color level"), 0)
            LinesMinimumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Lines")?("Minimum green color level"), 0), ConfigToken("Screensaver")?("Lines")?("Minimum green color level"), 0)
            LinesMinimumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Lines")?("Minimum blue color level"), 0), ConfigToken("Screensaver")?("Lines")?("Minimum blue color level"), 0)
            LinesMinimumColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Lines")?("Minimum color level"), 0), ConfigToken("Screensaver")?("Lines")?("Minimum color level"), 0)
            LinesMaximumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Lines")?("Maximum red color level"), 0), ConfigToken("Screensaver")?("Lines")?("Maximum red color level"), 255)
            LinesMaximumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Lines")?("Maximum green color level"), 0), ConfigToken("Screensaver")?("Lines")?("Maximum green color level"), 255)
            LinesMaximumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Lines")?("Maximum blue color level"), 0), ConfigToken("Screensaver")?("Lines")?("Maximum blue color level"), 255)
            LinesMaximumColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Lines")?("Maximum color level"), 0), ConfigToken("Screensaver")?("Lines")?("Maximum color level"), 255)

            '> Dissolve
            Dissolve255Colors = If(ConfigToken("Screensaver")?("Dissolve")?("Activate 255 Color Mode"), False)
            DissolveTrueColor = If(ConfigToken("Screensaver")?("Dissolve")?("Activate True Color Mode"), True)
            DissolveBackgroundColor = New Color(If(ConfigToken("Screensaver")?("Dissolve")?("Background color"), ConsoleColors.Black).ToString).PlainSequence
            DissolveMinimumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Dissolve")?("Minimum red color level"), 0), ConfigToken("Screensaver")?("Dissolve")?("Minimum red color level"), 0)
            DissolveMinimumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Dissolve")?("Minimum green color level"), 0), ConfigToken("Screensaver")?("Dissolve")?("Minimum green color level"), 0)
            DissolveMinimumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Dissolve")?("Minimum blue color level"), 0), ConfigToken("Screensaver")?("Dissolve")?("Minimum blue color level"), 0)
            DissolveMinimumColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Dissolve")?("Minimum color level"), 0), ConfigToken("Screensaver")?("Dissolve")?("Minimum color level"), 0)
            DissolveMaximumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Dissolve")?("Maximum red color level"), 0), ConfigToken("Screensaver")?("Dissolve")?("Maximum red color level"), 255)
            DissolveMaximumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Dissolve")?("Maximum green color level"), 0), ConfigToken("Screensaver")?("Dissolve")?("Maximum green color level"), 255)
            DissolveMaximumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Dissolve")?("Maximum blue color level"), 0), ConfigToken("Screensaver")?("Dissolve")?("Maximum blue color level"), 255)
            DissolveMaximumColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Dissolve")?("Maximum color level"), 0), ConfigToken("Screensaver")?("Dissolve")?("Maximum color level"), 255)

            '> BouncingBlock
            BouncingBlock255Colors = If(ConfigToken("Screensaver")?("BouncingBlock")?("Activate 255 Color Mode"), False)
            BouncingBlockTrueColor = If(ConfigToken("Screensaver")?("BouncingBlock")?("Activate True Color Mode"), True)
            BouncingBlockDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("BouncingBlock")?("Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("BouncingBlock")?("Delay in Milliseconds"), 10)
            BouncingBlockBackgroundColor = New Color(If(ConfigToken("Screensaver")?("BouncingBlock")?("Background color"), ConsoleColors.Black).ToString).PlainSequence
            BouncingBlockForegroundColor = New Color(If(ConfigToken("Screensaver")?("BouncingBlock")?("Foreground color"), ConsoleColors.White).ToString).PlainSequence
            BouncingBlockMinimumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("BouncingBlock")?("Minimum red color level"), 0), ConfigToken("Screensaver")?("BouncingBlock")?("Minimum red color level"), 0)
            BouncingBlockMinimumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("BouncingBlock")?("Minimum green color level"), 0), ConfigToken("Screensaver")?("BouncingBlock")?("Minimum green color level"), 0)
            BouncingBlockMinimumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("BouncingBlock")?("Minimum blue color level"), 0), ConfigToken("Screensaver")?("BouncingBlock")?("Minimum blue color level"), 0)
            BouncingBlockMinimumColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("BouncingBlock")?("Minimum color level"), 0), ConfigToken("Screensaver")?("BouncingBlock")?("Minimum color level"), 0)
            BouncingBlockMaximumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("BouncingBlock")?("Maximum red color level"), 0), ConfigToken("Screensaver")?("BouncingBlock")?("Maximum red color level"), 255)
            BouncingBlockMaximumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("BouncingBlock")?("Maximum green color level"), 0), ConfigToken("Screensaver")?("BouncingBlock")?("Maximum green color level"), 255)
            BouncingBlockMaximumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("BouncingBlock")?("Maximum blue color level"), 0), ConfigToken("Screensaver")?("BouncingBlock")?("Maximum blue color level"), 255)
            BouncingBlockMaximumColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("BouncingBlock")?("Maximum color level"), 0), ConfigToken("Screensaver")?("BouncingBlock")?("Maximum color level"), 255)

            '> BouncingText
            BouncingText255Colors = If(ConfigToken("Screensaver")?("BouncingText")?("Activate 255 Color Mode"), False)
            BouncingTextTrueColor = If(ConfigToken("Screensaver")?("BouncingText")?("Activate True Color Mode"), True)
            BouncingTextDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("BouncingText")?("Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("BouncingText")?("Delay in Milliseconds"), 10)
            BouncingTextWrite = If(ConfigToken("Screensaver")?("BouncingText")?("Text Shown"), "Kernel Simulator")
            BouncingTextBackgroundColor = New Color(If(ConfigToken("Screensaver")?("BouncingText")?("Background color"), ConsoleColors.Black).ToString).PlainSequence
            BouncingTextForegroundColor = New Color(If(ConfigToken("Screensaver")?("BouncingText")?("Foreground color"), ConsoleColors.White).ToString).PlainSequence
            BouncingTextMinimumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("BouncingText")?("Minimum red color level"), 0), ConfigToken("Screensaver")?("BouncingText")?("Minimum red color level"), 0)
            BouncingTextMinimumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("BouncingText")?("Minimum green color level"), 0), ConfigToken("Screensaver")?("BouncingText")?("Minimum green color level"), 0)
            BouncingTextMinimumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("BouncingText")?("Minimum blue color level"), 0), ConfigToken("Screensaver")?("BouncingText")?("Minimum blue color level"), 0)
            BouncingTextMinimumColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("BouncingText")?("Minimum color level"), 0), ConfigToken("Screensaver")?("BouncingText")?("Minimum color level"), 0)
            BouncingTextMaximumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("BouncingText")?("Maximum red color level"), 0), ConfigToken("Screensaver")?("BouncingText")?("Maximum red color level"), 255)
            BouncingTextMaximumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("BouncingText")?("Maximum green color level"), 0), ConfigToken("Screensaver")?("BouncingText")?("Maximum green color level"), 255)
            BouncingTextMaximumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("BouncingText")?("Maximum blue color level"), 0), ConfigToken("Screensaver")?("BouncingText")?("Maximum blue color level"), 255)
            BouncingTextMaximumColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("BouncingText")?("Maximum color level"), 0), ConfigToken("Screensaver")?("BouncingText")?("Maximum color level"), 255)

            '> ProgressClock
            ProgressClock255Colors = If(ConfigToken("Screensaver")?("ProgressClock")?("Activate 255 Color Mode"), False)
            ProgressClockTrueColor = If(ConfigToken("Screensaver")?("ProgressClock")?("Activate True Color Mode"), True)
            ProgressClockCycleColors = If(ConfigToken("Screensaver")?("ProgressClock")?("Cycle Colors"), True)
            ProgressClockSecondsProgressColor = If(ConfigToken("Screensaver")?("ProgressClock")?("Color of Seconds Bar"), 4)
            ProgressClockMinutesProgressColor = If(ConfigToken("Screensaver")?("ProgressClock")?("Color of Minutes Bar"), 5)
            ProgressClockHoursProgressColor = If(ConfigToken("Screensaver")?("ProgressClock")?("Color of Hours Bar"), 6)
            ProgressClockProgressColor = If(ConfigToken("Screensaver")?("ProgressClock")?("Color of Information"), 7)
            ProgressClockCycleColorsTicks = If(Integer.TryParse(ConfigToken("Screensaver")?("ProgressClock")?("Ticks to change color"), 0), ConfigToken("Screensaver")?("ProgressClock")?("Ticks to change color"), 20)
            ProgressClockDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("ProgressClock")?("Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("ProgressClock")?("Delay in Milliseconds"), 500)
            ProgressClockUpperLeftCornerCharHours = If(ConfigToken("Screensaver")?("ProgressClock")?("Upper left corner character for hours bar"), "╔")
            ProgressClockUpperLeftCornerCharMinutes = If(ConfigToken("Screensaver")?("ProgressClock")?("Upper left corner character for minutes bar"), "╔")
            ProgressClockUpperLeftCornerCharSeconds = If(ConfigToken("Screensaver")?("ProgressClock")?("Upper left corner character for seconds bar"), "╔")
            ProgressClockUpperRightCornerCharHours = If(ConfigToken("Screensaver")?("ProgressClock")?("Upper right corner character for hours bar"), "╗")
            ProgressClockUpperRightCornerCharMinutes = If(ConfigToken("Screensaver")?("ProgressClock")?("Upper right corner character for minutes bar"), "╗")
            ProgressClockUpperRightCornerCharSeconds = If(ConfigToken("Screensaver")?("ProgressClock")?("Upper right corner character for seconds bar"), "╗")
            ProgressClockLowerLeftCornerCharHours = If(ConfigToken("Screensaver")?("ProgressClock")?("Lower left corner character for hours bar"), "╚")
            ProgressClockLowerLeftCornerCharMinutes = If(ConfigToken("Screensaver")?("ProgressClock")?("Lower left corner character for minutes bar"), "╚")
            ProgressClockLowerLeftCornerCharSeconds = If(ConfigToken("Screensaver")?("ProgressClock")?("Lower left corner character for seconds bar"), "╚")
            ProgressClockLowerRightCornerCharHours = If(ConfigToken("Screensaver")?("ProgressClock")?("Lower right corner character for hours bar"), "╝")
            ProgressClockLowerRightCornerCharMinutes = If(ConfigToken("Screensaver")?("ProgressClock")?("Lower right corner character for minutes bar"), "╝")
            ProgressClockLowerRightCornerCharSeconds = If(ConfigToken("Screensaver")?("ProgressClock")?("Lower right corner character for seconds bar"), "╝")
            ProgressClockUpperFrameCharHours = If(ConfigToken("Screensaver")?("ProgressClock")?("Upper frame character for hours bar"), "═")
            ProgressClockUpperFrameCharMinutes = If(ConfigToken("Screensaver")?("ProgressClock")?("Upper frame character for minutes bar"), "═")
            ProgressClockUpperFrameCharSeconds = If(ConfigToken("Screensaver")?("ProgressClock")?("Upper frame character for seconds bar"), "═")
            ProgressClockLowerFrameCharHours = If(ConfigToken("Screensaver")?("ProgressClock")?("Lower frame character for hours bar"), "═")
            ProgressClockLowerFrameCharMinutes = If(ConfigToken("Screensaver")?("ProgressClock")?("Lower frame character for minutes bar"), "═")
            ProgressClockLowerFrameCharSeconds = If(ConfigToken("Screensaver")?("ProgressClock")?("Lower frame character for seconds bar"), "═")
            ProgressClockLeftFrameCharHours = If(ConfigToken("Screensaver")?("ProgressClock")?("Left frame character for hours bar"), "║")
            ProgressClockLeftFrameCharMinutes = If(ConfigToken("Screensaver")?("ProgressClock")?("Left frame character for minutes bar"), "║")
            ProgressClockLeftFrameCharSeconds = If(ConfigToken("Screensaver")?("ProgressClock")?("Left frame character for seconds bar"), "║")
            ProgressClockRightFrameCharHours = If(ConfigToken("Screensaver")?("ProgressClock")?("Right frame character for hours bar"), "║")
            ProgressClockRightFrameCharMinutes = If(ConfigToken("Screensaver")?("ProgressClock")?("Right frame character for minutes bar"), "║")
            ProgressClockRightFrameCharSeconds = If(ConfigToken("Screensaver")?("ProgressClock")?("Right frame character for seconds bar"), "║")
            ProgressClockInfoTextHours = If(ConfigToken("Screensaver")?("ProgressClock")?("Information text for hours"), "")
            ProgressClockInfoTextMinutes = If(ConfigToken("Screensaver")?("ProgressClock")?("Information text for minutes"), "")
            ProgressClockInfoTextSeconds = If(ConfigToken("Screensaver")?("ProgressClock")?("Information text for seconds"), "")
            ProgressClockMinimumRedColorLevelHours = If(Integer.TryParse(ConfigToken("Screensaver")?("ProgressClock")?("Minimum red color level for hours"), 0), ConfigToken("Screensaver")?("ProgressClock")?("Minimum red color level for hours"), 0)
            ProgressClockMinimumGreenColorLevelHours = If(Integer.TryParse(ConfigToken("Screensaver")?("ProgressClock")?("Minimum green color level for hours"), 0), ConfigToken("Screensaver")?("ProgressClock")?("Minimum green color level for hours"), 0)
            ProgressClockMinimumBlueColorLevelHours = If(Integer.TryParse(ConfigToken("Screensaver")?("ProgressClock")?("Minimum blue color level for hours"), 0), ConfigToken("Screensaver")?("ProgressClock")?("Minimum blue color level for hours"), 0)
            ProgressClockMinimumColorLevelHours = If(Integer.TryParse(ConfigToken("Screensaver")?("ProgressClock")?("Minimum color level for hours"), 0), ConfigToken("Screensaver")?("ProgressClock")?("Minimum color level for hours"), 0)
            ProgressClockMaximumRedColorLevelHours = If(Integer.TryParse(ConfigToken("Screensaver")?("ProgressClock")?("Maximum red color level for hours"), 0), ConfigToken("Screensaver")?("ProgressClock")?("Maximum red color level for hours"), 255)
            ProgressClockMaximumGreenColorLevelHours = If(Integer.TryParse(ConfigToken("Screensaver")?("ProgressClock")?("Maximum green color level for hours"), 0), ConfigToken("Screensaver")?("ProgressClock")?("Maximum green color level for hours"), 255)
            ProgressClockMaximumBlueColorLevelHours = If(Integer.TryParse(ConfigToken("Screensaver")?("ProgressClock")?("Maximum blue color level for hours"), 0), ConfigToken("Screensaver")?("ProgressClock")?("Maximum blue color level for hours"), 255)
            ProgressClockMaximumColorLevelHours = If(Integer.TryParse(ConfigToken("Screensaver")?("ProgressClock")?("Maximum color level for hours"), 0), ConfigToken("Screensaver")?("ProgressClock")?("Maximum color level for hours"), 255)
            ProgressClockMinimumRedColorLevelMinutes = If(Integer.TryParse(ConfigToken("Screensaver")?("ProgressClock")?("Minimum red color level for minutes"), 0), ConfigToken("Screensaver")?("ProgressClock")?("Minimum red color level for minutes"), 0)
            ProgressClockMinimumGreenColorLevelMinutes = If(Integer.TryParse(ConfigToken("Screensaver")?("ProgressClock")?("Minimum green color level for minutes"), 0), ConfigToken("Screensaver")?("ProgressClock")?("Minimum green color level for minutes"), 0)
            ProgressClockMinimumBlueColorLevelMinutes = If(Integer.TryParse(ConfigToken("Screensaver")?("ProgressClock")?("Minimum blue color level for minutes"), 0), ConfigToken("Screensaver")?("ProgressClock")?("Minimum blue color level for minutes"), 0)
            ProgressClockMinimumColorLevelMinutes = If(Integer.TryParse(ConfigToken("Screensaver")?("ProgressClock")?("Minimum color level for minutes"), 0), ConfigToken("Screensaver")?("ProgressClock")?("Minimum color level for minutes"), 0)
            ProgressClockMaximumRedColorLevelMinutes = If(Integer.TryParse(ConfigToken("Screensaver")?("ProgressClock")?("Maximum red color level for minutes"), 0), ConfigToken("Screensaver")?("ProgressClock")?("Maximum red color level for minutes"), 255)
            ProgressClockMaximumGreenColorLevelMinutes = If(Integer.TryParse(ConfigToken("Screensaver")?("ProgressClock")?("Maximum green color level for minutes"), 0), ConfigToken("Screensaver")?("ProgressClock")?("Maximum green color level for minutes"), 255)
            ProgressClockMaximumBlueColorLevelMinutes = If(Integer.TryParse(ConfigToken("Screensaver")?("ProgressClock")?("Maximum blue color level for minutes"), 0), ConfigToken("Screensaver")?("ProgressClock")?("Maximum blue color level for minutes"), 255)
            ProgressClockMaximumColorLevelMinutes = If(Integer.TryParse(ConfigToken("Screensaver")?("ProgressClock")?("Maximum color level for minutes"), 0), ConfigToken("Screensaver")?("ProgressClock")?("Maximum color level for minutes"), 255)
            ProgressClockMinimumRedColorLevelSeconds = If(Integer.TryParse(ConfigToken("Screensaver")?("ProgressClock")?("Minimum red color level for seconds"), 0), ConfigToken("Screensaver")?("ProgressClock")?("Minimum red color level for seconds"), 0)
            ProgressClockMinimumGreenColorLevelSeconds = If(Integer.TryParse(ConfigToken("Screensaver")?("ProgressClock")?("Minimum green color level for seconds"), 0), ConfigToken("Screensaver")?("ProgressClock")?("Minimum green color level for seconds"), 0)
            ProgressClockMinimumBlueColorLevelSeconds = If(Integer.TryParse(ConfigToken("Screensaver")?("ProgressClock")?("Minimum blue color level for seconds"), 0), ConfigToken("Screensaver")?("ProgressClock")?("Minimum blue color level for seconds"), 0)
            ProgressClockMinimumColorLevelSeconds = If(Integer.TryParse(ConfigToken("Screensaver")?("ProgressClock")?("Minimum color level for seconds"), 0), ConfigToken("Screensaver")?("ProgressClock")?("Minimum color level for seconds"), 0)
            ProgressClockMaximumRedColorLevelSeconds = If(Integer.TryParse(ConfigToken("Screensaver")?("ProgressClock")?("Maximum red color level for seconds"), 0), ConfigToken("Screensaver")?("ProgressClock")?("Maximum red color level for seconds"), 255)
            ProgressClockMaximumGreenColorLevelSeconds = If(Integer.TryParse(ConfigToken("Screensaver")?("ProgressClock")?("Maximum green color level for seconds"), 0), ConfigToken("Screensaver")?("ProgressClock")?("Maximum green color level for seconds"), 255)
            ProgressClockMaximumBlueColorLevelSeconds = If(Integer.TryParse(ConfigToken("Screensaver")?("ProgressClock")?("Maximum blue color level for seconds"), 0), ConfigToken("Screensaver")?("ProgressClock")?("Maximum blue color level for seconds"), 255)
            ProgressClockMaximumColorLevelSeconds = If(Integer.TryParse(ConfigToken("Screensaver")?("ProgressClock")?("Maximum color level for seconds"), 0), ConfigToken("Screensaver")?("ProgressClock")?("Maximum color level for seconds"), 255)
            ProgressClockMinimumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("ProgressClock")?("Minimum red color level"), 0), ConfigToken("Screensaver")?("ProgressClock")?("Minimum red color level"), 0)
            ProgressClockMinimumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("ProgressClock")?("Minimum green color level"), 0), ConfigToken("Screensaver")?("ProgressClock")?("Minimum green color level"), 0)
            ProgressClockMinimumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("ProgressClock")?("Minimum blue color level"), 0), ConfigToken("Screensaver")?("ProgressClock")?("Minimum blue color level"), 0)
            ProgressClockMinimumColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("ProgressClock")?("Minimum color level"), 0), ConfigToken("Screensaver")?("ProgressClock")?("Minimum color level"), 0)
            ProgressClockMaximumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("ProgressClock")?("Maximum red color level"), 0), ConfigToken("Screensaver")?("ProgressClock")?("Maximum red color level"), 255)
            ProgressClockMaximumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("ProgressClock")?("Maximum green color level"), 0), ConfigToken("Screensaver")?("ProgressClock")?("Maximum green color level"), 255)
            ProgressClockMaximumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("ProgressClock")?("Maximum blue color level"), 0), ConfigToken("Screensaver")?("ProgressClock")?("Maximum blue color level"), 255)
            ProgressClockMaximumColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("ProgressClock")?("Maximum color level"), 0), ConfigToken("Screensaver")?("ProgressClock")?("Maximum color level"), 255)

            '> Lighter
            Lighter255Colors = If(ConfigToken("Screensaver")?("Lighter")?("Activate 255 Color Mode"), False)
            LighterTrueColor = If(ConfigToken("Screensaver")?("Lighter")?("Activate True Color Mode"), True)
            LighterDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("Lighter")?("Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("Lighter")?("Delay in Milliseconds"), 100)
            LighterMaxPositions = If(Integer.TryParse(ConfigToken("Screensaver")?("Lighter")?("Max Positions Count"), 0), ConfigToken("Screensaver")?("Lighter")?("Max Positions Count"), 10)
            LighterBackgroundColor = New Color(If(ConfigToken("Screensaver")?("Lighter")?("Background color"), ConsoleColors.Black).ToString).PlainSequence
            LighterMinimumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Lighter")?("Minimum red color level"), 0), ConfigToken("Screensaver")?("Lighter")?("Minimum red color level"), 0)
            LighterMinimumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Lighter")?("Minimum green color level"), 0), ConfigToken("Screensaver")?("Lighter")?("Minimum green color level"), 0)
            LighterMinimumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Lighter")?("Minimum blue color level"), 0), ConfigToken("Screensaver")?("Lighter")?("Minimum blue color level"), 0)
            LighterMinimumColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Lighter")?("Minimum color level"), 0), ConfigToken("Screensaver")?("Lighter")?("Minimum color level"), 0)
            LighterMaximumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Lighter")?("Maximum red color level"), 0), ConfigToken("Screensaver")?("Lighter")?("Maximum red color level"), 255)
            LighterMaximumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Lighter")?("Maximum green color level"), 0), ConfigToken("Screensaver")?("Lighter")?("Maximum green color level"), 255)
            LighterMaximumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Lighter")?("Maximum blue color level"), 0), ConfigToken("Screensaver")?("Lighter")?("Maximum blue color level"), 255)
            LighterMaximumColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Lighter")?("Maximum color level"), 0), ConfigToken("Screensaver")?("Lighter")?("Maximum color level"), 255)

            '> Wipe
            Wipe255Colors = If(ConfigToken("Screensaver")?("Wipe")?("Activate 255 Color Mode"), False)
            WipeTrueColor = If(ConfigToken("Screensaver")?("Wipe")?("Activate True Color Mode"), True)
            WipeDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("Wipe")?("Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("Wipe")?("Delay in Milliseconds"), 10)
            WipeWipesNeededToChangeDirection = If(Integer.TryParse(ConfigToken("Screensaver")?("Wipe")?("Wipes to change direction"), 0), ConfigToken("Screensaver")?("Wipe")?("Wipes to change direction"), 10)
            WipeBackgroundColor = New Color(If(ConfigToken("Screensaver")?("Wipe")?("Background color"), ConsoleColors.Black).ToString).PlainSequence
            WipeMinimumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Wipe")?("Minimum red color level"), 0), ConfigToken("Screensaver")?("Wipe")?("Minimum red color level"), 0)
            WipeMinimumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Wipe")?("Minimum green color level"), 0), ConfigToken("Screensaver")?("Wipe")?("Minimum green color level"), 0)
            WipeMinimumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Wipe")?("Minimum blue color level"), 0), ConfigToken("Screensaver")?("Wipe")?("Minimum blue color level"), 0)
            WipeMinimumColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Wipe")?("Minimum color level"), 0), ConfigToken("Screensaver")?("Wipe")?("Minimum color level"), 0)
            WipeMaximumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Wipe")?("Maximum red color level"), 0), ConfigToken("Screensaver")?("Wipe")?("Maximum red color level"), 255)
            WipeMaximumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Wipe")?("Maximum green color level"), 0), ConfigToken("Screensaver")?("Wipe")?("Maximum green color level"), 255)
            WipeMaximumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Wipe")?("Maximum blue color level"), 0), ConfigToken("Screensaver")?("Wipe")?("Maximum blue color level"), 255)
            WipeMaximumColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Wipe")?("Maximum color level"), 0), ConfigToken("Screensaver")?("Wipe")?("Maximum color level"), 255)

            '> Fader
            FaderDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("Fader")?("Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("Fader")?("Delay in Milliseconds"), 50)
            FaderFadeOutDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("Fader")?("Fade Out Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("Fader")?("Fade Out Delay in Milliseconds"), 3000)
            FaderWrite = If(ConfigToken("Screensaver")?("Fader")?("Text Shown"), "Kernel Simulator")
            FaderMaxSteps = If(Integer.TryParse(ConfigToken("Screensaver")?("Fader")?("Max Fade Steps"), 0), ConfigToken("Screensaver")?("Fader")?("Max Fade Steps"), 25)
            FaderBackgroundColor = New Color(If(ConfigToken("Screensaver")?("Fader")?("Background color"), ConsoleColors.Black).ToString).PlainSequence
            FaderMinimumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Fader")?("Minimum red color level"), 0), ConfigToken("Screensaver")?("Fader")?("Minimum red color level"), 0)
            FaderMinimumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Fader")?("Minimum green color level"), 0), ConfigToken("Screensaver")?("Fader")?("Minimum green color level"), 0)
            FaderMinimumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Fader")?("Minimum blue color level"), 0), ConfigToken("Screensaver")?("Fader")?("Minimum blue color level"), 0)
            FaderMaximumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Fader")?("Maximum red color level"), 0), ConfigToken("Screensaver")?("Fader")?("Maximum red color level"), 255)
            FaderMaximumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Fader")?("Maximum green color level"), 0), ConfigToken("Screensaver")?("Fader")?("Maximum green color level"), 255)
            FaderMaximumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Fader")?("Maximum blue color level"), 0), ConfigToken("Screensaver")?("Fader")?("Maximum blue color level"), 255)

            '> FaderBack
            FaderBackDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("FaderBack")?("Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("FaderBack")?("Delay in Milliseconds"), 50)
            FaderBackFadeOutDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("FaderBack")?("Fade Out Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("FaderBack")?("Fade Out Delay in Milliseconds"), 3000)
            FaderBackMaxSteps = If(Integer.TryParse(ConfigToken("Screensaver")?("FaderBack")?("Max Fade Steps"), 0), ConfigToken("Screensaver")?("FaderBack")?("Max Fade Steps"), 25)
            FaderBackMinimumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("FaderBack")?("Minimum red color level"), 0), ConfigToken("Screensaver")?("FaderBack")?("Minimum red color level"), 0)
            FaderBackMinimumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("FaderBack")?("Minimum green color level"), 0), ConfigToken("Screensaver")?("FaderBack")?("Minimum green color level"), 0)
            FaderBackMinimumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("FaderBack")?("Minimum blue color level"), 0), ConfigToken("Screensaver")?("FaderBack")?("Minimum blue color level"), 0)
            FaderBackMaximumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("FaderBack")?("Maximum red color level"), 0), ConfigToken("Screensaver")?("FaderBack")?("Maximum red color level"), 255)
            FaderBackMaximumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("FaderBack")?("Maximum green color level"), 0), ConfigToken("Screensaver")?("FaderBack")?("Maximum green color level"), 255)
            FaderBackMaximumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("FaderBack")?("Maximum blue color level"), 0), ConfigToken("Screensaver")?("FaderBack")?("Maximum blue color level"), 255)

            '> BeatFader
            BeatFader255Colors = If(ConfigToken("Screensaver")?("BeatFader")?("Activate 255 Color Mode"), False)
            BeatFaderTrueColor = If(ConfigToken("Screensaver")?("BeatFader")?("Activate True Color Mode"), True)
            BeatFaderCycleColors = If(ConfigToken("Screensaver")?("BeatFader")?("Cycle Colors"), True)
            BeatFaderBeatColor = If(ConfigToken("Screensaver")?("BeatFader")?("Beat Color"), 17)
            BeatFaderDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("BeatFader")?("Delay in Beats Per Minute"), 0), ConfigToken("Screensaver")?("BeatFader")?("Delay in Beats Per Minute"), 120)
            BeatFaderMaxSteps = If(Integer.TryParse(ConfigToken("Screensaver")?("BeatFader")?("Max Fade Steps"), 0), ConfigToken("Screensaver")?("BeatFader")?("Max Fade Steps"), 25)
            BeatFaderMinimumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("BeatFader")?("Minimum red color level"), 0), ConfigToken("Screensaver")?("BeatFader")?("Minimum red color level"), 0)
            BeatFaderMinimumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("BeatFader")?("Minimum green color level"), 0), ConfigToken("Screensaver")?("BeatFader")?("Minimum green color level"), 0)
            BeatFaderMinimumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("BeatFader")?("Minimum blue color level"), 0), ConfigToken("Screensaver")?("BeatFader")?("Minimum blue color level"), 0)
            BeatFaderMinimumColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("BeatFader")?("Minimum color level"), 0), ConfigToken("Screensaver")?("BeatFader")?("Minimum color level"), 0)
            BeatFaderMaximumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("BeatFader")?("Maximum red color level"), 0), ConfigToken("Screensaver")?("BeatFader")?("Maximum red color level"), 255)
            BeatFaderMaximumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("BeatFader")?("Maximum green color level"), 0), ConfigToken("Screensaver")?("BeatFader")?("Maximum green color level"), 255)
            BeatFaderMaximumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("BeatFader")?("Maximum blue color level"), 0), ConfigToken("Screensaver")?("BeatFader")?("Maximum blue color level"), 255)
            BeatFaderMaximumColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("BeatFader")?("Maximum color level"), 0), ConfigToken("Screensaver")?("BeatFader")?("Maximum color level"), 255)

            '> Typo
            TypoDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("Typo")?("Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("Typo")?("Delay in Milliseconds"), 50)
            TypoWriteAgainDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("Typo")?("Write Again Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("Typo")?("Write Again Delay in Milliseconds"), 3000)
            TypoWrite = If(ConfigToken("Screensaver")?("Typo")?("Text Shown"), "Kernel Simulator")
            TypoWritingSpeedMin = If(Integer.TryParse(ConfigToken("Screensaver")?("Typo")?("Minimum writing speed in WPM"), 0), ConfigToken("Screensaver")?("Typo")?("Minimum writing speed in WPM"), 50)
            TypoWritingSpeedMax = If(Integer.TryParse(ConfigToken("Screensaver")?("Typo")?("Maximum writing speed in WPM"), 0), ConfigToken("Screensaver")?("Typo")?("Maximum writing speed in WPM"), 80)
            TypoMissStrikePossibility = If(Integer.TryParse(ConfigToken("Screensaver")?("Typo")?("Probability of typo in percent"), 0), ConfigToken("Screensaver")?("Typo")?("Probability of typo in percent"), 20)
            TypoMissPossibility = If(Integer.TryParse(ConfigToken("Screensaver")?("Typo")?("Probability of miss in percent"), 0), ConfigToken("Screensaver")?("Typo")?("Probability of miss in percent"), 10)
            TypoTextColor = New Color(If(ConfigToken("Screensaver")?("Typo")?("Text color"), ConsoleColors.White).ToString).PlainSequence

            '> Marquee
            Marquee255Colors = If(ConfigToken("Screensaver")?("Marquee")?("Activate 255 Color Mode"), False)
            MarqueeTrueColor = If(ConfigToken("Screensaver")?("Marquee")?("Activate True Color Mode"), True)
            MarqueeWrite = If(ConfigToken("Screensaver")?("Marquee")?("Text Shown"), "Kernel Simulator")
            MarqueeDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("Marquee")?("Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("Marquee")?("Delay in Milliseconds"), 10)
            MarqueeAlwaysCentered = If(ConfigToken("Screensaver")?("Marquee")?("Always Centered"), True)
            MarqueeUseConsoleAPI = If(ConfigToken("Screensaver")?("Marquee")?("Use Console API"), False)
            MarqueeMinimumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Marquee")?("Minimum red color level"), 0), ConfigToken("Screensaver")?("Marquee")?("Minimum red color level"), 0)
            MarqueeMinimumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Marquee")?("Minimum green color level"), 0), ConfigToken("Screensaver")?("Marquee")?("Minimum green color level"), 0)
            MarqueeMinimumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Marquee")?("Minimum blue color level"), 0), ConfigToken("Screensaver")?("Marquee")?("Minimum blue color level"), 0)
            MarqueeMinimumColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Marquee")?("Minimum color level"), 0), ConfigToken("Screensaver")?("Marquee")?("Minimum color level"), 0)
            MarqueeMaximumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Marquee")?("Maximum red color level"), 0), ConfigToken("Screensaver")?("Marquee")?("Maximum red color level"), 255)
            MarqueeMaximumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Marquee")?("Maximum green color level"), 0), ConfigToken("Screensaver")?("Marquee")?("Maximum green color level"), 255)
            MarqueeMaximumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Marquee")?("Maximum blue color level"), 0), ConfigToken("Screensaver")?("Marquee")?("Maximum blue color level"), 255)
            MarqueeMaximumColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Marquee")?("Maximum color level"), 0), ConfigToken("Screensaver")?("Marquee")?("Maximum color level"), 255)
            MarqueeBackgroundColor = New Color(If(ConfigToken("Screensaver")?("Marquee")?("Background color"), ConsoleColors.Black).ToString).PlainSequence

            '> Matrix
            MatrixDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("Matrix")?("Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("Matrix")?("Delay in Milliseconds"), 1)

            '> Linotypo
            LinotypoDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("Linotypo")?("Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("Linotypo")?("Delay in Milliseconds"), 50)
            LinotypoNewScreenDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("Linotypo")?("New Screen Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("Linotypo")?("New Screen Delay in Milliseconds"), 3000)
            LinotypoWrite = If(ConfigToken("Screensaver")?("Linotypo")?("Text Shown"), "Kernel Simulator")
            LinotypoWritingSpeedMin = If(Integer.TryParse(ConfigToken("Screensaver")?("Linotypo")?("Minimum writing speed in WPM"), 0), ConfigToken("Screensaver")?("Linotypo")?("Minimum writing speed in WPM"), 50)
            LinotypoWritingSpeedMax = If(Integer.TryParse(ConfigToken("Screensaver")?("Linotypo")?("Maximum writing speed in WPM"), 0), ConfigToken("Screensaver")?("Linotypo")?("Maximum writing speed in WPM"), 80)
            LinotypoMissStrikePossibility = If(Integer.TryParse(ConfigToken("Screensaver")?("Linotypo")?("Probability of typo in percent"), 0), ConfigToken("Screensaver")?("Linotypo")?("Probability of typo in percent"), 1)
            LinotypoTextColumns = If(Integer.TryParse(ConfigToken("Screensaver")?("Linotypo")?("Column Count"), 0), ConfigToken("Screensaver")?("Linotypo")?("Column Count"), 3)
            LinotypoEtaoinThreshold = If(Integer.TryParse(ConfigToken("Screensaver")?("Linotypo")?("Line Fill Threshold"), 0), ConfigToken("Screensaver")?("Linotypo")?("Line Fill Threshold"), 5)
            LinotypoEtaoinCappingPossibility = If(Integer.TryParse(ConfigToken("Screensaver")?("Linotypo")?("Line Fill Capping Probability in percent"), 0), ConfigToken("Screensaver")?("Linotypo")?("Line Fill Capping Probability in percent"), 5)
            LinotypoEtaoinType = If(ConfigToken("Screensaver")?("Linotypo")?("Line Fill Type") IsNot Nothing, If([Enum].TryParse(ConfigToken("Screensaver")?("Linotypo")?("Line Fill Type"), LinotypoEtaoinType), LinotypoEtaoinType, FillType.EtaoinPattern), FillType.EtaoinPattern)
            LinotypoMissPossibility = If(Integer.TryParse(ConfigToken("Screensaver")?("Linotypo")?("Probability of miss in percent"), 0), ConfigToken("Screensaver")?("Linotypo")?("Probability of miss in percent"), 10)
            LinotypoTextColor = New Color(If(ConfigToken("Screensaver")?("Linotypo")?("Text color"), ConsoleColors.White).ToString).PlainSequence

            '> Typewriter
            TypewriterDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("Typewriter")?("Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("Typewriter")?("Delay in Milliseconds"), 50)
            TypewriterNewScreenDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("Typewriter")?("New Screen Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("Typewriter")?("New Screen Delay in Milliseconds"), 3000)
            TypewriterWrite = If(ConfigToken("Screensaver")?("Typewriter")?("Text Shown"), "Kernel Simulator")
            TypewriterWritingSpeedMin = If(Integer.TryParse(ConfigToken("Screensaver")?("Typewriter")?("Minimum writing speed in WPM"), 0), ConfigToken("Screensaver")?("Typewriter")?("Minimum writing speed in WPM"), 50)
            TypewriterWritingSpeedMax = If(Integer.TryParse(ConfigToken("Screensaver")?("Typewriter")?("Maximum writing speed in WPM"), 0), ConfigToken("Screensaver")?("Typewriter")?("Maximum writing speed in WPM"), 80)
            TypewriterTextColor = New Color(If(ConfigToken("Screensaver")?("Typewriter")?("Text color"), ConsoleColors.White).ToString).PlainSequence

            '> FlashColor
            FlashColor255Colors = If(ConfigToken("Screensaver")?("FlashColor")?("Activate 255 Color Mode"), False)
            FlashColorTrueColor = If(ConfigToken("Screensaver")?("FlashColor")?("Activate True Color Mode"), True)
            FlashColorDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("FlashColor")?("Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("FlashColor")?("Delay in Milliseconds"), 1)
            FlashColorBackgroundColor = New Color(If(ConfigToken("Screensaver")?("FlashColor")?("Background color"), ConsoleColors.Black).ToString).PlainSequence
            FlashColorMinimumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("FlashColor")?("Minimum red color level"), 0), ConfigToken("Screensaver")?("FlashColor")?("Minimum red color level"), 0)
            FlashColorMinimumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("FlashColor")?("Minimum green color level"), 0), ConfigToken("Screensaver")?("FlashColor")?("Minimum green color level"), 0)
            FlashColorMinimumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("FlashColor")?("Minimum blue color level"), 0), ConfigToken("Screensaver")?("FlashColor")?("Minimum blue color level"), 0)
            FlashColorMinimumColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("FlashColor")?("Minimum color level"), 0), ConfigToken("Screensaver")?("FlashColor")?("Minimum color level"), 0)
            FlashColorMaximumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("FlashColor")?("Maximum red color level"), 0), ConfigToken("Screensaver")?("FlashColor")?("Maximum red color level"), 255)
            FlashColorMaximumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("FlashColor")?("Maximum green color level"), 0), ConfigToken("Screensaver")?("FlashColor")?("Maximum green color level"), 255)
            FlashColorMaximumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("FlashColor")?("Maximum blue color level"), 0), ConfigToken("Screensaver")?("FlashColor")?("Maximum blue color level"), 255)
            FlashColorMaximumColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("FlashColor")?("Maximum color level"), 0), ConfigToken("Screensaver")?("FlashColor")?("Maximum color level"), 255)

            '> SpotWrite
            SpotWriteDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("SpotWrite")?("Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("SpotWrite")?("Delay in Milliseconds"), 50)
            SpotWriteNewScreenDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("SpotWrite")?("New Screen Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("SpotWrite")?("New Screen Delay in Milliseconds"), 3000)
            SpotWriteWrite = If(ConfigToken("Screensaver")?("SpotWrite")?("Text Shown"), "Kernel Simulator")
            SpotWriteTextColor = New Color(If(ConfigToken("Screensaver")?("SpotWrite")?("Text color"), ConsoleColors.White).ToString).PlainSequence

            '> Ramp
            Ramp255Colors = If(ConfigToken("Screensaver")?("Ramp")?("Activate 255 Color Mode"), False)
            RampTrueColor = If(ConfigToken("Screensaver")?("Ramp")?("Activate True Color Mode"), True)
            RampDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("Ramp")?("Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("Ramp")?("Delay in Milliseconds"), 20)
            RampNextRampDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("Ramp")?("Next ramp interval"), 0), ConfigToken("Screensaver")?("Ramp")?("Next ramp interval"), 250)
            RampUpperLeftCornerChar = If(ConfigToken("Screensaver")?("Ramp")?("Upper left corner character for ramp bar"), "╔")
            RampUpperRightCornerChar = If(ConfigToken("Screensaver")?("Ramp")?("Upper right corner character for ramp bar"), "╗")
            RampLowerLeftCornerChar = If(ConfigToken("Screensaver")?("Ramp")?("Lower left corner character for ramp bar"), "╚")
            RampLowerRightCornerChar = If(ConfigToken("Screensaver")?("Ramp")?("Lower right corner character for ramp bar"), "╝")
            RampUpperFrameChar = If(ConfigToken("Screensaver")?("Ramp")?("Upper frame character for ramp bar"), "═")
            RampLowerFrameChar = If(ConfigToken("Screensaver")?("Ramp")?("Lower frame character for ramp bar"), "═")
            RampLeftFrameChar = If(ConfigToken("Screensaver")?("Ramp")?("Left frame character for ramp bar"), "║")
            RampRightFrameChar = If(ConfigToken("Screensaver")?("Ramp")?("Right frame character for ramp bar"), "║")
            RampMinimumRedColorLevelStart = If(Integer.TryParse(ConfigToken("Screensaver")?("Ramp")?("Minimum red color level for start color"), 0), ConfigToken("Screensaver")?("Ramp")?("Minimum red color level for start color"), 0)
            RampMinimumGreenColorLevelStart = If(Integer.TryParse(ConfigToken("Screensaver")?("Ramp")?("Minimum green color level for start color"), 0), ConfigToken("Screensaver")?("Ramp")?("Minimum green color level for start color"), 0)
            RampMinimumBlueColorLevelStart = If(Integer.TryParse(ConfigToken("Screensaver")?("Ramp")?("Minimum blue color level for start color"), 0), ConfigToken("Screensaver")?("Ramp")?("Minimum blue color level for start color"), 0)
            RampMinimumColorLevelStart = If(Integer.TryParse(ConfigToken("Screensaver")?("Ramp")?("Minimum color level for start color"), 0), ConfigToken("Screensaver")?("Ramp")?("Minimum color level for start color"), 0)
            RampMaximumRedColorLevelStart = If(Integer.TryParse(ConfigToken("Screensaver")?("Ramp")?("Maximum red color level for start color"), 0), ConfigToken("Screensaver")?("Ramp")?("Maximum red color level for start color"), 255)
            RampMaximumGreenColorLevelStart = If(Integer.TryParse(ConfigToken("Screensaver")?("Ramp")?("Maximum green color level for start color"), 0), ConfigToken("Screensaver")?("Ramp")?("Maximum green color level for start color"), 255)
            RampMaximumBlueColorLevelStart = If(Integer.TryParse(ConfigToken("Screensaver")?("Ramp")?("Maximum blue color level for start color"), 0), ConfigToken("Screensaver")?("Ramp")?("Maximum blue color level for start color"), 255)
            RampMaximumColorLevelStart = If(Integer.TryParse(ConfigToken("Screensaver")?("Ramp")?("Maximum color level for start color"), 0), ConfigToken("Screensaver")?("Ramp")?("Maximum color level for start color"), 255)
            RampMinimumRedColorLevelEnd = If(Integer.TryParse(ConfigToken("Screensaver")?("Ramp")?("Minimum red color level for end color"), 0), ConfigToken("Screensaver")?("Ramp")?("Minimum red color level for end color"), 0)
            RampMinimumGreenColorLevelEnd = If(Integer.TryParse(ConfigToken("Screensaver")?("Ramp")?("Minimum green color level for end color"), 0), ConfigToken("Screensaver")?("Ramp")?("Minimum green color level for end color"), 0)
            RampMinimumBlueColorLevelEnd = If(Integer.TryParse(ConfigToken("Screensaver")?("Ramp")?("Minimum blue color level for end color"), 0), ConfigToken("Screensaver")?("Ramp")?("Minimum blue color level for end color"), 0)
            RampMinimumColorLevelEnd = If(Integer.TryParse(ConfigToken("Screensaver")?("Ramp")?("Minimum color level for end color"), 0), ConfigToken("Screensaver")?("Ramp")?("Minimum color level for end color"), 0)
            RampMaximumRedColorLevelEnd = If(Integer.TryParse(ConfigToken("Screensaver")?("Ramp")?("Maximum red color level for end color"), 0), ConfigToken("Screensaver")?("Ramp")?("Maximum red color level for end color"), 255)
            RampMaximumGreenColorLevelEnd = If(Integer.TryParse(ConfigToken("Screensaver")?("Ramp")?("Maximum green color level for end color"), 0), ConfigToken("Screensaver")?("Ramp")?("Maximum green color level for end color"), 255)
            RampMaximumBlueColorLevelEnd = If(Integer.TryParse(ConfigToken("Screensaver")?("Ramp")?("Maximum blue color level for end color"), 0), ConfigToken("Screensaver")?("Ramp")?("Maximum blue color level for end color"), 255)
            RampMaximumColorLevelEnd = If(Integer.TryParse(ConfigToken("Screensaver")?("Ramp")?("Maximum color level for end color"), 0), ConfigToken("Screensaver")?("Ramp")?("Maximum color level for end color"), 255)
            RampUpperLeftCornerColor = New Color(If(ConfigToken("Screensaver")?("Ramp")?("Upper left corner color for ramp bar"), ConsoleColors.Gray).ToString).PlainSequence
            RampUpperRightCornerColor = New Color(If(ConfigToken("Screensaver")?("Ramp")?("Upper right corner color for ramp bar"), ConsoleColors.Gray).ToString).PlainSequence
            RampLowerLeftCornerColor = New Color(If(ConfigToken("Screensaver")?("Ramp")?("Lower left corner color for ramp bar"), ConsoleColors.Gray).ToString).PlainSequence
            RampLowerRightCornerColor = New Color(If(ConfigToken("Screensaver")?("Ramp")?("Lower right corner color for ramp bar"), ConsoleColors.Gray).ToString).PlainSequence
            RampUpperFrameColor = New Color(If(ConfigToken("Screensaver")?("Ramp")?("Upper frame color for ramp bar"), ConsoleColors.Gray).ToString).PlainSequence
            RampLowerFrameColor = New Color(If(ConfigToken("Screensaver")?("Ramp")?("Lower frame color for ramp bar"), ConsoleColors.Gray).ToString).PlainSequence
            RampLeftFrameColor = New Color(If(ConfigToken("Screensaver")?("Ramp")?("Left frame color for ramp bar"), ConsoleColors.Gray).ToString).PlainSequence
            RampRightFrameColor = New Color(If(ConfigToken("Screensaver")?("Ramp")?("Right frame color for ramp bar"), ConsoleColors.Gray).ToString).PlainSequence
            RampUseBorderColors = If(ConfigToken("Screensaver")?("Ramp")?("Use border colors for ramp bar"), False)

            '> StackBox
            StackBox255Colors = If(ConfigToken("Screensaver")?("StackBox")?("Activate 255 Color Mode"), False)
            StackBoxTrueColor = If(ConfigToken("Screensaver")?("StackBox")?("Activate True Color Mode"), True)
            StackBoxDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("StackBox")?("Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("StackBox")?("Delay in Milliseconds"), 10)
            StackBoxMinimumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("StackBox")?("Minimum red color level"), 0), ConfigToken("Screensaver")?("StackBox")?("Minimum red color level"), 0)
            StackBoxMinimumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("StackBox")?("Minimum green color level"), 0), ConfigToken("Screensaver")?("StackBox")?("Minimum green color level"), 0)
            StackBoxMinimumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("StackBox")?("Minimum blue color level"), 0), ConfigToken("Screensaver")?("StackBox")?("Minimum blue color level"), 0)
            StackBoxMinimumColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("StackBox")?("Minimum color level"), 0), ConfigToken("Screensaver")?("StackBox")?("Minimum color level"), 0)
            StackBoxMaximumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("StackBox")?("Maximum red color level"), 0), ConfigToken("Screensaver")?("StackBox")?("Maximum red color level"), 255)
            StackBoxMaximumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("StackBox")?("Maximum green color level"), 0), ConfigToken("Screensaver")?("StackBox")?("Maximum green color level"), 255)
            StackBoxMaximumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("StackBox")?("Maximum blue color level"), 0), ConfigToken("Screensaver")?("StackBox")?("Maximum blue color level"), 255)
            StackBoxMaximumColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("StackBox")?("Maximum color level"), 0), ConfigToken("Screensaver")?("StackBox")?("Maximum color level"), 255)
            StackBoxFill = If(ConfigToken("Screensaver")?("StackBox")?("Fill the boxes"), True)

            '> Snaker
            Snaker255Colors = If(ConfigToken("Screensaver")?("Snaker")?("Activate 255 Color Mode"), False)
            SnakerTrueColor = If(ConfigToken("Screensaver")?("Snaker")?("Activate True Color Mode"), True)
            SnakerDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("Snaker")?("Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("Snaker")?("Delay in Milliseconds"), 100)
            SnakerStageDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("Snaker")?("Stage delay in milliseconds"), 0), ConfigToken("Screensaver")?("Snaker")?("Stage delay in milliseconds"), 5000)
            SnakerMinimumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Snaker")?("Minimum red color level"), 0), ConfigToken("Screensaver")?("Snaker")?("Minimum red color level"), 0)
            SnakerMinimumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Snaker")?("Minimum green color level"), 0), ConfigToken("Screensaver")?("Snaker")?("Minimum green color level"), 0)
            SnakerMinimumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Snaker")?("Minimum blue color level"), 0), ConfigToken("Screensaver")?("Snaker")?("Minimum blue color level"), 0)
            SnakerMinimumColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Snaker")?("Minimum color level"), 0), ConfigToken("Screensaver")?("Snaker")?("Minimum color level"), 0)
            SnakerMaximumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Snaker")?("Maximum red color level"), 0), ConfigToken("Screensaver")?("Snaker")?("Maximum red color level"), 255)
            SnakerMaximumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Snaker")?("Maximum green color level"), 0), ConfigToken("Screensaver")?("Snaker")?("Maximum green color level"), 255)
            SnakerMaximumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Snaker")?("Maximum blue color level"), 0), ConfigToken("Screensaver")?("Snaker")?("Maximum blue color level"), 255)
            SnakerMaximumColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Snaker")?("Maximum color level"), 0), ConfigToken("Screensaver")?("Snaker")?("Maximum color level"), 255)

            '> BarRot
            BarRot255Colors = If(ConfigToken("Screensaver")?("BarRot")?("Activate 255 Color Mode"), False)
            BarRotTrueColor = If(ConfigToken("Screensaver")?("BarRot")?("Activate True Color Mode"), True)
            BarRotDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("BarRot")?("Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("BarRot")?("Delay in Milliseconds"), 10)
            BarRotNextRampDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("BarRot")?("Next ramp rot interval"), 0), ConfigToken("Screensaver")?("BarRot")?("Next ramp rot interval"), 250)
            BarRotUpperLeftCornerChar = If(ConfigToken("Screensaver")?("BarRot")?("Upper left corner character for ramp bar"), "╔")
            BarRotUpperRightCornerChar = If(ConfigToken("Screensaver")?("BarRot")?("Upper right corner character for ramp bar"), "╗")
            BarRotLowerLeftCornerChar = If(ConfigToken("Screensaver")?("BarRot")?("Lower left corner character for ramp bar"), "╚")
            BarRotLowerRightCornerChar = If(ConfigToken("Screensaver")?("BarRot")?("Lower right corner character for ramp bar"), "╝")
            BarRotUpperFrameChar = If(ConfigToken("Screensaver")?("BarRot")?("Upper frame character for ramp bar"), "═")
            BarRotLowerFrameChar = If(ConfigToken("Screensaver")?("BarRot")?("Lower frame character for ramp bar"), "═")
            BarRotLeftFrameChar = If(ConfigToken("Screensaver")?("BarRot")?("Left frame character for ramp bar"), "║")
            BarRotRightFrameChar = If(ConfigToken("Screensaver")?("BarRot")?("Right frame character for ramp bar"), "║")
            BarRotMinimumRedColorLevelStart = If(Integer.TryParse(ConfigToken("Screensaver")?("BarRot")?("Minimum red color level for start color"), 0), ConfigToken("Screensaver")?("BarRot")?("Minimum red color level for start color"), 0)
            BarRotMinimumGreenColorLevelStart = If(Integer.TryParse(ConfigToken("Screensaver")?("BarRot")?("Minimum green color level for start color"), 0), ConfigToken("Screensaver")?("BarRot")?("Minimum green color level for start color"), 0)
            BarRotMinimumBlueColorLevelStart = If(Integer.TryParse(ConfigToken("Screensaver")?("BarRot")?("Minimum blue color level for start color"), 0), ConfigToken("Screensaver")?("BarRot")?("Minimum blue color level for start color"), 0)
            BarRotMaximumRedColorLevelStart = If(Integer.TryParse(ConfigToken("Screensaver")?("BarRot")?("Maximum red color level for start color"), 0), ConfigToken("Screensaver")?("BarRot")?("Maximum red color level for start color"), 255)
            BarRotMaximumGreenColorLevelStart = If(Integer.TryParse(ConfigToken("Screensaver")?("BarRot")?("Maximum green color level for start color"), 0), ConfigToken("Screensaver")?("BarRot")?("Maximum green color level for start color"), 255)
            BarRotMaximumBlueColorLevelStart = If(Integer.TryParse(ConfigToken("Screensaver")?("BarRot")?("Maximum blue color level for start color"), 0), ConfigToken("Screensaver")?("BarRot")?("Maximum blue color level for start color"), 255)
            BarRotMinimumRedColorLevelEnd = If(Integer.TryParse(ConfigToken("Screensaver")?("BarRot")?("Minimum red color level for end color"), 0), ConfigToken("Screensaver")?("BarRot")?("Minimum red color level for end color"), 0)
            BarRotMinimumGreenColorLevelEnd = If(Integer.TryParse(ConfigToken("Screensaver")?("BarRot")?("Minimum green color level for end color"), 0), ConfigToken("Screensaver")?("BarRot")?("Minimum green color level for end color"), 0)
            BarRotMinimumBlueColorLevelEnd = If(Integer.TryParse(ConfigToken("Screensaver")?("BarRot")?("Minimum blue color level for end color"), 0), ConfigToken("Screensaver")?("BarRot")?("Minimum blue color level for end color"), 0)
            BarRotMaximumRedColorLevelEnd = If(Integer.TryParse(ConfigToken("Screensaver")?("BarRot")?("Maximum red color level for end color"), 0), ConfigToken("Screensaver")?("BarRot")?("Maximum red color level for end color"), 255)
            BarRotMaximumGreenColorLevelEnd = If(Integer.TryParse(ConfigToken("Screensaver")?("BarRot")?("Maximum green color level for end color"), 0), ConfigToken("Screensaver")?("BarRot")?("Maximum green color level for end color"), 255)
            BarRotMaximumBlueColorLevelEnd = If(Integer.TryParse(ConfigToken("Screensaver")?("BarRot")?("Maximum blue color level for end color"), 0), ConfigToken("Screensaver")?("BarRot")?("Maximum blue color level for end color"), 255)
            BarRotUpperLeftCornerColor = New Color(If(ConfigToken("Screensaver")?("BarRot")?("Upper left corner color for ramp bar"), ConsoleColors.Gray).ToString).PlainSequence
            BarRotUpperRightCornerColor = New Color(If(ConfigToken("Screensaver")?("BarRot")?("Upper right corner color for ramp bar"), ConsoleColors.Gray).ToString).PlainSequence
            BarRotLowerLeftCornerColor = New Color(If(ConfigToken("Screensaver")?("BarRot")?("Lower left corner color for ramp bar"), ConsoleColors.Gray).ToString).PlainSequence
            BarRotLowerRightCornerColor = New Color(If(ConfigToken("Screensaver")?("BarRot")?("Lower right corner color for ramp bar"), ConsoleColors.Gray).ToString).PlainSequence
            BarRotUpperFrameColor = New Color(If(ConfigToken("Screensaver")?("BarRot")?("Upper frame color for ramp bar"), ConsoleColors.Gray).ToString).PlainSequence
            BarRotLowerFrameColor = New Color(If(ConfigToken("Screensaver")?("BarRot")?("Lower frame color for ramp bar"), ConsoleColors.Gray).ToString).PlainSequence
            BarRotLeftFrameColor = New Color(If(ConfigToken("Screensaver")?("BarRot")?("Left frame color for ramp bar"), ConsoleColors.Gray).ToString).PlainSequence
            BarRotRightFrameColor = New Color(If(ConfigToken("Screensaver")?("BarRot")?("Right frame color for ramp bar"), ConsoleColors.Gray).ToString).PlainSequence
            BarRotUseBorderColors = If(ConfigToken("Screensaver")?("BarRot")?("Use border colors for ramp bar"), False)

            '> Fireworks
            Fireworks255Colors = If(ConfigToken("Screensaver")?("Fireworks")?("Activate 255 Color Mode"), False)
            FireworksTrueColor = If(ConfigToken("Screensaver")?("Fireworks")?("Activate True Color Mode"), True)
            FireworksDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("Fireworks")?("Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("Fireworks")?("Delay in Milliseconds"), 10)
            FireworksRadius = If(Integer.TryParse(ConfigToken("Screensaver")?("Fireworks")?("Firework explosion radius"), 0), ConfigToken("Screensaver")?("Fireworks")?("Firework explosion radius"), 5)
            FireworksMinimumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Fireworks")?("Minimum red color level"), 0), ConfigToken("Screensaver")?("Fireworks")?("Minimum red color level"), 0)
            FireworksMinimumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Fireworks")?("Minimum green color level"), 0), ConfigToken("Screensaver")?("Fireworks")?("Minimum green color level"), 0)
            FireworksMinimumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Fireworks")?("Minimum blue color level"), 0), ConfigToken("Screensaver")?("Fireworks")?("Minimum blue color level"), 0)
            FireworksMinimumColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Fireworks")?("Minimum color level"), 0), ConfigToken("Screensaver")?("Fireworks")?("Minimum color level"), 0)
            FireworksMaximumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Fireworks")?("Maximum red color level"), 0), ConfigToken("Screensaver")?("Fireworks")?("Maximum red color level"), 255)
            FireworksMaximumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Fireworks")?("Maximum green color level"), 0), ConfigToken("Screensaver")?("Fireworks")?("Maximum green color level"), 255)
            FireworksMaximumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Fireworks")?("Maximum blue color level"), 0), ConfigToken("Screensaver")?("Fireworks")?("Maximum blue color level"), 255)
            FireworksMaximumColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Fireworks")?("Maximum color level"), 0), ConfigToken("Screensaver")?("Fireworks")?("Maximum color level"), 255)

            '> Figlet
            Figlet255Colors = If(ConfigToken("Screensaver")?("Figlet")?("Activate 255 Color Mode"), False)
            FigletTrueColor = If(ConfigToken("Screensaver")?("Figlet")?("Activate True Color Mode"), True)
            FigletDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("Figlet")?("Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("Figlet")?("Delay in Milliseconds"), 10)
            FigletText = If(ConfigToken("Screensaver")?("Figlet")?("Text Shown"), "Kernel Simulator")
            FigletFont = If(ConfigToken("Screensaver")?("Figlet")?("Figlet font"), "Small")
            FigletMinimumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Figlet")?("Minimum red color level"), 0), ConfigToken("Screensaver")?("Figlet")?("Minimum red color level"), 0)
            FigletMinimumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Figlet")?("Minimum green color level"), 0), ConfigToken("Screensaver")?("Figlet")?("Minimum green color level"), 0)
            FigletMinimumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Figlet")?("Minimum blue color level"), 0), ConfigToken("Screensaver")?("Figlet")?("Minimum blue color level"), 0)
            FigletMinimumColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Figlet")?("Minimum color level"), 0), ConfigToken("Screensaver")?("Figlet")?("Minimum color level"), 0)
            FigletMaximumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Figlet")?("Maximum red color level"), 0), ConfigToken("Screensaver")?("Figlet")?("Maximum red color level"), 255)
            FigletMaximumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Figlet")?("Maximum green color level"), 0), ConfigToken("Screensaver")?("Figlet")?("Maximum green color level"), 255)
            FigletMaximumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Figlet")?("Maximum blue color level"), 0), ConfigToken("Screensaver")?("Figlet")?("Maximum blue color level"), 255)
            FigletMaximumColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Figlet")?("Maximum color level"), 0), ConfigToken("Screensaver")?("Figlet")?("Maximum color level"), 255)

            '> FlashText
            FlashText255Colors = If(ConfigToken("Screensaver")?("FlashText")?("Activate 255 Color Mode"), False)
            FlashTextTrueColor = If(ConfigToken("Screensaver")?("FlashText")?("Activate True Color Mode"), True)
            FlashTextDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("FlashText")?("Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("FlashText")?("Delay in Milliseconds"), 10)
            FlashTextWrite = If(ConfigToken("Screensaver")?("FlashText")?("Text Shown"), "Kernel Simulator")
            FlashTextBackgroundColor = New Color(If(ConfigToken("Screensaver")?("FlashText")?("Background color"), ConsoleColors.Black).ToString).PlainSequence
            FlashTextMinimumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("FlashText")?("Minimum red color level"), 0), ConfigToken("Screensaver")?("FlashText")?("Minimum red color level"), 0)
            FlashTextMinimumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("FlashText")?("Minimum green color level"), 0), ConfigToken("Screensaver")?("FlashText")?("Minimum green color level"), 0)
            FlashTextMinimumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("FlashText")?("Minimum blue color level"), 0), ConfigToken("Screensaver")?("FlashText")?("Minimum blue color level"), 0)
            FlashTextMinimumColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("FlashText")?("Minimum color level"), 0), ConfigToken("Screensaver")?("FlashText")?("Minimum color level"), 0)
            FlashTextMaximumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("FlashText")?("Maximum red color level"), 0), ConfigToken("Screensaver")?("FlashText")?("Maximum red color level"), 255)
            FlashTextMaximumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("FlashText")?("Maximum green color level"), 0), ConfigToken("Screensaver")?("FlashText")?("Maximum green color level"), 255)
            FlashTextMaximumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("FlashText")?("Maximum blue color level"), 0), ConfigToken("Screensaver")?("FlashText")?("Maximum blue color level"), 255)
            FlashTextMaximumColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("FlashText")?("Maximum color level"), 0), ConfigToken("Screensaver")?("FlashText")?("Maximum color level"), 255)

            '> Noise
            NoiseNewScreenDelay = If(ConfigToken("Screensaver")?("Noise")?("New Screen Delay in Milliseconds"), 5000)
            NoiseDensity = If(ConfigToken("Screensaver")?("Noise")?("Noise density"), 40)

            '> PersonLookup
            PersonLookupDelay = If(ConfigToken("Screensaver")?("PersonLookup")?("Delay in Milliseconds"), 75)
            PersonLookupLookedUpDelay = If(ConfigToken("Screensaver")?("PersonLookup")?("New Screen Delay in Milliseconds"), 10000)
            PersonLookupMinimumNames = If(ConfigToken("Screensaver")?("PersonLookup")?("Minimum names count"), 10)
            PersonLookupMaximumNames = If(ConfigToken("Screensaver")?("PersonLookup")?("Maximum names count"), 1000)
            PersonLookupMinimumAgeYears = If(ConfigToken("Screensaver")?("PersonLookup")?("Minimum age years count"), 18)
            PersonLookupMaximumAgeYears = If(ConfigToken("Screensaver")?("PersonLookup")?("Maximum age years count"), 100)

            '> DateAndTime
            DateAndTime255Colors = If(ConfigToken("Screensaver")?("DateAndTime")?("Activate 255 Color Mode"), False)
            DateAndTimeTrueColor = If(ConfigToken("Screensaver")?("DateAndTime")?("Activate True Color Mode"), True)
            DateAndTimeDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("DateAndTime")?("Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("DateAndTime")?("Delay in Milliseconds"), 1000)
            DateAndTimeMinimumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("DateAndTime")?("Minimum red color level"), 0), ConfigToken("Screensaver")?("DateAndTime")?("Minimum red color level"), 0)
            DateAndTimeMinimumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("DateAndTime")?("Minimum green color level"), 0), ConfigToken("Screensaver")?("DateAndTime")?("Minimum green color level"), 0)
            DateAndTimeMinimumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("DateAndTime")?("Minimum blue color level"), 0), ConfigToken("Screensaver")?("DateAndTime")?("Minimum blue color level"), 0)
            DateAndTimeMinimumColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("DateAndTime")?("Minimum color level"), 0), ConfigToken("Screensaver")?("DateAndTime")?("Minimum color level"), 0)
            DateAndTimeMaximumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("DateAndTime")?("Maximum red color level"), 0), ConfigToken("Screensaver")?("DateAndTime")?("Maximum red color level"), 255)
            DateAndTimeMaximumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("DateAndTime")?("Maximum green color level"), 0), ConfigToken("Screensaver")?("DateAndTime")?("Maximum green color level"), 255)
            DateAndTimeMaximumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("DateAndTime")?("Maximum blue color level"), 0), ConfigToken("Screensaver")?("DateAndTime")?("Maximum blue color level"), 255)
            DateAndTimeMaximumColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("DateAndTime")?("Maximum color level"), 0), ConfigToken("Screensaver")?("DateAndTime")?("Maximum color level"), 255)

            '> Glitch
            GlitchDelay = If(ConfigToken("Screensaver")?("Glitch")?("Delay in Milliseconds"), 10)
            GlitchDensity = If(ConfigToken("Screensaver")?("Glitch")?("Glitch density"), 40)

            '> Indeterminate
            Indeterminate255Colors = If(ConfigToken("Screensaver")?("Indeterminate")?("Activate 255 Color Mode"), False)
            IndeterminateTrueColor = If(ConfigToken("Screensaver")?("Indeterminate")?("Activate True Color Mode"), True)
            IndeterminateDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("Indeterminate")?("Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("Indeterminate")?("Delay in Milliseconds"), 20)
            IndeterminateUpperLeftCornerChar = If(ConfigToken("Screensaver")?("Indeterminate")?("Upper left corner character for ramp bar"), "╔")
            IndeterminateUpperRightCornerChar = If(ConfigToken("Screensaver")?("Indeterminate")?("Upper right corner character for ramp bar"), "╗")
            IndeterminateLowerLeftCornerChar = If(ConfigToken("Screensaver")?("Indeterminate")?("Lower left corner character for ramp bar"), "╚")
            IndeterminateLowerRightCornerChar = If(ConfigToken("Screensaver")?("Indeterminate")?("Lower right corner character for ramp bar"), "╝")
            IndeterminateUpperFrameChar = If(ConfigToken("Screensaver")?("Indeterminate")?("Upper frame character for ramp bar"), "═")
            IndeterminateLowerFrameChar = If(ConfigToken("Screensaver")?("Indeterminate")?("Lower frame character for ramp bar"), "═")
            IndeterminateLeftFrameChar = If(ConfigToken("Screensaver")?("Indeterminate")?("Left frame character for ramp bar"), "║")
            IndeterminateRightFrameChar = If(ConfigToken("Screensaver")?("Indeterminate")?("Right frame character for ramp bar"), "║")
            IndeterminateMinimumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Indeterminate")?("Minimum red color level"), 0), ConfigToken("Screensaver")?("Indeterminate")?("Minimum red color level"), 0)
            IndeterminateMinimumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Indeterminate")?("Minimum green color level"), 0), ConfigToken("Screensaver")?("Indeterminate")?("Minimum green color level"), 0)
            IndeterminateMinimumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Indeterminate")?("Minimum blue color level"), 0), ConfigToken("Screensaver")?("Indeterminate")?("Minimum blue color level"), 0)
            IndeterminateMinimumColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Indeterminate")?("Minimum color level"), 0), ConfigToken("Screensaver")?("Indeterminate")?("Minimum color level"), 0)
            IndeterminateMaximumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Indeterminate")?("Maximum red color level"), 0), ConfigToken("Screensaver")?("Indeterminate")?("Maximum red color level"), 255)
            IndeterminateMaximumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Indeterminate")?("Maximum green color level"), 0), ConfigToken("Screensaver")?("Indeterminate")?("Maximum green color level"), 255)
            IndeterminateMaximumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Indeterminate")?("Maximum blue color level"), 0), ConfigToken("Screensaver")?("Indeterminate")?("Maximum blue color level"), 255)
            IndeterminateMaximumColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Indeterminate")?("Maximum color level"), 0), ConfigToken("Screensaver")?("Indeterminate")?("Maximum color level"), 255)
            IndeterminateUpperLeftCornerColor = New Color(If(ConfigToken("Screensaver")?("Indeterminate")?("Upper left corner color for ramp bar"), ConsoleColors.Gray).ToString).PlainSequence
            IndeterminateUpperRightCornerColor = New Color(If(ConfigToken("Screensaver")?("Indeterminate")?("Upper right corner color for ramp bar"), ConsoleColors.Gray).ToString).PlainSequence
            IndeterminateLowerLeftCornerColor = New Color(If(ConfigToken("Screensaver")?("Indeterminate")?("Lower left corner color for ramp bar"), ConsoleColors.Gray).ToString).PlainSequence
            IndeterminateLowerRightCornerColor = New Color(If(ConfigToken("Screensaver")?("Indeterminate")?("Lower right corner color for ramp bar"), ConsoleColors.Gray).ToString).PlainSequence
            IndeterminateUpperFrameColor = New Color(If(ConfigToken("Screensaver")?("Indeterminate")?("Upper frame color for ramp bar"), ConsoleColors.Gray).ToString).PlainSequence
            IndeterminateLowerFrameColor = New Color(If(ConfigToken("Screensaver")?("Indeterminate")?("Lower frame color for ramp bar"), ConsoleColors.Gray).ToString).PlainSequence
            IndeterminateLeftFrameColor = New Color(If(ConfigToken("Screensaver")?("Indeterminate")?("Left frame color for ramp bar"), ConsoleColors.Gray).ToString).PlainSequence
            IndeterminateRightFrameColor = New Color(If(ConfigToken("Screensaver")?("Indeterminate")?("Right frame color for ramp bar"), ConsoleColors.Gray).ToString).PlainSequence
            IndeterminateUseBorderColors = If(ConfigToken("Screensaver")?("Indeterminate")?("Use border colors for ramp bar"), False)

            '> Pulse
            PulseDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("Pulse")?("Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("Pulse")?("Delay in Milliseconds"), 50)
            PulseMaxSteps = If(Integer.TryParse(ConfigToken("Screensaver")?("Pulse")?("Max Fade Steps"), 0), ConfigToken("Screensaver")?("Pulse")?("Max Fade Steps"), 25)
            PulseMinimumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Pulse")?("Minimum red color level"), 0), ConfigToken("Screensaver")?("Pulse")?("Minimum red color level"), 0)
            PulseMinimumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Pulse")?("Minimum green color level"), 0), ConfigToken("Screensaver")?("Pulse")?("Minimum green color level"), 0)
            PulseMinimumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Pulse")?("Minimum blue color level"), 0), ConfigToken("Screensaver")?("Pulse")?("Minimum blue color level"), 0)
            PulseMaximumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Pulse")?("Maximum red color level"), 0), ConfigToken("Screensaver")?("Pulse")?("Maximum red color level"), 255)
            PulseMaximumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Pulse")?("Maximum green color level"), 0), ConfigToken("Screensaver")?("Pulse")?("Maximum green color level"), 255)
            PulseMaximumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("Pulse")?("Maximum blue color level"), 0), ConfigToken("Screensaver")?("Pulse")?("Maximum blue color level"), 255)

            '> BeatPulse
            BeatPulse255Colors = If(ConfigToken("Screensaver")?("BeatPulse")?("Activate 255 Color Mode"), False)
            BeatPulseTrueColor = If(ConfigToken("Screensaver")?("BeatPulse")?("Activate True Color Mode"), True)
            BeatPulseCycleColors = If(ConfigToken("Screensaver")?("BeatPulse")?("Cycle Colors"), True)
            BeatPulseBeatColor = If(ConfigToken("Screensaver")?("BeatPulse")?("Beat Color"), 17)
            BeatPulseDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("BeatPulse")?("Delay in Beats Per Minute"), 0), ConfigToken("Screensaver")?("BeatPulse")?("Delay in Beats Per Minute"), 120)
            BeatPulseMaxSteps = If(Integer.TryParse(ConfigToken("Screensaver")?("BeatPulse")?("Max Fade Steps"), 0), ConfigToken("Screensaver")?("BeatPulse")?("Max Fade Steps"), 25)
            BeatPulseMinimumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("BeatPulse")?("Minimum red color level"), 0), ConfigToken("Screensaver")?("BeatPulse")?("Minimum red color level"), 0)
            BeatPulseMinimumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("BeatPulse")?("Minimum green color level"), 0), ConfigToken("Screensaver")?("BeatPulse")?("Minimum green color level"), 0)
            BeatPulseMinimumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("BeatPulse")?("Minimum blue color level"), 0), ConfigToken("Screensaver")?("BeatPulse")?("Minimum blue color level"), 0)
            BeatPulseMinimumColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("BeatPulse")?("Minimum color level"), 0), ConfigToken("Screensaver")?("BeatPulse")?("Minimum color level"), 0)
            BeatPulseMaximumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("BeatPulse")?("Maximum red color level"), 0), ConfigToken("Screensaver")?("BeatPulse")?("Maximum red color level"), 255)
            BeatPulseMaximumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("BeatPulse")?("Maximum green color level"), 0), ConfigToken("Screensaver")?("BeatPulse")?("Maximum green color level"), 255)
            BeatPulseMaximumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("BeatPulse")?("Maximum blue color level"), 0), ConfigToken("Screensaver")?("BeatPulse")?("Maximum blue color level"), 255)
            BeatPulseMaximumColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("BeatPulse")?("Maximum color level"), 0), ConfigToken("Screensaver")?("BeatPulse")?("Maximum color level"), 255)

            '> EdgePulse
            EdgePulseDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("EdgePulse")?("Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("EdgePulse")?("Delay in Milliseconds"), 50)
            EdgePulseMaxSteps = If(Integer.TryParse(ConfigToken("Screensaver")?("EdgePulse")?("Max Fade Steps"), 0), ConfigToken("Screensaver")?("EdgePulse")?("Max Fade Steps"), 25)
            EdgePulseMinimumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("EdgePulse")?("Minimum red color level"), 0), ConfigToken("Screensaver")?("EdgePulse")?("Minimum red color level"), 0)
            EdgePulseMinimumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("EdgePulse")?("Minimum green color level"), 0), ConfigToken("Screensaver")?("EdgePulse")?("Minimum green color level"), 0)
            EdgePulseMinimumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("EdgePulse")?("Minimum blue color level"), 0), ConfigToken("Screensaver")?("EdgePulse")?("Minimum blue color level"), 0)
            EdgePulseMaximumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("EdgePulse")?("Maximum red color level"), 0), ConfigToken("Screensaver")?("EdgePulse")?("Maximum red color level"), 255)
            EdgePulseMaximumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("EdgePulse")?("Maximum green color level"), 0), ConfigToken("Screensaver")?("EdgePulse")?("Maximum green color level"), 255)
            EdgePulseMaximumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("EdgePulse")?("Maximum blue color level"), 0), ConfigToken("Screensaver")?("EdgePulse")?("Maximum blue color level"), 255)

            '> BeatEdgePulse
            BeatEdgePulse255Colors = If(ConfigToken("Screensaver")?("BeatEdgePulse")?("Activate 255 Color Mode"), False)
            BeatEdgePulseTrueColor = If(ConfigToken("Screensaver")?("BeatEdgePulse")?("Activate True Color Mode"), True)
            BeatEdgePulseCycleColors = If(ConfigToken("Screensaver")?("BeatEdgePulse")?("Cycle Colors"), True)
            BeatEdgePulseBeatColor = If(ConfigToken("Screensaver")?("BeatEdgePulse")?("Beat Color"), 17)
            BeatEdgePulseDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("BeatEdgePulse")?("Delay in Beats Per Minute"), 0), ConfigToken("Screensaver")?("BeatEdgePulse")?("Delay in Beats Per Minute"), 120)
            BeatEdgePulseMaxSteps = If(Integer.TryParse(ConfigToken("Screensaver")?("BeatEdgePulse")?("Max Fade Steps"), 0), ConfigToken("Screensaver")?("BeatEdgePulse")?("Max Fade Steps"), 25)
            BeatEdgePulseMinimumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("BeatEdgePulse")?("Minimum red color level"), 0), ConfigToken("Screensaver")?("BeatEdgePulse")?("Minimum red color level"), 0)
            BeatEdgePulseMinimumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("BeatEdgePulse")?("Minimum green color level"), 0), ConfigToken("Screensaver")?("BeatEdgePulse")?("Minimum green color level"), 0)
            BeatEdgePulseMinimumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("BeatEdgePulse")?("Minimum blue color level"), 0), ConfigToken("Screensaver")?("BeatEdgePulse")?("Minimum blue color level"), 0)
            BeatEdgePulseMinimumColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("BeatEdgePulse")?("Minimum color level"), 0), ConfigToken("Screensaver")?("BeatEdgePulse")?("Minimum color level"), 0)
            BeatEdgePulseMaximumRedColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("BeatEdgePulse")?("Maximum red color level"), 0), ConfigToken("Screensaver")?("BeatEdgePulse")?("Maximum red color level"), 255)
            BeatEdgePulseMaximumGreenColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("BeatEdgePulse")?("Maximum green color level"), 0), ConfigToken("Screensaver")?("BeatEdgePulse")?("Maximum green color level"), 255)
            BeatEdgePulseMaximumBlueColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("BeatEdgePulse")?("Maximum blue color level"), 0), ConfigToken("Screensaver")?("BeatEdgePulse")?("Maximum blue color level"), 255)
            BeatEdgePulseMaximumColorLevel = If(Integer.TryParse(ConfigToken("Screensaver")?("BeatEdgePulse")?("Maximum color level"), 0), ConfigToken("Screensaver")?("BeatEdgePulse")?("Maximum color level"), 255)

            'Splash Section - Splash-specific settings go below:
            '> Simple
            SimpleProgressTextLocation = If(ConfigToken("Splash")?("Simple")?("Progress text location") IsNot Nothing, If([Enum].TryParse(ConfigToken("Splash")?("Simple")?("Progress text location"), SimpleProgressTextLocation), SimpleProgressTextLocation, TextLocation.Top), TextLocation.Top)

            '> Progress
            ProgressProgressColor = New Color(If(ConfigToken("Splash")?("Progress")?("Progress bar color").ToString, ColorTools.ProgressColor.PlainSequence)).PlainSequence
            ProgressProgressTextLocation = If(ConfigToken("Splash")?("Progress")?("Progress text location") IsNot Nothing, If([Enum].TryParse(ConfigToken("Splash")?("Progress")?("Progress text location"), ProgressProgressTextLocation), ProgressProgressTextLocation, TextLocation.Top), TextLocation.Top)

            'Misc Section
            Wdbg(DebugLevel.I, "Parsing misc section...")
            CornerTimeDate = If(ConfigToken("Misc")?("Show Time/Date on Upper Right Corner"), False)
            StartScroll = If(ConfigToken("Misc")?("Marquee on startup"), True)
            LongTimeDate = If(ConfigToken("Misc")?("Long Time and Date"), True)
            PreferredUnit = If(ConfigToken("Misc")?("Preferred Unit for Temperature") IsNot Nothing, If([Enum].TryParse(ConfigToken("Misc")?("Preferred Unit for Temperature"), PreferredUnit), PreferredUnit, UnitMeasurement.Metric), UnitMeasurement.Metric)
            TextEdit_AutoSaveFlag = If(ConfigToken("Misc")?("Enable text editor autosave"), True)
            TextEdit_AutoSaveInterval = If(Integer.TryParse(ConfigToken("Misc")?("Text editor autosave interval"), 0), ConfigToken("Misc")?("Text editor autosave interval"), 60)
            WrapListOutputs = If(ConfigToken("Misc")?("Wrap list outputs"), False)
            DrawBorderNotification = If(ConfigToken("Misc")?("Draw notification border"), False)
            BlacklistedModsString = If(ConfigToken("Misc")?("Blacklisted mods"), "")
            SolverMinimumNumber = If(Integer.TryParse(ConfigToken("Misc")?("Solver minimum number"), 0), ConfigToken("Misc")?("Solver minimum number"), 1000)
            SolverMaximumNumber = If(Integer.TryParse(ConfigToken("Misc")?("Solver maximum number"), 0), ConfigToken("Misc")?("Solver maximum number"), 1000)
            SolverShowInput = If(ConfigToken("Misc")?("Solver show input"), False)
            NotifyUpperLeftCornerChar = If(ConfigToken("Misc")?("Upper left corner character for notification border"), "╔")
            NotifyUpperRightCornerChar = If(ConfigToken("Misc")?("Upper right corner character for notification border"), "╗")
            NotifyLowerLeftCornerChar = If(ConfigToken("Misc")?("Lower left corner character for notification border"), "╚")
            NotifyLowerRightCornerChar = If(ConfigToken("Misc")?("Lower right corner character for notification border"), "╝")
            NotifyUpperFrameChar = If(ConfigToken("Misc")?("Upper frame character for notification border"), "═")
            NotifyLowerFrameChar = If(ConfigToken("Misc")?("Lower frame character for notification border"), "═")
            NotifyLeftFrameChar = If(ConfigToken("Misc")?("Left frame character for notification border"), "║")
            NotifyRightFrameChar = If(ConfigToken("Misc")?("Right frame character for notification border"), "║")
            ManpageInfoStyle = If(ConfigToken("Misc")?("Manual page information style"), "")
            SpeedPressCurrentDifficulty = If(ConfigToken("Misc")?("Default difficulty for SpeedPress") IsNot Nothing, If([Enum].TryParse(ConfigToken("Misc")?("Default difficulty for SpeedPress"), SpeedPressCurrentDifficulty), SpeedPressCurrentDifficulty, SpeedPressDifficulty.Medium), SpeedPressDifficulty.Medium)
            SpeedPressTimeout = If(Integer.TryParse(ConfigToken("Misc")?("Keypress timeout for SpeedPress"), 0), ConfigToken("Misc")?("Keypress timeout for SpeedPress"), 3000)
            ShowHeadlineOnLogin = If(ConfigToken("Misc")?("Show latest RSS headline on login"), False)
            RssHeadlineUrl = If(ConfigToken("Misc")?("RSS headline URL"), "https://www.techrepublic.com/rssfeeds/articles/")
            SaveEventsRemindersDestructively = If(ConfigToken("Misc")?("Save all events and/or reminders destructively"), False)
            WheelUpperLeftCornerChar = If(ConfigToken("Misc")?("Upper left corner character for RGB color wheel"), "╔")
            WheelUpperRightCornerChar = If(ConfigToken("Misc")?("Upper right corner character for RGB color wheel"), "╗")
            WheelLowerLeftCornerChar = If(ConfigToken("Misc")?("Lower left corner character for RGB color wheel"), "╚")
            WheelLowerRightCornerChar = If(ConfigToken("Misc")?("Lower right corner character for RGB color wheel"), "╝")
            WheelUpperFrameChar = If(ConfigToken("Misc")?("Upper frame character for RGB color wheel"), "═")
            WheelLowerFrameChar = If(ConfigToken("Misc")?("Lower frame character for RGB color wheel"), "═")
            WheelLeftFrameChar = If(ConfigToken("Misc")?("Left frame character for RGB color wheel"), "║")
            WheelRightFrameChar = If(ConfigToken("Misc")?("Right frame character for RGB color wheel"), "║")
            JsonShell_Formatting = If(ConfigToken("Misc")?("Default JSON formatting for JSON shell") IsNot Nothing, If([Enum].TryParse(ConfigToken("Misc")?("Default JSON formatting for JSON shell"), JsonShell_Formatting), JsonShell_Formatting, Formatting.Indented), Formatting.Indented)
            EnableFigletTimer = If(ConfigToken("Misc")?("Enable Figlet for timer"), False)
            TimerFigletFont = If(ConfigToken("Misc")?("Figlet font for timer"), "Small")
            ShowCommandsCount = If(ConfigToken("Misc")?("Show the commands count on help"), False)
            ShowShellCommandsCount = If(ConfigToken("Misc")?("Show the shell commands count on help"), True)
            ShowModCommandsCount = If(ConfigToken("Misc")?("Show the mod commands count on help"), True)
            ShowShellAliasesCount = If(ConfigToken("Misc")?("Show the aliases count on help"), True)
            CurrentMask = If(ConfigToken("Misc")?("Password mask character"), "*"c)
            ProgressUpperLeftCornerChar = If(ConfigToken("Misc")?("Upper left corner character for progress bars"), "╔")
            ProgressUpperRightCornerChar = If(ConfigToken("Misc")?("Upper right corner character for progress bars"), "╗")
            ProgressLowerLeftCornerChar = If(ConfigToken("Misc")?("Lower left corner character for progress bars"), "╚")
            ProgressLowerRightCornerChar = If(ConfigToken("Misc")?("Lower right corner character for progress bars"), "╝")
            ProgressUpperFrameChar = If(ConfigToken("Misc")?("Upper frame character for progress bars"), "═")
            ProgressLowerFrameChar = If(ConfigToken("Misc")?("Lower frame character for progress bars"), "═")
            ProgressLeftFrameChar = If(ConfigToken("Misc")?("Left frame character for progress bars"), "║")
            ProgressRightFrameChar = If(ConfigToken("Misc")?("Right frame character for progress bars"), "║")
            LoveOrHateUsersCount = If(Integer.TryParse(ConfigToken("Misc")?("Users count for love or hate comments"), 0), ConfigToken("Misc")?("Users count for love or hate comments"), 20)
            InputHistoryEnabled = If(ConfigToken("Misc")?("Input history enabled"), True)
            InputClipboardEnabled = If(ConfigToken("Misc")?("Input clipboard enabled"), True)
            InputUndoEnabled = If(ConfigToken("Misc")?("Input undo enabled"), True)

            'Check to see if the config needs fixes
            RepairConfig()

            'Raise event
            KernelEventManager.RaiseConfigRead()
        End Sub

        ''' <summary>
        ''' Configures the kernel according to the custom kernel configuration file
        ''' </summary>
        ''' <returns>True if successful; False if unsuccessful</returns>
        ''' <exception cref="Exceptions.ConfigException"></exception>
        Function TryReadConfig(ConfigPath As String) As Boolean
            Try
                ReadConfig(ConfigPath)
                Return True
            Catch nre As NullReferenceException
                'Rare, but repair config if an NRE is caught.
                Wdbg(DebugLevel.E, "Error trying to read config: {0}", nre.Message)
                RepairConfig()
            Catch ex As Exception
                KernelEventManager.RaiseConfigReadError(ex)
                WStkTrc(ex)
                If Not KernelBooted Then
                    NotifySend(New Notification(DoTranslation("Error loading settings"),
                                                DoTranslation("There is an error while loading settings. You may need to check the settings file."),
                                                NotifPriority.Medium, NotifType.Normal))
                End If
                Wdbg(DebugLevel.E, "Error trying to read config: {0}", ex.Message)
                Throw New Exceptions.ConfigException(DoTranslation("There is an error trying to read configuration: {0}."), ex, ex.Message)
            End Try
            Return False
        End Function

        ''' <summary>
        ''' Main loader for configuration file
        ''' </summary>
        Sub InitializeConfig()
            'Make a config file if not found
            If Not FileExists(GetKernelPath(KernelPathType.Configuration)) Then
                Wdbg(DebugLevel.E, "No config file found. Creating...")
                CreateConfig()
            End If

            'Load and read config
            Try
                TryReadConfig()
            Catch cex As Exceptions.ConfigException
                Write(cex.Message, True, ColTypes.Error)
                WStkTrc(cex)
            End Try
        End Sub

        ''' <summary>
        ''' Initializes the config token
        ''' </summary>
        Sub InitializeConfigToken()
            InitializeConfigToken(GetKernelPath(KernelPathType.Configuration))
        End Sub

        ''' <summary>
        ''' Initializes the config token
        ''' </summary>
        Sub InitializeConfigToken(ConfigPath As String)
            ThrowOnInvalidPath(ConfigPath)
            ConfigToken = JObject.Parse(File.ReadAllText(ConfigPath))
        End Sub

    End Module
End Namespace
