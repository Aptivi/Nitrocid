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
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Misc.Screensaver;
using Nitrocid.Base.Kernel.Configuration;
using Terminaux.Colors;
using Terminaux.Base;
using Terminaux.Base.Extensions;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for Mesmerize
    /// </summary>
    public class MesmerizeDisplay : BaseScreensaver, IScreensaver
    {

        private enum DirectionMode
        {
            CenterMiddleToCenterRight,
            CenterRightToLowBottom,
            LowBottomToLowMiddle,
            LowMiddleToCenterMiddle,
            CenterMiddleToTopMiddle,
            TopMiddleToTopLeft,
            TopLeftToCenterLeft,
            CenterLeftToCenterMiddle,
        }

        private (int, int) dotCurrentPosition;
        private DirectionMode dotDirection = DirectionMode.CenterMiddleToCenterRight;
        private Color dotColor = Color.Empty;
        private readonly List<Color> dotColorShades = [];
        private readonly List<(int, int)> dotPositions = [];

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "Mesmerize";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Clear the console
            ConsoleColoring.LoadBackDry("0;0;0");
            ConsoleWrapper.Clear();

            // Reset dot positions and color shades
            dotPositions.Clear();
            dotCurrentPosition = (ConsoleWrapper.WindowWidth / 2,
                                  ConsoleWrapper.WindowHeight / 2);
            dotColorShades.Clear();
            dotDirection = DirectionMode.CenterMiddleToCenterRight;

            // Assign a color to the dot
            int RedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.MesmerizeMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.MesmerizeMaximumRedColorLevel);
            int GreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.MesmerizeMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.MesmerizeMaximumGreenColorLevel);
            int BlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.MesmerizeMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.MesmerizeMaximumBlueColorLevel);
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", vars: [RedColorNum, GreenColorNum, BlueColorNum]);
            dotColor = new Color(RedColorNum, GreenColorNum, BlueColorNum);

            // Assign shades of color
            int maxPositions = ConsoleWrapper.WindowWidth / 2;
            for (int i = 0; i < maxPositions; i++)
            {
                int finalR = (int)(dotColor.RGB.R * ((maxPositions - i - 1) / (double)(maxPositions - 1)));
                int finalG = (int)(dotColor.RGB.G * ((maxPositions - i - 1) / (double)(maxPositions - 1)));
                int finalB = (int)(dotColor.RGB.B * ((maxPositions - i - 1) / (double)(maxPositions - 1)));
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", vars: [finalR, finalG, finalB]);
                Color colorShade = new(finalR, finalG, finalB);
                dotColorShades.Add(colorShade);
            }
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            if (!ConsoleResizeHandler.WasResized(false))
            {
                // Setting maximum and minimum limits for console height and width
                int minLeft = 8;
                int midLeft = ConsoleWrapper.WindowWidth / 2;
                int maxLeft = ConsoleWrapper.WindowWidth - minLeft;
                int minTop = 2;
                int midTop = ConsoleWrapper.WindowHeight / 2;
                int maxTop = ConsoleWrapper.WindowHeight - minTop - 1;

                // For tails. We don't need more than midLeft covered positions.
                dotPositions.Add(dotCurrentPosition);
                if (dotPositions.Count > midLeft)
                    dotPositions.RemoveAt(0);

                // Now, iterate through all the dot positions to create a dot that will spin in two rectangles in opposing sides
                for (int i = 0; i < dotPositions.Count; i++)
                {
                    // Get the dot position and the shade color
                    int left = dotPositions[i].Item1;
                    int top = dotPositions[i].Item2;
                    Color finalDotColor = dotColorShades[dotPositions.Count - 1 - i];

                    // Just place the dot in its place.
                    TextWriterWhereColor.WriteWhereColorBack(" ", left, top, Color.Empty, finalDotColor);
                }

                // For positioning, change the dot position based on the state of movement
                switch (dotDirection)
                {
                    // Initial state. Moving from the middle of the center to the middle of the right edge.
                    case DirectionMode.CenterMiddleToCenterRight:
                        dotCurrentPosition.Item1++;
                        if (dotCurrentPosition.Item1 == maxLeft)
                            dotDirection++;
                        break;

                    // Moving from the middle of the right edge to the lower right corner.
                    case DirectionMode.CenterRightToLowBottom:
                        dotCurrentPosition.Item2++;
                        if (dotCurrentPosition.Item2 == maxTop)
                            dotDirection++;
                        break;

                    // Moving from the lower right corner to the middle of the bottom edge.
                    case DirectionMode.LowBottomToLowMiddle:
                        dotCurrentPosition.Item1--;
                        if (dotCurrentPosition.Item1 == midLeft)
                            dotDirection++;
                        break;

                    // Moving from the middle of the bottom edge to the middle of the center.
                    case DirectionMode.LowMiddleToCenterMiddle:
                        dotCurrentPosition.Item2--;
                        if (dotCurrentPosition.Item2 == midTop)
                            dotDirection++;
                        break;

                    // Moving from the middle of the center to the middle of the top edge.
                    case DirectionMode.CenterMiddleToTopMiddle:
                        dotCurrentPosition.Item2--;
                        if (dotCurrentPosition.Item2 == minTop)
                            dotDirection++;
                        break;

                    // Moving from the middle of the top edge to the upper left corner.
                    case DirectionMode.TopMiddleToTopLeft:
                        dotCurrentPosition.Item1--;
                        if (dotCurrentPosition.Item1 == minLeft)
                            dotDirection++;
                        break;

                    // Moving from the upper left corner to the middle of the left edge.
                    case DirectionMode.TopLeftToCenterLeft:
                        dotCurrentPosition.Item2++;
                        if (dotCurrentPosition.Item2 == midTop)
                            dotDirection++;
                        break;

                    // Moving from the middle of the left edge to the middle of the center.
                    case DirectionMode.CenterLeftToCenterMiddle:
                        dotCurrentPosition.Item1++;
                        if (dotCurrentPosition.Item1 == midLeft)
                            dotDirection = DirectionMode.CenterMiddleToCenterRight;
                        break;
                }
            }
            else
            {
                // Someone have resized the terminal window during screensaver display.
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.W, "Resize-syncing. Re-initializing...");
                ScreensaverPreparation();
            }
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.MesmerizeDelay);
        }

    }
}
