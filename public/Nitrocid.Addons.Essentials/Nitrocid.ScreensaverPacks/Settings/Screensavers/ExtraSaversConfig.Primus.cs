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
        private bool primusTrueColor = true;
        private int primusDelay = 20;
        private int primusMinimumRedColorLevel = 0;
        private int primusMinimumGreenColorLevel = 0;
        private int primusMinimumBlueColorLevel = 0;
        private int primusMinimumColorLevel = 0;
        private int primusMaximumRedColorLevel = 255;
        private int primusMaximumGreenColorLevel = 255;
        private int primusMaximumBlueColorLevel = 255;
        private int primusMaximumColorLevel = 255;

        /// <summary>
        /// [Primus] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool PrimusTrueColor
        {
            get
            {
                return primusTrueColor;
            }
            set
            {
                primusTrueColor = value;
            }
        }
        /// <summary>
        /// [Primus] How many milliseconds to wait before making the next write?
        /// </summary>
        public int PrimusDelay
        {
            get
            {
                return primusDelay;
            }
            set
            {
                if (value <= 0)
                    value = 20;
                primusDelay = value;
            }
        }
        /// <summary>
        /// [Primus] The minimum red color level (true color)
        /// </summary>
        public int PrimusMinimumRedColorLevel
        {
            get
            {
                return primusMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                primusMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Primus] The minimum green color level (true color)
        /// </summary>
        public int PrimusMinimumGreenColorLevel
        {
            get
            {
                return primusMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                primusMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Primus] The minimum blue color level (true color)
        /// </summary>
        public int PrimusMinimumBlueColorLevel
        {
            get
            {
                return primusMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                primusMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Primus] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int PrimusMinimumColorLevel
        {
            get
            {
                return primusMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                primusMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Primus] The maximum red color level (true color)
        /// </summary>
        public int PrimusMaximumRedColorLevel
        {
            get
            {
                return primusMaximumRedColorLevel;
            }
            set
            {
                if (value <= primusMinimumRedColorLevel)
                    value = primusMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                primusMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Primus] The maximum green color level (true color)
        /// </summary>
        public int PrimusMaximumGreenColorLevel
        {
            get
            {
                return primusMaximumGreenColorLevel;
            }
            set
            {
                if (value <= primusMinimumGreenColorLevel)
                    value = primusMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                primusMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Primus] The maximum blue color level (true color)
        /// </summary>
        public int PrimusMaximumBlueColorLevel
        {
            get
            {
                return primusMaximumBlueColorLevel;
            }
            set
            {
                if (value <= primusMinimumBlueColorLevel)
                    value = primusMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                primusMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Primus] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int PrimusMaximumColorLevel
        {
            get
            {
                return primusMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= primusMinimumColorLevel)
                    value = primusMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                primusMaximumColorLevel = value;
            }
        }
    }
}
