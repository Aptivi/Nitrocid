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
        private int waveDelay = 100;
        private double waveFrequencyLevel = 3;
        private int waveMinimumRedColorLevel = 0;
        private int waveMinimumGreenColorLevel = 0;
        private int waveMinimumBlueColorLevel = 0;
        private int waveMinimumColorLevel = 0;
        private int waveMaximumRedColorLevel = 255;
        private int waveMaximumGreenColorLevel = 255;
        private int waveMaximumBlueColorLevel = 255;
        private int waveMaximumColorLevel = 255;

        /// <summary>
        /// [Wave] How many milliseconds to wait before making the next write?
        /// </summary>
        public int WaveDelay
        {
            get
            {
                return waveDelay;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                waveDelay = value;
            }
        }
        /// <summary>
        /// [Wave] The level of the frequency. This is the denominator of the Pi value (3.1415926...) in mathematics, defined by <see cref="Math.PI"/>.
        /// </summary>
        public double WaveFrequencyLevel
        {
            get
            {
                return waveFrequencyLevel;
            }
            set
            {
                if (value <= 0)
                    value = 3;
                waveFrequencyLevel = value;
            }
        }
        /// <summary>
        /// [Wave] The minimum red color level (true color)
        /// </summary>
        public int WaveMinimumRedColorLevel
        {
            get
            {
                return waveMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                waveMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Wave] The minimum green color level (true color)
        /// </summary>
        public int WaveMinimumGreenColorLevel
        {
            get
            {
                return waveMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                waveMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Wave] The minimum blue color level (true color)
        /// </summary>
        public int WaveMinimumBlueColorLevel
        {
            get
            {
                return waveMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                waveMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Wave] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int WaveMinimumColorLevel
        {
            get
            {
                return waveMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                waveMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Wave] The maximum red color level (true color)
        /// </summary>
        public int WaveMaximumRedColorLevel
        {
            get
            {
                return waveMaximumRedColorLevel;
            }
            set
            {
                if (value <= waveMaximumRedColorLevel)
                    value = waveMaximumRedColorLevel;
                if (value > 255)
                    value = 255;
                waveMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Wave] The maximum green color level (true color)
        /// </summary>
        public int WaveMaximumGreenColorLevel
        {
            get
            {
                return waveMaximumGreenColorLevel;
            }
            set
            {
                if (value <= waveMaximumGreenColorLevel)
                    value = waveMaximumGreenColorLevel;
                if (value > 255)
                    value = 255;
                waveMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Wave] The maximum blue color level (true color)
        /// </summary>
        public int WaveMaximumBlueColorLevel
        {
            get
            {
                return waveMaximumBlueColorLevel;
            }
            set
            {
                if (value <= waveMaximumBlueColorLevel)
                    value = waveMaximumBlueColorLevel;
                if (value > 255)
                    value = 255;
                waveMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Wave] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int WaveMaximumColorLevel
        {
            get
            {
                return waveMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= waveMaximumColorLevel)
                    value = waveMaximumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                waveMaximumColorLevel = value;
            }
        }
    }
}
