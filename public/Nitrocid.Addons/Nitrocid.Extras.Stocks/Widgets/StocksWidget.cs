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
using Terminaux.Themes.Colors;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Network.Transfer;
using Nitrocid.Base.Misc.Widgets;
using System.Text;
using Colorimetry.Data;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Base.Extensions;

namespace Nitrocid.Extras.Stocks.Widgets
{
    /// <summary>
    /// Stocks widget
    /// </summary>
    public class StocksWidget : BaseWidget, IWidget
    {
        /// <summary>
        /// Selects how to align the notification icons
        /// </summary>
        public string ApiKey { get; set; }
        
        /// <summary>
        /// Stocks company
        /// </summary>
        public string Company { get; set; }

        /// <inheritdoc/>
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

            // User needs to provide the API key
            if (string.IsNullOrWhiteSpace(ApiKey))
            {
                displayer.Text = LanguageTools.GetLocalized("NKS_STOCKS_AVAPIKEYCONFIGURE");
                display.Append(displayer.Render());
            }
            else
            {
                // Get the stock info
                string stocksJson = NetworkTransfer.DownloadString($"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={Company}&apikey={ApiKey}", false);
                var stocksToken = JToken.Parse(stocksJson);
                var stocksIntervalToken = stocksToken["Time Series (Daily)"];
                if (stocksIntervalToken is null)
                {
                    displayer.Text = LanguageTools.GetLocalized("NKS_STOCKS_NODATA");
                    display.Append(displayer.Render());
                }
                else
                {
                    string ianaTimeZone = (string?)stocksToken?["Meta Data"]?["5. Time Zone"] ?? "";
                    string? high = (string?)stocksIntervalToken?.First?.First?["2. high"];
                    string? low = (string?)stocksIntervalToken?.First?.First?["3. low"];
                    displayer.Text =
                        $"{ConsoleColoring.RenderSetConsoleColor(ThemeColorsTools.GetColor(ThemeColorType.NeutralText))}H: {ConsoleColoring.RenderSetConsoleColor(ConsoleColors.Lime)}{high}" +
                        $"{ConsoleColoring.RenderSetConsoleColor(ThemeColorsTools.GetColor(ThemeColorType.NeutralText))} | L: {ConsoleColoring.RenderSetConsoleColor(ConsoleColors.Red)}{low}" +
                        $"{ConsoleColoring.RenderSetConsoleColor(ThemeColorsTools.GetColor(ThemeColorType.NeutralText))}";
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

        /// <summary>
        /// Makes a new stocks widget instance
        /// </summary>
        public StocksWidget()
        {
            ApiKey = StocksInit.StocksConfig.StocksApiKey;
            Company = StocksInit.StocksConfig.StocksCompany;
        }
    }
}
