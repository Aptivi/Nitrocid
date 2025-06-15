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
using Nitrocid.Languages;
using Nitrocid.Network.Transfer;
using Nitrocid.Users.Login.Widgets;
using System.Text;
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Nitrocid.Extras.Stocks.Widgets
{
    internal class StocksWidget : BaseWidget, IWidget
    {
        private bool isReady = false;

        public override string Cleanup(int left, int top, int width, int height) =>
            "";

        public override string Initialize(int left, int top, int width, int height)
        {
            isReady = !string.IsNullOrWhiteSpace(StocksInit.StocksConfig.StocksApiKey);
            return "";
        }

        public override string Render(int left, int top, int width, int height)
        {
            var display = new StringBuilder();
            var displayer = new AlignedText()
            {
                Top = top + (height / 2),
                Left = left,
                Width = width,
                Settings = new()
                {
                    Alignment = TextAlignment.Middle
                }
            };
            if (!isReady)
            {
                displayer.Text = LanguageTools.GetLocalized("NKS_STOCKS_AVAPIKEYCONFIGURE");
                display.Append(displayer.Render());
            }
            else
            {
                // Get the stock info
                string stocksJson = NetworkTransfer.DownloadString($"https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol={StocksInit.StocksConfig.StocksCompany}&interval=60min&outputsize=full&apikey={StocksInit.StocksConfig.StocksApiKey}", false);
                var stocksToken = JToken.Parse(stocksJson);
                var stocksIntervalToken = stocksToken["Time Series (60min)"];
                if (stocksIntervalToken is null)
                {
                    displayer.Text = LanguageTools.GetLocalized("NKS_STOCKS_NODATA");
                    display.Append(displayer.Render());
                }
                else
                {
                    string ianaTimeZone = (string?)stocksToken?["Meta Data"]?["6. Time Zone"] ?? "";
                    string? high = (string?)stocksIntervalToken?.First?.First?["2. high"];
                    string? low = (string?)stocksIntervalToken?.First?.First?["3. low"];
                    displayer.Text =
                        $"{ColorTools.RenderSetConsoleColor(KernelColorTools.GetColor(KernelColorType.NeutralText))}H: {ColorTools.RenderSetConsoleColor(ConsoleColors.Lime)}{high}" +
                        $"{ColorTools.RenderSetConsoleColor(KernelColorTools.GetColor(KernelColorType.NeutralText))} | L: {ColorTools.RenderSetConsoleColor(ConsoleColors.Red)}{low}" +
                        $"{ColorTools.RenderSetConsoleColor(KernelColorTools.GetColor(KernelColorType.NeutralText))}";
                    display.Append(displayer.Render());
                    if (top + (height / 2) + 1 <= top + height)
                    {
                        displayer.Top += 1;
                        displayer.Text = ianaTimeZone;
                        display.Append(displayer.Render());
                    }
                }
            }
            return display.ToString();
        }
    }
}
