
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

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for BeatFader
    /// </summary>
    public static class BeatFaderSettings
    {

        /// <summary>
        /// [BeatFader] Enable truecolor support. Has a higher priority than 255 color support. Please note that it only works if color cycling is enabled.
        /// </summary>
        public static bool BeatFaderTrueColor
        {
            get
            {
                return Config.SaverConfig.BeatFaderTrueColor;
            }
            set
            {
                Config.SaverConfig.BeatFaderTrueColor = value;
            }
        }
        /// <summary>
        /// [BeatFader] Enable color cycling (uses RNG. If disabled, uses the <see cref="BeatFaderBeatColor"/> color.)
        /// </summary>
        public static bool BeatFaderCycleColors
        {
            get
            {
                return Config.SaverConfig.BeatFaderCycleColors;
            }
            set
            {
                Config.SaverConfig.BeatFaderCycleColors = value;
            }
        }
        /// <summary>
        /// [BeatFader] The color of beats. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public static string BeatFaderBeatColor
        {
            get
            {
                return Config.SaverConfig.BeatFaderBeatColor;
            }
            set
            {
                Config.SaverConfig.BeatFaderBeatColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [BeatFader] How many beats per minute to wait before making the next write?
        /// </summary>
        public static int BeatFaderDelay
        {
            get
            {
                return Config.SaverConfig.BeatFaderDelay;
            }
            set
            {
                if (value <= 0)
                    value = 120;
                Config.SaverConfig.BeatFaderDelay = value;
            }
        }
        /// <summary>
        /// [BeatFader] How many fade steps to do?
        /// </summary>
        public static int BeatFaderMaxSteps
        {
            get
            {
                return Config.SaverConfig.BeatFaderMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                Config.SaverConfig.BeatFaderMaxSteps = value;
            }
        }
        /// <summary>
        /// [BeatFader] The minimum red color level (true color)
        /// </summary>
        public static int BeatFaderMinimumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.BeatFaderMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.BeatFaderMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatFader] The minimum green color level (true color)
        /// </summary>
        public static int BeatFaderMinimumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.BeatFaderMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.BeatFaderMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatFader] The minimum blue color level (true color)
        /// </summary>
        public static int BeatFaderMinimumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.BeatFaderMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.BeatFaderMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatFader] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int BeatFaderMinimumColorLevel
        {
            get
            {
                return Config.SaverConfig.BeatFaderMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                Config.SaverConfig.BeatFaderMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatFader] The maximum red color level (true color)
        /// </summary>
        public static int BeatFaderMaximumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.BeatFaderMaximumRedColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.BeatFaderMinimumRedColorLevel)
                    value = Config.SaverConfig.BeatFaderMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.BeatFaderMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatFader] The maximum green color level (true color)
        /// </summary>
        public static int BeatFaderMaximumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.BeatFaderMaximumGreenColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.BeatFaderMinimumGreenColorLevel)
                    value = Config.SaverConfig.BeatFaderMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.BeatFaderMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatFader] The maximum blue color level (true color)
        /// </summary>
        public static int BeatFaderMaximumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.BeatFaderMaximumBlueColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.BeatFaderMinimumBlueColorLevel)
                    value = Config.SaverConfig.BeatFaderMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.BeatFaderMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatFader] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int BeatFaderMaximumColorLevel
        {
            get
            {
                return Config.SaverConfig.BeatFaderMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= Config.SaverConfig.BeatFaderMinimumColorLevel)
                    value = Config.SaverConfig.BeatFaderMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                Config.SaverConfig.BeatFaderMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for BeatFader
    /// </summary>
    public class BeatFaderDisplay : BaseScreensaver, IScreensaver
    {

        private Animations.BeatFader.BeatFaderSettings BeatFaderSettingsInstance;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "BeatFader";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleBase.ConsoleWrapper.WindowWidth, ConsoleBase.ConsoleWrapper.WindowHeight);
            BeatFaderSettingsInstance = new Animations.BeatFader.BeatFaderSettings()
            {
                BeatFaderTrueColor = BeatFaderSettings.BeatFaderTrueColor,
                BeatFaderBeatColor = BeatFaderSettings.BeatFaderBeatColor,
                BeatFaderDelay = BeatFaderSettings.BeatFaderDelay,
                BeatFaderMaxSteps = BeatFaderSettings.BeatFaderMaxSteps,
                BeatFaderCycleColors = BeatFaderSettings.BeatFaderCycleColors,
                BeatFaderMinimumRedColorLevel = BeatFaderSettings.BeatFaderMinimumRedColorLevel,
                BeatFaderMinimumGreenColorLevel = BeatFaderSettings.BeatFaderMinimumGreenColorLevel,
                BeatFaderMinimumBlueColorLevel = BeatFaderSettings.BeatFaderMinimumBlueColorLevel,
                BeatFaderMinimumColorLevel = BeatFaderSettings.BeatFaderMinimumColorLevel,
                BeatFaderMaximumRedColorLevel = BeatFaderSettings.BeatFaderMaximumRedColorLevel,
                BeatFaderMaximumGreenColorLevel = BeatFaderSettings.BeatFaderMaximumGreenColorLevel,
                BeatFaderMaximumBlueColorLevel = BeatFaderSettings.BeatFaderMaximumBlueColorLevel,
                BeatFaderMaximumColorLevel = BeatFaderSettings.BeatFaderMaximumColorLevel
            };
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic() => Animations.BeatFader.BeatFader.Simulate(BeatFaderSettingsInstance);

    }
}
