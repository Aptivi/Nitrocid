
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
using KS.ConsoleBase;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Misc.Probers.Placeholder;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.FancyWriters;
using KS.TimeDate;
using ColorTools = KS.ConsoleBase.Colors.ColorTools;

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
            get => Config.SaverConfig.ProgressClockTrueColor;
            set => Config.SaverConfig.ProgressClockTrueColor = value;
        }
        /// <summary>
        /// [ProgressClock] Enable color cycling (uses RNG. If disabled, uses the <see cref="ProgressClockSecondsProgressColor"/>, <see cref="ProgressClockMinutesProgressColor"/>, and <see cref="ProgressClockHoursProgressColor"/> colors.)
        /// </summary>
        public static bool ProgressClockCycleColors
        {
            get => Config.SaverConfig.ProgressClockCycleColors;
            set => Config.SaverConfig.ProgressClockCycleColors = value;
        }
        /// <summary>
        /// [ProgressClock] The color of seconds progress bar. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public static string ProgressClockSecondsProgressColor
        {
            get => Config.SaverConfig.ProgressClockSecondsProgressColor;
            set => Config.SaverConfig.ProgressClockSecondsProgressColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [ProgressClock] The color of minutes progress bar. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public static string ProgressClockMinutesProgressColor
        {
            get => Config.SaverConfig.ProgressClockMinutesProgressColor;
            set => Config.SaverConfig.ProgressClockMinutesProgressColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [ProgressClock] The color of hours progress bar. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public static string ProgressClockHoursProgressColor
        {
            get => Config.SaverConfig.ProgressClockHoursProgressColor;
            set => Config.SaverConfig.ProgressClockHoursProgressColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [ProgressClock] The color of date information. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public static string ProgressClockProgressColor
        {
            get => Config.SaverConfig.ProgressClockProgressColor;
            set => Config.SaverConfig.ProgressClockProgressColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [ProgressClock] If color cycling is enabled, how many ticks before changing colors? 1 tick = 0.5 seconds
        /// </summary>
        public static long ProgressClockCycleColorsTicks
        {
            get => Config.SaverConfig.ProgressClockCycleColorsTicks;
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
            get => Config.SaverConfig.ProgressClockDelay;
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
        public static char ProgressClockUpperLeftCornerCharHours
        {
            get => Config.SaverConfig.ProgressClockUpperLeftCornerCharHours;
            set => Config.SaverConfig.ProgressClockUpperLeftCornerCharHours = value;
        }
        /// <summary>
        /// [ProgressClock] Upper left corner character for minutes bar
        /// </summary>
        public static char ProgressClockUpperLeftCornerCharMinutes
        {
            get => Config.SaverConfig.ProgressClockUpperLeftCornerCharMinutes;
            set => Config.SaverConfig.ProgressClockUpperLeftCornerCharMinutes = value;
        }
        /// <summary>
        /// [ProgressClock] Upper left corner character for seconds bar
        /// </summary>
        public static char ProgressClockUpperLeftCornerCharSeconds
        {
            get => Config.SaverConfig.ProgressClockUpperLeftCornerCharSeconds;
            set => Config.SaverConfig.ProgressClockUpperLeftCornerCharSeconds = value;
        }
        /// <summary>
        /// [ProgressClock] Upper right corner character for hours bar
        /// </summary>
        public static char ProgressClockUpperRightCornerCharHours
        {
            get => Config.SaverConfig.ProgressClockUpperRightCornerCharHours;
            set => Config.SaverConfig.ProgressClockUpperRightCornerCharHours = value;
        }
        /// <summary>
        /// [ProgressClock] Upper right corner character for minutes bar
        /// </summary>
        public static char ProgressClockUpperRightCornerCharMinutes
        {
            get => Config.SaverConfig.ProgressClockUpperRightCornerCharMinutes;
            set => Config.SaverConfig.ProgressClockUpperRightCornerCharMinutes = value;
        }
        /// <summary>
        /// [ProgressClock] Upper right corner character for seconds bar
        /// </summary>
        public static char ProgressClockUpperRightCornerCharSeconds
        {
            get => Config.SaverConfig.ProgressClockUpperRightCornerCharSeconds;
            set => Config.SaverConfig.ProgressClockUpperRightCornerCharSeconds = value;
        }
        /// <summary>
        /// [ProgressClock] Lower left corner character for hours bar
        /// </summary>
        public static char ProgressClockLowerLeftCornerCharHours
        {
            get => Config.SaverConfig.ProgressClockLowerLeftCornerCharHours;
            set => Config.SaverConfig.ProgressClockLowerLeftCornerCharHours = value;
        }
        /// <summary>
        /// [ProgressClock] Lower left corner character for minutes bar
        /// </summary>
        public static char ProgressClockLowerLeftCornerCharMinutes
        {
            get => Config.SaverConfig.ProgressClockLowerLeftCornerCharMinutes;
            set => Config.SaverConfig.ProgressClockLowerLeftCornerCharMinutes = value;
        }
        /// <summary>
        /// [ProgressClock] Lower left corner character for seconds bar
        /// </summary>
        public static char ProgressClockLowerLeftCornerCharSeconds
        {
            get => Config.SaverConfig.ProgressClockLowerLeftCornerCharSeconds;
            set => Config.SaverConfig.ProgressClockLowerLeftCornerCharSeconds = value;
        }
        /// <summary>
        /// [ProgressClock] Lower right corner character for hours bar
        /// </summary>
        public static char ProgressClockLowerRightCornerCharHours
        {
            get => Config.SaverConfig.ProgressClockLowerRightCornerCharHours;
            set => Config.SaverConfig.ProgressClockLowerRightCornerCharHours = value;
        }
        /// <summary>
        /// [ProgressClock] Lower right corner character for minutes bar
        /// </summary>
        public static char ProgressClockLowerRightCornerCharMinutes
        {
            get => Config.SaverConfig.ProgressClockLowerRightCornerCharMinutes;
            set => Config.SaverConfig.ProgressClockLowerRightCornerCharMinutes = value;
        }
        /// <summary>
        /// [ProgressClock] Lower right corner character for seconds bar
        /// </summary>
        public static char ProgressClockLowerRightCornerCharSeconds
        {
            get => Config.SaverConfig.ProgressClockLowerRightCornerCharSeconds;
            set => Config.SaverConfig.ProgressClockLowerRightCornerCharSeconds = value;
        }
        /// <summary>
        /// [ProgressClock] Upper frame character for hours bar
        /// </summary>
        public static char ProgressClockUpperFrameCharHours
        {
            get => Config.SaverConfig.ProgressClockUpperFrameCharHours;
            set => Config.SaverConfig.ProgressClockUpperFrameCharHours = value;
        }
        /// <summary>
        /// [ProgressClock] Upper frame character for minutes bar
        /// </summary>
        public static char ProgressClockUpperFrameCharMinutes
        {
            get => Config.SaverConfig.ProgressClockUpperFrameCharMinutes;
            set => Config.SaverConfig.ProgressClockUpperFrameCharMinutes = value;
        }
        /// <summary>
        /// [ProgressClock] Upper frame character for seconds bar
        /// </summary>
        public static char ProgressClockUpperFrameCharSeconds
        {
            get => Config.SaverConfig.ProgressClockUpperFrameCharSeconds;
            set => Config.SaverConfig.ProgressClockUpperFrameCharSeconds = value;
        }
        /// <summary>
        /// [ProgressClock] Lower frame character for hours bar
        /// </summary>
        public static char ProgressClockLowerFrameCharHours
        {
            get => Config.SaverConfig.ProgressClockLowerFrameCharHours;
            set => Config.SaverConfig.ProgressClockLowerFrameCharHours = value;
        }
        /// <summary>
        /// [ProgressClock] Lower frame character for minutes bar
        /// </summary>
        public static char ProgressClockLowerFrameCharMinutes
        {
            get => Config.SaverConfig.ProgressClockLowerFrameCharMinutes;
            set => Config.SaverConfig.ProgressClockLowerFrameCharMinutes = value;
        }
        /// <summary>
        /// [ProgressClock] Lower frame character for seconds bar
        /// </summary>
        public static char ProgressClockLowerFrameCharSeconds
        {
            get => Config.SaverConfig.ProgressClockLowerFrameCharSeconds;
            set => Config.SaverConfig.ProgressClockLowerFrameCharSeconds = value;
        }
        /// <summary>
        /// [ProgressClock] Left frame character for hours bar
        /// </summary>
        public static char ProgressClockLeftFrameCharHours
        {
            get => Config.SaverConfig.ProgressClockLeftFrameCharHours;
            set => Config.SaverConfig.ProgressClockLeftFrameCharHours = value;
        }
        /// <summary>
        /// [ProgressClock] Left frame character for minutes bar
        /// </summary>
        public static char ProgressClockLeftFrameCharMinutes
        {
            get => Config.SaverConfig.ProgressClockLeftFrameCharMinutes;
            set => Config.SaverConfig.ProgressClockLeftFrameCharMinutes = value;
        }
        /// <summary>
        /// [ProgressClock] Left frame character for seconds bar
        /// </summary>
        public static char ProgressClockLeftFrameCharSeconds
        {
            get => Config.SaverConfig.ProgressClockLeftFrameCharSeconds;
            set => Config.SaverConfig.ProgressClockLeftFrameCharSeconds = value;
        }
        /// <summary>
        /// [ProgressClock] Right frame character for hours bar
        /// </summary>
        public static char ProgressClockRightFrameCharHours
        {
            get => Config.SaverConfig.ProgressClockRightFrameCharHours;
            set => Config.SaverConfig.ProgressClockRightFrameCharHours = value;
        }
        /// <summary>
        /// [ProgressClock] Right frame character for minutes bar
        /// </summary>
        public static char ProgressClockRightFrameCharMinutes
        {
            get => Config.SaverConfig.ProgressClockRightFrameCharMinutes;
            set => Config.SaverConfig.ProgressClockRightFrameCharMinutes = value;
        }
        /// <summary>
        /// [ProgressClock] Right frame character for seconds bar
        /// </summary>
        public static char ProgressClockRightFrameCharSeconds
        {
            get => Config.SaverConfig.ProgressClockRightFrameCharSeconds;
            set => Config.SaverConfig.ProgressClockRightFrameCharSeconds = value;
        }
        /// <summary>
        /// [ProgressClock] Information text for hours bar
        /// </summary>
        public static string ProgressClockInfoTextHours
        {
            get => Config.SaverConfig.ProgressClockInfoTextHours;
            set => Config.SaverConfig.ProgressClockInfoTextHours = value;
        }
        /// <summary>
        /// [ProgressClock] Information text for minutes bar
        /// </summary>
        public static string ProgressClockInfoTextMinutes
        {
            get => Config.SaverConfig.ProgressClockInfoTextMinutes;
            set => Config.SaverConfig.ProgressClockInfoTextMinutes = value;
        }
        /// <summary>
        /// [ProgressClock] Information text for seconds bar
        /// </summary>
        public static string ProgressClockInfoTextSeconds
        {
            get => Config.SaverConfig.ProgressClockInfoTextSeconds;
            set => Config.SaverConfig.ProgressClockInfoTextSeconds = value;
        }
        /// <summary>
        /// [ProgressClock] The minimum red color level (true color - hours)
        /// </summary>
        public static int ProgressClockMinimumRedColorLevelHours
        {
            get => Config.SaverConfig.ProgressClockMinimumRedColorLevelHours;
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
            get => Config.SaverConfig.ProgressClockMinimumGreenColorLevelHours;
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
            get => Config.SaverConfig.ProgressClockMinimumBlueColorLevelHours;
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
            get => Config.SaverConfig.ProgressClockMinimumColorLevelHours;
            set
            {
                int FinalMinimumLevel = 255;
                if (value < 0)
                    value = 1;
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
            get => Config.SaverConfig.ProgressClockMaximumRedColorLevelHours;
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
            get => Config.SaverConfig.ProgressClockMaximumGreenColorLevelHours;
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
            get => Config.SaverConfig.ProgressClockMaximumBlueColorLevelHours;
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
            get => Config.SaverConfig.ProgressClockMaximumColorLevelHours;
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
            get => Config.SaverConfig.ProgressClockMinimumRedColorLevelMinutes;
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
            get => Config.SaverConfig.ProgressClockMinimumGreenColorLevelMinutes;
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
            get => Config.SaverConfig.ProgressClockMinimumBlueColorLevelMinutes;
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
            get => Config.SaverConfig.ProgressClockMinimumColorLevelMinutes;
            set
            {
                int FinalMinimumLevel = 255;
                if (value < 0)
                    value = 1;
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
            get => Config.SaverConfig.ProgressClockMaximumRedColorLevelMinutes;
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
            get => Config.SaverConfig.ProgressClockMaximumGreenColorLevelMinutes;
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
            get => Config.SaverConfig.ProgressClockMaximumBlueColorLevelMinutes;
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
            get => Config.SaverConfig.ProgressClockMaximumColorLevelMinutes;
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
            get => Config.SaverConfig.ProgressClockMinimumRedColorLevelSeconds;
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
            get => Config.SaverConfig.ProgressClockMinimumGreenColorLevelSeconds;
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
            get => Config.SaverConfig.ProgressClockMinimumBlueColorLevelSeconds;
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
            get => Config.SaverConfig.ProgressClockMinimumColorLevelSeconds;
            set
            {
                int FinalMinimumLevel = 255;
                if (value < 0)
                    value = 1;
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
            get => Config.SaverConfig.ProgressClockMaximumRedColorLevelSeconds;
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
            get => Config.SaverConfig.ProgressClockMaximumGreenColorLevelSeconds;
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
            get => Config.SaverConfig.ProgressClockMaximumBlueColorLevelSeconds;
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
            get => Config.SaverConfig.ProgressClockMaximumColorLevelSeconds;
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
            get => Config.SaverConfig.ProgressClockMinimumRedColorLevel;
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
            get => Config.SaverConfig.ProgressClockMinimumGreenColorLevel;
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
            get => Config.SaverConfig.ProgressClockMinimumBlueColorLevel;
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
            get => Config.SaverConfig.ProgressClockMinimumColorLevel;
            set
            {
                int FinalMinimumLevel = 255;
                if (value < 0)
                    value = 1;
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
            get => Config.SaverConfig.ProgressClockMaximumRedColorLevel;
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
            get => Config.SaverConfig.ProgressClockMaximumGreenColorLevel;
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
            get => Config.SaverConfig.ProgressClockMaximumBlueColorLevel;
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
            get => Config.SaverConfig.ProgressClockMaximumColorLevel;
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

        private Color ColorStorageHours = Color.Empty,
                      ColorStorageMinutes = Color.Empty,
                      ColorStorageSeconds = Color.Empty,
                      ColorStorage = Color.Empty;
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
            int ProgressFillPositionHours, ProgressFillPositionMinutes, ProgressFillPositionSeconds;
            int InformationPositionHours, InformationPositionMinutes, InformationPositionSeconds;

            DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Current tick: {0}", CurrentTicks);
            if (ProgressClockSettings.ProgressClockCycleColors)
            {
                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Cycling colors...");
                if (CurrentTicks >= ProgressClockSettings.ProgressClockCycleColorsTicks)
                {
                    var type = ProgressClockSettings.ProgressClockTrueColor ? ColorType.TrueColor : ColorType._255Color;
                    DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Current tick equals the maximum ticks to change color.");
                    ColorStorageHours =
                        ColorTools.GetRandomColor(type, ProgressClockSettings.ProgressClockMinimumColorLevelHours, ProgressClockSettings.ProgressClockMaximumColorLevelHours,
                                                        ProgressClockSettings.ProgressClockMinimumRedColorLevelHours, ProgressClockSettings.ProgressClockMaximumRedColorLevelHours,
                                                        ProgressClockSettings.ProgressClockMinimumGreenColorLevelHours, ProgressClockSettings.ProgressClockMaximumGreenColorLevelHours,
                                                        ProgressClockSettings.ProgressClockMinimumBlueColorLevelHours, ProgressClockSettings.ProgressClockMaximumBlueColorLevelHours);
                    DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (Hours) (R;G;B: {0};{1};{2})", ColorStorageHours.R, ColorStorageHours.G, ColorStorageHours.B);
                    ColorStorageMinutes =
                        ColorTools.GetRandomColor(type, ProgressClockSettings.ProgressClockMinimumColorLevelMinutes, ProgressClockSettings.ProgressClockMaximumColorLevelMinutes,
                                                        ProgressClockSettings.ProgressClockMinimumRedColorLevelMinutes, ProgressClockSettings.ProgressClockMaximumRedColorLevelMinutes,
                                                        ProgressClockSettings.ProgressClockMinimumGreenColorLevelMinutes, ProgressClockSettings.ProgressClockMaximumGreenColorLevelMinutes,
                                                        ProgressClockSettings.ProgressClockMinimumBlueColorLevelMinutes, ProgressClockSettings.ProgressClockMaximumBlueColorLevelMinutes);
                    DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (Minutes) (R;G;B: {0};{1};{2})", ColorStorageMinutes.R, ColorStorageMinutes.G, ColorStorageMinutes.B);
                    ColorStorageSeconds =
                        ColorTools.GetRandomColor(type, ProgressClockSettings.ProgressClockMinimumColorLevelSeconds, ProgressClockSettings.ProgressClockMaximumColorLevelSeconds,
                                                        ProgressClockSettings.ProgressClockMinimumRedColorLevelSeconds, ProgressClockSettings.ProgressClockMaximumRedColorLevelSeconds,
                                                        ProgressClockSettings.ProgressClockMinimumGreenColorLevelSeconds, ProgressClockSettings.ProgressClockMaximumGreenColorLevelSeconds,
                                                        ProgressClockSettings.ProgressClockMinimumBlueColorLevelSeconds, ProgressClockSettings.ProgressClockMaximumBlueColorLevelSeconds);
                    DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (Seconds) (R;G;B: {0};{1};{2})", ColorStorageSeconds.R, ColorStorageSeconds.G, ColorStorageSeconds.B);
                    ColorStorage =
                        ColorTools.GetRandomColor(type, ProgressClockSettings.ProgressClockMinimumColorLevel, ProgressClockSettings.ProgressClockMaximumColorLevel,
                                                        ProgressClockSettings.ProgressClockMinimumRedColorLevel, ProgressClockSettings.ProgressClockMaximumRedColorLevel,
                                                        ProgressClockSettings.ProgressClockMinimumGreenColorLevel, ProgressClockSettings.ProgressClockMaximumGreenColorLevel,
                                                        ProgressClockSettings.ProgressClockMinimumBlueColorLevel, ProgressClockSettings.ProgressClockMaximumBlueColorLevel);
                    DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", ColorStorage.R, ColorStorage.G, ColorStorage.B);
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
                BoxFrameColor.WriteBoxFrame(4, (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) - 11, ConsoleWrapper.WindowWidth - 10, 1,
                    ProgressClockSettings.ProgressClockUpperLeftCornerCharHours, ProgressClockSettings.ProgressClockLowerLeftCornerCharHours,
                    ProgressClockSettings.ProgressClockUpperRightCornerCharHours, ProgressClockSettings.ProgressClockLowerRightCornerCharHours,
                    ProgressClockSettings.ProgressClockUpperFrameCharHours, ProgressClockSettings.ProgressClockLowerFrameCharHours,
                    ProgressClockSettings.ProgressClockLeftFrameCharHours, ProgressClockSettings.ProgressClockRightFrameCharHours,
                    ColorStorageHours);

                // Minutes
                BoxFrameColor.WriteBoxFrame(4, (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) - 2, ConsoleWrapper.WindowWidth - 10, 1,
                    ProgressClockSettings.ProgressClockUpperLeftCornerCharMinutes, ProgressClockSettings.ProgressClockLowerLeftCornerCharMinutes,
                    ProgressClockSettings.ProgressClockUpperRightCornerCharMinutes, ProgressClockSettings.ProgressClockLowerRightCornerCharMinutes,
                    ProgressClockSettings.ProgressClockUpperFrameCharMinutes, ProgressClockSettings.ProgressClockLowerFrameCharMinutes,
                    ProgressClockSettings.ProgressClockLeftFrameCharMinutes, ProgressClockSettings.ProgressClockRightFrameCharMinutes,
                    ColorStorageMinutes);

                // Seconds
                BoxFrameColor.WriteBoxFrame(4, (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) + 7, ConsoleWrapper.WindowWidth - 10, 1,
                    ProgressClockSettings.ProgressClockUpperLeftCornerCharSeconds, ProgressClockSettings.ProgressClockLowerLeftCornerCharSeconds,
                    ProgressClockSettings.ProgressClockUpperRightCornerCharSeconds, ProgressClockSettings.ProgressClockLowerRightCornerCharSeconds,
                    ProgressClockSettings.ProgressClockUpperFrameCharSeconds, ProgressClockSettings.ProgressClockLowerFrameCharSeconds,
                    ProgressClockSettings.ProgressClockLeftFrameCharSeconds, ProgressClockSettings.ProgressClockRightFrameCharSeconds,
                    ColorStorageSeconds);

                // Fill progress for hours, minutes, and seconds
                if (!(TimeDateTools.KernelDateTime.Hour == 0))
                    TextWriterWhereColor.WriteWhere(new string(' ', ConsoleExtensions.PercentRepeat(TimeDateTools.KernelDateTime.Hour, 24, 10)), 5, ProgressFillPositionHours, true, Color.Empty, ColorStorageHours);
                if (!(TimeDateTools.KernelDateTime.Minute == 0))
                    TextWriterWhereColor.WriteWhere(new string(' ', ConsoleExtensions.PercentRepeat(TimeDateTools.KernelDateTime.Minute, 60, 10)), 5, ProgressFillPositionMinutes, true, Color.Empty, ColorStorageMinutes);
                if (!(TimeDateTools.KernelDateTime.Second == 0))
                    TextWriterWhereColor.WriteWhere(new string(' ', ConsoleExtensions.PercentRepeat(TimeDateTools.KernelDateTime.Second, 60, 10)), 5, ProgressFillPositionSeconds, true, Color.Empty, ColorStorageSeconds);

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
                TextWriterWhereColor.WriteWhere(TimeDateRenderers.Render(), (int)Math.Round(ConsoleWrapper.WindowWidth / 2d - TimeDateRenderers.Render().Length / 2d), ConsoleWrapper.WindowHeight - 2, ColorStorage);
            }
            if (ProgressClockSettings.ProgressClockCycleColors)
                CurrentTicks += 1L;

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(ProgressClockSettings.ProgressClockDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
