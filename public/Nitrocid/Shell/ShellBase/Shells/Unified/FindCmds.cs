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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;

namespace Nitrocid.Shell.ShellBase.Shells.Unified
{
    /// <summary>
    /// Finds commands
    /// </summary>
    /// <remarks>
    /// This command allows you to find a list of available commands by a given command name pattern.
    /// </remarks>
    class FindCmdsUnifiedCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var commands = CommandManager.FindCommands(parameters.ArgumentsList[0], ShellManager.CurrentShellType);
            foreach (var command in commands)
            {
                TextWriters.Write("- ", false, KernelColorType.ListEntry);
                TextWriters.Write(command.Command, KernelColorType.ListValue);
            }
            if (commands.Length == 0)
                TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_FINDCMDS_NOTFOUND"), KernelColorType.Error);
            return 0;
        }

    }
}
