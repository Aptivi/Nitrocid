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
        private bool textWanderTrueColor = true;
        private int textWanderDelay = 1000;
        private string textWanderWrite = "Nitrocid KS";
        private int textWanderMinimumRedColorLevel = 0;
        private int textWanderMinimumGreenColorLevel = 0;
        private int textWanderMinimumBlueColorLevel = 0;
        private int textWanderMinimumColorLevel = 0;
        private int textWanderMaximumRedColorLevel = 255;
        private int textWanderMaximumGreenColorLevel = 255;
        private int textWanderMaximumBlueColorLevel = 255;
        private int textWanderMaximumColorLevel = 255;

        /// <summary>
        /// [TextWander] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool TextWanderTrueColor
        {
            get
            {
                return textWanderTrueColor;
            }
            set
            {
                textWanderTrueColor = value;
            }
        }
        /// <summary>
        /// [TextWander] How many milliseconds to wait before making the next write?
        /// </summary>
        public int TextWanderDelay
        {
            get
            {
                return textWanderDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                textWanderDelay = value;
            }
        }
        /// <summary>
        /// [TextWander] TextWander for Bouncing TextWander. Shorter is better.
        /// </summary>
        public string TextWanderWrite
        {
            get
            {
                return textWanderWrite;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Nitrocid KS";
                textWanderWrite = value;
            }
        }
        /// <summary>
        /// [TextWander] The minimum red color level (true color)
        /// </summary>
        public int TextWanderMinimumRedColorLevel
        {
            get
            {
                return textWanderMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                textWanderMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [TextWander] The minimum green color level (true color)
        /// </summary>
        public int TextWanderMinimumGreenColorLevel
        {
            get
            {
                return textWanderMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                textWanderMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [TextWander] The minimum blue color level (true color)
        /// </summary>
        public int TextWanderMinimumBlueColorLevel
        {
            get
            {
                return textWanderMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                textWanderMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [TextWander] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int TextWanderMinimumColorLevel
        {
            get
            {
                return textWanderMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                textWanderMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [TextWander] The maximum red color level (true color)
        /// </summary>
        public int TextWanderMaximumRedColorLevel
        {
            get
            {
                return textWanderMaximumRedColorLevel;
            }
            set
            {
                if (value <= textWanderMinimumRedColorLevel)
                    value = textWanderMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                textWanderMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [TextWander] The maximum green color level (true color)
        /// </summary>
        public int TextWanderMaximumGreenColorLevel
        {
            get
            {
                return textWanderMaximumGreenColorLevel;
            }
            set
            {
                if (value <= textWanderMinimumGreenColorLevel)
                    value = textWanderMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                textWanderMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [TextWander] The maximum blue color level (true color)
        /// </summary>
        public int TextWanderMaximumBlueColorLevel
        {
            get
            {
                return textWanderMaximumBlueColorLevel;
            }
            set
            {
                if (value <= textWanderMinimumBlueColorLevel)
                    value = textWanderMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                textWanderMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [TextWander] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int TextWanderMaximumColorLevel
        {
            get
            {
                return textWanderMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= textWanderMinimumColorLevel)
                    value = textWanderMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                textWanderMaximumColorLevel = value;
            }
        }
    }
}
