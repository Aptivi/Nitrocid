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
        private bool flashColorTrueColor = true;
        private int flashColorDelay = 20;
        private string flashColorBackgroundColor = new Color(ConsoleColors.Black).PlainSequence;
        private int flashColorMinimumRedColorLevel = 0;
        private int flashColorMinimumGreenColorLevel = 0;
        private int flashColorMinimumBlueColorLevel = 0;
        private int flashColorMinimumColorLevel = 0;
        private int flashColorMaximumRedColorLevel = 255;
        private int flashColorMaximumGreenColorLevel = 255;
        private int flashColorMaximumBlueColorLevel = 255;
        private int flashColorMaximumColorLevel = 255;

        /// <summary>
        /// [FlashColor] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool FlashColorTrueColor
        {
            get
            {
                return flashColorTrueColor;
            }
            set
            {
                flashColorTrueColor = value;
            }
        }
        /// <summary>
        /// [FlashColor] How many milliseconds to wait before making the next write?
        /// </summary>
        public int FlashColorDelay
        {
            get
            {
                return flashColorDelay;
            }
            set
            {
                if (value <= 0)
                    value = 20;
                flashColorDelay = value;
            }
        }
        /// <summary>
        /// [FlashColor] Screensaver background color
        /// </summary>
        public string FlashColorBackgroundColor
        {
            get
            {
                return flashColorBackgroundColor;
            }
            set
            {
                flashColorBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [FlashColor] The minimum red color level (true color)
        /// </summary>
        public int FlashColorMinimumRedColorLevel
        {
            get
            {
                return flashColorMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                flashColorMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashColor] The minimum green color level (true color)
        /// </summary>
        public int FlashColorMinimumGreenColorLevel
        {
            get
            {
                return flashColorMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                flashColorMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashColor] The minimum blue color level (true color)
        /// </summary>
        public int FlashColorMinimumBlueColorLevel
        {
            get
            {
                return flashColorMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                flashColorMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashColor] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int FlashColorMinimumColorLevel
        {
            get
            {
                return flashColorMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                flashColorMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashColor] The maximum red color level (true color)
        /// </summary>
        public int FlashColorMaximumRedColorLevel
        {
            get
            {
                return flashColorMaximumRedColorLevel;
            }
            set
            {
                if (value <= flashColorMinimumRedColorLevel)
                    value = flashColorMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                flashColorMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashColor] The maximum green color level (true color)
        /// </summary>
        public int FlashColorMaximumGreenColorLevel
        {
            get
            {
                return flashColorMaximumGreenColorLevel;
            }
            set
            {
                if (value <= flashColorMinimumGreenColorLevel)
                    value = flashColorMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                flashColorMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashColor] The maximum blue color level (true color)
        /// </summary>
        public int FlashColorMaximumBlueColorLevel
        {
            get
            {
                return flashColorMaximumBlueColorLevel;
            }
            set
            {
                if (value <= flashColorMinimumBlueColorLevel)
                    value = flashColorMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                flashColorMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashColor] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int FlashColorMaximumColorLevel
        {
            get
            {
                return flashColorMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= flashColorMinimumColorLevel)
                    value = flashColorMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                flashColorMaximumColorLevel = value;
            }
        }
    }
}
