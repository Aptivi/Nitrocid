
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
using Extensification.StringExts;
using KS.ConsoleBase;
using KS.Drivers.RNG;
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

        private static bool _TrueColor = true;
        private static int _Delay = 20;
        private static string _UpperLeftCornerChar = "╔";
        private static string _UpperRightCornerChar = "╗";
        private static string _LowerLeftCornerChar = "╚";
        private static string _LowerRightCornerChar = "╝";
        private static string _UpperFrameChar = "═";
        private static string _LowerFrameChar = "═";
        private static string _LeftFrameChar = "║";
        private static string _RightFrameChar = "║";
        private static int _MinimumRedColorLevel = 0;
        private static int _MinimumGreenColorLevel = 0;
        private static int _MinimumBlueColorLevel = 0;
        private static int _MinimumColorLevel = 0;
        private static int _MaximumRedColorLevel = 255;
        private static int _MaximumGreenColorLevel = 255;
        private static int _MaximumBlueColorLevel = 255;
        private static int _MaximumColorLevel = 255;
        private static string _UpperLeftCornerColor = 7.ToString();
        private static string _UpperRightCornerColor = 7.ToString();
        private static string _LowerLeftCornerColor = 7.ToString();
        private static string _LowerRightCornerColor = 7.ToString();
        private static string _UpperFrameColor = 7.ToString();
        private static string _LowerFrameColor = 7.ToString();
        private static string _LeftFrameColor = 7.ToString();
        private static string _RightFrameColor = 7.ToString();
        private static bool _UseBorderColors;

        /// <summary>
        /// [Indeterminate] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool IndeterminateTrueColor
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
        /// [Indeterminate] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int IndeterminateDelay
        {
            get
            {
                return _Delay;
            }
            set
            {
                if (value <= 0)
                    value = 20;
                _Delay = value;
            }
        }
        /// <summary>
        /// [Indeterminate] Upper left corner character 
        /// </summary>
        public static string IndeterminateUpperLeftCornerChar
        {
            get
            {
                return _UpperLeftCornerChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╔";
                _UpperLeftCornerChar = value;
            }
        }
        /// <summary>
        /// [Indeterminate] Upper right corner character 
        /// </summary>
        public static string IndeterminateUpperRightCornerChar
        {
            get
            {
                return _UpperRightCornerChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╗";
                _UpperRightCornerChar = value;
            }
        }
        /// <summary>
        /// [Indeterminate] Lower left corner character 
        /// </summary>
        public static string IndeterminateLowerLeftCornerChar
        {
            get
            {
                return _LowerLeftCornerChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╚";
                _LowerLeftCornerChar = value;
            }
        }
        /// <summary>
        /// [Indeterminate] Lower right corner character 
        /// </summary>
        public static string IndeterminateLowerRightCornerChar
        {
            get
            {
                return _LowerRightCornerChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╝";
                _LowerRightCornerChar = value;
            }
        }
        /// <summary>
        /// [Indeterminate] Upper frame character 
        /// </summary>
        public static string IndeterminateUpperFrameChar
        {
            get
            {
                return _UpperFrameChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "═";
                _UpperFrameChar = value;
            }
        }
        /// <summary>
        /// [Indeterminate] Lower frame character 
        /// </summary>
        public static string IndeterminateLowerFrameChar
        {
            get
            {
                return _LowerFrameChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "═";
                _LowerFrameChar = value;
            }
        }
        /// <summary>
        /// [Indeterminate] Left frame character 
        /// </summary>
        public static string IndeterminateLeftFrameChar
        {
            get
            {
                return _LeftFrameChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "║";
                _LeftFrameChar = value;
            }
        }
        /// <summary>
        /// [Indeterminate] Right frame character 
        /// </summary>
        public static string IndeterminateRightFrameChar
        {
            get
            {
                return _RightFrameChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "║";
                _RightFrameChar = value;
            }
        }
        /// <summary>
        /// [Indeterminate] The minimum red color level (true color)
        /// </summary>
        public static int IndeterminateMinimumRedColorLevel
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
        /// [Indeterminate] The minimum green color level (true color)
        /// </summary>
        public static int IndeterminateMinimumGreenColorLevel
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
        /// [Indeterminate] The minimum blue color level (true color)
        /// </summary>
        public static int IndeterminateMinimumBlueColorLevel
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
        /// [Indeterminate] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int IndeterminateMinimumColorLevel
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
        /// [Indeterminate] The maximum red color level (true color)
        /// </summary>
        public static int IndeterminateMaximumRedColorLevel
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
        /// [Indeterminate] The maximum green color level (true color)
        /// </summary>
        public static int IndeterminateMaximumGreenColorLevel
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
        /// [Indeterminate] The maximum blue color level (true color)
        /// </summary>
        public static int IndeterminateMaximumBlueColorLevel
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
        /// [Indeterminate] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int IndeterminateMaximumColorLevel
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
        /// <summary>
        /// [Indeterminate] Upper left corner color.
        /// </summary>
        public static string IndeterminateUpperLeftCornerColor
        {
            get
            {
                return _UpperLeftCornerColor;
            }
            set
            {
                _UpperLeftCornerColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [Indeterminate] Upper right corner color.
        /// </summary>
        public static string IndeterminateUpperRightCornerColor
        {
            get
            {
                return _UpperRightCornerColor;
            }
            set
            {
                _UpperRightCornerColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [Indeterminate] Lower left corner color.
        /// </summary>
        public static string IndeterminateLowerLeftCornerColor
        {
            get
            {
                return _LowerLeftCornerColor;
            }
            set
            {
                _LowerLeftCornerColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [Indeterminate] Lower right corner color.
        /// </summary>
        public static string IndeterminateLowerRightCornerColor
        {
            get
            {
                return _LowerRightCornerColor;
            }
            set
            {
                _LowerRightCornerColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [Indeterminate] Upper frame color.
        /// </summary>
        public static string IndeterminateUpperFrameColor
        {
            get
            {
                return _UpperFrameColor;
            }
            set
            {
                _UpperFrameColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [Indeterminate] Lower frame color.
        /// </summary>
        public static string IndeterminateLowerFrameColor
        {
            get
            {
                return _LowerFrameColor;
            }
            set
            {
                _LowerFrameColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [Indeterminate] Left frame color.
        /// </summary>
        public static string IndeterminateLeftFrameColor
        {
            get
            {
                return _LeftFrameColor;
            }
            set
            {
                _LeftFrameColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [Indeterminate] Right frame color.
        /// </summary>
        public static string IndeterminateRightFrameColor
        {
            get
            {
                return _RightFrameColor;
            }
            set
            {
                _RightFrameColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [Indeterminate] Use the border colors.
        /// </summary>
        public static bool IndeterminateUseBorderColors
        {
            get
            {
                return _UseBorderColors;
            }
            set
            {
                _UseBorderColors = value;
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
                TextWriterWhereColor.WriteWhere(IndeterminateSettings.IndeterminateUpperLeftCornerChar, RampFrameStartWidth, RampCenterPosition - 2, false, IndeterminateSettings.IndeterminateUseBorderColors ? new Color(IndeterminateSettings.IndeterminateUpperLeftCornerColor) : ColorTools.GetGray());
                TextWriterColor.Write(IndeterminateSettings.IndeterminateUpperFrameChar.Repeat(RampFrameSpaces), false, IndeterminateSettings.IndeterminateUseBorderColors ? new Color(IndeterminateSettings.IndeterminateUpperFrameColor) : ColorTools.GetGray());
                TextWriterColor.Write(IndeterminateSettings.IndeterminateUpperRightCornerChar, false, IndeterminateSettings.IndeterminateUseBorderColors ? new Color(IndeterminateSettings.IndeterminateUpperRightCornerColor) : ColorTools.GetGray());
                TextWriterWhereColor.WriteWhere(IndeterminateSettings.IndeterminateLeftFrameChar, RampFrameStartWidth, RampCenterPosition - 1, false, IndeterminateSettings.IndeterminateUseBorderColors ? new Color(IndeterminateSettings.IndeterminateLeftFrameColor) : ColorTools.GetGray());
                TextWriterWhereColor.WriteWhere(IndeterminateSettings.IndeterminateLeftFrameChar, RampFrameStartWidth, RampCenterPosition, false, IndeterminateSettings.IndeterminateUseBorderColors ? new Color(IndeterminateSettings.IndeterminateLeftFrameColor) : ColorTools.GetGray());
                TextWriterWhereColor.WriteWhere(IndeterminateSettings.IndeterminateLeftFrameChar, RampFrameStartWidth, RampCenterPosition + 1, false, IndeterminateSettings.IndeterminateUseBorderColors ? new Color(IndeterminateSettings.IndeterminateLeftFrameColor) : ColorTools.GetGray());
                TextWriterWhereColor.WriteWhere(IndeterminateSettings.IndeterminateRightFrameChar, RampFrameEndWidth + 1, RampCenterPosition - 1, false, IndeterminateSettings.IndeterminateUseBorderColors ? new Color(IndeterminateSettings.IndeterminateLeftFrameColor) : ColorTools.GetGray());
                TextWriterWhereColor.WriteWhere(IndeterminateSettings.IndeterminateRightFrameChar, RampFrameEndWidth + 1, RampCenterPosition, false, IndeterminateSettings.IndeterminateUseBorderColors ? new Color(IndeterminateSettings.IndeterminateLeftFrameColor) : ColorTools.GetGray());
                TextWriterWhereColor.WriteWhere(IndeterminateSettings.IndeterminateRightFrameChar, RampFrameEndWidth + 1, RampCenterPosition + 1, false, IndeterminateSettings.IndeterminateUseBorderColors ? new Color(IndeterminateSettings.IndeterminateLeftFrameColor) : ColorTools.GetGray());
                TextWriterWhereColor.WriteWhere(IndeterminateSettings.IndeterminateLowerLeftCornerChar, RampFrameStartWidth, RampCenterPosition + 2, false, IndeterminateSettings.IndeterminateUseBorderColors ? new Color(IndeterminateSettings.IndeterminateLowerLeftCornerColor) : ColorTools.GetGray());
                TextWriterColor.Write(IndeterminateSettings.IndeterminateLowerFrameChar.Repeat(RampFrameSpaces), false, IndeterminateSettings.IndeterminateUseBorderColors ? new Color(IndeterminateSettings.IndeterminateLowerFrameColor) : ColorTools.GetGray());
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
