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
        private bool colorBleedTrueColor = true;
        private int colorBleedDelay = 10;
        private int colorBleedMaxSteps = 25;
        private int colorBleedDropChance = 40;
        private int colorBleedMinimumRedColorLevel = 0;
        private int colorBleedMinimumGreenColorLevel = 0;
        private int colorBleedMinimumBlueColorLevel = 0;
        private int colorBleedMinimumColorLevel = 0;
        private int colorBleedMaximumRedColorLevel = 255;
        private int colorBleedMaximumGreenColorLevel = 255;
        private int colorBleedMaximumBlueColorLevel = 255;
        private int colorBleedMaximumColorLevel = 255;

        /// <summary>
        /// [ColorBleed] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool ColorBleedTrueColor
        {
            get
            {
                return colorBleedTrueColor;
            }
            set
            {
                colorBleedTrueColor = value;
            }
        }
        /// <summary>
        /// [ColorBleed] How many milliseconds to wait before making the next write?
        /// </summary>
        public int ColorBleedDelay
        {
            get
            {
                return colorBleedDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                colorBleedDelay = value;
            }
        }
        /// <summary>
        /// [ColorBleed] How many fade steps to do?
        /// </summary>
        public int ColorBleedMaxSteps
        {
            get
            {
                return colorBleedMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                colorBleedMaxSteps = value;
            }
        }
        /// <summary>
        /// [ColorBleed] Chance to drop a new falling color
        /// </summary>
        public int ColorBleedDropChance
        {
            get
            {
                return colorBleedDropChance;
            }
            set
            {
                if (value <= 0)
                    value = 40;
                colorBleedDropChance = value;
            }
        }
        /// <summary>
        /// [ColorBleed] The minimum red color level (true color)
        /// </summary>
        public int ColorBleedMinimumRedColorLevel
        {
            get
            {
                return colorBleedMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                colorBleedMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorBleed] The minimum green color level (true color)
        /// </summary>
        public int ColorBleedMinimumGreenColorLevel
        {
            get
            {
                return colorBleedMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                colorBleedMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorBleed] The minimum blue color level (true color)
        /// </summary>
        public int ColorBleedMinimumBlueColorLevel
        {
            get
            {
                return colorBleedMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                colorBleedMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorBleed] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int ColorBleedMinimumColorLevel
        {
            get
            {
                return colorBleedMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                colorBleedMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorBleed] The maximum red color level (true color)
        /// </summary>
        public int ColorBleedMaximumRedColorLevel
        {
            get
            {
                return colorBleedMaximumRedColorLevel;
            }
            set
            {
                if (value <= colorBleedMinimumRedColorLevel)
                    value = colorBleedMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                colorBleedMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorBleed] The maximum green color level (true color)
        /// </summary>
        public int ColorBleedMaximumGreenColorLevel
        {
            get
            {
                return colorBleedMaximumGreenColorLevel;
            }
            set
            {
                if (value <= colorBleedMinimumGreenColorLevel)
                    value = colorBleedMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                colorBleedMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorBleed] The maximum blue color level (true color)
        /// </summary>
        public int ColorBleedMaximumBlueColorLevel
        {
            get
            {
                return colorBleedMaximumBlueColorLevel;
            }
            set
            {
                if (value <= colorBleedMinimumBlueColorLevel)
                    value = colorBleedMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                colorBleedMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorBleed] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int ColorBleedMaximumColorLevel
        {
            get
            {
                return colorBleedMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= colorBleedMinimumColorLevel)
                    value = colorBleedMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                colorBleedMaximumColorLevel = value;
            }
        }
    }
}
