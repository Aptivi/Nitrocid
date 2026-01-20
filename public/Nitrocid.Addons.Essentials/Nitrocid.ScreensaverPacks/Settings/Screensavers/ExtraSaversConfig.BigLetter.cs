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
using Textify.Data.Figlet;

namespace Nitrocid.ScreensaverPacks.Settings
{
    /// <summary>
    /// Screensaver kernel configuration instance
    /// </summary>
    public partial class ExtraSaversConfig : BaseKernelConfig
    {
        private bool bigLetterTrueColor = true;
        private int bigLetterDelay = 1000;
        private string bigLetterFont = "small";
        private bool bigLetterRainbowMode;
        private int bigLetterMinimumRedColorLevel = 0;
        private int bigLetterMinimumGreenColorLevel = 0;
        private int bigLetterMinimumBlueColorLevel = 0;
        private int bigLetterMinimumColorLevel = 0;
        private int bigLetterMaximumRedColorLevel = 255;
        private int bigLetterMaximumGreenColorLevel = 255;
        private int bigLetterMaximumBlueColorLevel = 255;
        private int bigLetterMaximumColorLevel = 255;

        /// <summary>
        /// [BigLetter] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool BigLetterTrueColor
        {
            get
            {
                return bigLetterTrueColor;
            }
            set
            {
                bigLetterTrueColor = value;
            }
        }
        /// <summary>
        /// [BigLetter] How many milliseconds to wait before making the next write?
        /// </summary>
        public int BigLetterDelay
        {
            get
            {
                return bigLetterDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                bigLetterDelay = value;
            }
        }
        /// <summary>
        /// [BigLetter] BigLetter font supported by the figlet library used.
        /// </summary>
        public string BigLetterFont
        {
            get
            {
                return bigLetterFont;
            }
            set
            {
                bigLetterFont = FigletTools.GetFigletFonts().ContainsKey(value) ? value : "small";
            }
        }
        /// <summary>
        /// [BigLetter] Enables the rainbow colors mode
        /// </summary>
        public bool BigLetterRainbowMode
        {
            get
            {
                return bigLetterRainbowMode;
            }
            set
            {
                bigLetterRainbowMode = value;
            }
        }
        /// <summary>
        /// [BigLetter] The minimum red color level (true color)
        /// </summary>
        public int BigLetterMinimumRedColorLevel
        {
            get
            {
                return bigLetterMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                bigLetterMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BigLetter] The minimum green color level (true color)
        /// </summary>
        public int BigLetterMinimumGreenColorLevel
        {
            get
            {
                return bigLetterMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                bigLetterMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BigLetter] The minimum blue color level (true color)
        /// </summary>
        public int BigLetterMinimumBlueColorLevel
        {
            get
            {
                return bigLetterMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                bigLetterMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BigLetter] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int BigLetterMinimumColorLevel
        {
            get
            {
                return bigLetterMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                bigLetterMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [BigLetter] The maximum red color level (true color)
        /// </summary>
        public int BigLetterMaximumRedColorLevel
        {
            get
            {
                return bigLetterMaximumRedColorLevel;
            }
            set
            {
                if (value <= bigLetterMinimumRedColorLevel)
                    value = bigLetterMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                bigLetterMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BigLetter] The maximum green color level (true color)
        /// </summary>
        public int BigLetterMaximumGreenColorLevel
        {
            get
            {
                return bigLetterMaximumGreenColorLevel;
            }
            set
            {
                if (value <= bigLetterMinimumGreenColorLevel)
                    value = bigLetterMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                bigLetterMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BigLetter] The maximum blue color level (true color)
        /// </summary>
        public int BigLetterMaximumBlueColorLevel
        {
            get
            {
                return bigLetterMaximumBlueColorLevel;
            }
            set
            {
                if (value <= bigLetterMinimumBlueColorLevel)
                    value = bigLetterMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                bigLetterMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BigLetter] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int BigLetterMaximumColorLevel
        {
            get
            {
                return bigLetterMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= bigLetterMinimumColorLevel)
                    value = bigLetterMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                bigLetterMaximumColorLevel = value;
            }
        }
    }
}
