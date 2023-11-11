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

using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.Drivers.RNG;
using KS.Kernel.Debugging;
using KS.Kernel.Threading;
using KS.Misc.Screensaver;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Settings for DoorShift
    /// </summary>
    public static class DoorShiftSettings
    {

        /// <summary>
        /// [DoorShift] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool DoorShiftTrueColor
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.DoorShiftTrueColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.DoorShiftTrueColor = value;
            }
        }
        /// <summary>
        /// [DoorShift] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int DoorShiftDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.DoorShiftDelay;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.DoorShiftDelay = value;
            }
        }
        /// <summary>
        /// [DoorShift] Screensaver background color
        /// </summary>
        public static string DoorShiftBackgroundColor
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.DoorShiftBackgroundColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.DoorShiftBackgroundColor = value;
            }
        }
        /// <summary>
        /// [DoorShift] The minimum red color level (true color)
        /// </summary>
        public static int DoorShiftMinimumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.DoorShiftMinimumRedColorLevel;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.DoorShiftMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [DoorShift] The minimum green color level (true color)
        /// </summary>
        public static int DoorShiftMinimumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.DoorShiftMinimumGreenColorLevel;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.DoorShiftMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [DoorShift] The minimum blue color level (true color)
        /// </summary>
        public static int DoorShiftMinimumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.DoorShiftMinimumBlueColorLevel;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.DoorShiftMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [DoorShift] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int DoorShiftMinimumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.DoorShiftMinimumColorLevel;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.DoorShiftMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [DoorShift] The maximum red color level (true color)
        /// </summary>
        public static int DoorShiftMaximumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.DoorShiftMaximumRedColorLevel;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.DoorShiftMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [DoorShift] The maximum green color level (true color)
        /// </summary>
        public static int DoorShiftMaximumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.DoorShiftMaximumGreenColorLevel;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.DoorShiftMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [DoorShift] The maximum blue color level (true color)
        /// </summary>
        public static int DoorShiftMaximumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.DoorShiftMaximumBlueColorLevel;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.DoorShiftMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [DoorShift] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int DoorShiftMaximumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.DoorShiftMaximumColorLevel;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.DoorShiftMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for DoorShift
    /// </summary>
    public class DoorShiftDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "DoorShift";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            KernelColorTools.LoadBack(new Color(DoorShiftSettings.DoorShiftBackgroundColor));
            ConsoleWrapper.CursorVisible = false;
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Whether the door is closing or opening
            bool isClosing = RandomDriver.RandomChance(30);

            // Select a color
            if (DoorShiftSettings.DoorShiftTrueColor)
            {
                int RedColorNum = RandomDriver.Random(DoorShiftSettings.DoorShiftMinimumRedColorLevel, DoorShiftSettings.DoorShiftMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(DoorShiftSettings.DoorShiftMinimumGreenColorLevel, DoorShiftSettings.DoorShiftMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(DoorShiftSettings.DoorShiftMinimumBlueColorLevel, DoorShiftSettings.DoorShiftMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                if (!ConsoleResizeListener.WasResized(false))
                    KernelColorTools.SetConsoleColor(new Color($"{RedColorNum};{GreenColorNum};{BlueColorNum}"), true);
            }
            else
            {
                int ColorNum = RandomDriver.Random(DoorShiftSettings.DoorShiftMinimumColorLevel, DoorShiftSettings.DoorShiftMaximumColorLevel);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
                if (!ConsoleResizeListener.WasResized(false))
                    KernelColorTools.SetConsoleColor(new Color(ColorNum), true);
            }

            // Set max height and width
            int MaxWindowHeight = ConsoleWrapper.WindowHeight - 1;
            int halfWidth = ConsoleWrapper.WindowWidth / 2;
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Max height {0}", MaxWindowHeight);
            if (isClosing)
            {
                for (int column = 0; column <= halfWidth; column++)
                {
                    if (ConsoleResizeListener.WasResized(false))
                        break;
                    for (int Row = 0; Row <= MaxWindowHeight; Row++)
                    {
                        if (ConsoleResizeListener.WasResized(false))
                            break;

                        // Check the positions
                        int leftDoorPos = column;
                        int rightDoorPos = ConsoleWrapper.WindowWidth - column - 1;
                        if (leftDoorPos > halfWidth)
                            leftDoorPos = halfWidth;
                        if (rightDoorPos < halfWidth)
                            rightDoorPos = halfWidth;

                        // Do the actual writing
                        DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Setting position to {0}", column - 1, Row);
                        ConsoleWrapper.SetCursorPosition(leftDoorPos, Row);
                        ConsoleWrapper.Write(" ");
                        ConsoleWrapper.SetCursorPosition(rightDoorPos, Row);
                        ConsoleWrapper.Write(" ");
                        DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Written blanks {0} times", ConsoleWrapper.WindowWidth - column + 1);
                    }
                    ThreadManager.SleepNoBlock(DoorShiftSettings.DoorShiftDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                }
            }
            else
            {
                for (int column = 0; column <= halfWidth; column++)
                {
                    if (ConsoleResizeListener.WasResized(false))
                        break;
                    for (int Row = 0; Row <= MaxWindowHeight; Row++)
                    {
                        if (ConsoleResizeListener.WasResized(false))
                            break;

                        // Check the positions
                        int leftDoorPos = halfWidth - column;
                        int rightDoorPos = halfWidth + column;
                        if (leftDoorPos < 0)
                            leftDoorPos = 0;
                        if (rightDoorPos >= ConsoleWrapper.WindowWidth)
                            rightDoorPos = ConsoleWrapper.WindowWidth - 1;

                        // Do the actual writing
                        DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Setting position to {0}", column - 1, Row);
                        ConsoleWrapper.SetCursorPosition(leftDoorPos, Row);
                        ConsoleWrapper.Write(" ");
                        ConsoleWrapper.SetCursorPosition(rightDoorPos, Row);
                        ConsoleWrapper.Write(" ");
                        DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Written blanks {0} times", ConsoleWrapper.WindowWidth - column + 1);
                    }
                    ThreadManager.SleepNoBlock(DoorShiftSettings.DoorShiftDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                }
            }

            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(DoorShiftSettings.DoorShiftDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
