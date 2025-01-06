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
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Misc.Screensaver;
using Nitrocid.Kernel.Configuration;
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Colors.Data;

namespace Nitrocid.ScreensaverPacks.Animations.BeatPulse
{
    /// <summary>
    /// Beat pulse animation module
    /// </summary>
    public static class BeatPulse
    {

        /// <summary>
        /// Simulates the beat pulsing animation
        /// </summary>
        public static void Simulate(BeatPulseSettings? Settings)
        {
            Settings ??= new();
            ConsoleWrapper.CursorVisible = false;
            int BeatInterval = (int)Math.Round(60000d / Settings.BeatPulseDelay);
            int BeatIntervalStep = (int)Math.Round(BeatInterval / (double)Settings.BeatPulseMaxSteps);
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Beat interval from {0} BPM: {1}", vars: [Settings.BeatPulseDelay, BeatInterval]);
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Beat steps: {0} ms", vars: [Settings.BeatPulseDelay, BeatIntervalStep]);
            ScreensaverManager.Delay(BeatIntervalStep);

            // If we're cycling colors, set them. Else, use the user-provided color
            int RedColorNum, GreenColorNum, BlueColorNum;
            if (Settings.BeatPulseCycleColors)
            {
                // We're cycling. Select the color mode, starting from true color
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Cycling colors...");
                if (Settings.BeatPulseTrueColor)
                {
                    RedColorNum = RandomDriver.Random(Settings.BeatPulseMinimumRedColorLevel, Settings.BeatPulseMaximumRedColorLevel);
                    GreenColorNum = RandomDriver.Random(Settings.BeatPulseMinimumGreenColorLevel, Settings.BeatPulseMaximumGreenColorLevel);
                    BlueColorNum = RandomDriver.Random(Settings.BeatPulseMinimumBlueColorLevel, Settings.BeatPulseMaximumBlueColorLevel);
                }
                else
                {
                    var ConsoleColor = new Color((ConsoleColors)RandomDriver.Random(Settings.BeatPulseMinimumColorLevel, Settings.BeatPulseMaximumColorLevel));
                    RedColorNum = ConsoleColor.RGB.R;
                    GreenColorNum = ConsoleColor.RGB.G;
                    BlueColorNum = ConsoleColor.RGB.B;
                }
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", vars: [RedColorNum, GreenColorNum, BlueColorNum]);
            }
            else
            {
                // We're not cycling. Parse the color and then select the color mode, starting from true color
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Parsing colors... {0}", vars: [Settings.BeatPulseBeatColor]);
                var UserColor = new Color(Settings.BeatPulseBeatColor);
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
            double ThresholdRed = RedColorNum / (double)Settings.BeatPulseMaxSteps;
            double ThresholdGreen = GreenColorNum / (double)Settings.BeatPulseMaxSteps;
            double ThresholdBlue = BlueColorNum / (double)Settings.BeatPulseMaxSteps;
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Color threshold (R;G;B: {0};{1};{2})", vars: [ThresholdRed, ThresholdGreen, ThresholdBlue]);

            // Fade in
            int CurrentColorRedIn = 0;
            int CurrentColorGreenIn = 0;
            int CurrentColorBlueIn = 0;
            for (int CurrentStep = Settings.BeatPulseMaxSteps; CurrentStep >= 1; CurrentStep -= 1)
            {
                if (ConsoleResizeHandler.WasResized(false))
                    break;
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Step {0}/{1}", vars: [CurrentStep, BeatIntervalStep]);
                ScreensaverManager.Delay(BeatIntervalStep);
                CurrentColorRedIn = (int)Math.Round(CurrentColorRedIn + ThresholdRed);
                CurrentColorGreenIn = (int)Math.Round(CurrentColorGreenIn + ThresholdGreen);
                CurrentColorBlueIn = (int)Math.Round(CurrentColorBlueIn + ThresholdBlue);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Color in (R;G;B: {0};{1};{2})", vars: [CurrentColorRedIn, CurrentColorGreenIn, CurrentColorBlueIn]);
                if (!ConsoleResizeHandler.WasResized(false))
                    ColorTools.LoadBackDry(new Color(CurrentColorRedIn, CurrentColorGreenIn, CurrentColorBlueIn));
            }

            // Fade out
            for (int CurrentStep = 1; CurrentStep <= Settings.BeatPulseMaxSteps; CurrentStep++)
            {
                if (ConsoleResizeHandler.WasResized(false))
                    break;
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Step {0}/{1} each {2} ms", vars: [CurrentStep, Settings.BeatPulseMaxSteps, BeatIntervalStep]);
                ScreensaverManager.Delay(BeatIntervalStep);
                int CurrentColorRedOut = (int)Math.Round(RedColorNum - ThresholdRed * CurrentStep);
                int CurrentColorGreenOut = (int)Math.Round(GreenColorNum - ThresholdGreen * CurrentStep);
                int CurrentColorBlueOut = (int)Math.Round(BlueColorNum - ThresholdBlue * CurrentStep);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", vars: [RedColorNum, GreenColorNum, BlueColorNum]);
                if (!ConsoleResizeHandler.WasResized(false))
                    ColorTools.LoadBackDry(new Color(CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut));
            }

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ScreensaverManager.Delay(Settings.BeatPulseDelay);
        }

    }
}
