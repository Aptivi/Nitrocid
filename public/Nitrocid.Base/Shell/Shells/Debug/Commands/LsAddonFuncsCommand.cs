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
using System.Linq;
using Nitrocid.Base.Kernel.Extensions;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Base.Shell.Shells.Debug.Commands
{
    /// <summary>
    /// You can list all the available public functions from an addon
    /// </summary>
    /// <remarks>
    /// This command lets you list all the public functions from an addon.
    /// </remarks>
    class LsAddonFuncsCommand : BaseCommand, ICommand
    {
        public override string Command =>
            "lsaddonfuncs";

        public override string HelpDefinition =>
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_DEBUG_COMMAND_LSADDONFUNCS_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "addon", new CommandArgumentPartOptions()
                    {
                        AutoCompleter = (_) => AddonTools.GetAddons(),
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_DEBUG_COMMAND_LSADDONFIELDS_ARGUMENT_NAME_DESC"
                    }),
                    new CommandArgumentPart(true, "type", new CommandArgumentPartOptions()
                    {
                        AutoCompleter = (arg) => InterAddonTools.ListAvailableTypes(arg[0]).Select((type) => type.FullName ?? "").ToArray(),
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_DEBUG_COMMAND_LSADDONFIELDS_ARGUMENT_TYPE_DESC"
                    }),
                ])
            ];

        public override CommandFlags Flags =>
            CommandFlags.Wrappable | CommandFlags.RedirectionSupported;

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            string addonName = parameters.ArgumentsList[0];
            string typeName = parameters.ArgumentsList[1];
            SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_DEBUG_LSFUNCS_TITLE") + $" {addonName}, {typeName}", ThemeColorsTools.GetColor(ThemeColorType.ListTitle));

            // List all the available addons
            var list = InterAddonTools.ListAvailableFunctions(addonName, typeName).Keys;
            ListWriterColor.WriteList(list);
            return 0;
        }

        public override int ExecuteDumb(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            string addonName = parameters.ArgumentsList[0];
            string typeName = parameters.ArgumentsList[1];
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_DEBUG_LSFUNCS_TITLE") + $" {addonName}, {typeName}");

            // List all the available addons
            var list = InterAddonTools.ListAvailableFunctions(addonName, typeName);
            foreach (var function in list)
                TextWriterColor.Write($"  - {function.Key}");
            return 0;
        }

    }
}
#endif
