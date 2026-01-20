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
        private int mesmerizeDelay = 10;
        private int mesmerizeMinimumRedColorLevel = 0;
        private int mesmerizeMinimumGreenColorLevel = 0;
        private int mesmerizeMinimumBlueColorLevel = 0;
        private int mesmerizeMinimumColorLevel = 0;
        private int mesmerizeMaximumRedColorLevel = 255;
        private int mesmerizeMaximumGreenColorLevel = 255;
        private int mesmerizeMaximumBlueColorLevel = 255;
        private int mesmerizeMaximumColorLevel = 255;

        /// <summary>
        /// [Mesmerize] How many milliseconds to wait before making the next write?
        /// </summary>
        public int MesmerizeDelay
        {
            get
            {
                return mesmerizeDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                mesmerizeDelay = value;
            }
        }
        /// <summary>
        /// [Mesmerize] The minimum red color level (true color)
        /// </summary>
        public int MesmerizeMinimumRedColorLevel
        {
            get
            {
                return mesmerizeMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                mesmerizeMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Mesmerize] The minimum green color level (true color)
        /// </summary>
        public int MesmerizeMinimumGreenColorLevel
        {
            get
            {
                return mesmerizeMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                mesmerizeMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Mesmerize] The minimum blue color level (true color)
        /// </summary>
        public int MesmerizeMinimumBlueColorLevel
        {
            get
            {
                return mesmerizeMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                mesmerizeMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Mesmerize] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int MesmerizeMinimumColorLevel
        {
            get
            {
                return mesmerizeMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                mesmerizeMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Mesmerize] The maximum red color level (true color)
        /// </summary>
        public int MesmerizeMaximumRedColorLevel
        {
            get
            {
                return mesmerizeMaximumRedColorLevel;
            }
            set
            {
                if (value <= mesmerizeMaximumRedColorLevel)
                    value = mesmerizeMaximumRedColorLevel;
                if (value > 255)
                    value = 255;
                mesmerizeMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Mesmerize] The maximum green color level (true color)
        /// </summary>
        public int MesmerizeMaximumGreenColorLevel
        {
            get
            {
                return mesmerizeMaximumGreenColorLevel;
            }
            set
            {
                if (value <= mesmerizeMaximumGreenColorLevel)
                    value = mesmerizeMaximumGreenColorLevel;
                if (value > 255)
                    value = 255;
                mesmerizeMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Mesmerize] The maximum blue color level (true color)
        /// </summary>
        public int MesmerizeMaximumBlueColorLevel
        {
            get
            {
                return mesmerizeMaximumBlueColorLevel;
            }
            set
            {
                if (value <= mesmerizeMaximumBlueColorLevel)
                    value = mesmerizeMaximumBlueColorLevel;
                if (value > 255)
                    value = 255;
                mesmerizeMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Mesmerize] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int MesmerizeMaximumColorLevel
        {
            get
            {
                return mesmerizeMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= mesmerizeMaximumColorLevel)
                    value = mesmerizeMaximumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                mesmerizeMaximumColorLevel = value;
            }
        }
    }
}
