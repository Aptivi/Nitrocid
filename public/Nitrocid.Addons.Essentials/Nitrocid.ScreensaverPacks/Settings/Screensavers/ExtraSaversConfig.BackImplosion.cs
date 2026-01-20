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
        private bool backImplosionTrueColor = true;
        private int backImplosionDelay = 20;
        private int backImplosionMinimumRedColorLevel = 0;
        private int backImplosionMinimumGreenColorLevel = 0;
        private int backImplosionMinimumBlueColorLevel = 0;
        private int backImplosionMinimumColorLevel = 0;
        private int backImplosionMaximumRedColorLevel = 255;
        private int backImplosionMaximumGreenColorLevel = 255;
        private int backImplosionMaximumBlueColorLevel = 255;
        private int backImplosionMaximumColorLevel = 255;

        /// <summary>
        /// [BackImplosion] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool BackImplosionTrueColor
        {
            get
            {
                return backImplosionTrueColor;
            }
            set
            {
                backImplosionTrueColor = value;
            }
        }
        /// <summary>
        /// [BackImplosion] How many milliseconds to wait before making the next write?
        /// </summary>
        public int BackImplosionDelay
        {
            get
            {
                return backImplosionDelay;
            }
            set
            {
                if (value <= 0)
                    value = 20;
                backImplosionDelay = value;
            }
        }
        /// <summary>
        /// [BackImplosion] The minimum red color level (true color)
        /// </summary>
        public int BackImplosionMinimumRedColorLevel
        {
            get
            {
                return backImplosionMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                backImplosionMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BackImplosion] The minimum green color level (true color)
        /// </summary>
        public int BackImplosionMinimumGreenColorLevel
        {
            get
            {
                return backImplosionMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                backImplosionMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BackImplosion] The minimum blue color level (true color)
        /// </summary>
        public int BackImplosionMinimumBlueColorLevel
        {
            get
            {
                return backImplosionMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                backImplosionMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BackImplosion] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int BackImplosionMinimumColorLevel
        {
            get
            {
                return backImplosionMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                backImplosionMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [BackImplosion] The maximum red color level (true color)
        /// </summary>
        public int BackImplosionMaximumRedColorLevel
        {
            get
            {
                return backImplosionMaximumRedColorLevel;
            }
            set
            {
                if (value <= backImplosionMinimumRedColorLevel)
                    value = backImplosionMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                backImplosionMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BackImplosion] The maximum green color level (true color)
        /// </summary>
        public int BackImplosionMaximumGreenColorLevel
        {
            get
            {
                return backImplosionMaximumGreenColorLevel;
            }
            set
            {
                if (value <= backImplosionMinimumGreenColorLevel)
                    value = backImplosionMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                backImplosionMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BackImplosion] The maximum blue color level (true color)
        /// </summary>
        public int BackImplosionMaximumBlueColorLevel
        {
            get
            {
                return backImplosionMaximumBlueColorLevel;
            }
            set
            {
                if (value <= backImplosionMinimumBlueColorLevel)
                    value = backImplosionMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                backImplosionMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BackImplosion] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int BackImplosionMaximumColorLevel
        {
            get
            {
                return backImplosionMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= backImplosionMinimumColorLevel)
                    value = backImplosionMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                backImplosionMaximumColorLevel = value;
            }
        }
    }
}
