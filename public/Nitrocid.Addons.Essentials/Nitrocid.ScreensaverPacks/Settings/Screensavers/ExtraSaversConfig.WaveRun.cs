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
        private int waveRunDelay = 100;
        private double waveRunFrequencyLevel = 3;
        private int waveRunMinimumRedColorLevel = 0;
        private int waveRunMinimumGreenColorLevel = 0;
        private int waveRunMinimumBlueColorLevel = 0;
        private int waveRunMinimumColorLevel = 0;
        private int waveRunMaximumRedColorLevel = 255;
        private int waveRunMaximumGreenColorLevel = 255;
        private int waveRunMaximumBlueColorLevel = 255;
        private int waveRunMaximumColorLevel = 255;

        /// <summary>
        /// [WaveRun] How many milliseconds to wait before making the next write?
        /// </summary>
        public int WaveRunDelay
        {
            get
            {
                return waveRunDelay;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                waveRunDelay = value;
            }
        }
        /// <summary>
        /// [WaveRun] The level of the frequency. This is the denominator of the Pi value (3.1415926...) in mathematics, defined by <see cref="Math.PI"/>.
        /// </summary>
        public double WaveRunFrequencyLevel
        {
            get
            {
                return waveRunFrequencyLevel;
            }
            set
            {
                if (value <= 0)
                    value = 3;
                waveRunFrequencyLevel = value;
            }
        }
        /// <summary>
        /// [WaveRun] The minimum red color level (true color)
        /// </summary>
        public int WaveRunMinimumRedColorLevel
        {
            get
            {
                return waveRunMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                waveRunMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [WaveRun] The minimum green color level (true color)
        /// </summary>
        public int WaveRunMinimumGreenColorLevel
        {
            get
            {
                return waveRunMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                waveRunMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [WaveRun] The minimum blue color level (true color)
        /// </summary>
        public int WaveRunMinimumBlueColorLevel
        {
            get
            {
                return waveRunMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                waveRunMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [WaveRun] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int WaveRunMinimumColorLevel
        {
            get
            {
                return waveRunMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                waveRunMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [WaveRun] The maximum red color level (true color)
        /// </summary>
        public int WaveRunMaximumRedColorLevel
        {
            get
            {
                return waveRunMaximumRedColorLevel;
            }
            set
            {
                if (value <= waveRunMaximumRedColorLevel)
                    value = waveRunMaximumRedColorLevel;
                if (value > 255)
                    value = 255;
                waveRunMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [WaveRun] The maximum green color level (true color)
        /// </summary>
        public int WaveRunMaximumGreenColorLevel
        {
            get
            {
                return waveRunMaximumGreenColorLevel;
            }
            set
            {
                if (value <= waveRunMaximumGreenColorLevel)
                    value = waveRunMaximumGreenColorLevel;
                if (value > 255)
                    value = 255;
                waveRunMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [WaveRun] The maximum blue color level (true color)
        /// </summary>
        public int WaveRunMaximumBlueColorLevel
        {
            get
            {
                return waveRunMaximumBlueColorLevel;
            }
            set
            {
                if (value <= waveRunMaximumBlueColorLevel)
                    value = waveRunMaximumBlueColorLevel;
                if (value > 255)
                    value = 255;
                waveRunMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [WaveRun] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int WaveRunMaximumColorLevel
        {
            get
            {
                return waveRunMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= waveRunMaximumColorLevel)
                    value = waveRunMaximumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                waveRunMaximumColorLevel = value;
            }
        }
    }
}
