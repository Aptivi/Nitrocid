
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

namespace Nitrocid.Extras.GitShell.Git.Commands
{
    /// <summary>
    /// Lists all branches
    /// </summary>
    /// <remarks>
    /// This command lets you list all branches in your Git repository.
    /// </remarks>
    class Git_LsBranchesCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var branches = GitShellCommon.Repository.Branches;
            foreach (var branch in branches)
            {
                TextWriterColor.Write($"- [{(branch.IsRemote ? "R" : " ")}-{(branch.IsTracking ? "T" : " ")}-{(branch.IsCurrentRepositoryHead ? "H" : " ")}] {branch.CanonicalName} [{branch.FriendlyName}]", true, KernelColorType.ListEntry);
                TextWriterColor.Write($"  {branch.Tip.Sha[..7]}: {branch.Tip.MessageShort}", true, KernelColorType.ListValue);
            }
            return 0;
        }

    }
}
