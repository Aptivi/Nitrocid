
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

using ColorSeq;
using KS.ConsoleBase;
using KS.Drivers.RNG;
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using ColorTools = KS.ConsoleBase.Colors.ColorTools;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for SnakeFill
    /// </summary>
    public static class SnakeFillSettings
    {

        private static bool _TrueColor = true;
        private static int _Delay = 10;
        private static int _MinimumRedColorLevel = 0;
        private static int _MinimumGreenColorLevel = 0;
        private static int _MinimumBlueColorLevel = 0;
        private static int _MinimumColorLevel = 0;
        private static int _MaximumRedColorLevel = 255;
        private static int _MaximumGreenColorLevel = 255;
        private static int _MaximumBlueColorLevel = 255;
        private static int _MaximumColorLevel = 255;

        /// <summary>
        /// [SnakeFill] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool SnakeFillTrueColor
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
        /// [SnakeFill] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int SnakeFillDelay
        {
            get
            {
                return _Delay;
            }
            set
            {
                _Delay = value;
            }
        }
        /// <summary>
        /// [SnakeFill] The minimum red color level (true color)
        /// </summary>
        public static int SnakeFillMinimumRedColorLevel
        {
            get
            {
                return _MinimumRedColorLevel;
            }
            set
            {
                _MinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [SnakeFill] The minimum green color level (true color)
        /// </summary>
        public static int SnakeFillMinimumGreenColorLevel
        {
            get
            {
                return _MinimumGreenColorLevel;
            }
            set
            {
                _MinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [SnakeFill] The minimum blue color level (true color)
        /// </summary>
        public static int SnakeFillMinimumBlueColorLevel
        {
            get
            {
                return _MinimumBlueColorLevel;
            }
            set
            {
                _MinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [SnakeFill] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int SnakeFillMinimumColorLevel
        {
            get
            {
                return _MinimumColorLevel;
            }
            set
            {
                _MinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [SnakeFill] The maximum red color level (true color)
        /// </summary>
        public static int SnakeFillMaximumRedColorLevel
        {
            get
            {
                return _MaximumRedColorLevel;
            }
            set
            {
                _MaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [SnakeFill] The maximum green color level (true color)
        /// </summary>
        public static int SnakeFillMaximumGreenColorLevel
        {
            get
            {
                return _MaximumGreenColorLevel;
            }
            set
            {
                _MaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [SnakeFill] The maximum blue color level (true color)
        /// </summary>
        public static int SnakeFillMaximumBlueColorLevel
        {
            get
            {
                return _MaximumBlueColorLevel;
            }
            set
            {
                _MaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [SnakeFill] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int SnakeFillMaximumColorLevel
        {
            get
            {
                return _MaximumColorLevel;
            }
            set
            {
                _MaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for SnakeFill
    /// </summary>
    public class SnakeFillDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "SnakeFill";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ConsoleWrapper.Clear();
            ConsoleWrapper.CursorVisible = false;
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Select a color
            if (SnakeFillSettings.SnakeFillTrueColor)
            {
                int RedColorNum = RandomDriver.Random(SnakeFillSettings.SnakeFillMinimumRedColorLevel, SnakeFillSettings.SnakeFillMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(SnakeFillSettings.SnakeFillMinimumGreenColorLevel, SnakeFillSettings.SnakeFillMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(SnakeFillSettings.SnakeFillMinimumBlueColorLevel, SnakeFillSettings.SnakeFillMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                if (!ConsoleResizeListener.WasResized(false))
                    ColorTools.SetConsoleColor(new Color($"{RedColorNum};{GreenColorNum};{BlueColorNum}"), true, true);
            }
            else
            {
                int ColorNum = RandomDriver.Random(SnakeFillSettings.SnakeFillMinimumColorLevel, SnakeFillSettings.SnakeFillMaximumColorLevel);
                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
                if (!ConsoleResizeListener.WasResized(false))
                    ColorTools.SetConsoleColor(new Color(ColorNum), true, true);
            }

            // Set max height
            int MaxWindowHeight = ConsoleWrapper.WindowHeight - 1;
            DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Max height {0}", MaxWindowHeight);

            // Fill the screen!
            bool reverseHeightAxis = false;
            for (int x = 0; x < ConsoleWrapper.WindowWidth; x++)
            {
                if (ConsoleResizeListener.WasResized(false))
                    break;

                // Select the height and fill the entire screen
                if (reverseHeightAxis)
                {
                    for (int y = MaxWindowHeight; y >= 0; y--)
                    {
                        if (ConsoleResizeListener.WasResized(false))
                            break;

                        TextWriterWhereColor.WriteWhere(" ", x, y);
                        ThreadManager.SleepNoBlock(SnakeFillSettings.SnakeFillDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        reverseHeightAxis = false;
                    }
                }
                else
                {
                    for (int y = 0; y <= MaxWindowHeight; y++)
                    {
                        if (ConsoleResizeListener.WasResized(false))
                            break;

                        TextWriterWhereColor.WriteWhere(" ", x, y);
                        ThreadManager.SleepNoBlock(SnakeFillSettings.SnakeFillDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        reverseHeightAxis = true;
                    }
                }
            }

            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(SnakeFillSettings.SnakeFillDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
