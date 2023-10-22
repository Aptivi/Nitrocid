
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

using FluentFTP;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Files;
using KS.Files.Folders;
using KS.Kernel.Debugging.RemoteDebug;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Notifications;
using KS.Misc.Screensaver;
using KS.Network.Base;
using KS.Network.RPC;
using KS.Shell.Prompts;
using KS.Shell.Shells.FTP;
using KS.Shell.Shells.Hex;
using KS.Shell.Shells.Mail;
using KS.Shell.Shells.RSS;
using KS.Shell.Shells.Text;
using KS.Shell.ShellBase.Shells;
using MimeKit.Text;
using Newtonsoft.Json;
using System;
using KS.Drivers;
using KS.Drivers.Console;
using KS.Drivers.RNG;
using KS.Drivers.Network;
using KS.Drivers.Filesystem;
using KS.Drivers.Encryption;
using KS.Drivers.Regexp;
using KS.ConsoleBase.Inputs.Styles;
using KS.ConsoleBase.Writers.MiscWriters;
using KS.ConsoleBase.Writers.FancyWriters.Tools;
using Terminaux.Colors;
using Terminaux.Colors.Accessibility;
using Figletize;
using KS.Users.Login.Handlers;
using KS.Kernel.Configuration.Settings;
using KS.Drivers.Encoding;
using KS.Drivers.DebugLogger;
using KS.Files.Operations.Querying;
using KS.Drivers.HardwareProber;
using KS.Misc.Text;
using KS.Shell.Shells.Json;

namespace KS.Kernel.Configuration.Instances
{
    /// <summary>
    /// Main kernel configuration instance
    /// </summary>
    public class KernelMainConfig : BaseKernelConfig, IKernelConfig
    {
        /// <inheritdoc/>
        [JsonIgnore]
        public override SettingsEntry[] SettingsEntries =>
            ConfigTools.GetSettingsEntries(Resources.SettingsResources.SettingsEntries);

        /// <summary>
        /// Each startup, it will check for updates.
        /// </summary>
        public bool CheckUpdateStart { get; set; } = true;
        /// <summary>
        /// If specified, it will display customized startup banner with placeholder support. You can use {0} for kernel version.
        /// </summary>
        public string CustomBanner
        {
            get => WelcomeMessage.GetCustomBanner();
            set => WelcomeMessage.customBanner = value;
        }
        /// <summary>
        /// When switching languages, change the month names, calendar, etc.
        /// </summary>
        public bool LangChangeCulture { get; set; }
        /// <summary>
        /// Specifies the kernel language.
        /// </summary>
        public string CurrentLanguage
        {
            get => LanguageManager.currentLanguage is not null ? LanguageManager.currentLanguage.ThreeLetterLanguageName : "eng";
            set => LanguageManager.SetLangDry(value);
        }
        /// <summary>
        /// Which variant of the current language is being used to change the month names, calendar, etc.?
        /// </summary>
        public string CurrentCultStr { get; set; } = "en-US";
        /// <summary>
        /// Shows brief information about the application on boot.
        /// </summary>
        public bool ShowAppInfoOnBoot { get; set; } = true;
        /// <summary>
        /// Shows how much time did the kernel take to finish a stage.
        /// </summary>
        public bool ShowStageFinishTimes { get; set; }
        /// <summary>
        /// Automatically start the kernel modifications on boot.
        /// </summary>
        public bool StartKernelMods { get; set; }
        /// <summary>
        /// Shows the current time, time zone, and date before logging in.
        /// </summary>
        public bool ShowCurrentTimeBeforeLogin { get; set; } = true;
        /// <summary>
        /// If there is a minor fault during kernel boot, notifies the user about it.
        /// </summary>
        public bool NotifyFaultsBoot { get; set; } = true;
        /// <summary>
        /// If there is any kernel error, choose whether or not to print the stack trace to the console.
        /// </summary>
        public bool ShowStackTraceOnKernelError { get; set; }
        /// <summary>
        /// If there is any update, the kernel will automatically download it.
        /// </summary>
        public bool AutoDownloadUpdate { get; set; } = true;
        /// <summary>
        /// Enables debugging for the kernel event system
        /// </summary>
        public bool EventDebug { get; set; }
        /// <summary>
        /// Enables the stylish splash screen on startup. Please note that it will disable argument prompt and test shell pre-boot.
        /// </summary>
        public bool EnableSplash { get; set; } = true;
        /// <summary>
        /// Splash name from the available splashes implemented in the kernel.
        /// </summary>
        public string SplashName { get; set; } = "Welcome";
        /// <summary>
        /// Whether to simulate a situation where there is no APM available. If enabled, it informs the user that it's now safe to turn off the computer upon shutdown.
        /// </summary>
        public bool SimulateNoAPM { get; set; }
        /// <summary>
        /// If you are color blind or if you want to simulate color blindness, then you can enable it.
        /// </summary>
        public bool ColorBlind
        {
            get => ColorTools.EnableColorTransformation;
            set => ColorTools.EnableColorTransformation = value;
        }
        /// <summary>
        /// The type of color blindness, whether it's protan, deuter, or tritan.
        /// </summary>
        public int BlindnessDeficiency
        {
            get => (int)ColorTools.ColorDeficiency;
            set => ColorTools.ColorDeficiency = (Deficiency)value;
        }
        /// <summary>
        /// How severe is the color blindness?
        /// </summary>
        public double BlindnessSeverity
        {
            get => ColorTools.ColorDeficiencySeverity;
            set => ColorTools.ColorDeficiencySeverity = value;
        }
        /// <summary>
        /// Enables beeping upon shutting down the kernel.
        /// </summary>
        public bool BeepOnShutdown { get; set; }
        /// <summary>
        /// Enables delaying upon shutting down the kernel.
        /// </summary>
        public bool DelayOnShutdown { get; set; }
        /// <summary>
        /// If you want the simulation of color blindness to be simple, enable this. Please note that the formula used in this simulation method may not be accurate.
        /// </summary>
        public bool ColorBlindSimple
        {
            get => ColorTools.EnableSimpleColorTransformation;
            set => ColorTools.EnableSimpleColorTransformation = value;
        }
        /// <summary>
        /// If you are sure that the console supports true color, or if you want to change your terminal to a terminal that supports true color, change this value.
        /// </summary>
        public bool ConsoleSupportsTrueColor { get; set; } = true;
        /// <summary>
        /// Set the language codepage upon switching languages (Windows only)
        /// </summary>
        public bool SetCodepage { get; set; } = true;
        /// <summary>
        /// Development notice acknowledged
        /// </summary>
        public bool DevNoticeConsented { get; set; }
        /// <summary>
        /// Allow untrusted mods
        /// </summary>
        public bool AllowUntrustedMods { get; set; }
        /// <summary>
        /// User Name Shell Color
        /// </summary>
        public string UserNameShellColor
        {
            get => KernelColorTools.GetColor(KernelColorType.UserNameShell).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.UserNameShell, new Color(value));
        }
        /// <summary>
        /// Host Name Shell Color
        /// </summary>
        public string HostNameShellColor
        {
            get => KernelColorTools.GetColor(KernelColorType.HostNameShell).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.HostNameShell, new Color(value));
        }
        /// <summary>
        /// Continuable Kernel Error Color
        /// </summary>
        public string ContinuableKernelErrorColor
        {
            get => KernelColorTools.GetColor(KernelColorType.ContKernelError).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.ContKernelError, new Color(value));
        }
        /// <summary>
        /// Uncontinuable Kernel Error Color
        /// </summary>
        public string UncontinuableKernelErrorColor
        {
            get => KernelColorTools.GetColor(KernelColorType.UncontKernelError).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.UncontKernelError, new Color(value));
        }
        /// <summary>
        /// Text Color
        /// </summary>
        public string TextColor
        {
            get => KernelColorTools.GetColor(KernelColorType.NeutralText).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.NeutralText, new Color(value));
        }
        /// <summary>
        /// License Color
        /// </summary>
        public string LicenseColor
        {
            get => KernelColorTools.GetColor(KernelColorType.License).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.License, new Color(value));
        }
        /// <summary>
        /// Background Color
        /// </summary>
        public string BackgroundColor
        {
            get => KernelColorTools.GetColor(KernelColorType.Background).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.Background, new Color(value));
        }
        /// <summary>
        /// Input Color
        /// </summary>
        public string InputColor
        {
            get => KernelColorTools.GetColor(KernelColorType.Input).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.Input, new Color(value));
        }
        /// <summary>
        /// List Entry Color
        /// </summary>
        public string ListEntryColor
        {
            get => KernelColorTools.GetColor(KernelColorType.ListEntry).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.ListEntry, new Color(value));
        }
        /// <summary>
        /// List Value Color
        /// </summary>
        public string ListValueColor
        {
            get => KernelColorTools.GetColor(KernelColorType.ListValue).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.ListValue, new Color(value));
        }
        /// <summary>
        /// Kernel Stage Color
        /// </summary>
        public string KernelStageColor
        {
            get => KernelColorTools.GetColor(KernelColorType.Stage).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.Stage, new Color(value));
        }
        /// <summary>
        /// Error Text Color
        /// </summary>
        public string ErrorTextColor
        {
            get => KernelColorTools.GetColor(KernelColorType.Error).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.Error, new Color(value));
        }
        /// <summary>
        /// Warning Text Color
        /// </summary>
        public string WarningTextColor
        {
            get => KernelColorTools.GetColor(KernelColorType.Warning).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.Warning, new Color(value));
        }
        /// <summary>
        /// Option Color
        /// </summary>
        public string OptionColor
        {
            get => KernelColorTools.GetColor(KernelColorType.Option).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.Option, new Color(value));
        }
        /// <summary>
        /// Banner Color
        /// </summary>
        public string BannerColor
        {
            get => KernelColorTools.GetColor(KernelColorType.Banner).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.Banner, new Color(value));
        }
        /// <summary>
        /// Notification Title Color
        /// </summary>
        public string NotificationTitleColor
        {
            get => KernelColorTools.GetColor(KernelColorType.NotificationTitle).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.NotificationTitle, new Color(value));
        }
        /// <summary>
        /// Notification Description Color
        /// </summary>
        public string NotificationDescriptionColor
        {
            get => KernelColorTools.GetColor(KernelColorType.NotificationDescription).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.NotificationDescription, new Color(value));
        }
        /// <summary>
        /// Notification Progress Color
        /// </summary>
        public string NotificationProgressColor
        {
            get => KernelColorTools.GetColor(KernelColorType.NotificationProgress).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.NotificationProgress, new Color(value));
        }
        /// <summary>
        /// Notification Failure Color
        /// </summary>
        public string NotificationFailureColor
        {
            get => KernelColorTools.GetColor(KernelColorType.NotificationFailure).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.NotificationFailure, new Color(value));
        }
        /// <summary>
        /// Question Color
        /// </summary>
        public string QuestionColor
        {
            get => KernelColorTools.GetColor(KernelColorType.Question).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.Question, new Color(value));
        }
        /// <summary>
        /// Success Color
        /// </summary>
        public string SuccessColor
        {
            get => KernelColorTools.GetColor(KernelColorType.Success).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.Success, new Color(value));
        }
        /// <summary>
        /// User Dollar Color
        /// </summary>
        public string UserDollarColor
        {
            get => KernelColorTools.GetColor(KernelColorType.UserDollar).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.UserDollar, new Color(value));
        }
        /// <summary>
        /// Tip Color
        /// </summary>
        public string TipColor
        {
            get => KernelColorTools.GetColor(KernelColorType.Tip).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.Tip, new Color(value));
        }
        /// <summary>
        /// Separator Text Color
        /// </summary>
        public string SeparatorTextColor
        {
            get => KernelColorTools.GetColor(KernelColorType.SeparatorText).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.SeparatorText, new Color(value));
        }
        /// <summary>
        /// Separator Color
        /// </summary>
        public string SeparatorColor
        {
            get => KernelColorTools.GetColor(KernelColorType.Separator).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.Separator, new Color(value));
        }
        /// <summary>
        /// List Title Color
        /// </summary>
        public string ListTitleColor
        {
            get => KernelColorTools.GetColor(KernelColorType.ListTitle).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.ListTitle, new Color(value));
        }
        /// <summary>
        /// Development Warning Color
        /// </summary>
        public string DevelopmentWarningColor
        {
            get => KernelColorTools.GetColor(KernelColorType.DevelopmentWarning).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.DevelopmentWarning, new Color(value));
        }
        /// <summary>
        /// Stage Time Color
        /// </summary>
        public string StageTimeColor
        {
            get => KernelColorTools.GetColor(KernelColorType.StageTime).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.StageTime, new Color(value));
        }
        /// <summary>
        /// Progress Color
        /// </summary>
        public string ProgressColor
        {
            get => KernelColorTools.GetColor(KernelColorType.Progress).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.Progress, new Color(value));
        }
        /// <summary>
        /// Back Option Color
        /// </summary>
        public string BackOptionColor
        {
            get => KernelColorTools.GetColor(KernelColorType.BackOption).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.BackOption, new Color(value));
        }
        /// <summary>
        /// Low Priority Border Color
        /// </summary>
        public string LowPriorityBorderColor
        {
            get => KernelColorTools.GetColor(KernelColorType.LowPriorityBorder).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.LowPriorityBorder, new Color(value));
        }
        /// <summary>
        /// Medium Priority Border Color
        /// </summary>
        public string MediumPriorityBorderColor
        {
            get => KernelColorTools.GetColor(KernelColorType.MediumPriorityBorder).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.MediumPriorityBorder, new Color(value));
        }
        /// <summary>
        /// High Priority Border Color
        /// </summary>
        public string HighPriorityBorderColor
        {
            get => KernelColorTools.GetColor(KernelColorType.HighPriorityBorder).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.HighPriorityBorder, new Color(value));
        }
        /// <summary>
        /// Table Separator Color
        /// </summary>
        public string TableSeparatorColor
        {
            get => KernelColorTools.GetColor(KernelColorType.TableSeparator).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.TableSeparator, new Color(value));
        }
        /// <summary>
        /// Table Header Color
        /// </summary>
        public string TableHeaderColor
        {
            get => KernelColorTools.GetColor(KernelColorType.TableHeader).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.TableHeader, new Color(value));
        }
        /// <summary>
        /// Table Value Color
        /// </summary>
        public string TableValueColor
        {
            get => KernelColorTools.GetColor(KernelColorType.TableValue).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.TableValue, new Color(value));
        }
        /// <summary>
        /// Selected Option Color
        /// </summary>
        public string SelectedOptionColor
        {
            get => KernelColorTools.GetColor(KernelColorType.SelectedOption).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.SelectedOption, new Color(value));
        }
        /// <summary>
        /// Alternative Option Color
        /// </summary>
        public string AlternativeOptionColor
        {
            get => KernelColorTools.GetColor(KernelColorType.AlternativeOption).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.AlternativeOption, new Color(value));
        }
        /// <summary>
        /// Weekend Day Color
        /// </summary>
        public string WeekendDayColor
        {
            get => KernelColorTools.GetColor(KernelColorType.WeekendDay).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.WeekendDay, new Color(value));
        }
        /// <summary>
        /// Event Day Color
        /// </summary>
        public string EventDayColor
        {
            get => KernelColorTools.GetColor(KernelColorType.EventDay).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.EventDay, new Color(value));
        }
        /// <summary>
        /// Table Title Color
        /// </summary>
        public string TableTitleColor
        {
            get => KernelColorTools.GetColor(KernelColorType.TableTitle).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.TableTitle, new Color(value));
        }
        /// <summary>
        /// Today Day Color
        /// </summary>
        public string TodayDayColor
        {
            get => KernelColorTools.GetColor(KernelColorType.TodayDay).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.TodayDay, new Color(value));
        }
        /// <summary>
        /// Interactive TUI background color
        /// </summary>
        public string TuiBackgroundColor
        {
            get => KernelColorTools.GetColor(KernelColorType.TuiBackground).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.TuiBackground, new Color(value));
        }
        /// <summary>
        /// Interactive TUI foreground color
        /// </summary>
        public string TuiForegroundColor
        {
            get => KernelColorTools.GetColor(KernelColorType.TuiForeground).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.TuiForeground, new Color(value));
        }
        /// <summary>
        /// Interactive TUI pane background color
        /// </summary>
        public string TuiPaneBackgroundColor
        {
            get => KernelColorTools.GetColor(KernelColorType.TuiPaneBackground).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.TuiPaneBackground, new Color(value));
        }
        /// <summary>
        /// Interactive TUI pane separator color
        /// </summary>
        public string TuiPaneSeparatorColor
        {
            get => KernelColorTools.GetColor(KernelColorType.TuiPaneSeparator).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.TuiPaneSeparator, new Color(value));
        }
        /// <summary>
        /// Interactive TUI selected pane separator color
        /// </summary>
        public string TuiPaneSelectedSeparatorColor
        {
            get => KernelColorTools.GetColor(KernelColorType.TuiPaneSelectedSeparator).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.TuiPaneSelectedSeparator, new Color(value));
        }
        /// <summary>
        /// Interactive TUI selected pane item foreground color
        /// </summary>
        public string TuiPaneSelectedItemForeColor
        {
            get => KernelColorTools.GetColor(KernelColorType.TuiPaneSelectedItemFore).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.TuiPaneSelectedItemFore, new Color(value));
        }
        /// <summary>
        /// Interactive TUI selected pane item background color
        /// </summary>
        public string TuiPaneSelectedItemBackColor
        {
            get => KernelColorTools.GetColor(KernelColorType.TuiPaneSelectedItemBack).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.TuiPaneSelectedItemBack, new Color(value));
        }
        /// <summary>
        /// Interactive TUI pane item foreground color
        /// </summary>
        public string TuiPaneItemForeColor
        {
            get => KernelColorTools.GetColor(KernelColorType.TuiPaneItemFore).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.TuiPaneItemFore, new Color(value));
        }
        /// <summary>
        /// Interactive TUI pane item background color
        /// </summary>
        public string TuiPaneItemBackColor
        {
            get => KernelColorTools.GetColor(KernelColorType.TuiPaneItemBack).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.TuiPaneItemBack, new Color(value));
        }
        /// <summary>
        /// Interactive TUI option background color
        /// </summary>
        public string TuiOptionBackgroundColor
        {
            get => KernelColorTools.GetColor(KernelColorType.TuiOptionBackground).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.TuiOptionBackground, new Color(value));
        }
        /// <summary>
        /// Interactive TUI option foreground color
        /// </summary>
        public string TuiOptionForegroundColor
        {
            get => KernelColorTools.GetColor(KernelColorType.TuiOptionForeground).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.TuiOptionForeground, new Color(value));
        }
        /// <summary>
        /// Interactive TUI option binding name color
        /// </summary>
        public string TuiKeyBindingOptionColor
        {
            get => KernelColorTools.GetColor(KernelColorType.TuiKeyBindingOption).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.TuiKeyBindingOption, new Color(value));
        }
        /// <summary>
        /// Interactive TUI box background color
        /// </summary>
        public string TuiBoxBackgroundColor
        {
            get => KernelColorTools.GetColor(KernelColorType.TuiBoxBackground).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.TuiBoxBackground, new Color(value));
        }
        /// <summary>
        /// Interactive TUI box foreground color
        /// </summary>
        public string TuiBoxForegroundColor
        {
            get => KernelColorTools.GetColor(KernelColorType.TuiBoxForeground).PlainSequence;
            set => KernelColorTools.SetColor(KernelColorType.TuiBoxForeground, new Color(value));
        }
        /// <summary>
        /// Keep hardware probing messages silent.
        /// </summary>
        public bool QuietHardwareProbe { get; set; }
        /// <summary>
        /// Make hardware probing messages a bit talkative.
        /// </summary>
        public bool VerboseHardwareProbe { get; set; }
        /// <summary>
        /// Show Message of the Day before displaying login screen.
        /// </summary>
        public bool ShowMOTD { get; set; } = true;
        /// <summary>
        /// Clear screen before displaying login screen.
        /// </summary>
        public bool ClearOnLogin { get; set; }
        /// <summary>
        /// The kernel host name to communicate with the rest of the computers
        /// </summary>
        public string HostName { get; set; } = "kernel";
        /// <summary>
        /// Shows available users if enabled
        /// </summary>
        public bool ShowAvailableUsers { get; set; } = true;
        /// <summary>
        /// Which file is the MOTD text file? Write an absolute path to the text file
        /// </summary>
        public string MotdFilePath { get; set; } = Paths.GetKernelPath(KernelPathType.MOTD);
        /// <summary>
        /// Which file is the MAL text file? Write an absolute path to the text file
        /// </summary>
        public string MalFilePath { get; set; } = Paths.GetKernelPath(KernelPathType.MAL);
        /// <summary>
        /// Write how you want your login prompt to be. Leave blank to use default style. Placeholders are parsed
        /// </summary>
        public string UsernamePrompt { get; set; } = "";
        /// <summary>
        /// Write how you want your password prompt to be. Leave blank to use default style. Placeholders are parsed
        /// </summary>
        public string PasswordPrompt { get; set; } = "";
        /// <summary>
        /// Shows Message of the Day after displaying login screen
        /// </summary>
        public bool ShowMAL { get; set; } = true;
        /// <summary>
        /// Includes the anonymous users in the list
        /// </summary>
        public bool IncludeAnonymous { get; set; }
        /// <summary>
        /// Includes the disabled users in the list
        /// </summary>
        public bool IncludeDisabled { get; set; }
        /// <summary>
        /// Whether to show the MOTD and the headline at the bottom or at the top of the clock
        /// </summary>
        public bool MotdHeadlineBottom { get; set; }
        /// <summary>
        /// Current login handler.
        /// </summary>
        public string CurrentLoginHandler
        {
            get => LoginHandlerTools.CurrentHandlerName;
            set => LoginHandlerTools.CurrentHandlerName = value;
        }
        /// <summary>
        /// Simplified help command for all the shells
        /// </summary>
        public bool SimHelp { get; set; }
        /// <summary>
        /// Sets the shell's current directory. Write an absolute path to any existing directory
        /// </summary>
        public string CurrentDir
        {
            get
            {
                return CurrentDirectory._CurrentDirectory;
            }
            set
            {
                FilesystemTools.ThrowOnInvalidPath(value);
                value = FilesystemTools.NeutralizePath(value);
                if (Checking.FolderExists(value))
                {
                    CurrentDirectory._CurrentDirectory = value;
                }
                else
                {
                    throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("Directory {0} not found"), value);
                }
            }
        }
        /// <summary>
        /// Group of paths separated by the colon. It works the same as PATH. Write a full path to a folder or a folder name. When you're finished, write \"q\". Write a minus sign next to the path to remove an existing directory.
        /// </summary>
        public string PathsToLookup { get; set; } = Environment.GetEnvironmentVariable("PATH");
        /// <summary>
        /// Prompt Preset
        /// </summary>
        public string PromptPreset
        {
            get => PromptPresetManager.GetCurrentPresetBaseFromShell(ShellType.Shell).PresetName;
            set => PromptPresetManager.SetPreset(value, ShellType.Shell, false);
        }
        /// <summary>
        /// FTP Prompt Preset
        /// </summary>
        public string FTPPromptPreset
        {
            get => PromptPresetManager.GetCurrentPresetBaseFromShell(ShellType.FTPShell).PresetName;
            set => PromptPresetManager.SetPreset(value, ShellType.FTPShell, false);
        }
        /// <summary>
        /// Mail Prompt Preset
        /// </summary>
        public string MailPromptPreset
        {
            get => PromptPresetManager.GetCurrentPresetBaseFromShell(ShellType.MailShell).PresetName;
            set => PromptPresetManager.SetPreset(value, ShellType.MailShell, false);
        }
        /// <summary>
        /// SFTP Prompt Preset
        /// </summary>
        public string SFTPPromptPreset
        {
            get => PromptPresetManager.GetCurrentPresetBaseFromShell(ShellType.SFTPShell).PresetName;
            set => PromptPresetManager.SetPreset(value, ShellType.SFTPShell, false);
        }
        /// <summary>
        /// RSS Prompt Preset
        /// </summary>
        public string RSSPromptPreset
        {
            get => PromptPresetManager.GetCurrentPresetBaseFromShell(ShellType.RSSShell).PresetName;
            set => PromptPresetManager.SetPreset(value, ShellType.RSSShell, false);
        }
        /// <summary>
        /// Text Edit Prompt Preset
        /// </summary>
        public string TextEditPromptPreset
        {
            get => PromptPresetManager.GetCurrentPresetBaseFromShell(ShellType.TextShell).PresetName;
            set => PromptPresetManager.SetPreset(value, ShellType.TextShell, false);
        }
        /// <summary>
        /// JSON Shell Prompt Preset
        /// </summary>
        public string JSONShellPromptPreset
        {
            get => PromptPresetManager.GetCurrentPresetBaseFromShell(ShellType.JsonShell).PresetName;
            set => PromptPresetManager.SetPreset(value, ShellType.JsonShell, false);
        }
        /// <summary>
        /// Hex Edit Prompt Preset
        /// </summary>
        public string HexEditPromptPreset
        {
            get => PromptPresetManager.GetCurrentPresetBaseFromShell(ShellType.HexShell).PresetName;
            set => PromptPresetManager.SetPreset(value, ShellType.HexShell, false);
        }
        /// <summary>
        /// HTTP Shell Prompt Preset
        /// </summary>
        public string HTTPShellPromptPreset
        {
            get => PromptPresetManager.GetCurrentPresetBaseFromShell(ShellType.HTTPShell).PresetName;
            set => PromptPresetManager.SetPreset(value, ShellType.HTTPShell, false);
        }
        /// <summary>
        /// Admin Shell Prompt Preset
        /// </summary>
        public string AdminShellPromptPreset
        {
            get => PromptPresetManager.GetCurrentPresetBaseFromShell(ShellType.AdminShell).PresetName;
            set => PromptPresetManager.SetPreset(value, ShellType.AdminShell, false);
        }
        /// <summary>
        /// SQL Shell Prompt Preset
        /// </summary>
        public string SqlShellPromptPreset
        {
            get => PromptPresetManager.GetCurrentPresetBaseFromShell(ShellType.SqlShell).PresetName;
            set => PromptPresetManager.SetPreset(value, ShellType.SqlShell, false);
        }
        /// <summary>
        /// Debug Shell Prompt Preset
        /// </summary>
        public string DebugShellPromptPreset
        {
            get => PromptPresetManager.GetCurrentPresetBaseFromShell(ShellType.DebugShell).PresetName;
            set => PromptPresetManager.SetPreset(value, ShellType.DebugShell, false);
        }
        /// <summary>
        /// Default choice output type
        /// </summary>
        public int DefaultChoiceOutputType { get; set; } = (int)ChoiceOutputType.Modern;
        /// <summary>
        /// Sets console title on command execution
        /// </summary>
        public bool SetTitleOnCommandExecution { get; set; } = true;
        /// <summary>
        /// Controls how the files will be sorted
        /// </summary>
        public int SortMode { get; set; } = (int)FilesystemSortOptions.FullName;
        /// <summary>
        /// Controls the direction of filesystem sorting whether it's ascending or descending
        /// </summary>
        public int SortDirection { get; set; } = (int)FilesystemSortDirection.Ascending;
        /// <summary>
        /// Shows hidden files.
        /// </summary>
        public bool HiddenFiles { get; set; }
        /// <summary>
        /// If enabled, the kernel will parse the whole folder for its total size. Else, will only parse the surface.
        /// </summary>
        public bool FullParseMode { get; set; }
        /// <summary>
        /// Shows what file is being processed during the filesystem operations
        /// </summary>
        public bool ShowFilesystemProgress { get; set; } = true;
        /// <summary>
        /// Shows the brief file details while listing files
        /// </summary>
        public bool ShowFileDetailsList { get; set; } = true;
        /// <summary>
        /// Hides the annoying message if the listing function tries to open an unauthorized folder
        /// </summary>
        public bool SuppressUnauthorizedMessages { get; set; } = true;
        /// <summary>
        /// Makes the "cat" command print the file's line numbers
        /// </summary>
        public bool PrintLineNumbers { get; set; }
        /// <summary>
        /// Sorts the filesystem list professionally
        /// </summary>
        public bool SortList { get; set; } = true;
        /// <summary>
        /// If enabled, shows the total folder size in list, depending on how to calculate the folder sizes according to the configuration.
        /// </summary>
        public bool ShowTotalSizeInList { get; set; }
        /// <summary>
        /// Write a remote debugger port. It must be numeric, and must not be already used. Otherwise, remote debugger will fail to open the port
        /// </summary>
        public int DebugPort
        {
            get => RemoteDebugger.debugPort;
            set => RemoteDebugger.debugPort = value < 0 ? 3014 : value;
        }
        /// <summary>
        /// Write how many times the "get" command should retry failed downloads. It must be numeric.
        /// </summary>
        public int DownloadRetries
        {
            get => NetworkTools.downloadRetries;
            set => NetworkTools.downloadRetries = value < 0 ? 3 : value;
        }
        /// <summary>
        /// Write how many times the "put" command should retry failed uploads. It must be numeric.
        /// </summary>
        public int UploadRetries
        {
            get => NetworkTools.uploadRetries;
            set => NetworkTools.uploadRetries = value < 0 ? 3 : value;
        }
        /// <summary>
        /// If true, it makes "get" or "put" show the progress bar while downloading or uploading.
        /// </summary>
        public bool ShowProgress { get; set; } = true;
        /// <summary>
        /// Whether or not to log FTP username
        /// </summary>
        public bool FTPLoggerUsername { get; set; }
        /// <summary>
        /// Whether or not to log FTP IP address
        /// </summary>
        public bool FTPLoggerIP { get; set; }
        /// <summary>
        /// Pick the first profile only when connecting
        /// </summary>
        public bool FTPFirstProfileOnly { get; set; }
        /// <summary>
        /// When listing mail messages, show body preview
        /// </summary>
        public bool ShowPreview { get; set; }
        /// <summary>
        /// Records remote debug chat to debug log
        /// </summary>
        public bool RecordChatToDebugLog { get; set; } = true;
        /// <summary>
        /// Shows the SSH server banner on connection
        /// </summary>
        public bool SSHBanner { get; set; }
        /// <summary>
        /// Whether or not to enable RPC
        /// </summary>
        public bool RPCEnabled { get; set; }
        /// <summary>
        /// Write an RPC port. It must be numeric, and must not be already used. Otherwise, RPC will fail to open the port.
        /// </summary>
        public int RPCPort
        {
            get => RemoteProcedure.rpcPort;
            set => RemoteProcedure.rpcPort = value < 0 ? 12345 : value;
        }
        /// <summary>
        /// Shows the FTP file details while listing remote directories
        /// </summary>
        public bool FtpShowDetailsInList { get; set; } = true;
        /// <summary>
        /// Write how you want your login prompt to be. Leave blank to use default style. Placeholders are parsed
        /// </summary>
        public string FtpUserPromptStyle { get; set; } = "";
        /// <summary>
        /// Write how you want your password prompt to be. Leave blank to use default style. Placeholders are parsed
        /// </summary>
        public string FtpPassPromptStyle { get; set; } = "";
        /// <summary>
        /// Uses the first FTP profile to connect to FTP
        /// </summary>
        public bool FtpUseFirstProfile { get; set; }
        /// <summary>
        /// If enabled, adds a new connection to the FTP speed dial
        /// </summary>
        public bool FtpNewConnectionsToSpeedDial { get; set; } = true;
        /// <summary>
        /// Tries to validate the FTP certificates. Turning it off is not recommended
        /// </summary>
        public bool FtpTryToValidateCertificate { get; set; } = true;
        /// <summary>
        /// Shows the FTP message of the day on login
        /// </summary>
        public bool FtpShowMotd { get; set; } = true;
        /// <summary>
        /// Always accept invalid FTP certificates. Turning it on is not recommended as it may pose security risks
        /// </summary>
        public bool FtpAlwaysAcceptInvalidCerts { get; set; }
        /// <summary>
        /// Write how you want your login prompt to be. Leave blank to use default style. Placeholders are parsed
        /// </summary>
        public string MailUserPromptStyle { get; set; } = "";
        /// <summary>
        /// Write how you want your password prompt to be. Leave blank to use default style. Placeholders are parsed
        /// </summary>
        public string MailPassPromptStyle { get; set; } = "";
        /// <summary>
        /// Write how you want your IMAP server prompt to be. Leave blank to use default style. Placeholders are parsed
        /// </summary>
        public string MailIMAPPromptStyle { get; set; } = "";
        /// <summary>
        /// Write how you want your SMTP server prompt to be. Leave blank to use default style. Placeholders are parsed
        /// </summary>
        public string MailSMTPPromptStyle { get; set; } = "";
        /// <summary>
        /// Automatically detect the mail server based on the given address
        /// </summary>
        public bool MailAutoDetectServer { get; set; } = true;
        /// <summary>
        /// Enables mail server debug
        /// </summary>
        public bool MailDebug { get; set; }
        /// <summary>
        /// Notifies you for any new mail messages
        /// </summary>
        public bool MailNotifyNewMail { get; set; } = true;
        /// <summary>
        /// Write how you want your GPG password prompt to be. Leave blank to use default style. Placeholders are parsed
        /// </summary>
        public string MailGPGPromptStyle { get; set; } = "";
        /// <summary>
        /// How many milliseconds to send the IMAP ping?
        /// </summary>
        public int MailImapPingInterval
        {
            get => MailShellCommon.imapPingInterval;
            set => MailShellCommon.imapPingInterval = value < 0 ? 30000 : value;
        }
        /// <summary>
        /// How many milliseconds to send the SMTP ping?
        /// </summary>
        public int MailSmtpPingInterval
        {
            get => MailShellCommon.smtpPingInterval;
            set => MailShellCommon.smtpPingInterval = value < 0 ? 30000 : value;
        }
        /// <summary>
        /// Controls how the mail text will be shown
        /// </summary>
        public int MailTextFormat { get; set; } = (int)TextFormat.Plain;
        /// <summary>
        /// If you want remote debug to start on boot, enable this
        /// </summary>
        public bool RDebugAutoStart { get; set; } = true;
        /// <summary>
        /// Specifies the remote debug message format. {0} for name, {1} for message
        /// </summary>
        public string RDebugMessageFormat { get; set; } = "";
        /// <summary>
        /// Write how you want your RSS feed server prompt to be. Leave blank to use default style. Placeholders are parsed.
        /// </summary>
        public string RSSFeedUrlPromptStyle { get; set; } = "";
        /// <summary>
        /// Auto refresh RSS feed
        /// </summary>
        public bool RSSRefreshFeeds { get; set; } = true;
        /// <summary>
        /// How many milliseconds to refresh the RSS feed?
        /// </summary>
        public int RSSRefreshInterval
        {
            get => RSSShellCommon.refreshInterval;
            set => RSSShellCommon.refreshInterval = value < 0 ? 60000 : value;
        }
        /// <summary>
        /// Shows the SFTP file details while listing remote directories
        /// </summary>
        public bool SFTPShowDetailsInList { get; set; } = true;
        /// <summary>
        /// Write how you want your login prompt to be. Leave blank to use default style. Placeholders are parsed
        /// </summary>
        public string SFTPUserPromptStyle { get; set; } = "";
        /// <summary>
        /// If enabled, adds a new connection to the SFTP speed dial
        /// </summary>
        public bool SFTPNewConnectionsToSpeedDial { get; set; } = true;
        /// <summary>
        /// How many milliseconds to wait before declaring timeout?
        /// </summary>
        public int PingTimeout
        {
            get => NetworkTools.pingTimeout;
            set => NetworkTools.pingTimeout = value < 0 ? 60000 : value;
        }
        /// <summary>
        /// Write how you want your download percentage text to be. Leave blank to use default style. Placeholders are parsed. {0} for downloaded size, {1} for target size, {2} for percentage.
        /// </summary>
        public string DownloadPercentagePrint { get; set; } = "";
        /// <summary>
        /// Write how you want your upload percentage text to be. Leave blank to use default style. Placeholders are parsed. {0} for uploaded size, {1} for target size, {2} for percentage.
        /// </summary>
        public string UploadPercentagePrint { get; set; } = "";
        /// <summary>
        /// Whether to recursively hash a directory. Please note that not all the FTP servers support that
        /// </summary>
        public bool FtpRecursiveHashing { get; set; }
        /// <summary>
        /// How many e-mail messages to display in one page?
        /// </summary>
        public int MailMaxMessagesInPage
        {
            get => MailShellCommon.maxMessagesInPage;
            set => MailShellCommon.maxMessagesInPage = value < 0 ? 10 : value;
        }
        /// <summary>
        /// If enabled, the mail shell will show how many bytes transmitted when downloading mail.
        /// </summary>
        public bool MailShowProgress { get; set; } = true;
        /// <summary>
        /// Write how you want your mail transfer progress style to be. Leave blank to use default style. Placeholders are parsed. {0} for transferred size and {1} for total size.
        /// </summary>
        public string MailProgressStyle { get; set; } = "";
        /// <summary>
        /// Write how you want your mail transfer progress style to be. Leave blank to use default style. Placeholders are parsed. {0} for transferred size.
        /// </summary>
        public string MailProgressStyleSingle { get; set; } = "";
        /// <summary>
        /// Shows the notification showing the download progress
        /// </summary>
        public bool DownloadNotificationProvoke { get; set; }
        /// <summary>
        /// Shows the notification showing the upload progress
        /// </summary>
        public bool UploadNotificationProvoke { get; set; }
        /// <summary>
        /// How many milliseconds to wait before RSS feed fetch timeout?
        /// </summary>
        public int RSSFetchTimeout
        {
            get => RSSShellCommon.fetchTimeout;
            set => RSSShellCommon.fetchTimeout = value < 0 ? 60000 : value;
        }
        /// <summary>
        /// How many times to verify the upload and download and retry if the verification fails before the download fails as a whole?
        /// </summary>
        public int FtpVerifyRetryAttempts
        {
            get => FTPShellCommon.verifyRetryAttempts;
            set => FTPShellCommon.verifyRetryAttempts = value < 0 ? 3 : value;
        }
        /// <summary>
        /// How many milliseconds to wait before the FTP connection timeout?
        /// </summary>
        public int FtpConnectTimeout
        {
            get => FTPShellCommon.connectTimeout;
            set => FTPShellCommon.connectTimeout = value < 0 ? 15000 : value;
        }
        /// <summary>
        /// How many milliseconds to wait before the FTP data connection timeout?
        /// </summary>
        public int FtpDataConnectTimeout
        {
            get => FTPShellCommon.dataConnectTimeout;
            set => FTPShellCommon.dataConnectTimeout = value < 0 ? 15000 : value;
        }
        /// <summary>
        /// Choose the version of Internet Protocol that the FTP server supports and that the FTP client uses
        /// </summary>
        public int FtpProtocolVersions { get; set; } = (int)FtpIpVersion.ANY;
        /// <summary>
        /// If enabled, will use the notification system to notify the host of remote debug connection error. Otherwise, will use the default console writing.
        /// </summary>
        public bool NotifyOnRemoteDebugConnectionError { get; set; } = true;
        /// <summary>
        /// Which screensaver do you want to lock your screen with?
        /// </summary>
        public string DefaultSaverName
        {
            get => ScreensaverManager.defSaverName;
            set => ScreensaverManager.defSaverName = ScreensaverManager.Screensavers.ContainsKey(value) ? value : "plain";
        }
        /// <summary>
        /// Write when to launch screensaver after specified milliseconds. It must be numeric
        /// </summary>
        public int ScreenTimeout
        {
            get => ScreensaverManager.scrnTimeout;
            set => ScreensaverManager.scrnTimeout = value < 0 ? 300000 : value;
        }
        /// <summary>
        /// Enables debugging for screensavers. Please note that it may quickly fill the debug log and slightly slow the screensaver down, depending on the screensaver used. Only works if kernel debugging is enabled for diagnostic purposes.
        /// </summary>
        public bool ScreensaverDebug { get; set; }
        /// <summary>
        /// After locking the screen, ask for password
        /// </summary>
        public bool PasswordLock { get; set; } = true;
        /// <summary>
        /// Current console driver
        /// </summary>
        public string CurrentConsoleDriver
        {
            get => DriverHandler.GetDriverName<IConsoleDriver>(DriverHandler.CurrentConsoleDriver);
            set => ConsoleDriverTools.SetConsoleDriver(value);
        }
        /// <summary>
        /// Current random number generator driver
        /// </summary>
        public string CurrentRandomDriver
        {
            get => DriverHandler.GetDriverName<IRandomDriver>(DriverHandler.CurrentRandomDriver);
            set => RandomDriverTools.SetRandomDriver(value);
        }
        /// <summary>
        /// Current network driver
        /// </summary>
        public string CurrentNetworkDriver
        {
            get => DriverHandler.GetDriverName<INetworkDriver>(DriverHandler.CurrentNetworkDriver);
            set => NetworkDriverTools.SetNetworkDriver(value);
        }
        /// <summary>
        /// Current filesystem driver
        /// </summary>
        public string CurrentFilesystemDriver
        {
            get => DriverHandler.GetDriverName<IFilesystemDriver>(DriverHandler.CurrentFilesystemDriver);
            set => FilesystemDriverTools.SetFilesystemDriver(value);
        }
        /// <summary>
        /// Current encryption driver
        /// </summary>
        public string CurrentEncryptionDriver
        {
            get => DriverHandler.GetDriverName<IEncryptionDriver>(DriverHandler.CurrentEncryptionDriver);
            set => EncryptionDriverTools.SetEncryptionDriver(value);
        }
        /// <summary>
        /// Current regular expression driver
        /// </summary>
        public string CurrentRegexpDriver
        {
            get => DriverHandler.GetDriverName<IRegexpDriver>(DriverHandler.CurrentRegexpDriver);
            set => RegexpDriverTools.SetRegexpDriver(value);
        }
        /// <summary>
        /// Current regular expression driver
        /// </summary>
        public string CurrentDebugLoggerDriver
        {
            get => DriverHandler.GetDriverName<IDebugLoggerDriver>(DriverHandler.CurrentDebugLoggerDriver);
            set => DebugLoggerDriverTools.SetDebugLoggerDriver(value);
        }
        /// <summary>
        /// Current encoding driver
        /// </summary>
        public string CurrentEncodingDriver
        {
            get => DriverHandler.GetDriverName<IEncodingDriver>(DriverHandler.CurrentEncodingDriver);
            set => EncodingDriverTools.SetEncodingDriver(value);
        }
        /// <summary>
        /// Current hardware prober driver
        /// </summary>
        public string CurrentHardwareProberDriver
        {
            get => DriverHandler.GetDriverName<IHardwareProberDriver>(DriverHandler.CurrentHardwareProberDriver);
            set => HardwareProberDriverTools.SetHardwareProberDriver(value);
        }
        /// <summary>
        /// The time and date will be shown in the upper right corner of the screen
        /// </summary>
        public bool CornerTimeDate { get; set; }
        /// <summary>
        /// Enables eyecandy on startup
        /// </summary>
        public bool StartScroll { get; set; } = true;
        /// <summary>
        /// The time and date will be longer, showing full month names, etc.
        /// </summary>
        public bool LongTimeDate { get; set; } = true;
        /// <summary>
        /// Turns on or off the text editor autosave feature
        /// </summary>
        public bool TextEditAutoSaveFlag { get; set; } = true;
        /// <summary>
        /// If autosave is enabled, the text file will be saved for each "n" seconds
        /// </summary>
        public int TextEditAutoSaveInterval
        {
            get => TextEditShellCommon.autoSaveInterval;
            set => TextEditShellCommon.autoSaveInterval = value < 0 ? 60 : value;
        }
        /// <summary>
        /// Turns on or off the hex editor autosave feature
        /// </summary>
        public bool HexEditAutoSaveFlag { get; set; } = true;
        /// <summary>
        /// If autosave is enabled, the binary file will be saved for each "n" seconds
        /// </summary>
        public int HexEditAutoSaveInterval
        {
            get => HexEditShellCommon.autoSaveInterval;
            set => HexEditShellCommon.autoSaveInterval = value < 0 ? 60 : value;
        }
        /// <summary>
        /// Turns on or off the hex editor autosave feature
        /// </summary>
        public bool JsonEditAutoSaveFlag { get; set; } = true;
        /// <summary>
        /// If autosave is enabled, the binary file will be saved for each "n" seconds
        /// </summary>
        public int JsonEditAutoSaveInterval
        {
            get => JsonShellCommon.autoSaveInterval;
            set => JsonShellCommon.autoSaveInterval = value < 0 ? 60 : value;
        }
        /// <summary>
        /// Wraps the list outputs if it seems too long for the current console geometry
        /// </summary>
        public bool WrapListOutputs { get; set; }
        /// <summary>
        /// Covers the notification with the border
        /// </summary>
        public bool DrawBorderNotification { get; set; } = true;
        /// <summary>
        /// Write the filenames of the mods that will not run on startup. When you're finished, write "q". Write a minus sign next to the path to remove an existing mod.
        /// </summary>
        public string BlacklistedModsString { get; set; } = "";
        /// <summary>
        /// A character that resembles the upper left corner. Be sure to only input one character
        /// </summary>
        public char NotifyUpperLeftCornerChar
        {
            get => NotificationManager.notifyUpperLeftCornerChar;
            set => NotificationManager.notifyUpperLeftCornerChar = value;
        }
        /// <summary>
        /// A character that resembles the upper right corner. Be sure to only input one character
        /// </summary>
        public char NotifyUpperRightCornerChar
        {
            get => NotificationManager.notifyUpperRightCornerChar;
            set => NotificationManager.notifyUpperRightCornerChar = value;
        }
        /// <summary>
        /// A character that resembles the lower left corner. Be sure to only input one character
        /// </summary>
        public char NotifyLowerLeftCornerChar
        {
            get => NotificationManager.notifyLowerLeftCornerChar;
            set => NotificationManager.notifyLowerLeftCornerChar = value;
        }
        /// <summary>
        /// A character that resembles the lower right corner. Be sure to only input one character
        /// </summary>
        public char NotifyLowerRightCornerChar
        {
            get => NotificationManager.notifyLowerRightCornerChar;
            set => NotificationManager.notifyLowerRightCornerChar = value;
        }
        /// <summary>
        /// A character that resembles the upper frame. Be sure to only input one character
        /// </summary>
        public char NotifyUpperFrameChar
        {
            get => NotificationManager.notifyUpperFrameChar;
            set => NotificationManager.notifyUpperFrameChar = value;
        }
        /// <summary>
        /// A character that resembles the lower frame. Be sure to only input one character
        /// </summary>
        public char NotifyLowerFrameChar
        {
            get => NotificationManager.notifyLowerFrameChar;
            set => NotificationManager.notifyLowerFrameChar = value;
        }
        /// <summary>
        /// A character that resembles the left frame. Be sure to only input one character
        /// </summary>
        public char NotifyLeftFrameChar
        {
            get => NotificationManager.notifyLeftFrameChar;
            set => NotificationManager.notifyLeftFrameChar = value;
        }
        /// <summary>
        /// A character that resembles the right frame. Be sure to only input one character
        /// </summary>
        public char NotifyRightFrameChar
        {
            get => NotificationManager.notifyRightFrameChar;
            set => NotificationManager.notifyRightFrameChar = value;
        }
        /// <summary>
        /// Each login, it will show the latest RSS headline from the selected headline URL
        /// </summary>
        public bool ShowHeadlineOnLogin { get; set; }
        /// <summary>
        /// RSS headline URL to be used when showing the latest headline. This is usually your favorite feed
        /// </summary>
        public string RssHeadlineUrl { get; set; } = "https://www.techrepublic.com/rssfeeds/articles/";
        /// <summary>
        /// Selects the default JSON formatting (beautified or minified) for the JSON shell to save
        /// </summary>
        public int JsonShellFormatting { get; set; } = (int)Formatting.Indented;
        /// <summary>
        /// Shows the commands count in the command list, controlled by the three count show switches for different kinds of commands.
        /// </summary>
        public bool ShowCommandsCount { get; set; }
        /// <summary>
        /// Show the shell commands count on help
        /// </summary>
        public bool ShowShellCommandsCount { get; set; } = true;
        /// <summary>
        /// Show the mod commands count on help
        /// </summary>
        public bool ShowModCommandsCount { get; set; } = true;
        /// <summary>
        /// Show the aliases count on help
        /// </summary>
        public bool ShowShellAliasesCount { get; set; } = true;
        /// <summary>
        /// Show the unified commands count on help
        /// </summary>
        public bool ShowUnifiedCommandsCount { get; set; } = true;
        /// <summary>
        /// Show the addon commands count on help
        /// </summary>
        public bool ShowAddonCommandsCount { get; set; } = true;
        /// <summary>
        /// A character that masks the password. Leave blank for more security
        /// </summary>
        public string CurrentMask
        {
            get => Input.currentMask;
            set => Input.currentMask = string.IsNullOrEmpty(value) ? "*" : value[0].ToString();
        }
        /// <summary>
        /// A character that resembles the upper left corner. Be sure to only input one character
        /// </summary>
        public char ProgressUpperLeftCornerChar
        {
            get => ProgressTools.progressUpperLeftCornerChar;
            set => ProgressTools.progressUpperLeftCornerChar = value;
        }
        /// <summary>
        /// A character that resembles the upper right corner. Be sure to only input one character
        /// </summary>
        public char ProgressUpperRightCornerChar
        {
            get => ProgressTools.progressUpperRightCornerChar;
            set => ProgressTools.progressUpperRightCornerChar = value;
        }
        /// <summary>
        /// A character that resembles the lower left corner. Be sure to only input one character
        /// </summary>
        public char ProgressLowerLeftCornerChar
        {
            get => ProgressTools.progressLowerLeftCornerChar;
            set => ProgressTools.progressLowerLeftCornerChar = value;
        }
        /// <summary>
        /// A character that resembles the lower right corner. Be sure to only input one character
        /// </summary>
        public char ProgressLowerRightCornerChar
        {
            get => ProgressTools.progressLowerRightCornerChar;
            set => ProgressTools.progressLowerRightCornerChar = value;
        }
        /// <summary>
        /// A character that resembles the upper frame. Be sure to only input one character
        /// </summary>
        public char ProgressUpperFrameChar
        {
            get => ProgressTools.progressUpperFrameChar;
            set => ProgressTools.progressUpperFrameChar = value;
        }
        /// <summary>
        /// A character that resembles the lower frame. Be sure to only input one character
        /// </summary>
        public char ProgressLowerFrameChar
        {
            get => ProgressTools.progressLowerFrameChar;
            set => ProgressTools.progressLowerFrameChar = value;
        }
        /// <summary>
        /// A character that resembles the left frame. Be sure to only input one character
        /// </summary>
        public char ProgressLeftFrameChar
        {
            get => ProgressTools.progressLeftFrameChar;
            set => ProgressTools.progressLeftFrameChar = value;
        }
        /// <summary>
        /// A character that resembles the right frame. Be sure to only input one character
        /// </summary>
        public char ProgressRightFrameChar
        {
            get => ProgressTools.progressRightFrameChar;
            set => ProgressTools.progressRightFrameChar = value;
        }
        /// <summary>
        /// Whether the input history is enabled or not. If enabled, you can access recently typed commands using the up or down arrow keys.
        /// </summary>
        public bool InputHistoryEnabled
        {
            get
            {
                return Input.globalSettings.HistoryEnabled;
            }
            set
            {
                Input.globalSettings.HistoryEnabled = value;
            }
        }
        /// <summary>
        /// Enables the scroll bar in selection screens
        /// </summary>
        public bool EnableScrollBarInSelection { get; set; } = true;
        /// <summary>
        /// If Do Not Disturb is enabled, all notifications received will be supressed from the display. This means that you won't be able to see any notification to help you focus.
        /// </summary>
        public bool DoNotDisturb
        {
            get => NotificationManager.dnd;
            set => NotificationManager.dnd = value;
        }
        /// <summary>
        /// A character that resembles the upper left corner. Be sure to only input one character.
        /// </summary>
        public char BorderUpperLeftCornerChar
        {
            get => BorderTools._borderUpperLeftCornerChar;
            set => BorderTools._borderUpperLeftCornerChar = value;
        }
        /// <summary>
        /// A character that resembles the upper right corner. Be sure to only input one character.
        /// </summary>
        public char BorderUpperRightCornerChar
        {
            get => BorderTools._borderUpperRightCornerChar;
            set => BorderTools._borderUpperRightCornerChar = value;
        }
        /// <summary>
        /// A character that resembles the lower left corner. Be sure to only input one character.
        /// </summary>
        public char BorderLowerLeftCornerChar
        {
            get => BorderTools._borderLowerLeftCornerChar;
            set => BorderTools._borderLowerLeftCornerChar = value;
        }
        /// <summary>
        /// A character that resembles the lower right corner. Be sure to only input one character.
        /// </summary>
        public char BorderLowerRightCornerChar
        {
            get => BorderTools._borderLowerRightCornerChar;
            set => BorderTools._borderLowerRightCornerChar = value;
        }
        /// <summary>
        /// A character that resembles the upper frame. Be sure to only input one character.
        /// </summary>
        public char BorderUpperFrameChar
        {
            get => BorderTools._borderUpperFrameChar;
            set => BorderTools._borderUpperFrameChar = value;
        }
        /// <summary>
        /// A character that resembles the lower frame. Be sure to only input one character.
        /// </summary>
        public char BorderLowerFrameChar
        {
            get => BorderTools._borderLowerFrameChar;
            set => BorderTools._borderLowerFrameChar = value;
        }
        /// <summary>
        /// A character that resembles the left frame. Be sure to only input one character.
        /// </summary>
        public char BorderLeftFrameChar
        {
            get => BorderTools._borderLeftFrameChar;
            set => BorderTools._borderLeftFrameChar = value;
        }
        /// <summary>
        /// A character that resembles the right frame. Be sure to only input one character.
        /// </summary>
        public char BorderRightFrameChar
        {
            get => BorderTools._borderRightFrameChar;
            set => BorderTools._borderRightFrameChar = value;
        }
        /// <summary>
        /// Censor private information that may be printed to the debug logs.
        /// </summary>
        public bool DebugCensorPrivateInfo { get; set; }
        /// <summary>
        /// Shows all new notifications as asterisks. This option is ignored in notifications with progress bar.
        /// </summary>
        public bool NotifyDisplayAsAsterisk { get; set; }
        /// <summary>
        /// Whether to show the file size in the status
        /// </summary>
        public bool IfmShowFileSize { get; set; }
        /// <summary>
        /// If enabled, uses the classic header style in the settings app. Otherwise, the new one.
        /// </summary>
        public bool ClassicSettingsHeaderStyle { get; set; }
        /// <summary>
        /// If enabled, opts you in to the new color selector.
        /// </summary>
        public bool UseNewColorSelector { get; set; }
        /// <summary>
        /// Specifies the default figlet font name
        /// </summary>
        public string DefaultFigletFontName
        {
            get => TextTools.defaultFigletFontName;
            set => TextTools.defaultFigletFontName = FigletTools.GetFigletFonts().ContainsKey(value) ? value : "speed";
        }
    }
}
