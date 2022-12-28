
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

        private static bool _rampTrueColor = true;
        private static int _rampDelay = 20;
        private static int _rampNextRampDelay = 250;
        private static string _rampUpperLeftCornerChar = "╔";
        private static string _rampUpperRightCornerChar = "╗";
        private static string _rampLowerLeftCornerChar = "╚";
        private static string _rampLowerRightCornerChar = "╝";
        private static string _rampUpperFrameChar = "═";
        private static string _rampLowerFrameChar = "═";
        private static string _rampLeftFrameChar = "║";
        private static string _rampRightFrameChar = "║";
        private static int _rampMinimumRedColorLevelStart = 0;
        private static int _rampMinimumGreenColorLevelStart = 0;
        private static int _rampMinimumBlueColorLevelStart = 0;
        private static int _rampMinimumColorLevelStart = 0;
        private static int _rampMaximumRedColorLevelStart = 255;
        private static int _rampMaximumGreenColorLevelStart = 255;
        private static int _rampMaximumBlueColorLevelStart = 255;
        private static int _rampMaximumColorLevelStart = 255;
        private static int _rampMinimumRedColorLevelEnd = 0;
        private static int _rampMinimumGreenColorLevelEnd = 0;
        private static int _rampMinimumBlueColorLevelEnd = 0;
        private static int _rampMinimumColorLevelEnd = 0;
        private static int _rampMaximumRedColorLevelEnd = 255;
        private static int _rampMaximumGreenColorLevelEnd = 255;
        private static int _rampMaximumBlueColorLevelEnd = 255;
        private static int _rampMaximumColorLevelEnd = 255;
        private static string _rampUpperLeftCornerColor = 7.ToString();
        private static string _rampUpperRightCornerColor = 7.ToString();
        private static string _rampLowerLeftCornerColor = 7.ToString();
        private static string _rampLowerRightCornerColor = 7.ToString();
        private static string _rampUpperFrameColor = 7.ToString();
        private static string _rampLowerFrameColor = 7.ToString();
        private static string _rampLeftFrameColor = 7.ToString();
        private static string _rampRightFrameColor = 7.ToString();
        private static bool _rampUseBorderColors;

        /// <summary>
        /// [Ramp] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool RampTrueColor
        {
            get
            {
                return _rampTrueColor;
            }
            set
            {
                _rampTrueColor = value;
            }
        }
        /// <summary>
        /// [Ramp] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int RampDelay
        {
            get
            {
                return _rampDelay;
            }
            set
            {
                if (value <= 0)
                    value = 20;
                _rampDelay = value;
            }
        }
        /// <summary>
        /// [Ramp] How many milliseconds to wait before starting the next ramp?
        /// </summary>
        public static int RampNextRampDelay
        {
            get
            {
                return _rampNextRampDelay;
            }
            set
            {
                if (value <= 0)
                    value = 250;
                _rampNextRampDelay = value;
            }
        }
        /// <summary>
        /// [Ramp] Upper left corner character 
        /// </summary>
        public static string RampUpperLeftCornerChar
        {
            get
            {
                return _rampUpperLeftCornerChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╔";
                _rampUpperLeftCornerChar = value;
            }
        }
        /// <summary>
        /// [Ramp] Upper right corner character 
        /// </summary>
        public static string RampUpperRightCornerChar
        {
            get
            {
                return _rampUpperRightCornerChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╗";
                _rampUpperRightCornerChar = value;
            }
        }
        /// <summary>
        /// [Ramp] Lower left corner character 
        /// </summary>
        public static string RampLowerLeftCornerChar
        {
            get
            {
                return _rampLowerLeftCornerChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╚";
                _rampLowerLeftCornerChar = value;
            }
        }
        /// <summary>
        /// [Ramp] Lower right corner character 
        /// </summary>
        public static string RampLowerRightCornerChar
        {
            get
            {
                return _rampLowerRightCornerChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╝";
                _rampLowerRightCornerChar = value;
            }
        }
        /// <summary>
        /// [Ramp] Upper frame character 
        /// </summary>
        public static string RampUpperFrameChar
        {
            get
            {
                return _rampUpperFrameChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "═";
                _rampUpperFrameChar = value;
            }
        }
        /// <summary>
        /// [Ramp] Lower frame character 
        /// </summary>
        public static string RampLowerFrameChar
        {
            get
            {
                return _rampLowerFrameChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "═";
                _rampLowerFrameChar = value;
            }
        }
        /// <summary>
        /// [Ramp] Left frame character 
        /// </summary>
        public static string RampLeftFrameChar
        {
            get
            {
                return _rampLeftFrameChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "║";
                _rampLeftFrameChar = value;
            }
        }
        /// <summary>
        /// [Ramp] Right frame character 
        /// </summary>
        public static string RampRightFrameChar
        {
            get
            {
                return _rampRightFrameChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "║";
                _rampRightFrameChar = value;
            }
        }
        /// <summary>
        /// [Ramp] The minimum red color level (true color - start)
        /// </summary>
        public static int RampMinimumRedColorLevelStart
        {
            get
            {
                return _rampMinimumRedColorLevelStart;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _rampMinimumRedColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Ramp] The minimum green color level (true color - start)
        /// </summary>
        public static int RampMinimumGreenColorLevelStart
        {
            get
            {
                return _rampMinimumGreenColorLevelStart;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _rampMinimumGreenColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Ramp] The minimum blue color level (true color - start)
        /// </summary>
        public static int RampMinimumBlueColorLevelStart
        {
            get
            {
                return _rampMinimumBlueColorLevelStart;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _rampMinimumBlueColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Ramp] The minimum color level (255 colors or 16 colors - start)
        /// </summary>
        public static int RampMinimumColorLevelStart
        {
            get
            {
                return _rampMinimumColorLevelStart;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                _rampMinimumColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Ramp] The maximum red color level (true color - start)
        /// </summary>
        public static int RampMaximumRedColorLevelStart
        {
            get
            {
                return _rampMaximumRedColorLevelStart;
            }
            set
            {
                if (value <= _rampMinimumRedColorLevelStart)
                    value = _rampMinimumRedColorLevelStart;
                if (value > 255)
                    value = 255;
                _rampMaximumRedColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Ramp] The maximum green color level (true color - start)
        /// </summary>
        public static int RampMaximumGreenColorLevelStart
        {
            get
            {
                return _rampMaximumGreenColorLevelStart;
            }
            set
            {
                if (value <= _rampMinimumGreenColorLevelStart)
                    value = _rampMinimumGreenColorLevelStart;
                if (value > 255)
                    value = 255;
                _rampMaximumGreenColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Ramp] The maximum blue color level (true color - start)
        /// </summary>
        public static int RampMaximumBlueColorLevelStart
        {
            get
            {
                return _rampMaximumBlueColorLevelStart;
            }
            set
            {
                if (value <= _rampMinimumBlueColorLevelStart)
                    value = _rampMinimumBlueColorLevelStart;
                if (value > 255)
                    value = 255;
                _rampMaximumBlueColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Ramp] The maximum color level (255 colors or 16 colors - start)
        /// </summary>
        public static int RampMaximumColorLevelStart
        {
            get
            {
                return _rampMaximumColorLevelStart;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= _rampMinimumColorLevelStart)
                    value = _rampMinimumColorLevelStart;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                _rampMaximumColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Ramp] The minimum red color level (true color - end)
        /// </summary>
        public static int RampMinimumRedColorLevelEnd
        {
            get
            {
                return _rampMinimumRedColorLevelEnd;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _rampMinimumRedColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Ramp] The minimum green color level (true color - end)
        /// </summary>
        public static int RampMinimumGreenColorLevelEnd
        {
            get
            {
                return _rampMinimumGreenColorLevelEnd;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _rampMinimumGreenColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Ramp] The minimum blue color level (true color - end)
        /// </summary>
        public static int RampMinimumBlueColorLevelEnd
        {
            get
            {
                return _rampMinimumBlueColorLevelEnd;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _rampMinimumBlueColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Ramp] The minimum color level (255 colors or 16 colors - end)
        /// </summary>
        public static int RampMinimumColorLevelEnd
        {
            get
            {
                return _rampMinimumColorLevelEnd;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                _rampMinimumColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Ramp] The maximum red color level (true color - end)
        /// </summary>
        public static int RampMaximumRedColorLevelEnd
        {
            get
            {
                return _rampMaximumRedColorLevelEnd;
            }
            set
            {
                if (value <= _rampMinimumRedColorLevelEnd)
                    value = _rampMinimumRedColorLevelEnd;
                if (value > 255)
                    value = 255;
                _rampMaximumRedColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Ramp] The maximum green color level (true color - end)
        /// </summary>
        public static int RampMaximumGreenColorLevelEnd
        {
            get
            {
                return _rampMaximumGreenColorLevelEnd;
            }
            set
            {
                if (value <= _rampMinimumGreenColorLevelEnd)
                    value = _rampMinimumGreenColorLevelEnd;
                if (value > 255)
                    value = 255;
                _rampMaximumGreenColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Ramp] The maximum blue color level (true color - end)
        /// </summary>
        public static int RampMaximumBlueColorLevelEnd
        {
            get
            {
                return _rampMaximumBlueColorLevelEnd;
            }
            set
            {
                if (value <= _rampMinimumBlueColorLevelEnd)
                    value = _rampMinimumBlueColorLevelEnd;
                if (value > 255)
                    value = 255;
                _rampMaximumBlueColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Ramp] The maximum color level (255 colors or 16 colors - end)
        /// </summary>
        public static int RampMaximumColorLevelEnd
        {
            get
            {
                return _rampMaximumColorLevelEnd;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= _rampMinimumColorLevelEnd)
                    value = _rampMinimumColorLevelEnd;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                _rampMaximumColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Ramp] Upper left corner color.
        /// </summary>
        public static string RampUpperLeftCornerColor
        {
            get
            {
                return _rampUpperLeftCornerColor;
            }
            set
            {
                _rampUpperLeftCornerColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [Ramp] Upper right corner color.
        /// </summary>
        public static string RampUpperRightCornerColor
        {
            get
            {
                return _rampUpperRightCornerColor;
            }
            set
            {
                _rampUpperRightCornerColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [Ramp] Lower left corner color.
        /// </summary>
        public static string RampLowerLeftCornerColor
        {
            get
            {
                return _rampLowerLeftCornerColor;
            }
            set
            {
                _rampLowerLeftCornerColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [Ramp] Lower right corner color.
        /// </summary>
        public static string RampLowerRightCornerColor
        {
            get
            {
                return _rampLowerRightCornerColor;
            }
            set
            {
                _rampLowerRightCornerColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [Ramp] Upper frame color.
        /// </summary>
        public static string RampUpperFrameColor
        {
            get
            {
                return _rampUpperFrameColor;
            }
            set
            {
                _rampUpperFrameColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [Ramp] Lower frame color.
        /// </summary>
        public static string RampLowerFrameColor
        {
            get
            {
                return _rampLowerFrameColor;
            }
            set
            {
                _rampLowerFrameColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [Ramp] Left frame color.
        /// </summary>
        public static string RampLeftFrameColor
        {
            get
            {
                return _rampLeftFrameColor;
            }
            set
            {
                _rampLeftFrameColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [Ramp] Right frame color.
        /// </summary>
        public static string RampRightFrameColor
        {
            get
            {
                return _rampRightFrameColor;
            }
            set
            {
                _rampRightFrameColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [Ramp] Use the border colors.
        /// </summary>
        public static bool RampUseBorderColors
        {
            get
            {
                return _rampUseBorderColors;
            }
            set
            {
                _rampUseBorderColors = value;
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
