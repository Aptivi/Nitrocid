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

using System.Collections.Generic;
using Nitrocid.Misc.Screensaver;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for Siren
    /// </summary>
    public class SirenDisplay : BaseScreensaver, IScreensaver
    {

        internal readonly static Dictionary<string, Color[]> sirens = new()
        {
            { "Cop",        new Color[] { new(255, 0, 0), new(0, 0, 255) } },
            { "Ambulance",  new Color[] { new(255, 0, 0), new(128, 0, 0) } },
            { "Neon",       new Color[] { new(255, 0, 255), new(0, 255, 255) } }
        };
        private int step = 0;

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "Siren";

        /// <inheritdoc/>
        public override bool ScreensaverContainsFlashingImages =>
            true;

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            step = 0;
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Get step color array from siren type
            Color[] sirenColors = sirens[ScreensaverPackInit.SaversConfig.SirenStyle];

            // Step through the color
            step += 1;
            if (step >= sirenColors.Length)
                step = 0;

            // Set color
            ColorTools.LoadBackDry(sirenColors[step]);

            // Delay
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.SirenDelay);
        }

    }
}
