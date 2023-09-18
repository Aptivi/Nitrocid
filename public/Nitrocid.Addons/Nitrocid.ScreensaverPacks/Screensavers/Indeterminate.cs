
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
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Drivers.RNG;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Kernel.Threading;
using KS.Misc.Screensaver;
using Terminaux.Colors;

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
            get => Config.SaverConfig.IndeterminateTrueColor;
            set => Config.SaverConfig.IndeterminateTrueColor = value;
        }
        /// <summary>
        /// [Indeterminate] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int IndeterminateDelay
        {
            get => Config.SaverConfig.IndeterminateDelay;
            set
            {
                if (value <= 0)
                    value = 20;
                Config.SaverConfig.IndeterminateDelay = value;
            }
        }
        /// <summary>
        /// [Indeterminate] Upper left corner character 
        /// </summary>
        public static char IndeterminateUpperLeftCornerChar
        {
            get => Config.SaverConfig.IndeterminateUpperLeftCornerChar;
            set => Config.SaverConfig.IndeterminateUpperLeftCornerChar = value;
        }
        /// <summary>
        /// [Indeterminate] Upper right corner character 
        /// </summary>
        public static char IndeterminateUpperRightCornerChar
        {
            get => Config.SaverConfig.IndeterminateUpperRightCornerChar;
            set => Config.SaverConfig.IndeterminateUpperRightCornerChar = value;
        }
        /// <summary>
        /// [Indeterminate] Lower left corner character 
        /// </summary>
        public static char IndeterminateLowerLeftCornerChar
        {
            get => Config.SaverConfig.IndeterminateLowerLeftCornerChar;
            set => Config.SaverConfig.IndeterminateLowerLeftCornerChar = value;
        }
        /// <summary>
        /// [Indeterminate] Lower right corner character 
        /// </summary>
        public static char IndeterminateLowerRightCornerChar
        {
            get => Config.SaverConfig.IndeterminateLowerRightCornerChar;
            set => Config.SaverConfig.IndeterminateLowerRightCornerChar = value;
        }
        /// <summary>
        /// [Indeterminate] Upper frame character 
        /// </summary>
        public static char IndeterminateUpperFrameChar
        {
            get => Config.SaverConfig.IndeterminateUpperFrameChar;
            set => Config.SaverConfig.IndeterminateUpperFrameChar = value;
        }
        /// <summary>
        /// [Indeterminate] Lower frame character 
        /// </summary>
        public static char IndeterminateLowerFrameChar
        {
            get => Config.SaverConfig.IndeterminateLowerFrameChar;
            set => Config.SaverConfig.IndeterminateLowerFrameChar = value;
        }
        /// <summary>
        /// [Indeterminate] Left frame character 
        /// </summary>
        public static char IndeterminateLeftFrameChar
        {
            get => Config.SaverConfig.IndeterminateLeftFrameChar;
            set => Config.SaverConfig.IndeterminateLeftFrameChar = value;
        }
        /// <summary>
        /// [Indeterminate] Right frame character 
        /// </summary>
        public static char IndeterminateRightFrameChar
        {
            get => Config.SaverConfig.IndeterminateRightFrameChar;
            set => Config.SaverConfig.IndeterminateRightFrameChar = value;
        }
        /// <summary>
        /// [Indeterminate] The minimum red color level (true color)
        /// </summary>
        public static int IndeterminateMinimumRedColorLevel
        {
            get => Config.SaverConfig.IndeterminateMinimumRedColorLevel;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.IndeterminateMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Indeterminate] The minimum green color level (true color)
        /// </summary>
        public static int IndeterminateMinimumGreenColorLevel
        {
            get => Config.SaverConfig.IndeterminateMinimumGreenColorLevel;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.IndeterminateMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Indeterminate] The minimum blue color level (true color)
        /// </summary>
        public static int IndeterminateMinimumBlueColorLevel
        {
            get => Config.SaverConfig.IndeterminateMinimumBlueColorLevel;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.IndeterminateMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Indeterminate] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int IndeterminateMinimumColorLevel
        {
            get => Config.SaverConfig.IndeterminateMinimumColorLevel;
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                Config.SaverConfig.IndeterminateMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Indeterminate] The maximum red color level (true color)
        /// </summary>
        public static int IndeterminateMaximumRedColorLevel
        {
            get => Config.SaverConfig.IndeterminateMaximumRedColorLevel;
            set
            {
                if (value <= Config.SaverConfig.IndeterminateMinimumRedColorLevel)
                    value = Config.SaverConfig.IndeterminateMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.IndeterminateMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Indeterminate] The maximum green color level (true color)
        /// </summary>
        public static int IndeterminateMaximumGreenColorLevel
        {
            get => Config.SaverConfig.IndeterminateMaximumGreenColorLevel;
            set
            {
                if (value <= Config.SaverConfig.IndeterminateMinimumGreenColorLevel)
                    value = Config.SaverConfig.IndeterminateMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.IndeterminateMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Indeterminate] The maximum blue color level (true color)
        /// </summary>
        public static int IndeterminateMaximumBlueColorLevel
        {
            get => Config.SaverConfig.IndeterminateMaximumBlueColorLevel;
            set
            {
                if (value <= Config.SaverConfig.IndeterminateMinimumBlueColorLevel)
                    value = Config.SaverConfig.IndeterminateMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.IndeterminateMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Indeterminate] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int IndeterminateMaximumColorLevel
        {
            get => Config.SaverConfig.IndeterminateMaximumColorLevel;
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= Config.SaverConfig.IndeterminateMinimumColorLevel)
                    value = Config.SaverConfig.IndeterminateMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                Config.SaverConfig.IndeterminateMaximumColorLevel = value;
            }
        }
        /// <summary>
        /// [Indeterminate] Upper left corner color.
        /// </summary>
        public static string IndeterminateUpperLeftCornerColor
        {
            get => Config.SaverConfig.IndeterminateUpperLeftCornerColor;
            set => Config.SaverConfig.IndeterminateUpperLeftCornerColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Indeterminate] Upper right corner color.
        /// </summary>
        public static string IndeterminateUpperRightCornerColor
        {
            get => Config.SaverConfig.IndeterminateUpperRightCornerColor;
            set => Config.SaverConfig.IndeterminateUpperRightCornerColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Indeterminate] Lower left corner color.
        /// </summary>
        public static string IndeterminateLowerLeftCornerColor
        {
            get => Config.SaverConfig.IndeterminateLowerLeftCornerColor;
            set => Config.SaverConfig.IndeterminateLowerLeftCornerColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Indeterminate] Lower right corner color.
        /// </summary>
        public static string IndeterminateLowerRightCornerColor
        {
            get => Config.SaverConfig.IndeterminateLowerRightCornerColor;
            set => Config.SaverConfig.IndeterminateLowerRightCornerColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Indeterminate] Upper frame color.
        /// </summary>
        public static string IndeterminateUpperFrameColor
        {
            get => Config.SaverConfig.IndeterminateUpperFrameColor;
            set => Config.SaverConfig.IndeterminateUpperFrameColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Indeterminate] Lower frame color.
        /// </summary>
        public static string IndeterminateLowerFrameColor
        {
            get => Config.SaverConfig.IndeterminateLowerFrameColor;
            set => Config.SaverConfig.IndeterminateLowerFrameColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Indeterminate] Left frame color.
        /// </summary>
        public static string IndeterminateLeftFrameColor
        {
            get => Config.SaverConfig.IndeterminateLeftFrameColor;
            set => Config.SaverConfig.IndeterminateLeftFrameColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Indeterminate] Right frame color.
        /// </summary>
        public static string IndeterminateRightFrameColor
        {
            get => Config.SaverConfig.IndeterminateRightFrameColor;
            set => Config.SaverConfig.IndeterminateRightFrameColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Indeterminate] Use the border colors.
        /// </summary>
        public static bool IndeterminateUseBorderColors
        {
            get => Config.SaverConfig.IndeterminateUseBorderColors;
            set => Config.SaverConfig.IndeterminateUseBorderColors = value;
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
            if (!ConsoleResizeListener.WasResized(false))
            {
                TextWriterWhereColor.WriteWhere(IndeterminateSettings.IndeterminateUpperLeftCornerChar.ToString(), RampFrameStartWidth, RampCenterPosition - 2, false, IndeterminateSettings.IndeterminateUseBorderColors ? new Color(IndeterminateSettings.IndeterminateUpperLeftCornerColor) : KernelColorTools.GetGray());
                TextWriterColor.Write(new string(IndeterminateSettings.IndeterminateUpperFrameChar, RampFrameSpaces), false, IndeterminateSettings.IndeterminateUseBorderColors ? new Color(IndeterminateSettings.IndeterminateUpperFrameColor) : KernelColorTools.GetGray());
                TextWriterColor.Write(IndeterminateSettings.IndeterminateUpperRightCornerChar.ToString(), false, IndeterminateSettings.IndeterminateUseBorderColors ? new Color(IndeterminateSettings.IndeterminateUpperRightCornerColor) : KernelColorTools.GetGray());
                TextWriterWhereColor.WriteWhere(IndeterminateSettings.IndeterminateLeftFrameChar.ToString(), RampFrameStartWidth, RampCenterPosition - 1, false, IndeterminateSettings.IndeterminateUseBorderColors ? new Color(IndeterminateSettings.IndeterminateLeftFrameColor) : KernelColorTools.GetGray());
                TextWriterWhereColor.WriteWhere(IndeterminateSettings.IndeterminateLeftFrameChar.ToString(), RampFrameStartWidth, RampCenterPosition, false, IndeterminateSettings.IndeterminateUseBorderColors ? new Color(IndeterminateSettings.IndeterminateLeftFrameColor) : KernelColorTools.GetGray());
                TextWriterWhereColor.WriteWhere(IndeterminateSettings.IndeterminateLeftFrameChar.ToString(), RampFrameStartWidth, RampCenterPosition + 1, false, IndeterminateSettings.IndeterminateUseBorderColors ? new Color(IndeterminateSettings.IndeterminateLeftFrameColor) : KernelColorTools.GetGray());
                TextWriterWhereColor.WriteWhere(IndeterminateSettings.IndeterminateRightFrameChar.ToString(), RampFrameEndWidth + 1, RampCenterPosition - 1, false, IndeterminateSettings.IndeterminateUseBorderColors ? new Color(IndeterminateSettings.IndeterminateLeftFrameColor) : KernelColorTools.GetGray());
                TextWriterWhereColor.WriteWhere(IndeterminateSettings.IndeterminateRightFrameChar.ToString(), RampFrameEndWidth + 1, RampCenterPosition, false, IndeterminateSettings.IndeterminateUseBorderColors ? new Color(IndeterminateSettings.IndeterminateLeftFrameColor) : KernelColorTools.GetGray());
                TextWriterWhereColor.WriteWhere(IndeterminateSettings.IndeterminateRightFrameChar.ToString(), RampFrameEndWidth + 1, RampCenterPosition + 1, false, IndeterminateSettings.IndeterminateUseBorderColors ? new Color(IndeterminateSettings.IndeterminateLeftFrameColor) : KernelColorTools.GetGray());
                TextWriterWhereColor.WriteWhere(IndeterminateSettings.IndeterminateLowerLeftCornerChar.ToString(), RampFrameStartWidth, RampCenterPosition + 2, false, IndeterminateSettings.IndeterminateUseBorderColors ? new Color(IndeterminateSettings.IndeterminateLowerLeftCornerColor) : KernelColorTools.GetGray());
                TextWriterColor.Write(new string(IndeterminateSettings.IndeterminateLowerFrameChar, RampFrameSpaces), false, IndeterminateSettings.IndeterminateUseBorderColors ? new Color(IndeterminateSettings.IndeterminateLowerFrameColor) : KernelColorTools.GetGray());
                TextWriterColor.Write(IndeterminateSettings.IndeterminateLowerRightCornerChar.ToString(), false, IndeterminateSettings.IndeterminateUseBorderColors ? new Color(IndeterminateSettings.IndeterminateLowerRightCornerColor) : KernelColorTools.GetGray());
            }

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
            while (!(IndeterminateCurrentBlockEnd == RampFrameBlockEndWidth & IndeterminateCurrentBlockDirection == IndeterminateDirection.LeftToRight | IndeterminateCurrentBlockStart == RampFrameBlockStartWidth & IndeterminateCurrentBlockDirection == IndeterminateDirection.RightToLeft))
            {
                if (ConsoleResizeListener.WasResized(false))
                    break;

                // Clear the ramp
                if (IndeterminateCurrentBlockDirection == IndeterminateDirection.LeftToRight)
                {
                    int start = IndeterminateCurrentBlockStart == RampFrameStartWidth + 1 ? IndeterminateCurrentBlockStart : IndeterminateCurrentBlockStart - 1;
                    for (int BlockPos = start; BlockPos <= IndeterminateCurrentBlockEnd; BlockPos++)
                    {
                        TextWriterWhereColor.WriteWhere(" ", BlockPos, RampCenterPosition - 1, true, Color.Empty, KernelColorTools.GetColor(KernelColorType.Background));
                        TextWriterWhereColor.WriteWhere(" ", BlockPos, RampCenterPosition, true, Color.Empty, KernelColorTools.GetColor(KernelColorType.Background));
                        TextWriterWhereColor.WriteWhere(" ", BlockPos, RampCenterPosition + 1, true, Color.Empty, KernelColorTools.GetColor(KernelColorType.Background));
                    }
                }
                else
                {
                    int end = IndeterminateCurrentBlockEnd == RampFrameEndWidth ? IndeterminateCurrentBlockEnd : IndeterminateCurrentBlockEnd + 1;
                    for (int BlockPos = IndeterminateCurrentBlockStart; BlockPos <= end; BlockPos++)
                    {
                        TextWriterWhereColor.WriteWhere(" ", BlockPos, RampCenterPosition - 1, true, Color.Empty, KernelColorTools.GetColor(KernelColorType.Background));
                        TextWriterWhereColor.WriteWhere(" ", BlockPos, RampCenterPosition, true, Color.Empty, KernelColorTools.GetColor(KernelColorType.Background));
                        TextWriterWhereColor.WriteWhere(" ", BlockPos, RampCenterPosition + 1, true, Color.Empty, KernelColorTools.GetColor(KernelColorType.Background));
                    }
                }

                // Fill the ramp
                KernelColorTools.SetConsoleColor(RampCurrentColorInstance, true);
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
            KernelColorTools.LoadBack();
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(IndeterminateSettings.IndeterminateDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

        private enum IndeterminateDirection
        {
            LeftToRight,
            RightToLeft
        }

    }
}
