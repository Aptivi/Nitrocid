
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
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.Drivers.RNG;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Kernel.Threading;
using Terminaux.Colors;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for Wipe
    /// </summary>
    public static class WipeSettings
    {

        /// <summary>
        /// [Wipe] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool WipeTrueColor
        {
            get
            {
                return Config.SaverConfig.WipeTrueColor;
            }
            set
            {
                Config.SaverConfig.WipeTrueColor = value;
            }
        }
        /// <summary>
        /// [Wipe] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int WipeDelay
        {
            get
            {
                return Config.SaverConfig.WipeDelay;
            }
            set
            {
                Config.SaverConfig.WipeDelay = value;
            }
        }
        /// <summary>
        /// [Wipe] How many wipes needed to change direction?
        /// </summary>
        public static int WipeWipesNeededToChangeDirection
        {
            get
            {
                return Config.SaverConfig.WipeWipesNeededToChangeDirection;
            }
            set
            {
                Config.SaverConfig.WipeWipesNeededToChangeDirection = value;
            }
        }
        /// <summary>
        /// [Wipe] Screensaver background color
        /// </summary>
        public static string WipeBackgroundColor
        {
            get
            {
                return Config.SaverConfig.WipeBackgroundColor;
            }
            set
            {
                Config.SaverConfig.WipeBackgroundColor = value;
            }
        }
        /// <summary>
        /// [Wipe] The minimum red color level (true color)
        /// </summary>
        public static int WipeMinimumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.WipeMinimumRedColorLevel;
            }
            set
            {
                Config.SaverConfig.WipeMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Wipe] The minimum green color level (true color)
        /// </summary>
        public static int WipeMinimumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.WipeMinimumGreenColorLevel;
            }
            set
            {
                Config.SaverConfig.WipeMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Wipe] The minimum blue color level (true color)
        /// </summary>
        public static int WipeMinimumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.WipeMinimumBlueColorLevel;
            }
            set
            {
                Config.SaverConfig.WipeMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Wipe] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int WipeMinimumColorLevel
        {
            get
            {
                return Config.SaverConfig.WipeMinimumColorLevel;
            }
            set
            {
                Config.SaverConfig.WipeMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Wipe] The maximum red color level (true color)
        /// </summary>
        public static int WipeMaximumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.WipeMaximumRedColorLevel;
            }
            set
            {
                Config.SaverConfig.WipeMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Wipe] The maximum green color level (true color)
        /// </summary>
        public static int WipeMaximumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.WipeMaximumGreenColorLevel;
            }
            set
            {
                Config.SaverConfig.WipeMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Wipe] The maximum blue color level (true color)
        /// </summary>
        public static int WipeMaximumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.WipeMaximumBlueColorLevel;
            }
            set
            {
                Config.SaverConfig.WipeMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Wipe] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int WipeMaximumColorLevel
        {
            get
            {
                return Config.SaverConfig.WipeMaximumColorLevel;
            }
            set
            {
                Config.SaverConfig.WipeMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for Wipe
    /// </summary>
    public class WipeDisplay : BaseScreensaver, IScreensaver
    {

        private WipeDirections ToDirection = WipeDirections.Right;
        private int TimesWiped = 0;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Wipe";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            KernelColorTools.LoadBack(new Color(WipeSettings.WipeBackgroundColor), true);
            ConsoleWrapper.ForegroundColor = ConsoleColor.White;
            ConsoleWrapper.CursorVisible = false;
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Select a color
            if (WipeSettings.WipeTrueColor)
            {
                int RedColorNum = RandomDriver.Random(WipeSettings.WipeMinimumRedColorLevel, WipeSettings.WipeMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(WipeSettings.WipeMinimumGreenColorLevel, WipeSettings.WipeMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(WipeSettings.WipeMinimumBlueColorLevel, WipeSettings.WipeMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                if (!ConsoleResizeListener.WasResized(false))
                    KernelColorTools.SetConsoleColor(new Color($"{RedColorNum};{GreenColorNum};{BlueColorNum}"), true, true);
            }
            else
            {
                int ColorNum = RandomDriver.Random(WipeSettings.WipeMinimumColorLevel, WipeSettings.WipeMaximumColorLevel);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
                if (!ConsoleResizeListener.WasResized(false))
                    KernelColorTools.SetConsoleColor(new Color(ColorNum), true, true);
            }

            // Set max height
            int MaxWindowHeight = ConsoleWrapper.WindowHeight - 1;
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Max height {0}", MaxWindowHeight);

            // Print a space {Column} times until the entire screen is wiped.
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Wipe direction {0}", ToDirection.ToString());
            switch (ToDirection)
            {
                case WipeDirections.Right:
                    {
                        for (int Column = 0; Column <= ConsoleWrapper.WindowWidth; Column++)
                        {
                            if (ConsoleResizeListener.WasResized(false))
                                break;
                            for (int Row = 0; Row <= MaxWindowHeight; Row++)
                            {
                                if (ConsoleResizeListener.WasResized(false))
                                    break;

                                // Do the actual writing
                                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Setting Y position to {0}", Row);
                                ConsoleWrapper.SetCursorPosition(0, Row);
                                ConsoleWrapper.Write(new string(' ', Column));
                                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Written blanks {0} times", Column);
                            }
                            ThreadManager.SleepNoBlock(WipeSettings.WipeDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }

                        break;
                    }
                case WipeDirections.Left:
                    {
                        for (int Column = ConsoleWrapper.WindowWidth; Column >= 1; Column -= 1)
                        {
                            if (ConsoleResizeListener.WasResized(false))
                                break;
                            for (int Row = 0; Row <= MaxWindowHeight; Row++)
                            {
                                if (ConsoleResizeListener.WasResized(false))
                                    break;

                                // Do the actual writing
                                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Setting position to {0}", Column - 1, Row);
                                ConsoleWrapper.SetCursorPosition(Column - 1, Row);
                                ConsoleWrapper.Write(new string(' ', ConsoleWrapper.WindowWidth - Column + 1));
                                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Written blanks {0} times", ConsoleWrapper.WindowWidth - Column + 1);
                            }
                            ThreadManager.SleepNoBlock(WipeSettings.WipeDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }

                        break;
                    }
                case WipeDirections.Top:
                    {
                        for (int Row = MaxWindowHeight; Row >= 0; Row -= 1)
                        {
                            if (ConsoleResizeListener.WasResized(false))
                                break;

                            // Do the actual writing
                            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Setting Y position to {0}", Row);
                            ConsoleWrapper.SetCursorPosition(0, Row);
                            ConsoleWrapper.Write(new string(' ', ConsoleWrapper.WindowWidth));
                            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Written blanks {0} times", ConsoleWrapper.WindowWidth);
                            ThreadManager.SleepNoBlock(WipeSettings.WipeDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }

                        break;
                    }
                case WipeDirections.Bottom:
                    {
                        for (int Row = 0; Row <= MaxWindowHeight; Row++)
                        {
                            if (ConsoleResizeListener.WasResized(false))
                                break;

                            // Do the actual writing
                            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Written blanks {0} times", ConsoleWrapper.WindowWidth);
                            ConsoleWrapper.Write(new string(' ', ConsoleWrapper.WindowWidth));
                            ThreadManager.SleepNoBlock(WipeSettings.WipeDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        ConsoleWrapper.SetCursorPosition(0, 0);
                        break;
                    }
            }

            if (!ConsoleResizeListener.WasResized(false))
            {
                TimesWiped += 1;
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Wiped {0} times out of {1}", TimesWiped, WipeSettings.WipeWipesNeededToChangeDirection);

                // Check if the number of times wiped is equal to the number of required times to change wiping direction.
                if (TimesWiped == WipeSettings.WipeWipesNeededToChangeDirection)
                {
                    TimesWiped = 0;
                    ToDirection = (WipeDirections)Convert.ToInt32(Enum.Parse(typeof(WipeDirections), RandomDriver.Random(3).ToString()));
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Changed direction to {0}", ToDirection.ToString());
                }
            }
            else
            {
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.W, "Resize-syncing. Clearing...");
                KernelColorTools.LoadBack(new Color(WipeSettings.WipeBackgroundColor), true);
            }

            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(WipeSettings.WipeDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

        /// <summary>
        /// Wipe directions
        /// </summary>
        private enum WipeDirections
        {
            /// <summary>
            /// Wipe from right to left
            /// </summary>
            Left,
            /// <summary>
            /// Wipe from left to right
            /// </summary>
            Right,
            /// <summary>
            /// Wipe from bottom to top
            /// </summary>
            Top,
            /// <summary>
            /// Wipe from top to bottom
            /// </summary>
            Bottom
        }

    }
}
