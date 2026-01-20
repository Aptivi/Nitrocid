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
        private int aberrationDelay = 100;
        private int aberrationProbability = 5;

        /// <summary>
        /// [Aberration] How many milliseconds to wait before making the next write?
        /// </summary>
        public int AberrationDelay
        {
            get
            {
                return aberrationDelay;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                aberrationDelay = value;
            }
        }
        /// <summary>
        /// [Aberration] Chance, in percent, for the screen glitch to occur
        /// </summary>
        public int AberrationProbability
        {
            get
            {
                return aberrationProbability;
            }
            set
            {
                if (value <= 0)
                    value = 5;
                aberrationProbability = value;
            }
        }
    }
}
