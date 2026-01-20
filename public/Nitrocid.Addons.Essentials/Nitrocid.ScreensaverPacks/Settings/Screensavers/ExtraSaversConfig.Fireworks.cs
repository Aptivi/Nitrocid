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
        private bool fireworksTrueColor = true;
        private int fireworksDelay = 50;
        private int fireworksRadius = 5;
        private int fireworksMinimumRedColorLevel = 0;
        private int fireworksMinimumGreenColorLevel = 0;
        private int fireworksMinimumBlueColorLevel = 0;
        private int fireworksMinimumColorLevel = 0;
        private int fireworksMaximumRedColorLevel = 255;
        private int fireworksMaximumGreenColorLevel = 255;
        private int fireworksMaximumBlueColorLevel = 255;
        private int fireworksMaximumColorLevel = 255;

        /// <summary>
        /// [Fireworks] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool FireworksTrueColor
        {
            get
            {
                return fireworksTrueColor;
            }
            set
            {
                fireworksTrueColor = value;
            }
        }
        /// <summary>
        /// [Fireworks] How many milliseconds to wait before making the next write?
        /// </summary>
        public int FireworksDelay
        {
            get
            {
                return fireworksDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                fireworksDelay = value;
            }
        }
        /// <summary>
        /// [Fireworks] The radius of the explosion
        /// </summary>
        public int FireworksRadius
        {
            get
            {
                return fireworksRadius;
            }
            set
            {
                if (value <= 0)
                    value = 5;
                fireworksRadius = value;
            }
        }
        /// <summary>
        /// [Fireworks] The minimum red color level (true color)
        /// </summary>
        public int FireworksMinimumRedColorLevel
        {
            get
            {
                return fireworksMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                fireworksMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Fireworks] The minimum green color level (true color)
        /// </summary>
        public int FireworksMinimumGreenColorLevel
        {
            get
            {
                return fireworksMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                fireworksMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Fireworks] The minimum blue color level (true color)
        /// </summary>
        public int FireworksMinimumBlueColorLevel
        {
            get
            {
                return fireworksMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                fireworksMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Fireworks] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int FireworksMinimumColorLevel
        {
            get
            {
                return fireworksMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                fireworksMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Fireworks] The maximum red color level (true color)
        /// </summary>
        public int FireworksMaximumRedColorLevel
        {
            get
            {
                return fireworksMaximumRedColorLevel;
            }
            set
            {
                if (value <= fireworksMinimumRedColorLevel)
                    value = fireworksMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                fireworksMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Fireworks] The maximum green color level (true color)
        /// </summary>
        public int FireworksMaximumGreenColorLevel
        {
            get
            {
                return fireworksMaximumGreenColorLevel;
            }
            set
            {
                if (value <= fireworksMinimumGreenColorLevel)
                    value = fireworksMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                fireworksMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Fireworks] The maximum blue color level (true color)
        /// </summary>
        public int FireworksMaximumBlueColorLevel
        {
            get
            {
                return fireworksMaximumBlueColorLevel;
            }
            set
            {
                if (value <= fireworksMinimumBlueColorLevel)
                    value = fireworksMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                fireworksMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Fireworks] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int FireworksMaximumColorLevel
        {
            get
            {
                return fireworksMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= fireworksMinimumColorLevel)
                    value = fireworksMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                fireworksMaximumColorLevel = value;
            }
        }
    }
}
