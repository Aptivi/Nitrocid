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

using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Threading;
using Nitrocid.Kernel.Time.Renderers;
using Nitrocid.Misc.Screensaver;
using Terminaux.Colors;
using Terminaux.Base;

namespace Nitrocid.ScreensaverPacks.Screensavers
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
                return ScreensaverPackInit.SaversConfig.DateAndTimeTrueColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.DateAndTimeTrueColor = value;
            }
        }
        /// <summary>
        /// [DateAndTime] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int DateAndTimeDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.DateAndTimeDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                ScreensaverPackInit.SaversConfig.DateAndTimeDelay = value;
            }
        }
        /// <summary>
        /// [DateAndTime] The minimum red color level (true color)
        /// </summary>
        public static int DateAndTimeMinimumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.DateAndTimeMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.DateAndTimeMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [DateAndTime] The minimum green color level (true color)
        /// </summary>
        public static int DateAndTimeMinimumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.DateAndTimeMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.DateAndTimeMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [DateAndTime] The minimum blue color level (true color)
        /// </summary>
        public static int DateAndTimeMinimumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.DateAndTimeMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.DateAndTimeMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [DateAndTime] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int DateAndTimeMinimumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.DateAndTimeMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                ScreensaverPackInit.SaversConfig.DateAndTimeMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [DateAndTime] The maximum red color level (true color)
        /// </summary>
        public static int DateAndTimeMaximumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.DateAndTimeMaximumRedColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.DateAndTimeMinimumRedColorLevel)
                    value = ScreensaverPackInit.SaversConfig.DateAndTimeMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.DateAndTimeMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [DateAndTime] The maximum green color level (true color)
        /// </summary>
        public static int DateAndTimeMaximumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.DateAndTimeMaximumGreenColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.DateAndTimeMinimumGreenColorLevel)
                    value = ScreensaverPackInit.SaversConfig.DateAndTimeMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.DateAndTimeMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [DateAndTime] The maximum blue color level (true color)
        /// </summary>
        public static int DateAndTimeMaximumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.DateAndTimeMaximumBlueColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.DateAndTimeMinimumBlueColorLevel)
                    value = ScreensaverPackInit.SaversConfig.DateAndTimeMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.DateAndTimeMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [DateAndTime] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int DateAndTimeMaximumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.DateAndTimeMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= ScreensaverPackInit.SaversConfig.DateAndTimeMinimumColorLevel)
                    value = ScreensaverPackInit.SaversConfig.DateAndTimeMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                ScreensaverPackInit.SaversConfig.DateAndTimeMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for DateAndTime
    /// </summary>
    public class DateAndTimeDisplay : BaseScreensaver, IScreensaver
    {

        private string lastRenderedDate = "";
        private string lastRenderedTime = "";

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "DateAndTime";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Get the color and positions
            Color timeColor = ChangeDateAndTimeColor();
            string renderedDate = TimeDateRenderers.RenderDate();
            string renderedTime = TimeDateRenderers.RenderTime();
            int halfConsoleY = (int)(ConsoleWrapper.WindowHeight / 2d);
            int datePosX = ConsoleWrapper.WindowWidth / 2 - renderedDate.Length / 2;
            int timePosX = ConsoleWrapper.WindowWidth / 2 - renderedTime.Length / 2;

            // Clear old date/time
            int oldDatePosX = ConsoleWrapper.WindowWidth / 2 - lastRenderedDate.Length / 2;
            int oldTimePosX = ConsoleWrapper.WindowWidth / 2 - lastRenderedTime.Length / 2;
            TextWriterWhereColor.WriteWhereColor(new string(' ', lastRenderedDate.Length), oldDatePosX, halfConsoleY, timeColor);
            TextWriterWhereColor.WriteWhereColor(new string(' ', lastRenderedTime.Length), oldTimePosX, halfConsoleY + 1, timeColor);

            // Write date and time
            TextWriterWhereColor.WriteWhereColor(renderedDate, datePosX, halfConsoleY, timeColor);
            TextWriterWhereColor.WriteWhereColor(renderedTime, timePosX, halfConsoleY + 1, timeColor);

            // Delay
            lastRenderedDate = renderedDate;
            lastRenderedTime = renderedTime;
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
