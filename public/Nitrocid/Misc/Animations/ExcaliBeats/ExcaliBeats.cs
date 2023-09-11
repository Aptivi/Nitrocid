
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
using Figletize;
using KS.ConsoleBase;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.ConsoleBase.Writers.FancyWriters.Tools;
using KS.Drivers.RNG;
using KS.Kernel.Debugging;
using KS.Kernel.Threading;
using KS.Misc.Screensaver;
using Terminaux.Colors;

namespace KS.Misc.Animations.ExcaliBeats
{
    /// <summary>
    /// ExcaliBeats animation module
    /// </summary>
    public static class ExcaliBeats
    {

        /// <summary>
        /// Simulates the ExcaliBeats animation
        /// </summary>
        public static void Simulate(ExcaliBeatsSettings Settings)
        {
            ConsoleWrapper.CursorVisible = false;

            // Trance mode isn't supported on Windows since the minimum limit for sleeping is 16ms.
            int BeatInterval = Settings.ExcaliBeatsTranceMode ? (int)Math.Round(60000d / (Settings.ExcaliBeatsDelay * 2)) : (int)Math.Round(60000d / Settings.ExcaliBeatsDelay);
            int maxSteps = Settings.ExcaliBeatsMaxSteps;
            int BeatIntervalStep = (int)Math.Round(BeatInterval / (double)maxSteps);
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Beat interval from {0} BPM: {1}", Settings.ExcaliBeatsDelay, BeatInterval);
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Beat steps: {0} ms", Settings.ExcaliBeatsDelay, BeatIntervalStep);
            ThreadManager.SleepNoBlock(BeatIntervalStep, ScreensaverDisplayer.ScreensaverDisplayerThread);

            // If we're cycling colors, set them. Else, use the user-provided color
            int RedColorNum, GreenColorNum, BlueColorNum;
            if (Settings.ExcaliBeatsCycleColors)
            {
                // We're cycling. Select the color mode, starting from true color
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Cycling colors...");
                if (Settings.ExcaliBeatsTrueColor)
                {
                    RedColorNum = RandomDriver.Random(Settings.ExcaliBeatsMinimumRedColorLevel, Settings.ExcaliBeatsMaximumRedColorLevel);
                    GreenColorNum = RandomDriver.Random(Settings.ExcaliBeatsMinimumGreenColorLevel, Settings.ExcaliBeatsMaximumGreenColorLevel);
                    BlueColorNum = RandomDriver.Random(Settings.ExcaliBeatsMinimumBlueColorLevel, Settings.ExcaliBeatsMaximumBlueColorLevel);
                }
                else
                {
                    var ConsoleColor = new ConsoleColorsInfo((ConsoleColors)RandomDriver.Random(Settings.ExcaliBeatsMinimumColorLevel, Settings.ExcaliBeatsMaximumColorLevel));
                    RedColorNum = ConsoleColor.R;
                    GreenColorNum = ConsoleColor.G;
                    BlueColorNum = ConsoleColor.B;
                }
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                // We're not cycling. Parse the color and then select the color mode, starting from true color
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Parsing colors... {0}", Settings.ExcaliBeatsBeatColor);
                var UserColor = new Color(Settings.ExcaliBeatsBeatColor);
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
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
            }

            // Set thresholds
            double ThresholdRed = RedColorNum / (double)maxSteps;
            double ThresholdGreen = GreenColorNum / (double)maxSteps;
            double ThresholdBlue = BlueColorNum / (double)maxSteps;
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Color threshold (R;G;B: {0};{1};{2})", ThresholdRed, ThresholdGreen, ThresholdBlue);

            // Populate the text
            string exStr = Settings.ExcaliBeatsExplicit ? "EXCALIBUR" : "EXCALIBEATS";
            var figFont = FigletTools.GetFigletFont("banner3");
            int figWidth = FigletTools.GetFigletWidth(exStr, figFont) / 2;
            int figHeight = FigletTools.GetFigletHeight(exStr, figFont) / 2;
            int consoleX = (ConsoleWrapper.WindowWidth / 2) - figWidth;
            int consoleY = (ConsoleWrapper.WindowHeight / 2) - figHeight;

            // Fade out
            for (int CurrentStep = 1; CurrentStep <= maxSteps; CurrentStep++)
            {
                if (ConsoleResizeListener.WasResized(false))
                    break;
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Step {0}/{1} each {2} ms", CurrentStep, maxSteps, BeatIntervalStep);
                ThreadManager.SleepNoBlock(BeatIntervalStep, System.Threading.Thread.CurrentThread);
                int CurrentColorRedOut = (int)Math.Round(RedColorNum - ThresholdRed * CurrentStep);
                int CurrentColorGreenOut = (int)Math.Round(GreenColorNum - ThresholdGreen * CurrentStep);
                int CurrentColorBlueOut = (int)Math.Round(BlueColorNum - ThresholdBlue * CurrentStep);
                var CurrentColorOut = new Color($"{CurrentColorRedOut};{CurrentColorGreenOut};{CurrentColorBlueOut}");
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                if (!ConsoleResizeListener.WasResized(false))
                    FigletWhereColor.WriteFigletWhere(exStr, consoleX, consoleY, false, figFont, CurrentColorOut);
            }

            // Reset resize sync
            ConsoleResizeListener.WasResized();
        }

    }
}
