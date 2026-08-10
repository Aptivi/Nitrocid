//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using Newtonsoft.Json.Linq;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Network.Transfer;
using Nitrocid.Extras.Stocks.Interactives;
using Terminaux.Inputs.Interactive;
using Terminaux.Reader;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Extras.Stocks.Commands
{
    /// <summary>
    /// Stocks interactive TUI (hourly stocks in full)
    /// </summary>
    class StockCommand : BaseCommand, ICommand
    {
        public override string Command =>
            "stock";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_STOCKS_COMMAND_STOCK_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(false, "company", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_STOCKS_COMMAND_STOCK_ARGUMENT_COMPANY_DESC"
                    }),
                    new CommandArgumentPart(false, "apikey", new CommandArgumentPartOptions()
                    {
                        // TODO: NKS_STOCKS_COMMAND_STOCK_ARGUMENT_APIKEY_DESC -> AlphaVantage API key
                        ArgumentDescription = /* Localizable */ "NKS_STOCKS_COMMAND_STOCK_ARGUMENT_APIKEY_DESC"
                    }),
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            // Get the symbol and prompt for the API key
            string symbol = parameters.ArgumentsList.Length >= 1 ? parameters.ArgumentsList[0] : StocksInit.StocksConfig.StocksCompany;
            string apiKey = parameters.ArgumentsList.Length >= 2 ? parameters.ArgumentsList[1] : StocksInit.StocksConfig.StocksApiKey;
            bool prompting = string.IsNullOrWhiteSpace(apiKey);
            while (prompting)
            {
                apiKey = TermReader.Read(LanguageTools.GetLocalized("NKS_STOCKS_AVAPIKEYPROMPT") + ": ");
                if (string.IsNullOrWhiteSpace(apiKey))
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_STOCKS_AVAPIKEYNEEDED"), ThemeColorType.Error);
                else if (apiKey == "demo")
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_STOCKS_AVAPIKEYISDEMO"), ThemeColorType.Error);
                else if (apiKey.Length != 16)
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_STOCKS_AVAPIKEYINVALIDLENGTH"), ThemeColorType.Error);
                else
                    prompting = false;
            }

            // Now, get the stock info
            string stocksJson = NetworkTransfer.DownloadString($"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={symbol}&apikey={apiKey}", false);
            var stocksToken = JToken.Parse(stocksJson);
            var stocksIntervalToken = stocksToken["Time Series (Daily)"];
            if (stocksIntervalToken is null)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_STOCKS_AVAPIFAILED") + ":", ThemeColorType.Error);
                TextWriterColor.Write(stocksJson, ThemeColorType.NeutralText);
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
