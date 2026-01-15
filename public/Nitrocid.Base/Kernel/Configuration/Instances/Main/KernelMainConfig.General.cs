//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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

using Terminaux.Base.Extensions;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Kernel.Starting;
using Nitrocid.Base.Misc.Widgets;
using Nitrocid.Base.Misc.Widgets.Implementations;
using Nitrocid.Base.Kernel.Time.Timezones;
using Nitrocid.Base.Shell.Homepage;

namespace Nitrocid.Base.Kernel.Configuration.Instances
{
    /// <summary>
    /// Main kernel configuration instance
    /// </summary>
    public partial class KernelMainConfig : BaseKernelConfig
    {
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
            get => LanguageManager.currentLanguage is not null ? LanguageManager.currentLanguage.Name : "en-US";
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
            get => ConsoleColoring.ConsoleSupportsTrueColor;
            set => ConsoleColoring.ConsoleSupportsTrueColor = value;
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
    }
}
