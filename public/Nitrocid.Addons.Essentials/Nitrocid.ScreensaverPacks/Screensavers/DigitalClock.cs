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
using Nitrocid.Base.Users.Login.Widgets;
using Nitrocid.Base.Users.Login.Widgets.Implementations;
using Terminaux.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for DigitalClock
    /// </summary>
    public class DigitalClockDisplay : BaseScreensaver, IScreensaver
    {
        private readonly DigitalClock widget = (DigitalClock)WidgetTools.GetWidget("DigitalClock");

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "DigitalClock";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            widget.Initialize();
            widget.clockColor = ChangeDigitalClockColor();
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            string widgetSeq = widget.Render();
            TextWriterRaw.WriteRaw(widgetSeq);
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.DigitalClockDelay);
        }

        private Color ChangeDigitalClockColor()
        {
            Color ColorInstance;
            if (ScreensaverPackInit.SaversConfig.DigitalClockTrueColor)
            {
                int RedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.DigitalClockMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.DigitalClockMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.DigitalClockMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.DigitalClockMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.DigitalClockMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.DigitalClockMaximumBlueColorLevel);
                ColorInstance = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.DigitalClockMinimumColorLevel, ScreensaverPackInit.SaversConfig.DigitalClockMaximumColorLevel);
                ColorInstance = new Color(ColorNum);
            }
            return ColorInstance;
        }
    }
}
