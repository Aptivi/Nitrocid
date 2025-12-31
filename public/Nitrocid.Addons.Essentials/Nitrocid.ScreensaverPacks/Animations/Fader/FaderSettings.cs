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

using Terminaux.Colors;
using Terminaux.Colors.Data;

namespace Nitrocid.ScreensaverPacks.Animations.Fader
{
    /// <summary>
    /// Fader settings
    /// </summary>
    public class FaderSettings
    {

        private int _faderDelay = 50;
        private int _faderFadeOutDelay = 3000;
        private string _faderWrite = "Nitrocid KS";
        private int _faderMaxSteps = 25;
        private string _faderBackgroundColor = new Color(ConsoleColors.Black).PlainSequence;
        private int _faderMinimumRedColorLevel = 0;
        private int _faderMinimumGreenColorLevel = 0;
        private int _faderMinimumBlueColorLevel = 0;
        private int _faderMaximumRedColorLevel = 255;
        private int _faderMaximumGreenColorLevel = 255;
        private int _faderMaximumBlueColorLevel = 255;

        /// <summary>
        /// [Fader] How many milliseconds to wait before making the next write?
        /// </summary>
        public int FaderDelay
        {
            get
            {
                return _faderDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                _faderDelay = value;
            }
        }
        /// <summary>
        /// [Fader] How many milliseconds to wait before fading the text out?
        /// </summary>
        public int FaderFadeOutDelay
        {
            get
            {
                return _faderFadeOutDelay;
            }
            set
            {
                if (value <= 0)
                    value = 3000;
                _faderFadeOutDelay = value;
            }
        }
        /// <summary>
        /// [Fader] Text for Fader. Shorter is better.
        /// </summary>
        public string FaderWrite
        {
            get
            {
                return _faderWrite;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Nitrocid KS";
                _faderWrite = value;
            }
        }
        /// <summary>
        /// [Fader] How many fade steps to do?
        /// </summary>
        public int FaderMaxSteps
        {
            get
            {
                return _faderMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                _faderMaxSteps = value;
            }
        }
        /// <summary>
        /// [Fader] Screensaver background color
        /// </summary>
        public string FaderBackgroundColor
        {
            get
            {
                return _faderBackgroundColor;
            }
            set
            {
                _faderBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [Fader] The minimum red color level (true color)
        /// </summary>
        public int FaderMinimumRedColorLevel
        {
            get
            {
                return _faderMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _faderMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Fader] The minimum green color level (true color)
        /// </summary>
        public int FaderMinimumGreenColorLevel
        {
            get
            {
                return _faderMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _faderMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Fader] The minimum blue color level (true color)
        /// </summary>
        public int FaderMinimumBlueColorLevel
        {
            get
            {
                return _faderMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _faderMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Fader] The maximum red color level (true color)
        /// </summary>
        public int FaderMaximumRedColorLevel
        {
            get
            {
                return _faderMaximumRedColorLevel;
            }
            set
            {
                if (value <= _faderMinimumRedColorLevel)
                    value = _faderMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                _faderMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Fader] The maximum green color level (true color)
        /// </summary>
        public int FaderMaximumGreenColorLevel
        {
            get
            {
                return _faderMaximumGreenColorLevel;
            }
            set
            {
                if (value <= _faderMinimumGreenColorLevel)
                    value = _faderMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                _faderMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Fader] The maximum blue color level (true color)
        /// </summary>
        public int FaderMaximumBlueColorLevel
        {
            get
            {
                return _faderMaximumBlueColorLevel;
            }
            set
            {
                if (value <= _faderMinimumBlueColorLevel)
                    value = _faderMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                _faderMaximumBlueColorLevel = value;
            }
        }

    }
}
