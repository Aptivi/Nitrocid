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
using Nitrocid.Extras.Animated.Animations.Spin;
using Terminaux.Base;

namespace Nitrocid.Extras.Animated.Screensavers
{
    /// <summary>
    /// Display code for Spin
    /// </summary>
    public class SpinDisplay : BaseScreensaver, IScreensaver
    {

        private SpinSettings? SpinSettingsInstance;

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", vars: [ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight]);
            SpinSettingsInstance = new SpinSettings()
            {
                SpinDelay = AnimatedInit.SaversConfig.SpinDelay
            };
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            Spin.Simulate(SpinSettingsInstance);
            ScreensaverManager.Delay(AnimatedInit.SaversConfig.SpinDelay);
        }

    }
}
