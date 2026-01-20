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
        private int squareCornerDelay = 10;
        private int squareCornerFadeOutDelay = 3000;
        private int squareCornerMaxSteps = 25;
        private int squareCornerMinimumRedColorLevel = 0;
        private int squareCornerMinimumGreenColorLevel = 0;
        private int squareCornerMinimumBlueColorLevel = 0;
        private int squareCornerMaximumRedColorLevel = 255;
        private int squareCornerMaximumGreenColorLevel = 255;
        private int squareCornerMaximumBlueColorLevel = 255;

        /// <summary>
        /// [SquareCorner] How many milliseconds to wait before making the next write?
        /// </summary>
        public int SquareCornerDelay
        {
            get
            {
                return squareCornerDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                squareCornerDelay = value;
            }
        }
        /// <summary>
        /// [SquareCorner] How many milliseconds to wait before fading the square out?
        /// </summary>
        public int SquareCornerFadeOutDelay
        {
            get
            {
                return squareCornerFadeOutDelay;
            }
            set
            {
                if (value <= 0)
                    value = 3000;
                squareCornerFadeOutDelay = value;
            }
        }
        /// <summary>
        /// [SquareCorner] How many fade steps to do?
        /// </summary>
        public int SquareCornerMaxSteps
        {
            get
            {
                return squareCornerMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                squareCornerMaxSteps = value;
            }
        }
        /// <summary>
        /// [SquareCorner] The minimum red color level (true color)
        /// </summary>
        public int SquareCornerMinimumRedColorLevel
        {
            get
            {
                return squareCornerMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                squareCornerMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [SquareCorner] The minimum green color level (true color)
        /// </summary>
        public int SquareCornerMinimumGreenColorLevel
        {
            get
            {
                return squareCornerMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                squareCornerMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [SquareCorner] The minimum blue color level (true color)
        /// </summary>
        public int SquareCornerMinimumBlueColorLevel
        {
            get
            {
                return squareCornerMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                squareCornerMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [SquareCorner] The maximum red color level (true color)
        /// </summary>
        public int SquareCornerMaximumRedColorLevel
        {
            get
            {
                return squareCornerMaximumRedColorLevel;
            }
            set
            {
                if (value <= squareCornerMinimumRedColorLevel)
                    value = squareCornerMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                squareCornerMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [SquareCorner] The maximum green color level (true color)
        /// </summary>
        public int SquareCornerMaximumGreenColorLevel
        {
            get
            {
                return squareCornerMaximumGreenColorLevel;
            }
            set
            {
                if (value <= squareCornerMinimumGreenColorLevel)
                    value = squareCornerMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                squareCornerMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [SquareCorner] The maximum blue color level (true color)
        /// </summary>
        public int SquareCornerMaximumBlueColorLevel
        {
            get
            {
                return squareCornerMaximumBlueColorLevel;
            }
            set
            {
                if (value <= squareCornerMinimumBlueColorLevel)
                    value = squareCornerMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                squareCornerMaximumBlueColorLevel = value;
            }
        }
    }
}
