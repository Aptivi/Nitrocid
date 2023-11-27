//
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
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

using KS.ConsoleBase.Inputs.Styles.Infobox;
using KS.Drivers.RNG;
using KS.Kernel.Threading;
using KS.Misc.Screensaver;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Settings for TextBox
    /// </summary>
    public static class TextBox
    {

        /// <summary>
        /// [TextBox] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool TextBoxTrueColor
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.TextBoxTrueColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.TextBoxTrueColor = value;
            }
        }
        /// <summary>
        /// [TextBox] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int TextBoxDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.TextBoxDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                ScreensaverPackInit.SaversConfig.TextBoxDelay = value;
            }
        }
        /// <summary>
        /// [TextBox] TextBox for Bouncing TextBox. Shorter is better.
        /// </summary>
        public static string TextBoxWrite
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.TextBoxWrite;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Nitrocid KS";
                ScreensaverPackInit.SaversConfig.TextBoxWrite = value;
            }
        }
        /// <summary>
        /// [TextBox] Enables the rainbow colors mode
        /// </summary>
        public static bool TextBoxRainbowMode
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.TextBoxRainbowMode;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.TextBoxRainbowMode = value;
            }
        }
        /// <summary>
        /// [TextBox] The minimum red color level (true color)
        /// </summary>
        public static int TextBoxMinimumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.TextBoxMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.TextBoxMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [TextBox] The minimum green color level (true color)
        /// </summary>
        public static int TextBoxMinimumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.TextBoxMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.TextBoxMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [TextBox] The minimum blue color level (true color)
        /// </summary>
        public static int TextBoxMinimumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.TextBoxMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.TextBoxMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [TextBox] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int TextBoxMinimumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.TextBoxMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                ScreensaverPackInit.SaversConfig.TextBoxMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [TextBox] The maximum red color level (true color)
        /// </summary>
        public static int TextBoxMaximumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.TextBoxMaximumRedColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.TextBoxMinimumRedColorLevel)
                    value = ScreensaverPackInit.SaversConfig.TextBoxMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.TextBoxMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [TextBox] The maximum green color level (true color)
        /// </summary>
        public static int TextBoxMaximumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.TextBoxMaximumGreenColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.TextBoxMinimumGreenColorLevel)
                    value = ScreensaverPackInit.SaversConfig.TextBoxMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.TextBoxMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [TextBox] The maximum blue color level (true color)
        /// </summary>
        public static int TextBoxMaximumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.TextBoxMaximumBlueColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.TextBoxMinimumBlueColorLevel)
                    value = ScreensaverPackInit.SaversConfig.TextBoxMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.TextBoxMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [TextBox] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int TextBoxMaximumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.TextBoxMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= ScreensaverPackInit.SaversConfig.TextBoxMinimumColorLevel)
                    value = ScreensaverPackInit.SaversConfig.TextBoxMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                ScreensaverPackInit.SaversConfig.TextBoxMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for TextBox
    /// </summary>
    public class TextBoxDisplay : BaseScreensaver, IScreensaver
    {

        private int currentHueAngle = 0;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "TextBox";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Get the color and positions
            Color color = ChangeTextBoxColor();
            string renderedTextBox = TextBox.TextBoxWrite;

            // Write the text
            if (TextBox.TextBoxRainbowMode)
            {
                color = new($"hsl:{currentHueAngle};100;50");
                currentHueAngle++;
                if (currentHueAngle > 360)
                    currentHueAngle = 0;
            }
            InfoBoxColor.WriteInfoBoxColor(renderedTextBox, false, color);

            // Delay
            int delay = TextBox.TextBoxRainbowMode ? 16 : TextBox.TextBoxDelay;
            ThreadManager.SleepNoBlock(delay, ScreensaverDisplayer.ScreensaverDisplayerThread);
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
            if (TextBox.TextBoxTrueColor)
            {
                int RedColorNum = RandomDriver.Random(TextBox.TextBoxMinimumRedColorLevel, TextBox.TextBoxMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(TextBox.TextBoxMinimumGreenColorLevel, TextBox.TextBoxMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(TextBox.TextBoxMinimumBlueColorLevel, TextBox.TextBoxMaximumBlueColorLevel);
                ColorInstance = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(TextBox.TextBoxMinimumColorLevel, TextBox.TextBoxMaximumColorLevel);
                ColorInstance = new Color(ColorNum);
            }
            return ColorInstance;
        }

    }
}
