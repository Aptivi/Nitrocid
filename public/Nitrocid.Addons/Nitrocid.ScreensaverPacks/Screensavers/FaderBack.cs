
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
                return ScreensaverPackInit.SaversConfig.FaderBackDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                ScreensaverPackInit.SaversConfig.FaderBackDelay = value;
            }
        }
        /// <summary>
        /// [FaderBack] How many milliseconds to wait before fading the text out?
        /// </summary>
        public static int FaderBackFadeOutDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FaderBackFadeOutDelay;
            }
            set
            {
                if (value <= 0)
                    value = 3000;
                ScreensaverPackInit.SaversConfig.FaderBackFadeOutDelay = value;
            }
        }
        /// <summary>
        /// [FaderBack] How many fade steps to do?
        /// </summary>
        public static int FaderBackMaxSteps
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FaderBackMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                ScreensaverPackInit.SaversConfig.FaderBackMaxSteps = value;
            }
        }
        /// <summary>
        /// [FaderBack] The minimum red color level (true color)
        /// </summary>
        public static int FaderBackMinimumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FaderBackMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.FaderBackMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FaderBack] The minimum green color level (true color)
        /// </summary>
        public static int FaderBackMinimumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FaderBackMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.FaderBackMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FaderBack] The minimum blue color level (true color)
        /// </summary>
        public static int FaderBackMinimumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FaderBackMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.FaderBackMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [FaderBack] The maximum red color level (true color)
        /// </summary>
        public static int FaderBackMaximumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FaderBackMaximumRedColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.FaderBackMinimumRedColorLevel)
                    value = ScreensaverPackInit.SaversConfig.FaderBackMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.FaderBackMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FaderBack] The maximum green color level (true color)
        /// </summary>
        public static int FaderBackMaximumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FaderBackMaximumGreenColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.FaderBackMinimumGreenColorLevel)
                    value = ScreensaverPackInit.SaversConfig.FaderBackMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.FaderBackMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FaderBack] The maximum blue color level (true color)
        /// </summary>
        public static int FaderBackMaximumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FaderBackMaximumBlueColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.FaderBackMinimumBlueColorLevel)
                    value = ScreensaverPackInit.SaversConfig.FaderBackMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.FaderBackMaximumBlueColorLevel = value;
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
