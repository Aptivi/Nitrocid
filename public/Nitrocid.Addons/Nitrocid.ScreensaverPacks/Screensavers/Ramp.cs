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
using Terminaux.Colors;
using Terminaux.Base;
using Nitrocid.Kernel.Configuration;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.ConsoleBase.Colors;
using Terminaux.Writer.CyclicWriters.Graphical;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for Ramp
    /// </summary>
    public class RampDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "Ramp";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            int RedColorNumFrom = RandomDriver.Random(ScreensaverPackInit.SaversConfig.RampMinimumRedColorLevelStart, ScreensaverPackInit.SaversConfig.RampMaximumRedColorLevelStart);
            int GreenColorNumFrom = RandomDriver.Random(ScreensaverPackInit.SaversConfig.RampMinimumGreenColorLevelStart, ScreensaverPackInit.SaversConfig.RampMaximumGreenColorLevelStart);
            int BlueColorNumFrom = RandomDriver.Random(ScreensaverPackInit.SaversConfig.RampMinimumBlueColorLevelStart, ScreensaverPackInit.SaversConfig.RampMaximumBlueColorLevelStart);
            int ColorNumFrom = RandomDriver.Random(ScreensaverPackInit.SaversConfig.RampMinimumColorLevelStart, ScreensaverPackInit.SaversConfig.RampMaximumColorLevelStart);
            int RedColorNumTo = RandomDriver.Random(ScreensaverPackInit.SaversConfig.RampMinimumRedColorLevelEnd, ScreensaverPackInit.SaversConfig.RampMaximumRedColorLevelEnd);
            int GreenColorNumTo = RandomDriver.Random(ScreensaverPackInit.SaversConfig.RampMinimumGreenColorLevelEnd, ScreensaverPackInit.SaversConfig.RampMaximumGreenColorLevelEnd);
            int BlueColorNumTo = RandomDriver.Random(ScreensaverPackInit.SaversConfig.RampMinimumBlueColorLevelEnd, ScreensaverPackInit.SaversConfig.RampMaximumBlueColorLevelEnd);
            int ColorNumTo = RandomDriver.Random(ScreensaverPackInit.SaversConfig.RampMinimumColorLevelEnd, ScreensaverPackInit.SaversConfig.RampMaximumColorLevelEnd);

            // Console resizing can sometimes cause the cursor to remain visible. This happens on Windows 10's terminal.
            ConsoleWrapper.CursorVisible = false;

            // Set start and end widths for the ramp frame
            int RampFrameStartWidth = 4;
            int RampFrameEndWidth = ConsoleWrapper.WindowWidth - RampFrameStartWidth;
            int RampFrameSpaces = RampFrameEndWidth - RampFrameStartWidth - 1;
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Start width: {0}, End width: {1}, Spaces: {2}", vars: [RampFrameStartWidth, RampFrameEndWidth, RampFrameSpaces]);

            // Set thresholds for color ramps
            int RampColorRedThreshold = RedColorNumFrom - RedColorNumTo;
            int RampColorGreenThreshold = GreenColorNumFrom - GreenColorNumTo;
            int RampColorBlueThreshold = BlueColorNumFrom - BlueColorNumTo;
            int RampColorThreshold = ColorNumFrom - ColorNumTo;
            double RampColorRedSteps = RampColorRedThreshold / (double)RampFrameSpaces;
            double RampColorGreenSteps = RampColorGreenThreshold / (double)RampFrameSpaces;
            double RampColorBlueSteps = RampColorBlueThreshold / (double)RampFrameSpaces;
            double RampColorSteps = RampColorThreshold / (double)RampFrameSpaces;
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Set thresholds (RGB: {0};{1};{2} | Normal: {3})", vars: [RampColorRedThreshold, RampColorGreenThreshold, RampColorBlueThreshold, RampColorThreshold]);
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Steps by {0} spaces (RGB: {1};{2};{3} | Normal: {4})", vars: [RampFrameSpaces, RampColorRedSteps, RampColorGreenSteps, RampColorBlueSteps, RampColorSteps]);

            // Let the ramp be printed in the center
            int RampCenterPosition = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d);
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Center position: {0}", vars: [RampCenterPosition]);

            // Set the current positions
            int RampCurrentPositionLeft = RampFrameStartWidth + 1;
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Current left position: {0}", vars: [RampCurrentPositionLeft]);

            // Draw the frame
            if (!ConsoleResizeHandler.WasResized(false))
            {
                var border = new Border()
                {
                    Left = RampFrameStartWidth,
                    Top = RampCenterPosition - 2,
                    Width = RampFrameSpaces,
                    Height = 3,
                    Color =
                        ScreensaverPackInit.SaversConfig.RampUseBorderColors ?
                        new Color(ScreensaverPackInit.SaversConfig.RampLeftFrameColor) :
                        ColorTools.GetGray(),
                };
                TextWriterRaw.WriteRaw(border.Render());
            }

            // Draw the ramp
            if (ScreensaverPackInit.SaversConfig.RampTrueColor)
            {
                // Set the current colors
                double RampCurrentColorRed = RedColorNumFrom;
                double RampCurrentColorGreen = GreenColorNumFrom;
                double RampCurrentColorBlue = BlueColorNumFrom;
                var RampCurrentColorInstance = new Color($"{Convert.ToInt32(RampCurrentColorRed)};{Convert.ToInt32(RampCurrentColorGreen)};{Convert.ToInt32(RampCurrentColorBlue)}");

                // Set the console color and fill the ramp!
                ColorTools.SetConsoleColorDry(RampCurrentColorInstance, true);
                int step = 1;
                while (step <= RampFrameSpaces)
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

                    // Change the colors
                    RampCurrentColorRed -= RampColorRedSteps;
                    RampCurrentColorGreen -= RampColorGreenSteps;
                    RampCurrentColorBlue -= RampColorBlueSteps;
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got new current colors (R;G;B: {0};{1};{2}) subtracting from {3};{4};{5}", vars: [RampCurrentColorRed, RampCurrentColorGreen, RampCurrentColorBlue, RampColorRedSteps, RampColorGreenSteps, RampColorBlueSteps]);
                    RampCurrentColorInstance = new Color($"{Convert.ToInt32(RampCurrentColorRed)};{Convert.ToInt32(RampCurrentColorGreen)};{Convert.ToInt32(RampCurrentColorBlue)}");
                    ColorTools.SetConsoleColorDry(RampCurrentColorInstance, true);

                    // Delay writing
                    step++;
                    ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.RampDelay);
                }
            }
            else
            {
                // Set the current colors
                double RampCurrentColor = ColorNumFrom;
                var RampCurrentColorInstance = new Color(Convert.ToInt32(RampCurrentColor));

                // Set the console color and fill the ramp!
                ColorTools.SetConsoleColorDry(RampCurrentColorInstance, true);
                while (Convert.ToInt32(RampCurrentColor) != ColorNumTo)
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

                    // Change the colors
                    RampCurrentColor -= RampColorSteps;
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got new current colors (Normal: {0}) subtracting from {1}", vars: [RampCurrentColor, RampColorSteps]);
                    RampCurrentColorInstance = new Color(Convert.ToInt32(RampCurrentColor));
                    ColorTools.SetConsoleColorDry(RampCurrentColorInstance, true);

                    // Delay writing
                    ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.RampDelay);
                }
            }
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.RampNextRampDelay);
            KernelColorTools.LoadBackground();
            ConsoleResizeHandler.WasResized();
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.RampDelay);
        }

    }
}
