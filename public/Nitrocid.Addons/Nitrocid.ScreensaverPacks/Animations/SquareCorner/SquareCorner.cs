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

using Terminaux.Colors;
using System;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Drivers.RNG;
using Nitrocid.Misc.Screensaver;
using Terminaux.Base;
using Nitrocid.Kernel.Configuration;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Graphical;

namespace Nitrocid.ScreensaverPacks.Animations.SquareCorner
{
    /// <summary>
    /// SquareCorner animation module
    /// </summary>
    public static class SquareCorner
    {

        private static SquareCornerDirection cornerDirection;

        /// <summary>
        /// Simulates the square corner animation
        /// </summary>
        public static void Simulate(SquareCornerSettings? Settings)
        {
            Settings ??= new();
            int RedColorNum = RandomDriver.Random(Settings.SquareCornerMinimumRedColorLevel, Settings.SquareCornerMaximumRedColorLevel);
            int GreenColorNum = RandomDriver.Random(Settings.SquareCornerMinimumGreenColorLevel, Settings.SquareCornerMaximumGreenColorLevel);
            int BlueColorNum = RandomDriver.Random(Settings.SquareCornerMinimumBlueColorLevel, Settings.SquareCornerMaximumBlueColorLevel);
            ConsoleWrapper.CursorVisible = false;

            // Set thresholds
            double ThresholdRed = RedColorNum / (double)Settings.SquareCornerMaxSteps;
            double ThresholdGreen = GreenColorNum / (double)Settings.SquareCornerMaxSteps;
            double ThresholdBlue = BlueColorNum / (double)Settings.SquareCornerMaxSteps;
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Color threshold (R;G;B: {0})", vars: [ThresholdRed, ThresholdGreen, ThresholdBlue]);

            // Determine direction based on value
            cornerDirection = (SquareCornerDirection)RandomDriver.Random(3);
            int left = 2;
            int top = 0;
            int width = 6;
            int height = 3;
            switch (cornerDirection)
            {
                case SquareCornerDirection.UpperLeft:
                    left = 2;
                    top = 0;
                    break;
                case SquareCornerDirection.UpperRight:
                    left = ConsoleWrapper.WindowWidth - width - 2;
                    top = 0;
                    break;
                case SquareCornerDirection.LowerLeft:
                    left = 2;
                    top = ConsoleWrapper.WindowHeight - height - 2;
                    break;
                case SquareCornerDirection.LowerRight:
                    left = ConsoleWrapper.WindowWidth - width - 2;
                    top = ConsoleWrapper.WindowHeight - height - 2;
                    break;
            }

            // Fade in
            int CurrentColorRedIn = 0;
            int CurrentColorGreenIn = 0;
            int CurrentColorBlueIn = 0;
            for (int CurrentStep = Settings.SquareCornerMaxSteps; CurrentStep >= 1; CurrentStep -= 1)
            {
                if (ConsoleResizeHandler.WasResized(false))
                    break;
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Step {0}/{1}", vars: [CurrentStep, Settings.SquareCornerMaxSteps]);
                ScreensaverManager.Delay(Settings.SquareCornerDelay);
                CurrentColorRedIn = (int)Math.Round(CurrentColorRedIn + ThresholdRed);
                CurrentColorGreenIn = (int)Math.Round(CurrentColorGreenIn + ThresholdGreen);
                CurrentColorBlueIn = (int)Math.Round(CurrentColorBlueIn + ThresholdBlue);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Color in (R;G;B: {0};{1};{2})", vars: [CurrentColorRedIn, CurrentColorGreenIn, CurrentColorBlueIn]);

                var box = new Box()
                {
                    Left = left,
                    Top = top,
                    Width = width,
                    Height = height,
                    Color = new Color(CurrentColorRedIn, CurrentColorGreenIn, CurrentColorBlueIn),
                };
                TextWriterRaw.WriteRaw(box.Render());
            }

            // Wait until fade out
            if (!ConsoleResizeHandler.WasResized(false))
            {
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Waiting {0} ms...", vars: [Settings.SquareCornerFadeOutDelay]);
                ScreensaverManager.Delay(Settings.SquareCornerFadeOutDelay);
            }

            // Fade out
            for (int CurrentStep = 1; CurrentStep <= Settings.SquareCornerMaxSteps; CurrentStep++)
            {
                if (ConsoleResizeHandler.WasResized(false))
                    break;
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Step {0}/{1}", vars: [CurrentStep, Settings.SquareCornerMaxSteps]);
                ScreensaverManager.Delay(Settings.SquareCornerDelay);
                int CurrentColorRedOut = (int)Math.Round(RedColorNum - ThresholdRed * CurrentStep);
                int CurrentColorGreenOut = (int)Math.Round(GreenColorNum - ThresholdGreen * CurrentStep);
                int CurrentColorBlueOut = (int)Math.Round(BlueColorNum - ThresholdBlue * CurrentStep);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", vars: [CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut]);

                var box = new Box()
                {
                    Left = left,
                    Top = top,
                    Width = width,
                    Height = height,
                    Color = new Color(CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut),
                };
                TextWriterRaw.WriteRaw(box.Render());
            }

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ScreensaverManager.Delay(Settings.SquareCornerDelay);
        }

    }
}
