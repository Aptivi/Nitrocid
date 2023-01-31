
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using KS.Kernel.Debugging;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for BSOD
    /// </summary>
    public static class BSODSettings
    {

        private static int _Delay = 10000;

        /// <summary>
        /// [BSOD] How many beats per minute to wait before making the next write?
        /// </summary>
        public static int BSODDelay
        {
            get
            {
                return _Delay;
            }
            set
            {
                if (value <= 0)
                    value = 10000;
                _Delay = value;
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
            ConsoleBase.ConsoleWrapper.BackgroundColor = ConsoleColor.Black;
            ConsoleBase.ConsoleWrapper.Clear();
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleBase.ConsoleWrapper.WindowWidth, ConsoleBase.ConsoleWrapper.WindowHeight);
            BSODSettingsInstance = new Animations.BSOD.BSODSettings()
            {
                BSODDelay = BSODSettings.BSODDelay
            };
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic() => Animations.BSOD.BSOD.Simulate(BSODSettingsInstance);

    }
}
