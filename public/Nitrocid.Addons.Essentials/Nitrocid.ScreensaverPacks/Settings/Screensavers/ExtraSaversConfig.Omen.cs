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
        private int omenDelay = 100;
        private string omenWrite = "Nitrocid KS";
        private int omenMaximumBackColorLevel = 32;
        private int omenMaximumLineColorLevel = 64;
        private int omenMaximumTextColorLevel = 128;

        /// <summary>
        /// [Omen] How many milliseconds to wait before making the next write?
        /// </summary>
        public int OmenDelay
        {
            get
            {
                return omenDelay;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                omenDelay = value;
            }
        }
        /// <summary>
        /// [Omen] Text for Omen. Shorter is better.
        /// </summary>
        public string OmenWrite
        {
            get
            {
                return omenWrite;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Nitrocid KS";
                omenWrite = value;
            }
        }
        /// <summary>
        /// [Omen] Maximum background color level to use
        /// </summary>
        public int OmenMaximumBackColorLevel
        {
            get
            {
                return omenMaximumBackColorLevel;
            }
            set
            {
                if (value <= 0 || value > 255)
                    value = 32;
                omenMaximumBackColorLevel = value;
            }
        }
        /// <summary>
        /// [Omen] Maximum line color level to use
        /// </summary>
        public int OmenMaximumLineColorLevel
        {
            get
            {
                return omenMaximumLineColorLevel;
            }
            set
            {
                if (value <= 0 || value > 255)
                    value = 64;
                omenMaximumLineColorLevel = value;
            }
        }
        /// <summary>
        /// [Omen] Maximum text color level to use
        /// </summary>
        public int OmenMaximumTextColorLevel
        {
            get
            {
                return omenMaximumTextColorLevel;
            }
            set
            {
                if (value <= 0 || value > 255)
                    value = 64;
                omenMaximumTextColorLevel = value;
            }
        }
    }
}
