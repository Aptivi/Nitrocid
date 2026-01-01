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

using System;
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
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for WordHasher
    /// </summary>
    public class WordHasherDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "WordHasher";

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
            var hasherColor = ChangeWordHasherColor();

            // Write word and hash
            string word = WordManager.GetRandomWord();
            string wordHash = DriverHandler.GetDriver<IEncryptionDriver>("SHA256").GetEncryptedString(word);
            var figFont = FigletTools.GetFigletFont("small");
            int figHeight = FigletTools.GetFigletHeight(word, figFont) / 2;
            int consoleY = ConsoleWrapper.WindowHeight / 2 - figHeight;
            int hashY = ConsoleWrapper.WindowHeight / 2 + figHeight + 2;
            ConsoleWrapper.Clear();
            var wordText = new AlignedFigletText(figFont)
            {
                Top = consoleY,
                Text = word,
                ForegroundColor = hasherColor,
                Settings = new()
                {
                    Alignment = TextAlignment.Middle,
                }
            };
            TextWriterRaw.WriteRaw(wordText.Render());
            TextWriterWhereColor.WriteWhereColor(wordHash, (int)Math.Round(ConsoleWrapper.WindowWidth / 2d - wordHash.Length / 2d), hashY, hasherColor);

            // Delay
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.WordHasherDelay);
        }

        /// <summary>
        /// Changes the color of word and its hash
        /// </summary>
        private Color ChangeWordHasherColor()
        {
            Color ColorInstance;
            if (ScreensaverPackInit.SaversConfig.WordHasherTrueColor)
            {
                int RedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.WordHasherMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.WordHasherMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.WordHasherMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.WordHasherMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.WordHasherMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.WordHasherMaximumBlueColorLevel);
                ColorInstance = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.WordHasherMinimumColorLevel, ScreensaverPackInit.SaversConfig.WordHasherMaximumColorLevel);
                ColorInstance = new Color(ColorNum);
            }
            return ColorInstance;
        }

    }
}
