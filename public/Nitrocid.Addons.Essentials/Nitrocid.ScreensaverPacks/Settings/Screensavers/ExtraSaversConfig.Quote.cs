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
        private bool quoteTrueColor = true;
        private int quoteDelay = 10000;
        private int quoteMinimumRedColorLevel = 0;
        private int quoteMinimumGreenColorLevel = 0;
        private int quoteMinimumBlueColorLevel = 0;
        private int quoteMinimumColorLevel = 0;
        private int quoteMaximumRedColorLevel = 255;
        private int quoteMaximumGreenColorLevel = 255;
        private int quoteMaximumBlueColorLevel = 255;
        private int quoteMaximumColorLevel = 255;

        /// <summary>
        /// [Quote] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool QuoteTrueColor
        {
            get
            {
                return quoteTrueColor;
            }
            set
            {
                quoteTrueColor = value;
            }
        }
        /// <summary>
        /// [Quote] How many milliseconds to wait before making the next write?
        /// </summary>
        public int QuoteDelay
        {
            get
            {
                return quoteDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10000;
                quoteDelay = value;
            }
        }
        /// <summary>
        /// [Quote] The minimum red color level (true color)
        /// </summary>
        public int QuoteMinimumRedColorLevel
        {
            get
            {
                return quoteMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                quoteMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Quote] The minimum green color level (true color)
        /// </summary>
        public int QuoteMinimumGreenColorLevel
        {
            get
            {
                return quoteMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                quoteMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Quote] The minimum blue color level (true color)
        /// </summary>
        public int QuoteMinimumBlueColorLevel
        {
            get
            {
                return quoteMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                quoteMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Quote] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int QuoteMinimumColorLevel
        {
            get
            {
                return quoteMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                quoteMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Quote] The maximum red color level (true color)
        /// </summary>
        public int QuoteMaximumRedColorLevel
        {
            get
            {
                return quoteMaximumRedColorLevel;
            }
            set
            {
                if (value <= quoteMinimumRedColorLevel)
                    value = quoteMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                quoteMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Quote] The maximum green color level (true color)
        /// </summary>
        public int QuoteMaximumGreenColorLevel
        {
            get
            {
                return quoteMaximumGreenColorLevel;
            }
            set
            {
                if (value <= quoteMinimumGreenColorLevel)
                    value = quoteMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                quoteMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Quote] The maximum blue color level (true color)
        /// </summary>
        public int QuoteMaximumBlueColorLevel
        {
            get
            {
                return quoteMaximumBlueColorLevel;
            }
            set
            {
                if (value <= quoteMinimumBlueColorLevel)
                    value = quoteMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                quoteMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Quote] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int QuoteMaximumColorLevel
        {
            get
            {
                return quoteMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= quoteMinimumColorLevel)
                    value = quoteMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                quoteMaximumColorLevel = value;
            }
        }
    }
}
