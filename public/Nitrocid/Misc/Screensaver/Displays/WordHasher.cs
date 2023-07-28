
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

using System;
using ColorSeq;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.ConsoleBase.Writers.FancyWriters.Tools;
using KS.Drivers;
using KS.Drivers.RNG;
using KS.Kernel.Configuration;
using KS.Kernel.Threading;
using Wordament;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for WordHasher
    /// </summary>
    public static class WordHasherSettings
    {

        /// <summary>
        /// [WordHasher] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool WordHasherTrueColor
        {
            get
            {
                return Config.SaverConfig.WordHasherTrueColor;
            }
            set
            {
                Config.SaverConfig.WordHasherTrueColor = value;
            }
        }
        /// <summary>
        /// [WordHasher] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int WordHasherDelay
        {
            get
            {
                return Config.SaverConfig.WordHasherDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                Config.SaverConfig.WordHasherDelay = value;
            }
        }
        /// <summary>
        /// [WordHasher] The minimum red color level (true color)
        /// </summary>
        public static int WordHasherMinimumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.WordHasherMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.WordHasherMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasher] The minimum green color level (true color)
        /// </summary>
        public static int WordHasherMinimumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.WordHasherMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.WordHasherMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasher] The minimum blue color level (true color)
        /// </summary>
        public static int WordHasherMinimumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.WordHasherMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.WordHasherMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasher] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int WordHasherMinimumColorLevel
        {
            get
            {
                return Config.SaverConfig.WordHasherMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                Config.SaverConfig.WordHasherMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasher] The maximum red color level (true color)
        /// </summary>
        public static int WordHasherMaximumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.WordHasherMaximumRedColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.WordHasherMinimumRedColorLevel)
                    value = Config.SaverConfig.WordHasherMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.WordHasherMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasher] The maximum green color level (true color)
        /// </summary>
        public static int WordHasherMaximumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.WordHasherMaximumGreenColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.WordHasherMinimumGreenColorLevel)
                    value = Config.SaverConfig.WordHasherMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.WordHasherMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasher] The maximum blue color level (true color)
        /// </summary>
        public static int WordHasherMaximumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.WordHasherMaximumBlueColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.WordHasherMinimumBlueColorLevel)
                    value = Config.SaverConfig.WordHasherMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.WordHasherMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasher] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int WordHasherMaximumColorLevel
        {
            get
            {
                return Config.SaverConfig.WordHasherMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= Config.SaverConfig.WordHasherMinimumColorLevel)
                    value = Config.SaverConfig.WordHasherMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                Config.SaverConfig.WordHasherMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for WordHasher
    /// </summary>
    public class WordHasherDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "WordHasher";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.Clear();

            // Write word and hash
            string word = WordManager.GetRandomWord();
            string wordHash = DriverHandler.CurrentEncryptionDriverLocal.GetEncryptedString(word);
            var figFont = FigletTools.GetFigletFont("Small");
            int figHeight = FigletTools.GetFigletHeight(word, figFont) / 2;
            int consoleY = (ConsoleWrapper.WindowHeight / 2) - figHeight;
            int hashY = (ConsoleWrapper.WindowHeight / 2) + figHeight + 2;
            KernelColorTools.SetConsoleColor(ChangeWordHasherColor());
            CenteredFigletTextColor.WriteCenteredFiglet(consoleY, figFont, word);
            TextWriterWhereColor.WriteWhere(wordHash, (int)Math.Round(ConsoleWrapper.WindowWidth / 2d - wordHash.Length / 2d), hashY);

            // Delay
            ThreadManager.SleepNoBlock(WordHasherSettings.WordHasherDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

        /// <summary>
        /// Changes the color of word and its hash
        /// </summary>
        public Color ChangeWordHasherColor()
        {
            Color ColorInstance;
            if (WordHasherSettings.WordHasherTrueColor)
            {
                int RedColorNum = RandomDriver.Random(WordHasherSettings.WordHasherMinimumRedColorLevel, WordHasherSettings.WordHasherMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(WordHasherSettings.WordHasherMinimumGreenColorLevel, WordHasherSettings.WordHasherMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(WordHasherSettings.WordHasherMinimumBlueColorLevel, WordHasherSettings.WordHasherMaximumBlueColorLevel);
                ColorInstance = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(WordHasherSettings.WordHasherMinimumColorLevel, WordHasherSettings.WordHasherMaximumColorLevel);
                ColorInstance = new Color(ColorNum);
            }
            return ColorInstance;
        }

    }
}
