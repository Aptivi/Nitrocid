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
        private bool fallingLineTrueColor = true;
        private int fallingLineDelay = 10;
        private int fallingLineMaxSteps = 25;
        private int fallingLineMinimumRedColorLevel = 0;
        private int fallingLineMinimumGreenColorLevel = 0;
        private int fallingLineMinimumBlueColorLevel = 0;
        private int fallingLineMinimumColorLevel = 0;
        private int fallingLineMaximumRedColorLevel = 255;
        private int fallingLineMaximumGreenColorLevel = 255;
        private int fallingLineMaximumBlueColorLevel = 255;
        private int fallingLineMaximumColorLevel = 255;

        /// <summary>
        /// [FallingLine] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool FallingLineTrueColor
        {
            get
            {
                return fallingLineTrueColor;
            }
            set
            {
                fallingLineTrueColor = value;
            }
        }
        /// <summary>
        /// [FallingLine] How many milliseconds to wait before making the next write?
        /// </summary>
        public int FallingLineDelay
        {
            get
            {
                return fallingLineDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                fallingLineDelay = value;
            }
        }
        /// <summary>
        /// [FallingLine] How many fade steps to do?
        /// </summary>
        public int FallingLineMaxSteps
        {
            get
            {
                return fallingLineMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                fallingLineMaxSteps = value;
            }
        }
        /// <summary>
        /// [FallingLine] The minimum red color level (true color)
        /// </summary>
        public int FallingLineMinimumRedColorLevel
        {
            get
            {
                return fallingLineMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                fallingLineMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FallingLine] The minimum green color level (true color)
        /// </summary>
        public int FallingLineMinimumGreenColorLevel
        {
            get
            {
                return fallingLineMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                fallingLineMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FallingLine] The minimum blue color level (true color)
        /// </summary>
        public int FallingLineMinimumBlueColorLevel
        {
            get
            {
                return fallingLineMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                fallingLineMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [FallingLine] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int FallingLineMinimumColorLevel
        {
            get
            {
                return fallingLineMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                fallingLineMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [FallingLine] The maximum red color level (true color)
        /// </summary>
        public int FallingLineMaximumRedColorLevel
        {
            get
            {
                return fallingLineMaximumRedColorLevel;
            }
            set
            {
                if (value <= fallingLineMinimumRedColorLevel)
                    value = fallingLineMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                fallingLineMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FallingLine] The maximum green color level (true color)
        /// </summary>
        public int FallingLineMaximumGreenColorLevel
        {
            get
            {
                return fallingLineMaximumGreenColorLevel;
            }
            set
            {
                if (value <= fallingLineMinimumGreenColorLevel)
                    value = fallingLineMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                fallingLineMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FallingLine] The maximum blue color level (true color)
        /// </summary>
        public int FallingLineMaximumBlueColorLevel
        {
            get
            {
                return fallingLineMaximumBlueColorLevel;
            }
            set
            {
                if (value <= fallingLineMinimumBlueColorLevel)
                    value = fallingLineMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                fallingLineMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [FallingLine] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int FallingLineMaximumColorLevel
        {
            get
            {
                return fallingLineMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= fallingLineMinimumColorLevel)
                    value = fallingLineMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                fallingLineMaximumColorLevel = value;
            }
        }
    }
}
