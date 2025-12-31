//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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
    /// Display code for Pulse
    /// </summary>
    public class PulseDisplay : BaseScreensaver, IScreensaver
    {

        private Animations.Pulse.PulseSettings? PulseSettingsInstance;

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "Pulse";

        /// <inheritdoc/>
        public override bool ScreensaverContainsFlashingImages =>
            true;

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", vars: [ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight]);
            PulseSettingsInstance = new Animations.Pulse.PulseSettings()
            {
                PulseDelay = ScreensaverPackInit.SaversConfig.PulseDelay,
                PulseMaxSteps = ScreensaverPackInit.SaversConfig.PulseMaxSteps,
                PulseMinimumRedColorLevel = ScreensaverPackInit.SaversConfig.PulseMinimumRedColorLevel,
                PulseMinimumGreenColorLevel = ScreensaverPackInit.SaversConfig.PulseMinimumGreenColorLevel,
                PulseMinimumBlueColorLevel = ScreensaverPackInit.SaversConfig.PulseMinimumBlueColorLevel,
                PulseMaximumRedColorLevel = ScreensaverPackInit.SaversConfig.PulseMaximumRedColorLevel,
                PulseMaximumGreenColorLevel = ScreensaverPackInit.SaversConfig.PulseMaximumGreenColorLevel,
                PulseMaximumBlueColorLevel = ScreensaverPackInit.SaversConfig.PulseMaximumBlueColorLevel
            };
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            Animations.Pulse.Pulse.Simulate(PulseSettingsInstance);
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.PulseDelay);
        }

    }
}
