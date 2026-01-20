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
using Textify.Data.Figlet;

namespace Nitrocid.ScreensaverPacks.Settings
{
    /// <summary>
    /// Screensaver kernel configuration instance
    /// </summary>
    public partial class ExtraSaversConfig : BaseKernelConfig
    {
        private bool figletTrueColor = true;
        private int figletDelay = 1000;
        private string figletText = "Nitrocid KS";
        private string figletFont = "small";
        private bool figletRainbowMode;
        private int figletMinimumRedColorLevel = 0;
        private int figletMinimumGreenColorLevel = 0;
        private int figletMinimumBlueColorLevel = 0;
        private int figletMinimumColorLevel = 0;
        private int figletMaximumRedColorLevel = 255;
        private int figletMaximumGreenColorLevel = 255;
        private int figletMaximumBlueColorLevel = 255;
        private int figletMaximumColorLevel = 255;

        /// <summary>
        /// [Figlet] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool FigletTrueColor
        {
            get
            {
                return figletTrueColor;
            }
            set
            {
                figletTrueColor = value;
            }
        }
        /// <summary>
        /// [Figlet] How many milliseconds to wait before making the next write?
        /// </summary>
        public int FigletDelay
        {
            get
            {
                return figletDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                figletDelay = value;
            }
        }
        /// <summary>
        /// [Figlet] Text for Figlet. Shorter is better.
        /// </summary>
        public string FigletText
        {
            get
            {
                return figletText;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Nitrocid KS";
                figletText = value;
            }
        }
        /// <summary>
        /// [Figlet] Figlet font supported by the figlet library used.
        /// </summary>
        public string FigletFont
        {
            get
            {
                return figletFont;
            }
            set
            {
                figletFont = FigletTools.GetFigletFonts().ContainsKey(value) ? value : "small";
            }
        }
        /// <summary>
        /// [Figlet] Enables the rainbow colors mode
        /// </summary>
        public bool FigletRainbowMode
        {
            get
            {
                return figletRainbowMode;
            }
            set
            {
                figletRainbowMode = value;
            }
        }
        /// <summary>
        /// [Figlet] The minimum red color level (true color)
        /// </summary>
        public int FigletMinimumRedColorLevel
        {
            get
            {
                return figletMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                figletMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Figlet] The minimum green color level (true color)
        /// </summary>
        public int FigletMinimumGreenColorLevel
        {
            get
            {
                return figletMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                figletMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Figlet] The minimum blue color level (true color)
        /// </summary>
        public int FigletMinimumBlueColorLevel
        {
            get
            {
                return figletMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                figletMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Figlet] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int FigletMinimumColorLevel
        {
            get
            {
                return figletMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                figletMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Figlet] The maximum red color level (true color)
        /// </summary>
        public int FigletMaximumRedColorLevel
        {
            get
            {
                return figletMaximumRedColorLevel;
            }
            set
            {
                if (value <= figletMinimumRedColorLevel)
                    value = figletMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                figletMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Figlet] The maximum green color level (true color)
        /// </summary>
        public int FigletMaximumGreenColorLevel
        {
            get
            {
                return figletMaximumGreenColorLevel;
            }
            set
            {
                if (value <= figletMinimumGreenColorLevel)
                    value = figletMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                figletMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Figlet] The maximum blue color level (true color)
        /// </summary>
        public int FigletMaximumBlueColorLevel
        {
            get
            {
                return figletMaximumBlueColorLevel;
            }
            set
            {
                if (value <= figletMinimumBlueColorLevel)
                    value = figletMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                figletMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Figlet] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int FigletMaximumColorLevel
        {
            get
            {
                return figletMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= figletMinimumColorLevel)
                    value = figletMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                figletMaximumColorLevel = value;
            }
        }
    }
}
