﻿
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

using System;
using System.Collections.Generic;
using System.Text;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Drivers.RNG;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Kernel.Threading;
using Terminaux.Colors;
using Terminaux.Sequences.Builder.Types;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for MatrixBleed
    /// </summary>
    public static class MatrixBleedSettings
    {

        /// <summary>
        /// [MatrixBleed] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int MatrixBleedDelay
        {
            get
            {
                return Config.SaverConfig.MatrixBleedDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                Config.SaverConfig.MatrixBleedDelay = value;
            }
        }
        /// <summary>
        /// [MatrixBleed] How many fade steps to do?
        /// </summary>
        public static int MatrixBleedMaxSteps
        {
            get
            {
                return Config.SaverConfig.MatrixBleedMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                Config.SaverConfig.MatrixBleedMaxSteps = value;
            }
        }
        /// <summary>
        /// [MatrixBleed] Chance to drop a new falling color
        /// </summary>
        public static int MatrixBleedDropChance
        {
            get
            {
                return Config.SaverConfig.MatrixBleedDropChance;
            }
            set
            {
                if (value <= 0)
                    value = 40;
                Config.SaverConfig.MatrixBleedDropChance = value;
            }
        }

    }

    /// <summary>
    /// Display code for MatrixBleed
    /// </summary>
    public class MatrixBleedDisplay : BaseScreensaver, IScreensaver
    {

        private static readonly List<MatrixBleedState> bleedStates = new();

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "MatrixBleed";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            KernelColorTools.LoadBack("0;0;0");
            ConsoleWrapper.Clear();
            ConsoleWrapper.CursorVisible = false;
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Now, determine the fall end position
            int FallEnd = ConsoleWrapper.WindowHeight - 1;

            // Invoke the "chance"-based random number generator to decide whether a line is about to fall.
            bool newFall = RandomDriver.RandomChance(MatrixBleedSettings.MatrixBleedDropChance);
            if (newFall)
                bleedStates.Add(new MatrixBleedState());

            // Now, iterate through the bleed states
            for (int bleedIdx = 0; bleedIdx < bleedStates.Count; bleedIdx++)
            {
                // Choose the column for the falling line
                var bleedState = bleedStates[bleedIdx];

                // Make the line fall down
                switch (bleedState.fallState)
                {
                    case MatrixBleedFallState.Falling:
                        bleedState.Fall();
                        bleedState.fallStep++;
                        if (bleedState.fallStep > FallEnd)
                            bleedState.fallState = MatrixBleedFallState.Fading;
                        break;
                    case MatrixBleedFallState.Fading:
                        bleedState.Fade();
                        bleedState.fadeStep++;
                        if (bleedState.fadeStep > MatrixBleedSettings.MatrixBleedMaxSteps)
                            bleedState.fallState = MatrixBleedFallState.Done;
                        break;
                }
            }

            // Purge the "Done" falls
            for (int bleedIdx = bleedStates.Count - 1; bleedIdx >= 0; bleedIdx--)
            {
                var bleedState = bleedStates[bleedIdx];
                if (bleedState.fallState == MatrixBleedFallState.Done)
                {
                    bleedStates.RemoveAt(bleedIdx);
                    bleedState.Unreserve(bleedState.ColumnLine);
                }
            }

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(MatrixBleedSettings.MatrixBleedDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

        /// <inheritdoc/>
        public override void ScreensaverOutro() =>
            bleedStates.Clear();

    }

    internal class MatrixBleedState
    {
        internal MatrixBleedFallState fallState = MatrixBleedFallState.Falling;
        internal int ColumnLine;
        internal int fallStep;
        internal int fadeStep;
        private readonly List<(int, int, string)> CoveredPositions = new();
        private readonly Color foreground = new("0;255;0");
        private readonly Color background = new("0;0;0");
        private static readonly List<int> reservedColumns = new();

        internal void Fall()
        {
            // Check to see if user decided to resize
            if (ConsoleResizeListener.WasResized(false))
                return;

            // Print a block and add the covered position to the list so fading down can be done
            string renderedNumber = RandomDriver.Random(1).ToString();
            TextWriterWhereColor.WriteWhereColorBack(renderedNumber, ColumnLine, fallStep, false, foreground, background);
            var PositionTuple = (ColumnLine, fallStep, renderedNumber);
            CoveredPositions.Add(PositionTuple);
        }

        internal void Fade()
        {
            // Check to see if user decided to resize
            if (ConsoleResizeListener.WasResized(false))
                return;

            // Set thresholds
            double ThresholdRed = foreground.R / (double)MatrixBleedSettings.MatrixBleedMaxSteps;
            double ThresholdGreen = foreground.G / (double)MatrixBleedSettings.MatrixBleedMaxSteps;
            double ThresholdBlue = foreground.B / (double)MatrixBleedSettings.MatrixBleedMaxSteps;
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Color threshold (R;G;B: {0})", ThresholdRed, ThresholdGreen, ThresholdBlue);

            // Set color fade steps
            int CurrentColorRedOut = (int)Math.Round(foreground.R - ThresholdRed * fadeStep);
            int CurrentColorGreenOut = (int)Math.Round(foreground.G - ThresholdGreen * fadeStep);
            int CurrentColorBlueOut = (int)Math.Round(foreground.B - ThresholdBlue * fadeStep);
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut);

            // Get the positions and write the block with new color
            var CurrentFadeColor = new Color(CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut);
            var bleedBuilder = new StringBuilder();
            foreach ((int, int, string) PositionTuple in CoveredPositions)
            {
                // Check to see if user decided to resize
                if (ConsoleResizeListener.WasResized(false))
                    break;

                // Actually fade the line out
                int PositionLeft = PositionTuple.Item1;
                int PositionTop = PositionTuple.Item2;
                string renderedNumber = PositionTuple.Item3;
                bleedBuilder.Append($"{CsiSequences.GenerateCsiCursorPosition(PositionLeft + 1, PositionTop + 1)}{renderedNumber}");
            }
            TextWriterWhereColor.WriteWhereColorBack(bleedBuilder.ToString(), ColumnLine, 0, false, CurrentFadeColor, background);
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

    internal enum MatrixBleedFallState
    {
        Falling,
        Fading,
        Done
    }
}