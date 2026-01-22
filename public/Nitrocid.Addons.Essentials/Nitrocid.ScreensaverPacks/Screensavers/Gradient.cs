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
using System.Text;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Misc.Screensaver;
using Nitrocid.Kernel.Configuration;
using Terminaux.Colors;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Base;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for Gradient
    /// </summary>
    public class GradientDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "Gradient";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Select a color range for the ramp
            int RedColorNumFrom = RandomDriver.Random(ScreensaverPackInit.SaversConfig.GradientMinimumRedColorLevelStart, ScreensaverPackInit.SaversConfig.GradientMaximumRedColorLevelStart);
            int GreenColorNumFrom = RandomDriver.Random(ScreensaverPackInit.SaversConfig.GradientMinimumGreenColorLevelStart, ScreensaverPackInit.SaversConfig.GradientMaximumGreenColorLevelStart);
            int BlueColorNumFrom = RandomDriver.Random(ScreensaverPackInit.SaversConfig.GradientMinimumBlueColorLevelStart, ScreensaverPackInit.SaversConfig.GradientMaximumBlueColorLevelStart);
            int RedColorNumTo = RandomDriver.Random(ScreensaverPackInit.SaversConfig.GradientMinimumRedColorLevelEnd, ScreensaverPackInit.SaversConfig.GradientMaximumRedColorLevelEnd);
            int GreenColorNumTo = RandomDriver.Random(ScreensaverPackInit.SaversConfig.GradientMinimumGreenColorLevelEnd, ScreensaverPackInit.SaversConfig.GradientMaximumGreenColorLevelEnd);
            int BlueColorNumTo = RandomDriver.Random(ScreensaverPackInit.SaversConfig.GradientMinimumBlueColorLevelEnd, ScreensaverPackInit.SaversConfig.GradientMaximumBlueColorLevelEnd);
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color from (R;G;B: {0};{1};{2}) to (R;G;B: {3};{4};{5})", vars: [RedColorNumFrom, GreenColorNumFrom, BlueColorNumFrom, RedColorNumTo, GreenColorNumTo, BlueColorNumTo]);

            // Set thresholds for color gradient rot
            int RotFrameSpaces = ConsoleWrapper.WindowWidth;
            int RotColorRedThreshold = RedColorNumFrom - RedColorNumTo;
            int RotColorGreenThreshold = GreenColorNumFrom - GreenColorNumTo;
            int RotColorBlueThreshold = BlueColorNumFrom - BlueColorNumTo;
            double RotColorRedSteps = RotColorRedThreshold / RotFrameSpaces;
            double RotColorGreenSteps = RotColorGreenThreshold / RotFrameSpaces;
            double RotColorBlueSteps = RotColorBlueThreshold / RotFrameSpaces;
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Set thresholds (RGB: {0};{1};{2})", vars: [RotColorRedThreshold, RotColorGreenThreshold, RotColorBlueThreshold]);
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Steps by {0} spaces (RGB: {1};{2};{3})", vars: [RotFrameSpaces, RotColorRedSteps, RotColorGreenSteps, RotColorBlueSteps]);

            // Set the current colors
            double RotCurrentColorRed = RedColorNumFrom;
            double RotCurrentColorGreen = GreenColorNumFrom;
            double RotCurrentColorBlue = BlueColorNumFrom;

            // Fill the entire screen
            StringBuilder gradientBuilder = new();
            for (int x = 0; x < ConsoleWrapper.WindowWidth; x++)
            {
                if (ConsoleResizeHandler.WasResized(false))
                    break;
                if (ScreensaverManager.Bailing)
                    return;

                // Write the background gradient!
                var RotCurrentColorInstance = new Color($"{Convert.ToInt32(RotCurrentColorRed)};{Convert.ToInt32(RotCurrentColorGreen)};{Convert.ToInt32(RotCurrentColorBlue)}");
                for (int y = 0; y < ConsoleWrapper.WindowHeight; y++)
                    gradientBuilder.Append($"{CsiSequences.GenerateCsiCursorPosition(x + 1, y + 1)}{RotCurrentColorInstance.VTSequenceBackgroundTrueColor()} ");

                // Change the colors
                RotCurrentColorRed -= RotColorRedSteps;
                RotCurrentColorGreen -= RotColorGreenSteps;
                RotCurrentColorBlue -= RotColorBlueSteps;
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got new current colors (R;G;B: {0};{1};{2}) subtracting from {3};{4};{5}", vars: [RotCurrentColorRed, RotCurrentColorGreen, RotCurrentColorBlue, RotColorRedSteps, RotColorGreenSteps, RotColorBlueSteps]);
            }
            TextWriterRaw.WritePlain(gradientBuilder.ToString(), false);

            // Reset resize sync
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.GradientNextRotDelay);
            ConsoleResizeHandler.WasResized();
        }

    }
}
