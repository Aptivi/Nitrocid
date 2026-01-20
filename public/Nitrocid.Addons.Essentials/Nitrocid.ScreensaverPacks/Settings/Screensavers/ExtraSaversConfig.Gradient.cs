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
        private int gradientNextRotDelay = 3000;
        private int gradientMinimumRedColorLevelStart = 0;
        private int gradientMinimumGreenColorLevelStart = 0;
        private int gradientMinimumBlueColorLevelStart = 0;
        private int gradientMaximumRedColorLevelStart = 255;
        private int gradientMaximumGreenColorLevelStart = 255;
        private int gradientMaximumBlueColorLevelStart = 255;
        private int gradientMinimumRedColorLevelEnd = 0;
        private int gradientMinimumGreenColorLevelEnd = 0;
        private int gradientMinimumBlueColorLevelEnd = 0;
        private int gradientMaximumRedColorLevelEnd = 255;
        private int gradientMaximumGreenColorLevelEnd = 255;
        private int gradientMaximumBlueColorLevelEnd = 255;

        /// <summary>
        /// [Gradient] How many milliseconds to wait before rotting the next screen?
        /// </summary>
        public int GradientNextRotDelay
        {
            get
            {
                return gradientNextRotDelay;
            }
            set
            {
                if (value <= 0)
                    value = 3000;
                gradientNextRotDelay = value;
            }
        }
        /// <summary>
        /// [Gradient] The minimum red color level (true color - start)
        /// </summary>
        public int GradientMinimumRedColorLevelStart
        {
            get
            {
                return gradientMinimumRedColorLevelStart;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                gradientMinimumRedColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Gradient] The minimum green color level (true color - start)
        /// </summary>
        public int GradientMinimumGreenColorLevelStart
        {
            get
            {
                return gradientMinimumGreenColorLevelStart;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                gradientMinimumGreenColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Gradient] The minimum blue color level (true color - start)
        /// </summary>
        public int GradientMinimumBlueColorLevelStart
        {
            get
            {
                return gradientMinimumBlueColorLevelStart;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                gradientMinimumBlueColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Gradient] The maximum red color level (true color - start)
        /// </summary>
        public int GradientMaximumRedColorLevelStart
        {
            get
            {
                return gradientMaximumRedColorLevelStart;
            }
            set
            {
                if (value <= gradientMinimumRedColorLevelStart)
                    value = gradientMinimumRedColorLevelStart;
                if (value > 255)
                    value = 255;
                gradientMaximumRedColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Gradient] The maximum green color level (true color - start)
        /// </summary>
        public int GradientMaximumGreenColorLevelStart
        {
            get
            {
                return gradientMaximumGreenColorLevelStart;
            }
            set
            {
                if (value <= gradientMinimumGreenColorLevelStart)
                    value = gradientMinimumGreenColorLevelStart;
                if (value > 255)
                    value = 255;
                gradientMaximumGreenColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Gradient] The maximum blue color level (true color - start)
        /// </summary>
        public int GradientMaximumBlueColorLevelStart
        {
            get
            {
                return gradientMaximumBlueColorLevelStart;
            }
            set
            {
                if (value <= gradientMinimumBlueColorLevelStart)
                    value = gradientMinimumBlueColorLevelStart;
                if (value > 255)
                    value = 255;
                gradientMaximumBlueColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Gradient] The minimum red color level (true color - end)
        /// </summary>
        public int GradientMinimumRedColorLevelEnd
        {
            get
            {
                return gradientMinimumRedColorLevelEnd;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                gradientMinimumRedColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Gradient] The minimum green color level (true color - end)
        /// </summary>
        public int GradientMinimumGreenColorLevelEnd
        {
            get
            {
                return gradientMinimumGreenColorLevelEnd;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                gradientMinimumGreenColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Gradient] The minimum blue color level (true color - end)
        /// </summary>
        public int GradientMinimumBlueColorLevelEnd
        {
            get
            {
                return gradientMinimumBlueColorLevelEnd;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                gradientMinimumBlueColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Gradient] The maximum red color level (true color - end)
        /// </summary>
        public int GradientMaximumRedColorLevelEnd
        {
            get
            {
                return gradientMaximumRedColorLevelEnd;
            }
            set
            {
                if (value <= gradientMinimumRedColorLevelEnd)
                    value = gradientMinimumRedColorLevelEnd;
                if (value > 255)
                    value = 255;
                gradientMaximumRedColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Gradient] The maximum green color level (true color - end)
        /// </summary>
        public int GradientMaximumGreenColorLevelEnd
        {
            get
            {
                return gradientMaximumGreenColorLevelEnd;
            }
            set
            {
                if (value <= gradientMinimumGreenColorLevelEnd)
                    value = gradientMinimumGreenColorLevelEnd;
                if (value > 255)
                    value = 255;
                gradientMaximumGreenColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Gradient] The maximum blue color level (true color - end)
        /// </summary>
        public int GradientMaximumBlueColorLevelEnd
        {
            get
            {
                return gradientMaximumBlueColorLevelEnd;
            }
            set
            {
                if (value <= gradientMinimumBlueColorLevelEnd)
                    value = gradientMinimumBlueColorLevelEnd;
                if (value > 255)
                    value = 255;
                gradientMaximumBlueColorLevelEnd = value;
            }
        }
    }
}
