﻿//
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
using Nitrocid.Base.Kernel.Configuration;
using Terminaux.Base;
using Terminaux.Colors;
using Nitrocid.Base.Misc.Screensaver;

namespace Nitrocid.ScreensaverPacks.Animations.EdgePulse
{
    /// <summary>
    /// Edge pulse animation module
    /// </summary>
    public static class EdgePulse
    {

        /// <summary>
        /// Simulates the edge pulsing animation
        /// </summary>
        public static void Simulate(EdgePulseSettings? Settings)
        {
            Settings ??= new();

            // Now, do the rest
            int RedColorNum = RandomDriver.Random(Settings.EdgePulseMinimumRedColorLevel, Settings.EdgePulseMaximumRedColorLevel);
            int GreenColorNum = RandomDriver.Random(Settings.EdgePulseMinimumGreenColorLevel, Settings.EdgePulseMaximumGreenColorLevel);
            int BlueColorNum = RandomDriver.Random(Settings.EdgePulseMinimumBlueColorLevel, Settings.EdgePulseMaximumBlueColorLevel);
            ConsoleWrapper.CursorVisible = false;

            // Set thresholds
            double ThresholdRed = RedColorNum / (double)Settings.EdgePulseMaxSteps;
            double ThresholdGreen = GreenColorNum / (double)Settings.EdgePulseMaxSteps;
            double ThresholdBlue = BlueColorNum / (double)Settings.EdgePulseMaxSteps;
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Color threshold (R;G;B: {0})", vars: [ThresholdRed, ThresholdGreen, ThresholdBlue]);

            // Fade in
            int CurrentColorRedIn = 0;
            int CurrentColorGreenIn = 0;
            int CurrentColorBlueIn = 0;
            for (int CurrentStep = Settings.EdgePulseMaxSteps; CurrentStep >= 1; CurrentStep -= 1)
            {
                if (ConsoleResizeHandler.WasResized(false))
                    break;
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Step {0}/{1}", vars: [CurrentStep, Settings.EdgePulseMaxSteps]);
                ScreensaverManager.Delay(Settings.EdgePulseDelay);
                CurrentColorRedIn = (int)Math.Round(CurrentColorRedIn + ThresholdRed);
                CurrentColorGreenIn = (int)Math.Round(CurrentColorGreenIn + ThresholdGreen);
                CurrentColorBlueIn = (int)Math.Round(CurrentColorBlueIn + ThresholdBlue);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Color in (R;G;B: {0};{1};{2})", vars: [CurrentColorRedIn, CurrentColorGreenIn, CurrentColorBlueIn]);
                if (!ConsoleResizeHandler.WasResized(false))
                {
                    ColorTools.SetConsoleColorDry(new Color(CurrentColorRedIn, CurrentColorGreenIn, CurrentColorBlueIn), true);
                    FillIn();
                }
            }

            // Fade out
            for (int CurrentStep = 1; CurrentStep <= Settings.EdgePulseMaxSteps; CurrentStep++)
            {
                if (ConsoleResizeHandler.WasResized(false))
                    break;
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Step {0}/{1}", vars: [CurrentStep, Settings.EdgePulseMaxSteps]);
                ScreensaverManager.Delay(Settings.EdgePulseDelay);
                int CurrentColorRedOut = (int)Math.Round(RedColorNum - ThresholdRed * CurrentStep);
                int CurrentColorGreenOut = (int)Math.Round(GreenColorNum - ThresholdGreen * CurrentStep);
                int CurrentColorBlueOut = (int)Math.Round(BlueColorNum - ThresholdBlue * CurrentStep);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", vars: [CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut]);
                if (!ConsoleResizeHandler.WasResized(false))
                {
                    ColorTools.SetConsoleColorDry(new Color(CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut), true);
                    FillIn();
                }
            }

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ScreensaverManager.Delay(Settings.EdgePulseDelay);
        }

        private static void FillIn()
        {
            int FloorTopLeftEdge = 0;
            int FloorBottomLeftEdge = 0;
            DebugWriter.WriteDebug(DebugLevel.I, "Top left edge: {0}, Bottom left edge: {1}", vars: [FloorTopLeftEdge, FloorBottomLeftEdge]);

            int FloorTopRightEdge = ConsoleWrapper.WindowWidth - 1;
            int FloorBottomRightEdge = ConsoleWrapper.WindowWidth - 1;
            DebugWriter.WriteDebug(DebugLevel.I, "Top right edge: {0}, Bottom right edge: {1}", vars: [FloorTopRightEdge, FloorBottomRightEdge]);

            int FloorTopEdge = 0;
            int FloorBottomEdge = ConsoleWrapper.WindowHeight - 1;
            DebugWriter.WriteDebug(DebugLevel.I, "Top edge: {0}, Bottom edge: {1}", vars: [FloorTopEdge, FloorBottomEdge]);

            int FloorLeftEdge = 0;
            int FloorRightEdge = ConsoleWrapper.WindowWidth - 2;
            DebugWriter.WriteDebug(DebugLevel.I, "Left edge: {0}, Right edge: {1}", vars: [FloorLeftEdge, FloorRightEdge]);

            // First, draw the floor top edge
            for (int x = FloorTopLeftEdge; x <= FloorTopRightEdge; x++)
            {
                ConsoleWrapper.SetCursorPosition(x, 0);
                DebugWriter.WriteDebug(DebugLevel.I, "Drawing floor top edge ({0}, {1})", vars: [x, 1]);
                ConsoleWrapper.Write(" ");
            }

            // Second, draw the floor bottom edge
            for (int x = FloorBottomLeftEdge; x <= FloorBottomRightEdge; x++)
            {
                ConsoleWrapper.SetCursorPosition(x, FloorBottomEdge);
                DebugWriter.WriteDebug(DebugLevel.I, "Drawing floor bottom edge ({0}, {1})", vars: [x, FloorBottomEdge]);
                ConsoleWrapper.Write(" ");
            }

            // Third, draw the floor left edge
            for (int y = FloorTopEdge; y <= FloorBottomEdge; y++)
            {
                ConsoleWrapper.SetCursorPosition(FloorLeftEdge, y);
                DebugWriter.WriteDebug(DebugLevel.I, "Drawing floor left edge ({0}, {1})", vars: [FloorLeftEdge, y]);
                ConsoleWrapper.Write("  ");
            }

            // Finally, draw the floor right edge
            for (int y = FloorTopEdge; y <= FloorBottomEdge; y++)
            {
                ConsoleWrapper.SetCursorPosition(FloorRightEdge, y);
                DebugWriter.WriteDebug(DebugLevel.I, "Drawing floor right edge ({0}, {1})", vars: [FloorRightEdge, y]);
                ConsoleWrapper.Write("  ");
            }
        }

    }
}
