
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

using KS.ConsoleBase.Colors;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using Terminaux.Colors;

namespace KS.Misc.Screensaver.Displays
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
                return Config.SaverConfig.FaderDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                Config.SaverConfig.FaderDelay = value;
            }
        }
        /// <summary>
        /// [Fader] How many milliseconds to wait before fading the text out?
        /// </summary>
        public static int FaderFadeOutDelay
        {
            get
            {
                return Config.SaverConfig.FaderFadeOutDelay;
            }
            set
            {
                if (value <= 0)
                    value = 3000;
                Config.SaverConfig.FaderFadeOutDelay = value;
            }
        }
        /// <summary>
        /// [Fader] Text for Fader. Shorter is better.
        /// </summary>
        public static string FaderWrite
        {
            get
            {
                return Config.SaverConfig.FaderWrite;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Nitrocid KS";
                Config.SaverConfig.FaderWrite = value;
            }
        }
        /// <summary>
        /// [Fader] How many fade steps to do?
        /// </summary>
        public static int FaderMaxSteps
        {
            get
            {
                return Config.SaverConfig.FaderMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                Config.SaverConfig.FaderMaxSteps = value;
            }
        }
        /// <summary>
        /// [Fader] Screensaver background color
        /// </summary>
        public static string FaderBackgroundColor
        {
            get
            {
                return Config.SaverConfig.FaderBackgroundColor;
            }
            set
            {
                Config.SaverConfig.FaderBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [Fader] The minimum red color level (true color)
        /// </summary>
        public static int FaderMinimumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.FaderMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.FaderMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Fader] The minimum green color level (true color)
        /// </summary>
        public static int FaderMinimumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.FaderMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.FaderMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Fader] The minimum blue color level (true color)
        /// </summary>
        public static int FaderMinimumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.FaderMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.FaderMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Fader] The maximum red color level (true color)
        /// </summary>
        public static int FaderMaximumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.FaderMaximumRedColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.FaderMinimumRedColorLevel)
                    value = Config.SaverConfig.FaderMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.FaderMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Fader] The maximum green color level (true color)
        /// </summary>
        public static int FaderMaximumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.FaderMaximumGreenColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.FaderMinimumGreenColorLevel)
                    value = Config.SaverConfig.FaderMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.FaderMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Fader] The maximum blue color level (true color)
        /// </summary>
        public static int FaderMaximumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.FaderMaximumBlueColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.FaderMinimumBlueColorLevel)
                    value = Config.SaverConfig.FaderMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.FaderMaximumBlueColorLevel = value;
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
            KernelColorTools.LoadBack(new Color(FaderSettings.FaderBackgroundColor), true);
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleBase.ConsoleWrapper.WindowWidth, ConsoleBase.ConsoleWrapper.WindowHeight);
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
