
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
using KS.Kernel.Configuration;
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

        /// <summary>
        /// [Gradient] How many milliseconds to wait before rotting the next screen?
        /// </summary>
        public static int GradientNextRampDelay
        {
            get
            {
                return Config.SaverConfig.GradientNextRampDelay;
            }
            set
            {
                if (value <= 0)
                    value = 250;
                Config.SaverConfig.GradientNextRampDelay = value;
            }
        }
        /// <summary>
        /// [Gradient] The minimum red color level (true color - start)
        /// </summary>
        public static int GradientMinimumRedColorLevelStart
        {
            get
            {
                return Config.SaverConfig.GradientMinimumRedColorLevelStart;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.GradientMinimumRedColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Gradient] The minimum green color level (true color - start)
        /// </summary>
        public static int GradientMinimumGreenColorLevelStart
        {
            get
            {
                return Config.SaverConfig.GradientMinimumGreenColorLevelStart;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.GradientMinimumGreenColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Gradient] The minimum blue color level (true color - start)
        /// </summary>
        public static int GradientMinimumBlueColorLevelStart
        {
            get
            {
                return Config.SaverConfig.GradientMinimumBlueColorLevelStart;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.GradientMinimumBlueColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Gradient] The maximum red color level (true color - start)
        /// </summary>
        public static int GradientMaximumRedColorLevelStart
        {
            get
            {
                return Config.SaverConfig.GradientMaximumRedColorLevelStart;
            }
            set
            {
                if (value <= Config.SaverConfig.GradientMinimumRedColorLevelStart)
                    value = Config.SaverConfig.GradientMinimumRedColorLevelStart;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.GradientMaximumRedColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Gradient] The maximum green color level (true color - start)
        /// </summary>
        public static int GradientMaximumGreenColorLevelStart
        {
            get
            {
                return Config.SaverConfig.GradientMaximumGreenColorLevelStart;
            }
            set
            {
                if (value <= Config.SaverConfig.GradientMinimumGreenColorLevelStart)
                    value = Config.SaverConfig.GradientMinimumGreenColorLevelStart;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.GradientMaximumGreenColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Gradient] The maximum blue color level (true color - start)
        /// </summary>
        public static int GradientMaximumBlueColorLevelStart
        {
            get
            {
                return Config.SaverConfig.GradientMaximumBlueColorLevelStart;
            }
            set
            {
                if (value <= Config.SaverConfig.GradientMinimumBlueColorLevelStart)
                    value = Config.SaverConfig.GradientMinimumBlueColorLevelStart;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.GradientMaximumBlueColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Gradient] The minimum red color level (true color - end)
        /// </summary>
        public static int GradientMinimumRedColorLevelEnd
        {
            get
            {
                return Config.SaverConfig.GradientMinimumRedColorLevelEnd;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.GradientMinimumRedColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Gradient] The minimum green color level (true color - end)
        /// </summary>
        public static int GradientMinimumGreenColorLevelEnd
        {
            get
            {
                return Config.SaverConfig.GradientMinimumGreenColorLevelEnd;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.GradientMinimumGreenColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Gradient] The minimum blue color level (true color - end)
        /// </summary>
        public static int GradientMinimumBlueColorLevelEnd
        {
            get
            {
                return Config.SaverConfig.GradientMinimumBlueColorLevelEnd;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.GradientMinimumBlueColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Gradient] The maximum red color level (true color - end)
        /// </summary>
        public static int GradientMaximumRedColorLevelEnd
        {
            get
            {
                return Config.SaverConfig.GradientMaximumRedColorLevelEnd;
            }
            set
            {
                if (value <= Config.SaverConfig.GradientMinimumRedColorLevelEnd)
                    value = Config.SaverConfig.GradientMinimumRedColorLevelEnd;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.GradientMaximumRedColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Gradient] The maximum green color level (true color - end)
        /// </summary>
        public static int GradientMaximumGreenColorLevelEnd
        {
            get
            {
                return Config.SaverConfig.GradientMaximumGreenColorLevelEnd;
            }
            set
            {
                if (value <= Config.SaverConfig.GradientMinimumGreenColorLevelEnd)
                    value = Config.SaverConfig.GradientMinimumGreenColorLevelEnd;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.GradientMaximumGreenColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Gradient] The maximum blue color level (true color - end)
        /// </summary>
        public static int GradientMaximumBlueColorLevelEnd
        {
            get
            {
                return Config.SaverConfig.GradientMaximumBlueColorLevelEnd;
            }
            set
            {
                if (value <= Config.SaverConfig.GradientMinimumBlueColorLevelEnd)
                    value = Config.SaverConfig.GradientMinimumBlueColorLevelEnd;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.GradientMaximumBlueColorLevelEnd = value;
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
            DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got color from (R;G;B: {0};{1};{2}) to (R;G;B: {3};{4};{5})", RedColorNumFrom, GreenColorNumFrom, BlueColorNumFrom, RedColorNumTo, GreenColorNumTo, BlueColorNumTo);

            // Set thresholds for color ramp
            int RampFrameSpaces = ConsoleWrapper.WindowWidth;
            int RampColorRedThreshold = RedColorNumFrom - RedColorNumTo;
            int RampColorGreenThreshold = GreenColorNumFrom - GreenColorNumTo;
            int RampColorBlueThreshold = BlueColorNumFrom - BlueColorNumTo;
            double RampColorRedSteps = RampColorRedThreshold / RampFrameSpaces;
            double RampColorGreenSteps = RampColorGreenThreshold / RampFrameSpaces;
            double RampColorBlueSteps = RampColorBlueThreshold / RampFrameSpaces;
            DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Set thresholds (RGB: {0};{1};{2})", RampColorRedThreshold, RampColorGreenThreshold, RampColorBlueThreshold);
            DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Steps by {0} spaces (RGB: {1};{2};{3})", RampFrameSpaces, RampColorRedSteps, RampColorGreenSteps, RampColorBlueSteps);

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
                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got new current colors (R;G;B: {0};{1};{2}) subtracting from {3};{4};{5}", RampCurrentColorRed, RampCurrentColorGreen, RampCurrentColorBlue, RampColorRedSteps, RampColorGreenSteps, RampColorBlueSteps);
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
