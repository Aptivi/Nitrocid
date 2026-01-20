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
using System;

namespace Nitrocid.ScreensaverPacks.Settings
{
    /// <summary>
    /// Screensaver kernel configuration instance
    /// </summary>
    public partial class ExtraSaversConfig : BaseKernelConfig
    {
        private int swivelRevealDelay = 100;
        private double swivelRevealHorizontalFrequencyLevel = 3;
        private double swivelRevealVerticalFrequencyLevel = 8;
        private int swivelRevealMinimumRedColorLevel = 0;
        private int swivelRevealMinimumGreenColorLevel = 0;
        private int swivelRevealMinimumBlueColorLevel = 0;
        private int swivelRevealMinimumColorLevel = 0;
        private int swivelRevealMaximumRedColorLevel = 255;
        private int swivelRevealMaximumGreenColorLevel = 255;
        private int swivelRevealMaximumBlueColorLevel = 255;
        private int swivelRevealMaximumColorLevel = 255;

        /// <summary>
        /// [SwivelReveal] How many milliseconds to wait before making the next write?
        /// </summary>
        public int SwivelRevealDelay
        {
            get
            {
                return swivelRevealDelay;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                swivelRevealDelay = value;
            }
        }
        /// <summary>
        /// [SwivelReveal] The level of the horizontal frequency. This is the denominator of the Pi value (3.1415926...) in mathematics, defined by <see cref="Math.PI"/>. Use this to create beautiful wavy swivelReveals!
        /// </summary>
        public double SwivelRevealHorizontalFrequencyLevel
        {
            get
            {
                return swivelRevealHorizontalFrequencyLevel;
            }
            set
            {
                if (value <= 0)
                    value = 3;
                swivelRevealHorizontalFrequencyLevel = value;
            }
        }
        /// <summary>
        /// [SwivelReveal] The level of the vertical frequency. This is the denominator of the Pi value (3.1415926...) in mathematics, defined by <see cref="Math.PI"/>. Use this to create beautiful wavy swivelReveals!
        /// </summary>
        public double SwivelRevealVerticalFrequencyLevel
        {
            get
            {
                return swivelRevealVerticalFrequencyLevel;
            }
            set
            {
                if (value <= 0)
                    value = 8;
                swivelRevealVerticalFrequencyLevel = value;
            }
        }
        /// <summary>
        /// [SwivelReveal] The minimum red color level (true color)
        /// </summary>
        public int SwivelRevealMinimumRedColorLevel
        {
            get
            {
                return swivelRevealMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                swivelRevealMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [SwivelReveal] The minimum green color level (true color)
        /// </summary>
        public int SwivelRevealMinimumGreenColorLevel
        {
            get
            {
                return swivelRevealMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                swivelRevealMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [SwivelReveal] The minimum blue color level (true color)
        /// </summary>
        public int SwivelRevealMinimumBlueColorLevel
        {
            get
            {
                return swivelRevealMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                swivelRevealMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [SwivelReveal] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int SwivelRevealMinimumColorLevel
        {
            get
            {
                return swivelRevealMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                swivelRevealMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [SwivelReveal] The maximum red color level (true color)
        /// </summary>
        public int SwivelRevealMaximumRedColorLevel
        {
            get
            {
                return swivelRevealMaximumRedColorLevel;
            }
            set
            {
                if (value <= swivelRevealMaximumRedColorLevel)
                    value = swivelRevealMaximumRedColorLevel;
                if (value > 255)
                    value = 255;
                swivelRevealMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [SwivelReveal] The maximum green color level (true color)
        /// </summary>
        public int SwivelRevealMaximumGreenColorLevel
        {
            get
            {
                return swivelRevealMaximumGreenColorLevel;
            }
            set
            {
                if (value <= swivelRevealMaximumGreenColorLevel)
                    value = swivelRevealMaximumGreenColorLevel;
                if (value > 255)
                    value = 255;
                swivelRevealMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [SwivelReveal] The maximum blue color level (true color)
        /// </summary>
        public int SwivelRevealMaximumBlueColorLevel
        {
            get
            {
                return swivelRevealMaximumBlueColorLevel;
            }
            set
            {
                if (value <= swivelRevealMaximumBlueColorLevel)
                    value = swivelRevealMaximumBlueColorLevel;
                if (value > 255)
                    value = 255;
                swivelRevealMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [SwivelReveal] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int SwivelRevealMaximumColorLevel
        {
            get
            {
                return swivelRevealMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= swivelRevealMaximumColorLevel)
                    value = swivelRevealMaximumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                swivelRevealMaximumColorLevel = value;
            }
        }
    }
}
