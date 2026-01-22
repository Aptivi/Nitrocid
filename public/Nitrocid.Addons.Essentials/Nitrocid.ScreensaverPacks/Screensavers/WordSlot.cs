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

using Textify.Data.Figlet;
using Terminaux.Colors;
using Textify.Data.Words;
using Nitrocid.Misc.Screensaver;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Languages;
using Nitrocid.Drivers.RNG;
using Terminaux.Base;
using Terminaux.Colors.Data;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for WordSlot
    /// </summary>
    public class WordSlotDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "WordSlot";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            base.ScreensaverPreparation();

            // Write loading
            string word = Translate.DoTranslation("Loading...");
            var figFont = FigletTools.GetFigletFont("small");
            int figHeight = FigletTools.GetFigletHeight(word, figFont) / 2;
            int consoleY = ConsoleWrapper.WindowHeight / 2 - figHeight;
            var wordText = new AlignedFigletText(figFont)
            {
                Text = word,
                Top = consoleY,
                ForegroundColor = ConsoleColors.Green,
                Settings = new()
                {
                    Alignment = TextAlignment.Middle,
                }
            };
            TextWriterRaw.WriteRaw(wordText.Render());
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Change color
            var hasherColor = ChangeWordSlotColor();

            // Make it work like a slot machine found in casinos, but in words instead of three items and with no "chips" earned.
            int steps = RandomDriver.Random(10, 100);
            for (int s = 0; s < steps; s++)
            {
                if (ConsoleResizeHandler.WasResized(false))
                    break;
                if (ScreensaverManager.Bailing)
                    return;

                ConsoleWrapper.Clear();

                // Write word
                string word = WordManager.GetRandomWord();
                var figFont = FigletTools.GetFigletFont("small");
                int figHeight = FigletTools.GetFigletHeight(word, figFont) / 2;
                int consoleY = ConsoleWrapper.WindowHeight / 2 - figHeight;
                var wordText = new AlignedFigletText(figFont)
                {
                    Text = word,
                    Top = consoleY,
                    ForegroundColor = hasherColor,
                    Settings = new()
                    {
                        Alignment = TextAlignment.Middle,
                    }
                };
                TextWriterRaw.WriteRaw(wordText.Render());
                ScreensaverManager.Delay(25, true);
            }

            // Delay
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.WordSlotDelay, true);
        }

        /// <summary>
        /// Changes the color of word and its hash
        /// </summary>
        private Color ChangeWordSlotColor()
        {
            Color ColorInstance;
            if (ScreensaverPackInit.SaversConfig.WordSlotTrueColor)
            {
                int RedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.WordSlotMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.WordSlotMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.WordSlotMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.WordSlotMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.WordSlotMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.WordSlotMaximumBlueColorLevel);
                ColorInstance = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.WordSlotMinimumColorLevel, ScreensaverPackInit.SaversConfig.WordSlotMaximumColorLevel);
                ColorInstance = new Color(ColorNum);
            }
            return ColorInstance;
        }

    }
}
