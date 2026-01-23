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

using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Time;
using Nitrocid.Kernel.Time.Renderers;
using System;
using System.Text;
using Terminaux.Base.Extensions;
using Terminaux.Base.Structures;
using Terminaux.Colors;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Graphical.Shapes;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Nitrocid.Users.Login.Widgets.Implementations
{
    internal class AnalogClock : BaseWidget, IWidget
    {
        internal Color timeColor = Color.Empty;
        internal Color bezelColor = Color.Empty;
        internal Color handsColor = Color.Empty;
        internal Color secondsHandColor = Color.Empty;
        internal bool showSecondsHand = false;
        private bool clear = false;
        private string lastRendered = "";
        private Coordinate lastCenter = new(0, 0);
        private Coordinate lastHours = new(0, 0);
        private Coordinate lastMinutes = new(0, 0);
        private Coordinate lastSeconds = new(0, 0);

        public override string Cleanup(int left, int top, int width, int height) =>
            "";

        public override string Initialize(int left, int top, int width, int height)
        {
            timeColor = ChangeAnalogClockColor();
            bezelColor = ChangeAnalogClockColor();
            handsColor = ChangeAnalogClockColor();
            secondsHandColor = ChangeAnalogClockColor();
            showSecondsHand = Config.WidgetConfig.AnalogShowSecondsHand;
            lastCenter = new(0, 0);
            lastHours = new(0, 0);
            lastMinutes = new(0, 0);
            lastSeconds = new(0, 0);
            clear = false;
            return "";
        }

        public override string Render(int left, int top, int width, int height)
        {
            var builder = new StringBuilder();
            double ToRad(int degrees) =>
                degrees * (Math.PI / 180d);

            // Get the date and time
            string renderedDate = TimeDateRenderers.RenderDate();
            string renderedTime = TimeDateRenderers.RenderTime();
            string rendered = $"{renderedDate} / {renderedTime}";
            int posY = top + height - 2;

            // Clear old values
            if (clear)
            {
                // Clear old date/time
                int oldPosX = (left + width) / 2 - lastRendered.Length / 2;
                var timeDateClear = new AlignedText()
                {
                    Text = new string(' ', ConsoleChar.EstimateCellWidth(lastRendered)),
                    Top = posY,
                    Left = left,
                    Width = width,
                    ForegroundColor = timeColor
                };
                builder.Append(timeDateClear.Render());

                // Clear old bezels
                builder.Append(GetLineFrom(lastCenter, lastHours).Render());
                builder.Append(GetLineFrom(lastCenter, lastMinutes).Render());
                if (Config.WidgetConfig.AnalogShowSecondsHand)
                    builder.Append(GetLineFrom(lastCenter, lastSeconds).Render());
            }
            clear = true;

            // Write date and time
            var timeDate = new AlignedText()
            {
                Text = rendered,
                Top = posY,
                Left = left,
                Width = width,
                ForegroundColor = timeColor,
                Settings = new()
                {
                    Alignment = TextAlignment.Middle
                },
            };
            builder.Append(timeDate.Render());
            lastRendered = rendered;

            // Now, draw the bezel
            int bezelTop = top + 1;
            int bezelHeight = height - 4 - (height % 2 == 0 ? 1 : 0);
            int bezelWidth = bezelHeight * 2;
            int bezelLeft = width / 2 - bezelHeight + left + 1;
            (int x, int y) radius = (bezelLeft + bezelWidth / 2, bezelTop + bezelHeight / 2);
            int bezelRadius = radius.y - bezelTop;
            var bezel = new Circle(bezelHeight, bezelLeft, bezelTop, false, bezelColor);
            builder.Append(bezel.Render());
            lastCenter = new(radius.x, radius.y);

            // Draw the hands (hours and minutes)
            int hoursPos = TimeDateTools.KernelDateTime.Hour % 12;
            int minutesPos = TimeDateTools.KernelDateTime.Minute;
            int hoursRadius = (int)(bezelRadius / 3d);
            int minutesRadius = (int)(bezelRadius * 2 / 3d);
            int hoursAngle = (int)(360 * (hoursPos / 12d)) - 90;
            int minutesAngle = (int)(360 * (minutesPos / 60d)) - 90;
            (int x, int y) hours = ((int)(radius.x + hoursRadius * Math.Cos(ToRad(hoursAngle))), (int)(radius.y + hoursRadius * Math.Sin(ToRad(hoursAngle))));
            (int x, int y) minutes = ((int)(radius.x + minutesRadius * Math.Cos(ToRad(minutesAngle))), (int)(radius.y + minutesRadius * Math.Sin(ToRad(minutesAngle))));
            hours.x += hours.x - radius.x;
            minutes.x += minutes.x - radius.x;
            builder.Append(GetLineFrom(radius, hours, handsColor).Render());
            builder.Append(GetLineFrom(radius, minutes, handsColor).Render());
            lastHours = new(hours.x, hours.y);
            lastMinutes = new(minutes.x, minutes.y);

            // Draw the seconds hand (optional)
            if (Config.WidgetConfig.AnalogShowSecondsHand)
            {
                int secondsPos = TimeDateTools.KernelDateTime.Second;
                int secondsRadius = (int)(bezelRadius * 2.5 / 3d);
                int secondsAngle = (int)(360 * (secondsPos / 60d)) - 90;
                (int x, int y) seconds = ((int)(radius.x + secondsRadius * Math.Cos(ToRad(secondsAngle))), (int)(radius.y + secondsRadius * Math.Sin(ToRad(secondsAngle))));
                seconds.x += seconds.x - radius.x;
                builder.Append(GetLineFrom(radius, seconds, secondsHandColor).Render());
                lastSeconds = new(seconds.x, seconds.y);
            }

            // Return the results
            return builder.ToString();
        }

        /// <summary>
        /// Changes the color of date and time
        /// </summary>
        private Color ChangeAnalogClockColor()
        {
            Color ColorInstance;
            if (Config.WidgetConfig.AnalogTrueColor)
            {
                int RedColorNum = RandomDriver.Random(Config.WidgetConfig.AnalogMinimumRedColorLevel, Config.WidgetConfig.AnalogMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(Config.WidgetConfig.AnalogMinimumGreenColorLevel, Config.WidgetConfig.AnalogMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(Config.WidgetConfig.AnalogMinimumBlueColorLevel, Config.WidgetConfig.AnalogMaximumBlueColorLevel);
                ColorInstance = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(Config.WidgetConfig.AnalogMinimumColorLevel, Config.WidgetConfig.AnalogMaximumColorLevel);
                ColorInstance = new Color(ColorNum);
            }
            return ColorInstance;
        }

        private Line GetLineFrom(Coordinate startPos, Coordinate endPos) =>
            GetLineFrom((startPos.X, startPos.Y), (endPos.X, endPos.Y), ConsoleColoring.CurrentBackgroundColor);

        private Line GetLineFrom((int x, int y) startPos, (int x, int y) endPos, Color color)
        {
            return new()
            {
                StartPos = new(startPos.x, startPos.y),
                EndPos = new(endPos.x, endPos.y),
                DoubleWidth = false,
                Color = color
            };
        }
    }
}
