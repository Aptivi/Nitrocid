//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Misc.Screensaver;
using Nitrocid.Extras.Animated.Animations.BeatPulse;
using Terminaux.Base;

namespace Nitrocid.Extras.Animated.Screensavers
{
    /// <summary>
    /// Display code for BeatPulse
    /// </summary>
    public class BeatPulseDisplay : BaseScreensaver, IScreensaver
    {

        private BeatPulseSettings? BeatPulseSettingsInstance;
        /// <inheritdoc/>
        public override bool ScreensaverContainsFlashingImages =>
            true;

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", vars: [ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight]);
            BeatPulseSettingsInstance = new Animations.BeatPulse.BeatPulseSettings()
            {
                BeatPulseTrueColor = AnimatedInit.SaversConfig.BeatPulseTrueColor,
                BeatPulseBeatColor = AnimatedInit.SaversConfig.BeatPulseBeatColor,
                BeatPulseDelay = AnimatedInit.SaversConfig.BeatPulseDelay,
                BeatPulseMaxSteps = AnimatedInit.SaversConfig.BeatPulseMaxSteps,
                BeatPulseCycleColors = AnimatedInit.SaversConfig.BeatPulseCycleColors,
                BeatPulseMinimumRedColorLevel = AnimatedInit.SaversConfig.BeatPulseMinimumRedColorLevel,
                BeatPulseMinimumGreenColorLevel = AnimatedInit.SaversConfig.BeatPulseMinimumGreenColorLevel,
                BeatPulseMinimumBlueColorLevel = AnimatedInit.SaversConfig.BeatPulseMinimumBlueColorLevel,
                BeatPulseMinimumColorLevel = AnimatedInit.SaversConfig.BeatPulseMinimumColorLevel,
                BeatPulseMaximumRedColorLevel = AnimatedInit.SaversConfig.BeatPulseMaximumRedColorLevel,
                BeatPulseMaximumGreenColorLevel = AnimatedInit.SaversConfig.BeatPulseMaximumGreenColorLevel,
                BeatPulseMaximumBlueColorLevel = AnimatedInit.SaversConfig.BeatPulseMaximumBlueColorLevel,
                BeatPulseMaximumColorLevel = AnimatedInit.SaversConfig.BeatPulseMaximumColorLevel
            };
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            BeatPulse.Simulate(BeatPulseSettingsInstance);
            ScreensaverManager.Delay(AnimatedInit.SaversConfig.BeatPulseDelay);
        }

    }
}
