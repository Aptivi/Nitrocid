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

using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Drivers.RNG;
using KS.Kernel.Debugging;
using KS.Kernel.Threading;
using KS.Misc.Screensaver;
using System.Collections.Generic;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Settings for LetterScatter
    /// </summary>
    public static class LetterScatterSettings
    {

        /// <summary>
        /// [LetterScatter] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int LetterScatterDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.LetterScatterDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1;
                ScreensaverPackInit.SaversConfig.LetterScatterDelay = value;
            }
        }
        /// <summary>
        /// [LetterScatter] Screensaver background color
        /// </summary>
        public static string LetterScatterBackgroundColor
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.LetterScatterBackgroundColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.LetterScatterBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [LetterScatter] Screensaver foreground color
        /// </summary>
        public static string LetterScatterForegroundColor
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.LetterScatterForegroundColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.LetterScatterForegroundColor = new Color(value).PlainSequence;
            }
        }

    }

    /// <summary>
    /// Display code for LetterScatter
    /// </summary>
    public class LetterScatterDisplay : BaseScreensaver, IScreensaver
    {

        private readonly Dictionary<(int, int), char> characters = [];
        private readonly char minChar = 'a';
        private readonly char maxChar = 'z';

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "LetterScatter";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            characters.Clear();
            KernelColorTools.SetConsoleColor(new Color(LetterScatterSettings.LetterScatterForegroundColor));
            KernelColorTools.LoadBack(new Color(LetterScatterSettings.LetterScatterBackgroundColor));
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;
            int Left = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
            int Top = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
            var leftTop = (Left, Top);
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Selected left and top: {0}, {1}", Left, Top);
            if (!ConsoleResizeListener.WasResized(false))
            {
                if (characters.TryGetValue(leftTop, out char charValue))
                {
                    characters[leftTop] = ++charValue;
                    if (charValue >= maxChar)
                        characters[leftTop] = minChar;
                }
                else
                    characters.Add(leftTop, minChar);
                TextWriterWhereColor.WriteWhere(characters[leftTop].ToString(), Left, Top);
            }
            else
            {
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.W, "Color-syncing. Clearing...");
                ConsoleWrapper.Clear();
            }

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(LetterScatterSettings.LetterScatterDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
