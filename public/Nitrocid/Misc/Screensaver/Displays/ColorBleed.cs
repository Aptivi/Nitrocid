
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
                return Config.SaverConfig.ColorBleedTrueColor;
            }
            set
            {
                Config.SaverConfig.ColorBleedTrueColor = value;
            }
        }
        /// <summary>
        /// [ColorBleed] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int ColorBleedDelay
        {
            get
            {
                return Config.SaverConfig.ColorBleedDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                Config.SaverConfig.ColorBleedDelay = value;
            }
        }
        /// <summary>
        /// [ColorBleed] How many fade steps to do?
        /// </summary>
        public static int ColorBleedMaxSteps
        {
            get
            {
                return Config.SaverConfig.ColorBleedMaxSteps;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                Config.SaverConfig.ColorBleedMaxSteps = value;
            }
        }
        /// <summary>
        /// [ColorBleed] Chance to drop a new falling color
        /// </summary>
        public static int ColorBleedDropChance
        {
            get
            {
                return Config.SaverConfig.ColorBleedDropChance;
            }
            set
            {
                if (value <= 0)
                    value = 40;
                Config.SaverConfig.ColorBleedDropChance = value;
            }
        }
        /// <summary>
        /// [ColorBleed] The minimum red color level (true color)
        /// </summary>
        public static int ColorBleedMinimumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.ColorBleedMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.ColorBleedMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorBleed] The minimum green color level (true color)
        /// </summary>
        public static int ColorBleedMinimumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.ColorBleedMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.ColorBleedMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorBleed] The minimum blue color level (true color)
        /// </summary>
        public static int ColorBleedMinimumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.ColorBleedMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.ColorBleedMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorBleed] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int ColorBleedMinimumColorLevel
        {
            get
            {
                return Config.SaverConfig.ColorBleedMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                Config.SaverConfig.ColorBleedMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorBleed] The maximum red color level (true color)
        /// </summary>
        public static int ColorBleedMaximumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.ColorBleedMaximumRedColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.ColorBleedMinimumRedColorLevel)
                    value = Config.SaverConfig.ColorBleedMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.ColorBleedMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorBleed] The maximum green color level (true color)
        /// </summary>
        public static int ColorBleedMaximumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.ColorBleedMaximumGreenColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.ColorBleedMinimumGreenColorLevel)
                    value = Config.SaverConfig.ColorBleedMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.ColorBleedMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorBleed] The maximum blue color level (true color)
        /// </summary>
        public static int ColorBleedMaximumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.ColorBleedMaximumBlueColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.ColorBleedMinimumBlueColorLevel)
                    value = Config.SaverConfig.ColorBleedMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.ColorBleedMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorBleed] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int ColorBleedMaximumColorLevel
        {
            get
            {
                return Config.SaverConfig.ColorBleedMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= Config.SaverConfig.ColorBleedMinimumColorLevel)
                    value = Config.SaverConfig.ColorBleedMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                Config.SaverConfig.ColorBleedMaximumColorLevel = value;
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
            KernelColorTools.LoadBack(0);
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
                    BleedState.reservedColumns.Remove(bleedState.ColumnLine);
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

            // Delay
            ThreadManager.SleepNoBlock(ColorBleedSettings.ColorBleedDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
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
            foreach (Tuple<int, int> PositionTuple in CoveredPositions)
            {
                // Check to see if user decided to resize
                if (ConsoleResizeListener.WasResized(false))
                    break;

                // Actually fade the line out
                int PositionLeft = PositionTuple.Item1;
                int PositionTop = PositionTuple.Item2;
                TextWriterWhereColor.WriteWhere(" ", PositionLeft, PositionTop, false, Color.Empty, CurrentFadeColor);
            }

            // Delay
            ThreadManager.SleepNoBlock(ColorBleedSettings.ColorBleedDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

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
