using System;
using System.Collections.Generic;
using ColorSeq;
using KS.ConsoleBase.Colors;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;

// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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

using KS.TimeDate;

namespace KS.Misc.Screensaver.Displays
{
    public static class DateAndTimeSettings
    {

        private static bool _dateAndTime255Colors;
        private static bool _dateAndTimeTrueColor = true;
        private static int _dateAndTimeDelay = 1000;
        private static int _dateAndTimeMinimumRedColorLevel = 0;
        private static int _dateAndTimeMinimumGreenColorLevel = 0;
        private static int _dateAndTimeMinimumBlueColorLevel = 0;
        private static int _dateAndTimeMinimumColorLevel = 0;
        private static int _dateAndTimeMaximumRedColorLevel = 255;
        private static int _dateAndTimeMaximumGreenColorLevel = 255;
        private static int _dateAndTimeMaximumBlueColorLevel = 255;
        private static int _dateAndTimeMaximumColorLevel = 255;

        /// <summary>
        /// [DateAndTime] Enable 255 color support. Has a higher priority than 16 color support.
        /// </summary>
        public static bool DateAndTime255Colors
        {
            get
            {
                return _dateAndTime255Colors;
            }
            set
            {
                _dateAndTime255Colors = value;
            }
        }
        /// <summary>
        /// [DateAndTime] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool DateAndTimeTrueColor
        {
            get
            {
                return _dateAndTimeTrueColor;
            }
            set
            {
                _dateAndTimeTrueColor = value;
            }
        }
        /// <summary>
        /// [DateAndTime] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int DateAndTimeDelay
        {
            get
            {
                return _dateAndTimeDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                _dateAndTimeDelay = value;
            }
        }
        /// <summary>
        /// [DateAndTime] The minimum red color level (true color)
        /// </summary>
        public static int DateAndTimeMinimumRedColorLevel
        {
            get
            {
                return _dateAndTimeMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _dateAndTimeMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [DateAndTime] The minimum green color level (true color)
        /// </summary>
        public static int DateAndTimeMinimumGreenColorLevel
        {
            get
            {
                return _dateAndTimeMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _dateAndTimeMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [DateAndTime] The minimum blue color level (true color)
        /// </summary>
        public static int DateAndTimeMinimumBlueColorLevel
        {
            get
            {
                return _dateAndTimeMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _dateAndTimeMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [DateAndTime] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int DateAndTimeMinimumColorLevel
        {
            get
            {
                return _dateAndTimeMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = _dateAndTime255Colors | _dateAndTimeTrueColor ? 255 : 15;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                _dateAndTimeMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [DateAndTime] The maximum red color level (true color)
        /// </summary>
        public static int DateAndTimeMaximumRedColorLevel
        {
            get
            {
                return _dateAndTimeMaximumRedColorLevel;
            }
            set
            {
                if (value <= _dateAndTimeMinimumRedColorLevel)
                    value = _dateAndTimeMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                _dateAndTimeMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [DateAndTime] The maximum green color level (true color)
        /// </summary>
        public static int DateAndTimeMaximumGreenColorLevel
        {
            get
            {
                return _dateAndTimeMaximumGreenColorLevel;
            }
            set
            {
                if (value <= _dateAndTimeMinimumGreenColorLevel)
                    value = _dateAndTimeMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                _dateAndTimeMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [DateAndTime] The maximum blue color level (true color)
        /// </summary>
        public static int DateAndTimeMaximumBlueColorLevel
        {
            get
            {
                return _dateAndTimeMaximumBlueColorLevel;
            }
            set
            {
                if (value <= _dateAndTimeMinimumBlueColorLevel)
                    value = _dateAndTimeMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                _dateAndTimeMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [DateAndTime] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int DateAndTimeMaximumColorLevel
        {
            get
            {
                return _dateAndTimeMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = _dateAndTime255Colors | _dateAndTimeTrueColor ? 255 : 15;
                if (value <= _dateAndTimeMinimumColorLevel)
                    value = _dateAndTimeMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                _dateAndTimeMaximumColorLevel = value;
            }
        }

    }

    public class DateAndTimeDisplay : BaseScreensaver, IScreensaver
    {

        private Random RandomDriver;

        public override string ScreensaverName { get; set; } = "DateAndTime";

        public override Dictionary<string, object> ScreensaverSettings { get; set; }

        public override void ScreensaverPreparation()
        {
            // Variable preparations
            RandomDriver = new Random();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
        }

        public override void ScreensaverLogic()
        {
            Console.CursorVisible = false;
            Console.Clear();

            // Write date and time
            ColorTools.SetConsoleColor(ChangeDateAndTimeColor());
            TextWriterWhereColor.WriteWherePlain(TimeDateRenderers.RenderDate(), (int)Math.Round(Console.WindowWidth / 2d - TimeDateRenderers.RenderDate().Length / 2d), (int)Math.Round(Console.WindowHeight / 2d - 1d));
            TextWriterWhereColor.WriteWherePlain(TimeDateRenderers.RenderTime(), (int)Math.Round(Console.WindowWidth / 2d - TimeDateRenderers.RenderTime().Length / 2d), (int)Math.Round(Console.WindowHeight / 2d));

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
                int RedColorNum = RandomDriver.Next(DateAndTimeSettings.DateAndTimeMinimumRedColorLevel, DateAndTimeSettings.DateAndTimeMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Next(DateAndTimeSettings.DateAndTimeMinimumGreenColorLevel, DateAndTimeSettings.DateAndTimeMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Next(DateAndTimeSettings.DateAndTimeMinimumBlueColorLevel, DateAndTimeSettings.DateAndTimeMaximumBlueColorLevel);
                ColorInstance = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else if (DateAndTimeSettings.DateAndTime255Colors)
            {
                int ColorNum = RandomDriver.Next(DateAndTimeSettings.DateAndTimeMinimumColorLevel, DateAndTimeSettings.DateAndTimeMaximumColorLevel);
                ColorInstance = new Color(ColorNum);
            }
            else
            {
                ColorInstance = new Color((int)Screensaver.colors[RandomDriver.Next(DateAndTimeSettings.DateAndTimeMinimumColorLevel, DateAndTimeSettings.DateAndTimeMaximumColorLevel)]);
            }
            return ColorInstance;
        }

    }
}