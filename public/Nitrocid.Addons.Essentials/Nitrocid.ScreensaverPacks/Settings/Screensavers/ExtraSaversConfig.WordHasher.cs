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
        private bool wordHasherTrueColor = true;
        private int wordHasherDelay = 1000;
        private int wordHasherMinimumRedColorLevel = 0;
        private int wordHasherMinimumGreenColorLevel = 0;
        private int wordHasherMinimumBlueColorLevel = 0;
        private int wordHasherMinimumColorLevel = 0;
        private int wordHasherMaximumRedColorLevel = 255;
        private int wordHasherMaximumGreenColorLevel = 255;
        private int wordHasherMaximumBlueColorLevel = 255;
        private int wordHasherMaximumColorLevel = 255;

        /// <summary>
        /// [WordHasher] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool WordHasherTrueColor
        {
            get
            {
                return wordHasherTrueColor;
            }
            set
            {
                wordHasherTrueColor = value;
            }
        }
        /// <summary>
        /// [WordHasher] How many milliseconds to wait before making the next write?
        /// </summary>
        public int WordHasherDelay
        {
            get
            {
                return wordHasherDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                wordHasherDelay = value;
            }
        }
        /// <summary>
        /// [WordHasher] The minimum red color level (true color)
        /// </summary>
        public int WordHasherMinimumRedColorLevel
        {
            get
            {
                return wordHasherMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                wordHasherMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasher] The minimum green color level (true color)
        /// </summary>
        public int WordHasherMinimumGreenColorLevel
        {
            get
            {
                return wordHasherMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                wordHasherMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasher] The minimum blue color level (true color)
        /// </summary>
        public int WordHasherMinimumBlueColorLevel
        {
            get
            {
                return wordHasherMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                wordHasherMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasher] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int WordHasherMinimumColorLevel
        {
            get
            {
                return wordHasherMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                wordHasherMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasher] The maximum red color level (true color)
        /// </summary>
        public int WordHasherMaximumRedColorLevel
        {
            get
            {
                return wordHasherMaximumRedColorLevel;
            }
            set
            {
                if (value <= wordHasherMinimumRedColorLevel)
                    value = wordHasherMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                wordHasherMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasher] The maximum green color level (true color)
        /// </summary>
        public int WordHasherMaximumGreenColorLevel
        {
            get
            {
                return wordHasherMaximumGreenColorLevel;
            }
            set
            {
                if (value <= wordHasherMinimumGreenColorLevel)
                    value = wordHasherMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                wordHasherMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasher] The maximum blue color level (true color)
        /// </summary>
        public int WordHasherMaximumBlueColorLevel
        {
            get
            {
                return wordHasherMaximumBlueColorLevel;
            }
            set
            {
                if (value <= wordHasherMinimumBlueColorLevel)
                    value = wordHasherMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                wordHasherMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasher] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int WordHasherMaximumColorLevel
        {
            get
            {
                return wordHasherMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= wordHasherMinimumColorLevel)
                    value = wordHasherMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                wordHasherMaximumColorLevel = value;
            }
        }
    }
}
