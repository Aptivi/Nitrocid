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
        private bool digitalClockTrueColor = true;
        private int digitalClockDelay = 1000;
        private int digitalClockMinimumRedColorLevel = 0;
        private int digitalClockMinimumGreenColorLevel = 0;
        private int digitalClockMinimumBlueColorLevel = 0;
        private int digitalClockMinimumColorLevel = 0;
        private int digitalClockMaximumRedColorLevel = 255;
        private int digitalClockMaximumGreenColorLevel = 255;
        private int digitalClockMaximumBlueColorLevel = 255;
        private int digitalClockMaximumColorLevel = 255;

        /// <summary>
        /// [DigitalClock] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool DigitalClockTrueColor
        {
            get
            {
                return digitalClockTrueColor;
            }
            set
            {
                digitalClockTrueColor = value;
            }
        }
        /// <summary>
        /// [DigitalClock] How many milliseconds to wait before making the next write?
        /// </summary>
        public int DigitalClockDelay
        {
            get
            {
                return digitalClockDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                digitalClockDelay = value;
            }
        }
        /// <summary>
        /// [DigitalClock] The minimum red color level (true color)
        /// </summary>
        public int DigitalClockMinimumRedColorLevel
        {
            get
            {
                return digitalClockMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                digitalClockMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [DigitalClock] The minimum green color level (true color)
        /// </summary>
        public int DigitalClockMinimumGreenColorLevel
        {
            get
            {
                return digitalClockMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                digitalClockMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [DigitalClock] The minimum blue color level (true color)
        /// </summary>
        public int DigitalClockMinimumBlueColorLevel
        {
            get
            {
                return digitalClockMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                digitalClockMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [DigitalClock] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int DigitalClockMinimumColorLevel
        {
            get
            {
                return digitalClockMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                digitalClockMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [DigitalClock] The maximum red color level (true color)
        /// </summary>
        public int DigitalClockMaximumRedColorLevel
        {
            get
            {
                return digitalClockMaximumRedColorLevel;
            }
            set
            {
                if (value <= digitalClockMinimumRedColorLevel)
                    value = digitalClockMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                digitalClockMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [DigitalClock] The maximum green color level (true color)
        /// </summary>
        public int DigitalClockMaximumGreenColorLevel
        {
            get
            {
                return digitalClockMaximumGreenColorLevel;
            }
            set
            {
                if (value <= digitalClockMinimumGreenColorLevel)
                    value = digitalClockMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                digitalClockMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [DigitalClock] The maximum blue color level (true color)
        /// </summary>
        public int DigitalClockMaximumBlueColorLevel
        {
            get
            {
                return digitalClockMaximumBlueColorLevel;
            }
            set
            {
                if (value <= digitalClockMinimumBlueColorLevel)
                    value = digitalClockMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                digitalClockMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [DigitalClock] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int DigitalClockMaximumColorLevel
        {
            get
            {
                return digitalClockMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= digitalClockMinimumColorLevel)
                    value = digitalClockMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                digitalClockMaximumColorLevel = value;
            }
        }
    }
}
