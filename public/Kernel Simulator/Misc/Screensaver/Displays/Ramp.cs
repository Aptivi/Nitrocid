
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
    /// Settings for Ramp
    /// </summary>
    public static class RampSettings
    {

        private static bool _TrueColor = true;
        private static int _Delay = 20;
        private static int _NextRampDelay = 250;
        private static string _UpperLeftCornerChar = "╔";
        private static string _UpperRightCornerChar = "╗";
        private static string _LowerLeftCornerChar = "╚";
        private static string _LowerRightCornerChar = "╝";
        private static string _UpperFrameChar = "═";
        private static string _LowerFrameChar = "═";
        private static string _LeftFrameChar = "║";
        private static string _RightFrameChar = "║";
        private static int _MinimumRedColorLevelStart = 0;
        private static int _MinimumGreenColorLevelStart = 0;
        private static int _MinimumBlueColorLevelStart = 0;
        private static int _MinimumColorLevelStart = 0;
        private static int _MaximumRedColorLevelStart = 255;
        private static int _MaximumGreenColorLevelStart = 255;
        private static int _MaximumBlueColorLevelStart = 255;
        private static int _MaximumColorLevelStart = 255;
        private static int _MinimumRedColorLevelEnd = 0;
        private static int _MinimumGreenColorLevelEnd = 0;
        private static int _MinimumBlueColorLevelEnd = 0;
        private static int _MinimumColorLevelEnd = 0;
        private static int _MaximumRedColorLevelEnd = 255;
        private static int _MaximumGreenColorLevelEnd = 255;
        private static int _MaximumBlueColorLevelEnd = 255;
        private static int _MaximumColorLevelEnd = 255;
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
        /// [Ramp] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool RampTrueColor
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
        /// [Ramp] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int RampDelay
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
        /// [Ramp] How many milliseconds to wait before starting the next ramp?
        /// </summary>
        public static int RampNextRampDelay
        {
            get
            {
                return _NextRampDelay;
            }
            set
            {
                if (value <= 0)
                    value = 250;
                _NextRampDelay = value;
            }
        }
        /// <summary>
        /// [Ramp] Upper left corner character 
        /// </summary>
        public static string RampUpperLeftCornerChar
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
        /// [Ramp] Upper right corner character 
        /// </summary>
        public static string RampUpperRightCornerChar
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
        /// [Ramp] Lower left corner character 
        /// </summary>
        public static string RampLowerLeftCornerChar
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
        /// [Ramp] Lower right corner character 
        /// </summary>
        public static string RampLowerRightCornerChar
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
        /// [Ramp] Upper frame character 
        /// </summary>
        public static string RampUpperFrameChar
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
        /// [Ramp] Lower frame character 
        /// </summary>
        public static string RampLowerFrameChar
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
        /// [Ramp] Left frame character 
        /// </summary>
        public static string RampLeftFrameChar
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
        /// [Ramp] Right frame character 
        /// </summary>
        public static string RampRightFrameChar
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
        /// [Ramp] The minimum red color level (true color - start)
        /// </summary>
        public static int RampMinimumRedColorLevelStart
        {
            get
            {
                return _MinimumRedColorLevelStart;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _MinimumRedColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Ramp] The minimum green color level (true color - start)
        /// </summary>
        public static int RampMinimumGreenColorLevelStart
        {
            get
            {
                return _MinimumGreenColorLevelStart;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _MinimumGreenColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Ramp] The minimum blue color level (true color - start)
        /// </summary>
        public static int RampMinimumBlueColorLevelStart
        {
            get
            {
                return _MinimumBlueColorLevelStart;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _MinimumBlueColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Ramp] The minimum color level (255 colors or 16 colors - start)
        /// </summary>
        public static int RampMinimumColorLevelStart
        {
            get
            {
                return _MinimumColorLevelStart;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                _MinimumColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Ramp] The maximum red color level (true color - start)
        /// </summary>
        public static int RampMaximumRedColorLevelStart
        {
            get
            {
                return _MaximumRedColorLevelStart;
            }
            set
            {
                if (value <= _MinimumRedColorLevelStart)
                    value = _MinimumRedColorLevelStart;
                if (value > 255)
                    value = 255;
                _MaximumRedColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Ramp] The maximum green color level (true color - start)
        /// </summary>
        public static int RampMaximumGreenColorLevelStart
        {
            get
            {
                return _MaximumGreenColorLevelStart;
            }
            set
            {
                if (value <= _MinimumGreenColorLevelStart)
                    value = _MinimumGreenColorLevelStart;
                if (value > 255)
                    value = 255;
                _MaximumGreenColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Ramp] The maximum blue color level (true color - start)
        /// </summary>
        public static int RampMaximumBlueColorLevelStart
        {
            get
            {
                return _MaximumBlueColorLevelStart;
            }
            set
            {
                if (value <= _MinimumBlueColorLevelStart)
                    value = _MinimumBlueColorLevelStart;
                if (value > 255)
                    value = 255;
                _MaximumBlueColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Ramp] The maximum color level (255 colors or 16 colors - start)
        /// </summary>
        public static int RampMaximumColorLevelStart
        {
            get
            {
                return _MaximumColorLevelStart;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= _MinimumColorLevelStart)
                    value = _MinimumColorLevelStart;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                _MaximumColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Ramp] The minimum red color level (true color - end)
        /// </summary>
        public static int RampMinimumRedColorLevelEnd
        {
            get
            {
                return _MinimumRedColorLevelEnd;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _MinimumRedColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Ramp] The minimum green color level (true color - end)
        /// </summary>
        public static int RampMinimumGreenColorLevelEnd
        {
            get
            {
                return _MinimumGreenColorLevelEnd;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _MinimumGreenColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Ramp] The minimum blue color level (true color - end)
        /// </summary>
        public static int RampMinimumBlueColorLevelEnd
        {
            get
            {
                return _MinimumBlueColorLevelEnd;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _MinimumBlueColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Ramp] The minimum color level (255 colors or 16 colors - end)
        /// </summary>
        public static int RampMinimumColorLevelEnd
        {
            get
            {
                return _MinimumColorLevelEnd;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                _MinimumColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Ramp] The maximum red color level (true color - end)
        /// </summary>
        public static int RampMaximumRedColorLevelEnd
        {
            get
            {
                return _MaximumRedColorLevelEnd;
            }
            set
            {
                if (value <= _MinimumRedColorLevelEnd)
                    value = _MinimumRedColorLevelEnd;
                if (value > 255)
                    value = 255;
                _MaximumRedColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Ramp] The maximum green color level (true color - end)
        /// </summary>
        public static int RampMaximumGreenColorLevelEnd
        {
            get
            {
                return _MaximumGreenColorLevelEnd;
            }
            set
            {
                if (value <= _MinimumGreenColorLevelEnd)
                    value = _MinimumGreenColorLevelEnd;
                if (value > 255)
                    value = 255;
                _MaximumGreenColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Ramp] The maximum blue color level (true color - end)
        /// </summary>
        public static int RampMaximumBlueColorLevelEnd
        {
            get
            {
                return _MaximumBlueColorLevelEnd;
            }
            set
            {
                if (value <= _MinimumBlueColorLevelEnd)
                    value = _MinimumBlueColorLevelEnd;
                if (value > 255)
                    value = 255;
                _MaximumBlueColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Ramp] The maximum color level (255 colors or 16 colors - end)
        /// </summary>
        public static int RampMaximumColorLevelEnd
        {
            get
            {
                return _MaximumColorLevelEnd;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= _MinimumColorLevelEnd)
                    value = _MinimumColorLevelEnd;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                _MaximumColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Ramp] Upper left corner color.
        /// </summary>
        public static string RampUpperLeftCornerColor
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
        /// [Ramp] Upper right corner color.
        /// </summary>
        public static string RampUpperRightCornerColor
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
        /// [Ramp] Lower left corner color.
        /// </summary>
        public static string RampLowerLeftCornerColor
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
        /// [Ramp] Lower right corner color.
        /// </summary>
        public static string RampLowerRightCornerColor
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
        /// [Ramp] Upper frame color.
        /// </summary>
        public static string RampUpperFrameColor
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
        /// [Ramp] Lower frame color.
        /// </summary>
        public static string RampLowerFrameColor
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
        /// [Ramp] Left frame color.
        /// </summary>
        public static string RampLeftFrameColor
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
        /// [Ramp] Right frame color.
        /// </summary>
        public static string RampRightFrameColor
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
        /// [Ramp] Use the border colors.
        /// </summary>
        public static bool RampUseBorderColors
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
    /// Display code for Ramp
    /// </summary>
    public class RampDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Ramp";

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
            int RedColorNumFrom = RandomDriver.Random(RampSettings.RampMinimumRedColorLevelStart, RampSettings.RampMaximumRedColorLevelStart);
            int GreenColorNumFrom = RandomDriver.Random(RampSettings.RampMinimumGreenColorLevelStart, RampSettings.RampMaximumGreenColorLevelStart);
            int BlueColorNumFrom = RandomDriver.Random(RampSettings.RampMinimumBlueColorLevelStart, RampSettings.RampMaximumBlueColorLevelStart);
            int ColorNumFrom = RandomDriver.Random(RampSettings.RampMinimumColorLevelStart, RampSettings.RampMaximumColorLevelStart);
            int RedColorNumTo = RandomDriver.Random(RampSettings.RampMinimumRedColorLevelEnd, RampSettings.RampMaximumRedColorLevelEnd);
            int GreenColorNumTo = RandomDriver.Random(RampSettings.RampMinimumGreenColorLevelEnd, RampSettings.RampMaximumGreenColorLevelEnd);
            int BlueColorNumTo = RandomDriver.Random(RampSettings.RampMinimumBlueColorLevelEnd, RampSettings.RampMaximumBlueColorLevelEnd);
            int ColorNumTo = RandomDriver.Random(RampSettings.RampMinimumColorLevelEnd, RampSettings.RampMaximumColorLevelEnd);

            // Console resizing can sometimes cause the cursor to remain visible. This happens on Windows 10's terminal.
            ConsoleWrapper.CursorVisible = false;

            // Set start and end widths for the ramp frame
            int RampFrameStartWidth = 4;
            int RampFrameEndWidth = ConsoleWrapper.WindowWidth - RampFrameStartWidth;
            int RampFrameSpaces = RampFrameEndWidth - RampFrameStartWidth;
            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Start width: {0}, End width: {1}, Spaces: {2}", RampFrameStartWidth, RampFrameEndWidth, RampFrameSpaces);

            // Set thresholds for color ramps
            int RampColorRedThreshold = RedColorNumFrom - RedColorNumTo;
            int RampColorGreenThreshold = GreenColorNumFrom - GreenColorNumTo;
            int RampColorBlueThreshold = BlueColorNumFrom - BlueColorNumTo;
            int RampColorThreshold = ColorNumFrom - ColorNumTo;
            double RampColorRedSteps = RampColorRedThreshold / (double)RampFrameSpaces;
            double RampColorGreenSteps = RampColorGreenThreshold / (double)RampFrameSpaces;
            double RampColorBlueSteps = RampColorBlueThreshold / (double)RampFrameSpaces;
            double RampColorSteps = RampColorThreshold / (double)RampFrameSpaces;
            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Set thresholds (RGB: {0};{1};{2} | Normal: {3})", RampColorRedThreshold, RampColorGreenThreshold, RampColorBlueThreshold, RampColorThreshold);
            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Steps by {0} spaces (RGB: {1};{2};{3} | Normal: {4})", RampFrameSpaces, RampColorRedSteps, RampColorGreenSteps, RampColorBlueSteps, RampColorSteps);

            // Let the ramp be printed in the center
            int RampCenterPosition = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d);
            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Center position: {0}", RampCenterPosition);

            // Set the current positions
            int RampCurrentPositionLeft = RampFrameStartWidth + 1;
            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Current left position: {0}", RampCurrentPositionLeft);

            // Draw the frame
            if (!ConsoleResizeListener.WasResized(false))
            {
                TextWriterWhereColor.WriteWhere(RampSettings.RampUpperLeftCornerChar, RampFrameStartWidth, RampCenterPosition - 2, false, RampSettings.RampUseBorderColors ? new Color(RampSettings.RampUpperLeftCornerColor) : new Color((int)ConsoleColors.Gray));
                TextWriterColor.Write(RampSettings.RampUpperFrameChar.Repeat(RampFrameSpaces), false, RampSettings.RampUseBorderColors ? new Color(RampSettings.RampUpperFrameColor) : new Color((int)ConsoleColors.Gray));
                TextWriterColor.Write(RampSettings.RampUpperRightCornerChar, false, RampSettings.RampUseBorderColors ? new Color(RampSettings.RampUpperRightCornerColor) : new Color((int)ConsoleColors.Gray));
                TextWriterWhereColor.WriteWhere(RampSettings.RampLeftFrameChar, RampFrameStartWidth, RampCenterPosition - 1, false, RampSettings.RampUseBorderColors ? new Color(RampSettings.RampLeftFrameColor) : new Color((int)ConsoleColors.Gray));
                TextWriterWhereColor.WriteWhere(RampSettings.RampLeftFrameChar, RampFrameStartWidth, RampCenterPosition, false, RampSettings.RampUseBorderColors ? new Color(RampSettings.RampLeftFrameColor) : new Color((int)ConsoleColors.Gray));
                TextWriterWhereColor.WriteWhere(RampSettings.RampLeftFrameChar, RampFrameStartWidth, RampCenterPosition + 1, false, RampSettings.RampUseBorderColors ? new Color(RampSettings.RampLeftFrameColor) : new Color((int)ConsoleColors.Gray));
                TextWriterWhereColor.WriteWhere(RampSettings.RampRightFrameChar, RampFrameEndWidth + 1, RampCenterPosition - 1, false, RampSettings.RampUseBorderColors ? new Color(RampSettings.RampLeftFrameColor) : new Color((int)ConsoleColors.Gray));
                TextWriterWhereColor.WriteWhere(RampSettings.RampRightFrameChar, RampFrameEndWidth + 1, RampCenterPosition, false, RampSettings.RampUseBorderColors ? new Color(RampSettings.RampLeftFrameColor) : new Color((int)ConsoleColors.Gray));
                TextWriterWhereColor.WriteWhere(RampSettings.RampRightFrameChar, RampFrameEndWidth + 1, RampCenterPosition + 1, false, RampSettings.RampUseBorderColors ? new Color(RampSettings.RampLeftFrameColor) : new Color((int)ConsoleColors.Gray));
                TextWriterWhereColor.WriteWhere(RampSettings.RampLowerLeftCornerChar, RampFrameStartWidth, RampCenterPosition + 2, false, RampSettings.RampUseBorderColors ? new Color(RampSettings.RampLowerLeftCornerColor) : new Color((int)ConsoleColors.Gray));
                TextWriterColor.Write(RampSettings.RampLowerFrameChar.Repeat(RampFrameSpaces), false, RampSettings.RampUseBorderColors ? new Color(RampSettings.RampLowerFrameColor) : new Color((int)ConsoleColors.Gray));
                TextWriterColor.Write(RampSettings.RampLowerRightCornerChar, false, RampSettings.RampUseBorderColors ? new Color(RampSettings.RampLowerRightCornerColor) : new Color((int)ConsoleColors.Gray));
            }

            // Draw the ramp
            if (RampSettings.RampTrueColor)
            {
                // Set the current colors
                double RampCurrentColorRed = RedColorNumFrom;
                double RampCurrentColorGreen = GreenColorNumFrom;
                double RampCurrentColorBlue = BlueColorNumFrom;
                var RampCurrentColorInstance = new Color($"{Convert.ToInt32(RampCurrentColorRed)};{Convert.ToInt32(RampCurrentColorGreen)};{Convert.ToInt32(RampCurrentColorBlue)}");

                // Set the console color and fill the ramp!
                ColorTools.SetConsoleColor(RampCurrentColorInstance, true, true);
                while (!(Convert.ToInt32(RampCurrentColorRed) == RedColorNumTo & Convert.ToInt32(RampCurrentColorGreen) == GreenColorNumTo & Convert.ToInt32(RampCurrentColorBlue) == BlueColorNumTo))
                {
                    if (ConsoleResizeListener.WasResized(false))
                        break;
                    ConsoleWrapper.SetCursorPosition(RampCurrentPositionLeft, RampCenterPosition - 1);
                    ConsoleWrapper.Write(' ');
                    ConsoleWrapper.SetCursorPosition(RampCurrentPositionLeft, RampCenterPosition);
                    ConsoleWrapper.Write(' ');
                    ConsoleWrapper.SetCursorPosition(RampCurrentPositionLeft, RampCenterPosition + 1);
                    ConsoleWrapper.Write(' ');
                    RampCurrentPositionLeft = ConsoleWrapper.CursorLeft;

                    // Change the colors
                    RampCurrentColorRed -= RampColorRedSteps;
                    RampCurrentColorGreen -= RampColorGreenSteps;
                    RampCurrentColorBlue -= RampColorBlueSteps;
                    DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got new current colors (R;G;B: {0};{1};{2}) subtracting from {3};{4};{5}", RampCurrentColorRed, RampCurrentColorGreen, RampCurrentColorBlue, RampColorRedSteps, RampColorGreenSteps, RampColorBlueSteps);
                    RampCurrentColorInstance = new Color($"{Convert.ToInt32(RampCurrentColorRed)};{Convert.ToInt32(RampCurrentColorGreen)};{Convert.ToInt32(RampCurrentColorBlue)}");
                    ColorTools.SetConsoleColor(RampCurrentColorInstance, true, true);

                    // Delay writing
                    ThreadManager.SleepNoBlock(RampSettings.RampDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                }
            }
            else
            {
                // Set the current colors
                double RampCurrentColor = ColorNumFrom;
                var RampCurrentColorInstance = new Color(Convert.ToInt32(RampCurrentColor));

                // Set the console color and fill the ramp!
                ColorTools.SetConsoleColor(RampCurrentColorInstance, true, true);
                while (Convert.ToInt32(RampCurrentColor) != ColorNumTo)
                {
                    if (ConsoleResizeListener.WasResized(false))
                        break;
                    ConsoleWrapper.SetCursorPosition(RampCurrentPositionLeft, RampCenterPosition - 1);
                    ConsoleWrapper.Write(' ');
                    ConsoleWrapper.SetCursorPosition(RampCurrentPositionLeft, RampCenterPosition);
                    ConsoleWrapper.Write(' ');
                    ConsoleWrapper.SetCursorPosition(RampCurrentPositionLeft, RampCenterPosition + 1);
                    ConsoleWrapper.Write(' ');
                    RampCurrentPositionLeft = ConsoleWrapper.CursorLeft;

                    // Change the colors
                    RampCurrentColor -= RampColorSteps;
                    DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got new current colors (Normal: {0}) subtracting from {1}", RampCurrentColor, RampColorSteps);
                    RampCurrentColorInstance = new Color(Convert.ToInt32(RampCurrentColor));
                    ColorTools.SetConsoleColor(RampCurrentColorInstance, true, true);

                    // Delay writing
                    ThreadManager.SleepNoBlock(RampSettings.RampDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                }
            }
            ThreadManager.SleepNoBlock(RampSettings.RampNextRampDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            ConsoleWrapper.BackgroundColor = ConsoleColor.Black;
            ConsoleWrapper.Clear();
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(RampSettings.RampDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
