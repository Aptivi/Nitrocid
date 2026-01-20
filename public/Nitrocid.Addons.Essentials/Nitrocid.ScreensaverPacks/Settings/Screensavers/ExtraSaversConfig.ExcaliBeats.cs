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

using Nitrocid.Base.Kernel;
using Nitrocid.Base.Kernel.Configuration.Instances;
using System.Runtime.Versioning;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Settings
{
    /// <summary>
    /// Screensaver kernel configuration instance
    /// </summary>
    public partial class ExtraSaversConfig : BaseKernelConfig
    {
        private bool excaliBeatsTrueColor = true;
        private int excaliBeatsDelay = 140;
        private bool excaliBeatsCycleColors = true;
        private bool excaliBeatsExplicit = true;
        private bool excaliBeatsTranceMode;
        private string excaliBeatsBeatColor = "17";
        private int excaliBeatsMaxSteps = 25;
        private int excaliBeatsMinimumRedColorLevel = 0;
        private int excaliBeatsMinimumGreenColorLevel = 0;
        private int excaliBeatsMinimumBlueColorLevel = 0;
        private int excaliBeatsMinimumColorLevel = 0;
        private int excaliBeatsMaximumRedColorLevel = 255;
        private int excaliBeatsMaximumGreenColorLevel = 255;
        private int excaliBeatsMaximumBlueColorLevel = 255;
        private int excaliBeatsMaximumColorLevel = 255;

        /// <summary>
        /// [ExcaliBeats] Enable truecolor support. Has a higher priority than 255 color support. Please note that it only works if color cycling is enabled.
        /// </summary>
        public bool ExcaliBeatsTrueColor
        {
            get
            {
                return excaliBeatsTrueColor;
            }
            set
            {
                excaliBeatsTrueColor = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] Enable color cycling (uses RNG. If disabled, uses the <see cref="ExcaliBeatsBeatColor"/> color.)
        /// </summary>
        public bool ExcaliBeatsCycleColors
        {
            get
            {
                return excaliBeatsCycleColors;
            }
            set
            {
                excaliBeatsCycleColors = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] Explicitly change the text to Excalibur
        /// </summary>
        public bool ExcaliBeatsExplicit
        {
            get
            {
                return excaliBeatsExplicit;
            }
            set
            {
                excaliBeatsExplicit = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] [Linux only] Trance mode - Multiplies the BPM by 2 to simulate the trance music style
        /// </summary>
        public bool ExcaliBeatsTranceMode
        {
            get
            {
                return excaliBeatsTranceMode;
            }
            [UnsupportedOSPlatform("windows")]
            set
            {
                if (KernelPlatform.IsOnUnix())
                    excaliBeatsTranceMode = value;
                else
                    excaliBeatsTranceMode = false;
            }
        }
        /// <summary>
        /// [ExcaliBeats] The color of beats. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public string ExcaliBeatsBeatColor
        {
            get
            {
                return excaliBeatsBeatColor;
            }
            set
            {
                excaliBeatsBeatColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [ExcaliBeats] How many beats per minute to wait before making the next write?
        /// </summary>
        public int ExcaliBeatsDelay
        {
            get
            {
                return excaliBeatsDelay;
            }
            set
            {
                if (value <= 0)
                    value = 140;
                excaliBeatsDelay = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] How many fade steps to do?
        /// </summary>
        public int ExcaliBeatsMaxSteps
        {
            get
            {
                return excaliBeatsMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                excaliBeatsMaxSteps = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] The minimum red color level (true color)
        /// </summary>
        public int ExcaliBeatsMinimumRedColorLevel
        {
            get
            {
                return excaliBeatsMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                excaliBeatsMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] The minimum green color level (true color)
        /// </summary>
        public int ExcaliBeatsMinimumGreenColorLevel
        {
            get
            {
                return excaliBeatsMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                excaliBeatsMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] The minimum blue color level (true color)
        /// </summary>
        public int ExcaliBeatsMinimumBlueColorLevel
        {
            get
            {
                return excaliBeatsMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                excaliBeatsMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int ExcaliBeatsMinimumColorLevel
        {
            get
            {
                return excaliBeatsMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                excaliBeatsMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] The maximum red color level (true color)
        /// </summary>
        public int ExcaliBeatsMaximumRedColorLevel
        {
            get
            {
                return excaliBeatsMaximumRedColorLevel;
            }
            set
            {
                if (value <= excaliBeatsMinimumRedColorLevel)
                    value = excaliBeatsMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                excaliBeatsMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] The maximum green color level (true color)
        /// </summary>
        public int ExcaliBeatsMaximumGreenColorLevel
        {
            get
            {
                return excaliBeatsMaximumGreenColorLevel;
            }
            set
            {
                if (value <= excaliBeatsMinimumGreenColorLevel)
                    value = excaliBeatsMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                excaliBeatsMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] The maximum blue color level (true color)
        /// </summary>
        public int ExcaliBeatsMaximumBlueColorLevel
        {
            get
            {
                return excaliBeatsMaximumBlueColorLevel;
            }
            set
            {
                if (value <= excaliBeatsMinimumBlueColorLevel)
                    value = excaliBeatsMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                excaliBeatsMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int ExcaliBeatsMaximumColorLevel
        {
            get
            {
                return excaliBeatsMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= excaliBeatsMinimumColorLevel)
                    value = excaliBeatsMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                excaliBeatsMaximumColorLevel = value;
            }
        }
    }
}
