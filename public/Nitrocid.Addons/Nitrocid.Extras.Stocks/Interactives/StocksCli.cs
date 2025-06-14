//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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

using System.Collections.Generic;
using System.Linq;
using Terminaux.Inputs.Interactive;
using Nitrocid.Languages;
using Newtonsoft.Json.Linq;
using System.Text;
using Nitrocid.Kernel.Exceptions;

namespace Nitrocid.Extras.Stocks.Interactives
{
    /// <summary>
    /// Stocks TUI class
    /// </summary>
    public class StocksCli : BaseInteractiveTui<string>, IInteractiveTui<string>
    {
        internal JToken? stocksToken;
        internal string? ianaTimeZone;

        /// <inheritdoc/>
        public override IEnumerable<string> PrimaryDataSource =>
            stocksToken?.Select((token) => ((JProperty)token).Name) ??
                throw new KernelException(KernelExceptionType.Unknown, LanguageTools.GetLocalized("NKS_STOCKS_TUI_CANTOBTAININFO", "Nitrocid.Extras.Stocks"));

        /// <inheritdoc/>
        public override string GetStatusFromItem(string item) =>
            $"{item} @ {ianaTimeZone}";

        /// <inheritdoc/>
        public override string GetEntryFromItem(string item) =>
            item;

        /// <inheritdoc/>
        public override string GetInfoFromItem(string item)
        {
            var builder = new StringBuilder();
            if (stocksToken is null)
                return "";
            var itemToken = stocksToken[item];
            if (itemToken is null)
                return "";
            string? open = (string?)itemToken["1. open"];
            string? high = (string?)itemToken["2. high"];
            string? low = (string?)itemToken["3. low"];
            string? close = (string?)itemToken["4. close"];
            string? volume = (string?)itemToken["5. volume"];

            builder.AppendLine(LanguageTools.GetLocalized("NKS_STOCKS_TUI_OPENING", "Nitrocid.Extras.Stocks") + $": {open}");
            builder.AppendLine(LanguageTools.GetLocalized("NKS_STOCKS_TUI_HIGH", "Nitrocid.Extras.Stocks") + $": {high}");
            builder.AppendLine(LanguageTools.GetLocalized("NKS_STOCKS_TUI_LOW", "Nitrocid.Extras.Stocks") + $": {low}");
            builder.AppendLine(LanguageTools.GetLocalized("NKS_STOCKS_TUI_CLOSING", "Nitrocid.Extras.Stocks") + $": {close}");
            builder.AppendLine(LanguageTools.GetLocalized("NKS_STOCKS_TUI_VOLUME", "Nitrocid.Extras.Stocks") + $": {volume}");
            return builder.ToString();
        }
    }
}
