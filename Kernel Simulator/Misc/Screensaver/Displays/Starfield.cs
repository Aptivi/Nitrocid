
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
using KS.Kernel.Debugging;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for Starfield
    /// </summary>
    public static class StarfieldSettings
    {

        private static int _starfieldDelay = 10;

        /// <summary>
        /// [Starfield] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int StarfieldDelay
        {
            get
            {
                return _starfieldDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                _starfieldDelay = value;
            }
        }

    }

    /// <summary>
    /// Display code for Starfield
    /// </summary>
    public class StarfieldDisplay : BaseScreensaver, IScreensaver
    {

        private Random RandomDriver;
        private int CurrentWindowWidth;
        private int CurrentWindowHeight;
        private bool ResizeSyncing;
        private readonly List<Tuple<int, int>> Stars = new();

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Starfield";

        /// <inheritdoc/>
        public override Dictionary<string, object> ScreensaverSettings { get; set; }

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            RandomDriver = new Random();
            CurrentWindowWidth = ConsoleBase.ConsoleWrapper.WindowWidth;
            CurrentWindowHeight = ConsoleBase.ConsoleWrapper.WindowHeight;
            ConsoleBase.ConsoleWrapper.BackgroundColor = ConsoleColor.Black;
            ConsoleBase.ConsoleWrapper.ForegroundColor = ConsoleColor.White;
            ConsoleBase.ConsoleWrapper.Clear();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleBase.ConsoleWrapper.CursorVisible = false;

            // Move the stars left
            for (int Star = 0, loopTo = Stars.Count - 1; Star <= loopTo; Star++)
            {
                int StarX = Stars[Star].Item1 - 1;
                int StarY = Stars[Star].Item2;
                Stars[Star] = new Tuple<int, int>(StarX, StarY);
            }

            // If any star is out of X range, delete it
            for (int StarIndex = Stars.Count - 1; StarIndex >= 0; StarIndex -= 1)
            {
                var Star = Stars[StarIndex];
                if (Star.Item1 < 0)
                {
                    // The star went beyond. Remove it.
                    Stars.RemoveAt(StarIndex);
                }
            }

            // Add new star if guaranteed
            double StarShowProbability = 10d / 100d;
            bool StarShowGuaranteed = RandomDriver.NextDouble() < StarShowProbability;
            if (StarShowGuaranteed)
            {
                int StarX = ConsoleBase.ConsoleWrapper.WindowWidth - 1;
                int StarY = RandomDriver.Next(ConsoleBase.ConsoleWrapper.WindowHeight - 1);
                Stars.Add(new Tuple<int, int>(StarX, StarY));
            }

            // Draw stars
            if (CurrentWindowHeight != ConsoleBase.ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleBase.ConsoleWrapper.WindowWidth)
                ResizeSyncing = true;
            if (!ResizeSyncing)
            {
                for (int StarIndex = Stars.Count - 1; StarIndex >= 0; StarIndex -= 1)
                {
                    var Star = Stars[StarIndex];
                    char StarSymbol = '*';
                    int StarX = Star.Item1;
                    int StarY = Star.Item2;
                    TextWriterWhereColor.WriteWhere(Convert.ToString(StarSymbol), StarX, StarY, false, ConsoleColor.White);
                }
            }
            else
            {
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.W, "Resize-syncing. Clearing...");
                Stars.Clear();
            }

            // Reset resize sync
            ResizeSyncing = false;
            CurrentWindowWidth = ConsoleBase.ConsoleWrapper.WindowWidth;
            CurrentWindowHeight = ConsoleBase.ConsoleWrapper.WindowHeight;
            ThreadManager.SleepNoBlock(StarfieldSettings.StarfieldDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            ConsoleBase.ConsoleWrapper.Clear();
        }

    }
}
