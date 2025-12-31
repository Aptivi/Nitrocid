//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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
using Nitrocid.Base.Kernel.Configuration;
using Terminaux.Base;
using Terminaux.Colors;
using Nitrocid.Base.Misc.Screensaver;

namespace Nitrocid.ScreensaverPacks.Animations.Pulse
{
    /// <summary>
    /// Pulse animation module
    /// </summary>
    public static class Pulse
    {

        /// <summary>
        /// Simulates the pulsing animation
        /// </summary>
        public static void Simulate(PulseSettings? Settings)
        {
            Settings ??= new();
            int RedColorNum = RandomDriver.Random(Settings.PulseMinimumRedColorLevel, Settings.PulseMaximumRedColorLevel);
            int GreenColorNum = RandomDriver.Random(Settings.PulseMinimumGreenColorLevel, Settings.PulseMaximumGreenColorLevel);
            int BlueColorNum = RandomDriver.Random(Settings.PulseMinimumBlueColorLevel, Settings.PulseMaximumBlueColorLevel);
            ConsoleWrapper.CursorVisible = false;

            // Set thresholds
            double ThresholdRed = RedColorNum / (double)Settings.PulseMaxSteps;
            double ThresholdGreen = GreenColorNum / (double)Settings.PulseMaxSteps;
            double ThresholdBlue = BlueColorNum / (double)Settings.PulseMaxSteps;
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Color threshold (R;G;B: {0})", vars: [ThresholdRed, ThresholdGreen, ThresholdBlue]);

            // Fade in
            int CurrentColorRedIn = 0;
            int CurrentColorGreenIn = 0;
            int CurrentColorBlueIn = 0;
            for (int CurrentStep = Settings.PulseMaxSteps; CurrentStep >= 1; CurrentStep -= 1)
            {
                if (ConsoleResizeHandler.WasResized(false))
                    break;
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Step {0}/{1}", vars: [CurrentStep, Settings.PulseMaxSteps]);
                ScreensaverManager.Delay(Settings.PulseDelay);
                CurrentColorRedIn = (int)Math.Round(CurrentColorRedIn + ThresholdRed);
                CurrentColorGreenIn = (int)Math.Round(CurrentColorGreenIn + ThresholdGreen);
                CurrentColorBlueIn = (int)Math.Round(CurrentColorBlueIn + ThresholdBlue);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Color in (R;G;B: {0};{1};{2})", vars: [CurrentColorRedIn, CurrentColorGreenIn, CurrentColorBlueIn]);
                if (!ConsoleResizeHandler.WasResized(false))
                    ColorTools.LoadBackDry(new Color(CurrentColorRedIn, CurrentColorGreenIn, CurrentColorBlueIn));
            }

            // Fade out
            for (int CurrentStep = 1; CurrentStep <= Settings.PulseMaxSteps; CurrentStep++)
            {
                if (ConsoleResizeHandler.WasResized(false))
                    break;
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Step {0}/{1}", vars: [CurrentStep, Settings.PulseMaxSteps]);
                ScreensaverManager.Delay(Settings.PulseDelay);
                int CurrentColorRedOut = (int)Math.Round(RedColorNum - ThresholdRed * CurrentStep);
                int CurrentColorGreenOut = (int)Math.Round(GreenColorNum - ThresholdGreen * CurrentStep);
                int CurrentColorBlueOut = (int)Math.Round(BlueColorNum - ThresholdBlue * CurrentStep);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", vars: [CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut]);
                if (!ConsoleResizeHandler.WasResized(false))
                    ColorTools.LoadBackDry(new Color(CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut));
            }

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ScreensaverManager.Delay(Settings.PulseDelay);
        }

    }
}
