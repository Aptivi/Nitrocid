
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

using KS.ConsoleBase;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Misc.Screensaver;

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
                return Config.SaverConfig.SquareCornerDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                Config.SaverConfig.SquareCornerDelay = value;
            }
        }
        /// <summary>
        /// [SquareCorner] How many milliseconds to wait before fading the square out?
        /// </summary>
        public static int SquareCornerFadeOutDelay
        {
            get
            {
                return Config.SaverConfig.SquareCornerFadeOutDelay;
            }
            set
            {
                if (value <= 0)
                    value = 3000;
                Config.SaverConfig.SquareCornerFadeOutDelay = value;
            }
        }
        /// <summary>
        /// [SquareCorner] How many fade steps to do?
        /// </summary>
        public static int SquareCornerMaxSteps
        {
            get
            {
                return Config.SaverConfig.SquareCornerMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                Config.SaverConfig.SquareCornerMaxSteps = value;
            }
        }
        /// <summary>
        /// [SquareCorner] The minimum red color level (true color)
        /// </summary>
        public static int SquareCornerMinimumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.SquareCornerMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.SquareCornerMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [SquareCorner] The minimum green color level (true color)
        /// </summary>
        public static int SquareCornerMinimumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.SquareCornerMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.SquareCornerMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [SquareCorner] The minimum blue color level (true color)
        /// </summary>
        public static int SquareCornerMinimumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.SquareCornerMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.SquareCornerMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [SquareCorner] The maximum red color level (true color)
        /// </summary>
        public static int SquareCornerMaximumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.SquareCornerMaximumRedColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.SquareCornerMinimumRedColorLevel)
                    value = Config.SaverConfig.SquareCornerMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.SquareCornerMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [SquareCorner] The maximum green color level (true color)
        /// </summary>
        public static int SquareCornerMaximumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.SquareCornerMaximumGreenColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.SquareCornerMinimumGreenColorLevel)
                    value = Config.SaverConfig.SquareCornerMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.SquareCornerMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [SquareCorner] The maximum blue color level (true color)
        /// </summary>
        public static int SquareCornerMaximumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.SquareCornerMaximumBlueColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.SquareCornerMinimumBlueColorLevel)
                    value = Config.SaverConfig.SquareCornerMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.SquareCornerMaximumBlueColorLevel = value;
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
