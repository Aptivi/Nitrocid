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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Time;
using Nitrocid.Kernel.Time.Renderers;
using System.Text;
using Terminaux.Base;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Writer.FancyWriters;
using Textify.Figlet;

namespace Nitrocid.Users.Login.Widgets.Implementations
{
    internal class DigitalClock : BaseWidget, IWidget
    {
        public override string Render(int left, int top, int width, int height)
        {
            var display = new StringBuilder();
            string timeStr = TimeDateRenderers.RenderTime(FormatType.Short);

            // Clear the console and write the time using figlet
            display.Append(
                CsiSequences.GenerateCsiCursorPosition(1, 1) +
                CsiSequences.GenerateCsiEraseInDisplay(0)
            );
            var figFont = FigletTools.GetFigletFont(Config.MainConfig.DefaultFigletFontName);
            int figHeight = FigletTools.GetFigletHeight(timeStr, figFont) / 2;
            int consoleY = height / 2 - figHeight;
            display.Append(
                KernelColorTools.GetColor(KernelColorType.Stage).VTSequenceForeground +
                CenteredFigletTextColor.RenderCenteredFiglet(consoleY, figFont, timeStr) +
                KernelColorTools.GetColor(KernelColorType.NeutralText).VTSequenceForeground
            );

            // Print the date
            if (Config.WidgetConfig.DigitalDisplayDate)
            {
                string dateStr = $"{TimeDateRenderers.RenderDate()}";
                int consoleInfoY = (height / 2) + figHeight + 2;
                display.Append(
                    KernelColorTools.GetColor(KernelColorType.Stage).VTSequenceForeground +
                    CenteredTextColor.RenderCenteredOneLine(consoleInfoY, dateStr) +
                    KernelColorTools.GetColor(KernelColorType.NeutralText).VTSequenceForeground
                );
            }

            // Print everything
            return display.ToString();
        }
    }
}
