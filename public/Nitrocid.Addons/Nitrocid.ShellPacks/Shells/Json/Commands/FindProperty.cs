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
using Nitrocid.ShellPacks.Tools;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Switches;

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

        public override int Execute(CommandParameters parameters, ref string variableValue)
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
