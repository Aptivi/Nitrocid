
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
using KS.Kernel.Debugging;
using KS.Misc.Threading;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for Pulse
    /// </summary>
    public static class PulseSettings
    {

        private static int _pulseDelay = 50;
        private static int _pulseMaxSteps = 25;
        private static int _pulseMinimumRedColorLevel = 0;
        private static int _pulseMinimumGreenColorLevel = 0;
        private static int _pulseMinimumBlueColorLevel = 0;
        private static int _pulseMaximumRedColorLevel = 255;
        private static int _pulseMaximumGreenColorLevel = 255;
        private static int _pulseMaximumBlueColorLevel = 255;

        /// <summary>
        /// [Pulse] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int PulseDelay
        {
            get
            {
                return _pulseDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                _pulseDelay = value;
            }
        }
        /// <summary>
        /// [Pulse] How many fade steps to do?
        /// </summary>
        public static int PulseMaxSteps
        {
            get
            {
                return _pulseMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                _pulseMaxSteps = value;
            }
        }
        /// <summary>
        /// [Pulse] The minimum red color level (true color)
        /// </summary>
        public static int PulseMinimumRedColorLevel
        {
            get
            {
                return _pulseMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _pulseMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Pulse] The minimum green color level (true color)
        /// </summary>
        public static int PulseMinimumGreenColorLevel
        {
            get
            {
                return _pulseMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _pulseMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Pulse] The minimum blue color level (true color)
        /// </summary>
        public static int PulseMinimumBlueColorLevel
        {
            get
            {
                return _pulseMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _pulseMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Pulse] The maximum red color level (true color)
        /// </summary>
        public static int PulseMaximumRedColorLevel
        {
            get
            {
                return _pulseMaximumRedColorLevel;
            }
            set
            {
                if (value <= _pulseMinimumRedColorLevel)
                    value = _pulseMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                _pulseMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Pulse] The maximum green color level (true color)
        /// </summary>
        public static int PulseMaximumGreenColorLevel
        {
            get
            {
                return _pulseMaximumGreenColorLevel;
            }
            set
            {
                if (value <= _pulseMinimumGreenColorLevel)
                    value = _pulseMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                _pulseMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Pulse] The maximum blue color level (true color)
        /// </summary>
        public static int PulseMaximumBlueColorLevel
        {
            get
            {
                return _pulseMaximumBlueColorLevel;
            }
            set
            {
                if (value <= _pulseMinimumBlueColorLevel)
                    value = _pulseMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                _pulseMaximumBlueColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for Pulse
    /// </summary>
    public class PulseDisplay : BaseScreensaver, IScreensaver
    {

        private Animations.Pulse.PulseSettings PulseSettingsInstance;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Pulse";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ConsoleBase.ConsoleWrapper.BackgroundColor = ConsoleColor.Black;
            ConsoleBase.ConsoleWrapper.ForegroundColor = ConsoleColor.White;
            ConsoleBase.ConsoleWrapper.Clear();
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleBase.ConsoleWrapper.WindowWidth, ConsoleBase.ConsoleWrapper.WindowHeight);
            PulseSettingsInstance = new Animations.Pulse.PulseSettings()
            {
                PulseDelay = PulseSettings.PulseDelay,
                PulseMaxSteps = PulseSettings.PulseMaxSteps,
                PulseMinimumRedColorLevel = PulseSettings.PulseMinimumRedColorLevel,
                PulseMinimumGreenColorLevel = PulseSettings.PulseMinimumGreenColorLevel,
                PulseMinimumBlueColorLevel = PulseSettings.PulseMinimumBlueColorLevel,
                PulseMaximumRedColorLevel = PulseSettings.PulseMaximumRedColorLevel,
                PulseMaximumGreenColorLevel = PulseSettings.PulseMaximumGreenColorLevel,
                PulseMaximumBlueColorLevel = PulseSettings.PulseMaximumBlueColorLevel
            };
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            Animations.Pulse.Pulse.Simulate(PulseSettingsInstance);
            ThreadManager.SleepNoBlock(PulseSettings.PulseDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
