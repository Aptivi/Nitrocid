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

using System.Linq;
using LibGit2Sharp;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using GitCommand = LibGit2Sharp.Commands;

namespace Nitrocid.ShellPacks.Shells.Git.Commands
{
    /// <summary>
    /// Check out a branch
    /// </summary>
    /// <remarks>
    /// This command lets you checkout a branch in your Git repository.
    /// </remarks>
    class CheckoutCommand : BaseCommand, ICommand
    {

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            var gitShell = (GitShell?)shell ??
                throw new KernelException(KernelExceptionType.Git, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_LASTSHELLTYPEMISMATCH"));
            var status = gitShell.Repository.RetrieveStatus();

            // Check to see if the repo has been modified
            if (status.IsDirty)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_NEEDSSAVING"), true, ThemeColorType.Error);
                return 9;
            }

            // Check for existence
            if (gitShell.Repository is null)
                return 43;
            var branches = gitShell.Repository.Branches;
            var branchFriendlyNames = branches.Select((branch) => branch.FriendlyName).ToArray();
            var branchCanonNames = branches.Select((branch) => branch.CanonicalName).ToArray();
            string requestedBranch = parameters.ArgumentsList[0];
            if (!branchFriendlyNames.Contains(requestedBranch) && !branchCanonNames.Contains(requestedBranch))
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_NOBRANCH") + $" {requestedBranch}", true, ThemeColorType.Error);
                return 10;
            }

            // Now, checkout the branch.
            string canonCheckout = branchCanonNames.First((branchName) => branchName.Contains(requestedBranch));
            var branch = branches.First((branch) => branch.CanonicalName == canonCheckout);
            GitCommand.Checkout(gitShell.Repository, branch);
            gitShell.branchName = gitShell.Repository.Head.CanonicalName;
            return 0;
        }

    }
}
