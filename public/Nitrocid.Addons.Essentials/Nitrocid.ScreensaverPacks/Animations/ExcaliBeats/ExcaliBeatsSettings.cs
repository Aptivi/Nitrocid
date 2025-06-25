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

using Nitrocid.Base.Kernel;
using System.Runtime.Versioning;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Animations.ExcaliBeats
{
    /// <summary>
    /// Beat fader settings
    /// </summary>
    public class ExcaliBeatsSettings
    {

        private bool _ExcaliBeatsTrueColor = true;
        private int _ExcaliBeatsDelay = 140;
        private int _ExcaliBeatsMaxSteps = 30;
        private bool _ExcaliBeatsCycleColors = true;
        private bool _ExcaliBeatsExplicit = true;
        private bool _ExcaliBeatsTranceMode = false;
        private string _ExcaliBeatsBeatColor = "17";
        private int _ExcaliBeatsMinimumRedColorLevel = 0;
        private int _ExcaliBeatsMinimumGreenColorLevel = 0;
        private int _ExcaliBeatsMinimumBlueColorLevel = 0;
        private int _ExcaliBeatsMinimumColorLevel = 0;
        private int _ExcaliBeatsMaximumRedColorLevel = 255;
        private int _ExcaliBeatsMaximumGreenColorLevel = 255;
        private int _ExcaliBeatsMaximumBlueColorLevel = 255;
        private int _ExcaliBeatsMaximumColorLevel = 255;

        /// <summary>
        /// [ExcaliBeats] Enable truecolor support. Has a higher priority than 255 color support. Please note that it only works if color cycling is enabled.
        /// </summary>
        public bool ExcaliBeatsTrueColor
        {
            get
            {
                return _ExcaliBeatsTrueColor;
            }
            set
            {
                _ExcaliBeatsTrueColor = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] Enable color cycling (uses RNG. If disabled, uses the <see cref="ExcaliBeatsBeatColor"/> color.)
        /// </summary>
        public bool ExcaliBeatsCycleColors
        {
            get
            {
                return _ExcaliBeatsCycleColors;
            }
            set
            {
                _ExcaliBeatsCycleColors = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] Explicitly change the text to Excalibur
        /// </summary>
        public bool ExcaliBeatsExplicit
        {
            get
            {
                return _ExcaliBeatsExplicit;
            }
            set
            {
                _ExcaliBeatsExplicit = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] [Linux only] Trance mode - Multiplies the BPM by 2 to simulate the trance music style
        /// </summary>
        public bool ExcaliBeatsTranceMode
        {
            get
            {
                return _ExcaliBeatsTranceMode;
            }
            [UnsupportedOSPlatform("windows")]
            set
            {
                if (KernelPlatform.IsOnUnix())
                    _ExcaliBeatsTranceMode = value;
                else
                    _ExcaliBeatsTranceMode = false;
            }
        }
        /// <summary>
        /// [ExcaliBeats] The color of beats. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public string ExcaliBeatsBeatColor
        {
            get
            {
                return _ExcaliBeatsBeatColor;
            }
            set
            {
                _ExcaliBeatsBeatColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [ExcaliBeats] How many beats per minute to wait before making the next write?
        /// </summary>
        public int ExcaliBeatsDelay
        {
            get
            {
                return _ExcaliBeatsDelay;
            }
            set
            {
                if (value <= 0)
                    value = 140;
                _ExcaliBeatsDelay = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] How many fade steps to do?
        /// </summary>
        public int ExcaliBeatsMaxSteps
        {
            get
            {
                return _ExcaliBeatsMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                _ExcaliBeatsMaxSteps = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] The minimum red color level (true color)
        /// </summary>
        public int ExcaliBeatsMinimumRedColorLevel
        {
            get
            {
                return _ExcaliBeatsMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _ExcaliBeatsMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] The minimum green color level (true color)
        /// </summary>
        public int ExcaliBeatsMinimumGreenColorLevel
        {
            get
            {
                return _ExcaliBeatsMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _ExcaliBeatsMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] The minimum blue color level (true color)
        /// </summary>
        public int ExcaliBeatsMinimumBlueColorLevel
        {
            get
            {
                return _ExcaliBeatsMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _ExcaliBeatsMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int ExcaliBeatsMinimumColorLevel
        {
            get
            {
                return _ExcaliBeatsMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                _ExcaliBeatsMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] The maximum red color level (true color)
        /// </summary>
        public int ExcaliBeatsMaximumRedColorLevel
        {
            get
            {
                return _ExcaliBeatsMaximumRedColorLevel;
            }
            set
            {
                if (value <= _ExcaliBeatsMinimumRedColorLevel)
                    value = _ExcaliBeatsMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                _ExcaliBeatsMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] The maximum green color level (true color)
        /// </summary>
        public int ExcaliBeatsMaximumGreenColorLevel
        {
            get
            {
                return _ExcaliBeatsMaximumGreenColorLevel;
            }
            set
            {
                if (value <= _ExcaliBeatsMinimumGreenColorLevel)
                    value = _ExcaliBeatsMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                _ExcaliBeatsMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] The maximum blue color level (true color)
        /// </summary>
        public int ExcaliBeatsMaximumBlueColorLevel
        {
            get
            {
                return _ExcaliBeatsMaximumBlueColorLevel;
            }
            set
            {
                if (value <= _ExcaliBeatsMinimumBlueColorLevel)
                    value = _ExcaliBeatsMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                _ExcaliBeatsMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int ExcaliBeatsMaximumColorLevel
        {
            get
            {
                return _ExcaliBeatsMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= _ExcaliBeatsMinimumColorLevel)
                    value = _ExcaliBeatsMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                _ExcaliBeatsMaximumColorLevel = value;
            }
        }

    }
}
