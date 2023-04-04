
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
using KS.Misc.Threading;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for Pulse
    /// </summary>
    public static class PulseSettings
    {

        /// <summary>
        /// [Pulse] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int PulseDelay
        {
            get
            {
                return Config.SaverConfig.PulseDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                Config.SaverConfig.PulseDelay = value;
            }
        }
        /// <summary>
        /// [Pulse] How many fade steps to do?
        /// </summary>
        public static int PulseMaxSteps
        {
            get
            {
                return Config.SaverConfig.PulseMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                Config.SaverConfig.PulseMaxSteps = value;
            }
        }
        /// <summary>
        /// [Pulse] The minimum red color level (true color)
        /// </summary>
        public static int PulseMinimumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.PulseMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.PulseMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Pulse] The minimum green color level (true color)
        /// </summary>
        public static int PulseMinimumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.PulseMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.PulseMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Pulse] The minimum blue color level (true color)
        /// </summary>
        public static int PulseMinimumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.PulseMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.PulseMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Pulse] The maximum red color level (true color)
        /// </summary>
        public static int PulseMaximumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.PulseMaximumRedColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.PulseMinimumRedColorLevel)
                    value = Config.SaverConfig.PulseMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.PulseMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Pulse] The maximum green color level (true color)
        /// </summary>
        public static int PulseMaximumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.PulseMaximumGreenColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.PulseMinimumGreenColorLevel)
                    value = Config.SaverConfig.PulseMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.PulseMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Pulse] The maximum blue color level (true color)
        /// </summary>
        public static int PulseMaximumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.PulseMaximumBlueColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.PulseMinimumBlueColorLevel)
                    value = Config.SaverConfig.PulseMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.PulseMaximumBlueColorLevel = value;
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
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            Animations.Pulse.Pulse.Simulate(PulseSettingsInstance);
            ThreadManager.SleepNoBlock(PulseSettings.PulseDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
