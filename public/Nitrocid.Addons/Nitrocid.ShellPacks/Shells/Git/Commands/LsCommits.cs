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

using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Commands;

namespace Nitrocid.ShellPacks.Shells.Git.Commands
{
    /// <summary>
    /// Lists all commits
    /// </summary>
    /// <remarks>
    /// This command lets you list all commits in your Git repository.
    /// </remarks>
    class LsCommitsCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (GitShellCommon.Repository is null)
                return 43;
            var commits = GitShellCommon.Repository.Commits;
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_LSCOMMITS_TITLE") + $" {GitShellCommon.BranchName}:");
            foreach (var commit in commits)
            {
                TextWriterColor.Write($"- {commit.Sha[..7]}, {commit.Committer.Name} <{commit.Committer.Email}>", true, ThemeColorType.ListEntry);
                TextWriterColor.Write($"  - {commit.MessageShort}", true, ThemeColorType.ListValue);
            }
            return 0;
        }

    }
}
