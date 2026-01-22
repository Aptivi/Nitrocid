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

using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Drivers.RNG;
using Nitrocid.Misc.Screensaver;
using System;
using System.Collections.Generic;
using Terminaux.Colors;
using Terminaux.Base;
using Terminaux.Colors.Data;
using Terminaux.Base.Extensions;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for SkyComet
    /// </summary>
    public class SkyCometDisplay : BaseScreensaver, IScreensaver
    {

        private static readonly List<(int left, int top)> stars = [];

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "SkyComet";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Get random stars
            UpdateStars();
            ConsoleColoring.LoadBackDry(new Color(0, 0, 0));
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Update stars if refreshing
            UpdateStars();

            // Draw the stars
            string starChar = "*";
            RefreshStars();

            // If there is a chance that a comet appears, make it come from the right to the left
            bool cometAppearing = RandomDriver.RandomChance(1);
            if (cometAppearing)
            {
                // First, the positions
                int height = ConsoleWrapper.WindowHeight;
                int launchX = ConsoleWrapper.WindowWidth - 1;
                int launchY = RandomDriver.Random(height / 8, height / 2);
                int endY = RandomDriver.Random(launchY - 6, launchY + 6);

                // Then, the thresholds
                double thresholdY = -(endY - launchY) / ((double)ConsoleWrapper.WindowHeight * 4);

                // Then, the covered positions
                List<(int, int)> coveredPositions = [];
                Color color = ConsoleColors.LightSkyBlue1;
                for (int j = 0; j < ConsoleWrapper.WindowWidth; j++)
                {
                    if (ConsoleResizeHandler.WasResized(false))
                        break;

                    int currentPosX = launchX - j;
                    int currentPosY = (int)(launchY - (thresholdY * j));
                    if (currentPosY < 0)
                        break;
                    coveredPositions.Add((currentPosX, currentPosY));
                    TextWriterWhereColor.WriteWhereColorBack(starChar, currentPosX, currentPosY, ConsoleColors.LightSkyBlue1, new Color(0, 0, 0));
                }

                // Finally, fade out
                int steps = 25;
                int currentR = color.RGB.R;
                int currentG = color.RGB.G;
                int currentB = color.RGB.B;
                double thresholdRed = currentR / (double)steps;
                double thresholdGreen = currentG / (double)steps;
                double thresholdBlue = currentB / (double)steps;
                for (int j = 1; j <= steps; j++)
                {
                    if (ConsoleResizeHandler.WasResized(false))
                        break;

                    // Fade out to black by getting its threshold
                    RefreshStars();
                    currentR = (int)Math.Round(color.RGB.R - thresholdRed * j);
                    currentG = (int)Math.Round(color.RGB.G - thresholdGreen * j);
                    currentB = (int)Math.Round(color.RGB.B - thresholdBlue * j);
                    Color result = new(currentR, currentG, currentB);

                    // Actually print things out
                    foreach (var pos in coveredPositions)
                        TextWriterWhereColor.WriteWhereColorBack(starChar, pos.Item1, pos.Item2, result, new Color(0, 0, 0));
                }
            }

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.SkyCometDelay);
        }

        /// <inheritdoc/>
        public override void ScreensaverOutro()
        {
            stars.Clear();
        }

        private static void UpdateStars()
        {
            if (stars.Count > 0)
                return;
            int width = ConsoleWrapper.WindowWidth;
            int height = ConsoleWrapper.WindowHeight;
            int numberStars = width * height / ((width / 6) + (height / 3));
            for (int i = 0; i < numberStars; i++)
            {
                // Get the random position
                int randomLeft = RandomDriver.RandomIdx(width);
                int randomTop = RandomDriver.RandomIdx(height);

                // Now, add the star to the list
                stars.Add((randomLeft, randomTop));
            }
        }

        private static void RefreshStars()
        {
            // Draw the stars
            string starChar = "*";
            foreach (var star in stars)
            {
                if (ConsoleResizeHandler.WasResized(false))
                    break;

                // Get the position and write it
                int left = star.left;
                int top = star.top;
                TextWriterWhereColor.WriteWhereColorBack(starChar, left, top, ConsoleColors.White, new Color(0, 0, 0));
            }
        }

    }
}
