//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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

using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Misc.Screensaver;
using Terminaux.Base;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for BeatPulse
    /// </summary>
    public class BeatPulseDisplay : BaseScreensaver, IScreensaver
    {

        private Animations.BeatPulse.BeatPulseSettings? BeatPulseSettingsInstance;

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "BeatPulse";

        /// <inheritdoc/>
        public override bool ScreensaverContainsFlashingImages =>
            true;

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", vars: [ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight]);
            BeatPulseSettingsInstance = new Animations.BeatPulse.BeatPulseSettings()
            {
                BeatPulseTrueColor = ScreensaverPackInit.SaversConfig.BeatPulseTrueColor,
                BeatPulseBeatColor = ScreensaverPackInit.SaversConfig.BeatPulseBeatColor,
                BeatPulseDelay = ScreensaverPackInit.SaversConfig.BeatPulseDelay,
                BeatPulseMaxSteps = ScreensaverPackInit.SaversConfig.BeatPulseMaxSteps,
                BeatPulseCycleColors = ScreensaverPackInit.SaversConfig.BeatPulseCycleColors,
                BeatPulseMinimumRedColorLevel = ScreensaverPackInit.SaversConfig.BeatPulseMinimumRedColorLevel,
                BeatPulseMinimumGreenColorLevel = ScreensaverPackInit.SaversConfig.BeatPulseMinimumGreenColorLevel,
                BeatPulseMinimumBlueColorLevel = ScreensaverPackInit.SaversConfig.BeatPulseMinimumBlueColorLevel,
                BeatPulseMinimumColorLevel = ScreensaverPackInit.SaversConfig.BeatPulseMinimumColorLevel,
                BeatPulseMaximumRedColorLevel = ScreensaverPackInit.SaversConfig.BeatPulseMaximumRedColorLevel,
                BeatPulseMaximumGreenColorLevel = ScreensaverPackInit.SaversConfig.BeatPulseMaximumGreenColorLevel,
                BeatPulseMaximumBlueColorLevel = ScreensaverPackInit.SaversConfig.BeatPulseMaximumBlueColorLevel,
                BeatPulseMaximumColorLevel = ScreensaverPackInit.SaversConfig.BeatPulseMaximumColorLevel
            };
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            Animations.BeatPulse.BeatPulse.Simulate(BeatPulseSettingsInstance);
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.BeatPulseDelay);
        }

    }
}
