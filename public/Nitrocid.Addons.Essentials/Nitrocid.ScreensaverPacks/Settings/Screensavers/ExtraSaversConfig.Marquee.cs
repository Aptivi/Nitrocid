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
        private bool marqueeTrueColor = true;
        private int marqueeDelay = 10;
        private string marqueeWrite = "Nitrocid KS";
        private bool marqueeAlwaysCentered = true;
        private bool marqueeUseConsoleAPI;
        private string marqueeBackgroundColor = new Color(ConsoleColors.Black).PlainSequence;
        private int marqueeMinimumRedColorLevel = 0;
        private int marqueeMinimumGreenColorLevel = 0;
        private int marqueeMinimumBlueColorLevel = 0;
        private int marqueeMinimumColorLevel = 0;
        private int marqueeMaximumRedColorLevel = 255;
        private int marqueeMaximumGreenColorLevel = 255;
        private int marqueeMaximumBlueColorLevel = 255;
        private int marqueeMaximumColorLevel = 255;

        /// <summary>
        /// [Marquee] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool MarqueeTrueColor
        {
            get
            {
                return marqueeTrueColor;
            }
            set
            {
                marqueeTrueColor = value;
            }
        }
        /// <summary>
        /// [Marquee] How many milliseconds to wait before making the next write?
        /// </summary>
        public int MarqueeDelay
        {
            get
            {
                return marqueeDelay;
            }
            set
            {
                marqueeDelay = value;
            }
        }
        /// <summary>
        /// [Marquee] Text for Marquee. Shorter is better.
        /// </summary>
        public string MarqueeWrite
        {
            get
            {
                return marqueeWrite;
            }
            set
            {
                marqueeWrite = value;
            }
        }
        /// <summary>
        /// [Marquee] Whether the text is always on center.
        /// </summary>
        public bool MarqueeAlwaysCentered
        {
            get
            {
                return marqueeAlwaysCentered;
            }
            set
            {
                marqueeAlwaysCentered = value;
            }
        }
        /// <summary>
        /// [Marquee] Whether to use the KS.ConsoleBase.ConsoleWrapper.Clear() API (slow) or use the line-clearing VT sequence (fast).
        /// </summary>
        public bool MarqueeUseConsoleAPI
        {
            get
            {
                return marqueeUseConsoleAPI;
            }
            set
            {
                marqueeUseConsoleAPI = value;
            }
        }
        /// <summary>
        /// [Marquee] Screensaver background color
        /// </summary>
        public string MarqueeBackgroundColor
        {
            get
            {
                return marqueeBackgroundColor;
            }
            set
            {
                marqueeBackgroundColor = value;
            }
        }
        /// <summary>
        /// [Marquee] The minimum red color level (true color)
        /// </summary>
        public int MarqueeMinimumRedColorLevel
        {
            get
            {
                return marqueeMinimumRedColorLevel;
            }
            set
            {
                marqueeMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Marquee] The minimum green color level (true color)
        /// </summary>
        public int MarqueeMinimumGreenColorLevel
        {
            get
            {
                return marqueeMinimumGreenColorLevel;
            }
            set
            {
                marqueeMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Marquee] The minimum blue color level (true color)
        /// </summary>
        public int MarqueeMinimumBlueColorLevel
        {
            get
            {
                return marqueeMinimumBlueColorLevel;
            }
            set
            {
                marqueeMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Marquee] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int MarqueeMinimumColorLevel
        {
            get
            {
                return marqueeMinimumColorLevel;
            }
            set
            {
                marqueeMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Marquee] The maximum red color level (true color)
        /// </summary>
        public int MarqueeMaximumRedColorLevel
        {
            get
            {
                return marqueeMaximumRedColorLevel;
            }
            set
            {
                marqueeMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Marquee] The maximum green color level (true color)
        /// </summary>
        public int MarqueeMaximumGreenColorLevel
        {
            get
            {
                return marqueeMaximumGreenColorLevel;
            }
            set
            {
                marqueeMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Marquee] The maximum blue color level (true color)
        /// </summary>
        public int MarqueeMaximumBlueColorLevel
        {
            get
            {
                return marqueeMaximumBlueColorLevel;
            }
            set
            {
                marqueeMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Marquee] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int MarqueeMaximumColorLevel
        {
            get
            {
                return marqueeMaximumColorLevel;
            }
            set
            {
                marqueeMaximumColorLevel = value;
            }
        }
    }
}
