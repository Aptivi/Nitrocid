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

using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Misc.Screensaver;
using Nitrocid.Base.Kernel.Configuration;
using System;
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Colors.Themes.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for Following
    /// </summary>
    public class FollowingDisplay : BaseScreensaver, IScreensaver
    {
        private Color? dotColor;
        private (int x, int y) start;
        private (int x, int y) end;
        private int currentStep;

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "Following";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            if (ScreensaverPackInit.SaversConfig.FollowingTrueColor)
            {
                int RedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.FollowingMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.FollowingMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.FollowingMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.FollowingMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.FollowingMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.FollowingMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", vars: [RedColorNum, GreenColorNum, BlueColorNum]);
                var ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                dotColor = ColorStorage;
            }
            else
            {
                int color = RandomDriver.Random(ScreensaverPackInit.SaversConfig.FollowingMinimumColorLevel, ScreensaverPackInit.SaversConfig.FollowingMaximumColorLevel);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color ({0})", vars: [color]);
                dotColor = new Color(color);
            }
            start = (RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth), RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight));
            end = (RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth), RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight));
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", vars: [ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight]);
            currentStep = 0;
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Clear screen if necessary
            if (currentStep == 20)
            {
                currentStep = 0;
                ThemeColorsTools.LoadBackground();
            }

            // Draw the start and the end position marker
            ColorTools.SetConsoleColorDry(new(ConsoleColors.Lime), true);
            TextWriterWhereColor.WriteWherePlain(" ", start.x, start.y);
            TextWriterWhereColor.WriteWherePlain(" ", end.x, end.y);

            // Select a color
            if (dotColor is not null)
                ColorTools.SetConsoleColorDry(dotColor, true);

            // Draw a dot
            int posX = start.x;
            int posY = start.y;
            int width = end.x - start.x;
            int height = end.y - start.y;

            // Get the differences according to the given points
            int differenceX1 = width < 0 ? -1 : width > 0 ? 1 : 0;
            int differenceX2 = width < 0 ? -1 : width > 0 ? 1 : 0;
            int differenceY1 = height < 0 ? -1 : height > 0 ? 1 : 0;
            int differenceY2 = 0;

            // Get the longest and the shortest width and height
            int longestLine = Math.Abs(width);
            int shortestLine = Math.Abs(height);
            if (longestLine <= shortestLine)
            {
                longestLine = Math.Abs(height);
                shortestLine = Math.Abs(width);
                differenceY2 = height < 0 ? -1 : height > 0 ? 1 : 0;
                differenceX2 = 0;
            }

            // Now, render a line and move on
            int determine = longestLine >> 1;
            for (int i = 0; i <= longestLine; i++)
            {
                if (ConsoleResizeHandler.WasResized(false))
                    break;
                TextWriterWhereColor.WriteWherePlain(" ", posX, posY);
                determine += shortestLine;
                if (determine >= longestLine)
                {
                    // We've reached the longest iteration!
                    determine -= longestLine;
                    posX += differenceX1;
                    posY += differenceY1;
                }
                else
                {
                    // Keep going...
                    posX += differenceX2;
                    posY += differenceY2;
                }
                ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.FollowingDelay);
            }

            // Reset resize sync
            if (ConsoleResizeHandler.WasResized(false))
            {
                start = (RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth), RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight));
                end = (RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth), RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight));
            }
            else
            {
                start = (end.x, end.y);
                end = (RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth), RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight));
            }
            currentStep++;
            ConsoleResizeHandler.WasResized();
        }

    }
}
