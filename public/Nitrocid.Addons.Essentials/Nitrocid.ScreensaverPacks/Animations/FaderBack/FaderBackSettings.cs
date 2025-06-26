//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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

namespace Nitrocid.ScreensaverPacks.Animations.FaderBack
{
    /// <summary>
    /// Background fader settings
    /// </summary>
    public class FaderBackSettings
    {

        private int _faderBackDelay = 50;
        private int _faderBackFadeOutDelay = 3000;
        private int _faderBackMaxSteps = 25;
        private int _faderBackMinimumRedColorLevel = 0;
        private int _faderBackMinimumGreenColorLevel = 0;
        private int _faderBackMinimumBlueColorLevel = 0;
        private int _faderBackMaximumRedColorLevel = 255;
        private int _faderBackMaximumGreenColorLevel = 255;
        private int _faderBackMaximumBlueColorLevel = 255;

        /// <summary>
        /// [FaderBack] How many milliseconds to wait before making the next write?
        /// </summary>
        public int FaderBackDelay
        {
            get
            {
                return _faderBackDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                _faderBackDelay = value;
            }
        }
        /// <summary>
        /// [FaderBack] How many milliseconds to wait before fading the text out?
        /// </summary>
        public int FaderBackFadeOutDelay
        {
            get
            {
                return _faderBackFadeOutDelay;
            }
            set
            {
                if (value <= 0)
                    value = 3000;
                _faderBackFadeOutDelay = value;
            }
        }
        /// <summary>
        /// [FaderBack] How many fade steps to do?
        /// </summary>
        public int FaderBackMaxSteps
        {
            get
            {
                return _faderBackMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                _faderBackMaxSteps = value;
            }
        }
        /// <summary>
        /// [FaderBack] The minimum red color level (true color)
        /// </summary>
        public int FaderBackMinimumRedColorLevel
        {
            get
            {
                return _faderBackMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _faderBackMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FaderBack] The minimum green color level (true color)
        /// </summary>
        public int FaderBackMinimumGreenColorLevel
        {
            get
            {
                return _faderBackMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _faderBackMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FaderBack] The minimum blue color level (true color)
        /// </summary>
        public int FaderBackMinimumBlueColorLevel
        {
            get
            {
                return _faderBackMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _faderBackMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [FaderBack] The maximum red color level (true color)
        /// </summary>
        public int FaderBackMaximumRedColorLevel
        {
            get
            {
                return _faderBackMaximumRedColorLevel;
            }
            set
            {
                if (value <= _faderBackMinimumRedColorLevel)
                    value = _faderBackMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                _faderBackMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FaderBack] The maximum green color level (true color)
        /// </summary>
        public int FaderBackMaximumGreenColorLevel
        {
            get
            {
                return _faderBackMaximumGreenColorLevel;
            }
            set
            {
                if (value <= _faderBackMinimumGreenColorLevel)
                    value = _faderBackMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                _faderBackMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FaderBack] The maximum blue color level (true color)
        /// </summary>
        public int FaderBackMaximumBlueColorLevel
        {
            get
            {
                return _faderBackMaximumBlueColorLevel;
            }
            set
            {
                if (value <= _faderBackMinimumBlueColorLevel)
                    value = _faderBackMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                _faderBackMaximumBlueColorLevel = value;
            }
        }

    }
}
