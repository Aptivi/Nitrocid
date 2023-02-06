
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

using KS.ConsoleBase;
using KS.Kernel.Debugging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VT.NET;

namespace KS.Misc.Text
{
    /// <summary>
    /// Tools for text manipulation
    /// </summary>
    public static class TextTools
    {
        /// <summary>
        /// Gets the wrapped sentences for text wrapping for console
        /// </summary>
        /// <param name="text">Text to be wrapped</param>
        /// <param name="maximumLength">Maximum length of text before wrapping</param>
        public static string[] GetWrappedSentences(string text, int maximumLength) =>
            GetWrappedSentences(text, maximumLength, 0);

        /// <summary>
        /// Gets the wrapped sentences for text wrapping for console
        /// </summary>
        /// <param name="text">Text to be wrapped</param>
        /// <param name="maximumLength">Maximum length of text before wrapping</param>
        /// <param name="indentLength">Indentation length</param>
        public static string[] GetWrappedSentences(string text, int maximumLength, int indentLength)
        {
            if (string.IsNullOrEmpty(text))
                return new string[] { "" };

            // Split the paragraph into sentences that have the length of maximum characters that can be printed in various terminal
            // sizes.
            var IncompleteSentences = new List<string>();
            var IncompleteSentenceBuilder = new StringBuilder();

            // This indent length count tells us how many spaces are used for indenting the paragraph. This is only set for
            // the first time and will be reverted back to zero after the incomplete sentence is formed.
            for (int i = 0; i < text.Length; i++)
            {
                // Check the character to see if we're at the VT sequence
                char ParagraphChar = text[i];

                // Append the character into the incomplete sentence builder.
                IncompleteSentenceBuilder.Append(ParagraphChar);

                // Check to see if we're at the maximum character number
                if (IncompleteSentenceBuilder.Length == maximumLength - indentLength | i == text.Length - 1)
                {
                    // We're at the character number of maximum character. Add the sentence to the list for "wrapping" in columns.
                    DebugWriter.WriteDebug(DebugLevel.I, "Adding {0} to the list... Incomplete sentences: {1}", IncompleteSentenceBuilder.ToString(), IncompleteSentences.Count);
                    IncompleteSentences.Add(IncompleteSentenceBuilder.ToString());

                    // Clean everything up
                    IncompleteSentenceBuilder.Clear();
                    indentLength = 0;
                }
            }

            return IncompleteSentences.ToArray();
        }
    }
}
