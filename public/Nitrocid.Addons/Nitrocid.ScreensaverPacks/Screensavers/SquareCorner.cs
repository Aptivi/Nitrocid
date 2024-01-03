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

using Nitrocid.ConsoleBase;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Misc.Screensaver;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Settings for SquareCorner
    /// </summary>
    public static class SquareCornerSettings
    {

        /// <summary>
        /// [SquareCorner] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int SquareCornerDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.SquareCornerDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                ScreensaverPackInit.SaversConfig.SquareCornerDelay = value;
            }
        }
        /// <summary>
        /// [SquareCorner] How many milliseconds to wait before fading the square out?
        /// </summary>
        public static int SquareCornerFadeOutDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.SquareCornerFadeOutDelay;
            }
            set
            {
                if (value <= 0)
                    value = 3000;
                ScreensaverPackInit.SaversConfig.SquareCornerFadeOutDelay = value;
            }
        }
        /// <summary>
        /// [SquareCorner] How many fade steps to do?
        /// </summary>
        public static int SquareCornerMaxSteps
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.SquareCornerMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                ScreensaverPackInit.SaversConfig.SquareCornerMaxSteps = value;
            }
        }
        /// <summary>
        /// [SquareCorner] The minimum red color level (true color)
        /// </summary>
        public static int SquareCornerMinimumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.SquareCornerMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.SquareCornerMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [SquareCorner] The minimum green color level (true color)
        /// </summary>
        public static int SquareCornerMinimumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.SquareCornerMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.SquareCornerMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [SquareCorner] The minimum blue color level (true color)
        /// </summary>
        public static int SquareCornerMinimumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.SquareCornerMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.SquareCornerMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [SquareCorner] The maximum red color level (true color)
        /// </summary>
        public static int SquareCornerMaximumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.SquareCornerMaximumRedColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.SquareCornerMinimumRedColorLevel)
                    value = ScreensaverPackInit.SaversConfig.SquareCornerMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.SquareCornerMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [SquareCorner] The maximum green color level (true color)
        /// </summary>
        public static int SquareCornerMaximumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.SquareCornerMaximumGreenColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.SquareCornerMinimumGreenColorLevel)
                    value = ScreensaverPackInit.SaversConfig.SquareCornerMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.SquareCornerMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [SquareCorner] The maximum blue color level (true color)
        /// </summary>
        public static int SquareCornerMaximumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.SquareCornerMaximumBlueColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.SquareCornerMinimumBlueColorLevel)
                    value = ScreensaverPackInit.SaversConfig.SquareCornerMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.SquareCornerMaximumBlueColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for SquareCorner
    /// </summary>
    public class SquareCornerDisplay : BaseScreensaver, IScreensaver
    {

        private Animations.SquareCorner.SquareCornerSettings SquareCornerSettingsInstance;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "SquareCorner";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);
            SquareCornerSettingsInstance = new Animations.SquareCorner.SquareCornerSettings()
            {
                SquareCornerDelay = SquareCornerSettings.SquareCornerDelay,
                SquareCornerFadeOutDelay = SquareCornerSettings.SquareCornerFadeOutDelay,
                SquareCornerMaxSteps = SquareCornerSettings.SquareCornerMaxSteps,
                SquareCornerMinimumRedColorLevel = SquareCornerSettings.SquareCornerMinimumRedColorLevel,
                SquareCornerMinimumGreenColorLevel = SquareCornerSettings.SquareCornerMinimumGreenColorLevel,
                SquareCornerMinimumBlueColorLevel = SquareCornerSettings.SquareCornerMinimumBlueColorLevel,
                SquareCornerMaximumRedColorLevel = SquareCornerSettings.SquareCornerMaximumRedColorLevel,
                SquareCornerMaximumGreenColorLevel = SquareCornerSettings.SquareCornerMaximumGreenColorLevel,
                SquareCornerMaximumBlueColorLevel = SquareCornerSettings.SquareCornerMaximumBlueColorLevel,
            };
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic() => Animations.SquareCorner.SquareCorner.Simulate(SquareCornerSettingsInstance);

    }
}
