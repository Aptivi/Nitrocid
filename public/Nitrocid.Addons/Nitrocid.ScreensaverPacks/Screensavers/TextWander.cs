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
    /// Settings for TextWander
    /// </summary>
    public static class TextWanderSettings
    {

        /// <summary>
        /// [TextWander] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool TextWanderTrueColor
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.TextWanderTrueColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.TextWanderTrueColor = value;
            }
        }
        /// <summary>
        /// [TextWander] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int TextWanderDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.TextWanderDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                ScreensaverPackInit.SaversConfig.TextWanderDelay = value;
            }
        }
        /// <summary>
        /// [TextWander] TextWander for Bouncing TextWander. Shorter is better.
        /// </summary>
        public static string TextWanderWrite
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.TextWanderWrite;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Nitrocid KS";
                ScreensaverPackInit.SaversConfig.TextWanderWrite = value;
            }
        }
        /// <summary>
        /// [TextWander] The minimum red color level (true color)
        /// </summary>
        public static int TextWanderMinimumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.TextWanderMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.TextWanderMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [TextWander] The minimum green color level (true color)
        /// </summary>
        public static int TextWanderMinimumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.TextWanderMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.TextWanderMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [TextWander] The minimum blue color level (true color)
        /// </summary>
        public static int TextWanderMinimumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.TextWanderMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.TextWanderMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [TextWander] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int TextWanderMinimumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.TextWanderMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                ScreensaverPackInit.SaversConfig.TextWanderMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [TextWander] The maximum red color level (true color)
        /// </summary>
        public static int TextWanderMaximumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.TextWanderMaximumRedColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.TextWanderMinimumRedColorLevel)
                    value = ScreensaverPackInit.SaversConfig.TextWanderMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.TextWanderMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [TextWander] The maximum green color level (true color)
        /// </summary>
        public static int TextWanderMaximumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.TextWanderMaximumGreenColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.TextWanderMinimumGreenColorLevel)
                    value = ScreensaverPackInit.SaversConfig.TextWanderMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.TextWanderMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [TextWander] The maximum blue color level (true color)
        /// </summary>
        public static int TextWanderMaximumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.TextWanderMaximumBlueColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.TextWanderMinimumBlueColorLevel)
                    value = ScreensaverPackInit.SaversConfig.TextWanderMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.TextWanderMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [TextWander] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int TextWanderMaximumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.TextWanderMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= ScreensaverPackInit.SaversConfig.TextWanderMinimumColorLevel)
                    value = ScreensaverPackInit.SaversConfig.TextWanderMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                ScreensaverPackInit.SaversConfig.TextWanderMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for TextWander
    /// </summary>
    public class TextWanderDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "TextWander";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Get the color and positions
            Color color = ChangeTextWanderColor();
            string renderedTextWander = TextWanderSettings.TextWanderWrite;
            int furthestX = ConsoleWrapper.WindowWidth - renderedTextWander.Length;
            int randomPosX = RandomDriver.RandomIdx(furthestX);
            int randomPosY = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);

            // Write the text
            TextWriterWhereColor.WriteWhereColor(renderedTextWander, randomPosX, randomPosY, color);

            // Delay
            ThreadManager.SleepNoBlock(TextWanderSettings.TextWanderDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            TextWriterWhereColor.WriteWhereColor(new string(' ', renderedTextWander.Length), randomPosX, randomPosY, color);
        }

        /// <summary>
        /// Changes the color of date and time
        /// </summary>
        public Color ChangeTextWanderColor()
        {
            Color ColorInstance;
            if (TextWanderSettings.TextWanderTrueColor)
            {
                int RedColorNum = RandomDriver.Random(TextWanderSettings.TextWanderMinimumRedColorLevel, TextWanderSettings.TextWanderMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(TextWanderSettings.TextWanderMinimumGreenColorLevel, TextWanderSettings.TextWanderMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(TextWanderSettings.TextWanderMinimumBlueColorLevel, TextWanderSettings.TextWanderMaximumBlueColorLevel);
                ColorInstance = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(TextWanderSettings.TextWanderMinimumColorLevel, TextWanderSettings.TextWanderMaximumColorLevel);
                ColorInstance = new Color(ColorNum);
            }
            return ColorInstance;
        }

    }
}
