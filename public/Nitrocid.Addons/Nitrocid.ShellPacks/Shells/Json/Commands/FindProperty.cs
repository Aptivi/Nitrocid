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

using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.ShellPacks.Tools;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Switches;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.ShellPacks.Shells.Json.Commands
{
    /// <summary>
    /// Finds a property
    /// </summary>
    /// <remarks>
    /// You can use this command to search for a property in the parent property. Note that the parent property must exist.
    /// </remarks>
    class FindPropertyCommand : BaseCommand, ICommand
    {
        public override string Command =>
            "findproperty";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_COMMAND_FINDPROPERTY_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "propertyName", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_JSON_COMMAND_ARGUMENT_PROPERTYNAME_DESC"
                    })
                ],
                [
                    new SwitchInfo("parentProperty", /* Localizable */ "NKS_SHELLPACKS_JSON_COMMAND_FINDPROPERTY_SWITCH_PARENTPROPERTY_DESC", new SwitchOptions()
                    {
                        ArgumentsRequired = true
                    })
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            string parent = SwitchManager.GetSwitchValue(parameters.SwitchesList, "-parentProperty");
            var token = JsonTools.GetTokenSafe(parent, parameters.ArgumentsList[0]);
            if (token != null)
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_PROPERTY_FOUND") + $" {token.Path}");
            else
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_PROPERTY_NOTFOUND"));
            return 0;
        }
    }
}
