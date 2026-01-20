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
        private bool barWaveTrueColor = true;
        private int barWaveDelay = 100;
        private double barWaveFrequencyLevel = 2;
        private int barWaveMinimumRedColorLevel = 0;
        private int barWaveMinimumGreenColorLevel = 0;
        private int barWaveMinimumBlueColorLevel = 0;
        private int barWaveMinimumColorLevel = 0;
        private int barWaveMaximumRedColorLevel = 255;
        private int barWaveMaximumGreenColorLevel = 255;
        private int barWaveMaximumBlueColorLevel = 255;
        private int barWaveMaximumColorLevel = 255;

        /// <summary>
        /// [BarWave] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool BarWaveTrueColor
        {
            get
            {
                return barWaveTrueColor;
            }
            set
            {
                barWaveTrueColor = value;
            }
        }
        /// <summary>
        /// [BarWave] The level of the frequency. This is the denominator of the Pi value (3.1415926...) in mathematics, defined by <see cref="Math.PI"/>.
        /// </summary>
        public double BarWaveFrequencyLevel
        {
            get
            {
                return barWaveFrequencyLevel;
            }
            set
            {
                if (value <= 0)
                    value = 2;
                barWaveFrequencyLevel = value;
            }
        }
        /// <summary>
        /// [BarWave] How many milliseconds to wait before making the next write?
        /// </summary>
        public int BarWaveDelay
        {
            get
            {
                return barWaveDelay;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                barWaveDelay = value;
            }
        }
        /// <summary>
        /// [BarWave] The minimum red color level (true color)
        /// </summary>
        public int BarWaveMinimumRedColorLevel
        {
            get
            {
                return barWaveMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                barWaveMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BarWave] The minimum green color level (true color)
        /// </summary>
        public int BarWaveMinimumGreenColorLevel
        {
            get
            {
                return barWaveMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                barWaveMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BarWave] The minimum blue color level (true color)
        /// </summary>
        public int BarWaveMinimumBlueColorLevel
        {
            get
            {
                return barWaveMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                barWaveMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BarWave] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int BarWaveMinimumColorLevel
        {
            get
            {
                return barWaveMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                barWaveMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [BarWave] The maximum red color level (true color)
        /// </summary>
        public int BarWaveMaximumRedColorLevel
        {
            get
            {
                return barWaveMaximumRedColorLevel;
            }
            set
            {
                if (value <= barWaveMaximumRedColorLevel)
                    value = barWaveMaximumRedColorLevel;
                if (value > 255)
                    value = 255;
                barWaveMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BarWave] The maximum green color level (true color)
        /// </summary>
        public int BarWaveMaximumGreenColorLevel
        {
            get
            {
                return barWaveMaximumGreenColorLevel;
            }
            set
            {
                if (value <= barWaveMaximumGreenColorLevel)
                    value = barWaveMaximumGreenColorLevel;
                if (value > 255)
                    value = 255;
                barWaveMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BarWave] The maximum blue color level (true color)
        /// </summary>
        public int BarWaveMaximumBlueColorLevel
        {
            get
            {
                return barWaveMaximumBlueColorLevel;
            }
            set
            {
                if (value <= barWaveMaximumBlueColorLevel)
                    value = barWaveMaximumBlueColorLevel;
                if (value > 255)
                    value = 255;
                barWaveMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BarWave] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int BarWaveMaximumColorLevel
        {
            get
            {
                return barWaveMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= barWaveMaximumColorLevel)
                    value = barWaveMaximumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                barWaveMaximumColorLevel = value;
            }
        }
    }
}
