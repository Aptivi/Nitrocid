
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
using ColorTools = KS.ConsoleBase.Colors.ColorTools;

namespace KS.Misc.Screensaver.Displays
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
            get
            {
                return Config.SaverConfig.IndeterminateTrueColor;
            }
            set
            {
                Config.SaverConfig.IndeterminateTrueColor = value;
            }
        }
        /// <summary>
        /// [Indeterminate] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int IndeterminateDelay
        {
            get
            {
                return Config.SaverConfig.IndeterminateDelay;
            }
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
        public static string IndeterminateUpperLeftCornerChar
        {
            get
            {
                return Config.SaverConfig.IndeterminateUpperLeftCornerChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╔";
                Config.SaverConfig.IndeterminateUpperLeftCornerChar = value;
            }
        }
        /// <summary>
        /// [Indeterminate] Upper right corner character 
        /// </summary>
        public static string IndeterminateUpperRightCornerChar
        {
            get
            {
                return Config.SaverConfig.IndeterminateUpperRightCornerChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╗";
                Config.SaverConfig.IndeterminateUpperRightCornerChar = value;
            }
        }
        /// <summary>
        /// [Indeterminate] Lower left corner character 
        /// </summary>
        public static string IndeterminateLowerLeftCornerChar
        {
            get
            {
                return Config.SaverConfig.IndeterminateLowerLeftCornerChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╚";
                Config.SaverConfig.IndeterminateLowerLeftCornerChar = value;
            }
        }
        /// <summary>
        /// [Indeterminate] Lower right corner character 
        /// </summary>
        public static string IndeterminateLowerRightCornerChar
        {
            get
            {
                return Config.SaverConfig.IndeterminateLowerRightCornerChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╝";
                Config.SaverConfig.IndeterminateLowerRightCornerChar = value;
            }
        }
        /// <summary>
        /// [Indeterminate] Upper frame character 
        /// </summary>
        public static string IndeterminateUpperFrameChar
        {
            get
            {
                return Config.SaverConfig.IndeterminateUpperFrameChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "═";
                Config.SaverConfig.IndeterminateUpperFrameChar = value;
            }
        }
        /// <summary>
        /// [Indeterminate] Lower frame character 
        /// </summary>
        public static string IndeterminateLowerFrameChar
        {
            get
            {
                return Config.SaverConfig.IndeterminateLowerFrameChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "═";
                Config.SaverConfig.IndeterminateLowerFrameChar = value;
            }
        }
        /// <summary>
        /// [Indeterminate] Left frame character 
        /// </summary>
        public static string IndeterminateLeftFrameChar
        {
            get
            {
                return Config.SaverConfig.IndeterminateLeftFrameChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "║";
                Config.SaverConfig.IndeterminateLeftFrameChar = value;
            }
        }
        /// <summary>
        /// [Indeterminate] Right frame character 
        /// </summary>
        public static string IndeterminateRightFrameChar
        {
            get
            {
                return Config.SaverConfig.IndeterminateRightFrameChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "║";
                Config.SaverConfig.IndeterminateRightFrameChar = value;
            }
        }
        /// <summary>
        /// [Indeterminate] The minimum red color level (true color)
        /// </summary>
        public static int IndeterminateMinimumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.IndeterminateMinimumRedColorLevel;
            }
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
            get
            {
                return Config.SaverConfig.IndeterminateMinimumGreenColorLevel;
            }
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
            get
            {
                return Config.SaverConfig.IndeterminateMinimumBlueColorLevel;
            }
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
            get
            {
                return Config.SaverConfig.IndeterminateMinimumColorLevel;
            }
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
            get
            {
                return Config.SaverConfig.IndeterminateMaximumRedColorLevel;
            }
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
            get
            {
                return Config.SaverConfig.IndeterminateMaximumGreenColorLevel;
            }
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
            get
            {
                return Config.SaverConfig.IndeterminateMaximumBlueColorLevel;
            }
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
            get
            {
                return Config.SaverConfig.IndeterminateMaximumColorLevel;
            }
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
            get
            {
                return Config.SaverConfig.IndeterminateUpperLeftCornerColor;
            }
            set
            {
                Config.SaverConfig.IndeterminateUpperLeftCornerColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [Indeterminate] Upper right corner color.
        /// </summary>
        public static string IndeterminateUpperRightCornerColor
        {
            get
            {
                return Config.SaverConfig.IndeterminateUpperRightCornerColor;
            }
            set
            {
                Config.SaverConfig.IndeterminateUpperRightCornerColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [Indeterminate] Lower left corner color.
        /// </summary>
        public static string IndeterminateLowerLeftCornerColor
        {
            get
            {
                return Config.SaverConfig.IndeterminateLowerLeftCornerColor;
            }
            set
            {
                Config.SaverConfig.IndeterminateLowerLeftCornerColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [Indeterminate] Lower right corner color.
        /// </summary>
        public static string IndeterminateLowerRightCornerColor
        {
            get
            {
                return Config.SaverConfig.IndeterminateLowerRightCornerColor;
            }
            set
            {
                Config.SaverConfig.IndeterminateLowerRightCornerColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [Indeterminate] Upper frame color.
        /// </summary>
        public static string IndeterminateUpperFrameColor
        {
            get
            {
                return Config.SaverConfig.IndeterminateUpperFrameColor;
            }
            set
            {
                Config.SaverConfig.IndeterminateUpperFrameColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [Indeterminate] Lower frame color.
        /// </summary>
        public static string IndeterminateLowerFrameColor
        {
            get
            {
                return Config.SaverConfig.IndeterminateLowerFrameColor;
            }
            set
            {
                Config.SaverConfig.IndeterminateLowerFrameColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [Indeterminate] Left frame color.
        /// </summary>
        public static string IndeterminateLeftFrameColor
        {
            get
            {
                return Config.SaverConfig.IndeterminateLeftFrameColor;
            }
            set
            {
                Config.SaverConfig.IndeterminateLeftFrameColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [Indeterminate] Right frame color.
        /// </summary>
        public static string IndeterminateRightFrameColor
        {
            get
            {
                return Config.SaverConfig.IndeterminateRightFrameColor;
            }
            set
            {
                Config.SaverConfig.IndeterminateRightFrameColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [Indeterminate] Use the border colors.
        /// </summary>
        public static bool IndeterminateUseBorderColors
        {
            get
            {
                return Config.SaverConfig.IndeterminateUseBorderColors;
            }
            set
            {
                Config.SaverConfig.IndeterminateUseBorderColors = value;
            }
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
            DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Start width: {0}, End width: {1}, Spaces: {2}", RampFrameStartWidth, RampFrameEndWidth, RampFrameSpaces);

            // Let the ramp be printed in the center
            int RampCenterPosition = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d);
            DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Center position: {0}", RampCenterPosition);

            // Draw the frame
            if (!ConsoleResizeListener.WasResized(false))
            {
                // TODO: Deal with these, too.
                TextWriterWhereColor.WriteWhere(IndeterminateSettings.IndeterminateUpperLeftCornerChar, RampFrameStartWidth, RampCenterPosition - 2, false, IndeterminateSettings.IndeterminateUseBorderColors ? new Color(IndeterminateSettings.IndeterminateUpperLeftCornerColor) : ColorTools.GetGray());
                TextWriterColor.Write(new string(IndeterminateSettings.IndeterminateUpperFrameChar[0], RampFrameSpaces), false, IndeterminateSettings.IndeterminateUseBorderColors ? new Color(IndeterminateSettings.IndeterminateUpperFrameColor) : ColorTools.GetGray());
                TextWriterColor.Write(IndeterminateSettings.IndeterminateUpperRightCornerChar, false, IndeterminateSettings.IndeterminateUseBorderColors ? new Color(IndeterminateSettings.IndeterminateUpperRightCornerColor) : ColorTools.GetGray());
                TextWriterWhereColor.WriteWhere(IndeterminateSettings.IndeterminateLeftFrameChar, RampFrameStartWidth, RampCenterPosition - 1, false, IndeterminateSettings.IndeterminateUseBorderColors ? new Color(IndeterminateSettings.IndeterminateLeftFrameColor) : ColorTools.GetGray());
                TextWriterWhereColor.WriteWhere(IndeterminateSettings.IndeterminateLeftFrameChar, RampFrameStartWidth, RampCenterPosition, false, IndeterminateSettings.IndeterminateUseBorderColors ? new Color(IndeterminateSettings.IndeterminateLeftFrameColor) : ColorTools.GetGray());
                TextWriterWhereColor.WriteWhere(IndeterminateSettings.IndeterminateLeftFrameChar, RampFrameStartWidth, RampCenterPosition + 1, false, IndeterminateSettings.IndeterminateUseBorderColors ? new Color(IndeterminateSettings.IndeterminateLeftFrameColor) : ColorTools.GetGray());
                TextWriterWhereColor.WriteWhere(IndeterminateSettings.IndeterminateRightFrameChar, RampFrameEndWidth + 1, RampCenterPosition - 1, false, IndeterminateSettings.IndeterminateUseBorderColors ? new Color(IndeterminateSettings.IndeterminateLeftFrameColor) : ColorTools.GetGray());
                TextWriterWhereColor.WriteWhere(IndeterminateSettings.IndeterminateRightFrameChar, RampFrameEndWidth + 1, RampCenterPosition, false, IndeterminateSettings.IndeterminateUseBorderColors ? new Color(IndeterminateSettings.IndeterminateLeftFrameColor) : ColorTools.GetGray());
                TextWriterWhereColor.WriteWhere(IndeterminateSettings.IndeterminateRightFrameChar, RampFrameEndWidth + 1, RampCenterPosition + 1, false, IndeterminateSettings.IndeterminateUseBorderColors ? new Color(IndeterminateSettings.IndeterminateLeftFrameColor) : ColorTools.GetGray());
                TextWriterWhereColor.WriteWhere(IndeterminateSettings.IndeterminateLowerLeftCornerChar, RampFrameStartWidth, RampCenterPosition + 2, false, IndeterminateSettings.IndeterminateUseBorderColors ? new Color(IndeterminateSettings.IndeterminateLowerLeftCornerColor) : ColorTools.GetGray());
                TextWriterColor.Write(new string(IndeterminateSettings.IndeterminateLowerFrameChar[0], RampFrameSpaces), false, IndeterminateSettings.IndeterminateUseBorderColors ? new Color(IndeterminateSettings.IndeterminateLowerFrameColor) : ColorTools.GetGray());
                TextWriterColor.Write(IndeterminateSettings.IndeterminateLowerRightCornerChar, false, IndeterminateSettings.IndeterminateUseBorderColors ? new Color(IndeterminateSettings.IndeterminateLowerRightCornerColor) : ColorTools.GetGray());
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
                ConsoleWrapper.BackgroundColor = ConsoleColor.Black;
                if (IndeterminateCurrentBlockDirection == IndeterminateDirection.LeftToRight)
                {
                    for (int BlockPos = RampFrameBlockStartWidth; BlockPos <= IndeterminateCurrentBlockStart; BlockPos++)
                    {
                        TextWriterWhereColor.WriteWhere(" ", BlockPos, RampCenterPosition - 1, true);
                        TextWriterWhereColor.WriteWhere(" ", BlockPos, RampCenterPosition, true);
                        TextWriterWhereColor.WriteWhere(" ", BlockPos, RampCenterPosition + 1, true);
                    }
                }
                else
                {
                    for (int BlockPos = IndeterminateCurrentBlockEnd; BlockPos <= RampFrameBlockEndWidth; BlockPos++)
                    {
                        TextWriterWhereColor.WriteWhere(" ", BlockPos, RampCenterPosition - 1, true);
                        TextWriterWhereColor.WriteWhere(" ", BlockPos, RampCenterPosition, true);
                        TextWriterWhereColor.WriteWhere(" ", BlockPos, RampCenterPosition + 1, true);
                    }
                }

                // Fill the ramp
                ColorTools.SetConsoleColor(RampCurrentColorInstance, true, true);
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
                        {
                            IndeterminateCurrentBlockStart += 1;
                            IndeterminateCurrentBlockEnd += 1;
                            break;
                        }
                    case IndeterminateDirection.RightToLeft:
                        {
                            IndeterminateCurrentBlockStart -= 1;
                            IndeterminateCurrentBlockEnd -= 1;
                            break;
                        }
                }

                // Delay writing
                ThreadManager.SleepNoBlock(IndeterminateSettings.IndeterminateDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            }

            // Change the direction enumeration
            switch (IndeterminateCurrentBlockDirection)
            {
                case IndeterminateDirection.LeftToRight:
                    {
                        IndeterminateCurrentBlockDirection = IndeterminateDirection.RightToLeft;
                        break;
                    }
                case IndeterminateDirection.RightToLeft:
                    {
                        IndeterminateCurrentBlockDirection = IndeterminateDirection.LeftToRight;
                        break;
                    }
            }

            ConsoleWrapper.BackgroundColor = ConsoleColor.Black;
            ConsoleWrapper.Clear();
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
