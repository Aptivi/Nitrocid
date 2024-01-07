//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using Nitrocid.ConsoleBase;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Threading;
using Nitrocid.Misc.Screensaver;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
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
                return ScreensaverPackInit.SaversConfig.TextTrueColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.TextTrueColor = value;
            }
        }
        /// <summary>
        /// [Text] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int TextDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.TextDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                ScreensaverPackInit.SaversConfig.TextDelay = value;
            }
        }
        /// <summary>
        /// [Text] Text for Bouncing Text. Shorter is better.
        /// </summary>
        public static string TextWrite
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.TextWrite;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Nitrocid KS";
                ScreensaverPackInit.SaversConfig.TextWrite = value;
            }
        }
        /// <summary>
        /// [Text] Enables the rainbow colors mode
        /// </summary>
        public static bool TextRainbowMode
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.TextRainbowMode;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.TextRainbowMode = value;
            }
        }
        /// <summary>
        /// [Text] The minimum red color level (true color)
        /// </summary>
        public static int TextMinimumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.TextMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.TextMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Text] The minimum green color level (true color)
        /// </summary>
        public static int TextMinimumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.TextMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.TextMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Text] The minimum blue color level (true color)
        /// </summary>
        public static int TextMinimumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.TextMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.TextMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Text] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int TextMinimumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.TextMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                ScreensaverPackInit.SaversConfig.TextMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Text] The maximum red color level (true color)
        /// </summary>
        public static int TextMaximumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.TextMaximumRedColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.TextMinimumRedColorLevel)
                    value = ScreensaverPackInit.SaversConfig.TextMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.TextMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Text] The maximum green color level (true color)
        /// </summary>
        public static int TextMaximumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.TextMaximumGreenColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.TextMinimumGreenColorLevel)
                    value = ScreensaverPackInit.SaversConfig.TextMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.TextMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Text] The maximum blue color level (true color)
        /// </summary>
        public static int TextMaximumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.TextMaximumBlueColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.TextMinimumBlueColorLevel)
                    value = ScreensaverPackInit.SaversConfig.TextMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.TextMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Text] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int TextMaximumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.TextMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= ScreensaverPackInit.SaversConfig.TextMinimumColorLevel)
                    value = ScreensaverPackInit.SaversConfig.TextMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                ScreensaverPackInit.SaversConfig.TextMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for Text
    /// </summary>
    public class TextDisplay : BaseScreensaver, IScreensaver
    {

        private int currentHueAngle = 0;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Text";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Get the color and positions
            Color color = ChangeTextColor();
            string renderedText = TextSettings.TextWrite;
            int halfConsoleY = (int)(ConsoleWrapper.WindowHeight / 2d);
            int textPosX = ConsoleWrapper.WindowWidth / 2 - renderedText.Length / 2;

            // Write the text
            if (TextSettings.TextRainbowMode)
            {
                color = new($"hsl:{currentHueAngle};100;50");
                currentHueAngle++;
                if (currentHueAngle > 360)
                    currentHueAngle = 0;
            }
            TextWriterWhereColor.WriteWhereColor(renderedText, textPosX, halfConsoleY, color);

            // Delay
            int delay = TextSettings.TextRainbowMode ? 16 : TextSettings.TextDelay;
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
        private Color ChangeTextColor()
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
