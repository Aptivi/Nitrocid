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
        private bool piTrueColor = true;
        private int piDelay = 20;
        private int piMinimumRedColorLevel = 0;
        private int piMinimumGreenColorLevel = 0;
        private int piMinimumBlueColorLevel = 0;
        private int piMinimumColorLevel = 0;
        private int piMaximumRedColorLevel = 255;
        private int piMaximumGreenColorLevel = 255;
        private int piMaximumBlueColorLevel = 255;
        private int piMaximumColorLevel = 255;

        /// <summary>
        /// [Pi] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool PiTrueColor
        {
            get
            {
                return piTrueColor;
            }
            set
            {
                piTrueColor = value;
            }
        }
        /// <summary>
        /// [Pi] How many milliseconds to wait before making the next write?
        /// </summary>
        public int PiDelay
        {
            get
            {
                return piDelay;
            }
            set
            {
                if (value <= 0)
                    value = 20;
                piDelay = value;
            }
        }
        /// <summary>
        /// [Pi] The minimum red color level (true color)
        /// </summary>
        public int PiMinimumRedColorLevel
        {
            get
            {
                return piMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                piMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Pi] The minimum green color level (true color)
        /// </summary>
        public int PiMinimumGreenColorLevel
        {
            get
            {
                return piMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                piMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Pi] The minimum blue color level (true color)
        /// </summary>
        public int PiMinimumBlueColorLevel
        {
            get
            {
                return piMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                piMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Pi] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int PiMinimumColorLevel
        {
            get
            {
                return piMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                piMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Pi] The maximum red color level (true color)
        /// </summary>
        public int PiMaximumRedColorLevel
        {
            get
            {
                return piMaximumRedColorLevel;
            }
            set
            {
                if (value <= piMinimumRedColorLevel)
                    value = piMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                piMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Pi] The maximum green color level (true color)
        /// </summary>
        public int PiMaximumGreenColorLevel
        {
            get
            {
                return piMaximumGreenColorLevel;
            }
            set
            {
                if (value <= piMinimumGreenColorLevel)
                    value = piMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                piMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Pi] The maximum blue color level (true color)
        /// </summary>
        public int PiMaximumBlueColorLevel
        {
            get
            {
                return piMaximumBlueColorLevel;
            }
            set
            {
                if (value <= piMinimumBlueColorLevel)
                    value = piMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                piMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Pi] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int PiMaximumColorLevel
        {
            get
            {
                return piMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= piMinimumColorLevel)
                    value = piMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                piMaximumColorLevel = value;
            }
        }
    }
}
