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
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Colorimetry;
using Nitrocid.Extras.Animated.Animations.Fader;

namespace Nitrocid.Extras.Animated.Screensavers
{
    /// <summary>
    /// Display code for Fader
    /// </summary>
    public class FaderDisplay : BaseScreensaver, IScreensaver
    {

        private FaderSettings? FaderSettingsInstance;

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            base.ScreensaverPreparation();
            ConsoleColoring.LoadBackDry(new Color(AnimatedInit.SaversConfig.FaderBackgroundColor));
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", vars: [ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight]);
            FaderSettingsInstance = new FaderSettings()
            {
                FaderDelay = AnimatedInit.SaversConfig.FaderDelay,
                FaderWrite = AnimatedInit.SaversConfig.FaderWrite,
                FaderBackgroundColor = AnimatedInit.SaversConfig.FaderBackgroundColor,
                FaderFadeOutDelay = AnimatedInit.SaversConfig.FaderFadeOutDelay,
                FaderMaxSteps = AnimatedInit.SaversConfig.FaderMaxSteps,
                FaderMinimumRedColorLevel = AnimatedInit.SaversConfig.FaderMinimumRedColorLevel,
                FaderMinimumGreenColorLevel = AnimatedInit.SaversConfig.FaderMinimumGreenColorLevel,
                FaderMinimumBlueColorLevel = AnimatedInit.SaversConfig.FaderMinimumBlueColorLevel,
                FaderMaximumRedColorLevel = AnimatedInit.SaversConfig.FaderMaximumRedColorLevel,
                FaderMaximumGreenColorLevel = AnimatedInit.SaversConfig.FaderMaximumGreenColorLevel,
                FaderMaximumBlueColorLevel = AnimatedInit.SaversConfig.FaderMaximumBlueColorLevel
            };
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic() => Fader.Simulate(FaderSettingsInstance);

    }
}
