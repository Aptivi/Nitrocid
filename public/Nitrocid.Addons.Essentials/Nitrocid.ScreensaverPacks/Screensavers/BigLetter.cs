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
using System.Linq;
using Textify.Data.Figlet;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Misc.Screensaver;
using Nitrocid.Base.Kernel.Configuration;
using Terminaux.Colors;
using Textify.General;
using Terminaux.Base;
using Terminaux.Colors.Themes.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for Figlet
    /// </summary>
    public class BigLetterDisplay : BaseScreensaver, IScreensaver
    {

        private int currentHueAngle = 0;

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "BigLetter";

        /// <inheritdoc/>
        public override void ScreensaverPreparation() =>
            ThemeColorsTools.LoadBackground();

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            int ConsoleMiddleWidth = (int)Math.Round(ConsoleWrapper.WindowWidth / 2d);
            int ConsoleMiddleHeight = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d);
            var FigletFontUsed = FigletTools.GetFigletFont(ScreensaverPackInit.SaversConfig.BigLetterFont);
            ConsoleWrapper.CursorVisible = false;
            ConsoleWrapper.Clear();

            // Set colors
            var ColorStorage = new Color(255, 255, 255);
            if (ScreensaverPackInit.SaversConfig.BigLetterTrueColor)
            {
                int RedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.BigLetterMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.BigLetterMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.BigLetterMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.BigLetterMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.BigLetterMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.BigLetterMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", vars: [RedColorNum, GreenColorNum, BlueColorNum]);
                ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.BigLetterMinimumColorLevel, ScreensaverPackInit.SaversConfig.BigLetterMaximumColorLevel);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color ({0})", vars: [ColorNum]);
                ColorStorage = new Color(ColorNum);
            }
            if (ScreensaverPackInit.SaversConfig.BigLetterRainbowMode)
            {
                ColorStorage = new($"hsl:{currentHueAngle};100;50");
                currentHueAngle++;
                if (currentHueAngle > 360)
                    currentHueAngle = 0;
            }

            // Prepare the figlet font for writing
            var chars = CharManager.GetAllLetters(false);
            string FigletWrite = $"{chars[RandomDriver.RandomIdx(chars.Length)]}";
            FigletWrite = FigletFontUsed.Render(FigletWrite);
            var FigletWriteLines = FigletWrite.SplitNewLines().SkipWhile(string.IsNullOrEmpty).ToArray();
            int FigletHeight = (int)Math.Round(ConsoleMiddleHeight - FigletWriteLines.Length / 2d);
            int FigletWidth = (int)Math.Round(ConsoleMiddleWidth - FigletWriteLines[0].Length / 2d);

            // Actually write it
            if (!ConsoleResizeHandler.WasResized(false))
                TextWriterWhereColor.WriteWhereColor(FigletWrite, FigletWidth, FigletHeight, true, ColorStorage);

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            int delay = ScreensaverPackInit.SaversConfig.BigLetterRainbowMode ? 16 : ScreensaverPackInit.SaversConfig.BigLetterDelay;
            ScreensaverManager.Delay(delay);
        }

        /// <inheritdoc/>
        public override void ScreensaverOutro()
        {
            currentHueAngle = 0;
        }

    }
}
