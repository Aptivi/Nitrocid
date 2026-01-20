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
        private bool commitMilestoneTrueColor = true;
        private int commitMilestoneDelay = 1000;
        private bool commitMilestoneRainbowMode;
        private int commitMilestoneMinimumRedColorLevel = 0;
        private int commitMilestoneMinimumGreenColorLevel = 0;
        private int commitMilestoneMinimumBlueColorLevel = 0;
        private int commitMilestoneMinimumColorLevel = 0;
        private int commitMilestoneMaximumRedColorLevel = 255;
        private int commitMilestoneMaximumGreenColorLevel = 255;
        private int commitMilestoneMaximumBlueColorLevel = 255;
        private int commitMilestoneMaximumColorLevel = 255;

        /// <summary>
        /// [CommitMilestone] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool CommitMilestoneTrueColor
        {
            get
            {
                return commitMilestoneTrueColor;
            }
            set
            {
                commitMilestoneTrueColor = value;
            }
        }
        /// <summary>
        /// [CommitMilestone] How many milliseconds to wait before making the next write?
        /// </summary>
        public int CommitMilestoneDelay
        {
            get
            {
                return commitMilestoneDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                commitMilestoneDelay = value;
            }
        }
        /// <summary>
        /// [CommitMilestone] Enables the rainbow colors mode
        /// </summary>
        public bool CommitMilestoneRainbowMode
        {
            get
            {
                return commitMilestoneRainbowMode;
            }
            set
            {
                commitMilestoneRainbowMode = value;
            }
        }
        /// <summary>
        /// [CommitMilestone] The minimum red color level (true color)
        /// </summary>
        public int CommitMilestoneMinimumRedColorLevel
        {
            get
            {
                return commitMilestoneMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                commitMilestoneMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [CommitMilestone] The minimum green color level (true color)
        /// </summary>
        public int CommitMilestoneMinimumGreenColorLevel
        {
            get
            {
                return commitMilestoneMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                commitMilestoneMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [CommitMilestone] The minimum blue color level (true color)
        /// </summary>
        public int CommitMilestoneMinimumBlueColorLevel
        {
            get
            {
                return commitMilestoneMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                commitMilestoneMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [CommitMilestone] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int CommitMilestoneMinimumColorLevel
        {
            get
            {
                return commitMilestoneMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                commitMilestoneMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [CommitMilestone] The maximum red color level (true color)
        /// </summary>
        public int CommitMilestoneMaximumRedColorLevel
        {
            get
            {
                return commitMilestoneMaximumRedColorLevel;
            }
            set
            {
                if (value <= commitMilestoneMinimumRedColorLevel)
                    value = commitMilestoneMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                commitMilestoneMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [CommitMilestone] The maximum green color level (true color)
        /// </summary>
        public int CommitMilestoneMaximumGreenColorLevel
        {
            get
            {
                return commitMilestoneMaximumGreenColorLevel;
            }
            set
            {
                if (value <= commitMilestoneMinimumGreenColorLevel)
                    value = commitMilestoneMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                commitMilestoneMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [CommitMilestone] The maximum blue color level (true color)
        /// </summary>
        public int CommitMilestoneMaximumBlueColorLevel
        {
            get
            {
                return commitMilestoneMaximumBlueColorLevel;
            }
            set
            {
                if (value <= commitMilestoneMinimumBlueColorLevel)
                    value = commitMilestoneMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                commitMilestoneMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [CommitMilestone] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int CommitMilestoneMaximumColorLevel
        {
            get
            {
                return commitMilestoneMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= commitMilestoneMinimumColorLevel)
                    value = commitMilestoneMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                commitMilestoneMaximumColorLevel = value;
            }
        }
    }
}
