
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

using System.Collections.Generic;
using ColorSeq;
using KS.ConsoleBase;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Drivers.RNG;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Kernel.Threading;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for Mesmerize
    /// </summary>
    public static class MesmerizeSettings
    {

        /// <summary>
        /// [Mesmerize] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int MesmerizeDelay
        {
            get
            {
                return Config.SaverConfig.MesmerizeDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                Config.SaverConfig.MesmerizeDelay = value;
            }
        }
        /// <summary>
        /// [Mesmerize] The minimum red color level (true color)
        /// </summary>
        public static int MesmerizeMinimumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.MesmerizeMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.MesmerizeMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Mesmerize] The minimum green color level (true color)
        /// </summary>
        public static int MesmerizeMinimumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.MesmerizeMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.MesmerizeMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Mesmerize] The minimum blue color level (true color)
        /// </summary>
        public static int MesmerizeMinimumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.MesmerizeMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.MesmerizeMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Mesmerize] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int MesmerizeMinimumColorLevel
        {
            get
            {
                return Config.SaverConfig.MesmerizeMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                Config.SaverConfig.MesmerizeMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Mesmerize] The maximum red color level (true color)
        /// </summary>
        public static int MesmerizeMaximumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.MesmerizeMaximumRedColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.MesmerizeMaximumRedColorLevel)
                    value = Config.SaverConfig.MesmerizeMaximumRedColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.MesmerizeMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Mesmerize] The maximum green color level (true color)
        /// </summary>
        public static int MesmerizeMaximumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.MesmerizeMaximumGreenColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.MesmerizeMaximumGreenColorLevel)
                    value = Config.SaverConfig.MesmerizeMaximumGreenColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.MesmerizeMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Mesmerize] The maximum blue color level (true color)
        /// </summary>
        public static int MesmerizeMaximumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.MesmerizeMaximumBlueColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.MesmerizeMaximumBlueColorLevel)
                    value = Config.SaverConfig.MesmerizeMaximumBlueColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.MesmerizeMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Mesmerize] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int MesmerizeMaximumColorLevel
        {
            get
            {
                return Config.SaverConfig.MesmerizeMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= Config.SaverConfig.MesmerizeMaximumColorLevel)
                    value = Config.SaverConfig.MesmerizeMaximumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                Config.SaverConfig.MesmerizeMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for Mesmerize
    /// </summary>
    public class MesmerizeDisplay : BaseScreensaver, IScreensaver
    {

        private enum DirectionMode
        {
            CenterMiddleToCenterRight,
            CenterRightToLowBottom,
            LowBottomToLowMiddle,
            LowMiddleToCenterMiddle,
            CenterMiddleToTopMiddle,
            TopMiddleToTopLeft,
            TopLeftToCenterLeft,
            CenterLeftToCenterMiddle,
        }

        private (int, int) dotCurrentPosition;
        private DirectionMode dotDirection = DirectionMode.CenterMiddleToCenterRight;
        private Color dotColor = Color.Empty;
        private readonly List<Color> dotColorShades = new();
        private readonly List<(int, int)> dotPositions = new();

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Mesmerize";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            base.ScreensaverPreparation();

            // Reset dot positions
            dotPositions.Clear();
            dotCurrentPosition = (ConsoleWrapper.WindowWidth / 2,
                                  ConsoleWrapper.WindowHeight / 2);

            // Assign a color to the dot
            int RedColorNum = RandomDriver.Random(MesmerizeSettings.MesmerizeMinimumRedColorLevel, MesmerizeSettings.MesmerizeMaximumRedColorLevel);
            int GreenColorNum = RandomDriver.Random(MesmerizeSettings.MesmerizeMinimumGreenColorLevel, MesmerizeSettings.MesmerizeMaximumGreenColorLevel);
            int BlueColorNum = RandomDriver.Random(MesmerizeSettings.MesmerizeMinimumBlueColorLevel, MesmerizeSettings.MesmerizeMaximumBlueColorLevel);
            DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
            dotColor = new Color(RedColorNum, GreenColorNum, BlueColorNum);

            // Assign shades of color
            int maxPositions = ConsoleWrapper.WindowWidth / 2;
            for (int i = 0; i < maxPositions; i++)
            {
                int finalR = (int)(dotColor.R * ((maxPositions - i - 1) / (double)(maxPositions - 1)));
                int finalG = (int)(dotColor.G * ((maxPositions - i - 1) / (double)(maxPositions - 1)));
                int finalB = (int)(dotColor.B * ((maxPositions - i - 1) / (double)(maxPositions - 1)));
                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", finalR, finalG, finalB);
                Color colorShade = new(finalR, finalG, finalB);
                dotColorShades.Add(colorShade);
            }
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;
            if (!ConsoleResizeListener.WasResized(false))
            {
                // Setting maximum and minimum limits for console height and width
                int minLeft = 8;
                int midLeft = ConsoleWrapper.WindowWidth / 2;
                int maxLeft = ConsoleWrapper.WindowWidth - minLeft;
                int minTop = 2;
                int midTop = ConsoleWrapper.WindowHeight / 2;
                int maxTop = ConsoleWrapper.WindowHeight - minTop - 1;

                // For tails. We don't need more than midLeft covered positions.
                dotPositions.Add(dotCurrentPosition);
                if (dotPositions.Count > midLeft)
                    dotPositions.RemoveAt(0);

                // Now, iterate through all the dot positions to create a dot that will spin in two rectangles in opposing sides
                for (int i = 0; i < dotPositions.Count; i++)
                {
                    // Get the dot position and the shade color
                    int left = dotPositions[i].Item1;
                    int top = dotPositions[i].Item2;
                    Color finalDotColor = dotColorShades[dotPositions.Count - 1 - i];

                    // Just place the dot in its place.
                    TextWriterWhereColor.WriteWhere(" ", left, top, Color.Empty, finalDotColor);
                }

                // For positioning, change the dot position based on the state of movement
                switch (dotDirection)
                {
                    // Initial state. Moving from the middle of the center to the middle of the right edge.
                    case DirectionMode.CenterMiddleToCenterRight:
                        dotCurrentPosition.Item1++;
                        if (dotCurrentPosition.Item1 == maxLeft)
                            dotDirection++;
                        break;

                    // Moving from the middle of the right edge to the lower right corner.
                    case DirectionMode.CenterRightToLowBottom:
                        dotCurrentPosition.Item2++;
                        if (dotCurrentPosition.Item2 == maxTop)
                            dotDirection++;
                        break;

                    // Moving from the lower right corner to the middle of the bottom edge.
                    case DirectionMode.LowBottomToLowMiddle:
                        dotCurrentPosition.Item1--;
                        if (dotCurrentPosition.Item1 == midLeft)
                            dotDirection++;
                        break;

                    // Moving from the middle of the bottom edge to the middle of the center.
                    case DirectionMode.LowMiddleToCenterMiddle:
                        dotCurrentPosition.Item2--;
                        if (dotCurrentPosition.Item2 == midTop)
                            dotDirection++;
                        break;

                    // Moving from the middle of the center to the middle of the top edge.
                    case DirectionMode.CenterMiddleToTopMiddle:
                        dotCurrentPosition.Item2--;
                        if (dotCurrentPosition.Item2 == minTop)
                            dotDirection++;
                        break;

                    // Moving from the middle of the top edge to the upper left corner.
                    case DirectionMode.TopMiddleToTopLeft:
                        dotCurrentPosition.Item1--;
                        if (dotCurrentPosition.Item1 == minLeft)
                            dotDirection++;
                        break;

                    // Moving from the upper left corner to the middle of the left edge.
                    case DirectionMode.TopLeftToCenterLeft:
                        dotCurrentPosition.Item2++;
                        if (dotCurrentPosition.Item2 == midTop)
                            dotDirection++;
                        break;

                    // Moving from the middle of the left edge to the middle of the center.
                    case DirectionMode.CenterLeftToCenterMiddle:
                        dotCurrentPosition.Item1++;
                        if (dotCurrentPosition.Item1 == midLeft)
                            dotDirection = DirectionMode.CenterMiddleToCenterRight;
                        break;
                }
            }
            else
            {
                // Someone have resized the terminal window during screensaver display.
                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.W, "Resize-syncing. Re-initializing...");
                ScreensaverPreparation();
            }

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(MesmerizeSettings.MesmerizeDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
