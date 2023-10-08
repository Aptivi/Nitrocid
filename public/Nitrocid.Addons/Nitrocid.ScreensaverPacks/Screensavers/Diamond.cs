
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

        private static Color selectedColor = ConsoleColors.LightCyan1;
        private static readonly List<Color> diamondColors = new()
        {
            ConsoleColors.LightCyan1,
            ConsoleColors.Pink1,
            ConsoleColors.White
        };

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

            // Delay
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(DiamondSettings.DiamondDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
