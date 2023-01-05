
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
    /// Settings for BarRot
    /// </summary>
    public static class BarRotSettings
    {

        private static bool _TrueColor = true;
        private static int _Delay = 10;
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
        private static int _MaximumRedColorLevelStart = 255;
        private static int _MaximumGreenColorLevelStart = 255;
        private static int _MaximumBlueColorLevelStart = 255;
        private static int _MinimumRedColorLevelEnd = 0;
        private static int _MinimumGreenColorLevelEnd = 0;
        private static int _MinimumBlueColorLevelEnd = 0;
        private static int _MaximumRedColorLevelEnd = 255;
        private static int _MaximumGreenColorLevelEnd = 255;
        private static int _MaximumBlueColorLevelEnd = 255;
        private static string _UpperLeftCornerColor = "192;192;192";
        private static string _UpperRightCornerColor = "192;192;192";
        private static string _LowerLeftCornerColor = "192;192;192";
        private static string _LowerRightCornerColor = "192;192;192";
        private static string _UpperFrameColor = "192;192;192";
        private static string _LowerFrameColor = "192;192;192";
        private static string _LeftFrameColor = "192;192;192";
        private static string _RightFrameColor = "192;192;192";
        private static bool _UseBorderColors;

        /// <summary>
        /// [BarRot] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool BarRotTrueColor
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
        /// [BarRot] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int BarRotDelay
        {
            get
            {
                return _Delay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                _Delay = value;
            }
        }
        /// <summary>
        /// [BarRot] How many milliseconds to wait before rotting the next ramp's one end?
        /// </summary>
        public static int BarRotNextRampDelay
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
        /// [BarRot] Upper left corner character 
        /// </summary>
        public static string BarRotUpperLeftCornerChar
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
        /// [BarRot] Upper right corner character 
        /// </summary>
        public static string BarRotUpperRightCornerChar
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
        /// [BarRot] Lower left corner character 
        /// </summary>
        public static string BarRotLowerLeftCornerChar
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
        /// [BarRot] Lower right corner character 
        /// </summary>
        public static string BarRotLowerRightCornerChar
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
        /// [BarRot] Upper frame character 
        /// </summary>
        public static string BarRotUpperFrameChar
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
        /// [BarRot] Lower frame character 
        /// </summary>
        public static string BarRotLowerFrameChar
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
        /// [BarRot] Left frame character 
        /// </summary>
        public static string BarRotLeftFrameChar
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
        /// [BarRot] Right frame character 
        /// </summary>
        public static string BarRotRightFrameChar
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
        /// [BarRot] The minimum red color level (true color - start)
        /// </summary>
        public static int BarRotMinimumRedColorLevelStart
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
        /// [BarRot] The minimum green color level (true color - start)
        /// </summary>
        public static int BarRotMinimumGreenColorLevelStart
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
        /// [BarRot] The minimum blue color level (true color - start)
        /// </summary>
        public static int BarRotMinimumBlueColorLevelStart
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
        /// [BarRot] The maximum red color level (true color - start)
        /// </summary>
        public static int BarRotMaximumRedColorLevelStart
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
        /// [BarRot] The maximum green color level (true color - start)
        /// </summary>
        public static int BarRotMaximumGreenColorLevelStart
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
        /// [BarRot] The maximum blue color level (true color - start)
        /// </summary>
        public static int BarRotMaximumBlueColorLevelStart
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
        /// [BarRot] The minimum red color level (true color - end)
        /// </summary>
        public static int BarRotMinimumRedColorLevelEnd
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
        /// [BarRot] The minimum green color level (true color - end)
        /// </summary>
        public static int BarRotMinimumGreenColorLevelEnd
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
        /// [BarRot] The minimum blue color level (true color - end)
        /// </summary>
        public static int BarRotMinimumBlueColorLevelEnd
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
        /// [BarRot] The maximum red color level (true color - end)
        /// </summary>
        public static int BarRotMaximumRedColorLevelEnd
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
        /// [BarRot] The maximum green color level (true color - end)
        /// </summary>
        public static int BarRotMaximumGreenColorLevelEnd
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
        /// [BarRot] The maximum blue color level (true color - end)
        /// </summary>
        public static int BarRotMaximumBlueColorLevelEnd
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
        /// [BarRot] Upper left corner color.
        /// </summary>
        public static string BarRotUpperLeftCornerColor
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
        /// [BarRot] Upper right corner color.
        /// </summary>
        public static string BarRotUpperRightCornerColor
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
        /// [BarRot] Lower left corner color.
        /// </summary>
        public static string BarRotLowerLeftCornerColor
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
        /// [BarRot] Lower right corner color.
        /// </summary>
        public static string BarRotLowerRightCornerColor
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
        /// [BarRot] Upper frame color.
        /// </summary>
        public static string BarRotUpperFrameColor
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
        /// [BarRot] Lower frame color.
        /// </summary>
        public static string BarRotLowerFrameColor
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
        /// [BarRot] Left frame color.
        /// </summary>
        public static string BarRotLeftFrameColor
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
        /// [BarRot] Right frame color.
        /// </summary>
        public static string BarRotRightFrameColor
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
        /// [BarRot] Use the border colors.
        /// </summary>
        public static bool BarRotUseBorderColors
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
    /// Display code for BarRot
    /// </summary>
    public class BarRotDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "BarRot";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ConsoleWrapper.BackgroundColor = ConsoleColor.Black;
            ConsoleWrapper.ForegroundColor = ConsoleColor.White;
            ConsoleWrapper.Clear();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Select a color range for the ramp
            int RedColorNumFrom = RandomDriver.Random(BarRotSettings.BarRotMinimumRedColorLevelStart, BarRotSettings.BarRotMaximumRedColorLevelStart);
            int GreenColorNumFrom = RandomDriver.Random(BarRotSettings.BarRotMinimumGreenColorLevelStart, BarRotSettings.BarRotMaximumGreenColorLevelStart);
            int BlueColorNumFrom = RandomDriver.Random(BarRotSettings.BarRotMinimumBlueColorLevelStart, BarRotSettings.BarRotMaximumBlueColorLevelStart);
            int RedColorNumTo = RandomDriver.Random(BarRotSettings.BarRotMinimumRedColorLevelEnd, BarRotSettings.BarRotMaximumRedColorLevelEnd);
            int GreenColorNumTo = RandomDriver.Random(BarRotSettings.BarRotMinimumGreenColorLevelEnd, BarRotSettings.BarRotMaximumGreenColorLevelEnd);
            int BlueColorNumTo = RandomDriver.Random(BarRotSettings.BarRotMinimumBlueColorLevelEnd, BarRotSettings.BarRotMaximumBlueColorLevelEnd);
            DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got color from (R;G;B: {0};{1};{2}) to (R;G;B: {3};{4};{5})", RedColorNumFrom, GreenColorNumFrom, BlueColorNumFrom, RedColorNumTo, GreenColorNumTo, BlueColorNumTo);

            // Set start and end widths for the ramp frame
            int RampFrameStartWidth = 4;
            int RampFrameEndWidth = ConsoleWrapper.WindowWidth - RampFrameStartWidth;
            int RampFrameSpaces = RampFrameEndWidth - RampFrameStartWidth;
            DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Start width: {0}, End width: {1}, Spaces: {2}", RampFrameStartWidth, RampFrameEndWidth, RampFrameSpaces);

            // Set thresholds for color ramp
            int RampColorRedThreshold = RedColorNumFrom - RedColorNumTo;
            int RampColorGreenThreshold = GreenColorNumFrom - GreenColorNumTo;
            int RampColorBlueThreshold = BlueColorNumFrom - BlueColorNumTo;
            double RampColorRedSteps = RampColorRedThreshold / (double)RampFrameSpaces;
            double RampColorGreenSteps = RampColorGreenThreshold / (double)RampFrameSpaces;
            double RampColorBlueSteps = RampColorBlueThreshold / (double)RampFrameSpaces;
            DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Set thresholds (RGB: {0};{1};{2})", RampColorRedThreshold, RampColorGreenThreshold, RampColorBlueThreshold);
            DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Steps by {0} spaces (RGB: {1};{2};{3})", RampFrameSpaces, RampColorRedSteps, RampColorGreenSteps, RampColorBlueSteps);

            // Let the ramp be printed in the center
            int RampCenterPosition = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d);
            DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Center position: {0}", RampCenterPosition);

            // Set the current positions
            int RampCurrentPositionLeft = RampFrameStartWidth + 1;
            DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Current left position: {0}", RampCurrentPositionLeft);

            // Draw the frame
            if (!ConsoleResizeListener.WasResized(false))
            {
                TextWriterWhereColor.WriteWhere(BarRotSettings.BarRotUpperLeftCornerChar, RampFrameStartWidth, RampCenterPosition - 2, false, BarRotSettings.BarRotUseBorderColors ? new Color(BarRotSettings.BarRotUpperLeftCornerColor) : new Color((int)ConsoleColors.Gray));
                TextWriterColor.Write(BarRotSettings.BarRotUpperFrameChar.Repeat(RampFrameSpaces), false, BarRotSettings.BarRotUseBorderColors ? new Color(BarRotSettings.BarRotUpperFrameColor) : new Color((int)ConsoleColors.Gray));
                TextWriterColor.Write(BarRotSettings.BarRotUpperRightCornerChar, false, BarRotSettings.BarRotUseBorderColors ? new Color(BarRotSettings.BarRotUpperRightCornerColor) : new Color((int)ConsoleColors.Gray));
                TextWriterWhereColor.WriteWhere(BarRotSettings.BarRotLeftFrameChar, RampFrameStartWidth, RampCenterPosition - 1, false, BarRotSettings.BarRotUseBorderColors ? new Color(BarRotSettings.BarRotLeftFrameColor) : new Color((int)ConsoleColors.Gray));
                TextWriterWhereColor.WriteWhere(BarRotSettings.BarRotLeftFrameChar, RampFrameStartWidth, RampCenterPosition, false, BarRotSettings.BarRotUseBorderColors ? new Color(BarRotSettings.BarRotLeftFrameColor) : new Color((int)ConsoleColors.Gray));
                TextWriterWhereColor.WriteWhere(BarRotSettings.BarRotLeftFrameChar, RampFrameStartWidth, RampCenterPosition + 1, false, BarRotSettings.BarRotUseBorderColors ? new Color(BarRotSettings.BarRotLeftFrameColor) : new Color((int)ConsoleColors.Gray));
                TextWriterWhereColor.WriteWhere(BarRotSettings.BarRotRightFrameChar, RampFrameEndWidth + 1, RampCenterPosition - 1, false, BarRotSettings.BarRotUseBorderColors ? new Color(BarRotSettings.BarRotLeftFrameColor) : new Color((int)ConsoleColors.Gray));
                TextWriterWhereColor.WriteWhere(BarRotSettings.BarRotRightFrameChar, RampFrameEndWidth + 1, RampCenterPosition, false, BarRotSettings.BarRotUseBorderColors ? new Color(BarRotSettings.BarRotLeftFrameColor) : new Color((int)ConsoleColors.Gray));
                TextWriterWhereColor.WriteWhere(BarRotSettings.BarRotRightFrameChar, RampFrameEndWidth + 1, RampCenterPosition + 1, false, BarRotSettings.BarRotUseBorderColors ? new Color(BarRotSettings.BarRotLeftFrameColor) : new Color((int)ConsoleColors.Gray));
                TextWriterWhereColor.WriteWhere(BarRotSettings.BarRotLowerLeftCornerChar, RampFrameStartWidth, RampCenterPosition + 2, false, BarRotSettings.BarRotUseBorderColors ? new Color(BarRotSettings.BarRotLowerLeftCornerColor) : new Color((int)ConsoleColors.Gray));
                TextWriterColor.Write(BarRotSettings.BarRotLowerFrameChar.Repeat(RampFrameSpaces), false, BarRotSettings.BarRotUseBorderColors ? new Color(BarRotSettings.BarRotLowerFrameColor) : new Color((int)ConsoleColors.Gray));
                TextWriterColor.Write(BarRotSettings.BarRotLowerRightCornerChar, false, BarRotSettings.BarRotUseBorderColors ? new Color(BarRotSettings.BarRotLowerRightCornerColor) : new Color((int)ConsoleColors.Gray));
            }

            // Set the current colors
            double RampCurrentColorRed = RedColorNumFrom;
            double RampCurrentColorGreen = GreenColorNumFrom;
            double RampCurrentColorBlue = BlueColorNumFrom;

            // Set the console color and fill the ramp!
            while (!(Convert.ToInt32(RampCurrentColorRed) == RedColorNumTo & Convert.ToInt32(RampCurrentColorGreen) == GreenColorNumTo & Convert.ToInt32(RampCurrentColorBlue) == BlueColorNumTo))
            {
                if (ConsoleResizeListener.WasResized(false))
                    break;

                // Populate the variables for sub-gradients
                int RampSubgradientRedColorNumFrom = RedColorNumFrom;
                int RampSubgradientGreenColorNumFrom = GreenColorNumFrom;
                int RampSubgradientBlueColorNumFrom = BlueColorNumFrom;
                int RampSubgradientRedColorNumTo = (int)Math.Round(RampCurrentColorRed);
                int RampSubgradientGreenColorNumTo = (int)Math.Round(RampCurrentColorGreen);
                int RampSubgradientBlueColorNumTo = (int)Math.Round(RampCurrentColorBlue);
                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got subgradient color from (R;G;B: {0};{1};{2}) to (R;G;B: {3};{4};{5})", RampSubgradientRedColorNumFrom, RampSubgradientGreenColorNumFrom, RampSubgradientBlueColorNumFrom, RampSubgradientRedColorNumTo, RampSubgradientGreenColorNumTo, RampSubgradientBlueColorNumTo);

                // Set the sub-gradient current colors
                double RampSubgradientCurrentColorRed = RampSubgradientRedColorNumFrom;
                double RampSubgradientCurrentColorGreen = RampSubgradientGreenColorNumFrom;
                double RampSubgradientCurrentColorBlue = RampSubgradientBlueColorNumFrom;
                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got subgradient current colors (R;G;B: {0};{1};{2})", RampSubgradientCurrentColorRed, RampSubgradientCurrentColorGreen, RampSubgradientCurrentColorBlue);

                // Set the sub-gradient thresholds
                int RampSubgradientColorRedThreshold = RampSubgradientRedColorNumFrom - RampSubgradientRedColorNumTo;
                int RampSubgradientColorGreenThreshold = RampSubgradientGreenColorNumFrom - RampSubgradientGreenColorNumTo;
                int RampSubgradientColorBlueThreshold = RampSubgradientBlueColorNumFrom - RampSubgradientBlueColorNumTo;
                double RampSubgradientColorRedSteps = RampSubgradientColorRedThreshold / (double)RampFrameSpaces;
                double RampSubgradientColorGreenSteps = RampSubgradientColorGreenThreshold / (double)RampFrameSpaces;
                double RampSubgradientColorBlueSteps = RampSubgradientColorBlueThreshold / (double)RampFrameSpaces;
                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Set subgradient thresholds (RGB: {0};{1};{2})", RampSubgradientColorRedThreshold, RampSubgradientColorGreenThreshold, RampSubgradientColorBlueThreshold);
                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Steps by {0} spaces for subgradient (RGB: {1};{2};{3})", RampFrameSpaces, RampSubgradientColorRedSteps, RampSubgradientColorGreenSteps, RampSubgradientColorBlueSteps);

                // Make a new instance
                var RampSubgradientCurrentColorInstance = new Color($"{Convert.ToInt32(RampSubgradientCurrentColorRed)};{Convert.ToInt32(RampSubgradientCurrentColorGreen)};{Convert.ToInt32(RampSubgradientCurrentColorBlue)}");
                ColorTools.SetConsoleColor(RampSubgradientCurrentColorInstance, true, true);

                // Try to fill the ramp
                int RampSubgradientStepsMade = 0;
                while (RampSubgradientStepsMade != RampFrameSpaces)
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
                    RampSubgradientStepsMade += 1;

                    // Change the colors
                    RampSubgradientCurrentColorRed -= RampSubgradientColorRedSteps;
                    RampSubgradientCurrentColorGreen -= RampSubgradientColorGreenSteps;
                    RampSubgradientCurrentColorBlue -= RampSubgradientColorBlueSteps;
                    DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got new subgradient current colors (R;G;B: {0};{1};{2}) subtracting from {3};{4};{5}", RampSubgradientCurrentColorRed, RampSubgradientCurrentColorGreen, RampSubgradientCurrentColorBlue, RampSubgradientColorRedSteps, RampSubgradientColorGreenSteps, RampSubgradientColorBlueSteps);
                    RampSubgradientCurrentColorInstance = new Color($"{Convert.ToInt32(RampSubgradientCurrentColorRed)};{Convert.ToInt32(RampSubgradientCurrentColorGreen)};{Convert.ToInt32(RampSubgradientCurrentColorBlue)}");
                    ColorTools.SetConsoleColor(RampSubgradientCurrentColorInstance, true, true);
                }

                // Change the colors
                RampCurrentColorRed -= RampColorRedSteps;
                RampCurrentColorGreen -= RampColorGreenSteps;
                RampCurrentColorBlue -= RampColorBlueSteps;
                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got new current colors (R;G;B: {0};{1};{2}) subtracting from {3};{4};{5}", RampCurrentColorRed, RampCurrentColorGreen, RampCurrentColorBlue, RampColorRedSteps, RampColorGreenSteps, RampColorBlueSteps);

                // Delay writing
                RampCurrentPositionLeft = RampFrameStartWidth + 1;
                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Current left position: {0}", RampCurrentPositionLeft);
                ThreadManager.SleepNoBlock(BarRotSettings.BarRotDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            }
            if (!ConsoleResizeListener.WasResized(false))
                ThreadManager.SleepNoBlock(BarRotSettings.BarRotNextRampDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);

            // Clear the scene
            ConsoleWrapper.BackgroundColor = ConsoleColor.Black;
            ConsoleWrapper.Clear();

            // Reset resize sync
            ConsoleResizeListener.WasResized();
        }

    }
}
