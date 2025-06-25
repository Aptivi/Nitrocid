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
using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Misc.Screensaver;
using Nitrocid.Base.Kernel.Configuration;
using Terminaux.Base;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Animations.FaderBack
{
    /// <summary>
    /// Background fader animation module
    /// </summary>
    public static class FaderBack
    {

        /// <summary>
        /// Simulates the background fading animation
        /// </summary>
        public static void Simulate(FaderBackSettings? Settings)
        {
            Settings ??= new();
            int RedColorNum = RandomDriver.Random(Settings.FaderBackMinimumRedColorLevel, Settings.FaderBackMaximumRedColorLevel);
            int GreenColorNum = RandomDriver.Random(Settings.FaderBackMinimumGreenColorLevel, Settings.FaderBackMaximumGreenColorLevel);
            int BlueColorNum = RandomDriver.Random(Settings.FaderBackMinimumBlueColorLevel, Settings.FaderBackMaximumBlueColorLevel);
            ConsoleWrapper.CursorVisible = false;

            // Set thresholds
            double ThresholdRed = RedColorNum / (double)Settings.FaderBackMaxSteps;
            double ThresholdGreen = GreenColorNum / (double)Settings.FaderBackMaxSteps;
            double ThresholdBlue = BlueColorNum / (double)Settings.FaderBackMaxSteps;
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Color threshold (R;G;B: {0})", vars: [ThresholdRed, ThresholdGreen, ThresholdBlue]);

            // Fade in
            int CurrentColorRedIn = 0;
            int CurrentColorGreenIn = 0;
            int CurrentColorBlueIn = 0;
            for (int CurrentStep = Settings.FaderBackMaxSteps; CurrentStep >= 1; CurrentStep -= 1)
            {
                if (ConsoleResizeHandler.WasResized(false))
                    break;
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Step {0}/{1}", vars: [CurrentStep, Settings.FaderBackMaxSteps]);
                ScreensaverManager.Delay(Settings.FaderBackDelay);
                CurrentColorRedIn = (int)Math.Round(CurrentColorRedIn + ThresholdRed);
                CurrentColorGreenIn = (int)Math.Round(CurrentColorGreenIn + ThresholdGreen);
                CurrentColorBlueIn = (int)Math.Round(CurrentColorBlueIn + ThresholdBlue);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Color in (R;G;B: {0};{1};{2})", vars: [CurrentColorRedIn, CurrentColorGreenIn, CurrentColorBlueIn]);
                ColorTools.LoadBackDry(new Color($"{CurrentColorRedIn};{CurrentColorGreenIn};{CurrentColorBlueIn}"));
            }

            // Wait until fade out
            if (!ConsoleResizeHandler.WasResized(false))
            {
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Waiting {0} ms...", vars: [Settings.FaderBackFadeOutDelay]);
                ScreensaverManager.Delay(Settings.FaderBackFadeOutDelay);
            }

            // Fade out
            for (int CurrentStep = 1; CurrentStep <= Settings.FaderBackMaxSteps; CurrentStep++)
            {
                if (ConsoleResizeHandler.WasResized(false))
                    break;
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Step {0}/{1}", vars: [CurrentStep, Settings.FaderBackMaxSteps]);
                ScreensaverManager.Delay(Settings.FaderBackDelay);
                int CurrentColorRedOut = (int)Math.Round(RedColorNum - ThresholdRed * CurrentStep);
                int CurrentColorGreenOut = (int)Math.Round(GreenColorNum - ThresholdGreen * CurrentStep);
                int CurrentColorBlueOut = (int)Math.Round(BlueColorNum - ThresholdBlue * CurrentStep);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", vars: [CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut]);
                ColorTools.LoadBackDry(new Color($"{CurrentColorRedOut};{CurrentColorGreenOut};{CurrentColorBlueOut}"));
            }

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ScreensaverManager.Delay(Settings.FaderBackDelay);
        }

    }
}
