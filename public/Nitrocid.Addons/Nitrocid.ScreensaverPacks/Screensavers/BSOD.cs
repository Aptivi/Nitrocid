//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using Nitrocid.ConsoleBase;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Misc.Screensaver;
using Terminaux.Base;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Settings for BSOD
    /// </summary>
    public static class BSODSettings
    {

        /// <summary>
        /// [BSOD] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int BSODDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BSODDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10000;
                ScreensaverPackInit.SaversConfig.BSODDelay = value;
            }
        }

    }

    /// <summary>
    /// Display code for BSOD
    /// </summary>
    public class BSODDisplay : BaseScreensaver, IScreensaver
    {

        private Animations.BSOD.BSODSettings BSODSettingsInstance;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "BSOD";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);
            BSODSettingsInstance = new Animations.BSOD.BSODSettings()
            {
                BSODDelay = BSODSettings.BSODDelay
            };
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic() => Animations.BSOD.BSOD.Simulate(BSODSettingsInstance);

    }
}
