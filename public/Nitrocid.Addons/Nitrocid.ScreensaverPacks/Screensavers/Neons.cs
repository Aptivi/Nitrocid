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

using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Misc.Screensaver;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Textify.Data.Figlet;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.CyclicWriters.Graphical;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for Neons
    /// </summary>
    public class NeonsDisplay : BaseScreensaver, IScreensaver
    {
        private int currentStep = 0;

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "Neons";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Get the color and positions
            Color color = currentStep == 0 ? ConsoleColors.Fuchsia : ConsoleColors.Aqua;
            var figletFont = FigletTools.GetFigletFont(ScreensaverPackInit.SaversConfig.NeonsFont);
            var renderedText = new AlignedFigletText(figletFont)
            {
                Text = "Neon!",
                ForegroundColor = color,
                Settings = new()
                {
                    Alignment = TextAlignment.Middle
                }
            };

            // Write the neons
            TextWriterRaw.WriteRaw(renderedText.Render());

            // Delay
            currentStep++;
            if (currentStep == 2)
                currentStep = 0;
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.NeonsDelay);
        }

        /// <inheritdoc/>
        public override void ScreensaverOutro()
        {
            currentStep = 0;
        }
    }
}
