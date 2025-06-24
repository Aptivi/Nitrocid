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

using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Misc.Screensaver;
using Terminaux.Base;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for SquareCorner
    /// </summary>
    public class SquareCornerDisplay : BaseScreensaver, IScreensaver
    {

        private Animations.SquareCorner.SquareCornerSettings? SquareCornerSettingsInstance;

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "SquareCorner";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", vars: [ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight]);
            SquareCornerSettingsInstance = new Animations.SquareCorner.SquareCornerSettings()
            {
                SquareCornerDelay = ScreensaverPackInit.SaversConfig.SquareCornerDelay,
                SquareCornerFadeOutDelay = ScreensaverPackInit.SaversConfig.SquareCornerFadeOutDelay,
                SquareCornerMaxSteps = ScreensaverPackInit.SaversConfig.SquareCornerMaxSteps,
                SquareCornerMinimumRedColorLevel = ScreensaverPackInit.SaversConfig.SquareCornerMinimumRedColorLevel,
                SquareCornerMinimumGreenColorLevel = ScreensaverPackInit.SaversConfig.SquareCornerMinimumGreenColorLevel,
                SquareCornerMinimumBlueColorLevel = ScreensaverPackInit.SaversConfig.SquareCornerMinimumBlueColorLevel,
                SquareCornerMaximumRedColorLevel = ScreensaverPackInit.SaversConfig.SquareCornerMaximumRedColorLevel,
                SquareCornerMaximumGreenColorLevel = ScreensaverPackInit.SaversConfig.SquareCornerMaximumGreenColorLevel,
                SquareCornerMaximumBlueColorLevel = ScreensaverPackInit.SaversConfig.SquareCornerMaximumBlueColorLevel,
            };
            ColorTools.LoadBackDry(0);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic() => Animations.SquareCorner.SquareCorner.Simulate(SquareCornerSettingsInstance);

    }
}
