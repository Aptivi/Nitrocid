
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
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
using ColorSeq;
using Extensification.StringExts;
using KS.ConsoleBase;
using KS.Drivers.RNG;
using KS.Kernel.Debugging;
using KS.Misc.Probers;
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

        private static bool _progressClockTrueColor = true;
        private static bool _progressClockCycleColors = true;
        private static string _progressClockSecondsProgressColor = 4.ToString();
        private static string _progressClockMinutesProgressColor = 5.ToString();
        private static string _progressClockHoursProgressColor = 6.ToString();
        private static string _progressClockProgressColor = 7.ToString();
        private static long _progressClockCycleColorsTicks = 20L;
        private static int _progressClockDelay = 500;
        private static string _progressClockUpperLeftCornerCharHours = "╔";
        private static string _progressClockUpperLeftCornerCharMinutes = "╔";
        private static string _progressClockUpperLeftCornerCharSeconds = "╔";
        private static string _progressClockUpperRightCornerCharHours = "╗";
        private static string _progressClockUpperRightCornerCharMinutes = "╗";
        private static string _progressClockUpperRightCornerCharSeconds = "╗";
        private static string _progressClockLowerLeftCornerCharHours = "╚";
        private static string _progressClockLowerLeftCornerCharMinutes = "╚";
        private static string _progressClockLowerLeftCornerCharSeconds = "╚";
        private static string _progressClockLowerRightCornerCharHours = "╝";
        private static string _progressClockLowerRightCornerCharMinutes = "╝";
        private static string _progressClockLowerRightCornerCharSeconds = "╝";
        private static string _progressClockUpperFrameCharHours = "═";
        private static string _progressClockUpperFrameCharMinutes = "═";
        private static string _progressClockUpperFrameCharSeconds = "═";
        private static string _progressClockLowerFrameCharHours = "═";
        private static string _progressClockLowerFrameCharMinutes = "═";
        private static string _progressClockLowerFrameCharSeconds = "═";
        private static string _progressClockLeftFrameCharHours = "║";
        private static string _progressClockLeftFrameCharMinutes = "║";
        private static string _progressClockLeftFrameCharSeconds = "║";
        private static string _progressClockRightFrameCharHours = "║";
        private static string _progressClockRightFrameCharMinutes = "║";
        private static string _progressClockRightFrameCharSeconds = "║";
        private static string _progressClockInfoTextHours = "";
        private static string _progressClockInfoTextMinutes = "";
        private static string _progressClockInfoTextSeconds = "";
        private static int _progressClockMinimumRedColorLevelHours = 0;
        private static int _progressClockMinimumGreenColorLevelHours = 0;
        private static int _progressClockMinimumBlueColorLevelHours = 0;
        private static int _progressClockMinimumColorLevelHours = 0;
        private static int _progressClockMaximumRedColorLevelHours = 255;
        private static int _progressClockMaximumGreenColorLevelHours = 255;
        private static int _progressClockMaximumBlueColorLevelHours = 255;
        private static int _progressClockMaximumColorLevelHours = 255;
        private static int _progressClockMinimumRedColorLevelMinutes = 0;
        private static int _progressClockMinimumGreenColorLevelMinutes = 0;
        private static int _progressClockMinimumBlueColorLevelMinutes = 0;
        private static int _progressClockMinimumColorLevelMinutes = 0;
        private static int _progressClockMaximumRedColorLevelMinutes = 255;
        private static int _progressClockMaximumGreenColorLevelMinutes = 255;
        private static int _progressClockMaximumBlueColorLevelMinutes = 255;
        private static int _progressClockMaximumColorLevelMinutes = 255;
        private static int _progressClockMinimumRedColorLevelSeconds = 0;
        private static int _progressClockMinimumGreenColorLevelSeconds = 0;
        private static int _progressClockMinimumBlueColorLevelSeconds = 0;
        private static int _progressClockMinimumColorLevelSeconds = 0;
        private static int _progressClockMaximumRedColorLevelSeconds = 255;
        private static int _progressClockMaximumGreenColorLevelSeconds = 255;
        private static int _progressClockMaximumBlueColorLevelSeconds = 255;
        private static int _progressClockMaximumColorLevelSeconds = 255;
        private static int _progressClockMinimumRedColorLevel = 0;
        private static int _progressClockMinimumGreenColorLevel = 0;
        private static int _progressClockMinimumBlueColorLevel = 0;
        private static int _progressClockMinimumColorLevel = 0;
        private static int _progressClockMaximumRedColorLevel = 255;
        private static int _progressClockMaximumGreenColorLevel = 255;
        private static int _progressClockMaximumBlueColorLevel = 255;
        private static int _progressClockMaximumColorLevel = 255;

        /// <summary>
        /// [ProgressClock] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool ProgressClockTrueColor
        {
            get
            {
                return _progressClockTrueColor;
            }
            set
            {
                _progressClockTrueColor = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Enable color cycling (uses RNG. If disabled, uses the <see cref="ProgressClockSecondsProgressColor"/>, <see cref="ProgressClockMinutesProgressColor"/>, and <see cref="ProgressClockHoursProgressColor"/> colors.)
        /// </summary>
        public static bool ProgressClockCycleColors
        {
            get
            {
                return _progressClockCycleColors;
            }
            set
            {
                _progressClockCycleColors = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The color of seconds progress bar. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public static string ProgressClockSecondsProgressColor
        {
            get
            {
                return _progressClockSecondsProgressColor;
            }
            set
            {
                _progressClockSecondsProgressColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [ProgressClock] The color of minutes progress bar. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public static string ProgressClockMinutesProgressColor
        {
            get
            {
                return _progressClockMinutesProgressColor;
            }
            set
            {
                _progressClockMinutesProgressColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [ProgressClock] The color of hours progress bar. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public static string ProgressClockHoursProgressColor
        {
            get
            {
                return _progressClockHoursProgressColor;
            }
            set
            {
                _progressClockHoursProgressColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [ProgressClock] The color of date information. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public static string ProgressClockProgressColor
        {
            get
            {
                return _progressClockProgressColor;
            }
            set
            {
                _progressClockProgressColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [ProgressClock] If color cycling is enabled, how many ticks before changing colors? 1 tick = 0.5 seconds
        /// </summary>
        public static long ProgressClockCycleColorsTicks
        {
            get
            {
                return _progressClockCycleColorsTicks;
            }
            set
            {
                if (value <= 0L)
                    value = 20L;
                _progressClockCycleColorsTicks = value;
            }
        }
        /// <summary>
        /// [ProgressClock] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int ProgressClockDelay
        {
            get
            {
                return _progressClockDelay;
            }
            set
            {
                if (value <= 0)
                    value = 500;
                _progressClockDelay = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Upper left corner character for hours bar
        /// </summary>
        public static string ProgressClockUpperLeftCornerCharHours
        {
            get
            {
                return _progressClockUpperLeftCornerCharHours;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╔";
                _progressClockUpperLeftCornerCharHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Upper left corner character for minutes bar
        /// </summary>
        public static string ProgressClockUpperLeftCornerCharMinutes
        {
            get
            {
                return _progressClockUpperLeftCornerCharMinutes;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╔";
                _progressClockUpperLeftCornerCharMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Upper left corner character for seconds bar
        /// </summary>
        public static string ProgressClockUpperLeftCornerCharSeconds
        {
            get
            {
                return _progressClockUpperLeftCornerCharSeconds;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╔";
                _progressClockUpperLeftCornerCharSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Upper right corner character for hours bar
        /// </summary>
        public static string ProgressClockUpperRightCornerCharHours
        {
            get
            {
                return _progressClockUpperRightCornerCharHours;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╗";
                _progressClockUpperRightCornerCharHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Upper right corner character for minutes bar
        /// </summary>
        public static string ProgressClockUpperRightCornerCharMinutes
        {
            get
            {
                return _progressClockUpperRightCornerCharMinutes;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╗";
                _progressClockUpperRightCornerCharMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Upper right corner character for seconds bar
        /// </summary>
        public static string ProgressClockUpperRightCornerCharSeconds
        {
            get
            {
                return _progressClockUpperRightCornerCharSeconds;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╗";
                _progressClockUpperRightCornerCharSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Lower left corner character for hours bar
        /// </summary>
        public static string ProgressClockLowerLeftCornerCharHours
        {
            get
            {
                return _progressClockLowerLeftCornerCharHours;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╚";
                _progressClockLowerLeftCornerCharHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Lower left corner character for minutes bar
        /// </summary>
        public static string ProgressClockLowerLeftCornerCharMinutes
        {
            get
            {
                return _progressClockLowerLeftCornerCharMinutes;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╚";
                _progressClockLowerLeftCornerCharMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Lower left corner character for seconds bar
        /// </summary>
        public static string ProgressClockLowerLeftCornerCharSeconds
        {
            get
            {
                return _progressClockLowerLeftCornerCharSeconds;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╚";
                _progressClockLowerLeftCornerCharSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Lower right corner character for hours bar
        /// </summary>
        public static string ProgressClockLowerRightCornerCharHours
        {
            get
            {
                return _progressClockLowerRightCornerCharHours;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╝";
                _progressClockLowerRightCornerCharHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Lower right corner character for minutes bar
        /// </summary>
        public static string ProgressClockLowerRightCornerCharMinutes
        {
            get
            {
                return _progressClockLowerRightCornerCharMinutes;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╝";
                _progressClockLowerRightCornerCharMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Lower right corner character for seconds bar
        /// </summary>
        public static string ProgressClockLowerRightCornerCharSeconds
        {
            get
            {
                return _progressClockLowerRightCornerCharSeconds;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╝";
                _progressClockLowerRightCornerCharSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Upper frame character for hours bar
        /// </summary>
        public static string ProgressClockUpperFrameCharHours
        {
            get
            {
                return _progressClockUpperFrameCharHours;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "═";
                _progressClockUpperFrameCharHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Upper frame character for minutes bar
        /// </summary>
        public static string ProgressClockUpperFrameCharMinutes
        {
            get
            {
                return _progressClockUpperFrameCharMinutes;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "═";
                _progressClockUpperFrameCharMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Upper frame character for seconds bar
        /// </summary>
        public static string ProgressClockUpperFrameCharSeconds
        {
            get
            {
                return _progressClockUpperFrameCharSeconds;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "═";
                _progressClockUpperFrameCharSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Lower frame character for hours bar
        /// </summary>
        public static string ProgressClockLowerFrameCharHours
        {
            get
            {
                return _progressClockLowerFrameCharHours;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "═";
                _progressClockLowerFrameCharHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Lower frame character for minutes bar
        /// </summary>
        public static string ProgressClockLowerFrameCharMinutes
        {
            get
            {
                return _progressClockLowerFrameCharMinutes;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "═";
                _progressClockLowerFrameCharMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Lower frame character for seconds bar
        /// </summary>
        public static string ProgressClockLowerFrameCharSeconds
        {
            get
            {
                return _progressClockLowerFrameCharSeconds;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "═";
                _progressClockLowerFrameCharSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Left frame character for hours bar
        /// </summary>
        public static string ProgressClockLeftFrameCharHours
        {
            get
            {
                return _progressClockLeftFrameCharHours;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "║";
                _progressClockLeftFrameCharHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Left frame character for minutes bar
        /// </summary>
        public static string ProgressClockLeftFrameCharMinutes
        {
            get
            {
                return _progressClockLeftFrameCharMinutes;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "║";
                _progressClockLeftFrameCharMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Left frame character for seconds bar
        /// </summary>
        public static string ProgressClockLeftFrameCharSeconds
        {
            get
            {
                return _progressClockLeftFrameCharSeconds;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "║";
                _progressClockLeftFrameCharSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Right frame character for hours bar
        /// </summary>
        public static string ProgressClockRightFrameCharHours
        {
            get
            {
                return _progressClockRightFrameCharHours;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "║";
                _progressClockRightFrameCharHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Right frame character for minutes bar
        /// </summary>
        public static string ProgressClockRightFrameCharMinutes
        {
            get
            {
                return _progressClockRightFrameCharMinutes;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "║";
                _progressClockRightFrameCharMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Right frame character for seconds bar
        /// </summary>
        public static string ProgressClockRightFrameCharSeconds
        {
            get
            {
                return _progressClockRightFrameCharSeconds;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "║";
                _progressClockRightFrameCharSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Information text for hours bar
        /// </summary>
        public static string ProgressClockInfoTextHours
        {
            get
            {
                return _progressClockInfoTextHours;
            }
            set
            {
                _progressClockInfoTextHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Information text for minutes bar
        /// </summary>
        public static string ProgressClockInfoTextMinutes
        {
            get
            {
                return _progressClockInfoTextMinutes;
            }
            set
            {
                _progressClockInfoTextMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Information text for seconds bar
        /// </summary>
        public static string ProgressClockInfoTextSeconds
        {
            get
            {
                return _progressClockInfoTextSeconds;
            }
            set
            {
                _progressClockInfoTextSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum red color level (true color - hours)
        /// </summary>
        public static int ProgressClockMinimumRedColorLevelHours
        {
            get
            {
                return _progressClockMinimumRedColorLevelHours;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _progressClockMinimumRedColorLevelHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum green color level (true color - hours)
        /// </summary>
        public static int ProgressClockMinimumGreenColorLevelHours
        {
            get
            {
                return _progressClockMinimumGreenColorLevelHours;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _progressClockMinimumGreenColorLevelHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum blue color level (true color - hours)
        /// </summary>
        public static int ProgressClockMinimumBlueColorLevelHours
        {
            get
            {
                return _progressClockMinimumBlueColorLevelHours;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _progressClockMinimumBlueColorLevelHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum color level (255 colors or 16 colors - hours)
        /// </summary>
        public static int ProgressClockMinimumColorLevelHours
        {
            get
            {
                return _progressClockMinimumColorLevelHours;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                _progressClockMinimumColorLevelHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum red color level (true color - hours)
        /// </summary>
        public static int ProgressClockMaximumRedColorLevelHours
        {
            get
            {
                return _progressClockMaximumRedColorLevelHours;
            }
            set
            {
                if (value <= _progressClockMinimumRedColorLevelHours)
                    value = _progressClockMinimumRedColorLevelHours;
                if (value > 255)
                    value = 255;
                _progressClockMaximumRedColorLevelHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum green color level (true color - hours)
        /// </summary>
        public static int ProgressClockMaximumGreenColorLevelHours
        {
            get
            {
                return _progressClockMaximumGreenColorLevelHours;
            }
            set
            {
                if (value <= _progressClockMinimumGreenColorLevelHours)
                    value = _progressClockMinimumGreenColorLevelHours;
                if (value > 255)
                    value = 255;
                _progressClockMaximumGreenColorLevelHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum blue color level (true color - hours)
        /// </summary>
        public static int ProgressClockMaximumBlueColorLevelHours
        {
            get
            {
                return _progressClockMaximumBlueColorLevelHours;
            }
            set
            {
                if (value <= _progressClockMinimumBlueColorLevelHours)
                    value = _progressClockMinimumBlueColorLevelHours;
                if (value > 255)
                    value = 255;
                _progressClockMaximumBlueColorLevelHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum color level (255 colors or 16 colors - hours)
        /// </summary>
        public static int ProgressClockMaximumColorLevelHours
        {
            get
            {
                return _progressClockMaximumColorLevelHours;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= _progressClockMinimumColorLevelHours)
                    value = _progressClockMinimumColorLevelHours;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                _progressClockMaximumColorLevelHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum red color level (true color - minutes)
        /// </summary>
        public static int ProgressClockMinimumRedColorLevelMinutes
        {
            get
            {
                return _progressClockMinimumRedColorLevelMinutes;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _progressClockMinimumRedColorLevelMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum green color level (true color - minutes)
        /// </summary>
        public static int ProgressClockMinimumGreenColorLevelMinutes
        {
            get
            {
                return _progressClockMinimumGreenColorLevelMinutes;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _progressClockMinimumGreenColorLevelMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum blue color level (true color - minutes)
        /// </summary>
        public static int ProgressClockMinimumBlueColorLevelMinutes
        {
            get
            {
                return _progressClockMinimumBlueColorLevelMinutes;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _progressClockMinimumBlueColorLevelMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum color level (255 colors or 16 colors - minutes)
        /// </summary>
        public static int ProgressClockMinimumColorLevelMinutes
        {
            get
            {
                return _progressClockMinimumColorLevelMinutes;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                _progressClockMinimumColorLevelMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum red color level (true color - minutes)
        /// </summary>
        public static int ProgressClockMaximumRedColorLevelMinutes
        {
            get
            {
                return _progressClockMaximumRedColorLevelMinutes;
            }
            set
            {
                if (value <= _progressClockMinimumRedColorLevelMinutes)
                    value = _progressClockMinimumRedColorLevelMinutes;
                if (value > 255)
                    value = 255;
                _progressClockMaximumRedColorLevelMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum green color level (true color - minutes)
        /// </summary>
        public static int ProgressClockMaximumGreenColorLevelMinutes
        {
            get
            {
                return _progressClockMaximumGreenColorLevelMinutes;
            }
            set
            {
                if (value <= _progressClockMinimumGreenColorLevelMinutes)
                    value = _progressClockMinimumGreenColorLevelMinutes;
                if (value > 255)
                    value = 255;
                _progressClockMaximumGreenColorLevelMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum blue color level (true color - minutes)
        /// </summary>
        public static int ProgressClockMaximumBlueColorLevelMinutes
        {
            get
            {
                return _progressClockMaximumBlueColorLevelMinutes;
            }
            set
            {
                if (value <= _progressClockMinimumBlueColorLevelMinutes)
                    value = _progressClockMinimumBlueColorLevelMinutes;
                if (value > 255)
                    value = 255;
                _progressClockMaximumBlueColorLevelMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum color level (255 colors or 16 colors - minutes)
        /// </summary>
        public static int ProgressClockMaximumColorLevelMinutes
        {
            get
            {
                return _progressClockMaximumColorLevelMinutes;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= _progressClockMinimumColorLevelMinutes)
                    value = _progressClockMinimumColorLevelMinutes;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                _progressClockMaximumColorLevelMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum red color level (true color - seconds)
        /// </summary>
        public static int ProgressClockMinimumRedColorLevelSeconds
        {
            get
            {
                return _progressClockMinimumRedColorLevelSeconds;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _progressClockMinimumRedColorLevelSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum green color level (true color - seconds)
        /// </summary>
        public static int ProgressClockMinimumGreenColorLevelSeconds
        {
            get
            {
                return _progressClockMinimumGreenColorLevelSeconds;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _progressClockMinimumGreenColorLevelSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum blue color level (true color - seconds)
        /// </summary>
        public static int ProgressClockMinimumBlueColorLevelSeconds
        {
            get
            {
                return _progressClockMinimumBlueColorLevelSeconds;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _progressClockMinimumBlueColorLevelSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum color level (255 colors or 16 colors - seconds)
        /// </summary>
        public static int ProgressClockMinimumColorLevelSeconds
        {
            get
            {
                return _progressClockMinimumColorLevelSeconds;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                _progressClockMinimumColorLevelSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum red color level (true color - seconds)
        /// </summary>
        public static int ProgressClockMaximumRedColorLevelSeconds
        {
            get
            {
                return _progressClockMaximumRedColorLevelSeconds;
            }
            set
            {
                if (value <= _progressClockMinimumRedColorLevelSeconds)
                    value = _progressClockMinimumRedColorLevelSeconds;
                if (value > 255)
                    value = 255;
                _progressClockMaximumRedColorLevelSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum green color level (true color - seconds)
        /// </summary>
        public static int ProgressClockMaximumGreenColorLevelSeconds
        {
            get
            {
                return _progressClockMaximumGreenColorLevelSeconds;
            }
            set
            {
                if (value <= _progressClockMinimumGreenColorLevelSeconds)
                    value = _progressClockMinimumGreenColorLevelSeconds;
                if (value > 255)
                    value = 255;
                _progressClockMaximumGreenColorLevelSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum blue color level (true color - seconds)
        /// </summary>
        public static int ProgressClockMaximumBlueColorLevelSeconds
        {
            get
            {
                return _progressClockMaximumBlueColorLevelSeconds;
            }
            set
            {
                if (value <= _progressClockMinimumBlueColorLevelSeconds)
                    value = _progressClockMinimumBlueColorLevelSeconds;
                if (value > 255)
                    value = 255;
                _progressClockMaximumBlueColorLevelSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum color level (255 colors or 16 colors - seconds)
        /// </summary>
        public static int ProgressClockMaximumColorLevelSeconds
        {
            get
            {
                return _progressClockMaximumColorLevelSeconds;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= _progressClockMinimumColorLevelSeconds)
                    value = _progressClockMinimumColorLevelSeconds;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                _progressClockMaximumColorLevelSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum red color level (true color)
        /// </summary>
        public static int ProgressClockMinimumRedColorLevel
        {
            get
            {
                return _progressClockMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _progressClockMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum green color level (true color)
        /// </summary>
        public static int ProgressClockMinimumGreenColorLevel
        {
            get
            {
                return _progressClockMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _progressClockMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum blue color level (true color)
        /// </summary>
        public static int ProgressClockMinimumBlueColorLevel
        {
            get
            {
                return _progressClockMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _progressClockMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int ProgressClockMinimumColorLevel
        {
            get
            {
                return _progressClockMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                _progressClockMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum red color level (true color)
        /// </summary>
        public static int ProgressClockMaximumRedColorLevel
        {
            get
            {
                return _progressClockMaximumRedColorLevel;
            }
            set
            {
                if (value <= _progressClockMinimumRedColorLevel)
                    value = _progressClockMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                _progressClockMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum green color level (true color)
        /// </summary>
        public static int ProgressClockMaximumGreenColorLevel
        {
            get
            {
                return _progressClockMaximumGreenColorLevel;
            }
            set
            {
                if (value <= _progressClockMinimumGreenColorLevel)
                    value = _progressClockMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                _progressClockMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum blue color level (true color)
        /// </summary>
        public static int ProgressClockMaximumBlueColorLevel
        {
            get
            {
                return _progressClockMaximumBlueColorLevel;
            }
            set
            {
                if (value <= _progressClockMinimumBlueColorLevel)
                    value = _progressClockMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                _progressClockMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int ProgressClockMaximumColorLevel
        {
            get
            {
                return _progressClockMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= _progressClockMinimumColorLevel)
                    value = _progressClockMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                _progressClockMaximumColorLevel = value;
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

            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Current tick: {0}", CurrentTicks);
            if (ProgressClockSettings.ProgressClockCycleColors)
            {
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Cycling colors...");
                if (CurrentTicks >= ProgressClockSettings.ProgressClockCycleColorsTicks)
                {
                    if (ProgressClockSettings.ProgressClockTrueColor)
                    {
                        DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Current tick equals the maximum ticks to change color.");
                        RedColorNumHours = RandomDriver.Random(ProgressClockSettings.ProgressClockMinimumRedColorLevelHours, ProgressClockSettings.ProgressClockMaximumRedColorLevelHours);
                        GreenColorNumHours = RandomDriver.Random(ProgressClockSettings.ProgressClockMinimumGreenColorLevelHours, ProgressClockSettings.ProgressClockMaximumGreenColorLevelHours);
                        BlueColorNumHours = RandomDriver.Random(ProgressClockSettings.ProgressClockMinimumBlueColorLevelHours, ProgressClockSettings.ProgressClockMaximumBlueColorLevelHours);
                        DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (Hours) (R;G;B: {0};{1};{2})", RedColorNumHours, GreenColorNumHours, BlueColorNumHours);
                        RedColorNumMinutes = RandomDriver.Random(ProgressClockSettings.ProgressClockMinimumRedColorLevelMinutes, ProgressClockSettings.ProgressClockMaximumRedColorLevelMinutes);
                        GreenColorNumMinutes = RandomDriver.Random(ProgressClockSettings.ProgressClockMinimumGreenColorLevelMinutes, ProgressClockSettings.ProgressClockMaximumGreenColorLevelMinutes);
                        BlueColorNumMinutes = RandomDriver.Random(ProgressClockSettings.ProgressClockMinimumBlueColorLevelMinutes, ProgressClockSettings.ProgressClockMaximumBlueColorLevelMinutes);
                        DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (Minutes) (R;G;B: {0};{1};{2})", RedColorNumMinutes, GreenColorNumMinutes, BlueColorNumMinutes);
                        RedColorNumSeconds = RandomDriver.Random(ProgressClockSettings.ProgressClockMinimumRedColorLevelSeconds, ProgressClockSettings.ProgressClockMaximumRedColorLevelSeconds);
                        GreenColorNumSeconds = RandomDriver.Random(ProgressClockSettings.ProgressClockMinimumGreenColorLevelSeconds, ProgressClockSettings.ProgressClockMaximumGreenColorLevelSeconds);
                        BlueColorNumSeconds = RandomDriver.Random(ProgressClockSettings.ProgressClockMinimumBlueColorLevelSeconds, ProgressClockSettings.ProgressClockMaximumBlueColorLevelSeconds);
                        DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (Seconds) (R;G;B: {0};{1};{2})", RedColorNumSeconds, GreenColorNumSeconds, BlueColorNumSeconds);
                        RedColorNum = RandomDriver.Random(ProgressClockSettings.ProgressClockMinimumRedColorLevel, ProgressClockSettings.ProgressClockMaximumRedColorLevel);
                        GreenColorNum = RandomDriver.Random(ProgressClockSettings.ProgressClockMinimumGreenColorLevel, ProgressClockSettings.ProgressClockMaximumGreenColorLevel);
                        BlueColorNum = RandomDriver.Random(ProgressClockSettings.ProgressClockMinimumBlueColorLevel, ProgressClockSettings.ProgressClockMaximumBlueColorLevel);
                        DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                        ColorStorageHours = new Color(RedColorNumHours, GreenColorNumHours, BlueColorNumHours);
                        ColorStorageMinutes = new Color(RedColorNumMinutes, GreenColorNumMinutes, BlueColorNumMinutes);
                        ColorStorageSeconds = new Color(RedColorNumSeconds, GreenColorNumSeconds, BlueColorNumSeconds);
                        ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                    }
                    else
                    {
                        ColorNumHours = RandomDriver.Random(ProgressClockSettings.ProgressClockMinimumColorLevelHours, ProgressClockSettings.ProgressClockMaximumColorLevelHours);
                        DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (Hours) ({0})", ColorNumHours);
                        ColorNumMinutes = RandomDriver.Random(ProgressClockSettings.ProgressClockMinimumColorLevelMinutes, ProgressClockSettings.ProgressClockMaximumColorLevelMinutes);
                        DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (Minutes) ({0})", ColorNumMinutes);
                        ColorNumSeconds = RandomDriver.Random(ProgressClockSettings.ProgressClockMinimumColorLevelSeconds, ProgressClockSettings.ProgressClockMaximumColorLevelSeconds);
                        DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (Seconds) ({0})", ColorNumSeconds);
                        ColorNum = RandomDriver.Random(ProgressClockSettings.ProgressClockMinimumColorLevel, ProgressClockSettings.ProgressClockMaximumColorLevel);
                        DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
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
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Parsing colors...");
                ColorStorageHours = new Color(ProgressClockSettings.ProgressClockHoursProgressColor);
                ColorStorageMinutes = new Color(ProgressClockSettings.ProgressClockMinutesProgressColor);
                ColorStorageSeconds = new Color(ProgressClockSettings.ProgressClockSecondsProgressColor);
                ColorStorage = new Color(ProgressClockSettings.ProgressClockProgressColor);
            }
            ProgressFillPositionHours = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) - 10;
            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Fill position for progress (Hours) {0}", ProgressFillPositionHours);
            ProgressFillPositionMinutes = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) - 1;
            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Fill position for progress (Minutes) {0}", ProgressFillPositionMinutes);
            ProgressFillPositionSeconds = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) + 8;
            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Fill position for progress (Seconds) {0}", ProgressFillPositionSeconds);
            InformationPositionHours = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) - 12;
            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Fill position for info (Hours) {0}", InformationPositionHours);
            InformationPositionMinutes = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) - 3;
            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Fill position for info (Minutes) {0}", InformationPositionMinutes);
            InformationPositionSeconds = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) + 6;
            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Fill position for info (Seconds) {0}", InformationPositionSeconds);

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
                if (!(TimeDate.TimeDate.KernelDateTime.Hour == 0))
                    TextWriterWhereColor.WriteWhere(" ".Repeat(ConsoleExtensions.PercentRepeat(TimeDate.TimeDate.KernelDateTime.Hour, 24, 10)), 5, ProgressFillPositionHours, true, Color.Empty, ColorStorageHours);
                if (!(TimeDate.TimeDate.KernelDateTime.Minute == 0))
                    TextWriterWhereColor.WriteWhere(" ".Repeat(ConsoleExtensions.PercentRepeat(TimeDate.TimeDate.KernelDateTime.Minute, 60, 10)), 5, ProgressFillPositionMinutes, true, Color.Empty, ColorStorageMinutes);
                if (!(TimeDate.TimeDate.KernelDateTime.Second == 0))
                    TextWriterWhereColor.WriteWhere(" ".Repeat(ConsoleExtensions.PercentRepeat(TimeDate.TimeDate.KernelDateTime.Second, 60, 10)), 5, ProgressFillPositionSeconds, true, Color.Empty, ColorStorageSeconds);

                // Print information
                if (!string.IsNullOrEmpty(ProgressClockSettings.ProgressClockInfoTextHours))
                {
                    TextWriterWhereColor.WriteWhere(PlaceParse.ProbePlaces(ProgressClockSettings.ProgressClockInfoTextHours), 4, InformationPositionHours, true, ColorStorageHours, TimeDate.TimeDate.KernelDateTime.Hour);
                }
                else
                {
                    TextWriterWhereColor.WriteWhere("H: {0}/24", 4, InformationPositionHours, true, ColorStorageHours, TimeDate.TimeDate.KernelDateTime.Hour);
                }
                if (!string.IsNullOrEmpty(ProgressClockSettings.ProgressClockInfoTextMinutes))
                {
                    TextWriterWhereColor.WriteWhere(PlaceParse.ProbePlaces(ProgressClockSettings.ProgressClockInfoTextMinutes), 4, InformationPositionMinutes, true, ColorStorageMinutes, TimeDate.TimeDate.KernelDateTime.Minute);
                }
                else
                {
                    TextWriterWhereColor.WriteWhere("M: {0}/60", 4, InformationPositionMinutes, true, ColorStorageMinutes, TimeDate.TimeDate.KernelDateTime.Minute);
                }
                if (!string.IsNullOrEmpty(ProgressClockSettings.ProgressClockInfoTextHours))
                {
                    TextWriterWhereColor.WriteWhere(PlaceParse.ProbePlaces(ProgressClockSettings.ProgressClockInfoTextSeconds), 4, InformationPositionSeconds, true, ColorStorageSeconds, TimeDate.TimeDate.KernelDateTime.Second);
                }
                else
                {
                    TextWriterWhereColor.WriteWhere("S: {0}/60", 4, InformationPositionSeconds, true, ColorStorageSeconds, TimeDate.TimeDate.KernelDateTime.Second);
                }

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
