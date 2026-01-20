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

using Nitrocid.Base.Kernel.Configuration.Instances;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Settings
{
    /// <summary>
    /// Screensaver kernel configuration instance
    /// </summary>
    public partial class ExtraSaversConfig : BaseKernelConfig
    {
        private bool progressClockTrueColor = true;
        private bool progressClockCycleColors = true;
        private int progressClockCycleColorsTicks = 20;
        private string progressClockSecondsProgressColor = "4";
        private string progressClockMinutesProgressColor = "5";
        private string progressClockHoursProgressColor = "6";
        private string progressClockProgressColor = "7";
        private int progressClockDelay = 500;
        private char progressClockUpperLeftCornerCharHours = '╭';
        private char progressClockUpperLeftCornerCharMinutes = '╭';
        private char progressClockUpperLeftCornerCharSeconds = '╭';
        private char progressClockUpperRightCornerCharHours = '╮';
        private char progressClockUpperRightCornerCharMinutes = '╮';
        private char progressClockUpperRightCornerCharSeconds = '╮';
        private char progressClockLowerLeftCornerCharHours = '╰';
        private char progressClockLowerLeftCornerCharMinutes = '╰';
        private char progressClockLowerLeftCornerCharSeconds = '╰';
        private char progressClockLowerRightCornerCharHours = '╯';
        private char progressClockLowerRightCornerCharMinutes = '╯';
        private char progressClockLowerRightCornerCharSeconds = '╯';
        private char progressClockUpperFrameCharHours = '─';
        private char progressClockUpperFrameCharMinutes = '─';
        private char progressClockUpperFrameCharSeconds = '─';
        private char progressClockLowerFrameCharHours = '─';
        private char progressClockLowerFrameCharMinutes = '─';
        private char progressClockLowerFrameCharSeconds = '─';
        private char progressClockLeftFrameCharHours = '│';
        private char progressClockLeftFrameCharMinutes = '│';
        private char progressClockLeftFrameCharSeconds = '│';
        private char progressClockRightFrameCharHours = '│';
        private char progressClockRightFrameCharMinutes = '│';
        private char progressClockRightFrameCharSeconds = '│';
        private string progressClockInfoTextHours = "";
        private string progressClockInfoTextMinutes = "";
        private string progressClockInfoTextSeconds = "";
        private int progressClockMinimumRedColorLevelHours = 0;
        private int progressClockMinimumGreenColorLevelHours = 0;
        private int progressClockMinimumBlueColorLevelHours = 0;
        private int progressClockMinimumColorLevelHours = 1;
        private int progressClockMaximumRedColorLevelHours = 255;
        private int progressClockMaximumGreenColorLevelHours = 255;
        private int progressClockMaximumBlueColorLevelHours = 255;
        private int progressClockMaximumColorLevelHours = 255;
        private int progressClockMinimumRedColorLevelMinutes = 0;
        private int progressClockMinimumGreenColorLevelMinutes = 0;
        private int progressClockMinimumBlueColorLevelMinutes = 0;
        private int progressClockMinimumColorLevelMinutes = 1;
        private int progressClockMaximumRedColorLevelMinutes = 255;
        private int progressClockMaximumGreenColorLevelMinutes = 255;
        private int progressClockMaximumBlueColorLevelMinutes = 255;
        private int progressClockMaximumColorLevelMinutes = 255;
        private int progressClockMinimumRedColorLevelSeconds = 0;
        private int progressClockMinimumGreenColorLevelSeconds = 0;
        private int progressClockMinimumBlueColorLevelSeconds = 0;
        private int progressClockMinimumColorLevelSeconds = 1;
        private int progressClockMaximumRedColorLevelSeconds = 255;
        private int progressClockMaximumGreenColorLevelSeconds = 255;
        private int progressClockMaximumBlueColorLevelSeconds = 255;
        private int progressClockMaximumColorLevelSeconds = 255;
        private int progressClockMinimumRedColorLevel = 0;
        private int progressClockMinimumGreenColorLevel = 0;
        private int progressClockMinimumBlueColorLevel = 0;
        private int progressClockMinimumColorLevel = 1;
        private int progressClockMaximumRedColorLevel = 255;
        private int progressClockMaximumGreenColorLevel = 255;
        private int progressClockMaximumBlueColorLevel = 255;
        private int progressClockMaximumColorLevel = 255;

        /// <summary>
        /// [ProgressClock] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool ProgressClockTrueColor
        {
            get => progressClockTrueColor;
            set => progressClockTrueColor = value;
        }
        /// <summary>
        /// [ProgressClock] Enable color cycling (uses RNG. If disabled, uses the <see cref="ProgressClockSecondsProgressColor"/>, <see cref="ProgressClockMinutesProgressColor"/>, and <see cref="ProgressClockHoursProgressColor"/> colors.)
        /// </summary>
        public bool ProgressClockCycleColors
        {
            get => progressClockCycleColors;
            set => progressClockCycleColors = value;
        }
        /// <summary>
        /// [ProgressClock] The color of seconds progress bar. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public string ProgressClockSecondsProgressColor
        {
            get => progressClockSecondsProgressColor;
            set => progressClockSecondsProgressColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [ProgressClock] The color of minutes progress bar. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public string ProgressClockMinutesProgressColor
        {
            get => progressClockMinutesProgressColor;
            set => progressClockMinutesProgressColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [ProgressClock] The color of hours progress bar. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public string ProgressClockHoursProgressColor
        {
            get => progressClockHoursProgressColor;
            set => progressClockHoursProgressColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [ProgressClock] The color of date information. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public string ProgressClockProgressColor
        {
            get => progressClockProgressColor;
            set => progressClockProgressColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [ProgressClock] If color cycling is enabled, how many ticks before changing colors? 1 tick = 0.5 seconds
        /// </summary>
        public long ProgressClockCycleColorsTicks
        {
            get => progressClockCycleColorsTicks;
            set
            {
                if (value <= 0L)
                    value = 20L;
                progressClockCycleColorsTicks = (int)value;
            }
        }
        /// <summary>
        /// [ProgressClock] How many milliseconds to wait before making the next write?
        /// </summary>
        public int ProgressClockDelay
        {
            get => progressClockDelay;
            set
            {
                if (value <= 0)
                    value = 500;
                progressClockDelay = value;
            }
        }
        /// <summary>
        /// [ProgressClock] Upper left corner character for hours bar
        /// </summary>
        public char ProgressClockUpperLeftCornerCharHours
        {
            get => progressClockUpperLeftCornerCharHours;
            set => progressClockUpperLeftCornerCharHours = value;
        }
        /// <summary>
        /// [ProgressClock] Upper left corner character for minutes bar
        /// </summary>
        public char ProgressClockUpperLeftCornerCharMinutes
        {
            get => progressClockUpperLeftCornerCharMinutes;
            set => progressClockUpperLeftCornerCharMinutes = value;
        }
        /// <summary>
        /// [ProgressClock] Upper left corner character for seconds bar
        /// </summary>
        public char ProgressClockUpperLeftCornerCharSeconds
        {
            get => progressClockUpperLeftCornerCharSeconds;
            set => progressClockUpperLeftCornerCharSeconds = value;
        }
        /// <summary>
        /// [ProgressClock] Upper right corner character for hours bar
        /// </summary>
        public char ProgressClockUpperRightCornerCharHours
        {
            get => progressClockUpperRightCornerCharHours;
            set => progressClockUpperRightCornerCharHours = value;
        }
        /// <summary>
        /// [ProgressClock] Upper right corner character for minutes bar
        /// </summary>
        public char ProgressClockUpperRightCornerCharMinutes
        {
            get => progressClockUpperRightCornerCharMinutes;
            set => progressClockUpperRightCornerCharMinutes = value;
        }
        /// <summary>
        /// [ProgressClock] Upper right corner character for seconds bar
        /// </summary>
        public char ProgressClockUpperRightCornerCharSeconds
        {
            get => progressClockUpperRightCornerCharSeconds;
            set => progressClockUpperRightCornerCharSeconds = value;
        }
        /// <summary>
        /// [ProgressClock] Lower left corner character for hours bar
        /// </summary>
        public char ProgressClockLowerLeftCornerCharHours
        {
            get => progressClockLowerLeftCornerCharHours;
            set => progressClockLowerLeftCornerCharHours = value;
        }
        /// <summary>
        /// [ProgressClock] Lower left corner character for minutes bar
        /// </summary>
        public char ProgressClockLowerLeftCornerCharMinutes
        {
            get => progressClockLowerLeftCornerCharMinutes;
            set => progressClockLowerLeftCornerCharMinutes = value;
        }
        /// <summary>
        /// [ProgressClock] Lower left corner character for seconds bar
        /// </summary>
        public char ProgressClockLowerLeftCornerCharSeconds
        {
            get => progressClockLowerLeftCornerCharSeconds;
            set => progressClockLowerLeftCornerCharSeconds = value;
        }
        /// <summary>
        /// [ProgressClock] Lower right corner character for hours bar
        /// </summary>
        public char ProgressClockLowerRightCornerCharHours
        {
            get => progressClockLowerRightCornerCharHours;
            set => progressClockLowerRightCornerCharHours = value;
        }
        /// <summary>
        /// [ProgressClock] Lower right corner character for minutes bar
        /// </summary>
        public char ProgressClockLowerRightCornerCharMinutes
        {
            get => progressClockLowerRightCornerCharMinutes;
            set => progressClockLowerRightCornerCharMinutes = value;
        }
        /// <summary>
        /// [ProgressClock] Lower right corner character for seconds bar
        /// </summary>
        public char ProgressClockLowerRightCornerCharSeconds
        {
            get => progressClockLowerRightCornerCharSeconds;
            set => progressClockLowerRightCornerCharSeconds = value;
        }
        /// <summary>
        /// [ProgressClock] Upper frame character for hours bar
        /// </summary>
        public char ProgressClockUpperFrameCharHours
        {
            get => progressClockUpperFrameCharHours;
            set => progressClockUpperFrameCharHours = value;
        }
        /// <summary>
        /// [ProgressClock] Upper frame character for minutes bar
        /// </summary>
        public char ProgressClockUpperFrameCharMinutes
        {
            get => progressClockUpperFrameCharMinutes;
            set => progressClockUpperFrameCharMinutes = value;
        }
        /// <summary>
        /// [ProgressClock] Upper frame character for seconds bar
        /// </summary>
        public char ProgressClockUpperFrameCharSeconds
        {
            get => progressClockUpperFrameCharSeconds;
            set => progressClockUpperFrameCharSeconds = value;
        }
        /// <summary>
        /// [ProgressClock] Lower frame character for hours bar
        /// </summary>
        public char ProgressClockLowerFrameCharHours
        {
            get => progressClockLowerFrameCharHours;
            set => progressClockLowerFrameCharHours = value;
        }
        /// <summary>
        /// [ProgressClock] Lower frame character for minutes bar
        /// </summary>
        public char ProgressClockLowerFrameCharMinutes
        {
            get => progressClockLowerFrameCharMinutes;
            set => progressClockLowerFrameCharMinutes = value;
        }
        /// <summary>
        /// [ProgressClock] Lower frame character for seconds bar
        /// </summary>
        public char ProgressClockLowerFrameCharSeconds
        {
            get => progressClockLowerFrameCharSeconds;
            set => progressClockLowerFrameCharSeconds = value;
        }
        /// <summary>
        /// [ProgressClock] Left frame character for hours bar
        /// </summary>
        public char ProgressClockLeftFrameCharHours
        {
            get => progressClockLeftFrameCharHours;
            set => progressClockLeftFrameCharHours = value;
        }
        /// <summary>
        /// [ProgressClock] Left frame character for minutes bar
        /// </summary>
        public char ProgressClockLeftFrameCharMinutes
        {
            get => progressClockLeftFrameCharMinutes;
            set => progressClockLeftFrameCharMinutes = value;
        }
        /// <summary>
        /// [ProgressClock] Left frame character for seconds bar
        /// </summary>
        public char ProgressClockLeftFrameCharSeconds
        {
            get => progressClockLeftFrameCharSeconds;
            set => progressClockLeftFrameCharSeconds = value;
        }
        /// <summary>
        /// [ProgressClock] Right frame character for hours bar
        /// </summary>
        public char ProgressClockRightFrameCharHours
        {
            get => progressClockRightFrameCharHours;
            set => progressClockRightFrameCharHours = value;
        }
        /// <summary>
        /// [ProgressClock] Right frame character for minutes bar
        /// </summary>
        public char ProgressClockRightFrameCharMinutes
        {
            get => progressClockRightFrameCharMinutes;
            set => progressClockRightFrameCharMinutes = value;
        }
        /// <summary>
        /// [ProgressClock] Right frame character for seconds bar
        /// </summary>
        public char ProgressClockRightFrameCharSeconds
        {
            get => progressClockRightFrameCharSeconds;
            set => progressClockRightFrameCharSeconds = value;
        }
        /// <summary>
        /// [ProgressClock] Information text for hours bar
        /// </summary>
        public string ProgressClockInfoTextHours
        {
            get => progressClockInfoTextHours;
            set => progressClockInfoTextHours = value;
        }
        /// <summary>
        /// [ProgressClock] Information text for minutes bar
        /// </summary>
        public string ProgressClockInfoTextMinutes
        {
            get => progressClockInfoTextMinutes;
            set => progressClockInfoTextMinutes = value;
        }
        /// <summary>
        /// [ProgressClock] Information text for seconds bar
        /// </summary>
        public string ProgressClockInfoTextSeconds
        {
            get => progressClockInfoTextSeconds;
            set => progressClockInfoTextSeconds = value;
        }
        /// <summary>
        /// [ProgressClock] The minimum red color level (true color - hours)
        /// </summary>
        public int ProgressClockMinimumRedColorLevelHours
        {
            get => progressClockMinimumRedColorLevelHours;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                progressClockMinimumRedColorLevelHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum green color level (true color - hours)
        /// </summary>
        public int ProgressClockMinimumGreenColorLevelHours
        {
            get => progressClockMinimumGreenColorLevelHours;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                progressClockMinimumGreenColorLevelHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum blue color level (true color - hours)
        /// </summary>
        public int ProgressClockMinimumBlueColorLevelHours
        {
            get => progressClockMinimumBlueColorLevelHours;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                progressClockMinimumBlueColorLevelHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum color level (255 colors or 16 colors - hours)
        /// </summary>
        public int ProgressClockMinimumColorLevelHours
        {
            get => progressClockMinimumColorLevelHours;
            set
            {
                int FinalMinimumLevel = 255;
                if (value < 0)
                    value = 1;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                progressClockMinimumColorLevelHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum red color level (true color - hours)
        /// </summary>
        public int ProgressClockMaximumRedColorLevelHours
        {
            get => progressClockMaximumRedColorLevelHours;
            set
            {
                if (value <= progressClockMinimumRedColorLevelHours)
                    value = progressClockMinimumRedColorLevelHours;
                if (value > 255)
                    value = 255;
                progressClockMaximumRedColorLevelHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum green color level (true color - hours)
        /// </summary>
        public int ProgressClockMaximumGreenColorLevelHours
        {
            get => progressClockMaximumGreenColorLevelHours;
            set
            {
                if (value <= progressClockMinimumGreenColorLevelHours)
                    value = progressClockMinimumGreenColorLevelHours;
                if (value > 255)
                    value = 255;
                progressClockMaximumGreenColorLevelHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum blue color level (true color - hours)
        /// </summary>
        public int ProgressClockMaximumBlueColorLevelHours
        {
            get => progressClockMaximumBlueColorLevelHours;
            set
            {
                if (value <= progressClockMinimumBlueColorLevelHours)
                    value = progressClockMinimumBlueColorLevelHours;
                if (value > 255)
                    value = 255;
                progressClockMaximumBlueColorLevelHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum color level (255 colors or 16 colors - hours)
        /// </summary>
        public int ProgressClockMaximumColorLevelHours
        {
            get => progressClockMaximumColorLevelHours;
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= progressClockMinimumColorLevelHours)
                    value = progressClockMinimumColorLevelHours;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                progressClockMaximumColorLevelHours = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum red color level (true color - minutes)
        /// </summary>
        public int ProgressClockMinimumRedColorLevelMinutes
        {
            get => progressClockMinimumRedColorLevelMinutes;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                progressClockMinimumRedColorLevelMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum green color level (true color - minutes)
        /// </summary>
        public int ProgressClockMinimumGreenColorLevelMinutes
        {
            get => progressClockMinimumGreenColorLevelMinutes;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                progressClockMinimumGreenColorLevelMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum blue color level (true color - minutes)
        /// </summary>
        public int ProgressClockMinimumBlueColorLevelMinutes
        {
            get => progressClockMinimumBlueColorLevelMinutes;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                progressClockMinimumBlueColorLevelMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum color level (255 colors or 16 colors - minutes)
        /// </summary>
        public int ProgressClockMinimumColorLevelMinutes
        {
            get => progressClockMinimumColorLevelMinutes;
            set
            {
                int FinalMinimumLevel = 255;
                if (value < 0)
                    value = 1;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                progressClockMinimumColorLevelMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum red color level (true color - minutes)
        /// </summary>
        public int ProgressClockMaximumRedColorLevelMinutes
        {
            get => progressClockMaximumRedColorLevelMinutes;
            set
            {
                if (value <= progressClockMinimumRedColorLevelMinutes)
                    value = progressClockMinimumRedColorLevelMinutes;
                if (value > 255)
                    value = 255;
                progressClockMaximumRedColorLevelMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum green color level (true color - minutes)
        /// </summary>
        public int ProgressClockMaximumGreenColorLevelMinutes
        {
            get => progressClockMaximumGreenColorLevelMinutes;
            set
            {
                if (value <= progressClockMinimumGreenColorLevelMinutes)
                    value = progressClockMinimumGreenColorLevelMinutes;
                if (value > 255)
                    value = 255;
                progressClockMaximumGreenColorLevelMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum blue color level (true color - minutes)
        /// </summary>
        public int ProgressClockMaximumBlueColorLevelMinutes
        {
            get => progressClockMaximumBlueColorLevelMinutes;
            set
            {
                if (value <= progressClockMinimumBlueColorLevelMinutes)
                    value = progressClockMinimumBlueColorLevelMinutes;
                if (value > 255)
                    value = 255;
                progressClockMaximumBlueColorLevelMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum color level (255 colors or 16 colors - minutes)
        /// </summary>
        public int ProgressClockMaximumColorLevelMinutes
        {
            get => progressClockMaximumColorLevelMinutes;
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= progressClockMinimumColorLevelMinutes)
                    value = progressClockMinimumColorLevelMinutes;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                progressClockMaximumColorLevelMinutes = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum red color level (true color - seconds)
        /// </summary>
        public int ProgressClockMinimumRedColorLevelSeconds
        {
            get => progressClockMinimumRedColorLevelSeconds;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                progressClockMinimumRedColorLevelSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum green color level (true color - seconds)
        /// </summary>
        public int ProgressClockMinimumGreenColorLevelSeconds
        {
            get => progressClockMinimumGreenColorLevelSeconds;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                progressClockMinimumGreenColorLevelSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum blue color level (true color - seconds)
        /// </summary>
        public int ProgressClockMinimumBlueColorLevelSeconds
        {
            get => progressClockMinimumBlueColorLevelSeconds;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                progressClockMinimumBlueColorLevelSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum color level (255 colors or 16 colors - seconds)
        /// </summary>
        public int ProgressClockMinimumColorLevelSeconds
        {
            get => progressClockMinimumColorLevelSeconds;
            set
            {
                int FinalMinimumLevel = 255;
                if (value < 0)
                    value = 1;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                progressClockMinimumColorLevelSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum red color level (true color - seconds)
        /// </summary>
        public int ProgressClockMaximumRedColorLevelSeconds
        {
            get => progressClockMaximumRedColorLevelSeconds;
            set
            {
                if (value <= progressClockMinimumRedColorLevelSeconds)
                    value = progressClockMinimumRedColorLevelSeconds;
                if (value > 255)
                    value = 255;
                progressClockMaximumRedColorLevelSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum green color level (true color - seconds)
        /// </summary>
        public int ProgressClockMaximumGreenColorLevelSeconds
        {
            get => progressClockMaximumGreenColorLevelSeconds;
            set
            {
                if (value <= progressClockMinimumGreenColorLevelSeconds)
                    value = progressClockMinimumGreenColorLevelSeconds;
                if (value > 255)
                    value = 255;
                progressClockMaximumGreenColorLevelSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum blue color level (true color - seconds)
        /// </summary>
        public int ProgressClockMaximumBlueColorLevelSeconds
        {
            get => progressClockMaximumBlueColorLevelSeconds;
            set
            {
                if (value <= progressClockMinimumBlueColorLevelSeconds)
                    value = progressClockMinimumBlueColorLevelSeconds;
                if (value > 255)
                    value = 255;
                progressClockMaximumBlueColorLevelSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum color level (255 colors or 16 colors - seconds)
        /// </summary>
        public int ProgressClockMaximumColorLevelSeconds
        {
            get => progressClockMaximumColorLevelSeconds;
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= progressClockMinimumColorLevelSeconds)
                    value = progressClockMinimumColorLevelSeconds;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                progressClockMaximumColorLevelSeconds = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum red color level (true color)
        /// </summary>
        public int ProgressClockMinimumRedColorLevel
        {
            get => progressClockMinimumRedColorLevel;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                progressClockMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum green color level (true color)
        /// </summary>
        public int ProgressClockMinimumGreenColorLevel
        {
            get => progressClockMinimumGreenColorLevel;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                progressClockMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum blue color level (true color)
        /// </summary>
        public int ProgressClockMinimumBlueColorLevel
        {
            get => progressClockMinimumBlueColorLevel;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                progressClockMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int ProgressClockMinimumColorLevel
        {
            get => progressClockMinimumColorLevel;
            set
            {
                int FinalMinimumLevel = 255;
                if (value < 0)
                    value = 1;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                progressClockMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum red color level (true color)
        /// </summary>
        public int ProgressClockMaximumRedColorLevel
        {
            get => progressClockMaximumRedColorLevel;
            set
            {
                if (value <= progressClockMinimumRedColorLevel)
                    value = progressClockMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                progressClockMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum green color level (true color)
        /// </summary>
        public int ProgressClockMaximumGreenColorLevel
        {
            get => progressClockMaximumGreenColorLevel;
            set
            {
                if (value <= progressClockMinimumGreenColorLevel)
                    value = progressClockMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                progressClockMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum blue color level (true color)
        /// </summary>
        public int ProgressClockMaximumBlueColorLevel
        {
            get => progressClockMaximumBlueColorLevel;
            set
            {
                if (value <= progressClockMinimumBlueColorLevel)
                    value = progressClockMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                progressClockMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ProgressClock] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int ProgressClockMaximumColorLevel
        {
            get => progressClockMaximumColorLevel;
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= progressClockMinimumColorLevel)
                    value = progressClockMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                progressClockMaximumColorLevel = value;
            }
        }
    }
}
