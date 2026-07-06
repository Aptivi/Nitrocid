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

using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Misc.Screensaver;
using Colorimetry;
using Terminaux.Base;
using Nitrocid.Base.Kernel.Time;
using Colorimetry.Transformation;
using Nitrocid.Base.Misc.Widgets.Implementations;
using System;
using Terminaux.Base.Extensions;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for Clochroma Analog
    /// </summary>
    public class ClochromaAnalogDisplay : BaseScreensaver, IScreensaver
    {
        private readonly AnalogClock widget = new();
        private DateTime lastDateTime;
        private bool needsRefresh = true;

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            base.ScreensaverPreparation();
            widget.TimeColor = ChangeAnalogClockColor();
            widget.BezelColor = ChangeAnalogClockColor();
            widget.HandsColor = ChangeAnalogClockColor();
            widget.SecondsHandColor = ChangeAnalogClockColor();
            widget.ShowSecondsHand = ScreensaverPackInit.SaversConfig.ClochromaAnalogShowSecondsHand;
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Get the color
            Color timeColor = TimeToColor();
            Color bgColor = TransformationTools.GetDarkBackground(timeColor);
            widget.BackgroundColor = bgColor;

            // Render the analog clock
            if (needsRefresh || ConsoleResizeHandler.WasResized(false))
            {
                ConsoleColoring.LoadBackDry(bgColor);
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

        private Color ChangeAnalogClockColor() =>
            ColorTools.GetRandomColor(
                ScreensaverPackInit.SaversConfig.ClochromaAnalogTrueColor ? ColorType.TrueColor : ColorType.EightBitColor,
                ScreensaverPackInit.SaversConfig.ClochromaAnalogMinimumColorLevel, ScreensaverPackInit.SaversConfig.ClochromaAnalogMaximumColorLevel,
                ScreensaverPackInit.SaversConfig.ClochromaAnalogMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.ClochromaAnalogMaximumRedColorLevel,
                ScreensaverPackInit.SaversConfig.ClochromaAnalogMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.ClochromaAnalogMaximumGreenColorLevel,
                ScreensaverPackInit.SaversConfig.ClochromaAnalogMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.ClochromaAnalogMaximumBlueColorLevel
            );
    }
}
