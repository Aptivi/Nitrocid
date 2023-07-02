
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

using System;
using ColorSeq;
using Extensification.StringExts;
using KS.ConsoleBase;
using KS.Drivers.RNG;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Misc.Probers.Placeholder;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using KS.TimeDate;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for ProgressClock
    /// </summary>
    public static class ProgressClockSettings
    {

        /// <summary>
        /// [ProgressClock] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool ProgressClockTrueColor
        {
            get
            {
                return Config.SaverConfig.ProgressClockTrueColor;
            }
            set
            {
                Config.SaverConfig.ProgressClockTrueColor = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Enable color cycling (uses RNG. If disabled, uses the <see cref="ProgressClockSecondsProgressColor"/>, <see cref="ProgressClockMinutesProgressColor"/>, and <see cref="ProgressClockHoursProgressColor"/> colors.)
        /// </summary>
        public static bool ProgressClockCycleColors
        {
            get
            {
                return Config.SaverConfig.ProgressClockCycleColors;
            }
            set
            {
                Config.SaverConfig.ProgressClockCycleColors = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The color of seconds progress bar. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public static string ProgressClockSecondsProgressColor
        {
            get
            {
                return Config.SaverConfig.ProgressClockSecondsProgressColor;
            }
            set
            {
                Config.SaverConfig.ProgressClockSecondsProgressColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [ProgressClock] The color of minutes progress bar. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public static string ProgressClockMinutesProgressColor
        {
            get
            {
                return Config.SaverConfig.ProgressClockMinutesProgressColor;
            }
            set
            {
                Config.SaverConfig.ProgressClockMinutesProgressColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [ProgressClock] The color of hours progress bar. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public static string ProgressClockHoursProgressColor
        {
            get
            {
                return Config.SaverConfig.ProgressClockHoursProgressColor;
            }
            set
            {
                Config.SaverConfig.ProgressClockHoursProgressColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [ProgressClock] The color of date information. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public static string ProgressClockProgressColor
        {
            get
            {
                return Config.SaverConfig.ProgressClockProgressColor;
            }
            set
            {
                Config.SaverConfig.ProgressClockProgressColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [ProgressClock] If color cycling is enabled, how many ticks before changing colors? 1 tick = 0.5 seconds
        /// </summary>
        public static long ProgressClockCycleColorsTicks
        {
            get
            {
                return Config.SaverConfig.ProgressClockCycleColorsTicks;
            }
            set
            {
                if (value <= 0L)
                    value = 20L;
                Config.SaverConfig.ProgressClockCycleColorsTicks = (int)value;
            }
        }
        /// <summary>
        /// [ProgressClock] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int ProgressClockDelay
        {
            get
            {
                return Config.SaverConfig.ProgressClockDelay;
            }
            set
            {
                if (value <= 0)
                    value = 500;
                Config.SaverConfig.ProgressClockDelay = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Upper left corner character for hours bar
        /// </summary>
        public static string ProgressClockUpperLeftCornerCharHours
        {
            get
            {
                return Config.SaverConfig.ProgressClockUpperLeftCornerCharHours;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╔";
                Config.SaverConfig.ProgressClockUpperLeftCornerCharHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Upper left corner character for minutes bar
        /// </summary>
        public static string ProgressClockUpperLeftCornerCharMinutes
        {
            get
            {
                return Config.SaverConfig.ProgressClockUpperLeftCornerCharMinutes;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╔";
                Config.SaverConfig.ProgressClockUpperLeftCornerCharMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Upper left corner character for seconds bar
        /// </summary>
        public static string ProgressClockUpperLeftCornerCharSeconds
        {
            get
            {
                return Config.SaverConfig.ProgressClockUpperLeftCornerCharSeconds;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╔";
                Config.SaverConfig.ProgressClockUpperLeftCornerCharSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Upper right corner character for hours bar
        /// </summary>
        public static string ProgressClockUpperRightCornerCharHours
        {
            get
            {
                return Config.SaverConfig.ProgressClockUpperRightCornerCharHours;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╗";
                Config.SaverConfig.ProgressClockUpperRightCornerCharHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Upper right corner character for minutes bar
        /// </summary>
        public static string ProgressClockUpperRightCornerCharMinutes
        {
            get
            {
                return Config.SaverConfig.ProgressClockUpperRightCornerCharMinutes;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╗";
                Config.SaverConfig.ProgressClockUpperRightCornerCharMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Upper right corner character for seconds bar
        /// </summary>
        public static string ProgressClockUpperRightCornerCharSeconds
        {
            get
            {
                return Config.SaverConfig.ProgressClockUpperRightCornerCharSeconds;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╗";
                Config.SaverConfig.ProgressClockUpperRightCornerCharSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Lower left corner character for hours bar
        /// </summary>
        public static string ProgressClockLowerLeftCornerCharHours
        {
            get
            {
                return Config.SaverConfig.ProgressClockLowerLeftCornerCharHours;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╚";
                Config.SaverConfig.ProgressClockLowerLeftCornerCharHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Lower left corner character for minutes bar
        /// </summary>
        public static string ProgressClockLowerLeftCornerCharMinutes
        {
            get
            {
                return Config.SaverConfig.ProgressClockLowerLeftCornerCharMinutes;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╚";
                Config.SaverConfig.ProgressClockLowerLeftCornerCharMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Lower left corner character for seconds bar
        /// </summary>
        public static string ProgressClockLowerLeftCornerCharSeconds
        {
            get
            {
                return Config.SaverConfig.ProgressClockLowerLeftCornerCharSeconds;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╚";
                Config.SaverConfig.ProgressClockLowerLeftCornerCharSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Lower right corner character for hours bar
        /// </summary>
        public static string ProgressClockLowerRightCornerCharHours
        {
            get
            {
                return Config.SaverConfig.ProgressClockLowerRightCornerCharHours;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╝";
                Config.SaverConfig.ProgressClockLowerRightCornerCharHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Lower right corner character for minutes bar
        /// </summary>
        public static string ProgressClockLowerRightCornerCharMinutes
        {
            get
            {
                return Config.SaverConfig.ProgressClockLowerRightCornerCharMinutes;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╝";
                Config.SaverConfig.ProgressClockLowerRightCornerCharMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Lower right corner character for seconds bar
        /// </summary>
        public static string ProgressClockLowerRightCornerCharSeconds
        {
            get
            {
                return Config.SaverConfig.ProgressClockLowerRightCornerCharSeconds;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╝";
                Config.SaverConfig.ProgressClockLowerRightCornerCharSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Upper frame character for hours bar
        /// </summary>
        public static string ProgressClockUpperFrameCharHours
        {
            get
            {
                return Config.SaverConfig.ProgressClockUpperFrameCharHours;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "═";
                Config.SaverConfig.ProgressClockUpperFrameCharHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Upper frame character for minutes bar
        /// </summary>
        public static string ProgressClockUpperFrameCharMinutes
        {
            get
            {
                return Config.SaverConfig.ProgressClockUpperFrameCharMinutes;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "═";
                Config.SaverConfig.ProgressClockUpperFrameCharMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Upper frame character for seconds bar
        /// </summary>
        public static string ProgressClockUpperFrameCharSeconds
        {
            get
            {
                return Config.SaverConfig.ProgressClockUpperFrameCharSeconds;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "═";
                Config.SaverConfig.ProgressClockUpperFrameCharSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Lower frame character for hours bar
        /// </summary>
        public static string ProgressClockLowerFrameCharHours
        {
            get
            {
                return Config.SaverConfig.ProgressClockLowerFrameCharHours;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "═";
                Config.SaverConfig.ProgressClockLowerFrameCharHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Lower frame character for minutes bar
        /// </summary>
        public static string ProgressClockLowerFrameCharMinutes
        {
            get
            {
                return Config.SaverConfig.ProgressClockLowerFrameCharMinutes;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "═";
                Config.SaverConfig.ProgressClockLowerFrameCharMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Lower frame character for seconds bar
        /// </summary>
        public static string ProgressClockLowerFrameCharSeconds
        {
            get
            {
                return Config.SaverConfig.ProgressClockLowerFrameCharSeconds;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "═";
                Config.SaverConfig.ProgressClockLowerFrameCharSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Left frame character for hours bar
        /// </summary>
        public static string ProgressClockLeftFrameCharHours
        {
            get
            {
                return Config.SaverConfig.ProgressClockLeftFrameCharHours;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "║";
                Config.SaverConfig.ProgressClockLeftFrameCharHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Left frame character for minutes bar
        /// </summary>
        public static string ProgressClockLeftFrameCharMinutes
        {
            get
            {
                return Config.SaverConfig.ProgressClockLeftFrameCharMinutes;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "║";
                Config.SaverConfig.ProgressClockLeftFrameCharMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Left frame character for seconds bar
        /// </summary>
        public static string ProgressClockLeftFrameCharSeconds
        {
            get
            {
                return Config.SaverConfig.ProgressClockLeftFrameCharSeconds;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "║";
                Config.SaverConfig.ProgressClockLeftFrameCharSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Right frame character for hours bar
        /// </summary>
        public static string ProgressClockRightFrameCharHours
        {
            get
            {
                return Config.SaverConfig.ProgressClockRightFrameCharHours;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "║";
                Config.SaverConfig.ProgressClockRightFrameCharHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Right frame character for minutes bar
        /// </summary>
        public static string ProgressClockRightFrameCharMinutes
        {
            get
            {
                return Config.SaverConfig.ProgressClockRightFrameCharMinutes;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "║";
                Config.SaverConfig.ProgressClockRightFrameCharMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Right frame character for seconds bar
        /// </summary>
        public static string ProgressClockRightFrameCharSeconds
        {
            get
            {
                return Config.SaverConfig.ProgressClockRightFrameCharSeconds;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "║";
                Config.SaverConfig.ProgressClockRightFrameCharSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Information text for hours bar
        /// </summary>
        public static string ProgressClockInfoTextHours
        {
            get
            {
                return Config.SaverConfig.ProgressClockInfoTextHours;
            }
            set
            {
                Config.SaverConfig.ProgressClockInfoTextHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Information text for minutes bar
        /// </summary>
        public static string ProgressClockInfoTextMinutes
        {
            get
            {
                return Config.SaverConfig.ProgressClockInfoTextMinutes;
            }
            set
            {
                Config.SaverConfig.ProgressClockInfoTextMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Information text for seconds bar
        /// </summary>
        public static string ProgressClockInfoTextSeconds
        {
            get
            {
                return Config.SaverConfig.ProgressClockInfoTextSeconds;
            }
            set
            {
                Config.SaverConfig.ProgressClockInfoTextSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum red color level (true color - hours)
        /// </summary>
        public static int ProgressClockMinimumRedColorLevelHours
        {
            get
            {
                return Config.SaverConfig.ProgressClockMinimumRedColorLevelHours;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.ProgressClockMinimumRedColorLevelHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum green color level (true color - hours)
        /// </summary>
        public static int ProgressClockMinimumGreenColorLevelHours
        {
            get
            {
                return Config.SaverConfig.ProgressClockMinimumGreenColorLevelHours;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.ProgressClockMinimumGreenColorLevelHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum blue color level (true color - hours)
        /// </summary>
        public static int ProgressClockMinimumBlueColorLevelHours
        {
            get
            {
                return Config.SaverConfig.ProgressClockMinimumBlueColorLevelHours;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.ProgressClockMinimumBlueColorLevelHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum color level (255 colors or 16 colors - hours)
        /// </summary>
        public static int ProgressClockMinimumColorLevelHours
        {
            get
            {
                return Config.SaverConfig.ProgressClockMinimumColorLevelHours;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                Config.SaverConfig.ProgressClockMinimumColorLevelHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum red color level (true color - hours)
        /// </summary>
        public static int ProgressClockMaximumRedColorLevelHours
        {
            get
            {
                return Config.SaverConfig.ProgressClockMaximumRedColorLevelHours;
            }
            set
            {
                if (value <= Config.SaverConfig.ProgressClockMinimumRedColorLevelHours)
                    value = Config.SaverConfig.ProgressClockMinimumRedColorLevelHours;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.ProgressClockMaximumRedColorLevelHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum green color level (true color - hours)
        /// </summary>
        public static int ProgressClockMaximumGreenColorLevelHours
        {
            get
            {
                return Config.SaverConfig.ProgressClockMaximumGreenColorLevelHours;
            }
            set
            {
                if (value <= Config.SaverConfig.ProgressClockMinimumGreenColorLevelHours)
                    value = Config.SaverConfig.ProgressClockMinimumGreenColorLevelHours;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.ProgressClockMaximumGreenColorLevelHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum blue color level (true color - hours)
        /// </summary>
        public static int ProgressClockMaximumBlueColorLevelHours
        {
            get
            {
                return Config.SaverConfig.ProgressClockMaximumBlueColorLevelHours;
            }
            set
            {
                if (value <= Config.SaverConfig.ProgressClockMinimumBlueColorLevelHours)
                    value = Config.SaverConfig.ProgressClockMinimumBlueColorLevelHours;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.ProgressClockMaximumBlueColorLevelHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum color level (255 colors or 16 colors - hours)
        /// </summary>
        public static int ProgressClockMaximumColorLevelHours
        {
            get
            {
                return Config.SaverConfig.ProgressClockMaximumColorLevelHours;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= Config.SaverConfig.ProgressClockMinimumColorLevelHours)
                    value = Config.SaverConfig.ProgressClockMinimumColorLevelHours;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                Config.SaverConfig.ProgressClockMaximumColorLevelHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum red color level (true color - minutes)
        /// </summary>
        public static int ProgressClockMinimumRedColorLevelMinutes
        {
            get
            {
                return Config.SaverConfig.ProgressClockMinimumRedColorLevelMinutes;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.ProgressClockMinimumRedColorLevelMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum green color level (true color - minutes)
        /// </summary>
        public static int ProgressClockMinimumGreenColorLevelMinutes
        {
            get
            {
                return Config.SaverConfig.ProgressClockMinimumGreenColorLevelMinutes;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.ProgressClockMinimumGreenColorLevelMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum blue color level (true color - minutes)
        /// </summary>
        public static int ProgressClockMinimumBlueColorLevelMinutes
        {
            get
            {
                return Config.SaverConfig.ProgressClockMinimumBlueColorLevelMinutes;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.ProgressClockMinimumBlueColorLevelMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum color level (255 colors or 16 colors - minutes)
        /// </summary>
        public static int ProgressClockMinimumColorLevelMinutes
        {
            get
            {
                return Config.SaverConfig.ProgressClockMinimumColorLevelMinutes;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                Config.SaverConfig.ProgressClockMinimumColorLevelMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum red color level (true color - minutes)
        /// </summary>
        public static int ProgressClockMaximumRedColorLevelMinutes
        {
            get
            {
                return Config.SaverConfig.ProgressClockMaximumRedColorLevelMinutes;
            }
            set
            {
                if (value <= Config.SaverConfig.ProgressClockMinimumRedColorLevelMinutes)
                    value = Config.SaverConfig.ProgressClockMinimumRedColorLevelMinutes;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.ProgressClockMaximumRedColorLevelMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum green color level (true color - minutes)
        /// </summary>
        public static int ProgressClockMaximumGreenColorLevelMinutes
        {
            get
            {
                return Config.SaverConfig.ProgressClockMaximumGreenColorLevelMinutes;
            }
            set
            {
                if (value <= Config.SaverConfig.ProgressClockMinimumGreenColorLevelMinutes)
                    value = Config.SaverConfig.ProgressClockMinimumGreenColorLevelMinutes;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.ProgressClockMaximumGreenColorLevelMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum blue color level (true color - minutes)
        /// </summary>
        public static int ProgressClockMaximumBlueColorLevelMinutes
        {
            get
            {
                return Config.SaverConfig.ProgressClockMaximumBlueColorLevelMinutes;
            }
            set
            {
                if (value <= Config.SaverConfig.ProgressClockMinimumBlueColorLevelMinutes)
                    value = Config.SaverConfig.ProgressClockMinimumBlueColorLevelMinutes;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.ProgressClockMaximumBlueColorLevelMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum color level (255 colors or 16 colors - minutes)
        /// </summary>
        public static int ProgressClockMaximumColorLevelMinutes
        {
            get
            {
                return Config.SaverConfig.ProgressClockMaximumColorLevelMinutes;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= Config.SaverConfig.ProgressClockMinimumColorLevelMinutes)
                    value = Config.SaverConfig.ProgressClockMinimumColorLevelMinutes;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                Config.SaverConfig.ProgressClockMaximumColorLevelMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum red color level (true color - seconds)
        /// </summary>
        public static int ProgressClockMinimumRedColorLevelSeconds
        {
            get
            {
                return Config.SaverConfig.ProgressClockMinimumRedColorLevelSeconds;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.ProgressClockMinimumRedColorLevelSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum green color level (true color - seconds)
        /// </summary>
        public static int ProgressClockMinimumGreenColorLevelSeconds
        {
            get
            {
                return Config.SaverConfig.ProgressClockMinimumGreenColorLevelSeconds;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.ProgressClockMinimumGreenColorLevelSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum blue color level (true color - seconds)
        /// </summary>
        public static int ProgressClockMinimumBlueColorLevelSeconds
        {
            get
            {
                return Config.SaverConfig.ProgressClockMinimumBlueColorLevelSeconds;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.ProgressClockMinimumBlueColorLevelSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum color level (255 colors or 16 colors - seconds)
        /// </summary>
        public static int ProgressClockMinimumColorLevelSeconds
        {
            get
            {
                return Config.SaverConfig.ProgressClockMinimumColorLevelSeconds;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                Config.SaverConfig.ProgressClockMinimumColorLevelSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum red color level (true color - seconds)
        /// </summary>
        public static int ProgressClockMaximumRedColorLevelSeconds
        {
            get
            {
                return Config.SaverConfig.ProgressClockMaximumRedColorLevelSeconds;
            }
            set
            {
                if (value <= Config.SaverConfig.ProgressClockMinimumRedColorLevelSeconds)
                    value = Config.SaverConfig.ProgressClockMinimumRedColorLevelSeconds;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.ProgressClockMaximumRedColorLevelSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum green color level (true color - seconds)
        /// </summary>
        public static int ProgressClockMaximumGreenColorLevelSeconds
        {
            get
            {
                return Config.SaverConfig.ProgressClockMaximumGreenColorLevelSeconds;
            }
            set
            {
                if (value <= Config.SaverConfig.ProgressClockMinimumGreenColorLevelSeconds)
                    value = Config.SaverConfig.ProgressClockMinimumGreenColorLevelSeconds;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.ProgressClockMaximumGreenColorLevelSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum blue color level (true color - seconds)
        /// </summary>
        public static int ProgressClockMaximumBlueColorLevelSeconds
        {
            get
            {
                return Config.SaverConfig.ProgressClockMaximumBlueColorLevelSeconds;
            }
            set
            {
                if (value <= Config.SaverConfig.ProgressClockMinimumBlueColorLevelSeconds)
                    value = Config.SaverConfig.ProgressClockMinimumBlueColorLevelSeconds;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.ProgressClockMaximumBlueColorLevelSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum color level (255 colors or 16 colors - seconds)
        /// </summary>
        public static int ProgressClockMaximumColorLevelSeconds
        {
            get
            {
                return Config.SaverConfig.ProgressClockMaximumColorLevelSeconds;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= Config.SaverConfig.ProgressClockMinimumColorLevelSeconds)
                    value = Config.SaverConfig.ProgressClockMinimumColorLevelSeconds;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                Config.SaverConfig.ProgressClockMaximumColorLevelSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum red color level (true color)
        /// </summary>
        public static int ProgressClockMinimumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.ProgressClockMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.ProgressClockMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum green color level (true color)
        /// </summary>
        public static int ProgressClockMinimumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.ProgressClockMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.ProgressClockMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum blue color level (true color)
        /// </summary>
        public static int ProgressClockMinimumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.ProgressClockMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.ProgressClockMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int ProgressClockMinimumColorLevel
        {
            get
            {
                return Config.SaverConfig.ProgressClockMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                Config.SaverConfig.ProgressClockMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum red color level (true color)
        /// </summary>
        public static int ProgressClockMaximumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.ProgressClockMaximumRedColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.ProgressClockMinimumRedColorLevel)
                    value = Config.SaverConfig.ProgressClockMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.ProgressClockMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum green color level (true color)
        /// </summary>
        public static int ProgressClockMaximumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.ProgressClockMaximumGreenColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.ProgressClockMinimumGreenColorLevel)
                    value = Config.SaverConfig.ProgressClockMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.ProgressClockMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum blue color level (true color)
        /// </summary>
        public static int ProgressClockMaximumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.ProgressClockMaximumBlueColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.ProgressClockMinimumBlueColorLevel)
                    value = Config.SaverConfig.ProgressClockMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.ProgressClockMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int ProgressClockMaximumColorLevel
        {
            get
            {
                return Config.SaverConfig.ProgressClockMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= Config.SaverConfig.ProgressClockMinimumColorLevel)
                    value = Config.SaverConfig.ProgressClockMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                Config.SaverConfig.ProgressClockMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for ProgressClock
    /// </summary>
    public class ProgressClockDisplay : BaseScreensaver, IScreensaver
    {

        private long CurrentTicks;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "ProgressClock";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            CurrentTicks = ProgressClockSettings.ProgressClockCycleColorsTicks;
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;
            ConsoleWrapper.Clear();

            // Prepare colors
            int RedColorNumHours, GreenColorNumHours, BlueColorNumHours;
            int RedColorNumMinutes, GreenColorNumMinutes, BlueColorNumMinutes;
            int RedColorNumSeconds, GreenColorNumSeconds, BlueColorNumSeconds;
            int RedColorNum, GreenColorNum, BlueColorNum;
            int ColorNumHours, ColorNumMinutes, ColorNumSeconds, ColorNum;
            int ProgressFillPositionHours, ProgressFillPositionMinutes, ProgressFillPositionSeconds;
            int InformationPositionHours, InformationPositionMinutes, InformationPositionSeconds;
            Color ColorStorageHours = default, ColorStorageMinutes = default, ColorStorageSeconds = default, ColorStorage;

            DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Current tick: {0}", CurrentTicks);
            if (ProgressClockSettings.ProgressClockCycleColors)
            {
                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Cycling colors...");
                if (CurrentTicks >= ProgressClockSettings.ProgressClockCycleColorsTicks)
                {
                    if (ProgressClockSettings.ProgressClockTrueColor)
                    {
                        DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Current tick equals the maximum ticks to change color.");
                        RedColorNumHours = RandomDriver.Random(ProgressClockSettings.ProgressClockMinimumRedColorLevelHours, ProgressClockSettings.ProgressClockMaximumRedColorLevelHours);
                        GreenColorNumHours = RandomDriver.Random(ProgressClockSettings.ProgressClockMinimumGreenColorLevelHours, ProgressClockSettings.ProgressClockMaximumGreenColorLevelHours);
                        BlueColorNumHours = RandomDriver.Random(ProgressClockSettings.ProgressClockMinimumBlueColorLevelHours, ProgressClockSettings.ProgressClockMaximumBlueColorLevelHours);
                        DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (Hours) (R;G;B: {0};{1};{2})", RedColorNumHours, GreenColorNumHours, BlueColorNumHours);
                        RedColorNumMinutes = RandomDriver.Random(ProgressClockSettings.ProgressClockMinimumRedColorLevelMinutes, ProgressClockSettings.ProgressClockMaximumRedColorLevelMinutes);
                        GreenColorNumMinutes = RandomDriver.Random(ProgressClockSettings.ProgressClockMinimumGreenColorLevelMinutes, ProgressClockSettings.ProgressClockMaximumGreenColorLevelMinutes);
                        BlueColorNumMinutes = RandomDriver.Random(ProgressClockSettings.ProgressClockMinimumBlueColorLevelMinutes, ProgressClockSettings.ProgressClockMaximumBlueColorLevelMinutes);
                        DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (Minutes) (R;G;B: {0};{1};{2})", RedColorNumMinutes, GreenColorNumMinutes, BlueColorNumMinutes);
                        RedColorNumSeconds = RandomDriver.Random(ProgressClockSettings.ProgressClockMinimumRedColorLevelSeconds, ProgressClockSettings.ProgressClockMaximumRedColorLevelSeconds);
                        GreenColorNumSeconds = RandomDriver.Random(ProgressClockSettings.ProgressClockMinimumGreenColorLevelSeconds, ProgressClockSettings.ProgressClockMaximumGreenColorLevelSeconds);
                        BlueColorNumSeconds = RandomDriver.Random(ProgressClockSettings.ProgressClockMinimumBlueColorLevelSeconds, ProgressClockSettings.ProgressClockMaximumBlueColorLevelSeconds);
                        DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (Seconds) (R;G;B: {0};{1};{2})", RedColorNumSeconds, GreenColorNumSeconds, BlueColorNumSeconds);
                        RedColorNum = RandomDriver.Random(ProgressClockSettings.ProgressClockMinimumRedColorLevel, ProgressClockSettings.ProgressClockMaximumRedColorLevel);
                        GreenColorNum = RandomDriver.Random(ProgressClockSettings.ProgressClockMinimumGreenColorLevel, ProgressClockSettings.ProgressClockMaximumGreenColorLevel);
                        BlueColorNum = RandomDriver.Random(ProgressClockSettings.ProgressClockMinimumBlueColorLevel, ProgressClockSettings.ProgressClockMaximumBlueColorLevel);
                        DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                        ColorStorageHours = new Color(RedColorNumHours, GreenColorNumHours, BlueColorNumHours);
                        ColorStorageMinutes = new Color(RedColorNumMinutes, GreenColorNumMinutes, BlueColorNumMinutes);
                        ColorStorageSeconds = new Color(RedColorNumSeconds, GreenColorNumSeconds, BlueColorNumSeconds);
                        ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                    }
                    else
                    {
                        ColorNumHours = RandomDriver.Random(ProgressClockSettings.ProgressClockMinimumColorLevelHours, ProgressClockSettings.ProgressClockMaximumColorLevelHours);
                        DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (Hours) ({0})", ColorNumHours);
                        ColorNumMinutes = RandomDriver.Random(ProgressClockSettings.ProgressClockMinimumColorLevelMinutes, ProgressClockSettings.ProgressClockMaximumColorLevelMinutes);
                        DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (Minutes) ({0})", ColorNumMinutes);
                        ColorNumSeconds = RandomDriver.Random(ProgressClockSettings.ProgressClockMinimumColorLevelSeconds, ProgressClockSettings.ProgressClockMaximumColorLevelSeconds);
                        DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (Seconds) ({0})", ColorNumSeconds);
                        ColorNum = RandomDriver.Random(ProgressClockSettings.ProgressClockMinimumColorLevel, ProgressClockSettings.ProgressClockMaximumColorLevel);
                        DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
                        ColorStorageHours = new Color(ColorNumHours);
                        ColorStorageMinutes = new Color(ColorNumMinutes);
                        ColorStorageSeconds = new Color(ColorNumSeconds);
                        ColorStorage = new Color(ColorNum);
                    }
                    CurrentTicks = 0L;
                }
            }
            else
            {
                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Parsing colors...");
                ColorStorageHours = new Color(ProgressClockSettings.ProgressClockHoursProgressColor);
                ColorStorageMinutes = new Color(ProgressClockSettings.ProgressClockMinutesProgressColor);
                ColorStorageSeconds = new Color(ProgressClockSettings.ProgressClockSecondsProgressColor);
                ColorStorage = new Color(ProgressClockSettings.ProgressClockProgressColor);
            }
            ProgressFillPositionHours = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) - 10;
            DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Fill position for progress (Hours) {0}", ProgressFillPositionHours);
            ProgressFillPositionMinutes = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) - 1;
            DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Fill position for progress (Minutes) {0}", ProgressFillPositionMinutes);
            ProgressFillPositionSeconds = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) + 8;
            DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Fill position for progress (Seconds) {0}", ProgressFillPositionSeconds);
            InformationPositionHours = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) - 12;
            DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Fill position for info (Hours) {0}", InformationPositionHours);
            InformationPositionMinutes = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) - 3;
            DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Fill position for info (Minutes) {0}", InformationPositionMinutes);
            InformationPositionSeconds = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) + 6;
            DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Fill position for info (Seconds) {0}", InformationPositionSeconds);

            if (!ConsoleResizeListener.WasResized(false))
            {
                // Hours
                TextWriterWhereColor.WriteWhere(ProgressClockSettings.ProgressClockLowerLeftCornerCharHours + ProgressClockSettings.ProgressClockLowerFrameCharHours.Repeat(ConsoleWrapper.WindowWidth - 10) + ProgressClockSettings.ProgressClockLowerRightCornerCharHours, 4, (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) - 9, true, ColorStorageHours);         // Bottom of Hours
                TextWriterWhereColor.WriteWhere(ProgressClockSettings.ProgressClockLeftFrameCharHours + " ".Repeat(ConsoleWrapper.WindowWidth - 10) + ProgressClockSettings.ProgressClockRightFrameCharHours, 4, ProgressFillPositionHours, true, ColorStorageHours);                                                           // Medium of Hours
                TextWriterWhereColor.WriteWhere(ProgressClockSettings.ProgressClockUpperLeftCornerCharHours + ProgressClockSettings.ProgressClockUpperFrameCharHours.Repeat(ConsoleWrapper.WindowWidth - 10) + ProgressClockSettings.ProgressClockUpperRightCornerCharHours, 4, (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) - 11, true, ColorStorageHours);        // Top of Hours

                // Minutes
                TextWriterWhereColor.WriteWhere(ProgressClockSettings.ProgressClockLowerLeftCornerCharMinutes + ProgressClockSettings.ProgressClockLowerFrameCharMinutes.Repeat(ConsoleWrapper.WindowWidth - 10) + ProgressClockSettings.ProgressClockLowerRightCornerCharMinutes, 4, (int)Math.Round(ConsoleWrapper.WindowHeight / 2d), true, ColorStorageMinutes);     // Bottom of Minutes
                TextWriterWhereColor.WriteWhere(ProgressClockSettings.ProgressClockLeftFrameCharMinutes + " ".Repeat(ConsoleWrapper.WindowWidth - 10) + ProgressClockSettings.ProgressClockRightFrameCharMinutes, 4, ProgressFillPositionMinutes, true, ColorStorageMinutes);                                                   // Medium of Minutes
                TextWriterWhereColor.WriteWhere(ProgressClockSettings.ProgressClockUpperLeftCornerCharMinutes + ProgressClockSettings.ProgressClockUpperFrameCharMinutes.Repeat(ConsoleWrapper.WindowWidth - 10) + ProgressClockSettings.ProgressClockUpperRightCornerCharMinutes, 4, (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) - 2, true, ColorStorageMinutes); // Top of Minutes

                // Seconds
                TextWriterWhereColor.WriteWhere(ProgressClockSettings.ProgressClockLowerLeftCornerCharSeconds + ProgressClockSettings.ProgressClockLowerFrameCharSeconds.Repeat(ConsoleWrapper.WindowWidth - 10) + ProgressClockSettings.ProgressClockLowerRightCornerCharSeconds, 4, (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) + 9, true, ColorStorageSeconds); // Bottom of Seconds
                TextWriterWhereColor.WriteWhere(ProgressClockSettings.ProgressClockLeftFrameCharSeconds + " ".Repeat(ConsoleWrapper.WindowWidth - 10) + ProgressClockSettings.ProgressClockRightFrameCharSeconds, 4, ProgressFillPositionSeconds, true, ColorStorageSeconds);                                                   // Medium of Seconds
                TextWriterWhereColor.WriteWhere(ProgressClockSettings.ProgressClockUpperLeftCornerCharSeconds + ProgressClockSettings.ProgressClockUpperFrameCharSeconds.Repeat(ConsoleWrapper.WindowWidth - 10) + ProgressClockSettings.ProgressClockUpperRightCornerCharSeconds, 4, (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) + 7, true, ColorStorageSeconds); // Top of Seconds

                // Fill progress for hours, minutes, and seconds
                if (!(TimeDateTools.KernelDateTime.Hour == 0))
                    TextWriterWhereColor.WriteWhere(" ".Repeat(ConsoleExtensions.PercentRepeat(TimeDateTools.KernelDateTime.Hour, 24, 10)), 5, ProgressFillPositionHours, true, Color.Empty, ColorStorageHours);
                if (!(TimeDateTools.KernelDateTime.Minute == 0))
                    TextWriterWhereColor.WriteWhere(" ".Repeat(ConsoleExtensions.PercentRepeat(TimeDateTools.KernelDateTime.Minute, 60, 10)), 5, ProgressFillPositionMinutes, true, Color.Empty, ColorStorageMinutes);
                if (!(TimeDateTools.KernelDateTime.Second == 0))
                    TextWriterWhereColor.WriteWhere(" ".Repeat(ConsoleExtensions.PercentRepeat(TimeDateTools.KernelDateTime.Second, 60, 10)), 5, ProgressFillPositionSeconds, true, Color.Empty, ColorStorageSeconds);

                // Print information
                if (!string.IsNullOrEmpty(ProgressClockSettings.ProgressClockInfoTextHours))
                    TextWriterWhereColor.WriteWhere(PlaceParse.ProbePlaces(ProgressClockSettings.ProgressClockInfoTextHours), 4, InformationPositionHours, true, ColorStorageHours, TimeDateTools.KernelDateTime.Hour);
                else
                    TextWriterWhereColor.WriteWhere("H: {0}/24", 4, InformationPositionHours, true, ColorStorageHours, TimeDateTools.KernelDateTime.Hour);
                if (!string.IsNullOrEmpty(ProgressClockSettings.ProgressClockInfoTextMinutes))
                    TextWriterWhereColor.WriteWhere(PlaceParse.ProbePlaces(ProgressClockSettings.ProgressClockInfoTextMinutes), 4, InformationPositionMinutes, true, ColorStorageMinutes, TimeDateTools.KernelDateTime.Minute);
                else
                    TextWriterWhereColor.WriteWhere("M: {0}/60", 4, InformationPositionMinutes, true, ColorStorageMinutes, TimeDateTools.KernelDateTime.Minute);
                if (!string.IsNullOrEmpty(ProgressClockSettings.ProgressClockInfoTextHours))
                    TextWriterWhereColor.WriteWhere(PlaceParse.ProbePlaces(ProgressClockSettings.ProgressClockInfoTextSeconds), 4, InformationPositionSeconds, true, ColorStorageSeconds, TimeDateTools.KernelDateTime.Second);
                else
                    TextWriterWhereColor.WriteWhere("S: {0}/60", 4, InformationPositionSeconds, true, ColorStorageSeconds, TimeDateTools.KernelDateTime.Second);

                // Print date information
                TextWriterWhereColor.WriteWhere(TimeDateRenderers.Render(), (int)Math.Round(ConsoleWrapper.WindowWidth / 2d - TimeDateRenderers.Render().Length / 2d), ConsoleWrapper.WindowHeight - 2, ColorStorageSeconds);
            }
            if (ProgressClockSettings.ProgressClockCycleColors)
                CurrentTicks += 1L;

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(ProgressClockSettings.ProgressClockDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
