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

namespace Nitrocid.Extras.Amusements.Settings
{
    /// <summary>
    /// Screensaver kernel configuration instance
    /// </summary>
    public partial class AmusementsSaversConfig : BaseKernelConfig
    {
        private bool snakerTrueColor = true;
        private int snakerDelay = 100;
        private int snakerStageDelay = 5000;
        private int snakerMinimumRedColorLevel = 0;
        private int snakerMinimumGreenColorLevel = 0;
        private int snakerMinimumBlueColorLevel = 0;
        private int snakerMinimumColorLevel = 0;
        private int snakerMaximumRedColorLevel = 255;
        private int snakerMaximumGreenColorLevel = 255;
        private int snakerMaximumBlueColorLevel = 255;
        private int snakerMaximumColorLevel = 255;

        /// <summary>
        /// [Snaker] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool SnakerTrueColor
        {
            get
            {
                return snakerTrueColor;
            }
            set
            {
                snakerTrueColor = value;
            }
        }
        /// <summary>
        /// [Snaker] How many milliseconds to wait before making the next write?
        /// </summary>
        public int SnakerDelay
        {
            get
            {
                return snakerDelay;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                snakerDelay = value;
            }
        }
        /// <summary>
        /// [Snaker] How many milliseconds to wait before making the next stage?
        /// </summary>
        public int SnakerStageDelay
        {
            get
            {
                return snakerStageDelay;
            }
            set
            {
                if (value <= 0)
                    value = 5000;
                snakerStageDelay = value;
            }
        }
        /// <summary>
        /// [Snaker] The minimum red color level (true color)
        /// </summary>
        public int SnakerMinimumRedColorLevel
        {
            get
            {
                return snakerMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                snakerMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Snaker] The minimum green color level (true color)
        /// </summary>
        public int SnakerMinimumGreenColorLevel
        {
            get
            {
                return snakerMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                snakerMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Snaker] The minimum blue color level (true color)
        /// </summary>
        public int SnakerMinimumBlueColorLevel
        {
            get
            {
                return snakerMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                snakerMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Snaker] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int SnakerMinimumColorLevel
        {
            get
            {
                return snakerMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                snakerMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Snaker] The maximum red color level (true color)
        /// </summary>
        public int SnakerMaximumRedColorLevel
        {
            get
            {
                return snakerMaximumRedColorLevel;
            }
            set
            {
                if (value <= snakerMinimumRedColorLevel)
                    value = snakerMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                snakerMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Snaker] The maximum green color level (true color)
        /// </summary>
        public int SnakerMaximumGreenColorLevel
        {
            get
            {
                return snakerMaximumGreenColorLevel;
            }
            set
            {
                if (value <= snakerMinimumGreenColorLevel)
                    value = snakerMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                snakerMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Snaker] The maximum blue color level (true color)
        /// </summary>
        public int SnakerMaximumBlueColorLevel
        {
            get
            {
                return snakerMaximumBlueColorLevel;
            }
            set
            {
                if (value <= snakerMinimumBlueColorLevel)
                    value = snakerMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                snakerMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Snaker] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int SnakerMaximumColorLevel
        {
            get
            {
                return snakerMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= snakerMinimumColorLevel)
                    value = snakerMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                snakerMaximumColorLevel = value;
            }
        }
    }
}
