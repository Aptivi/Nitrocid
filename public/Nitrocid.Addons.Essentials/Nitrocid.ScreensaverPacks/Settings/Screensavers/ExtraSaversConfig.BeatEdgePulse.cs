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
        private bool beatEdgePulseTrueColor = true;
        private int beatEdgePulseDelay = 50;
        private bool beatEdgePulseCycleColors = true;
        private string beatEdgePulseBeatColor = "17";
        private int beatEdgePulseMaxSteps = 25;
        private int beatEdgePulseMinimumRedColorLevel = 0;
        private int beatEdgePulseMinimumGreenColorLevel = 0;
        private int beatEdgePulseMinimumBlueColorLevel = 0;
        private int beatEdgePulseMinimumColorLevel = 0;
        private int beatEdgePulseMaximumRedColorLevel = 255;
        private int beatEdgePulseMaximumGreenColorLevel = 255;
        private int beatEdgePulseMaximumBlueColorLevel = 255;
        private int beatEdgePulseMaximumColorLevel = 255;

        /// <summary>
        /// [BeatEdgePulse] Enable truecolor support. Has a higher priority than 255 color support. Please note that it only works if color cycling is enabled.
        /// </summary>
        public bool BeatEdgePulseTrueColor
        {
            get
            {
                return beatEdgePulseTrueColor;
            }
            set
            {
                beatEdgePulseTrueColor = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] Enable color cycling (uses RNG. If disabled, uses the <see cref="BeatEdgePulseBeatColor"/> color.)
        /// </summary>
        public bool BeatEdgePulseCycleColors
        {
            get
            {
                return beatEdgePulseCycleColors;
            }
            set
            {
                beatEdgePulseCycleColors = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] The color of beats. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public string BeatEdgePulseBeatColor
        {
            get
            {
                return beatEdgePulseBeatColor;
            }
            set
            {
                beatEdgePulseBeatColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] How many milliseconds to wait before making the next write?
        /// </summary>
        public int BeatEdgePulseDelay
        {
            get
            {
                return beatEdgePulseDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                beatEdgePulseDelay = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] How many fade steps to do?
        /// </summary>
        public int BeatEdgePulseMaxSteps
        {
            get
            {
                return beatEdgePulseMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                beatEdgePulseMaxSteps = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] The minimum red color level (true color)
        /// </summary>
        public int BeatEdgePulseMinimumRedColorLevel
        {
            get
            {
                return beatEdgePulseMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                beatEdgePulseMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] The minimum green color level (true color)
        /// </summary>
        public int BeatEdgePulseMinimumGreenColorLevel
        {
            get
            {
                return beatEdgePulseMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                beatEdgePulseMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] The minimum blue color level (true color)
        /// </summary>
        public int BeatEdgePulseMinimumBlueColorLevel
        {
            get
            {
                return beatEdgePulseMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                beatEdgePulseMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int BeatEdgePulseMinimumColorLevel
        {
            get
            {
                return beatEdgePulseMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                beatEdgePulseMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] The maximum red color level (true color)
        /// </summary>
        public int BeatEdgePulseMaximumRedColorLevel
        {
            get
            {
                return beatEdgePulseMaximumRedColorLevel;
            }
            set
            {
                if (value <= beatEdgePulseMinimumRedColorLevel)
                    value = beatEdgePulseMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                beatEdgePulseMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] The maximum green color level (true color)
        /// </summary>
        public int BeatEdgePulseMaximumGreenColorLevel
        {
            get
            {
                return beatEdgePulseMaximumGreenColorLevel;
            }
            set
            {
                if (value <= beatEdgePulseMinimumGreenColorLevel)
                    value = beatEdgePulseMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                beatEdgePulseMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] The maximum blue color level (true color)
        /// </summary>
        public int BeatEdgePulseMaximumBlueColorLevel
        {
            get
            {
                return beatEdgePulseMaximumBlueColorLevel;
            }
            set
            {
                if (value <= beatEdgePulseMinimumBlueColorLevel)
                    value = beatEdgePulseMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                beatEdgePulseMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int BeatEdgePulseMaximumColorLevel
        {
            get
            {
                return beatEdgePulseMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= beatEdgePulseMinimumColorLevel)
                    value = beatEdgePulseMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                beatEdgePulseMaximumColorLevel = value;
            }
        }
    }
}
