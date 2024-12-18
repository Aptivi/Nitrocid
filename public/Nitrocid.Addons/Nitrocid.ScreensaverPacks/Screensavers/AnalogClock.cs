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

using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Threading;
using Nitrocid.Kernel.Time.Renderers;
using Nitrocid.Misc.Screensaver;
using Terminaux.Colors;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Graphics.Shapes;
using Nitrocid.Kernel.Time;
using System;
using Terminaux.Graphics;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for AnalogClock
    /// </summary>
    public class AnalogClockDisplay : BaseScreensaver, IScreensaver
    {
        private Color timeColor = Color.Empty;
        private Color bezelColor = Color.Empty;
        private Color handsColor = Color.Empty;
        private Color secondsHandColor = Color.Empty;
        private string lastRendered = "";
        private (int x, int y) lastCenter = (0, 0);
        private (int x, int y) lastHours = (0, 0);
        private (int x, int y) lastMinutes = (0, 0);
        private (int x, int y) lastSeconds = (0, 0);

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "AnalogClock";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            timeColor = ChangeAnalogClockColor();
            bezelColor = ChangeAnalogClockColor();
            handsColor = ChangeAnalogClockColor();
            secondsHandColor = ChangeAnalogClockColor();
            lastCenter = (0, 0);
            lastHours = (0, 0);
            lastMinutes = (0, 0);
            lastSeconds = (0, 0);
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            double ToRad(int degrees) =>
                degrees * (Math.PI / 180d);

            // Get the date and time
            string renderedDate = TimeDateRenderers.RenderDate();
            string renderedTime = TimeDateRenderers.RenderTime();
            string rendered = $"{renderedDate} / {renderedTime}";
            int posY = ConsoleWrapper.WindowHeight - 2;
            int posX = ConsoleWrapper.WindowWidth / 2 - rendered.Length / 2;

            // Clear old date/time
            int oldPosX = ConsoleWrapper.WindowWidth / 2 - lastRendered.Length / 2;
            TextWriterWhereColor.WriteWhereColor(new string(' ', ConsoleChar.EstimateCellWidth(lastRendered)), oldPosX, posY, timeColor);

            // Clear old bezels
            TextWriterRaw.WriteRaw(GraphicsTools.RenderLine(lastCenter, lastHours, ColorTools.CurrentBackgroundColor));
            TextWriterRaw.WriteRaw(GraphicsTools.RenderLine(lastCenter, lastMinutes, ColorTools.CurrentBackgroundColor));
            if (ScreensaverPackInit.SaversConfig.AnalogClockShowSecondsHand)
                TextWriterRaw.WriteRaw(GraphicsTools.RenderLine(lastCenter, lastSeconds, ColorTools.CurrentBackgroundColor));

            // Write date and time
            TextWriterWhereColor.WriteWhereColor(rendered, posX, posY, timeColor);
            lastRendered = rendered;

            // Now, draw the bezel
            int bezelTop = 2;
            int bezelHeight = ConsoleWrapper.WindowHeight - bezelTop * 2 - 3;
            int bezelWidth = bezelHeight * 2;
            int bezelLeft = ConsoleWrapper.WindowWidth / 2 - bezelHeight;
            (int x, int y) radius = (bezelLeft + bezelWidth / 2, bezelTop + bezelHeight / 2);
            int bezelRadius = radius.y - bezelTop;
            var bezel = new Circle(bezelHeight, bezelLeft, bezelTop, false, bezelColor);
            TextWriterRaw.WriteRaw(bezel.Render());
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
            TextWriterRaw.WriteRaw(GraphicsTools.RenderLine(radius, hours, handsColor));
            TextWriterRaw.WriteRaw(GraphicsTools.RenderLine(radius, minutes, handsColor));
            lastHours = hours;
            lastMinutes = minutes;

            // Draw the seconds hand (optional)
            if (ScreensaverPackInit.SaversConfig.AnalogClockShowSecondsHand)
            {
                int secondsPos = TimeDateTools.KernelDateTime.Second;
                int secondsRadius = (int)(bezelRadius * 2.5 / 3d);
                int secondsAngle = (int)(360 * (secondsPos / 60d)) - 90;
                (int x, int y) seconds = ((int)(radius.x + secondsRadius * Math.Cos(ToRad(secondsAngle))), (int)(radius.y + secondsRadius * Math.Sin(ToRad(secondsAngle))));
                seconds.x += (seconds.x - radius.x);
                TextWriterRaw.WriteRaw(GraphicsTools.RenderLine(radius, seconds, secondsHandColor));
                lastSeconds = seconds;
            }

            // Delay
            ThreadManager.SleepNoBlock(ScreensaverPackInit.SaversConfig.AnalogClockDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

        /// <summary>
        /// Changes the color of date and time
        /// </summary>
        public Color ChangeAnalogClockColor()
        {
            Color ColorInstance;
            if (ScreensaverPackInit.SaversConfig.AnalogClockTrueColor)
            {
                int RedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.AnalogClockMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.AnalogClockMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.AnalogClockMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.AnalogClockMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.AnalogClockMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.AnalogClockMaximumBlueColorLevel);
                ColorInstance = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.AnalogClockMinimumColorLevel, ScreensaverPackInit.SaversConfig.AnalogClockMaximumColorLevel);
                ColorInstance = new Color(ColorNum);
            }
            return ColorInstance;
        }

    }
}
