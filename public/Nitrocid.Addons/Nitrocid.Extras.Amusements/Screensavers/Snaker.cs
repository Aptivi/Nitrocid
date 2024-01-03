//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using Nitrocid.Extras.Amusements.Amusements.Games;
using Nitrocid.Kernel.Threading;
using Nitrocid.Misc.Screensaver;

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
                return AmusementsInit.SaversConfig.SnakerTrueColor;
            }
            set
            {
                AmusementsInit.SaversConfig.SnakerTrueColor = value;
            }
        }
        /// <summary>
        /// [Snaker] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int SnakerDelay
        {
            get
            {
                return AmusementsInit.SaversConfig.SnakerDelay;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                AmusementsInit.SaversConfig.SnakerDelay = value;
            }
        }
        /// <summary>
        /// [Snaker] How many milliseconds to wait before making the next stage?
        /// </summary>
        public static int SnakerStageDelay
        {
            get
            {
                return AmusementsInit.SaversConfig.SnakerStageDelay;
            }
            set
            {
                if (value <= 0)
                    value = 5000;
                AmusementsInit.SaversConfig.SnakerStageDelay = value;
            }
        }
        /// <summary>
        /// [Snaker] The minimum red color level (true color)
        /// </summary>
        public static int SnakerMinimumRedColorLevel
        {
            get
            {
                return AmusementsInit.SaversConfig.SnakerMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                AmusementsInit.SaversConfig.SnakerMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Snaker] The minimum green color level (true color)
        /// </summary>
        public static int SnakerMinimumGreenColorLevel
        {
            get
            {
                return AmusementsInit.SaversConfig.SnakerMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                AmusementsInit.SaversConfig.SnakerMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Snaker] The minimum blue color level (true color)
        /// </summary>
        public static int SnakerMinimumBlueColorLevel
        {
            get
            {
                return AmusementsInit.SaversConfig.SnakerMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                AmusementsInit.SaversConfig.SnakerMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Snaker] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int SnakerMinimumColorLevel
        {
            get
            {
                return AmusementsInit.SaversConfig.SnakerMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                AmusementsInit.SaversConfig.SnakerMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Snaker] The maximum red color level (true color)
        /// </summary>
        public static int SnakerMaximumRedColorLevel
        {
            get
            {
                return AmusementsInit.SaversConfig.SnakerMaximumRedColorLevel;
            }
            set
            {
                if (value <= AmusementsInit.SaversConfig.SnakerMinimumRedColorLevel)
                    value = AmusementsInit.SaversConfig.SnakerMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                AmusementsInit.SaversConfig.SnakerMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Snaker] The maximum green color level (true color)
        /// </summary>
        public static int SnakerMaximumGreenColorLevel
        {
            get
            {
                return AmusementsInit.SaversConfig.SnakerMaximumGreenColorLevel;
            }
            set
            {
                if (value <= AmusementsInit.SaversConfig.SnakerMinimumGreenColorLevel)
                    value = AmusementsInit.SaversConfig.SnakerMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                AmusementsInit.SaversConfig.SnakerMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Snaker] The maximum blue color level (true color)
        /// </summary>
        public static int SnakerMaximumBlueColorLevel
        {
            get
            {
                return AmusementsInit.SaversConfig.SnakerMaximumBlueColorLevel;
            }
            set
            {
                if (value <= AmusementsInit.SaversConfig.SnakerMinimumBlueColorLevel)
                    value = AmusementsInit.SaversConfig.SnakerMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                AmusementsInit.SaversConfig.SnakerMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Snaker] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int SnakerMaximumColorLevel
        {
            get
            {
                return AmusementsInit.SaversConfig.SnakerMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= AmusementsInit.SaversConfig.SnakerMinimumColorLevel)
                    value = AmusementsInit.SaversConfig.SnakerMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                AmusementsInit.SaversConfig.SnakerMaximumColorLevel = value;
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
