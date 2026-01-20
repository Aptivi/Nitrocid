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
        private bool twoSpinsTrueColor = true;
        private int twoSpinsDelay = 25;
        private int twoSpinsMinimumRedColorLevel = 0;
        private int twoSpinsMinimumGreenColorLevel = 0;
        private int twoSpinsMinimumBlueColorLevel = 0;
        private int twoSpinsMinimumColorLevel = 0;
        private int twoSpinsMaximumRedColorLevel = 255;
        private int twoSpinsMaximumGreenColorLevel = 255;
        private int twoSpinsMaximumBlueColorLevel = 255;
        private int twoSpinsMaximumColorLevel = 255;

        /// <summary>
        /// [TwoSpins] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool TwoSpinsTrueColor
        {
            get
            {
                return twoSpinsTrueColor;
            }
            set
            {
                twoSpinsTrueColor = value;
            }
        }
        /// <summary>
        /// [TwoSpins] How many milliseconds to wait before making the next write?
        /// </summary>
        public int TwoSpinsDelay
        {
            get
            {
                return twoSpinsDelay;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                twoSpinsDelay = value;
            }
        }
        /// <summary>
        /// [TwoSpins] The minimum red color level (true color)
        /// </summary>
        public int TwoSpinsMinimumRedColorLevel
        {
            get
            {
                return twoSpinsMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                twoSpinsMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [TwoSpins] The minimum green color level (true color)
        /// </summary>
        public int TwoSpinsMinimumGreenColorLevel
        {
            get
            {
                return twoSpinsMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                twoSpinsMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [TwoSpins] The minimum blue color level (true color)
        /// </summary>
        public int TwoSpinsMinimumBlueColorLevel
        {
            get
            {
                return twoSpinsMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                twoSpinsMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [TwoSpins] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int TwoSpinsMinimumColorLevel
        {
            get
            {
                return twoSpinsMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                twoSpinsMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [TwoSpins] The maximum red color level (true color)
        /// </summary>
        public int TwoSpinsMaximumRedColorLevel
        {
            get
            {
                return twoSpinsMaximumRedColorLevel;
            }
            set
            {
                if (value <= twoSpinsMinimumRedColorLevel)
                    value = twoSpinsMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                twoSpinsMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [TwoSpins] The maximum green color level (true color)
        /// </summary>
        public int TwoSpinsMaximumGreenColorLevel
        {
            get
            {
                return twoSpinsMaximumGreenColorLevel;
            }
            set
            {
                if (value <= twoSpinsMinimumGreenColorLevel)
                    value = twoSpinsMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                twoSpinsMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [TwoSpins] The maximum blue color level (true color)
        /// </summary>
        public int TwoSpinsMaximumBlueColorLevel
        {
            get
            {
                return twoSpinsMaximumBlueColorLevel;
            }
            set
            {
                if (value <= twoSpinsMinimumBlueColorLevel)
                    value = twoSpinsMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                twoSpinsMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [TwoSpins] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int TwoSpinsMaximumColorLevel
        {
            get
            {
                return twoSpinsMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= twoSpinsMinimumColorLevel)
                    value = twoSpinsMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                twoSpinsMaximumColorLevel = value;
            }
        }
    }
}
