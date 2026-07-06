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

using Nitrocid.Base.Misc.Screensaver;
using Nitrocid.Base.Misc.Widgets.Implementations;
using Colorimetry;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for DigitalClock
    /// </summary>
    public class DigitalClockDisplay : BaseScreensaver, IScreensaver
    {
        private readonly DigitalClock widget = new();

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            base.ScreensaverPreparation();
            widget.ClockColor = ChangeDigitalClockColor();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            string widgetSeq = widget.Render();
            TextWriterRaw.WriteRaw(widgetSeq);
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.DigitalClockDelay);
        }

        private Color ChangeDigitalClockColor() =>
            ColorTools.GetRandomColor(
                ScreensaverPackInit.SaversConfig.DigitalClockTrueColor ? ColorType.TrueColor : ColorType.EightBitColor,
                ScreensaverPackInit.SaversConfig.DigitalClockMinimumColorLevel, ScreensaverPackInit.SaversConfig.DigitalClockMaximumColorLevel,
                ScreensaverPackInit.SaversConfig.DigitalClockMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.DigitalClockMaximumRedColorLevel,
                ScreensaverPackInit.SaversConfig.DigitalClockMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.DigitalClockMaximumGreenColorLevel,
                ScreensaverPackInit.SaversConfig.DigitalClockMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.DigitalClockMaximumBlueColorLevel
            );
    }
}
