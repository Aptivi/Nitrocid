
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
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Drivers.RNG;
using KS.Kernel.Debugging;
using KS.Kernel.Threading;
using KS.Misc.Screensaver;
using System;
using System.Collections.Generic;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Settings for SkyComet
    /// </summary>
    public static class SkyCometSettings
    {

        /// <summary>
        /// [SkyComet] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int SkyCometDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.SkyCometDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                ScreensaverPackInit.SaversConfig.SkyCometDelay = value;
            }
        }

    }

    /// <summary>
    /// Display code for SkyComet
    /// </summary>
    public class SkyCometDisplay : BaseScreensaver, IScreensaver
    {

        private static readonly List<(int left, int top)> stars = new();

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "SkyComet";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Get random stars
            UpdateStars();
            KernelColorTools.LoadBack(new Color(0, 0, 0));
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
                List<(int, int)> coveredPositions = new();
                Color color = ConsoleColors.LightSkyBlue1;
                for (int j = 0; j < ConsoleWrapper.WindowWidth; j++)
                {
                    if (ConsoleResizeListener.WasResized(false))
                        break;

                    int currentPosX = launchX - j;
                    int currentPosY = (int)(launchY - (thresholdY * j));
                    if (currentPosY < 0)
                        break;
                    coveredPositions.Add((currentPosX, currentPosY));
                    TextWriterWhereColor.WriteWhere(starChar, currentPosX, currentPosY, ConsoleColors.LightSkyBlue1);
                }

                // Finally, fade out
                int steps = 25;
                int currentR = color.R;
                int currentG = color.G;
                int currentB = color.B;
                double thresholdRed = currentR / (double)steps;
                double thresholdGreen = currentG / (double)steps;
                double thresholdBlue = currentB / (double)steps;
                for (int j = 1; j <= steps; j++)
                {
                    if (ConsoleResizeListener.WasResized(false))
                        break;

                    // Fade out to black by getting its threshold
                    RefreshStars();
                    currentR = (int)Math.Round(color.R - thresholdRed * j);
                    currentG = (int)Math.Round(color.G - thresholdGreen * j);
                    currentB = (int)Math.Round(color.B - thresholdBlue * j);
                    Color result = new(currentR, currentG, currentB);

                    // Actually print things out
                    foreach (var pos in coveredPositions)
                        TextWriterWhereColor.WriteWhere(starChar, pos.Item1, pos.Item2, result);
                }
            }

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(SkyCometSettings.SkyCometDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
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
                if (ConsoleResizeListener.WasResized(false))
                    break;

                // Get the position and write it
                int left = star.left;
                int top = star.top;
                TextWriterWhereColor.WriteWhere(starChar, left, top, ConsoleColors.White);
            }
        }

    }
}
