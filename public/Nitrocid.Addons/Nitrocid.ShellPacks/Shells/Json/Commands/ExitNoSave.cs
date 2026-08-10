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

using Nitrocid.Base.Languages;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;

namespace Nitrocid.ShellPacks.Shells.Json.Commands
{
    /// <summary>
    /// Exits the JSON shell without saving changes to the JSON file if any.
    /// </summary>
    class ExitNoSaveCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "exitnosave";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_COMMAND_EXITNOSAVE_DESC");

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            ShellManager.KillShell();
            return 0;
        }
    }
}
