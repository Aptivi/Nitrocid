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
    /// Display code for TextReveal
    /// </summary>
    public class TextRevealDisplay : BaseScreensaver, IScreensaver
    {
        // TODO: NKS_SCREENSAVERPACKS_TEXTREVEAL_SETTINGS_DESC -> Reveals the text printed in the background
        private Animations.TextReveal.TextRevealSettings? TextRevealSettingsInstance;

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "TextReveal";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            base.ScreensaverPreparation();
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", vars: [ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight]);
            TextRevealSettingsInstance = new Animations.TextReveal.TextRevealSettings()
            {
                TextRevealDelay = ScreensaverPackInit.SaversConfig.TextRevealDelay,
                TextRevealWrite = ScreensaverPackInit.SaversConfig.TextRevealWrite,
                TextRevealFadeOutDelay = ScreensaverPackInit.SaversConfig.TextRevealFadeOutDelay,
                TextRevealNewScreenDelay = ScreensaverPackInit.SaversConfig.TextRevealNewScreenDelay,
                TextRevealMaxSteps = ScreensaverPackInit.SaversConfig.TextRevealMaxSteps,
                TextRevealMinimumRedColorLevel = ScreensaverPackInit.SaversConfig.TextRevealMinimumRedColorLevel,
                TextRevealMinimumGreenColorLevel = ScreensaverPackInit.SaversConfig.TextRevealMinimumGreenColorLevel,
                TextRevealMinimumBlueColorLevel = ScreensaverPackInit.SaversConfig.TextRevealMinimumBlueColorLevel,
                TextRevealMaximumRedColorLevel = ScreensaverPackInit.SaversConfig.TextRevealMaximumRedColorLevel,
                TextRevealMaximumGreenColorLevel = ScreensaverPackInit.SaversConfig.TextRevealMaximumGreenColorLevel,
                TextRevealMaximumBlueColorLevel = ScreensaverPackInit.SaversConfig.TextRevealMaximumBlueColorLevel
            };
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic() => Animations.TextReveal.TextReveal.Simulate(TextRevealSettingsInstance);

    }
}
