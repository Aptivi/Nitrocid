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
        private int gradientBloomDelay = 50;
        private bool gradientBloomDarkColors;
        private int gradientBloomSteps = 100;

        /// <summary>
        /// [GradientBloom] How many milliseconds to wait before making the next write?
        /// </summary>
        public int GradientBloomDelay
        {
            get
            {
                return gradientBloomDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                gradientBloomDelay = value;
            }
        }
        /// <summary>
        /// [GradientBloom] Whether to use dark colors or not
        /// </summary>
        public bool GradientBloomDarkColors
        {
            get
            {
                return gradientBloomDarkColors;
            }
            set
            {
                gradientBloomDarkColors = value;
            }
        }
        /// <summary>
        /// [GradientBloom] How many color steps for transitioning between two colors?
        /// </summary>
        public int GradientBloomSteps
        {
            get
            {
                return gradientBloomSteps;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                gradientBloomSteps = value;
            }
        }
    }
}
