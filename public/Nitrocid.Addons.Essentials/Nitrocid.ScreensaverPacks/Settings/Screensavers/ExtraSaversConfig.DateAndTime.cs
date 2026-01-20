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
        private bool dateAndTimeTrueColor = true;
        private int dateAndTimeDelay = 1000;
        private int dateAndTimeMinimumRedColorLevel = 0;
        private int dateAndTimeMinimumGreenColorLevel = 0;
        private int dateAndTimeMinimumBlueColorLevel = 0;
        private int dateAndTimeMinimumColorLevel = 0;
        private int dateAndTimeMaximumRedColorLevel = 255;
        private int dateAndTimeMaximumGreenColorLevel = 255;
        private int dateAndTimeMaximumBlueColorLevel = 255;
        private int dateAndTimeMaximumColorLevel = 255;

        /// <summary>
        /// [DateAndTime] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool DateAndTimeTrueColor
        {
            get
            {
                return dateAndTimeTrueColor;
            }
            set
            {
                dateAndTimeTrueColor = value;
            }
        }
        /// <summary>
        /// [DateAndTime] How many milliseconds to wait before making the next write?
        /// </summary>
        public int DateAndTimeDelay
        {
            get
            {
                return dateAndTimeDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                dateAndTimeDelay = value;
            }
        }
        /// <summary>
        /// [DateAndTime] The minimum red color level (true color)
        /// </summary>
        public int DateAndTimeMinimumRedColorLevel
        {
            get
            {
                return dateAndTimeMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                dateAndTimeMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [DateAndTime] The minimum green color level (true color)
        /// </summary>
        public int DateAndTimeMinimumGreenColorLevel
        {
            get
            {
                return dateAndTimeMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                dateAndTimeMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [DateAndTime] The minimum blue color level (true color)
        /// </summary>
        public int DateAndTimeMinimumBlueColorLevel
        {
            get
            {
                return dateAndTimeMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                dateAndTimeMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [DateAndTime] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int DateAndTimeMinimumColorLevel
        {
            get
            {
                return dateAndTimeMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                dateAndTimeMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [DateAndTime] The maximum red color level (true color)
        /// </summary>
        public int DateAndTimeMaximumRedColorLevel
        {
            get
            {
                return dateAndTimeMaximumRedColorLevel;
            }
            set
            {
                if (value <= dateAndTimeMinimumRedColorLevel)
                    value = dateAndTimeMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                dateAndTimeMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [DateAndTime] The maximum green color level (true color)
        /// </summary>
        public int DateAndTimeMaximumGreenColorLevel
        {
            get
            {
                return dateAndTimeMaximumGreenColorLevel;
            }
            set
            {
                if (value <= dateAndTimeMinimumGreenColorLevel)
                    value = dateAndTimeMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                dateAndTimeMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [DateAndTime] The maximum blue color level (true color)
        /// </summary>
        public int DateAndTimeMaximumBlueColorLevel
        {
            get
            {
                return dateAndTimeMaximumBlueColorLevel;
            }
            set
            {
                if (value <= dateAndTimeMinimumBlueColorLevel)
                    value = dateAndTimeMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                dateAndTimeMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [DateAndTime] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int DateAndTimeMaximumColorLevel
        {
            get
            {
                return dateAndTimeMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= dateAndTimeMinimumColorLevel)
                    value = dateAndTimeMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                dateAndTimeMaximumColorLevel = value;
            }
        }
    }
}
