
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
using System.Text;
using Figletize;
using KS.ConsoleBase;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.Drivers;
using KS.Drivers.Encryption;
using KS.Drivers.RNG;
using KS.Kernel.Configuration;
using KS.Kernel.Threading;
using KS.Languages;
using KS.Misc.Screensaver;
using Terminaux.Colors;
using Wordament;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Settings for WordHasherWrite
    /// </summary>
    public static class WordHasherWriteSettings
    {

        /// <summary>
        /// [WordHasherWrite] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool WordHasherWriteTrueColor
        {
            get
            {
                return Config.SaverConfig.WordHasherWriteTrueColor;
            }
            set
            {
                Config.SaverConfig.WordHasherWriteTrueColor = value;
            }
        }
        /// <summary>
        /// [WordHasherWrite] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int WordHasherWriteDelay
        {
            get
            {
                return Config.SaverConfig.WordHasherWriteDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                Config.SaverConfig.WordHasherWriteDelay = value;
            }
        }
        /// <summary>
        /// [WordHasherWrite] The minimum red color level (true color)
        /// </summary>
        public static int WordHasherWriteMinimumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.WordHasherWriteMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.WordHasherWriteMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasherWrite] The minimum green color level (true color)
        /// </summary>
        public static int WordHasherWriteMinimumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.WordHasherWriteMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.WordHasherWriteMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasherWrite] The minimum blue color level (true color)
        /// </summary>
        public static int WordHasherWriteMinimumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.WordHasherWriteMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.WordHasherWriteMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasherWrite] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int WordHasherWriteMinimumColorLevel
        {
            get
            {
                return Config.SaverConfig.WordHasherWriteMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                Config.SaverConfig.WordHasherWriteMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasherWrite] The maximum red color level (true color)
        /// </summary>
        public static int WordHasherWriteMaximumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.WordHasherWriteMaximumRedColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.WordHasherWriteMinimumRedColorLevel)
                    value = Config.SaverConfig.WordHasherWriteMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.WordHasherWriteMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasherWrite] The maximum green color level (true color)
        /// </summary>
        public static int WordHasherWriteMaximumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.WordHasherWriteMaximumGreenColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.WordHasherWriteMinimumGreenColorLevel)
                    value = Config.SaverConfig.WordHasherWriteMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.WordHasherWriteMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasherWrite] The maximum blue color level (true color)
        /// </summary>
        public static int WordHasherWriteMaximumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.WordHasherWriteMaximumBlueColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.WordHasherWriteMinimumBlueColorLevel)
                    value = Config.SaverConfig.WordHasherWriteMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.WordHasherWriteMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasherWrite] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int WordHasherWriteMaximumColorLevel
        {
            get
            {
                return Config.SaverConfig.WordHasherWriteMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= Config.SaverConfig.WordHasherWriteMinimumColorLevel)
                    value = Config.SaverConfig.WordHasherWriteMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                Config.SaverConfig.WordHasherWriteMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for WordHasherWrite
    /// </summary>
    public class WordHasherWriteDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "WordHasherWrite";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            base.ScreensaverPreparation();

            // Write loading
            string word = Translate.DoTranslation("Loading...");
            string wordHash = DriverHandler.GetDriver<IEncryptionDriver>("SHA256").GetEncryptedString(word);
            var figFont = FigletTools.GetFigletFont("small");
            int figHeight = FigletTools.GetFigletHeight(word, figFont) / 2;
            int consoleY = ConsoleWrapper.WindowHeight / 2 - figHeight;
            int hashY = ConsoleWrapper.WindowHeight / 2 + figHeight + 2;
            CenteredFigletTextColor.WriteCenteredFiglet(consoleY, figFont, word, ConsoleColors.Green);
            TextWriterWhereColor.WriteWhere(wordHash, (int)Math.Round(ConsoleWrapper.WindowWidth / 2d - wordHash.Length / 2d), hashY, ConsoleColors.Green);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Change color
            var hasherColor = ChangeWordHasherWriteColor();

            // Write word and hash
            string word = WordManager.GetRandomWord();
            var figFont = FigletTools.GetFigletFont("small");
            StringBuilder builtWord = new();
            for (int i = 0; i < word.Length; i++)
            {
                builtWord.Append(word[i]);
                string finalWord = builtWord.ToString();
                string wordHash = DriverHandler.GetDriver<IEncryptionDriver>("SHA256").GetEncryptedString(finalWord);
                int figHeight = FigletTools.GetFigletHeight(finalWord, figFont) / 2;
                int consoleY = ConsoleWrapper.WindowHeight / 2 - figHeight;
                int hashY = ConsoleWrapper.WindowHeight / 2 + figHeight + 2;
                ThreadManager.SleepNoBlock(250, ScreensaverDisplayer.ScreensaverDisplayerThread);
                ConsoleWrapper.Clear();
                CenteredFigletTextColor.WriteCenteredFiglet(consoleY, figFont, finalWord, hasherColor);
                TextWriterWhereColor.WriteWhere(wordHash, (int)Math.Round(ConsoleWrapper.WindowWidth / 2d - wordHash.Length / 2d), hashY, hasherColor);
            }

            // Delay
            ThreadManager.SleepNoBlock(WordHasherWriteSettings.WordHasherWriteDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);

            // Destruct word
            for (int i = builtWord.Length - 1; i >= 0; i--)
            {
                builtWord.Remove(i, 1);
                if (builtWord.Length == 0)
                    break;
                string finalWord = builtWord.ToString();
                string wordHash = DriverHandler.GetDriver<IEncryptionDriver>("SHA256").GetEncryptedString(finalWord);
                int figHeight = FigletTools.GetFigletHeight(finalWord, figFont) / 2;
                int consoleY = ConsoleWrapper.WindowHeight / 2 - figHeight;
                int hashY = ConsoleWrapper.WindowHeight / 2 + figHeight + 2;
                ThreadManager.SleepNoBlock(250, ScreensaverDisplayer.ScreensaverDisplayerThread);
                ConsoleWrapper.Clear();
                CenteredFigletTextColor.WriteCenteredFiglet(consoleY, figFont, finalWord, hasherColor);
                TextWriterWhereColor.WriteWhere(wordHash, (int)Math.Round(ConsoleWrapper.WindowWidth / 2d - wordHash.Length / 2d), hashY, hasherColor);
            }
        }

        /// <summary>
        /// Changes the color of word and its hash
        /// </summary>
        private Color ChangeWordHasherWriteColor()
        {
            Color ColorInstance;
            if (WordHasherWriteSettings.WordHasherWriteTrueColor)
            {
                int RedColorNum = RandomDriver.Random(WordHasherWriteSettings.WordHasherWriteMinimumRedColorLevel, WordHasherWriteSettings.WordHasherWriteMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(WordHasherWriteSettings.WordHasherWriteMinimumGreenColorLevel, WordHasherWriteSettings.WordHasherWriteMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(WordHasherWriteSettings.WordHasherWriteMinimumBlueColorLevel, WordHasherWriteSettings.WordHasherWriteMaximumBlueColorLevel);
                ColorInstance = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(WordHasherWriteSettings.WordHasherWriteMinimumColorLevel, WordHasherWriteSettings.WordHasherWriteMaximumColorLevel);
                ColorInstance = new Color(ColorNum);
            }
            return ColorInstance;
        }

    }
}
