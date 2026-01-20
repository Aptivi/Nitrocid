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
        private bool wipeTrueColor = true;
        private int wipeDelay = 10;
        private int wipeWipesNeededToChangeDirection = 10;
        private string wipeBackgroundColor = new Color(ConsoleColors.Black).PlainSequence;
        private int wipeMinimumRedColorLevel = 0;
        private int wipeMinimumGreenColorLevel = 0;
        private int wipeMinimumBlueColorLevel = 0;
        private int wipeMinimumColorLevel = 0;
        private int wipeMaximumRedColorLevel = 255;
        private int wipeMaximumGreenColorLevel = 255;
        private int wipeMaximumBlueColorLevel = 255;
        private int wipeMaximumColorLevel = 255;

        /// <summary>
        /// [Wipe] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool WipeTrueColor
        {
            get
            {
                return wipeTrueColor;
            }
            set
            {
                wipeTrueColor = value;
            }
        }
        /// <summary>
        /// [Wipe] How many milliseconds to wait before making the next write?
        /// </summary>
        public int WipeDelay
        {
            get
            {
                return wipeDelay;
            }
            set
            {
                wipeDelay = value;
            }
        }
        /// <summary>
        /// [Wipe] How many wipes needed to change direction?
        /// </summary>
        public int WipeWipesNeededToChangeDirection
        {
            get
            {
                return wipeWipesNeededToChangeDirection;
            }
            set
            {
                wipeWipesNeededToChangeDirection = value;
            }
        }
        /// <summary>
        /// [Wipe] Screensaver background color
        /// </summary>
        public string WipeBackgroundColor
        {
            get
            {
                return wipeBackgroundColor;
            }
            set
            {
                wipeBackgroundColor = value;
            }
        }
        /// <summary>
        /// [Wipe] The minimum red color level (true color)
        /// </summary>
        public int WipeMinimumRedColorLevel
        {
            get
            {
                return wipeMinimumRedColorLevel;
            }
            set
            {
                wipeMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Wipe] The minimum green color level (true color)
        /// </summary>
        public int WipeMinimumGreenColorLevel
        {
            get
            {
                return wipeMinimumGreenColorLevel;
            }
            set
            {
                wipeMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Wipe] The minimum blue color level (true color)
        /// </summary>
        public int WipeMinimumBlueColorLevel
        {
            get
            {
                return wipeMinimumBlueColorLevel;
            }
            set
            {
                wipeMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Wipe] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int WipeMinimumColorLevel
        {
            get
            {
                return wipeMinimumColorLevel;
            }
            set
            {
                wipeMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Wipe] The maximum red color level (true color)
        /// </summary>
        public int WipeMaximumRedColorLevel
        {
            get
            {
                return wipeMaximumRedColorLevel;
            }
            set
            {
                wipeMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Wipe] The maximum green color level (true color)
        /// </summary>
        public int WipeMaximumGreenColorLevel
        {
            get
            {
                return wipeMaximumGreenColorLevel;
            }
            set
            {
                wipeMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Wipe] The maximum blue color level (true color)
        /// </summary>
        public int WipeMaximumBlueColorLevel
        {
            get
            {
                return wipeMaximumBlueColorLevel;
            }
            set
            {
                wipeMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Wipe] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int WipeMaximumColorLevel
        {
            get
            {
                return wipeMaximumColorLevel;
            }
            set
            {
                wipeMaximumColorLevel = value;
            }
        }
    }
}
