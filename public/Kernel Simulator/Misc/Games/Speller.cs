
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
using System.Collections.Generic;
using System.Linq;
using Extensification.StringExts;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Drivers.RNG;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Network.Base.Transfer;

namespace KS.Misc.Games
{
    static class Speller
    {

        public static List<string> Words = new();

        /// <summary>
        /// Initializes the game
        /// </summary>
        public static void InitializeWords()
        {
            string RandomWord;
            string SpeltWord;
            TextWriterColor.Write(Translate.DoTranslation("Press CTRL+C to exit."), true, KernelColorType.Tip);
            if (Words.Count == 0)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Downloading words...");
                Words.AddRange(NetworkTransfer.DownloadString("https://cdn.jsdelivr.net/gh/sindresorhus/word-list/words.txt").SplitNewLines().ToList());
            }
            while (true)
            {
                RandomWord = Words.ElementAt(RandomDriver.Random(Words.Count));
                DebugWriter.WriteDebug(DebugLevel.I, "Word: {0}", RandomWord);
                TextWriterColor.Write(RandomWord, true, KernelColorType.Input);
                SpeltWord = Input.ReadLineNoInput(Convert.ToChar("\0"));

                if (SpeltWord == RandomWord)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Spelt: {0} = {1}", SpeltWord, RandomWord);
                    TextWriterColor.Write(Translate.DoTranslation("Spelt perfectly!"), true, KernelColorType.Success);
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Spelt: {0} != {1}", SpeltWord, RandomWord);
                    TextWriterColor.Write(Translate.DoTranslation("Spelt incorrectly."), true, KernelColorType.Warning);
                }
            }
        }

    }
}
