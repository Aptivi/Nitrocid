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

using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Kernel.Time.Renderers;
using Nitrocid.Base.Misc.Screensaver;
using Terminaux.Colors;
using Terminaux.Base;
using Nitrocid.Base.Kernel.Time;
using Terminaux.Colors.Transformation;
using Nitrocid.Base.Users.Login.Widgets.Implementations;
using Nitrocid.Base.Users.Login.Widgets;
using Nitrocid.Base.Drivers.RNG;
using System;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for Clochroma Analog
    /// </summary>
    public class ClochromaAnalogDisplay : BaseScreensaver, IScreensaver
    {
        private readonly AnalogClock widget = (AnalogClock)WidgetTools.GetWidget("AnalogClock");
        private DateTime lastDateTime;
        private bool needsRefresh = true;

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "ClochromaAnalog";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            widget.Initialize();
            widget.timeColor = ChangeAnalogClockColor();
            widget.bezelColor = ChangeAnalogClockColor();
            widget.handsColor = ChangeAnalogClockColor();
            widget.secondsHandColor = ChangeAnalogClockColor();
            widget.showSecondsHand = ScreensaverPackInit.SaversConfig.ClochromaAnalogShowSecondsHand;
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Get the color
            Color timeColor = TimeToColor();
            Color bgColor = TransformationTools.GetDarkBackground(timeColor);
            widget.backgroundColor = bgColor;

            // Render the analog clock
            if (needsRefresh || ConsoleResizeHandler.WasResized(false))
            {
                ColorTools.LoadBackDry(bgColor);
                needsRefresh = false;
            }
            string widgetSeq = widget.Render();
            TextWriterRaw.WriteRaw(widgetSeq);

            // Delay
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.ClochromaAnalogDelay);
        }

        private Color TimeToColor()
        {
            var currentDate = TimeDateTools.KernelDateTime;
            string hourCode = ScreensaverPackInit.SaversConfig.ClochromaAnalogBright ? $"{(int)(currentDate.Hour / 23d * 255):X2}" : $"{currentDate.Hour:00}";
            string minuteCode = ScreensaverPackInit.SaversConfig.ClochromaAnalogBright ? $"{(int)(currentDate.Minute / 60d * 255):X2}" : $"{currentDate.Minute:00}";
            string secondCode = ScreensaverPackInit.SaversConfig.ClochromaAnalogBright ? $"{(int)(currentDate.Second / 60d * 255):X2}" : $"{currentDate.Second:00}";
            Color timeColor = $"#{hourCode}{minuteCode}{secondCode}";
            if (DateTimeDifferentNoMs(currentDate))
                needsRefresh = true;
            lastDateTime = currentDate;
            return timeColor;
        }

        private bool DateTimeDifferentNoMs(DateTime dateTime) =>
            dateTime.Year != lastDateTime.Year ||
            dateTime.Month != lastDateTime.Month ||
            dateTime.Day != lastDateTime.Day ||
            dateTime.Hour != lastDateTime.Hour ||
            dateTime.Minute != lastDateTime.Minute ||
            dateTime.Second != lastDateTime.Second;

        private Color ChangeAnalogClockColor()
        {
            Color ColorInstance;
            if (ScreensaverPackInit.SaversConfig.ClochromaAnalogTrueColor)
            {
                int RedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.ClochromaAnalogMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.ClochromaAnalogMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.ClochromaAnalogMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.ClochromaAnalogMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.ClochromaAnalogMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.ClochromaAnalogMaximumBlueColorLevel);
                ColorInstance = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.ClochromaAnalogMinimumColorLevel, ScreensaverPackInit.SaversConfig.ClochromaAnalogMaximumColorLevel);
                ColorInstance = new Color(ColorNum);
            }
            return ColorInstance;
        }
    }
}
