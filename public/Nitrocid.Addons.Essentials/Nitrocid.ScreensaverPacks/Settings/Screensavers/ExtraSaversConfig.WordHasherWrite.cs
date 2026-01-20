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
        private bool wordHasherWriteTrueColor = true;
        private int wordHasherWriteDelay = 1000;
        private int wordHasherWriteMinimumRedColorLevel = 0;
        private int wordHasherWriteMinimumGreenColorLevel = 0;
        private int wordHasherWriteMinimumBlueColorLevel = 0;
        private int wordHasherWriteMinimumColorLevel = 0;
        private int wordHasherWriteMaximumRedColorLevel = 255;
        private int wordHasherWriteMaximumGreenColorLevel = 255;
        private int wordHasherWriteMaximumBlueColorLevel = 255;
        private int wordHasherWriteMaximumColorLevel = 255;

        /// <summary>
        /// [WordHasherWrite] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool WordHasherWriteTrueColor
        {
            get
            {
                return wordHasherWriteTrueColor;
            }
            set
            {
                wordHasherWriteTrueColor = value;
            }
        }
        /// <summary>
        /// [WordHasherWrite] How many milliseconds to wait before making the next write?
        /// </summary>
        public int WordHasherWriteDelay
        {
            get
            {
                return wordHasherWriteDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                wordHasherWriteDelay = value;
            }
        }
        /// <summary>
        /// [WordHasherWrite] The minimum red color level (true color)
        /// </summary>
        public int WordHasherWriteMinimumRedColorLevel
        {
            get
            {
                return wordHasherWriteMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                wordHasherWriteMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasherWrite] The minimum green color level (true color)
        /// </summary>
        public int WordHasherWriteMinimumGreenColorLevel
        {
            get
            {
                return wordHasherWriteMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                wordHasherWriteMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasherWrite] The minimum blue color level (true color)
        /// </summary>
        public int WordHasherWriteMinimumBlueColorLevel
        {
            get
            {
                return wordHasherWriteMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                wordHasherWriteMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasherWrite] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int WordHasherWriteMinimumColorLevel
        {
            get
            {
                return wordHasherWriteMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                wordHasherWriteMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasherWrite] The maximum red color level (true color)
        /// </summary>
        public int WordHasherWriteMaximumRedColorLevel
        {
            get
            {
                return wordHasherWriteMaximumRedColorLevel;
            }
            set
            {
                if (value <= wordHasherWriteMinimumRedColorLevel)
                    value = wordHasherWriteMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                wordHasherWriteMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasherWrite] The maximum green color level (true color)
        /// </summary>
        public int WordHasherWriteMaximumGreenColorLevel
        {
            get
            {
                return wordHasherWriteMaximumGreenColorLevel;
            }
            set
            {
                if (value <= wordHasherWriteMinimumGreenColorLevel)
                    value = wordHasherWriteMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                wordHasherWriteMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasherWrite] The maximum blue color level (true color)
        /// </summary>
        public int WordHasherWriteMaximumBlueColorLevel
        {
            get
            {
                return wordHasherWriteMaximumBlueColorLevel;
            }
            set
            {
                if (value <= wordHasherWriteMinimumBlueColorLevel)
                    value = wordHasherWriteMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                wordHasherWriteMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasherWrite] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int WordHasherWriteMaximumColorLevel
        {
            get
            {
                return wordHasherWriteMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= wordHasherWriteMinimumColorLevel)
                    value = wordHasherWriteMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                wordHasherWriteMaximumColorLevel = value;
            }
        }
    }
}
