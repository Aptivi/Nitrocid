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
        private int boxGridDelay = 5000;
        private int boxGridMinimumRedColorLevel = 0;
        private int boxGridMinimumGreenColorLevel = 0;
        private int boxGridMinimumBlueColorLevel = 0;
        private int boxGridMaximumRedColorLevel = 255;
        private int boxGridMaximumGreenColorLevel = 255;
        private int boxGridMaximumBlueColorLevel = 255;

        /// <summary>
        /// [BoxGrid] How many milliseconds to wait before making the next write?
        /// </summary>
        public int BoxGridDelay
        {
            get
            {
                return boxGridDelay;
            }
            set
            {
                if (value <= 0)
                    value = 5000;
                boxGridDelay = value;
            }
        }
        /// <summary>
        /// [BoxGrid] The minimum red color level (true color)
        /// </summary>
        public int BoxGridMinimumRedColorLevel
        {
            get
            {
                return boxGridMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                boxGridMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BoxGrid] The minimum green color level (true color)
        /// </summary>
        public int BoxGridMinimumGreenColorLevel
        {
            get
            {
                return boxGridMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                boxGridMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BoxGrid] The minimum blue color level (true color)
        /// </summary>
        public int BoxGridMinimumBlueColorLevel
        {
            get
            {
                return boxGridMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                boxGridMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BoxGrid] The maximum red color level (true color)
        /// </summary>
        public int BoxGridMaximumRedColorLevel
        {
            get
            {
                return boxGridMaximumRedColorLevel;
            }
            set
            {
                if (value <= boxGridMinimumRedColorLevel)
                    value = boxGridMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                boxGridMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BoxGrid] The maximum green color level (true color)
        /// </summary>
        public int BoxGridMaximumGreenColorLevel
        {
            get
            {
                return boxGridMaximumGreenColorLevel;
            }
            set
            {
                if (value <= boxGridMinimumGreenColorLevel)
                    value = boxGridMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                boxGridMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BoxGrid] The maximum blue color level (true color)
        /// </summary>
        public int BoxGridMaximumBlueColorLevel
        {
            get
            {
                return boxGridMaximumBlueColorLevel;
            }
            set
            {
                if (value <= boxGridMinimumBlueColorLevel)
                    value = boxGridMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                boxGridMaximumBlueColorLevel = value;
            }
        }
    }
}
