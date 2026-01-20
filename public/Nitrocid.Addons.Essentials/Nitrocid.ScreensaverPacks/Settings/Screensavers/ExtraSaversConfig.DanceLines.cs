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
        private bool danceLinesTrueColor = true;
        private int danceLinesDelay = 50;
        private string danceLinesLineChar = "-";
        private string danceLinesBackgroundColor = new Color(ConsoleColors.Black).PlainSequence;
        private int danceLinesMinimumRedColorLevel = 0;
        private int danceLinesMinimumGreenColorLevel = 0;
        private int danceLinesMinimumBlueColorLevel = 0;
        private int danceLinesMinimumColorLevel = 0;
        private int danceLinesMaximumRedColorLevel = 255;
        private int danceLinesMaximumGreenColorLevel = 255;
        private int danceLinesMaximumBlueColorLevel = 255;
        private int danceLinesMaximumColorLevel = 255;

        /// <summary>
        /// [DanceLines] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool DanceLinesTrueColor
        {
            get
            {
                return danceLinesTrueColor;
            }
            set
            {
                danceLinesTrueColor = value;
            }
        }
        /// <summary>
        /// [DanceLines] How many milliseconds to wait before making the next write?
        /// </summary>
        public int DanceLinesDelay
        {
            get
            {
                return danceLinesDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                danceLinesDelay = value;
            }
        }
        /// <summary>
        /// [DanceLines] Line character
        /// </summary>
        public string DanceLinesLineChar
        {
            get
            {
                return danceLinesLineChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "-";
                danceLinesLineChar = value;
            }
        }
        /// <summary>
        /// [DanceLines] Screensaver background color
        /// </summary>
        public string DanceLinesBackgroundColor
        {
            get
            {
                return danceLinesBackgroundColor;
            }
            set
            {
                danceLinesBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [DanceLines] The minimum red color level (true color)
        /// </summary>
        public int DanceLinesMinimumRedColorLevel
        {
            get
            {
                return danceLinesMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                danceLinesMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [DanceLines] The minimum green color level (true color)
        /// </summary>
        public int DanceLinesMinimumGreenColorLevel
        {
            get
            {
                return danceLinesMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                danceLinesMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [DanceLines] The minimum blue color level (true color)
        /// </summary>
        public int DanceLinesMinimumBlueColorLevel
        {
            get
            {
                return danceLinesMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                danceLinesMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [DanceLines] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int DanceLinesMinimumColorLevel
        {
            get
            {
                return danceLinesMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                danceLinesMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [DanceLines] The maximum red color level (true color)
        /// </summary>
        public int DanceLinesMaximumRedColorLevel
        {
            get
            {
                return danceLinesMaximumRedColorLevel;
            }
            set
            {
                if (value <= danceLinesMinimumRedColorLevel)
                    value = danceLinesMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                danceLinesMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [DanceLines] The maximum green color level (true color)
        /// </summary>
        public int DanceLinesMaximumGreenColorLevel
        {
            get
            {
                return danceLinesMaximumGreenColorLevel;
            }
            set
            {
                if (value <= danceLinesMinimumGreenColorLevel)
                    value = danceLinesMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                danceLinesMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [DanceLines] The maximum blue color level (true color)
        /// </summary>
        public int DanceLinesMaximumBlueColorLevel
        {
            get
            {
                return danceLinesMaximumBlueColorLevel;
            }
            set
            {
                if (value <= danceLinesMinimumBlueColorLevel)
                    value = danceLinesMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                danceLinesMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [DanceLines] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int DanceLinesMaximumColorLevel
        {
            get
            {
                return danceLinesMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= danceLinesMinimumColorLevel)
                    value = danceLinesMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                danceLinesMaximumColorLevel = value;
            }
        }
    }
}
