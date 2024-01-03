//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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
using Nitrocid.ConsoleBase;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Threading;
using Nitrocid.Misc.Screensaver;
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
        public static void Simulate(FaderBackSettings Settings)
        {
            int RedColorNum = RandomDriver.Random(Settings.FaderBackMinimumRedColorLevel, Settings.FaderBackMaximumRedColorLevel);
            int GreenColorNum = RandomDriver.Random(Settings.FaderBackMinimumGreenColorLevel, Settings.FaderBackMaximumGreenColorLevel);
            int BlueColorNum = RandomDriver.Random(Settings.FaderBackMinimumBlueColorLevel, Settings.FaderBackMaximumBlueColorLevel);
            ConsoleWrapper.CursorVisible = false;

            // Set thresholds
            double ThresholdRed = RedColorNum / (double)Settings.FaderBackMaxSteps;
            double ThresholdGreen = GreenColorNum / (double)Settings.FaderBackMaxSteps;
            double ThresholdBlue = BlueColorNum / (double)Settings.FaderBackMaxSteps;
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Color threshold (R;G;B: {0})", ThresholdRed, ThresholdGreen, ThresholdBlue);

            // Fade in
            int CurrentColorRedIn = 0;
            int CurrentColorGreenIn = 0;
            int CurrentColorBlueIn = 0;
            for (int CurrentStep = Settings.FaderBackMaxSteps; CurrentStep >= 1; CurrentStep -= 1)
            {
                if (ConsoleResizeListener.WasResized(false))
                    break;
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Step {0}/{1}", CurrentStep, Settings.FaderBackMaxSteps);
                ThreadManager.SleepNoBlock(Settings.FaderBackDelay, System.Threading.Thread.CurrentThread);
                CurrentColorRedIn = (int)Math.Round(CurrentColorRedIn + ThresholdRed);
                CurrentColorGreenIn = (int)Math.Round(CurrentColorGreenIn + ThresholdGreen);
                CurrentColorBlueIn = (int)Math.Round(CurrentColorBlueIn + ThresholdBlue);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Color in (R;G;B: {0};{1};{2})", CurrentColorRedIn, CurrentColorGreenIn, CurrentColorBlueIn);
                KernelColorTools.LoadBack(new Color($"{CurrentColorRedIn};{CurrentColorGreenIn};{CurrentColorBlueIn}"));
            }

            // Wait until fade out
            if (!ConsoleResizeListener.WasResized(false))
            {
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Waiting {0} ms...", Settings.FaderBackFadeOutDelay);
                ThreadManager.SleepNoBlock(Settings.FaderBackFadeOutDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            }

            // Fade out
            for (int CurrentStep = 1; CurrentStep <= Settings.FaderBackMaxSteps; CurrentStep++)
            {
                if (ConsoleResizeListener.WasResized(false))
                    break;
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Step {0}/{1}", CurrentStep, Settings.FaderBackMaxSteps);
                ThreadManager.SleepNoBlock(Settings.FaderBackDelay, System.Threading.Thread.CurrentThread);
                int CurrentColorRedOut = (int)Math.Round(RedColorNum - ThresholdRed * CurrentStep);
                int CurrentColorGreenOut = (int)Math.Round(GreenColorNum - ThresholdGreen * CurrentStep);
                int CurrentColorBlueOut = (int)Math.Round(BlueColorNum - ThresholdBlue * CurrentStep);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut);
                KernelColorTools.LoadBack(new Color($"{CurrentColorRedOut};{CurrentColorGreenOut};{CurrentColorBlueOut}"));
            }

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(Settings.FaderBackDelay, System.Threading.Thread.CurrentThread);
        }

    }
}
