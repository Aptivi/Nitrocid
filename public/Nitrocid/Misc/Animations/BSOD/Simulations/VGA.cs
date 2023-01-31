
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

using GlitchSettings = KS.Misc.Animations.Glitch.GlitchSettings;
using DisplayGlitchSettings = KS.Misc.Screensaver.Displays.GlitchSettings;

namespace KS.Misc.Animations.BSOD.Simulations
{
    internal class VGA : BaseBSOD
    {
        public override void Simulate()
        {
            var gs = new GlitchSettings()
            {
                GlitchDelay = DisplayGlitchSettings.GlitchDelay,
                GlitchDensity = DisplayGlitchSettings.GlitchDensity
            };
            Glitch.Glitch.Simulate(gs);
        }
    }
}
