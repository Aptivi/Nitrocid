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

using System.Text;
using Terminaux.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Base;
using Terminaux.Colors.Data;
using Nitrocid.Base.Misc.Screensaver;

namespace Nitrocid.ScreensaverPacks.Animations.Spin
{
    /// <summary>
    /// Spin animation module
    /// </summary>
    public static class Spin
    {

        private static int CurrentWindowWidth;
        private static int CurrentWindowHeight;
        private static int currentSpinStep = 0;
        private static readonly char[] spinSteps = ['/', '|', '\\', '-'];

        /// <summary>
        /// Simulates the pulsing animation
        /// </summary>
        public static void Simulate(SpinSettings? Settings)
        {
            Settings ??= new();
            CurrentWindowWidth = ConsoleWrapper.WindowWidth;
            CurrentWindowHeight = ConsoleWrapper.WindowHeight;
            ConsoleWrapper.CursorVisible = false;

            // Get spin character from current index
            char spinStep = spinSteps[currentSpinStep];

            StringBuilder spinBuffer = new();

            // Make a spin buffer
            for (int x = 0; x < CurrentWindowWidth; x++)
            {
                for (int y = 0; y < CurrentWindowHeight; y++)
                {
                    spinBuffer.Append(spinStep.ToString());
                }
            }

            // Spin!
            if (!ConsoleResizeHandler.WasResized(false))
            {
                TextWriterWhereColor.WriteWhereColorBack(spinBuffer.ToString(), 0, 0, true, new Color(ConsoleColors.White), new Color(ConsoleColors.Black));
                ScreensaverManager.Delay(Settings.SpinDelay);
            }

            // Step the current spin step forward
            currentSpinStep++;
            if (currentSpinStep >= spinSteps.Length)
                currentSpinStep = 0;

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            CurrentWindowWidth = ConsoleWrapper.WindowWidth;
            CurrentWindowHeight = ConsoleWrapper.WindowHeight;
        }

    }
}
