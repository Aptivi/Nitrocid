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
using KS.Drivers.RNG;
using KS.Kernel.Debugging;
using KS.Kernel.Threading;
using KS.Misc.Screensaver;
using KS.Misc.Text;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Settings for Marquee
    /// </summary>
    public static class MarqueeSettings
    {

        /// <summary>
        /// [Marquee] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool MarqueeTrueColor
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.MarqueeTrueColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.MarqueeTrueColor = value;
            }
        }
        /// <summary>
        /// [Marquee] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int MarqueeDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.MarqueeDelay;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.MarqueeDelay = value;
            }
        }
        /// <summary>
        /// [Marquee] Text for Marquee. Shorter is better.
        /// </summary>
        public static string MarqueeWrite
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.MarqueeWrite;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.MarqueeWrite = value;
            }
        }
        /// <summary>
        /// [Marquee] Whether the text is always on center.
        /// </summary>
        public static bool MarqueeAlwaysCentered
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.MarqueeAlwaysCentered;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.MarqueeAlwaysCentered = value;
            }
        }
        /// <summary>
        /// [Marquee] Whether to use the KS.ConsoleBase.ConsoleWrapper.Clear() API (slow) or use the line-clearing VT sequence (fast).
        /// </summary>
        public static bool MarqueeUseConsoleAPI
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.MarqueeUseConsoleAPI;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.MarqueeUseConsoleAPI = value;
            }
        }
        /// <summary>
        /// [Marquee] Screensaver background color
        /// </summary>
        public static string MarqueeBackgroundColor
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.MarqueeBackgroundColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.MarqueeBackgroundColor = value;
            }
        }
        /// <summary>
        /// [Marquee] The minimum red color level (true color)
        /// </summary>
        public static int MarqueeMinimumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.MarqueeMinimumRedColorLevel;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.MarqueeMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Marquee] The minimum green color level (true color)
        /// </summary>
        public static int MarqueeMinimumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.MarqueeMinimumGreenColorLevel;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.MarqueeMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Marquee] The minimum blue color level (true color)
        /// </summary>
        public static int MarqueeMinimumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.MarqueeMinimumBlueColorLevel;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.MarqueeMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Marquee] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int MarqueeMinimumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.MarqueeMinimumColorLevel;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.MarqueeMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Marquee] The maximum red color level (true color)
        /// </summary>
        public static int MarqueeMaximumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.MarqueeMaximumRedColorLevel;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.MarqueeMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Marquee] The maximum green color level (true color)
        /// </summary>
        public static int MarqueeMaximumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.MarqueeMaximumGreenColorLevel;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.MarqueeMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Marquee] The maximum blue color level (true color)
        /// </summary>
        public static int MarqueeMaximumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.MarqueeMaximumBlueColorLevel;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.MarqueeMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Marquee] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int MarqueeMaximumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.MarqueeMaximumColorLevel;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.MarqueeMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for Marquee
    /// </summary>
    public class MarqueeDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Marquee";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            KernelColorTools.LoadBack(new Color(MarqueeSettings.MarqueeBackgroundColor));
            ConsoleWrapper.ForegroundColor = ConsoleColor.White;
            MarqueeSettings.MarqueeWrite = MarqueeSettings.MarqueeWrite.ReplaceAll(new string[] { Convert.ToChar(13).ToString(), Convert.ToChar(10).ToString() }, " - ");
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;
            ConsoleWrapper.Clear();

            // Ensure that the top position of the written text is always centered if AlwaysCentered is enabled. Else, select a random height.
            int TopPrinted = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d);
            if (!MarqueeSettings.MarqueeAlwaysCentered)
            {
                TopPrinted = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
            }
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Top position: {0}", TopPrinted);

            // Start with the left position as the right position.
            int CurrentLeft = ConsoleWrapper.WindowWidth - 1;
            int CurrentLeftOtherEnd = ConsoleWrapper.WindowWidth - 1;
            int CurrentCharacterNum = 0;

            // We need to set colors for the text.
            if (MarqueeSettings.MarqueeTrueColor)
            {
                int RedColorNum = RandomDriver.Random(MarqueeSettings.MarqueeMinimumRedColorLevel, MarqueeSettings.MarqueeMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(MarqueeSettings.MarqueeMinimumGreenColorLevel, MarqueeSettings.MarqueeMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(MarqueeSettings.MarqueeMinimumBlueColorLevel, MarqueeSettings.MarqueeMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                KernelColorTools.SetConsoleColor(new Color($"{RedColorNum};{GreenColorNum};{BlueColorNum}"));
            }
            else
            {
                int color = RandomDriver.Random(MarqueeSettings.MarqueeMinimumColorLevel, MarqueeSettings.MarqueeMaximumColorLevel);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color ({0})", color);
                KernelColorTools.SetConsoleColor(new Color(color));
            }

            // If the text is at the right and is longer than the console width, crop it until it's complete.
            while (CurrentLeftOtherEnd != 0)
            {
                ThreadManager.SleepNoBlock(MarqueeSettings.MarqueeDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                if (ConsoleResizeListener.WasResized(false))
                    break;
                if (MarqueeSettings.MarqueeUseConsoleAPI)
                    ConsoleWrapper.Clear();
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Current left: {0} | Current left on other end: {1}", CurrentLeft, CurrentLeftOtherEnd);

                // Declare variable for written marquee text
                string MarqueeWritten = MarqueeSettings.MarqueeWrite;
                bool Middle = MarqueeSettings.MarqueeWrite.Length - (CurrentLeftOtherEnd - CurrentLeft) != CurrentCharacterNum - (CurrentLeftOtherEnd - CurrentLeft);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Middle of long text: {0}", Middle);

                // If the current left position is not zero (not on the left), take the substring starting from the beginning of the string until the
                // written variable equals the base text variable. However, if we're on the left, take the substring so that the character which was
                // shown previously won't be shown again.
                if (CurrentLeft != 0)
                {
                    MarqueeWritten = MarqueeWritten[..(CurrentLeftOtherEnd - CurrentLeft)];
                }
                else if (CurrentLeft == 0 & Middle)
                {
                    MarqueeWritten = MarqueeWritten.Substring(CurrentCharacterNum - (CurrentLeftOtherEnd - CurrentLeft), CurrentLeftOtherEnd - CurrentLeft);
                }
                else
                {
                    MarqueeWritten = MarqueeWritten[(MarqueeSettings.MarqueeWrite.Length - (CurrentLeftOtherEnd - CurrentLeft))..];
                }
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Written result: {0}", MarqueeWritten);
                if (!MarqueeSettings.MarqueeUseConsoleAPI)
                    MarqueeWritten += $"{ConsoleExtensions.GetClearLineToRightSequence()}";

                // Set the appropriate cursor position and write the results
                ConsoleWrapper.SetCursorPosition(CurrentLeft, TopPrinted);
                ConsoleWrapper.Write(MarqueeWritten);
                if (Middle)
                    CurrentCharacterNum += 1;

                // If we're not on the left, decrement the current left position
                if (CurrentLeft != 0)
                {
                    CurrentLeft -= 1;
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Not on left. Decremented left position {0}", CurrentLeft);
                }

                // If we're on the left or the entire text is written, decrement the current left other end position
                if (!Middle)
                {
                    CurrentLeftOtherEnd -= 1;
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "On left or entire text written. Decremented left other end position {0}", CurrentLeftOtherEnd);
                }
            }

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(MarqueeSettings.MarqueeDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
