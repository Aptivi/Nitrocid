
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

        private static bool _TrueColor = true;
        private static bool _CycleColors = true;
        private static string _SecondsProgressColor = 4.ToString();
        private static string _MinutesProgressColor = 5.ToString();
        private static string _HoursProgressColor = 6.ToString();
        private static string _ProgressColor = 7.ToString();
        private static long _CycleColorsTicks = 20L;
        private static int _Delay = 500;
        private static string _UpperLeftCornerCharHours = "╔";
        private static string _UpperLeftCornerCharMinutes = "╔";
        private static string _UpperLeftCornerCharSeconds = "╔";
        private static string _UpperRightCornerCharHours = "╗";
        private static string _UpperRightCornerCharMinutes = "╗";
        private static string _UpperRightCornerCharSeconds = "╗";
        private static string _LowerLeftCornerCharHours = "╚";
        private static string _LowerLeftCornerCharMinutes = "╚";
        private static string _LowerLeftCornerCharSeconds = "╚";
        private static string _LowerRightCornerCharHours = "╝";
        private static string _LowerRightCornerCharMinutes = "╝";
        private static string _LowerRightCornerCharSeconds = "╝";
        private static string _UpperFrameCharHours = "═";
        private static string _UpperFrameCharMinutes = "═";
        private static string _UpperFrameCharSeconds = "═";
        private static string _LowerFrameCharHours = "═";
        private static string _LowerFrameCharMinutes = "═";
        private static string _LowerFrameCharSeconds = "═";
        private static string _LeftFrameCharHours = "║";
        private static string _LeftFrameCharMinutes = "║";
        private static string _LeftFrameCharSeconds = "║";
        private static string _RightFrameCharHours = "║";
        private static string _RightFrameCharMinutes = "║";
        private static string _RightFrameCharSeconds = "║";
        private static string _InfoTextHours = "";
        private static string _InfoTextMinutes = "";
        private static string _InfoTextSeconds = "";
        private static int _MinimumRedColorLevelHours = 0;
        private static int _MinimumGreenColorLevelHours = 0;
        private static int _MinimumBlueColorLevelHours = 0;
        private static int _MinimumColorLevelHours = 0;
        private static int _MaximumRedColorLevelHours = 255;
        private static int _MaximumGreenColorLevelHours = 255;
        private static int _MaximumBlueColorLevelHours = 255;
        private static int _MaximumColorLevelHours = 255;
        private static int _MinimumRedColorLevelMinutes = 0;
        private static int _MinimumGreenColorLevelMinutes = 0;
        private static int _MinimumBlueColorLevelMinutes = 0;
        private static int _MinimumColorLevelMinutes = 0;
        private static int _MaximumRedColorLevelMinutes = 255;
        private static int _MaximumGreenColorLevelMinutes = 255;
        private static int _MaximumBlueColorLevelMinutes = 255;
        private static int _MaximumColorLevelMinutes = 255;
        private static int _MinimumRedColorLevelSeconds = 0;
        private static int _MinimumGreenColorLevelSeconds = 0;
        private static int _MinimumBlueColorLevelSeconds = 0;
        private static int _MinimumColorLevelSeconds = 0;
        private static int _MaximumRedColorLevelSeconds = 255;
        private static int _MaximumGreenColorLevelSeconds = 255;
        private static int _MaximumBlueColorLevelSeconds = 255;
        private static int _MaximumColorLevelSeconds = 255;
        private static int _MinimumRedColorLevel = 0;
        private static int _MinimumGreenColorLevel = 0;
        private static int _MinimumBlueColorLevel = 0;
        private static int _MinimumColorLevel = 0;
        private static int _MaximumRedColorLevel = 255;
        private static int _MaximumGreenColorLevel = 255;
        private static int _MaximumBlueColorLevel = 255;
        private static int _MaximumColorLevel = 255;

        /// <summary>
        /// [ProgressClock] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool ProgressClockTrueColor
        {
            get
            {
                return _TrueColor;
            }
            set
            {
                _TrueColor = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Enable color cycling (uses RNG. If disabled, uses the <see cref="ProgressClockSecondsProgressColor"/>, <see cref="ProgressClockMinutesProgressColor"/>, and <see cref="ProgressClockHoursProgressColor"/> colors.)
        /// </summary>
        public static bool ProgressClockCycleColors
        {
            get
            {
                return _CycleColors;
            }
            set
            {
                _CycleColors = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The color of seconds progress bar. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public static string ProgressClockSecondsProgressColor
        {
            get
            {
                return _SecondsProgressColor;
            }
            set
            {
                _SecondsProgressColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [ProgressClock] The color of minutes progress bar. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public static string ProgressClockMinutesProgressColor
        {
            get
            {
                return _MinutesProgressColor;
            }
            set
            {
                _MinutesProgressColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [ProgressClock] The color of hours progress bar. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public static string ProgressClockHoursProgressColor
        {
            get
            {
                return _HoursProgressColor;
            }
            set
            {
                _HoursProgressColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [ProgressClock] The color of date information. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public static string ProgressClockProgressColor
        {
            get
            {
                return _ProgressColor;
            }
            set
            {
                _ProgressColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [ProgressClock] If color cycling is enabled, how many ticks before changing colors? 1 tick = 0.5 seconds
        /// </summary>
        public static long ProgressClockCycleColorsTicks
        {
            get
            {
                return _CycleColorsTicks;
            }
            set
            {
                if (value <= 0L)
                    value = 20L;
                _CycleColorsTicks = value;
            }
        }
        /// <summary>
        /// [ProgressClock] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int ProgressClockDelay
        {
            get
            {
                return _Delay;
            }
            set
            {
                if (value <= 0)
                    value = 500;
                _Delay = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Upper left corner character for hours bar
        /// </summary>
        public static string ProgressClockUpperLeftCornerCharHours
        {
            get
            {
                return _UpperLeftCornerCharHours;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╔";
                _UpperLeftCornerCharHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Upper left corner character for minutes bar
        /// </summary>
        public static string ProgressClockUpperLeftCornerCharMinutes
        {
            get
            {
                return _UpperLeftCornerCharMinutes;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╔";
                _UpperLeftCornerCharMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Upper left corner character for seconds bar
        /// </summary>
        public static string ProgressClockUpperLeftCornerCharSeconds
        {
            get
            {
                return _UpperLeftCornerCharSeconds;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╔";
                _UpperLeftCornerCharSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Upper right corner character for hours bar
        /// </summary>
        public static string ProgressClockUpperRightCornerCharHours
        {
            get
            {
                return _UpperRightCornerCharHours;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╗";
                _UpperRightCornerCharHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Upper right corner character for minutes bar
        /// </summary>
        public static string ProgressClockUpperRightCornerCharMinutes
        {
            get
            {
                return _UpperRightCornerCharMinutes;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╗";
                _UpperRightCornerCharMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Upper right corner character for seconds bar
        /// </summary>
        public static string ProgressClockUpperRightCornerCharSeconds
        {
            get
            {
                return _UpperRightCornerCharSeconds;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╗";
                _UpperRightCornerCharSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Lower left corner character for hours bar
        /// </summary>
        public static string ProgressClockLowerLeftCornerCharHours
        {
            get
            {
                return _LowerLeftCornerCharHours;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╚";
                _LowerLeftCornerCharHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Lower left corner character for minutes bar
        /// </summary>
        public static string ProgressClockLowerLeftCornerCharMinutes
        {
            get
            {
                return _LowerLeftCornerCharMinutes;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╚";
                _LowerLeftCornerCharMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Lower left corner character for seconds bar
        /// </summary>
        public static string ProgressClockLowerLeftCornerCharSeconds
        {
            get
            {
                return _LowerLeftCornerCharSeconds;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╚";
                _LowerLeftCornerCharSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Lower right corner character for hours bar
        /// </summary>
        public static string ProgressClockLowerRightCornerCharHours
        {
            get
            {
                return _LowerRightCornerCharHours;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╝";
                _LowerRightCornerCharHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Lower right corner character for minutes bar
        /// </summary>
        public static string ProgressClockLowerRightCornerCharMinutes
        {
            get
            {
                return _LowerRightCornerCharMinutes;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╝";
                _LowerRightCornerCharMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Lower right corner character for seconds bar
        /// </summary>
        public static string ProgressClockLowerRightCornerCharSeconds
        {
            get
            {
                return _LowerRightCornerCharSeconds;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╝";
                _LowerRightCornerCharSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Upper frame character for hours bar
        /// </summary>
        public static string ProgressClockUpperFrameCharHours
        {
            get
            {
                return _UpperFrameCharHours;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "═";
                _UpperFrameCharHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Upper frame character for minutes bar
        /// </summary>
        public static string ProgressClockUpperFrameCharMinutes
        {
            get
            {
                return _UpperFrameCharMinutes;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "═";
                _UpperFrameCharMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Upper frame character for seconds bar
        /// </summary>
        public static string ProgressClockUpperFrameCharSeconds
        {
            get
            {
                return _UpperFrameCharSeconds;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "═";
                _UpperFrameCharSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Lower frame character for hours bar
        /// </summary>
        public static string ProgressClockLowerFrameCharHours
        {
            get
            {
                return _LowerFrameCharHours;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "═";
                _LowerFrameCharHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Lower frame character for minutes bar
        /// </summary>
        public static string ProgressClockLowerFrameCharMinutes
        {
            get
            {
                return _LowerFrameCharMinutes;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "═";
                _LowerFrameCharMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Lower frame character for seconds bar
        /// </summary>
        public static string ProgressClockLowerFrameCharSeconds
        {
            get
            {
                return _LowerFrameCharSeconds;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "═";
                _LowerFrameCharSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Left frame character for hours bar
        /// </summary>
        public static string ProgressClockLeftFrameCharHours
        {
            get
            {
                return _LeftFrameCharHours;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "║";
                _LeftFrameCharHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Left frame character for minutes bar
        /// </summary>
        public static string ProgressClockLeftFrameCharMinutes
        {
            get
            {
                return _LeftFrameCharMinutes;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "║";
                _LeftFrameCharMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Left frame character for seconds bar
        /// </summary>
        public static string ProgressClockLeftFrameCharSeconds
        {
            get
            {
                return _LeftFrameCharSeconds;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "║";
                _LeftFrameCharSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Right frame character for hours bar
        /// </summary>
        public static string ProgressClockRightFrameCharHours
        {
            get
            {
                return _RightFrameCharHours;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "║";
                _RightFrameCharHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Right frame character for minutes bar
        /// </summary>
        public static string ProgressClockRightFrameCharMinutes
        {
            get
            {
                return _RightFrameCharMinutes;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "║";
                _RightFrameCharMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Right frame character for seconds bar
        /// </summary>
        public static string ProgressClockRightFrameCharSeconds
        {
            get
            {
                return _RightFrameCharSeconds;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "║";
                _RightFrameCharSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Information text for hours bar
        /// </summary>
        public static string ProgressClockInfoTextHours
        {
            get
            {
                return _InfoTextHours;
            }
            set
            {
                _InfoTextHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Information text for minutes bar
        /// </summary>
        public static string ProgressClockInfoTextMinutes
        {
            get
            {
                return _InfoTextMinutes;
            }
            set
            {
                _InfoTextMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Information text for seconds bar
        /// </summary>
        public static string ProgressClockInfoTextSeconds
        {
            get
            {
                return _InfoTextSeconds;
            }
            set
            {
                _InfoTextSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum red color level (true color - hours)
        /// </summary>
        public static int ProgressClockMinimumRedColorLevelHours
        {
            get
            {
                return _MinimumRedColorLevelHours;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _MinimumRedColorLevelHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum green color level (true color - hours)
        /// </summary>
        public static int ProgressClockMinimumGreenColorLevelHours
        {
            get
            {
                return _MinimumGreenColorLevelHours;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _MinimumGreenColorLevelHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum blue color level (true color - hours)
        /// </summary>
        public static int ProgressClockMinimumBlueColorLevelHours
        {
            get
            {
                return _MinimumBlueColorLevelHours;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _MinimumBlueColorLevelHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum color level (255 colors or 16 colors - hours)
        /// </summary>
        public static int ProgressClockMinimumColorLevelHours
        {
            get
            {
                return _MinimumColorLevelHours;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                _MinimumColorLevelHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum red color level (true color - hours)
        /// </summary>
        public static int ProgressClockMaximumRedColorLevelHours
        {
            get
            {
                return _MaximumRedColorLevelHours;
            }
            set
            {
                if (value <= _MinimumRedColorLevelHours)
                    value = _MinimumRedColorLevelHours;
                if (value > 255)
                    value = 255;
                _MaximumRedColorLevelHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum green color level (true color - hours)
        /// </summary>
        public static int ProgressClockMaximumGreenColorLevelHours
        {
            get
            {
                return _MaximumGreenColorLevelHours;
            }
            set
            {
                if (value <= _MinimumGreenColorLevelHours)
                    value = _MinimumGreenColorLevelHours;
                if (value > 255)
                    value = 255;
                _MaximumGreenColorLevelHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum blue color level (true color - hours)
        /// </summary>
        public static int ProgressClockMaximumBlueColorLevelHours
        {
            get
            {
                return _MaximumBlueColorLevelHours;
            }
            set
            {
                if (value <= _MinimumBlueColorLevelHours)
                    value = _MinimumBlueColorLevelHours;
                if (value > 255)
                    value = 255;
                _MaximumBlueColorLevelHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum color level (255 colors or 16 colors - hours)
        /// </summary>
        public static int ProgressClockMaximumColorLevelHours
        {
            get
            {
                return _MaximumColorLevelHours;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= _MinimumColorLevelHours)
                    value = _MinimumColorLevelHours;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                _MaximumColorLevelHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum red color level (true color - minutes)
        /// </summary>
        public static int ProgressClockMinimumRedColorLevelMinutes
        {
            get
            {
                return _MinimumRedColorLevelMinutes;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _MinimumRedColorLevelMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum green color level (true color - minutes)
        /// </summary>
        public static int ProgressClockMinimumGreenColorLevelMinutes
        {
            get
            {
                return _MinimumGreenColorLevelMinutes;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _MinimumGreenColorLevelMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum blue color level (true color - minutes)
        /// </summary>
        public static int ProgressClockMinimumBlueColorLevelMinutes
        {
            get
            {
                return _MinimumBlueColorLevelMinutes;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _MinimumBlueColorLevelMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum color level (255 colors or 16 colors - minutes)
        /// </summary>
        public static int ProgressClockMinimumColorLevelMinutes
        {
            get
            {
                return _MinimumColorLevelMinutes;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                _MinimumColorLevelMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum red color level (true color - minutes)
        /// </summary>
        public static int ProgressClockMaximumRedColorLevelMinutes
        {
            get
            {
                return _MaximumRedColorLevelMinutes;
            }
            set
            {
                if (value <= _MinimumRedColorLevelMinutes)
                    value = _MinimumRedColorLevelMinutes;
                if (value > 255)
                    value = 255;
                _MaximumRedColorLevelMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum green color level (true color - minutes)
        /// </summary>
        public static int ProgressClockMaximumGreenColorLevelMinutes
        {
            get
            {
                return _MaximumGreenColorLevelMinutes;
            }
            set
            {
                if (value <= _MinimumGreenColorLevelMinutes)
                    value = _MinimumGreenColorLevelMinutes;
                if (value > 255)
                    value = 255;
                _MaximumGreenColorLevelMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum blue color level (true color - minutes)
        /// </summary>
        public static int ProgressClockMaximumBlueColorLevelMinutes
        {
            get
            {
                return _MaximumBlueColorLevelMinutes;
            }
            set
            {
                if (value <= _MinimumBlueColorLevelMinutes)
                    value = _MinimumBlueColorLevelMinutes;
                if (value > 255)
                    value = 255;
                _MaximumBlueColorLevelMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum color level (255 colors or 16 colors - minutes)
        /// </summary>
        public static int ProgressClockMaximumColorLevelMinutes
        {
            get
            {
                return _MaximumColorLevelMinutes;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= _MinimumColorLevelMinutes)
                    value = _MinimumColorLevelMinutes;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                _MaximumColorLevelMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum red color level (true color - seconds)
        /// </summary>
        public static int ProgressClockMinimumRedColorLevelSeconds
        {
            get
            {
                return _MinimumRedColorLevelSeconds;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _MinimumRedColorLevelSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum green color level (true color - seconds)
        /// </summary>
        public static int ProgressClockMinimumGreenColorLevelSeconds
        {
            get
            {
                return _MinimumGreenColorLevelSeconds;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _MinimumGreenColorLevelSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum blue color level (true color - seconds)
        /// </summary>
        public static int ProgressClockMinimumBlueColorLevelSeconds
        {
            get
            {
                return _MinimumBlueColorLevelSeconds;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _MinimumBlueColorLevelSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum color level (255 colors or 16 colors - seconds)
        /// </summary>
        public static int ProgressClockMinimumColorLevelSeconds
        {
            get
            {
                return _MinimumColorLevelSeconds;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                _MinimumColorLevelSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum red color level (true color - seconds)
        /// </summary>
        public static int ProgressClockMaximumRedColorLevelSeconds
        {
            get
            {
                return _MaximumRedColorLevelSeconds;
            }
            set
            {
                if (value <= _MinimumRedColorLevelSeconds)
                    value = _MinimumRedColorLevelSeconds;
                if (value > 255)
                    value = 255;
                _MaximumRedColorLevelSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum green color level (true color - seconds)
        /// </summary>
        public static int ProgressClockMaximumGreenColorLevelSeconds
        {
            get
            {
                return _MaximumGreenColorLevelSeconds;
            }
            set
            {
                if (value <= _MinimumGreenColorLevelSeconds)
                    value = _MinimumGreenColorLevelSeconds;
                if (value > 255)
                    value = 255;
                _MaximumGreenColorLevelSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum blue color level (true color - seconds)
        /// </summary>
        public static int ProgressClockMaximumBlueColorLevelSeconds
        {
            get
            {
                return _MaximumBlueColorLevelSeconds;
            }
            set
            {
                if (value <= _MinimumBlueColorLevelSeconds)
                    value = _MinimumBlueColorLevelSeconds;
                if (value > 255)
                    value = 255;
                _MaximumBlueColorLevelSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum color level (255 colors or 16 colors - seconds)
        /// </summary>
        public static int ProgressClockMaximumColorLevelSeconds
        {
            get
            {
                return _MaximumColorLevelSeconds;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= _MinimumColorLevelSeconds)
                    value = _MinimumColorLevelSeconds;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                _MaximumColorLevelSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum red color level (true color)
        /// </summary>
        public static int ProgressClockMinimumRedColorLevel
        {
            get
            {
                return _MinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _MinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum green color level (true color)
        /// </summary>
        public static int ProgressClockMinimumGreenColorLevel
        {
            get
            {
                return _MinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _MinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum blue color level (true color)
        /// </summary>
        public static int ProgressClockMinimumBlueColorLevel
        {
            get
            {
                return _MinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _MinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int ProgressClockMinimumColorLevel
        {
            get
            {
                return _MinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                _MinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum red color level (true color)
        /// </summary>
        public static int ProgressClockMaximumRedColorLevel
        {
            get
            {
                return _MaximumRedColorLevel;
            }
            set
            {
                if (value <= _MinimumRedColorLevel)
                    value = _MinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                _MaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum green color level (true color)
        /// </summary>
        public static int ProgressClockMaximumGreenColorLevel
        {
            get
            {
                return _MaximumGreenColorLevel;
            }
            set
            {
                if (value <= _MinimumGreenColorLevel)
                    value = _MinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                _MaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum blue color level (true color)
        /// </summary>
        public static int ProgressClockMaximumBlueColorLevel
        {
            get
            {
                return _MaximumBlueColorLevel;
            }
            set
            {
                if (value <= _MinimumBlueColorLevel)
                    value = _MinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                _MaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int ProgressClockMaximumColorLevel
        {
            get
            {
                return _MaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= _MinimumColorLevel)
                    value = _MinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                _MaximumColorLevel = value;
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
