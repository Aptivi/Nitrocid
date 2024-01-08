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
using Nitrocid.Kernel.Threading;
using Nitrocid.Misc.Screensaver;
using Terminaux.Base;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Settings for BeatPulse
    /// </summary>
    public static class BeatPulseSettings
    {

        /// <summary>
        /// [BeatPulse] Enable truecolor support. Has a higher priority than 255 color support. Please note that it only works if color cycling is enabled.
        /// </summary>
        public static bool BeatPulseTrueColor
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BeatPulseTrueColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.BeatPulseTrueColor = value;
            }
        }
        /// <summary>
        /// [BeatPulse] Enable color cycling (uses RNG. If disabled, uses the <see cref="BeatPulseBeatColor"/> color.)
        /// </summary>
        public static bool BeatPulseCycleColors
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BeatPulseCycleColors;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.BeatPulseCycleColors = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The color of beats. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public static string BeatPulseBeatColor
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BeatPulseBeatColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.BeatPulseBeatColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [BeatPulse] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int BeatPulseDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BeatPulseDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                ScreensaverPackInit.SaversConfig.BeatPulseDelay = value;
            }
        }
        /// <summary>
        /// [BeatPulse] How many fade steps to do?
        /// </summary>
        public static int BeatPulseMaxSteps
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BeatPulseMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                ScreensaverPackInit.SaversConfig.BeatPulseMaxSteps = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The minimum red color level (true color)
        /// </summary>
        public static int BeatPulseMinimumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BeatPulseMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.BeatPulseMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The minimum green color level (true color)
        /// </summary>
        public static int BeatPulseMinimumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BeatPulseMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.BeatPulseMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The minimum blue color level (true color)
        /// </summary>
        public static int BeatPulseMinimumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BeatPulseMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.BeatPulseMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int BeatPulseMinimumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BeatPulseMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                ScreensaverPackInit.SaversConfig.BeatPulseMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The maximum red color level (true color)
        /// </summary>
        public static int BeatPulseMaximumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BeatPulseMaximumRedColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.BeatPulseMinimumRedColorLevel)
                    value = ScreensaverPackInit.SaversConfig.BeatPulseMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.BeatPulseMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The maximum green color level (true color)
        /// </summary>
        public static int BeatPulseMaximumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BeatPulseMaximumGreenColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.BeatPulseMinimumGreenColorLevel)
                    value = ScreensaverPackInit.SaversConfig.BeatPulseMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.BeatPulseMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The maximum blue color level (true color)
        /// </summary>
        public static int BeatPulseMaximumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BeatPulseMaximumBlueColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.BeatPulseMinimumBlueColorLevel)
                    value = ScreensaverPackInit.SaversConfig.BeatPulseMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.BeatPulseMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int BeatPulseMaximumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BeatPulseMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= ScreensaverPackInit.SaversConfig.BeatPulseMinimumColorLevel)
                    value = ScreensaverPackInit.SaversConfig.BeatPulseMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                ScreensaverPackInit.SaversConfig.BeatPulseMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for BeatPulse
    /// </summary>
    public class BeatPulseDisplay : BaseScreensaver, IScreensaver
    {

        private Animations.BeatPulse.BeatPulseSettings BeatPulseSettingsInstance;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "BeatPulse";

        /// <inheritdoc/>
        public override bool ScreensaverContainsFlashingImages { get; set; } = true;

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);
            BeatPulseSettingsInstance = new Animations.BeatPulse.BeatPulseSettings()
            {
                BeatPulseTrueColor = BeatPulseSettings.BeatPulseTrueColor,
                BeatPulseBeatColor = BeatPulseSettings.BeatPulseBeatColor,
                BeatPulseDelay = BeatPulseSettings.BeatPulseDelay,
                BeatPulseMaxSteps = BeatPulseSettings.BeatPulseMaxSteps,
                BeatPulseCycleColors = BeatPulseSettings.BeatPulseCycleColors,
                BeatPulseMinimumRedColorLevel = BeatPulseSettings.BeatPulseMinimumRedColorLevel,
                BeatPulseMinimumGreenColorLevel = BeatPulseSettings.BeatPulseMinimumGreenColorLevel,
                BeatPulseMinimumBlueColorLevel = BeatPulseSettings.BeatPulseMinimumBlueColorLevel,
                BeatPulseMinimumColorLevel = BeatPulseSettings.BeatPulseMinimumColorLevel,
                BeatPulseMaximumRedColorLevel = BeatPulseSettings.BeatPulseMaximumRedColorLevel,
                BeatPulseMaximumGreenColorLevel = BeatPulseSettings.BeatPulseMaximumGreenColorLevel,
                BeatPulseMaximumBlueColorLevel = BeatPulseSettings.BeatPulseMaximumBlueColorLevel,
                BeatPulseMaximumColorLevel = BeatPulseSettings.BeatPulseMaximumColorLevel
            };
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            Animations.BeatPulse.BeatPulse.Simulate(BeatPulseSettingsInstance);
            ThreadManager.SleepNoBlock(BeatPulseSettings.BeatPulseDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
