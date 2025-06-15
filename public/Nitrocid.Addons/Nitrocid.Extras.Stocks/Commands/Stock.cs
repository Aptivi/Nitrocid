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

using Newtonsoft.Json.Linq;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Extras.Stocks.Interactives;
using Nitrocid.Languages;
using Nitrocid.Network.Transfer;
using Nitrocid.Shell.ShellBase.Commands;
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
                    TextWriters.Write(LanguageTools.GetLocalized("NKS_STOCKS_AVAPIKEYNEEDED"), KernelColorType.Error);
                if (apiKey == "demo")
                {
                    TextWriters.Write(LanguageTools.GetLocalized("NKS_STOCKS_AVAPIKEYISDEMO"), KernelColorType.Error);
                    apiKey = "";
                }
                if (apiKey.Length != 16)
                {
                    TextWriters.Write(LanguageTools.GetLocalized("NKS_STOCKS_AVAPIKEYINVALIDLENGTH"), KernelColorType.Error);
                    apiKey = "";
                }
            }

            // Now, get the stock info
            string stocksJson = NetworkTransfer.DownloadString($"https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol={symbol}&interval=60min&outputsize=full&apikey={apiKey}", false);
            var stocksToken = JToken.Parse(stocksJson);
            var stocksIntervalToken = stocksToken["Time Series (60min)"];
            if (stocksIntervalToken is null)
            {
                TextWriters.Write(LanguageTools.GetLocalized("NKS_STOCKS_AVAPIFAILED") + ":", KernelColorType.Error);
                TextWriters.Write(stocksJson, KernelColorType.NeutralText);
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
