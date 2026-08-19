//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
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
        private int hellDelay = 100;
        private int hellMaximumBackColorLevel = 32;

        /// <summary>
        /// [Hell] How many milliseconds to wait before making the next write?
        /// </summary>
        public int HellDelay
        {
            get
            {
                return hellDelay;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                hellDelay = value;
            }
        }
        /// <summary>
        /// [Hell] Maximum background color level to use
        /// </summary>
        public int HellMaximumBackColorLevel
        {
            get
            {
                return hellMaximumBackColorLevel;
            }
            set
            {
                if (value <= 0 || value > 255)
                    value = 32;
                hellMaximumBackColorLevel = value;
            }
        }
    }
}
