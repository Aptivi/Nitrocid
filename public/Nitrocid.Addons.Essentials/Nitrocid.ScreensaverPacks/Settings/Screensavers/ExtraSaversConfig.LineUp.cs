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
        private bool lineUpTrueColor = true;
        private int lineUpDelay = 500;
        private string lineUpLineChar = "-";
        private string lineUpBackgroundColor = new Color(ConsoleColors.Black).PlainSequence;
        private int lineUpMinimumRedColorLevel = 0;
        private int lineUpMinimumGreenColorLevel = 0;
        private int lineUpMinimumBlueColorLevel = 0;
        private int lineUpMinimumColorLevel = 0;
        private int lineUpMaximumRedColorLevel = 255;
        private int lineUpMaximumGreenColorLevel = 255;
        private int lineUpMaximumBlueColorLevel = 255;
        private int lineUpMaximumColorLevel = 255;

        /// <summary>
        /// [LineUp] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool LineUpTrueColor
        {
            get
            {
                return lineUpTrueColor;
            }
            set
            {
                lineUpTrueColor = value;
            }
        }

        /// <summary>
        /// [LineUp] How many milliseconds to wait before making the next write?
        /// </summary>
        public int LineUpDelay
        {
            get
            {
                return lineUpDelay;
            }
            set
            {
                if (value <= 0)
                    value = 500;
                lineUpDelay = value;
            }
        }

        /// <summary>
        /// [LineUp] Line character
        /// </summary>
        public string LineUpLineChar
        {
            get
            {
                return lineUpLineChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "-";
                lineUpLineChar = value;
            }
        }

        /// <summary>
        /// [LineUp] Screensaver background color
        /// </summary>
        public string LineUpBackgroundColor
        {
            get
            {
                return lineUpBackgroundColor;
            }
            set
            {
                lineUpBackgroundColor = new Color(value).PlainSequence;
            }
        }

        /// <summary>
        /// [LineUp] The minimum red color level (true color)
        /// </summary>
        public int LineUpMinimumRedColorLevel
        {
            get
            {
                return lineUpMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                lineUpMinimumRedColorLevel = value;
            }
        }

        /// <summary>
        /// [LineUp] The minimum green color level (true color)
        /// </summary>
        public int LineUpMinimumGreenColorLevel
        {
            get
            {
                return lineUpMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                lineUpMinimumGreenColorLevel = value;
            }
        }

        /// <summary>
        /// [LineUp] The minimum blue color level (true color)
        /// </summary>
        public int LineUpMinimumBlueColorLevel
        {
            get
            {
                return lineUpMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                lineUpMinimumBlueColorLevel = value;
            }
        }

        /// <summary>
        /// [LineUp] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int LineUpMinimumColorLevel
        {
            get
            {
                return lineUpMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                lineUpMinimumColorLevel = value;
            }
        }

        /// <summary>
        /// [LineUp] The maximum red color level (true color)
        /// </summary>
        public int LineUpMaximumRedColorLevel
        {
            get
            {
                return lineUpMaximumRedColorLevel;
            }
            set
            {
                if (value <= lineUpMinimumRedColorLevel)
                    value = lineUpMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                lineUpMaximumRedColorLevel = value;
            }
        }

        /// <summary>
        /// [LineUp] The maximum green color level (true color)
        /// </summary>
        public int LineUpMaximumGreenColorLevel
        {
            get
            {
                return lineUpMaximumGreenColorLevel;
            }
            set
            {
                if (value <= lineUpMinimumGreenColorLevel)
                    value = lineUpMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                lineUpMaximumGreenColorLevel = value;
            }
        }

        /// <summary>
        /// [LineUp] The maximum blue color level (true color)
        /// </summary>
        public int LineUpMaximumBlueColorLevel
        {
            get
            {
                return lineUpMaximumBlueColorLevel;
            }
            set
            {
                if (value <= lineUpMinimumBlueColorLevel)
                    value = lineUpMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                lineUpMaximumBlueColorLevel = value;
            }
        }

        /// <summary>
        /// [LineUp] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int LineUpMaximumColorLevel
        {
            get
            {
                return lineUpMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= lineUpMinimumColorLevel)
                    value = lineUpMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                lineUpMaximumColorLevel = value;
            }
        }
    }
}
