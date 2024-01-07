//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using Nitrocid.ConsoleBase;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers.FancyWriters;
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Threading;
using Nitrocid.Misc.Reflection;
using Nitrocid.Misc.Screensaver;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Settings for StackBox
    /// </summary>
    public static class StackBoxSettings
    {

        /// <summary>
        /// [StackBox] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool StackBoxTrueColor
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.StackBoxTrueColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.StackBoxTrueColor = value;
            }
        }
        /// <summary>
        /// [StackBox] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int StackBoxDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.StackBoxDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                ScreensaverPackInit.SaversConfig.StackBoxDelay = value;
            }
        }
        /// <summary>
        /// [StackBox] Whether to fill in the boxes drawn, or only draw the outline
        /// </summary>
        public static bool StackBoxFill
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.StackBoxFill;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.StackBoxFill = value;
            }
        }
        /// <summary>
        /// [StackBox] The minimum red color level (true color)
        /// </summary>
        public static int StackBoxMinimumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.StackBoxMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.StackBoxMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [StackBox] The minimum green color level (true color)
        /// </summary>
        public static int StackBoxMinimumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.StackBoxMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.StackBoxMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [StackBox] The minimum blue color level (true color)
        /// </summary>
        public static int StackBoxMinimumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.StackBoxMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.StackBoxMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [StackBox] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int StackBoxMinimumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.StackBoxMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                ScreensaverPackInit.SaversConfig.StackBoxMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [StackBox] The maximum red color level (true color)
        /// </summary>
        public static int StackBoxMaximumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.StackBoxMaximumRedColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.StackBoxMinimumRedColorLevel)
                    value = ScreensaverPackInit.SaversConfig.StackBoxMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.StackBoxMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [StackBox] The maximum green color level (true color)
        /// </summary>
        public static int StackBoxMaximumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.StackBoxMaximumGreenColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.StackBoxMinimumGreenColorLevel)
                    value = ScreensaverPackInit.SaversConfig.StackBoxMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.StackBoxMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [StackBox] The maximum blue color level (true color)
        /// </summary>
        public static int StackBoxMaximumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.StackBoxMaximumBlueColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.StackBoxMinimumBlueColorLevel)
                    value = ScreensaverPackInit.SaversConfig.StackBoxMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.StackBoxMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [StackBox] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int StackBoxMaximumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.StackBoxMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= ScreensaverPackInit.SaversConfig.StackBoxMinimumColorLevel)
                    value = ScreensaverPackInit.SaversConfig.StackBoxMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                ScreensaverPackInit.SaversConfig.StackBoxMaximumColorLevel = value;
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
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;
            if (ConsoleResizeListener.WasResized(false))
            {
                ColorTools.LoadBack(new Color(ConsoleColors.Black));

                // Reset resize sync
                ConsoleResizeListener.WasResized();
            }
            else
            {
                bool Drawable = true;

                // Get the required positions for the box
                int BoxStartX = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
                int BoxEndX = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Box X position {0} -> {1}", BoxStartX, BoxEndX);
                int BoxStartY = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
                int BoxEndY = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Box Y position {0} -> {1}", BoxStartY, BoxEndY);

                // Check to see if start is less than or equal to end
                BoxStartX.SwapIfSourceLarger(ref BoxEndX);
                BoxStartY.SwapIfSourceLarger(ref BoxEndY);
                if (BoxStartX == BoxEndX | BoxStartY == BoxEndY)
                {
                    // Don't draw; it won't be shown anyways
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Asking StackBox not to draw. Consult above two lines.");
                    Drawable = false;
                }

                if (Drawable)
                {
                    Color color;

                    // Select color
                    if (StackBoxSettings.StackBoxTrueColor)
                    {
                        int RedColorNum = RandomDriver.Random(StackBoxSettings.StackBoxMinimumRedColorLevel, StackBoxSettings.StackBoxMaximumRedColorLevel);
                        int GreenColorNum = RandomDriver.Random(StackBoxSettings.StackBoxMinimumGreenColorLevel, StackBoxSettings.StackBoxMaximumGreenColorLevel);
                        int BlueColorNum = RandomDriver.Random(StackBoxSettings.StackBoxMinimumBlueColorLevel, StackBoxSettings.StackBoxMaximumBlueColorLevel);
                        DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                        color = new Color($"{RedColorNum};{GreenColorNum};{BlueColorNum}");
                    }
                    else
                    {
                        int ColorNum = RandomDriver.Random(StackBoxSettings.StackBoxMinimumColorLevel, StackBoxSettings.StackBoxMaximumColorLevel);
                        DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
                        color = new Color(ColorNum);
                    }

                    // Draw the box
                    if (StackBoxSettings.StackBoxFill)
                        BoxColor.WriteBox(BoxStartX, BoxStartY, BoxEndX - BoxStartX, BoxEndY - BoxStartY, color);
                    else
                        BoxFrameColor.WriteBoxFrame(BoxStartX, BoxStartY, BoxEndX - BoxStartX, BoxEndY - BoxStartY, color);
                }
            }
            ThreadManager.SleepNoBlock(StackBoxSettings.StackBoxDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
