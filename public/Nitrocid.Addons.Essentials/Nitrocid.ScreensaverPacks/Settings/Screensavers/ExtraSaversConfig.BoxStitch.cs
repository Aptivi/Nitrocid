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
        private int boxStitchDelay = 5000;
        private int boxStitchLineDelay = 100;
        private int boxStitchMinimumRedColorLevel = 0;
        private int boxStitchMinimumGreenColorLevel = 0;
        private int boxStitchMinimumBlueColorLevel = 0;
        private int boxStitchMinimumColorLevel = 0;
        private int boxStitchMaximumRedColorLevel = 255;
        private int boxStitchMaximumGreenColorLevel = 255;
        private int boxStitchMaximumBlueColorLevel = 255;
        private int boxStitchMaximumColorLevel = 255;

        /// <summary>
        /// [BoxStitch] How many milliseconds to wait before making the next write?
        /// </summary>
        public int BoxStitchDelay
        {
            get
            {
                return boxStitchDelay;
            }
            set
            {
                if (value <= 0)
                    value = 5000;
                boxStitchDelay = value;
            }
        }
        /// <summary>
        /// [BoxStitch] How many milliseconds to wait before adding a new line?
        /// </summary>
        public int BoxStitchLineDelay
        {
            get
            {
                return boxStitchLineDelay;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                boxStitchLineDelay = value;
            }
        }
        /// <summary>
        /// [BoxStitch] The minimum red color level (true color)
        /// </summary>
        public int BoxStitchMinimumRedColorLevel
        {
            get
            {
                return boxStitchMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                boxStitchMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BoxStitch] The minimum green color level (true color)
        /// </summary>
        public int BoxStitchMinimumGreenColorLevel
        {
            get
            {
                return boxStitchMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                boxStitchMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BoxStitch] The minimum blue color level (true color)
        /// </summary>
        public int BoxStitchMinimumBlueColorLevel
        {
            get
            {
                return boxStitchMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                boxStitchMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BoxStitch] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int BoxStitchMinimumColorLevel
        {
            get
            {
                return boxStitchMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                boxStitchMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [BoxStitch] The maximum red color level (true color)
        /// </summary>
        public int BoxStitchMaximumRedColorLevel
        {
            get
            {
                return boxStitchMaximumRedColorLevel;
            }
            set
            {
                if (value <= boxStitchMinimumRedColorLevel)
                    value = boxStitchMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                boxStitchMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BoxStitch] The maximum green color level (true color)
        /// </summary>
        public int BoxStitchMaximumGreenColorLevel
        {
            get
            {
                return boxStitchMaximumGreenColorLevel;
            }
            set
            {
                if (value <= boxStitchMinimumGreenColorLevel)
                    value = boxStitchMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                boxStitchMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BoxStitch] The maximum blue color level (true color)
        /// </summary>
        public int BoxStitchMaximumBlueColorLevel
        {
            get
            {
                return boxStitchMaximumBlueColorLevel;
            }
            set
            {
                if (value <= boxStitchMinimumBlueColorLevel)
                    value = boxStitchMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                boxStitchMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BoxStitch] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int BoxStitchMaximumColorLevel
        {
            get
            {
                return boxStitchMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= boxStitchMinimumColorLevel)
                    value = boxStitchMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                boxStitchMaximumColorLevel = value;
            }
        }
    }
}
