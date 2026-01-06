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
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Misc.Screensaver;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Kernel.Time;
using Nitrocid.Base.Languages;
using Terminaux.Base;
using Terminaux.Colors.Data;
using Nitrocid.Base.Kernel.Configuration;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.CyclicWriters.Graphical;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for NewYear
    /// </summary>
    public class NewYearDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "NewYear";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ColorTools.LoadBackDry(new Color(ConsoleColors.Black));
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", vars: [ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight]);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            Color darkGreen = new(ConsoleColors.DarkGreen);
            Color green = new(ConsoleColors.Green);
            Color black = new(ConsoleColors.Black);

            // Get the current year
            int currentYear = TimeDateTools.KernelDateTime.Year;
            var currentYearDate = new DateTime(currentYear, 1, 1);

            // Select mode
            if (TimeDateTools.KernelDateTime.Date == currentYearDate)
            {
                // We're at the new year!
                string currentYearStr = currentYear.ToString();
                var figFont = FigletTools.GetFigletFont(Config.MainConfig.DefaultFigletFontName);
                int figHeight = FigletTools.GetFigletHeight(currentYearStr, figFont) / 2;
                int consoleY = ConsoleWrapper.WindowHeight / 2 - figHeight;
                var yearText = new AlignedFigletText(figFont)
                {
                    Text = currentYearStr,
                    Top = consoleY,
                    ForegroundColor = green,
                    BackgroundColor = black,
                    Settings = new()
                    {
                        Alignment = TextAlignment.Middle,
                    }
                };
                TextWriterRaw.WriteRaw(yearText.Render());

                // Congratulate!
                string cong = LanguageTools.GetLocalized("NKS_SCREENSAVERPACKS_NEWYEAR_CONGRATS");
                int consoleInfoY = ConsoleWrapper.WindowHeight / 2 + figHeight + 2;
                var congratsText = new AlignedText()
                {
                    Top = consoleInfoY,
                    Text = cong,
                    ForegroundColor = green,
                    BackgroundColor = black,
                    Settings = new()
                    {
                        Alignment = TextAlignment.Middle,
                    }
                };
                TextWriterRaw.WriteRaw(congratsText.Render());
            }
            else
            {
                // Print the countdown, but print the next year first using Figlet
                string nextYearStr = $"{currentYear + 1}";
                var figFont = FigletTools.GetFigletFont(Config.MainConfig.DefaultFigletFontName);
                int figHeight = FigletTools.GetFigletHeight(nextYearStr, figFont) / 2;
                int consoleY = ConsoleWrapper.WindowHeight / 2 - figHeight;
                var yearText = new AlignedFigletText(figFont)
                {
                    Text = nextYearStr,
                    Top = consoleY,
                    ForegroundColor = darkGreen,
                    BackgroundColor = black,
                    Settings = new()
                    {
                        Alignment = TextAlignment.Middle,
                    }
                };
                TextWriterRaw.WriteRaw(yearText.Render());

                // Print the time remaining
                var nextYearDate = new DateTime(currentYear + 1, 1, 1);
                var distance = nextYearDate - TimeDateTools.KernelDateTime;
                string distanceStr = distance.ToString("dd\\d\\ hh\\:mm\\:ss") + " " + LanguageTools.GetLocalized("NKS_SCREENSAVERPACKS_NEWYEAR_LEFT");
                int consoleInfoY = ConsoleWrapper.WindowHeight / 2 + figHeight + 2;
                var distanceText = new AlignedText()
                {
                    Top = consoleInfoY,
                    Text = distanceStr,
                    ForegroundColor = darkGreen,
                    BackgroundColor = black,
                    Settings = new()
                    {
                        Alignment = TextAlignment.Middle,
                    }
                };
                TextWriterRaw.WriteRaw(distanceText.Render());
            }

            // Reset
            ScreensaverManager.Delay(500);
        }

    }
}
