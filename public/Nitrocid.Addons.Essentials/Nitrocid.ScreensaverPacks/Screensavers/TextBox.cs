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

using Terminaux.Inputs.Styles.Infobox;
using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Misc.Screensaver;
using Terminaux.Colors;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for TextBox
    /// </summary>
    public class TextBoxDisplay : BaseScreensaver, IScreensaver
    {

        private int currentHueAngle = 0;

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "TextBox";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Get the color and positions
            Color color = ChangeTextBoxColor();
            string renderedTextBox = ScreensaverPackInit.SaversConfig.TextBoxWrite;

            // Write the text
            if (ScreensaverPackInit.SaversConfig.TextBoxRainbowMode)
            {
                color = new($"hsl:{currentHueAngle};100;50");
                currentHueAngle++;
                if (currentHueAngle > 360)
                    currentHueAngle = 0;
            }
            var infoBox = new InfoBox()
            {
                Text = renderedTextBox,
                Settings = new InfoBoxSettings()
                {
                    ForegroundColor = color
                }
            };
            TextWriterRaw.WriteRaw(infoBox.Render());

            // Delay
            int delay = ScreensaverPackInit.SaversConfig.TextBoxRainbowMode ? 16 : ScreensaverPackInit.SaversConfig.TextBoxDelay;
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
        private Color ChangeTextBoxColor()
        {
            Color ColorInstance;
            if (ScreensaverPackInit.SaversConfig.TextBoxTrueColor)
            {
                int RedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.TextBoxMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.TextBoxMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.TextBoxMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.TextBoxMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.TextBoxMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.TextBoxMaximumBlueColorLevel);
                ColorInstance = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.TextBoxMinimumColorLevel, ScreensaverPackInit.SaversConfig.TextBoxMaximumColorLevel);
                ColorInstance = new Color(ColorNum);
            }
            return ColorInstance;
        }

    }
}
