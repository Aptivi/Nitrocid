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
using Terminaux.Base.Extensions;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for ExcaliBeats
    /// </summary>
    public class ExcaliBeatsDisplay : BaseScreensaver, IScreensaver
    {

        private Animations.ExcaliBeats.ExcaliBeatsSettings? ExcaliBeatsSettingsInstance;

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "ExcaliBeats";

        /// <inheritdoc/>
        public override bool ScreensaverContainsFlashingImages =>
            true;

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", vars: [ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight]);
            ExcaliBeatsSettingsInstance = new Animations.ExcaliBeats.ExcaliBeatsSettings()
            {
                ExcaliBeatsTrueColor = ScreensaverPackInit.SaversConfig.ExcaliBeatsTrueColor,
                ExcaliBeatsBeatColor = ScreensaverPackInit.SaversConfig.ExcaliBeatsBeatColor,
                ExcaliBeatsDelay = ScreensaverPackInit.SaversConfig.ExcaliBeatsDelay,
                ExcaliBeatsMaxSteps = ScreensaverPackInit.SaversConfig.ExcaliBeatsMaxSteps,
                ExcaliBeatsCycleColors = ScreensaverPackInit.SaversConfig.ExcaliBeatsCycleColors,
                ExcaliBeatsExplicit = ScreensaverPackInit.SaversConfig.ExcaliBeatsExplicit,
                ExcaliBeatsMinimumRedColorLevel = ScreensaverPackInit.SaversConfig.ExcaliBeatsMinimumRedColorLevel,
                ExcaliBeatsMinimumGreenColorLevel = ScreensaverPackInit.SaversConfig.ExcaliBeatsMinimumGreenColorLevel,
                ExcaliBeatsMinimumBlueColorLevel = ScreensaverPackInit.SaversConfig.ExcaliBeatsMinimumBlueColorLevel,
                ExcaliBeatsMinimumColorLevel = ScreensaverPackInit.SaversConfig.ExcaliBeatsMinimumColorLevel,
                ExcaliBeatsMaximumRedColorLevel = ScreensaverPackInit.SaversConfig.ExcaliBeatsMaximumRedColorLevel,
                ExcaliBeatsMaximumGreenColorLevel = ScreensaverPackInit.SaversConfig.ExcaliBeatsMaximumGreenColorLevel,
                ExcaliBeatsMaximumBlueColorLevel = ScreensaverPackInit.SaversConfig.ExcaliBeatsMaximumBlueColorLevel,
                ExcaliBeatsMaximumColorLevel = ScreensaverPackInit.SaversConfig.ExcaliBeatsMaximumColorLevel
            };
            ConsoleColoring.LoadBackDry(0);
            ConsoleWrapper.Clear();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic() => Animations.ExcaliBeats.ExcaliBeats.Simulate(ExcaliBeatsSettingsInstance);

    }
}
