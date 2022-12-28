
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
using KS.Misc.Text;
using KS.Misc.Threading;
using ColorTools = KS.ConsoleBase.Colors.ColorTools;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for Marquee
    /// </summary>
    public static class MarqueeSettings
    {

        private static bool _marqueeTrueColor = true;
        private static int _marqueeDelay = 10;
        private static string _marqueeWrite = "Kernel Simulator";
        private static bool _marqueeAlwaysCentered = true;
        private static bool _marqueeUseConsoleAPI = false;
        private static string _marqueeBackgroundColor = new Color((int)ConsoleColor.Black).PlainSequence;
        private static int _marqueeMinimumRedColorLevel = 0;
        private static int _marqueeMinimumGreenColorLevel = 0;
        private static int _marqueeMinimumBlueColorLevel = 0;
        private static int _marqueeMinimumColorLevel = 0;
        private static int _marqueeMaximumRedColorLevel = 255;
        private static int _marqueeMaximumGreenColorLevel = 255;
        private static int _marqueeMaximumBlueColorLevel = 255;
        private static int _marqueeMaximumColorLevel = 0;

        /// <summary>
        /// [Marquee] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool MarqueeTrueColor
        {
            get
            {
                return _marqueeTrueColor;
            }
            set
            {
                _marqueeTrueColor = value;
            }
        }
        /// <summary>
        /// [Marquee] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int MarqueeDelay
        {
            get
            {
                return _marqueeDelay;
            }
            set
            {
                _marqueeDelay = value;
            }
        }
        /// <summary>
        /// [Marquee] Text for Marquee. Shorter is better.
        /// </summary>
        public static string MarqueeWrite
        {
            get
            {
                return _marqueeWrite;
            }
            set
            {
                _marqueeWrite = value;
            }
        }
        /// <summary>
        /// [Marquee] Whether the text is always on center.
        /// </summary>
        public static bool MarqueeAlwaysCentered
        {
            get
            {
                return _marqueeAlwaysCentered;
            }
            set
            {
                _marqueeAlwaysCentered = value;
            }
        }
        /// <summary>
        /// [Marquee] Whether to use the KS.ConsoleBase.ConsoleWrapper.Clear() API (slow) or use the line-clearing VT sequence (fast).
        /// </summary>
        public static bool MarqueeUseConsoleAPI
        {
            get
            {
                return _marqueeUseConsoleAPI;
            }
            set
            {
                _marqueeUseConsoleAPI = value;
            }
        }
        /// <summary>
        /// [Marquee] Screensaver background color
        /// </summary>
        public static string MarqueeBackgroundColor
        {
            get
            {
                return _marqueeBackgroundColor;
            }
            set
            {
                _marqueeBackgroundColor = value;
            }
        }
        /// <summary>
        /// [Marquee] The minimum red color level (true color)
        /// </summary>
        public static int MarqueeMinimumRedColorLevel
        {
            get
            {
                return _marqueeMinimumRedColorLevel;
            }
            set
            {
                _marqueeMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Marquee] The minimum green color level (true color)
        /// </summary>
        public static int MarqueeMinimumGreenColorLevel
        {
            get
            {
                return _marqueeMinimumGreenColorLevel;
            }
            set
            {
                _marqueeMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Marquee] The minimum blue color level (true color)
        /// </summary>
        public static int MarqueeMinimumBlueColorLevel
        {
            get
            {
                return _marqueeMinimumBlueColorLevel;
            }
            set
            {
                _marqueeMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Marquee] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int MarqueeMinimumColorLevel
        {
            get
            {
                return _marqueeMinimumColorLevel;
            }
            set
            {
                _marqueeMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Marquee] The maximum red color level (true color)
        /// </summary>
        public static int MarqueeMaximumRedColorLevel
        {
            get
            {
                return _marqueeMaximumRedColorLevel;
            }
            set
            {
                _marqueeMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Marquee] The maximum green color level (true color)
        /// </summary>
        public static int MarqueeMaximumGreenColorLevel
        {
            get
            {
                return _marqueeMaximumGreenColorLevel;
            }
            set
            {
                _marqueeMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Marquee] The maximum blue color level (true color)
        /// </summary>
        public static int MarqueeMaximumBlueColorLevel
        {
            get
            {
                return _marqueeMaximumBlueColorLevel;
            }
            set
            {
                _marqueeMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Marquee] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int MarqueeMaximumColorLevel
        {
            get
            {
                return _marqueeMaximumColorLevel;
            }
            set
            {
                _marqueeMaximumColorLevel = value;
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
            ColorTools.LoadBack(new Color(MarqueeSettings.MarqueeBackgroundColor), true);
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
            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Top position: {0}", TopPrinted);

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
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                ColorTools.SetConsoleColor(new Color($"{RedColorNum};{GreenColorNum};{BlueColorNum}"));
            }
            else
            {
                int color = RandomDriver.Random(MarqueeSettings.MarqueeMinimumColorLevel, MarqueeSettings.MarqueeMaximumColorLevel);
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", color);
                ColorTools.SetConsoleColor(new Color(color));
            }

            // If the text is at the right and is longer than the console width, crop it until it's complete.
            while (CurrentLeftOtherEnd != 0)
            {
                ThreadManager.SleepNoBlock(MarqueeSettings.MarqueeDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                if (ConsoleResizeListener.WasResized(false))
                    break;
                if (MarqueeSettings.MarqueeUseConsoleAPI)
                    ConsoleWrapper.Clear();
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Current left: {0} | Current left on other end: {1}", CurrentLeft, CurrentLeftOtherEnd);

                // Declare variable for written marquee text
                string MarqueeWritten = MarqueeSettings.MarqueeWrite;
                bool Middle = MarqueeSettings.MarqueeWrite.Length - (CurrentLeftOtherEnd - CurrentLeft) != CurrentCharacterNum - (CurrentLeftOtherEnd - CurrentLeft);
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Middle of long text: {0}", Middle);

                // If the current left position is not zero (not on the left), take the substring starting from the beginning of the string until the
                // written variable equals the base text variable. However, if we're on the left, take the substring so that the character which was
                // shown previously won't be shown again.
                if (!(CurrentLeft == 0))
                {
                    MarqueeWritten = MarqueeWritten.Substring(0, CurrentLeftOtherEnd - CurrentLeft);
                }
                else if (CurrentLeft == 0 & Middle)
                {
                    MarqueeWritten = MarqueeWritten.Substring(CurrentCharacterNum - (CurrentLeftOtherEnd - CurrentLeft), CurrentLeftOtherEnd - CurrentLeft);
                }
                else
                {
                    MarqueeWritten = MarqueeWritten.Substring(MarqueeSettings.MarqueeWrite.Length - (CurrentLeftOtherEnd - CurrentLeft));
                }
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Written result: {0}", MarqueeWritten);
                if (!MarqueeSettings.MarqueeUseConsoleAPI)
                    MarqueeWritten += Convert.ToString(CharManager.GetEsc()) + "[0K";

                // Set the appropriate cursor position and write the results
                ConsoleWrapper.SetCursorPosition(CurrentLeft, TopPrinted);
                ConsoleWrapper.Write(MarqueeWritten);
                if (Middle)
                    CurrentCharacterNum += 1;

                // If we're not on the left, decrement the current left position
                if (!(CurrentLeft == 0))
                {
                    CurrentLeft -= 1;
                    DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Not on left. Decremented left position {0}", CurrentLeft);
                }

                // If we're on the left or the entire text is written, decrement the current left other end position
                if (!Middle)
                {
                    CurrentLeftOtherEnd -= 1;
                    DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "On left or entire text written. Decremented left other end position {0}", CurrentLeftOtherEnd);
                }
            }

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(MarqueeSettings.MarqueeDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
