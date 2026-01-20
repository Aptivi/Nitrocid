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
        private bool bouncingBlockTrueColor = true;
        private int bouncingBlockDelay = 10;
        private string bouncingBlockBackgroundColor = new Color(ConsoleColors.Black).PlainSequence;
        private string bouncingBlockForegroundColor = new Color(ConsoleColors.White).PlainSequence;
        private int bouncingBlockMinimumRedColorLevel = 0;
        private int bouncingBlockMinimumGreenColorLevel = 0;
        private int bouncingBlockMinimumBlueColorLevel = 0;
        private int bouncingBlockMinimumColorLevel = 0;
        private int bouncingBlockMaximumRedColorLevel = 255;
        private int bouncingBlockMaximumGreenColorLevel = 255;
        private int bouncingBlockMaximumBlueColorLevel = 255;
        private int bouncingBlockMaximumColorLevel = 255;

        /// <summary>
        /// [BouncingBlock] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool BouncingBlockTrueColor
        {
            get
            {
                return bouncingBlockTrueColor;
            }
            set
            {
                bouncingBlockTrueColor = value;
            }
        }
        /// <summary>
        /// [BouncingBlock] How many milliseconds to wait before making the next write?
        /// </summary>
        public int BouncingBlockDelay
        {
            get
            {
                return bouncingBlockDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                bouncingBlockDelay = value;
            }
        }
        /// <summary>
        /// [BouncingBlock] Screensaver background color
        /// </summary>
        public string BouncingBlockBackgroundColor
        {
            get
            {
                return bouncingBlockBackgroundColor;
            }
            set
            {
                bouncingBlockBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [BouncingBlock] Screensaver foreground color
        /// </summary>
        public string BouncingBlockForegroundColor
        {
            get
            {
                return bouncingBlockForegroundColor;
            }
            set
            {
                bouncingBlockForegroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [BouncingBlock] The minimum red color level (true color)
        /// </summary>
        public int BouncingBlockMinimumRedColorLevel
        {
            get
            {
                return bouncingBlockMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                bouncingBlockMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingBlock] The minimum green color level (true color)
        /// </summary>
        public int BouncingBlockMinimumGreenColorLevel
        {
            get
            {
                return bouncingBlockMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                bouncingBlockMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingBlock] The minimum blue color level (true color)
        /// </summary>
        public int BouncingBlockMinimumBlueColorLevel
        {
            get
            {
                return bouncingBlockMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                bouncingBlockMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingBlock] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int BouncingBlockMinimumColorLevel
        {
            get
            {
                return bouncingBlockMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                bouncingBlockMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingBlock] The maximum red color level (true color)
        /// </summary>
        public int BouncingBlockMaximumRedColorLevel
        {
            get
            {
                return bouncingBlockMaximumRedColorLevel;
            }
            set
            {
                if (value <= bouncingBlockMinimumRedColorLevel)
                    value = bouncingBlockMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                bouncingBlockMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingBlock] The maximum green color level (true color)
        /// </summary>
        public int BouncingBlockMaximumGreenColorLevel
        {
            get
            {
                return bouncingBlockMaximumGreenColorLevel;
            }
            set
            {
                if (value <= bouncingBlockMinimumGreenColorLevel)
                    value = bouncingBlockMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                bouncingBlockMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingBlock] The maximum blue color level (true color)
        /// </summary>
        public int BouncingBlockMaximumBlueColorLevel
        {
            get
            {
                return bouncingBlockMaximumBlueColorLevel;
            }
            set
            {
                if (value <= bouncingBlockMinimumBlueColorLevel)
                    value = bouncingBlockMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                bouncingBlockMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingBlock] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int BouncingBlockMaximumColorLevel
        {
            get
            {
                return bouncingBlockMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= bouncingBlockMinimumColorLevel)
                    value = bouncingBlockMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                bouncingBlockMaximumColorLevel = value;
            }
        }
    }
}
