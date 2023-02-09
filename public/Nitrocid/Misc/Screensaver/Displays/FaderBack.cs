
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

using KS.Kernel.Debugging;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for FaderBack
    /// </summary>
    public static class FaderBackSettings
    {

        private static int _Delay = 10;
        private static int _FadeOutDelay = 3000;
        private static int _MaxSteps = 25;
        private static int _MinimumRedColorLevel = 0;
        private static int _MinimumGreenColorLevel = 0;
        private static int _MinimumBlueColorLevel = 0;
        private static int _MaximumRedColorLevel = 255;
        private static int _MaximumGreenColorLevel = 255;
        private static int _MaximumBlueColorLevel = 255;

        /// <summary>
        /// [FaderBack] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int FaderBackDelay
        {
            get
            {
                return _Delay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                _Delay = value;
            }
        }
        /// <summary>
        /// [FaderBack] How many milliseconds to wait before fading the text out?
        /// </summary>
        public static int FaderBackFadeOutDelay
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
        /// [FaderBack] How many fade steps to do?
        /// </summary>
        public static int FaderBackMaxSteps
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
        /// [FaderBack] The minimum red color level (true color)
        /// </summary>
        public static int FaderBackMinimumRedColorLevel
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
        /// [FaderBack] The minimum green color level (true color)
        /// </summary>
        public static int FaderBackMinimumGreenColorLevel
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
        /// [FaderBack] The minimum blue color level (true color)
        /// </summary>
        public static int FaderBackMinimumBlueColorLevel
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
        /// [FaderBack] The maximum red color level (true color)
        /// </summary>
        public static int FaderBackMaximumRedColorLevel
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
        /// [FaderBack] The maximum green color level (true color)
        /// </summary>
        public static int FaderBackMaximumGreenColorLevel
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
        /// [FaderBack] The maximum blue color level (true color)
        /// </summary>
        public static int FaderBackMaximumBlueColorLevel
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
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic() => Animations.FaderBack.FaderBack.Simulate(FaderBackSettingsInstance);

    }
}
