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
using System.Text;
using Nitrocid.Base.Drivers;
using Nitrocid.Base.Drivers.Encryption;
using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Screensaver;
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Textify.Data.Figlet;
using Textify.Data.NameGen;
using Textify.Data.Words;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for MarkScreen
    /// </summary>
    public class MarkScreenDisplay : BaseScreensaver, IScreensaver
    {
        // TODO: NKS_SCREENSAVERPACKS_MARKSCREEN_SETTINGS_DESC -> Shows an old-style terminal screen that features student names and grades for all subjects, similar to an old university grade book
        private string[] names = [];

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "MarkScreen";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            names = NameGenerator.GenerateNames(1000);
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            var builder = new StringBuilder();

            // Write an infobox border
            int height = ConsoleWrapper.WindowHeight - 4;
            int width = ConsoleWrapper.WindowWidth - 8;
            int posX = ConsoleWrapper.WindowWidth / 2 - width / 2 - 1;
            int posY = ConsoleWrapper.WindowHeight / 2 - height / 2 - 1;
            var border = new BoxFrame()
            {
                Left = posX,
                Top = posY,
                Width = width,
                Height = height,
                UseColors = true,
            };
            builder.Append(border.Render());

            // Delay
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.MarkScreenDelay);
        }

    }
}
