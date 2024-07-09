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

using System.Collections.Generic;
using System.Linq;
using Terminaux.Inputs.Interactive;
using Nitrocid.Languages;
using Newtonsoft.Json.Linq;
using System.Text;

namespace Nitrocid.Extras.Stocks.Interactives
{
    /// <summary>
    /// Stocks TUI class
    /// </summary>
    public class StocksCli : BaseInteractiveTui<string>, IInteractiveTui<string>
    {
        internal JToken stocksToken;
        internal string ianaTimeZone;

        /// <inheritdoc/>
        public override IEnumerable<string> PrimaryDataSource =>
            stocksToken.Select((token) => ((JProperty)token).Name);

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
            string open = (string)stocksToken[item]["1. open"];
            string high = (string)stocksToken[item]["2. high"];
            string low = (string)stocksToken[item]["3. low"];
            string close = (string)stocksToken[item]["4. close"];
            string volume = (string)stocksToken[item]["5. volume"];

            builder.AppendLine(Translate.DoTranslation("Opening stock price") + $": {open}");
            builder.AppendLine(Translate.DoTranslation("High stock price") + $": {high}");
            builder.AppendLine(Translate.DoTranslation("Low stock price") + $": {low}");
            builder.AppendLine(Translate.DoTranslation("Closing stock price") + $": {close}");
            builder.AppendLine(Translate.DoTranslation("Stock volume") + $": {volume}");
            return builder.ToString();
        }
    }
}
