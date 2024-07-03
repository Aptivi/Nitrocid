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

using Newtonsoft.Json.Linq;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Time;
using Nitrocid.Kernel.Time.Renderers;
using Nitrocid.Languages;
using Nitrocid.Network.Transfer;
using Nitrocid.Users.Login.Widgets;
using System.Text;
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Writer.FancyWriters;
using Textify.Figlet;

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
            if (!isReady)
                display.Append(CenteredTextColor.RenderCentered(top + (height / 2), Translate.DoTranslation("API Key is required. Configure from the settings."), left, ConsoleWrapper.WindowWidth - (left + width)));
            else
            {
                // Get the stock info
                string stocksJson = NetworkTransfer.DownloadString($"https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol={StocksInit.StocksConfig.StocksCompany}&interval=60min&outputsize=full&apikey={StocksInit.StocksConfig.StocksApiKey}", false);
                var stocksToken = JToken.Parse(stocksJson);
                var stocksIntervalToken = stocksToken["Time Series (60min)"];
                if (stocksIntervalToken is null)
                    display.Append(CenteredTextColor.RenderCentered(top + (height / 2), Translate.DoTranslation("No stock data available."), left, ConsoleWrapper.WindowWidth - (left + width)));
                else
                {
                    string ianaTimeZone = (string)stocksToken["Meta Data"]["6. Time Zone"];
                    string high = (string)stocksIntervalToken.First.First["2. high"];
                    string low = (string)stocksIntervalToken.First.First["3. low"];
                    display.Append(CenteredTextColor.RenderCentered(top + (height / 2),
                        $"{ColorTools.RenderSetConsoleColor(KernelColorTools.GetColor(KernelColorType.NeutralText))}H: {ColorTools.RenderSetConsoleColor(ConsoleColors.Lime)}{high}" +
                        $"{ColorTools.RenderSetConsoleColor(KernelColorTools.GetColor(KernelColorType.NeutralText))} | L: {ColorTools.RenderSetConsoleColor(ConsoleColors.Red)}{low}" +
                        $"{ColorTools.RenderSetConsoleColor(KernelColorTools.GetColor(KernelColorType.NeutralText))}"
                    , left, ConsoleWrapper.WindowWidth - (left + width)));
                    if (top + (height / 2) + 1 <= top + height)
                        display.Append(CenteredTextColor.RenderCentered(top + (height / 2) + 1, ianaTimeZone, left, ConsoleWrapper.WindowWidth - (left + width)));
                }
            }
            return display.ToString();
        }

        /// <summary>
        /// Changes the color of date and time
        /// </summary>
        private Color ChangeDateAndTimeColor()
        {
            Color ColorInstance;
            if (Config.WidgetConfig.DigitalTrueColor)
            {
                int RedColorNum = RandomDriver.Random(Config.WidgetConfig.DigitalMinimumRedColorLevel, Config.WidgetConfig.DigitalMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(Config.WidgetConfig.DigitalMinimumGreenColorLevel, Config.WidgetConfig.DigitalMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(Config.WidgetConfig.DigitalMinimumBlueColorLevel, Config.WidgetConfig.DigitalMaximumBlueColorLevel);
                ColorInstance = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(Config.WidgetConfig.DigitalMinimumColorLevel, Config.WidgetConfig.DigitalMaximumColorLevel);
                ColorInstance = new Color(ColorNum);
            }
            return ColorInstance;
        }
    }
}
