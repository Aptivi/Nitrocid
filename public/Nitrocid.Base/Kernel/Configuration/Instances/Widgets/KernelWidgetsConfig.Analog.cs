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

namespace Nitrocid.Base.Kernel.Configuration.Instances
{
    /// <summary>
    /// Widgets kernel configuration instance
    /// </summary>
    public partial class KernelWidgetsConfig : BaseKernelConfig
    {
        private bool analogShowSecondsHand = true;
        private bool analogTrueColor = true;
        private int analogDelay = 1000;
        private int analogMinimumRedColorLevel = 0;
        private int analogMinimumGreenColorLevel = 0;
        private int analogMinimumBlueColorLevel = 0;
        private int analogMinimumColorLevel = 0;
        private int analogMaximumRedColorLevel = 255;
        private int analogMaximumGreenColorLevel = 255;
        private int analogMaximumBlueColorLevel = 255;
        private int analogMaximumColorLevel = 255;
        private bool analogCycleColors = false;
        private string analogTimeInfoColor = ConsoleColors.Silver.ToString();
        private string analogBezelColor = ConsoleColors.Yellow.ToString();
        private string analogHandsColor = ConsoleColors.Grey.ToString();
        private string analogSecondsHandColor = ConsoleColors.Silver.ToString();

        /// <summary>
        /// [Analog] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool AnalogTrueColor
        {
            get
            {
                return analogTrueColor;
            }
            set
            {
                analogTrueColor = value;
            }
        }
        /// <summary>
        /// [Analog] How many milliseconds to wait before making the next write?
        /// </summary>
        public int AnalogDelay
        {
            get
            {
                return analogDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                analogDelay = value;
            }
        }
        /// <summary>
        /// [Analog] Shows the seconds hand.
        /// </summary>
        public bool AnalogShowSecondsHand
        {
            get
            {
                return analogShowSecondsHand;
            }
            set
            {
                analogShowSecondsHand = value;
            }
        }
        /// <summary>
        /// [Analog] Cycles colors
        /// </summary>
        // TODO: NKS_SETTINGS_COMMON_CYCLECOLORS_NAME -> Cycle colors
        // TODO: NKS_SETTINGS_COMMON_CYCLECOLORS_DESC -> Automatically chooses a random set of colors
        public bool AnalogCycleColors
        {
            get => analogCycleColors;
            set => analogCycleColors = value;
        }
        /// <summary>
        /// [Analog] Color of the time information text
        /// </summary>
        // TODO: NKS_SETTINGS_COMMON_CLOCKTIMEINFOCOLOR_NAME -> Color of time info text
        // TODO: NKS_SETTINGS_COMMON_CLOCKTIMEINFOCOLOR_DESC -> Chooses a color of time information
        public string AnalogTimeInfoColor
        {
            get => analogTimeInfoColor;
            set => analogTimeInfoColor = value;
        }
        /// <summary>
        /// [Analog] Color of the bezel
        /// </summary>
        // TODO: NKS_SETTINGS_COMMON_CLOCKBEZELCOLOR_NAME -> Color of bezel
        // TODO: NKS_SETTINGS_COMMON_CLOCKBEZELCOLOR_DESC -> Chooses a color of the analog clock's bezel
        public string AnalogBezelColor
        {
            get => analogBezelColor;
            set => analogBezelColor = value;
        }
        /// <summary>
        /// [Analog] Color of the hands
        /// </summary>
        // TODO: NKS_SETTINGS_COMMON_CLOCKHANDSCOLOR_NAME -> Color of hands
        // TODO: NKS_SETTINGS_COMMON_CLOCKHANDSCOLOR_DESC -> Chooses a color of the analog clock's hands
        public string AnalogHandsColor
        {
            get => analogHandsColor;
            set => analogHandsColor = value;
        }
        /// <summary>
        /// [Analog] Color of the seconds hand
        /// </summary>
        // TODO: NKS_SETTINGS_COMMON_CLOCKSECONDSHANDSCOLOR_NAME -> Color of the seconds hand
        // TODO: NKS_SETTINGS_COMMON_CLOCKSECONDSHANDSCOLOR_DESC -> Chooses a color of the analog clock's seconds hand
        public string AnalogSecondsHandColor
        {
            get => analogSecondsHandColor;
            set => analogSecondsHandColor = value;
        }
        /// <summary>
        /// [Analog] The minimum red color level (true color)
        /// </summary>
        public int AnalogMinimumRedColorLevel
        {
            get
            {
                return analogMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                analogMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Analog] The minimum green color level (true color)
        /// </summary>
        public int AnalogMinimumGreenColorLevel
        {
            get
            {
                return analogMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                analogMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Analog] The minimum blue color level (true color)
        /// </summary>
        public int AnalogMinimumBlueColorLevel
        {
            get
            {
                return analogMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                analogMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Analog] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int AnalogMinimumColorLevel
        {
            get
            {
                return analogMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                analogMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Analog] The maximum red color level (true color)
        /// </summary>
        public int AnalogMaximumRedColorLevel
        {
            get
            {
                return analogMaximumRedColorLevel;
            }
            set
            {
                if (value <= analogMinimumRedColorLevel)
                    value = analogMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                analogMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Analog] The maximum green color level (true color)
        /// </summary>
        public int AnalogMaximumGreenColorLevel
        {
            get
            {
                return analogMaximumGreenColorLevel;
            }
            set
            {
                if (value <= analogMinimumGreenColorLevel)
                    value = analogMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                analogMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Analog] The maximum blue color level (true color)
        /// </summary>
        public int AnalogMaximumBlueColorLevel
        {
            get
            {
                return analogMaximumBlueColorLevel;
            }
            set
            {
                if (value <= analogMinimumBlueColorLevel)
                    value = analogMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                analogMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Analog] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int AnalogMaximumColorLevel
        {
            get
            {
                return analogMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= analogMinimumColorLevel)
                    value = analogMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                analogMaximumColorLevel = value;
            }
        }
    }
}
