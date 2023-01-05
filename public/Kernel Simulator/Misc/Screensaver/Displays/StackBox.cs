
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
using Extensification.IntegerExts;
using KS.ConsoleBase;
using KS.Drivers.RNG;
using KS.Kernel.Debugging;
using KS.Misc.Threading;
using ColorTools = KS.ConsoleBase.Colors.ColorTools;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for StackBox
    /// </summary>
    public static class StackBoxSettings
    {

        private static bool _TrueColor = true;
        private static int _Delay = 10;
        private static bool _Fill = true;
        private static int _MinimumRedColorLevel = 0;
        private static int _MinimumGreenColorLevel = 0;
        private static int _MinimumBlueColorLevel = 0;
        private static int _MinimumColorLevel = 0;
        private static int _MaximumRedColorLevel = 255;
        private static int _MaximumGreenColorLevel = 255;
        private static int _MaximumBlueColorLevel = 255;
        private static int _MaximumColorLevel = 255;

        /// <summary>
        /// [StackBox] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool StackBoxTrueColor
        {
            get
            {
                return _TrueColor;
            }
            set
            {
                _TrueColor = value;
            }
        }
        /// <summary>
        /// [StackBox] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int StackBoxDelay
        {
            get
            {
                return _Delay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                _Delay = value;
            }
        }
        /// <summary>
        /// [StackBox] Whether to fill in the boxes drawn, or only draw the outline
        /// </summary>
        public static bool StackBoxFill
        {
            get
            {
                return _Fill;
            }
            set
            {
                _Fill = value;
            }
        }
        /// <summary>
        /// [StackBox] The minimum red color level (true color)
        /// </summary>
        public static int StackBoxMinimumRedColorLevel
        {
            get
            {
                return _MinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _MinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [StackBox] The minimum green color level (true color)
        /// </summary>
        public static int StackBoxMinimumGreenColorLevel
        {
            get
            {
                return _MinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _MinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [StackBox] The minimum blue color level (true color)
        /// </summary>
        public static int StackBoxMinimumBlueColorLevel
        {
            get
            {
                return _MinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _MinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [StackBox] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int StackBoxMinimumColorLevel
        {
            get
            {
                return _MinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                _MinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [StackBox] The maximum red color level (true color)
        /// </summary>
        public static int StackBoxMaximumRedColorLevel
        {
            get
            {
                return _MaximumRedColorLevel;
            }
            set
            {
                if (value <= _MinimumRedColorLevel)
                    value = _MinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                _MaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [StackBox] The maximum green color level (true color)
        /// </summary>
        public static int StackBoxMaximumGreenColorLevel
        {
            get
            {
                return _MaximumGreenColorLevel;
            }
            set
            {
                if (value <= _MinimumGreenColorLevel)
                    value = _MinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                _MaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [StackBox] The maximum blue color level (true color)
        /// </summary>
        public static int StackBoxMaximumBlueColorLevel
        {
            get
            {
                return _MaximumBlueColorLevel;
            }
            set
            {
                if (value <= _MinimumBlueColorLevel)
                    value = _MinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                _MaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [StackBox] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int StackBoxMaximumColorLevel
        {
            get
            {
                return _MaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= _MinimumColorLevel)
                    value = _MinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                _MaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for StackBox
    /// </summary>
    public class StackBoxDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "StackBox";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ConsoleWrapper.BackgroundColor = ConsoleColor.Black;
            ConsoleWrapper.Clear();
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;
            if (ConsoleResizeListener.WasResized(false))
            {
                ConsoleWrapper.BackgroundColor = ConsoleColor.Black;
                ConsoleWrapper.Clear();

                // Reset resize sync
                ConsoleResizeListener.WasResized();
            }
            else
            {
                bool Drawable = true;

                // Get the required positions for the box
                int BoxStartX = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
                int BoxEndX = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Box X position {0} -> {1}", BoxStartX, BoxEndX);
                int BoxStartY = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
                int BoxEndY = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Box Y position {0} -> {1}", BoxStartY, BoxEndY);

                // Check to see if start is less than or equal to end
                BoxStartX.SwapIfSourceLarger(ref BoxEndX);
                BoxStartY.SwapIfSourceLarger(ref BoxEndY);
                if (BoxStartX == BoxEndX | BoxStartY == BoxEndY)
                {
                    // Don't draw; it won't be shown anyways
                    DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Asking StackBox not to draw. Consult above two lines.");
                    Drawable = false;
                }

                if (Drawable)
                {
                    // Select color
                    if (StackBoxSettings.StackBoxTrueColor)
                    {
                        int RedColorNum = RandomDriver.Random(StackBoxSettings.StackBoxMinimumRedColorLevel, StackBoxSettings.StackBoxMaximumRedColorLevel);
                        int GreenColorNum = RandomDriver.Random(StackBoxSettings.StackBoxMinimumGreenColorLevel, StackBoxSettings.StackBoxMaximumGreenColorLevel);
                        int BlueColorNum = RandomDriver.Random(StackBoxSettings.StackBoxMinimumBlueColorLevel, StackBoxSettings.StackBoxMaximumBlueColorLevel);
                        DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                        ColorTools.SetConsoleColor(new Color($"{RedColorNum};{GreenColorNum};{BlueColorNum}"), true, true);
                    }
                    else
                    {
                        int ColorNum = RandomDriver.Random(StackBoxSettings.StackBoxMinimumColorLevel, StackBoxSettings.StackBoxMaximumColorLevel);
                        DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
                        ColorTools.SetConsoleColor(new Color(ColorNum), true, true);
                    }

                    // Draw the box
                    if (StackBoxSettings.StackBoxFill)
                    {
                        // Cover all the positions
                        for (int X = BoxStartX; X <= BoxEndX; X++)
                        {
                            for (int Y = BoxStartY; Y <= BoxEndY; Y++)
                            {
                                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Filling {0},{1}...", X, Y);
                                ConsoleWrapper.SetCursorPosition(X, Y);
                                ConsoleWrapper.Write(" ");
                            }
                        }
                    }
                    else
                    {
                        // Draw the upper and lower borders
                        for (int X = BoxStartX; X <= BoxEndX; X++)
                        {
                            ConsoleWrapper.SetCursorPosition(X, BoxStartY);
                            ConsoleWrapper.Write(" ");
                            DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Drawn upper border at {0}", X);
                            ConsoleWrapper.SetCursorPosition(X, BoxEndY);
                            ConsoleWrapper.Write(" ");
                            DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Drawn lower border at {0}", X);
                        }

                        // Draw the left and right borders
                        for (int Y = BoxStartY; Y <= BoxEndY; Y++)
                        {
                            ConsoleWrapper.SetCursorPosition(BoxStartX, Y);
                            ConsoleWrapper.Write(" ");
                            if (!(BoxStartX >= ConsoleWrapper.WindowWidth - 1))
                                ConsoleWrapper.Write(" ");
                            DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Drawn left border at {0}", Y);
                            ConsoleWrapper.SetCursorPosition(BoxEndX, Y);
                            ConsoleWrapper.Write(" ");
                            if (!(BoxEndX >= ConsoleWrapper.WindowWidth - 1))
                                ConsoleWrapper.Write(" ");
                            DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Drawn right border at {0}", Y);
                        }
                    }
                }
            }
            ThreadManager.SleepNoBlock(StackBoxSettings.StackBoxDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
