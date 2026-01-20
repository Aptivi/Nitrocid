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
using Terminaux.Colors;
using Terminaux.Colors.Data;

namespace Nitrocid.ScreensaverPacks.Settings
{
    /// <summary>
    /// Screensaver kernel configuration instance
    /// </summary>
    public partial class ExtraSaversConfig : BaseKernelConfig
    {
        private bool flashTextTrueColor = true;
        private int flashTextDelay = 100;
        private string flashTextWrite = "Nitrocid KS";
        private string flashTextBackgroundColor = new Color(ConsoleColors.Black).PlainSequence;
        private int flashTextMinimumRedColorLevel = 0;
        private int flashTextMinimumGreenColorLevel = 0;
        private int flashTextMinimumBlueColorLevel = 0;
        private int flashTextMinimumColorLevel = 0;
        private int flashTextMaximumRedColorLevel = 255;
        private int flashTextMaximumGreenColorLevel = 255;
        private int flashTextMaximumBlueColorLevel = 255;
        private int flashTextMaximumColorLevel = 255;

        /// <summary>
        /// [FlashText] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool FlashTextTrueColor
        {
            get
            {
                return flashTextTrueColor;
            }
            set
            {
                flashTextTrueColor = value;
            }
        }
        /// <summary>
        /// [FlashText] How many milliseconds to wait before making the next write?
        /// </summary>
        public int FlashTextDelay
        {
            get
            {
                return flashTextDelay;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                flashTextDelay = value;
            }
        }
        /// <summary>
        /// [FlashText] Text for FlashText. Shorter is better.
        /// </summary>
        public string FlashTextWrite
        {
            get
            {
                return flashTextWrite;
            }
            set
            {
                flashTextWrite = value;
            }
        }
        /// <summary>
        /// [FlashText] Screensaver background color
        /// </summary>
        public string FlashTextBackgroundColor
        {
            get
            {
                return flashTextBackgroundColor;
            }
            set
            {
                flashTextBackgroundColor = value;
            }
        }
        /// <summary>
        /// [FlashText] The minimum red color level (true color)
        /// </summary>
        public int FlashTextMinimumRedColorLevel
        {
            get
            {
                return flashTextMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                flashTextMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashText] The minimum green color level (true color)
        /// </summary>
        public int FlashTextMinimumGreenColorLevel
        {
            get
            {
                return flashTextMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                flashTextMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashText] The minimum blue color level (true color)
        /// </summary>
        public int FlashTextMinimumBlueColorLevel
        {
            get
            {
                return flashTextMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                flashTextMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashText] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int FlashTextMinimumColorLevel
        {
            get
            {
                return flashTextMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                flashTextMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashText] The maximum red color level (true color)
        /// </summary>
        public int FlashTextMaximumRedColorLevel
        {
            get
            {
                return flashTextMaximumRedColorLevel;
            }
            set
            {
                if (value <= flashTextMinimumRedColorLevel)
                    value = flashTextMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                flashTextMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashText] The maximum green color level (true color)
        /// </summary>
        public int FlashTextMaximumGreenColorLevel
        {
            get
            {
                return flashTextMaximumGreenColorLevel;
            }
            set
            {
                if (value <= flashTextMinimumGreenColorLevel)
                    value = flashTextMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                flashTextMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashText] The maximum blue color level (true color)
        /// </summary>
        public int FlashTextMaximumBlueColorLevel
        {
            get
            {
                return flashTextMaximumBlueColorLevel;
            }
            set
            {
                if (value <= flashTextMinimumBlueColorLevel)
                    value = flashTextMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                flashTextMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashText] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int FlashTextMaximumColorLevel
        {
            get
            {
                return flashTextMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= flashTextMinimumColorLevel)
                    value = flashTextMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                flashTextMaximumColorLevel = value;
            }
        }
    }
}
