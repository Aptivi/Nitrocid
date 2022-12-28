
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using ColorSeq;
using KS.Kernel.Debugging;
using ColorTools = KS.ConsoleBase.Colors.ColorTools;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for Fader
    /// </summary>
    public static class FaderSettings
    {

        private static int _faderDelay = 50;
        private static int _faderFadeOutDelay = 3000;
        private static string _faderWrite = "Kernel Simulator";
        private static int _faderMaxSteps = 25;
        private static string _faderBackgroundColor = new Color((int)ConsoleColor.Black).PlainSequence;
        private static int _faderMinimumRedColorLevel = 0;
        private static int _faderMinimumGreenColorLevel = 0;
        private static int _faderMinimumBlueColorLevel = 0;
        private static int _faderMaximumRedColorLevel = 255;
        private static int _faderMaximumGreenColorLevel = 255;
        private static int _faderMaximumBlueColorLevel = 255;

        /// <summary>
        /// [Fader] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int FaderDelay
        {
            get
            {
                return _faderDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                _faderDelay = value;
            }
        }
        /// <summary>
        /// [Fader] How many milliseconds to wait before fading the text out?
        /// </summary>
        public static int FaderFadeOutDelay
        {
            get
            {
                return _faderFadeOutDelay;
            }
            set
            {
                if (value <= 0)
                    value = 3000;
                _faderFadeOutDelay = value;
            }
        }
        /// <summary>
        /// [Fader] Text for Fader. Shorter is better.
        /// </summary>
        public static string FaderWrite
        {
            get
            {
                return _faderWrite;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Kernel Simulator";
                _faderWrite = value;
            }
        }
        /// <summary>
        /// [Fader] How many fade steps to do?
        /// </summary>
        public static int FaderMaxSteps
        {
            get
            {
                return _faderMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                _faderMaxSteps = value;
            }
        }
        /// <summary>
        /// [Fader] Screensaver background color
        /// </summary>
        public static string FaderBackgroundColor
        {
            get
            {
                return _faderBackgroundColor;
            }
            set
            {
                _faderBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [Fader] The minimum red color level (true color)
        /// </summary>
        public static int FaderMinimumRedColorLevel
        {
            get
            {
                return _faderMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _faderMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Fader] The minimum green color level (true color)
        /// </summary>
        public static int FaderMinimumGreenColorLevel
        {
            get
            {
                return _faderMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _faderMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Fader] The minimum blue color level (true color)
        /// </summary>
        public static int FaderMinimumBlueColorLevel
        {
            get
            {
                return _faderMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _faderMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Fader] The maximum red color level (true color)
        /// </summary>
        public static int FaderMaximumRedColorLevel
        {
            get
            {
                return _faderMaximumRedColorLevel;
            }
            set
            {
                if (value <= _faderMinimumRedColorLevel)
                    value = _faderMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                _faderMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Fader] The maximum green color level (true color)
        /// </summary>
        public static int FaderMaximumGreenColorLevel
        {
            get
            {
                return _faderMaximumGreenColorLevel;
            }
            set
            {
                if (value <= _faderMinimumGreenColorLevel)
                    value = _faderMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                _faderMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Fader] The maximum blue color level (true color)
        /// </summary>
        public static int FaderMaximumBlueColorLevel
        {
            get
            {
                return _faderMaximumBlueColorLevel;
            }
            set
            {
                if (value <= _faderMinimumBlueColorLevel)
                    value = _faderMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                _faderMaximumBlueColorLevel = value;
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
            // Variable preparations
            ColorTools.LoadBack(new Color(FaderSettings.FaderBackgroundColor), true);
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
