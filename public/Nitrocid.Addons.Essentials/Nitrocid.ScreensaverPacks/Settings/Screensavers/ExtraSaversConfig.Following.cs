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
        private bool followingTrueColor = true;
        private int followingDelay = 100;
        private int followingMinimumRedColorLevel = 0;
        private int followingMinimumGreenColorLevel = 0;
        private int followingMinimumBlueColorLevel = 0;
        private int followingMinimumColorLevel = 0;
        private int followingMaximumRedColorLevel = 255;
        private int followingMaximumGreenColorLevel = 255;
        private int followingMaximumBlueColorLevel = 255;
        private int followingMaximumColorLevel = 255;

        /// <summary>
        /// [Following] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool FollowingTrueColor
        {
            get
            {
                return followingTrueColor;
            }
            set
            {
                followingTrueColor = value;
            }
        }
        /// <summary>
        /// [Following] How many milliseconds to wait before making the next write?
        /// </summary>
        public int FollowingDelay
        {
            get
            {
                return followingDelay;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                followingDelay = value;
            }
        }
        /// <summary>
        /// [Following] The minimum red color level (true color)
        /// </summary>
        public int FollowingMinimumRedColorLevel
        {
            get
            {
                return followingMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                followingMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Following] The minimum green color level (true color)
        /// </summary>
        public int FollowingMinimumGreenColorLevel
        {
            get
            {
                return followingMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                followingMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Following] The minimum blue color level (true color)
        /// </summary>
        public int FollowingMinimumBlueColorLevel
        {
            get
            {
                return followingMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                followingMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Following] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int FollowingMinimumColorLevel
        {
            get
            {
                return followingMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                followingMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Following] The maximum red color level (true color)
        /// </summary>
        public int FollowingMaximumRedColorLevel
        {
            get
            {
                return followingMaximumRedColorLevel;
            }
            set
            {
                if (value <= followingMaximumRedColorLevel)
                    value = followingMaximumRedColorLevel;
                if (value > 255)
                    value = 255;
                followingMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Following] The maximum green color level (true color)
        /// </summary>
        public int FollowingMaximumGreenColorLevel
        {
            get
            {
                return followingMaximumGreenColorLevel;
            }
            set
            {
                if (value <= followingMaximumGreenColorLevel)
                    value = followingMaximumGreenColorLevel;
                if (value > 255)
                    value = 255;
                followingMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Following] The maximum blue color level (true color)
        /// </summary>
        public int FollowingMaximumBlueColorLevel
        {
            get
            {
                return followingMaximumBlueColorLevel;
            }
            set
            {
                if (value <= followingMaximumBlueColorLevel)
                    value = followingMaximumBlueColorLevel;
                if (value > 255)
                    value = 255;
                followingMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Following] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int FollowingMaximumColorLevel
        {
            get
            {
                return followingMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= followingMaximumColorLevel)
                    value = followingMaximumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                followingMaximumColorLevel = value;
            }
        }
    }
}
