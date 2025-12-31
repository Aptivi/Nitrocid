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

using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.ShellPacks.Tools;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Switches;
using System;

namespace Nitrocid.ShellPacks.Shells.Json.Commands
{
    /// <summary>
    /// Adds a new object, property, or array
    /// </summary>
    /// <remarks>
    /// You can use this command to add an object, a property, or an to the end of the parent token. Note that the parent token must exist.
    /// </remarks>
    class AddCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string parent = SwitchManager.GetSwitchValue(parameters.SwitchesList, "-parentPath");
            string type = SwitchManager.GetSwitchValue(parameters.SwitchesList, "-type");
            string propName = SwitchManager.GetSwitchValue(parameters.SwitchesList, "-propName");

            try
            {
                JsonTools.Add(parent, type, propName, parameters.ArgumentsList[0]);
            }
            catch (KernelException kex)
            {
                TextWriterColor.Write(kex.Message, ThemeColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.JsonEditor);
            }
            catch (Exception ex)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_ADD_ITEM_FAILURE") + $" {ex.Message}", ThemeColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.JsonEditor);
            }
            return 0;
        }
    }
}
