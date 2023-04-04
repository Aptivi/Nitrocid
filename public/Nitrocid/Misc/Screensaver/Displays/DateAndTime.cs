
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
using KS.Drivers.RNG;
using KS.Kernel.Configuration;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using KS.TimeDate;
using ColorTools = KS.ConsoleBase.Colors.ColorTools;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for DateAndTime
    /// </summary>
    public static class DateAndTimeSettings
    {

        /// <summary>
        /// [DateAndTime] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool DateAndTimeTrueColor
        {
            get
            {
                return Config.SaverConfig.DateAndTimeTrueColor;
            }
            set
            {
                Config.SaverConfig.DateAndTimeTrueColor = value;
            }
        }
        /// <summary>
        /// [DateAndTime] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int DateAndTimeDelay
        {
            get
            {
                return Config.SaverConfig.DateAndTimeDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                Config.SaverConfig.DateAndTimeDelay = value;
            }
        }
        /// <summary>
        /// [DateAndTime] The minimum red color level (true color)
        /// </summary>
        public static int DateAndTimeMinimumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.DateAndTimeMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.DateAndTimeMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [DateAndTime] The minimum green color level (true color)
        /// </summary>
        public static int DateAndTimeMinimumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.DateAndTimeMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.DateAndTimeMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [DateAndTime] The minimum blue color level (true color)
        /// </summary>
        public static int DateAndTimeMinimumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.DateAndTimeMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.DateAndTimeMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [DateAndTime] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int DateAndTimeMinimumColorLevel
        {
            get
            {
                return Config.SaverConfig.DateAndTimeMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                Config.SaverConfig.DateAndTimeMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [DateAndTime] The maximum red color level (true color)
        /// </summary>
        public static int DateAndTimeMaximumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.DateAndTimeMaximumRedColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.DateAndTimeMinimumRedColorLevel)
                    value = Config.SaverConfig.DateAndTimeMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.DateAndTimeMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [DateAndTime] The maximum green color level (true color)
        /// </summary>
        public static int DateAndTimeMaximumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.DateAndTimeMaximumGreenColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.DateAndTimeMinimumGreenColorLevel)
                    value = Config.SaverConfig.DateAndTimeMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.DateAndTimeMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [DateAndTime] The maximum blue color level (true color)
        /// </summary>
        public static int DateAndTimeMaximumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.DateAndTimeMaximumBlueColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.DateAndTimeMinimumBlueColorLevel)
                    value = Config.SaverConfig.DateAndTimeMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.DateAndTimeMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [DateAndTime] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int DateAndTimeMaximumColorLevel
        {
            get
            {
                return Config.SaverConfig.DateAndTimeMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= Config.SaverConfig.DateAndTimeMinimumColorLevel)
                    value = Config.SaverConfig.DateAndTimeMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                Config.SaverConfig.DateAndTimeMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for DateAndTime
    /// </summary>
    public class DateAndTimeDisplay : BaseScreensaver, IScreensaver
    {


        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "DateAndTime";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleBase.ConsoleWrapper.Clear();

            // Write date and time
            ColorTools.SetConsoleColor(ChangeDateAndTimeColor());
            TextWriterWhereColor.WriteWhere(TimeDateRenderers.RenderDate(), (int)Math.Round(ConsoleBase.ConsoleWrapper.WindowWidth / 2d - TimeDateRenderers.RenderDate().Length / 2d), (int)Math.Round(ConsoleBase.ConsoleWrapper.WindowHeight / 2d - 1d));
            TextWriterWhereColor.WriteWhere(TimeDateRenderers.RenderTime(), (int)Math.Round(ConsoleBase.ConsoleWrapper.WindowWidth / 2d - TimeDateRenderers.RenderTime().Length / 2d), (int)Math.Round(ConsoleBase.ConsoleWrapper.WindowHeight / 2d));

            // Delay
            ThreadManager.SleepNoBlock(DateAndTimeSettings.DateAndTimeDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

        /// <summary>
        /// Changes the color of date and time
        /// </summary>
        public Color ChangeDateAndTimeColor()
        {
            Color ColorInstance;
            if (DateAndTimeSettings.DateAndTimeTrueColor)
            {
                int RedColorNum = RandomDriver.Random(DateAndTimeSettings.DateAndTimeMinimumRedColorLevel, DateAndTimeSettings.DateAndTimeMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(DateAndTimeSettings.DateAndTimeMinimumGreenColorLevel, DateAndTimeSettings.DateAndTimeMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(DateAndTimeSettings.DateAndTimeMinimumBlueColorLevel, DateAndTimeSettings.DateAndTimeMaximumBlueColorLevel);
                ColorInstance = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(DateAndTimeSettings.DateAndTimeMinimumColorLevel, DateAndTimeSettings.DateAndTimeMaximumColorLevel);
                ColorInstance = new Color(ColorNum);
            }
            return ColorInstance;
        }

    }
}
