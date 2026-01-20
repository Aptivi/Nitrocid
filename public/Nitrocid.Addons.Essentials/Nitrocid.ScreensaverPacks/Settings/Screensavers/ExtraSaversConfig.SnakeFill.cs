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
        private bool snakeFillTrueColor = true;
        private int snakeFillDelay = 10;
        private int snakeFillMinimumRedColorLevel = 0;
        private int snakeFillMinimumGreenColorLevel = 0;
        private int snakeFillMinimumBlueColorLevel = 0;
        private int snakeFillMinimumColorLevel = 0;
        private int snakeFillMaximumRedColorLevel = 255;
        private int snakeFillMaximumGreenColorLevel = 255;
        private int snakeFillMaximumBlueColorLevel = 255;
        private int snakeFillMaximumColorLevel = 255;

        /// <summary>
        /// [SnakeFill] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool SnakeFillTrueColor
        {
            get
            {
                return snakeFillTrueColor;
            }
            set
            {
                snakeFillTrueColor = value;
            }
        }
        /// <summary>
        /// [SnakeFill] How many milliseconds to wait before making the next write?
        /// </summary>
        public int SnakeFillDelay
        {
            get
            {
                return snakeFillDelay;
            }
            set
            {
                snakeFillDelay = value;
            }
        }
        /// <summary>
        /// [SnakeFill] The minimum red color level (true color)
        /// </summary>
        public int SnakeFillMinimumRedColorLevel
        {
            get
            {
                return snakeFillMinimumRedColorLevel;
            }
            set
            {
                snakeFillMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [SnakeFill] The minimum green color level (true color)
        /// </summary>
        public int SnakeFillMinimumGreenColorLevel
        {
            get
            {
                return snakeFillMinimumGreenColorLevel;
            }
            set
            {
                snakeFillMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [SnakeFill] The minimum blue color level (true color)
        /// </summary>
        public int SnakeFillMinimumBlueColorLevel
        {
            get
            {
                return snakeFillMinimumBlueColorLevel;
            }
            set
            {
                snakeFillMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [SnakeFill] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int SnakeFillMinimumColorLevel
        {
            get
            {
                return snakeFillMinimumColorLevel;
            }
            set
            {
                snakeFillMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [SnakeFill] The maximum red color level (true color)
        /// </summary>
        public int SnakeFillMaximumRedColorLevel
        {
            get
            {
                return snakeFillMaximumRedColorLevel;
            }
            set
            {
                snakeFillMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [SnakeFill] The maximum green color level (true color)
        /// </summary>
        public int SnakeFillMaximumGreenColorLevel
        {
            get
            {
                return snakeFillMaximumGreenColorLevel;
            }
            set
            {
                snakeFillMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [SnakeFill] The maximum blue color level (true color)
        /// </summary>
        public int SnakeFillMaximumBlueColorLevel
        {
            get
            {
                return snakeFillMaximumBlueColorLevel;
            }
            set
            {
                snakeFillMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [SnakeFill] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int SnakeFillMaximumColorLevel
        {
            get
            {
                return snakeFillMaximumColorLevel;
            }
            set
            {
                snakeFillMaximumColorLevel = value;
            }
        }
    }
}
