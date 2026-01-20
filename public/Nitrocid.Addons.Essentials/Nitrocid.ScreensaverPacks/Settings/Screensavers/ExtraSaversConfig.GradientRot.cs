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
        private int gradientRotDelay = 10;
        private int gradientRotNextRampDelay = 250;
        private int gradientRotMinimumRedColorLevelStart = 0;
        private int gradientRotMinimumGreenColorLevelStart = 0;
        private int gradientRotMinimumBlueColorLevelStart = 0;
        private int gradientRotMaximumRedColorLevelStart = 255;
        private int gradientRotMaximumGreenColorLevelStart = 255;
        private int gradientRotMaximumBlueColorLevelStart = 255;
        private int gradientRotMinimumRedColorLevelEnd = 0;
        private int gradientRotMinimumGreenColorLevelEnd = 0;
        private int gradientRotMinimumBlueColorLevelEnd = 0;
        private int gradientRotMaximumRedColorLevelEnd = 255;
        private int gradientRotMaximumGreenColorLevelEnd = 255;
        private int gradientRotMaximumBlueColorLevelEnd = 255;

        /// <summary>
        /// [GradientRot] How many milliseconds to wait before making the next write?
        /// </summary>
        public int GradientRotDelay
        {
            get
            {
                return gradientRotDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                gradientRotDelay = value;
            }
        }
        /// <summary>
        /// [GradientRot] How many milliseconds to wait before rotting the next screen?
        /// </summary>
        public int GradientRotNextRampDelay
        {
            get
            {
                return gradientRotNextRampDelay;
            }
            set
            {
                if (value <= 0)
                    value = 250;
                gradientRotNextRampDelay = value;
            }
        }
        /// <summary>
        /// [GradientRot] The minimum red color level (true color - start)
        /// </summary>
        public int GradientRotMinimumRedColorLevelStart
        {
            get
            {
                return gradientRotMinimumRedColorLevelStart;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                gradientRotMinimumRedColorLevelStart = value;
            }
        }
        /// <summary>
        /// [GradientRot] The minimum green color level (true color - start)
        /// </summary>
        public int GradientRotMinimumGreenColorLevelStart
        {
            get
            {
                return gradientRotMinimumGreenColorLevelStart;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                gradientRotMinimumGreenColorLevelStart = value;
            }
        }
        /// <summary>
        /// [GradientRot] The minimum blue color level (true color - start)
        /// </summary>
        public int GradientRotMinimumBlueColorLevelStart
        {
            get
            {
                return gradientRotMinimumBlueColorLevelStart;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                gradientRotMinimumBlueColorLevelStart = value;
            }
        }
        /// <summary>
        /// [GradientRot] The maximum red color level (true color - start)
        /// </summary>
        public int GradientRotMaximumRedColorLevelStart
        {
            get
            {
                return gradientRotMaximumRedColorLevelStart;
            }
            set
            {
                if (value <= gradientRotMinimumRedColorLevelStart)
                    value = gradientRotMinimumRedColorLevelStart;
                if (value > 255)
                    value = 255;
                gradientRotMaximumRedColorLevelStart = value;
            }
        }
        /// <summary>
        /// [GradientRot] The maximum green color level (true color - start)
        /// </summary>
        public int GradientRotMaximumGreenColorLevelStart
        {
            get
            {
                return gradientRotMaximumGreenColorLevelStart;
            }
            set
            {
                if (value <= gradientRotMinimumGreenColorLevelStart)
                    value = gradientRotMinimumGreenColorLevelStart;
                if (value > 255)
                    value = 255;
                gradientRotMaximumGreenColorLevelStart = value;
            }
        }
        /// <summary>
        /// [GradientRot] The maximum blue color level (true color - start)
        /// </summary>
        public int GradientRotMaximumBlueColorLevelStart
        {
            get
            {
                return gradientRotMaximumBlueColorLevelStart;
            }
            set
            {
                if (value <= gradientRotMinimumBlueColorLevelStart)
                    value = gradientRotMinimumBlueColorLevelStart;
                if (value > 255)
                    value = 255;
                gradientRotMaximumBlueColorLevelStart = value;
            }
        }
        /// <summary>
        /// [GradientRot] The minimum red color level (true color - end)
        /// </summary>
        public int GradientRotMinimumRedColorLevelEnd
        {
            get
            {
                return gradientRotMinimumRedColorLevelEnd;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                gradientRotMinimumRedColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [GradientRot] The minimum green color level (true color - end)
        /// </summary>
        public int GradientRotMinimumGreenColorLevelEnd
        {
            get
            {
                return gradientRotMinimumGreenColorLevelEnd;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                gradientRotMinimumGreenColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [GradientRot] The minimum blue color level (true color - end)
        /// </summary>
        public int GradientRotMinimumBlueColorLevelEnd
        {
            get
            {
                return gradientRotMinimumBlueColorLevelEnd;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                gradientRotMinimumBlueColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [GradientRot] The maximum red color level (true color - end)
        /// </summary>
        public int GradientRotMaximumRedColorLevelEnd
        {
            get
            {
                return gradientRotMaximumRedColorLevelEnd;
            }
            set
            {
                if (value <= gradientRotMinimumRedColorLevelEnd)
                    value = gradientRotMinimumRedColorLevelEnd;
                if (value > 255)
                    value = 255;
                gradientRotMaximumRedColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [GradientRot] The maximum green color level (true color - end)
        /// </summary>
        public int GradientRotMaximumGreenColorLevelEnd
        {
            get
            {
                return gradientRotMaximumGreenColorLevelEnd;
            }
            set
            {
                if (value <= gradientRotMinimumGreenColorLevelEnd)
                    value = gradientRotMinimumGreenColorLevelEnd;
                if (value > 255)
                    value = 255;
                gradientRotMaximumGreenColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [GradientRot] The maximum blue color level (true color - end)
        /// </summary>
        public int GradientRotMaximumBlueColorLevelEnd
        {
            get
            {
                return gradientRotMaximumBlueColorLevelEnd;
            }
            set
            {
                if (value <= gradientRotMinimumBlueColorLevelEnd)
                    value = gradientRotMinimumBlueColorLevelEnd;
                if (value > 255)
                    value = 255;
                gradientRotMaximumBlueColorLevelEnd = value;
            }
        }
    }
}
