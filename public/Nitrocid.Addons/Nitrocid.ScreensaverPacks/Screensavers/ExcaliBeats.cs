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

using System.Runtime.Versioning;
using Nitrocid.ConsoleBase;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Kernel;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Misc.Screensaver;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Settings for ExcaliBeats
    /// </summary>
    public static class ExcaliBeatsSettings
    {

        /// <summary>
        /// [ExcaliBeats] Enable truecolor support. Has a higher priority than 255 color support. Please note that it only works if color cycling is enabled.
        /// </summary>
        public static bool ExcaliBeatsTrueColor
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ExcaliBeatsTrueColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.ExcaliBeatsTrueColor = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] Enable color cycling (uses RNG. If disabled, uses the <see cref="ExcaliBeatsBeatColor"/> color.)
        /// </summary>
        public static bool ExcaliBeatsCycleColors
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ExcaliBeatsCycleColors;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.ExcaliBeatsCycleColors = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] Explicitly change the text to Excalibur
        /// </summary>
        public static bool ExcaliBeatsExplicit
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ExcaliBeatsExplicit;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.ExcaliBeatsExplicit = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] [Linux only] Trance mode - Multiplies the BPM by 2 to simulate the trance music style
        /// </summary>
        public static bool ExcaliBeatsTranceMode
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ExcaliBeatsTranceMode;
            }
            [UnsupportedOSPlatform("windows")]
            set
            {
                if (KernelPlatform.IsOnUnix())
                    ScreensaverPackInit.SaversConfig.ExcaliBeatsTranceMode = value;
                else
                    ScreensaverPackInit.SaversConfig.ExcaliBeatsTranceMode = false;
            }
        }
        /// <summary>
        /// [ExcaliBeats] The color of beats. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public static string ExcaliBeatsBeatColor
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ExcaliBeatsBeatColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.ExcaliBeatsBeatColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [ExcaliBeats] How many beats per minute to wait before making the next write?
        /// </summary>
        public static int ExcaliBeatsDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ExcaliBeatsDelay;
            }
            set
            {
                if (value <= 0)
                    value = 140;
                ScreensaverPackInit.SaversConfig.ExcaliBeatsDelay = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] How many fade steps to do?
        /// </summary>
        public static int ExcaliBeatsMaxSteps
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ExcaliBeatsMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                ScreensaverPackInit.SaversConfig.ExcaliBeatsMaxSteps = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] The minimum red color level (true color)
        /// </summary>
        public static int ExcaliBeatsMinimumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ExcaliBeatsMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ExcaliBeatsMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] The minimum green color level (true color)
        /// </summary>
        public static int ExcaliBeatsMinimumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ExcaliBeatsMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ExcaliBeatsMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] The minimum blue color level (true color)
        /// </summary>
        public static int ExcaliBeatsMinimumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ExcaliBeatsMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ExcaliBeatsMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int ExcaliBeatsMinimumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ExcaliBeatsMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                ScreensaverPackInit.SaversConfig.ExcaliBeatsMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] The maximum red color level (true color)
        /// </summary>
        public static int ExcaliBeatsMaximumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ExcaliBeatsMaximumRedColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.ExcaliBeatsMinimumRedColorLevel)
                    value = ScreensaverPackInit.SaversConfig.ExcaliBeatsMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ExcaliBeatsMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] The maximum green color level (true color)
        /// </summary>
        public static int ExcaliBeatsMaximumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ExcaliBeatsMaximumGreenColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.ExcaliBeatsMinimumGreenColorLevel)
                    value = ScreensaverPackInit.SaversConfig.ExcaliBeatsMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ExcaliBeatsMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] The maximum blue color level (true color)
        /// </summary>
        public static int ExcaliBeatsMaximumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ExcaliBeatsMaximumBlueColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.ExcaliBeatsMinimumBlueColorLevel)
                    value = ScreensaverPackInit.SaversConfig.ExcaliBeatsMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ExcaliBeatsMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int ExcaliBeatsMaximumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ExcaliBeatsMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= ScreensaverPackInit.SaversConfig.ExcaliBeatsMinimumColorLevel)
                    value = ScreensaverPackInit.SaversConfig.ExcaliBeatsMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                ScreensaverPackInit.SaversConfig.ExcaliBeatsMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for ExcaliBeats
    /// </summary>
    public class ExcaliBeatsDisplay : BaseScreensaver, IScreensaver
    {

        private Animations.ExcaliBeats.ExcaliBeatsSettings ExcaliBeatsSettingsInstance;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "ExcaliBeats";

        /// <inheritdoc/>
        public override bool ScreensaverContainsFlashingImages { get; set; } = true;

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);
            ExcaliBeatsSettingsInstance = new Animations.ExcaliBeats.ExcaliBeatsSettings()
            {
                ExcaliBeatsTrueColor = ExcaliBeatsSettings.ExcaliBeatsTrueColor,
                ExcaliBeatsBeatColor = ExcaliBeatsSettings.ExcaliBeatsBeatColor,
                ExcaliBeatsDelay = ExcaliBeatsSettings.ExcaliBeatsDelay,
                ExcaliBeatsMaxSteps = ExcaliBeatsSettings.ExcaliBeatsMaxSteps,
                ExcaliBeatsCycleColors = ExcaliBeatsSettings.ExcaliBeatsCycleColors,
                ExcaliBeatsExplicit = ExcaliBeatsSettings.ExcaliBeatsExplicit,
                ExcaliBeatsMinimumRedColorLevel = ExcaliBeatsSettings.ExcaliBeatsMinimumRedColorLevel,
                ExcaliBeatsMinimumGreenColorLevel = ExcaliBeatsSettings.ExcaliBeatsMinimumGreenColorLevel,
                ExcaliBeatsMinimumBlueColorLevel = ExcaliBeatsSettings.ExcaliBeatsMinimumBlueColorLevel,
                ExcaliBeatsMinimumColorLevel = ExcaliBeatsSettings.ExcaliBeatsMinimumColorLevel,
                ExcaliBeatsMaximumRedColorLevel = ExcaliBeatsSettings.ExcaliBeatsMaximumRedColorLevel,
                ExcaliBeatsMaximumGreenColorLevel = ExcaliBeatsSettings.ExcaliBeatsMaximumGreenColorLevel,
                ExcaliBeatsMaximumBlueColorLevel = ExcaliBeatsSettings.ExcaliBeatsMaximumBlueColorLevel,
                ExcaliBeatsMaximumColorLevel = ExcaliBeatsSettings.ExcaliBeatsMaximumColorLevel
            };
            KernelColorTools.LoadBack(0);
            ConsoleWrapper.Clear();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic() => Animations.ExcaliBeats.ExcaliBeats.Simulate(ExcaliBeatsSettingsInstance);

    }
}
