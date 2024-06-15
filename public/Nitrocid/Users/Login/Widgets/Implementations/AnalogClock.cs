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
using System;
using System.Text;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Graphics;
using Terminaux.Graphics.Shapes;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Users.Login.Widgets.Implementations
{
    internal class AnalogClock : BaseWidget, IWidget
    {
        private string lastRendered = "";
        private (int x, int y) lastCenter = (0, 0);
        private (int x, int y) lastHours = (0, 0);
        private (int x, int y) lastMinutes = (0, 0);
        private (int x, int y) lastSeconds = (0, 0);

        public override string Render(int left, int top, int width, int height)
        {
            var builder = new StringBuilder();
            double ToRad(int degrees) =>
                degrees * (Math.PI / 180d);

            // Get the date and time
            string renderedDate = TimeDateRenderers.RenderDate();
            string renderedTime = TimeDateRenderers.RenderTime();
            string rendered = $"{renderedDate} / {renderedTime}";
            int posY = height - 2;
            int posX = width / 2 - rendered.Length / 2;

            // Clear old date/time
            int oldPosX = width / 2 - lastRendered.Length / 2;
            builder.Append(TextWriterWhereColor.RenderWhereColor(new string(' ', ConsoleChar.EstimateCellWidth(lastRendered)), oldPosX, posY, KernelColorTools.GetColor(KernelColorType.NeutralText)));

            // Clear old bezels
            builder.Append(GraphicsTools.RenderLine(lastCenter, lastHours, ColorTools.CurrentBackgroundColor));
            builder.Append(GraphicsTools.RenderLine(lastCenter, lastMinutes, ColorTools.CurrentBackgroundColor));
            if (Config.WidgetConfig.AnalogShowSecondsHand)
                builder.Append(GraphicsTools.RenderLine(lastCenter, lastSeconds, ColorTools.CurrentBackgroundColor));

            // Write date and time
            builder.Append(TextWriterWhereColor.RenderWhereColor(rendered, posX, posY, KernelColorTools.GetColor(KernelColorType.NeutralText)));
            lastRendered = rendered;

            // Now, draw the bezel
            int bezelTop = 2;
            int bezelHeight = height - bezelTop * 2 - 3;
            int bezelWidth = bezelHeight * 2;
            int bezelLeft = width / 2 - bezelHeight;
            (int x, int y) radius = (bezelLeft + bezelWidth / 2, bezelTop + bezelHeight / 2);
            int bezelRadius = radius.y - bezelTop;
            var bezel = new Circle(bezelHeight, bezelLeft, bezelTop, false, KernelColorTools.GetColor(KernelColorType.Stage));
            builder.Append(bezel.Render());
            lastCenter = radius;

            // Draw the hands (hours and minutes)
            int hoursPos = TimeDateTools.KernelDateTime.Hour % 12;
            int minutesPos = TimeDateTools.KernelDateTime.Minute;
            int hoursRadius = (int)(bezelRadius / 3d);
            int minutesRadius = (int)(bezelRadius * 2 / 3d);
            int hoursAngle = (int)(360 * (hoursPos / 12d)) - 90;
            int minutesAngle = (int)(360 * (minutesPos / 60d)) - 90;
            (int x, int y) hours = ((int)(radius.x + hoursRadius * Math.Cos(ToRad(hoursAngle))), (int)(radius.y + hoursRadius * Math.Sin(ToRad(hoursAngle))));
            (int x, int y) minutes = ((int)(radius.x + minutesRadius * Math.Cos(ToRad(minutesAngle))), (int)(radius.y + minutesRadius * Math.Sin(ToRad(minutesAngle))));
            hours.x += (hours.x - radius.x);
            minutes.x += (minutes.x - radius.x);
            builder.Append(GraphicsTools.RenderLine(radius, hours, KernelColorTools.GetColor(KernelColorType.Stage)));
            builder.Append(GraphicsTools.RenderLine(radius, minutes, KernelColorTools.GetColor(KernelColorType.Stage)));
            lastHours = hours;
            lastMinutes = minutes;

            // Draw the seconds hand (optional)
            if (Config.WidgetConfig.AnalogShowSecondsHand)
            {
                int secondsPos = TimeDateTools.KernelDateTime.Second;
                int secondsRadius = (int)(bezelRadius * 2.5 / 3d);
                int secondsAngle = (int)(360 * (secondsPos / 60d)) - 90;
                (int x, int y) seconds = ((int)(radius.x + secondsRadius * Math.Cos(ToRad(secondsAngle))), (int)(radius.y + secondsRadius * Math.Sin(ToRad(secondsAngle))));
                seconds.x += (seconds.x - radius.x);
                builder.Append(GraphicsTools.RenderLine(radius, seconds, ConsoleColorData.Red.Color));
                lastSeconds = seconds;
            }

            // Return the results
            return builder.ToString();
        }
    }
}
