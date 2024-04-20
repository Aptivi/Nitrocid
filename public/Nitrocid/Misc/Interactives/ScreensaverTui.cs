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
using System.Collections.Generic;
using Nitrocid.Languages;
using Nitrocid.Misc.Screensaver;
using Terminaux.Inputs.Interactive;
using Terminaux.Reader;
using Textify.General;

namespace Nitrocid.Misc.Interactives
{
    internal class ScreensaverTui : BaseInteractiveTui<string>, IInteractiveTui<string>
    {
        public override InteractiveTuiBinding[] Bindings { get; } =
        [
            // Operations
            new InteractiveTuiBinding("Preview", ConsoleKey.Enter,
                (saver, _) => PressAndBailHelper((string)saver))
        ];

        /// <inheritdoc/>
        public override IEnumerable<string> PrimaryDataSource =>
            ScreensaverManager.GetScreensaverNames();

        /// <inheritdoc/>
        public override string GetInfoFromItem(string item)
        {
            // Get an instance of the screensaver to grab its info from
            var screensaver = ScreensaverManager.GetScreensaver(item);

            // Generate the rendered text
            string name = screensaver.ScreensaverName;
            bool flashing = screensaver.ScreensaverContainsFlashingImages;

            // Render them to the second pane
            return
                Translate.DoTranslation("Screensaver name") + $": {name}" + CharManager.NewLine +
                Translate.DoTranslation("Screensaver contains flashing images") + $": {flashing}";
            ;
        }

        /// <inheritdoc/>
        public override string GetStatusFromItem(string item) =>
            item;

        /// <inheritdoc/>
        public override string GetEntryFromItem(string item) =>
            item;

        private static void PressAndBailHelper(string saver)
        {
            ScreensaverManager.ShowSavers(saver);
            if (ScreensaverManager.inSaver)
            {
                TermReader.ReadKey();
                ScreensaverDisplayer.BailFromScreensaver();
            }
        }
    }
}
