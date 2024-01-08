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

using Nitrocid.Kernel.Debugging;
using Nitrocid.Misc.Screensaver;
using Terminaux.Base;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Settings for Fader
    /// </summary>
    public static class FaderSettings
    {

        /// <summary>
        /// [Fader] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int FaderDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FaderDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                ScreensaverPackInit.SaversConfig.FaderDelay = value;
            }
        }
        /// <summary>
        /// [Fader] How many milliseconds to wait before fading the text out?
        /// </summary>
        public static int FaderFadeOutDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FaderFadeOutDelay;
            }
            set
            {
                if (value <= 0)
                    value = 3000;
                ScreensaverPackInit.SaversConfig.FaderFadeOutDelay = value;
            }
        }
        /// <summary>
        /// [Fader] Text for Fader. Shorter is better.
        /// </summary>
        public static string FaderWrite
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FaderWrite;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Nitrocid KS";
                ScreensaverPackInit.SaversConfig.FaderWrite = value;
            }
        }
        /// <summary>
        /// [Fader] How many fade steps to do?
        /// </summary>
        public static int FaderMaxSteps
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FaderMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                ScreensaverPackInit.SaversConfig.FaderMaxSteps = value;
            }
        }
        /// <summary>
        /// [Fader] Screensaver background color
        /// </summary>
        public static string FaderBackgroundColor
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FaderBackgroundColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.FaderBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [Fader] The minimum red color level (true color)
        /// </summary>
        public static int FaderMinimumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FaderMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.FaderMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Fader] The minimum green color level (true color)
        /// </summary>
        public static int FaderMinimumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FaderMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.FaderMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Fader] The minimum blue color level (true color)
        /// </summary>
        public static int FaderMinimumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FaderMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.FaderMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Fader] The maximum red color level (true color)
        /// </summary>
        public static int FaderMaximumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FaderMaximumRedColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.FaderMinimumRedColorLevel)
                    value = ScreensaverPackInit.SaversConfig.FaderMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.FaderMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Fader] The maximum green color level (true color)
        /// </summary>
        public static int FaderMaximumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FaderMaximumGreenColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.FaderMinimumGreenColorLevel)
                    value = ScreensaverPackInit.SaversConfig.FaderMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.FaderMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Fader] The maximum blue color level (true color)
        /// </summary>
        public static int FaderMaximumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FaderMaximumBlueColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.FaderMinimumBlueColorLevel)
                    value = ScreensaverPackInit.SaversConfig.FaderMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.FaderMaximumBlueColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for Fader
    /// </summary>
    public class FaderDisplay : BaseScreensaver, IScreensaver
    {

        private Animations.Fader.FaderSettings FaderSettingsInstance;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Fader";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            base.ScreensaverPreparation();
            ColorTools.LoadBack(new Color(FaderSettings.FaderBackgroundColor));
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);
            FaderSettingsInstance = new Animations.Fader.FaderSettings()
            {
                FaderDelay = FaderSettings.FaderDelay,
                FaderWrite = FaderSettings.FaderWrite,
                FaderBackgroundColor = FaderSettings.FaderBackgroundColor,
                FaderFadeOutDelay = FaderSettings.FaderFadeOutDelay,
                FaderMaxSteps = FaderSettings.FaderMaxSteps,
                FaderMinimumRedColorLevel = FaderSettings.FaderMinimumRedColorLevel,
                FaderMinimumGreenColorLevel = FaderSettings.FaderMinimumGreenColorLevel,
                FaderMinimumBlueColorLevel = FaderSettings.FaderMinimumBlueColorLevel,
                FaderMaximumRedColorLevel = FaderSettings.FaderMaximumRedColorLevel,
                FaderMaximumGreenColorLevel = FaderSettings.FaderMaximumGreenColorLevel,
                FaderMaximumBlueColorLevel = FaderSettings.FaderMaximumBlueColorLevel
            };
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic() => Animations.Fader.Fader.Simulate(FaderSettingsInstance);

    }
}
