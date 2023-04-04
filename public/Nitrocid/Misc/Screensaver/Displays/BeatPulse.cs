
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

using ColorSeq;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Misc.Threading;

namespace KS.Misc.Screensaver.Displays
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
                return Config.SaverConfig.BeatPulseTrueColor;
            }
            set
            {
                Config.SaverConfig.BeatPulseTrueColor = value;
            }
        }
        /// <summary>
        /// [BeatPulse] Enable color cycling (uses RNG. If disabled, uses the <see cref="BeatPulseBeatColor"/> color.)
        /// </summary>
        public static bool BeatPulseCycleColors
        {
            get
            {
                return Config.SaverConfig.BeatPulseCycleColors;
            }
            set
            {
                Config.SaverConfig.BeatPulseCycleColors = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The color of beats. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public static string BeatPulseBeatColor
        {
            get
            {
                return Config.SaverConfig.BeatPulseBeatColor;
            }
            set
            {
                Config.SaverConfig.BeatPulseBeatColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [BeatPulse] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int BeatPulseDelay
        {
            get
            {
                return Config.SaverConfig.BeatPulseDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                Config.SaverConfig.BeatPulseDelay = value;
            }
        }
        /// <summary>
        /// [BeatPulse] How many fade steps to do?
        /// </summary>
        public static int BeatPulseMaxSteps
        {
            get
            {
                return Config.SaverConfig.BeatPulseMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                Config.SaverConfig.BeatPulseMaxSteps = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The minimum red color level (true color)
        /// </summary>
        public static int BeatPulseMinimumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.BeatPulseMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.BeatPulseMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The minimum green color level (true color)
        /// </summary>
        public static int BeatPulseMinimumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.BeatPulseMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.BeatPulseMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The minimum blue color level (true color)
        /// </summary>
        public static int BeatPulseMinimumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.BeatPulseMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.BeatPulseMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int BeatPulseMinimumColorLevel
        {
            get
            {
                return Config.SaverConfig.BeatPulseMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                Config.SaverConfig.BeatPulseMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The maximum red color level (true color)
        /// </summary>
        public static int BeatPulseMaximumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.BeatPulseMaximumRedColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.BeatPulseMinimumRedColorLevel)
                    value = Config.SaverConfig.BeatPulseMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.BeatPulseMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The maximum green color level (true color)
        /// </summary>
        public static int BeatPulseMaximumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.BeatPulseMaximumGreenColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.BeatPulseMinimumGreenColorLevel)
                    value = Config.SaverConfig.BeatPulseMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.BeatPulseMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The maximum blue color level (true color)
        /// </summary>
        public static int BeatPulseMaximumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.BeatPulseMaximumBlueColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.BeatPulseMinimumBlueColorLevel)
                    value = Config.SaverConfig.BeatPulseMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.BeatPulseMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int BeatPulseMaximumColorLevel
        {
            get
            {
                return Config.SaverConfig.BeatPulseMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= Config.SaverConfig.BeatPulseMinimumColorLevel)
                    value = Config.SaverConfig.BeatPulseMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                Config.SaverConfig.BeatPulseMaximumColorLevel = value;
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
        public override void ScreensaverPreparation()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleBase.ConsoleWrapper.WindowWidth, ConsoleBase.ConsoleWrapper.WindowHeight);
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
