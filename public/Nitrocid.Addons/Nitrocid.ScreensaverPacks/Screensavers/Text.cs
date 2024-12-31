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
    /// Display code for Text
    /// </summary>
    public class TextDisplay : BaseScreensaver, IScreensaver
    {

        private int currentHueAngle = 0;

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "Text";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Get the color and positions
            Color color = ChangeTextColor();
            string renderedText = ScreensaverPackInit.SaversConfig.TextWrite;
            int halfConsoleY = (int)(ConsoleWrapper.WindowHeight / 2d);
            int textPosX = ConsoleWrapper.WindowWidth / 2 - renderedText.Length / 2;

            // Write the text
            if (ScreensaverPackInit.SaversConfig.TextRainbowMode)
            {
                color = new($"hsl:{currentHueAngle};100;50");
                currentHueAngle++;
                if (currentHueAngle > 360)
                    currentHueAngle = 0;
            }
            TextWriterWhereColor.WriteWhereColor(renderedText, textPosX, halfConsoleY, color);

            // Delay
            int delay = ScreensaverPackInit.SaversConfig.TextRainbowMode ? 16 : ScreensaverPackInit.SaversConfig.TextDelay;
            ScreensaverManager.Delay(delay);
        }

        /// <inheritdoc/>
        public override void ScreensaverOutro()
        {
            currentHueAngle = 0;
        }

        /// <summary>
        /// Changes the color of date and time
        /// </summary>
        private Color ChangeTextColor()
        {
            Color ColorInstance;
            if (ScreensaverPackInit.SaversConfig.TextTrueColor)
            {
                int RedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.TextMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.TextMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.TextMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.TextMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.TextMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.TextMaximumBlueColorLevel);
                ColorInstance = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.TextMinimumColorLevel, ScreensaverPackInit.SaversConfig.TextMaximumColorLevel);
                ColorInstance = new Color(ColorNum);
            }
            return ColorInstance;
        }

    }
}
