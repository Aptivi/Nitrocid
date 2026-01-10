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
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Misc.Screensaver;
using Nitrocid.Base.Kernel.Configuration;
using Terminaux.Colors;
using Terminaux.Base;
using Terminaux.Base.Extensions;

namespace Nitrocid.ScreensaverPacks.Animations.TextReveal
{
    /// <summary>
    /// TextReveal animation module
    /// </summary>
    public static class TextReveal
    {

        /// <summary>
        /// Simulates the fading animation
        /// </summary>
        public static void Simulate(TextRevealSettings? Settings)
        {
            Settings ??= new();
            int RedColorNum = RandomDriver.Random(Settings.TextRevealMinimumRedColorLevel, Settings.TextRevealMaximumRedColorLevel);
            int GreenColorNum = RandomDriver.Random(Settings.TextRevealMinimumGreenColorLevel, Settings.TextRevealMaximumGreenColorLevel);
            int BlueColorNum = RandomDriver.Random(Settings.TextRevealMinimumBlueColorLevel, Settings.TextRevealMaximumBlueColorLevel);
            var backgroundColor = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            ConsoleColoring.LoadBackDry(backgroundColor);

            // Check the text
            int Left = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
            int Top = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Selected left and top: {0}, {1}", vars: [Left, Top]);
            int textWidth = ConsoleChar.EstimateCellWidth(Settings.TextRevealWrite);
            if (textWidth + Left >= ConsoleWrapper.WindowWidth)
            {
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Text length of {0} exceeded window width of {1}.", vars: [textWidth + Left, ConsoleWrapper.WindowWidth]);
                Left -= textWidth + 1;
            }

            // Set thresholds
            double ThresholdRed = RedColorNum / (double)Settings.TextRevealMaxSteps;
            double ThresholdGreen = GreenColorNum / (double)Settings.TextRevealMaxSteps;
            double ThresholdBlue = BlueColorNum / (double)Settings.TextRevealMaxSteps;
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Color threshold (R;G;B: {0})", vars: [ThresholdRed, ThresholdGreen, ThresholdBlue]);

            // Wait until fade out
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Waiting {0} ms...", vars: [Settings.TextRevealFadeOutDelay]);
            ScreensaverManager.Delay(Settings.TextRevealFadeOutDelay);

            // Fade out
            for (int CurrentStep = 1; CurrentStep <= Settings.TextRevealMaxSteps; CurrentStep++)
            {
                if (ConsoleResizeHandler.WasResized(false))
                    break;
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Step {0}/{1}", vars: [CurrentStep, Settings.TextRevealMaxSteps]);
                ScreensaverManager.Delay(Settings.TextRevealDelay);
                int CurrentColorRedOut = (int)Math.Round(RedColorNum - ThresholdRed * CurrentStep);
                int CurrentColorGreenOut = (int)Math.Round(GreenColorNum - ThresholdGreen * CurrentStep);
                int CurrentColorBlueOut = (int)Math.Round(BlueColorNum - ThresholdBlue * CurrentStep);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", vars: [CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut]);
                if (!ConsoleResizeHandler.WasResized(false))
                    TextWriterWhereColor.WriteWhereColorBack(Settings.TextRevealWrite, Left, Top, true, new Color(CurrentColorRedOut + ";" + CurrentColorGreenOut + ";" + CurrentColorBlueOut), backgroundColor);
            }

            // Wait until new screen
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Waiting {0} ms...", vars: [Settings.TextRevealNewScreenDelay]);
            ScreensaverManager.Delay(Settings.TextRevealNewScreenDelay);

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
        }

    }
}
