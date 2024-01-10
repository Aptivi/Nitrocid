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
using Nitrocid.Kernel.Debugging;
using Nitrocid.Misc.Screensaver;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Kernel.Time;
using Nitrocid.Misc.Text;
using Terminaux.Writer.FancyWriters;
using Nitrocid.Languages;
using Nitrocid.Kernel.Threading;
using Terminaux.Base;
using Terminaux.Colors.Data;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for NewYear
    /// </summary>
    public class NewYearDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "NewYear";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ColorTools.LoadBackDry(new Color(ConsoleColors.Black));
            ConsoleWrapper.CursorVisible = false;
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            Color darkGreen = new(ConsoleColors.DarkGreen);
            Color green = new(ConsoleColors.Green);

            // Get the current year
            int currentYear = TimeDateTools.KernelDateTime.Year;
            var currentYearDate = new DateTime(currentYear, 1, 1);

            // Select mode
            if (TimeDateTools.KernelDateTime.Date == currentYearDate)
            {
                // We're at the new year!
                string currentYearStr = currentYear.ToString();
                var figFont = FigletTools.GetFigletFont(FigletTextTools.DefaultFigletFontName);
                int figHeight = FigletTools.GetFigletHeight(currentYearStr, figFont) / 2;
                CenteredFigletTextColor.WriteCenteredFigletColor(figFont, currentYearStr, green);

                // Congratulate!
                string cong = Translate.DoTranslation("Happy new year!");
                int consoleInfoX = ConsoleWrapper.WindowWidth / 2 - cong.Length / 2;
                int consoleInfoY = ConsoleWrapper.WindowHeight / 2 + figHeight + 2;
                TextWriterWhereColor.WriteWhere(cong, consoleInfoX, consoleInfoY);
            }
            else
            {
                // Print the countdown, but print the next year first using Figlet
                string nextYearStr = $"{currentYear + 1}";
                var figFont = FigletTools.GetFigletFont(FigletTextTools.DefaultFigletFontName);
                int figHeight = FigletTools.GetFigletHeight(nextYearStr, figFont) / 2;
                CenteredFigletTextColor.WriteCenteredFigletColor(figFont, nextYearStr, darkGreen);

                // Print the time remaining
                var nextYearDate = new DateTime(currentYear + 1, 1, 1);
                var distance = nextYearDate - TimeDateTools.KernelDateTime;
                string distanceStr = distance.ToString("dd\\d\\ hh\\:mm\\:ss") + " left till " + nextYearStr;
                int consoleInfoY = ConsoleWrapper.WindowHeight / 2 + figHeight + 2;
                CenteredTextColor.WriteCentered(consoleInfoY, distanceStr);
            }

            // Reset
            ThreadManager.SleepNoBlock(500, ScreensaverDisplayer.ScreensaverDisplayerThread);
            ConsoleResizeHandler.WasResized();
        }

    }
}
