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

namespace Nitrocid.ScreensaverPacks.Settings
{
    /// <summary>
    /// Screensaver kernel configuration instance
    /// </summary>
    public partial class ExtraSaversConfig : BaseKernelConfig
    {
        private bool worldClockTrueColor = true;
        private int worldClockDelay = 1000;
        private int worldClockNextZoneRefreshes = 5;
        private int worldClockMinimumRedColorLevel = 0;
        private int worldClockMinimumGreenColorLevel = 0;
        private int worldClockMinimumBlueColorLevel = 0;
        private int worldClockMinimumColorLevel = 0;
        private int worldClockMaximumRedColorLevel = 255;
        private int worldClockMaximumGreenColorLevel = 255;
        private int worldClockMaximumBlueColorLevel = 255;
        private int worldClockMaximumColorLevel = 255;

        /// <summary>
        /// [WorldClock] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool WorldClockTrueColor
        {
            get
            {
                return worldClockTrueColor;
            }
            set
            {
                worldClockTrueColor = value;
            }
        }
        /// <summary>
        /// [WorldClock] How many milliseconds to wait before making the next write?
        /// </summary>
        public int WorldClockDelay
        {
            get
            {
                return worldClockDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                worldClockDelay = value;
            }
        }
        /// <summary>
        /// [WorldClock] How many refreshes before making the next write?
        /// </summary>
        public int WorldClockNextZoneRefreshes
        {
            get
            {
                return worldClockNextZoneRefreshes;
            }
            set
            {
                if (value <= 0)
                    value = 5;
                worldClockNextZoneRefreshes = value;
            }
        }
        /// <summary>
        /// [WorldClock] The minimum red color level (true color)
        /// </summary>
        public int WorldClockMinimumRedColorLevel
        {
            get
            {
                return worldClockMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                worldClockMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [WorldClock] The minimum green color level (true color)
        /// </summary>
        public int WorldClockMinimumGreenColorLevel
        {
            get
            {
                return worldClockMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                worldClockMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [WorldClock] The minimum blue color level (true color)
        /// </summary>
        public int WorldClockMinimumBlueColorLevel
        {
            get
            {
                return worldClockMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                worldClockMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [WorldClock] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int WorldClockMinimumColorLevel
        {
            get
            {
                return worldClockMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                worldClockMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [WorldClock] The maximum red color level (true color)
        /// </summary>
        public int WorldClockMaximumRedColorLevel
        {
            get
            {
                return worldClockMaximumRedColorLevel;
            }
            set
            {
                if (value <= worldClockMinimumRedColorLevel)
                    value = worldClockMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                worldClockMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [WorldClock] The maximum green color level (true color)
        /// </summary>
        public int WorldClockMaximumGreenColorLevel
        {
            get
            {
                return worldClockMaximumGreenColorLevel;
            }
            set
            {
                if (value <= worldClockMinimumGreenColorLevel)
                    value = worldClockMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                worldClockMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [WorldClock] The maximum blue color level (true color)
        /// </summary>
        public int WorldClockMaximumBlueColorLevel
        {
            get
            {
                return worldClockMaximumBlueColorLevel;
            }
            set
            {
                if (value <= worldClockMinimumBlueColorLevel)
                    value = worldClockMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                worldClockMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [WorldClock] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int WorldClockMaximumColorLevel
        {
            get
            {
                return worldClockMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= worldClockMinimumColorLevel)
                    value = worldClockMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                worldClockMaximumColorLevel = value;
            }
        }
    }
}
