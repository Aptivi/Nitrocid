
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
using KS.Kernel.Debugging;
using KS.Kernel.Threading;
using KS.Misc.Screensaver;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Settings for BouncingText
    /// </summary>
    public static class BouncingTextSettings
    {

        /// <summary>
        /// [BouncingText] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool BouncingTextTrueColor
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BouncingTextTrueColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.BouncingTextTrueColor = value;
            }
        }
        /// <summary>
        /// [BouncingText] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int BouncingTextDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BouncingTextDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                ScreensaverPackInit.SaversConfig.BouncingTextDelay = value;
            }
        }
        /// <summary>
        /// [BouncingText] Text for Bouncing Text. Shorter is better.
        /// </summary>
        public static string BouncingTextWrite
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BouncingTextWrite;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Nitrocid KS";
                ScreensaverPackInit.SaversConfig.BouncingTextWrite = value;
            }
        }
        /// <summary>
        /// [BouncingText] Screensaver background color
        /// </summary>
        public static string BouncingTextBackgroundColor
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BouncingTextBackgroundColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.BouncingTextBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [BouncingText] Screensaver foreground color
        /// </summary>
        public static string BouncingTextForegroundColor
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BouncingTextForegroundColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.BouncingTextForegroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [BouncingText] The minimum red color level (true color)
        /// </summary>
        public static int BouncingTextMinimumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BouncingTextMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.BouncingTextMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingText] The minimum green color level (true color)
        /// </summary>
        public static int BouncingTextMinimumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BouncingTextMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.BouncingTextMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingText] The minimum blue color level (true color)
        /// </summary>
        public static int BouncingTextMinimumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BouncingTextMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.BouncingTextMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingText] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int BouncingTextMinimumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BouncingTextMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                ScreensaverPackInit.SaversConfig.BouncingTextMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingText] The maximum red color level (true color)
        /// </summary>
        public static int BouncingTextMaximumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BouncingTextMaximumRedColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.BouncingTextMinimumRedColorLevel)
                    value = ScreensaverPackInit.SaversConfig.BouncingTextMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.BouncingTextMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingText] The maximum green color level (true color)
        /// </summary>
        public static int BouncingTextMaximumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BouncingTextMaximumGreenColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.BouncingTextMinimumGreenColorLevel)
                    value = ScreensaverPackInit.SaversConfig.BouncingTextMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.BouncingTextMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingText] The maximum blue color level (true color)
        /// </summary>
        public static int BouncingTextMaximumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BouncingTextMaximumBlueColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.BouncingTextMinimumBlueColorLevel)
                    value = ScreensaverPackInit.SaversConfig.BouncingTextMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.BouncingTextMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [BouncingText] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int BouncingTextMaximumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.BouncingTextMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= ScreensaverPackInit.SaversConfig.BouncingTextMinimumColorLevel)
                    value = ScreensaverPackInit.SaversConfig.BouncingTextMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                ScreensaverPackInit.SaversConfig.BouncingTextMaximumColorLevel = value;
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
        private int lastLeft, lastTop;
        private Color BouncingColor;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "BouncingText";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            KernelColorTools.SetConsoleColor(new Color(BouncingTextSettings.BouncingTextForegroundColor));
            KernelColorTools.LoadBack(new Color(BouncingTextSettings.BouncingTextBackgroundColor));
            RowText = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d);
            ColumnFirstLetter = (int)Math.Round(ConsoleWrapper.WindowWidth / 2d - BouncingTextSettings.BouncingTextWrite.Length / 2d);
            ColumnLastLetter = (int)Math.Round(ConsoleWrapper.WindowWidth / 2d + BouncingTextSettings.BouncingTextWrite.Length / 2d);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Clear the old text position
            int diff = ColumnLastLetter - ColumnFirstLetter + 1;
            TextWriterWhereColor.WriteWhere(new string(' ', diff), lastLeft, lastTop, true, Color.Empty, BouncingTextSettings.BouncingTextBackgroundColor);

            // Define the color
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Row text: {0}", RowText);
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Column first letter of text: {0}", ColumnFirstLetter);
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Column last letter of text: {0}", ColumnLastLetter);
            if (BouncingColor is null)
            {
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Defining color...");
                BouncingColor = ChangeBouncingTextColor();
            }
            if (!ConsoleResizeListener.WasResized(false))
            {
                TextWriterWhereColor.WriteWhere(BouncingTextSettings.BouncingTextWrite, ColumnFirstLetter, RowText, true, BouncingColor, BouncingTextSettings.BouncingTextBackgroundColor);
            }
            else
            {
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.W, "We're resize-syncing! Setting RowText, ColumnFirstLetter, and ColumnLastLetter to its original position...");
                RowText = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d);
                ColumnFirstLetter = (int)Math.Round(ConsoleWrapper.WindowWidth / 2d - BouncingTextSettings.BouncingTextWrite.Length / 2d);
                ColumnLastLetter = (int)Math.Round(ConsoleWrapper.WindowWidth / 2d + BouncingTextSettings.BouncingTextWrite.Length / 2d);
            }

            // Set the old positions to clear
            lastLeft = ColumnFirstLetter;
            lastTop = RowText;

            // Change the direction of text
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Text is facing {0}.", Direction);
            if (Direction == "BottomRight")
            {
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Increasing row and column text position");
                RowText += 1;
                ColumnFirstLetter += 1;
                ColumnLastLetter += 1;
            }
            else if (Direction == "BottomLeft")
            {
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Increasing row and decreasing column text position");
                RowText += 1;
                ColumnFirstLetter -= 1;
                ColumnLastLetter -= 1;
            }
            else if (Direction == "TopRight")
            {
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Decreasing row and increasing column text position");
                RowText -= 1;
                ColumnFirstLetter += 1;
                ColumnLastLetter += 1;
            }
            else if (Direction == "TopLeft")
            {
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Decreasing row and column text position");
                RowText -= 1;
                ColumnFirstLetter -= 1;
                ColumnLastLetter -= 1;
            }

            // Check to see if the text is on the edge
            if (RowText == ConsoleWrapper.WindowHeight - 2)
            {
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "We're on the bottom.");
                Direction = Direction.Replace("Bottom", "Top");
                BouncingColor = ChangeBouncingTextColor();
            }
            else if (RowText == 1)
            {
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "We're on the top.");
                Direction = Direction.Replace("Top", "Bottom");
                BouncingColor = ChangeBouncingTextColor();
            }

            if (ColumnLastLetter == ConsoleWrapper.WindowWidth - 1)
            {
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "We're on the right.");
                Direction = Direction.Replace("Right", "Left");
                BouncingColor = ChangeBouncingTextColor();
            }
            else if (ColumnFirstLetter == 1)
            {
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "We're on the left.");
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
