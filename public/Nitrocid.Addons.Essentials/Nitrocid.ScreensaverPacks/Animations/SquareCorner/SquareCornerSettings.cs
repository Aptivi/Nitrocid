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

namespace Nitrocid.ScreensaverPacks.Animations.SquareCorner
{
    /// <summary>
    /// SquareCorner settings
    /// </summary>
    public class SquareCornerSettings
    {

        private int _squareCornerDelay = 10;
        private int _squareCornerFadeOutDelay = 3000;
        private int _squareCornerMaxSteps = 25;
        private int _squareCornerMinimumRedColorLevel = 0;
        private int _squareCornerMinimumGreenColorLevel = 0;
        private int _squareCornerMinimumBlueColorLevel = 0;
        private int _squareCornerMaximumRedColorLevel = 255;
        private int _squareCornerMaximumGreenColorLevel = 255;
        private int _squareCornerMaximumBlueColorLevel = 255;

        /// <summary>
        /// [SquareCorner] How many milliseconds to wait before making the next write?
        /// </summary>
        public int SquareCornerDelay
        {
            get
            {
                return _squareCornerDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                _squareCornerDelay = value;
            }
        }
        /// <summary>
        /// [SquareCorner] How many milliseconds to wait before fading the square out?
        /// </summary>
        public int SquareCornerFadeOutDelay
        {
            get
            {
                return _squareCornerFadeOutDelay;
            }
            set
            {
                if (value <= 0)
                    value = 3000;
                _squareCornerFadeOutDelay = value;
            }
        }
        /// <summary>
        /// [SquareCorner] How many fade steps to do?
        /// </summary>
        public int SquareCornerMaxSteps
        {
            get
            {
                return _squareCornerMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                _squareCornerMaxSteps = value;
            }
        }
        /// <summary>
        /// [SquareCorner] The minimum red color level (true color)
        /// </summary>
        public int SquareCornerMinimumRedColorLevel
        {
            get
            {
                return _squareCornerMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _squareCornerMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [SquareCorner] The minimum green color level (true color)
        /// </summary>
        public int SquareCornerMinimumGreenColorLevel
        {
            get
            {
                return _squareCornerMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _squareCornerMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [SquareCorner] The minimum blue color level (true color)
        /// </summary>
        public int SquareCornerMinimumBlueColorLevel
        {
            get
            {
                return _squareCornerMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _squareCornerMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [SquareCorner] The maximum red color level (true color)
        /// </summary>
        public int SquareCornerMaximumRedColorLevel
        {
            get
            {
                return _squareCornerMaximumRedColorLevel;
            }
            set
            {
                if (value <= _squareCornerMinimumRedColorLevel)
                    value = _squareCornerMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                _squareCornerMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [SquareCorner] The maximum green color level (true color)
        /// </summary>
        public int SquareCornerMaximumGreenColorLevel
        {
            get
            {
                return _squareCornerMaximumGreenColorLevel;
            }
            set
            {
                if (value <= _squareCornerMinimumGreenColorLevel)
                    value = _squareCornerMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                _squareCornerMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [SquareCorner] The maximum blue color level (true color)
        /// </summary>
        public int SquareCornerMaximumBlueColorLevel
        {
            get
            {
                return _squareCornerMaximumBlueColorLevel;
            }
            set
            {
                if (value <= _squareCornerMinimumBlueColorLevel)
                    value = _squareCornerMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                _squareCornerMaximumBlueColorLevel = value;
            }
        }

    }
}
