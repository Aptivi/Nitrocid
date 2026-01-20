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
        private int swivelDelay = 100;
        private double swivelHorizontalFrequencyLevel = 3;
        private double swivelVerticalFrequencyLevel = 8;
        private int swivelMinimumRedColorLevel = 0;
        private int swivelMinimumGreenColorLevel = 0;
        private int swivelMinimumBlueColorLevel = 0;
        private int swivelMinimumColorLevel = 0;
        private int swivelMaximumRedColorLevel = 255;
        private int swivelMaximumGreenColorLevel = 255;
        private int swivelMaximumBlueColorLevel = 255;
        private int swivelMaximumColorLevel = 255;

        /// <summary>
        /// [Swivel] How many milliseconds to wait before making the next write?
        /// </summary>
        public int SwivelDelay
        {
            get
            {
                return swivelDelay;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                swivelDelay = value;
            }
        }
        /// <summary>
        /// [Swivel] The level of the horizontal frequency. This is the denominator of the Pi value (3.1415926...) in mathematics, defined by <see cref="Math.PI"/>. Use this to create beautiful wavy swivels!
        /// </summary>
        public double SwivelHorizontalFrequencyLevel
        {
            get
            {
                return swivelHorizontalFrequencyLevel;
            }
            set
            {
                if (value <= 0)
                    value = 3;
                swivelHorizontalFrequencyLevel = value;
            }
        }
        /// <summary>
        /// [Swivel] The level of the vertical frequency. This is the denominator of the Pi value (3.1415926...) in mathematics, defined by <see cref="Math.PI"/>. Use this to create beautiful wavy swivels!
        /// </summary>
        public double SwivelVerticalFrequencyLevel
        {
            get
            {
                return swivelVerticalFrequencyLevel;
            }
            set
            {
                if (value <= 0)
                    value = 8;
                swivelVerticalFrequencyLevel = value;
            }
        }
        /// <summary>
        /// [Swivel] The minimum red color level (true color)
        /// </summary>
        public int SwivelMinimumRedColorLevel
        {
            get
            {
                return swivelMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                swivelMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Swivel] The minimum green color level (true color)
        /// </summary>
        public int SwivelMinimumGreenColorLevel
        {
            get
            {
                return swivelMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                swivelMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Swivel] The minimum blue color level (true color)
        /// </summary>
        public int SwivelMinimumBlueColorLevel
        {
            get
            {
                return swivelMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                swivelMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Swivel] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int SwivelMinimumColorLevel
        {
            get
            {
                return swivelMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                swivelMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Swivel] The maximum red color level (true color)
        /// </summary>
        public int SwivelMaximumRedColorLevel
        {
            get
            {
                return swivelMaximumRedColorLevel;
            }
            set
            {
                if (value <= swivelMaximumRedColorLevel)
                    value = swivelMaximumRedColorLevel;
                if (value > 255)
                    value = 255;
                swivelMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Swivel] The maximum green color level (true color)
        /// </summary>
        public int SwivelMaximumGreenColorLevel
        {
            get
            {
                return swivelMaximumGreenColorLevel;
            }
            set
            {
                if (value <= swivelMaximumGreenColorLevel)
                    value = swivelMaximumGreenColorLevel;
                if (value > 255)
                    value = 255;
                swivelMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Swivel] The maximum blue color level (true color)
        /// </summary>
        public int SwivelMaximumBlueColorLevel
        {
            get
            {
                return swivelMaximumBlueColorLevel;
            }
            set
            {
                if (value <= swivelMaximumBlueColorLevel)
                    value = swivelMaximumBlueColorLevel;
                if (value > 255)
                    value = 255;
                swivelMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Swivel] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int SwivelMaximumColorLevel
        {
            get
            {
                return swivelMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= swivelMaximumColorLevel)
                    value = swivelMaximumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                swivelMaximumColorLevel = value;
            }
        }
    }
}
