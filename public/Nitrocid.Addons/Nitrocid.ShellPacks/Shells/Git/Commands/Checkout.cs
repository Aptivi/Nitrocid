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
using Terminaux.Shell.Arguments;
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
        public override string Command => 
            "checkout";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_CHECKOUT_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "branch", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_GIT_COMMAND_CHECKOUT_ARGUMENT_BRANCH_DESC"
                    })
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            var status = GitShellCommon.Repository.RetrieveStatus();

            // Check to see if the repo has been modified
            if (status.IsDirty)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_NEEDSSAVING"), true, ThemeColorType.Error);
                return 9;
            }

            // Check for existence
            if (GitShellCommon.Repository is null)
                return 43;
            var branches = GitShellCommon.Repository.Branches;
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
            GitCommand.Checkout(GitShellCommon.Repository, branch);
            GitShellCommon.branchName = GitShellCommon.Repository.Head.CanonicalName;
            return 0;
        }

    }
}
