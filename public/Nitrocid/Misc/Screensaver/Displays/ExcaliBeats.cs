
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

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for ExcaliBeats
    /// </summary>
    public static class ExcaliBeatsSettings
    {

        private static bool _TrueColor = true;
        private static bool _CycleColors = true;
        private static bool _Explicit = true;
        private static bool _TranceMode = true;
        private static string _BeatColor = "17";
        private static int _Delay = 140;
        private static int _MaxSteps = 25;
        private static int _MinimumRedColorLevel = 0;
        private static int _MinimumGreenColorLevel = 0;
        private static int _MinimumBlueColorLevel = 0;
        private static int _MinimumColorLevel = 0;
        private static int _MaximumRedColorLevel = 255;
        private static int _MaximumGreenColorLevel = 255;
        private static int _MaximumBlueColorLevel = 255;
        private static int _MaximumColorLevel = 255;

        /// <summary>
        /// [ExcaliBeats] Enable truecolor support. Has a higher priority than 255 color support. Please note that it only works if color cycling is enabled.
        /// </summary>
        public static bool ExcaliBeatsTrueColor
        {
            get
            {
                return _TrueColor;
            }
            set
            {
                _TrueColor = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] Enable color cycling (uses RNG. If disabled, uses the <see cref="ExcaliBeatsBeatColor"/> color.)
        /// </summary>
        public static bool ExcaliBeatsCycleColors
        {
            get
            {
                return _CycleColors;
            }
            set
            {
                _CycleColors = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] Explicitly change the text to Excalibur
        /// </summary>
        public static bool ExcaliBeatsExplicit
        {
            get
            {
                return _Explicit;
            }
            set
            {
                _Explicit = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] Trance mode - Multiplies the BPM by 2 to simulate the trance music style
        /// </summary>
        public static bool ExcaliBeatsTranceMode
        {
            get
            {
                return _TranceMode;
            }
            set
            {
                _TranceMode = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] The color of beats. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public static string ExcaliBeatsBeatColor
        {
            get
            {
                return _BeatColor;
            }
            set
            {
                _BeatColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [ExcaliBeats] How many beats per minute to wait before making the next write?
        /// </summary>
        public static int ExcaliBeatsDelay
        {
            get
            {
                return _Delay;
            }
            set
            {
                if (value <= 0)
                    value = 140;
                _Delay = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] How many fade steps to do?
        /// </summary>
        public static int ExcaliBeatsMaxSteps
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
        /// [ExcaliBeats] The minimum red color level (true color)
        /// </summary>
        public static int ExcaliBeatsMinimumRedColorLevel
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
        /// [ExcaliBeats] The minimum green color level (true color)
        /// </summary>
        public static int ExcaliBeatsMinimumGreenColorLevel
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
        /// [ExcaliBeats] The minimum blue color level (true color)
        /// </summary>
        public static int ExcaliBeatsMinimumBlueColorLevel
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
        /// [ExcaliBeats] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int ExcaliBeatsMinimumColorLevel
        {
            get
            {
                return _MinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                _MinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [ExcaliBeats] The maximum red color level (true color)
        /// </summary>
        public static int ExcaliBeatsMaximumRedColorLevel
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
        /// [ExcaliBeats] The maximum green color level (true color)
        /// </summary>
        public static int ExcaliBeatsMaximumGreenColorLevel
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
        /// [ExcaliBeats] The maximum blue color level (true color)
        /// </summary>
        public static int ExcaliBeatsMaximumBlueColorLevel
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
        /// <summary>
        /// [ExcaliBeats] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int ExcaliBeatsMaximumColorLevel
        {
            get
            {
                return _MaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= _MinimumColorLevel)
                    value = _MinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                _MaximumColorLevel = value;
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
        public override void ScreensaverPreparation()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleBase.ConsoleWrapper.WindowWidth, ConsoleBase.ConsoleWrapper.WindowHeight);
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
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic() => Animations.ExcaliBeats.ExcaliBeats.Simulate(ExcaliBeatsSettingsInstance);

    }
}
