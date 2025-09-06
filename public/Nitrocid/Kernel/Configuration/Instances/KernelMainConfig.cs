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
using Nitrocid.Kernel.Configuration.Settings;
using Nitrocid.Shell.Shells.Text;
using Terminaux.Shell.Shells;
using Nitrocid.Files;
using Nitrocid.ConsoleBase.Inputs;
using Nitrocid.Shell.Shells.Hex;
using Nitrocid.Misc.Screensaver;
using Nitrocid.Kernel.Debugging.RemoteDebug;
using Nitrocid.Kernel.Threading.Performance;
using Nitrocid.Files.Folders;
using Nitrocid.Languages;
using Nitrocid.Misc.Notifications;
using Nitrocid.Kernel.Exceptions;
using Terminaux.Inputs.Styles.Choice;
using Terminaux.Shell.Prompts;
using Nitrocid.Users.Login.Handlers;
using Nitrocid.Files.Paths;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Kernel.Debugging.RemoteDebug.RemoteChat;
using Nitrocid.Kernel.Time.Timezones;
using Nitrocid.Network.Types.RPC;
using Nitrocid.Network;
using Terminaux.Inputs.Styles.Selection;
using Nitrocid.Misc.Reflection.Internal;
using Nitrocid.Users.Login;
using Nitrocid.ConsoleBase;
using Nitrocid.Shell.Homepage;
using Terminaux.Inputs;
using Nitrocid.Users.Login.Widgets;
using Nitrocid.Users.Login.Widgets.Implementations;
using Nitrocid.Kernel.Starting;
using Terminaux.Inputs.Interactive;
using Terminaux.Reader;
using Nitrocid.Misc.Audio;
using System.Linq;

namespace Nitrocid.Kernel.Configuration.Instances
{
    /// <summary>
    /// Main kernel configuration instance
    /// </summary>
    public class KernelMainConfig : BaseKernelConfig, IKernelConfig
    {
        /// <inheritdoc/>
        [JsonIgnore]
        public override SettingsEntry[] SettingsEntries
        {
            get
            {
                var dataStream = ResourcesManager.GetData("SettingsEntries.json", ResourcesType.Settings) ??
                    throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Failed to obtain main settings entries."));
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
        public bool UseAccentColors { get; set; }
        /// <summary>
        /// Accent color (foreground)
        /// </summary>
        public string AccentForegroundColor
        {
            get => KernelColorTools.accentForegroundColor.PlainSequence;
            set => KernelColorTools.accentForegroundColor = new Color(value);
        }
        /// <summary>
        /// Accent color (background)
        /// </summary>
        public string AccentBackgroundColor
        {
            get => KernelColorTools.accentBackgroundColor.PlainSequence;
            set => KernelColorTools.accentBackgroundColor = new Color(value);
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
            set
            {
                var color = new Color(value);
                KernelColorTools.SetColor(KernelColorType.NeutralText, color);
                SelectionStyleSettings.GlobalSettings.TextColor = color;
            }
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
            set
            {
                var color = new Color(value);
                KernelColorTools.SetColor(KernelColorType.Background, color);
                SelectionStyleSettings.GlobalSettings.BackgroundColor = color;
            }
        }
        /// <summary>
        /// Input Color
        /// </summary>
        public string InputColor
        {
            get => KernelColorTools.GetColor(KernelColorType.Input).PlainSequence;
            set
            {
                var color = new Color(value);
                KernelColorTools.SetColor(KernelColorType.Input, color);
                SelectionStyleSettings.GlobalSettings.InputColor = color;
            }
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
            set
            {
                var color = new Color(value);
                KernelColorTools.SetColor(KernelColorType.Option, color);
                SelectionStyleSettings.GlobalSettings.OptionColor = color;
            }
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
            set
            {
                var color = new Color(value);
                KernelColorTools.SetColor(KernelColorType.Question, color);
                SelectionStyleSettings.GlobalSettings.QuestionColor = color;
            }
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
            set
            {
                var color = new Color(value);
                KernelColorTools.SetColor(KernelColorType.Separator, color);
                SelectionStyleSettings.GlobalSettings.SeparatorColor = color;
            }
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
            set
            {
                var color = new Color(value);
                KernelColorTools.SetColor(KernelColorType.SelectedOption, color);
                SelectionStyleSettings.GlobalSettings.SelectedOptionColor = color;
            }
        }
        /// <summary>
        /// Alternative Option Color
        /// </summary>
        public string AlternativeOptionColor
        {
            get => KernelColorTools.GetColor(KernelColorType.AlternativeOption).PlainSequence;
            set
            {
                var color = new Color(value);
                KernelColorTools.SetColor(KernelColorType.AlternativeOption, color);
                SelectionStyleSettings.GlobalSettings.AltOptionColor = color;
            }
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
            set
            {
                var color = new Color(value);
                KernelColorTools.SetColor(KernelColorType.TuiBackground, color);
                InteractiveTuiSettings.GlobalSettings.BackgroundColor = color;
            }
        }
        /// <summary>
        /// Interactive TUI foreground color
        /// </summary>
        public string TuiForegroundColor
        {
            get => KernelColorTools.GetColor(KernelColorType.TuiForeground).PlainSequence;
            set
            {
                var color = new Color(value);
                KernelColorTools.SetColor(KernelColorType.TuiForeground, color);
                InteractiveTuiSettings.GlobalSettings.ForegroundColor = color;
            }
        }
        /// <summary>
        /// Interactive TUI pane background color
        /// </summary>
        public string TuiPaneBackgroundColor
        {
            get => KernelColorTools.GetColor(KernelColorType.TuiPaneBackground).PlainSequence;
            set
            {
                var color = new Color(value);
                KernelColorTools.SetColor(KernelColorType.TuiPaneBackground, color);
                InteractiveTuiSettings.GlobalSettings.PaneBackgroundColor = color;
            }
        }
        /// <summary>
        /// Interactive TUI pane separator color
        /// </summary>
        public string TuiPaneSeparatorColor
        {
            get => KernelColorTools.GetColor(KernelColorType.TuiPaneSeparator).PlainSequence;
            set
            {
                var color = new Color(value);
                KernelColorTools.SetColor(KernelColorType.TuiPaneSeparator, color);
                InteractiveTuiSettings.GlobalSettings.PaneSeparatorColor = color;
            }
        }
        /// <summary>
        /// Interactive TUI selected pane separator color
        /// </summary>
        public string TuiPaneSelectedSeparatorColor
        {
            get => KernelColorTools.GetColor(KernelColorType.TuiPaneSelectedSeparator).PlainSequence;
            set
            {
                var color = new Color(value);
                KernelColorTools.SetColor(KernelColorType.TuiPaneSelectedSeparator, color);
                InteractiveTuiSettings.GlobalSettings.PaneSelectedSeparatorColor = color;
            }
        }
        /// <summary>
        /// Interactive TUI selected pane item foreground color
        /// </summary>
        public string TuiPaneSelectedItemForeColor
        {
            get => KernelColorTools.GetColor(KernelColorType.TuiPaneSelectedItemFore).PlainSequence;
            set
            {
                var color = new Color(value);
                KernelColorTools.SetColor(KernelColorType.TuiPaneSelectedItemFore, color);
                InteractiveTuiSettings.GlobalSettings.PaneSelectedItemForeColor = color;
            }
        }
        /// <summary>
        /// Interactive TUI selected pane item background color
        /// </summary>
        public string TuiPaneSelectedItemBackColor
        {
            get => KernelColorTools.GetColor(KernelColorType.TuiPaneSelectedItemBack).PlainSequence;
            set
            {
                var color = new Color(value);
                KernelColorTools.SetColor(KernelColorType.TuiPaneSelectedItemBack, color);
                InteractiveTuiSettings.GlobalSettings.PaneSelectedItemBackColor = color;
            }
        }
        /// <summary>
        /// Interactive TUI pane item foreground color
        /// </summary>
        public string TuiPaneItemForeColor
        {
            get => KernelColorTools.GetColor(KernelColorType.TuiPaneItemFore).PlainSequence;
            set
            {
                var color = new Color(value);
                KernelColorTools.SetColor(KernelColorType.TuiPaneItemFore, color);
                InteractiveTuiSettings.GlobalSettings.PaneItemForeColor = color;
            }
        }
        /// <summary>
        /// Interactive TUI pane item background color
        /// </summary>
        public string TuiPaneItemBackColor
        {
            get => KernelColorTools.GetColor(KernelColorType.TuiPaneItemBack).PlainSequence;
            set
            {
                var color = new Color(value);
                KernelColorTools.SetColor(KernelColorType.TuiPaneItemBack, color);
                InteractiveTuiSettings.GlobalSettings.PaneItemBackColor = color;
            }
        }
        /// <summary>
        /// Interactive TUI option background color
        /// </summary>
        public string TuiOptionBackgroundColor
        {
            get => KernelColorTools.GetColor(KernelColorType.TuiOptionBackground).PlainSequence;
            set
            {
                var color = new Color(value);
                KernelColorTools.SetColor(KernelColorType.TuiOptionBackground, color);
                InteractiveTuiSettings.GlobalSettings.OptionBackgroundColor = color;
            }
        }
        /// <summary>
        /// Interactive TUI option foreground color
        /// </summary>
        public string TuiOptionForegroundColor
        {
            get => KernelColorTools.GetColor(KernelColorType.TuiOptionForeground).PlainSequence;
            set
            {
                var color = new Color(value);
                KernelColorTools.SetColor(KernelColorType.TuiOptionForeground, color);
                InteractiveTuiSettings.GlobalSettings.OptionForegroundColor = color;
            }
        }
        /// <summary>
        /// Interactive TUI option binding name color
        /// </summary>
        public string TuiKeyBindingOptionColor
        {
            get => KernelColorTools.GetColor(KernelColorType.TuiKeyBindingOption).PlainSequence;
            set
            {
                var color = new Color(value);
                KernelColorTools.SetColor(KernelColorType.TuiKeyBindingOption, color);
                InteractiveTuiSettings.GlobalSettings.KeyBindingOptionColor = color;
            }
        }
        /// <summary>
        /// Interactive TUI box background color
        /// </summary>
        public string TuiBoxBackgroundColor
        {
            get => KernelColorTools.GetColor(KernelColorType.TuiBoxBackground).PlainSequence;
            set
            {
                var color = new Color(value);
                KernelColorTools.SetColor(KernelColorType.TuiBoxBackground, color);
                InteractiveTuiSettings.GlobalSettings.BoxBackgroundColor = color;
            }
        }
        /// <summary>
        /// Interactive TUI box foreground color
        /// </summary>
        public string TuiBoxForegroundColor
        {
            get => KernelColorTools.GetColor(KernelColorType.TuiBoxForeground).PlainSequence;
            set
            {
                var color = new Color(value);
                KernelColorTools.SetColor(KernelColorType.TuiBoxForeground, color);
                InteractiveTuiSettings.GlobalSettings.BoxForegroundColor = color;
            }
        }
        /// <summary>
        /// Disabled option color
        /// </summary>
        public string DisabledOptionColor
        {
            get => KernelColorTools.GetColor(KernelColorType.DisabledOption).PlainSequence;
            set
            {
                var color = new Color(value);
                KernelColorTools.SetColor(KernelColorType.DisabledOption, color);
                SelectionStyleSettings.GlobalSettings.DisabledOptionColor = color;
            }
        }
        /// <summary>
        /// Interactive TUI builtin key binding background color
        /// </summary>
        public string TuiKeyBindingBuiltinBackgroundColor
        {
            get => KernelColorTools.GetColor(KernelColorType.TuiKeyBindingBuiltinBackground).PlainSequence;
            set
            {
                var color = new Color(value);
                KernelColorTools.SetColor(KernelColorType.TuiKeyBindingBuiltinBackground, color);
                InteractiveTuiSettings.GlobalSettings.KeyBindingBuiltinBackgroundColor = color;
            }
        }
        /// <summary>
        /// Interactive TUI builtin key binding foreground color
        /// </summary>
        public string TuiKeyBindingBuiltinForegroundColor
        {
            get => KernelColorTools.GetColor(KernelColorType.TuiKeyBindingBuiltinForeground).PlainSequence;
            set
            {
                var color = new Color(value);
                KernelColorTools.SetColor(KernelColorType.TuiKeyBindingBuiltinForeground, color);
                InteractiveTuiSettings.GlobalSettings.KeyBindingBuiltinForegroundColor = color;
            }
        }
        /// <summary>
        /// Interactive TUI builtin key binding color
        /// </summary>
        public string TuiKeyBindingBuiltinColor
        {
            get => KernelColorTools.GetColor(KernelColorType.TuiKeyBindingBuiltin).PlainSequence;
            set
            {
                var color = new Color(value);
                KernelColorTools.SetColor(KernelColorType.TuiKeyBindingBuiltin, color);
                InteractiveTuiSettings.GlobalSettings.KeyBindingBuiltinColor = color;
            }
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
                    FilesystemTools._CurrentDirectory = value;
                else
                    throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("Directory {0} not found"), value);
            }
        }
        /// <summary>
        /// Group of paths separated by the colon. It works the same as PATH. Write a full path to a folder or a folder name. When you're finished, write \"q\". Write a minus sign next to the path to remove an existing directory.
        /// </summary>
        public string PathsToLookup { get; set; } = Environment.GetEnvironmentVariable("PATH") ?? "";
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
        private double audioCueVolume = 1.0;
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
        /// Whether to play keyboard cues for each keypress or not
        /// </summary>
        public bool EnableKeyboardCues
        {
            get => InputTools.globalSettings.KeyboardCues;
            set
            {
                TermReader.GlobalReaderSettings.KeyboardCues = value;
                InputTools.globalSettings.KeyboardCues = value;
            }
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
        public bool EnableNavigationSounds { get; set; }
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
            get => audioCueVolume;
            set
            {
                audioCueVolume = value;
                TermReader.GlobalReaderSettings.CueVolume = value;
                InputTools.globalSettings.CueVolume = value;
            }
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
                InputTools.globalSettings.CueWrite = cue.KeyboardCueTypeStream ?? InputTools.globalSettings.CueWrite;
                InputTools.globalSettings.CueRubout = cue.KeyboardCueBackspaceStream ?? InputTools.globalSettings.CueRubout;
                InputTools.globalSettings.CueEnter = cue.KeyboardCueEnterStream ?? InputTools.globalSettings.CueEnter;
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
            get => InputTools.currentMask;
            set => InputTools.currentMask = string.IsNullOrEmpty(value) ? "*" : value[0].ToString();
        }
        /// <summary>
        /// Whether the input history is enabled or not. If enabled, you can access recently typed commands using the up or down arrow keys.
        /// </summary>
        public bool InputHistoryEnabled
        {
            get => InputTools.globalSettings.HistoryEnabled;
            set => InputTools.globalSettings.HistoryEnabled = value;
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
