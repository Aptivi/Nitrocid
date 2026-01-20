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
using Terminaux.Colors;
using Terminaux.Colors.Data;

namespace Nitrocid.ScreensaverPacks.Settings
{
    /// <summary>
    /// Screensaver kernel configuration instance
    /// </summary>
    public partial class ExtraSaversConfig : BaseKernelConfig
    {
        private bool danceNumbersTrueColor = true;
        private int danceNumbersDelay = 50;
        private string danceNumbersBackgroundColor = new Color(ConsoleColors.Black).PlainSequence;
        private int danceNumbersMinimumRedColorLevel = 0;
        private int danceNumbersMinimumGreenColorLevel = 0;
        private int danceNumbersMinimumBlueColorLevel = 0;
        private int danceNumbersMinimumColorLevel = 0;
        private int danceNumbersMaximumRedColorLevel = 255;
        private int danceNumbersMaximumGreenColorLevel = 255;
        private int danceNumbersMaximumBlueColorLevel = 255;
        private int danceNumbersMaximumColorLevel = 255;

        /// <summary>
        /// [DanceNumbers] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool DanceNumbersTrueColor
        {
            get
            {
                return danceNumbersTrueColor;
            }
            set
            {
                danceNumbersTrueColor = value;
            }
        }
        /// <summary>
        /// [DanceNumbers] How many milliseconds to wait before making the next write?
        /// </summary>
        public int DanceNumbersDelay
        {
            get
            {
                return danceNumbersDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                danceNumbersDelay = value;
            }
        }
        /// <summary>
        /// [DanceNumbers] Screensaver background color
        /// </summary>
        public string DanceNumbersBackgroundColor
        {
            get
            {
                return danceNumbersBackgroundColor;
            }
            set
            {
                danceNumbersBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [DanceNumbers] The minimum red color level (true color)
        /// </summary>
        public int DanceNumbersMinimumRedColorLevel
        {
            get
            {
                return danceNumbersMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                danceNumbersMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [DanceNumbers] The minimum green color level (true color)
        /// </summary>
        public int DanceNumbersMinimumGreenColorLevel
        {
            get
            {
                return danceNumbersMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                danceNumbersMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [DanceNumbers] The minimum blue color level (true color)
        /// </summary>
        public int DanceNumbersMinimumBlueColorLevel
        {
            get
            {
                return danceNumbersMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                danceNumbersMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [DanceNumbers] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int DanceNumbersMinimumColorLevel
        {
            get
            {
                return danceNumbersMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                danceNumbersMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [DanceNumbers] The maximum red color level (true color)
        /// </summary>
        public int DanceNumbersMaximumRedColorLevel
        {
            get
            {
                return danceNumbersMaximumRedColorLevel;
            }
            set
            {
                if (value <= danceNumbersMinimumRedColorLevel)
                    value = danceNumbersMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                danceNumbersMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [DanceNumbers] The maximum green color level (true color)
        /// </summary>
        public int DanceNumbersMaximumGreenColorLevel
        {
            get
            {
                return danceNumbersMaximumGreenColorLevel;
            }
            set
            {
                if (value <= danceNumbersMinimumGreenColorLevel)
                    value = danceNumbersMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                danceNumbersMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [DanceNumbers] The maximum blue color level (true color)
        /// </summary>
        public int DanceNumbersMaximumBlueColorLevel
        {
            get
            {
                return danceNumbersMaximumBlueColorLevel;
            }
            set
            {
                if (value <= danceNumbersMinimumBlueColorLevel)
                    value = danceNumbersMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                danceNumbersMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [DanceNumbers] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int DanceNumbersMaximumColorLevel
        {
            get
            {
                return danceNumbersMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= danceNumbersMinimumColorLevel)
                    value = danceNumbersMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                danceNumbersMaximumColorLevel = value;
            }
        }
    }
}
