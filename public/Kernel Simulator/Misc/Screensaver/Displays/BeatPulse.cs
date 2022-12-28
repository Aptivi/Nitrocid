
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
    /// Settings for BeatPulse
    /// </summary>
    public static class BeatPulseSettings
    {

        private static bool _beatpulseTrueColor = true;
        private static bool _beatpulseCycleColors = true;
        private static string _beatpulseBeatColor = 17.ToString();
        private static int _beatpulseDelay = 50;
        private static int _beatpulseMaxSteps = 25;
        private static int _beatpulseMinimumRedColorLevel = 0;
        private static int _beatpulseMinimumGreenColorLevel = 0;
        private static int _beatpulseMinimumBlueColorLevel = 0;
        private static int _beatpulseMinimumColorLevel = 0;
        private static int _beatpulseMaximumRedColorLevel = 255;
        private static int _beatpulseMaximumGreenColorLevel = 255;
        private static int _beatpulseMaximumBlueColorLevel = 255;
        private static int _beatpulseMaximumColorLevel = 255;

        /// <summary>
        /// [BeatPulse] Enable truecolor support. Has a higher priority than 255 color support. Please note that it only works if color cycling is enabled.
        /// </summary>
        public static bool BeatPulseTrueColor
        {
            get
            {
                return _beatpulseTrueColor;
            }
            set
            {
                _beatpulseTrueColor = value;
            }
        }
        /// <summary>
        /// [BeatPulse] Enable color cycling (uses RNG. If disabled, uses the <see cref="BeatPulseBeatColor"/> color.)
        /// </summary>
        public static bool BeatPulseCycleColors
        {
            get
            {
                return _beatpulseCycleColors;
            }
            set
            {
                _beatpulseCycleColors = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The color of beats. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        /// </summary>
        public static string BeatPulseBeatColor
        {
            get
            {
                return _beatpulseBeatColor;
            }
            set
            {
                _beatpulseBeatColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [BeatPulse] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int BeatPulseDelay
        {
            get
            {
                return _beatpulseDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                _beatpulseDelay = value;
            }
        }
        /// <summary>
        /// [BeatPulse] How many fade steps to do?
        /// </summary>
        public static int BeatPulseMaxSteps
        {
            get
            {
                return _beatpulseMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                _beatpulseMaxSteps = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The minimum red color level (true color)
        /// </summary>
        public static int BeatPulseMinimumRedColorLevel
        {
            get
            {
                return _beatpulseMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _beatpulseMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The minimum green color level (true color)
        /// </summary>
        public static int BeatPulseMinimumGreenColorLevel
        {
            get
            {
                return _beatpulseMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _beatpulseMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The minimum blue color level (true color)
        /// </summary>
        public static int BeatPulseMinimumBlueColorLevel
        {
            get
            {
                return _beatpulseMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _beatpulseMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int BeatPulseMinimumColorLevel
        {
            get
            {
                return _beatpulseMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                _beatpulseMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The maximum red color level (true color)
        /// </summary>
        public static int BeatPulseMaximumRedColorLevel
        {
            get
            {
                return _beatpulseMaximumRedColorLevel;
            }
            set
            {
                if (value <= _beatpulseMinimumRedColorLevel)
                    value = _beatpulseMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                _beatpulseMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The maximum green color level (true color)
        /// </summary>
        public static int BeatPulseMaximumGreenColorLevel
        {
            get
            {
                return _beatpulseMaximumGreenColorLevel;
            }
            set
            {
                if (value <= _beatpulseMinimumGreenColorLevel)
                    value = _beatpulseMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                _beatpulseMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The maximum blue color level (true color)
        /// </summary>
        public static int BeatPulseMaximumBlueColorLevel
        {
            get
            {
                return _beatpulseMaximumBlueColorLevel;
            }
            set
            {
                if (value <= _beatpulseMinimumBlueColorLevel)
                    value = _beatpulseMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                _beatpulseMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BeatPulse] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int BeatPulseMaximumColorLevel
        {
            get
            {
                return _beatpulseMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= _beatpulseMinimumColorLevel)
                    value = _beatpulseMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                _beatpulseMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for BeatPulse
    /// </summary>
    public class BeatPulseDisplay : BaseScreensaver, IScreensaver
    {

        private Animations.BeatPulse.BeatPulseSettings BeatPulseSettingsInstance;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "BeatPulse";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ConsoleBase.ConsoleWrapper.BackgroundColor = ConsoleColor.Black;
            ConsoleBase.ConsoleWrapper.ForegroundColor = ConsoleColor.White;
            ConsoleBase.ConsoleWrapper.Clear();
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleBase.ConsoleWrapper.WindowWidth, ConsoleBase.ConsoleWrapper.WindowHeight);
            BeatPulseSettingsInstance = new Animations.BeatPulse.BeatPulseSettings()
            {
                BeatPulseTrueColor = BeatPulseSettings.BeatPulseTrueColor,
                BeatPulseBeatColor = BeatPulseSettings.BeatPulseBeatColor,
                BeatPulseDelay = BeatPulseSettings.BeatPulseDelay,
                BeatPulseMaxSteps = BeatPulseSettings.BeatPulseMaxSteps,
                BeatPulseCycleColors = BeatPulseSettings.BeatPulseCycleColors,
                BeatPulseMinimumRedColorLevel = BeatPulseSettings.BeatPulseMinimumRedColorLevel,
                BeatPulseMinimumGreenColorLevel = BeatPulseSettings.BeatPulseMinimumGreenColorLevel,
                BeatPulseMinimumBlueColorLevel = BeatPulseSettings.BeatPulseMinimumBlueColorLevel,
                BeatPulseMinimumColorLevel = BeatPulseSettings.BeatPulseMinimumColorLevel,
                BeatPulseMaximumRedColorLevel = BeatPulseSettings.BeatPulseMaximumRedColorLevel,
                BeatPulseMaximumGreenColorLevel = BeatPulseSettings.BeatPulseMaximumGreenColorLevel,
                BeatPulseMaximumBlueColorLevel = BeatPulseSettings.BeatPulseMaximumBlueColorLevel,
                BeatPulseMaximumColorLevel = BeatPulseSettings.BeatPulseMaximumColorLevel
            };
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            Animations.BeatPulse.BeatPulse.Simulate(BeatPulseSettingsInstance);
            ThreadManager.SleepNoBlock(BeatPulseSettings.BeatPulseDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
