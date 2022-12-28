
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.Kernel.Debugging;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Custom screensaver displayer
    /// </summary>
    public class CustomDisplay : BaseScreensaver, IScreensaver
    {

        private BaseScreensaver CustomSaver { get; set; }

        // To Screensaver Developers: ONLY put the effect code in your ScreensaverLogic() routine.
        // Set colors, write welcome message, etc. with the exception of infinite loop and the effect code in ScreensaverPreparation() routine
        // Recommended: Turn off console cursor, and clear the screen in ScreensaverPreparation() routine.

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Custom";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ConsoleBase.ConsoleWrapper.CursorVisible = false;
            DebugWriter.WriteDebug(DebugLevel.I, "Entered CustomSaver.ScreensaverPreparation().");
            CustomSaver.ScreensaverPreparation();
            DebugWriter.WriteDebug(DebugLevel.I, "Exited CustomSaver.ScreensaverPreparation().");
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Entered CustomSaver.ScreensaverLogic().");
            CustomSaver.ScreensaverLogic();
            DebugWriter.WriteDebug(DebugLevel.I, "Exited CustomSaver.ScreensaverLogic().");
        }

        /// <summary>
        /// Generates a new custom screensaver displayer from the screensaver base
        /// </summary>
        /// <param name="CustomSaver">Saver base</param>
        public CustomDisplay(BaseScreensaver CustomSaver) => this.CustomSaver = CustomSaver;

    }
}
