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
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Extras.Stocks.Interactives;
using Nitrocid.Languages;
using Nitrocid.Network.Transfer;
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
                apiKey = TermReader.Read(Translate.DoTranslation("Enter your AlphaVantage API key") + ": ");
                if (string.IsNullOrWhiteSpace(apiKey))
                    TextWriters.Write(Translate.DoTranslation("Please provide your API key."), KernelColorType.Error);
                if (apiKey == "demo")
                {
                    TextWriters.Write(Translate.DoTranslation("Demonstration API key can't be used."), KernelColorType.Error);
                    apiKey = "";
                }
                if (apiKey.Length != 16)
                {
                    TextWriters.Write(Translate.DoTranslation("Your API key should be 16 characters long."), KernelColorType.Error);
                    apiKey = "";
                }
            }

            // Now, get the stock info
            string stocksJson = NetworkTransfer.DownloadString($"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={symbol}&apikey={apiKey}", false);
            var stocksToken = JToken.Parse(stocksJson);
            var stocksIntervalToken = stocksToken["Time Series (Daily)"];
            if (stocksIntervalToken is null)
            {
                TextWriters.Write(Translate.DoTranslation("An error occurred while fetching the stocks data. Additional information can be found here") + ":", KernelColorType.Error);
                TextWriters.Write(stocksJson, KernelColorType.NeutralText);
                return 40;
            }
            string? ianaTimeZone = (string?)stocksToken?["Meta Data"]?["5. Time Zone"];

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
