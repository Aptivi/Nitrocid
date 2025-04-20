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

using System;
using System.Text;
using Textify.Data.Figlet;
using Terminaux.Colors;
using Textify.Data.Words;
using Nitrocid.Drivers;
using Nitrocid.Misc.Screensaver;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Languages;
using Nitrocid.Drivers.Encryption;
using Nitrocid.Drivers.RNG;
using Terminaux.Base;
using Terminaux.Colors.Data;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.CyclicWriters.Graphical;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for WordHasherWrite
    /// </summary>
    public class WordHasherWriteDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "WordHasherWrite";

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
            var wordText = new AlignedFigletText(figFont)
            {
                Top = consoleY,
                Text = word,
                ForegroundColor = ConsoleColors.Green,
                Settings = new()
                {
                    Alignment = TextAlignment.Middle,
                }
            };
            TextWriterRaw.WriteRaw(wordText.Render());
            TextWriterWhereColor.WriteWhereColor(wordHash, (int)Math.Round(ConsoleWrapper.WindowWidth / 2d - wordHash.Length / 2d), hashY, ConsoleColors.Green);
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
                ScreensaverManager.Delay(250);
                ConsoleWrapper.Clear();
                var wordText = new AlignedFigletText(figFont)
                {
                    Top = consoleY,
                    Text = finalWord,
                    ForegroundColor = hasherColor,
                    Settings = new()
                    {
                        Alignment = TextAlignment.Middle,
                    }
                };
                TextWriterRaw.WriteRaw(wordText.Render());
                TextWriterWhereColor.WriteWhereColor(wordHash, (int)Math.Round(ConsoleWrapper.WindowWidth / 2d - wordHash.Length / 2d), hashY, hasherColor);
            }

            // Delay
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.WordHasherWriteDelay);

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
                ScreensaverManager.Delay(250);
                ConsoleWrapper.Clear();
                var wordText = new AlignedFigletText(figFont)
                {
                    Top = consoleY,
                    Text = finalWord,
                    ForegroundColor = hasherColor,
                    Settings = new()
                    {
                        Alignment = TextAlignment.Middle,
                    }
                };
                TextWriterRaw.WriteRaw(wordText.Render());
                TextWriterWhereColor.WriteWhereColor(wordHash, (int)Math.Round(ConsoleWrapper.WindowWidth / 2d - wordHash.Length / 2d), hashY, hasherColor);
            }
        }

        /// <summary>
        /// Changes the color of word and its hash
        /// </summary>
        private Color ChangeWordHasherWriteColor()
        {
            Color ColorInstance;
            if (ScreensaverPackInit.SaversConfig.WordHasherWriteTrueColor)
            {
                int RedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.WordHasherWriteMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.WordHasherWriteMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.WordHasherWriteMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.WordHasherWriteMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.WordHasherWriteMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.WordHasherWriteMaximumBlueColorLevel);
                ColorInstance = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.WordHasherWriteMinimumColorLevel, ScreensaverPackInit.SaversConfig.WordHasherWriteMaximumColorLevel);
                ColorInstance = new Color(ColorNum);
            }
            return ColorInstance;
        }

    }
}
