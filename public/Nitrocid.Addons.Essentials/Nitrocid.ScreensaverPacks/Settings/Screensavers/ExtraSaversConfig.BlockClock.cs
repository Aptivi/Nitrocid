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
        private bool blockClockTrueColor = true;
        private int blockClockDelay = 1000;
        private int blockClockMinimumRedColorLevel = 0;
        private int blockClockMinimumGreenColorLevel = 0;
        private int blockClockMinimumBlueColorLevel = 0;
        private int blockClockMinimumColorLevel = 0;
        private int blockClockMaximumRedColorLevel = 255;
        private int blockClockMaximumGreenColorLevel = 255;
        private int blockClockMaximumBlueColorLevel = 255;
        private int blockClockMaximumColorLevel = 255;

        /// <summary>
        /// [BlockClock] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool BlockClockTrueColor
        {
            get
            {
                return blockClockTrueColor;
            }
            set
            {
                blockClockTrueColor = value;
            }
        }
        /// <summary>
        /// [BlockClock] How many milliseconds to wait before making the next write?
        /// </summary>
        public int BlockClockDelay
        {
            get
            {
                return blockClockDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                blockClockDelay = value;
            }
        }
        /// <summary>
        /// [BlockClock] The minimum red color level (true color)
        /// </summary>
        public int BlockClockMinimumRedColorLevel
        {
            get
            {
                return blockClockMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                blockClockMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BlockClock] The minimum green color level (true color)
        /// </summary>
        public int BlockClockMinimumGreenColorLevel
        {
            get
            {
                return blockClockMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                blockClockMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BlockClock] The minimum blue color level (true color)
        /// </summary>
        public int BlockClockMinimumBlueColorLevel
        {
            get
            {
                return blockClockMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                blockClockMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BlockClock] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int BlockClockMinimumColorLevel
        {
            get
            {
                return blockClockMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                blockClockMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [BlockClock] The maximum red color level (true color)
        /// </summary>
        public int BlockClockMaximumRedColorLevel
        {
            get
            {
                return blockClockMaximumRedColorLevel;
            }
            set
            {
                if (value <= blockClockMinimumRedColorLevel)
                    value = blockClockMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                blockClockMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BlockClock] The maximum green color level (true color)
        /// </summary>
        public int BlockClockMaximumGreenColorLevel
        {
            get
            {
                return blockClockMaximumGreenColorLevel;
            }
            set
            {
                if (value <= blockClockMinimumGreenColorLevel)
                    value = blockClockMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                blockClockMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BlockClock] The maximum blue color level (true color)
        /// </summary>
        public int BlockClockMaximumBlueColorLevel
        {
            get
            {
                return blockClockMaximumBlueColorLevel;
            }
            set
            {
                if (value <= blockClockMinimumBlueColorLevel)
                    value = blockClockMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                blockClockMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BlockClock] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int BlockClockMaximumColorLevel
        {
            get
            {
                return blockClockMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= blockClockMinimumColorLevel)
                    value = blockClockMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                blockClockMaximumColorLevel = value;
            }
        }
    }
}
