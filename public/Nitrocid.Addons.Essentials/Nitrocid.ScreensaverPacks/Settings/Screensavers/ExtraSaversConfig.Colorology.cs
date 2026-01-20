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
using Nitrocid.ScreensaverPacks.Screensavers.Utilities;

namespace Nitrocid.ScreensaverPacks.Settings
{
    /// <summary>
    /// Screensaver kernel configuration instance
    /// </summary>
    public partial class ExtraSaversConfig : BaseKernelConfig
    {
        private int colorologyDelay = 50;
        private bool colorologyDarkColors;
        private int colorologySteps = 100;
        private ColorTransitionMethod colorologyMethod = ColorTransitionMethod.Rgb;

        /// <summary>
        /// [Colorology] How many milliseconds to wait before making the next write?
        /// </summary>
        public int ColorologyDelay
        {
            get
            {
                return colorologyDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                colorologyDelay = value;
            }
        }
        /// <summary>
        /// [Colorology] Whether to use dark colors or not
        /// </summary>
        public bool ColorologyDarkColors
        {
            get
            {
                return colorologyDarkColors;
            }
            set
            {
                colorologyDarkColors = value;
            }
        }
        /// <summary>
        /// [Colorology] How many color steps for transitioning between two colors?
        /// </summary>
        public int ColorologySteps
        {
            get
            {
                return colorologySteps;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                colorologySteps = value;
            }
        }
        /// <summary>
        /// [Colorology] How many color steps for transitioning between two colors?
        /// </summary>
        public ColorTransitionMethod ColorologyMethod
        {
            get
            {
                return colorologyMethod;
            }
            set
            {
                if (value < 0 || value > ColorTransitionMethod.PingPong)
                    value = ColorTransitionMethod.Rgb;
                colorologyMethod = value;
            }
        }
    }
}
