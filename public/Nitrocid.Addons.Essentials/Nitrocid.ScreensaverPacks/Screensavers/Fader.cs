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
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for Fader
    /// </summary>
    public class FaderDisplay : BaseScreensaver, IScreensaver
    {

        private Animations.Fader.FaderSettings? FaderSettingsInstance;

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "Fader";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            base.ScreensaverPreparation();
            ColorTools.LoadBackDry(new Color(ScreensaverPackInit.SaversConfig.FaderBackgroundColor));
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", vars: [ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight]);
            FaderSettingsInstance = new Animations.Fader.FaderSettings()
            {
                FaderDelay = ScreensaverPackInit.SaversConfig.FaderDelay,
                FaderWrite = ScreensaverPackInit.SaversConfig.FaderWrite,
                FaderBackgroundColor = ScreensaverPackInit.SaversConfig.FaderBackgroundColor,
                FaderFadeOutDelay = ScreensaverPackInit.SaversConfig.FaderFadeOutDelay,
                FaderMaxSteps = ScreensaverPackInit.SaversConfig.FaderMaxSteps,
                FaderMinimumRedColorLevel = ScreensaverPackInit.SaversConfig.FaderMinimumRedColorLevel,
                FaderMinimumGreenColorLevel = ScreensaverPackInit.SaversConfig.FaderMinimumGreenColorLevel,
                FaderMinimumBlueColorLevel = ScreensaverPackInit.SaversConfig.FaderMinimumBlueColorLevel,
                FaderMaximumRedColorLevel = ScreensaverPackInit.SaversConfig.FaderMaximumRedColorLevel,
                FaderMaximumGreenColorLevel = ScreensaverPackInit.SaversConfig.FaderMaximumGreenColorLevel,
                FaderMaximumBlueColorLevel = ScreensaverPackInit.SaversConfig.FaderMaximumBlueColorLevel
            };
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic() => Animations.Fader.Fader.Simulate(FaderSettingsInstance);

    }
}
