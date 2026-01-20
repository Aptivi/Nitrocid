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
        private bool beatFaderTrueColor = true;
        private int beatFaderDelay = 120;
        private bool beatFaderCycleColors = true;
        private string beatFaderBeatColor = "17";
        private int beatFaderMaxSteps = 25;
        private int beatFaderMinimumRedColorLevel = 0;
        private int beatFaderMinimumGreenColorLevel = 0;
        private int beatFaderMinimumBlueColorLevel = 0;
        private int beatFaderMinimumColorLevel = 0;
        private int beatFaderMaximumRedColorLevel = 255;
        private int beatFaderMaximumGreenColorLevel = 255;
        private int beatFaderMaximumBlueColorLevel = 255;
        private int beatFaderMaximumColorLevel = 255;

        /// <summary>
        /// [BeatFader] Enable truecolor support. Has a higher priority than 255 color support. Please note that it only works if color cycling is enabled.
        /// </summary>
        public bool BeatFaderTrueColor
        {
            get
            {
                return beatFaderTrueColor;
            }
            set
            {
                beatFaderTrueColor = value;
            }
        }
        /// <summary>
        /// [BeatFader] Enable color cycling (uses RNG. If disabled, uses the <see cref="BeatFaderBeatColor"/> color.)
        /// </summary>
        public bool BeatFaderCycleColors
        {
            get
            {
                return beatFaderCycleColors;
            }
            set
            {
                beatFaderCycleColors = value;
            }
        }
        /// <summary>
        /// [BeatFader] The color of beats. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public string BeatFaderBeatColor
        {
            get
            {
                return beatFaderBeatColor;
            }
            set
            {
                beatFaderBeatColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [BeatFader] How many beats per minute to wait before making the next write?
        /// </summary>
        public int BeatFaderDelay
        {
            get
            {
                return beatFaderDelay;
            }
            set
            {
                if (value <= 0)
                    value = 120;
                beatFaderDelay = value;
            }
        }
        /// <summary>
        /// [BeatFader] How many fade steps to do?
        /// </summary>
        public int BeatFaderMaxSteps
        {
            get
            {
                return beatFaderMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                beatFaderMaxSteps = value;
            }
        }
        /// <summary>
        /// [BeatFader] The minimum red color level (true color)
        /// </summary>
        public int BeatFaderMinimumRedColorLevel
        {
            get
            {
                return beatFaderMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                beatFaderMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatFader] The minimum green color level (true color)
        /// </summary>
        public int BeatFaderMinimumGreenColorLevel
        {
            get
            {
                return beatFaderMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                beatFaderMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatFader] The minimum blue color level (true color)
        /// </summary>
        public int BeatFaderMinimumBlueColorLevel
        {
            get
            {
                return beatFaderMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                beatFaderMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatFader] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int BeatFaderMinimumColorLevel
        {
            get
            {
                return beatFaderMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                beatFaderMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatFader] The maximum red color level (true color)
        /// </summary>
        public int BeatFaderMaximumRedColorLevel
        {
            get
            {
                return beatFaderMaximumRedColorLevel;
            }
            set
            {
                if (value <= beatFaderMinimumRedColorLevel)
                    value = beatFaderMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                beatFaderMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatFader] The maximum green color level (true color)
        /// </summary>
        public int BeatFaderMaximumGreenColorLevel
        {
            get
            {
                return beatFaderMaximumGreenColorLevel;
            }
            set
            {
                if (value <= beatFaderMinimumGreenColorLevel)
                    value = beatFaderMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                beatFaderMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatFader] The maximum blue color level (true color)
        /// </summary>
        public int BeatFaderMaximumBlueColorLevel
        {
            get
            {
                return beatFaderMaximumBlueColorLevel;
            }
            set
            {
                if (value <= beatFaderMinimumBlueColorLevel)
                    value = beatFaderMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                beatFaderMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatFader] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int BeatFaderMaximumColorLevel
        {
            get
            {
                return beatFaderMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= beatFaderMinimumColorLevel)
                    value = beatFaderMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                beatFaderMaximumColorLevel = value;
            }
        }
    }
}
