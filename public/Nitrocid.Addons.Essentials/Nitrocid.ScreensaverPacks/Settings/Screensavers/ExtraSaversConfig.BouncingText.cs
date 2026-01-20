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
        private bool bouncingTextTrueColor = true;
        private int bouncingTextDelay = 50;
        private string bouncingTextWrite = "Nitrocid KS";
        private string bouncingTextBackgroundColor = new Color(ConsoleColors.Black).PlainSequence;
        private string bouncingTextForegroundColor = new Color(ConsoleColors.White).PlainSequence;
        private int bouncingTextMinimumRedColorLevel = 0;
        private int bouncingTextMinimumGreenColorLevel = 0;
        private int bouncingTextMinimumBlueColorLevel = 0;
        private int bouncingTextMinimumColorLevel = 0;
        private int bouncingTextMaximumRedColorLevel = 255;
        private int bouncingTextMaximumGreenColorLevel = 255;
        private int bouncingTextMaximumBlueColorLevel = 255;
        private int bouncingTextMaximumColorLevel = 255;

        /// <summary>
        /// [BouncingText] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool BouncingTextTrueColor
        {
            get
            {
                return bouncingTextTrueColor;
            }
            set
            {
                bouncingTextTrueColor = value;
            }
        }
        /// <summary>
        /// [BouncingText] How many milliseconds to wait before making the next write?
        /// </summary>
        public int BouncingTextDelay
        {
            get
            {
                return bouncingTextDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                bouncingTextDelay = value;
            }
        }
        /// <summary>
        /// [BouncingText] Text for Bouncing Text. Shorter is better.
        /// </summary>
        public string BouncingTextWrite
        {
            get
            {
                return bouncingTextWrite;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Nitrocid KS";
                bouncingTextWrite = value;
            }
        }
        /// <summary>
        /// [BouncingText] Screensaver background color
        /// </summary>
        public string BouncingTextBackgroundColor
        {
            get
            {
                return bouncingTextBackgroundColor;
            }
            set
            {
                bouncingTextBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [BouncingText] Screensaver foreground color
        /// </summary>
        public string BouncingTextForegroundColor
        {
            get
            {
                return bouncingTextForegroundColor;
            }
            set
            {
                bouncingTextForegroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [BouncingText] The minimum red color level (true color)
        /// </summary>
        public int BouncingTextMinimumRedColorLevel
        {
            get
            {
                return bouncingTextMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                bouncingTextMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingText] The minimum green color level (true color)
        /// </summary>
        public int BouncingTextMinimumGreenColorLevel
        {
            get
            {
                return bouncingTextMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                bouncingTextMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingText] The minimum blue color level (true color)
        /// </summary>
        public int BouncingTextMinimumBlueColorLevel
        {
            get
            {
                return bouncingTextMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                bouncingTextMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingText] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int BouncingTextMinimumColorLevel
        {
            get
            {
                return bouncingTextMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                bouncingTextMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingText] The maximum red color level (true color)
        /// </summary>
        public int BouncingTextMaximumRedColorLevel
        {
            get
            {
                return bouncingTextMaximumRedColorLevel;
            }
            set
            {
                if (value <= bouncingTextMinimumRedColorLevel)
                    value = bouncingTextMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                bouncingTextMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingText] The maximum green color level (true color)
        /// </summary>
        public int BouncingTextMaximumGreenColorLevel
        {
            get
            {
                return bouncingTextMaximumGreenColorLevel;
            }
            set
            {
                if (value <= bouncingTextMinimumGreenColorLevel)
                    value = bouncingTextMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                bouncingTextMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingText] The maximum blue color level (true color)
        /// </summary>
        public int BouncingTextMaximumBlueColorLevel
        {
            get
            {
                return bouncingTextMaximumBlueColorLevel;
            }
            set
            {
                if (value <= bouncingTextMinimumBlueColorLevel)
                    value = bouncingTextMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                bouncingTextMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingText] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int BouncingTextMaximumColorLevel
        {
            get
            {
                return bouncingTextMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= bouncingTextMinimumColorLevel)
                    value = bouncingTextMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                bouncingTextMaximumColorLevel = value;
            }
        }
    }
}
