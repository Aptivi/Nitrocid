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

using System;
using System.Collections.Generic;
using System.Text;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Misc.Screensaver;
using Terminaux.Colors;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Base;
using Nitrocid.Base.Kernel.Configuration;
using Terminaux.Base.Extensions;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for FallingLine
    /// </summary>
    public class FallingLineDisplay : BaseScreensaver, IScreensaver
    {

        private int ColumnLine;
        private readonly List<Tuple<int, int>> CoveredPositions = [];

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "FallingLine";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            CoveredPositions.Clear();
            ConsoleColoring.LoadBackDry("0;0;0");
            ConsoleWrapper.Clear();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Choose the column for the falling line
            ColumnLine = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);

            // Now, determine the fall start and end position
            int FallStart = 0;
            int FallEnd = ConsoleWrapper.WindowHeight - 1;

            // Select the color
            Color ColorStorage;
            if (ScreensaverPackInit.SaversConfig.FallingLineTrueColor)
            {
                int RedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.FallingLineMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.FallingLineMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.FallingLineMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.FallingLineMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.FallingLineMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.FallingLineMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", vars: [RedColorNum, GreenColorNum, BlueColorNum]);
                ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                ConsoleColoring.SetConsoleColorDry(ColorStorage, true);
            }
            else
            {
                int ColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.FallingLineMinimumColorLevel, ScreensaverPackInit.SaversConfig.FallingLineMaximumColorLevel);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color ({0})", vars: [ColorNum]);
                ColorStorage = new Color(ColorNum);
                ConsoleColoring.SetConsoleColorDry(ColorStorage, true);
            }

            // Make the line fall down
            for (int Fall = FallStart; Fall <= FallEnd; Fall++)
            {
                // Check to see if user decided to resize
                if (ConsoleResizeHandler.WasResized(false))
                    break;
                if (ScreensaverManager.Bailing)
                    return;

                // Print a block and add the covered position to the list so fading down can be done
                TextWriterWhereColor.WriteWhereColorBack(" ", ColumnLine, Fall, false, Color.Empty, ColorStorage);
                var PositionTuple = new Tuple<int, int>(ColumnLine, Fall);
                CoveredPositions.Add(PositionTuple);

                // Delay
                ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.FallingLineDelay);
            }

            // Fade the line down. Please note that this requires true-color support in the terminal to work properly.
            for (int StepNum = 0; StepNum <= ScreensaverPackInit.SaversConfig.FallingLineMaxSteps; StepNum++)
            {
                // Check to see if user decided to resize
                if (ConsoleResizeHandler.WasResized(false))
                    break;
                if (ScreensaverManager.Bailing)
                    return;

                // Set thresholds
                double ThresholdRed = ColorStorage.RGB.R / (double)ScreensaverPackInit.SaversConfig.FallingLineMaxSteps;
                double ThresholdGreen = ColorStorage.RGB.G / (double)ScreensaverPackInit.SaversConfig.FallingLineMaxSteps;
                double ThresholdBlue = ColorStorage.RGB.B / (double)ScreensaverPackInit.SaversConfig.FallingLineMaxSteps;
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Color threshold (R;G;B: {0})", vars: [ThresholdRed, ThresholdGreen, ThresholdBlue]);

                // Set color fade steps
                int CurrentColorRedOut = (int)Math.Round(ColorStorage.RGB.R - ThresholdRed * StepNum);
                int CurrentColorGreenOut = (int)Math.Round(ColorStorage.RGB.G - ThresholdGreen * StepNum);
                int CurrentColorBlueOut = (int)Math.Round(ColorStorage.RGB.B - ThresholdBlue * StepNum);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", vars: [CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut]);

                // Get the positions and write the block with new color
                var CurrentFadeColor = new Color(CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut);
                var bleedBuilder = new StringBuilder();
                foreach (Tuple<int, int> PositionTuple in CoveredPositions)
                {
                    // Check to see if user decided to resize
                    if (ConsoleResizeHandler.WasResized(false))
                        break;
                    if (ScreensaverManager.Bailing)
                        return;

                    // Actually fade the line out
                    int PositionLeft = PositionTuple.Item1;
                    int PositionTop = PositionTuple.Item2;
                    bleedBuilder.Append($"{CsiSequences.GenerateCsiCursorPosition(PositionLeft + 1, PositionTop + 1)} ");
                }
                TextWriterWhereColor.WriteWhereColorBack(bleedBuilder.ToString(), ColumnLine, 0, false, Color.Empty, CurrentFadeColor);

                // Delay
                ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.FallingLineDelay);
            }

            // Reset covered positions
            CoveredPositions.Clear();
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.FallingLineDelay);
        }

        /// <inheritdoc/>
        public override void ScreensaverOutro() =>
            CoveredPositions.Clear();

    }
}
