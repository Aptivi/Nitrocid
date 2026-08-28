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

using System;
using Nitrocid.Extras.JsonShell.Tools;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Switches;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Extras.JsonShell.Json.Commands
{
    /// <summary>
    /// Sets a new object, property, or array
    /// </summary>
    /// <remarks>
    /// You can use this command to set an object, a property, or an to the end of the parent token. Note that the parent token must exist.
    /// </remarks>
    class SetCommand : BaseCommand, ICommand
    {
        public override string Command =>
            "set";

        public override string HelpDefinition =>
            LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_COMMAND_SET_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "jsonValue", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_JSON_COMMAND_SET_ARGUMENT_JSONVALUE_DESC"
                    })
                ],
                [
                    new SwitchInfo("parentPath", /* Localizable */ "NKS_SHELLPACKS_JSON_COMMAND_ADD_SWITCH_PARENTPATH_DESC", new SwitchOptions()
                    {
                        ArgumentsRequired = true
                    }),
                    new SwitchInfo("type", /* Localizable */ "NKS_SHELLPACKS_JSON_COMMAND_ADD_SWITCH_TYPE_DESC", new SwitchOptions()
                    {
                        ArgumentsRequired = true,
                        IsRequired = true
                    }),
                    new SwitchInfo("propName", /* Localizable */ "NKS_SHELLPACKS_JSON_COMMAND_SWITCH_PROPNAME_DESC", new SwitchOptions()
                    {
                        ArgumentsRequired = true
                    }),
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            string parent = SwitchManager.GetSwitchValue(parameters.SwitchesList, "-parentPath");
            string type = SwitchManager.GetSwitchValue(parameters.SwitchesList, "-type");
            string propName = SwitchManager.GetSwitchValue(parameters.SwitchesList, "-propName");

            try
            {
                JsonTools.Set(parent, type, propName, parameters.ArgumentsList[0]);
            }
            catch (KernelException kex)
            {
                TextWriterColor.Write(kex.Message, ThemeColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.JsonEditor);
            }
            catch (Exception ex)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_SETITEMFAILED") + $" {ex.Message}", ThemeColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.JsonEditor);
            }
            return 0;
        }
    }
}
