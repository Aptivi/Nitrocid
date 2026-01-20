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
        private bool zebraShiftTrueColor = true;
        private int zebraShiftDelay = 25;
        private int zebraShiftMinimumRedColorLevel = 0;
        private int zebraShiftMinimumGreenColorLevel = 0;
        private int zebraShiftMinimumBlueColorLevel = 0;
        private int zebraShiftMinimumColorLevel = 0;
        private int zebraShiftMaximumRedColorLevel = 255;
        private int zebraShiftMaximumGreenColorLevel = 255;
        private int zebraShiftMaximumBlueColorLevel = 255;
        private int zebraShiftMaximumColorLevel = 255;

        /// <summary>
        /// [ZebraShift] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool ZebraShiftTrueColor
        {
            get
            {
                return zebraShiftTrueColor;
            }
            set
            {
                zebraShiftTrueColor = value;
            }
        }
        /// <summary>
        /// [ZebraShift] How many milliseconds to wait before making the next write?
        /// </summary>
        public int ZebraShiftDelay
        {
            get
            {
                return zebraShiftDelay;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                zebraShiftDelay = value;
            }
        }
        /// <summary>
        /// [ZebraShift] The minimum red color level (true color)
        /// </summary>
        public int ZebraShiftMinimumRedColorLevel
        {
            get
            {
                return zebraShiftMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                zebraShiftMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ZebraShift] The minimum green color level (true color)
        /// </summary>
        public int ZebraShiftMinimumGreenColorLevel
        {
            get
            {
                return zebraShiftMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                zebraShiftMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ZebraShift] The minimum blue color level (true color)
        /// </summary>
        public int ZebraShiftMinimumBlueColorLevel
        {
            get
            {
                return zebraShiftMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                zebraShiftMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ZebraShift] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int ZebraShiftMinimumColorLevel
        {
            get
            {
                return zebraShiftMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                zebraShiftMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [ZebraShift] The maximum red color level (true color)
        /// </summary>
        public int ZebraShiftMaximumRedColorLevel
        {
            get
            {
                return zebraShiftMaximumRedColorLevel;
            }
            set
            {
                if (value <= zebraShiftMinimumRedColorLevel)
                    value = zebraShiftMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                zebraShiftMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ZebraShift] The maximum green color level (true color)
        /// </summary>
        public int ZebraShiftMaximumGreenColorLevel
        {
            get
            {
                return zebraShiftMaximumGreenColorLevel;
            }
            set
            {
                if (value <= zebraShiftMinimumGreenColorLevel)
                    value = zebraShiftMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                zebraShiftMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ZebraShift] The maximum blue color level (true color)
        /// </summary>
        public int ZebraShiftMaximumBlueColorLevel
        {
            get
            {
                return zebraShiftMaximumBlueColorLevel;
            }
            set
            {
                if (value <= zebraShiftMinimumBlueColorLevel)
                    value = zebraShiftMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                zebraShiftMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ZebraShift] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int ZebraShiftMaximumColorLevel
        {
            get
            {
                return zebraShiftMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= zebraShiftMinimumColorLevel)
                    value = zebraShiftMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                zebraShiftMaximumColorLevel = value;
            }
        }
    }
}
