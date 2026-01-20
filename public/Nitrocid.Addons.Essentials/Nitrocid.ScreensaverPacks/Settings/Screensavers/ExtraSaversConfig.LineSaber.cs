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
        private bool lineSaberTrueColor = true;
        private int lineSaberDelay = 500;
        private string lineSaberLineChar = "-";
        private string lineSaberVerticalLineChar = "|";
        private int lineSaberLineDensity = 35;
        private int lineSaberVerticalLineDensity = 20;
        private int lineSaberReverseDensity = 30;
        private string lineSaberBackgroundColor = new Color(ConsoleColors.Black).PlainSequence;
        private int lineSaberMinimumRedColorLevel = 0;
        private int lineSaberMinimumGreenColorLevel = 0;
        private int lineSaberMinimumBlueColorLevel = 0;
        private int lineSaberMinimumColorLevel = 0;
        private int lineSaberMaximumRedColorLevel = 255;
        private int lineSaberMaximumGreenColorLevel = 255;
        private int lineSaberMaximumBlueColorLevel = 255;
        private int lineSaberMaximumColorLevel = 255;

        /// <summary>
        /// [LineSaber] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool LineSaberTrueColor
        {
            get
            {
                return lineSaberTrueColor;
            }
            set
            {
                lineSaberTrueColor = value;
            }
        }

        /// <summary>
        /// [LineSaber] How many milliseconds to wait before making the next write?
        /// </summary>
        public int LineSaberDelay
        {
            get
            {
                return lineSaberDelay;
            }
            set
            {
                if (value <= 0)
                    value = 500;
                lineSaberDelay = value;
            }
        }

        /// <summary>
        /// [LineSaber] Line character
        /// </summary>
        public string LineSaberLineChar
        {
            get
            {
                return lineSaberLineChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "-";
                lineSaberLineChar = value;
            }
        }

        /// <summary>
        /// [LineSaber] Vertical line character
        /// </summary>
        public string LineSaberVerticalLineChar
        {
            get
            {
                return lineSaberVerticalLineChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "|";
                lineSaberVerticalLineChar = value;
            }
        }

        /// <summary>
        /// [LineSaber] The density of lines
        /// </summary>
        public int LineSaberLineDensity
        {
            get
            {
                return lineSaberLineDensity;
            }
            set
            {
                if (value < 0)
                    value = 35;
                if (value > 100)
                    value = 35;
                lineSaberLineDensity = value;
            }
        }

        /// <summary>
        /// [LineSaber] The density of vertical lines (if one is needed)
        /// </summary>
        public int LineSaberVerticalLineDensity
        {
            get
            {
                return lineSaberVerticalLineDensity;
            }
            set
            {
                if (value < 0)
                    value = 20;
                if (value > 100)
                    value = 20;
                lineSaberVerticalLineDensity = value;
            }
        }

        /// <summary>
        /// [LineSaber] The density of reverse direction
        /// </summary>
        public int LineSaberReverseDensity
        {
            get
            {
                return lineSaberReverseDensity;
            }
            set
            {
                if (value < 0)
                    value = 20;
                if (value > 100)
                    value = 20;
                lineSaberReverseDensity = value;
            }
        }

        /// <summary>
        /// [LineSaber] Screensaver background color
        /// </summary>
        public string LineSaberBackgroundColor
        {
            get
            {
                return lineSaberBackgroundColor;
            }
            set
            {
                lineSaberBackgroundColor = new Color(value).PlainSequence;
            }
        }

        /// <summary>
        /// [LineSaber] The minimum red color level (true color)
        /// </summary>
        public int LineSaberMinimumRedColorLevel
        {
            get
            {
                return lineSaberMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                lineSaberMinimumRedColorLevel = value;
            }
        }

        /// <summary>
        /// [LineSaber] The minimum green color level (true color)
        /// </summary>
        public int LineSaberMinimumGreenColorLevel
        {
            get
            {
                return lineSaberMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                lineSaberMinimumGreenColorLevel = value;
            }
        }

        /// <summary>
        /// [LineSaber] The minimum blue color level (true color)
        /// </summary>
        public int LineSaberMinimumBlueColorLevel
        {
            get
            {
                return lineSaberMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                lineSaberMinimumBlueColorLevel = value;
            }
        }

        /// <summary>
        /// [LineSaber] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int LineSaberMinimumColorLevel
        {
            get
            {
                return lineSaberMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                lineSaberMinimumColorLevel = value;
            }
        }

        /// <summary>
        /// [LineSaber] The maximum red color level (true color)
        /// </summary>
        public int LineSaberMaximumRedColorLevel
        {
            get
            {
                return lineSaberMaximumRedColorLevel;
            }
            set
            {
                if (value <= lineSaberMinimumRedColorLevel)
                    value = lineSaberMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                lineSaberMaximumRedColorLevel = value;
            }
        }

        /// <summary>
        /// [LineSaber] The maximum green color level (true color)
        /// </summary>
        public int LineSaberMaximumGreenColorLevel
        {
            get
            {
                return lineSaberMaximumGreenColorLevel;
            }
            set
            {
                if (value <= lineSaberMinimumGreenColorLevel)
                    value = lineSaberMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                lineSaberMaximumGreenColorLevel = value;
            }
        }

        /// <summary>
        /// [LineSaber] The maximum blue color level (true color)
        /// </summary>
        public int LineSaberMaximumBlueColorLevel
        {
            get
            {
                return lineSaberMaximumBlueColorLevel;
            }
            set
            {
                if (value <= lineSaberMinimumBlueColorLevel)
                    value = lineSaberMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                lineSaberMaximumBlueColorLevel = value;
            }
        }

        /// <summary>
        /// [LineSaber] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int LineSaberMaximumColorLevel
        {
            get
            {
                return lineSaberMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= lineSaberMinimumColorLevel)
                    value = lineSaberMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                lineSaberMaximumColorLevel = value;
            }
        }
    }
}
