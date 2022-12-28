
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

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for FaderBack
    /// </summary>
    public static class FaderBackSettings
    {

        private static int _faderBackDelay = 10;
        private static int _faderBackFadeOutDelay = 3000;
        private static int _faderBackMaxSteps = 25;
        private static int _faderBackMinimumRedColorLevel = 0;
        private static int _faderBackMinimumGreenColorLevel = 0;
        private static int _faderBackMinimumBlueColorLevel = 0;
        private static int _faderBackMaximumRedColorLevel = 255;
        private static int _faderBackMaximumGreenColorLevel = 255;
        private static int _faderBackMaximumBlueColorLevel = 255;

        /// <summary>
        /// [FaderBack] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int FaderBackDelay
        {
            get
            {
                return _faderBackDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                _faderBackDelay = value;
            }
        }
        /// <summary>
        /// [FaderBack] How many milliseconds to wait before fading the text out?
        /// </summary>
        public static int FaderBackFadeOutDelay
        {
            get
            {
                return _faderBackFadeOutDelay;
            }
            set
            {
                if (value <= 0)
                    value = 3000;
                _faderBackFadeOutDelay = value;
            }
        }
        /// <summary>
        /// [FaderBack] How many fade steps to do?
        /// </summary>
        public static int FaderBackMaxSteps
        {
            get
            {
                return _faderBackMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                _faderBackMaxSteps = value;
            }
        }
        /// <summary>
        /// [FaderBack] The minimum red color level (true color)
        /// </summary>
        public static int FaderBackMinimumRedColorLevel
        {
            get
            {
                return _faderBackMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _faderBackMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FaderBack] The minimum green color level (true color)
        /// </summary>
        public static int FaderBackMinimumGreenColorLevel
        {
            get
            {
                return _faderBackMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _faderBackMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FaderBack] The minimum blue color level (true color)
        /// </summary>
        public static int FaderBackMinimumBlueColorLevel
        {
            get
            {
                return _faderBackMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _faderBackMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [FaderBack] The maximum red color level (true color)
        /// </summary>
        public static int FaderBackMaximumRedColorLevel
        {
            get
            {
                return _faderBackMaximumRedColorLevel;
            }
            set
            {
                if (value <= _faderBackMinimumRedColorLevel)
                    value = _faderBackMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                _faderBackMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FaderBack] The maximum green color level (true color)
        /// </summary>
        public static int FaderBackMaximumGreenColorLevel
        {
            get
            {
                return _faderBackMaximumGreenColorLevel;
            }
            set
            {
                if (value <= _faderBackMinimumGreenColorLevel)
                    value = _faderBackMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                _faderBackMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FaderBack] The maximum blue color level (true color)
        /// </summary>
        public static int FaderBackMaximumBlueColorLevel
        {
            get
            {
                return _faderBackMaximumBlueColorLevel;
            }
            set
            {
                if (value <= _faderBackMinimumBlueColorLevel)
                    value = _faderBackMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                _faderBackMaximumBlueColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for FaderBack
    /// </summary>
    public class FaderBackDisplay : BaseScreensaver, IScreensaver
    {

        private Animations.FaderBack.FaderBackSettings FaderBackSettingsInstance;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "FaderBack";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ConsoleBase.ConsoleWrapper.BackgroundColor = ConsoleColor.Black;
            ConsoleBase.ConsoleWrapper.Clear();
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleBase.ConsoleWrapper.WindowWidth, ConsoleBase.ConsoleWrapper.WindowHeight);
            FaderBackSettingsInstance = new Animations.FaderBack.FaderBackSettings()
            {
                FaderBackDelay = FaderBackSettings.FaderBackDelay,
                FaderBackFadeOutDelay = FaderBackSettings.FaderBackFadeOutDelay,
                FaderBackMaxSteps = FaderBackSettings.FaderBackMaxSteps,
                FaderBackMinimumRedColorLevel = FaderBackSettings.FaderBackMinimumRedColorLevel,
                FaderBackMinimumGreenColorLevel = FaderBackSettings.FaderBackMinimumGreenColorLevel,
                FaderBackMinimumBlueColorLevel = FaderBackSettings.FaderBackMinimumBlueColorLevel,
                FaderBackMaximumRedColorLevel = FaderBackSettings.FaderBackMaximumRedColorLevel,
                FaderBackMaximumGreenColorLevel = FaderBackSettings.FaderBackMaximumGreenColorLevel,
                FaderBackMaximumBlueColorLevel = FaderBackSettings.FaderBackMaximumBlueColorLevel
            };
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic() => Animations.FaderBack.FaderBack.Simulate(FaderBackSettingsInstance);

    }
}
