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
using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Debugging;
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Sequences.Builder.Types;

namespace Nitrocid.Base.Misc.Screensaver.Displays.Utilities
{
    internal class MatrixBleedState
    {
        internal MatrixBleedFallState fallState = MatrixBleedFallState.Falling;
        internal int ColumnLine;
        internal int fallStep;
        internal int fadeStep;
        internal static StringBuilder bleedBuffer = new();
        private readonly List<(int, int, string)> CoveredPositions = [];
        private readonly Color foreground = new("0;255;0");
        private readonly Color background = new("0;0;0");
        private static readonly List<int> reservedColumns = [];

        internal void Fall()
        {
            // Check to see if user decided to resize
            if (ConsoleResizeHandler.WasResized(false))
                return;

            // Print a block and add the covered position to the list so fading down can be done
            string renderedNumber = RandomDriver.Random(1).ToString();
            bleedBuffer.Append(
                $"{CsiSequences.GenerateCsiCursorPosition(ColumnLine + 1, fallStep + 1)}" +
                $"{foreground.VTSequenceForeground()}" +
                $"{background.VTSequenceBackground()}" +
                $"{renderedNumber}"
            );
            var PositionTuple = (ColumnLine, fallStep, renderedNumber);
            CoveredPositions.Add(PositionTuple);
        }

        internal void Fade()
        {
            // Check to see if user decided to resize
            if (ConsoleResizeHandler.WasResized(false))
                return;

            // Set thresholds
            double ThresholdRed = foreground.RGB.R / (double)Config.SaverConfig.MatrixBleedMaxSteps;
            double ThresholdGreen = foreground.RGB.G / (double)Config.SaverConfig.MatrixBleedMaxSteps;
            double ThresholdBlue = foreground.RGB.B / (double)Config.SaverConfig.MatrixBleedMaxSteps;
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Color threshold (R;G;B: {0})", vars: [ThresholdRed, ThresholdGreen, ThresholdBlue]);

            // Set color fade steps
            int CurrentColorRedOut = (int)Math.Round(foreground.RGB.R - ThresholdRed * fadeStep);
            int CurrentColorGreenOut = (int)Math.Round(foreground.RGB.G - ThresholdGreen * fadeStep);
            int CurrentColorBlueOut = (int)Math.Round(foreground.RGB.B - ThresholdBlue * fadeStep);
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", vars: [CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut]);

            // Get the positions and write the block with new color
            var CurrentFadeColor = new Color(CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut);
            foreach ((int, int, string) PositionTuple in CoveredPositions)
            {
                // Check to see if user decided to resize
                if (ConsoleResizeHandler.WasResized(false))
                    break;

                // Actually fade the line out
                int PositionLeft = PositionTuple.Item1;
                int PositionTop = PositionTuple.Item2;
                string renderedNumber = PositionTuple.Item3;
                bleedBuffer.Append(
                    $"{CsiSequences.GenerateCsiCursorPosition(PositionLeft + 1, PositionTop + 1)}" +
                    $"{CurrentFadeColor.VTSequenceForeground()}" +
                    $"{background.VTSequenceBackground()}" +
                    $"{renderedNumber}"
                );
            }
        }

        internal void Unreserve(int column) =>
            reservedColumns.Remove(column);

        internal MatrixBleedState()
        {
            int columnLine = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
            while (reservedColumns.Contains(columnLine))
                columnLine = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
            reservedColumns.Add(columnLine);
            ColumnLine = columnLine;
        }
    }
}
