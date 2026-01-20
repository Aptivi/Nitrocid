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
        private int noiseNewScreenDelay = 5000;
        private int noiseDensity = 40;

        /// <summary>
        /// [Noise] How many milliseconds to wait before making the new screen?
        /// </summary>
        public int NoiseNewScreenDelay
        {
            get
            {
                return noiseNewScreenDelay;
            }
            set
            {
                if (value <= 0)
                    value = 5000;
                noiseNewScreenDelay = value;
            }
        }
        /// <summary>
        /// [Noise] The noise density in percent
        /// </summary>
        public int NoiseDensity
        {
            get
            {
                return noiseDensity;
            }
            set
            {
                if (value < 0)
                    value = 40;
                if (value > 100)
                    value = 40;
                noiseDensity = value;
            }
        }
    }
}
