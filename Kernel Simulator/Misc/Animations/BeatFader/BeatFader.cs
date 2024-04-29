﻿//
// Kernel Simulator  Copyright (C) 2018-2024  Aptivi
//
// This file is part of Kernel Simulator
//
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using KS.ConsoleBase.Colors;

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

using KS.Misc.Screensaver;
using KS.Misc.Threading;
using KS.Misc.Writers.DebugWriters;
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Colors.Data;

namespace KS.Misc.Animations.BeatFader
{
    public static class BeatFader
    {

        private static int CurrentWindowWidth;
        private static int CurrentWindowHeight;
        private static bool ResizeSyncing;

        /// <summary>
        /// Simulates the beat fading animation
        /// </summary>
        public static void Simulate(BeatFaderSettings Settings)
        {
            CurrentWindowWidth = ConsoleWrapper.WindowWidth;
            CurrentWindowHeight = ConsoleWrapper.WindowHeight;
            var RandomDriver = Settings.RandomDriver;
            ConsoleWrapper.CursorVisible = false;
            int BeatInterval = (int)Math.Round(60000d / Settings.BeatFaderDelay);
            int BeatIntervalStep = (int)Math.Round(BeatInterval / (double)Settings.BeatFaderMaxSteps);
            DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Beat interval from {0} BPM: {1}", Settings.BeatFaderDelay, BeatInterval);
            DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Beat steps: {0} ms", Settings.BeatFaderDelay, BeatIntervalStep);
            ThreadManager.SleepNoBlock(BeatIntervalStep, ScreensaverDisplayer.ScreensaverDisplayerThread);

            // If we're cycling colors, set them. Else, use the user-provided color
            int RedColorNum = default, GreenColorNum = default, BlueColorNum = default;
            if (Settings.BeatFaderCycleColors)
            {
                // We're cycling. Select the color mode, starting from true color
                DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Cycling colors...");
                if (Settings.BeatFaderTrueColor)
                {
                    RedColorNum = RandomDriver.Next(Settings.BeatFaderMinimumRedColorLevel, Settings.BeatFaderMinimumRedColorLevel);
                    GreenColorNum = RandomDriver.Next(Settings.BeatFaderMinimumGreenColorLevel, Settings.BeatFaderMaximumGreenColorLevel);
                    BlueColorNum = RandomDriver.Next(Settings.BeatFaderMinimumBlueColorLevel, Settings.BeatFaderMaximumBlueColorLevel);
                }
                else if (Settings.BeatFader255Colors)
                {
                    var ConsoleColor = new Color((ConsoleColors)RandomDriver.Next(Settings.BeatFaderMinimumColorLevel, Settings.BeatFaderMaximumColorLevel));
                    RedColorNum = ConsoleColor.RGB.R;
                    GreenColorNum = ConsoleColor.RGB.G;
                    BlueColorNum = ConsoleColor.RGB.B;
                }
                else
                {
                    var ConsoleColor = new Color((ConsoleColors)RandomDriver.Next(Settings.BeatFaderMinimumColorLevel, Settings.BeatFaderMaximumColorLevel));
                    RedColorNum = ConsoleColor.RGB.R;
                    GreenColorNum = ConsoleColor.RGB.G;
                    BlueColorNum = ConsoleColor.RGB.B;
                }
                DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                // We're not cycling. Parse the color and then select the color mode, starting from true color
                DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Parsing colors... {0}", Settings.BeatFaderBeatColor);
                var UserColor = new Color(Settings.BeatFaderBeatColor);
                if (UserColor.Type == ColorType.TrueColor)
                {
                    RedColorNum = UserColor.RGB.R;
                    GreenColorNum = UserColor.RGB.G;
                    BlueColorNum = UserColor.RGB.B;
                }
                else if (UserColor.Type == ColorType.EightBitColor)
                {
                    var ConsoleColor = new Color((ConsoleColors)Convert.ToInt32(UserColor.PlainSequence));
                    RedColorNum = ConsoleColor.RGB.R;
                    GreenColorNum = ConsoleColor.RGB.G;
                    BlueColorNum = ConsoleColor.RGB.B;
                }
                DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
            }

            // Set thresholds
            double ThresholdRed = RedColorNum / (double)Settings.BeatFaderMaxSteps;
            double ThresholdGreen = GreenColorNum / (double)Settings.BeatFaderMaxSteps;
            double ThresholdBlue = BlueColorNum / (double)Settings.BeatFaderMaxSteps;
            DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Color threshold (R;G;B: {0};{1};{2})", ThresholdRed, ThresholdGreen, ThresholdBlue);

            // Fade out
            for (int CurrentStep = 1, loopTo = Settings.BeatFaderMaxSteps; CurrentStep <= loopTo; CurrentStep++)
            {
                if (CurrentWindowHeight != ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleWrapper.WindowWidth)
                    ResizeSyncing = true;
                if (ResizeSyncing)
                    break;
                DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Step {0}/{1} each {2} ms", CurrentStep, Settings.BeatFaderMaxSteps, BeatIntervalStep);
                ThreadManager.SleepNoBlock(BeatIntervalStep, System.Threading.Thread.CurrentThread);
                int CurrentColorRedOut = (int)Math.Round(RedColorNum - ThresholdRed * CurrentStep);
                int CurrentColorGreenOut = (int)Math.Round(GreenColorNum - ThresholdGreen * CurrentStep);
                int CurrentColorBlueOut = (int)Math.Round(BlueColorNum - ThresholdBlue * CurrentStep);
                DebugWriter.WdbgConditional(ref Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                if (CurrentWindowHeight != ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleWrapper.WindowWidth)
                    ResizeSyncing = true;
                if (!ResizeSyncing)
                {
                    KernelColorTools.SetConsoleColor(new Color($"{CurrentColorRedOut};{CurrentColorGreenOut};{CurrentColorBlueOut}"), true);
                    ConsoleWrapper.Clear();
                }
            }

            // Reset resize sync
            ResizeSyncing = false;
            CurrentWindowWidth = ConsoleWrapper.WindowWidth;
            CurrentWindowHeight = ConsoleWrapper.WindowHeight;
            ThreadManager.SleepNoBlock(Settings.BeatFaderDelay, System.Threading.Thread.CurrentThread);
        }

    }
}