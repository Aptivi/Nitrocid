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
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Settings
{
    /// <summary>
    /// Screensaver kernel configuration instance
    /// </summary>
    public partial class ExtraSaversConfig : BaseKernelConfig
    {
        private bool beatPulseTrueColor = true;
        private int beatPulseDelay = 50;
        private bool beatPulseCycleColors = true;
        private string beatPulseBeatColor = "17";
        private int beatPulseMaxSteps = 25;
        private int beatPulseMinimumRedColorLevel = 0;
        private int beatPulseMinimumGreenColorLevel = 0;
        private int beatPulseMinimumBlueColorLevel = 0;
        private int beatPulseMinimumColorLevel = 0;
        private int beatPulseMaximumRedColorLevel = 255;
        private int beatPulseMaximumGreenColorLevel = 255;
        private int beatPulseMaximumBlueColorLevel = 255;
        private int beatPulseMaximumColorLevel = 255;

        /// <summary>
        /// [BeatPulse] Enable truecolor support. Has a higher priority than 255 color support. Please note that it only works if color cycling is enabled.
        /// </summary>
        public bool BeatPulseTrueColor
        {
            get
            {
                return beatPulseTrueColor;
            }
            set
            {
                beatPulseTrueColor = value;
            }
        }
        /// <summary>
        /// [BeatPulse] Enable color cycling (uses RNG. If disabled, uses the <see cref="BeatPulseBeatColor"/> color.)
        /// </summary>
        public bool BeatPulseCycleColors
        {
            get
            {
                return beatPulseCycleColors;
            }
            set
            {
                beatPulseCycleColors = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The color of beats. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public string BeatPulseBeatColor
        {
            get
            {
                return beatPulseBeatColor;
            }
            set
            {
                beatPulseBeatColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [BeatPulse] How many milliseconds to wait before making the next write?
        /// </summary>
        public int BeatPulseDelay
        {
            get
            {
                return beatPulseDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                beatPulseDelay = value;
            }
        }
        /// <summary>
        /// [BeatPulse] How many fade steps to do?
        /// </summary>
        public int BeatPulseMaxSteps
        {
            get
            {
                return beatPulseMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                beatPulseMaxSteps = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The minimum red color level (true color)
        /// </summary>
        public int BeatPulseMinimumRedColorLevel
        {
            get
            {
                return beatPulseMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                beatPulseMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The minimum green color level (true color)
        /// </summary>
        public int BeatPulseMinimumGreenColorLevel
        {
            get
            {
                return beatPulseMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                beatPulseMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The minimum blue color level (true color)
        /// </summary>
        public int BeatPulseMinimumBlueColorLevel
        {
            get
            {
                return beatPulseMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                beatPulseMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int BeatPulseMinimumColorLevel
        {
            get
            {
                return beatPulseMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                beatPulseMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The maximum red color level (true color)
        /// </summary>
        public int BeatPulseMaximumRedColorLevel
        {
            get
            {
                return beatPulseMaximumRedColorLevel;
            }
            set
            {
                if (value <= beatPulseMinimumRedColorLevel)
                    value = beatPulseMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                beatPulseMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The maximum green color level (true color)
        /// </summary>
        public int BeatPulseMaximumGreenColorLevel
        {
            get
            {
                return beatPulseMaximumGreenColorLevel;
            }
            set
            {
                if (value <= beatPulseMinimumGreenColorLevel)
                    value = beatPulseMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                beatPulseMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The maximum blue color level (true color)
        /// </summary>
        public int BeatPulseMaximumBlueColorLevel
        {
            get
            {
                return beatPulseMaximumBlueColorLevel;
            }
            set
            {
                if (value <= beatPulseMinimumBlueColorLevel)
                    value = beatPulseMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                beatPulseMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int BeatPulseMaximumColorLevel
        {
            get
            {
                return beatPulseMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= beatPulseMinimumColorLevel)
                    value = beatPulseMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                beatPulseMaximumColorLevel = value;
            }
        }
    }
}
