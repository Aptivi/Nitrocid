
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
using KS.Kernel.Debugging;
using KS.Misc.Threading;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for BeatEdgePulse
    /// </summary>
    public static class BeatEdgePulseSettings
    {

        private static bool _TrueColor = true;
        private static bool _CycleColors = true;
        private static string _BeatColor = "17";
        private static int _Delay = 50;
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
        /// [BeatEdgePulse] Enable truecolor support. Has a higher priority than 255 color support. Please note that it only works if color cycling is enabled.
        /// </summary>
        public static bool BeatEdgePulseTrueColor
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
        /// [BeatEdgePulse] Enable color cycling (uses RNG. If disabled, uses the <see cref="BeatEdgePulseBeatColor"/> color.)
        /// </summary>
        public static bool BeatEdgePulseCycleColors
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
        /// [BeatEdgePulse] The color of beats. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public static string BeatEdgePulseBeatColor
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
        /// [BeatEdgePulse] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int BeatEdgePulseDelay
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
        /// [BeatEdgePulse] How many fade steps to do?
        /// </summary>
        public static int BeatEdgePulseMaxSteps
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
        /// [BeatEdgePulse] The minimum red color level (true color)
        /// </summary>
        public static int BeatEdgePulseMinimumRedColorLevel
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
        /// [BeatEdgePulse] The minimum green color level (true color)
        /// </summary>
        public static int BeatEdgePulseMinimumGreenColorLevel
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
        /// [BeatEdgePulse] The minimum blue color level (true color)
        /// </summary>
        public static int BeatEdgePulseMinimumBlueColorLevel
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
        /// [BeatEdgePulse] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int BeatEdgePulseMinimumColorLevel
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
        /// [BeatEdgePulse] The maximum red color level (true color)
        /// </summary>
        public static int BeatEdgePulseMaximumRedColorLevel
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
        /// [BeatEdgePulse] The maximum green color level (true color)
        /// </summary>
        public static int BeatEdgePulseMaximumGreenColorLevel
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
        /// [BeatEdgePulse] The maximum blue color level (true color)
        /// </summary>
        public static int BeatEdgePulseMaximumBlueColorLevel
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
        /// [BeatEdgePulse] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int BeatEdgePulseMaximumColorLevel
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
    /// Display code for BeatEdgePulse
    /// </summary>
    public class BeatEdgePulseDisplay : BaseScreensaver, IScreensaver
    {

        private Animations.BeatEdgePulse.BeatEdgePulseSettings BeatEdgePulseSettingsInstance;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "BeatEdgePulse";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleBase.ConsoleWrapper.WindowWidth, ConsoleBase.ConsoleWrapper.WindowHeight);
            BeatEdgePulseSettingsInstance = new Animations.BeatEdgePulse.BeatEdgePulseSettings()
            {
                BeatEdgePulseTrueColor = BeatEdgePulseSettings.BeatEdgePulseTrueColor,
                BeatEdgePulseBeatColor = BeatEdgePulseSettings.BeatEdgePulseBeatColor,
                BeatEdgePulseDelay = BeatEdgePulseSettings.BeatEdgePulseDelay,
                BeatEdgePulseMaxSteps = BeatEdgePulseSettings.BeatEdgePulseMaxSteps,
                BeatEdgePulseCycleColors = BeatEdgePulseSettings.BeatEdgePulseCycleColors,
                BeatEdgePulseMinimumRedColorLevel = BeatEdgePulseSettings.BeatEdgePulseMinimumRedColorLevel,
                BeatEdgePulseMinimumGreenColorLevel = BeatEdgePulseSettings.BeatEdgePulseMinimumGreenColorLevel,
                BeatEdgePulseMinimumBlueColorLevel = BeatEdgePulseSettings.BeatEdgePulseMinimumBlueColorLevel,
                BeatEdgePulseMinimumColorLevel = BeatEdgePulseSettings.BeatEdgePulseMinimumColorLevel,
                BeatEdgePulseMaximumRedColorLevel = BeatEdgePulseSettings.BeatEdgePulseMaximumRedColorLevel,
                BeatEdgePulseMaximumGreenColorLevel = BeatEdgePulseSettings.BeatEdgePulseMaximumGreenColorLevel,
                BeatEdgePulseMaximumBlueColorLevel = BeatEdgePulseSettings.BeatEdgePulseMaximumBlueColorLevel,
                BeatEdgePulseMaximumColorLevel = BeatEdgePulseSettings.BeatEdgePulseMaximumColorLevel
            };
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            Animations.BeatEdgePulse.BeatEdgePulse.Simulate(BeatEdgePulseSettingsInstance);
            ThreadManager.SleepNoBlock(BeatEdgePulseSettings.BeatEdgePulseDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
