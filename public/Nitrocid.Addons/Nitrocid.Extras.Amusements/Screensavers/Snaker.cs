
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.Kernel.Configuration;
using KS.Kernel.Threading;
using KS.Misc.Screensaver;
using Nitrocid.Extras.Amusements.Amusements.Games;

namespace Nitrocid.Extras.Amusements.Screensavers
{
    /// <summary>
    /// Settings for Snaker
    /// </summary>
    public static class SnakerSettings
    {

        /// <summary>
        /// [Snaker] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool SnakerTrueColor
        {
            get
            {
                return Config.SaverConfig.SnakerTrueColor;
            }
            set
            {
                Config.SaverConfig.SnakerTrueColor = value;
            }
        }
        /// <summary>
        /// [Snaker] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int SnakerDelay
        {
            get
            {
                return Config.SaverConfig.SnakerDelay;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                Config.SaverConfig.SnakerDelay = value;
            }
        }
        /// <summary>
        /// [Snaker] How many milliseconds to wait before making the next stage?
        /// </summary>
        public static int SnakerStageDelay
        {
            get
            {
                return Config.SaverConfig.SnakerStageDelay;
            }
            set
            {
                if (value <= 0)
                    value = 5000;
                Config.SaverConfig.SnakerStageDelay = value;
            }
        }
        /// <summary>
        /// [Snaker] The minimum red color level (true color)
        /// </summary>
        public static int SnakerMinimumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.SnakerMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.SnakerMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Snaker] The minimum green color level (true color)
        /// </summary>
        public static int SnakerMinimumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.SnakerMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.SnakerMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Snaker] The minimum blue color level (true color)
        /// </summary>
        public static int SnakerMinimumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.SnakerMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.SnakerMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Snaker] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int SnakerMinimumColorLevel
        {
            get
            {
                return Config.SaverConfig.SnakerMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                Config.SaverConfig.SnakerMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Snaker] The maximum red color level (true color)
        /// </summary>
        public static int SnakerMaximumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.SnakerMaximumRedColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.SnakerMinimumRedColorLevel)
                    value = Config.SaverConfig.SnakerMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.SnakerMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Snaker] The maximum green color level (true color)
        /// </summary>
        public static int SnakerMaximumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.SnakerMaximumGreenColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.SnakerMinimumGreenColorLevel)
                    value = Config.SaverConfig.SnakerMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.SnakerMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Snaker] The maximum blue color level (true color)
        /// </summary>
        public static int SnakerMaximumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.SnakerMaximumBlueColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.SnakerMinimumBlueColorLevel)
                    value = Config.SaverConfig.SnakerMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.SnakerMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Snaker] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int SnakerMaximumColorLevel
        {
            get
            {
                return Config.SaverConfig.SnakerMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= Config.SaverConfig.SnakerMinimumColorLevel)
                    value = Config.SaverConfig.SnakerMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                Config.SaverConfig.SnakerMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for Snaker
    /// </summary>
    public class SnakerDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Snaker";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            Snaker.InitializeSnaker(true);
            ThreadManager.SleepNoBlock(SnakerSettings.SnakerDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
