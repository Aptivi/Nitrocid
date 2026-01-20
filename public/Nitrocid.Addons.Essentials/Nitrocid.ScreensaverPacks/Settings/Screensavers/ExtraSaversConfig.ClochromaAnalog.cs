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
        private bool clochromaAnalogTrueColor = true;
        private int clochromaAnalogDelay = 1000;
        private bool clochromaAnalogShowSecondsHand = true;
        private int clochromaAnalogMinimumRedColorLevel = 0;
        private int clochromaAnalogMinimumGreenColorLevel = 0;
        private int clochromaAnalogMinimumBlueColorLevel = 0;
        private int clochromaAnalogMinimumColorLevel = 0;
        private int clochromaAnalogMaximumRedColorLevel = 255;
        private int clochromaAnalogMaximumGreenColorLevel = 255;
        private int clochromaAnalogMaximumBlueColorLevel = 255;
        private int clochromaAnalogMaximumColorLevel = 255;
        private bool clochromaAnalogBright = false;

        /// <summary>
        /// [ClochromaAnalog] Whether to use bright or dark version.
        /// </summary>
        public bool ClochromaAnalogBright
        {
            get
            {
                return clochromaAnalogBright;
            }
            set
            {
                clochromaAnalogBright = value;
            }
        }
        /// <summary>
        /// [ClochromaAnalog] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool ClochromaAnalogTrueColor
        {
            get
            {
                return clochromaAnalogTrueColor;
            }
            set
            {
                clochromaAnalogTrueColor = value;
            }
        }
        /// <summary>
        /// [ClochromaAnalog] How many milliseconds to wait before making the next write?
        /// </summary>
        public int ClochromaAnalogDelay
        {
            get
            {
                return clochromaAnalogDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                clochromaAnalogDelay = value;
            }
        }
        /// <summary>
        /// [ClochromaAnalog] Shows the seconds hand.
        /// </summary>
        public bool ClochromaAnalogShowSecondsHand
        {
            get
            {
                return clochromaAnalogShowSecondsHand;
            }
            set
            {
                clochromaAnalogShowSecondsHand = value;
            }
        }
        /// <summary>
        /// [ClochromaAnalog] The minimum red color level (true color)
        /// </summary>
        public int ClochromaAnalogMinimumRedColorLevel
        {
            get
            {
                return clochromaAnalogMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                clochromaAnalogMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ClochromaAnalog] The minimum green color level (true color)
        /// </summary>
        public int ClochromaAnalogMinimumGreenColorLevel
        {
            get
            {
                return clochromaAnalogMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                clochromaAnalogMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ClochromaAnalog] The minimum blue color level (true color)
        /// </summary>
        public int ClochromaAnalogMinimumBlueColorLevel
        {
            get
            {
                return clochromaAnalogMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                clochromaAnalogMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ClochromaAnalog] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int ClochromaAnalogMinimumColorLevel
        {
            get
            {
                return clochromaAnalogMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                clochromaAnalogMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [ClochromaAnalog] The maximum red color level (true color)
        /// </summary>
        public int ClochromaAnalogMaximumRedColorLevel
        {
            get
            {
                return clochromaAnalogMaximumRedColorLevel;
            }
            set
            {
                if (value <= clochromaAnalogMinimumRedColorLevel)
                    value = clochromaAnalogMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                clochromaAnalogMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ClochromaAnalog] The maximum green color level (true color)
        /// </summary>
        public int ClochromaAnalogMaximumGreenColorLevel
        {
            get
            {
                return clochromaAnalogMaximumGreenColorLevel;
            }
            set
            {
                if (value <= clochromaAnalogMinimumGreenColorLevel)
                    value = clochromaAnalogMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                clochromaAnalogMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ClochromaAnalog] The maximum blue color level (true color)
        /// </summary>
        public int ClochromaAnalogMaximumBlueColorLevel
        {
            get
            {
                return clochromaAnalogMaximumBlueColorLevel;
            }
            set
            {
                if (value <= clochromaAnalogMinimumBlueColorLevel)
                    value = clochromaAnalogMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                clochromaAnalogMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ClochromaAnalog] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int ClochromaAnalogMaximumColorLevel
        {
            get
            {
                return clochromaAnalogMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= clochromaAnalogMinimumColorLevel)
                    value = clochromaAnalogMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                clochromaAnalogMaximumColorLevel = value;
            }
        }
    }
}
