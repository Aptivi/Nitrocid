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

using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Time;
using Nitrocid.Base.Kernel.Time.Renderers;
using System.Text;
using Terminaux.Colors;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Textify.Data.Figlet;

namespace Nitrocid.Base.Users.Login.Widgets.Implementations
{
    internal class DigitalClock : BaseWidget, IWidget
    {
        internal Color clockColor = Color.Empty;
        internal Color backgroundColor = Color.Empty;

        public override string Cleanup(int left, int top, int width, int height) =>
            "";

        public override string Initialize(int left, int top, int width, int height)
        {
            clockColor = ChangeDateAndTimeColor();
            backgroundColor = ColorTools.CurrentBackgroundColor;
            return "";
        }

        public override string Render(int left, int top, int width, int height)
        {
            var display = new StringBuilder();
            string timeStr = TimeDateRenderers.RenderTime(FormatType.Short);

            // Write the time using figlet
            var figFont = FigletTools.GetFigletFont(Config.MainConfig.DefaultFigletFontName);
            int figHeight = FigletTools.GetFigletHeight(timeStr, figFont) / 2;
            int consoleY = height / 2 - figHeight;
            var timeText = new AlignedFigletText(figFont)
            {
                Text = timeStr,
                ForegroundColor = clockColor,
                BackgroundColor = backgroundColor,
                Top = consoleY,
                Settings = new()
                {
                    Alignment = TextAlignment.Middle,
                }
            };
            display.Append(timeText.Render());

            // Print the date
            if (Config.WidgetConfig.DigitalDisplayDate)
            {
                string dateStr = $"{TimeDateRenderers.RenderDate()}";
                int consoleInfoY = height / 2 + figHeight + 2;
                var dateText = new AlignedText()
                {
                    Text = dateStr,
                    ForegroundColor = clockColor,
                    BackgroundColor = backgroundColor,
                    Top = consoleInfoY,
                    OneLine = true,
                    Left = left,
                    Width = width,
                    Settings = new()
                    {
                        Alignment = TextAlignment.Middle,
                    }
                };
                display.Append(dateText.Render());
            }

            // Print everything
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
