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
using Nitrocid.Drivers.RNG;
using Nitrocid.Misc.Screensaver;
using Terminaux.Colors;
using Terminaux.Base;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for TextWander
    /// </summary>
    public class TextWanderDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "TextWander";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Get the color and positions
            Color color = ChangeTextWanderColor();
            string renderedTextWander = ScreensaverPackInit.SaversConfig.TextWanderWrite;
            int furthestX = ConsoleWrapper.WindowWidth - renderedTextWander.Length;
            int randomPosX = RandomDriver.RandomIdx(furthestX);
            int randomPosY = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);

            // Write the text
            TextWriterWhereColor.WriteWhereColor(renderedTextWander, randomPosX, randomPosY, color);

            // Delay
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.TextWanderDelay);
            TextWriterWhereColor.WriteWhereColor(new string(' ', renderedTextWander.Length), randomPosX, randomPosY, color);
        }

        /// <summary>
        /// Changes the color of date and time
        /// </summary>
        public Color ChangeTextWanderColor()
        {
            Color ColorInstance;
            if (ScreensaverPackInit.SaversConfig.TextWanderTrueColor)
            {
                int RedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.TextWanderMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.TextWanderMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.TextWanderMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.TextWanderMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.TextWanderMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.TextWanderMaximumBlueColorLevel);
                ColorInstance = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.TextWanderMinimumColorLevel, ScreensaverPackInit.SaversConfig.TextWanderMaximumColorLevel);
                ColorInstance = new Color(ColorNum);
            }
            return ColorInstance;
        }

    }
}
