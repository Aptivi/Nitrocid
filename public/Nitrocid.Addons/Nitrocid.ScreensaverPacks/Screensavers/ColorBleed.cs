
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
using KS.Kernel.Debugging;
using KS.Kernel.Threading;
using KS.Misc.Screensaver;
using Terminaux.Colors;
using Terminaux.Sequences.Builder;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Settings for ColorBleed
    /// </summary>
    public static class ColorBleedSettings
    {

        /// <summary>
        /// [ColorBleed] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool ColorBleedTrueColor
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ColorBleedTrueColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.ColorBleedTrueColor = value;
            }
        }
        /// <summary>
        /// [ColorBleed] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int ColorBleedDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ColorBleedDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                ScreensaverPackInit.SaversConfig.ColorBleedDelay = value;
            }
        }
        /// <summary>
        /// [ColorBleed] How many fade steps to do?
        /// </summary>
        public static int ColorBleedMaxSteps
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ColorBleedMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                ScreensaverPackInit.SaversConfig.ColorBleedMaxSteps = value;
            }
        }
        /// <summary>
        /// [ColorBleed] Chance to drop a new falling color
        /// </summary>
        public static int ColorBleedDropChance
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ColorBleedDropChance;
            }
            set
            {
                if (value <= 0)
                    value = 40;
                ScreensaverPackInit.SaversConfig.ColorBleedDropChance = value;
            }
        }
        /// <summary>
        /// [ColorBleed] The minimum red color level (true color)
        /// </summary>
        public static int ColorBleedMinimumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ColorBleedMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ColorBleedMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorBleed] The minimum green color level (true color)
        /// </summary>
        public static int ColorBleedMinimumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ColorBleedMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ColorBleedMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorBleed] The minimum blue color level (true color)
        /// </summary>
        public static int ColorBleedMinimumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ColorBleedMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ColorBleedMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorBleed] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int ColorBleedMinimumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ColorBleedMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                ScreensaverPackInit.SaversConfig.ColorBleedMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorBleed] The maximum red color level (true color)
        /// </summary>
        public static int ColorBleedMaximumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ColorBleedMaximumRedColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.ColorBleedMinimumRedColorLevel)
                    value = ScreensaverPackInit.SaversConfig.ColorBleedMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ColorBleedMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorBleed] The maximum green color level (true color)
        /// </summary>
        public static int ColorBleedMaximumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ColorBleedMaximumGreenColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.ColorBleedMinimumGreenColorLevel)
                    value = ScreensaverPackInit.SaversConfig.ColorBleedMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ColorBleedMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorBleed] The maximum blue color level (true color)
        /// </summary>
        public static int ColorBleedMaximumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ColorBleedMaximumBlueColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.ColorBleedMinimumBlueColorLevel)
                    value = ScreensaverPackInit.SaversConfig.ColorBleedMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ColorBleedMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorBleed] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int ColorBleedMaximumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ColorBleedMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= ScreensaverPackInit.SaversConfig.ColorBleedMinimumColorLevel)
                    value = ScreensaverPackInit.SaversConfig.ColorBleedMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                ScreensaverPackInit.SaversConfig.ColorBleedMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for ColorBleed
    /// </summary>
    public class ColorBleedDisplay : BaseScreensaver, IScreensaver
    {

        private static readonly List<BleedState> bleedStates = new();

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "ColorBleed";

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
            bool newFall = RandomDriver.RandomChance(ColorBleedSettings.ColorBleedDropChance);
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
                        if (bleedState.fadeStep > ColorBleedSettings.ColorBleedMaxSteps)
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

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(ColorBleedSettings.ColorBleedDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
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
        internal static readonly List<int> reservedColumns = new();
        private readonly Color ColorStorage;
        private readonly List<Tuple<int, int>> CoveredPositions = new();

        internal void Fall()
        {
            // Check to see if user decided to resize
            if (ConsoleResizeListener.WasResized(false))
                return;

            // Print a block and add the covered position to the list so fading down can be done
            TextWriterWhereColor.WriteWhere(" ", ColumnLine, fallStep, false, Color.Empty, ColorStorage);
            var PositionTuple = new Tuple<int, int>(ColumnLine, fallStep);
            CoveredPositions.Add(PositionTuple);
        }

        internal void Fade()
        {
            // Check to see if user decided to resize
            if (ConsoleResizeListener.WasResized(false))
                return;

            // Set thresholds
            double ThresholdRed = ColorStorage.R / (double)ColorBleedSettings.ColorBleedMaxSteps;
            double ThresholdGreen = ColorStorage.G / (double)ColorBleedSettings.ColorBleedMaxSteps;
            double ThresholdBlue = ColorStorage.B / (double)ColorBleedSettings.ColorBleedMaxSteps;
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Color threshold (R;G;B: {0})", ThresholdRed, ThresholdGreen, ThresholdBlue);

            // Set color fade steps
            int CurrentColorRedOut = (int)Math.Round(ColorStorage.R - ThresholdRed * fadeStep);
            int CurrentColorGreenOut = (int)Math.Round(ColorStorage.G - ThresholdGreen * fadeStep);
            int CurrentColorBlueOut = (int)Math.Round(ColorStorage.B - ThresholdBlue * fadeStep);
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
                bleedBuilder.Append($"{VtSequenceBuilderTools.BuildVtSequence(VtSequenceSpecificTypes.CsiCursorPosition, PositionLeft + 1, PositionTop + 1)} ");
            }
            TextWriterWhereColor.WriteWhere(bleedBuilder.ToString(), ColumnLine, 0, false, Color.Empty, CurrentFadeColor);
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
            if (ColorBleedSettings.ColorBleedTrueColor)
            {
                int RedColorNum = RandomDriver.Random(ColorBleedSettings.ColorBleedMinimumRedColorLevel, ColorBleedSettings.ColorBleedMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(ColorBleedSettings.ColorBleedMinimumGreenColorLevel, ColorBleedSettings.ColorBleedMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(ColorBleedSettings.ColorBleedMinimumBlueColorLevel, ColorBleedSettings.ColorBleedMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(ColorBleedSettings.ColorBleedMinimumColorLevel, ColorBleedSettings.ColorBleedMaximumColorLevel);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
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
