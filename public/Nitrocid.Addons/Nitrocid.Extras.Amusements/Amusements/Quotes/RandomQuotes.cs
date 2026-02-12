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

using System.Text;
using Nettify.Quotes;
using Newtonsoft.Json.Linq;
using Nitrocid.Drivers.RNG;
using Nitrocid.Languages;
using Nitrocid.Misc.Reflection.Internal;
using Terminaux.Base.Extensions;
using Textify.General;

namespace Nitrocid.Extras.Amusements.Amusements.Quotes
{
    internal static class RandomQuotes
    {
        internal static string RenderQuote()
        {
            // Get a random quote
            var quote = QuoteTools.GetRandomQuote();
            string content = quote?.Content ?? Translate.DoTranslation("Unknown quote.");
            string author = quote?.Author ?? Translate.DoTranslation("Unknown author.");

            // Specify how to render the quote
            StringBuilder elegantQuote = new();
            int padding = 3;
            int maxQuoteWidth = 50;
            int maxWidth = 50 + padding * 2;

            // Get quote incomplete sentences
            string[] quoteSentences = ConsoleMisc.GetWrappedSentencesByWords(content, maxQuoteWidth);

            // Render the sentences to the builder
            for (int i = 0; i < quoteSentences.Length; i++)
            {
                string quoteSentence = quoteSentences[i];

                // If the sentence is the first one, don't do padding and write a quote mark.
                if (i == 0)
                    elegantQuote.Append($"\"{CharManager.NewLine}   ");
                else
                    elegantQuote.Append(new string(' ', padding));

                // Render the sentence
                elegantQuote.Append(quoteSentence);

                // Check to see if we're going to put the second quote mark
                if (i == quoteSentences.Length - 1)
                    elegantQuote.AppendLine(CharManager.NewLine + new string(' ', maxQuoteWidth + padding) + "  \"");
                else
                    elegantQuote.AppendLine();
            }

            // Now, write the name of the author
            int authorPosition = maxWidth - ConsoleChar.EstimateCellWidth(author);
            elegantQuote.AppendLine();
            elegantQuote.Append(new string(' ', authorPosition));
            elegantQuote.Append(author);
            return elegantQuote.ToString();
        }
    }
}
