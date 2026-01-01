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

using System.Collections.Generic;
using System.Text;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Drivers.RNG;
using Nitrocid.Misc.Screensaver;
using Terminaux.Colors;
using Terminaux.Base;
using Terminaux.Colors.Data;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for Diamond
    /// </summary>
    public class DiamondDisplay : BaseScreensaver, IScreensaver
    {

        private static bool shining;
        private static bool seen;
        private static readonly int maxShineSteps = 15;
        private static Color selectedColor = ConsoleColors.LightCyan1;
        private static readonly List<Color> diamondColors =
        [
            ConsoleColors.LightCyan1,
            ConsoleColors.Pink1,
            ConsoleColors.White
        ];

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "Diamond";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            seen = false;
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
            var colorShaded = new Color(selectedColor.RGB.R / 3, selectedColor.RGB.G / 3, selectedColor.RGB.B / 3);
            int shinePlaceX = halfWidth;
            int shinePlaceY = halfHeight - (halfWidth - minLeft);
            int shinePlaceX2 = minLeft;
            int shinePlaceY2 = halfHeight;
            shining = RandomDriver.RandomChance(5);

            // Diamond buffer
            var buffer = new StringBuilder();

            // Draw the diamond once
            if (!seen)
            {
                // Now, draw the diamond (the first part)
                for (int left = minLeft; left <= halfWidth; left++)
                {
                    int diff = left - minLeft;
                    for (int top = halfHeight - diff; top <= halfHeight + diff; top++)
                    {
                        bool drawingEdge = top == halfHeight - diff || top == halfHeight + diff;
                        if (ConsoleResizeHandler.WasResized(false))
                            break;

                        // Determine the color by draw mode
                        var finalColor = drawingEdge ? selectedColor : colorShaded;
                        buffer.Append(TextWriterWhereColor.RenderWhereColorBack(" ", left, top, Color.Empty, finalColor));
                    }
                }

                // Now, draw the diamond (the second part)
                for (int left = halfWidth; left <= maxLeft; left++)
                {
                    int diff = left - maxLeft;
                    for (int top = halfHeight + diff; top <= halfHeight - diff; top++)
                    {
                        bool drawingEdge = top == halfHeight - diff || top == halfHeight + diff;
                        if (ConsoleResizeHandler.WasResized(false))
                            break;

                        // Determine the color by draw mode
                        var finalColor = drawingEdge ? selectedColor : colorShaded;
                        buffer.Append(TextWriterWhereColor.RenderWhereColorBack(" ", left, top, Color.Empty, finalColor));
                    }
                }

                // Now, write the diamond buffer
                TextWriterRaw.WritePlain(buffer.ToString(), false);
                buffer.Clear();
                seen = true;
            }

            // If shining, fade the small light in
            if (shining)
            {
                // Get the shined color
                int shinedR = selectedColor.RGB.R * 2;
                if (shinedR > 255)
                    shinedR = 255;
                int shinedG = selectedColor.RGB.G * 2;
                if (shinedG > 255)
                    shinedG = 255;
                int shinedB = selectedColor.RGB.B * 2;
                if (shinedB > 255)
                    shinedB = 255;
                var shined = new Color(shinedR, shinedG, shinedB);

                // Get the threshold from either the shade (for the bottom), the diamond color (for the center), or the
                // black color (the rest of the directions)
                var black = new Color(ConsoleColors.Black);
                double thresholdBottomR = (colorShaded.RGB.R - shined.RGB.R) / (double)maxShineSteps;
                double thresholdBottomG = (colorShaded.RGB.G - shined.RGB.G) / (double)maxShineSteps;
                double thresholdBottomB = (colorShaded.RGB.B - shined.RGB.B) / (double)maxShineSteps;
                double thresholdCenterR = (selectedColor.RGB.R - shined.RGB.R) / (double)maxShineSteps;
                double thresholdCenterG = (selectedColor.RGB.G - shined.RGB.G) / (double)maxShineSteps;
                double thresholdCenterB = (selectedColor.RGB.B - shined.RGB.B) / (double)maxShineSteps;
                double thresholdRestR = (black.RGB.R - shined.RGB.R) / (double)maxShineSteps;
                double thresholdRestG = (black.RGB.G - shined.RGB.G) / (double)maxShineSteps;
                double thresholdRestB = (black.RGB.B - shined.RGB.B) / (double)maxShineSteps;

                // Now, transition to the target color
                double currentBottomR = colorShaded.RGB.R;
                double currentBottomG = colorShaded.RGB.G;
                double currentBottomB = colorShaded.RGB.B;
                double currentCenterR = selectedColor.RGB.R;
                double currentCenterG = selectedColor.RGB.G;
                double currentCenterB = selectedColor.RGB.B;
                double currentRestR = black.RGB.R;
                double currentRestG = black.RGB.G;
                double currentRestB = black.RGB.B;
                for (int currentStep = 1; currentStep <= maxShineSteps; currentStep++)
                {
                    if (ConsoleResizeHandler.WasResized(false))
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
                    buffer.Append(
                        TextWriterWhereColor.RenderWhereColorBack(" ", shinePlaceX, shinePlaceY, Color.Empty, colCenter) +
                        TextWriterWhereColor.RenderWhereColorBack(" ", shinePlaceX, shinePlaceY + 1, Color.Empty, colBottom) +
                        TextWriterWhereColor.RenderWhereColorBack(" ", shinePlaceX, shinePlaceY + 2, Color.Empty, colBottom) +
                        TextWriterWhereColor.RenderWhereColorBack(" ", shinePlaceX, shinePlaceY - 1, Color.Empty, colRest) +
                        TextWriterWhereColor.RenderWhereColorBack(" ", shinePlaceX, shinePlaceY - 2, Color.Empty, colRest) +
                        TextWriterWhereColor.RenderWhereColorBack(" ", shinePlaceX - 1, shinePlaceY, Color.Empty, colRest) +
                        TextWriterWhereColor.RenderWhereColorBack(" ", shinePlaceX - 2, shinePlaceY, Color.Empty, colRest) +
                        TextWriterWhereColor.RenderWhereColorBack(" ", shinePlaceX + 1, shinePlaceY, Color.Empty, colRest) +
                        TextWriterWhereColor.RenderWhereColorBack(" ", shinePlaceX + 2, shinePlaceY, Color.Empty, colRest) +
                        TextWriterWhereColor.RenderWhereColorBack(" ", shinePlaceX2, shinePlaceY2, Color.Empty, colCenter) +
                        TextWriterWhereColor.RenderWhereColorBack(" ", shinePlaceX2, shinePlaceY2 + 1, Color.Empty, colRest) +
                        TextWriterWhereColor.RenderWhereColorBack(" ", shinePlaceX2, shinePlaceY2 + 2, Color.Empty, colRest) +
                        TextWriterWhereColor.RenderWhereColorBack(" ", shinePlaceX2, shinePlaceY2 - 1, Color.Empty, colRest) +
                        TextWriterWhereColor.RenderWhereColorBack(" ", shinePlaceX2, shinePlaceY2 - 2, Color.Empty, colRest) +
                        TextWriterWhereColor.RenderWhereColorBack(" ", shinePlaceX2 - 1, shinePlaceY2, Color.Empty, colRest) +
                        TextWriterWhereColor.RenderWhereColorBack(" ", shinePlaceX2 - 2, shinePlaceY2, Color.Empty, colRest) +
                        TextWriterWhereColor.RenderWhereColorBack(" ", shinePlaceX2 + 1, shinePlaceY2, Color.Empty, colBottom) +
                        TextWriterWhereColor.RenderWhereColorBack(" ", shinePlaceX2 + 2, shinePlaceY2, Color.Empty, colBottom)
                    );

                    // Now, write the diamond buffer
                    TextWriterRaw.WritePlain(buffer.ToString(), false);
                    buffer.Clear();

                    // Sleep
                    ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.DiamondDelay);
                }
                for (int currentStep = 1; currentStep <= maxShineSteps; currentStep++)
                {
                    if (ConsoleResizeHandler.WasResized(false))
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
                    buffer.Append(
                        TextWriterWhereColor.RenderWhereColorBack(" ", shinePlaceX, shinePlaceY, Color.Empty, colCenter) +
                        TextWriterWhereColor.RenderWhereColorBack(" ", shinePlaceX, shinePlaceY + 1, Color.Empty, colBottom) +
                        TextWriterWhereColor.RenderWhereColorBack(" ", shinePlaceX, shinePlaceY + 2, Color.Empty, colBottom) +
                        TextWriterWhereColor.RenderWhereColorBack(" ", shinePlaceX, shinePlaceY - 1, Color.Empty, colRest) +
                        TextWriterWhereColor.RenderWhereColorBack(" ", shinePlaceX, shinePlaceY - 2, Color.Empty, colRest) +
                        TextWriterWhereColor.RenderWhereColorBack(" ", shinePlaceX - 1, shinePlaceY, Color.Empty, colRest) +
                        TextWriterWhereColor.RenderWhereColorBack(" ", shinePlaceX - 2, shinePlaceY, Color.Empty, colRest) +
                        TextWriterWhereColor.RenderWhereColorBack(" ", shinePlaceX + 1, shinePlaceY, Color.Empty, colRest) +
                        TextWriterWhereColor.RenderWhereColorBack(" ", shinePlaceX + 2, shinePlaceY, Color.Empty, colRest) +
                        TextWriterWhereColor.RenderWhereColorBack(" ", shinePlaceX2, shinePlaceY2, Color.Empty, colCenter) +
                        TextWriterWhereColor.RenderWhereColorBack(" ", shinePlaceX2, shinePlaceY2 + 1, Color.Empty, colRest) +
                        TextWriterWhereColor.RenderWhereColorBack(" ", shinePlaceX2, shinePlaceY2 + 2, Color.Empty, colRest) +
                        TextWriterWhereColor.RenderWhereColorBack(" ", shinePlaceX2, shinePlaceY2 - 1, Color.Empty, colRest) +
                        TextWriterWhereColor.RenderWhereColorBack(" ", shinePlaceX2, shinePlaceY2 - 2, Color.Empty, colRest) +
                        TextWriterWhereColor.RenderWhereColorBack(" ", shinePlaceX2 - 1, shinePlaceY2, Color.Empty, colRest) +
                        TextWriterWhereColor.RenderWhereColorBack(" ", shinePlaceX2 - 2, shinePlaceY2, Color.Empty, colRest) +
                        TextWriterWhereColor.RenderWhereColorBack(" ", shinePlaceX2 + 1, shinePlaceY2, Color.Empty, colBottom) +
                        TextWriterWhereColor.RenderWhereColorBack(" ", shinePlaceX2 + 2, shinePlaceY2, Color.Empty, colBottom)
                    );

                    // Now, write the diamond buffer
                    TextWriterRaw.WritePlain(buffer.ToString(), false);
                    buffer.Clear();

                    // Sleep
                    ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.DiamondDelay);
                }
            }

            // Delay
            ConsoleResizeHandler.WasResized();
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.DiamondDelay);
        }

    }
}
