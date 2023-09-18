
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
    /// Settings for FaderBack
    /// </summary>
    public static class FaderBackSettings
    {

        /// <summary>
        /// [FaderBack] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int FaderBackDelay
        {
            get
            {
                return Config.SaverConfig.FaderBackDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                Config.SaverConfig.FaderBackDelay = value;
            }
        }
        /// <summary>
        /// [FaderBack] How many milliseconds to wait before fading the text out?
        /// </summary>
        public static int FaderBackFadeOutDelay
        {
            get
            {
                return Config.SaverConfig.FaderBackFadeOutDelay;
            }
            set
            {
                if (value <= 0)
                    value = 3000;
                Config.SaverConfig.FaderBackFadeOutDelay = value;
            }
        }
        /// <summary>
        /// [FaderBack] How many fade steps to do?
        /// </summary>
        public static int FaderBackMaxSteps
        {
            get
            {
                return Config.SaverConfig.FaderBackMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                Config.SaverConfig.FaderBackMaxSteps = value;
            }
        }
        /// <summary>
        /// [FaderBack] The minimum red color level (true color)
        /// </summary>
        public static int FaderBackMinimumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.FaderBackMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.FaderBackMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FaderBack] The minimum green color level (true color)
        /// </summary>
        public static int FaderBackMinimumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.FaderBackMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.FaderBackMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FaderBack] The minimum blue color level (true color)
        /// </summary>
        public static int FaderBackMinimumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.FaderBackMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.FaderBackMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [FaderBack] The maximum red color level (true color)
        /// </summary>
        public static int FaderBackMaximumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.FaderBackMaximumRedColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.FaderBackMinimumRedColorLevel)
                    value = Config.SaverConfig.FaderBackMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.FaderBackMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FaderBack] The maximum green color level (true color)
        /// </summary>
        public static int FaderBackMaximumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.FaderBackMaximumGreenColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.FaderBackMinimumGreenColorLevel)
                    value = Config.SaverConfig.FaderBackMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.FaderBackMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FaderBack] The maximum blue color level (true color)
        /// </summary>
        public static int FaderBackMaximumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.FaderBackMaximumBlueColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.FaderBackMinimumBlueColorLevel)
                    value = Config.SaverConfig.FaderBackMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.FaderBackMaximumBlueColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for FaderBack
    /// </summary>
    public class FaderBackDisplay : BaseScreensaver, IScreensaver
    {

        private Animations.FaderBack.FaderBackSettings FaderBackSettingsInstance;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "FaderBack";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);
            FaderBackSettingsInstance = new Animations.FaderBack.FaderBackSettings()
            {
                FaderBackDelay = FaderBackSettings.FaderBackDelay,
                FaderBackFadeOutDelay = FaderBackSettings.FaderBackFadeOutDelay,
                FaderBackMaxSteps = FaderBackSettings.FaderBackMaxSteps,
                FaderBackMinimumRedColorLevel = FaderBackSettings.FaderBackMinimumRedColorLevel,
                FaderBackMinimumGreenColorLevel = FaderBackSettings.FaderBackMinimumGreenColorLevel,
                FaderBackMinimumBlueColorLevel = FaderBackSettings.FaderBackMinimumBlueColorLevel,
                FaderBackMaximumRedColorLevel = FaderBackSettings.FaderBackMaximumRedColorLevel,
                FaderBackMaximumGreenColorLevel = FaderBackSettings.FaderBackMaximumGreenColorLevel,
                FaderBackMaximumBlueColorLevel = FaderBackSettings.FaderBackMaximumBlueColorLevel
            };
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic() => Animations.FaderBack.FaderBack.Simulate(FaderBackSettingsInstance);

    }
}
