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
        private bool doorShiftTrueColor = true;
        private int doorShiftDelay = 10;
        private string doorShiftBackgroundColor = new Color(ConsoleColors.Black).PlainSequence;
        private int doorShiftMinimumRedColorLevel = 0;
        private int doorShiftMinimumGreenColorLevel = 0;
        private int doorShiftMinimumBlueColorLevel = 0;
        private int doorShiftMinimumColorLevel = 0;
        private int doorShiftMaximumRedColorLevel = 255;
        private int doorShiftMaximumGreenColorLevel = 255;
        private int doorShiftMaximumBlueColorLevel = 255;
        private int doorShiftMaximumColorLevel = 255;

        /// <summary>
        /// [DoorShift] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool DoorShiftTrueColor
        {
            get
            {
                return doorShiftTrueColor;
            }
            set
            {
                doorShiftTrueColor = value;
            }
        }
        /// <summary>
        /// [DoorShift] How many milliseconds to wait before making the next write?
        /// </summary>
        public int DoorShiftDelay
        {
            get
            {
                return doorShiftDelay;
            }
            set
            {
                doorShiftDelay = value;
            }
        }
        /// <summary>
        /// [DoorShift] Screensaver background color
        /// </summary>
        public string DoorShiftBackgroundColor
        {
            get
            {
                return doorShiftBackgroundColor;
            }
            set
            {
                doorShiftBackgroundColor = value;
            }
        }
        /// <summary>
        /// [DoorShift] The minimum red color level (true color)
        /// </summary>
        public int DoorShiftMinimumRedColorLevel
        {
            get
            {
                return doorShiftMinimumRedColorLevel;
            }
            set
            {
                doorShiftMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [DoorShift] The minimum green color level (true color)
        /// </summary>
        public int DoorShiftMinimumGreenColorLevel
        {
            get
            {
                return doorShiftMinimumGreenColorLevel;
            }
            set
            {
                doorShiftMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [DoorShift] The minimum blue color level (true color)
        /// </summary>
        public int DoorShiftMinimumBlueColorLevel
        {
            get
            {
                return doorShiftMinimumBlueColorLevel;
            }
            set
            {
                doorShiftMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [DoorShift] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int DoorShiftMinimumColorLevel
        {
            get
            {
                return doorShiftMinimumColorLevel;
            }
            set
            {
                doorShiftMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [DoorShift] The maximum red color level (true color)
        /// </summary>
        public int DoorShiftMaximumRedColorLevel
        {
            get
            {
                return doorShiftMaximumRedColorLevel;
            }
            set
            {
                doorShiftMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [DoorShift] The maximum green color level (true color)
        /// </summary>
        public int DoorShiftMaximumGreenColorLevel
        {
            get
            {
                return doorShiftMaximumGreenColorLevel;
            }
            set
            {
                doorShiftMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [DoorShift] The maximum blue color level (true color)
        /// </summary>
        public int DoorShiftMaximumBlueColorLevel
        {
            get
            {
                return doorShiftMaximumBlueColorLevel;
            }
            set
            {
                doorShiftMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [DoorShift] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int DoorShiftMaximumColorLevel
        {
            get
            {
                return doorShiftMaximumColorLevel;
            }
            set
            {
                doorShiftMaximumColorLevel = value;
            }
        }
    }
}
