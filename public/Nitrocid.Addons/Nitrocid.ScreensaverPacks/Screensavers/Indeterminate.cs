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

using System;
using Terminaux.Colors;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Misc.Screensaver;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Drivers.RNG;
using Terminaux.Writer.FancyWriters;
using Nitrocid.Kernel.Threading;
using Terminaux.Base;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Settings for Indeterminate
    /// </summary>
    public static class IndeterminateSettings
    {

        /// <summary>
        /// [Indeterminate] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool IndeterminateTrueColor
        {
            get => ScreensaverPackInit.SaversConfig.IndeterminateTrueColor;
            set => ScreensaverPackInit.SaversConfig.IndeterminateTrueColor = value;
        }
        /// <summary>
        /// [Indeterminate] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int IndeterminateDelay
        {
            get => ScreensaverPackInit.SaversConfig.IndeterminateDelay;
            set
            {
                if (value <= 0)
                    value = 20;
                ScreensaverPackInit.SaversConfig.IndeterminateDelay = value;
            }
        }
        /// <summary>
        /// [Indeterminate] Upper left corner character 
        /// </summary>
        public static char IndeterminateUpperLeftCornerChar
        {
            get => ScreensaverPackInit.SaversConfig.IndeterminateUpperLeftCornerChar;
            set => ScreensaverPackInit.SaversConfig.IndeterminateUpperLeftCornerChar = value;
        }
        /// <summary>
        /// [Indeterminate] Upper right corner character 
        /// </summary>
        public static char IndeterminateUpperRightCornerChar
        {
            get => ScreensaverPackInit.SaversConfig.IndeterminateUpperRightCornerChar;
            set => ScreensaverPackInit.SaversConfig.IndeterminateUpperRightCornerChar = value;
        }
        /// <summary>
        /// [Indeterminate] Lower left corner character 
        /// </summary>
        public static char IndeterminateLowerLeftCornerChar
        {
            get => ScreensaverPackInit.SaversConfig.IndeterminateLowerLeftCornerChar;
            set => ScreensaverPackInit.SaversConfig.IndeterminateLowerLeftCornerChar = value;
        }
        /// <summary>
        /// [Indeterminate] Lower right corner character 
        /// </summary>
        public static char IndeterminateLowerRightCornerChar
        {
            get => ScreensaverPackInit.SaversConfig.IndeterminateLowerRightCornerChar;
            set => ScreensaverPackInit.SaversConfig.IndeterminateLowerRightCornerChar = value;
        }
        /// <summary>
        /// [Indeterminate] Upper frame character 
        /// </summary>
        public static char IndeterminateUpperFrameChar
        {
            get => ScreensaverPackInit.SaversConfig.IndeterminateUpperFrameChar;
            set => ScreensaverPackInit.SaversConfig.IndeterminateUpperFrameChar = value;
        }
        /// <summary>
        /// [Indeterminate] Lower frame character 
        /// </summary>
        public static char IndeterminateLowerFrameChar
        {
            get => ScreensaverPackInit.SaversConfig.IndeterminateLowerFrameChar;
            set => ScreensaverPackInit.SaversConfig.IndeterminateLowerFrameChar = value;
        }
        /// <summary>
        /// [Indeterminate] Left frame character 
        /// </summary>
        public static char IndeterminateLeftFrameChar
        {
            get => ScreensaverPackInit.SaversConfig.IndeterminateLeftFrameChar;
            set => ScreensaverPackInit.SaversConfig.IndeterminateLeftFrameChar = value;
        }
        /// <summary>
        /// [Indeterminate] Right frame character 
        /// </summary>
        public static char IndeterminateRightFrameChar
        {
            get => ScreensaverPackInit.SaversConfig.IndeterminateRightFrameChar;
            set => ScreensaverPackInit.SaversConfig.IndeterminateRightFrameChar = value;
        }
        /// <summary>
        /// [Indeterminate] The minimum red color level (true color)
        /// </summary>
        public static int IndeterminateMinimumRedColorLevel
        {
            get => ScreensaverPackInit.SaversConfig.IndeterminateMinimumRedColorLevel;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.IndeterminateMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Indeterminate] The minimum green color level (true color)
        /// </summary>
        public static int IndeterminateMinimumGreenColorLevel
        {
            get => ScreensaverPackInit.SaversConfig.IndeterminateMinimumGreenColorLevel;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.IndeterminateMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Indeterminate] The minimum blue color level (true color)
        /// </summary>
        public static int IndeterminateMinimumBlueColorLevel
        {
            get => ScreensaverPackInit.SaversConfig.IndeterminateMinimumBlueColorLevel;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.IndeterminateMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Indeterminate] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int IndeterminateMinimumColorLevel
        {
            get => ScreensaverPackInit.SaversConfig.IndeterminateMinimumColorLevel;
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                ScreensaverPackInit.SaversConfig.IndeterminateMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Indeterminate] The maximum red color level (true color)
        /// </summary>
        public static int IndeterminateMaximumRedColorLevel
        {
            get => ScreensaverPackInit.SaversConfig.IndeterminateMaximumRedColorLevel;
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.IndeterminateMinimumRedColorLevel)
                    value = ScreensaverPackInit.SaversConfig.IndeterminateMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.IndeterminateMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Indeterminate] The maximum green color level (true color)
        /// </summary>
        public static int IndeterminateMaximumGreenColorLevel
        {
            get => ScreensaverPackInit.SaversConfig.IndeterminateMaximumGreenColorLevel;
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.IndeterminateMinimumGreenColorLevel)
                    value = ScreensaverPackInit.SaversConfig.IndeterminateMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.IndeterminateMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Indeterminate] The maximum blue color level (true color)
        /// </summary>
        public static int IndeterminateMaximumBlueColorLevel
        {
            get => ScreensaverPackInit.SaversConfig.IndeterminateMaximumBlueColorLevel;
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.IndeterminateMinimumBlueColorLevel)
                    value = ScreensaverPackInit.SaversConfig.IndeterminateMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.IndeterminateMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Indeterminate] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int IndeterminateMaximumColorLevel
        {
            get => ScreensaverPackInit.SaversConfig.IndeterminateMaximumColorLevel;
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= ScreensaverPackInit.SaversConfig.IndeterminateMinimumColorLevel)
                    value = ScreensaverPackInit.SaversConfig.IndeterminateMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                ScreensaverPackInit.SaversConfig.IndeterminateMaximumColorLevel = value;
            }
        }
        /// <summary>
        /// [Indeterminate] Upper left corner color.
        /// </summary>
        public static string IndeterminateUpperLeftCornerColor
        {
            get => ScreensaverPackInit.SaversConfig.IndeterminateUpperLeftCornerColor;
            set => ScreensaverPackInit.SaversConfig.IndeterminateUpperLeftCornerColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Indeterminate] Upper right corner color.
        /// </summary>
        public static string IndeterminateUpperRightCornerColor
        {
            get => ScreensaverPackInit.SaversConfig.IndeterminateUpperRightCornerColor;
            set => ScreensaverPackInit.SaversConfig.IndeterminateUpperRightCornerColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Indeterminate] Lower left corner color.
        /// </summary>
        public static string IndeterminateLowerLeftCornerColor
        {
            get => ScreensaverPackInit.SaversConfig.IndeterminateLowerLeftCornerColor;
            set => ScreensaverPackInit.SaversConfig.IndeterminateLowerLeftCornerColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Indeterminate] Lower right corner color.
        /// </summary>
        public static string IndeterminateLowerRightCornerColor
        {
            get => ScreensaverPackInit.SaversConfig.IndeterminateLowerRightCornerColor;
            set => ScreensaverPackInit.SaversConfig.IndeterminateLowerRightCornerColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Indeterminate] Upper frame color.
        /// </summary>
        public static string IndeterminateUpperFrameColor
        {
            get => ScreensaverPackInit.SaversConfig.IndeterminateUpperFrameColor;
            set => ScreensaverPackInit.SaversConfig.IndeterminateUpperFrameColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Indeterminate] Lower frame color.
        /// </summary>
        public static string IndeterminateLowerFrameColor
        {
            get => ScreensaverPackInit.SaversConfig.IndeterminateLowerFrameColor;
            set => ScreensaverPackInit.SaversConfig.IndeterminateLowerFrameColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Indeterminate] Left frame color.
        /// </summary>
        public static string IndeterminateLeftFrameColor
        {
            get => ScreensaverPackInit.SaversConfig.IndeterminateLeftFrameColor;
            set => ScreensaverPackInit.SaversConfig.IndeterminateLeftFrameColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Indeterminate] Right frame color.
        /// </summary>
        public static string IndeterminateRightFrameColor
        {
            get => ScreensaverPackInit.SaversConfig.IndeterminateRightFrameColor;
            set => ScreensaverPackInit.SaversConfig.IndeterminateRightFrameColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Indeterminate] Use the border colors.
        /// </summary>
        public static bool IndeterminateUseBorderColors
        {
            get => ScreensaverPackInit.SaversConfig.IndeterminateUseBorderColors;
            set => ScreensaverPackInit.SaversConfig.IndeterminateUseBorderColors = value;
        }

    }

    /// <summary>
    /// Display code for Indeterminate
    /// </summary>
    public class IndeterminateDisplay : BaseScreensaver, IScreensaver
    {

        private readonly int RampFrameBlockStartWidth = 5;
        private readonly int RampFrameBlockWidth = 3;
        private int IndeterminateCurrentBlockStart;
        private int IndeterminateCurrentBlockEnd;
        private IndeterminateDirection IndeterminateCurrentBlockDirection = IndeterminateDirection.LeftToRight;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Indeterminate";

        /// <inheritdoc/>
        public IndeterminateDisplay()
        {
            IndeterminateCurrentBlockStart = RampFrameBlockStartWidth;
            IndeterminateCurrentBlockEnd = IndeterminateCurrentBlockStart + RampFrameBlockWidth;
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            int RedColorNum = RandomDriver.Random(IndeterminateSettings.IndeterminateMinimumRedColorLevel, IndeterminateSettings.IndeterminateMaximumRedColorLevel);
            int GreenColorNum = RandomDriver.Random(IndeterminateSettings.IndeterminateMinimumGreenColorLevel, IndeterminateSettings.IndeterminateMaximumGreenColorLevel);
            int BlueColorNum = RandomDriver.Random(IndeterminateSettings.IndeterminateMinimumBlueColorLevel, IndeterminateSettings.IndeterminateMaximumBlueColorLevel);
            int ColorNum = RandomDriver.Random(IndeterminateSettings.IndeterminateMinimumColorLevel, IndeterminateSettings.IndeterminateMaximumColorLevel);

            // Console resizing can sometimes cause the cursor to remain visible. This happens on Windows 10's terminal.
            ConsoleWrapper.CursorVisible = false;

            // Set start and end widths for the ramp frame
            int RampFrameStartWidth = 4;
            int RampFrameEndWidth = ConsoleWrapper.WindowWidth - RampFrameStartWidth;
            int RampFrameSpaces = RampFrameEndWidth - RampFrameStartWidth;
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Start width: {0}, End width: {1}, Spaces: {2}", RampFrameStartWidth, RampFrameEndWidth, RampFrameSpaces);

            // Let the ramp be printed in the center
            int RampCenterPosition = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d);
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Center position: {0}", RampCenterPosition);

            // Draw the frame
            if (!ConsoleResizeHandler.WasResized(false))
                BorderColor.WriteBorder(RampFrameStartWidth, RampCenterPosition - 2, RampFrameSpaces, 3, IndeterminateSettings.IndeterminateUseBorderColors ? new Color(IndeterminateSettings.IndeterminateLeftFrameColor) : ColorTools.GetGray());

            // Draw the ramp
            int RampFrameBlockEndWidth = RampFrameEndWidth;
            Color RampCurrentColorInstance;
            if (IndeterminateSettings.IndeterminateTrueColor)
                // Set the current colors
                RampCurrentColorInstance = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            else
                // Set the current colors
                RampCurrentColorInstance = new Color(ColorNum);

            // Fill the ramp!
            while (
                (IndeterminateCurrentBlockEnd != RampFrameBlockEndWidth && IndeterminateCurrentBlockDirection == IndeterminateDirection.LeftToRight) ||
                (IndeterminateCurrentBlockStart != RampFrameBlockStartWidth && IndeterminateCurrentBlockDirection == IndeterminateDirection.RightToLeft)
            )
            {
                if (ConsoleResizeHandler.WasResized(false))
                    break;

                // Clear the ramp
                if (IndeterminateCurrentBlockDirection == IndeterminateDirection.LeftToRight)
                {
                    int start = IndeterminateCurrentBlockStart == RampFrameStartWidth + 1 ? IndeterminateCurrentBlockStart : IndeterminateCurrentBlockStart - 1;
                    for (int BlockPos = start; BlockPos <= IndeterminateCurrentBlockEnd; BlockPos++)
                    {
                        TextWriterWhereColor.WriteWhereColorBack(" ", BlockPos, RampCenterPosition - 1, true, Color.Empty, KernelColorTools.GetColor(KernelColorType.Background));
                        TextWriterWhereColor.WriteWhereColorBack(" ", BlockPos, RampCenterPosition, true, Color.Empty, KernelColorTools.GetColor(KernelColorType.Background));
                        TextWriterWhereColor.WriteWhereColorBack(" ", BlockPos, RampCenterPosition + 1, true, Color.Empty, KernelColorTools.GetColor(KernelColorType.Background));
                    }
                }
                else
                {
                    int end = IndeterminateCurrentBlockEnd == RampFrameEndWidth ? IndeterminateCurrentBlockEnd : IndeterminateCurrentBlockEnd + 1;
                    for (int BlockPos = IndeterminateCurrentBlockStart; BlockPos <= end; BlockPos++)
                    {
                        TextWriterWhereColor.WriteWhereColorBack(" ", BlockPos, RampCenterPosition - 1, true, Color.Empty, KernelColorTools.GetColor(KernelColorType.Background));
                        TextWriterWhereColor.WriteWhereColorBack(" ", BlockPos, RampCenterPosition, true, Color.Empty, KernelColorTools.GetColor(KernelColorType.Background));
                        TextWriterWhereColor.WriteWhereColorBack(" ", BlockPos, RampCenterPosition + 1, true, Color.Empty, KernelColorTools.GetColor(KernelColorType.Background));
                    }
                }

                // Fill the ramp
                ColorTools.SetConsoleColorDry(RampCurrentColorInstance, true);
                for (int BlockPos = IndeterminateCurrentBlockStart; BlockPos <= IndeterminateCurrentBlockEnd; BlockPos++)
                {
                    TextWriterWhereColor.WriteWhere(" ", BlockPos, RampCenterPosition - 1, true);
                    TextWriterWhereColor.WriteWhere(" ", BlockPos, RampCenterPosition, true);
                    TextWriterWhereColor.WriteWhere(" ", BlockPos, RampCenterPosition + 1, true);
                }

                // Change the start and end positions
                switch (IndeterminateCurrentBlockDirection)
                {
                    case IndeterminateDirection.LeftToRight:
                        IndeterminateCurrentBlockStart += 1;
                        IndeterminateCurrentBlockEnd += 1;
                        break;
                    case IndeterminateDirection.RightToLeft:
                        IndeterminateCurrentBlockStart -= 1;
                        IndeterminateCurrentBlockEnd -= 1;
                        break;
                }

                // Delay writing
                ThreadManager.SleepNoBlock(IndeterminateSettings.IndeterminateDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            }

            // Change the direction enumeration
            switch (IndeterminateCurrentBlockDirection)
            {
                case IndeterminateDirection.LeftToRight:
                    IndeterminateCurrentBlockDirection = IndeterminateDirection.RightToLeft;
                    break;
                case IndeterminateDirection.RightToLeft:
                    IndeterminateCurrentBlockDirection = IndeterminateDirection.LeftToRight;
                    break;
            }

            // Reset the background
            ColorTools.LoadBack();
            ConsoleResizeHandler.WasResized();
            ThreadManager.SleepNoBlock(IndeterminateSettings.IndeterminateDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

        private enum IndeterminateDirection
        {
            LeftToRight,
            RightToLeft
        }

    }
}
