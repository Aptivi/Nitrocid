
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
    /// Settings for BarRot
    /// </summary>
    public static class BarRotSettings
    {

        /// <summary>
        /// [BarRot] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool BarRotTrueColor
        {
            get
            {
                return Config.SaverConfig.BarRotTrueColor;
            }
            set
            {
                Config.SaverConfig.BarRotTrueColor = value;
            }
        }
        /// <summary>
        /// [BarRot] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int BarRotDelay
        {
            get
            {
                return Config.SaverConfig.BarRotDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                Config.SaverConfig.BarRotDelay = value;
            }
        }
        /// <summary>
        /// [BarRot] How many milliseconds to wait before rotting the next ramp's one end?
        /// </summary>
        public static int BarRotNextRampDelay
        {
            get
            {
                return Config.SaverConfig.BarRotNextRampDelay;
            }
            set
            {
                if (value <= 0)
                    value = 250;
                Config.SaverConfig.BarRotNextRampDelay = value;
            }
        }
        /// <summary>
        /// [BarRot] Upper left corner character 
        /// </summary>
        public static string BarRotUpperLeftCornerChar
        {
            get
            {
                return Config.SaverConfig.BarRotUpperLeftCornerChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╔";
                Config.SaverConfig.BarRotUpperLeftCornerChar = value;
            }
        }
        /// <summary>
        /// [BarRot] Upper right corner character 
        /// </summary>
        public static string BarRotUpperRightCornerChar
        {
            get
            {
                return Config.SaverConfig.BarRotUpperRightCornerChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╗";
                Config.SaverConfig.BarRotUpperRightCornerChar = value;
            }
        }
        /// <summary>
        /// [BarRot] Lower left corner character 
        /// </summary>
        public static string BarRotLowerLeftCornerChar
        {
            get
            {
                return Config.SaverConfig.BarRotLowerLeftCornerChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╚";
                Config.SaverConfig.BarRotLowerLeftCornerChar = value;
            }
        }
        /// <summary>
        /// [BarRot] Lower right corner character 
        /// </summary>
        public static string BarRotLowerRightCornerChar
        {
            get
            {
                return Config.SaverConfig.BarRotLowerRightCornerChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╝";
                Config.SaverConfig.BarRotLowerRightCornerChar = value;
            }
        }
        /// <summary>
        /// [BarRot] Upper frame character 
        /// </summary>
        public static string BarRotUpperFrameChar
        {
            get
            {
                return Config.SaverConfig.BarRotUpperFrameChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "═";
                Config.SaverConfig.BarRotUpperFrameChar = value;
            }
        }
        /// <summary>
        /// [BarRot] Lower frame character 
        /// </summary>
        public static string BarRotLowerFrameChar
        {
            get
            {
                return Config.SaverConfig.BarRotLowerFrameChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "═";
                Config.SaverConfig.BarRotLowerFrameChar = value;
            }
        }
        /// <summary>
        /// [BarRot] Left frame character 
        /// </summary>
        public static string BarRotLeftFrameChar
        {
            get
            {
                return Config.SaverConfig.BarRotLeftFrameChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "║";
                Config.SaverConfig.BarRotLeftFrameChar = value;
            }
        }
        /// <summary>
        /// [BarRot] Right frame character 
        /// </summary>
        public static string BarRotRightFrameChar
        {
            get
            {
                return Config.SaverConfig.BarRotRightFrameChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "║";
                Config.SaverConfig.BarRotRightFrameChar = value;
            }
        }
        /// <summary>
        /// [BarRot] The minimum red color level (true color - start)
        /// </summary>
        public static int BarRotMinimumRedColorLevelStart
        {
            get
            {
                return Config.SaverConfig.BarRotMinimumRedColorLevelStart;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.BarRotMinimumRedColorLevelStart = value;
            }
        }
        /// <summary>
        /// [BarRot] The minimum green color level (true color - start)
        /// </summary>
        public static int BarRotMinimumGreenColorLevelStart
        {
            get
            {
                return Config.SaverConfig.BarRotMinimumGreenColorLevelStart;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.BarRotMinimumGreenColorLevelStart = value;
            }
        }
        /// <summary>
        /// [BarRot] The minimum blue color level (true color - start)
        /// </summary>
        public static int BarRotMinimumBlueColorLevelStart
        {
            get
            {
                return Config.SaverConfig.BarRotMinimumBlueColorLevelStart;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.BarRotMinimumBlueColorLevelStart = value;
            }
        }
        /// <summary>
        /// [BarRot] The maximum red color level (true color - start)
        /// </summary>
        public static int BarRotMaximumRedColorLevelStart
        {
            get
            {
                return Config.SaverConfig.BarRotMaximumRedColorLevelStart;
            }
            set
            {
                if (value <= Config.SaverConfig.BarRotMinimumRedColorLevelStart)
                    value = Config.SaverConfig.BarRotMinimumRedColorLevelStart;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.BarRotMaximumRedColorLevelStart = value;
            }
        }
        /// <summary>
        /// [BarRot] The maximum green color level (true color - start)
        /// </summary>
        public static int BarRotMaximumGreenColorLevelStart
        {
            get
            {
                return Config.SaverConfig.BarRotMaximumGreenColorLevelStart;
            }
            set
            {
                if (value <= Config.SaverConfig.BarRotMinimumGreenColorLevelStart)
                    value = Config.SaverConfig.BarRotMinimumGreenColorLevelStart;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.BarRotMaximumGreenColorLevelStart = value;
            }
        }
        /// <summary>
        /// [BarRot] The maximum blue color level (true color - start)
        /// </summary>
        public static int BarRotMaximumBlueColorLevelStart
        {
            get
            {
                return Config.SaverConfig.BarRotMaximumBlueColorLevelStart;
            }
            set
            {
                if (value <= Config.SaverConfig.BarRotMinimumBlueColorLevelStart)
                    value = Config.SaverConfig.BarRotMinimumBlueColorLevelStart;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.BarRotMaximumBlueColorLevelStart = value;
            }
        }
        /// <summary>
        /// [BarRot] The minimum red color level (true color - end)
        /// </summary>
        public static int BarRotMinimumRedColorLevelEnd
        {
            get
            {
                return Config.SaverConfig.BarRotMinimumRedColorLevelEnd;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.BarRotMinimumRedColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [BarRot] The minimum green color level (true color - end)
        /// </summary>
        public static int BarRotMinimumGreenColorLevelEnd
        {
            get
            {
                return Config.SaverConfig.BarRotMinimumGreenColorLevelEnd;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.BarRotMinimumGreenColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [BarRot] The minimum blue color level (true color - end)
        /// </summary>
        public static int BarRotMinimumBlueColorLevelEnd
        {
            get
            {
                return Config.SaverConfig.BarRotMinimumBlueColorLevelEnd;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.BarRotMinimumBlueColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [BarRot] The maximum red color level (true color - end)
        /// </summary>
        public static int BarRotMaximumRedColorLevelEnd
        {
            get
            {
                return Config.SaverConfig.BarRotMaximumRedColorLevelEnd;
            }
            set
            {
                if (value <= Config.SaverConfig.BarRotMinimumRedColorLevelEnd)
                    value = Config.SaverConfig.BarRotMinimumRedColorLevelEnd;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.BarRotMaximumRedColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [BarRot] The maximum green color level (true color - end)
        /// </summary>
        public static int BarRotMaximumGreenColorLevelEnd
        {
            get
            {
                return Config.SaverConfig.BarRotMaximumGreenColorLevelEnd;
            }
            set
            {
                if (value <= Config.SaverConfig.BarRotMinimumGreenColorLevelEnd)
                    value = Config.SaverConfig.BarRotMinimumGreenColorLevelEnd;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.BarRotMaximumGreenColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [BarRot] The maximum blue color level (true color - end)
        /// </summary>
        public static int BarRotMaximumBlueColorLevelEnd
        {
            get
            {
                return Config.SaverConfig.BarRotMaximumBlueColorLevelEnd;
            }
            set
            {
                if (value <= Config.SaverConfig.BarRotMinimumBlueColorLevelEnd)
                    value = Config.SaverConfig.BarRotMinimumBlueColorLevelEnd;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.BarRotMaximumBlueColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [BarRot] Upper left corner color.
        /// </summary>
        public static string BarRotUpperLeftCornerColor
        {
            get
            {
                return Config.SaverConfig.BarRotUpperLeftCornerColor;
            }
            set
            {
                Config.SaverConfig.BarRotUpperLeftCornerColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [BarRot] Upper right corner color.
        /// </summary>
        public static string BarRotUpperRightCornerColor
        {
            get
            {
                return Config.SaverConfig.BarRotUpperRightCornerColor;
            }
            set
            {
                Config.SaverConfig.BarRotUpperRightCornerColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [BarRot] Lower left corner color.
        /// </summary>
        public static string BarRotLowerLeftCornerColor
        {
            get
            {
                return Config.SaverConfig.BarRotLowerLeftCornerColor;
            }
            set
            {
                Config.SaverConfig.BarRotLowerLeftCornerColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [BarRot] Lower right corner color.
        /// </summary>
        public static string BarRotLowerRightCornerColor
        {
            get
            {
                return Config.SaverConfig.BarRotLowerRightCornerColor;
            }
            set
            {
                Config.SaverConfig.BarRotLowerRightCornerColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [BarRot] Upper frame color.
        /// </summary>
        public static string BarRotUpperFrameColor
        {
            get
            {
                return Config.SaverConfig.BarRotUpperFrameColor;
            }
            set
            {
                Config.SaverConfig.BarRotUpperFrameColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [BarRot] Lower frame color.
        /// </summary>
        public static string BarRotLowerFrameColor
        {
            get
            {
                return Config.SaverConfig.BarRotLowerFrameColor;
            }
            set
            {
                Config.SaverConfig.BarRotLowerFrameColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [BarRot] Left frame color.
        /// </summary>
        public static string BarRotLeftFrameColor
        {
            get
            {
                return Config.SaverConfig.BarRotLeftFrameColor;
            }
            set
            {
                Config.SaverConfig.BarRotLeftFrameColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [BarRot] Right frame color.
        /// </summary>
        public static string BarRotRightFrameColor
        {
            get
            {
                return Config.SaverConfig.BarRotRightFrameColor;
            }
            set
            {
                Config.SaverConfig.BarRotRightFrameColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [BarRot] Use the border colors.
        /// </summary>
        public static bool BarRotUseBorderColors
        {
            get
            {
                return Config.SaverConfig.BarRotUseBorderColors;
            }
            set
            {
                Config.SaverConfig.BarRotUseBorderColors = value;
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
        public override void ScreensaverPreparation() => 
            ColorTools.LoadBack();

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
                // TODO: Deal with these, too.
                TextWriterWhereColor.WriteWhere(BarRotSettings.BarRotUpperLeftCornerChar, RampFrameStartWidth, RampCenterPosition - 2, false, BarRotSettings.BarRotUseBorderColors ? new Color(BarRotSettings.BarRotUpperLeftCornerColor) : ColorTools.GetGray());
                TextWriterColor.Write(new string(BarRotSettings.BarRotUpperFrameChar[0], RampFrameSpaces), false, BarRotSettings.BarRotUseBorderColors ? new Color(BarRotSettings.BarRotUpperFrameColor) : ColorTools.GetGray());
                TextWriterColor.Write(BarRotSettings.BarRotUpperRightCornerChar, false, BarRotSettings.BarRotUseBorderColors ? new Color(BarRotSettings.BarRotUpperRightCornerColor) : ColorTools.GetGray());
                TextWriterWhereColor.WriteWhere(BarRotSettings.BarRotLeftFrameChar, RampFrameStartWidth, RampCenterPosition - 1, false, BarRotSettings.BarRotUseBorderColors ? new Color(BarRotSettings.BarRotLeftFrameColor) : ColorTools.GetGray());
                TextWriterWhereColor.WriteWhere(BarRotSettings.BarRotLeftFrameChar, RampFrameStartWidth, RampCenterPosition, false, BarRotSettings.BarRotUseBorderColors ? new Color(BarRotSettings.BarRotLeftFrameColor) : ColorTools.GetGray());
                TextWriterWhereColor.WriteWhere(BarRotSettings.BarRotLeftFrameChar, RampFrameStartWidth, RampCenterPosition + 1, false, BarRotSettings.BarRotUseBorderColors ? new Color(BarRotSettings.BarRotLeftFrameColor) : ColorTools.GetGray());
                TextWriterWhereColor.WriteWhere(BarRotSettings.BarRotRightFrameChar, RampFrameEndWidth + 1, RampCenterPosition - 1, false, BarRotSettings.BarRotUseBorderColors ? new Color(BarRotSettings.BarRotLeftFrameColor) : ColorTools.GetGray());
                TextWriterWhereColor.WriteWhere(BarRotSettings.BarRotRightFrameChar, RampFrameEndWidth + 1, RampCenterPosition, false, BarRotSettings.BarRotUseBorderColors ? new Color(BarRotSettings.BarRotLeftFrameColor) : ColorTools.GetGray());
                TextWriterWhereColor.WriteWhere(BarRotSettings.BarRotRightFrameChar, RampFrameEndWidth + 1, RampCenterPosition + 1, false, BarRotSettings.BarRotUseBorderColors ? new Color(BarRotSettings.BarRotLeftFrameColor) : ColorTools.GetGray());
                TextWriterWhereColor.WriteWhere(BarRotSettings.BarRotLowerLeftCornerChar, RampFrameStartWidth, RampCenterPosition + 2, false, BarRotSettings.BarRotUseBorderColors ? new Color(BarRotSettings.BarRotLowerLeftCornerColor) : ColorTools.GetGray());
                TextWriterColor.Write(new string(BarRotSettings.BarRotLowerFrameChar[0], RampFrameSpaces), false, BarRotSettings.BarRotUseBorderColors ? new Color(BarRotSettings.BarRotLowerFrameColor) : ColorTools.GetGray());
                TextWriterColor.Write(BarRotSettings.BarRotLowerRightCornerChar, false, BarRotSettings.BarRotUseBorderColors ? new Color(BarRotSettings.BarRotLowerRightCornerColor) : ColorTools.GetGray());
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
