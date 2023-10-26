//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.ConsoleBase;
using KS.Kernel.Debugging;
using KS.Misc.Screensaver;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
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
                return ScreensaverPackInit.SaversConfig.BeatFaderTrueColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.BeatFaderTrueColor = value;
            }
        }
        /// <summary>
        /// [BeatFader] Enable color cycling (uses RNG. If disabled, uses the <see cref="BeatFaderBeatColor"/> color.)
        /// </summary>
        public static bool BeatFaderCycleColors
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BeatFaderCycleColors;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.BeatFaderCycleColors = value;
            }
        }
        /// <summary>
        /// [BeatFader] The color of beats. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public static string BeatFaderBeatColor
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BeatFaderBeatColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.BeatFaderBeatColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [BeatFader] How many beats per minute to wait before making the next write?
        /// </summary>
        public static int BeatFaderDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BeatFaderDelay;
            }
            set
            {
                if (value <= 0)
                    value = 120;
                ScreensaverPackInit.SaversConfig.BeatFaderDelay = value;
            }
        }
        /// <summary>
        /// [BeatFader] How many fade steps to do?
        /// </summary>
        public static int BeatFaderMaxSteps
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BeatFaderMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                ScreensaverPackInit.SaversConfig.BeatFaderMaxSteps = value;
            }
        }
        /// <summary>
        /// [BeatFader] The minimum red color level (true color)
        /// </summary>
        public static int BeatFaderMinimumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BeatFaderMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.BeatFaderMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatFader] The minimum green color level (true color)
        /// </summary>
        public static int BeatFaderMinimumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BeatFaderMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.BeatFaderMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatFader] The minimum blue color level (true color)
        /// </summary>
        public static int BeatFaderMinimumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BeatFaderMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.BeatFaderMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatFader] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int BeatFaderMinimumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BeatFaderMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                ScreensaverPackInit.SaversConfig.BeatFaderMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatFader] The maximum red color level (true color)
        /// </summary>
        public static int BeatFaderMaximumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BeatFaderMaximumRedColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.BeatFaderMinimumRedColorLevel)
                    value = ScreensaverPackInit.SaversConfig.BeatFaderMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.BeatFaderMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatFader] The maximum green color level (true color)
        /// </summary>
        public static int BeatFaderMaximumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BeatFaderMaximumGreenColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.BeatFaderMinimumGreenColorLevel)
                    value = ScreensaverPackInit.SaversConfig.BeatFaderMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.BeatFaderMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatFader] The maximum blue color level (true color)
        /// </summary>
        public static int BeatFaderMaximumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BeatFaderMaximumBlueColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.BeatFaderMinimumBlueColorLevel)
                    value = ScreensaverPackInit.SaversConfig.BeatFaderMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.BeatFaderMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatFader] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int BeatFaderMaximumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BeatFaderMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= ScreensaverPackInit.SaversConfig.BeatFaderMinimumColorLevel)
                    value = ScreensaverPackInit.SaversConfig.BeatFaderMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                ScreensaverPackInit.SaversConfig.BeatFaderMaximumColorLevel = value;
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
        public override bool ScreensaverContainsFlashingImages { get; set; } = true;

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);
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
