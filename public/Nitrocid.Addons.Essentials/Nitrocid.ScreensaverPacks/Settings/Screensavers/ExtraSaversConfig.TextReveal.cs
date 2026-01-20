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

namespace Nitrocid.ScreensaverPacks.Settings
{
    /// <summary>
    /// Screensaver kernel configuration instance
    /// </summary>
    public partial class ExtraSaversConfig : BaseKernelConfig
    {
        private int textRevealDelay = 50;
        private int textRevealFadeOutDelay = 3000;
        private int textRevealNewScreenDelay = 10000;
        private string textRevealWrite = "Nitrocid KS";
        private int textRevealMaxSteps = 25;
        private int textRevealMinimumRedColorLevel = 0;
        private int textRevealMinimumGreenColorLevel = 0;
        private int textRevealMinimumBlueColorLevel = 0;
        private int textRevealMaximumRedColorLevel = 255;
        private int textRevealMaximumGreenColorLevel = 255;
        private int textRevealMaximumBlueColorLevel = 255;

        /// <summary>
        /// [TextReveal] How many milliseconds to wait before making the next write?
        /// </summary>
        public int TextRevealDelay
        {
            get
            {
                return textRevealDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                textRevealDelay = value;
            }
        }
        /// <summary>
        /// [TextReveal] How many milliseconds to wait before fading the text out?
        /// </summary>
        public int TextRevealFadeOutDelay
        {
            get
            {
                return textRevealFadeOutDelay;
            }
            set
            {
                if (value <= 0)
                    value = 3000;
                textRevealFadeOutDelay = value;
            }
        }
        /// <summary>
        /// [TextReveal] How many milliseconds to wait before writing in the new screen?
        /// </summary>
        public int TextRevealNewScreenDelay
        {
            get
            {
                return textRevealNewScreenDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10000;
                textRevealNewScreenDelay = value;
            }
        }
        /// <summary>
        /// [TextReveal] Text for TextReveal. Shorter is better.
        /// </summary>
        public string TextRevealWrite
        {
            get
            {
                return textRevealWrite;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Nitrocid KS";
                textRevealWrite = value;
            }
        }
        /// <summary>
        /// [TextReveal] How many fade steps to do?
        /// </summary>
        public int TextRevealMaxSteps
        {
            get
            {
                return textRevealMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                textRevealMaxSteps = value;
            }
        }
        /// <summary>
        /// [TextReveal] The minimum red color level (true color)
        /// </summary>
        public int TextRevealMinimumRedColorLevel
        {
            get
            {
                return textRevealMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                textRevealMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [TextReveal] The minimum green color level (true color)
        /// </summary>
        public int TextRevealMinimumGreenColorLevel
        {
            get
            {
                return textRevealMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                textRevealMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [TextReveal] The minimum blue color level (true color)
        /// </summary>
        public int TextRevealMinimumBlueColorLevel
        {
            get
            {
                return textRevealMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                textRevealMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [TextReveal] The maximum red color level (true color)
        /// </summary>
        public int TextRevealMaximumRedColorLevel
        {
            get
            {
                return textRevealMaximumRedColorLevel;
            }
            set
            {
                if (value <= textRevealMinimumRedColorLevel)
                    value = textRevealMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                textRevealMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [TextReveal] The maximum green color level (true color)
        /// </summary>
        public int TextRevealMaximumGreenColorLevel
        {
            get
            {
                return textRevealMaximumGreenColorLevel;
            }
            set
            {
                if (value <= textRevealMinimumGreenColorLevel)
                    value = textRevealMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                textRevealMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [TextReveal] The maximum blue color level (true color)
        /// </summary>
        public int TextRevealMaximumBlueColorLevel
        {
            get
            {
                return textRevealMaximumBlueColorLevel;
            }
            set
            {
                if (value <= textRevealMinimumBlueColorLevel)
                    value = textRevealMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                textRevealMaximumBlueColorLevel = value;
            }
        }
    }
}
