//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

#if NKS_EXTENSIONS
using Terminaux.Shell.Commands;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Themes.Colors;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Kernel.Extensions;
using Terminaux.Shell.Shells;

namespace Nitrocid.Base.Shell.Shells.Debug.Commands
{
    /// <summary>
    /// You can list all the available public properties from an addon
    /// </summary>
    /// <remarks>
    /// This command lets you list all the public properties from an addon.
    /// </remarks>
    class LsAddonPropsCommand : BaseCommand, ICommand
    {

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            string addonName = parameters.ArgumentsList[0];
            string typeName = parameters.ArgumentsList[1];
            SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_DEBUG_LSPROPERTIES_TITLE") + $" {addonName}, {typeName}", ThemeColorsTools.GetColor(ThemeColorType.ListTitle));

            // List all the available addons
            var list = InterAddonTools.ListAvailableProperties(addonName, typeName).Keys;
            ListWriterColor.WriteList(list);
            return 0;
        }

        public override int ExecuteDumb(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            string addonName = parameters.ArgumentsList[0];
            string typeName = parameters.ArgumentsList[1];
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_DEBUG_LSPROPERTIES_TITLE") + $" {addonName}, {typeName}");

            // List all the available addons
            var list = InterAddonTools.ListAvailableProperties(addonName, typeName);
            foreach (var property in list)
                TextWriterColor.Write($"  - {property.Key}");
            return 0;
        }

    }
}
#endif
