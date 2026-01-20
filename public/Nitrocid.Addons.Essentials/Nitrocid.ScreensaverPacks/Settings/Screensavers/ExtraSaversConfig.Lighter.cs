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
        private bool lighterTrueColor = true;
        private int lighterDelay = 100;
        private int lighterMaxPositions = 10;
        private string lighterBackgroundColor = new Color(ConsoleColors.Black).PlainSequence;
        private int lighterMinimumRedColorLevel = 0;
        private int lighterMinimumGreenColorLevel = 0;
        private int lighterMinimumBlueColorLevel = 0;
        private int lighterMinimumColorLevel = 0;
        private int lighterMaximumRedColorLevel = 255;
        private int lighterMaximumGreenColorLevel = 255;
        private int lighterMaximumBlueColorLevel = 255;
        private int lighterMaximumColorLevel = 255;

        /// <summary>
        /// [Lighter] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool LighterTrueColor
        {
            get
            {
                return lighterTrueColor;
            }
            set
            {
                lighterTrueColor = value;
            }
        }
        /// <summary>
        /// [Lighter] How many milliseconds to wait before making the next write?
        /// </summary>
        public int LighterDelay
        {
            get
            {
                return lighterDelay;
            }
            set
            {
                lighterDelay = value;
            }
        }
        /// <summary>
        /// [Lighter] How many positions to write before starting to blacken them?
        /// </summary>
        public int LighterMaxPositions
        {
            get
            {
                return lighterMaxPositions;
            }
            set
            {
                lighterMaxPositions = value;
            }
        }
        /// <summary>
        /// [Lighter] Screensaver background color
        /// </summary>
        public string LighterBackgroundColor
        {
            get
            {
                return lighterBackgroundColor;
            }
            set
            {
                lighterBackgroundColor = value;
            }
        }
        /// <summary>
        /// [Lighter] The minimum red color level (true color)
        /// </summary>
        public int LighterMinimumRedColorLevel
        {
            get
            {
                return lighterMinimumRedColorLevel;
            }
            set
            {
                lighterMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Lighter] The minimum green color level (true color)
        /// </summary>
        public int LighterMinimumGreenColorLevel
        {
            get
            {
                return lighterMinimumGreenColorLevel;
            }
            set
            {
                lighterMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Lighter] The minimum blue color level (true color)
        /// </summary>
        public int LighterMinimumBlueColorLevel
        {
            get
            {
                return lighterMinimumBlueColorLevel;
            }
            set
            {
                lighterMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Lighter] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int LighterMinimumColorLevel
        {
            get
            {
                return lighterMinimumColorLevel;
            }
            set
            {
                lighterMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Lighter] The maximum red color level (true color)
        /// </summary>
        public int LighterMaximumRedColorLevel
        {
            get
            {
                return lighterMaximumRedColorLevel;
            }
            set
            {
                lighterMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Lighter] The maximum green color level (true color)
        /// </summary>
        public int LighterMaximumGreenColorLevel
        {
            get
            {
                return lighterMaximumGreenColorLevel;
            }
            set
            {
                lighterMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Lighter] The maximum blue color level (true color)
        /// </summary>
        public int LighterMaximumBlueColorLevel
        {
            get
            {
                return lighterMaximumBlueColorLevel;
            }
            set
            {
                lighterMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Lighter] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int LighterMaximumColorLevel
        {
            get
            {
                return lighterMaximumColorLevel;
            }
            set
            {
                lighterMaximumColorLevel = value;
            }
        }
    }
}
