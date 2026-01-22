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
using Nitrocid.Kernel.Configuration;
using Nitrocid.ConsoleBase.Colors;
using Terminaux.Base.Extensions;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for BarRot
    /// </summary>
    public class BarRotDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "BarRot";

        /// <inheritdoc/>
        public override void ScreensaverPreparation() =>
            KernelColorTools.LoadBackground();

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Select a color range for the ramp
            int RedColorNumFrom = RandomDriver.Random(ScreensaverPackInit.SaversConfig.BarRotMinimumRedColorLevelStart, ScreensaverPackInit.SaversConfig.BarRotMaximumRedColorLevelStart);
            int GreenColorNumFrom = RandomDriver.Random(ScreensaverPackInit.SaversConfig.BarRotMinimumGreenColorLevelStart, ScreensaverPackInit.SaversConfig.BarRotMaximumGreenColorLevelStart);
            int BlueColorNumFrom = RandomDriver.Random(ScreensaverPackInit.SaversConfig.BarRotMinimumBlueColorLevelStart, ScreensaverPackInit.SaversConfig.BarRotMaximumBlueColorLevelStart);
            int RedColorNumTo = RandomDriver.Random(ScreensaverPackInit.SaversConfig.BarRotMinimumRedColorLevelEnd, ScreensaverPackInit.SaversConfig.BarRotMaximumRedColorLevelEnd);
            int GreenColorNumTo = RandomDriver.Random(ScreensaverPackInit.SaversConfig.BarRotMinimumGreenColorLevelEnd, ScreensaverPackInit.SaversConfig.BarRotMaximumGreenColorLevelEnd);
            int BlueColorNumTo = RandomDriver.Random(ScreensaverPackInit.SaversConfig.BarRotMinimumBlueColorLevelEnd, ScreensaverPackInit.SaversConfig.BarRotMaximumBlueColorLevelEnd);
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color from (R;G;B: {0};{1};{2}) to (R;G;B: {3};{4};{5})", vars: [RedColorNumFrom, GreenColorNumFrom, BlueColorNumFrom, RedColorNumTo, GreenColorNumTo, BlueColorNumTo]);

            // Set start and end widths for the ramp frame
            int RampFrameStartWidth = 4;
            int RampFrameEndWidth = ConsoleWrapper.WindowWidth - RampFrameStartWidth;
            int RampFrameSpaces = RampFrameEndWidth - RampFrameStartWidth;
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Start width: {0}, End width: {1}, Spaces: {2}", vars: [RampFrameStartWidth, RampFrameEndWidth, RampFrameSpaces]);

            // Set thresholds for color ramp
            int RampColorRedThreshold = RedColorNumFrom - RedColorNumTo;
            int RampColorGreenThreshold = GreenColorNumFrom - GreenColorNumTo;
            int RampColorBlueThreshold = BlueColorNumFrom - BlueColorNumTo;
            double RampColorRedSteps = RampColorRedThreshold / (double)RampFrameSpaces;
            double RampColorGreenSteps = RampColorGreenThreshold / (double)RampFrameSpaces;
            double RampColorBlueSteps = RampColorBlueThreshold / (double)RampFrameSpaces;
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Set thresholds (RGB: {0};{1};{2})", vars: [RampColorRedThreshold, RampColorGreenThreshold, RampColorBlueThreshold]);
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Steps by {0} spaces (RGB: {1};{2};{3})", vars: [RampFrameSpaces, RampColorRedSteps, RampColorGreenSteps, RampColorBlueSteps]);

            // Let the ramp be printed in the center
            int RampCenterPosition = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d);
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Center position: {0}", vars: [RampCenterPosition]);

            // Set the current positions
            int RampCurrentPositionLeft = RampFrameStartWidth + 1;
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Current left position: {0}", vars: [RampCurrentPositionLeft]);

            // Draw the frame
            if (!ConsoleResizeHandler.WasResized(false))
            {
                TextWriterWhereColor.WriteWhereColor(ScreensaverPackInit.SaversConfig.BarRotUpperLeftCornerChar.ToString(), RampFrameStartWidth, RampCenterPosition - 2, false, ScreensaverPackInit.SaversConfig.BarRotUseBorderColors ? new Color(ScreensaverPackInit.SaversConfig.BarRotUpperLeftCornerColor) : ConsoleColoring.GetGray());
                TextWriterColor.WriteColor(new string(ScreensaverPackInit.SaversConfig.BarRotUpperFrameChar, RampFrameSpaces), false, ScreensaverPackInit.SaversConfig.BarRotUseBorderColors ? new Color(ScreensaverPackInit.SaversConfig.BarRotUpperFrameColor) : ConsoleColoring.GetGray());
                TextWriterColor.WriteColor(ScreensaverPackInit.SaversConfig.BarRotUpperRightCornerChar.ToString(), false, ScreensaverPackInit.SaversConfig.BarRotUseBorderColors ? new Color(ScreensaverPackInit.SaversConfig.BarRotUpperRightCornerColor) : ConsoleColoring.GetGray());
                TextWriterWhereColor.WriteWhereColor(ScreensaverPackInit.SaversConfig.BarRotLeftFrameChar.ToString(), RampFrameStartWidth, RampCenterPosition - 1, false, ScreensaverPackInit.SaversConfig.BarRotUseBorderColors ? new Color(ScreensaverPackInit.SaversConfig.BarRotLeftFrameColor) : ConsoleColoring.GetGray());
                TextWriterWhereColor.WriteWhereColor(ScreensaverPackInit.SaversConfig.BarRotLeftFrameChar.ToString(), RampFrameStartWidth, RampCenterPosition, false, ScreensaverPackInit.SaversConfig.BarRotUseBorderColors ? new Color(ScreensaverPackInit.SaversConfig.BarRotLeftFrameColor) : ConsoleColoring.GetGray());
                TextWriterWhereColor.WriteWhereColor(ScreensaverPackInit.SaversConfig.BarRotLeftFrameChar.ToString(), RampFrameStartWidth, RampCenterPosition + 1, false, ScreensaverPackInit.SaversConfig.BarRotUseBorderColors ? new Color(ScreensaverPackInit.SaversConfig.BarRotLeftFrameColor) : ConsoleColoring.GetGray());
                TextWriterWhereColor.WriteWhereColor(ScreensaverPackInit.SaversConfig.BarRotRightFrameChar.ToString(), RampFrameEndWidth + 1, RampCenterPosition - 1, false, ScreensaverPackInit.SaversConfig.BarRotUseBorderColors ? new Color(ScreensaverPackInit.SaversConfig.BarRotLeftFrameColor) : ConsoleColoring.GetGray());
                TextWriterWhereColor.WriteWhereColor(ScreensaverPackInit.SaversConfig.BarRotRightFrameChar.ToString(), RampFrameEndWidth + 1, RampCenterPosition, false, ScreensaverPackInit.SaversConfig.BarRotUseBorderColors ? new Color(ScreensaverPackInit.SaversConfig.BarRotLeftFrameColor) : ConsoleColoring.GetGray());
                TextWriterWhereColor.WriteWhereColor(ScreensaverPackInit.SaversConfig.BarRotRightFrameChar.ToString(), RampFrameEndWidth + 1, RampCenterPosition + 1, false, ScreensaverPackInit.SaversConfig.BarRotUseBorderColors ? new Color(ScreensaverPackInit.SaversConfig.BarRotLeftFrameColor) : ConsoleColoring.GetGray());
                TextWriterWhereColor.WriteWhereColor(ScreensaverPackInit.SaversConfig.BarRotLowerLeftCornerChar.ToString(), RampFrameStartWidth, RampCenterPosition + 2, false, ScreensaverPackInit.SaversConfig.BarRotUseBorderColors ? new Color(ScreensaverPackInit.SaversConfig.BarRotLowerLeftCornerColor) : ConsoleColoring.GetGray());
                TextWriterColor.WriteColor(new string(ScreensaverPackInit.SaversConfig.BarRotLowerFrameChar, RampFrameSpaces), false, ScreensaverPackInit.SaversConfig.BarRotUseBorderColors ? new Color(ScreensaverPackInit.SaversConfig.BarRotLowerFrameColor) : ConsoleColoring.GetGray());
                TextWriterColor.WriteColor(ScreensaverPackInit.SaversConfig.BarRotLowerRightCornerChar.ToString(), false, ScreensaverPackInit.SaversConfig.BarRotUseBorderColors ? new Color(ScreensaverPackInit.SaversConfig.BarRotLowerRightCornerColor) : ConsoleColoring.GetGray());
            }

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
                while (RampSubgradientStepsMade != RampFrameSpaces)
                {
                    if (ConsoleResizeHandler.WasResized(false))
                        break;
                    ConsoleWrapper.SetCursorPosition(RampCurrentPositionLeft, RampCenterPosition - 1);
                    ConsoleWrapper.Write(' ');
                    ConsoleWrapper.SetCursorPosition(RampCurrentPositionLeft, RampCenterPosition);
                    ConsoleWrapper.Write(' ');
                    ConsoleWrapper.SetCursorPosition(RampCurrentPositionLeft, RampCenterPosition + 1);
                    ConsoleWrapper.Write(' ');
                    RampCurrentPositionLeft = ConsoleWrapper.CursorLeft;
                    RampSubgradientStepsMade += 1;

                    // Change the colors
                    RampSubgradientCurrentColorRed -= RampSubgradientColorRedSteps;
                    RampSubgradientCurrentColorGreen -= RampSubgradientColorGreenSteps;
                    RampSubgradientCurrentColorBlue -= RampSubgradientColorBlueSteps;
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got new subgradient current colors (R;G;B: {0};{1};{2}) subtracting from {3};{4};{5}", vars: [RampSubgradientCurrentColorRed, RampSubgradientCurrentColorGreen, RampSubgradientCurrentColorBlue, RampSubgradientColorRedSteps, RampSubgradientColorGreenSteps, RampSubgradientColorBlueSteps]);
                    RampSubgradientCurrentColorInstance = new Color($"{Convert.ToInt32(RampSubgradientCurrentColorRed)};{Convert.ToInt32(RampSubgradientCurrentColorGreen)};{Convert.ToInt32(RampSubgradientCurrentColorBlue)}");
                    ConsoleColoring.SetConsoleColorDry(RampSubgradientCurrentColorInstance, true);
                }

                // Change the colors
                RampCurrentColorRed -= RampColorRedSteps;
                RampCurrentColorGreen -= RampColorGreenSteps;
                RampCurrentColorBlue -= RampColorBlueSteps;
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got new current colors (R;G;B: {0};{1};{2}) subtracting from {3};{4};{5}", vars: [RampCurrentColorRed, RampCurrentColorGreen, RampCurrentColorBlue, RampColorRedSteps, RampColorGreenSteps, RampColorBlueSteps]);

                // Delay writing
                RampCurrentPositionLeft = RampFrameStartWidth + 1;
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Current left position: {0}", vars: [RampCurrentPositionLeft]);
                ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.BarRotDelay);
            }
            if (!ConsoleResizeHandler.WasResized(false))
                ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.BarRotNextRampDelay);

            // Clear the scene
            KernelColorTools.LoadBackground();

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
        }

    }
}
