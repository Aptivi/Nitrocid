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
    /// Settings for AnalogClock
    /// </summary>
    public static class AnalogClockSettings
    {

        /// <summary>
        /// [AnalogClock] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool AnalogClockTrueColor
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.AnalogClockTrueColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.AnalogClockTrueColor = value;
            }
        }
        /// <summary>
        /// [AnalogClock] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int AnalogClockDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.AnalogClockDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                ScreensaverPackInit.SaversConfig.AnalogClockDelay = value;
            }
        }
        /// <summary>
        /// [AnalogClock] Shows the seconds hand.
        /// </summary>
        public static bool AnalogClockShowSecondsHand
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.AnalogClockShowSecondsHand;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.AnalogClockShowSecondsHand = value;
            }
        }
        /// <summary>
        /// [AnalogClock] The minimum red color level (true color)
        /// </summary>
        public static int AnalogClockMinimumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.AnalogClockMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.AnalogClockMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [AnalogClock] The minimum green color level (true color)
        /// </summary>
        public static int AnalogClockMinimumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.AnalogClockMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.AnalogClockMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [AnalogClock] The minimum blue color level (true color)
        /// </summary>
        public static int AnalogClockMinimumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.AnalogClockMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.AnalogClockMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [AnalogClock] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int AnalogClockMinimumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.AnalogClockMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                ScreensaverPackInit.SaversConfig.AnalogClockMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [AnalogClock] The maximum red color level (true color)
        /// </summary>
        public static int AnalogClockMaximumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.AnalogClockMaximumRedColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.AnalogClockMinimumRedColorLevel)
                    value = ScreensaverPackInit.SaversConfig.AnalogClockMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.AnalogClockMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [AnalogClock] The maximum green color level (true color)
        /// </summary>
        public static int AnalogClockMaximumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.AnalogClockMaximumGreenColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.AnalogClockMinimumGreenColorLevel)
                    value = ScreensaverPackInit.SaversConfig.AnalogClockMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.AnalogClockMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [AnalogClock] The maximum blue color level (true color)
        /// </summary>
        public static int AnalogClockMaximumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.AnalogClockMaximumBlueColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.AnalogClockMinimumBlueColorLevel)
                    value = ScreensaverPackInit.SaversConfig.AnalogClockMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.AnalogClockMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [AnalogClock] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int AnalogClockMaximumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.AnalogClockMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= ScreensaverPackInit.SaversConfig.AnalogClockMinimumColorLevel)
                    value = ScreensaverPackInit.SaversConfig.AnalogClockMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                ScreensaverPackInit.SaversConfig.AnalogClockMaximumColorLevel = value;
            }
        }

    }

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
        public override string ScreensaverName { get; set; } = "AnalogClock";

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
            if (AnalogClockSettings.AnalogClockShowSecondsHand)
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
            if (AnalogClockSettings.AnalogClockShowSecondsHand)
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
            ThreadManager.SleepNoBlock(AnalogClockSettings.AnalogClockDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

        /// <summary>
        /// Changes the color of date and time
        /// </summary>
        public Color ChangeAnalogClockColor()
        {
            Color ColorInstance;
            if (AnalogClockSettings.AnalogClockTrueColor)
            {
                int RedColorNum = RandomDriver.Random(AnalogClockSettings.AnalogClockMinimumRedColorLevel, AnalogClockSettings.AnalogClockMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(AnalogClockSettings.AnalogClockMinimumGreenColorLevel, AnalogClockSettings.AnalogClockMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(AnalogClockSettings.AnalogClockMinimumBlueColorLevel, AnalogClockSettings.AnalogClockMaximumBlueColorLevel);
                ColorInstance = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(AnalogClockSettings.AnalogClockMinimumColorLevel, AnalogClockSettings.AnalogClockMaximumColorLevel);
                ColorInstance = new Color(ColorNum);
            }
            return ColorInstance;
        }

    }
}
