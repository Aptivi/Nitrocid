
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
using ColorSeq;
using KS.ConsoleBase;
using KS.Drivers.RNG;
using KS.Kernel.Debugging;
using KS.Misc.Screensaver;
using KS.Misc.Threading;
using ColorTools = KS.ConsoleBase.Colors.ColorTools;

namespace KS.Misc.Animations.BeatFader
{
    /// <summary>
    /// Beat fader animation module
    /// </summary>
    public static class BeatFader
    {

        /// <summary>
        /// Simulates the beat fading animation
        /// </summary>
        public static void Simulate(BeatFaderSettings Settings)
        {
            ConsoleWrapper.CursorVisible = false;
            int BeatInterval = (int)Math.Round(60000d / Settings.BeatFaderDelay);
            int BeatIntervalStep = (int)Math.Round(BeatInterval / (double)Settings.BeatFaderMaxSteps);
            DebugWriter.WriteDebugConditional(Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Beat interval from {0} BPM: {1}", Settings.BeatFaderDelay, BeatInterval);
            DebugWriter.WriteDebugConditional(Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Beat steps: {0} ms", Settings.BeatFaderDelay, BeatIntervalStep);
            ThreadManager.SleepNoBlock(BeatIntervalStep, ScreensaverDisplayer.ScreensaverDisplayerThread);

            // If we're cycling colors, set them. Else, use the user-provided color
            int RedColorNum;
            int GreenColorNum;
            int BlueColorNum;
            if (Settings.BeatFaderCycleColors)
            {
                // We're cycling. Select the color mode, starting from true color
                DebugWriter.WriteDebugConditional(Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Cycling colors...");
                if (Settings.BeatFaderTrueColor)
                {
                    RedColorNum = RandomDriver.Random(Settings.BeatFaderMinimumRedColorLevel, Settings.BeatFaderMaximumRedColorLevel);
                    GreenColorNum = RandomDriver.Random(Settings.BeatFaderMinimumGreenColorLevel, Settings.BeatFaderMaximumGreenColorLevel);
                    BlueColorNum = RandomDriver.Random(Settings.BeatFaderMinimumBlueColorLevel, Settings.BeatFaderMaximumBlueColorLevel);
                }
                else
                {
                    var ConsoleColor = new ConsoleColorsInfo((ConsoleColors)RandomDriver.Random(Settings.BeatFaderMinimumColorLevel, Settings.BeatFaderMaximumColorLevel));
                    RedColorNum = ConsoleColor.R;
                    GreenColorNum = ConsoleColor.G;
                    BlueColorNum = ConsoleColor.B;
                }
                DebugWriter.WriteDebugConditional(Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                // We're not cycling. Parse the color and then select the color mode, starting from true color
                DebugWriter.WriteDebugConditional(Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Parsing colors... {0}", Settings.BeatFaderBeatColor);
                var UserColor = new Color(Settings.BeatFaderBeatColor);
                if (UserColor.Type == ColorType.TrueColor)
                {
                    RedColorNum = UserColor.R;
                    GreenColorNum = UserColor.G;
                    BlueColorNum = UserColor.B;
                }
                else
                {
                    var ConsoleColor = new ConsoleColorsInfo((ConsoleColors)Convert.ToInt32(UserColor.PlainSequence));
                    RedColorNum = ConsoleColor.R;
                    GreenColorNum = ConsoleColor.G;
                    BlueColorNum = ConsoleColor.B;
                }
                DebugWriter.WriteDebugConditional(Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
            }

            // Set thresholds
            double ThresholdRed = RedColorNum / (double)Settings.BeatFaderMaxSteps;
            double ThresholdGreen = GreenColorNum / (double)Settings.BeatFaderMaxSteps;
            double ThresholdBlue = BlueColorNum / (double)Settings.BeatFaderMaxSteps;
            DebugWriter.WriteDebugConditional(Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Color threshold (R;G;B: {0};{1};{2})", ThresholdRed, ThresholdGreen, ThresholdBlue);

            // Fade out
            for (int CurrentStep = 1; CurrentStep <= Settings.BeatFaderMaxSteps; CurrentStep++)
            {
                if (ConsoleResizeListener.WasResized(false))
                    break;
                DebugWriter.WriteDebugConditional(Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Step {0}/{1} each {2} ms", CurrentStep, Settings.BeatFaderMaxSteps, BeatIntervalStep);
                ThreadManager.SleepNoBlock(BeatIntervalStep, System.Threading.Thread.CurrentThread);
                int CurrentColorRedOut = (int)Math.Round(RedColorNum - ThresholdRed * CurrentStep);
                int CurrentColorGreenOut = (int)Math.Round(GreenColorNum - ThresholdGreen * CurrentStep);
                int CurrentColorBlueOut = (int)Math.Round(BlueColorNum - ThresholdBlue * CurrentStep);
                DebugWriter.WriteDebugConditional(Screensaver.Screensaver.ScreensaverDebug, DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                if (!ConsoleResizeListener.WasResized(false))
                    ColorTools.LoadBack(new Color($"{CurrentColorRedOut};{CurrentColorGreenOut};{CurrentColorBlueOut}"), true);
            }

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(Settings.BeatFaderDelay, System.Threading.Thread.CurrentThread);
        }

    }
}
