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
        private bool textBoxTrueColor = true;
        private int textBoxDelay = 1000;
        private string textBoxWrite = "Nitrocid KS";
        private bool textBoxRainbowMode;
        private int textBoxMinimumRedColorLevel = 0;
        private int textBoxMinimumGreenColorLevel = 0;
        private int textBoxMinimumBlueColorLevel = 0;
        private int textBoxMinimumColorLevel = 0;
        private int textBoxMaximumRedColorLevel = 255;
        private int textBoxMaximumGreenColorLevel = 255;
        private int textBoxMaximumBlueColorLevel = 255;
        private int textBoxMaximumColorLevel = 255;

        /// <summary>
        /// [TextBox] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool TextBoxTrueColor
        {
            get
            {
                return textBoxTrueColor;
            }
            set
            {
                textBoxTrueColor = value;
            }
        }
        /// <summary>
        /// [TextBox] How many milliseconds to wait before making the next write?
        /// </summary>
        public int TextBoxDelay
        {
            get
            {
                return textBoxDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                textBoxDelay = value;
            }
        }
        /// <summary>
        /// [TextBox] TextBox for Bouncing TextBox. Shorter is better.
        /// </summary>
        public string TextBoxWrite
        {
            get
            {
                return textBoxWrite;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Nitrocid KS";
                textBoxWrite = value;
            }
        }
        /// <summary>
        /// [TextBox] Enables the rainbow colors mode
        /// </summary>
        public bool TextBoxRainbowMode
        {
            get
            {
                return textBoxRainbowMode;
            }
            set
            {
                textBoxRainbowMode = value;
            }
        }
        /// <summary>
        /// [TextBox] The minimum red color level (true color)
        /// </summary>
        public int TextBoxMinimumRedColorLevel
        {
            get
            {
                return textBoxMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                textBoxMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [TextBox] The minimum green color level (true color)
        /// </summary>
        public int TextBoxMinimumGreenColorLevel
        {
            get
            {
                return textBoxMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                textBoxMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [TextBox] The minimum blue color level (true color)
        /// </summary>
        public int TextBoxMinimumBlueColorLevel
        {
            get
            {
                return textBoxMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                textBoxMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [TextBox] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int TextBoxMinimumColorLevel
        {
            get
            {
                return textBoxMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                textBoxMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [TextBox] The maximum red color level (true color)
        /// </summary>
        public int TextBoxMaximumRedColorLevel
        {
            get
            {
                return textBoxMaximumRedColorLevel;
            }
            set
            {
                if (value <= textBoxMinimumRedColorLevel)
                    value = textBoxMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                textBoxMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [TextBox] The maximum green color level (true color)
        /// </summary>
        public int TextBoxMaximumGreenColorLevel
        {
            get
            {
                return textBoxMaximumGreenColorLevel;
            }
            set
            {
                if (value <= textBoxMinimumGreenColorLevel)
                    value = textBoxMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                textBoxMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [TextBox] The maximum blue color level (true color)
        /// </summary>
        public int TextBoxMaximumBlueColorLevel
        {
            get
            {
                return textBoxMaximumBlueColorLevel;
            }
            set
            {
                if (value <= textBoxMinimumBlueColorLevel)
                    value = textBoxMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                textBoxMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [TextBox] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int TextBoxMaximumColorLevel
        {
            get
            {
                return textBoxMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= textBoxMinimumColorLevel)
                    value = textBoxMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                textBoxMaximumColorLevel = value;
            }
        }
    }
}
