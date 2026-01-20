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
        private bool stackBoxTrueColor = true;
        private int stackBoxDelay = 10;
        private int stackBoxMinimumRedColorLevel = 0;
        private int stackBoxMinimumGreenColorLevel = 0;
        private int stackBoxMinimumBlueColorLevel = 0;
        private int stackBoxMinimumColorLevel = 0;
        private int stackBoxMaximumRedColorLevel = 255;
        private int stackBoxMaximumGreenColorLevel = 255;
        private int stackBoxMaximumBlueColorLevel = 255;
        private int stackBoxMaximumColorLevel = 255;
        private bool stackBoxFill = true;

        /// <summary>
        /// [StackBox] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool StackBoxTrueColor
        {
            get
            {
                return stackBoxTrueColor;
            }
            set
            {
                stackBoxTrueColor = value;
            }
        }
        /// <summary>
        /// [StackBox] How many milliseconds to wait before making the next write?
        /// </summary>
        public int StackBoxDelay
        {
            get
            {
                return stackBoxDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                stackBoxDelay = value;
            }
        }
        /// <summary>
        /// [StackBox] Whether to fill in the boxes drawn, or only draw the outline
        /// </summary>
        public bool StackBoxFill
        {
            get
            {
                return stackBoxFill;
            }
            set
            {
                stackBoxFill = value;
            }
        }
        /// <summary>
        /// [StackBox] The minimum red color level (true color)
        /// </summary>
        public int StackBoxMinimumRedColorLevel
        {
            get
            {
                return stackBoxMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                stackBoxMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [StackBox] The minimum green color level (true color)
        /// </summary>
        public int StackBoxMinimumGreenColorLevel
        {
            get
            {
                return stackBoxMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                stackBoxMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [StackBox] The minimum blue color level (true color)
        /// </summary>
        public int StackBoxMinimumBlueColorLevel
        {
            get
            {
                return stackBoxMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                stackBoxMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [StackBox] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int StackBoxMinimumColorLevel
        {
            get
            {
                return stackBoxMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                stackBoxMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [StackBox] The maximum red color level (true color)
        /// </summary>
        public int StackBoxMaximumRedColorLevel
        {
            get
            {
                return stackBoxMaximumRedColorLevel;
            }
            set
            {
                if (value <= stackBoxMinimumRedColorLevel)
                    value = stackBoxMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                stackBoxMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [StackBox] The maximum green color level (true color)
        /// </summary>
        public int StackBoxMaximumGreenColorLevel
        {
            get
            {
                return stackBoxMaximumGreenColorLevel;
            }
            set
            {
                if (value <= stackBoxMinimumGreenColorLevel)
                    value = stackBoxMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                stackBoxMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [StackBox] The maximum blue color level (true color)
        /// </summary>
        public int StackBoxMaximumBlueColorLevel
        {
            get
            {
                return stackBoxMaximumBlueColorLevel;
            }
            set
            {
                if (value <= stackBoxMinimumBlueColorLevel)
                    value = stackBoxMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                stackBoxMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [StackBox] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int StackBoxMaximumColorLevel
        {
            get
            {
                return stackBoxMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= stackBoxMinimumColorLevel)
                    value = stackBoxMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                stackBoxMaximumColorLevel = value;
            }
        }
    }
}
