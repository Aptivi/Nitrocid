
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
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Drivers.RNG;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Kernel.Threading;
using Terminaux.Colors;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for Matrix
    /// </summary>
    public static class MatrixSettings
    {

        /// <summary>
        /// [Matrix] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int MatrixDelay
        {
            get
            {
                return Config.SaverConfig.MatrixDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                Config.SaverConfig.MatrixDelay = value;
            }
        }
        /// <summary>
        /// [Matrix] How many fade steps to do?
        /// </summary>
        public static int MatrixMaxSteps
        {
            get
            {
                return Config.SaverConfig.MatrixMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                Config.SaverConfig.MatrixMaxSteps = value;
            }
        }

    }

    /// <summary>
    /// Display code for Matrix
    /// </summary>
    public class MatrixDisplay : BaseScreensaver, IScreensaver
    {

        private int ColumnLine;
        private readonly List<(int, int, string)> CoveredPositions = new();
        private readonly Color foreground = new(ConsoleColors.Green);
        private readonly Color background = new(ConsoleColors.Black);

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Matrix";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            KernelColorTools.LoadBack(0);
            ConsoleWrapper.Clear();
            ConsoleWrapper.CursorVisible = false;
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Choose the column for the falling line
            ColumnLine = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);

            // Now, determine the fall start and end position
            int FallStart = 0;
            int FallEnd = ConsoleWrapper.WindowHeight - 1;

            // Make the line fall down
            for (int Fall = FallStart; Fall <= FallEnd; Fall++)
            {
                // Check to see if user decided to resize
                if (ConsoleResizeListener.WasResized(false))
                    break;

                // Print a block and add the covered position to the list so fading down can be done
                string renderedNumber = RandomDriver.Random(1).ToString();
                TextWriterWhereColor.WriteWhere(renderedNumber, ColumnLine, Fall, false, foreground, background);
                var PositionTuple = (ColumnLine, Fall, renderedNumber);
                CoveredPositions.Add(PositionTuple);

                // Delay
                ThreadManager.SleepNoBlock(MatrixSettings.MatrixDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            }

            // Fade the line down. Please note that this requires true-color support in the terminal to work properly.
            for (int StepNum = 0; StepNum <= MatrixSettings.MatrixMaxSteps; StepNum++)
            {
                // Check to see if user decided to resize
                if (ConsoleResizeListener.WasResized(false))
                    break;

                // Set thresholds
                double ThresholdRed = foreground.R / (double)MatrixSettings.MatrixMaxSteps;
                double ThresholdGreen = foreground.G / (double)MatrixSettings.MatrixMaxSteps;
                double ThresholdBlue = foreground.B / (double)MatrixSettings.MatrixMaxSteps;
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Color threshold (R;G;B: {0})", ThresholdRed, ThresholdGreen, ThresholdBlue);

                // Set color fade steps
                int CurrentColorRedOut = (int)Math.Round(foreground.R - ThresholdRed * StepNum);
                int CurrentColorGreenOut = (int)Math.Round(foreground.G - ThresholdGreen * StepNum);
                int CurrentColorBlueOut = (int)Math.Round(foreground.B - ThresholdBlue * StepNum);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut);

                // Get the positions and write the block with new color
                var CurrentFadeColor = new Color(CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut);
                foreach ((int, int, string) PositionTuple in CoveredPositions)
                {
                    // Check to see if user decided to resize
                    if (ConsoleResizeListener.WasResized(false))
                        break;

                    // Actually fade the line out
                    int PositionLeft = PositionTuple.Item1;
                    int PositionTop = PositionTuple.Item2;
                    string renderedNumber = PositionTuple.Item3;
                    TextWriterWhereColor.WriteWhere(renderedNumber, PositionLeft, PositionTop, false, CurrentFadeColor, background);
                }

                // Delay
                ThreadManager.SleepNoBlock(MatrixSettings.MatrixDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            }

            // Reset covered positions
            CoveredPositions.Clear();

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(MatrixSettings.MatrixDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
