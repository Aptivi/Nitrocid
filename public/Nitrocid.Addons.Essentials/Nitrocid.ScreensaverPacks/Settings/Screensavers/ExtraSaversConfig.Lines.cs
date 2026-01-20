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
        private bool linesTrueColor = true;
        private int linesDelay = 500;
        private string linesLineChar = "-";
        private string linesBackgroundColor = new Color(ConsoleColors.Black).PlainSequence;
        private int linesMinimumRedColorLevel = 0;
        private int linesMinimumGreenColorLevel = 0;
        private int linesMinimumBlueColorLevel = 0;
        private int linesMinimumColorLevel = 0;
        private int linesMaximumRedColorLevel = 255;
        private int linesMaximumGreenColorLevel = 255;
        private int linesMaximumBlueColorLevel = 255;
        private int linesMaximumColorLevel = 255;

        /// <summary>
        /// [Lines] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool LinesTrueColor
        {
            get
            {
                return linesTrueColor;
            }
            set
            {
                linesTrueColor = value;
            }
        }

        /// <summary>
        /// [Lines] How many milliseconds to wait before making the next write?
        /// </summary>
        public int LinesDelay
        {
            get
            {
                return linesDelay;
            }
            set
            {
                if (value <= 0)
                    value = 500;
                linesDelay = value;
            }
        }

        /// <summary>
        /// [Lines] Line character
        /// </summary>
        public string LinesLineChar
        {
            get
            {
                return linesLineChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "-";
                linesLineChar = value;
            }
        }

        /// <summary>
        /// [Lines] Screensaver background color
        /// </summary>
        public string LinesBackgroundColor
        {
            get
            {
                return linesBackgroundColor;
            }
            set
            {
                linesBackgroundColor = new Color(value).PlainSequence;
            }
        }

        /// <summary>
        /// [Lines] The minimum red color level (true color)
        /// </summary>
        public int LinesMinimumRedColorLevel
        {
            get
            {
                return linesMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                linesMinimumRedColorLevel = value;
            }
        }

        /// <summary>
        /// [Lines] The minimum green color level (true color)
        /// </summary>
        public int LinesMinimumGreenColorLevel
        {
            get
            {
                return linesMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                linesMinimumGreenColorLevel = value;
            }
        }

        /// <summary>
        /// [Lines] The minimum blue color level (true color)
        /// </summary>
        public int LinesMinimumBlueColorLevel
        {
            get
            {
                return linesMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                linesMinimumBlueColorLevel = value;
            }
        }

        /// <summary>
        /// [Lines] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int LinesMinimumColorLevel
        {
            get
            {
                return linesMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                linesMinimumColorLevel = value;
            }
        }

        /// <summary>
        /// [Lines] The maximum red color level (true color)
        /// </summary>
        public int LinesMaximumRedColorLevel
        {
            get
            {
                return linesMaximumRedColorLevel;
            }
            set
            {
                if (value <= linesMinimumRedColorLevel)
                    value = linesMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                linesMaximumRedColorLevel = value;
            }
        }

        /// <summary>
        /// [Lines] The maximum green color level (true color)
        /// </summary>
        public int LinesMaximumGreenColorLevel
        {
            get
            {
                return linesMaximumGreenColorLevel;
            }
            set
            {
                if (value <= linesMinimumGreenColorLevel)
                    value = linesMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                linesMaximumGreenColorLevel = value;
            }
        }

        /// <summary>
        /// [Lines] The maximum blue color level (true color)
        /// </summary>
        public int LinesMaximumBlueColorLevel
        {
            get
            {
                return linesMaximumBlueColorLevel;
            }
            set
            {
                if (value <= linesMinimumBlueColorLevel)
                    value = linesMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                linesMaximumBlueColorLevel = value;
            }
        }

        /// <summary>
        /// [Lines] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int LinesMaximumColorLevel
        {
            get
            {
                return linesMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= linesMinimumColorLevel)
                    value = linesMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                linesMaximumColorLevel = value;
            }
        }
    }
}
