
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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

using System.Collections.Generic;
using ColorSeq;
using KS.ConsoleBase.Colors;
using KS.Drivers.RNG;
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Misc.Threading;
using KS.Misc.Writers.WriterBase;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for SnakeFill
    /// </summary>
    public static class SnakeFillSettings
    {

        private static bool _snakeFill255Colors;
        private static bool _snakeFillTrueColor = true;
        private static int _snakeFillDelay = 10;
        private static int _snakeFillMinimumRedColorLevel = 0;
        private static int _snakeFillMinimumGreenColorLevel = 0;
        private static int _snakeFillMinimumBlueColorLevel = 0;
        private static int _snakeFillMinimumColorLevel = 0;
        private static int _snakeFillMaximumRedColorLevel = 255;
        private static int _snakeFillMaximumGreenColorLevel = 255;
        private static int _snakeFillMaximumBlueColorLevel = 255;
        private static int _snakeFillMaximumColorLevel = 255;

        /// <summary>
        /// [SnakeFill] Enable 255 color support. Has a higher priority than 16 color support.
        /// </summary>
        public static bool SnakeFill255Colors
        {
            get
            {
                return _snakeFill255Colors;
            }
            set
            {
                _snakeFill255Colors = value;
            }
        }
        /// <summary>
        /// [SnakeFill] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool SnakeFillTrueColor
        {
            get
            {
                return _snakeFillTrueColor;
            }
            set
            {
                _snakeFillTrueColor = value;
            }
        }
        /// <summary>
        /// [SnakeFill] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int SnakeFillDelay
        {
            get
            {
                return _snakeFillDelay;
            }
            set
            {
                _snakeFillDelay = value;
            }
        }
        /// <summary>
        /// [SnakeFill] The minimum red color level (true color)
        /// </summary>
        public static int SnakeFillMinimumRedColorLevel
        {
            get
            {
                return _snakeFillMinimumRedColorLevel;
            }
            set
            {
                _snakeFillMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [SnakeFill] The minimum green color level (true color)
        /// </summary>
        public static int SnakeFillMinimumGreenColorLevel
        {
            get
            {
                return _snakeFillMinimumGreenColorLevel;
            }
            set
            {
                _snakeFillMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [SnakeFill] The minimum blue color level (true color)
        /// </summary>
        public static int SnakeFillMinimumBlueColorLevel
        {
            get
            {
                return _snakeFillMinimumBlueColorLevel;
            }
            set
            {
                _snakeFillMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [SnakeFill] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int SnakeFillMinimumColorLevel
        {
            get
            {
                return _snakeFillMinimumColorLevel;
            }
            set
            {
                _snakeFillMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [SnakeFill] The maximum red color level (true color)
        /// </summary>
        public static int SnakeFillMaximumRedColorLevel
        {
            get
            {
                return _snakeFillMaximumRedColorLevel;
            }
            set
            {
                _snakeFillMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [SnakeFill] The maximum green color level (true color)
        /// </summary>
        public static int SnakeFillMaximumGreenColorLevel
        {
            get
            {
                return _snakeFillMaximumGreenColorLevel;
            }
            set
            {
                _snakeFillMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [SnakeFill] The maximum blue color level (true color)
        /// </summary>
        public static int SnakeFillMaximumBlueColorLevel
        {
            get
            {
                return _snakeFillMaximumBlueColorLevel;
            }
            set
            {
                _snakeFillMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [SnakeFill] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int SnakeFillMaximumColorLevel
        {
            get
            {
                return _snakeFillMaximumColorLevel;
            }
            set
            {
                _snakeFillMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for SnakeFill
    /// </summary>
    public class SnakeFillDisplay : BaseScreensaver, IScreensaver
    {

        private int CurrentWindowWidth;
        private int CurrentWindowHeight;
        private bool ResizeSyncing;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "SnakeFill";

        /// <inheritdoc/>
        public override Dictionary<string, object> ScreensaverSettings { get; set; }

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            CurrentWindowWidth = ConsoleBase.ConsoleWrapper.WindowWidth;
            CurrentWindowHeight = ConsoleBase.ConsoleWrapper.WindowHeight;
            ConsoleBase.ConsoleWrapper.Clear();
            ConsoleBase.ConsoleWrapper.CursorVisible = false;
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleBase.ConsoleWrapper.CursorVisible = false;
            if (CurrentWindowHeight != ConsoleBase.ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleBase.ConsoleWrapper.WindowWidth)
                ResizeSyncing = true;

            // Select a color
            if (SnakeFillSettings.SnakeFillTrueColor)
            {
                int RedColorNum = RandomDriver.Random(SnakeFillSettings.SnakeFillMinimumRedColorLevel, SnakeFillSettings.SnakeFillMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(SnakeFillSettings.SnakeFillMinimumGreenColorLevel, SnakeFillSettings.SnakeFillMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(SnakeFillSettings.SnakeFillMinimumBlueColorLevel, SnakeFillSettings.SnakeFillMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                if (!ResizeSyncing)
                    ColorTools.SetConsoleColor(new Color($"{RedColorNum};{GreenColorNum};{BlueColorNum}"), true, true);
            }
            else if (SnakeFillSettings.SnakeFill255Colors)
            {
                int ColorNum = RandomDriver.Random(SnakeFillSettings.SnakeFillMinimumColorLevel, SnakeFillSettings.SnakeFillMaximumColorLevel);
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
                if (!ResizeSyncing)
                    ColorTools.SetConsoleColor(new Color(ColorNum), true, true);
            }
            else
            {
                if (!ResizeSyncing)
                    ConsoleBase.ConsoleWrapper.BackgroundColor = Screensaver.colors[RandomDriver.Random(SnakeFillSettings.SnakeFillMinimumColorLevel, SnakeFillSettings.SnakeFillMaximumColorLevel)];
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ConsoleBase.ConsoleWrapper.BackgroundColor);
            }

            // Set max height according to platform
            int MaxWindowHeight = ConsoleBase.ConsoleWrapper.WindowHeight;
            if (KernelPlatform.IsOnUnix())
                MaxWindowHeight -= 1;
            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Max height {0}", MaxWindowHeight);

            // Fill the screen!
            bool reverseHeightAxis = false;
            for (int x = 0; x < ConsoleBase.ConsoleWrapper.WindowWidth; x++)
            {
                if (CurrentWindowHeight != ConsoleBase.ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleBase.ConsoleWrapper.WindowWidth)
                    ResizeSyncing = true;
                if (ResizeSyncing)
                    break;

                // Select the height and fill the entire screen
                if (reverseHeightAxis)
                {
                    for (int y = MaxWindowHeight - 1; y >= 0; y--)
                    {
                        if (CurrentWindowHeight != ConsoleBase.ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleBase.ConsoleWrapper.WindowWidth)
                            ResizeSyncing = true;
                        if (ResizeSyncing)
                            break;

                        WriterPlainManager.CurrentPlain.WriteWherePlain(" ", x, y);
                        ThreadManager.SleepNoBlock(SnakeFillSettings.SnakeFillDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        reverseHeightAxis = false;
                    }
                }
                else
                {
                    for (int y = 0; y < MaxWindowHeight; y++)
                    {
                        if (CurrentWindowHeight != ConsoleBase.ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleBase.ConsoleWrapper.WindowWidth)
                            ResizeSyncing = true;
                        if (ResizeSyncing)
                            break;

                        WriterPlainManager.CurrentPlain.WriteWherePlain(" ", x, y);
                        ThreadManager.SleepNoBlock(SnakeFillSettings.SnakeFillDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        reverseHeightAxis = true;
                    }
                }
            }

            ResizeSyncing = false;
            CurrentWindowWidth = ConsoleBase.ConsoleWrapper.WindowWidth;
            CurrentWindowHeight = ConsoleBase.ConsoleWrapper.WindowHeight;
            ThreadManager.SleepNoBlock(SnakeFillSettings.SnakeFillDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
