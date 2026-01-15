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

namespace Nitrocid.Base.Kernel.Configuration.Instances
{
    /// <summary>
    /// Widgets kernel configuration instance
    /// </summary>
    public partial class KernelWidgetsConfig : BaseKernelConfig
    {
        private bool digitalTrueColor = true;
        private int digitalDelay = 1000;
        private int digitalMinimumRedColorLevel = 0;
        private int digitalMinimumGreenColorLevel = 0;
        private int digitalMinimumBlueColorLevel = 0;
        private int digitalMinimumColorLevel = 0;
        private int digitalMaximumRedColorLevel = 255;
        private int digitalMaximumGreenColorLevel = 255;
        private int digitalMaximumBlueColorLevel = 255;
        private int digitalMaximumColorLevel = 255;
        private bool digitalDisplayDate = true;

        /// <summary>
        /// [Digital] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool DigitalTrueColor
        {
            get
            {
                return digitalTrueColor;
            }
            set
            {
                digitalTrueColor = value;
            }
        }
        /// <summary>
        /// [Digital] How many milliseconds to wait before making the next write?
        /// </summary>
        public int DigitalDelay
        {
            get
            {
                return digitalDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                digitalDelay = value;
            }
        }
        /// <summary>
        /// [Digital] Displays the date in the digital clock.
        /// </summary>
        public bool DigitalDisplayDate
        {
            get => digitalDisplayDate;
            set => digitalDisplayDate = value;
        }
        /// <summary>
        /// [Digital] The minimum red color level (true color)
        /// </summary>
        public int DigitalMinimumRedColorLevel
        {
            get
            {
                return digitalMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                digitalMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Digital] The minimum green color level (true color)
        /// </summary>
        public int DigitalMinimumGreenColorLevel
        {
            get
            {
                return digitalMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                digitalMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Digital] The minimum blue color level (true color)
        /// </summary>
        public int DigitalMinimumBlueColorLevel
        {
            get
            {
                return digitalMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                digitalMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Digital] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int DigitalMinimumColorLevel
        {
            get
            {
                return digitalMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                digitalMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Digital] The maximum red color level (true color)
        /// </summary>
        public int DigitalMaximumRedColorLevel
        {
            get
            {
                return digitalMaximumRedColorLevel;
            }
            set
            {
                if (value <= digitalMinimumRedColorLevel)
                    value = digitalMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                digitalMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Digital] The maximum green color level (true color)
        /// </summary>
        public int DigitalMaximumGreenColorLevel
        {
            get
            {
                return digitalMaximumGreenColorLevel;
            }
            set
            {
                if (value <= digitalMinimumGreenColorLevel)
                    value = digitalMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                digitalMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Digital] The maximum blue color level (true color)
        /// </summary>
        public int DigitalMaximumBlueColorLevel
        {
            get
            {
                return digitalMaximumBlueColorLevel;
            }
            set
            {
                if (value <= digitalMinimumBlueColorLevel)
                    value = digitalMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                digitalMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Digital] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int DigitalMaximumColorLevel
        {
            get
            {
                return digitalMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= digitalMinimumColorLevel)
                    value = digitalMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                digitalMaximumColorLevel = value;
            }
        }
    }
}
