
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using ColorSeq;
using KS.ConsoleBase;
using KS.Drivers.RNG;
using KS.Kernel.Debugging;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using ColorTools = KS.ConsoleBase.Colors.ColorTools;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for GradientRot
    /// </summary>
    public static class GradientRotSettings
    {

        private static int _gradientRotDelay = 10;
        private static int _gradientRotNextScreenDelay = 250;
        private static int _gradientRotMinimumRedColorLevelStart = 0;
        private static int _gradientRotMinimumGreenColorLevelStart = 0;
        private static int _gradientRotMinimumBlueColorLevelStart = 0;
        private static int _gradientRotMaximumRedColorLevelStart = 255;
        private static int _gradientRotMaximumGreenColorLevelStart = 255;
        private static int _gradientRotMaximumBlueColorLevelStart = 255;
        private static int _gradientRotMinimumRedColorLevelEnd = 0;
        private static int _gradientRotMinimumGreenColorLevelEnd = 0;
        private static int _gradientRotMinimumBlueColorLevelEnd = 0;
        private static int _gradientRotMaximumRedColorLevelEnd = 255;
        private static int _gradientRotMaximumGreenColorLevelEnd = 255;
        private static int _gradientRotMaximumBlueColorLevelEnd = 255;

        /// <summary>
        /// [GradientRot] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int GradientRotDelay
        {
            get
            {
                return _gradientRotDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                _gradientRotDelay = value;
            }
        }
        /// <summary>
        /// [GradientRot] How many milliseconds to wait before rotting the next screen?
        /// </summary>
        public static int GradientRotNextRampDelay
        {
            get
            {
                return _gradientRotNextScreenDelay;
            }
            set
            {
                if (value <= 0)
                    value = 250;
                _gradientRotNextScreenDelay = value;
            }
        }
        /// <summary>
        /// [GradientRot] The minimum red color level (true color - start)
        /// </summary>
        public static int GradientRotMinimumRedColorLevelStart
        {
            get
            {
                return _gradientRotMinimumRedColorLevelStart;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _gradientRotMinimumRedColorLevelStart = value;
            }
        }
        /// <summary>
        /// [GradientRot] The minimum green color level (true color - start)
        /// </summary>
        public static int GradientRotMinimumGreenColorLevelStart
        {
            get
            {
                return _gradientRotMinimumGreenColorLevelStart;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _gradientRotMinimumGreenColorLevelStart = value;
            }
        }
        /// <summary>
        /// [GradientRot] The minimum blue color level (true color - start)
        /// </summary>
        public static int GradientRotMinimumBlueColorLevelStart
        {
            get
            {
                return _gradientRotMinimumBlueColorLevelStart;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _gradientRotMinimumBlueColorLevelStart = value;
            }
        }
        /// <summary>
        /// [GradientRot] The maximum red color level (true color - start)
        /// </summary>
        public static int GradientRotMaximumRedColorLevelStart
        {
            get
            {
                return _gradientRotMaximumRedColorLevelStart;
            }
            set
            {
                if (value <= _gradientRotMinimumRedColorLevelStart)
                    value = _gradientRotMinimumRedColorLevelStart;
                if (value > 255)
                    value = 255;
                _gradientRotMaximumRedColorLevelStart = value;
            }
        }
        /// <summary>
        /// [GradientRot] The maximum green color level (true color - start)
        /// </summary>
        public static int GradientRotMaximumGreenColorLevelStart
        {
            get
            {
                return _gradientRotMaximumGreenColorLevelStart;
            }
            set
            {
                if (value <= _gradientRotMinimumGreenColorLevelStart)
                    value = _gradientRotMinimumGreenColorLevelStart;
                if (value > 255)
                    value = 255;
                _gradientRotMaximumGreenColorLevelStart = value;
            }
        }
        /// <summary>
        /// [GradientRot] The maximum blue color level (true color - start)
        /// </summary>
        public static int GradientRotMaximumBlueColorLevelStart
        {
            get
            {
                return _gradientRotMaximumBlueColorLevelStart;
            }
            set
            {
                if (value <= _gradientRotMinimumBlueColorLevelStart)
                    value = _gradientRotMinimumBlueColorLevelStart;
                if (value > 255)
                    value = 255;
                _gradientRotMaximumBlueColorLevelStart = value;
            }
        }
        /// <summary>
        /// [GradientRot] The minimum red color level (true color - end)
        /// </summary>
        public static int GradientRotMinimumRedColorLevelEnd
        {
            get
            {
                return _gradientRotMinimumRedColorLevelEnd;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _gradientRotMinimumRedColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [GradientRot] The minimum green color level (true color - end)
        /// </summary>
        public static int GradientRotMinimumGreenColorLevelEnd
        {
            get
            {
                return _gradientRotMinimumGreenColorLevelEnd;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _gradientRotMinimumGreenColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [GradientRot] The minimum blue color level (true color - end)
        /// </summary>
        public static int GradientRotMinimumBlueColorLevelEnd
        {
            get
            {
                return _gradientRotMinimumBlueColorLevelEnd;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _gradientRotMinimumBlueColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [GradientRot] The maximum red color level (true color - end)
        /// </summary>
        public static int GradientRotMaximumRedColorLevelEnd
        {
            get
            {
                return _gradientRotMaximumRedColorLevelEnd;
            }
            set
            {
                if (value <= _gradientRotMinimumRedColorLevelEnd)
                    value = _gradientRotMinimumRedColorLevelEnd;
                if (value > 255)
                    value = 255;
                _gradientRotMaximumRedColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [GradientRot] The maximum green color level (true color - end)
        /// </summary>
        public static int GradientRotMaximumGreenColorLevelEnd
        {
            get
            {
                return _gradientRotMaximumGreenColorLevelEnd;
            }
            set
            {
                if (value <= _gradientRotMinimumGreenColorLevelEnd)
                    value = _gradientRotMinimumGreenColorLevelEnd;
                if (value > 255)
                    value = 255;
                _gradientRotMaximumGreenColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [GradientRot] The maximum blue color level (true color - end)
        /// </summary>
        public static int GradientRotMaximumBlueColorLevelEnd
        {
            get
            {
                return _gradientRotMaximumBlueColorLevelEnd;
            }
            set
            {
                if (value <= _gradientRotMinimumBlueColorLevelEnd)
                    value = _gradientRotMinimumBlueColorLevelEnd;
                if (value > 255)
                    value = 255;
                _gradientRotMaximumBlueColorLevelEnd = value;
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
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ConsoleWrapper.BackgroundColor = ConsoleColor.Black;
            ConsoleWrapper.ForegroundColor = ConsoleColor.White;
            ConsoleWrapper.Clear();
        }

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
            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color from (R;G;B: {0};{1};{2}) to (R;G;B: {3};{4};{5})", RedColorNumFrom, GreenColorNumFrom, BlueColorNumFrom, RedColorNumTo, GreenColorNumTo, BlueColorNumTo);

            // Set thresholds for color ramp
            int RampFrameSpaces = ConsoleWrapper.WindowWidth;
            int RampColorRedThreshold = RedColorNumFrom - RedColorNumTo;
            int RampColorGreenThreshold = GreenColorNumFrom - GreenColorNumTo;
            int RampColorBlueThreshold = BlueColorNumFrom - BlueColorNumTo;
            double RampColorRedSteps = RampColorRedThreshold / RampFrameSpaces;
            double RampColorGreenSteps = RampColorGreenThreshold / RampFrameSpaces;
            double RampColorBlueSteps = RampColorBlueThreshold / RampFrameSpaces;
            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Set thresholds (RGB: {0};{1};{2})", RampColorRedThreshold, RampColorGreenThreshold, RampColorBlueThreshold);
            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Steps by {0} spaces (RGB: {1};{2};{3})", RampFrameSpaces, RampColorRedSteps, RampColorGreenSteps, RampColorBlueSteps);

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
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got subgradient color from (R;G;B: {0};{1};{2}) to (R;G;B: {3};{4};{5})", RampSubgradientRedColorNumFrom, RampSubgradientGreenColorNumFrom, RampSubgradientBlueColorNumFrom, RampSubgradientRedColorNumTo, RampSubgradientGreenColorNumTo, RampSubgradientBlueColorNumTo);

                // Set the sub-gradient current colors
                double RampSubgradientCurrentColorRed = RampSubgradientRedColorNumFrom;
                double RampSubgradientCurrentColorGreen = RampSubgradientGreenColorNumFrom;
                double RampSubgradientCurrentColorBlue = RampSubgradientBlueColorNumFrom;
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got subgradient current colors (R;G;B: {0};{1};{2})", RampSubgradientCurrentColorRed, RampSubgradientCurrentColorGreen, RampSubgradientCurrentColorBlue);

                // Set the sub-gradient thresholds
                int RampSubgradientColorRedThreshold = RampSubgradientRedColorNumFrom - RampSubgradientRedColorNumTo;
                int RampSubgradientColorGreenThreshold = RampSubgradientGreenColorNumFrom - RampSubgradientGreenColorNumTo;
                int RampSubgradientColorBlueThreshold = RampSubgradientBlueColorNumFrom - RampSubgradientBlueColorNumTo;
                double RampSubgradientColorRedSteps = RampSubgradientColorRedThreshold / (double)RampFrameSpaces;
                double RampSubgradientColorGreenSteps = RampSubgradientColorGreenThreshold / (double)RampFrameSpaces;
                double RampSubgradientColorBlueSteps = RampSubgradientColorBlueThreshold / (double)RampFrameSpaces;
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Set subgradient thresholds (RGB: {0};{1};{2})", RampSubgradientColorRedThreshold, RampSubgradientColorGreenThreshold, RampSubgradientColorBlueThreshold);
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Steps by {0} spaces for subgradient (RGB: {1};{2};{3})", RampFrameSpaces, RampSubgradientColorRedSteps, RampSubgradientColorGreenSteps, RampSubgradientColorBlueSteps);

                // Make a new instance
                var RampSubgradientCurrentColorInstance = new Color($"{Convert.ToInt32(RampSubgradientCurrentColorRed)};{Convert.ToInt32(RampSubgradientCurrentColorGreen)};{Convert.ToInt32(RampSubgradientCurrentColorBlue)}");
                ColorTools.SetConsoleColor(RampSubgradientCurrentColorInstance, true, true);

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
                    DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got new subgradient current colors (R;G;B: {0};{1};{2}) subtracting from {3};{4};{5}", RampSubgradientCurrentColorRed, RampSubgradientCurrentColorGreen, RampSubgradientCurrentColorBlue, RampSubgradientColorRedSteps, RampSubgradientColorGreenSteps, RampSubgradientColorBlueSteps);
                    RampSubgradientCurrentColorInstance = new Color($"{Convert.ToInt32(RampSubgradientCurrentColorRed)};{Convert.ToInt32(RampSubgradientCurrentColorGreen)};{Convert.ToInt32(RampSubgradientCurrentColorBlue)}");
                    ColorTools.SetConsoleColor(RampSubgradientCurrentColorInstance, true, true);
                }

                // Change the colors
                RampCurrentColorRed -= RampColorRedSteps;
                RampCurrentColorGreen -= RampColorGreenSteps;
                RampCurrentColorBlue -= RampColorBlueSteps;
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got new current colors (R;G;B: {0};{1};{2}) subtracting from {3};{4};{5}", RampCurrentColorRed, RampCurrentColorGreen, RampCurrentColorBlue, RampColorRedSteps, RampColorGreenSteps, RampColorBlueSteps);

                // Delay writing
                RampCurrentPositionLeft = 0;
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Current left position: {0}", RampCurrentPositionLeft);
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
