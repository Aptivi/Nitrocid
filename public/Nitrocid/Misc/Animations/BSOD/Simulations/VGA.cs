
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

using KS.ConsoleBase;
using KS.Drivers.RNG;
using KS.Kernel.Threading;
using KS.Misc.Screensaver;
using KS.Misc.Screensaver.Displays;
using BsodSettings = KS.Misc.Screensaver.Displays.BSODSettings;

namespace KS.Misc.Animations.BSOD.Simulations
{
    internal class VGA : BaseBSOD
    {
        public override void Simulate()
        {
            int millisecondsElapsed = 0;
            while (!(millisecondsElapsed >= BsodSettings.BSODDelay | ConsoleResizeListener.WasResized(false)))
            {
                if (!ConsoleResizeListener.WasResized(false))
                {
                    // Select random position to cover
                    int CoverX = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
                    int CoverY = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);

                    // Glitch!
                    Glitch.Glitch.GlitchAt(CoverX, CoverY);
                }
                else
                {
                    // We're resizing.
                    ConsoleWrapper.CursorVisible = false;
                    break;
                }
                ThreadManager.SleepNoBlock(GlitchSettings.GlitchDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                millisecondsElapsed += GlitchSettings.GlitchDelay;
            }
        }
    }
}
