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
        private bool colorMixTrueColor = true;
        private int colorMixDelay = 1;
        private string colorMixBackgroundColor = new Color(ConsoleColors.Black).PlainSequence;
        private int colorMixMinimumRedColorLevel = 0;
        private int colorMixMinimumGreenColorLevel = 0;
        private int colorMixMinimumBlueColorLevel = 0;
        private int colorMixMinimumColorLevel = 0;
        private int colorMixMaximumRedColorLevel = 255;
        private int colorMixMaximumGreenColorLevel = 255;
        private int colorMixMaximumBlueColorLevel = 255;
        private int colorMixMaximumColorLevel = 255;

        /// <summary>
        /// [ColorMix] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool ColorMixTrueColor
        {
            get
            {
                return colorMixTrueColor;
            }
            set
            {
                colorMixTrueColor = value;
            }
        }

        /// <summary>
        /// [ColorMix] How many milliseconds to wait before making the next write?
        /// </summary>
        public int ColorMixDelay
        {
            get
            {
                return colorMixDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1;
                colorMixDelay = value;
            }
        }

        /// <summary>
        /// [ColorMix] Screensaver background color
        /// </summary>
        public string ColorMixBackgroundColor
        {
            get
            {
                return colorMixBackgroundColor;
            }
            set
            {
                colorMixBackgroundColor = new Color(value).PlainSequence;
            }
        }

        /// <summary>
        /// [ColorMix] The minimum red color level (true color)
        /// </summary>
        public int ColorMixMinimumRedColorLevel
        {
            get
            {
                return colorMixMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                colorMixMinimumRedColorLevel = value;
            }
        }

        /// <summary>
        /// [ColorMix] The minimum green color level (true color)
        /// </summary>
        public int ColorMixMinimumGreenColorLevel
        {
            get
            {
                return colorMixMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                colorMixMinimumGreenColorLevel = value;
            }
        }

        /// <summary>
        /// [ColorMix] The minimum blue color level (true color)
        /// </summary>
        public int ColorMixMinimumBlueColorLevel
        {
            get
            {
                return colorMixMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                colorMixMinimumBlueColorLevel = value;
            }
        }

        /// <summary>
        /// [ColorMix] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int ColorMixMinimumColorLevel
        {
            get
            {
                return colorMixMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                colorMixMinimumColorLevel = value;
            }
        }

        /// <summary>
        /// [ColorMix] The maximum red color level (true color)
        /// </summary>
        public int ColorMixMaximumRedColorLevel
        {
            get
            {
                return colorMixMaximumRedColorLevel;
            }
            set
            {
                if (value <= colorMixMaximumRedColorLevel)
                    value = colorMixMaximumRedColorLevel;
                if (value > 255)
                    value = 255;
                colorMixMaximumRedColorLevel = value;
            }
        }

        /// <summary>
        /// [ColorMix] The maximum green color level (true color)
        /// </summary>
        public int ColorMixMaximumGreenColorLevel
        {
            get
            {
                return colorMixMaximumGreenColorLevel;
            }
            set
            {
                if (value <= colorMixMaximumGreenColorLevel)
                    value = colorMixMaximumGreenColorLevel;
                if (value > 255)
                    value = 255;
                colorMixMaximumGreenColorLevel = value;
            }
        }

        /// <summary>
        /// [ColorMix] The maximum blue color level (true color)
        /// </summary>
        public int ColorMixMaximumBlueColorLevel
        {
            get
            {
                return colorMixMaximumBlueColorLevel;
            }
            set
            {
                if (value <= colorMixMaximumBlueColorLevel)
                    value = colorMixMaximumBlueColorLevel;
                if (value > 255)
                    value = 255;
                colorMixMaximumBlueColorLevel = value;
            }
        }

        /// <summary>
        /// [ColorMix] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int ColorMixMaximumColorLevel
        {
            get
            {
                return colorMixMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= colorMixMaximumColorLevel)
                    value = colorMixMaximumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                colorMixMaximumColorLevel = value;
            }
        }
    }
}
