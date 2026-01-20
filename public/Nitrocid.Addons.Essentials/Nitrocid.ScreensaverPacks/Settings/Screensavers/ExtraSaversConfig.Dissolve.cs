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
        private bool dissolveTrueColor = true;
        private string dissolveBackgroundColor = new Color(ConsoleColors.Black).PlainSequence;
        private int dissolveMinimumRedColorLevel = 0;
        private int dissolveMinimumGreenColorLevel = 0;
        private int dissolveMinimumBlueColorLevel = 0;
        private int dissolveMinimumColorLevel = 0;
        private int dissolveMaximumRedColorLevel = 255;
        private int dissolveMaximumGreenColorLevel = 255;
        private int dissolveMaximumBlueColorLevel = 255;
        private int dissolveMaximumColorLevel = 255;

        /// <summary>
        /// [Dissolve] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool DissolveTrueColor
        {
            get
            {
                return dissolveTrueColor;
            }
            set
            {
                dissolveTrueColor = value;
            }
        }
        /// <summary>
        /// [Dissolve] Screensaver background color
        /// </summary>
        public string DissolveBackgroundColor
        {
            get
            {
                return dissolveBackgroundColor;
            }
            set
            {
                dissolveBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [Dissolve] The minimum red color level (true color)
        /// </summary>
        public int DissolveMinimumRedColorLevel
        {
            get
            {
                return dissolveMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                dissolveMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Dissolve] The minimum green color level (true color)
        /// </summary>
        public int DissolveMinimumGreenColorLevel
        {
            get
            {
                return dissolveMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                dissolveMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Dissolve] The minimum blue color level (true color)
        /// </summary>
        public int DissolveMinimumBlueColorLevel
        {
            get
            {
                return dissolveMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                dissolveMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Dissolve] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int DissolveMinimumColorLevel
        {
            get
            {
                return dissolveMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                dissolveMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Dissolve] The maximum red color level (true color)
        /// </summary>
        public int DissolveMaximumRedColorLevel
        {
            get
            {
                return dissolveMaximumRedColorLevel;
            }
            set
            {
                if (value <= dissolveMinimumRedColorLevel)
                    value = dissolveMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                dissolveMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Dissolve] The maximum green color level (true color)
        /// </summary>
        public int DissolveMaximumGreenColorLevel
        {
            get
            {
                return dissolveMaximumGreenColorLevel;
            }
            set
            {
                if (value <= dissolveMinimumGreenColorLevel)
                    value = dissolveMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                dissolveMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Dissolve] The maximum blue color level (true color)
        /// </summary>
        public int DissolveMaximumBlueColorLevel
        {
            get
            {
                return dissolveMaximumBlueColorLevel;
            }
            set
            {
                if (value <= dissolveMinimumBlueColorLevel)
                    value = dissolveMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                dissolveMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Dissolve] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int DissolveMaximumColorLevel
        {
            get
            {
                return dissolveMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= dissolveMinimumColorLevel)
                    value = dissolveMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                dissolveMaximumColorLevel = value;
            }
        }
    }
}
