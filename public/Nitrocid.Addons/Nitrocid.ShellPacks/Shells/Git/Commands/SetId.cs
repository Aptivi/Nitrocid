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

namespace Nitrocid.ShellPacks.Shells.Git.Commands
{
    /// <summary>
    /// Sets your identity
    /// </summary>
    /// <remarks>
    /// This command sets your Git identity for merge, pull, and push operations.
    /// </remarks>
    class SetIdCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            GitShellCommon.email = parameters.ArgumentsList[0];
            GitShellCommon.name = parameters.ArgumentsList[1];
            GitShellCommon.isIdentified = true;
            TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_SETID_SUCCESS", "Nitrocid.ShellPacks") + $": {GitShellCommon.name} <{GitShellCommon.email}>", true, KernelColorType.Success);
            return 0;
        }

    }
}
