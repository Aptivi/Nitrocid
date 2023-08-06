
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
    /// Settings for Ramp
    /// </summary>
    public static class RampSettings
    {

        /// <summary>
        /// [Ramp] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool RampTrueColor
        {
            get => Config.SaverConfig.RampTrueColor;
            set => Config.SaverConfig.RampTrueColor = value;
        }
        /// <summary>
        /// [Ramp] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int RampDelay
        {
            get => Config.SaverConfig.RampDelay;
            set
            {
                if (value <= 0)
                    value = 20;
                Config.SaverConfig.RampDelay = value;
            }
        }
        /// <summary>
        /// [Ramp] How many milliseconds to wait before starting the next ramp?
        /// </summary>
        public static int RampNextRampDelay
        {
            get => Config.SaverConfig.RampNextRampDelay;
            set
            {
                if (value <= 0)
                    value = 250;
                Config.SaverConfig.RampNextRampDelay = value;
            }
        }
        /// <summary>
        /// [Ramp] Upper left corner character 
        /// </summary>
        public static char RampUpperLeftCornerChar
        {
            get => Config.SaverConfig.RampUpperLeftCornerChar;
            set => Config.SaverConfig.RampUpperLeftCornerChar = value;
        }
        /// <summary>
        /// [Ramp] Upper right corner character 
        /// </summary>
        public static char RampUpperRightCornerChar
        {
            get => Config.SaverConfig.RampUpperRightCornerChar;
            set => Config.SaverConfig.RampUpperRightCornerChar = value;
        }
        /// <summary>
        /// [Ramp] Lower left corner character 
        /// </summary>
        public static char RampLowerLeftCornerChar
        {
            get => Config.SaverConfig.RampLowerLeftCornerChar;
            set => Config.SaverConfig.RampLowerLeftCornerChar = value;
        }
        /// <summary>
        /// [Ramp] Lower right corner character 
        /// </summary>
        public static char RampLowerRightCornerChar
        {
            get => Config.SaverConfig.RampLowerRightCornerChar;
            set => Config.SaverConfig.RampLowerRightCornerChar = value;
        }
        /// <summary>
        /// [Ramp] Upper frame character 
        /// </summary>
        public static char RampUpperFrameChar
        {
            get => Config.SaverConfig.RampUpperFrameChar;
            set => Config.SaverConfig.RampUpperFrameChar = value;
        }
        /// <summary>
        /// [Ramp] Lower frame character 
        /// </summary>
        public static char RampLowerFrameChar
        {
            get => Config.SaverConfig.RampLowerFrameChar;
            set => Config.SaverConfig.RampLowerFrameChar = value;
        }
        /// <summary>
        /// [Ramp] Left frame character 
        /// </summary>
        public static char RampLeftFrameChar
        {
            get => Config.SaverConfig.RampLeftFrameChar;
            set => Config.SaverConfig.RampLeftFrameChar = value;
        }
        /// <summary>
        /// [Ramp] Right frame character 
        /// </summary>
        public static char RampRightFrameChar
        {
            get => Config.SaverConfig.RampRightFrameChar;
            set => Config.SaverConfig.RampRightFrameChar = value;
        }
        /// <summary>
        /// [Ramp] The minimum red color level (true color - start)
        /// </summary>
        public static int RampMinimumRedColorLevelStart
        {
            get => Config.SaverConfig.RampMinimumRedColorLevelStart;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.RampMinimumRedColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Ramp] The minimum green color level (true color - start)
        /// </summary>
        public static int RampMinimumGreenColorLevelStart
        {
            get => Config.SaverConfig.RampMinimumGreenColorLevelStart;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.RampMinimumGreenColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Ramp] The minimum blue color level (true color - start)
        /// </summary>
        public static int RampMinimumBlueColorLevelStart
        {
            get => Config.SaverConfig.RampMinimumBlueColorLevelStart;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.RampMinimumBlueColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Ramp] The minimum color level (255 colors or 16 colors - start)
        /// </summary>
        public static int RampMinimumColorLevelStart
        {
            get => Config.SaverConfig.RampMinimumColorLevelStart;
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                Config.SaverConfig.RampMinimumColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Ramp] The maximum red color level (true color - start)
        /// </summary>
        public static int RampMaximumRedColorLevelStart
        {
            get => Config.SaverConfig.RampMaximumRedColorLevelStart;
            set
            {
                if (value <= Config.SaverConfig.RampMinimumRedColorLevelStart)
                    value = Config.SaverConfig.RampMinimumRedColorLevelStart;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.RampMaximumRedColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Ramp] The maximum green color level (true color - start)
        /// </summary>
        public static int RampMaximumGreenColorLevelStart
        {
            get => Config.SaverConfig.RampMaximumGreenColorLevelStart;
            set
            {
                if (value <= Config.SaverConfig.RampMinimumGreenColorLevelStart)
                    value = Config.SaverConfig.RampMinimumGreenColorLevelStart;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.RampMaximumGreenColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Ramp] The maximum blue color level (true color - start)
        /// </summary>
        public static int RampMaximumBlueColorLevelStart
        {
            get => Config.SaverConfig.RampMaximumBlueColorLevelStart;
            set
            {
                if (value <= Config.SaverConfig.RampMinimumBlueColorLevelStart)
                    value = Config.SaverConfig.RampMinimumBlueColorLevelStart;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.RampMaximumBlueColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Ramp] The maximum color level (255 colors or 16 colors - start)
        /// </summary>
        public static int RampMaximumColorLevelStart
        {
            get => Config.SaverConfig.RampMaximumColorLevelStart;
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= Config.SaverConfig.RampMinimumColorLevelStart)
                    value = Config.SaverConfig.RampMinimumColorLevelStart;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                Config.SaverConfig.RampMaximumColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Ramp] The minimum red color level (true color - end)
        /// </summary>
        public static int RampMinimumRedColorLevelEnd
        {
            get => Config.SaverConfig.RampMinimumRedColorLevelEnd;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.RampMinimumRedColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Ramp] The minimum green color level (true color - end)
        /// </summary>
        public static int RampMinimumGreenColorLevelEnd
        {
            get => Config.SaverConfig.RampMinimumGreenColorLevelEnd;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.RampMinimumGreenColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Ramp] The minimum blue color level (true color - end)
        /// </summary>
        public static int RampMinimumBlueColorLevelEnd
        {
            get => Config.SaverConfig.RampMinimumBlueColorLevelEnd;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.RampMinimumBlueColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Ramp] The minimum color level (255 colors or 16 colors - end)
        /// </summary>
        public static int RampMinimumColorLevelEnd
        {
            get => Config.SaverConfig.RampMinimumColorLevelEnd;
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                Config.SaverConfig.RampMinimumColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Ramp] The maximum red color level (true color - end)
        /// </summary>
        public static int RampMaximumRedColorLevelEnd
        {
            get => Config.SaverConfig.RampMaximumRedColorLevelEnd;
            set
            {
                if (value <= Config.SaverConfig.RampMinimumRedColorLevelEnd)
                    value = Config.SaverConfig.RampMinimumRedColorLevelEnd;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.RampMaximumRedColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Ramp] The maximum green color level (true color - end)
        /// </summary>
        public static int RampMaximumGreenColorLevelEnd
        {
            get => Config.SaverConfig.RampMaximumGreenColorLevelEnd;
            set
            {
                if (value <= Config.SaverConfig.RampMinimumGreenColorLevelEnd)
                    value = Config.SaverConfig.RampMinimumGreenColorLevelEnd;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.RampMaximumGreenColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Ramp] The maximum blue color level (true color - end)
        /// </summary>
        public static int RampMaximumBlueColorLevelEnd
        {
            get => Config.SaverConfig.RampMaximumBlueColorLevelEnd;
            set
            {
                if (value <= Config.SaverConfig.RampMinimumBlueColorLevelEnd)
                    value = Config.SaverConfig.RampMinimumBlueColorLevelEnd;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.RampMaximumBlueColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Ramp] The maximum color level (255 colors or 16 colors - end)
        /// </summary>
        public static int RampMaximumColorLevelEnd
        {
            get => Config.SaverConfig.RampMaximumColorLevelEnd;
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= Config.SaverConfig.RampMinimumColorLevelEnd)
                    value = Config.SaverConfig.RampMinimumColorLevelEnd;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                Config.SaverConfig.RampMaximumColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Ramp] Upper left corner color.
        /// </summary>
        public static string RampUpperLeftCornerColor
        {
            get => Config.SaverConfig.RampUpperLeftCornerColor;
            set => Config.SaverConfig.RampUpperLeftCornerColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Ramp] Upper right corner color.
        /// </summary>
        public static string RampUpperRightCornerColor
        {
            get => Config.SaverConfig.RampUpperRightCornerColor;
            set => Config.SaverConfig.RampUpperRightCornerColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Ramp] Lower left corner color.
        /// </summary>
        public static string RampLowerLeftCornerColor
        {
            get => Config.SaverConfig.RampLowerLeftCornerColor;
            set => Config.SaverConfig.RampLowerLeftCornerColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Ramp] Lower right corner color.
        /// </summary>
        public static string RampLowerRightCornerColor
        {
            get => Config.SaverConfig.RampLowerRightCornerColor;
            set => Config.SaverConfig.RampLowerRightCornerColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Ramp] Upper frame color.
        /// </summary>
        public static string RampUpperFrameColor
        {
            get => Config.SaverConfig.RampUpperFrameColor;
            set => Config.SaverConfig.RampUpperFrameColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Ramp] Lower frame color.
        /// </summary>
        public static string RampLowerFrameColor
        {
            get => Config.SaverConfig.RampLowerFrameColor;
            set => Config.SaverConfig.RampLowerFrameColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Ramp] Left frame color.
        /// </summary>
        public static string RampLeftFrameColor
        {
            get => Config.SaverConfig.RampLeftFrameColor;
            set => Config.SaverConfig.RampLeftFrameColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Ramp] Right frame color.
        /// </summary>
        public static string RampRightFrameColor
        {
            get => Config.SaverConfig.RampRightFrameColor;
            set => Config.SaverConfig.RampRightFrameColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Ramp] Use the border colors.
        /// </summary>
        public static bool RampUseBorderColors
        {
            get => Config.SaverConfig.RampUseBorderColors;
            set => Config.SaverConfig.RampUseBorderColors = value;
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
            DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Start width: {0}, End width: {1}, Spaces: {2}", RampFrameStartWidth, RampFrameEndWidth, RampFrameSpaces);

            // Set thresholds for color ramps
            int RampColorRedThreshold = RedColorNumFrom - RedColorNumTo;
            int RampColorGreenThreshold = GreenColorNumFrom - GreenColorNumTo;
            int RampColorBlueThreshold = BlueColorNumFrom - BlueColorNumTo;
            int RampColorThreshold = ColorNumFrom - ColorNumTo;
            double RampColorRedSteps = RampColorRedThreshold / (double)RampFrameSpaces;
            double RampColorGreenSteps = RampColorGreenThreshold / (double)RampFrameSpaces;
            double RampColorBlueSteps = RampColorBlueThreshold / (double)RampFrameSpaces;
            double RampColorSteps = RampColorThreshold / (double)RampFrameSpaces;
            DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Set thresholds (RGB: {0};{1};{2} | Normal: {3})", RampColorRedThreshold, RampColorGreenThreshold, RampColorBlueThreshold, RampColorThreshold);
            DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Steps by {0} spaces (RGB: {1};{2};{3} | Normal: {4})", RampFrameSpaces, RampColorRedSteps, RampColorGreenSteps, RampColorBlueSteps, RampColorSteps);

            // Let the ramp be printed in the center
            int RampCenterPosition = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d);
            DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Center position: {0}", RampCenterPosition);

            // Set the current positions
            int RampCurrentPositionLeft = RampFrameStartWidth + 1;
            DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Current left position: {0}", RampCurrentPositionLeft);

            // Draw the frame
            if (!ConsoleResizeListener.WasResized(false))
            {
                TextWriterWhereColor.WriteWhere(RampSettings.RampUpperLeftCornerChar.ToString(), RampFrameStartWidth, RampCenterPosition - 2, false, RampSettings.RampUseBorderColors ? new Color(RampSettings.RampUpperLeftCornerColor) : KernelColorTools.GetGray());
                TextWriterColor.Write(new string(RampSettings.RampUpperFrameChar, RampFrameSpaces), false, RampSettings.RampUseBorderColors ? new Color(RampSettings.RampUpperFrameColor) : KernelColorTools.GetGray());
                TextWriterColor.Write(RampSettings.RampUpperRightCornerChar.ToString(), false, RampSettings.RampUseBorderColors ? new Color(RampSettings.RampUpperRightCornerColor) : KernelColorTools.GetGray());
                TextWriterWhereColor.WriteWhere(RampSettings.RampLeftFrameChar.ToString(), RampFrameStartWidth, RampCenterPosition - 1, false, RampSettings.RampUseBorderColors ? new Color(RampSettings.RampLeftFrameColor) : KernelColorTools.GetGray());
                TextWriterWhereColor.WriteWhere(RampSettings.RampLeftFrameChar.ToString(), RampFrameStartWidth, RampCenterPosition, false, RampSettings.RampUseBorderColors ? new Color(RampSettings.RampLeftFrameColor) : KernelColorTools.GetGray());
                TextWriterWhereColor.WriteWhere(RampSettings.RampLeftFrameChar.ToString(), RampFrameStartWidth, RampCenterPosition + 1, false, RampSettings.RampUseBorderColors ? new Color(RampSettings.RampLeftFrameColor) : KernelColorTools.GetGray());
                TextWriterWhereColor.WriteWhere(RampSettings.RampRightFrameChar.ToString(), RampFrameEndWidth + 1, RampCenterPosition - 1, false, RampSettings.RampUseBorderColors ? new Color(RampSettings.RampRightFrameColor) : KernelColorTools.GetGray());
                TextWriterWhereColor.WriteWhere(RampSettings.RampRightFrameChar.ToString(), RampFrameEndWidth + 1, RampCenterPosition, false, RampSettings.RampUseBorderColors ? new Color(RampSettings.RampRightFrameColor) : KernelColorTools.GetGray());
                TextWriterWhereColor.WriteWhere(RampSettings.RampRightFrameChar.ToString(), RampFrameEndWidth + 1, RampCenterPosition + 1, false, RampSettings.RampUseBorderColors ? new Color(RampSettings.RampRightFrameColor) : KernelColorTools.GetGray());
                TextWriterWhereColor.WriteWhere(RampSettings.RampLowerLeftCornerChar.ToString(), RampFrameStartWidth, RampCenterPosition + 2, false, RampSettings.RampUseBorderColors ? new Color(RampSettings.RampLowerLeftCornerColor) : KernelColorTools.GetGray());
                TextWriterColor.Write(new string(RampSettings.RampLowerFrameChar, RampFrameSpaces), false, RampSettings.RampUseBorderColors ? new Color(RampSettings.RampLowerFrameColor) : KernelColorTools.GetGray());
                TextWriterColor.Write(RampSettings.RampLowerRightCornerChar.ToString(), false, RampSettings.RampUseBorderColors ? new Color(RampSettings.RampLowerRightCornerColor) : KernelColorTools.GetGray());
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
                KernelColorTools.SetConsoleColor(RampCurrentColorInstance, true, true);
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
                    DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got new current colors (R;G;B: {0};{1};{2}) subtracting from {3};{4};{5}", RampCurrentColorRed, RampCurrentColorGreen, RampCurrentColorBlue, RampColorRedSteps, RampColorGreenSteps, RampColorBlueSteps);
                    RampCurrentColorInstance = new Color($"{Convert.ToInt32(RampCurrentColorRed)};{Convert.ToInt32(RampCurrentColorGreen)};{Convert.ToInt32(RampCurrentColorBlue)}");
                    KernelColorTools.SetConsoleColor(RampCurrentColorInstance, true, true);

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
                KernelColorTools.SetConsoleColor(RampCurrentColorInstance, true, true);
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
                    DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got new current colors (Normal: {0}) subtracting from {1}", RampCurrentColor, RampColorSteps);
                    RampCurrentColorInstance = new Color(Convert.ToInt32(RampCurrentColor));
                    KernelColorTools.SetConsoleColor(RampCurrentColorInstance, true, true);

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
