
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

using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Kernel.Threading;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for EdgePulse
    /// </summary>
    public static class EdgePulseSettings
    {

        /// <summary>
        /// [EdgePulse] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int EdgePulseDelay
        {
            get
            {
                return Config.SaverConfig.EdgePulseDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                Config.SaverConfig.EdgePulseDelay = value;
            }
        }
        /// <summary>
        /// [EdgePulse] How many fade steps to do?
        /// </summary>
        public static int EdgePulseMaxSteps
        {
            get
            {
                return Config.SaverConfig.EdgePulseMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                Config.SaverConfig.EdgePulseMaxSteps = value;
            }
        }
        /// <summary>
        /// [EdgePulse] The minimum red color level (true color)
        /// </summary>
        public static int EdgePulseMinimumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.EdgePulseMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.EdgePulseMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [EdgePulse] The minimum green color level (true color)
        /// </summary>
        public static int EdgePulseMinimumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.EdgePulseMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.EdgePulseMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [EdgePulse] The minimum blue color level (true color)
        /// </summary>
        public static int EdgePulseMinimumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.EdgePulseMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.EdgePulseMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [EdgePulse] The maximum red color level (true color)
        /// </summary>
        public static int EdgePulseMaximumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.EdgePulseMaximumRedColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.EdgePulseMinimumRedColorLevel)
                    value = Config.SaverConfig.EdgePulseMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.EdgePulseMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [EdgePulse] The maximum green color level (true color)
        /// </summary>
        public static int EdgePulseMaximumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.EdgePulseMaximumGreenColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.EdgePulseMinimumGreenColorLevel)
                    value = Config.SaverConfig.EdgePulseMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.EdgePulseMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [EdgePulse] The maximum blue color level (true color)
        /// </summary>
        public static int EdgePulseMaximumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.EdgePulseMaximumBlueColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.EdgePulseMinimumBlueColorLevel)
                    value = Config.SaverConfig.EdgePulseMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.EdgePulseMaximumBlueColorLevel = value;
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
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            Animations.EdgePulse.EdgePulse.Simulate(EdgePulseSettingsInstance);
            ThreadManager.SleepNoBlock(EdgePulseSettings.EdgePulseDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
