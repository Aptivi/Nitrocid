
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
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.ConsoleBase.Writers.FancyWriters;
using KS.Drivers.RNG;
using KS.Kernel.Configuration;
using KS.Kernel.Threading;
using Terminaux.Colors;

namespace KS.Misc.Screensaver.Displays
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
                return Config.SaverConfig.TextBoxTrueColor;
            }
            set
            {
                Config.SaverConfig.TextBoxTrueColor = value;
            }
        }
        /// <summary>
        /// [TextBox] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int TextBoxDelay
        {
            get
            {
                return Config.SaverConfig.TextBoxDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                Config.SaverConfig.TextBoxDelay = value;
            }
        }
        /// <summary>
        /// [TextBox] TextBox for Bouncing TextBox. Shorter is better.
        /// </summary>
        public static string TextBoxWrite
        {
            get
            {
                return Config.SaverConfig.TextBoxWrite;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Nitrocid KS";
                Config.SaverConfig.TextBoxWrite = value;
            }
        }
        /// <summary>
        /// [TextBox] The minimum red color level (true color)
        /// </summary>
        public static int TextBoxMinimumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.TextBoxMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.TextBoxMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [TextBox] The minimum green color level (true color)
        /// </summary>
        public static int TextBoxMinimumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.TextBoxMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.TextBoxMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [TextBox] The minimum blue color level (true color)
        /// </summary>
        public static int TextBoxMinimumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.TextBoxMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.TextBoxMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [TextBox] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int TextBoxMinimumColorLevel
        {
            get
            {
                return Config.SaverConfig.TextBoxMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                Config.SaverConfig.TextBoxMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [TextBox] The maximum red color level (true color)
        /// </summary>
        public static int TextBoxMaximumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.TextBoxMaximumRedColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.TextBoxMinimumRedColorLevel)
                    value = Config.SaverConfig.TextBoxMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.TextBoxMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [TextBox] The maximum green color level (true color)
        /// </summary>
        public static int TextBoxMaximumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.TextBoxMaximumGreenColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.TextBoxMinimumGreenColorLevel)
                    value = Config.SaverConfig.TextBoxMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.TextBoxMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [TextBox] The maximum blue color level (true color)
        /// </summary>
        public static int TextBoxMaximumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.TextBoxMaximumBlueColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.TextBoxMinimumBlueColorLevel)
                    value = Config.SaverConfig.TextBoxMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.TextBoxMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [TextBox] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int TextBoxMaximumColorLevel
        {
            get
            {
                return Config.SaverConfig.TextBoxMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= Config.SaverConfig.TextBoxMinimumColorLevel)
                    value = Config.SaverConfig.TextBoxMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                Config.SaverConfig.TextBoxMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for TextBox
    /// </summary>
    public class TextBoxDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "TextBox";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Get the color and positions
            Color color = ChangeTextBoxColor();
            string renderedTextBox = TextBox.TextBoxWrite;

            // Write the text
            InfoBoxColor.WriteInfoBox(renderedTextBox, false, color);

            // Delay
            ThreadManager.SleepNoBlock(TextBox.TextBoxDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

        /// <summary>
        /// Changes the color of date and time
        /// </summary>
        public Color ChangeTextBoxColor()
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
