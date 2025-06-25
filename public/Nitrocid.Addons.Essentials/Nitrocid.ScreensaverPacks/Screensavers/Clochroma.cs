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

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for Clochroma
    /// </summary>
    public class ClochromaDisplay : BaseScreensaver, IScreensaver
    {
        /// <inheritdoc/>
        public override string ScreensaverName =>
            "Clochroma";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Get the color and positions
            var currentDate = TimeDateTools.KernelDateTime;
            string hourCode = ScreensaverPackInit.SaversConfig.ClochromaBright ? $"{(int)(currentDate.Hour / 23d * 255):X2}" : $"{currentDate.Hour:00}";
            string minuteCode = ScreensaverPackInit.SaversConfig.ClochromaBright ? $"{(int)(currentDate.Minute / 60d * 255):X2}" : $"{currentDate.Minute:00}";
            string secondCode = ScreensaverPackInit.SaversConfig.ClochromaBright ? $"{(int)(currentDate.Second / 60d * 255):X2}" : $"{currentDate.Second:00}";
            Color timeColor = $"#{hourCode}{minuteCode}{secondCode}";
            Color bgColor = TransformationTools.GetDarkBackground(timeColor);
            string renderedDate = TimeDateRenderers.RenderDate(currentDate);
            string renderedTime = TimeDateRenderers.RenderTime(currentDate);
            int halfConsoleY = (int)(ConsoleWrapper.WindowHeight / 2d);
            int datePosX = ConsoleWrapper.WindowWidth / 2 - renderedDate.Length / 2;
            int timePosX = ConsoleWrapper.WindowWidth / 2 - renderedTime.Length / 2;

            // Write date and time
            ColorTools.LoadBackDry(bgColor);
            TextWriterWhereColor.WriteWhereColorBack(renderedDate, datePosX, halfConsoleY, timeColor, bgColor);
            TextWriterWhereColor.WriteWhereColorBack(renderedTime, timePosX, halfConsoleY + 1, timeColor, bgColor);

            // Delay
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.ClochromaDelay);
        }
    }
}
