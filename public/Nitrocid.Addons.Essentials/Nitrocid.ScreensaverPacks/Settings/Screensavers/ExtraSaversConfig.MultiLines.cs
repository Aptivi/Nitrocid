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
        private bool multiLinesTrueColor = true;
        private int multiLinesDelay = 500;
        private string multiLinesLineChar = "-";
        private string multiLinesBackgroundColor = new Color(ConsoleColors.Black).PlainSequence;
        private int multiLinesMinimumRedColorLevel = 0;
        private int multiLinesMinimumGreenColorLevel = 0;
        private int multiLinesMinimumBlueColorLevel = 0;
        private int multiLinesMinimumColorLevel = 0;
        private int multiLinesMaximumRedColorLevel = 255;
        private int multiLinesMaximumGreenColorLevel = 255;
        private int multiLinesMaximumBlueColorLevel = 255;
        private int multiLinesMaximumColorLevel = 255;

        /// <summary>
        /// [MultiLines] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool MultiLinesTrueColor
        {
            get
            {
                return multiLinesTrueColor;
            }
            set
            {
                multiLinesTrueColor = value;
            }
        }
        /// <summary>
        /// [MultiLines] How many milliseconds to wait before making the next write?
        /// </summary>
        public int MultiLinesDelay
        {
            get
            {
                return multiLinesDelay;
            }
            set
            {
                if (value <= 0)
                    value = 500;
                multiLinesDelay = value;
            }
        }
        /// <summary>
        /// [MultiLines] Line character
        /// </summary>
        public string MultiLinesLineChar
        {
            get
            {
                return multiLinesLineChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "-";
                multiLinesLineChar = value;
            }
        }
        /// <summary>
        /// [MultiLines] Screensaver background color
        /// </summary>
        public string MultiLinesBackgroundColor
        {
            get
            {
                return multiLinesBackgroundColor;
            }
            set
            {
                multiLinesBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [MultiLines] The minimum red color level (true color)
        /// </summary>
        public int MultiLinesMinimumRedColorLevel
        {
            get
            {
                return multiLinesMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                multiLinesMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [MultiLines] The minimum green color level (true color)
        /// </summary>
        public int MultiLinesMinimumGreenColorLevel
        {
            get
            {
                return multiLinesMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                multiLinesMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [MultiLines] The minimum blue color level (true color)
        /// </summary>
        public int MultiLinesMinimumBlueColorLevel
        {
            get
            {
                return multiLinesMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                multiLinesMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [MultiLines] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int MultiLinesMinimumColorLevel
        {
            get
            {
                return multiLinesMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                multiLinesMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [MultiLines] The maximum red color level (true color)
        /// </summary>
        public int MultiLinesMaximumRedColorLevel
        {
            get
            {
                return multiLinesMaximumRedColorLevel;
            }
            set
            {
                if (value <= multiLinesMinimumRedColorLevel)
                    value = multiLinesMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                multiLinesMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [MultiLines] The maximum green color level (true color)
        /// </summary>
        public int MultiLinesMaximumGreenColorLevel
        {
            get
            {
                return multiLinesMaximumGreenColorLevel;
            }
            set
            {
                if (value <= multiLinesMinimumGreenColorLevel)
                    value = multiLinesMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                multiLinesMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [MultiLines] The maximum blue color level (true color)
        /// </summary>
        public int MultiLinesMaximumBlueColorLevel
        {
            get
            {
                return multiLinesMaximumBlueColorLevel;
            }
            set
            {
                if (value <= multiLinesMinimumBlueColorLevel)
                    value = multiLinesMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                multiLinesMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [MultiLines] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int MultiLinesMaximumColorLevel
        {
            get
            {
                return multiLinesMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= multiLinesMinimumColorLevel)
                    value = multiLinesMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                multiLinesMaximumColorLevel = value;
            }
        }
    }
}
