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

namespace Nitrocid.ScreensaverPacks.Settings
{
    /// <summary>
    /// Screensaver kernel configuration instance
    /// </summary>
    public partial class ExtraSaversConfig : BaseKernelConfig
    {
        private bool discoTrueColor = true;
        private int discoDelay = 100;
        private bool discoUseBeatsPerMinute;
        private bool discoCycleColors;
        private bool discoEnableFedMode;
        private int discoMinimumRedColorLevel = 0;
        private int discoMinimumGreenColorLevel = 0;
        private int discoMinimumBlueColorLevel = 0;
        private int discoMinimumColorLevel = 0;
        private int discoMaximumRedColorLevel = 255;
        private int discoMaximumGreenColorLevel = 255;
        private int discoMaximumBlueColorLevel = 255;
        private int discoMaximumColorLevel = 255;

        /// <summary>
        /// [Disco] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool DiscoTrueColor
        {
            get
            {
                return discoTrueColor;
            }
            set
            {
                discoTrueColor = value;
            }
        }

        /// <summary>
        /// [Disco] How many milliseconds, or beats per minute, to wait before making the next write?
        /// </summary>
        public int DiscoDelay
        {
            get
            {
                return discoDelay;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                discoDelay = value;
            }
        }

        /// <summary>
        /// [Disco] Whether to use the Beats Per Minute (1/4) to change the writing delay. If False, will use the standard milliseconds delay instead.
        /// </summary>
        public bool DiscoUseBeatsPerMinute
        {
            get
            {
                return discoUseBeatsPerMinute;
            }
            set
            {
                discoUseBeatsPerMinute = value;
            }
        }

        /// <summary>
        /// [Disco] Enable color cycling
        /// </summary>
        public bool DiscoCycleColors
        {
            get
            {
                return discoCycleColors;
            }
            set
            {
                discoCycleColors = value;
            }
        }

        /// <summary>
        /// [Disco] Uses the black and white cycle to produce the same effect as the legacy "fed" screensaver introduced back in v0.0.1
        /// </summary>
        public bool DiscoEnableFedMode
        {
            get
            {
                return discoEnableFedMode;
            }
            set
            {
                discoEnableFedMode = value;
            }
        }

        /// <summary>
        /// [Disco] The minimum red color level (true color)
        /// </summary>
        public int DiscoMinimumRedColorLevel
        {
            get
            {
                return discoMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                discoMinimumRedColorLevel = value;
            }
        }

        /// <summary>
        /// [Disco] The minimum green color level (true color)
        /// </summary>
        public int DiscoMinimumGreenColorLevel
        {
            get
            {
                return discoMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                discoMinimumGreenColorLevel = value;
            }
        }

        /// <summary>
        /// [Disco] The minimum blue color level (true color)
        /// </summary>
        public int DiscoMinimumBlueColorLevel
        {
            get
            {
                return discoMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                discoMinimumBlueColorLevel = value;
            }
        }

        /// <summary>
        /// [Disco] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int DiscoMinimumColorLevel
        {
            get
            {
                return discoMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                discoMinimumColorLevel = value;
            }
        }

        /// <summary>
        /// [Disco] The maximum red color level (true color)
        /// </summary>
        public int DiscoMaximumRedColorLevel
        {
            get
            {
                return discoMaximumRedColorLevel;
            }
            set
            {
                if (value <= discoMaximumRedColorLevel)
                    value = discoMaximumRedColorLevel;
                if (value > 255)
                    value = 255;
                discoMaximumRedColorLevel = value;
            }
        }

        /// <summary>
        /// [Disco] The maximum green color level (true color)
        /// </summary>
        public int DiscoMaximumGreenColorLevel
        {
            get
            {
                return discoMaximumGreenColorLevel;
            }
            set
            {
                if (value <= discoMaximumGreenColorLevel)
                    value = discoMaximumGreenColorLevel;
                if (value > 255)
                    value = 255;
                discoMaximumGreenColorLevel = value;
            }
        }

        /// <summary>
        /// [Disco] The maximum blue color level (true color)
        /// </summary>
        public int DiscoMaximumBlueColorLevel
        {
            get
            {
                return discoMaximumBlueColorLevel;
            }
            set
            {
                if (value <= discoMaximumBlueColorLevel)
                    value = discoMaximumBlueColorLevel;
                if (value > 255)
                    value = 255;
                discoMaximumBlueColorLevel = value;
            }
        }

        /// <summary>
        /// [Disco] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int DiscoMaximumColorLevel
        {
            get
            {
                return discoMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= discoMaximumColorLevel)
                    value = discoMaximumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                discoMaximumColorLevel = value;
            }
        }
    }
}
