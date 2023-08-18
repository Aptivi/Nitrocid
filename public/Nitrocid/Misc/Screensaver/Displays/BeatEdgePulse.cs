
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
using KS.ConsoleBase.Colors;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Kernel.Threading;
using Terminaux.Colors;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for BeatEdgePulse
    /// </summary>
    public static class BeatEdgePulseSettings
    {

        /// <summary>
        /// [BeatEdgePulse] Enable truecolor support. Has a higher priority than 255 color support. Please note that it only works if color cycling is enabled.
        /// </summary>
        public static bool BeatEdgePulseTrueColor
        {
            get
            {
                return Config.SaverConfig.BeatEdgePulseTrueColor;
            }
            set
            {
                Config.SaverConfig.BeatEdgePulseTrueColor = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] Enable color cycling (uses RNG. If disabled, uses the <see cref="BeatEdgePulseBeatColor"/> color.)
        /// </summary>
        public static bool BeatEdgePulseCycleColors
        {
            get
            {
                return Config.SaverConfig.BeatEdgePulseCycleColors;
            }
            set
            {
                Config.SaverConfig.BeatEdgePulseCycleColors = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] The color of beats. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public static string BeatEdgePulseBeatColor
        {
            get
            {
                return Config.SaverConfig.BeatEdgePulseBeatColor;
            }
            set
            {
                Config.SaverConfig.BeatEdgePulseBeatColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int BeatEdgePulseDelay
        {
            get
            {
                return Config.SaverConfig.BeatEdgePulseDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                Config.SaverConfig.BeatEdgePulseDelay = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] How many fade steps to do?
        /// </summary>
        public static int BeatEdgePulseMaxSteps
        {
            get
            {
                return Config.SaverConfig.BeatEdgePulseMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                Config.SaverConfig.BeatEdgePulseMaxSteps = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] The minimum red color level (true color)
        /// </summary>
        public static int BeatEdgePulseMinimumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.BeatEdgePulseMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.BeatEdgePulseMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] The minimum green color level (true color)
        /// </summary>
        public static int BeatEdgePulseMinimumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.BeatEdgePulseMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.BeatEdgePulseMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] The minimum blue color level (true color)
        /// </summary>
        public static int BeatEdgePulseMinimumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.BeatEdgePulseMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.BeatEdgePulseMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int BeatEdgePulseMinimumColorLevel
        {
            get
            {
                return Config.SaverConfig.BeatEdgePulseMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                Config.SaverConfig.BeatEdgePulseMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] The maximum red color level (true color)
        /// </summary>
        public static int BeatEdgePulseMaximumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.BeatEdgePulseMaximumRedColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.BeatEdgePulseMinimumRedColorLevel)
                    value = Config.SaverConfig.BeatEdgePulseMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.BeatEdgePulseMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] The maximum green color level (true color)
        /// </summary>
        public static int BeatEdgePulseMaximumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.BeatEdgePulseMaximumGreenColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.BeatEdgePulseMinimumGreenColorLevel)
                    value = Config.SaverConfig.BeatEdgePulseMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.BeatEdgePulseMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] The maximum blue color level (true color)
        /// </summary>
        public static int BeatEdgePulseMaximumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.BeatEdgePulseMaximumBlueColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.BeatEdgePulseMinimumBlueColorLevel)
                    value = Config.SaverConfig.BeatEdgePulseMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.BeatEdgePulseMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int BeatEdgePulseMaximumColorLevel
        {
            get
            {
                return Config.SaverConfig.BeatEdgePulseMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= Config.SaverConfig.BeatEdgePulseMinimumColorLevel)
                    value = Config.SaverConfig.BeatEdgePulseMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                Config.SaverConfig.BeatEdgePulseMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for BeatEdgePulse
    /// </summary>
    public class BeatEdgePulseDisplay : BaseScreensaver, IScreensaver
    {

        private Animations.BeatEdgePulse.BeatEdgePulseSettings BeatEdgePulseSettingsInstance;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "BeatEdgePulse";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);
            BeatEdgePulseSettingsInstance = new Animations.BeatEdgePulse.BeatEdgePulseSettings()
            {
                BeatEdgePulseTrueColor = BeatEdgePulseSettings.BeatEdgePulseTrueColor,
                BeatEdgePulseBeatColor = BeatEdgePulseSettings.BeatEdgePulseBeatColor,
                BeatEdgePulseDelay = BeatEdgePulseSettings.BeatEdgePulseDelay,
                BeatEdgePulseMaxSteps = BeatEdgePulseSettings.BeatEdgePulseMaxSteps,
                BeatEdgePulseCycleColors = BeatEdgePulseSettings.BeatEdgePulseCycleColors,
                BeatEdgePulseMinimumRedColorLevel = BeatEdgePulseSettings.BeatEdgePulseMinimumRedColorLevel,
                BeatEdgePulseMinimumGreenColorLevel = BeatEdgePulseSettings.BeatEdgePulseMinimumGreenColorLevel,
                BeatEdgePulseMinimumBlueColorLevel = BeatEdgePulseSettings.BeatEdgePulseMinimumBlueColorLevel,
                BeatEdgePulseMinimumColorLevel = BeatEdgePulseSettings.BeatEdgePulseMinimumColorLevel,
                BeatEdgePulseMaximumRedColorLevel = BeatEdgePulseSettings.BeatEdgePulseMaximumRedColorLevel,
                BeatEdgePulseMaximumGreenColorLevel = BeatEdgePulseSettings.BeatEdgePulseMaximumGreenColorLevel,
                BeatEdgePulseMaximumBlueColorLevel = BeatEdgePulseSettings.BeatEdgePulseMaximumBlueColorLevel,
                BeatEdgePulseMaximumColorLevel = BeatEdgePulseSettings.BeatEdgePulseMaximumColorLevel
            };
            KernelColorTools.LoadBack(0);
            ConsoleWrapper.CursorVisible = false;
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            Animations.BeatEdgePulse.BeatEdgePulse.Simulate(BeatEdgePulseSettingsInstance);
            ThreadManager.SleepNoBlock(BeatEdgePulseSettings.BeatEdgePulseDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
