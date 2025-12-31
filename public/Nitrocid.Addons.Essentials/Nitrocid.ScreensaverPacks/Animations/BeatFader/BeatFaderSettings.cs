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

namespace Nitrocid.ScreensaverPacks.Animations.BeatFader
{
    /// <summary>
    /// Beat fader settings
    /// </summary>
    public class BeatFaderSettings
    {

        private bool _beatFaderTrueColor = true;
        private int _beatFaderDelay = 120;
        private int _beatFaderMaxSteps = 30;
        private bool _beatFaderCycleColors = true;
        private string _beatFaderBeatColor = "17";
        private int _beatFaderMinimumRedColorLevel = 0;
        private int _beatFaderMinimumGreenColorLevel = 0;
        private int _beatFaderMinimumBlueColorLevel = 0;
        private int _beatFaderMinimumColorLevel = 0;
        private int _beatFaderMaximumRedColorLevel = 255;
        private int _beatFaderMaximumGreenColorLevel = 255;
        private int _beatFaderMaximumBlueColorLevel = 255;
        private int _beatFaderMaximumColorLevel = 255;

        /// <summary>
        /// [BeatFader] Enable truecolor support. Has a higher priority than 255 color support. Please note that it only works if color cycling is enabled.
        /// </summary>
        public bool BeatFaderTrueColor
        {
            get
            {
                return _beatFaderTrueColor;
            }
            set
            {
                _beatFaderTrueColor = value;
            }
        }
        /// <summary>
        /// [BeatFader] Enable color cycling (uses RNG. If disabled, uses the <see cref="BeatFaderBeatColor"/> color.)
        /// </summary>
        public bool BeatFaderCycleColors
        {
            get
            {
                return _beatFaderCycleColors;
            }
            set
            {
                _beatFaderCycleColors = value;
            }
        }
        /// <summary>
        /// [BeatFader] The color of beats. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public string BeatFaderBeatColor
        {
            get
            {
                return _beatFaderBeatColor;
            }
            set
            {
                _beatFaderBeatColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [BeatFader] How many beats per minute to wait before making the next write?
        /// </summary>
        public int BeatFaderDelay
        {
            get
            {
                return _beatFaderDelay;
            }
            set
            {
                if (value <= 0)
                    value = 120;
                _beatFaderDelay = value;
            }
        }
        /// <summary>
        /// [BeatFader] How many fade steps to do?
        /// </summary>
        public int BeatFaderMaxSteps
        {
            get
            {
                return _beatFaderMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                _beatFaderMaxSteps = value;
            }
        }
        /// <summary>
        /// [BeatFader] The minimum red color level (true color)
        /// </summary>
        public int BeatFaderMinimumRedColorLevel
        {
            get
            {
                return _beatFaderMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _beatFaderMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatFader] The minimum green color level (true color)
        /// </summary>
        public int BeatFaderMinimumGreenColorLevel
        {
            get
            {
                return _beatFaderMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _beatFaderMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatFader] The minimum blue color level (true color)
        /// </summary>
        public int BeatFaderMinimumBlueColorLevel
        {
            get
            {
                return _beatFaderMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _beatFaderMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatFader] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int BeatFaderMinimumColorLevel
        {
            get
            {
                return _beatFaderMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                _beatFaderMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatFader] The maximum red color level (true color)
        /// </summary>
        public int BeatFaderMaximumRedColorLevel
        {
            get
            {
                return _beatFaderMaximumRedColorLevel;
            }
            set
            {
                if (value <= _beatFaderMinimumRedColorLevel)
                    value = _beatFaderMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                _beatFaderMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatFader] The maximum green color level (true color)
        /// </summary>
        public int BeatFaderMaximumGreenColorLevel
        {
            get
            {
                return _beatFaderMaximumGreenColorLevel;
            }
            set
            {
                if (value <= _beatFaderMinimumGreenColorLevel)
                    value = _beatFaderMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                _beatFaderMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatFader] The maximum blue color level (true color)
        /// </summary>
        public int BeatFaderMaximumBlueColorLevel
        {
            get
            {
                return _beatFaderMaximumBlueColorLevel;
            }
            set
            {
                if (value <= _beatFaderMinimumBlueColorLevel)
                    value = _beatFaderMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                _beatFaderMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatFader] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int BeatFaderMaximumColorLevel
        {
            get
            {
                return _beatFaderMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= _beatFaderMinimumColorLevel)
                    value = _beatFaderMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                _beatFaderMaximumColorLevel = value;
            }
        }

    }
}
