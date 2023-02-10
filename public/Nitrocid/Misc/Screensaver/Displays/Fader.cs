
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

        private static int _Delay = 50;
        private static int _FadeOutDelay = 3000;
        private static string _Write = "Nitrocid KS";
        private static int _MaxSteps = 25;
        private static string _BackgroundColor = new Color(ConsoleColors.Black).PlainSequence;
        private static int _MinimumRedColorLevel = 0;
        private static int _MinimumGreenColorLevel = 0;
        private static int _MinimumBlueColorLevel = 0;
        private static int _MaximumRedColorLevel = 255;
        private static int _MaximumGreenColorLevel = 255;
        private static int _MaximumBlueColorLevel = 255;

        /// <summary>
        /// [Fader] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int FaderDelay
        {
            get
            {
                return _Delay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                _Delay = value;
            }
        }
        /// <summary>
        /// [Fader] How many milliseconds to wait before fading the text out?
        /// </summary>
        public static int FaderFadeOutDelay
        {
            get
            {
                return _FadeOutDelay;
            }
            set
            {
                if (value <= 0)
                    value = 3000;
                _FadeOutDelay = value;
            }
        }
        /// <summary>
        /// [Fader] Text for Fader. Shorter is better.
        /// </summary>
        public static string FaderWrite
        {
            get
            {
                return _Write;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Nitrocid KS";
                _Write = value;
            }
        }
        /// <summary>
        /// [Fader] How many fade steps to do?
        /// </summary>
        public static int FaderMaxSteps
        {
            get
            {
                return _MaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                _MaxSteps = value;
            }
        }
        /// <summary>
        /// [Fader] Screensaver background color
        /// </summary>
        public static string FaderBackgroundColor
        {
            get
            {
                return _BackgroundColor;
            }
            set
            {
                _BackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [Fader] The minimum red color level (true color)
        /// </summary>
        public static int FaderMinimumRedColorLevel
        {
            get
            {
                return _MinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _MinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Fader] The minimum green color level (true color)
        /// </summary>
        public static int FaderMinimumGreenColorLevel
        {
            get
            {
                return _MinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _MinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Fader] The minimum blue color level (true color)
        /// </summary>
        public static int FaderMinimumBlueColorLevel
        {
            get
            {
                return _MinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _MinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Fader] The maximum red color level (true color)
        /// </summary>
        public static int FaderMaximumRedColorLevel
        {
            get
            {
                return _MaximumRedColorLevel;
            }
            set
            {
                if (value <= _MinimumRedColorLevel)
                    value = _MinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                _MaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Fader] The maximum green color level (true color)
        /// </summary>
        public static int FaderMaximumGreenColorLevel
        {
            get
            {
                return _MaximumGreenColorLevel;
            }
            set
            {
                if (value <= _MinimumGreenColorLevel)
                    value = _MinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                _MaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Fader] The maximum blue color level (true color)
        /// </summary>
        public static int FaderMaximumBlueColorLevel
        {
            get
            {
                return _MaximumBlueColorLevel;
            }
            set
            {
                if (value <= _MinimumBlueColorLevel)
                    value = _MinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                _MaximumBlueColorLevel = value;
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
