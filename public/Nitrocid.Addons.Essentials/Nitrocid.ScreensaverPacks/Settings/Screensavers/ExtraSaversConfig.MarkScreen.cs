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
        private int markScreenDelay = 175;
        private int markScreenMinimumGrade = 4000;
        private int markScreenMaximumGrade = 10000;

        /// <summary>
        /// [MarkScreen] How many milliseconds to wait before making the next write?
        /// </summary>
        public int MarkScreenDelay
        {
            get
            {
                return markScreenDelay;
            }
            set
            {
                if (value <= 0)
                    value = 175;
                markScreenDelay = value;
            }
        }

        /// <summary>
        /// [MarkScreen] Minimum exam grade for subjects for a student
        /// </summary>
        public int MarkScreenMinimumGrade
        {
            get
            {
                return markScreenMinimumGrade;
            }
            set
            {
                if (value <= 0)
                    value = 4000;
                if (value >= 10000)
                    value = 10000;
                markScreenMinimumGrade = value;
            }
        }

        /// <summary>
        /// [MarkScreen] Maximum exam grade for subjects for a student
        /// </summary>
        public int MarkScreenMaximumGrade
        {
            get
            {
                return markScreenMaximumGrade;
            }
            set
            {
                if (value <= 0)
                    value = 4000;
                if (value >= 10000)
                    value = 10000;
                markScreenMaximumGrade = value;
            }
        }
    }
}
