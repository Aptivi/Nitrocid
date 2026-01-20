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
        private bool rippleTrueColor = true;
        private int rippleDelay = 20;
        private int rippleMinimumRedColorLevel = 0;
        private int rippleMinimumGreenColorLevel = 0;
        private int rippleMinimumBlueColorLevel = 0;
        private int rippleMinimumColorLevel = 0;
        private int rippleMaximumRedColorLevel = 255;
        private int rippleMaximumGreenColorLevel = 255;
        private int rippleMaximumBlueColorLevel = 255;
        private int rippleMaximumColorLevel = 255;

        /// <summary>
        /// [Ripple] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool RippleTrueColor
        {
            get
            {
                return rippleTrueColor;
            }
            set
            {
                rippleTrueColor = value;
            }
        }
        /// <summary>
        /// [Ripple] How many milliseconds to wait before making the next write?
        /// </summary>
        public int RippleDelay
        {
            get
            {
                return rippleDelay;
            }
            set
            {
                if (value <= 0)
                    value = 20;
                rippleDelay = value;
            }
        }
        /// <summary>
        /// [Ripple] The minimum red color level (true color)
        /// </summary>
        public int RippleMinimumRedColorLevel
        {
            get
            {
                return rippleMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                rippleMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Ripple] The minimum green color level (true color)
        /// </summary>
        public int RippleMinimumGreenColorLevel
        {
            get
            {
                return rippleMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                rippleMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Ripple] The minimum blue color level (true color)
        /// </summary>
        public int RippleMinimumBlueColorLevel
        {
            get
            {
                return rippleMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                rippleMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Ripple] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int RippleMinimumColorLevel
        {
            get
            {
                return rippleMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                rippleMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Ripple] The maximum red color level (true color)
        /// </summary>
        public int RippleMaximumRedColorLevel
        {
            get
            {
                return rippleMaximumRedColorLevel;
            }
            set
            {
                if (value <= rippleMinimumRedColorLevel)
                    value = rippleMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                rippleMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Ripple] The maximum green color level (true color)
        /// </summary>
        public int RippleMaximumGreenColorLevel
        {
            get
            {
                return rippleMaximumGreenColorLevel;
            }
            set
            {
                if (value <= rippleMinimumGreenColorLevel)
                    value = rippleMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                rippleMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Ripple] The maximum blue color level (true color)
        /// </summary>
        public int RippleMaximumBlueColorLevel
        {
            get
            {
                return rippleMaximumBlueColorLevel;
            }
            set
            {
                if (value <= rippleMinimumBlueColorLevel)
                    value = rippleMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                rippleMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Ripple] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int RippleMaximumColorLevel
        {
            get
            {
                return rippleMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= rippleMinimumColorLevel)
                    value = rippleMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                rippleMaximumColorLevel = value;
            }
        }
    }
}
