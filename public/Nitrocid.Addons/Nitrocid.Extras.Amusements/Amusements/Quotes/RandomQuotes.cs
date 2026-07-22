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

using Colorimetry;
using Nettify.Quotes;
using Nitrocid.Languages;

namespace Nitrocid.Extras.Amusements.Amusements.Quotes
{
    internal static class RandomQuotes
    {
        internal static string RenderQuote(Color? color = null)
        {
            // Get a random quote
            var quote = QuoteTools.GetRandomQuote();
            string content = quote?.Content ?? LanguageTools.GetLocalized("NKS_AMUSEMENTS_QUOTE_QUOTEUNKNOWN");
            string author = quote?.Author ?? LanguageTools.GetLocalized("NKS_AMUSEMENTS_QUOTE_AUTHORUNKNOWN");

            // Render the quote
            var renderableQuote = new Terminaux.Writer.CyclicWriters.Simple.Quote()
            {
                Author = author,
                QuoteText = content,
                UseColors = color is not null,
            };
            if (color is not null)
            {
                renderableQuote.ForegroundColor = color;
                renderableQuote.PadColor = color;
            }
            return renderableQuote.Render();
        }
    }
}
