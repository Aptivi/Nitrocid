//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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
using Nitrocid.ConsoleBase;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers.ConsoleWriters;
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Threading;
using Nitrocid.Misc.Screensaver;
using Terminaux.Colors;
using Textify.Sequences.Builder.Types;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Settings for FallingLine
    /// </summary>
    public static class FallingLineSettings
    {

        /// <summary>
        /// [FallingLine] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool FallingLineTrueColor
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FallingLineTrueColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.FallingLineTrueColor = value;
            }
        }
        /// <summary>
        /// [FallingLine] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int FallingLineDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FallingLineDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                ScreensaverPackInit.SaversConfig.FallingLineDelay = value;
            }
        }
        /// <summary>
        /// [FallingLine] How many fade steps to do?
        /// </summary>
        public static int FallingLineMaxSteps
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FallingLineMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                ScreensaverPackInit.SaversConfig.FallingLineMaxSteps = value;
            }
        }
        /// <summary>
        /// [FallingLine] The minimum red color level (true color)
        /// </summary>
        public static int FallingLineMinimumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FallingLineMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.FallingLineMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FallingLine] The minimum green color level (true color)
        /// </summary>
        public static int FallingLineMinimumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FallingLineMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.FallingLineMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FallingLine] The minimum blue color level (true color)
        /// </summary>
        public static int FallingLineMinimumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FallingLineMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.FallingLineMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [FallingLine] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int FallingLineMinimumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FallingLineMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                ScreensaverPackInit.SaversConfig.FallingLineMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [FallingLine] The maximum red color level (true color)
        /// </summary>
        public static int FallingLineMaximumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FallingLineMaximumRedColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.FallingLineMinimumRedColorLevel)
                    value = ScreensaverPackInit.SaversConfig.FallingLineMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.FallingLineMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FallingLine] The maximum green color level (true color)
        /// </summary>
        public static int FallingLineMaximumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FallingLineMaximumGreenColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.FallingLineMinimumGreenColorLevel)
                    value = ScreensaverPackInit.SaversConfig.FallingLineMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.FallingLineMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FallingLine] The maximum blue color level (true color)
        /// </summary>
        public static int FallingLineMaximumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FallingLineMaximumBlueColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.FallingLineMinimumBlueColorLevel)
                    value = ScreensaverPackInit.SaversConfig.FallingLineMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.FallingLineMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [FallingLine] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int FallingLineMaximumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FallingLineMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= ScreensaverPackInit.SaversConfig.FallingLineMinimumColorLevel)
                    value = ScreensaverPackInit.SaversConfig.FallingLineMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                ScreensaverPackInit.SaversConfig.FallingLineMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for FallingLine
    /// </summary>
    public class FallingLineDisplay : BaseScreensaver, IScreensaver
    {

        private int ColumnLine;
        private readonly List<Tuple<int, int>> CoveredPositions = [];

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "FallingLine";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            CoveredPositions.Clear();
            ColorTools.LoadBack("0;0;0");
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

            // Select the color
            Color ColorStorage;
            if (FallingLineSettings.FallingLineTrueColor)
            {
                int RedColorNum = RandomDriver.Random(FallingLineSettings.FallingLineMinimumRedColorLevel, FallingLineSettings.FallingLineMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(FallingLineSettings.FallingLineMinimumGreenColorLevel, FallingLineSettings.FallingLineMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(FallingLineSettings.FallingLineMinimumBlueColorLevel, FallingLineSettings.FallingLineMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                ColorTools.SetConsoleColor(ColorStorage, true);
            }
            else
            {
                int ColorNum = RandomDriver.Random(FallingLineSettings.FallingLineMinimumColorLevel, FallingLineSettings.FallingLineMaximumColorLevel);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
                ColorStorage = new Color(ColorNum);
                ColorTools.SetConsoleColor(ColorStorage, true);
            }

            // Make the line fall down
            for (int Fall = FallStart; Fall <= FallEnd; Fall++)
            {
                // Check to see if user decided to resize
                if (ConsoleResizeListener.WasResized(false))
                    break;

                // Print a block and add the covered position to the list so fading down can be done
                TextWriterWhereColor.WriteWhere(" ", ColumnLine, Fall, false);
                var PositionTuple = new Tuple<int, int>(ColumnLine, Fall);
                CoveredPositions.Add(PositionTuple);

                // Delay
                ThreadManager.SleepNoBlock(FallingLineSettings.FallingLineDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            }

            // Fade the line down. Please note that this requires true-color support in the terminal to work properly.
            for (int StepNum = 0; StepNum <= FallingLineSettings.FallingLineMaxSteps; StepNum++)
            {
                // Check to see if user decided to resize
                if (ConsoleResizeListener.WasResized(false))
                    break;

                // Set thresholds
                double ThresholdRed = ColorStorage.R / (double)FallingLineSettings.FallingLineMaxSteps;
                double ThresholdGreen = ColorStorage.G / (double)FallingLineSettings.FallingLineMaxSteps;
                double ThresholdBlue = ColorStorage.B / (double)FallingLineSettings.FallingLineMaxSteps;
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Color threshold (R;G;B: {0})", ThresholdRed, ThresholdGreen, ThresholdBlue);

                // Set color fade steps
                int CurrentColorRedOut = (int)Math.Round(ColorStorage.R - ThresholdRed * StepNum);
                int CurrentColorGreenOut = (int)Math.Round(ColorStorage.G - ThresholdGreen * StepNum);
                int CurrentColorBlueOut = (int)Math.Round(ColorStorage.B - ThresholdBlue * StepNum);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut);

                // Get the positions and write the block with new color
                var CurrentFadeColor = new Color(CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut);
                var bleedBuilder = new StringBuilder();
                foreach (Tuple<int, int> PositionTuple in CoveredPositions)
                {
                    // Check to see if user decided to resize
                    if (ConsoleResizeListener.WasResized(false))
                        break;

                    // Actually fade the line out
                    int PositionLeft = PositionTuple.Item1;
                    int PositionTop = PositionTuple.Item2;
                    bleedBuilder.Append($"{CsiSequences.GenerateCsiCursorPosition(PositionLeft + 1, PositionTop + 1)} ");
                }
                TextWriterWhereColor.WriteWhereColorBack(bleedBuilder.ToString(), ColumnLine, 0, false, Color.Empty, CurrentFadeColor);

                // Delay
                ThreadManager.SleepNoBlock(FallingLineSettings.FallingLineDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            }

            // Reset covered positions
            CoveredPositions.Clear();

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(FallingLineSettings.FallingLineDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

        /// <inheritdoc/>
        public override void ScreensaverOutro() =>
            CoveredPositions.Clear();

    }
}
