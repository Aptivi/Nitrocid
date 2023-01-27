
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

using Extensification.StringExts;
using KS.Drivers.RNG;
using KS.Kernel.Debugging;
using KS.Network.Base.Transfer;
using System.Collections.Generic;
using System.Linq;

namespace KS.Misc.Text
{
    /// <summary>
    /// Word management class
    /// </summary>
    public class WordManager
    {
        private static readonly List<string> Words = new();

        /// <summary>
        /// Initializes the words. Does nothing if already downloaded.
        /// </summary>
        public static void InitializeWords()
        {
            // Download the words
            if (Words.Count == 0)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Downloading words...");
                Words.AddRange(NetworkTransfer.DownloadString("https://cdn.jsdelivr.net/gh/sindresorhus/word-list/words.txt").SplitNewLines().ToList());
            }
        }

        /// <summary>
        /// Gets a random word
        /// </summary>
        /// <returns>A random word</returns>
        public static string GetRandomWord()
        {
            InitializeWords();
            return Words[RandomDriver.RandomIdx(Words.Count)];
        }

        /// <summary>
        /// Gets a random word conditionally
        /// </summary>
        /// <param name="wordMaxLength">The maximum length of the word</param>
        /// <param name="wordStartsWith">The word starts with...</param>
        /// <param name="wordEndsWith">The word ends with...</param>
        /// <returns>A random word</returns>
        public static string GetRandomWordConditional(int wordMaxLength, string wordStartsWith, string wordEndsWith)
        {
            // Get an initial word
            string word = GetRandomWord();
            bool lengthCheck = wordMaxLength > 0;
            bool startsCheck = !string.IsNullOrWhiteSpace(wordStartsWith);
            bool endsCheck = !string.IsNullOrWhiteSpace(wordEndsWith);

            // Loop until all the conditions that need to be checked are satisfied
            while (!(((lengthCheck && word.Length <= wordMaxLength)    || !lengthCheck) &&
                     ((startsCheck && word.StartsWith(wordStartsWith)) || !startsCheck) &&
                     ((endsCheck   && word.EndsWith(wordEndsWith))     || !endsCheck)))
                word = GetRandomWord();

            // Get a word that satisfies all the conditions
            return word;
        }
    }
}
