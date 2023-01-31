
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
using KS.Kernel.Debugging;
using KS.Misc.Threading;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for EdgePulse
    /// </summary>
    public static class EdgePulseSettings
    {

        private static int _Delay = 50;
        private static int _MaxSteps = 25;
        private static int _MinimumRedColorLevel = 0;
        private static int _MinimumGreenColorLevel = 0;
        private static int _MinimumBlueColorLevel = 0;
        private static int _MaximumRedColorLevel = 255;
        private static int _MaximumGreenColorLevel = 255;
        private static int _MaximumBlueColorLevel = 255;

        /// <summary>
        /// [EdgePulse] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int EdgePulseDelay
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
        /// [EdgePulse] How many fade steps to do?
        /// </summary>
        public static int EdgePulseMaxSteps
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
        /// [EdgePulse] The minimum red color level (true color)
        /// </summary>
        public static int EdgePulseMinimumRedColorLevel
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
        /// [EdgePulse] The minimum green color level (true color)
        /// </summary>
        public static int EdgePulseMinimumGreenColorLevel
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
        /// [EdgePulse] The minimum blue color level (true color)
        /// </summary>
        public static int EdgePulseMinimumBlueColorLevel
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
        /// [EdgePulse] The maximum red color level (true color)
        /// </summary>
        public static int EdgePulseMaximumRedColorLevel
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
        /// [EdgePulse] The maximum green color level (true color)
        /// </summary>
        public static int EdgePulseMaximumGreenColorLevel
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
        /// [EdgePulse] The maximum blue color level (true color)
        /// </summary>
        public static int EdgePulseMaximumBlueColorLevel
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
    /// Display code for EdgePulse
    /// </summary>
    public class EdgePulseDisplay : BaseScreensaver, IScreensaver
    {

        private Animations.EdgePulse.EdgePulseSettings EdgePulseSettingsInstance;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "EdgePulse";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ConsoleBase.ConsoleWrapper.BackgroundColor = ConsoleColor.Black;
            ConsoleBase.ConsoleWrapper.ForegroundColor = ConsoleColor.White;
            ConsoleBase.ConsoleWrapper.Clear();
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleBase.ConsoleWrapper.WindowWidth, ConsoleBase.ConsoleWrapper.WindowHeight);
            EdgePulseSettingsInstance = new Animations.EdgePulse.EdgePulseSettings()
            {
                EdgePulseDelay = EdgePulseSettings.EdgePulseDelay,
                EdgePulseMaxSteps = EdgePulseSettings.EdgePulseMaxSteps,
                EdgePulseMinimumRedColorLevel = EdgePulseSettings.EdgePulseMinimumRedColorLevel,
                EdgePulseMinimumGreenColorLevel = EdgePulseSettings.EdgePulseMinimumGreenColorLevel,
                EdgePulseMinimumBlueColorLevel = EdgePulseSettings.EdgePulseMinimumBlueColorLevel,
                EdgePulseMaximumRedColorLevel = EdgePulseSettings.EdgePulseMaximumRedColorLevel,
                EdgePulseMaximumGreenColorLevel = EdgePulseSettings.EdgePulseMaximumGreenColorLevel,
                EdgePulseMaximumBlueColorLevel = EdgePulseSettings.EdgePulseMaximumBlueColorLevel
            };
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            Animations.EdgePulse.EdgePulse.Simulate(EdgePulseSettingsInstance);
            ThreadManager.SleepNoBlock(EdgePulseSettings.EdgePulseDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
