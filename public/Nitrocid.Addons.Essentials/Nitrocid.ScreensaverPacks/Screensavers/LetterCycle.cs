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
using Textify.Data.Figlet;
using Terminaux.Colors;
using Textify.Data.Words;
using Nitrocid.Base.Drivers;
using Nitrocid.Base.Misc.Screensaver;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Drivers.Encryption;
using Nitrocid.Base.Drivers.RNG;
using Terminaux.Base;
using Terminaux.Colors.Data;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.CyclicWriters.Graphical;
using Textify.General;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for LetterCycle
    /// </summary>
    public class LetterCycleDisplay : BaseScreensaver, IScreensaver
    {
        // TODO: NKS_SCREENSAVERPACKS_LETTERCYCLE_SETTINGS_DESC -> Cycles English letters with Figlet
        /// <inheritdoc/>
        public override string ScreensaverName =>
            "LetterCycle";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Change color
            var color = ChangeLetterCycleColor();

            // Write word and hash
            char[] chars = CharManager.GetAllLettersAndNumbers(false);
            int charIdx = RandomDriver.RandomIdx(chars.Length);
            char character = chars[charIdx];
            string charString = character.ToString();
            string charId = $"{charIdx + 1}/{chars.Length}";
            var figFont = FigletTools.GetFigletFont("small");
            int figHeight = FigletTools.GetFigletHeight(charString, figFont) / 2;
            int consoleY = ConsoleWrapper.WindowHeight / 2 - figHeight;
            int hashY = ConsoleWrapper.WindowHeight / 2 + figHeight + 2;
            ConsoleWrapper.Clear();
            var wordText = new AlignedFigletText(figFont)
            {
                Top = consoleY,
                Text = charString,
                ForegroundColor = color,
                Settings = new()
                {
                    Alignment = TextAlignment.Middle,
                }
            };
            TextWriterRaw.WriteRaw(wordText.Render());
            TextWriterWhereColor.WriteWhereColor(charId, (int)Math.Round(ConsoleWrapper.WindowWidth / 2d - charId.Length / 2d), hashY, color);

            // Delay
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.LetterCycleDelay);
        }

        /// <summary>
        /// Changes the color of word and its hash
        /// </summary>
        private Color ChangeLetterCycleColor()
        {
            Color ColorInstance;
            if (ScreensaverPackInit.SaversConfig.LetterCycleTrueColor)
            {
                int RedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.LetterCycleMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.LetterCycleMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.LetterCycleMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.LetterCycleMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.LetterCycleMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.LetterCycleMaximumBlueColorLevel);
                ColorInstance = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.LetterCycleMinimumColorLevel, ScreensaverPackInit.SaversConfig.LetterCycleMaximumColorLevel);
                ColorInstance = new Color(ColorNum);
            }
            return ColorInstance;
        }

    }
}
