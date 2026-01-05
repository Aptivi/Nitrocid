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
using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Misc.Screensaver;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for Noisy
    /// </summary>
    public class NoisyDisplay : BaseScreensaver, IScreensaver
    {
        // TODO: NKS_SCREENSAVERPACKS_NOISY_SETTINGS_DESC -> Simulates television noise (either grayscale noise or colored noise)
        /// <inheritdoc/>
        public override string ScreensaverName =>
            "Noisy";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Generate either monochrome noise or colored noise, similar to old televisions
            bool needsColor = ScreensaverPackInit.SaversConfig.NoisyColor;
            int cellAmount = ConsoleWrapper.WindowWidth * ConsoleWrapper.WindowHeight;
            var noiseBuilder = new StringBuilder();
            for (int i = 0; i < cellAmount; i++)
            {
                // Make a noise color
                Color noiseColor = needsColor ? ColorTools.GetRandomColor() : new($"hsl:0;0;{RandomDriver.Random(100)}");
                noiseBuilder.Append(noiseColor.VTSequenceBackgroundTrueColor + " ");
            }
            noiseBuilder.Append(ConsolePositioning.RenderChangePosition(0, 0));
            ConsoleWrapper.Write(noiseBuilder.ToString());

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.NoisyDelay);
        }

    }
}
