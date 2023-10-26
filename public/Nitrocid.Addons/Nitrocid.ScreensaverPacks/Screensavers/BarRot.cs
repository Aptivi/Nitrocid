//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Drivers.RNG;
using KS.Kernel.Debugging;
using KS.Kernel.Threading;
using KS.Misc.Screensaver;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
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
            get => ScreensaverPackInit.SaversConfig.BarRotTrueColor;
            set => ScreensaverPackInit.SaversConfig.BarRotTrueColor = value;
        }
        /// <summary>
        /// [BarRot] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int BarRotDelay
        {
            get => ScreensaverPackInit.SaversConfig.BarRotDelay;
            set
            {
                if (value <= 0)
                    value = 10;
                ScreensaverPackInit.SaversConfig.BarRotDelay = value;
            }
        }
        /// <summary>
        /// [BarRot] How many milliseconds to wait before rotting the next ramp's one end?
        /// </summary>
        public static int BarRotNextRampDelay
        {
            get => ScreensaverPackInit.SaversConfig.BarRotNextRampDelay;
            set
            {
                if (value <= 0)
                    value = 250;
                ScreensaverPackInit.SaversConfig.BarRotNextRampDelay = value;
            }
        }
        /// <summary>
        /// [BarRot] Upper left corner character 
        /// </summary>
        public static char BarRotUpperLeftCornerChar
        {
            get => ScreensaverPackInit.SaversConfig.BarRotUpperLeftCornerChar;
            set => ScreensaverPackInit.SaversConfig.BarRotUpperLeftCornerChar = value;
        }
        /// <summary>
        /// [BarRot] Upper right corner character 
        /// </summary>
        public static char BarRotUpperRightCornerChar
        {
            get => ScreensaverPackInit.SaversConfig.BarRotUpperRightCornerChar;
            set => ScreensaverPackInit.SaversConfig.BarRotUpperRightCornerChar = value;
        }
        /// <summary>
        /// [BarRot] Lower left corner character 
        /// </summary>
        public static char BarRotLowerLeftCornerChar
        {
            get => ScreensaverPackInit.SaversConfig.BarRotLowerLeftCornerChar;
            set => ScreensaverPackInit.SaversConfig.BarRotLowerLeftCornerChar = value;
        }
        /// <summary>
        /// [BarRot] Lower right corner character 
        /// </summary>
        public static char BarRotLowerRightCornerChar
        {
            get => ScreensaverPackInit.SaversConfig.BarRotLowerRightCornerChar;
            set => ScreensaverPackInit.SaversConfig.BarRotLowerRightCornerChar = value;
        }
        /// <summary>
        /// [BarRot] Upper frame character 
        /// </summary>
        public static char BarRotUpperFrameChar
        {
            get => ScreensaverPackInit.SaversConfig.BarRotUpperFrameChar;
            set => ScreensaverPackInit.SaversConfig.BarRotUpperFrameChar = value;
        }
        /// <summary>
        /// [BarRot] Lower frame character 
        /// </summary>
        public static char BarRotLowerFrameChar
        {
            get => ScreensaverPackInit.SaversConfig.BarRotLowerFrameChar;
            set => ScreensaverPackInit.SaversConfig.BarRotLowerFrameChar = value;
        }
        /// <summary>
        /// [BarRot] Left frame character 
        /// </summary>
        public static char BarRotLeftFrameChar
        {
            get => ScreensaverPackInit.SaversConfig.BarRotLeftFrameChar;
            set => ScreensaverPackInit.SaversConfig.BarRotLeftFrameChar = value;
        }
        /// <summary>
        /// [BarRot] Right frame character 
        /// </summary>
        public static char BarRotRightFrameChar
        {
            get => ScreensaverPackInit.SaversConfig.BarRotRightFrameChar;
            set => ScreensaverPackInit.SaversConfig.BarRotRightFrameChar = value;
        }
        /// <summary>
        /// [BarRot] The minimum red color level (true color - start)
        /// </summary>
        public static int BarRotMinimumRedColorLevelStart
        {
            get => ScreensaverPackInit.SaversConfig.BarRotMinimumRedColorLevelStart;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.BarRotMinimumRedColorLevelStart = value;
            }
        }
        /// <summary>
        /// [BarRot] The minimum green color level (true color - start)
        /// </summary>
        public static int BarRotMinimumGreenColorLevelStart
        {
            get => ScreensaverPackInit.SaversConfig.BarRotMinimumGreenColorLevelStart;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.BarRotMinimumGreenColorLevelStart = value;
            }
        }
        /// <summary>
        /// [BarRot] The minimum blue color level (true color - start)
        /// </summary>
        public static int BarRotMinimumBlueColorLevelStart
        {
            get => ScreensaverPackInit.SaversConfig.BarRotMinimumBlueColorLevelStart;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.BarRotMinimumBlueColorLevelStart = value;
            }
        }
        /// <summary>
        /// [BarRot] The maximum red color level (true color - start)
        /// </summary>
        public static int BarRotMaximumRedColorLevelStart
        {
            get => ScreensaverPackInit.SaversConfig.BarRotMaximumRedColorLevelStart;
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.BarRotMinimumRedColorLevelStart)
                    value = ScreensaverPackInit.SaversConfig.BarRotMinimumRedColorLevelStart;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.BarRotMaximumRedColorLevelStart = value;
            }
        }
        /// <summary>
        /// [BarRot] The maximum green color level (true color - start)
        /// </summary>
        public static int BarRotMaximumGreenColorLevelStart
        {
            get => ScreensaverPackInit.SaversConfig.BarRotMaximumGreenColorLevelStart;
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.BarRotMinimumGreenColorLevelStart)
                    value = ScreensaverPackInit.SaversConfig.BarRotMinimumGreenColorLevelStart;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.BarRotMaximumGreenColorLevelStart = value;
            }
        }
        /// <summary>
        /// [BarRot] The maximum blue color level (true color - start)
        /// </summary>
        public static int BarRotMaximumBlueColorLevelStart
        {
            get => ScreensaverPackInit.SaversConfig.BarRotMaximumBlueColorLevelStart;
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.BarRotMinimumBlueColorLevelStart)
                    value = ScreensaverPackInit.SaversConfig.BarRotMinimumBlueColorLevelStart;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.BarRotMaximumBlueColorLevelStart = value;
            }
        }
        /// <summary>
        /// [BarRot] The minimum red color level (true color - end)
        /// </summary>
        public static int BarRotMinimumRedColorLevelEnd
        {
            get => ScreensaverPackInit.SaversConfig.BarRotMinimumRedColorLevelEnd;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.BarRotMinimumRedColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [BarRot] The minimum green color level (true color - end)
        /// </summary>
        public static int BarRotMinimumGreenColorLevelEnd
        {
            get => ScreensaverPackInit.SaversConfig.BarRotMinimumGreenColorLevelEnd;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.BarRotMinimumGreenColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [BarRot] The minimum blue color level (true color - end)
        /// </summary>
        public static int BarRotMinimumBlueColorLevelEnd
        {
            get => ScreensaverPackInit.SaversConfig.BarRotMinimumBlueColorLevelEnd;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.BarRotMinimumBlueColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [BarRot] The maximum red color level (true color - end)
        /// </summary>
        public static int BarRotMaximumRedColorLevelEnd
        {
            get => ScreensaverPackInit.SaversConfig.BarRotMaximumRedColorLevelEnd;
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.BarRotMinimumRedColorLevelEnd)
                    value = ScreensaverPackInit.SaversConfig.BarRotMinimumRedColorLevelEnd;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.BarRotMaximumRedColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [BarRot] The maximum green color level (true color - end)
        /// </summary>
        public static int BarRotMaximumGreenColorLevelEnd
        {
            get => ScreensaverPackInit.SaversConfig.BarRotMaximumGreenColorLevelEnd;
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.BarRotMinimumGreenColorLevelEnd)
                    value = ScreensaverPackInit.SaversConfig.BarRotMinimumGreenColorLevelEnd;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.BarRotMaximumGreenColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [BarRot] The maximum blue color level (true color - end)
        /// </summary>
        public static int BarRotMaximumBlueColorLevelEnd
        {
            get => ScreensaverPackInit.SaversConfig.BarRotMaximumBlueColorLevelEnd;
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.BarRotMinimumBlueColorLevelEnd)
                    value = ScreensaverPackInit.SaversConfig.BarRotMinimumBlueColorLevelEnd;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.BarRotMaximumBlueColorLevelEnd = value;
            }
        }
        /// <summary>
        /// [BarRot] Upper left corner color.
        /// </summary>
        public static string BarRotUpperLeftCornerColor
        {
            get => ScreensaverPackInit.SaversConfig.BarRotUpperLeftCornerColor;
            set => ScreensaverPackInit.SaversConfig.BarRotUpperLeftCornerColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [BarRot] Upper right corner color.
        /// </summary>
        public static string BarRotUpperRightCornerColor
        {
            get => ScreensaverPackInit.SaversConfig.BarRotUpperRightCornerColor;
            set => ScreensaverPackInit.SaversConfig.BarRotUpperRightCornerColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [BarRot] Lower left corner color.
        /// </summary>
        public static string BarRotLowerLeftCornerColor
        {
            get => ScreensaverPackInit.SaversConfig.BarRotLowerLeftCornerColor;
            set => ScreensaverPackInit.SaversConfig.BarRotLowerLeftCornerColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [BarRot] Lower right corner color.
        /// </summary>
        public static string BarRotLowerRightCornerColor
        {
            get => ScreensaverPackInit.SaversConfig.BarRotLowerRightCornerColor;
            set => ScreensaverPackInit.SaversConfig.BarRotLowerRightCornerColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [BarRot] Upper frame color.
        /// </summary>
        public static string BarRotUpperFrameColor
        {
            get => ScreensaverPackInit.SaversConfig.BarRotUpperFrameColor;
            set => ScreensaverPackInit.SaversConfig.BarRotUpperFrameColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [BarRot] Lower frame color.
        /// </summary>
        public static string BarRotLowerFrameColor
        {
            get => ScreensaverPackInit.SaversConfig.BarRotLowerFrameColor;
            set => ScreensaverPackInit.SaversConfig.BarRotLowerFrameColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [BarRot] Left frame color.
        /// </summary>
        public static string BarRotLeftFrameColor
        {
            get => ScreensaverPackInit.SaversConfig.BarRotLeftFrameColor;
            set => ScreensaverPackInit.SaversConfig.BarRotLeftFrameColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [BarRot] Right frame color.
        /// </summary>
        public static string BarRotRightFrameColor
        {
            get => ScreensaverPackInit.SaversConfig.BarRotRightFrameColor;
            set => ScreensaverPackInit.SaversConfig.BarRotRightFrameColor = new Color(value).PlainSequence;
        }
        /// <summary>
        /// [BarRot] Use the border colors.
        /// </summary>
        public static bool BarRotUseBorderColors
        {
            get => ScreensaverPackInit.SaversConfig.BarRotUseBorderColors;
            set => ScreensaverPackInit.SaversConfig.BarRotUseBorderColors = value;
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
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color from (R;G;B: {0};{1};{2}) to (R;G;B: {3};{4};{5})", RedColorNumFrom, GreenColorNumFrom, BlueColorNumFrom, RedColorNumTo, GreenColorNumTo, BlueColorNumTo);

            // Set start and end widths for the ramp frame
            int RampFrameStartWidth = 4;
            int RampFrameEndWidth = ConsoleWrapper.WindowWidth - RampFrameStartWidth;
            int RampFrameSpaces = RampFrameEndWidth - RampFrameStartWidth;
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Start width: {0}, End width: {1}, Spaces: {2}", RampFrameStartWidth, RampFrameEndWidth, RampFrameSpaces);

            // Set thresholds for color ramp
            int RampColorRedThreshold = RedColorNumFrom - RedColorNumTo;
            int RampColorGreenThreshold = GreenColorNumFrom - GreenColorNumTo;
            int RampColorBlueThreshold = BlueColorNumFrom - BlueColorNumTo;
            double RampColorRedSteps = RampColorRedThreshold / (double)RampFrameSpaces;
            double RampColorGreenSteps = RampColorGreenThreshold / (double)RampFrameSpaces;
            double RampColorBlueSteps = RampColorBlueThreshold / (double)RampFrameSpaces;
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Set thresholds (RGB: {0};{1};{2})", RampColorRedThreshold, RampColorGreenThreshold, RampColorBlueThreshold);
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Steps by {0} spaces (RGB: {1};{2};{3})", RampFrameSpaces, RampColorRedSteps, RampColorGreenSteps, RampColorBlueSteps);

            // Let the ramp be printed in the center
            int RampCenterPosition = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d);
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Center position: {0}", RampCenterPosition);

            // Set the current positions
            int RampCurrentPositionLeft = RampFrameStartWidth + 1;
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Current left position: {0}", RampCurrentPositionLeft);

            // Draw the frame
            if (!ConsoleResizeListener.WasResized(false))
            {
                TextWriterWhereColor.WriteWhereColor(BarRotSettings.BarRotUpperLeftCornerChar.ToString(), RampFrameStartWidth, RampCenterPosition - 2, false, BarRotSettings.BarRotUseBorderColors ? new Color(BarRotSettings.BarRotUpperLeftCornerColor) : KernelColorTools.GetGray());
                TextWriterColor.WriteColor(new string(BarRotSettings.BarRotUpperFrameChar, RampFrameSpaces), false, BarRotSettings.BarRotUseBorderColors ? new Color(BarRotSettings.BarRotUpperFrameColor) : KernelColorTools.GetGray());
                TextWriterColor.WriteColor(BarRotSettings.BarRotUpperRightCornerChar.ToString(), false, BarRotSettings.BarRotUseBorderColors ? new Color(BarRotSettings.BarRotUpperRightCornerColor) : KernelColorTools.GetGray());
                TextWriterWhereColor.WriteWhereColor(BarRotSettings.BarRotLeftFrameChar.ToString(), RampFrameStartWidth, RampCenterPosition - 1, false, BarRotSettings.BarRotUseBorderColors ? new Color(BarRotSettings.BarRotLeftFrameColor) : KernelColorTools.GetGray());
                TextWriterWhereColor.WriteWhereColor(BarRotSettings.BarRotLeftFrameChar.ToString(), RampFrameStartWidth, RampCenterPosition, false, BarRotSettings.BarRotUseBorderColors ? new Color(BarRotSettings.BarRotLeftFrameColor) : KernelColorTools.GetGray());
                TextWriterWhereColor.WriteWhereColor(BarRotSettings.BarRotLeftFrameChar.ToString(), RampFrameStartWidth, RampCenterPosition + 1, false, BarRotSettings.BarRotUseBorderColors ? new Color(BarRotSettings.BarRotLeftFrameColor) : KernelColorTools.GetGray());
                TextWriterWhereColor.WriteWhereColor(BarRotSettings.BarRotRightFrameChar.ToString(), RampFrameEndWidth + 1, RampCenterPosition - 1, false, BarRotSettings.BarRotUseBorderColors ? new Color(BarRotSettings.BarRotLeftFrameColor) : KernelColorTools.GetGray());
                TextWriterWhereColor.WriteWhereColor(BarRotSettings.BarRotRightFrameChar.ToString(), RampFrameEndWidth + 1, RampCenterPosition, false, BarRotSettings.BarRotUseBorderColors ? new Color(BarRotSettings.BarRotLeftFrameColor) : KernelColorTools.GetGray());
                TextWriterWhereColor.WriteWhereColor(BarRotSettings.BarRotRightFrameChar.ToString(), RampFrameEndWidth + 1, RampCenterPosition + 1, false, BarRotSettings.BarRotUseBorderColors ? new Color(BarRotSettings.BarRotLeftFrameColor) : KernelColorTools.GetGray());
                TextWriterWhereColor.WriteWhereColor(BarRotSettings.BarRotLowerLeftCornerChar.ToString(), RampFrameStartWidth, RampCenterPosition + 2, false, BarRotSettings.BarRotUseBorderColors ? new Color(BarRotSettings.BarRotLowerLeftCornerColor) : KernelColorTools.GetGray());
                TextWriterColor.WriteColor(new string(BarRotSettings.BarRotLowerFrameChar, RampFrameSpaces), false, BarRotSettings.BarRotUseBorderColors ? new Color(BarRotSettings.BarRotLowerFrameColor) : KernelColorTools.GetGray());
                TextWriterColor.WriteColor(BarRotSettings.BarRotLowerRightCornerChar.ToString(), false, BarRotSettings.BarRotUseBorderColors ? new Color(BarRotSettings.BarRotLowerRightCornerColor) : KernelColorTools.GetGray());
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
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got subgradient color from (R;G;B: {0};{1};{2}) to (R;G;B: {3};{4};{5})", RampSubgradientRedColorNumFrom, RampSubgradientGreenColorNumFrom, RampSubgradientBlueColorNumFrom, RampSubgradientRedColorNumTo, RampSubgradientGreenColorNumTo, RampSubgradientBlueColorNumTo);

                // Set the sub-gradient current colors
                double RampSubgradientCurrentColorRed = RampSubgradientRedColorNumFrom;
                double RampSubgradientCurrentColorGreen = RampSubgradientGreenColorNumFrom;
                double RampSubgradientCurrentColorBlue = RampSubgradientBlueColorNumFrom;
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got subgradient current colors (R;G;B: {0};{1};{2})", RampSubgradientCurrentColorRed, RampSubgradientCurrentColorGreen, RampSubgradientCurrentColorBlue);

                // Set the sub-gradient thresholds
                int RampSubgradientColorRedThreshold = RampSubgradientRedColorNumFrom - RampSubgradientRedColorNumTo;
                int RampSubgradientColorGreenThreshold = RampSubgradientGreenColorNumFrom - RampSubgradientGreenColorNumTo;
                int RampSubgradientColorBlueThreshold = RampSubgradientBlueColorNumFrom - RampSubgradientBlueColorNumTo;
                double RampSubgradientColorRedSteps = RampSubgradientColorRedThreshold / (double)RampFrameSpaces;
                double RampSubgradientColorGreenSteps = RampSubgradientColorGreenThreshold / (double)RampFrameSpaces;
                double RampSubgradientColorBlueSteps = RampSubgradientColorBlueThreshold / (double)RampFrameSpaces;
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Set subgradient thresholds (RGB: {0};{1};{2})", RampSubgradientColorRedThreshold, RampSubgradientColorGreenThreshold, RampSubgradientColorBlueThreshold);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Steps by {0} spaces for subgradient (RGB: {1};{2};{3})", RampFrameSpaces, RampSubgradientColorRedSteps, RampSubgradientColorGreenSteps, RampSubgradientColorBlueSteps);

                // Make a new instance
                var RampSubgradientCurrentColorInstance = new Color($"{Convert.ToInt32(RampSubgradientCurrentColorRed)};{Convert.ToInt32(RampSubgradientCurrentColorGreen)};{Convert.ToInt32(RampSubgradientCurrentColorBlue)}");
                KernelColorTools.SetConsoleColor(RampSubgradientCurrentColorInstance, true);

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
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got new subgradient current colors (R;G;B: {0};{1};{2}) subtracting from {3};{4};{5}", RampSubgradientCurrentColorRed, RampSubgradientCurrentColorGreen, RampSubgradientCurrentColorBlue, RampSubgradientColorRedSteps, RampSubgradientColorGreenSteps, RampSubgradientColorBlueSteps);
                    RampSubgradientCurrentColorInstance = new Color($"{Convert.ToInt32(RampSubgradientCurrentColorRed)};{Convert.ToInt32(RampSubgradientCurrentColorGreen)};{Convert.ToInt32(RampSubgradientCurrentColorBlue)}");
                    KernelColorTools.SetConsoleColor(RampSubgradientCurrentColorInstance, true);
                }

                // Change the colors
                RampCurrentColorRed -= RampColorRedSteps;
                RampCurrentColorGreen -= RampColorGreenSteps;
                RampCurrentColorBlue -= RampColorBlueSteps;
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got new current colors (R;G;B: {0};{1};{2}) subtracting from {3};{4};{5}", RampCurrentColorRed, RampCurrentColorGreen, RampCurrentColorBlue, RampColorRedSteps, RampColorGreenSteps, RampColorBlueSteps);

                // Delay writing
                RampCurrentPositionLeft = RampFrameStartWidth + 1;
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Current left position: {0}", RampCurrentPositionLeft);
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
