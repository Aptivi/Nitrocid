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
using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Kernel.Time.Renderers;
using Nitrocid.Base.Misc.Screensaver;
using Terminaux.Colors;
using Terminaux.Base;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for DateAndTime
    /// </summary>
    public class DateAndTimeDisplay : BaseScreensaver, IScreensaver
    {

        private string lastRenderedDate = "";
        private string lastRenderedTime = "";

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "DateAndTime";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Get the color and positions
            Color timeColor = ChangeDateAndTimeColor();
            string renderedDate = TimeDateRenderers.RenderDate();
            string renderedTime = TimeDateRenderers.RenderTime();
            int halfConsoleY = (int)(ConsoleWrapper.WindowHeight / 2d);
            int datePosX = ConsoleWrapper.WindowWidth / 2 - renderedDate.Length / 2;
            int timePosX = ConsoleWrapper.WindowWidth / 2 - renderedTime.Length / 2;

            // Clear old date/time
            int oldDatePosX = ConsoleWrapper.WindowWidth / 2 - lastRenderedDate.Length / 2;
            int oldTimePosX = ConsoleWrapper.WindowWidth / 2 - lastRenderedTime.Length / 2;
            TextWriterWhereColor.WriteWhereColor(new string(' ', lastRenderedDate.Length), oldDatePosX, halfConsoleY, timeColor);
            TextWriterWhereColor.WriteWhereColor(new string(' ', lastRenderedTime.Length), oldTimePosX, halfConsoleY + 1, timeColor);

            // Write date and time
            TextWriterWhereColor.WriteWhereColor(renderedDate, datePosX, halfConsoleY, timeColor);
            TextWriterWhereColor.WriteWhereColor(renderedTime, timePosX, halfConsoleY + 1, timeColor);

            // Delay
            lastRenderedDate = renderedDate;
            lastRenderedTime = renderedTime;
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.DateAndTimeDelay);
        }

        /// <summary>
        /// Changes the color of date and time
        /// </summary>
        public Color ChangeDateAndTimeColor()
        {
            Color ColorInstance;
            if (ScreensaverPackInit.SaversConfig.DateAndTimeTrueColor)
            {
                int RedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.DateAndTimeMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.DateAndTimeMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.DateAndTimeMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.DateAndTimeMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.DateAndTimeMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.DateAndTimeMaximumBlueColorLevel);
                ColorInstance = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.DateAndTimeMinimumColorLevel, ScreensaverPackInit.SaversConfig.DateAndTimeMaximumColorLevel);
                ColorInstance = new Color(ColorNum);
            }
            return ColorInstance;
        }

    }
}
