
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
using KS.Misc.Threading;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for BeatEdgePulse
    /// </summary>
    public static class BeatEdgePulseSettings
    {

        private static bool _beatedgepulseTrueColor = true;
        private static bool _beatedgepulseCycleColors = true;
        private static string _beatedgepulseBeatColor = 17.ToString();
        private static int _beatedgepulseDelay = 50;
        private static int _beatedgepulseMaxSteps = 25;
        private static int _beatedgepulseMinimumRedColorLevel = 0;
        private static int _beatedgepulseMinimumGreenColorLevel = 0;
        private static int _beatedgepulseMinimumBlueColorLevel = 0;
        private static int _beatedgepulseMinimumColorLevel = 0;
        private static int _beatedgepulseMaximumRedColorLevel = 255;
        private static int _beatedgepulseMaximumGreenColorLevel = 255;
        private static int _beatedgepulseMaximumBlueColorLevel = 255;
        private static int _beatedgepulseMaximumColorLevel = 255;

        /// <summary>
        /// [BeatEdgePulse] Enable truecolor support. Has a higher priority than 255 color support. Please note that it only works if color cycling is enabled.
        /// </summary>
        public static bool BeatEdgePulseTrueColor
        {
            get
            {
                return _beatedgepulseTrueColor;
            }
            set
            {
                _beatedgepulseTrueColor = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] Enable color cycling (uses RNG. If disabled, uses the <see cref="BeatEdgePulseBeatColor"/> color.)
        /// </summary>
        public static bool BeatEdgePulseCycleColors
        {
            get
            {
                return _beatedgepulseCycleColors;
            }
            set
            {
                _beatedgepulseCycleColors = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] The color of beats. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public static string BeatEdgePulseBeatColor
        {
            get
            {
                return _beatedgepulseBeatColor;
            }
            set
            {
                _beatedgepulseBeatColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int BeatEdgePulseDelay
        {
            get
            {
                return _beatedgepulseDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                _beatedgepulseDelay = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] How many fade steps to do?
        /// </summary>
        public static int BeatEdgePulseMaxSteps
        {
            get
            {
                return _beatedgepulseMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                _beatedgepulseMaxSteps = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] The minimum red color level (true color)
        /// </summary>
        public static int BeatEdgePulseMinimumRedColorLevel
        {
            get
            {
                return _beatedgepulseMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _beatedgepulseMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] The minimum green color level (true color)
        /// </summary>
        public static int BeatEdgePulseMinimumGreenColorLevel
        {
            get
            {
                return _beatedgepulseMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _beatedgepulseMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] The minimum blue color level (true color)
        /// </summary>
        public static int BeatEdgePulseMinimumBlueColorLevel
        {
            get
            {
                return _beatedgepulseMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _beatedgepulseMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int BeatEdgePulseMinimumColorLevel
        {
            get
            {
                return _beatedgepulseMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                _beatedgepulseMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] The maximum red color level (true color)
        /// </summary>
        public static int BeatEdgePulseMaximumRedColorLevel
        {
            get
            {
                return _beatedgepulseMaximumRedColorLevel;
            }
            set
            {
                if (value <= _beatedgepulseMinimumRedColorLevel)
                    value = _beatedgepulseMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                _beatedgepulseMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] The maximum green color level (true color)
        /// </summary>
        public static int BeatEdgePulseMaximumGreenColorLevel
        {
            get
            {
                return _beatedgepulseMaximumGreenColorLevel;
            }
            set
            {
                if (value <= _beatedgepulseMinimumGreenColorLevel)
                    value = _beatedgepulseMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                _beatedgepulseMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] The maximum blue color level (true color)
        /// </summary>
        public static int BeatEdgePulseMaximumBlueColorLevel
        {
            get
            {
                return _beatedgepulseMaximumBlueColorLevel;
            }
            set
            {
                if (value <= _beatedgepulseMinimumBlueColorLevel)
                    value = _beatedgepulseMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                _beatedgepulseMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatEdgePulse] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int BeatEdgePulseMaximumColorLevel
        {
            get
            {
                return _beatedgepulseMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= _beatedgepulseMinimumColorLevel)
                    value = _beatedgepulseMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                _beatedgepulseMaximumColorLevel = value;
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
            // Variable preparations
            ConsoleBase.ConsoleWrapper.BackgroundColor = ConsoleColor.Black;
            ConsoleBase.ConsoleWrapper.ForegroundColor = ConsoleColor.White;
            ConsoleBase.ConsoleWrapper.Clear();
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
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            Animations.BeatEdgePulse.BeatEdgePulse.Simulate(BeatEdgePulseSettingsInstance);
            ThreadManager.SleepNoBlock(BeatEdgePulseSettings.BeatEdgePulseDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
