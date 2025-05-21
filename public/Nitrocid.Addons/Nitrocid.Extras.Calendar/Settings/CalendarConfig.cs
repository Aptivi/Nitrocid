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
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Configuration.Instances;
using Nitrocid.Kernel.Configuration.Settings;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Misc.Reflection.Internal;

namespace Nitrocid.Extras.Calendar.Settings
{
    /// <summary>
    /// Configuration instance for calendar
    /// </summary>
    public class CalendarConfig : BaseKernelConfig, IKernelConfig
    {
        /// <inheritdoc/>
        [JsonIgnore]
        public override SettingsEntry[] SettingsEntries
        {
            get
            {
                var dataStream = ResourcesManager.GetData("CalendarSettings.json", ResourcesType.Misc, typeof(CalendarConfig).Assembly) ??
                    throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Failed to obtain settings entries."));
                string dataString = ResourcesManager.ConvertToString(dataStream);
                return ConfigTools.GetSettingsEntries(dataString);
            }
        }

        /// <summary>
        /// If enabled, deletes all events and/or reminders before saving all of them using the calendar command
        /// </summary>
        public bool SaveEventsRemindersDestructively { get; set; }

        #region Screensaver
        private bool calendarTrueColor = true;
        private int calendarDelay = 3000;
        private bool calendarUseSystemCulture = true;
        private string calendarCultureName = "en-US";
        private int calendarMinimumRedColorLevel = 0;
        private int calendarMinimumGreenColorLevel = 0;
        private int calendarMinimumBlueColorLevel = 0;
        private int calendarMinimumColorLevel = 0;
        private int calendarMaximumRedColorLevel = 255;
        private int calendarMaximumGreenColorLevel = 255;
        private int calendarMaximumBlueColorLevel = 255;
        private int calendarMaximumColorLevel = 255;

        /// <summary>
        /// [Calendar] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool CalendarTrueColor
        {
            get
            {
                return calendarTrueColor;
            }
            set
            {
                calendarTrueColor = value;
            }
        }
        /// <summary>
        /// [Calendar] How many milliseconds to wait before making the next write?
        /// </summary>
        public int CalendarDelay
        {
            get
            {
                return calendarDelay;
            }
            set
            {
                if (value <= 0)
                    value = 3000;
                calendarDelay = value;
            }
        }
        /// <summary>
        /// [Calendar] Whether to use the system culture assigned by <see cref="KernelMainConfig.CurrentCultureName"/> or by <see cref="CalendarCultureName"/>.
        /// </summary>
        public bool CalendarUseSystemCulture
        {
            get
            {
                return calendarUseSystemCulture;
            }
            set
            {
                calendarUseSystemCulture = value;
            }
        }
        /// <summary>
        /// [Calendar] Which culture is being used to change the month names, calendar, etc.?
        /// </summary>
        public string CalendarCultureName
        {
            get
            {
                return calendarCultureName;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "en-US";
                calendarCultureName = value;
            }
        }
        /// <summary>
        /// [Calendar] The minimum red color level (true color)
        /// </summary>
        public int CalendarMinimumRedColorLevel
        {
            get
            {
                return calendarMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                calendarMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Calendar] The minimum green color level (true color)
        /// </summary>
        public int CalendarMinimumGreenColorLevel
        {
            get
            {
                return calendarMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                calendarMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Calendar] The minimum blue color level (true color)
        /// </summary>
        public int CalendarMinimumBlueColorLevel
        {
            get
            {
                return calendarMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                calendarMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Calendar] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int CalendarMinimumColorLevel
        {
            get
            {
                return calendarMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                calendarMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Calendar] The maximum red color level (true color)
        /// </summary>
        public int CalendarMaximumRedColorLevel
        {
            get
            {
                return calendarMaximumRedColorLevel;
            }
            set
            {
                if (value <= calendarMinimumRedColorLevel)
                    value = calendarMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                calendarMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Calendar] The maximum green color level (true color)
        /// </summary>
        public int CalendarMaximumGreenColorLevel
        {
            get
            {
                return calendarMaximumGreenColorLevel;
            }
            set
            {
                if (value <= calendarMinimumGreenColorLevel)
                    value = calendarMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                calendarMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Calendar] The maximum blue color level (true color)
        /// </summary>
        public int CalendarMaximumBlueColorLevel
        {
            get
            {
                return calendarMaximumBlueColorLevel;
            }
            set
            {
                if (value <= calendarMinimumBlueColorLevel)
                    value = calendarMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                calendarMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Calendar] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int CalendarMaximumColorLevel
        {
            get
            {
                return calendarMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= calendarMinimumColorLevel)
                    value = calendarMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                calendarMaximumColorLevel = value;
            }
        }
        #endregion
    }
}
