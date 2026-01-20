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
        private int pulseDelay = 50;
        private int pulseMaxSteps = 25;
        private int pulseMinimumRedColorLevel = 0;
        private int pulseMinimumGreenColorLevel = 0;
        private int pulseMinimumBlueColorLevel = 0;
        private int pulseMaximumRedColorLevel = 255;
        private int pulseMaximumGreenColorLevel = 255;
        private int pulseMaximumBlueColorLevel = 255;

        /// <summary>
        /// [Pulse] How many milliseconds to wait before making the next write?
        /// </summary>
        public int PulseDelay
        {
            get
            {
                return pulseDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                pulseDelay = value;
            }
        }
        /// <summary>
        /// [Pulse] How many fade steps to do?
        /// </summary>
        public int PulseMaxSteps
        {
            get
            {
                return pulseMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                pulseMaxSteps = value;
            }
        }
        /// <summary>
        /// [Pulse] The minimum red color level (true color)
        /// </summary>
        public int PulseMinimumRedColorLevel
        {
            get
            {
                return pulseMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                pulseMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Pulse] The minimum green color level (true color)
        /// </summary>
        public int PulseMinimumGreenColorLevel
        {
            get
            {
                return pulseMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                pulseMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Pulse] The minimum blue color level (true color)
        /// </summary>
        public int PulseMinimumBlueColorLevel
        {
            get
            {
                return pulseMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                pulseMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Pulse] The maximum red color level (true color)
        /// </summary>
        public int PulseMaximumRedColorLevel
        {
            get
            {
                return pulseMaximumRedColorLevel;
            }
            set
            {
                if (value <= pulseMinimumRedColorLevel)
                    value = pulseMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                pulseMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Pulse] The maximum green color level (true color)
        /// </summary>
        public int PulseMaximumGreenColorLevel
        {
            get
            {
                return pulseMaximumGreenColorLevel;
            }
            set
            {
                if (value <= pulseMinimumGreenColorLevel)
                    value = pulseMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                pulseMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Pulse] The maximum blue color level (true color)
        /// </summary>
        public int PulseMaximumBlueColorLevel
        {
            get
            {
                return pulseMaximumBlueColorLevel;
            }
            set
            {
                if (value <= pulseMinimumBlueColorLevel)
                    value = pulseMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                pulseMaximumBlueColorLevel = value;
            }
        }
    }
}
