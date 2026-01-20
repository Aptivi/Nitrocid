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
        private int edgePulseDelay = 50;
        private int edgePulseMaxSteps = 25;
        private int edgePulseMinimumRedColorLevel = 0;
        private int edgePulseMinimumGreenColorLevel = 0;
        private int edgePulseMinimumBlueColorLevel = 0;
        private int edgePulseMaximumRedColorLevel = 255;
        private int edgePulseMaximumGreenColorLevel = 255;
        private int edgePulseMaximumBlueColorLevel = 255;

        /// <summary>
        /// [EdgePulse] How many milliseconds to wait before making the next write?
        /// </summary>
        public int EdgePulseDelay
        {
            get
            {
                return edgePulseDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                edgePulseDelay = value;
            }
        }
        /// <summary>
        /// [EdgePulse] How many fade steps to do?
        /// </summary>
        public int EdgePulseMaxSteps
        {
            get
            {
                return edgePulseMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                edgePulseMaxSteps = value;
            }
        }
        /// <summary>
        /// [EdgePulse] The minimum red color level (true color)
        /// </summary>
        public int EdgePulseMinimumRedColorLevel
        {
            get
            {
                return edgePulseMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                edgePulseMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [EdgePulse] The minimum green color level (true color)
        /// </summary>
        public int EdgePulseMinimumGreenColorLevel
        {
            get
            {
                return edgePulseMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                edgePulseMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [EdgePulse] The minimum blue color level (true color)
        /// </summary>
        public int EdgePulseMinimumBlueColorLevel
        {
            get
            {
                return edgePulseMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                edgePulseMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [EdgePulse] The maximum red color level (true color)
        /// </summary>
        public int EdgePulseMaximumRedColorLevel
        {
            get
            {
                return edgePulseMaximumRedColorLevel;
            }
            set
            {
                if (value <= edgePulseMinimumRedColorLevel)
                    value = edgePulseMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                edgePulseMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [EdgePulse] The maximum green color level (true color)
        /// </summary>
        public int EdgePulseMaximumGreenColorLevel
        {
            get
            {
                return edgePulseMaximumGreenColorLevel;
            }
            set
            {
                if (value <= edgePulseMinimumGreenColorLevel)
                    value = edgePulseMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                edgePulseMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [EdgePulse] The maximum blue color level (true color)
        /// </summary>
        public int EdgePulseMaximumBlueColorLevel
        {
            get
            {
                return edgePulseMaximumBlueColorLevel;
            }
            set
            {
                if (value <= edgePulseMinimumBlueColorLevel)
                    value = edgePulseMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                edgePulseMaximumBlueColorLevel = value;
            }
        }
    }
}
