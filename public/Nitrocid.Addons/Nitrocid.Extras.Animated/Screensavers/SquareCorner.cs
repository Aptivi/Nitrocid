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
using Nitrocid.Extras.Animated.Animations.SquareCorner;
using Terminaux.Base;
using Terminaux.Base.Extensions;

namespace Nitrocid.Extras.Animated.Screensavers
{
    /// <summary>
    /// Display code for SquareCorner
    /// </summary>
    public class SquareCornerDisplay : BaseScreensaver, IScreensaver
    {

        private SquareCornerSettings? SquareCornerSettingsInstance;
        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", vars: [ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight]);
            SquareCornerSettingsInstance = new Animations.SquareCorner.SquareCornerSettings()
            {
                SquareCornerDelay = AnimatedInit.SaversConfig.SquareCornerDelay,
                SquareCornerFadeOutDelay = AnimatedInit.SaversConfig.SquareCornerFadeOutDelay,
                SquareCornerMaxSteps = AnimatedInit.SaversConfig.SquareCornerMaxSteps,
                SquareCornerMinimumRedColorLevel = AnimatedInit.SaversConfig.SquareCornerMinimumRedColorLevel,
                SquareCornerMinimumGreenColorLevel = AnimatedInit.SaversConfig.SquareCornerMinimumGreenColorLevel,
                SquareCornerMinimumBlueColorLevel = AnimatedInit.SaversConfig.SquareCornerMinimumBlueColorLevel,
                SquareCornerMaximumRedColorLevel = AnimatedInit.SaversConfig.SquareCornerMaximumRedColorLevel,
                SquareCornerMaximumGreenColorLevel = AnimatedInit.SaversConfig.SquareCornerMaximumGreenColorLevel,
                SquareCornerMaximumBlueColorLevel = AnimatedInit.SaversConfig.SquareCornerMaximumBlueColorLevel,
            };
            ConsoleColoring.LoadBackDry(0);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic() => SquareCorner.Simulate(SquareCornerSettingsInstance);

    }
}
