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

namespace Nitrocid.ScreensaverPacks.Animations.TextReveal
{
    /// <summary>
    /// TextReveal settings
    /// </summary>
    public class TextRevealSettings
    {

        private int _textRevealDelay = 50;
        private int _textRevealFadeOutDelay = 3000;
        private int _textRevealNewScreenDelay = 10000;
        private string _textRevealWrite = "Nitrocid KS";
        private int _textRevealMaxSteps = 25;
        private int _textRevealMinimumRedColorLevel = 0;
        private int _textRevealMinimumGreenColorLevel = 0;
        private int _textRevealMinimumBlueColorLevel = 0;
        private int _textRevealMaximumRedColorLevel = 255;
        private int _textRevealMaximumGreenColorLevel = 255;
        private int _textRevealMaximumBlueColorLevel = 255;

        /// <summary>
        /// [TextReveal] How many milliseconds to wait before making the next write?
        /// </summary>
        public int TextRevealDelay
        {
            get
            {
                return _textRevealDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                _textRevealDelay = value;
            }
        }
        /// <summary>
        /// [TextReveal] How many milliseconds to wait before fading the text out?
        /// </summary>
        public int TextRevealFadeOutDelay
        {
            get
            {
                return _textRevealFadeOutDelay;
            }
            set
            {
                if (value <= 0)
                    value = 3000;
                _textRevealFadeOutDelay = value;
            }
        }
        /// <summary>
        /// [TextReveal] How many milliseconds to wait before writing in the new screen?
        /// </summary>
        public int TextRevealNewScreenDelay
        {
            get
            {
                return _textRevealNewScreenDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10000;
                _textRevealNewScreenDelay = value;
            }
        }
        /// <summary>
        /// [TextReveal] Text for TextReveal. Shorter is better.
        /// </summary>
        public string TextRevealWrite
        {
            get
            {
                return _textRevealWrite;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Nitrocid KS";
                _textRevealWrite = value;
            }
        }
        /// <summary>
        /// [TextReveal] How many fade steps to do?
        /// </summary>
        public int TextRevealMaxSteps
        {
            get
            {
                return _textRevealMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                _textRevealMaxSteps = value;
            }
        }
        /// <summary>
        /// [TextReveal] The minimum red color level (true color)
        /// </summary>
        public int TextRevealMinimumRedColorLevel
        {
            get
            {
                return _textRevealMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _textRevealMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [TextReveal] The minimum green color level (true color)
        /// </summary>
        public int TextRevealMinimumGreenColorLevel
        {
            get
            {
                return _textRevealMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _textRevealMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [TextReveal] The minimum blue color level (true color)
        /// </summary>
        public int TextRevealMinimumBlueColorLevel
        {
            get
            {
                return _textRevealMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _textRevealMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [TextReveal] The maximum red color level (true color)
        /// </summary>
        public int TextRevealMaximumRedColorLevel
        {
            get
            {
                return _textRevealMaximumRedColorLevel;
            }
            set
            {
                if (value <= _textRevealMinimumRedColorLevel)
                    value = _textRevealMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                _textRevealMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [TextReveal] The maximum green color level (true color)
        /// </summary>
        public int TextRevealMaximumGreenColorLevel
        {
            get
            {
                return _textRevealMaximumGreenColorLevel;
            }
            set
            {
                if (value <= _textRevealMinimumGreenColorLevel)
                    value = _textRevealMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                _textRevealMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [TextReveal] The maximum blue color level (true color)
        /// </summary>
        public int TextRevealMaximumBlueColorLevel
        {
            get
            {
                return _textRevealMaximumBlueColorLevel;
            }
            set
            {
                if (value <= _textRevealMinimumBlueColorLevel)
                    value = _textRevealMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                _textRevealMaximumBlueColorLevel = value;
            }
        }

    }
}
