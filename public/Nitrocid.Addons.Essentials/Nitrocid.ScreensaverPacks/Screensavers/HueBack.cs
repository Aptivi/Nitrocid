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

using Nitrocid.Base.Misc.Screensaver;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for HueBack
    /// </summary>
    public class HueBackDisplay : BaseScreensaver, IScreensaver
    {

        // Hue angle is in degrees and not radians
        private static int currentHueAngle = 0;

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "HueBack";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Prepare the color
            var color = new Color($"hsl:{currentHueAngle};{ScreensaverPackInit.SaversConfig.HueBackSaturation};{ScreensaverPackInit.SaversConfig.HueBackLuminance}");

            // Now, change the background color accordingly
            ConsoleColoring.LoadBackDry(color);
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.HueBackDelay);
            currentHueAngle++;
            if (currentHueAngle > 360)
                currentHueAngle = 0;
        }

        /// <inheritdoc/>
        public override void ScreensaverOutro()
        {
            currentHueAngle = 0;
        }

    }
}
