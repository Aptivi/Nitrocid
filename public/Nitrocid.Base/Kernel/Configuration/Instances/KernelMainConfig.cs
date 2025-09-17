//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using Newtonsoft.Json;
using System;
using Terminaux.Colors;
using Textify.Data.Figlet;
using Nitrocid.Base.Files;
using Terminaux.Inputs.Styles.Choice;
using Terminaux.Shell.Prompts;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Inputs;
using Terminaux.Reader;
using System.Linq;
using Terminaux.Base.Extensions;
using Nitrocid.Base.Kernel.Configuration.Settings;
using Nitrocid.Base.ConsoleBase;
using Nitrocid.Base.Network;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Shell.Shells.Text;
using Nitrocid.Base.Kernel.Debugging.RemoteDebug;
using Nitrocid.Base.Shell.Shells.Hex;
using Nitrocid.Base.Kernel.Threading.Performance;
using Nitrocid.Base.Misc.Audio;
using Nitrocid.Base.Misc.Screensaver;
using Nitrocid.Base.Misc.Reflection.Internal;
using Nitrocid.Base.Kernel.Starting;
using Nitrocid.Base.Misc.Notifications;
using Nitrocid.Base.Users.Login;
using Nitrocid.Base.Users.Login.Widgets;
using Nitrocid.Base.Users.Login.Widgets.Implementations;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Users.Login.Handlers;
using Nitrocid.Base.Files.Paths;
using Nitrocid.Base.ConsoleBase.Inputs;
using Nitrocid.Base.Files.Folders;
using Nitrocid.Base.Kernel.Debugging.RemoteDebug.RemoteChat;
using Nitrocid.Base.Kernel.Time.Timezones;
using Nitrocid.Base.Network.Types.RPC;
using Nitrocid.Base.Shell.Homepage;

namespace Nitrocid.Base.Kernel.Configuration.Instances
{
    /// <summary>
    /// Main kernel configuration instance
    /// </summary>
    public class KernelMainConfig : BaseKernelConfig
    {
        /// <inheritdoc/>
        [JsonIgnore]
        public override string Name =>
            LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_INSTANCES_MAINSETTINGS");

        /// <inheritdoc/>
        [JsonIgnore]
        public override SettingsEntry[] SettingsEntries
        {
            get
            {
                var dataStream = ResourcesManager.GetData("SettingsEntries.json", ResourcesType.Settings) ??
                    throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_EXCEPTION_ENTRIESFAILED_MAIN"));
                string dataString = ResourcesManager.ConvertToString(dataStream);
                return ConfigTools.GetSettingsEntries(dataString);
            }
        }

        [JsonIgnore]
        private string defaultFigletFontName = "speed";

        #region General
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
        /// Specifies the kernel language.
        /// </summary>
        public string CurrentLanguage
        {
            get => LanguageManager.currentLanguage is not null ? LanguageManager.currentLanguage.ThreeLetterLanguageName : "eng";
            set => LanguageManager.SetLangDry(value);
        }
        /// <summary>
        /// Which culture is being used to change the month names, calendar, etc.?
        /// </summary>
        public string CurrentCultureName { get; set; } = "en-US";
        /// <summary>
        /// Shows brief information about the application on boot.
        /// </summary>
        public bool ShowAppInfoOnBoot { get; set; } = true;
        /// <summary>
        /// Shows how much time did the kernel take to finish a stage.
        /// </summary>
        public bool ShowStageFinishTimes { get; set; }
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
        /// Enables beeping upon shutting down the kernel.
        /// </summary>
        public bool BeepOnShutdown { get; set; }
        /// <summary>
        /// Enables delaying upon shutting down the kernel.
        /// </summary>
        public bool DelayOnShutdown { get; set; }
        /// <summary>
        /// If you are sure that the console supports true color, or if you want to change your terminal to a terminal that supports true color, change this value.
        /// </summary>
        public bool ConsoleSupportsTrueColor
        {
            get => ColorTools.ConsoleSupportsTrueColor;
            set => ColorTools.ConsoleSupportsTrueColor = value;
        }
        /// <summary>
        /// Whether to use the operating system time zone or to use the kernel-wide time zone
        /// </summary>
        public bool UseSystemTimeZone
        {
            get => TimeZones.useSystemTimezone;
            set => TimeZones.useSystemTimezone = value;
        }
        /// <summary>
        /// The kenrnel-wide time zone name
        /// </summary>
        public string KernelWideTimeZone
        {
            get => TimeZones.defaultZoneName;
            set => TimeZones.defaultZoneName = TimeZones.TimeZoneExists(value) ? value : TimeZones.defaultZoneName;
        }
        /// <summary>
        /// Shows an informational box for the program license for fifteen seconds after each login
        /// </summary>
        public bool ShowLicenseInfoBox { get; set; } = true;
        /// <summary>
        /// Bootloader style
        /// </summary>
        public string BootStyle { get; set; } = "Default";
        /// <summary>
        /// Timeout to boot to the default selection
        /// </summary>
        public int BootSelectTimeoutSeconds { get; set; } = 10;
        /// <summary>
        /// The default boot entry FilesystemTools. This number is zero-based, so the first element is index 0, and so on.
        /// </summary>
        public int BootSelect { get; set; } = 0;
        /// <summary>
        /// Enables "The Nitrocid Homepage"
        /// </summary>
        public bool EnableHomepage
        {
            get => HomepageTools.isHomepageEnabled;
            set => HomepageTools.isHomepageEnabled = value;
        }
        /// <summary>
        /// Enables "The Nitrocid Homepage" widgets
        /// </summary>
        public bool EnableHomepageWidgets
        {
            get => HomepageTools.isHomepageWidgetEnabled;
            set => HomepageTools.isHomepageWidgetEnabled = value;
        }
        /// <summary>
        /// Enables "The Nitrocid Homepage" RSS feed widget
        /// </summary>
        public bool EnableHomepageRssFeed
        {
            get => HomepageTools.isHomepageRssFeedEnabled;
            set => HomepageTools.isHomepageRssFeedEnabled = value;
        }
        /// <summary>
        /// Select a widget to be displayed in the widget pane of the homepage
        /// </summary>
        public string HomepageWidget
        {
            get => HomepageTools.homepageWidgetName;
            set => HomepageTools.homepageWidgetName = WidgetTools.CheckWidget(value) ? value : nameof(AnalogClock);
        }
        #endregion

        #region Colors
        /// <summary>
        /// Whether to use accent colors for themes that support accents
        /// </summary>
        public bool UseAccentColors
        {
            get => ThemeColorsTools.UseAccentColors;
            set => ThemeColorsTools.UseAccentColors = value;
        }
        /// <summary>
        /// Accent color (foreground)
        /// </summary>
        public string AccentForegroundColor
        {
            get => ThemeColorsTools.AccentForegroundColor;
            set => ThemeColorsTools.AccentForegroundColor = value;
        }
        /// <summary>
        /// Accent color (background)
        /// </summary>
        public string AccentBackgroundColor
        {
            get => ThemeColorsTools.AccentBackgroundColor;
            set => ThemeColorsTools.AccentBackgroundColor = value;
        }
        /// <summary>
        /// Whether to use accent colors for themes that support accents
        /// </summary>
        public bool UseConsoleColorPalette
        {
            get => ColorTools.GlobalSettings.UseTerminalPalette;
            set => ColorTools.GlobalSettings.UseTerminalPalette = value;
        }
        /// <summary>
        /// Whether to allow background color
        /// </summary>
        public bool AllowBackgroundColor
        {
            get => ColorTools.AllowBackground;
            set => ColorTools.AllowBackground = value;
        }
        /// <summary>
        /// User Name Shell Color
        /// </summary>
        public string UserNameShellColor
        {
            get => ThemeColorsTools.GetColor("UserNameShellColor").PlainSequence;
            set => ThemeColorsTools.SetColor("UserNameShellColor", new Color(value));
        }
        /// <summary>
        /// Host Name Shell Color
        /// </summary>
        public string HostNameShellColor
        {
            get => ThemeColorsTools.GetColor("HostNameShellColor").PlainSequence;
            set => ThemeColorsTools.SetColor("HostNameShellColor", new Color(value));
        }
        /// <summary>
        /// Continuable Kernel Error Color
        /// </summary>
        public string ContinuableKernelErrorColor
        {
            get => ThemeColorsTools.GetColor("ContKernelErrorColor").PlainSequence;
            set => ThemeColorsTools.SetColor("ContKernelErrorColor", new Color(value));
        }
        /// <summary>
        /// Uncontinuable Kernel Error Color
        /// </summary>
        public string UncontinuableKernelErrorColor
        {
            get => ThemeColorsTools.GetColor("UncontKernelErrorColor").PlainSequence;
            set => ThemeColorsTools.SetColor("UncontKernelErrorColor", new Color(value));
        }
        /// <summary>
        /// Text Color
        /// </summary>
        public string TextColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.NeutralText).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.NeutralText, new Color(value));
        }
        /// <summary>
        /// License Color
        /// </summary>
        public string LicenseColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.License).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.License, new Color(value));
        }
        /// <summary>
        /// Background Color
        /// </summary>
        public string BackgroundColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.Background).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.Background, new Color(value));
        }
        /// <summary>
        /// Input Color
        /// </summary>
        public string InputColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.Input).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.Input, new Color(value));
        }
        /// <summary>
        /// List Entry Color
        /// </summary>
        public string ListEntryColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.ListEntry).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.ListEntry, new Color(value));
        }
        /// <summary>
        /// List Value Color
        /// </summary>
        public string ListValueColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.ListValue).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.ListValue, new Color(value));
        }
        /// <summary>
        /// Kernel Stage Color
        /// </summary>
        public string KernelStageColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.Stage).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.Stage, new Color(value));
        }
        /// <summary>
        /// Error Text Color
        /// </summary>
        public string ErrorTextColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.Error).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.Error, new Color(value));
        }
        /// <summary>
        /// Warning Text Color
        /// </summary>
        public string WarningTextColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.Warning).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.Warning, new Color(value));
        }
        /// <summary>
        /// Option Color
        /// </summary>
        public string OptionColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.Option).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.Option, new Color(value));
        }
        /// <summary>
        /// Banner Color
        /// </summary>
        public string BannerColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.Banner).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.Banner, new Color(value));
        }
        /// <summary>
        /// Notification Title Color
        /// </summary>
        public string NotificationTitleColor
        {
            get => ThemeColorsTools.GetColor("NotificationTitleColor").PlainSequence;
            set => ThemeColorsTools.SetColor("NotificationTitleColor", new Color(value));
        }
        /// <summary>
        /// Notification Description Color
        /// </summary>
        public string NotificationDescriptionColor
        {
            get => ThemeColorsTools.GetColor("NotificationDescriptionColor").PlainSequence;
            set => ThemeColorsTools.SetColor("NotificationDescriptionColor", new Color(value));
        }
        /// <summary>
        /// Notification Progress Color
        /// </summary>
        public string NotificationProgressColor
        {
            get => ThemeColorsTools.GetColor("NotificationProgressColor").PlainSequence;
            set => ThemeColorsTools.SetColor("NotificationProgressColor", new Color(value));
        }
        /// <summary>
        /// Notification Failure Color
        /// </summary>
        public string NotificationFailureColor
        {
            get => ThemeColorsTools.GetColor("NotificationFailureColor").PlainSequence;
            set => ThemeColorsTools.SetColor("NotificationFailureColor", new Color(value));
        }
        /// <summary>
        /// Question Color
        /// </summary>
        public string QuestionColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.Question).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.Question, new Color(value));
        }
        /// <summary>
        /// Success Color
        /// </summary>
        public string SuccessColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.Success).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.Success, new Color(value));
        }
        /// <summary>
        /// User Dollar Color
        /// </summary>
        public string UserDollarColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.UserDollar).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.UserDollar, new Color(value));
        }
        /// <summary>
        /// Tip Color
        /// </summary>
        public string TipColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.Tip).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.Tip, new Color(value));
        }
        /// <summary>
        /// Separator Text Color
        /// </summary>
        public string SeparatorTextColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.SeparatorText).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.SeparatorText, new Color(value));
        }
        /// <summary>
        /// Separator Color
        /// </summary>
        public string SeparatorColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.Separator).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.Separator, new Color(value));
        }
        /// <summary>
        /// List Title Color
        /// </summary>
        public string ListTitleColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.ListTitle).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.ListTitle, new Color(value));
        }
        /// <summary>
        /// Development Warning Color
        /// </summary>
        public string DevelopmentWarningColor
        {
            get => ThemeColorsTools.GetColor("DevelopmentWarningColor").PlainSequence;
            set => ThemeColorsTools.SetColor("DevelopmentWarningColor", new Color(value));
        }
        /// <summary>
        /// Stage Time Color
        /// </summary>
        public string StageTimeColor
        {
            get => ThemeColorsTools.GetColor("StageTimeColor").PlainSequence;
            set => ThemeColorsTools.SetColor("StageTimeColor", new Color(value));
        }
        /// <summary>
        /// Progress Color
        /// </summary>
        public string ProgressColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.Progress).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.Progress, new Color(value));
        }
        /// <summary>
        /// Back Option Color
        /// </summary>
        public string BackOptionColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.BackOption).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.BackOption, new Color(value));
        }
        /// <summary>
        /// Low Priority Border Color
        /// </summary>
        public string LowPriorityBorderColor
        {
            get => ThemeColorsTools.GetColor("LowPriorityBorderColor").PlainSequence;
            set => ThemeColorsTools.SetColor("LowPriorityBorderColor", new Color(value));
        }
        /// <summary>
        /// Medium Priority Border Color
        /// </summary>
        public string MediumPriorityBorderColor
        {
            get => ThemeColorsTools.GetColor("MediumPriorityBorderColor").PlainSequence;
            set => ThemeColorsTools.SetColor("MediumPriorityBorderColor", new Color(value));
        }
        /// <summary>
        /// High Priority Border Color
        /// </summary>
        public string HighPriorityBorderColor
        {
            get => ThemeColorsTools.GetColor("HighPriorityBorderColor").PlainSequence;
            set => ThemeColorsTools.SetColor("HighPriorityBorderColor", new Color(value));
        }
        /// <summary>
        /// Table Separator Color
        /// </summary>
        public string TableSeparatorColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.TableSeparator).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.TableSeparator, new Color(value));
        }
        /// <summary>
        /// Table Header Color
        /// </summary>
        public string TableHeaderColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.TableHeader).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.TableHeader, new Color(value));
        }
        /// <summary>
        /// Table Value Color
        /// </summary>
        public string TableValueColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.TableValue).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.TableValue, new Color(value));
        }
        /// <summary>
        /// Selected Option Color
        /// </summary>
        public string SelectedOptionColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.SelectedOption).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.SelectedOption, new Color(value));
        }
        /// <summary>
        /// Alternative Option Color
        /// </summary>
        public string AlternativeOptionColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.AlternativeOption).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.AlternativeOption, new Color(value));
        }
        /// <summary>
        /// Weekend Day Color
        /// </summary>
        public string WeekendDayColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.WeekendDay).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.WeekendDay, new Color(value));
        }
        /// <summary>
        /// Event Day Color
        /// </summary>
        public string EventDayColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.EventDay).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.EventDay, new Color(value));
        }
        /// <summary>
        /// Table Title Color
        /// </summary>
        public string TableTitleColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.TableTitle).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.TableTitle, new Color(value));
        }
        /// <summary>
        /// Today Day Color
        /// </summary>
        public string TodayDayColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.TodayDay).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.TodayDay, new Color(value));
        }
        /// <summary>
        /// Interactive TUI background color
        /// </summary>
        public string TuiBackgroundColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.TuiBackground).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.TuiBackground, new Color(value));
        }
        /// <summary>
        /// Interactive TUI foreground color
        /// </summary>
        public string TuiForegroundColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.TuiForeground).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.TuiForeground, new Color(value));
        }
        /// <summary>
        /// Interactive TUI pane background color
        /// </summary>
        public string TuiPaneBackgroundColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.TuiPaneBackground).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.TuiPaneBackground, new Color(value));
        }
        /// <summary>
        /// Interactive TUI pane separator color
        /// </summary>
        public string TuiPaneSeparatorColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.TuiPaneSeparator).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.TuiPaneSeparator, new Color(value));
        }
        /// <summary>
        /// Interactive TUI selected pane separator color
        /// </summary>
        public string TuiPaneSelectedSeparatorColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.TuiPaneSelectedSeparator).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.TuiPaneSelectedSeparator, new Color(value));
        }
        /// <summary>
        /// Interactive TUI selected pane item foreground color
        /// </summary>
        public string TuiPaneSelectedItemForeColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.TuiPaneSelectedItemFore).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.TuiPaneSelectedItemFore, new Color(value));
        }
        /// <summary>
        /// Interactive TUI selected pane item background color
        /// </summary>
        public string TuiPaneSelectedItemBackColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.TuiPaneSelectedItemBack).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.TuiPaneSelectedItemBack, new Color(value));
        }
        /// <summary>
        /// Interactive TUI pane item foreground color
        /// </summary>
        public string TuiPaneItemForeColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.TuiPaneItemFore).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.TuiPaneItemFore, new Color(value));
        }
        /// <summary>
        /// Interactive TUI pane item background color
        /// </summary>
        public string TuiPaneItemBackColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.TuiPaneItemBack).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.TuiPaneItemBack, new Color(value));
        }
        /// <summary>
        /// Interactive TUI option background color
        /// </summary>
        public string TuiOptionBackgroundColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.TuiOptionBackground).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.TuiOptionBackground, new Color(value));
        }
        /// <summary>
        /// Interactive TUI option foreground color
        /// </summary>
        public string TuiOptionForegroundColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.TuiOptionForeground).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.TuiOptionForeground, new Color(value));
        }
        /// <summary>
        /// Interactive TUI option binding name color
        /// </summary>
        public string TuiKeyBindingOptionColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.TuiKeyBindingOption).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.TuiKeyBindingOption, new Color(value));
        }
        /// <summary>
        /// Interactive TUI box background color
        /// </summary>
        public string TuiBoxBackgroundColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.TuiBoxBackground).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.TuiBoxBackground, new Color(value));
        }
        /// <summary>
        /// Interactive TUI box foreground color
        /// </summary>
        public string TuiBoxForegroundColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.TuiBoxForeground).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.TuiBoxForeground, new Color(value));
        }
        /// <summary>
        /// Disabled option color
        /// </summary>
        public string DisabledOptionColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.DisabledOption).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.DisabledOption, new Color(value));
        }
        /// <summary>
        /// Interactive TUI builtin key binding background color
        /// </summary>
        public string TuiKeyBindingBuiltinBackgroundColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.TuiKeyBindingBuiltinBackground).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.TuiKeyBindingBuiltinBackground, new Color(value));
        }
        /// <summary>
        /// Interactive TUI builtin key binding foreground color
        /// </summary>
        public string TuiKeyBindingBuiltinForegroundColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.TuiKeyBindingBuiltinForeground).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.TuiKeyBindingBuiltinForeground, new Color(value));
        }
        /// <summary>
        /// Interactive TUI builtin key binding color
        /// </summary>
        public string TuiKeyBindingBuiltinColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.TuiKeyBindingBuiltin).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.TuiKeyBindingBuiltin, new Color(value));
        }
        #endregion

        #region Hardware
        /// <summary>
        /// Keep hardware probing messages silent.
        /// </summary>
        public bool QuietHardwareProbe { get; set; }
        /// <summary>
        /// Make hardware probing messages a bit talkative.
        /// </summary>
        public bool VerboseHardwareProbe { get; set; }
        #endregion

        #region Login
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
        public string MotdFilePath { get; set; } = PathsManagement.GetKernelPath(KernelPathType.MOTD);
        /// <summary>
        /// Which file is the MAL text file? Write an absolute path to the text file
        /// </summary>
        public string MalFilePath { get; set; } = PathsManagement.GetKernelPath(KernelPathType.MAL);
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
        /// Enables the widgets in the modern logon handler and all the handlers that use the widget API.
        /// </summary>
        public bool EnableWidgets
        {
            get => ModernLogonScreen.enableWidgets;
            set => ModernLogonScreen.enableWidgets = value;
        }
        /// <summary>
        /// First widget for the modern logon handler. You can configure this widget in its respective settings entry.
        /// </summary>
        public string FirstWidget
        {
            get => ModernLogonScreen.firstWidgetName;
            set => ModernLogonScreen.firstWidgetName = value;
        }
        /// <summary>
        /// Second widget for the modern logon handler. You can configure this widget in its respective settings entry.
        /// </summary>
        public string SecondWidget
        {
            get => ModernLogonScreen.secondWidgetName;
            set => ModernLogonScreen.secondWidgetName = value;
        }
        #endregion

        #region Shell
        private string pathsToLookup = Environment.GetEnvironmentVariable("PATH") ?? "";

        /// <summary>
        /// Simplified help command for all the shells
        /// </summary>
        public bool SimHelp { get; set; }
        /// <summary>
        /// Sets the shell's current directory. Write an absolute path to any existing directory
        /// </summary>
        public string CurrentDir
        {
            get => FilesystemTools._CurrentDirectory;
            set
            {
                value = FilesystemTools.NeutralizePath(value);
                if (FilesystemTools.FolderExists(value))
                {
                    FilesystemTools._CurrentDirectory = value;
                    ConsoleFilesystem.CurrentDir = value;
                }
                else
                    throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXCEPTION_DIRECTORYNOTFOUND2"), value);
            }
        }
        /// <summary>
        /// Group of paths separated by the colon. It works the same as PATH. Write a full path to a folder or a folder name. When you're finished, write \"q\". Write a minus sign next to the path to remove an existing directory.
        /// </summary>
        public string PathsToLookup
        {
            get => pathsToLookup;
            set
            {
                pathsToLookup = value;
                ConsoleFilesystem.LookupPaths = value;
            }
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
        /// Shows the shell count in the normal UESH shell (depending on the preset)
        /// </summary>
        public bool ShowShellCount { get; set; }
        #endregion

        #region Shell Presets
        /// <summary>
        /// Prompt Preset
        /// </summary>
        public string PromptPreset
        {
            get => PromptPresetManager.GetCurrentPresetBaseFromShell("Shell").PresetName;
            set => PromptPresetManager.SetPreset(value, "Shell", false);
        }
        /// <summary>
        /// Text Edit Prompt Preset
        /// </summary>
        public string TextEditPromptPreset
        {
            get => PromptPresetManager.GetCurrentPresetBaseFromShell("TextShell").PresetName;
            set => PromptPresetManager.SetPreset(value, "TextShell", false);
        }
        /// <summary>
        /// Hex Edit Prompt Preset
        /// </summary>
        public string HexEditPromptPreset
        {
            get => PromptPresetManager.GetCurrentPresetBaseFromShell("HexShell").PresetName;
            set => PromptPresetManager.SetPreset(value, "HexShell", false);
        }
        /// <summary>
        /// Admin Shell Prompt Preset
        /// </summary>
        public string AdminShellPromptPreset
        {
            get => PromptPresetManager.GetCurrentPresetBaseFromShell("AdminShell").PresetName;
            set => PromptPresetManager.SetPreset(value, "AdminShell", false);
        }
        /// <summary>
        /// Debug Shell Prompt Preset
        /// </summary>
        public string DebugShellPromptPreset
        {
            get => PromptPresetManager.GetCurrentPresetBaseFromShell("DebugShell").PresetName;
            set => PromptPresetManager.SetPreset(value, "DebugShell", false);
        }
        #endregion

        #region Filesystem
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
        /// If enabled, sorts the list alphanumerically. Otherwise, sorts them alphabetically.
        /// </summary>
        public bool SortLogically { get; set; } = true;
        #endregion

        #region Network
        /// <summary>
        /// Write a remote debugger port. It must be numeric, and must not be already used. Otherwise, remote debugger will fail to open the port
        /// </summary>
        public int DebugPort
        {
            get => RemoteDebugger.debugPort;
            set => RemoteDebugger.debugPort = value < 0 ? 3014 : value;
        }
        /// <summary>
        /// Write a remote debugger chat port. It must be numeric, and must not be already used. Otherwise, remote debugger chat will fail to open the port
        /// </summary>
        public int DebugChatPort
        {
            get => RemoteChatTools.debugChatPort;
            set => RemoteChatTools.debugChatPort = value < 0 ? 3015 : value;
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
        /// If you want remote debug to start on boot, enable this
        /// </summary>
        public bool RDebugAutoStart { get; set; } = true;
        /// <summary>
        /// Specifies the remote debug message format. {0} for name, {1} for message
        /// </summary>
        public string RDebugMessageFormat { get; set; } = "";
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
        /// Shows the notification showing the download progress
        /// </summary>
        public bool DownloadNotificationProvoke { get; set; }
        /// <summary>
        /// Shows the notification showing the upload progress
        /// </summary>
        public bool UploadNotificationProvoke { get; set; }
        /// <summary>
        /// If enabled, will use the notification system to notify the host of remote debug connection error. Otherwise, will use the default console writing.
        /// </summary>
        public bool NotifyOnRemoteDebugConnectionError { get; set; } = true;
        #endregion

        #region Screensaver
        private int screensaverDelay = 10;

        /// <summary>
        /// Which screensaver do you want to lock your screen with?
        /// </summary>
        public string DefaultSaverName { get; set; } = "matrixbleed";
        /// <summary>
        /// Whether the screen idling is enabled or not
        /// </summary>
        public bool ScreenTimeoutEnabled
        {
            get => ScreensaverManager.scrnTimeoutEnabled;
            set
            {
                ScreensaverManager.scrnTimeoutEnabled = value;
                if (!value)
                    ScreensaverManager.StopTimeout();
                else
                    ScreensaverManager.StartTimeout();
            }
        }
        /// <summary>
        /// Minimum idling interval to launch screensaver
        /// </summary>
        public string ScreenTimeout
        {
            get => ScreensaverManager.scrnTimeout.ToString();
            set
            {
                // First, deal with merging milliseconds from old configs
                TimeSpan fallback = new(0, 5, 0);
                TimeSpan span;
                bool isOldFormat = int.TryParse(value, out int milliseconds);
                if (isOldFormat)
                {
                    span = TimeSpan.FromMilliseconds(milliseconds);
                    ScreensaverManager.scrnTimeout = span.TotalMinutes < 1.0d ? fallback : span;
                    return;
                }

                // Then, parse the timespan
                bool spanParsed = TimeSpan.TryParse(value, out span);
                if (!spanParsed)
                    ScreensaverManager.scrnTimeout = fallback;
                else
                    ScreensaverManager.scrnTimeout = span.TotalMinutes < 1.0d ? fallback : span;
            }
        }
        /// <summary>
        /// Enables debugging for screensavers. Please note that it may quickly fill the debug log and slightly slow the screensaver down, depending on the screensaver used. Only works if kernel debugging is enabled for diagnostic purposes.
        /// </summary>
        public bool ScreensaverDebug { get; set; }
        /// <summary>
        /// If you've acknowledged the photosensitive seizure warning, you can turn off the warning message that appears each time a fast-paced screensaver is run.
        /// </summary>
        public bool ScreensaverSeizureAcknowledged
        {
            get => ScreensaverManager.seizureAcknowledged;
            set => ScreensaverManager.seizureAcknowledged = value;
        }
        /// <summary>
        /// After locking the screen, ask for password
        /// </summary>
        public bool PasswordLock { get; set; } = true;
        /// <summary>
        /// If true, enables unified writing delay for all screensavers. Otherwise, it uses screensaver-specific configured delay values.
        /// </summary>
        public bool ScreensaverUnifiedDelay { get; set; } = true;
        /// <summary>
        /// How many milliseconds to wait before making the next write?
        /// </summary>
        public int ScreensaverDelay
        {
            get => screensaverDelay;
            set
            {
                if (value <= 0)
                    value = 10;
                screensaverDelay = value;
            }
        }
        #endregion

        #region Audio
        private string audioCueThemeName = "the_mirage";
        private bool enableAudio = true;

        /// <summary>
        /// Enables the whole audio system for system cues
        /// </summary>
        public bool EnableAudio
        {
            get => enableAudio;
            set
            {
                enableAudio = value;
                if (!value)
                {
                    EnableKeyboardCues = value;
                    EnableStartupSounds = value;
                    EnableShutdownSounds = value;
                    EnableNavigationSounds = value;
                    EnableLowPriorityNotificationSounds = value;
                    EnableMediumPriorityNotificationSounds = value;
                    EnableHighPriorityNotificationSounds = value;
                    EnableAmbientSoundFx = value;
                    EnableAmbientSoundFxIntense = value;
                }
            }
        }
        /// <summary>
        /// Whether to play keyboard cues for each keypress or not (<see cref="EnableNavigationSounds"/> must be enabled for this to take effect)
        /// </summary>
        public bool EnableKeyboardCues
        {
            get => TermReader.GlobalReaderSettings.KeyboardCues;
            set => TermReader.GlobalReaderSettings.KeyboardCues = value;
        }
        /// <summary>
        /// Whether to enable startup sounds or not
        /// </summary>
        public bool EnableStartupSounds { get; set; } = true;
        /// <summary>
        /// Whether to enable shutdown sounds or not
        /// </summary>
        public bool EnableShutdownSounds { get; set; } = true;
        /// <summary>
        /// Whether to enable navigation sounds or not
        /// </summary>
        public bool EnableNavigationSounds
        {
            get => Input.KeyboardCues;
            set => Input.KeyboardCues = value;
        }
        /// <summary>
        /// Whether to enable the notification sound for low-priority alerts or not
        /// </summary>
        public bool EnableLowPriorityNotificationSounds { get; set; } = true;
        /// <summary>
        /// Whether to enable the notification sound for medium-priority alerts or not
        /// </summary>
        public bool EnableMediumPriorityNotificationSounds { get; set; } = true;
        /// <summary>
        /// Whether to enable the notification sound for high-priority alerts or not
        /// </summary>
        public bool EnableHighPriorityNotificationSounds { get; set; } = true;
        /// <summary>
        /// Whether to play ambient screensaver sound effects or not
        /// </summary>
        public bool EnableAmbientSoundFx { get; set; }
        /// <summary>
        /// Whether to intensify the ambient screensaver sound effects or not
        /// </summary>
        public bool EnableAmbientSoundFxIntense { get; set; }
        /// <summary>
        /// Audio cue volume
        /// </summary>
        public double AudioCueVolume
        {
            get => Input.CueVolume;
            set => Input.CueVolume = value;
        }
        /// <summary>
        /// Audio cue volume for the reader
        /// </summary>
        // TODO: NKS_SETTINGS_KERNEL_AUDIO_CUEVOLUMEREADER_NAME -> "Audio cue volume for the reader"
        // TODO: NKS_SETTINGS_KERNEL_AUDIO_CUEVOLUMEREADER_DESC -> "Specify how loud do you want the reader audio cues to be"
        public double AudioCueVolumeReader
        {
            get => TermReader.GlobalReaderSettings.CueVolume;
            set => TermReader.GlobalReaderSettings.CueVolume = value;
        }
        /// <summary>
        /// Audio cue theme name
        /// </summary>
        public string AudioCueThemeName
        {
            get => audioCueThemeName;
            set
            {
                audioCueThemeName = AudioCuesTools.GetAudioThemeNames().Contains(value) ? value : "the_mirage";
                var cue = AudioCuesTools.GetAudioCue();
                TermReader.GlobalReaderSettings.CueWrite = cue.KeyboardCueTypeStream ?? TermReader.GlobalReaderSettings.CueWrite;
                TermReader.GlobalReaderSettings.CueRubout = cue.KeyboardCueBackspaceStream ?? TermReader.GlobalReaderSettings.CueRubout;
                TermReader.GlobalReaderSettings.CueEnter = cue.KeyboardCueEnterStream ?? TermReader.GlobalReaderSettings.CueEnter;
                Input.CueWrite = cue.KeyboardCueTypeStream ?? Input.CueWrite;
                Input.CueRubout = cue.KeyboardCueBackspaceStream ?? Input.CueRubout;
                Input.CueEnter = cue.KeyboardCueEnterStream ?? Input.CueEnter;
            }
        }
        #endregion

        #region Misc
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
        /// Covers the notification with the border
        /// </summary>
        public bool DrawBorderNotification { get; set; } = true;
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
        /// Shows the commands count in the command list, controlled by the three count show switches for different kinds of commands.
        /// </summary>
        public bool ShowCommandsCount { get; set; }
        /// <summary>
        /// Show the shell commands count on help
        /// </summary>
        public bool ShowShellCommandsCount { get; set; } = true;
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
            get => Input.PasswordMaskChar.ToString();
            set => Input.PasswordMaskChar = string.IsNullOrEmpty(value) ? '*' : value[0];
        }
        /// <summary>
        /// A character that masks the password. Leave blank for more security
        /// </summary>
        // TODO: NKS_SETTINGS_KERNEL_MISC_AESTHETICS_MASKCHARREADER_NAME -> "Password mask character (reader)"
        // TODO: NKS_SETTINGS_KERNEL_MISC_AESTHETICS_MASKCHARREADER_DESC -> "A character that masks the input password. Leave blank for more security."
        public string CurrentMaskReader
        {
            get => TermReader.GlobalReaderSettings.PasswordMaskChar.ToString();
            set => TermReader.GlobalReaderSettings.PasswordMaskChar = string.IsNullOrEmpty(value) ? '*' : value[0];
        }
        /// <summary>
        /// Whether the input history is enabled or not. If enabled, you can access recently typed commands using the up or down arrow keys.
        /// </summary>
        public bool InputHistoryEnabled
        {
            get => TermReader.GlobalReaderSettings.HistoryEnabled;
            set => TermReader.GlobalReaderSettings.HistoryEnabled = value;
        }
        /// <summary>
        /// Enables the scroll bar in selection screens
        /// </summary>
        public bool EnableScrollBarInSelection { get; set; } = true;
        /// <summary>
        /// If Do Not Disturb is enabled, all notifications received will be suppressed from the display. This means that you won't be able to see any notification to help you focus.
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
            get => BorderSettings.GlobalSettings.BorderUpperLeftCornerChar;
            set => BorderSettings.GlobalSettings.BorderUpperLeftCornerChar = value;
        }
        /// <summary>
        /// A character that resembles the upper right corner. Be sure to only input one character.
        /// </summary>
        public char BorderUpperRightCornerChar
        {
            get => BorderSettings.GlobalSettings.BorderUpperRightCornerChar;
            set => BorderSettings.GlobalSettings.BorderUpperRightCornerChar = value;
        }
        /// <summary>
        /// A character that resembles the lower left corner. Be sure to only input one character.
        /// </summary>
        public char BorderLowerLeftCornerChar
        {
            get => BorderSettings.GlobalSettings.BorderLowerLeftCornerChar;
            set => BorderSettings.GlobalSettings.BorderLowerLeftCornerChar = value;
        }
        /// <summary>
        /// A character that resembles the lower right corner. Be sure to only input one character.
        /// </summary>
        public char BorderLowerRightCornerChar
        {
            get => BorderSettings.GlobalSettings.BorderLowerRightCornerChar;
            set => BorderSettings.GlobalSettings.BorderLowerRightCornerChar = value;
        }
        /// <summary>
        /// A character that resembles the upper frame. Be sure to only input one character.
        /// </summary>
        public char BorderUpperFrameChar
        {
            get => BorderSettings.GlobalSettings.BorderUpperFrameChar;
            set => BorderSettings.GlobalSettings.BorderUpperFrameChar = value;
        }
        /// <summary>
        /// A character that resembles the lower frame. Be sure to only input one character.
        /// </summary>
        public char BorderLowerFrameChar
        {
            get => BorderSettings.GlobalSettings.BorderLowerFrameChar;
            set => BorderSettings.GlobalSettings.BorderLowerFrameChar = value;
        }
        /// <summary>
        /// A character that resembles the left frame. Be sure to only input one character.
        /// </summary>
        public char BorderLeftFrameChar
        {
            get => BorderSettings.GlobalSettings.BorderLeftFrameChar;
            set => BorderSettings.GlobalSettings.BorderLeftFrameChar = value;
        }
        /// <summary>
        /// A character that resembles the right frame. Be sure to only input one character.
        /// </summary>
        public char BorderRightFrameChar
        {
            get => BorderSettings.GlobalSettings.BorderRightFrameChar;
            set => BorderSettings.GlobalSettings.BorderRightFrameChar = value;
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
        /// Specifies the default figlet font name
        /// </summary>
        public string DefaultFigletFontName
        {
            get => defaultFigletFontName;
            set => defaultFigletFontName = FigletTools.GetFigletFonts().ContainsKey(value) ? value : "speed";
        }
        /// <summary>
        /// Whether to update the CPU usage or not
        /// </summary>
        public bool CpuUsageDebugEnabled
        {
            get => CpuUsageDebug.usageUpdateEnabled;
            set
            {
                CpuUsageDebug.usageUpdateEnabled = value;
                CpuUsageDebug.RunCpuUsageDebugger();
            }
        }
        /// <summary>
        /// The interval in which the CPU usage is printed
        /// </summary>
        public int CpuUsageUpdateInterval
        {
            get => CpuUsageDebug.usageIntervalUpdatePeriod;
            set => CpuUsageDebug.usageIntervalUpdatePeriod = value >= 1000 ? value : 1000;
        }
        /// <summary>
        /// Whether to initialize the mouse support for the kernel or not, essentially enabling all mods to handle the mouse pointer
        /// </summary>
        public bool InitializeCursorHandler
        {
            get => ConsolePointerHandler.enableHandler;
            set
            {
                if (value)
                {
                    ConsolePointerHandler.enableHandler = true;
                    ConsolePointerHandler.StartHandler();
                }
                else
                {
                    ConsolePointerHandler.StopHandler();
                    ConsolePointerHandler.enableHandler = false;
                }
            }
        }
        /// <summary>
        /// Whether to also enable the movement events or not, improving the user experience of some interactive applications
        /// </summary>
        public bool HandleCursorMovement
        {
            get => Input.EnableMovementEvents;
            set => Input.EnableMovementEvents = value;
        }
        #endregion
    }
}
