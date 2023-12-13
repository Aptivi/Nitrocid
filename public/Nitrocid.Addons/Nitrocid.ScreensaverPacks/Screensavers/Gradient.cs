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
using System.Text;
using KS.ConsoleBase;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Drivers.RNG;
using KS.Kernel.Debugging;
using KS.Kernel.Threading;
using KS.Misc.Screensaver;
using Terminaux.Colors;
using Textify.Sequences.Builder.Types;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Settings for Gradient
    /// </summary>
    public static class GradientSettings
    {

        /// <summary>
        /// [Gradient] How many milliseconds to wait before rotting the next screen?
        /// </summary>
        public static int GradientNextRotDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.GradientNextRotDelay;
            }
            set
            {
                if (value <= 0)
                    value = 3000;
                ScreensaverPackInit.SaversConfig.GradientNextRotDelay = value;
            }
        }
        /// <summary>
        /// [Gradient] The minimum red color level (true color - start)
        /// </summary>
        public static int GradientMinimumRedColorLevelStart
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.GradientMinimumRedColorLevelStart;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.GradientMinimumRedColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Gradient] The minimum green color level (true color - start)
        /// </summary>
        public static int GradientMinimumGreenColorLevelStart
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.GradientMinimumGreenColorLevelStart;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.GradientMinimumGreenColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Gradient] The minimum blue color level (true color - start)
        /// </summary>
        public static int GradientMinimumBlueColorLevelStart
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.GradientMinimumBlueColorLevelStart;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.GradientMinimumBlueColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Gradient] The maximum red color level (true color - start)
        /// </summary>
        public static int GradientMaximumRedColorLevelStart
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.GradientMaximumRedColorLevelStart;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.GradientMinimumRedColorLevelStart)
                    value = ScreensaverPackInit.SaversConfig.GradientMinimumRedColorLevelStart;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.GradientMaximumRedColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Gradient] The maximum green color level (true color - start)
        /// </summary>
        public static int GradientMaximumGreenColorLevelStart
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.GradientMaximumGreenColorLevelStart;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.GradientMinimumGreenColorLevelStart)
                    value = ScreensaverPackInit.SaversConfig.GradientMinimumGreenColorLevelStart;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.GradientMaximumGreenColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Gradient] The maximum blue color level (true color - start)
        /// </summary>
        public static int GradientMaximumBlueColorLevelStart
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.GradientMaximumBlueColorLevelStart;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.GradientMinimumBlueColorLevelStart)
                    value = ScreensaverPackInit.SaversConfig.GradientMinimumBlueColorLevelStart;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.GradientMaximumBlueColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Gradient] The minimum red color level (true color - end)
        /// </summary>
        public static int GradientMinimumRedColorLevelEnd
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.GradientMinimumRedColorLevelEnd;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.GradientMinimumRedColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Gradient] The minimum green color level (true color - end)
        /// </summary>
        public static int GradientMinimumGreenColorLevelEnd
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.GradientMinimumGreenColorLevelEnd;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.GradientMinimumGreenColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Gradient] The minimum blue color level (true color - end)
        /// </summary>
        public static int GradientMinimumBlueColorLevelEnd
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.GradientMinimumBlueColorLevelEnd;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.GradientMinimumBlueColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Gradient] The maximum red color level (true color - end)
        /// </summary>
        public static int GradientMaximumRedColorLevelEnd
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.GradientMaximumRedColorLevelEnd;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.GradientMinimumRedColorLevelEnd)
                    value = ScreensaverPackInit.SaversConfig.GradientMinimumRedColorLevelEnd;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.GradientMaximumRedColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Gradient] The maximum green color level (true color - end)
        /// </summary>
        public static int GradientMaximumGreenColorLevelEnd
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.GradientMaximumGreenColorLevelEnd;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.GradientMinimumGreenColorLevelEnd)
                    value = ScreensaverPackInit.SaversConfig.GradientMinimumGreenColorLevelEnd;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.GradientMaximumGreenColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Gradient] The maximum blue color level (true color - end)
        /// </summary>
        public static int GradientMaximumBlueColorLevelEnd
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.GradientMaximumBlueColorLevelEnd;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.GradientMinimumBlueColorLevelEnd)
                    value = ScreensaverPackInit.SaversConfig.GradientMinimumBlueColorLevelEnd;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.GradientMaximumBlueColorLevelEnd = value;
            }
        }

    }

    /// <summary>
    /// Display code for Gradient
    /// </summary>
    public class GradientDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Gradient";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Select a color range for the ramp
            int RedColorNumFrom = RandomDriver.Random(GradientSettings.GradientMinimumRedColorLevelStart, GradientSettings.GradientMaximumRedColorLevelStart);
            int GreenColorNumFrom = RandomDriver.Random(GradientSettings.GradientMinimumGreenColorLevelStart, GradientSettings.GradientMaximumGreenColorLevelStart);
            int BlueColorNumFrom = RandomDriver.Random(GradientSettings.GradientMinimumBlueColorLevelStart, GradientSettings.GradientMaximumBlueColorLevelStart);
            int RedColorNumTo = RandomDriver.Random(GradientSettings.GradientMinimumRedColorLevelEnd, GradientSettings.GradientMaximumRedColorLevelEnd);
            int GreenColorNumTo = RandomDriver.Random(GradientSettings.GradientMinimumGreenColorLevelEnd, GradientSettings.GradientMaximumGreenColorLevelEnd);
            int BlueColorNumTo = RandomDriver.Random(GradientSettings.GradientMinimumBlueColorLevelEnd, GradientSettings.GradientMaximumBlueColorLevelEnd);
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color from (R;G;B: {0};{1};{2}) to (R;G;B: {3};{4};{5})", RedColorNumFrom, GreenColorNumFrom, BlueColorNumFrom, RedColorNumTo, GreenColorNumTo, BlueColorNumTo);

            // Set thresholds for color gradient rot
            int RotFrameSpaces = ConsoleWrapper.WindowWidth;
            int RotColorRedThreshold = RedColorNumFrom - RedColorNumTo;
            int RotColorGreenThreshold = GreenColorNumFrom - GreenColorNumTo;
            int RotColorBlueThreshold = BlueColorNumFrom - BlueColorNumTo;
            double RotColorRedSteps = RotColorRedThreshold / RotFrameSpaces;
            double RotColorGreenSteps = RotColorGreenThreshold / RotFrameSpaces;
            double RotColorBlueSteps = RotColorBlueThreshold / RotFrameSpaces;
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Set thresholds (RGB: {0};{1};{2})", RotColorRedThreshold, RotColorGreenThreshold, RotColorBlueThreshold);
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Steps by {0} spaces (RGB: {1};{2};{3})", RotFrameSpaces, RotColorRedSteps, RotColorGreenSteps, RotColorBlueSteps);

            // Set the current colors
            double RotCurrentColorRed = RedColorNumFrom;
            double RotCurrentColorGreen = GreenColorNumFrom;
            double RotCurrentColorBlue = BlueColorNumFrom;

            // Fill the entire screen
            StringBuilder gradientBuilder = new();
            for (int x = 0; x < ConsoleWrapper.WindowWidth; x++)
            {
                if (ConsoleResizeListener.WasResized(false))
                    break;

                // Write the background gradient!
                var RotCurrentColorInstance = new Color($"{Convert.ToInt32(RotCurrentColorRed)};{Convert.ToInt32(RotCurrentColorGreen)};{Convert.ToInt32(RotCurrentColorBlue)}");
                for (int y = 0; y < ConsoleWrapper.WindowHeight; y++)
                    gradientBuilder.Append($"{CsiSequences.GenerateCsiCursorPosition(x + 1, y + 1)}{RotCurrentColorInstance.VTSequenceBackgroundTrueColor} ");

                // Change the colors
                RotCurrentColorRed -= RotColorRedSteps;
                RotCurrentColorGreen -= RotColorGreenSteps;
                RotCurrentColorBlue -= RotColorBlueSteps;
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got new current colors (R;G;B: {0};{1};{2}) subtracting from {3};{4};{5}", RotCurrentColorRed, RotCurrentColorGreen, RotCurrentColorBlue, RotColorRedSteps, RotColorGreenSteps, RotColorBlueSteps);
            }
            TextWriterColor.WritePlain(gradientBuilder.ToString(), false);

            // Reset resize sync
            ThreadManager.SleepNoBlock(GradientSettings.GradientNextRotDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            ConsoleResizeListener.WasResized();
        }

    }
}
