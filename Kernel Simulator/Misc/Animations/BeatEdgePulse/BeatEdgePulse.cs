
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using ColorSeq;
using KS.ConsoleBase.Colors;
using KS.Misc.Screensaver;
using KS.Misc.Threading;
using KS.Misc.Writers.DebugWriters;

namespace KS.Misc.Animations.BeatEdgePulse
{
    public static class BeatEdgePulse
    {

        private static int CurrentWindowWidth;
        private static int CurrentWindowHeight;
        private static bool ResizeSyncing;

        /// <summary>
        /// Simulates the beat pulsing animation
        /// </summary>
        public static void Simulate(BeatEdgePulseSettings Settings)
        {
            CurrentWindowWidth = Console.WindowWidth;
            CurrentWindowHeight = Console.WindowHeight;
            var RandomDriver = Settings.RandomDriver;
            Console.CursorVisible = false;
            int BeatInterval = (int)Math.Round(60000d / Settings.BeatEdgePulseDelay);
            int BeatIntervalStep = (int)Math.Round(BeatInterval / (double)Settings.BeatEdgePulseMaxSteps);
            DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Beat interval from {0} BPM: {1}", Settings.BeatEdgePulseDelay, BeatInterval);
            DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Beat steps: {0} ms", Settings.BeatEdgePulseDelay, BeatIntervalStep);
            ThreadManager.SleepNoBlock(BeatIntervalStep, ScreensaverDisplayer.ScreensaverDisplayerThread);

            // If we're cycling colors, set them. Else, use the user-provided color
            int RedColorNum = default, GreenColorNum = default, BlueColorNum = default;
            if (Settings.BeatEdgePulseCycleColors)
            {
                // We're cycling. Select the color mode, starting from true color
                DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Cycling colors...");
                if (Settings.BeatEdgePulseTrueColor)
                {
                    RedColorNum = RandomDriver.Next(Settings.BeatEdgePulseMinimumRedColorLevel, Settings.BeatEdgePulseMinimumRedColorLevel);
                    GreenColorNum = RandomDriver.Next(Settings.BeatEdgePulseMinimumGreenColorLevel, Settings.BeatEdgePulseMaximumGreenColorLevel);
                    BlueColorNum = RandomDriver.Next(Settings.BeatEdgePulseMinimumBlueColorLevel, Settings.BeatEdgePulseMaximumBlueColorLevel);
                }
                else if (Settings.BeatEdgePulse255Colors)
                {
                    var ConsoleColor = new ConsoleColorsInfo((ConsoleColors)RandomDriver.Next(Settings.BeatEdgePulseMinimumColorLevel, Settings.BeatEdgePulseMaximumColorLevel));
                    RedColorNum = ConsoleColor.R;
                    GreenColorNum = ConsoleColor.G;
                    BlueColorNum = ConsoleColor.B;
                }
                else
                {
                    var ConsoleColor = new ConsoleColorsInfo((ConsoleColors)RandomDriver.Next(Settings.BeatEdgePulseMinimumColorLevel, Settings.BeatEdgePulseMaximumColorLevel));
                    RedColorNum = ConsoleColor.R;
                    GreenColorNum = ConsoleColor.G;
                    BlueColorNum = ConsoleColor.B;
                }
                DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                // We're not cycling. Parse the color and then select the color mode, starting from true color
                DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Parsing colors... {0}", Settings.BeatEdgePulseBeatColor);
                var UserColor = new Color(Settings.BeatEdgePulseBeatColor);
                if (UserColor.Type == ColorType.TrueColor)
                {
                    RedColorNum = UserColor.R;
                    GreenColorNum = UserColor.G;
                    BlueColorNum = UserColor.B;
                }
                else if (UserColor.Type == ColorType._255Color)
                {
                    var ConsoleColor = new ConsoleColorsInfo((ConsoleColors)Convert.ToInt32(UserColor.PlainSequence));
                    RedColorNum = ConsoleColor.R;
                    GreenColorNum = ConsoleColor.G;
                    BlueColorNum = ConsoleColor.B;
                }
                DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
            }

            // Set thresholds
            double ThresholdRed = RedColorNum / (double)Settings.BeatEdgePulseMaxSteps;
            double ThresholdGreen = GreenColorNum / (double)Settings.BeatEdgePulseMaxSteps;
            double ThresholdBlue = BlueColorNum / (double)Settings.BeatEdgePulseMaxSteps;
            DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Color threshold (R;G;B: {0};{1};{2})", ThresholdRed, ThresholdGreen, ThresholdBlue);

            // Fade in
            int CurrentColorRedIn = 0;
            int CurrentColorGreenIn = 0;
            int CurrentColorBlueIn = 0;
            for (int CurrentStep = Settings.BeatEdgePulseMaxSteps; CurrentStep >= 1; CurrentStep -= 1)
            {
                if (CurrentWindowHeight != Console.WindowHeight | CurrentWindowWidth != Console.WindowWidth)
                    ResizeSyncing = true;
                if (ResizeSyncing)
                    break;
                DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Step {0}/{1}", CurrentStep, BeatIntervalStep);
                ThreadManager.SleepNoBlock(BeatIntervalStep, System.Threading.Thread.CurrentThread);
                CurrentColorRedIn = (int)Math.Round(CurrentColorRedIn + ThresholdRed);
                CurrentColorGreenIn = (int)Math.Round(CurrentColorGreenIn + ThresholdGreen);
                CurrentColorBlueIn = (int)Math.Round(CurrentColorBlueIn + ThresholdBlue);
                DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Color in (R;G;B: {0};{1};{2})", CurrentColorRedIn, CurrentColorGreenIn, CurrentColorBlueIn);
                if (CurrentWindowHeight != Console.WindowHeight | CurrentWindowWidth != Console.WindowWidth)
                    ResizeSyncing = true;
                if (!ResizeSyncing)
                {
                    ColorTools.SetConsoleColor(new Color(CurrentColorRedIn, CurrentColorGreenIn, CurrentColorBlueIn), true, true);
                    FillIn();
                }
            }

            // Fade out
            for (int CurrentStep = 1, loopTo = Settings.BeatEdgePulseMaxSteps; CurrentStep <= loopTo; CurrentStep++)
            {
                if (CurrentWindowHeight != Console.WindowHeight | CurrentWindowWidth != Console.WindowWidth)
                    ResizeSyncing = true;
                if (ResizeSyncing)
                    break;
                DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Step {0}/{1} each {2} ms", CurrentStep, Settings.BeatEdgePulseMaxSteps, BeatIntervalStep);
                ThreadManager.SleepNoBlock(BeatIntervalStep, System.Threading.Thread.CurrentThread);
                int CurrentColorRedOut = (int)Math.Round(RedColorNum - ThresholdRed * CurrentStep);
                int CurrentColorGreenOut = (int)Math.Round(GreenColorNum - ThresholdGreen * CurrentStep);
                int CurrentColorBlueOut = (int)Math.Round(BlueColorNum - ThresholdBlue * CurrentStep);
                DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                if (CurrentWindowHeight != Console.WindowHeight | CurrentWindowWidth != Console.WindowWidth)
                    ResizeSyncing = true;
                if (!ResizeSyncing)
                {
                    ColorTools.SetConsoleColor(new Color($"{CurrentColorRedOut};{CurrentColorGreenOut};{CurrentColorBlueOut}"), true, true);
                    FillIn();
                }
            }

            // Reset resize sync
            ResizeSyncing = false;
            CurrentWindowWidth = Console.WindowWidth;
            CurrentWindowHeight = Console.WindowHeight;
            ThreadManager.SleepNoBlock(Settings.BeatEdgePulseDelay, System.Threading.Thread.CurrentThread);
        }

        private static void FillIn()
        {
            int FloorTopLeftEdge = 0;
            int FloorBottomLeftEdge = 0;
            DebugWriter.Wdbg(DebugLevel.I, "Top left edge: {0}, Bottom left edge: {1}", FloorTopLeftEdge, FloorBottomLeftEdge);

            int FloorTopRightEdge = Console.WindowWidth - 1;
            int FloorBottomRightEdge = Console.WindowWidth - 1;
            DebugWriter.Wdbg(DebugLevel.I, "Top right edge: {0}, Bottom right edge: {1}", FloorTopRightEdge, FloorBottomRightEdge);

            int FloorTopEdge = 0;
            int FloorBottomEdge = Console.WindowHeight - 1;
            DebugWriter.Wdbg(DebugLevel.I, "Top edge: {0}, Bottom edge: {1}", FloorTopEdge, FloorBottomEdge);

            int FloorLeftEdge = 0;
            int FloorRightEdge = Console.WindowWidth - 2;
            DebugWriter.Wdbg(DebugLevel.I, "Left edge: {0}, Right edge: {1}", FloorLeftEdge, FloorRightEdge);

            // First, draw the floor top edge
            for (int x = FloorTopLeftEdge, loopTo = FloorTopRightEdge; x <= loopTo; x++)
            {
                Console.SetCursorPosition(x, 0);
                DebugWriter.Wdbg(DebugLevel.I, "Drawing floor top edge ({0}, {1})", x, 1);
                Console.Write(" ");
            }

            // Second, draw the floor bottom edge
            for (int x = FloorBottomLeftEdge, loopTo1 = FloorBottomRightEdge; x <= loopTo1; x++)
            {
                Console.SetCursorPosition(x, FloorBottomEdge);
                DebugWriter.Wdbg(DebugLevel.I, "Drawing floor bottom edge ({0}, {1})", x, FloorBottomEdge);
                Console.Write(" ");
            }

            // Third, draw the floor left edge
            for (int y = FloorTopEdge, loopTo2 = FloorBottomEdge; y <= loopTo2; y++)
            {
                Console.SetCursorPosition(FloorLeftEdge, y);
                DebugWriter.Wdbg(DebugLevel.I, "Drawing floor left edge ({0}, {1})", FloorLeftEdge, y);
                Console.Write("  ");
            }

            // Finally, draw the floor right edge
            for (int y = FloorTopEdge, loopTo3 = FloorBottomEdge; y <= loopTo3; y++)
            {
                Console.SetCursorPosition(FloorRightEdge, y);
                DebugWriter.Wdbg(DebugLevel.I, "Drawing floor right edge ({0}, {1})", FloorRightEdge, y);
                Console.Write("  ");
            }
        }

    }
}