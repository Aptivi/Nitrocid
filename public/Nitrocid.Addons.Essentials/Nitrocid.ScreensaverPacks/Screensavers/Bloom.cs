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

using Nitrocid.Misc.Screensaver;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for Bloom
    /// </summary>
    public class BloomDisplay : BaseScreensaver, IScreensaver
    {

        private Color? nextColor;
        private Color? currentColor;

        private static int MaxLevel =>
            ScreensaverPackInit.SaversConfig.BloomDarkColors ? 32 : 255;

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "Bloom";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            nextColor = ColorTools.GetRandomColor(ColorType.TrueColor, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel);
            currentColor = ColorTools.GetRandomColor(ColorType.TrueColor, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel);
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;
            if (currentColor is null || nextColor is null)
                return;

            // Prepare the colors
            int steps = ScreensaverPackInit.SaversConfig.BloomSteps;
            double thresholdR = (currentColor.RGB.R - nextColor.RGB.R) / (double)steps;
            double thresholdG = (currentColor.RGB.G - nextColor.RGB.G) / (double)steps;
            double thresholdB = (currentColor.RGB.B - nextColor.RGB.B) / (double)steps;

            // Now, transition from black to the target color
            double currentR = currentColor.RGB.R;
            double currentG = currentColor.RGB.G;
            double currentB = currentColor.RGB.B;
            for (int currentStep = 1; currentStep <= steps; currentStep++)
            {
                if (ConsoleResizeHandler.WasResized(false))
                    break;

                // Add the values according to the threshold
                currentR -= thresholdR;
                currentG -= thresholdG;
                currentB -= thresholdB;

                // Now, make a color and fill the console with it
                Color col = new((int)currentR, (int)currentG, (int)currentB);
                ConsoleColoring.LoadBackDry(col);

                // Sleep
                ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.BloomDelay);
            }

            // Generate new colors
            currentColor = nextColor;
            nextColor = ColorTools.GetRandomColor(ColorType.TrueColor, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel, 0, MaxLevel);
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.BloomDelay);

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
        }

    }
}
