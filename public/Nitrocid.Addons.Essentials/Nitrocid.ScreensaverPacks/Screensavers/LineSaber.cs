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
using Terminaux.Base;
using Terminaux.Colors;
using Nitrocid.Base.Kernel.Configuration;
using Terminaux.Writer.ConsoleWriters;
using System.Collections.Generic;
using Terminaux.Base.Extensions;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for LineSaber
    /// </summary>
    public class LineSaberDisplay : BaseScreensaver, IScreensaver
    {
        private List<(Color color, int position, bool vertical, bool reverse)> lines = [];

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "LineSaber";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            lines = [];
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Get line string
            string lineString = !string.IsNullOrWhiteSpace(ScreensaverPackInit.SaversConfig.LineSaberLineChar) ? ScreensaverPackInit.SaversConfig.LineSaberLineChar : "-";
            string verticalLineString = !string.IsNullOrWhiteSpace(ScreensaverPackInit.SaversConfig.LineSaberVerticalLineChar) ? ScreensaverPackInit.SaversConfig.LineSaberVerticalLineChar : "|";
            char lineStringChar = lineString[0];
            char verticalLineStringChar = verticalLineString[0];

            // Make a new line, if possible
            bool canCreateNewLine = RandomDriver.RandomChance(ScreensaverPackInit.SaversConfig.LineSaberLineDensity);
            if (canCreateNewLine)
            {
                bool vertical = RandomDriver.RandomChance(ScreensaverPackInit.SaversConfig.LineSaberVerticalLineDensity);
                bool reverse = RandomDriver.RandomChance(ScreensaverPackInit.SaversConfig.LineSaberReverseDensity);
                Color color = SelectColor();
                int position = 0;
                if (reverse)
                {
                    if (vertical)
                        position = ConsoleWrapper.WindowWidth - 1;
                    else
                        position = ConsoleWrapper.WindowHeight - 1;
                }
                lines.Add((color, position, vertical, reverse));
            }

            // Make a line string and its clear equivalent
            foreach (var (color, position, vertical, _) in lines)
            {
                string line = new(lineStringChar, ConsoleWrapper.WindowWidth);
                if (!ConsoleResizeHandler.WasResized(false))
                {
                    if (vertical)
                    {
                        for (int h = 0; h < ConsoleWrapper.WindowHeight; h++)
                            TextWriterWhereColor.WriteWhereColor($"{verticalLineStringChar}", position, h, color);
                    }
                    else
                    {
                        if (position < ConsoleWrapper.WindowHeight)
                            TextWriterWhereColor.WriteWhereColor(line, 0, position, color);
                    }
                }
            }
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.LineSaberDelay);

            // Clear all lines and reset them
            string clearLine = new(' ', ConsoleWrapper.WindowWidth);
            for (int i = lines.Count - 1; i >= 0; i--)
            {
                var (color, position, vertical, reverse) = lines[i];
                if (!ConsoleResizeHandler.WasResized(false))
                {
                    if (vertical)
                    {
                        for (int h = 0; h < ConsoleWrapper.WindowHeight; h++)
                            TextWriterWhereColor.WriteWhereColor(" ", position, h, color);
                    }
                    else
                    {
                        if (position < ConsoleWrapper.WindowHeight)
                            TextWriterWhereColor.WriteWherePlain(clearLine, 0, position);
                    }
                }

                // Reset the position if needed
                position += reverse ? -1 : 1;
                lines[i] = (color, position, vertical, reverse);
                if (position < 0 && reverse)
                    lines.RemoveAt(i);
                else if (position >= (vertical ? ConsoleWrapper.WindowWidth : ConsoleWrapper.WindowHeight) && !reverse)
                    lines.RemoveAt(i);
            }
        }

        private Color SelectColor()
        {
            if (ScreensaverPackInit.SaversConfig.LineSaberTrueColor)
            {
                ConsoleColoring.LoadBackDry(new Color(ScreensaverPackInit.SaversConfig.LineSaberBackgroundColor));
                int RedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.LineSaberMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.LineSaberMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.LineSaberMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.LineSaberMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.LineSaberMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.LineSaberMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", vars: [RedColorNum, GreenColorNum, BlueColorNum]);
                return new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                ConsoleColoring.LoadBackDry(new Color(ScreensaverPackInit.SaversConfig.LineSaberBackgroundColor));
                int color = RandomDriver.Random(ScreensaverPackInit.SaversConfig.LineSaberMinimumColorLevel, ScreensaverPackInit.SaversConfig.LineSaberMaximumColorLevel);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color ({0})", vars: [color]);
                return new Color(color);
            }
        }
    }
}
