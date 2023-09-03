
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

using KS.ConsoleBase;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Drivers.RNG;
using KS.Kernel.Configuration;
using KS.Kernel.Threading;
using Terminaux.Colors;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for Text
    /// </summary>
    public static class TextSettings
    {

        /// <summary>
        /// [Text] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool TextTrueColor
        {
            get
            {
                return Config.SaverConfig.TextTrueColor;
            }
            set
            {
                Config.SaverConfig.TextTrueColor = value;
            }
        }
        /// <summary>
        /// [Text] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int TextDelay
        {
            get
            {
                return Config.SaverConfig.TextDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                Config.SaverConfig.TextDelay = value;
            }
        }
        /// <summary>
        /// [Text] Text for Bouncing Text. Shorter is better.
        /// </summary>
        public static string TextWrite
        {
            get
            {
                return Config.SaverConfig.TextWrite;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Nitrocid KS";
                Config.SaverConfig.TextWrite = value;
            }
        }
        /// <summary>
        /// [Text] The minimum red color level (true color)
        /// </summary>
        public static int TextMinimumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.TextMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.TextMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Text] The minimum green color level (true color)
        /// </summary>
        public static int TextMinimumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.TextMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.TextMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Text] The minimum blue color level (true color)
        /// </summary>
        public static int TextMinimumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.TextMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.TextMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Text] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int TextMinimumColorLevel
        {
            get
            {
                return Config.SaverConfig.TextMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                Config.SaverConfig.TextMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Text] The maximum red color level (true color)
        /// </summary>
        public static int TextMaximumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.TextMaximumRedColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.TextMinimumRedColorLevel)
                    value = Config.SaverConfig.TextMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.TextMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Text] The maximum green color level (true color)
        /// </summary>
        public static int TextMaximumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.TextMaximumGreenColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.TextMinimumGreenColorLevel)
                    value = Config.SaverConfig.TextMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.TextMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Text] The maximum blue color level (true color)
        /// </summary>
        public static int TextMaximumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.TextMaximumBlueColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.TextMinimumBlueColorLevel)
                    value = Config.SaverConfig.TextMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.TextMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Text] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int TextMaximumColorLevel
        {
            get
            {
                return Config.SaverConfig.TextMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= Config.SaverConfig.TextMinimumColorLevel)
                    value = Config.SaverConfig.TextMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                Config.SaverConfig.TextMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for Text
    /// </summary>
    public class TextDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Text";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Get the color and positions
            Color color = ChangeTextColor();
            string renderedText = TextSettings.TextWrite;
            int halfConsoleY = (int)(ConsoleWrapper.WindowHeight / 2d);
            int textPosX = (ConsoleWrapper.WindowWidth / 2) - (renderedText.Length / 2);

            // Write the text
            TextWriterWhereColor.WriteWhere(renderedText, textPosX, halfConsoleY, color);

            // Delay
            ThreadManager.SleepNoBlock(TextSettings.TextDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

        /// <summary>
        /// Changes the color of date and time
        /// </summary>
        public Color ChangeTextColor()
        {
            Color ColorInstance;
            if (TextSettings.TextTrueColor)
            {
                int RedColorNum = RandomDriver.Random(TextSettings.TextMinimumRedColorLevel, TextSettings.TextMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(TextSettings.TextMinimumGreenColorLevel, TextSettings.TextMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(TextSettings.TextMinimumBlueColorLevel, TextSettings.TextMaximumBlueColorLevel);
                ColorInstance = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(TextSettings.TextMinimumColorLevel, TextSettings.TextMaximumColorLevel);
                ColorInstance = new Color(ColorNum);
            }
            return ColorInstance;
        }

    }
}
