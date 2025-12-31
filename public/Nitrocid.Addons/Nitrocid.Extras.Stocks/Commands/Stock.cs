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

using Newtonsoft.Json.Linq;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Extras.Stocks.Interactives;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Network.Transfer;
using Terminaux.Shell.Commands;
using Terminaux.Inputs.Interactive;
using Terminaux.Reader;

namespace Nitrocid.Extras.Stocks.Commands
{
    /// <summary>
    /// Stocks interactive TUI (hourly stocks in full)
    /// </summary>
    class StockCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Get the symbol and prompt for the API key
            string symbol = string.IsNullOrEmpty(parameters.ArgumentsText) ? StocksInit.StocksConfig.StocksCompany : parameters.ArgumentsText;
            string apiKey = StocksInit.StocksConfig.StocksApiKey;
            while (string.IsNullOrWhiteSpace(apiKey))
            {
                apiKey = TermReader.Read(LanguageTools.GetLocalized("NKS_STOCKS_AVAPIKEYPROMPT") + ": ");
                if (string.IsNullOrWhiteSpace(apiKey))
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_STOCKS_AVAPIKEYNEEDED"), ThemeColorType.Error);
                if (apiKey == "demo")
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_STOCKS_AVAPIKEYISDEMO"), ThemeColorType.Error);
                    apiKey = "";
                }
                if (apiKey.Length != 16)
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_STOCKS_AVAPIKEYINVALIDLENGTH"), ThemeColorType.Error);
                    apiKey = "";
                }
            }

            // Now, get the stock info
            string stocksJson = NetworkTransfer.DownloadString($"https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol={symbol}&interval=60min&outputsize=full&apikey={apiKey}", false);
            var stocksToken = JToken.Parse(stocksJson);
            var stocksIntervalToken = stocksToken["Time Series (60min)"];
            if (stocksIntervalToken is null)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_STOCKS_AVAPIFAILED") + ":", ThemeColorType.Error);
                TextWriterColor.Write(stocksJson, ThemeColorType.NeutralText);
                return 40;
            }
            string? ianaTimeZone = (string?)stocksToken?["Meta Data"]?["6. Time Zone"];

            // Construct the CLI to add the token
            var cli = new StocksCli()
            {
                stocksToken = stocksIntervalToken,
                ianaTimeZone = ianaTimeZone,
            };
            InteractiveTuiTools.OpenInteractiveTui(cli);
            return 0;
        }
    }
}
