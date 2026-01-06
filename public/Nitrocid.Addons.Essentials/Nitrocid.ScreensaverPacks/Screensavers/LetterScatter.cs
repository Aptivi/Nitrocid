//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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

using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Misc.Screensaver;
using Nitrocid.Base.Kernel.Configuration;
using System.Collections.Generic;
using Terminaux.Colors;
using Terminaux.Base;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for LetterScatter
    /// </summary>
    public class LetterScatterDisplay : BaseScreensaver, IScreensaver
    {

        private readonly Dictionary<(int, int), char> characters = [];
        private readonly char minChar = 'a';
        private readonly char maxChar = 'z';

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "LetterScatter";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            characters.Clear();
            ColorTools.SetConsoleColor(new Color(ScreensaverPackInit.SaversConfig.LetterScatterForegroundColor));
            ColorTools.LoadBackDry(new Color(ScreensaverPackInit.SaversConfig.LetterScatterBackgroundColor));
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", vars: [ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight]);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            int Left = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
            int Top = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
            var leftTop = (Left, Top);
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Selected left and top: {0}, {1}", vars: [Left, Top]);
            if (!ConsoleResizeHandler.WasResized(false))
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
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.W, "Color-syncing. Clearing...");
                ConsoleWrapper.Clear();
            }
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.LetterScatterDelay);
        }

    }
}
