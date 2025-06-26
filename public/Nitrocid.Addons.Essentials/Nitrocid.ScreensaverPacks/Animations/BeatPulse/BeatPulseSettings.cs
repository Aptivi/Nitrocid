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

using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Animations.BeatPulse
{
    /// <summary>
    /// Beat pulse settings
    /// </summary>
    public class BeatPulseSettings
    {

        private bool _beatpulseTrueColor = true;
        private int _beatpulseDelay = 120;
        private int _beatpulseMaxSteps = 30;
        private bool _beatpulseCycleColors = true;
        private string _beatpulseBeatColor = "17";
        private int _beatpulseMinimumRedColorLevel = 0;
        private int _beatpulseMinimumGreenColorLevel = 0;
        private int _beatpulseMinimumBlueColorLevel = 0;
        private int _beatpulseMinimumColorLevel = 0;
        private int _beatpulseMaximumRedColorLevel = 255;
        private int _beatpulseMaximumGreenColorLevel = 255;
        private int _beatpulseMaximumBlueColorLevel = 255;
        private int _beatpulseMaximumColorLevel = 255;

        /// <summary>
        /// [BeatPulse] Enable truecolor support. Has a higher priority than 255 color support. Please note that it only works if color cycling is enabled.
        /// </summary>
        public bool BeatPulseTrueColor
        {
            get
            {
                return _beatpulseTrueColor;
            }
            set
            {
                _beatpulseTrueColor = value;
            }
        }
        /// <summary>
        /// [BeatPulse] Enable color cycling (uses RNG. If disabled, uses the <see cref="BeatPulseBeatColor"/> color.)
        /// </summary>
        public bool BeatPulseCycleColors
        {
            get
            {
                return _beatpulseCycleColors;
            }
            set
            {
                _beatpulseCycleColors = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The color of beats. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public string BeatPulseBeatColor
        {
            get
            {
                return _beatpulseBeatColor;
            }
            set
            {
                _beatpulseBeatColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [BeatPulse] How many beats per minute to wait before making the next write?
        /// </summary>
        public int BeatPulseDelay
        {
            get
            {
                return _beatpulseDelay;
            }
            set
            {
                if (value <= 0)
                    value = 120;
                _beatpulseDelay = value;
            }
        }
        /// <summary>
        /// [BeatPulse] How many fade steps to do?
        /// </summary>
        public int BeatPulseMaxSteps
        {
            get
            {
                return _beatpulseMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                _beatpulseMaxSteps = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The minimum red color level (true color)
        /// </summary>
        public int BeatPulseMinimumRedColorLevel
        {
            get
            {
                return _beatpulseMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _beatpulseMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The minimum green color level (true color)
        /// </summary>
        public int BeatPulseMinimumGreenColorLevel
        {
            get
            {
                return _beatpulseMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _beatpulseMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The minimum blue color level (true color)
        /// </summary>
        public int BeatPulseMinimumBlueColorLevel
        {
            get
            {
                return _beatpulseMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _beatpulseMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int BeatPulseMinimumColorLevel
        {
            get
            {
                return _beatpulseMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                _beatpulseMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The maximum red color level (true color)
        /// </summary>
        public int BeatPulseMaximumRedColorLevel
        {
            get
            {
                return _beatpulseMaximumRedColorLevel;
            }
            set
            {
                if (value <= _beatpulseMinimumRedColorLevel)
                    value = _beatpulseMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                _beatpulseMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The maximum green color level (true color)
        /// </summary>
        public int BeatPulseMaximumGreenColorLevel
        {
            get
            {
                return _beatpulseMaximumGreenColorLevel;
            }
            set
            {
                if (value <= _beatpulseMinimumGreenColorLevel)
                    value = _beatpulseMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                _beatpulseMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The maximum blue color level (true color)
        /// </summary>
        public int BeatPulseMaximumBlueColorLevel
        {
            get
            {
                return _beatpulseMaximumBlueColorLevel;
            }
            set
            {
                if (value <= _beatpulseMinimumBlueColorLevel)
                    value = _beatpulseMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                _beatpulseMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int BeatPulseMaximumColorLevel
        {
            get
            {
                return _beatpulseMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= _beatpulseMinimumColorLevel)
                    value = _beatpulseMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                _beatpulseMaximumColorLevel = value;
            }
        }

    }
}
