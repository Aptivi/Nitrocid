
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

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for BeatFader
    /// </summary>
    public static class BeatFaderSettings
    {

        private static bool _TrueColor = true;
        private static bool _CycleColors = true;
        private static string _BeatColor = 17.ToString();
        private static int _Delay = 120;
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
        /// [BeatFader] Enable truecolor support. Has a higher priority than 255 color support. Please note that it only works if color cycling is enabled.
        /// </summary>
        public static bool BeatFaderTrueColor
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
        /// [BeatFader] Enable color cycling (uses RNG. If disabled, uses the <see cref="BeatFaderBeatColor"/> color.)
        /// </summary>
        public static bool BeatFaderCycleColors
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
        /// [BeatFader] The color of beats. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public static string BeatFaderBeatColor
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
        /// [BeatFader] How many beats per minute to wait before making the next write?
        /// </summary>
        public static int BeatFaderDelay
        {
            get
            {
                return _Delay;
            }
            set
            {
                if (value <= 0)
                    value = 120;
                _Delay = value;
            }
        }
        /// <summary>
        /// [BeatFader] How many fade steps to do?
        /// </summary>
        public static int BeatFaderMaxSteps
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
        /// [BeatFader] The minimum red color level (true color)
        /// </summary>
        public static int BeatFaderMinimumRedColorLevel
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
        /// [BeatFader] The minimum green color level (true color)
        /// </summary>
        public static int BeatFaderMinimumGreenColorLevel
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
        /// [BeatFader] The minimum blue color level (true color)
        /// </summary>
        public static int BeatFaderMinimumBlueColorLevel
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
        /// [BeatFader] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int BeatFaderMinimumColorLevel
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
        /// [BeatFader] The maximum red color level (true color)
        /// </summary>
        public static int BeatFaderMaximumRedColorLevel
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
        /// [BeatFader] The maximum green color level (true color)
        /// </summary>
        public static int BeatFaderMaximumGreenColorLevel
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
        /// [BeatFader] The maximum blue color level (true color)
        /// </summary>
        public static int BeatFaderMaximumBlueColorLevel
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
        /// [BeatFader] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int BeatFaderMaximumColorLevel
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
            // Variable preparations
            ConsoleBase.ConsoleWrapper.BackgroundColor = ConsoleColor.Black;
            ConsoleBase.ConsoleWrapper.Clear();
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
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic() => Animations.BeatFader.BeatFader.Simulate(BeatFaderSettingsInstance);

    }
}
