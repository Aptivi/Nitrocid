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

using Nitrocid.Kernel.Debugging;
using Nitrocid.Misc.Screensaver;
using Terminaux.Base;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for FaderBack
    /// </summary>
    public class FaderBackDisplay : BaseScreensaver, IScreensaver
    {

        private Animations.FaderBack.FaderBackSettings? FaderBackSettingsInstance;

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "FaderBack";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", vars: [ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight]);
            FaderBackSettingsInstance = new Animations.FaderBack.FaderBackSettings()
            {
                FaderBackDelay = ScreensaverPackInit.SaversConfig.FaderBackDelay,
                FaderBackFadeOutDelay = ScreensaverPackInit.SaversConfig.FaderBackFadeOutDelay,
                FaderBackMaxSteps = ScreensaverPackInit.SaversConfig.FaderBackMaxSteps,
                FaderBackMinimumRedColorLevel = ScreensaverPackInit.SaversConfig.FaderBackMinimumRedColorLevel,
                FaderBackMinimumGreenColorLevel = ScreensaverPackInit.SaversConfig.FaderBackMinimumGreenColorLevel,
                FaderBackMinimumBlueColorLevel = ScreensaverPackInit.SaversConfig.FaderBackMinimumBlueColorLevel,
                FaderBackMaximumRedColorLevel = ScreensaverPackInit.SaversConfig.FaderBackMaximumRedColorLevel,
                FaderBackMaximumGreenColorLevel = ScreensaverPackInit.SaversConfig.FaderBackMaximumGreenColorLevel,
                FaderBackMaximumBlueColorLevel = ScreensaverPackInit.SaversConfig.FaderBackMaximumBlueColorLevel
            };
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic() => Animations.FaderBack.FaderBack.Simulate(FaderBackSettingsInstance);

    }
}
