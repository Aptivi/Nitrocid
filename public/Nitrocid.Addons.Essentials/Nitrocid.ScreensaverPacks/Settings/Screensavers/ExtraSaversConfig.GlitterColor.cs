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
        private bool glitterColorTrueColor = true;
        private int glitterColorDelay = 1;
        private int glitterColorMinimumRedColorLevel = 0;
        private int glitterColorMinimumGreenColorLevel = 0;
        private int glitterColorMinimumBlueColorLevel = 0;
        private int glitterColorMinimumColorLevel = 0;
        private int glitterColorMaximumRedColorLevel = 255;
        private int glitterColorMaximumGreenColorLevel = 255;
        private int glitterColorMaximumBlueColorLevel = 255;
        private int glitterColorMaximumColorLevel = 255;

        /// <summary>
        /// [GlitterColor] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool GlitterColorTrueColor
        {
            get
            {
                return glitterColorTrueColor;
            }
            set
            {
                glitterColorTrueColor = value;
            }
        }

        /// <summary>
        /// [GlitterColor] How many milliseconds to wait before making the next write?
        /// </summary>
        public int GlitterColorDelay
        {
            get
            {
                return glitterColorDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1;
                glitterColorDelay = value;
            }
        }

        /// <summary>
        /// [GlitterColor] The minimum red color level (true color)
        /// </summary>
        public int GlitterColorMinimumRedColorLevel
        {
            get
            {
                return glitterColorMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                glitterColorMinimumRedColorLevel = value;
            }
        }

        /// <summary>
        /// [GlitterColor] The minimum green color level (true color)
        /// </summary>
        public int GlitterColorMinimumGreenColorLevel
        {
            get
            {
                return glitterColorMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                glitterColorMinimumGreenColorLevel = value;
            }
        }

        /// <summary>
        /// [GlitterColor] The minimum blue color level (true color)
        /// </summary>
        public int GlitterColorMinimumBlueColorLevel
        {
            get
            {
                return glitterColorMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                glitterColorMinimumBlueColorLevel = value;
            }
        }

        /// <summary>
        /// [GlitterColor] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int GlitterColorMinimumColorLevel
        {
            get
            {
                return glitterColorMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                glitterColorMinimumColorLevel = value;
            }
        }

        /// <summary>
        /// [GlitterColor] The maximum red color level (true color)
        /// </summary>
        public int GlitterColorMaximumRedColorLevel
        {
            get
            {
                return glitterColorMaximumRedColorLevel;
            }
            set
            {
                if (value <= glitterColorMinimumRedColorLevel)
                    value = glitterColorMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                glitterColorMaximumRedColorLevel = value;
            }
        }

        /// <summary>
        /// [GlitterColor] The maximum green color level (true color)
        /// </summary>
        public int GlitterColorMaximumGreenColorLevel
        {
            get
            {
                return glitterColorMaximumGreenColorLevel;
            }
            set
            {
                if (value <= glitterColorMinimumGreenColorLevel)
                    value = glitterColorMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                glitterColorMaximumGreenColorLevel = value;
            }
        }

        /// <summary>
        /// [GlitterColor] The maximum blue color level (true color)
        /// </summary>
        public int GlitterColorMaximumBlueColorLevel
        {
            get
            {
                return glitterColorMaximumBlueColorLevel;
            }
            set
            {
                if (value <= glitterColorMinimumBlueColorLevel)
                    value = glitterColorMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                glitterColorMaximumBlueColorLevel = value;
            }
        }

        /// <summary>
        /// [GlitterColor] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int GlitterColorMaximumColorLevel
        {
            get
            {
                return glitterColorMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= glitterColorMinimumColorLevel)
                    value = glitterColorMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                glitterColorMaximumColorLevel = value;
            }
        }
    }
}
