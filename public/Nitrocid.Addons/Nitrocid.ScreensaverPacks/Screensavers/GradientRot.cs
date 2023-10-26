//
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
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
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Drivers.RNG;
using KS.Kernel.Debugging;
using KS.Kernel.Threading;
using KS.Misc.Screensaver;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Settings for GradientRot
    /// </summary>
    public static class GradientRotSettings
    {

        /// <summary>
        /// [GradientRot] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int GradientRotDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.GradientRotDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                ScreensaverPackInit.SaversConfig.GradientRotDelay = value;
            }
        }
        /// <summary>
        /// [GradientRot] How many milliseconds to wait before rotting the next screen?
        /// </summary>
        public static int GradientRotNextRampDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.GradientRotNextRampDelay;
            }
            set
            {
                if (value <= 0)
                    value = 250;
                ScreensaverPackInit.SaversConfig.GradientRotNextRampDelay = value;
            }
        }
        /// <summary>
        /// [GradientRot] The minimum red color level (true color - start)
        /// </summary>
        public static int GradientRotMinimumRedColorLevelStart
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.GradientRotMinimumRedColorLevelStart;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.GradientRotMinimumRedColorLevelStart = value;
            }
        }
        /// <summary>
        /// [GradientRot] The minimum green color level (true color - start)
        /// </summary>
        public static int GradientRotMinimumGreenColorLevelStart
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.GradientRotMinimumGreenColorLevelStart;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.GradientRotMinimumGreenColorLevelStart = value;
            }
        }
        /// <summary>
        /// [GradientRot] The minimum blue color level (true color - start)
        /// </summary>
        public static int GradientRotMinimumBlueColorLevelStart
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.GradientRotMinimumBlueColorLevelStart;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.GradientRotMinimumBlueColorLevelStart = value;
            }
        }
        /// <summary>
        /// [GradientRot] The maximum red color level (true color - start)
        /// </summary>
        public static int GradientRotMaximumRedColorLevelStart
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.GradientRotMaximumRedColorLevelStart;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.GradientRotMinimumRedColorLevelStart)
                    value = ScreensaverPackInit.SaversConfig.GradientRotMinimumRedColorLevelStart;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.GradientRotMaximumRedColorLevelStart = value;
            }
        }
        /// <summary>
        /// [GradientRot] The maximum green color level (true color - start)
        /// </summary>
        public static int GradientRotMaximumGreenColorLevelStart
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.GradientRotMaximumGreenColorLevelStart;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.GradientRotMinimumGreenColorLevelStart)
                    value = ScreensaverPackInit.SaversConfig.GradientRotMinimumGreenColorLevelStart;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.GradientRotMaximumGreenColorLevelStart = value;
            }
        }
        /// <summary>
        /// [GradientRot] The maximum blue color level (true color - start)
        /// </summary>
        public static int GradientRotMaximumBlueColorLevelStart
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.GradientRotMaximumBlueColorLevelStart;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.GradientRotMinimumBlueColorLevelStart)
                    value = ScreensaverPackInit.SaversConfig.GradientRotMinimumBlueColorLevelStart;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.GradientRotMaximumBlueColorLevelStart = value;
            }
        }
        /// <summary>
        /// [GradientRot] The minimum red color level (true color - end)
        /// </summary>
        public static int GradientRotMinimumRedColorLevelEnd
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.GradientRotMinimumRedColorLevelEnd;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.GradientRotMinimumRedColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [GradientRot] The minimum green color level (true color - end)
        /// </summary>
        public static int GradientRotMinimumGreenColorLevelEnd
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.GradientRotMinimumGreenColorLevelEnd;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.GradientRotMinimumGreenColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [GradientRot] The minimum blue color level (true color - end)
        /// </summary>
        public static int GradientRotMinimumBlueColorLevelEnd
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.GradientRotMinimumBlueColorLevelEnd;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.GradientRotMinimumBlueColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [GradientRot] The maximum red color level (true color - end)
        /// </summary>
        public static int GradientRotMaximumRedColorLevelEnd
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.GradientRotMaximumRedColorLevelEnd;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.GradientRotMinimumRedColorLevelEnd)
                    value = ScreensaverPackInit.SaversConfig.GradientRotMinimumRedColorLevelEnd;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.GradientRotMaximumRedColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [GradientRot] The maximum green color level (true color - end)
        /// </summary>
        public static int GradientRotMaximumGreenColorLevelEnd
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.GradientRotMaximumGreenColorLevelEnd;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.GradientRotMinimumGreenColorLevelEnd)
                    value = ScreensaverPackInit.SaversConfig.GradientRotMinimumGreenColorLevelEnd;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.GradientRotMaximumGreenColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [GradientRot] The maximum blue color level (true color - end)
        /// </summary>
        public static int GradientRotMaximumBlueColorLevelEnd
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.GradientRotMaximumBlueColorLevelEnd;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.GradientRotMinimumBlueColorLevelEnd)
                    value = ScreensaverPackInit.SaversConfig.GradientRotMinimumBlueColorLevelEnd;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.GradientRotMaximumBlueColorLevelEnd = value;
            }
        }

    }

    /// <summary>
    /// Display code for GradientRot
    /// </summary>
    public class GradientRotDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "GradientRot";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Select a color range for the ramp
            int RedColorNumFrom = RandomDriver.Random(GradientRotSettings.GradientRotMinimumRedColorLevelStart, GradientRotSettings.GradientRotMaximumRedColorLevelStart);
            int GreenColorNumFrom = RandomDriver.Random(GradientRotSettings.GradientRotMinimumGreenColorLevelStart, GradientRotSettings.GradientRotMaximumGreenColorLevelStart);
            int BlueColorNumFrom = RandomDriver.Random(GradientRotSettings.GradientRotMinimumBlueColorLevelStart, GradientRotSettings.GradientRotMaximumBlueColorLevelStart);
            int RedColorNumTo = RandomDriver.Random(GradientRotSettings.GradientRotMinimumRedColorLevelEnd, GradientRotSettings.GradientRotMaximumRedColorLevelEnd);
            int GreenColorNumTo = RandomDriver.Random(GradientRotSettings.GradientRotMinimumGreenColorLevelEnd, GradientRotSettings.GradientRotMaximumGreenColorLevelEnd);
            int BlueColorNumTo = RandomDriver.Random(GradientRotSettings.GradientRotMinimumBlueColorLevelEnd, GradientRotSettings.GradientRotMaximumBlueColorLevelEnd);
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color from (R;G;B: {0};{1};{2}) to (R;G;B: {3};{4};{5})", RedColorNumFrom, GreenColorNumFrom, BlueColorNumFrom, RedColorNumTo, GreenColorNumTo, BlueColorNumTo);

            // Set thresholds for color ramp
            int RampFrameSpaces = ConsoleWrapper.WindowWidth;
            int RampColorRedThreshold = RedColorNumFrom - RedColorNumTo;
            int RampColorGreenThreshold = GreenColorNumFrom - GreenColorNumTo;
            int RampColorBlueThreshold = BlueColorNumFrom - BlueColorNumTo;
            double RampColorRedSteps = RampColorRedThreshold / (double)RampFrameSpaces;
            double RampColorGreenSteps = RampColorGreenThreshold / (double)RampFrameSpaces;
            double RampColorBlueSteps = RampColorBlueThreshold / (double)RampFrameSpaces;
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Set thresholds (RGB: {0};{1};{2})", RampColorRedThreshold, RampColorGreenThreshold, RampColorBlueThreshold);
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Steps by {0} spaces (RGB: {1};{2};{3})", RampFrameSpaces, RampColorRedSteps, RampColorGreenSteps, RampColorBlueSteps);

            // Set the current colors
            double RampCurrentColorRed = RedColorNumFrom;
            double RampCurrentColorGreen = GreenColorNumFrom;
            double RampCurrentColorBlue = BlueColorNumFrom;

            // Set the console color and fill the ramp!
            while (!(Convert.ToInt32(RampCurrentColorRed) == RedColorNumTo & Convert.ToInt32(RampCurrentColorGreen) == GreenColorNumTo & Convert.ToInt32(RampCurrentColorBlue) == BlueColorNumTo))
            {
                if (ConsoleResizeListener.WasResized(false))
                    break;

                // Populate the variables for sub-gradients
                int RampSubgradientRedColorNumFrom = RedColorNumFrom;
                int RampSubgradientGreenColorNumFrom = GreenColorNumFrom;
                int RampSubgradientBlueColorNumFrom = BlueColorNumFrom;
                int RampSubgradientRedColorNumTo = (int)Math.Round(RampCurrentColorRed);
                int RampSubgradientGreenColorNumTo = (int)Math.Round(RampCurrentColorGreen);
                int RampSubgradientBlueColorNumTo = (int)Math.Round(RampCurrentColorBlue);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got subgradient color from (R;G;B: {0};{1};{2}) to (R;G;B: {3};{4};{5})", RampSubgradientRedColorNumFrom, RampSubgradientGreenColorNumFrom, RampSubgradientBlueColorNumFrom, RampSubgradientRedColorNumTo, RampSubgradientGreenColorNumTo, RampSubgradientBlueColorNumTo);

                // Set the sub-gradient current colors
                double RampSubgradientCurrentColorRed = RampSubgradientRedColorNumFrom;
                double RampSubgradientCurrentColorGreen = RampSubgradientGreenColorNumFrom;
                double RampSubgradientCurrentColorBlue = RampSubgradientBlueColorNumFrom;
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got subgradient current colors (R;G;B: {0};{1};{2})", RampSubgradientCurrentColorRed, RampSubgradientCurrentColorGreen, RampSubgradientCurrentColorBlue);

                // Set the sub-gradient thresholds
                int RampSubgradientColorRedThreshold = RampSubgradientRedColorNumFrom - RampSubgradientRedColorNumTo;
                int RampSubgradientColorGreenThreshold = RampSubgradientGreenColorNumFrom - RampSubgradientGreenColorNumTo;
                int RampSubgradientColorBlueThreshold = RampSubgradientBlueColorNumFrom - RampSubgradientBlueColorNumTo;
                double RampSubgradientColorRedSteps = RampSubgradientColorRedThreshold / (double)RampFrameSpaces;
                double RampSubgradientColorGreenSteps = RampSubgradientColorGreenThreshold / (double)RampFrameSpaces;
                double RampSubgradientColorBlueSteps = RampSubgradientColorBlueThreshold / (double)RampFrameSpaces;
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Set subgradient thresholds (RGB: {0};{1};{2})", RampSubgradientColorRedThreshold, RampSubgradientColorGreenThreshold, RampSubgradientColorBlueThreshold);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Steps by {0} spaces for subgradient (RGB: {1};{2};{3})", RampFrameSpaces, RampSubgradientColorRedSteps, RampSubgradientColorGreenSteps, RampSubgradientColorBlueSteps);

                // Make a new instance
                var RampSubgradientCurrentColorInstance = new Color($"{Convert.ToInt32(RampSubgradientCurrentColorRed)};{Convert.ToInt32(RampSubgradientCurrentColorGreen)};{Convert.ToInt32(RampSubgradientCurrentColorBlue)}");
                KernelColorTools.SetConsoleColor(RampSubgradientCurrentColorInstance, true);

                // Try to fill the ramp
                int RampSubgradientStepsMade = 0;
                int RampCurrentPositionLeft = 0;
                while (RampSubgradientStepsMade != RampFrameSpaces)
                {
                    if (ConsoleResizeListener.WasResized(false))
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
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got new subgradient current colors (R;G;B: {0};{1};{2}) subtracting from {3};{4};{5}", RampSubgradientCurrentColorRed, RampSubgradientCurrentColorGreen, RampSubgradientCurrentColorBlue, RampSubgradientColorRedSteps, RampSubgradientColorGreenSteps, RampSubgradientColorBlueSteps);

                    // Check the values to make sure we don't go below zero
                    if (RampSubgradientCurrentColorRed < 0)
                    {
                        DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.W, "RampSubgradientCurrentColorRed is less than 0! Setting...");
                        RampSubgradientCurrentColorRed = 0;
                    }
                    if (RampSubgradientCurrentColorGreen < 0)
                    {
                        DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.W, "RampSubgradientCurrentColorGreen is less than 0! Setting...");
                        RampSubgradientCurrentColorGreen = 0;
                    }
                    if (RampSubgradientCurrentColorBlue < 0)
                    {
                        DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.W, "RampSubgradientCurrentColorBlue is less than 0! Setting...");
                        RampSubgradientCurrentColorBlue = 0;
                    }
                    RampSubgradientCurrentColorInstance = new Color($"{Convert.ToInt32(RampSubgradientCurrentColorRed)};{Convert.ToInt32(RampSubgradientCurrentColorGreen)};{Convert.ToInt32(RampSubgradientCurrentColorBlue)}");
                    KernelColorTools.SetConsoleColor(RampSubgradientCurrentColorInstance, true);
                }

                // Change the colors
                RampCurrentColorRed -= RampColorRedSteps;
                RampCurrentColorGreen -= RampColorGreenSteps;
                RampCurrentColorBlue -= RampColorBlueSteps;

                // Check the values to make sure we don't go below zero
                if (RampCurrentColorRed < 0)
                {
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.W, "RampCurrentColorRed is less than 0! Setting...");
                    RampCurrentColorRed = 0;
                }
                if (RampCurrentColorGreen < 0)
                {
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.W, "RampCurrentColorGreen is less than 0! Setting...");
                    RampCurrentColorGreen = 0;
                }
                if (RampCurrentColorBlue < 0)
                {
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.W, "RampCurrentColorBlue is less than 0! Setting...");
                    RampCurrentColorBlue = 0;
                }
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got new current colors (R;G;B: {0};{1};{2}) subtracting from {3};{4};{5}", RampCurrentColorRed, RampCurrentColorGreen, RampCurrentColorBlue, RampColorRedSteps, RampColorGreenSteps, RampColorBlueSteps);

                // Delay writing
                RampCurrentPositionLeft = 0;
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Current left position: {0}", RampCurrentPositionLeft);
                ThreadManager.SleepNoBlock(GradientRotSettings.GradientRotDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            }

            // Clear the scene
            ThreadManager.SleepNoBlock(GradientRotSettings.GradientRotNextRampDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            ConsoleWrapper.BackgroundColor = ConsoleColor.Black;
            ConsoleWrapper.Clear();

            // Reset resize sync
            ConsoleResizeListener.WasResized();
        }

    }
}
