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
        private bool laserBeamsTrueColor = true;
        private int laserBeamsDelay = 500;
        private string laserBeamsLineChar = "-";
        private string laserBeamsBackgroundColor = new Color(ConsoleColors.Black).PlainSequence;
        private int laserBeamsMinimumRedColorLevel = 0;
        private int laserBeamsMinimumGreenColorLevel = 0;
        private int laserBeamsMinimumBlueColorLevel = 0;
        private int laserBeamsMinimumColorLevel = 0;
        private int laserBeamsMaximumRedColorLevel = 255;
        private int laserBeamsMaximumGreenColorLevel = 255;
        private int laserBeamsMaximumBlueColorLevel = 255;
        private int laserBeamsMaximumColorLevel = 255;

        /// <summary>
        /// [LaserBeams] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool LaserBeamsTrueColor
        {
            get
            {
                return laserBeamsTrueColor;
            }
            set
            {
                laserBeamsTrueColor = value;
            }
        }
        /// <summary>
        /// [LaserBeams] How many milliseconds to wait before making the next write?
        /// </summary>
        public int LaserBeamsDelay
        {
            get
            {
                return laserBeamsDelay;
            }
            set
            {
                if (value <= 0)
                    value = 500;
                laserBeamsDelay = value;
            }
        }
        /// <summary>
        /// [LaserBeams] Line character
        /// </summary>
        public string LaserBeamsLineChar
        {
            get
            {
                return laserBeamsLineChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "-";
                laserBeamsLineChar = value;
            }
        }
        /// <summary>
        /// [LaserBeams] Screensaver background color
        /// </summary>
        public string LaserBeamsBackgroundColor
        {
            get
            {
                return laserBeamsBackgroundColor;
            }
            set
            {
                laserBeamsBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [LaserBeams] The minimum red color level (true color)
        /// </summary>
        public int LaserBeamsMinimumRedColorLevel
        {
            get
            {
                return laserBeamsMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                laserBeamsMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [LaserBeams] The minimum green color level (true color)
        /// </summary>
        public int LaserBeamsMinimumGreenColorLevel
        {
            get
            {
                return laserBeamsMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                laserBeamsMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [LaserBeams] The minimum blue color level (true color)
        /// </summary>
        public int LaserBeamsMinimumBlueColorLevel
        {
            get
            {
                return laserBeamsMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                laserBeamsMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [LaserBeams] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int LaserBeamsMinimumColorLevel
        {
            get
            {
                return laserBeamsMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                laserBeamsMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [LaserBeams] The maximum red color level (true color)
        /// </summary>
        public int LaserBeamsMaximumRedColorLevel
        {
            get
            {
                return laserBeamsMaximumRedColorLevel;
            }
            set
            {
                if (value <= laserBeamsMinimumRedColorLevel)
                    value = laserBeamsMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                laserBeamsMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [LaserBeams] The maximum green color level (true color)
        /// </summary>
        public int LaserBeamsMaximumGreenColorLevel
        {
            get
            {
                return laserBeamsMaximumGreenColorLevel;
            }
            set
            {
                if (value <= laserBeamsMinimumGreenColorLevel)
                    value = laserBeamsMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                laserBeamsMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [LaserBeams] The maximum blue color level (true color)
        /// </summary>
        public int LaserBeamsMaximumBlueColorLevel
        {
            get
            {
                return laserBeamsMaximumBlueColorLevel;
            }
            set
            {
                if (value <= laserBeamsMinimumBlueColorLevel)
                    value = laserBeamsMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                laserBeamsMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [LaserBeams] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int LaserBeamsMaximumColorLevel
        {
            get
            {
                return laserBeamsMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= laserBeamsMinimumColorLevel)
                    value = laserBeamsMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                laserBeamsMaximumColorLevel = value;
            }
        }
    }
}
