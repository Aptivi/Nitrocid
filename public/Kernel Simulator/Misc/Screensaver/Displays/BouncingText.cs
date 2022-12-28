
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
using KS.ConsoleBase;
using KS.Drivers.RNG;
using KS.Kernel.Debugging;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using ColorTools = KS.ConsoleBase.Colors.ColorTools;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for BouncingText
    /// </summary>
    public static class BouncingTextSettings
    {

        private static bool _bouncingTextTrueColor = true;
        private static int _bouncingTextDelay = 10;
        private static string _bouncingTextWrite = "Kernel Simulator";
        private static string _bouncingTextBackgroundColor = new Color((int)ConsoleColor.Black).PlainSequence;
        private static string _bouncingTextForegroundColor = new Color((int)ConsoleColor.White).PlainSequence;
        private static int _bouncingTextMinimumRedColorLevel = 0;
        private static int _bouncingTextMinimumGreenColorLevel = 0;
        private static int _bouncingTextMinimumBlueColorLevel = 0;
        private static int _bouncingTextMinimumColorLevel = 0;
        private static int _bouncingTextMaximumRedColorLevel = 255;
        private static int _bouncingTextMaximumGreenColorLevel = 255;
        private static int _bouncingTextMaximumBlueColorLevel = 255;
        private static int _bouncingTextMaximumColorLevel = 255;

        /// <summary>
        /// [BouncingText] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool BouncingTextTrueColor
        {
            get
            {
                return _bouncingTextTrueColor;
            }
            set
            {
                _bouncingTextTrueColor = value;
            }
        }
        /// <summary>
        /// [BouncingText] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int BouncingTextDelay
        {
            get
            {
                return _bouncingTextDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                _bouncingTextDelay = value;
            }
        }
        /// <summary>
        /// [BouncingText] Text for Bouncing Text. Shorter is better.
        /// </summary>
        public static string BouncingTextWrite
        {
            get
            {
                return _bouncingTextWrite;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Kernel Simulator";
                _bouncingTextWrite = value;
            }
        }
        /// <summary>
        /// [BouncingText] Screensaver background color
        /// </summary>
        public static string BouncingTextBackgroundColor
        {
            get
            {
                return _bouncingTextBackgroundColor;
            }
            set
            {
                _bouncingTextBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [BouncingText] Screensaver foreground color
        /// </summary>
        public static string BouncingTextForegroundColor
        {
            get
            {
                return _bouncingTextForegroundColor;
            }
            set
            {
                _bouncingTextForegroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [BouncingText] The minimum red color level (true color)
        /// </summary>
        public static int BouncingTextMinimumRedColorLevel
        {
            get
            {
                return _bouncingTextMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _bouncingTextMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingText] The minimum green color level (true color)
        /// </summary>
        public static int BouncingTextMinimumGreenColorLevel
        {
            get
            {
                return _bouncingTextMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _bouncingTextMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingText] The minimum blue color level (true color)
        /// </summary>
        public static int BouncingTextMinimumBlueColorLevel
        {
            get
            {
                return _bouncingTextMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _bouncingTextMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingText] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int BouncingTextMinimumColorLevel
        {
            get
            {
                return _bouncingTextMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                _bouncingTextMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingText] The maximum red color level (true color)
        /// </summary>
        public static int BouncingTextMaximumRedColorLevel
        {
            get
            {
                return _bouncingTextMaximumRedColorLevel;
            }
            set
            {
                if (value <= _bouncingTextMinimumRedColorLevel)
                    value = _bouncingTextMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                _bouncingTextMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingText] The maximum green color level (true color)
        /// </summary>
        public static int BouncingTextMaximumGreenColorLevel
        {
            get
            {
                return _bouncingTextMaximumGreenColorLevel;
            }
            set
            {
                if (value <= _bouncingTextMinimumGreenColorLevel)
                    value = _bouncingTextMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                _bouncingTextMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingText] The maximum blue color level (true color)
        /// </summary>
        public static int BouncingTextMaximumBlueColorLevel
        {
            get
            {
                return _bouncingTextMaximumBlueColorLevel;
            }
            set
            {
                if (value <= _bouncingTextMinimumBlueColorLevel)
                    value = _bouncingTextMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                _bouncingTextMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingText] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int BouncingTextMaximumColorLevel
        {
            get
            {
                return _bouncingTextMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= _bouncingTextMinimumColorLevel)
                    value = _bouncingTextMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                _bouncingTextMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for BouncingText
    /// </summary>
    public class BouncingTextDisplay : BaseScreensaver, IScreensaver
    {

        private string Direction = "BottomRight";
        private int RowText, ColumnFirstLetter, ColumnLastLetter;
        private Color BouncingColor;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "BouncingText";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ColorTools.SetConsoleColor(new Color(BouncingTextSettings.BouncingTextForegroundColor));
            ColorTools.LoadBack(new Color(BouncingTextSettings.BouncingTextBackgroundColor), true);
            RowText = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d);
            ColumnFirstLetter = (int)Math.Round(ConsoleWrapper.WindowWidth / 2d - BouncingTextSettings.BouncingTextWrite.Length / 2d);
            ColumnLastLetter = (int)Math.Round(ConsoleWrapper.WindowWidth / 2d + BouncingTextSettings.BouncingTextWrite.Length / 2d);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;
            ConsoleWrapper.Clear();

            // Define the color
            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Row text: {0}", RowText);
            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Column first letter of text: {0}", ColumnFirstLetter);
            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Column last letter of text: {0}", ColumnLastLetter);
            if (BouncingColor is null)
            {
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Defining color...");
                BouncingColor = ChangeBouncingTextColor();
            }
            if (!ConsoleResizeListener.WasResized(false))
            {
                TextWriterWhereColor.WriteWhere(BouncingTextSettings.BouncingTextWrite, ColumnFirstLetter, RowText, true, BouncingColor);
            }
            else
            {
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.W, "We're resize-syncing! Setting RowText, ColumnFirstLetter, and ColumnLastLetter to its original position...");
                RowText = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d);
                ColumnFirstLetter = (int)Math.Round(ConsoleWrapper.WindowWidth / 2d - BouncingTextSettings.BouncingTextWrite.Length / 2d);
                ColumnLastLetter = (int)Math.Round(ConsoleWrapper.WindowWidth / 2d + BouncingTextSettings.BouncingTextWrite.Length / 2d);
            }

            // Change the direction of text
            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Text is facing {0}.", Direction);
            if (Direction == "BottomRight")
            {
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Increasing row and column text position");
                RowText += 1;
                ColumnFirstLetter += 1;
                ColumnLastLetter += 1;
            }
            else if (Direction == "BottomLeft")
            {
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Increasing row and decreasing column text position");
                RowText += 1;
                ColumnFirstLetter -= 1;
                ColumnLastLetter -= 1;
            }
            else if (Direction == "TopRight")
            {
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Decreasing row and increasing column text position");
                RowText -= 1;
                ColumnFirstLetter += 1;
                ColumnLastLetter += 1;
            }
            else if (Direction == "TopLeft")
            {
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Decreasing row and column text position");
                RowText -= 1;
                ColumnFirstLetter -= 1;
                ColumnLastLetter -= 1;
            }

            // Check to see if the text is on the edge
            if (RowText == ConsoleWrapper.WindowHeight - 2)
            {
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "We're on the bottom.");
                Direction = Direction.Replace("Bottom", "Top");
                BouncingColor = ChangeBouncingTextColor();
            }
            else if (RowText == 1)
            {
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "We're on the top.");
                Direction = Direction.Replace("Top", "Bottom");
                BouncingColor = ChangeBouncingTextColor();
            }

            if (ColumnLastLetter == ConsoleWrapper.WindowWidth - 1)
            {
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "We're on the right.");
                Direction = Direction.Replace("Right", "Left");
                BouncingColor = ChangeBouncingTextColor();
            }
            else if (ColumnFirstLetter == 1)
            {
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "We're on the left.");
                Direction = Direction.Replace("Left", "Right");
                BouncingColor = ChangeBouncingTextColor();
            }

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(BouncingTextSettings.BouncingTextDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

        /// <summary>
        /// Changes the color of bouncing text
        /// </summary>
        public Color ChangeBouncingTextColor()
        {
            Color ColorInstance;
            if (BouncingTextSettings.BouncingTextTrueColor)
            {
                int RedColorNum = RandomDriver.Random(BouncingTextSettings.BouncingTextMinimumRedColorLevel, BouncingTextSettings.BouncingTextMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(BouncingTextSettings.BouncingTextMinimumGreenColorLevel, BouncingTextSettings.BouncingTextMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(BouncingTextSettings.BouncingTextMinimumBlueColorLevel, BouncingTextSettings.BouncingTextMaximumBlueColorLevel);
                ColorInstance = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(BouncingTextSettings.BouncingTextMinimumColorLevel, BouncingTextSettings.BouncingTextMaximumColorLevel);
                ColorInstance = new Color(ColorNum);
            }
            return ColorInstance;
        }

    }
}
