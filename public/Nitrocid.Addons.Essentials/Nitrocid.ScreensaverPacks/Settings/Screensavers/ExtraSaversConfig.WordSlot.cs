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
        private bool wordSlotTrueColor = true;
        private int wordSlotDelay = 1000;
        private int wordSlotMinimumRedColorLevel = 0;
        private int wordSlotMinimumGreenColorLevel = 0;
        private int wordSlotMinimumBlueColorLevel = 0;
        private int wordSlotMinimumColorLevel = 0;
        private int wordSlotMaximumRedColorLevel = 255;
        private int wordSlotMaximumGreenColorLevel = 255;
        private int wordSlotMaximumBlueColorLevel = 255;
        private int wordSlotMaximumColorLevel = 255;

        /// <summary>
        /// [WordSlot] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool WordSlotTrueColor
        {
            get
            {
                return wordSlotTrueColor;
            }
            set
            {
                wordSlotTrueColor = value;
            }
        }
        /// <summary>
        /// [WordSlot] How many milliseconds to wait before making the next write?
        /// </summary>
        public int WordSlotDelay
        {
            get
            {
                return wordSlotDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                wordSlotDelay = value;
            }
        }
        /// <summary>
        /// [WordSlot] The minimum red color level (true color)
        /// </summary>
        public int WordSlotMinimumRedColorLevel
        {
            get
            {
                return wordSlotMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                wordSlotMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [WordSlot] The minimum green color level (true color)
        /// </summary>
        public int WordSlotMinimumGreenColorLevel
        {
            get
            {
                return wordSlotMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                wordSlotMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [WordSlot] The minimum blue color level (true color)
        /// </summary>
        public int WordSlotMinimumBlueColorLevel
        {
            get
            {
                return wordSlotMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                wordSlotMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [WordSlot] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int WordSlotMinimumColorLevel
        {
            get
            {
                return wordSlotMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                wordSlotMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [WordSlot] The maximum red color level (true color)
        /// </summary>
        public int WordSlotMaximumRedColorLevel
        {
            get
            {
                return wordSlotMaximumRedColorLevel;
            }
            set
            {
                if (value <= wordSlotMinimumRedColorLevel)
                    value = wordSlotMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                wordSlotMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [WordSlot] The maximum green color level (true color)
        /// </summary>
        public int WordSlotMaximumGreenColorLevel
        {
            get
            {
                return wordSlotMaximumGreenColorLevel;
            }
            set
            {
                if (value <= wordSlotMinimumGreenColorLevel)
                    value = wordSlotMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                wordSlotMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [WordSlot] The maximum blue color level (true color)
        /// </summary>
        public int WordSlotMaximumBlueColorLevel
        {
            get
            {
                return wordSlotMaximumBlueColorLevel;
            }
            set
            {
                if (value <= wordSlotMinimumBlueColorLevel)
                    value = wordSlotMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                wordSlotMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [WordSlot] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int WordSlotMaximumColorLevel
        {
            get
            {
                return wordSlotMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= wordSlotMinimumColorLevel)
                    value = wordSlotMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                wordSlotMaximumColorLevel = value;
            }
        }
    }
}
