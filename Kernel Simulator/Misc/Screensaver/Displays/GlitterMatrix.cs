using System;
using System.Collections.Generic;
using ColorSeq;
using KS.ConsoleBase.Colors;
using KS.Misc.Threading;
using KS.Misc.Writers.DebugWriters;

// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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

namespace KS.Misc.Screensaver.Displays
{
    public static class GlitterMatrixSettings
    {

        private static int _glitterMatrixDelay = 1;
        private static string _glitterMatrixBackgroundColor = new Color((int)ConsoleColor.Black).PlainSequence;
        private static string _glitterMatrixForegroundColor = new Color((int)ConsoleColor.Green).PlainSequence;

        /// <summary>
        /// [GlitterMatrix] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int GlitterMatrixDelay
        {
            get
            {
                return _glitterMatrixDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1;
                _glitterMatrixDelay = value;
            }
        }
        /// <summary>
        /// [GlitterMatrix] Screensaver background color
        /// </summary>
        public static string GlitterMatrixBackgroundColor
        {
            get
            {
                return _glitterMatrixBackgroundColor;
            }
            set
            {
                _glitterMatrixBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [GlitterMatrix] Screensaver foreground color
        /// </summary>
        public static string GlitterMatrixForegroundColor
        {
            get
            {
                return _glitterMatrixForegroundColor;
            }
            set
            {
                _glitterMatrixForegroundColor = new Color(value).PlainSequence;
            }
        }

    }
    public class GlitterMatrixDisplay : BaseScreensaver, IScreensaver
    {

        private Random RandomDriver;
        private int CurrentWindowWidth;
        private int CurrentWindowHeight;
        private bool ResizeSyncing;

        public override string ScreensaverName { get; set; } = "GlitterMatrix";

        public override Dictionary<string, object> ScreensaverSettings { get; set; }

        public override void ScreensaverPreparation()
        {
            // Variable preparations
            RandomDriver = new Random();
            CurrentWindowWidth = Console.WindowWidth;
            CurrentWindowHeight = Console.WindowHeight;
            ColorTools.SetConsoleColor(new Color(GlitterMatrixSettings.GlitterMatrixBackgroundColor), true, true);
            ColorTools.SetConsoleColor(new Color(GlitterMatrixSettings.GlitterMatrixForegroundColor));
            Console.Clear();
            DebugWriter.Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight);
        }

        public override void ScreensaverLogic()
        {
            Console.CursorVisible = false;
            int Left = RandomDriver.Next(Console.WindowWidth);
            int Top = RandomDriver.Next(Console.WindowHeight);
            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Selected left and top: {0}, {1}", Left, Top);
            Console.SetCursorPosition(Left, Top);
            if (CurrentWindowHeight != Console.WindowHeight | CurrentWindowWidth != Console.WindowWidth)
                ResizeSyncing = true;
            if (!ResizeSyncing)
            {
                Console.Write(RandomDriver.Next(2).ToString());
            }
            else
            {
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.W, "Color-syncing. Clearing...");
                Console.Clear();
            }

            // Reset resize sync
            ResizeSyncing = false;
            CurrentWindowWidth = Console.WindowWidth;
            CurrentWindowHeight = Console.WindowHeight;
            ThreadManager.SleepNoBlock(GlitterMatrixSettings.GlitterMatrixDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}