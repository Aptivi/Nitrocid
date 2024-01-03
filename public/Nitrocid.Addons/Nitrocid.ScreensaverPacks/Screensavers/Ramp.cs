//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using Nitrocid.ConsoleBase;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers.FancyWriters;
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Threading;
using Nitrocid.Misc.Screensaver;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
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
            get => ScreensaverPackInit.SaversConfig.RampTrueColor;
            set => ScreensaverPackInit.SaversConfig.RampTrueColor = value;
        }
        /// <summary>
        /// [Ramp] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int RampDelay
        {
            get => ScreensaverPackInit.SaversConfig.RampDelay;
            set
            {
                if (value <= 0)
                    value = 20;
                ScreensaverPackInit.SaversConfig.RampDelay = value;
            }
        }
        /// <summary>
        /// [Ramp] How many milliseconds to wait before starting the next ramp?
        /// </summary>
        public static int RampNextRampDelay
        {
            get => ScreensaverPackInit.SaversConfig.RampNextRampDelay;
            set
            {
                if (value <= 0)
                    value = 250;
                ScreensaverPackInit.SaversConfig.RampNextRampDelay = value;
            }
        }
        /// <summary>
        /// [Ramp] Upper left corner character 
        /// </summary>
        public static char RampUpperLeftCornerChar
        {
            get => ScreensaverPackInit.SaversConfig.RampUpperLeftCornerChar;
            set => ScreensaverPackInit.SaversConfig.RampUpperLeftCornerChar = value;
        }
        /// <summary>
        /// [Ramp] Upper right corner character 
        /// </summary>
        public static char RampUpperRightCornerChar
        {
            get => ScreensaverPackInit.SaversConfig.RampUpperRightCornerChar;
            set => ScreensaverPackInit.SaversConfig.RampUpperRightCornerChar = value;
        }
        /// <summary>
        /// [Ramp] Lower left corner character 
        /// </summary>
        public static char RampLowerLeftCornerChar
        {
            get => ScreensaverPackInit.SaversConfig.RampLowerLeftCornerChar;
            set => ScreensaverPackInit.SaversConfig.RampLowerLeftCornerChar = value;
        }
        /// <summary>
        /// [Ramp] Lower right corner character 
        /// </summary>
        public static char RampLowerRightCornerChar
        {
            get => ScreensaverPackInit.SaversConfig.RampLowerRightCornerChar;
            set => ScreensaverPackInit.SaversConfig.RampLowerRightCornerChar = value;
        }
        /// <summary>
        /// [Ramp] Upper frame character 
        /// </summary>
        public static char RampUpperFrameChar
        {
            get => ScreensaverPackInit.SaversConfig.RampUpperFrameChar;
            set => ScreensaverPackInit.SaversConfig.RampUpperFrameChar = value;
        }
        /// <summary>
        /// [Ramp] Lower frame character 
        /// </summary>
        public static char RampLowerFrameChar
        {
            get => ScreensaverPackInit.SaversConfig.RampLowerFrameChar;
            set => ScreensaverPackInit.SaversConfig.RampLowerFrameChar = value;
        }
        /// <summary>
        /// [Ramp] Left frame character 
        /// </summary>
        public static char RampLeftFrameChar
        {
            get => ScreensaverPackInit.SaversConfig.RampLeftFrameChar;
            set => ScreensaverPackInit.SaversConfig.RampLeftFrameChar = value;
        }
        /// <summary>
        /// [Ramp] Right frame character 
        /// </summary>
        public static char RampRightFrameChar
        {
            get => ScreensaverPackInit.SaversConfig.RampRightFrameChar;
            set => ScreensaverPackInit.SaversConfig.RampRightFrameChar = value;
        }
        /// <summary>
        /// [Ramp] The minimum red color level (true color - start)
        /// </summary>
        public static int RampMinimumRedColorLevelStart
        {
            get => ScreensaverPackInit.SaversConfig.RampMinimumRedColorLevelStart;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.RampMinimumRedColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Ramp] The minimum green color level (true color - start)
        /// </summary>
        public static int RampMinimumGreenColorLevelStart
        {
            get => ScreensaverPackInit.SaversConfig.RampMinimumGreenColorLevelStart;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.RampMinimumGreenColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Ramp] The minimum blue color level (true color - start)
        /// </summary>
        public static int RampMinimumBlueColorLevelStart
        {
            get => ScreensaverPackInit.SaversConfig.RampMinimumBlueColorLevelStart;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.RampMinimumBlueColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Ramp] The minimum color level (255 colors or 16 colors - start)
        /// </summary>
        public static int RampMinimumColorLevelStart
        {
            get => ScreensaverPackInit.SaversConfig.RampMinimumColorLevelStart;
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                ScreensaverPackInit.SaversConfig.RampMinimumColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Ramp] The maximum red color level (true color - start)
        /// </summary>
        public static int RampMaximumRedColorLevelStart
        {
            get => ScreensaverPackInit.SaversConfig.RampMaximumRedColorLevelStart;
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.RampMinimumRedColorLevelStart)
                    value = ScreensaverPackInit.SaversConfig.RampMinimumRedColorLevelStart;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.RampMaximumRedColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Ramp] The maximum green color level (true color - start)
        /// </summary>
        public static int RampMaximumGreenColorLevelStart
        {
            get => ScreensaverPackInit.SaversConfig.RampMaximumGreenColorLevelStart;
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.RampMinimumGreenColorLevelStart)
                    value = ScreensaverPackInit.SaversConfig.RampMinimumGreenColorLevelStart;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.RampMaximumGreenColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Ramp] The maximum blue color level (true color - start)
        /// </summary>
        public static int RampMaximumBlueColorLevelStart
        {
            get => ScreensaverPackInit.SaversConfig.RampMaximumBlueColorLevelStart;
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.RampMinimumBlueColorLevelStart)
                    value = ScreensaverPackInit.SaversConfig.RampMinimumBlueColorLevelStart;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.RampMaximumBlueColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Ramp] The maximum color level (255 colors or 16 colors - start)
        /// </summary>
        public static int RampMaximumColorLevelStart
        {
            get => ScreensaverPackInit.SaversConfig.RampMaximumColorLevelStart;
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= ScreensaverPackInit.SaversConfig.RampMinimumColorLevelStart)
                    value = ScreensaverPackInit.SaversConfig.RampMinimumColorLevelStart;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                ScreensaverPackInit.SaversConfig.RampMaximumColorLevelStart = value;
            }
        }
        /// <summary>
        /// [Ramp] The minimum red color level (true color - end)
        /// </summary>
        public static int RampMinimumRedColorLevelEnd
        {
            get => ScreensaverPackInit.SaversConfig.RampMinimumRedColorLevelEnd;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.RampMinimumRedColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Ramp] The minimum green color level (true color - end)
        /// </summary>
        public static int RampMinimumGreenColorLevelEnd
        {
            get => ScreensaverPackInit.SaversConfig.RampMinimumGreenColorLevelEnd;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.RampMinimumGreenColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Ramp] The minimum blue color level (true color - end)
        /// </summary>
        public static int RampMinimumBlueColorLevelEnd
        {
            get => ScreensaverPackInit.SaversConfig.RampMinimumBlueColorLevelEnd;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.RampMinimumBlueColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Ramp] The minimum color level (255 colors or 16 colors - end)
        /// </summary>
        public static int RampMinimumColorLevelEnd
        {
            get => ScreensaverPackInit.SaversConfig.RampMinimumColorLevelEnd;
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                ScreensaverPackInit.SaversConfig.RampMinimumColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Ramp] The maximum red color level (true color - end)
        /// </summary>
        public static int RampMaximumRedColorLevelEnd
        {
            get => ScreensaverPackInit.SaversConfig.RampMaximumRedColorLevelEnd;
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.RampMinimumRedColorLevelEnd)
                    value = ScreensaverPackInit.SaversConfig.RampMinimumRedColorLevelEnd;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.RampMaximumRedColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Ramp] The maximum green color level (true color - end)
        /// </summary>
        public static int RampMaximumGreenColorLevelEnd
        {
            get => ScreensaverPackInit.SaversConfig.RampMaximumGreenColorLevelEnd;
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.RampMinimumGreenColorLevelEnd)
                    value = ScreensaverPackInit.SaversConfig.RampMinimumGreenColorLevelEnd;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.RampMaximumGreenColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Ramp] The maximum blue color level (true color - end)
        /// </summary>
        public static int RampMaximumBlueColorLevelEnd
        {
            get => ScreensaverPackInit.SaversConfig.RampMaximumBlueColorLevelEnd;
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.RampMinimumBlueColorLevelEnd)
                    value = ScreensaverPackInit.SaversConfig.RampMinimumBlueColorLevelEnd;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.RampMaximumBlueColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Ramp] The maximum color level (255 colors or 16 colors - end)
        /// </summary>
        public static int RampMaximumColorLevelEnd
        {
            get => ScreensaverPackInit.SaversConfig.RampMaximumColorLevelEnd;
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= ScreensaverPackInit.SaversConfig.RampMinimumColorLevelEnd)
                    value = ScreensaverPackInit.SaversConfig.RampMinimumColorLevelEnd;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                ScreensaverPackInit.SaversConfig.RampMaximumColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [Ramp] Upper left corner color.
        /// </summary>
        public static string RampUpperLeftCornerColor
        {
            get => ScreensaverPackInit.SaversConfig.RampUpperLeftCornerColor;
            set => ScreensaverPackInit.SaversConfig.RampUpperLeftCornerColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Ramp] Upper right corner color.
        /// </summary>
        public static string RampUpperRightCornerColor
        {
            get => ScreensaverPackInit.SaversConfig.RampUpperRightCornerColor;
            set => ScreensaverPackInit.SaversConfig.RampUpperRightCornerColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Ramp] Lower left corner color.
        /// </summary>
        public static string RampLowerLeftCornerColor
        {
            get => ScreensaverPackInit.SaversConfig.RampLowerLeftCornerColor;
            set => ScreensaverPackInit.SaversConfig.RampLowerLeftCornerColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Ramp] Lower right corner color.
        /// </summary>
        public static string RampLowerRightCornerColor
        {
            get => ScreensaverPackInit.SaversConfig.RampLowerRightCornerColor;
            set => ScreensaverPackInit.SaversConfig.RampLowerRightCornerColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Ramp] Upper frame color.
        /// </summary>
        public static string RampUpperFrameColor
        {
            get => ScreensaverPackInit.SaversConfig.RampUpperFrameColor;
            set => ScreensaverPackInit.SaversConfig.RampUpperFrameColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Ramp] Lower frame color.
        /// </summary>
        public static string RampLowerFrameColor
        {
            get => ScreensaverPackInit.SaversConfig.RampLowerFrameColor;
            set => ScreensaverPackInit.SaversConfig.RampLowerFrameColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Ramp] Left frame color.
        /// </summary>
        public static string RampLeftFrameColor
        {
            get => ScreensaverPackInit.SaversConfig.RampLeftFrameColor;
            set => ScreensaverPackInit.SaversConfig.RampLeftFrameColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Ramp] Right frame color.
        /// </summary>
        public static string RampRightFrameColor
        {
            get => ScreensaverPackInit.SaversConfig.RampRightFrameColor;
            set => ScreensaverPackInit.SaversConfig.RampRightFrameColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [Ramp] Use the border colors.
        /// </summary>
        public static bool RampUseBorderColors
        {
            get => ScreensaverPackInit.SaversConfig.RampUseBorderColors;
            set => ScreensaverPackInit.SaversConfig.RampUseBorderColors = value;
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
            int RampFrameSpaces = RampFrameEndWidth - RampFrameStartWidth - 1;
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Start width: {0}, End width: {1}, Spaces: {2}", RampFrameStartWidth, RampFrameEndWidth, RampFrameSpaces);

            // Set thresholds for color ramps
            int RampColorRedThreshold = RedColorNumFrom - RedColorNumTo;
            int RampColorGreenThreshold = GreenColorNumFrom - GreenColorNumTo;
            int RampColorBlueThreshold = BlueColorNumFrom - BlueColorNumTo;
            int RampColorThreshold = ColorNumFrom - ColorNumTo;
            double RampColorRedSteps = RampColorRedThreshold / (double)RampFrameSpaces;
            double RampColorGreenSteps = RampColorGreenThreshold / (double)RampFrameSpaces;
            double RampColorBlueSteps = RampColorBlueThreshold / (double)RampFrameSpaces;
            double RampColorSteps = RampColorThreshold / (double)RampFrameSpaces;
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Set thresholds (RGB: {0};{1};{2} | Normal: {3})", RampColorRedThreshold, RampColorGreenThreshold, RampColorBlueThreshold, RampColorThreshold);
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Steps by {0} spaces (RGB: {1};{2};{3} | Normal: {4})", RampFrameSpaces, RampColorRedSteps, RampColorGreenSteps, RampColorBlueSteps, RampColorSteps);

            // Let the ramp be printed in the center
            int RampCenterPosition = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d);
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Center position: {0}", RampCenterPosition);

            // Set the current positions
            int RampCurrentPositionLeft = RampFrameStartWidth + 1;
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Current left position: {0}", RampCurrentPositionLeft);

            // Draw the frame
            if (!ConsoleResizeListener.WasResized(false))
                BorderColor.WriteBorder(RampFrameStartWidth, RampCenterPosition - 2, RampFrameSpaces, 3, RampSettings.RampUseBorderColors ? new Color(RampSettings.RampLeftFrameColor) : KernelColorTools.GetGray());

            // Draw the ramp
            if (RampSettings.RampTrueColor)
            {
                // Set the current colors
                double RampCurrentColorRed = RedColorNumFrom;
                double RampCurrentColorGreen = GreenColorNumFrom;
                double RampCurrentColorBlue = BlueColorNumFrom;
                var RampCurrentColorInstance = new Color($"{Convert.ToInt32(RampCurrentColorRed)};{Convert.ToInt32(RampCurrentColorGreen)};{Convert.ToInt32(RampCurrentColorBlue)}");

                // Set the console color and fill the ramp!
                KernelColorTools.SetConsoleColor(RampCurrentColorInstance, true);
                int step = 1;
                while (step <= RampFrameSpaces)
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
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got new current colors (R;G;B: {0};{1};{2}) subtracting from {3};{4};{5}", RampCurrentColorRed, RampCurrentColorGreen, RampCurrentColorBlue, RampColorRedSteps, RampColorGreenSteps, RampColorBlueSteps);
                    RampCurrentColorInstance = new Color($"{Convert.ToInt32(RampCurrentColorRed)};{Convert.ToInt32(RampCurrentColorGreen)};{Convert.ToInt32(RampCurrentColorBlue)}");
                    KernelColorTools.SetConsoleColor(RampCurrentColorInstance, true);

                    // Delay writing
                    step++;
                    ThreadManager.SleepNoBlock(RampSettings.RampDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                }
            }
            else
            {
                // Set the current colors
                double RampCurrentColor = ColorNumFrom;
                var RampCurrentColorInstance = new Color(Convert.ToInt32(RampCurrentColor));

                // Set the console color and fill the ramp!
                KernelColorTools.SetConsoleColor(RampCurrentColorInstance, true);
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
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got new current colors (Normal: {0}) subtracting from {1}", RampCurrentColor, RampColorSteps);
                    RampCurrentColorInstance = new Color(Convert.ToInt32(RampCurrentColor));
                    KernelColorTools.SetConsoleColor(RampCurrentColorInstance, true);

                    // Delay writing
                    ThreadManager.SleepNoBlock(RampSettings.RampDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                }
            }
            ThreadManager.SleepNoBlock(RampSettings.RampNextRampDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            KernelColorTools.LoadBack();
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(RampSettings.RampDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
