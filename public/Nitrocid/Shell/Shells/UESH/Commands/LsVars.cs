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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Languages;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Scripting;
using Terminaux.Shell.Shells;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Lists variables
    /// </summary>
    /// <remarks>
    /// This command lists all the defined UESH variables by either the set or the setrange commands, UESH commands that define and set a variable to a value (choice, ...), a UESH script, a mod, or your system's environment variables.
    /// </remarks>
    class LsVarsCommand : BaseCommand, ICommand
    {
        public override string Command =>
            "lsvars";

        public override string HelpDefinition =>
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_LSVARS_DESC");

        public override CommandFlags Flags =>
            CommandFlags.RedirectionSupported | CommandFlags.Wrappable;

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            foreach (string VarName in MESHVariables.Variables.Keys)
            {
                TextWriters.Write($"- {VarName}: ", false, KernelColorType.ListEntry);
                TextWriters.Write(MESHVariables.Variables[VarName], true, KernelColorType.ListValue);
            }
            return 0;
        }

    }
}
