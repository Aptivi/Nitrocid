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

using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Time;
using Nitrocid.Base.Kernel.Time.Renderers;
using System.Text;
using Terminaux.Base.Extensions;
using Colorimetry;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Textify.Data.Figlet;

namespace Nitrocid.Base.Misc.Widgets.Implementations
{
    /// <summary>
    /// Digital clock widget
    /// </summary>
    public class DigitalClock : BaseWidget, IWidget
    {
        private Color clockColor = Color.Empty;
        private Color clockInfoColor = Color.Empty;
        private Color? backgroundColor;

        /// <summary>
        /// Clock text color
        /// </summary>
        public Color ClockColor
        {
            get => clockColor;
            set => clockColor = value;
        }

        /// <summary>
        /// Clock date info text color
        /// </summary>
        public Color ClockInfoColor
        {
            get => clockInfoColor;
            set => clockInfoColor = value;
        }

        /// <summary>
        /// Seconds hand color
        /// </summary>
        public Color BackgroundColor
        {
            get => backgroundColor ?? ConsoleColoring.CurrentBackgroundColor;
            set => backgroundColor = value;
        }

        /// <summary>
        /// Use a long format when rendering time
        /// </summary>
        public bool UseLongFormat { get; set; } = Config.WidgetConfig.DigitalUseLongFormat;

        /// <inheritdoc/>
        public override string Render(int left, int top, int width, int height)
        {
            var display = new StringBuilder();
            string timeStr = TimeDateRenderers.RenderTime(UseLongFormat ? FormatType.Long : FormatType.Short);

            // Write the time using figlet
            var figFont = FigletTools.GetFigletFont(Config.MainConfig.DefaultFigletFontName);
            int figHeight = FigletTools.GetFigletHeight(timeStr, figFont) / 2;
            int consoleY = height / 2 - figHeight;
            var timeText = new AlignedFigletText(figFont)
            {
                Text = timeStr,
                ForegroundColor = ClockColor,
                BackgroundColor = BackgroundColor,
                Top = consoleY,
                Left = left,
                Width = width,
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
                    ForegroundColor = ClockInfoColor,
                    BackgroundColor = BackgroundColor,
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
        /// Changes the color of digital clock
        /// </summary>
        private Color ChangeDigitalClockColor() =>
            ColorTools.GetRandomColor(
                Config.WidgetConfig.DigitalTrueColor ? ColorType.TrueColor : ColorType.EightBitColor,
                Config.WidgetConfig.DigitalMinimumColorLevel, Config.WidgetConfig.DigitalMaximumColorLevel,
                Config.WidgetConfig.DigitalMinimumRedColorLevel, Config.WidgetConfig.DigitalMaximumRedColorLevel,
                Config.WidgetConfig.DigitalMinimumGreenColorLevel, Config.WidgetConfig.DigitalMaximumGreenColorLevel,
                Config.WidgetConfig.DigitalMinimumBlueColorLevel, Config.WidgetConfig.DigitalMaximumBlueColorLevel
            );

        /// <summary>
        /// Makes a new digital clock widget instance
        /// </summary>
        public DigitalClock()
        {
            clockColor = Config.WidgetConfig.DigitalCycleColors ? ChangeDigitalClockColor() : Config.WidgetConfig.DigitalColor;
            clockInfoColor = Config.WidgetConfig.DigitalCycleColors ? ChangeDigitalClockColor() : Config.WidgetConfig.DigitalInfoColor;
        }
    }
}
