//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System.Threading;
using KS.Kernel.Debugging;
using KS.Misc.Splash;
using Nitrocid.ScreensaverPacks.Animations.BeatEdgePulse;

namespace Nitrocid.ScreensaverPacks.Splashes
{
    class SplashBeatEdgePulse : BaseSplash, ISplash
    {

        // Standalone splash information
        public override string SplashName => "BeatEdgePulse";

        // BeatEdgePulse-specific variables
        internal BeatEdgePulseSettings BeatEdgePulseSettings;

        public SplashBeatEdgePulse() => BeatEdgePulseSettings = new BeatEdgePulseSettings();

        // Actual logic
        public override void Display(SplashContext context)
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash displaying.");

                // Loop until we got a closing notification
                while (!SplashClosing)
                    BeatEdgePulse.Simulate(BeatEdgePulseSettings);
            }
            catch (ThreadInterruptedException)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash done.");
            }
        }

    }
}
