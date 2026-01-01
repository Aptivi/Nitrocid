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
    /// Display code for BeatFader
    /// </summary>
    public class BeatFaderDisplay : BaseScreensaver, IScreensaver
    {

        private Animations.BeatFader.BeatFaderSettings? BeatFaderSettingsInstance;

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "BeatFader";

        /// <inheritdoc/>
        public override bool ScreensaverContainsFlashingImages =>
            true;

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", vars: [ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight]);
            BeatFaderSettingsInstance = new Animations.BeatFader.BeatFaderSettings()
            {
                BeatFaderTrueColor = ScreensaverPackInit.SaversConfig.BeatFaderTrueColor,
                BeatFaderBeatColor = ScreensaverPackInit.SaversConfig.BeatFaderBeatColor,
                BeatFaderDelay = ScreensaverPackInit.SaversConfig.BeatFaderDelay,
                BeatFaderMaxSteps = ScreensaverPackInit.SaversConfig.BeatFaderMaxSteps,
                BeatFaderCycleColors = ScreensaverPackInit.SaversConfig.BeatFaderCycleColors,
                BeatFaderMinimumRedColorLevel = ScreensaverPackInit.SaversConfig.BeatFaderMinimumRedColorLevel,
                BeatFaderMinimumGreenColorLevel = ScreensaverPackInit.SaversConfig.BeatFaderMinimumGreenColorLevel,
                BeatFaderMinimumBlueColorLevel = ScreensaverPackInit.SaversConfig.BeatFaderMinimumBlueColorLevel,
                BeatFaderMinimumColorLevel = ScreensaverPackInit.SaversConfig.BeatFaderMinimumColorLevel,
                BeatFaderMaximumRedColorLevel = ScreensaverPackInit.SaversConfig.BeatFaderMaximumRedColorLevel,
                BeatFaderMaximumGreenColorLevel = ScreensaverPackInit.SaversConfig.BeatFaderMaximumGreenColorLevel,
                BeatFaderMaximumBlueColorLevel = ScreensaverPackInit.SaversConfig.BeatFaderMaximumBlueColorLevel,
                BeatFaderMaximumColorLevel = ScreensaverPackInit.SaversConfig.BeatFaderMaximumColorLevel
            };
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic() => Animations.BeatFader.BeatFader.Simulate(BeatFaderSettingsInstance);

    }
}
