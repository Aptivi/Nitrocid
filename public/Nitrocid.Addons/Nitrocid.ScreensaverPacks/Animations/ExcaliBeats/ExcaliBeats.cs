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
using Textify.Data.Figlet;
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Misc.Screensaver;
using Terminaux.Colors;
using Terminaux.Base;
using Terminaux.Colors.Data;
using Nitrocid.Kernel.Configuration;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Graphical;

namespace Nitrocid.ScreensaverPacks.Animations.ExcaliBeats
{
    /// <summary>
    /// ExcaliBeats animation module
    /// </summary>
    public static class ExcaliBeats
    {

        /// <summary>
        /// Simulates the ExcaliBeats animation
        /// </summary>
        public static void Simulate(ExcaliBeatsSettings? Settings)
        {
            Settings ??= new();
            ConsoleWrapper.CursorVisible = false;

            // Trance mode isn't supported on Windows since the minimum limit for sleeping is 16ms.
            int BeatInterval = Settings.ExcaliBeatsTranceMode ? (int)Math.Round(60000d / (Settings.ExcaliBeatsDelay * 2)) : (int)Math.Round(60000d / Settings.ExcaliBeatsDelay);
            int maxSteps = Settings.ExcaliBeatsMaxSteps;
            int BeatIntervalStep = (int)Math.Round(BeatInterval / (double)maxSteps);
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Beat interval from {0} BPM: {1}", vars: [Settings.ExcaliBeatsDelay, BeatInterval]);
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Beat steps: {0} ms", vars: [Settings.ExcaliBeatsDelay, BeatIntervalStep]);
            ScreensaverManager.Delay(BeatIntervalStep);

            // If we're cycling colors, set them. Else, use the user-provided color
            int RedColorNum, GreenColorNum, BlueColorNum;
            if (Settings.ExcaliBeatsCycleColors)
            {
                // We're cycling. Select the color mode, starting from true color
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Cycling colors...");
                if (Settings.ExcaliBeatsTrueColor)
                {
                    RedColorNum = RandomDriver.Random(Settings.ExcaliBeatsMinimumRedColorLevel, Settings.ExcaliBeatsMaximumRedColorLevel);
                    GreenColorNum = RandomDriver.Random(Settings.ExcaliBeatsMinimumGreenColorLevel, Settings.ExcaliBeatsMaximumGreenColorLevel);
                    BlueColorNum = RandomDriver.Random(Settings.ExcaliBeatsMinimumBlueColorLevel, Settings.ExcaliBeatsMaximumBlueColorLevel);
                }
                else
                {
                    var ConsoleColor = new Color((ConsoleColors)RandomDriver.Random(Settings.ExcaliBeatsMinimumColorLevel, Settings.ExcaliBeatsMaximumColorLevel));
                    RedColorNum = ConsoleColor.RGB.R;
                    GreenColorNum = ConsoleColor.RGB.G;
                    BlueColorNum = ConsoleColor.RGB.B;
                }
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", vars: [RedColorNum, GreenColorNum, BlueColorNum]);
            }
            else
            {
                // We're not cycling. Parse the color and then select the color mode, starting from true color
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Parsing colors... {0}", vars: [Settings.ExcaliBeatsBeatColor]);
                var UserColor = new Color(Settings.ExcaliBeatsBeatColor);
                if (UserColor.Type == ColorType.TrueColor)
                {
                    RedColorNum = UserColor.RGB.R;
                    GreenColorNum = UserColor.RGB.G;
                    BlueColorNum = UserColor.RGB.B;
                }
                else
                {
                    var ConsoleColor = new Color((ConsoleColors)Convert.ToInt32(UserColor.PlainSequence));
                    RedColorNum = ConsoleColor.RGB.R;
                    GreenColorNum = ConsoleColor.RGB.G;
                    BlueColorNum = ConsoleColor.RGB.B;
                }
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", vars: [RedColorNum, GreenColorNum, BlueColorNum]);
            }

            // Set thresholds
            double ThresholdRed = RedColorNum / (double)maxSteps;
            double ThresholdGreen = GreenColorNum / (double)maxSteps;
            double ThresholdBlue = BlueColorNum / (double)maxSteps;
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Color threshold (R;G;B: {0};{1};{2})", vars: [ThresholdRed, ThresholdGreen, ThresholdBlue]);

            // Flash!
            ColorTools.LoadBackDry("255;255;255");
            ScreensaverManager.Delay(20);
            ColorTools.LoadBackDry(0);

            // Populate the text
            string exStr = Settings.ExcaliBeatsExplicit ? "EXCALIBUR" : "EXCALIBEATS";
            var figFont = FigletTools.GetFigletFont(Config.MainConfig.DefaultFigletFontName);

            // Fade out
            for (int CurrentStep = 1; CurrentStep <= maxSteps; CurrentStep++)
            {
                if (ConsoleResizeHandler.WasResized(false))
                    break;
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Step {0}/{1} each {2} ms", vars: [CurrentStep, maxSteps, BeatIntervalStep]);
                ScreensaverManager.Delay(BeatIntervalStep);
                int CurrentColorRedOut = (int)Math.Round(RedColorNum - ThresholdRed * CurrentStep);
                int CurrentColorGreenOut = (int)Math.Round(GreenColorNum - ThresholdGreen * CurrentStep);
                int CurrentColorBlueOut = (int)Math.Round(BlueColorNum - ThresholdBlue * CurrentStep);
                var CurrentColorOut = new Color($"{CurrentColorRedOut};{CurrentColorGreenOut};{CurrentColorBlueOut}");
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", vars: [RedColorNum, GreenColorNum, BlueColorNum]);
                if (!ConsoleResizeHandler.WasResized(false))
                {
                    var exText = new AlignedFigletText(figFont)
                    {
                        Text = exStr,
                        ForegroundColor = CurrentColorOut,
                        Settings = new()
                        {
                            Alignment = TextAlignment.Middle,
                        }
                    };
                    TextWriterRaw.WriteRaw(exText.Render());
                }
            }

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
        }

    }
}
