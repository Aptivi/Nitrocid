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

namespace Nitrocid.ScreensaverPacks.Animations.BeatEdgePulse
{
    /// <summary>
    /// Beat edge pulse settings
    /// </summary>
    public class BeatEdgePulseSettings
    {

        private bool _beatedgepulseTrueColor = true;
        private int _beatedgepulseDelay = 120;
        private int _beatedgepulseMaxSteps = 30;
        private bool _beatedgepulseCycleColors = true;
        private string _beatedgepulseBeatColor = "17";
        private int _beatedgepulseMinimumRedColorLevel = 0;
        private int _beatedgepulseMinimumGreenColorLevel = 0;
        private int _beatedgepulseMinimumBlueColorLevel = 0;
        private int _beatedgepulseMinimumColorLevel = 0;
        private int _beatedgepulseMaximumRedColorLevel = 255;
        private int _beatedgepulseMaximumGreenColorLevel = 255;
        private int _beatedgepulseMaximumBlueColorLevel = 255;
        private int _beatedgepulseMaximumColorLevel = 255;

        /// <summary>
        /// [BeatEdgePulse] Enable truecolor support. Has a higher priority than 255 color support. Please note that it only works if color cycling is enabled.
        /// </summary>
        public bool BeatEdgePulseTrueColor
        {
            get
            {
                return _beatedgepulseTrueColor;
            }
            set
            {
                _beatedgepulseTrueColor = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] Enable color cycling (uses RNG. If disabled, uses the <see cref="BeatEdgePulseBeatColor"/> color.)
        /// </summary>
        public bool BeatEdgePulseCycleColors
        {
            get
            {
                return _beatedgepulseCycleColors;
            }
            set
            {
                _beatedgepulseCycleColors = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] The color of beats. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public string BeatEdgePulseBeatColor
        {
            get
            {
                return _beatedgepulseBeatColor;
            }
            set
            {
                _beatedgepulseBeatColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] How many beats per minute to wait before making the next write?
        /// </summary>
        public int BeatEdgePulseDelay
        {
            get
            {
                return _beatedgepulseDelay;
            }
            set
            {
                if (value <= 0)
                    value = 120;
                _beatedgepulseDelay = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] How many fade steps to do?
        /// </summary>
        public int BeatEdgePulseMaxSteps
        {
            get
            {
                return _beatedgepulseMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                _beatedgepulseMaxSteps = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] The minimum red color level (true color)
        /// </summary>
        public int BeatEdgePulseMinimumRedColorLevel
        {
            get
            {
                return _beatedgepulseMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _beatedgepulseMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] The minimum green color level (true color)
        /// </summary>
        public int BeatEdgePulseMinimumGreenColorLevel
        {
            get
            {
                return _beatedgepulseMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _beatedgepulseMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] The minimum blue color level (true color)
        /// </summary>
        public int BeatEdgePulseMinimumBlueColorLevel
        {
            get
            {
                return _beatedgepulseMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _beatedgepulseMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int BeatEdgePulseMinimumColorLevel
        {
            get
            {
                return _beatedgepulseMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                _beatedgepulseMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] The maximum red color level (true color)
        /// </summary>
        public int BeatEdgePulseMaximumRedColorLevel
        {
            get
            {
                return _beatedgepulseMaximumRedColorLevel;
            }
            set
            {
                if (value <= _beatedgepulseMinimumRedColorLevel)
                    value = _beatedgepulseMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                _beatedgepulseMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] The maximum green color level (true color)
        /// </summary>
        public int BeatEdgePulseMaximumGreenColorLevel
        {
            get
            {
                return _beatedgepulseMaximumGreenColorLevel;
            }
            set
            {
                if (value <= _beatedgepulseMinimumGreenColorLevel)
                    value = _beatedgepulseMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                _beatedgepulseMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] The maximum blue color level (true color)
        /// </summary>
        public int BeatEdgePulseMaximumBlueColorLevel
        {
            get
            {
                return _beatedgepulseMaximumBlueColorLevel;
            }
            set
            {
                if (value <= _beatedgepulseMinimumBlueColorLevel)
                    value = _beatedgepulseMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                _beatedgepulseMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int BeatEdgePulseMaximumColorLevel
        {
            get
            {
                return _beatedgepulseMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= _beatedgepulseMinimumColorLevel)
                    value = _beatedgepulseMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                _beatedgepulseMaximumColorLevel = value;
            }
        }

    }
}
