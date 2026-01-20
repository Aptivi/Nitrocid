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
        private bool colorSpreadTrueColor = true;
        private int colorSpreadDelay = 20;
        private int colorSpreadMinimumRedColorLevel = 0;
        private int colorSpreadMinimumGreenColorLevel = 0;
        private int colorSpreadMinimumBlueColorLevel = 0;
        private int colorSpreadMinimumColorLevel = 0;
        private int colorSpreadMaximumRedColorLevel = 255;
        private int colorSpreadMaximumGreenColorLevel = 255;
        private int colorSpreadMaximumBlueColorLevel = 255;
        private int colorSpreadMaximumColorLevel = 255;

        /// <summary>
        /// [ColorSpread] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool ColorSpreadTrueColor
        {
            get
            {
                return colorSpreadTrueColor;
            }
            set
            {
                colorSpreadTrueColor = value;
            }
        }
        /// <summary>
        /// [ColorSpread] How many milliseconds to wait before making the next write?
        /// </summary>
        public int ColorSpreadDelay
        {
            get
            {
                return colorSpreadDelay;
            }
            set
            {
                if (value <= 0)
                    value = 20;
                colorSpreadDelay = value;
            }
        }
        /// <summary>
        /// [ColorSpread] The minimum red color level (true color)
        /// </summary>
        public int ColorSpreadMinimumRedColorLevel
        {
            get
            {
                return colorSpreadMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                colorSpreadMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorSpread] The minimum green color level (true color)
        /// </summary>
        public int ColorSpreadMinimumGreenColorLevel
        {
            get
            {
                return colorSpreadMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                colorSpreadMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorSpread] The minimum blue color level (true color)
        /// </summary>
        public int ColorSpreadMinimumBlueColorLevel
        {
            get
            {
                return colorSpreadMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                colorSpreadMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorSpread] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int ColorSpreadMinimumColorLevel
        {
            get
            {
                return colorSpreadMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                colorSpreadMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorSpread] The maximum red color level (true color)
        /// </summary>
        public int ColorSpreadMaximumRedColorLevel
        {
            get
            {
                return colorSpreadMaximumRedColorLevel;
            }
            set
            {
                if (value <= colorSpreadMinimumRedColorLevel)
                    value = colorSpreadMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                colorSpreadMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorSpread] The maximum green color level (true color)
        /// </summary>
        public int ColorSpreadMaximumGreenColorLevel
        {
            get
            {
                return colorSpreadMaximumGreenColorLevel;
            }
            set
            {
                if (value <= colorSpreadMinimumGreenColorLevel)
                    value = colorSpreadMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                colorSpreadMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorSpread] The maximum blue color level (true color)
        /// </summary>
        public int ColorSpreadMaximumBlueColorLevel
        {
            get
            {
                return colorSpreadMaximumBlueColorLevel;
            }
            set
            {
                if (value <= colorSpreadMinimumBlueColorLevel)
                    value = colorSpreadMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                colorSpreadMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorSpread] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int ColorSpreadMaximumColorLevel
        {
            get
            {
                return colorSpreadMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= colorSpreadMinimumColorLevel)
                    value = colorSpreadMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                colorSpreadMaximumColorLevel = value;
            }
        }
    }
}
