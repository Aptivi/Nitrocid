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

using Nitrocid.Kernel.Debugging;
using Nitrocid.Misc.Screensaver;
using Terminaux.Base;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for EdgePulse
    /// </summary>
    public class EdgePulseDisplay : BaseScreensaver, IScreensaver
    {

        private Animations.EdgePulse.EdgePulseSettings? EdgePulseSettingsInstance;

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "EdgePulse";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", vars: [ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight]);
            EdgePulseSettingsInstance = new Animations.EdgePulse.EdgePulseSettings()
            {
                EdgePulseDelay = ScreensaverPackInit.SaversConfig.EdgePulseDelay,
                EdgePulseMaxSteps = ScreensaverPackInit.SaversConfig.EdgePulseMaxSteps,
                EdgePulseMinimumRedColorLevel = ScreensaverPackInit.SaversConfig.EdgePulseMinimumRedColorLevel,
                EdgePulseMinimumGreenColorLevel = ScreensaverPackInit.SaversConfig.EdgePulseMinimumGreenColorLevel,
                EdgePulseMinimumBlueColorLevel = ScreensaverPackInit.SaversConfig.EdgePulseMinimumBlueColorLevel,
                EdgePulseMaximumRedColorLevel = ScreensaverPackInit.SaversConfig.EdgePulseMaximumRedColorLevel,
                EdgePulseMaximumGreenColorLevel = ScreensaverPackInit.SaversConfig.EdgePulseMaximumGreenColorLevel,
                EdgePulseMaximumBlueColorLevel = ScreensaverPackInit.SaversConfig.EdgePulseMaximumBlueColorLevel
            };
            ColorTools.LoadBackDry("0;0;0");
            ConsoleWrapper.Clear();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            Animations.EdgePulse.EdgePulse.Simulate(EdgePulseSettingsInstance);
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.EdgePulseDelay);
        }

    }
}
