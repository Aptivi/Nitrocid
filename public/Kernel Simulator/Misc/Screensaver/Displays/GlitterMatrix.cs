
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

using System;
using System.Collections.Generic;
using ColorSeq;
using KS.ConsoleBase.Colors;
using KS.Drivers.RNG;
using KS.Kernel.Debugging;
using KS.Misc.Threading;
using ColorTools = KS.ConsoleBase.Colors.ColorTools;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for GlitterMatrix
    /// </summary>
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

    /// <summary>
    /// Display code for GlitterMatrix
    /// </summary>
    public class GlitterMatrixDisplay : BaseScreensaver, IScreensaver
    {

        private int CurrentWindowWidth;
        private int CurrentWindowHeight;
        private bool ResizeSyncing;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "GlitterMatrix";

        /// <inheritdoc/>
        public override Dictionary<string, object> ScreensaverSettings { get; set; }

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            CurrentWindowWidth = ConsoleBase.ConsoleWrapper.WindowWidth;
            CurrentWindowHeight = ConsoleBase.ConsoleWrapper.WindowHeight;
            ColorTools.SetConsoleColor(new Color(GlitterMatrixSettings.GlitterMatrixForegroundColor));
            ColorTools.LoadBack(new Color(GlitterMatrixSettings.GlitterMatrixBackgroundColor), true);
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleBase.ConsoleWrapper.WindowWidth, ConsoleBase.ConsoleWrapper.WindowHeight);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleBase.ConsoleWrapper.CursorVisible = false;
            int Left = RandomDriver.RandomIdx(ConsoleBase.ConsoleWrapper.WindowWidth);
            int Top = RandomDriver.RandomIdx(ConsoleBase.ConsoleWrapper.WindowHeight);
            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Selected left and top: {0}, {1}", Left, Top);
            ConsoleBase.ConsoleWrapper.SetCursorPosition(Left, Top);
            if (CurrentWindowHeight != ConsoleBase.ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleBase.ConsoleWrapper.WindowWidth)
                ResizeSyncing = true;
            if (!ResizeSyncing)
            {
                ConsoleBase.ConsoleWrapper.Write(RandomDriver.Random(1).ToString());
            }
            else
            {
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.W, "Color-syncing. Clearing...");
                ConsoleBase.ConsoleWrapper.Clear();
            }

            // Reset resize sync
            ResizeSyncing = false;
            CurrentWindowWidth = ConsoleBase.ConsoleWrapper.WindowWidth;
            CurrentWindowHeight = ConsoleBase.ConsoleWrapper.WindowHeight;
            ThreadManager.SleepNoBlock(GlitterMatrixSettings.GlitterMatrixDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
