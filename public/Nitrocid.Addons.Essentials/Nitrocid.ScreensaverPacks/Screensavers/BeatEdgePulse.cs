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
using Terminaux.Base.Extensions;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for BeatEdgePulse
    /// </summary>
    public class BeatEdgePulseDisplay : BaseScreensaver, IScreensaver
    {

        private Animations.BeatEdgePulse.BeatEdgePulseSettings? BeatEdgePulseSettingsInstance;

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "BeatEdgePulse";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", vars: [ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight]);
            BeatEdgePulseSettingsInstance = new Animations.BeatEdgePulse.BeatEdgePulseSettings()
            {
                BeatEdgePulseTrueColor = ScreensaverPackInit.SaversConfig.BeatEdgePulseTrueColor,
                BeatEdgePulseBeatColor = ScreensaverPackInit.SaversConfig.BeatEdgePulseBeatColor,
                BeatEdgePulseDelay = ScreensaverPackInit.SaversConfig.BeatEdgePulseDelay,
                BeatEdgePulseMaxSteps = ScreensaverPackInit.SaversConfig.BeatEdgePulseMaxSteps,
                BeatEdgePulseCycleColors = ScreensaverPackInit.SaversConfig.BeatEdgePulseCycleColors,
                BeatEdgePulseMinimumRedColorLevel = ScreensaverPackInit.SaversConfig.BeatEdgePulseMinimumRedColorLevel,
                BeatEdgePulseMinimumGreenColorLevel = ScreensaverPackInit.SaversConfig.BeatEdgePulseMinimumGreenColorLevel,
                BeatEdgePulseMinimumBlueColorLevel = ScreensaverPackInit.SaversConfig.BeatEdgePulseMinimumBlueColorLevel,
                BeatEdgePulseMinimumColorLevel = ScreensaverPackInit.SaversConfig.BeatEdgePulseMinimumColorLevel,
                BeatEdgePulseMaximumRedColorLevel = ScreensaverPackInit.SaversConfig.BeatEdgePulseMaximumRedColorLevel,
                BeatEdgePulseMaximumGreenColorLevel = ScreensaverPackInit.SaversConfig.BeatEdgePulseMaximumGreenColorLevel,
                BeatEdgePulseMaximumBlueColorLevel = ScreensaverPackInit.SaversConfig.BeatEdgePulseMaximumBlueColorLevel,
                BeatEdgePulseMaximumColorLevel = ScreensaverPackInit.SaversConfig.BeatEdgePulseMaximumColorLevel
            };
            ConsoleColoring.LoadBackDry("0;0;0");
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            Animations.BeatEdgePulse.BeatEdgePulse.Simulate(BeatEdgePulseSettingsInstance);
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.BeatEdgePulseDelay);
        }

    }
}
