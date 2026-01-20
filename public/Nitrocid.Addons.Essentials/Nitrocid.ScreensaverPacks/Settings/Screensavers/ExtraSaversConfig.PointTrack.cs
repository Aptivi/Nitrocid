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
        private int pointTrackDelay = 100;
        private double pointTrackHorizontalFrequencyLevel = 3;
        private double pointTrackVerticalFrequencyLevel = 8;
        private int pointTrackMinimumRedColorLevel = 0;
        private int pointTrackMinimumGreenColorLevel = 0;
        private int pointTrackMinimumBlueColorLevel = 0;
        private int pointTrackMinimumColorLevel = 0;
        private int pointTrackMaximumRedColorLevel = 255;
        private int pointTrackMaximumGreenColorLevel = 255;
        private int pointTrackMaximumBlueColorLevel = 255;
        private int pointTrackMaximumColorLevel = 255;

        /// <summary>
        /// [PointTrack] How many milliseconds to wait before making the next write?
        /// </summary>
        public int PointTrackDelay
        {
            get
            {
                return pointTrackDelay;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                pointTrackDelay = value;
            }
        }
        /// <summary>
        /// [PointTrack] The level of the horizontal frequency. This is the denominator of the Pi value (3.1415926...) in mathematics, defined by <see cref="Math.PI"/>. Use this to create beautiful wavy swivels!
        /// </summary>
        public double PointTrackHorizontalFrequencyLevel
        {
            get
            {
                return pointTrackHorizontalFrequencyLevel;
            }
            set
            {
                if (value <= 0)
                    value = 3;
                pointTrackHorizontalFrequencyLevel = value;
            }
        }
        /// <summary>
        /// [PointTrack] The level of the vertical frequency. This is the denominator of the Pi value (3.1415926...) in mathematics, defined by <see cref="Math.PI"/>. Use this to create beautiful wavy swivels!
        /// </summary>
        public double PointTrackVerticalFrequencyLevel
        {
            get
            {
                return pointTrackVerticalFrequencyLevel;
            }
            set
            {
                if (value <= 0)
                    value = 8;
                pointTrackVerticalFrequencyLevel = value;
            }
        }
        /// <summary>
        /// [PointTrack] The minimum red color level (true color)
        /// </summary>
        public int PointTrackMinimumRedColorLevel
        {
            get
            {
                return pointTrackMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                pointTrackMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [PointTrack] The minimum green color level (true color)
        /// </summary>
        public int PointTrackMinimumGreenColorLevel
        {
            get
            {
                return pointTrackMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                pointTrackMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [PointTrack] The minimum blue color level (true color)
        /// </summary>
        public int PointTrackMinimumBlueColorLevel
        {
            get
            {
                return pointTrackMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                pointTrackMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [PointTrack] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int PointTrackMinimumColorLevel
        {
            get
            {
                return pointTrackMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                pointTrackMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [PointTrack] The maximum red color level (true color)
        /// </summary>
        public int PointTrackMaximumRedColorLevel
        {
            get
            {
                return pointTrackMaximumRedColorLevel;
            }
            set
            {
                if (value <= pointTrackMaximumRedColorLevel)
                    value = pointTrackMaximumRedColorLevel;
                if (value > 255)
                    value = 255;
                pointTrackMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [PointTrack] The maximum green color level (true color)
        /// </summary>
        public int PointTrackMaximumGreenColorLevel
        {
            get
            {
                return pointTrackMaximumGreenColorLevel;
            }
            set
            {
                if (value <= pointTrackMaximumGreenColorLevel)
                    value = pointTrackMaximumGreenColorLevel;
                if (value > 255)
                    value = 255;
                pointTrackMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [PointTrack] The maximum blue color level (true color)
        /// </summary>
        public int PointTrackMaximumBlueColorLevel
        {
            get
            {
                return pointTrackMaximumBlueColorLevel;
            }
            set
            {
                if (value <= pointTrackMaximumBlueColorLevel)
                    value = pointTrackMaximumBlueColorLevel;
                if (value > 255)
                    value = 255;
                pointTrackMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [PointTrack] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int PointTrackMaximumColorLevel
        {
            get
            {
                return pointTrackMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= pointTrackMaximumColorLevel)
                    value = pointTrackMaximumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                pointTrackMaximumColorLevel = value;
            }
        }
    }
}
