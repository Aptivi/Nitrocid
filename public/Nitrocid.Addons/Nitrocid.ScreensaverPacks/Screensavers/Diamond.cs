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

using System.Collections.Generic;
using KS.ConsoleBase;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Drivers.RNG;
using KS.Kernel.Threading;
using KS.Misc.Screensaver;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Settings for Diamond
    /// </summary>
    public static class DiamondSettings
    {

        /// <summary>
        /// [Diamond] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int DiamondDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.DiamondDelay;
            }
            set
            {
                if (value <= 0)
                    value = 500;
                ScreensaverPackInit.SaversConfig.DiamondDelay = value;
            }
        }

    }

    /// <summary>
    /// Display code for Diamond
    /// </summary>
    public class DiamondDisplay : BaseScreensaver, IScreensaver
    {

        private static bool shining;
        private static readonly int maxShineSteps = 15;
        private static Color selectedColor = ConsoleColors.LightCyan1;
        private static readonly List<Color> diamondColors =
        [
            ConsoleColors.LightCyan1,
            ConsoleColors.Pink1,
            ConsoleColors.White
        ];

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Diamond";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            selectedColor = diamondColors[RandomDriver.RandomIdx(diamondColors.Count)];
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Get the console half width and half height
            int halfWidth = ConsoleWrapper.WindowWidth / 2;
            int halfHeight = ConsoleWrapper.WindowHeight / 2;

            // Get the maximum and minimum positions
            int minLeft = halfWidth - 8;
            int maxLeft = halfWidth + 8;

            // Get the color shades
            var colorShaded = new Color(selectedColor.R / 3, selectedColor.G / 3, selectedColor.B / 3);
            int shinePlaceX = halfWidth;
            int shinePlaceY = halfHeight - (halfWidth - minLeft);
            int shinePlaceX2 = minLeft;
            int shinePlaceY2 = halfHeight;
            shining = RandomDriver.RandomChance(5);

            // Now, draw the diamond (the first part)
            for (int left = minLeft; left <= halfWidth; left++)
            {
                int diff = left - minLeft;
                for (int top = halfHeight - diff; top <= halfHeight + diff; top++)
                {
                    bool drawingEdge = top == halfHeight - diff || top == halfHeight + diff;
                    if (ConsoleResizeListener.WasResized(false))
                        break;

                    // Determine the color by draw mode
                    var finalColor = drawingEdge ? selectedColor : colorShaded;
                    TextWriterWhereColor.WriteWhereColorBack(" ", left, top, Color.Empty, finalColor);
                }
            }

            // Now, draw the diamond (the second part)
            for (int left = halfWidth; left <= maxLeft; left++)
            {
                int diff = left - maxLeft;
                for (int top = halfHeight + diff; top <= halfHeight - diff; top++)
                {
                    bool drawingEdge = top == halfHeight - diff || top == halfHeight + diff;
                    if (ConsoleResizeListener.WasResized(false))
                        break;

                    // Determine the color by draw mode
                    var finalColor = drawingEdge ? selectedColor : colorShaded;
                    TextWriterWhereColor.WriteWhereColorBack(" ", left, top, Color.Empty, finalColor);
                }
            }

            // If shining, fade the small light in
            if (shining)
            {
                // Get the shined color
                int shinedR = selectedColor.R * 2;
                if (shinedR > 255)
                    shinedR = 255;
                int shinedG = selectedColor.G * 2;
                if (shinedG > 255)
                    shinedG = 255;
                int shinedB = selectedColor.B * 2;
                if (shinedB > 255)
                    shinedB = 255;
                var shined = new Color(shinedR, shinedG, shinedB);

                // Get the threshold from either the shade (for the bottom), the diamond color (for the center), or the
                // black color (the rest of the directions)
                var black = new Color(ConsoleColors.Black);
                double thresholdBottomR = (colorShaded.R - shined.R) / (double)maxShineSteps;
                double thresholdBottomG = (colorShaded.G - shined.G) / (double)maxShineSteps;
                double thresholdBottomB = (colorShaded.B - shined.B) / (double)maxShineSteps;
                double thresholdCenterR = (selectedColor.R - shined.R) / (double)maxShineSteps;
                double thresholdCenterG = (selectedColor.G - shined.G) / (double)maxShineSteps;
                double thresholdCenterB = (selectedColor.B - shined.B) / (double)maxShineSteps;
                double thresholdRestR = (black.R - shined.R) / (double)maxShineSteps;
                double thresholdRestG = (black.G - shined.G) / (double)maxShineSteps;
                double thresholdRestB = (black.B - shined.B) / (double)maxShineSteps;

                // Now, transition to the target color
                double currentBottomR = colorShaded.R;
                double currentBottomG = colorShaded.G;
                double currentBottomB = colorShaded.B;
                double currentCenterR = selectedColor.R;
                double currentCenterG = selectedColor.G;
                double currentCenterB = selectedColor.B;
                double currentRestR = black.R;
                double currentRestG = black.G;
                double currentRestB = black.B;
                for (int currentStep = 1; currentStep <= maxShineSteps; currentStep++)
                {
                    if (ConsoleResizeListener.WasResized(false))
                        break;

                    // Add the values according to the threshold
                    currentBottomR -= thresholdBottomR;
                    currentBottomG -= thresholdBottomG;
                    currentBottomB -= thresholdBottomB;
                    currentCenterR -= thresholdCenterR;
                    currentCenterG -= thresholdCenterG;
                    currentCenterB -= thresholdCenterB;
                    currentRestR -= thresholdRestR;
                    currentRestG -= thresholdRestG;
                    currentRestB -= thresholdRestB;

                    // Now, draw the shiny part
                    Color colBottom = new((int)currentBottomR, (int)currentBottomG, (int)currentBottomB);
                    Color colCenter = new((int)currentCenterR, (int)currentCenterG, (int)currentCenterB);
                    Color colRest = new((int)currentRestR, (int)currentRestG, (int)currentRestB);
                    TextWriterWhereColor.WriteWhereColorBack(" ", shinePlaceX, shinePlaceY, Color.Empty, colCenter);
                    TextWriterWhereColor.WriteWhereColorBack(" ", shinePlaceX, shinePlaceY + 1, Color.Empty, colBottom);
                    TextWriterWhereColor.WriteWhereColorBack(" ", shinePlaceX, shinePlaceY + 2, Color.Empty, colBottom);
                    TextWriterWhereColor.WriteWhereColorBack(" ", shinePlaceX, shinePlaceY - 1, Color.Empty, colRest);
                    TextWriterWhereColor.WriteWhereColorBack(" ", shinePlaceX, shinePlaceY - 2, Color.Empty, colRest);
                    TextWriterWhereColor.WriteWhereColorBack(" ", shinePlaceX - 1, shinePlaceY, Color.Empty, colRest);
                    TextWriterWhereColor.WriteWhereColorBack(" ", shinePlaceX - 2, shinePlaceY, Color.Empty, colRest);
                    TextWriterWhereColor.WriteWhereColorBack(" ", shinePlaceX + 1, shinePlaceY, Color.Empty, colRest);
                    TextWriterWhereColor.WriteWhereColorBack(" ", shinePlaceX + 2, shinePlaceY, Color.Empty, colRest);
                    TextWriterWhereColor.WriteWhereColorBack(" ", shinePlaceX2, shinePlaceY2, Color.Empty, colCenter);
                    TextWriterWhereColor.WriteWhereColorBack(" ", shinePlaceX2, shinePlaceY2 + 1, Color.Empty, colRest);
                    TextWriterWhereColor.WriteWhereColorBack(" ", shinePlaceX2, shinePlaceY2 + 2, Color.Empty, colRest);
                    TextWriterWhereColor.WriteWhereColorBack(" ", shinePlaceX2, shinePlaceY2 - 1, Color.Empty, colRest);
                    TextWriterWhereColor.WriteWhereColorBack(" ", shinePlaceX2, shinePlaceY2 - 2, Color.Empty, colRest);
                    TextWriterWhereColor.WriteWhereColorBack(" ", shinePlaceX2 - 1, shinePlaceY2, Color.Empty, colRest);
                    TextWriterWhereColor.WriteWhereColorBack(" ", shinePlaceX2 - 2, shinePlaceY2, Color.Empty, colRest);
                    TextWriterWhereColor.WriteWhereColorBack(" ", shinePlaceX2 + 1, shinePlaceY2, Color.Empty, colBottom);
                    TextWriterWhereColor.WriteWhereColorBack(" ", shinePlaceX2 + 2, shinePlaceY2, Color.Empty, colBottom);

                    // Sleep
                    ThreadManager.SleepNoBlock(DiamondSettings.DiamondDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                }
                for (int currentStep = 1; currentStep <= maxShineSteps; currentStep++)
                {
                    if (ConsoleResizeListener.WasResized(false))
                        break;

                    // Add the values according to the threshold
                    currentBottomR -= -thresholdBottomR;
                    currentBottomG -= -thresholdBottomG;
                    currentBottomB -= -thresholdBottomB;
                    currentCenterR -= -thresholdCenterR;
                    currentCenterG -= -thresholdCenterG;
                    currentCenterB -= -thresholdCenterB;
                    currentRestR -= -thresholdRestR;
                    currentRestG -= -thresholdRestG;
                    currentRestB -= -thresholdRestB;

                    // Now, draw the shiny part
                    Color colBottom = new((int)currentBottomR, (int)currentBottomG, (int)currentBottomB);
                    Color colCenter = new((int)currentCenterR, (int)currentCenterG, (int)currentCenterB);
                    Color colRest = new((int)currentRestR, (int)currentRestG, (int)currentRestB);
                    TextWriterWhereColor.WriteWhereColorBack(" ", shinePlaceX, shinePlaceY, Color.Empty, colCenter);
                    TextWriterWhereColor.WriteWhereColorBack(" ", shinePlaceX, shinePlaceY + 1, Color.Empty, colBottom);
                    TextWriterWhereColor.WriteWhereColorBack(" ", shinePlaceX, shinePlaceY + 2, Color.Empty, colBottom);
                    TextWriterWhereColor.WriteWhereColorBack(" ", shinePlaceX, shinePlaceY - 1, Color.Empty, colRest);
                    TextWriterWhereColor.WriteWhereColorBack(" ", shinePlaceX, shinePlaceY - 2, Color.Empty, colRest);
                    TextWriterWhereColor.WriteWhereColorBack(" ", shinePlaceX - 1, shinePlaceY, Color.Empty, colRest);
                    TextWriterWhereColor.WriteWhereColorBack(" ", shinePlaceX - 2, shinePlaceY, Color.Empty, colRest);
                    TextWriterWhereColor.WriteWhereColorBack(" ", shinePlaceX + 1, shinePlaceY, Color.Empty, colRest);
                    TextWriterWhereColor.WriteWhereColorBack(" ", shinePlaceX + 2, shinePlaceY, Color.Empty, colRest);
                    TextWriterWhereColor.WriteWhereColorBack(" ", shinePlaceX2, shinePlaceY2, Color.Empty, colCenter);
                    TextWriterWhereColor.WriteWhereColorBack(" ", shinePlaceX2, shinePlaceY2 + 1, Color.Empty, colRest);
                    TextWriterWhereColor.WriteWhereColorBack(" ", shinePlaceX2, shinePlaceY2 + 2, Color.Empty, colRest);
                    TextWriterWhereColor.WriteWhereColorBack(" ", shinePlaceX2, shinePlaceY2 - 1, Color.Empty, colRest);
                    TextWriterWhereColor.WriteWhereColorBack(" ", shinePlaceX2, shinePlaceY2 - 2, Color.Empty, colRest);
                    TextWriterWhereColor.WriteWhereColorBack(" ", shinePlaceX2 - 1, shinePlaceY2, Color.Empty, colRest);
                    TextWriterWhereColor.WriteWhereColorBack(" ", shinePlaceX2 - 2, shinePlaceY2, Color.Empty, colRest);
                    TextWriterWhereColor.WriteWhereColorBack(" ", shinePlaceX2 + 1, shinePlaceY2, Color.Empty, colBottom);
                    TextWriterWhereColor.WriteWhereColorBack(" ", shinePlaceX2 + 2, shinePlaceY2, Color.Empty, colBottom);

                    // Sleep
                    ThreadManager.SleepNoBlock(DiamondSettings.DiamondDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                }
            }

            // Delay
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(DiamondSettings.DiamondDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
