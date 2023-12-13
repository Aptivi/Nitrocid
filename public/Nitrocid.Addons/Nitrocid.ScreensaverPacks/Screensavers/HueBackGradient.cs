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

using KS.ConsoleBase;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Debugging;
using KS.Kernel.Threading;
using KS.Misc.Screensaver;
using System;
using System.Text;
using Terminaux.Colors;
using Terminaux.Colors.Models.Conversion;
using Textify.Sequences.Builder.Types;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Settings for HueBackGradient
    /// </summary>
    public static class HueBackGradientSettings
    {

        /// <summary>
        /// [HueBackGradient] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int HueBackGradientDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.HueBackGradientDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                ScreensaverPackInit.SaversConfig.HueBackGradientDelay = value;
            }
        }
        /// <summary>
        /// [HueBackGradient] How intense is the color?
        /// </summary>
        public static int HueBackGradientSaturation
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.HueBackGradientSaturation;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                if (value > 100)
                    value = 100;
                ScreensaverPackInit.SaversConfig.HueBackGradientSaturation = value;
            }
        }
        /// <summary>
        /// [HueBackGradient] How light is the color?
        /// </summary>
        public static int HueBackGradientLuminance
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.HueBackGradientLuminance;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                if (value > 100)
                    value = 100;
                ScreensaverPackInit.SaversConfig.HueBackGradientLuminance = value;
            }
        }

    }

    /// <summary>
    /// Display code for HueBackGradient
    /// </summary>
    public class HueBackGradientDisplay : BaseScreensaver, IScreensaver
    {

        // Hue angle is in degrees and not radians
        private static int currentHueAngle = 0;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "HueBackGradient";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Prepare the color
            var color = new Color($"hsl:{currentHueAngle};{HueBackGradientSettings.HueBackGradientSaturation};{HueBackGradientSettings.HueBackGradientLuminance}");
            var hsl = HslConversionTools.ConvertFrom(color.RGB);
            var reverseColor = new Color($"hsl:{hsl.ReverseHueWhole};{HueBackGradientSettings.HueBackGradientSaturation};{HueBackGradientSettings.HueBackGradientLuminance}");

            // Set thresholds for color ramp
            int RampFrameSpaces = ConsoleWrapper.WindowWidth;
            int RampColorRedThreshold = color.R - reverseColor.R;
            int RampColorGreenThreshold = color.G - reverseColor.G;
            int RampColorBlueThreshold = color.B - reverseColor.B;
            double RampColorRedSteps = RampColorRedThreshold / RampFrameSpaces;
            double RampColorGreenSteps = RampColorGreenThreshold / RampFrameSpaces;
            double RampColorBlueSteps = RampColorBlueThreshold / RampFrameSpaces;
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Set thresholds (RGB: {0};{1};{2})", RampColorRedThreshold, RampColorGreenThreshold, RampColorBlueThreshold);
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Steps by {0} spaces (RGB: {1};{2};{3})", RampFrameSpaces, RampColorRedSteps, RampColorGreenSteps, RampColorBlueSteps);

            // Set the current colors
            double RampCurrentColorRed = color.R;
            double RampCurrentColorGreen = color.G;
            double RampCurrentColorBlue = color.B;

            // Fill the entire screen
            StringBuilder gradientBuilder = new();
            for (int x = 0; x < ConsoleWrapper.WindowWidth; x++)
            {
                if (ConsoleResizeListener.WasResized(false))
                    break;

                // Write the background gradient!
                var RampCurrentColorInstance = new Color($"{Convert.ToInt32(RampCurrentColorRed)};{Convert.ToInt32(RampCurrentColorGreen)};{Convert.ToInt32(RampCurrentColorBlue)}");
                for (int y = 0; y < ConsoleWrapper.WindowHeight; y++)
                    gradientBuilder.Append($"{CsiSequences.GenerateCsiCursorPosition(x + 1, y + 1)}{RampCurrentColorInstance.VTSequenceBackgroundTrueColor} ");

                // Change the colors
                RampCurrentColorRed -= RampColorRedSteps;
                RampCurrentColorGreen -= RampColorGreenSteps;
                RampCurrentColorBlue -= RampColorBlueSteps;
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got new current colors (R;G;B: {0};{1};{2}) subtracting from {3};{4};{5}", RampCurrentColorRed, RampCurrentColorGreen, RampCurrentColorBlue, RampColorRedSteps, RampColorGreenSteps, RampColorBlueSteps);
            }
            TextWriterColor.WritePlain(gradientBuilder.ToString(), false);

            // Set the hue angle
            ThreadManager.SleepNoBlock(HueBackGradientSettings.HueBackGradientDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            currentHueAngle++;
            if (currentHueAngle > 360)
                currentHueAngle = 0;

            // Reset resize sync
            ConsoleResizeListener.WasResized();
        }

        /// <inheritdoc/>
        public override void ScreensaverOutro()
        {
            currentHueAngle = 0;
        }

    }
}
