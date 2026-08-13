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
using Nitrocid.Extras.Animated.Animations.TextReveal;
using Terminaux.Base;

namespace Nitrocid.Extras.Animated.Screensavers
{
    /// <summary>
    /// Display code for TextReveal
    /// </summary>
    public class TextRevealDisplay : BaseScreensaver, IScreensaver
    {
        private TextRevealSettings? TextRevealSettingsInstance;

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            base.ScreensaverPreparation();
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", vars: [ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight]);
            TextRevealSettingsInstance = new TextRevealSettings()
            {
                TextRevealDelay = AnimatedInit.SaversConfig.TextRevealDelay,
                TextRevealWrite = AnimatedInit.SaversConfig.TextRevealWrite,
                TextRevealFadeOutDelay = AnimatedInit.SaversConfig.TextRevealFadeOutDelay,
                TextRevealNewScreenDelay = AnimatedInit.SaversConfig.TextRevealNewScreenDelay,
                TextRevealMaxSteps = AnimatedInit.SaversConfig.TextRevealMaxSteps,
                TextRevealMinimumRedColorLevel = AnimatedInit.SaversConfig.TextRevealMinimumRedColorLevel,
                TextRevealMinimumGreenColorLevel = AnimatedInit.SaversConfig.TextRevealMinimumGreenColorLevel,
                TextRevealMinimumBlueColorLevel = AnimatedInit.SaversConfig.TextRevealMinimumBlueColorLevel,
                TextRevealMaximumRedColorLevel = AnimatedInit.SaversConfig.TextRevealMaximumRedColorLevel,
                TextRevealMaximumGreenColorLevel = AnimatedInit.SaversConfig.TextRevealMaximumGreenColorLevel,
                TextRevealMaximumBlueColorLevel = AnimatedInit.SaversConfig.TextRevealMaximumBlueColorLevel
            };
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic() => TextReveal.Simulate(TextRevealSettingsInstance);

    }
}
