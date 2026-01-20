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
        private int faderBackDelay = 10;
        private int faderBackFadeOutDelay = 3000;
        private int faderBackMaxSteps = 25;
        private int faderBackMinimumRedColorLevel = 0;
        private int faderBackMinimumGreenColorLevel = 0;
        private int faderBackMinimumBlueColorLevel = 0;
        private int faderBackMaximumRedColorLevel = 255;
        private int faderBackMaximumGreenColorLevel = 255;
        private int faderBackMaximumBlueColorLevel = 255;

        /// <summary>
        /// [FaderBack] How many milliseconds to wait before making the next write?
        /// </summary>
        public int FaderBackDelay
        {
            get
            {
                return faderBackDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                faderBackDelay = value;
            }
        }
        /// <summary>
        /// [FaderBack] How many milliseconds to wait before fading the text out?
        /// </summary>
        public int FaderBackFadeOutDelay
        {
            get
            {
                return faderBackFadeOutDelay;
            }
            set
            {
                if (value <= 0)
                    value = 3000;
                faderBackFadeOutDelay = value;
            }
        }
        /// <summary>
        /// [FaderBack] How many fade steps to do?
        /// </summary>
        public int FaderBackMaxSteps
        {
            get
            {
                return faderBackMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                faderBackMaxSteps = value;
            }
        }
        /// <summary>
        /// [FaderBack] The minimum red color level (true color)
        /// </summary>
        public int FaderBackMinimumRedColorLevel
        {
            get
            {
                return faderBackMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                faderBackMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FaderBack] The minimum green color level (true color)
        /// </summary>
        public int FaderBackMinimumGreenColorLevel
        {
            get
            {
                return faderBackMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                faderBackMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FaderBack] The minimum blue color level (true color)
        /// </summary>
        public int FaderBackMinimumBlueColorLevel
        {
            get
            {
                return faderBackMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                faderBackMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [FaderBack] The maximum red color level (true color)
        /// </summary>
        public int FaderBackMaximumRedColorLevel
        {
            get
            {
                return faderBackMaximumRedColorLevel;
            }
            set
            {
                if (value <= faderBackMinimumRedColorLevel)
                    value = faderBackMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                faderBackMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FaderBack] The maximum green color level (true color)
        /// </summary>
        public int FaderBackMaximumGreenColorLevel
        {
            get
            {
                return faderBackMaximumGreenColorLevel;
            }
            set
            {
                if (value <= faderBackMinimumGreenColorLevel)
                    value = faderBackMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                faderBackMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FaderBack] The maximum blue color level (true color)
        /// </summary>
        public int FaderBackMaximumBlueColorLevel
        {
            get
            {
                return faderBackMaximumBlueColorLevel;
            }
            set
            {
                if (value <= faderBackMinimumBlueColorLevel)
                    value = faderBackMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                faderBackMaximumBlueColorLevel = value;
            }
        }
    }
}
