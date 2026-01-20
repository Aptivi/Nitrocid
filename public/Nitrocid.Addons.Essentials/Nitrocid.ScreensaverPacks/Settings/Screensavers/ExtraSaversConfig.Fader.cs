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
        private int faderDelay = 50;
        private int faderFadeOutDelay = 3000;
        private string faderWrite = "Nitrocid KS";
        private int faderMaxSteps = 25;
        private string faderBackgroundColor = new Color(0, 0, 0).PlainSequence;
        private int faderMinimumRedColorLevel = 0;
        private int faderMinimumGreenColorLevel = 0;
        private int faderMinimumBlueColorLevel = 0;
        private int faderMaximumRedColorLevel = 255;
        private int faderMaximumGreenColorLevel = 255;
        private int faderMaximumBlueColorLevel = 255;

        /// <summary>
        /// [Fader] How many milliseconds to wait before making the next write?
        /// </summary>
        public int FaderDelay
        {
            get
            {
                return faderDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                faderDelay = value;
            }
        }
        /// <summary>
        /// [Fader] How many milliseconds to wait before fading the text out?
        /// </summary>
        public int FaderFadeOutDelay
        {
            get
            {
                return faderFadeOutDelay;
            }
            set
            {
                if (value <= 0)
                    value = 3000;
                faderFadeOutDelay = value;
            }
        }
        /// <summary>
        /// [Fader] Text for Fader. Shorter is better.
        /// </summary>
        public string FaderWrite
        {
            get
            {
                return faderWrite;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Nitrocid KS";
                faderWrite = value;
            }
        }
        /// <summary>
        /// [Fader] How many fade steps to do?
        /// </summary>
        public int FaderMaxSteps
        {
            get
            {
                return faderMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                faderMaxSteps = value;
            }
        }
        /// <summary>
        /// [Fader] Screensaver background color
        /// </summary>
        public string FaderBackgroundColor
        {
            get
            {
                return faderBackgroundColor;
            }
            set
            {
                faderBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [Fader] The minimum red color level (true color)
        /// </summary>
        public int FaderMinimumRedColorLevel
        {
            get
            {
                return faderMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                faderMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Fader] The minimum green color level (true color)
        /// </summary>
        public int FaderMinimumGreenColorLevel
        {
            get
            {
                return faderMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                faderMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Fader] The minimum blue color level (true color)
        /// </summary>
        public int FaderMinimumBlueColorLevel
        {
            get
            {
                return faderMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                faderMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Fader] The maximum red color level (true color)
        /// </summary>
        public int FaderMaximumRedColorLevel
        {
            get
            {
                return faderMaximumRedColorLevel;
            }
            set
            {
                if (value <= faderMinimumRedColorLevel)
                    value = faderMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                faderMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Fader] The maximum green color level (true color)
        /// </summary>
        public int FaderMaximumGreenColorLevel
        {
            get
            {
                return faderMaximumGreenColorLevel;
            }
            set
            {
                if (value <= faderMinimumGreenColorLevel)
                    value = faderMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                faderMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Fader] The maximum blue color level (true color)
        /// </summary>
        public int FaderMaximumBlueColorLevel
        {
            get
            {
                return faderMaximumBlueColorLevel;
            }
            set
            {
                if (value <= faderMinimumBlueColorLevel)
                    value = faderMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                faderMaximumBlueColorLevel = value;
            }
        }
    }
}
