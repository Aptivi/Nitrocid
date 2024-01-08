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

using System;
using Figletize;
using Terminaux.Colors;
using Textify.Words;
using Nitrocid.Drivers;
using Nitrocid.Misc.Screensaver;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Languages;
using Nitrocid.Drivers.Encryption;
using Nitrocid.ConsoleBase;
using Terminaux.Writer.FancyWriters;
using Nitrocid.Kernel.Threading;
using Nitrocid.Drivers.RNG;
using Terminaux.Base;

namespace Nitrocid.ScreensaverPacks.Screensavers
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
                return ScreensaverPackInit.SaversConfig.WordHasherTrueColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.WordHasherTrueColor = value;
            }
        }
        /// <summary>
        /// [WordHasher] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int WordHasherDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.WordHasherDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                ScreensaverPackInit.SaversConfig.WordHasherDelay = value;
            }
        }
        /// <summary>
        /// [WordHasher] The minimum red color level (true color)
        /// </summary>
        public static int WordHasherMinimumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.WordHasherMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.WordHasherMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasher] The minimum green color level (true color)
        /// </summary>
        public static int WordHasherMinimumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.WordHasherMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.WordHasherMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasher] The minimum blue color level (true color)
        /// </summary>
        public static int WordHasherMinimumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.WordHasherMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.WordHasherMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasher] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int WordHasherMinimumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.WordHasherMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                ScreensaverPackInit.SaversConfig.WordHasherMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasher] The maximum red color level (true color)
        /// </summary>
        public static int WordHasherMaximumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.WordHasherMaximumRedColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.WordHasherMinimumRedColorLevel)
                    value = ScreensaverPackInit.SaversConfig.WordHasherMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.WordHasherMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasher] The maximum green color level (true color)
        /// </summary>
        public static int WordHasherMaximumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.WordHasherMaximumGreenColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.WordHasherMinimumGreenColorLevel)
                    value = ScreensaverPackInit.SaversConfig.WordHasherMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.WordHasherMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasher] The maximum blue color level (true color)
        /// </summary>
        public static int WordHasherMaximumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.WordHasherMaximumBlueColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.WordHasherMinimumBlueColorLevel)
                    value = ScreensaverPackInit.SaversConfig.WordHasherMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.WordHasherMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [WordHasher] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int WordHasherMaximumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.WordHasherMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= ScreensaverPackInit.SaversConfig.WordHasherMinimumColorLevel)
                    value = ScreensaverPackInit.SaversConfig.WordHasherMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                ScreensaverPackInit.SaversConfig.WordHasherMaximumColorLevel = value;
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
            CenteredFigletTextColor.WriteCenteredFigletColor(consoleY, figFont, word, ConsoleColors.Green);
            TextWriterWhereColor.WriteWhereColor(wordHash, (int)Math.Round(ConsoleWrapper.WindowWidth / 2d - wordHash.Length / 2d), hashY, ConsoleColors.Green);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Change color
            var hasherColor = ChangeWordHasherColor();

            // Write word and hash
            string word = WordManager.GetRandomWord();
            string wordHash = DriverHandler.GetDriver<IEncryptionDriver>("SHA256").GetEncryptedString(word);
            var figFont = FigletTools.GetFigletFont("small");
            int figHeight = FigletTools.GetFigletHeight(word, figFont) / 2;
            int consoleY = ConsoleWrapper.WindowHeight / 2 - figHeight;
            int hashY = ConsoleWrapper.WindowHeight / 2 + figHeight + 2;
            ConsoleWrapper.Clear();
            CenteredFigletTextColor.WriteCenteredFigletColor(consoleY, figFont, word, hasherColor);
            TextWriterWhereColor.WriteWhereColor(wordHash, (int)Math.Round(ConsoleWrapper.WindowWidth / 2d - wordHash.Length / 2d), hashY, hasherColor);

            // Delay
            ThreadManager.SleepNoBlock(WordHasherSettings.WordHasherDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

        /// <summary>
        /// Changes the color of word and its hash
        /// </summary>
        private Color ChangeWordHasherColor()
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
