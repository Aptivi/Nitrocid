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

using LibGit2Sharp;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;
using System.Linq;

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

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (GitShellCommon.Repository is null)
                return 43;
            string commitish = parameters.ArgumentsList[0];
            var commit = GitShellCommon.Repository.Commits.Single((c) => c.Sha.StartsWith(commitish));
            TextWriterColor.Write(Translate.DoTranslation("Description for commit") + $" {commit.Sha}:");
            TextWriterColor.Write(GitShellCommon.Repository.Describe(commit, new DescribeOptions()));
            return 0;
        }

    }
}
