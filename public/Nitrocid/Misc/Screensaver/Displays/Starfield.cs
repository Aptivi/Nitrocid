
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
using System.Collections.Generic;
using ColorSeq;
using KS.ConsoleBase;
using KS.Drivers.RNG;
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

        private static int _Delay = 10;

        /// <summary>
        /// [Starfield] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int StarfieldDelay
        {
            get
            {
                return _Delay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                _Delay = value;
            }
        }

    }

    /// <summary>
    /// Display code for Starfield
    /// </summary>
    public class StarfieldDisplay : BaseScreensaver, IScreensaver
    {

        private readonly List<Tuple<int, int>> Stars = new();

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Starfield";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Move the stars left
            for (int Star = 0; Star <= Stars.Count - 1; Star++)
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
            bool StarShowGuaranteed = RandomDriver.RandomChance(10);
            if (StarShowGuaranteed)
            {
                int StarX = ConsoleWrapper.WindowWidth - 1;
                int StarY = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
                Stars.Add(new Tuple<int, int>(StarX, StarY));
            }

            // Draw stars
            if (!ConsoleResizeListener.WasResized(false))
            {
                for (int StarIndex = Stars.Count - 1; StarIndex >= 0; StarIndex -= 1)
                {
                    var Star = Stars[StarIndex];
                    char StarSymbol = '*';
                    int StarX = Star.Item1;
                    int StarY = Star.Item2;
                    TextWriterWhereColor.WriteWhere(Convert.ToString(StarSymbol), StarX, StarY, false, ConsoleColors.White);
                }
            }
            else
            {
                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.W, "Resize-syncing. Clearing...");
                Stars.Clear();
            }

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(StarfieldSettings.StarfieldDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            ConsoleWrapper.Clear();
        }

    }
}
