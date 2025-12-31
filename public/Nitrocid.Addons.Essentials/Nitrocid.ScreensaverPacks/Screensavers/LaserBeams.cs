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
using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Misc.Screensaver;
using Nitrocid.Base.Kernel.Configuration;
using Terminaux.Colors;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Base;
using Terminaux.Colors.Data;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for LaserBeams
    /// </summary>
    public class LaserBeamsDisplay : BaseScreensaver, IScreensaver
    {

        private static readonly List<(int, int)> laserEnds = [];

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "LaserBeams";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            laserEnds.Clear();
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", vars: [ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight]);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;
            ColorTools.LoadBackDry(new Color(ScreensaverPackInit.SaversConfig.LaserBeamsBackgroundColor));

            // Populate the laser ends
            laserEnds.AddRange(
                [
                    (0, (ConsoleWrapper.WindowHeight * 5 / 5) - 1),
                    (0, (ConsoleWrapper.WindowHeight * 4 / 5) - 1),
                    (0, (ConsoleWrapper.WindowHeight * 3 / 5) - 1),
                    (0, (ConsoleWrapper.WindowHeight * 2 / 5) - 1),
                    (0, (ConsoleWrapper.WindowHeight / 5) - 1),
                    (0, 0),
                    ((ConsoleWrapper.WindowWidth / 5) - 1, 0),
                    ((ConsoleWrapper.WindowWidth * 2 / 5) - 1, 0),
                    ((ConsoleWrapper.WindowWidth * 3 / 5) - 1, 0),
                    ((ConsoleWrapper.WindowWidth * 4 / 5) - 1, 0),
                    ((ConsoleWrapper.WindowWidth * 5 / 5) - 1, 0),
                ]
            );

            // Draw few beams
            int laserStartPosX = ConsoleWrapper.WindowWidth - 1;
            int laserStartPosY = ConsoleWrapper.WindowHeight - 1;
            StringBuilder laserBeamsBuilder = new();
            for (int i = 0; i < 11; i++)
            {
                // Select a color
                Color colorStorage;
                if (ScreensaverPackInit.SaversConfig.LaserBeamsTrueColor)
                {
                    int RedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.LaserBeamsMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.LaserBeamsMaximumRedColorLevel);
                    int GreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.LaserBeamsMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.LaserBeamsMaximumGreenColorLevel);
                    int BlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.LaserBeamsMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.LaserBeamsMaximumBlueColorLevel);
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", vars: [RedColorNum, GreenColorNum, BlueColorNum]);
                    colorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                }
                else
                {
                    int color = RandomDriver.Random(ScreensaverPackInit.SaversConfig.LaserBeamsMinimumColorLevel, ScreensaverPackInit.SaversConfig.LaserBeamsMaximumColorLevel);
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color ({0})", vars: [color]);
                    colorStorage = new Color(color);
                }

                // Get the laser end positions
                int laserEndPosX = laserEnds[i].Item1;
                int laserEndPosY = laserEnds[i].Item2;

                // Now, draw a laser beam
                double laserPosThresholdX = -(laserEndPosX - laserStartPosX) / (double)ConsoleWrapper.WindowWidth;
                double laserPosThresholdY = -(laserEndPosY - laserStartPosY) / ((double)ConsoleWrapper.WindowHeight * 4);
                int currentPosX = laserStartPosX;
                int currentPosY = laserStartPosY;
                int step = 0;
                while (currentPosX > 0 && currentPosY > 0)
                {
                    currentPosX = (int)(laserStartPosX - (laserPosThresholdX * step));
                    currentPosY = (int)(laserStartPosY - (laserPosThresholdY * step));
                    laserBeamsBuilder.Append($"{colorStorage.VTSequenceBackground}{CsiSequences.GenerateCsiCursorPosition(currentPosX + 1, currentPosY + 1)} ");
                    step++;
                }
            }

            // Make a beaming white particle to make it as if the lasers are fired
            laserBeamsBuilder.Append($"{new Color(ConsoleColors.White).VTSequenceBackground}{CsiSequences.GenerateCsiCursorPosition(ConsoleWrapper.WindowWidth + 1, ConsoleWrapper.WindowHeight + 1)} ");

            // Write the result
            TextWriterRaw.WritePlain(laserBeamsBuilder.ToString(), false);

            // Reset resize sync
            laserEnds.Clear();
            ConsoleResizeHandler.WasResized();
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.LaserBeamsDelay);
        }

        /// <inheritdoc/>
        public override void ScreensaverOutro() =>
            laserEnds.Clear();

    }
}
