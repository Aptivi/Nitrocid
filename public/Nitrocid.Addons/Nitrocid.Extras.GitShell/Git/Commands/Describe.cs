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
using Nitrocid.Languages;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Extras.GitShell.Git.Commands
{
    /// <summary>
    /// Describes a commit
    /// </summary>
    /// <remarks>
    /// This command lets you describe a commit.
    /// </remarks>
    class DescribeCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "describe";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_DESCRIBE_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "commitsha", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_GIT_COMMAND_DESCRIBE_ARGUMENT_COMMITSHA_DESC"
                    })
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            if (GitShellCommon.Repository is null)
                return 43;
            string commitish = parameters.ArgumentsList[0];
            var commit = GitShellCommon.Repository.Commits.Single((c) => c.Sha.StartsWith(commitish));
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMIT_DESC") + $" {commit.Sha}:");
            TextWriterColor.Write(GitShellCommon.Repository.Describe(commit, new DescribeOptions()));
            return 0;
        }

    }
}
