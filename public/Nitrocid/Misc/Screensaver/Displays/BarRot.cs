
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
using Terminaux.Colors;

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
            get => Config.SaverConfig.BarRotTrueColor;
            set => Config.SaverConfig.BarRotTrueColor = value;
        }
        /// <summary>
        /// [BarRot] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int BarRotDelay
        {
            get => Config.SaverConfig.BarRotDelay;
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
            get => Config.SaverConfig.BarRotNextRampDelay;
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
        public static char BarRotUpperLeftCornerChar
        {
            get => Config.SaverConfig.BarRotUpperLeftCornerChar;
            set => Config.SaverConfig.BarRotUpperLeftCornerChar = value;
        }
        /// <summary>
        /// [BarRot] Upper right corner character 
        /// </summary>
        public static char BarRotUpperRightCornerChar
        {
            get => Config.SaverConfig.BarRotUpperRightCornerChar;
            set => Config.SaverConfig.BarRotUpperRightCornerChar = value;
        }
        /// <summary>
        /// [BarRot] Lower left corner character 
        /// </summary>
        public static char BarRotLowerLeftCornerChar
        {
            get => Config.SaverConfig.BarRotLowerLeftCornerChar;
            set => Config.SaverConfig.BarRotLowerLeftCornerChar = value;
        }
        /// <summary>
        /// [BarRot] Lower right corner character 
        /// </summary>
        public static char BarRotLowerRightCornerChar
        {
            get => Config.SaverConfig.BarRotLowerRightCornerChar;
            set => Config.SaverConfig.BarRotLowerRightCornerChar = value;
        }
        /// <summary>
        /// [BarRot] Upper frame character 
        /// </summary>
        public static char BarRotUpperFrameChar
        {
            get => Config.SaverConfig.BarRotUpperFrameChar;
            set => Config.SaverConfig.BarRotUpperFrameChar = value;
        }
        /// <summary>
        /// [BarRot] Lower frame character 
        /// </summary>
        public static char BarRotLowerFrameChar
        {
            get => Config.SaverConfig.BarRotLowerFrameChar;
            set => Config.SaverConfig.BarRotLowerFrameChar = value;
        }
        /// <summary>
        /// [BarRot] Left frame character 
        /// </summary>
        public static char BarRotLeftFrameChar
        {
            get => Config.SaverConfig.BarRotLeftFrameChar;
            set => Config.SaverConfig.BarRotLeftFrameChar = value;
        }
        /// <summary>
        /// [BarRot] Right frame character 
        /// </summary>
        public static char BarRotRightFrameChar
        {
            get => Config.SaverConfig.BarRotRightFrameChar;
            set => Config.SaverConfig.BarRotRightFrameChar = value;
        }
        /// <summary>
        /// [BarRot] The minimum red color level (true color - start)
        /// </summary>
        public static int BarRotMinimumRedColorLevelStart
        {
            get => Config.SaverConfig.BarRotMinimumRedColorLevelStart;
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
            get => Config.SaverConfig.BarRotMinimumGreenColorLevelStart;
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
            get => Config.SaverConfig.BarRotMinimumBlueColorLevelStart;
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
            get => Config.SaverConfig.BarRotMaximumRedColorLevelStart;
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
            get => Config.SaverConfig.BarRotMaximumGreenColorLevelStart;
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
            get => Config.SaverConfig.BarRotMaximumBlueColorLevelStart;
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
            get => Config.SaverConfig.BarRotMinimumRedColorLevelEnd;
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
            get => Config.SaverConfig.BarRotMinimumGreenColorLevelEnd;
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
            get => Config.SaverConfig.BarRotMinimumBlueColorLevelEnd;
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
            get => Config.SaverConfig.BarRotMaximumRedColorLevelEnd;
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
            get => Config.SaverConfig.BarRotMaximumGreenColorLevelEnd;
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
            get => Config.SaverConfig.BarRotMaximumBlueColorLevelEnd;
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
            get => Config.SaverConfig.BarRotUpperLeftCornerColor;
            set => Config.SaverConfig.BarRotUpperLeftCornerColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [BarRot] Upper right corner color.
        /// </summary>
        public static string BarRotUpperRightCornerColor
        {
            get => Config.SaverConfig.BarRotUpperRightCornerColor;
            set => Config.SaverConfig.BarRotUpperRightCornerColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [BarRot] Lower left corner color.
        /// </summary>
        public static string BarRotLowerLeftCornerColor
        {
            get => Config.SaverConfig.BarRotLowerLeftCornerColor;
            set => Config.SaverConfig.BarRotLowerLeftCornerColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [BarRot] Lower right corner color.
        /// </summary>
        public static string BarRotLowerRightCornerColor
        {
            get => Config.SaverConfig.BarRotLowerRightCornerColor;
            set => Config.SaverConfig.BarRotLowerRightCornerColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [BarRot] Upper frame color.
        /// </summary>
        public static string BarRotUpperFrameColor
        {
            get => Config.SaverConfig.BarRotUpperFrameColor;
            set => Config.SaverConfig.BarRotUpperFrameColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [BarRot] Lower frame color.
        /// </summary>
        public static string BarRotLowerFrameColor
        {
            get => Config.SaverConfig.BarRotLowerFrameColor;
            set => Config.SaverConfig.BarRotLowerFrameColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [BarRot] Left frame color.
        /// </summary>
        public static string BarRotLeftFrameColor
        {
            get => Config.SaverConfig.BarRotLeftFrameColor;
            set => Config.SaverConfig.BarRotLeftFrameColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [BarRot] Right frame color.
        /// </summary>
        public static string BarRotRightFrameColor
        {
            get => Config.SaverConfig.BarRotRightFrameColor;
            set => Config.SaverConfig.BarRotRightFrameColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [BarRot] Use the border colors.
        /// </summary>
        public static bool BarRotUseBorderColors
        {
            get => Config.SaverConfig.BarRotUseBorderColors;
            set => Config.SaverConfig.BarRotUseBorderColors = value;
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
            KernelColorTools.LoadBack();

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
                TextWriterWhereColor.WriteWhere(BarRotSettings.BarRotUpperLeftCornerChar.ToString(), RampFrameStartWidth, RampCenterPosition - 2, false, BarRotSettings.BarRotUseBorderColors ? new Color(BarRotSettings.BarRotUpperLeftCornerColor) : KernelColorTools.GetGray());
                TextWriterColor.Write(new string(BarRotSettings.BarRotUpperFrameChar, RampFrameSpaces), false, BarRotSettings.BarRotUseBorderColors ? new Color(BarRotSettings.BarRotUpperFrameColor) : KernelColorTools.GetGray());
                TextWriterColor.Write(BarRotSettings.BarRotUpperRightCornerChar.ToString(), false, BarRotSettings.BarRotUseBorderColors ? new Color(BarRotSettings.BarRotUpperRightCornerColor) : KernelColorTools.GetGray());
                TextWriterWhereColor.WriteWhere(BarRotSettings.BarRotLeftFrameChar.ToString(), RampFrameStartWidth, RampCenterPosition - 1, false, BarRotSettings.BarRotUseBorderColors ? new Color(BarRotSettings.BarRotLeftFrameColor) : KernelColorTools.GetGray());
                TextWriterWhereColor.WriteWhere(BarRotSettings.BarRotLeftFrameChar.ToString(), RampFrameStartWidth, RampCenterPosition, false, BarRotSettings.BarRotUseBorderColors ? new Color(BarRotSettings.BarRotLeftFrameColor) : KernelColorTools.GetGray());
                TextWriterWhereColor.WriteWhere(BarRotSettings.BarRotLeftFrameChar.ToString(), RampFrameStartWidth, RampCenterPosition + 1, false, BarRotSettings.BarRotUseBorderColors ? new Color(BarRotSettings.BarRotLeftFrameColor) : KernelColorTools.GetGray());
                TextWriterWhereColor.WriteWhere(BarRotSettings.BarRotRightFrameChar.ToString(), RampFrameEndWidth + 1, RampCenterPosition - 1, false, BarRotSettings.BarRotUseBorderColors ? new Color(BarRotSettings.BarRotLeftFrameColor) : KernelColorTools.GetGray());
                TextWriterWhereColor.WriteWhere(BarRotSettings.BarRotRightFrameChar.ToString(), RampFrameEndWidth + 1, RampCenterPosition, false, BarRotSettings.BarRotUseBorderColors ? new Color(BarRotSettings.BarRotLeftFrameColor) : KernelColorTools.GetGray());
                TextWriterWhereColor.WriteWhere(BarRotSettings.BarRotRightFrameChar.ToString(), RampFrameEndWidth + 1, RampCenterPosition + 1, false, BarRotSettings.BarRotUseBorderColors ? new Color(BarRotSettings.BarRotLeftFrameColor) : KernelColorTools.GetGray());
                TextWriterWhereColor.WriteWhere(BarRotSettings.BarRotLowerLeftCornerChar.ToString(), RampFrameStartWidth, RampCenterPosition + 2, false, BarRotSettings.BarRotUseBorderColors ? new Color(BarRotSettings.BarRotLowerLeftCornerColor) : KernelColorTools.GetGray());
                TextWriterColor.Write(new string(BarRotSettings.BarRotLowerFrameChar, RampFrameSpaces), false, BarRotSettings.BarRotUseBorderColors ? new Color(BarRotSettings.BarRotLowerFrameColor) : KernelColorTools.GetGray());
                TextWriterColor.Write(BarRotSettings.BarRotLowerRightCornerChar.ToString(), false, BarRotSettings.BarRotUseBorderColors ? new Color(BarRotSettings.BarRotLowerRightCornerColor) : KernelColorTools.GetGray());
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
                KernelColorTools.SetConsoleColor(RampSubgradientCurrentColorInstance, true, true);

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
                    KernelColorTools.SetConsoleColor(RampSubgradientCurrentColorInstance, true, true);
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
            KernelColorTools.LoadBack();

            // Reset resize sync
            ConsoleResizeListener.WasResized();
        }

    }
}
