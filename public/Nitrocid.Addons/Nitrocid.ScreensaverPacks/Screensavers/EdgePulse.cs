//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.Kernel.Debugging;
using KS.Kernel.Threading;
using KS.Misc.Screensaver;

namespace Nitrocid.ScreensaverPacks.Screensavers
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
                return ScreensaverPackInit.SaversConfig.EdgePulseDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                ScreensaverPackInit.SaversConfig.EdgePulseDelay = value;
            }
        }
        /// <summary>
        /// [EdgePulse] How many fade steps to do?
        /// </summary>
        public static int EdgePulseMaxSteps
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.EdgePulseMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                ScreensaverPackInit.SaversConfig.EdgePulseMaxSteps = value;
            }
        }
        /// <summary>
        /// [EdgePulse] The minimum red color level (true color)
        /// </summary>
        public static int EdgePulseMinimumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.EdgePulseMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.EdgePulseMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [EdgePulse] The minimum green color level (true color)
        /// </summary>
        public static int EdgePulseMinimumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.EdgePulseMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.EdgePulseMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [EdgePulse] The minimum blue color level (true color)
        /// </summary>
        public static int EdgePulseMinimumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.EdgePulseMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.EdgePulseMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [EdgePulse] The maximum red color level (true color)
        /// </summary>
        public static int EdgePulseMaximumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.EdgePulseMaximumRedColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.EdgePulseMinimumRedColorLevel)
                    value = ScreensaverPackInit.SaversConfig.EdgePulseMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.EdgePulseMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [EdgePulse] The maximum green color level (true color)
        /// </summary>
        public static int EdgePulseMaximumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.EdgePulseMaximumGreenColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.EdgePulseMinimumGreenColorLevel)
                    value = ScreensaverPackInit.SaversConfig.EdgePulseMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.EdgePulseMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [EdgePulse] The maximum blue color level (true color)
        /// </summary>
        public static int EdgePulseMaximumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.EdgePulseMaximumBlueColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.EdgePulseMinimumBlueColorLevel)
                    value = ScreensaverPackInit.SaversConfig.EdgePulseMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.EdgePulseMaximumBlueColorLevel = value;
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
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);
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
            KernelColorTools.LoadBack("0;0;0");
            ConsoleWrapper.Clear();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            Animations.EdgePulse.EdgePulse.Simulate(EdgePulseSettingsInstance);
            ThreadManager.SleepNoBlock(EdgePulseSettings.EdgePulseDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
