
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
using KS.Drivers.RNG;
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
                int FinalMinimumLevel = 255;
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
                int FinalMaximumLevel = 255;
                if (value <= _dateAndTimeMinimumColorLevel)
                    value = _dateAndTimeMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                _dateAndTimeMaximumColorLevel = value;
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
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ConsoleBase.ConsoleWrapper.BackgroundColor = ConsoleColor.Black;
            ConsoleBase.ConsoleWrapper.Clear();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleBase.ConsoleWrapper.CursorVisible = false;
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
