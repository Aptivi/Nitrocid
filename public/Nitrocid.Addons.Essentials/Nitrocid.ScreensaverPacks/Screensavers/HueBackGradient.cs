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

using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Misc.Screensaver;
using System;
using System.Text;
using Terminaux.Colors;
using Terminaux.Colors.Models.Conversion;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Base;
using Nitrocid.Base.Kernel.Configuration;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for HueBackGradient
    /// </summary>
    public class HueBackGradientDisplay : BaseScreensaver, IScreensaver
    {

        // Hue angle is in degrees and not radians
        private static int currentHueAngle = 0;

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "HueBackGradient";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Prepare the color
            var color = new Color($"hsl:{currentHueAngle};{ScreensaverPackInit.SaversConfig.HueBackGradientSaturation};{ScreensaverPackInit.SaversConfig.HueBackGradientLuminance}");
            var hsl = ConversionTools.ToHsl(color.RGB);
            var reverseColor = new Color($"hsl:{hsl.ReverseHueWhole};{ScreensaverPackInit.SaversConfig.HueBackGradientSaturation};{ScreensaverPackInit.SaversConfig.HueBackGradientLuminance}");

            // Set thresholds for color ramp
            int RampFrameSpaces = ConsoleWrapper.WindowWidth;
            int RampColorRedThreshold = color.RGB.R - reverseColor.RGB.R;
            int RampColorGreenThreshold = color.RGB.G - reverseColor.RGB.G;
            int RampColorBlueThreshold = color.RGB.B - reverseColor.RGB.B;
            double RampColorRedSteps = RampColorRedThreshold / RampFrameSpaces;
            double RampColorGreenSteps = RampColorGreenThreshold / RampFrameSpaces;
            double RampColorBlueSteps = RampColorBlueThreshold / RampFrameSpaces;
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Set thresholds (RGB: {0};{1};{2})", vars: [RampColorRedThreshold, RampColorGreenThreshold, RampColorBlueThreshold]);
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Steps by {0} spaces (RGB: {1};{2};{3})", vars: [RampFrameSpaces, RampColorRedSteps, RampColorGreenSteps, RampColorBlueSteps]);

            // Set the current colors
            double RampCurrentColorRed = color.RGB.R;
            double RampCurrentColorGreen = color.RGB.G;
            double RampCurrentColorBlue = color.RGB.B;

            // Fill the entire screen
            StringBuilder gradientBuilder = new();
            for (int x = 0; x < ConsoleWrapper.WindowWidth; x++)
            {
                if (ConsoleResizeHandler.WasResized(false))
                    break;
                if (ScreensaverManager.Bailing)
                    return;

                // Write the background gradient!
                var RampCurrentColorInstance = new Color($"{Convert.ToInt32(RampCurrentColorRed)};{Convert.ToInt32(RampCurrentColorGreen)};{Convert.ToInt32(RampCurrentColorBlue)}");
                for (int y = 0; y < ConsoleWrapper.WindowHeight; y++)
                    gradientBuilder.Append($"{CsiSequences.GenerateCsiCursorPosition(x + 1, y + 1)}{RampCurrentColorInstance.VTSequenceBackground} ");

                // Change the colors
                RampCurrentColorRed -= RampColorRedSteps;
                RampCurrentColorGreen -= RampColorGreenSteps;
                RampCurrentColorBlue -= RampColorBlueSteps;
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got new current colors (R;G;B: {0};{1};{2}) subtracting from {3};{4};{5}", vars: [RampCurrentColorRed, RampCurrentColorGreen, RampCurrentColorBlue, RampColorRedSteps, RampColorGreenSteps, RampColorBlueSteps]);
            }
            TextWriterRaw.WritePlain(gradientBuilder.ToString(), false);

            // Set the hue angle
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.HueBackGradientDelay);
            currentHueAngle++;
            if (currentHueAngle > 360)
                currentHueAngle = 0;
        }

        /// <inheritdoc/>
        public override void ScreensaverOutro()
        {
            currentHueAngle = 0;
        }

    }
}
