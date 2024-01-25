//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using System;
using Terminaux.Colors;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Misc.Screensaver;
using Terminaux.Writer.FancyWriters;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Misc.Text.Probers.Placeholder;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Kernel.Time;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Kernel.Time.Renderers;
using Nitrocid.Kernel.Threading;
using Terminaux.Base;

namespace Nitrocid.ScreensaverPacks.Screensavers
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
            get => ScreensaverPackInit.SaversConfig.ProgressClockTrueColor;
            set => ScreensaverPackInit.SaversConfig.ProgressClockTrueColor = value;
        }
        /// <summary>
        /// [ProgressClock] Enable color cycling (uses RNG. If disabled, uses the <see cref="ProgressClockSecondsProgressColor"/>, <see cref="ProgressClockMinutesProgressColor"/>, and <see cref="ProgressClockHoursProgressColor"/> colors.)
        /// </summary>
        public static bool ProgressClockCycleColors
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockCycleColors;
            set => ScreensaverPackInit.SaversConfig.ProgressClockCycleColors = value;
        }
        /// <summary>
        /// [ProgressClock] The color of seconds progress bar. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public static string ProgressClockSecondsProgressColor
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockSecondsProgressColor;
            set => ScreensaverPackInit.SaversConfig.ProgressClockSecondsProgressColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [ProgressClock] The color of minutes progress bar. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public static string ProgressClockMinutesProgressColor
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockMinutesProgressColor;
            set => ScreensaverPackInit.SaversConfig.ProgressClockMinutesProgressColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [ProgressClock] The color of hours progress bar. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public static string ProgressClockHoursProgressColor
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockHoursProgressColor;
            set => ScreensaverPackInit.SaversConfig.ProgressClockHoursProgressColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [ProgressClock] The color of date information. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public static string ProgressClockProgressColor
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockProgressColor;
            set => ScreensaverPackInit.SaversConfig.ProgressClockProgressColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [ProgressClock] If color cycling is enabled, how many ticks before changing colors? 1 tick = 0.5 seconds
        /// </summary>
        public static long ProgressClockCycleColorsTicks
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockCycleColorsTicks;
            set
            {
                if (value <= 0L)
                    value = 20L;
                ScreensaverPackInit.SaversConfig.ProgressClockCycleColorsTicks = (int)value;
            }
        }
        /// <summary>
        /// [ProgressClock] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int ProgressClockDelay
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockDelay;
            set
            {
                if (value <= 0)
                    value = 500;
                ScreensaverPackInit.SaversConfig.ProgressClockDelay = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Upper left corner character for hours bar
        /// </summary>
        public static char ProgressClockUpperLeftCornerCharHours
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockUpperLeftCornerCharHours;
            set => ScreensaverPackInit.SaversConfig.ProgressClockUpperLeftCornerCharHours = value;
        }
        /// <summary>
        /// [ProgressClock] Upper left corner character for minutes bar
        /// </summary>
        public static char ProgressClockUpperLeftCornerCharMinutes
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockUpperLeftCornerCharMinutes;
            set => ScreensaverPackInit.SaversConfig.ProgressClockUpperLeftCornerCharMinutes = value;
        }
        /// <summary>
        /// [ProgressClock] Upper left corner character for seconds bar
        /// </summary>
        public static char ProgressClockUpperLeftCornerCharSeconds
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockUpperLeftCornerCharSeconds;
            set => ScreensaverPackInit.SaversConfig.ProgressClockUpperLeftCornerCharSeconds = value;
        }
        /// <summary>
        /// [ProgressClock] Upper right corner character for hours bar
        /// </summary>
        public static char ProgressClockUpperRightCornerCharHours
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockUpperRightCornerCharHours;
            set => ScreensaverPackInit.SaversConfig.ProgressClockUpperRightCornerCharHours = value;
        }
        /// <summary>
        /// [ProgressClock] Upper right corner character for minutes bar
        /// </summary>
        public static char ProgressClockUpperRightCornerCharMinutes
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockUpperRightCornerCharMinutes;
            set => ScreensaverPackInit.SaversConfig.ProgressClockUpperRightCornerCharMinutes = value;
        }
        /// <summary>
        /// [ProgressClock] Upper right corner character for seconds bar
        /// </summary>
        public static char ProgressClockUpperRightCornerCharSeconds
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockUpperRightCornerCharSeconds;
            set => ScreensaverPackInit.SaversConfig.ProgressClockUpperRightCornerCharSeconds = value;
        }
        /// <summary>
        /// [ProgressClock] Lower left corner character for hours bar
        /// </summary>
        public static char ProgressClockLowerLeftCornerCharHours
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockLowerLeftCornerCharHours;
            set => ScreensaverPackInit.SaversConfig.ProgressClockLowerLeftCornerCharHours = value;
        }
        /// <summary>
        /// [ProgressClock] Lower left corner character for minutes bar
        /// </summary>
        public static char ProgressClockLowerLeftCornerCharMinutes
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockLowerLeftCornerCharMinutes;
            set => ScreensaverPackInit.SaversConfig.ProgressClockLowerLeftCornerCharMinutes = value;
        }
        /// <summary>
        /// [ProgressClock] Lower left corner character for seconds bar
        /// </summary>
        public static char ProgressClockLowerLeftCornerCharSeconds
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockLowerLeftCornerCharSeconds;
            set => ScreensaverPackInit.SaversConfig.ProgressClockLowerLeftCornerCharSeconds = value;
        }
        /// <summary>
        /// [ProgressClock] Lower right corner character for hours bar
        /// </summary>
        public static char ProgressClockLowerRightCornerCharHours
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockLowerRightCornerCharHours;
            set => ScreensaverPackInit.SaversConfig.ProgressClockLowerRightCornerCharHours = value;
        }
        /// <summary>
        /// [ProgressClock] Lower right corner character for minutes bar
        /// </summary>
        public static char ProgressClockLowerRightCornerCharMinutes
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockLowerRightCornerCharMinutes;
            set => ScreensaverPackInit.SaversConfig.ProgressClockLowerRightCornerCharMinutes = value;
        }
        /// <summary>
        /// [ProgressClock] Lower right corner character for seconds bar
        /// </summary>
        public static char ProgressClockLowerRightCornerCharSeconds
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockLowerRightCornerCharSeconds;
            set => ScreensaverPackInit.SaversConfig.ProgressClockLowerRightCornerCharSeconds = value;
        }
        /// <summary>
        /// [ProgressClock] Upper frame character for hours bar
        /// </summary>
        public static char ProgressClockUpperFrameCharHours
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockUpperFrameCharHours;
            set => ScreensaverPackInit.SaversConfig.ProgressClockUpperFrameCharHours = value;
        }
        /// <summary>
        /// [ProgressClock] Upper frame character for minutes bar
        /// </summary>
        public static char ProgressClockUpperFrameCharMinutes
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockUpperFrameCharMinutes;
            set => ScreensaverPackInit.SaversConfig.ProgressClockUpperFrameCharMinutes = value;
        }
        /// <summary>
        /// [ProgressClock] Upper frame character for seconds bar
        /// </summary>
        public static char ProgressClockUpperFrameCharSeconds
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockUpperFrameCharSeconds;
            set => ScreensaverPackInit.SaversConfig.ProgressClockUpperFrameCharSeconds = value;
        }
        /// <summary>
        /// [ProgressClock] Lower frame character for hours bar
        /// </summary>
        public static char ProgressClockLowerFrameCharHours
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockLowerFrameCharHours;
            set => ScreensaverPackInit.SaversConfig.ProgressClockLowerFrameCharHours = value;
        }
        /// <summary>
        /// [ProgressClock] Lower frame character for minutes bar
        /// </summary>
        public static char ProgressClockLowerFrameCharMinutes
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockLowerFrameCharMinutes;
            set => ScreensaverPackInit.SaversConfig.ProgressClockLowerFrameCharMinutes = value;
        }
        /// <summary>
        /// [ProgressClock] Lower frame character for seconds bar
        /// </summary>
        public static char ProgressClockLowerFrameCharSeconds
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockLowerFrameCharSeconds;
            set => ScreensaverPackInit.SaversConfig.ProgressClockLowerFrameCharSeconds = value;
        }
        /// <summary>
        /// [ProgressClock] Left frame character for hours bar
        /// </summary>
        public static char ProgressClockLeftFrameCharHours
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockLeftFrameCharHours;
            set => ScreensaverPackInit.SaversConfig.ProgressClockLeftFrameCharHours = value;
        }
        /// <summary>
        /// [ProgressClock] Left frame character for minutes bar
        /// </summary>
        public static char ProgressClockLeftFrameCharMinutes
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockLeftFrameCharMinutes;
            set => ScreensaverPackInit.SaversConfig.ProgressClockLeftFrameCharMinutes = value;
        }
        /// <summary>
        /// [ProgressClock] Left frame character for seconds bar
        /// </summary>
        public static char ProgressClockLeftFrameCharSeconds
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockLeftFrameCharSeconds;
            set => ScreensaverPackInit.SaversConfig.ProgressClockLeftFrameCharSeconds = value;
        }
        /// <summary>
        /// [ProgressClock] Right frame character for hours bar
        /// </summary>
        public static char ProgressClockRightFrameCharHours
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockRightFrameCharHours;
            set => ScreensaverPackInit.SaversConfig.ProgressClockRightFrameCharHours = value;
        }
        /// <summary>
        /// [ProgressClock] Right frame character for minutes bar
        /// </summary>
        public static char ProgressClockRightFrameCharMinutes
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockRightFrameCharMinutes;
            set => ScreensaverPackInit.SaversConfig.ProgressClockRightFrameCharMinutes = value;
        }
        /// <summary>
        /// [ProgressClock] Right frame character for seconds bar
        /// </summary>
        public static char ProgressClockRightFrameCharSeconds
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockRightFrameCharSeconds;
            set => ScreensaverPackInit.SaversConfig.ProgressClockRightFrameCharSeconds = value;
        }
        /// <summary>
        /// [ProgressClock] Information text for hours bar
        /// </summary>
        public static string ProgressClockInfoTextHours
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockInfoTextHours;
            set => ScreensaverPackInit.SaversConfig.ProgressClockInfoTextHours = value;
        }
        /// <summary>
        /// [ProgressClock] Information text for minutes bar
        /// </summary>
        public static string ProgressClockInfoTextMinutes
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockInfoTextMinutes;
            set => ScreensaverPackInit.SaversConfig.ProgressClockInfoTextMinutes = value;
        }
        /// <summary>
        /// [ProgressClock] Information text for seconds bar
        /// </summary>
        public static string ProgressClockInfoTextSeconds
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockInfoTextSeconds;
            set => ScreensaverPackInit.SaversConfig.ProgressClockInfoTextSeconds = value;
        }
        /// <summary>
        /// [ProgressClock] The minimum red color level (true color - hours)
        /// </summary>
        public static int ProgressClockMinimumRedColorLevelHours
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockMinimumRedColorLevelHours;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ProgressClockMinimumRedColorLevelHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum green color level (true color - hours)
        /// </summary>
        public static int ProgressClockMinimumGreenColorLevelHours
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockMinimumGreenColorLevelHours;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ProgressClockMinimumGreenColorLevelHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum blue color level (true color - hours)
        /// </summary>
        public static int ProgressClockMinimumBlueColorLevelHours
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockMinimumBlueColorLevelHours;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ProgressClockMinimumBlueColorLevelHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum color level (255 colors or 16 colors - hours)
        /// </summary>
        public static int ProgressClockMinimumColorLevelHours
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockMinimumColorLevelHours;
            set
            {
                int FinalMinimumLevel = 255;
                if (value < 0)
                    value = 1;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                ScreensaverPackInit.SaversConfig.ProgressClockMinimumColorLevelHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum red color level (true color - hours)
        /// </summary>
        public static int ProgressClockMaximumRedColorLevelHours
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockMaximumRedColorLevelHours;
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.ProgressClockMinimumRedColorLevelHours)
                    value = ScreensaverPackInit.SaversConfig.ProgressClockMinimumRedColorLevelHours;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ProgressClockMaximumRedColorLevelHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum green color level (true color - hours)
        /// </summary>
        public static int ProgressClockMaximumGreenColorLevelHours
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockMaximumGreenColorLevelHours;
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.ProgressClockMinimumGreenColorLevelHours)
                    value = ScreensaverPackInit.SaversConfig.ProgressClockMinimumGreenColorLevelHours;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ProgressClockMaximumGreenColorLevelHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum blue color level (true color - hours)
        /// </summary>
        public static int ProgressClockMaximumBlueColorLevelHours
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockMaximumBlueColorLevelHours;
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.ProgressClockMinimumBlueColorLevelHours)
                    value = ScreensaverPackInit.SaversConfig.ProgressClockMinimumBlueColorLevelHours;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ProgressClockMaximumBlueColorLevelHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum color level (255 colors or 16 colors - hours)
        /// </summary>
        public static int ProgressClockMaximumColorLevelHours
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockMaximumColorLevelHours;
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= ScreensaverPackInit.SaversConfig.ProgressClockMinimumColorLevelHours)
                    value = ScreensaverPackInit.SaversConfig.ProgressClockMinimumColorLevelHours;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                ScreensaverPackInit.SaversConfig.ProgressClockMaximumColorLevelHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum red color level (true color - minutes)
        /// </summary>
        public static int ProgressClockMinimumRedColorLevelMinutes
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockMinimumRedColorLevelMinutes;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ProgressClockMinimumRedColorLevelMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum green color level (true color - minutes)
        /// </summary>
        public static int ProgressClockMinimumGreenColorLevelMinutes
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockMinimumGreenColorLevelMinutes;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ProgressClockMinimumGreenColorLevelMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum blue color level (true color - minutes)
        /// </summary>
        public static int ProgressClockMinimumBlueColorLevelMinutes
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockMinimumBlueColorLevelMinutes;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ProgressClockMinimumBlueColorLevelMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum color level (255 colors or 16 colors - minutes)
        /// </summary>
        public static int ProgressClockMinimumColorLevelMinutes
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockMinimumColorLevelMinutes;
            set
            {
                int FinalMinimumLevel = 255;
                if (value < 0)
                    value = 1;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                ScreensaverPackInit.SaversConfig.ProgressClockMinimumColorLevelMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum red color level (true color - minutes)
        /// </summary>
        public static int ProgressClockMaximumRedColorLevelMinutes
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockMaximumRedColorLevelMinutes;
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.ProgressClockMinimumRedColorLevelMinutes)
                    value = ScreensaverPackInit.SaversConfig.ProgressClockMinimumRedColorLevelMinutes;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ProgressClockMaximumRedColorLevelMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum green color level (true color - minutes)
        /// </summary>
        public static int ProgressClockMaximumGreenColorLevelMinutes
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockMaximumGreenColorLevelMinutes;
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.ProgressClockMinimumGreenColorLevelMinutes)
                    value = ScreensaverPackInit.SaversConfig.ProgressClockMinimumGreenColorLevelMinutes;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ProgressClockMaximumGreenColorLevelMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum blue color level (true color - minutes)
        /// </summary>
        public static int ProgressClockMaximumBlueColorLevelMinutes
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockMaximumBlueColorLevelMinutes;
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.ProgressClockMinimumBlueColorLevelMinutes)
                    value = ScreensaverPackInit.SaversConfig.ProgressClockMinimumBlueColorLevelMinutes;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ProgressClockMaximumBlueColorLevelMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum color level (255 colors or 16 colors - minutes)
        /// </summary>
        public static int ProgressClockMaximumColorLevelMinutes
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockMaximumColorLevelMinutes;
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= ScreensaverPackInit.SaversConfig.ProgressClockMinimumColorLevelMinutes)
                    value = ScreensaverPackInit.SaversConfig.ProgressClockMinimumColorLevelMinutes;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                ScreensaverPackInit.SaversConfig.ProgressClockMaximumColorLevelMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum red color level (true color - seconds)
        /// </summary>
        public static int ProgressClockMinimumRedColorLevelSeconds
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockMinimumRedColorLevelSeconds;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ProgressClockMinimumRedColorLevelSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum green color level (true color - seconds)
        /// </summary>
        public static int ProgressClockMinimumGreenColorLevelSeconds
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockMinimumGreenColorLevelSeconds;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ProgressClockMinimumGreenColorLevelSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum blue color level (true color - seconds)
        /// </summary>
        public static int ProgressClockMinimumBlueColorLevelSeconds
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockMinimumBlueColorLevelSeconds;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ProgressClockMinimumBlueColorLevelSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum color level (255 colors or 16 colors - seconds)
        /// </summary>
        public static int ProgressClockMinimumColorLevelSeconds
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockMinimumColorLevelSeconds;
            set
            {
                int FinalMinimumLevel = 255;
                if (value < 0)
                    value = 1;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                ScreensaverPackInit.SaversConfig.ProgressClockMinimumColorLevelSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum red color level (true color - seconds)
        /// </summary>
        public static int ProgressClockMaximumRedColorLevelSeconds
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockMaximumRedColorLevelSeconds;
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.ProgressClockMinimumRedColorLevelSeconds)
                    value = ScreensaverPackInit.SaversConfig.ProgressClockMinimumRedColorLevelSeconds;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ProgressClockMaximumRedColorLevelSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum green color level (true color - seconds)
        /// </summary>
        public static int ProgressClockMaximumGreenColorLevelSeconds
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockMaximumGreenColorLevelSeconds;
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.ProgressClockMinimumGreenColorLevelSeconds)
                    value = ScreensaverPackInit.SaversConfig.ProgressClockMinimumGreenColorLevelSeconds;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ProgressClockMaximumGreenColorLevelSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum blue color level (true color - seconds)
        /// </summary>
        public static int ProgressClockMaximumBlueColorLevelSeconds
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockMaximumBlueColorLevelSeconds;
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.ProgressClockMinimumBlueColorLevelSeconds)
                    value = ScreensaverPackInit.SaversConfig.ProgressClockMinimumBlueColorLevelSeconds;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ProgressClockMaximumBlueColorLevelSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum color level (255 colors or 16 colors - seconds)
        /// </summary>
        public static int ProgressClockMaximumColorLevelSeconds
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockMaximumColorLevelSeconds;
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= ScreensaverPackInit.SaversConfig.ProgressClockMinimumColorLevelSeconds)
                    value = ScreensaverPackInit.SaversConfig.ProgressClockMinimumColorLevelSeconds;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                ScreensaverPackInit.SaversConfig.ProgressClockMaximumColorLevelSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum red color level (true color)
        /// </summary>
        public static int ProgressClockMinimumRedColorLevel
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockMinimumRedColorLevel;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ProgressClockMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum green color level (true color)
        /// </summary>
        public static int ProgressClockMinimumGreenColorLevel
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockMinimumGreenColorLevel;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ProgressClockMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum blue color level (true color)
        /// </summary>
        public static int ProgressClockMinimumBlueColorLevel
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockMinimumBlueColorLevel;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ProgressClockMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int ProgressClockMinimumColorLevel
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockMinimumColorLevel;
            set
            {
                int FinalMinimumLevel = 255;
                if (value < 0)
                    value = 1;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                ScreensaverPackInit.SaversConfig.ProgressClockMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum red color level (true color)
        /// </summary>
        public static int ProgressClockMaximumRedColorLevel
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockMaximumRedColorLevel;
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.ProgressClockMinimumRedColorLevel)
                    value = ScreensaverPackInit.SaversConfig.ProgressClockMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ProgressClockMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum green color level (true color)
        /// </summary>
        public static int ProgressClockMaximumGreenColorLevel
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockMaximumGreenColorLevel;
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.ProgressClockMinimumGreenColorLevel)
                    value = ScreensaverPackInit.SaversConfig.ProgressClockMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ProgressClockMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum blue color level (true color)
        /// </summary>
        public static int ProgressClockMaximumBlueColorLevel
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockMaximumBlueColorLevel;
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.ProgressClockMinimumBlueColorLevel)
                    value = ScreensaverPackInit.SaversConfig.ProgressClockMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ProgressClockMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int ProgressClockMaximumColorLevel
        {
            get => ScreensaverPackInit.SaversConfig.ProgressClockMaximumColorLevel;
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= ScreensaverPackInit.SaversConfig.ProgressClockMinimumColorLevel)
                    value = ScreensaverPackInit.SaversConfig.ProgressClockMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                ScreensaverPackInit.SaversConfig.ProgressClockMaximumColorLevel = value;
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
        private string lastDate = "";

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "ProgressClock";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            CurrentTicks = ProgressClockSettings.ProgressClockCycleColorsTicks;
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Prepare colors
            int ProgressFillPositionHours, ProgressFillPositionMinutes, ProgressFillPositionSeconds;
            int InformationPositionHours, InformationPositionMinutes, InformationPositionSeconds;

            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Current tick: {0}", CurrentTicks);
            if (ProgressClockSettings.ProgressClockCycleColors)
            {
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Cycling colors...");
                if (CurrentTicks >= ProgressClockSettings.ProgressClockCycleColorsTicks)
                {
                    var type = ProgressClockSettings.ProgressClockTrueColor ? ColorType.TrueColor : ColorType._255Color;
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Current tick equals the maximum ticks to change color.");
                    ColorStorageHours =
                        ColorTools.GetRandomColor(type, ProgressClockSettings.ProgressClockMinimumColorLevelHours, ProgressClockSettings.ProgressClockMaximumColorLevelHours,
                                                        ProgressClockSettings.ProgressClockMinimumRedColorLevelHours, ProgressClockSettings.ProgressClockMaximumRedColorLevelHours,
                                                        ProgressClockSettings.ProgressClockMinimumGreenColorLevelHours, ProgressClockSettings.ProgressClockMaximumGreenColorLevelHours,
                                                        ProgressClockSettings.ProgressClockMinimumBlueColorLevelHours, ProgressClockSettings.ProgressClockMaximumBlueColorLevelHours);
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color (Hours) (R;G;B: {0};{1};{2})", ColorStorageHours.RGB.R, ColorStorageHours.RGB.G, ColorStorageHours.RGB.B);
                    ColorStorageMinutes =
                        ColorTools.GetRandomColor(type, ProgressClockSettings.ProgressClockMinimumColorLevelMinutes, ProgressClockSettings.ProgressClockMaximumColorLevelMinutes,
                                                        ProgressClockSettings.ProgressClockMinimumRedColorLevelMinutes, ProgressClockSettings.ProgressClockMaximumRedColorLevelMinutes,
                                                        ProgressClockSettings.ProgressClockMinimumGreenColorLevelMinutes, ProgressClockSettings.ProgressClockMaximumGreenColorLevelMinutes,
                                                        ProgressClockSettings.ProgressClockMinimumBlueColorLevelMinutes, ProgressClockSettings.ProgressClockMaximumBlueColorLevelMinutes);
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color (Minutes) (R;G;B: {0};{1};{2})", ColorStorageMinutes.RGB.R, ColorStorageMinutes.RGB.G, ColorStorageMinutes.RGB.B);
                    ColorStorageSeconds =
                        ColorTools.GetRandomColor(type, ProgressClockSettings.ProgressClockMinimumColorLevelSeconds, ProgressClockSettings.ProgressClockMaximumColorLevelSeconds,
                                                        ProgressClockSettings.ProgressClockMinimumRedColorLevelSeconds, ProgressClockSettings.ProgressClockMaximumRedColorLevelSeconds,
                                                        ProgressClockSettings.ProgressClockMinimumGreenColorLevelSeconds, ProgressClockSettings.ProgressClockMaximumGreenColorLevelSeconds,
                                                        ProgressClockSettings.ProgressClockMinimumBlueColorLevelSeconds, ProgressClockSettings.ProgressClockMaximumBlueColorLevelSeconds);
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color (Seconds) (R;G;B: {0};{1};{2})", ColorStorageSeconds.RGB.R, ColorStorageSeconds.RGB.G, ColorStorageSeconds.RGB.B);
                    ColorStorage =
                        ColorTools.GetRandomColor(type, ProgressClockSettings.ProgressClockMinimumColorLevel, ProgressClockSettings.ProgressClockMaximumColorLevel,
                                                        ProgressClockSettings.ProgressClockMinimumRedColorLevel, ProgressClockSettings.ProgressClockMaximumRedColorLevel,
                                                        ProgressClockSettings.ProgressClockMinimumGreenColorLevel, ProgressClockSettings.ProgressClockMaximumGreenColorLevel,
                                                        ProgressClockSettings.ProgressClockMinimumBlueColorLevel, ProgressClockSettings.ProgressClockMaximumBlueColorLevel);
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", ColorStorage.RGB.R, ColorStorage.RGB.G, ColorStorage.RGB.B);
                    CurrentTicks = 0L;
                }
            }
            else
            {
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Parsing colors...");
                ColorStorageHours = new Color(ProgressClockSettings.ProgressClockHoursProgressColor);
                ColorStorageMinutes = new Color(ProgressClockSettings.ProgressClockMinutesProgressColor);
                ColorStorageSeconds = new Color(ProgressClockSettings.ProgressClockSecondsProgressColor);
                ColorStorage = new Color(ProgressClockSettings.ProgressClockProgressColor);
            }
            ProgressFillPositionHours = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) - 10;
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Fill position for progress (Hours) {0}", ProgressFillPositionHours);
            ProgressFillPositionMinutes = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) - 1;
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Fill position for progress (Minutes) {0}", ProgressFillPositionMinutes);
            ProgressFillPositionSeconds = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) + 8;
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Fill position for progress (Seconds) {0}", ProgressFillPositionSeconds);
            InformationPositionHours = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) - 12;
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Fill position for info (Hours) {0}", InformationPositionHours);
            InformationPositionMinutes = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) - 3;
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Fill position for info (Minutes) {0}", InformationPositionMinutes);
            InformationPositionSeconds = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d) + 6;
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Fill position for info (Seconds) {0}", InformationPositionSeconds);

            if (!ConsoleResizeHandler.WasResized(false))
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
                if (TimeDateTools.KernelDateTime.Hour != 0)
                {
                    TextWriters.WriteWhere(new string(' ', ConsoleWrapper.WindowWidth - 10), 5, ProgressFillPositionHours, true, KernelColorType.NeutralText, KernelColorType.Background);
                    TextWriterWhereColor.WriteWhereColorBack(new string(' ', ConsoleExtensions.PercentRepeat(TimeDateTools.KernelDateTime.Hour, 24, 10)), 5, ProgressFillPositionHours, true, Color.Empty, ColorStorageHours);
                }
                if (TimeDateTools.KernelDateTime.Minute != 0)
                {
                    TextWriters.WriteWhere(new string(' ', ConsoleWrapper.WindowWidth - 10), 5, ProgressFillPositionMinutes, true, KernelColorType.NeutralText, KernelColorType.Background);
                    TextWriterWhereColor.WriteWhereColorBack(new string(' ', ConsoleExtensions.PercentRepeat(TimeDateTools.KernelDateTime.Minute, 60, 10)), 5, ProgressFillPositionMinutes, true, Color.Empty, ColorStorageMinutes);
                }
                if (TimeDateTools.KernelDateTime.Second != 0)
                {
                    TextWriters.WriteWhere(new string(' ', ConsoleWrapper.WindowWidth - 10), 5, ProgressFillPositionSeconds, true, KernelColorType.NeutralText, KernelColorType.Background);
                    TextWriterWhereColor.WriteWhereColorBack(new string(' ', ConsoleExtensions.PercentRepeat(TimeDateTools.KernelDateTime.Second, 60, 10)), 5, ProgressFillPositionSeconds, true, Color.Empty, ColorStorageSeconds);
                }

                // Print information
                if (!string.IsNullOrEmpty(ProgressClockSettings.ProgressClockInfoTextHours))
                    TextWriterWhereColor.WriteWhereColor(PlaceParse.ProbePlaces(ProgressClockSettings.ProgressClockInfoTextHours), 4, InformationPositionHours, true, ColorStorageHours, TimeDateTools.KernelDateTime.Hour);
                else
                    TextWriterWhereColor.WriteWhereColor("H: {0}/24  ", 4, InformationPositionHours, true, ColorStorageHours, TimeDateTools.KernelDateTime.Hour);
                if (!string.IsNullOrEmpty(ProgressClockSettings.ProgressClockInfoTextMinutes))
                    TextWriterWhereColor.WriteWhereColor(PlaceParse.ProbePlaces(ProgressClockSettings.ProgressClockInfoTextMinutes), 4, InformationPositionMinutes, true, ColorStorageMinutes, TimeDateTools.KernelDateTime.Minute);
                else
                    TextWriterWhereColor.WriteWhereColor("M: {0}/60  ", 4, InformationPositionMinutes, true, ColorStorageMinutes, TimeDateTools.KernelDateTime.Minute);
                if (!string.IsNullOrEmpty(ProgressClockSettings.ProgressClockInfoTextHours))
                    TextWriterWhereColor.WriteWhereColor(PlaceParse.ProbePlaces(ProgressClockSettings.ProgressClockInfoTextSeconds), 4, InformationPositionSeconds, true, ColorStorageSeconds, TimeDateTools.KernelDateTime.Second);
                else
                    TextWriterWhereColor.WriteWhereColor("S: {0}/60  ", 4, InformationPositionSeconds, true, ColorStorageSeconds, TimeDateTools.KernelDateTime.Second);

                // Print date information
                TextWriterWhereColor.WriteWhereColor(new string(' ', lastDate.Length), (int)Math.Round(ConsoleWrapper.WindowWidth / 2d - lastDate.Length / 2d), ConsoleWrapper.WindowHeight - 2, ColorStorage);
                string currentDate = TimeDateRenderers.Render();
                TextWriterWhereColor.WriteWhereColor(currentDate, (int)Math.Round(ConsoleWrapper.WindowWidth / 2d - currentDate.Length / 2d), ConsoleWrapper.WindowHeight - 2, ColorStorage);
                lastDate = currentDate;
            }
            if (ProgressClockSettings.ProgressClockCycleColors)
                CurrentTicks += 1L;

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ThreadManager.SleepNoBlock(ProgressClockSettings.ProgressClockDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
