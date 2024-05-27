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
using Textify.Figlet;
using Terminaux.Colors;
using Nitrocid.Misc.Screensaver;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Kernel.Time.Timezones;
using Nitrocid.Drivers.RNG;
using Terminaux.Writer.FancyWriters;
using Nitrocid.Kernel.Threading;
using Terminaux.Base;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Settings for WorldClock
    /// </summary>
    public static class WorldClockSettings
    {

        /// <summary>
        /// [WorldClock] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool WorldClockTrueColor
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.WorldClockTrueColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.WorldClockTrueColor = value;
            }
        }
        /// <summary>
        /// [WorldClock] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int WorldClockDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.WorldClockDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                ScreensaverPackInit.SaversConfig.WorldClockDelay = value;
            }
        }
        /// <summary>
        /// [WorldClock] How many refreshes before making the next write?
        /// </summary>
        public static int WorldClockNextZoneRefreshes
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.WorldClockNextZoneRefreshes;
            }
            set
            {
                if (value <= 0)
                    value = 5;
                ScreensaverPackInit.SaversConfig.WorldClockNextZoneRefreshes = value;
            }
        }
        /// <summary>
        /// [WorldClock] The minimum red color level (true color)
        /// </summary>
        public static int WorldClockMinimumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.WorldClockMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.WorldClockMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [WorldClock] The minimum green color level (true color)
        /// </summary>
        public static int WorldClockMinimumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.WorldClockMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.WorldClockMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [WorldClock] The minimum blue color level (true color)
        /// </summary>
        public static int WorldClockMinimumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.WorldClockMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.WorldClockMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [WorldClock] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int WorldClockMinimumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.WorldClockMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                ScreensaverPackInit.SaversConfig.WorldClockMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [WorldClock] The maximum red color level (true color)
        /// </summary>
        public static int WorldClockMaximumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.WorldClockMaximumRedColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.WorldClockMinimumRedColorLevel)
                    value = ScreensaverPackInit.SaversConfig.WorldClockMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.WorldClockMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [WorldClock] The maximum green color level (true color)
        /// </summary>
        public static int WorldClockMaximumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.WorldClockMaximumGreenColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.WorldClockMinimumGreenColorLevel)
                    value = ScreensaverPackInit.SaversConfig.WorldClockMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.WorldClockMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [WorldClock] The maximum blue color level (true color)
        /// </summary>
        public static int WorldClockMaximumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.WorldClockMaximumBlueColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.WorldClockMinimumBlueColorLevel)
                    value = ScreensaverPackInit.SaversConfig.WorldClockMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.WorldClockMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [WorldClock] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int WorldClockMaximumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.WorldClockMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= ScreensaverPackInit.SaversConfig.WorldClockMinimumColorLevel)
                    value = ScreensaverPackInit.SaversConfig.WorldClockMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                ScreensaverPackInit.SaversConfig.WorldClockMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for WorldClock
    /// </summary>
    public class WorldClockDisplay : BaseScreensaver, IScreensaver
    {
        string timeZoneName;
        int times;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "WorldClock";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            base.ScreensaverPreparation();
            var timeZone = TimeZones.GetTimeZoneNames();
            timeZoneName = timeZone[RandomDriver.RandomIdx(timeZone.Length)];
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Change color
            var color = ChangeWorldClockColor();

            // Write word and hash
            if (times > WorldClockSettings.WorldClockNextZoneRefreshes)
            {
                times = 0;
                var timeZone = TimeZones.GetTimeZoneNames();
                timeZoneName = timeZone[RandomDriver.RandomIdx(timeZone.Length)];
            }
            string time = TimeZoneRenderers.GetZoneTimeTimeString(timeZoneName);
            string date = TimeZoneRenderers.GetZoneTimeDateString(timeZoneName);
            var figFont = FigletTools.GetFigletFont("small");
            int figHeight = FigletTools.GetFigletHeight(time, figFont) / 2;
            int consoleY = ConsoleWrapper.WindowHeight / 2 - figHeight;
            int hashY = ConsoleWrapper.WindowHeight / 2 + figHeight + 2;
            ConsoleWrapper.Clear();
            CenteredFigletTextColor.WriteCenteredFigletColor(consoleY, figFont, time, color);
            TextWriterWhereColor.WriteWhereColor($"{date} @ {timeZoneName}", (int)Math.Round(ConsoleWrapper.WindowWidth / 2d - $"{date} @ {timeZoneName}".Length / 2d), hashY, color);

            // Delay
            ThreadManager.SleepNoBlock(WorldClockSettings.WorldClockDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            times++;
        }

        /// <summary>
        /// Changes the color of word and its hash
        /// </summary>
        private Color ChangeWorldClockColor()
        {
            Color ColorInstance;
            if (WorldClockSettings.WorldClockTrueColor)
            {
                int RedColorNum = RandomDriver.Random(WorldClockSettings.WorldClockMinimumRedColorLevel, WorldClockSettings.WorldClockMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(WorldClockSettings.WorldClockMinimumGreenColorLevel, WorldClockSettings.WorldClockMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(WorldClockSettings.WorldClockMinimumBlueColorLevel, WorldClockSettings.WorldClockMaximumBlueColorLevel);
                ColorInstance = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(WorldClockSettings.WorldClockMinimumColorLevel, WorldClockSettings.WorldClockMaximumColorLevel);
                ColorInstance = new Color(ColorNum);
            }
            return ColorInstance;
        }

    }
}
