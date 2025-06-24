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

using System.Collections.Generic;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Screensaver;
using Terminaux.Inputs;
using Terminaux.Inputs.Interactive;
using Textify.General;

namespace Nitrocid.Base.Misc.Interactives
{
    internal class ScreensaverCli : BaseInteractiveTui<string>, IInteractiveTui<string>
    {
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
                LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SETSAVER_ARGUMENT_SAVER_DESC") + $": {name}" + CharManager.NewLine +
                LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_SCREENSAVERTUI_FLASHING") + $": {flashing}";
            ;
        }

        /// <inheritdoc/>
        public override string GetStatusFromItem(string item) =>
            item;

        /// <inheritdoc/>
        public override string GetEntryFromItem(string item) =>
            item;

        internal void PressAndBailHelper(string? saver)
        {
            if (saver is null)
                return;
            ScreensaverManager.ShowSavers(saver);
            if (ScreensaverManager.inSaver)
            {
                Input.ReadKey();
                ScreensaverDisplayer.BailFromScreensaver();
            }
        }
    }
}
