
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

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for Gradient
    /// </summary>
    public static class GradientSettings
    {

        private static int _NextScreenDelay = 250;
        private static int _MinimumRedColorLevelStart = 0;
        private static int _MinimumGreenColorLevelStart = 0;
        private static int _MinimumBlueColorLevelStart = 0;
        private static int _MaximumRedColorLevelStart = 255;
        private static int _MaximumGreenColorLevelStart = 255;
        private static int _MaximumBlueColorLevelStart = 255;
        private static int _MinimumRedColorLevelEnd = 0;
        private static int _MinimumGreenColorLevelEnd = 0;
        private static int _MinimumBlueColorLevelEnd = 0;
        private static int _MaximumRedColorLevelEnd = 255;
        private static int _MaximumGreenColorLevelEnd = 255;
        private static int _MaximumBlueColorLevelEnd = 255;

        /// <summary>
        /// [Gradient] How many milliseconds to wait before rotting the next screen?
        /// </summary>
        public static int GradientNextRampDelay
        {
            get
            {
                return _NextScreenDelay;
            }
            set
            {
                if (value <= 0)
                    value = 250;
                _NextScreenDelay = value;
            }
        }
        /// <summary>
        /// [Gradient] The minimum red color level (true color - start)
        /// </summary>
        public static int GradientMinimumRedColorLevelStart
        {
            get
            {
                return _MinimumRedColorLevelStart;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _MinimumRedColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Gradient] The minimum green color level (true color - start)
        /// </summary>
        public static int GradientMinimumGreenColorLevelStart
        {
            get
            {
                return _MinimumGreenColorLevelStart;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _MinimumGreenColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Gradient] The minimum blue color level (true color - start)
        /// </summary>
        public static int GradientMinimumBlueColorLevelStart
        {
            get
            {
                return _MinimumBlueColorLevelStart;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _MinimumBlueColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Gradient] The maximum red color level (true color - start)
        /// </summary>
        public static int GradientMaximumRedColorLevelStart
        {
            get
            {
                return _MaximumRedColorLevelStart;
            }
            set
            {
                if (value <= _MinimumRedColorLevelStart)
                    value = _MinimumRedColorLevelStart;
                if (value > 255)
                    value = 255;
                _MaximumRedColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Gradient] The maximum green color level (true color - start)
        /// </summary>
        public static int GradientMaximumGreenColorLevelStart
        {
            get
            {
                return _MaximumGreenColorLevelStart;
            }
            set
            {
                if (value <= _MinimumGreenColorLevelStart)
                    value = _MinimumGreenColorLevelStart;
                if (value > 255)
                    value = 255;
                _MaximumGreenColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Gradient] The maximum blue color level (true color - start)
        /// </summary>
        public static int GradientMaximumBlueColorLevelStart
        {
            get
            {
                return _MaximumBlueColorLevelStart;
            }
            set
            {
                if (value <= _MinimumBlueColorLevelStart)
                    value = _MinimumBlueColorLevelStart;
                if (value > 255)
                    value = 255;
                _MaximumBlueColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Gradient] The minimum red color level (true color - end)
        /// </summary>
        public static int GradientMinimumRedColorLevelEnd
        {
            get
            {
                return _MinimumRedColorLevelEnd;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _MinimumRedColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Gradient] The minimum green color level (true color - end)
        /// </summary>
        public static int GradientMinimumGreenColorLevelEnd
        {
            get
            {
                return _MinimumGreenColorLevelEnd;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _MinimumGreenColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Gradient] The minimum blue color level (true color - end)
        /// </summary>
        public static int GradientMinimumBlueColorLevelEnd
        {
            get
            {
                return _MinimumBlueColorLevelEnd;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _MinimumBlueColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Gradient] The maximum red color level (true color - end)
        /// </summary>
        public static int GradientMaximumRedColorLevelEnd
        {
            get
            {
                return _MaximumRedColorLevelEnd;
            }
            set
            {
                if (value <= _MinimumRedColorLevelEnd)
                    value = _MinimumRedColorLevelEnd;
                if (value > 255)
                    value = 255;
                _MaximumRedColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Gradient] The maximum green color level (true color - end)
        /// </summary>
        public static int GradientMaximumGreenColorLevelEnd
        {
            get
            {
                return _MaximumGreenColorLevelEnd;
            }
            set
            {
                if (value <= _MinimumGreenColorLevelEnd)
                    value = _MinimumGreenColorLevelEnd;
                if (value > 255)
                    value = 255;
                _MaximumGreenColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Gradient] The maximum blue color level (true color - end)
        /// </summary>
        public static int GradientMaximumBlueColorLevelEnd
        {
            get
            {
                return _MaximumBlueColorLevelEnd;
            }
            set
            {
                if (value <= _MinimumBlueColorLevelEnd)
                    value = _MinimumBlueColorLevelEnd;
                if (value > 255)
                    value = 255;
                _MaximumBlueColorLevelEnd = value;
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
            int RedColorNumFrom = RandomDriver.Random(GradientSettings.GradientMinimumRedColorLevelStart, GradientSettings.GradientMaximumRedColorLevelStart);
            int GreenColorNumFrom = RandomDriver.Random(GradientSettings.GradientMinimumGreenColorLevelStart, GradientSettings.GradientMaximumGreenColorLevelStart);
            int BlueColorNumFrom = RandomDriver.Random(GradientSettings.GradientMinimumBlueColorLevelStart, GradientSettings.GradientMaximumBlueColorLevelStart);
            int RedColorNumTo = RandomDriver.Random(GradientSettings.GradientMinimumRedColorLevelEnd, GradientSettings.GradientMaximumRedColorLevelEnd);
            int GreenColorNumTo = RandomDriver.Random(GradientSettings.GradientMinimumGreenColorLevelEnd, GradientSettings.GradientMaximumGreenColorLevelEnd);
            int BlueColorNumTo = RandomDriver.Random(GradientSettings.GradientMinimumBlueColorLevelEnd, GradientSettings.GradientMaximumBlueColorLevelEnd);
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

            // Fill the entire screen
            for (int x = 0; x < ConsoleWrapper.WindowWidth; x++)
            {
                if (ConsoleResizeListener.WasResized(false))
                    break;

                // Write the background gradient!
                var RampCurrentColorInstance = new Color($"{Convert.ToInt32(RampCurrentColorRed)};{Convert.ToInt32(RampCurrentColorGreen)};{Convert.ToInt32(RampCurrentColorBlue)}");
                for (int y = 0; y < ConsoleWrapper.WindowHeight; y++)
                    TextWriterWhereColor.WriteWhere(" ", x, y, Color.Empty, RampCurrentColorInstance);

                // Change the colors
                RampCurrentColorRed -= RampColorRedSteps;
                RampCurrentColorGreen -= RampColorGreenSteps;
                RampCurrentColorBlue -= RampColorBlueSteps;
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got new current colors (R;G;B: {0};{1};{2}) subtracting from {3};{4};{5}", RampCurrentColorRed, RampCurrentColorGreen, RampCurrentColorBlue, RampColorRedSteps, RampColorGreenSteps, RampColorBlueSteps);
            }

            // Clear the scene
            ThreadManager.SleepNoBlock(GradientSettings.GradientNextRampDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            ConsoleWrapper.BackgroundColor = ConsoleColor.Black;
            ConsoleWrapper.Clear();

            // Reset resize sync
            ConsoleResizeListener.WasResized();
        }

    }
}
