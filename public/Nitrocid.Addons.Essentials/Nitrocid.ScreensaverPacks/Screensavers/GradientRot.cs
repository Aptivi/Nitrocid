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
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Misc.Screensaver;
using Terminaux.Colors;
using Terminaux.Base;
using Terminaux.Colors.Data;
using Nitrocid.Kernel.Configuration;
using Terminaux.Base.Extensions;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for GradientRot
    /// </summary>
    public class GradientRotDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "GradientRot";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Select a color range for the ramp
            int RedColorNumFrom = RandomDriver.Random(ScreensaverPackInit.SaversConfig.GradientRotMinimumRedColorLevelStart, ScreensaverPackInit.SaversConfig.GradientRotMaximumRedColorLevelStart);
            int GreenColorNumFrom = RandomDriver.Random(ScreensaverPackInit.SaversConfig.GradientRotMinimumGreenColorLevelStart, ScreensaverPackInit.SaversConfig.GradientRotMaximumGreenColorLevelStart);
            int BlueColorNumFrom = RandomDriver.Random(ScreensaverPackInit.SaversConfig.GradientRotMinimumBlueColorLevelStart, ScreensaverPackInit.SaversConfig.GradientRotMaximumBlueColorLevelStart);
            int RedColorNumTo = RandomDriver.Random(ScreensaverPackInit.SaversConfig.GradientRotMinimumRedColorLevelEnd, ScreensaverPackInit.SaversConfig.GradientRotMaximumRedColorLevelEnd);
            int GreenColorNumTo = RandomDriver.Random(ScreensaverPackInit.SaversConfig.GradientRotMinimumGreenColorLevelEnd, ScreensaverPackInit.SaversConfig.GradientRotMaximumGreenColorLevelEnd);
            int BlueColorNumTo = RandomDriver.Random(ScreensaverPackInit.SaversConfig.GradientRotMinimumBlueColorLevelEnd, ScreensaverPackInit.SaversConfig.GradientRotMaximumBlueColorLevelEnd);
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color from (R;G;B: {0};{1};{2}) to (R;G;B: {3};{4};{5})", vars: [RedColorNumFrom, GreenColorNumFrom, BlueColorNumFrom, RedColorNumTo, GreenColorNumTo, BlueColorNumTo]);

            // Set thresholds for color ramp
            int RampFrameSpaces = ConsoleWrapper.WindowWidth;
            int RampColorRedThreshold = RedColorNumFrom - RedColorNumTo;
            int RampColorGreenThreshold = GreenColorNumFrom - GreenColorNumTo;
            int RampColorBlueThreshold = BlueColorNumFrom - BlueColorNumTo;
            double RampColorRedSteps = RampColorRedThreshold / (double)RampFrameSpaces;
            double RampColorGreenSteps = RampColorGreenThreshold / (double)RampFrameSpaces;
            double RampColorBlueSteps = RampColorBlueThreshold / (double)RampFrameSpaces;
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Set thresholds (RGB: {0};{1};{2})", vars: [RampColorRedThreshold, RampColorGreenThreshold, RampColorBlueThreshold]);
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Steps by {0} spaces (RGB: {1};{2};{3})", vars: [RampFrameSpaces, RampColorRedSteps, RampColorGreenSteps, RampColorBlueSteps]);

            // Set the current colors
            double RampCurrentColorRed = RedColorNumFrom;
            double RampCurrentColorGreen = GreenColorNumFrom;
            double RampCurrentColorBlue = BlueColorNumFrom;

            // Set the console color and fill the ramp!
            while (
                Convert.ToInt32(RampCurrentColorRed) != RedColorNumTo &&
                Convert.ToInt32(RampCurrentColorGreen) != GreenColorNumTo &&
                Convert.ToInt32(RampCurrentColorBlue) != BlueColorNumTo
            )
            {
                if (ConsoleResizeHandler.WasResized(false))
                    break;

                // Populate the variables for sub-gradients
                int RampSubgradientRedColorNumFrom = RedColorNumFrom;
                int RampSubgradientGreenColorNumFrom = GreenColorNumFrom;
                int RampSubgradientBlueColorNumFrom = BlueColorNumFrom;
                int RampSubgradientRedColorNumTo = (int)Math.Round(RampCurrentColorRed);
                int RampSubgradientGreenColorNumTo = (int)Math.Round(RampCurrentColorGreen);
                int RampSubgradientBlueColorNumTo = (int)Math.Round(RampCurrentColorBlue);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got subgradient color from (R;G;B: {0};{1};{2}) to (R;G;B: {3};{4};{5})", vars: [RampSubgradientRedColorNumFrom, RampSubgradientGreenColorNumFrom, RampSubgradientBlueColorNumFrom, RampSubgradientRedColorNumTo, RampSubgradientGreenColorNumTo, RampSubgradientBlueColorNumTo]);

                // Set the sub-gradient current colors
                double RampSubgradientCurrentColorRed = RampSubgradientRedColorNumFrom;
                double RampSubgradientCurrentColorGreen = RampSubgradientGreenColorNumFrom;
                double RampSubgradientCurrentColorBlue = RampSubgradientBlueColorNumFrom;
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got subgradient current colors (R;G;B: {0};{1};{2})", vars: [RampSubgradientCurrentColorRed, RampSubgradientCurrentColorGreen, RampSubgradientCurrentColorBlue]);

                // Set the sub-gradient thresholds
                int RampSubgradientColorRedThreshold = RampSubgradientRedColorNumFrom - RampSubgradientRedColorNumTo;
                int RampSubgradientColorGreenThreshold = RampSubgradientGreenColorNumFrom - RampSubgradientGreenColorNumTo;
                int RampSubgradientColorBlueThreshold = RampSubgradientBlueColorNumFrom - RampSubgradientBlueColorNumTo;
                double RampSubgradientColorRedSteps = RampSubgradientColorRedThreshold / (double)RampFrameSpaces;
                double RampSubgradientColorGreenSteps = RampSubgradientColorGreenThreshold / (double)RampFrameSpaces;
                double RampSubgradientColorBlueSteps = RampSubgradientColorBlueThreshold / (double)RampFrameSpaces;
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Set subgradient thresholds (RGB: {0};{1};{2})", vars: [RampSubgradientColorRedThreshold, RampSubgradientColorGreenThreshold, RampSubgradientColorBlueThreshold]);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Steps by {0} spaces for subgradient (RGB: {1};{2};{3})", vars: [RampFrameSpaces, RampSubgradientColorRedSteps, RampSubgradientColorGreenSteps, RampSubgradientColorBlueSteps]);

                // Make a new instance
                var RampSubgradientCurrentColorInstance = new Color($"{Convert.ToInt32(RampSubgradientCurrentColorRed)};{Convert.ToInt32(RampSubgradientCurrentColorGreen)};{Convert.ToInt32(RampSubgradientCurrentColorBlue)}");
                ConsoleColoring.SetConsoleColorDry(RampSubgradientCurrentColorInstance, true);

                // Try to fill the ramp
                int RampSubgradientStepsMade = 0;
                int RampCurrentPositionLeft = 0;
                while (RampSubgradientStepsMade != RampFrameSpaces)
                {
                    if (ConsoleResizeHandler.WasResized(false))
                        break;

                    // Fill the entire screen
                    for (int y = 0; y < ConsoleWrapper.WindowHeight; y++)
                        TextWriterWhereColor.WriteWhere(" ", RampCurrentPositionLeft, y);

                    // Update left position
                    RampCurrentPositionLeft = ConsoleWrapper.CursorLeft;
                    RampSubgradientStepsMade += 1;

                    // Change the colors
                    RampSubgradientCurrentColorRed -= RampSubgradientColorRedSteps;
                    RampSubgradientCurrentColorGreen -= RampSubgradientColorGreenSteps;
                    RampSubgradientCurrentColorBlue -= RampSubgradientColorBlueSteps;
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got new subgradient current colors (R;G;B: {0};{1};{2}) subtracting from {3};{4};{5}", vars: [RampSubgradientCurrentColorRed, RampSubgradientCurrentColorGreen, RampSubgradientCurrentColorBlue, RampSubgradientColorRedSteps, RampSubgradientColorGreenSteps, RampSubgradientColorBlueSteps]);

                    // Check the values to make sure we don't go below zero
                    if (RampSubgradientCurrentColorRed < 0)
                    {
                        DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.W, "RampSubgradientCurrentColorRed is less than 0! Setting...");
                        RampSubgradientCurrentColorRed = 0;
                    }
                    if (RampSubgradientCurrentColorGreen < 0)
                    {
                        DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.W, "RampSubgradientCurrentColorGreen is less than 0! Setting...");
                        RampSubgradientCurrentColorGreen = 0;
                    }
                    if (RampSubgradientCurrentColorBlue < 0)
                    {
                        DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.W, "RampSubgradientCurrentColorBlue is less than 0! Setting...");
                        RampSubgradientCurrentColorBlue = 0;
                    }
                    RampSubgradientCurrentColorInstance = new Color($"{Convert.ToInt32(RampSubgradientCurrentColorRed)};{Convert.ToInt32(RampSubgradientCurrentColorGreen)};{Convert.ToInt32(RampSubgradientCurrentColorBlue)}");
                    ConsoleColoring.SetConsoleColorDry(RampSubgradientCurrentColorInstance, true);
                }

                // Change the colors
                RampCurrentColorRed -= RampColorRedSteps;
                RampCurrentColorGreen -= RampColorGreenSteps;
                RampCurrentColorBlue -= RampColorBlueSteps;

                // Check the values to make sure we don't go below zero
                if (RampCurrentColorRed < 0)
                {
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.W, "RampCurrentColorRed is less than 0! Setting...");
                    RampCurrentColorRed = 0;
                }
                if (RampCurrentColorGreen < 0)
                {
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.W, "RampCurrentColorGreen is less than 0! Setting...");
                    RampCurrentColorGreen = 0;
                }
                if (RampCurrentColorBlue < 0)
                {
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.W, "RampCurrentColorBlue is less than 0! Setting...");
                    RampCurrentColorBlue = 0;
                }
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got new current colors (R;G;B: {0};{1};{2}) subtracting from {3};{4};{5}", vars: [RampCurrentColorRed, RampCurrentColorGreen, RampCurrentColorBlue, RampColorRedSteps, RampColorGreenSteps, RampColorBlueSteps]);

                // Delay writing
                RampCurrentPositionLeft = 0;
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Current left position: {0}", vars: [RampCurrentPositionLeft]);
                ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.GradientRotDelay);
            }

            // Clear the scene
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.GradientRotNextRampDelay);
            ConsoleColoring.LoadBackDry(new Color(ConsoleColors.Black));

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
        }

    }
}
