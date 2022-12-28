
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

        private static bool _barRotTrueColor = true;
        private static int _barRotDelay = 10;
        private static int _barRotNextRampDelay = 250;
        private static string _barRotUpperLeftCornerChar = "╔";
        private static string _barRotUpperRightCornerChar = "╗";
        private static string _barRotLowerLeftCornerChar = "╚";
        private static string _barRotLowerRightCornerChar = "╝";
        private static string _barRotUpperFrameChar = "═";
        private static string _barRotLowerFrameChar = "═";
        private static string _barRotLeftFrameChar = "║";
        private static string _barRotRightFrameChar = "║";
        private static int _barRotMinimumRedColorLevelStart = 0;
        private static int _barRotMinimumGreenColorLevelStart = 0;
        private static int _barRotMinimumBlueColorLevelStart = 0;
        private static int _barRotMaximumRedColorLevelStart = 255;
        private static int _barRotMaximumGreenColorLevelStart = 255;
        private static int _barRotMaximumBlueColorLevelStart = 255;
        private static int _barRotMinimumRedColorLevelEnd = 0;
        private static int _barRotMinimumGreenColorLevelEnd = 0;
        private static int _barRotMinimumBlueColorLevelEnd = 0;
        private static int _barRotMaximumRedColorLevelEnd = 255;
        private static int _barRotMaximumGreenColorLevelEnd = 255;
        private static int _barRotMaximumBlueColorLevelEnd = 255;
        private static string _barRotUpperLeftCornerColor = "192;192;192";
        private static string _barRotUpperRightCornerColor = "192;192;192";
        private static string _barRotLowerLeftCornerColor = "192;192;192";
        private static string _barRotLowerRightCornerColor = "192;192;192";
        private static string _barRotUpperFrameColor = "192;192;192";
        private static string _barRotLowerFrameColor = "192;192;192";
        private static string _barRotLeftFrameColor = "192;192;192";
        private static string _barRotRightFrameColor = "192;192;192";
        private static bool _barRotUseBorderColors;

        /// <summary>
        /// [BarRot] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool BarRotTrueColor
        {
            get
            {
                return _barRotTrueColor;
            }
            set
            {
                _barRotTrueColor = value;
            }
        }
        /// <summary>
        /// [BarRot] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int BarRotDelay
        {
            get
            {
                return _barRotDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                _barRotDelay = value;
            }
        }
        /// <summary>
        /// [BarRot] How many milliseconds to wait before rotting the next ramp's one end?
        /// </summary>
        public static int BarRotNextRampDelay
        {
            get
            {
                return _barRotNextRampDelay;
            }
            set
            {
                if (value <= 0)
                    value = 250;
                _barRotNextRampDelay = value;
            }
        }
        /// <summary>
        /// [BarRot] Upper left corner character 
        /// </summary>
        public static string BarRotUpperLeftCornerChar
        {
            get
            {
                return _barRotUpperLeftCornerChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╔";
                _barRotUpperLeftCornerChar = value;
            }
        }
        /// <summary>
        /// [BarRot] Upper right corner character 
        /// </summary>
        public static string BarRotUpperRightCornerChar
        {
            get
            {
                return _barRotUpperRightCornerChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╗";
                _barRotUpperRightCornerChar = value;
            }
        }
        /// <summary>
        /// [BarRot] Lower left corner character 
        /// </summary>
        public static string BarRotLowerLeftCornerChar
        {
            get
            {
                return _barRotLowerLeftCornerChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╚";
                _barRotLowerLeftCornerChar = value;
            }
        }
        /// <summary>
        /// [BarRot] Lower right corner character 
        /// </summary>
        public static string BarRotLowerRightCornerChar
        {
            get
            {
                return _barRotLowerRightCornerChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╝";
                _barRotLowerRightCornerChar = value;
            }
        }
        /// <summary>
        /// [BarRot] Upper frame character 
        /// </summary>
        public static string BarRotUpperFrameChar
        {
            get
            {
                return _barRotUpperFrameChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "═";
                _barRotUpperFrameChar = value;
            }
        }
        /// <summary>
        /// [BarRot] Lower frame character 
        /// </summary>
        public static string BarRotLowerFrameChar
        {
            get
            {
                return _barRotLowerFrameChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "═";
                _barRotLowerFrameChar = value;
            }
        }
        /// <summary>
        /// [BarRot] Left frame character 
        /// </summary>
        public static string BarRotLeftFrameChar
        {
            get
            {
                return _barRotLeftFrameChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "║";
                _barRotLeftFrameChar = value;
            }
        }
        /// <summary>
        /// [BarRot] Right frame character 
        /// </summary>
        public static string BarRotRightFrameChar
        {
            get
            {
                return _barRotRightFrameChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "║";
                _barRotRightFrameChar = value;
            }
        }
        /// <summary>
        /// [BarRot] The minimum red color level (true color - start)
        /// </summary>
        public static int BarRotMinimumRedColorLevelStart
        {
            get
            {
                return _barRotMinimumRedColorLevelStart;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _barRotMinimumRedColorLevelStart = value;
            }
        }
        /// <summary>
        /// [BarRot] The minimum green color level (true color - start)
        /// </summary>
        public static int BarRotMinimumGreenColorLevelStart
        {
            get
            {
                return _barRotMinimumGreenColorLevelStart;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _barRotMinimumGreenColorLevelStart = value;
            }
        }
        /// <summary>
        /// [BarRot] The minimum blue color level (true color - start)
        /// </summary>
        public static int BarRotMinimumBlueColorLevelStart
        {
            get
            {
                return _barRotMinimumBlueColorLevelStart;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _barRotMinimumBlueColorLevelStart = value;
            }
        }
        /// <summary>
        /// [BarRot] The maximum red color level (true color - start)
        /// </summary>
        public static int BarRotMaximumRedColorLevelStart
        {
            get
            {
                return _barRotMaximumRedColorLevelStart;
            }
            set
            {
                if (value <= _barRotMinimumRedColorLevelStart)
                    value = _barRotMinimumRedColorLevelStart;
                if (value > 255)
                    value = 255;
                _barRotMaximumRedColorLevelStart = value;
            }
        }
        /// <summary>
        /// [BarRot] The maximum green color level (true color - start)
        /// </summary>
        public static int BarRotMaximumGreenColorLevelStart
        {
            get
            {
                return _barRotMaximumGreenColorLevelStart;
            }
            set
            {
                if (value <= _barRotMinimumGreenColorLevelStart)
                    value = _barRotMinimumGreenColorLevelStart;
                if (value > 255)
                    value = 255;
                _barRotMaximumGreenColorLevelStart = value;
            }
        }
        /// <summary>
        /// [BarRot] The maximum blue color level (true color - start)
        /// </summary>
        public static int BarRotMaximumBlueColorLevelStart
        {
            get
            {
                return _barRotMaximumBlueColorLevelStart;
            }
            set
            {
                if (value <= _barRotMinimumBlueColorLevelStart)
                    value = _barRotMinimumBlueColorLevelStart;
                if (value > 255)
                    value = 255;
                _barRotMaximumBlueColorLevelStart = value;
            }
        }
        /// <summary>
        /// [BarRot] The minimum red color level (true color - end)
        /// </summary>
        public static int BarRotMinimumRedColorLevelEnd
        {
            get
            {
                return _barRotMinimumRedColorLevelEnd;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _barRotMinimumRedColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [BarRot] The minimum green color level (true color - end)
        /// </summary>
        public static int BarRotMinimumGreenColorLevelEnd
        {
            get
            {
                return _barRotMinimumGreenColorLevelEnd;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _barRotMinimumGreenColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [BarRot] The minimum blue color level (true color - end)
        /// </summary>
        public static int BarRotMinimumBlueColorLevelEnd
        {
            get
            {
                return _barRotMinimumBlueColorLevelEnd;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _barRotMinimumBlueColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [BarRot] The maximum red color level (true color - end)
        /// </summary>
        public static int BarRotMaximumRedColorLevelEnd
        {
            get
            {
                return _barRotMaximumRedColorLevelEnd;
            }
            set
            {
                if (value <= _barRotMinimumRedColorLevelEnd)
                    value = _barRotMinimumRedColorLevelEnd;
                if (value > 255)
                    value = 255;
                _barRotMaximumRedColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [BarRot] The maximum green color level (true color - end)
        /// </summary>
        public static int BarRotMaximumGreenColorLevelEnd
        {
            get
            {
                return _barRotMaximumGreenColorLevelEnd;
            }
            set
            {
                if (value <= _barRotMinimumGreenColorLevelEnd)
                    value = _barRotMinimumGreenColorLevelEnd;
                if (value > 255)
                    value = 255;
                _barRotMaximumGreenColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [BarRot] The maximum blue color level (true color - end)
        /// </summary>
        public static int BarRotMaximumBlueColorLevelEnd
        {
            get
            {
                return _barRotMaximumBlueColorLevelEnd;
            }
            set
            {
                if (value <= _barRotMinimumBlueColorLevelEnd)
                    value = _barRotMinimumBlueColorLevelEnd;
                if (value > 255)
                    value = 255;
                _barRotMaximumBlueColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [BarRot] Upper left corner color.
        /// </summary>
        public static string BarRotUpperLeftCornerColor
        {
            get
            {
                return _barRotUpperLeftCornerColor;
            }
            set
            {
                _barRotUpperLeftCornerColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [BarRot] Upper right corner color.
        /// </summary>
        public static string BarRotUpperRightCornerColor
        {
            get
            {
                return _barRotUpperRightCornerColor;
            }
            set
            {
                _barRotUpperRightCornerColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [BarRot] Lower left corner color.
        /// </summary>
        public static string BarRotLowerLeftCornerColor
        {
            get
            {
                return _barRotLowerLeftCornerColor;
            }
            set
            {
                _barRotLowerLeftCornerColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [BarRot] Lower right corner color.
        /// </summary>
        public static string BarRotLowerRightCornerColor
        {
            get
            {
                return _barRotLowerRightCornerColor;
            }
            set
            {
                _barRotLowerRightCornerColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [BarRot] Upper frame color.
        /// </summary>
        public static string BarRotUpperFrameColor
        {
            get
            {
                return _barRotUpperFrameColor;
            }
            set
            {
                _barRotUpperFrameColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [BarRot] Lower frame color.
        /// </summary>
        public static string BarRotLowerFrameColor
        {
            get
            {
                return _barRotLowerFrameColor;
            }
            set
            {
                _barRotLowerFrameColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [BarRot] Left frame color.
        /// </summary>
        public static string BarRotLeftFrameColor
        {
            get
            {
                return _barRotLeftFrameColor;
            }
            set
            {
                _barRotLeftFrameColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [BarRot] Right frame color.
        /// </summary>
        public static string BarRotRightFrameColor
        {
            get
            {
                return _barRotRightFrameColor;
            }
            set
            {
                _barRotRightFrameColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [BarRot] Use the border colors.
        /// </summary>
        public static bool BarRotUseBorderColors
        {
            get
            {
                return _barRotUseBorderColors;
            }
            set
            {
                _barRotUseBorderColors = value;
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
            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color from (R;G;B: {0};{1};{2}) to (R;G;B: {3};{4};{5})", RedColorNumFrom, GreenColorNumFrom, BlueColorNumFrom, RedColorNumTo, GreenColorNumTo, BlueColorNumTo);

            // Set start and end widths for the ramp frame
            int RampFrameStartWidth = 4;
            int RampFrameEndWidth = ConsoleWrapper.WindowWidth - RampFrameStartWidth;
            int RampFrameSpaces = RampFrameEndWidth - RampFrameStartWidth;
            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Start width: {0}, End width: {1}, Spaces: {2}", RampFrameStartWidth, RampFrameEndWidth, RampFrameSpaces);

            // Set thresholds for color ramp
            int RampColorRedThreshold = RedColorNumFrom - RedColorNumTo;
            int RampColorGreenThreshold = GreenColorNumFrom - GreenColorNumTo;
            int RampColorBlueThreshold = BlueColorNumFrom - BlueColorNumTo;
            double RampColorRedSteps = RampColorRedThreshold / (double)RampFrameSpaces;
            double RampColorGreenSteps = RampColorGreenThreshold / (double)RampFrameSpaces;
            double RampColorBlueSteps = RampColorBlueThreshold / (double)RampFrameSpaces;
            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Set thresholds (RGB: {0};{1};{2})", RampColorRedThreshold, RampColorGreenThreshold, RampColorBlueThreshold);
            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Steps by {0} spaces (RGB: {1};{2};{3})", RampFrameSpaces, RampColorRedSteps, RampColorGreenSteps, RampColorBlueSteps);

            // Let the ramp be printed in the center
            int RampCenterPosition = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d);
            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Center position: {0}", RampCenterPosition);

            // Set the current positions
            int RampCurrentPositionLeft = RampFrameStartWidth + 1;
            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Current left position: {0}", RampCurrentPositionLeft);

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
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got subgradient color from (R;G;B: {0};{1};{2}) to (R;G;B: {3};{4};{5})", RampSubgradientRedColorNumFrom, RampSubgradientGreenColorNumFrom, RampSubgradientBlueColorNumFrom, RampSubgradientRedColorNumTo, RampSubgradientGreenColorNumTo, RampSubgradientBlueColorNumTo);

                // Set the sub-gradient current colors
                double RampSubgradientCurrentColorRed = RampSubgradientRedColorNumFrom;
                double RampSubgradientCurrentColorGreen = RampSubgradientGreenColorNumFrom;
                double RampSubgradientCurrentColorBlue = RampSubgradientBlueColorNumFrom;
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got subgradient current colors (R;G;B: {0};{1};{2})", RampSubgradientCurrentColorRed, RampSubgradientCurrentColorGreen, RampSubgradientCurrentColorBlue);

                // Set the sub-gradient thresholds
                int RampSubgradientColorRedThreshold = RampSubgradientRedColorNumFrom - RampSubgradientRedColorNumTo;
                int RampSubgradientColorGreenThreshold = RampSubgradientGreenColorNumFrom - RampSubgradientGreenColorNumTo;
                int RampSubgradientColorBlueThreshold = RampSubgradientBlueColorNumFrom - RampSubgradientBlueColorNumTo;
                double RampSubgradientColorRedSteps = RampSubgradientColorRedThreshold / (double)RampFrameSpaces;
                double RampSubgradientColorGreenSteps = RampSubgradientColorGreenThreshold / (double)RampFrameSpaces;
                double RampSubgradientColorBlueSteps = RampSubgradientColorBlueThreshold / (double)RampFrameSpaces;
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Set subgradient thresholds (RGB: {0};{1};{2})", RampSubgradientColorRedThreshold, RampSubgradientColorGreenThreshold, RampSubgradientColorBlueThreshold);
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Steps by {0} spaces for subgradient (RGB: {1};{2};{3})", RampFrameSpaces, RampSubgradientColorRedSteps, RampSubgradientColorGreenSteps, RampSubgradientColorBlueSteps);

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
                    DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got new subgradient current colors (R;G;B: {0};{1};{2}) subtracting from {3};{4};{5}", RampSubgradientCurrentColorRed, RampSubgradientCurrentColorGreen, RampSubgradientCurrentColorBlue, RampSubgradientColorRedSteps, RampSubgradientColorGreenSteps, RampSubgradientColorBlueSteps);
                    RampSubgradientCurrentColorInstance = new Color($"{Convert.ToInt32(RampSubgradientCurrentColorRed)};{Convert.ToInt32(RampSubgradientCurrentColorGreen)};{Convert.ToInt32(RampSubgradientCurrentColorBlue)}");
                    ColorTools.SetConsoleColor(RampSubgradientCurrentColorInstance, true, true);
                }

                // Change the colors
                RampCurrentColorRed -= RampColorRedSteps;
                RampCurrentColorGreen -= RampColorGreenSteps;
                RampCurrentColorBlue -= RampColorBlueSteps;
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got new current colors (R;G;B: {0};{1};{2}) subtracting from {3};{4};{5}", RampCurrentColorRed, RampCurrentColorGreen, RampCurrentColorBlue, RampColorRedSteps, RampColorGreenSteps, RampColorBlueSteps);

                // Delay writing
                RampCurrentPositionLeft = RampFrameStartWidth + 1;
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Current left position: {0}", RampCurrentPositionLeft);
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
