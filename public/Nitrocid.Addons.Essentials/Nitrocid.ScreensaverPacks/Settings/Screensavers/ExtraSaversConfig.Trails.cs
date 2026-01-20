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
        private int trailsDelay = 100;
        private double trailsHorizontalFrequencyLevel = 3;
        private double trailsVerticalFrequencyLevel = 8;
        private double trailsTrailLength = 10;
        private int trailsMinimumRedColorLevel = 0;
        private int trailsMinimumGreenColorLevel = 0;
        private int trailsMinimumBlueColorLevel = 0;
        private int trailsMinimumColorLevel = 0;
        private int trailsMaximumRedColorLevel = 255;
        private int trailsMaximumGreenColorLevel = 255;
        private int trailsMaximumBlueColorLevel = 255;
        private int trailsMaximumColorLevel = 255;

        /// <summary>
        /// [Trails] How many milliseconds to wait before making the next write?
        /// </summary>
        public int TrailsDelay
        {
            get
            {
                return trailsDelay;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                trailsDelay = value;
            }
        }
        /// <summary>
        /// [Trails] The level of the horizontal frequency. This is the denominator of the Pi value (3.1415926...) in mathematics, defined by <see cref="Math.PI"/>. Use this to create beautiful wavy swivels!
        /// </summary>
        public double TrailsHorizontalFrequencyLevel
        {
            get
            {
                return trailsHorizontalFrequencyLevel;
            }
            set
            {
                if (value <= 0)
                    value = 3;
                trailsHorizontalFrequencyLevel = value;
            }
        }
        /// <summary>
        /// [Trails] The level of the vertical frequency. This is the denominator of the Pi value (3.1415926...) in mathematics, defined by <see cref="Math.PI"/>. Use this to create beautiful wavy swivels!
        /// </summary>
        public double TrailsVerticalFrequencyLevel
        {
            get
            {
                return trailsVerticalFrequencyLevel;
            }
            set
            {
                if (value <= 0)
                    value = 8;
                trailsVerticalFrequencyLevel = value;
            }
        }
        /// <summary>
        /// [Trails] The length of the trail
        /// </summary>
        public double TrailsTrailLength
        {
            get
            {
                return trailsTrailLength;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                trailsTrailLength = value;
            }
        }
        /// <summary>
        /// [Trails] The minimum red color level (true color)
        /// </summary>
        public int TrailsMinimumRedColorLevel
        {
            get
            {
                return trailsMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                trailsMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Trails] The minimum green color level (true color)
        /// </summary>
        public int TrailsMinimumGreenColorLevel
        {
            get
            {
                return trailsMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                trailsMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Trails] The minimum blue color level (true color)
        /// </summary>
        public int TrailsMinimumBlueColorLevel
        {
            get
            {
                return trailsMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                trailsMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Trails] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int TrailsMinimumColorLevel
        {
            get
            {
                return trailsMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                trailsMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Trails] The maximum red color level (true color)
        /// </summary>
        public int TrailsMaximumRedColorLevel
        {
            get
            {
                return trailsMaximumRedColorLevel;
            }
            set
            {
                if (value <= trailsMaximumRedColorLevel)
                    value = trailsMaximumRedColorLevel;
                if (value > 255)
                    value = 255;
                trailsMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Trails] The maximum green color level (true color)
        /// </summary>
        public int TrailsMaximumGreenColorLevel
        {
            get
            {
                return trailsMaximumGreenColorLevel;
            }
            set
            {
                if (value <= trailsMaximumGreenColorLevel)
                    value = trailsMaximumGreenColorLevel;
                if (value > 255)
                    value = 255;
                trailsMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Trails] The maximum blue color level (true color)
        /// </summary>
        public int TrailsMaximumBlueColorLevel
        {
            get
            {
                return trailsMaximumBlueColorLevel;
            }
            set
            {
                if (value <= trailsMaximumBlueColorLevel)
                    value = trailsMaximumBlueColorLevel;
                if (value > 255)
                    value = 255;
                trailsMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Trails] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int TrailsMaximumColorLevel
        {
            get
            {
                return trailsMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= trailsMaximumColorLevel)
                    value = trailsMaximumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                trailsMaximumColorLevel = value;
            }
        }
    }
}
