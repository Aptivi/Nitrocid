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
        private bool fillFadeTrueColor = true;
        private int fillFadeMinimumRedColorLevel = 0;
        private int fillFadeMinimumGreenColorLevel = 0;
        private int fillFadeMinimumBlueColorLevel = 0;
        private int fillFadeMinimumColorLevel = 0;
        private int fillFadeMaximumGreenColorLevel = 255;
        private int fillFadeMaximumBlueColorLevel = 255;
        private int fillFadeMaximumColorLevel = 255;
        private int fillFadeMaximumRedColorLevel = 255;

        /// <summary>
        /// [FillFade] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool FillFadeTrueColor
        {
            get
            {
                return fillFadeTrueColor;
            }
            set
            {
                fillFadeTrueColor = value;
            }
        }
        /// <summary>
        /// [FillFade] The minimum red color level (true color)
        /// </summary>
        public int FillFadeMinimumRedColorLevel
        {
            get
            {
                return fillFadeMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                fillFadeMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FillFade] The minimum green color level (true color)
        /// </summary>
        public int FillFadeMinimumGreenColorLevel
        {
            get
            {
                return fillFadeMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                fillFadeMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FillFade] The minimum blue color level (true color)
        /// </summary>
        public int FillFadeMinimumBlueColorLevel
        {
            get
            {
                return fillFadeMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                fillFadeMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [FillFade] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int FillFadeMinimumColorLevel
        {
            get
            {
                return fillFadeMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                fillFadeMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [FillFade] The maximum red color level (true color)
        /// </summary>
        public int FillFadeMaximumRedColorLevel
        {
            get
            {
                return fillFadeMaximumRedColorLevel;
            }
            set
            {
                if (value <= fillFadeMinimumRedColorLevel)
                    value = fillFadeMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                fillFadeMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FillFade] The maximum green color level (true color)
        /// </summary>
        public int FillFadeMaximumGreenColorLevel
        {
            get
            {
                return fillFadeMaximumGreenColorLevel;
            }
            set
            {
                if (value <= fillFadeMinimumGreenColorLevel)
                    value = fillFadeMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                fillFadeMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FillFade] The maximum blue color level (true color)
        /// </summary>
        public int FillFadeMaximumBlueColorLevel
        {
            get
            {
                return fillFadeMaximumBlueColorLevel;
            }
            set
            {
                if (value <= fillFadeMinimumBlueColorLevel)
                    value = fillFadeMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                fillFadeMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [FillFade] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int FillFadeMaximumColorLevel
        {
            get
            {
                return fillFadeMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= fillFadeMinimumColorLevel)
                    value = fillFadeMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                fillFadeMaximumColorLevel = value;
            }
        }
    }
}
