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
using Nitrocid.Extras.Animated.Animations.BeatFader;
using Terminaux.Base;

namespace Nitrocid.Extras.Animated.Screensavers
{
    /// <summary>
    /// Display code for BeatFader
    /// </summary>
    public class BeatFaderDisplay : BaseScreensaver, IScreensaver
    {

        private Animations.BeatFader.BeatFaderSettings? BeatFaderSettingsInstance;
        /// <inheritdoc/>
        public override bool ScreensaverContainsFlashingImages =>
            true;

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", vars: [ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight]);
            BeatFaderSettingsInstance = new Animations.BeatFader.BeatFaderSettings()
            {
                BeatFaderTrueColor = AnimatedInit.SaversConfig.BeatFaderTrueColor,
                BeatFaderBeatColor = AnimatedInit.SaversConfig.BeatFaderBeatColor,
                BeatFaderDelay = AnimatedInit.SaversConfig.BeatFaderDelay,
                BeatFaderMaxSteps = AnimatedInit.SaversConfig.BeatFaderMaxSteps,
                BeatFaderCycleColors = AnimatedInit.SaversConfig.BeatFaderCycleColors,
                BeatFaderMinimumRedColorLevel = AnimatedInit.SaversConfig.BeatFaderMinimumRedColorLevel,
                BeatFaderMinimumGreenColorLevel = AnimatedInit.SaversConfig.BeatFaderMinimumGreenColorLevel,
                BeatFaderMinimumBlueColorLevel = AnimatedInit.SaversConfig.BeatFaderMinimumBlueColorLevel,
                BeatFaderMinimumColorLevel = AnimatedInit.SaversConfig.BeatFaderMinimumColorLevel,
                BeatFaderMaximumRedColorLevel = AnimatedInit.SaversConfig.BeatFaderMaximumRedColorLevel,
                BeatFaderMaximumGreenColorLevel = AnimatedInit.SaversConfig.BeatFaderMaximumGreenColorLevel,
                BeatFaderMaximumBlueColorLevel = AnimatedInit.SaversConfig.BeatFaderMaximumBlueColorLevel,
                BeatFaderMaximumColorLevel = AnimatedInit.SaversConfig.BeatFaderMaximumColorLevel
            };
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic() => BeatFader.Simulate(BeatFaderSettingsInstance);

    }
}
