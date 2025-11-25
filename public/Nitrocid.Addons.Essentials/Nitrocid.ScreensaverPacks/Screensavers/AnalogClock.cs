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
using Nitrocid.Base.Misc.Screensaver;
using Nitrocid.Base.Misc.Widgets;
using Nitrocid.Base.Misc.Widgets.Implementations;
using Terminaux.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for AnalogClock
    /// </summary>
    public class AnalogClockDisplay : BaseScreensaver, IScreensaver
    {
        private readonly AnalogClock widget = (AnalogClock)WidgetTools.GetWidget("AnalogClock");

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "AnalogClock";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            base.ScreensaverPreparation();
            widget.Initialize();
            widget.timeColor = ChangeAnalogClockColor();
            widget.bezelColor = ChangeAnalogClockColor();
            widget.handsColor = ChangeAnalogClockColor();
            widget.secondsHandColor = ChangeAnalogClockColor();
            widget.showSecondsHand = ScreensaverPackInit.SaversConfig.AnalogClockShowSecondsHand;
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            string widgetSeq = widget.Render();
            TextWriterRaw.WriteRaw(widgetSeq);
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.AnalogClockDelay);
        }

        private Color ChangeAnalogClockColor()
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
