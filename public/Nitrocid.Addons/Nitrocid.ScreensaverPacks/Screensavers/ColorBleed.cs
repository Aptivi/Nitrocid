//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for ColorBleed
    /// </summary>
    public class ColorBleedDisplay : BaseScreensaver, IScreensaver
    {

        private static readonly List<BleedState> bleedStates = [];

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "ColorBleed";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            bleedStates.Clear();
            ColorTools.LoadBackDry("0;0;0");
            ConsoleWrapper.Clear();
            ConsoleWrapper.CursorVisible = false;
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Now, determine the fall end position
            int FallEnd = ConsoleWrapper.WindowHeight - 1;

            // Invoke the "chance"-based random number generator to decide whether a line is about to fall.
            bool newFall = RandomDriver.RandomChance(ScreensaverPackInit.SaversConfig.ColorBleedDropChance);
            if (newFall)
                bleedStates.Add(new BleedState());

            // Now, iterate through the bleed states
            for (int bleedIdx = 0; bleedIdx < bleedStates.Count; bleedIdx++)
            {
                // Choose the column for the falling line
                var bleedState = bleedStates[bleedIdx];

                // Make the line fall down
                switch (bleedState.fallState)
                {
                    case BleedFallState.Falling:
                        bleedState.Fall();
                        bleedState.fallStep++;
                        if (bleedState.fallStep > FallEnd)
                            bleedState.fallState = BleedFallState.Fading;
                        break;
                    case BleedFallState.Fading:
                        bleedState.Fade();
                        bleedState.fadeStep++;
                        if (bleedState.fadeStep > ScreensaverPackInit.SaversConfig.ColorBleedMaxSteps)
                            bleedState.fallState = BleedFallState.Done;
                        break;
                }
            }

            // Purge the "Done" falls
            for (int bleedIdx = bleedStates.Count - 1; bleedIdx >= 0; bleedIdx--)
            {
                var bleedState = bleedStates[bleedIdx];
                if (bleedState.fallState == BleedFallState.Done)
                {
                    bleedStates.RemoveAt(bleedIdx);
                    bleedState.Unreserve(bleedState.ColumnLine);
                }
            }

            // Draw and clean the buffer
            string buffer = BleedState.bleedBuffer.ToString();
            BleedState.bleedBuffer.Clear();
            TextWriterRaw.WritePlain(buffer);

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.ColorBleedDelay);
        }

        /// <inheritdoc/>
        public override void ScreensaverOutro() =>
            bleedStates.Clear();

    }

    internal class BleedState
    {
        internal BleedFallState fallState = BleedFallState.Falling;
        internal int ColumnLine;
        internal int fallStep;
        internal int fadeStep;
        internal static StringBuilder bleedBuffer = new();
        internal static readonly List<int> reservedColumns = [];
        private readonly Color ColorStorage;
        private readonly List<Tuple<int, int>> CoveredPositions = [];

        internal void Fall()
        {
            // Check to see if user decided to resize
            if (ConsoleResizeHandler.WasResized(false))
                return;

            // Print a block and add the covered position to the list so fading down can be done
            bleedBuffer.Append(
                $"{CsiSequences.GenerateCsiCursorPosition(ColumnLine + 1, fallStep + 1)}" +
                $"{Color.Empty.VTSequenceForeground}" +
                $"{ColorStorage.VTSequenceBackground}" +
                $" "
            );
            TextWriterWhereColor.WriteWhereColorBack(" ", ColumnLine, fallStep, false, Color.Empty, ColorStorage);
            var PositionTuple = new Tuple<int, int>(ColumnLine, fallStep);
            CoveredPositions.Add(PositionTuple);
        }

        internal void Fade()
        {
            // Check to see if user decided to resize
            if (ConsoleResizeHandler.WasResized(false))
                return;

            // Set thresholds
            double ThresholdRed = ColorStorage.RGB.R / (double)ScreensaverPackInit.SaversConfig.ColorBleedMaxSteps;
            double ThresholdGreen = ColorStorage.RGB.G / (double)ScreensaverPackInit.SaversConfig.ColorBleedMaxSteps;
            double ThresholdBlue = ColorStorage.RGB.B / (double)ScreensaverPackInit.SaversConfig.ColorBleedMaxSteps;
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Color threshold (R;G;B: {0})", vars: [ThresholdRed, ThresholdGreen, ThresholdBlue]);

            // Set color fade steps
            int CurrentColorRedOut = (int)Math.Round(ColorStorage.RGB.R - ThresholdRed * fadeStep);
            int CurrentColorGreenOut = (int)Math.Round(ColorStorage.RGB.G - ThresholdGreen * fadeStep);
            int CurrentColorBlueOut = (int)Math.Round(ColorStorage.RGB.B - ThresholdBlue * fadeStep);
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", vars: [CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut]);

            // Get the positions and write the block with new color
            var CurrentFadeColor = new Color(CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut);
            foreach (Tuple<int, int> PositionTuple in CoveredPositions)
            {
                // Check to see if user decided to resize
                if (ConsoleResizeHandler.WasResized(false))
                    break;

                // Actually fade the line out
                int PositionLeft = PositionTuple.Item1;
                int PositionTop = PositionTuple.Item2;
                bleedBuffer.Append(
                    $"{CsiSequences.GenerateCsiCursorPosition(PositionLeft + 1, PositionTop + 1)}" +
                    $"{Color.Empty.VTSequenceForeground}" +
                    $"{CurrentFadeColor.VTSequenceBackground}" +
                     " ");
            }
        }

        internal void Unreserve(int column) =>
            reservedColumns.Remove(column);

        internal BleedState()
        {
            int columnLine = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
            while (reservedColumns.Contains(columnLine))
                columnLine = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
            reservedColumns.Add(columnLine);
            ColumnLine = columnLine;
            if (ScreensaverPackInit.SaversConfig.ColorBleedTrueColor)
            {
                int RedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.ColorBleedMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.ColorBleedMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.ColorBleedMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.ColorBleedMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.ColorBleedMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.ColorBleedMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", vars: [RedColorNum, GreenColorNum, BlueColorNum]);
                ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.ColorBleedMinimumColorLevel, ScreensaverPackInit.SaversConfig.ColorBleedMaximumColorLevel);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color ({0})", vars: [ColorNum]);
                ColorStorage = new Color(ColorNum);
            }
        }
    }

    internal enum BleedFallState
    {
        Falling,
        Fading,
        Done
    }
}
