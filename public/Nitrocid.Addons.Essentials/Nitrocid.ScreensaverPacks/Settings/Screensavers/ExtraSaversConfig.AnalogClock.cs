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

using Colorimetry.Data;
using Nitrocid.Base.Kernel.Configuration.Instances;

namespace Nitrocid.ScreensaverPacks.Settings
{
    /// <summary>
    /// Screensaver kernel configuration instance
    /// </summary>
    public partial class ExtraSaversConfig : BaseKernelConfig
    {
        private bool analogClockTrueColor = true;
        private int analogClockDelay = 1000;
        private bool analogClockShowSecondsHand = true;
        private int analogClockMinimumRedColorLevel = 0;
        private int analogClockMinimumGreenColorLevel = 0;
        private int analogClockMinimumBlueColorLevel = 0;
        private int analogClockMinimumColorLevel = 0;
        private int analogClockMaximumRedColorLevel = 255;
        private int analogClockMaximumGreenColorLevel = 255;
        private int analogClockMaximumBlueColorLevel = 255;
        private int analogClockMaximumColorLevel = 255;
        private bool analogClockCycleColors = false;
        private string analogClockTimeInfoColor = ConsoleColors.Silver.ToString();
        private string analogClockBezelColor = ConsoleColors.Yellow.ToString();
        private string analogClockHandsColor = ConsoleColors.Grey.ToString();
        private string analogClockSecondsHandColor = ConsoleColors.Silver.ToString();

        /// <summary>
        /// [AnalogClock] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool AnalogClockTrueColor
        {
            get
            {
                return analogClockTrueColor;
            }
            set
            {
                analogClockTrueColor = value;
            }
        }
        /// <summary>
        /// [AnalogClock] How many milliseconds to wait before making the next write?
        /// </summary>
        public int AnalogClockDelay
        {
            get
            {
                return analogClockDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                analogClockDelay = value;
            }
        }
        /// <summary>
        /// [AnalogClock] Shows the seconds hand.
        /// </summary>
        public bool AnalogClockShowSecondsHand
        {
            get
            {
                return analogClockShowSecondsHand;
            }
            set
            {
                analogClockShowSecondsHand = value;
            }
        }
        /// <summary>
        /// [AnalogClock] Cycles colors
        /// </summary>
        // TODO: NKS_SCREENSAVERPACKS_COMMON_SETTINGS_CYCLECOLORS_DESC -> Automatically chooses a random set of colors
        public bool AnalogClockCycleColors
        {
            get => analogClockCycleColors;
            set => analogClockCycleColors = value;
        }
        /// <summary>
        /// [AnalogClock] Color of the time information text
        /// </summary>
        // TODO: NKS_SCREENSAVERPACKS_ANALOGCLOCK_SETTINGS_CLOCKTIMEINFOCOLOR_NAME -> Color of time info text
        // TODO: NKS_SCREENSAVERPACKS_ANALOGCLOCK_SETTINGS_CLOCKTIMEINFOCOLOR_DESC -> Chooses a color of time information
        public string AnalogClockTimeInfoColor
        {
            get => analogClockTimeInfoColor;
            set => analogClockTimeInfoColor = value;
        }
        // TODO: NKS_SCREENSAVERPACKS_ANALOGCLOCK_SETTINGS_CLOCKBEZELCOLOR_NAME -> Color of bezel
        // TODO: NKS_SCREENSAVERPACKS_ANALOGCLOCK_SETTINGS_CLOCKBEZELCOLOR_DESC -> Chooses a color of the analog clock's bezel
        /// <summary>
        /// [AnalogClock] Color of the bezel
        /// </summary>
        public string AnalogClockBezelColor
        {
            get => analogClockBezelColor;
            set => analogClockBezelColor = value;
        }
        // TODO: NKS_SCREENSAVERPACKS_ANALOGCLOCK_SETTINGS_CLOCKHANDSCOLOR_NAME -> Color of hands
        // TODO: NKS_SCREENSAVERPACKS_ANALOGCLOCK_SETTINGS_CLOCKHANDSCOLOR_DESC -> Chooses a color of the analog clock's hands
        /// <summary>
        /// [AnalogClock] Color of the hands
        /// </summary>
        public string AnalogClockHandsColor
        {
            get => analogClockHandsColor;
            set => analogClockHandsColor = value;
        }
        // TODO: NKS_SCREENSAVERPACKS_ANALOGCLOCK_SETTINGS_CLOCKSECONDSHANDSCOLOR_NAME -> Color of the seconds hand
        // TODO: NKS_SCREENSAVERPACKS_ANALOGCLOCK_SETTINGS_CLOCKSECONDSHANDSCOLOR_DESC -> Chooses a color of the analog clock's seconds hand
        /// <summary>
        /// [AnalogClock] Color of the seconds hand
        /// </summary>
        public string AnalogClockSecondsHandColor
        {
            get => analogClockSecondsHandColor;
            set => analogClockSecondsHandColor = value;
        }
        /// <summary>
        /// [AnalogClock] The minimum red color level (true color)
        /// </summary>
        public int AnalogClockMinimumRedColorLevel
        {
            get
            {
                return analogClockMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                analogClockMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [AnalogClock] The minimum green color level (true color)
        /// </summary>
        public int AnalogClockMinimumGreenColorLevel
        {
            get
            {
                return analogClockMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                analogClockMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [AnalogClock] The minimum blue color level (true color)
        /// </summary>
        public int AnalogClockMinimumBlueColorLevel
        {
            get
            {
                return analogClockMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                analogClockMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [AnalogClock] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int AnalogClockMinimumColorLevel
        {
            get
            {
                return analogClockMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                analogClockMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [AnalogClock] The maximum red color level (true color)
        /// </summary>
        public int AnalogClockMaximumRedColorLevel
        {
            get
            {
                return analogClockMaximumRedColorLevel;
            }
            set
            {
                if (value <= analogClockMinimumRedColorLevel)
                    value = analogClockMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                analogClockMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [AnalogClock] The maximum green color level (true color)
        /// </summary>
        public int AnalogClockMaximumGreenColorLevel
        {
            get
            {
                return analogClockMaximumGreenColorLevel;
            }
            set
            {
                if (value <= analogClockMinimumGreenColorLevel)
                    value = analogClockMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                analogClockMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [AnalogClock] The maximum blue color level (true color)
        /// </summary>
        public int AnalogClockMaximumBlueColorLevel
        {
            get
            {
                return analogClockMaximumBlueColorLevel;
            }
            set
            {
                if (value <= analogClockMinimumBlueColorLevel)
                    value = analogClockMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                analogClockMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [AnalogClock] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int AnalogClockMaximumColorLevel
        {
            get
            {
                return analogClockMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= analogClockMinimumColorLevel)
                    value = analogClockMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                analogClockMaximumColorLevel = value;
            }
        }
    }
}
