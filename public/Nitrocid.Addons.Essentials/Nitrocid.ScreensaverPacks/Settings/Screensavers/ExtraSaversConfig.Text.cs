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
        private bool textTrueColor = true;
        private int textDelay = 1000;
        private string textWrite = "Nitrocid KS";
        private bool textRainbowMode;
        private int textMinimumRedColorLevel = 0;
        private int textMinimumGreenColorLevel = 0;
        private int textMinimumBlueColorLevel = 0;
        private int textMinimumColorLevel = 0;
        private int textMaximumRedColorLevel = 255;
        private int textMaximumGreenColorLevel = 255;
        private int textMaximumBlueColorLevel = 255;
        private int textMaximumColorLevel = 255;

        /// <summary>
        /// [Text] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool TextTrueColor
        {
            get
            {
                return textTrueColor;
            }
            set
            {
                textTrueColor = value;
            }
        }
        /// <summary>
        /// [Text] How many milliseconds to wait before making the next write?
        /// </summary>
        public int TextDelay
        {
            get
            {
                return textDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                textDelay = value;
            }
        }
        /// <summary>
        /// [Text] Text for Bouncing Text. Shorter is better.
        /// </summary>
        public string TextWrite
        {
            get
            {
                return textWrite;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Nitrocid KS";
                textWrite = value;
            }
        }
        /// <summary>
        /// [Text] Enables the rainbow colors mode
        /// </summary>
        public bool TextRainbowMode
        {
            get
            {
                return textRainbowMode;
            }
            set
            {
                textRainbowMode = value;
            }
        }
        /// <summary>
        /// [Text] The minimum red color level (true color)
        /// </summary>
        public int TextMinimumRedColorLevel
        {
            get
            {
                return textMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                textMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Text] The minimum green color level (true color)
        /// </summary>
        public int TextMinimumGreenColorLevel
        {
            get
            {
                return textMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                textMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Text] The minimum blue color level (true color)
        /// </summary>
        public int TextMinimumBlueColorLevel
        {
            get
            {
                return textMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                textMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Text] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int TextMinimumColorLevel
        {
            get
            {
                return textMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                textMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Text] The maximum red color level (true color)
        /// </summary>
        public int TextMaximumRedColorLevel
        {
            get
            {
                return textMaximumRedColorLevel;
            }
            set
            {
                if (value <= textMinimumRedColorLevel)
                    value = textMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                textMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Text] The maximum green color level (true color)
        /// </summary>
        public int TextMaximumGreenColorLevel
        {
            get
            {
                return textMaximumGreenColorLevel;
            }
            set
            {
                if (value <= textMinimumGreenColorLevel)
                    value = textMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                textMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Text] The maximum blue color level (true color)
        /// </summary>
        public int TextMaximumBlueColorLevel
        {
            get
            {
                return textMaximumBlueColorLevel;
            }
            set
            {
                if (value <= textMinimumBlueColorLevel)
                    value = textMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                textMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Text] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int TextMaximumColorLevel
        {
            get
            {
                return textMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= textMinimumColorLevel)
                    value = textMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                textMaximumColorLevel = value;
            }
        }
    }
}
