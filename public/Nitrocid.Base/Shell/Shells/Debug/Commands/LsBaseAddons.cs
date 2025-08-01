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

using System.Linq;
using Terminaux.Shell.Commands;
using Terminaux.Writer.ConsoleWriters;
using System;
using Terminaux.Colors.Data;
using Terminaux.Colors;
using Terminaux.Colors.Themes.Colors;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Kernel.Extensions;

namespace Nitrocid.Base.Shell.Shells.Debug.Commands
{
    /// <summary>
    /// You can list all the base addons
    /// </summary>
    /// <remarks>
    /// This command lets you list all the base addons that Nitrocid KS registered.
    /// </remarks>
    class LsBaseAddonsCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_DEBUG_LSBASEADDONS_TITLE"), ThemeColorsTools.GetColor(ThemeColorType.ListTitle));

            // List all the available addons
            foreach (var enumValue in Enum.GetValues<KnownAddons>())
            {
                // Get the localized name and the normal name
                string name = InterAddonTranslations.GetAddonName(enumValue);
                string localizedName = InterAddonTranslations.GetLocalizedAddonName(enumValue);

                // Now, check the status
                string[] addons = AddonTools.GetAddons();
                Color finalKeyColor = addons.Contains(name) ? ConsoleColors.DarkGreen : ConsoleColors.DarkRed;
                Color finalValueColor = addons.Contains(name) ? ConsoleColors.Green : ConsoleColors.Red;
                ListEntryWriterColor.WriteListEntry(enumValue.ToString(), localizedName, finalKeyColor, finalValueColor);
            }
            return 0;
        }

    }
}
