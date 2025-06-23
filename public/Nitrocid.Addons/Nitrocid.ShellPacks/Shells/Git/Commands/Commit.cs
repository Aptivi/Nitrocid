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

using LibGit2Sharp;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Kernel.Time;
using Nitrocid.Kernel.Time.Timezones;
using Nitrocid.Languages;
using Terminaux.Shell.Commands;

namespace Nitrocid.ShellPacks.Shells.Git.Commands
{
    /// <summary>
    /// Makes a commit
    /// </summary>
    /// <remarks>
    /// This command makes a commit containing all the changes.
    /// </remarks>
    class CommitCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (!GitShellCommon.isIdentified)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_NEEDSIDENTIFICATION_1") + " 'setid' " + LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_NEEDSIDENTIFICATION_2"), true, ThemeColorType.Error);
                return 15;
            }
            var author = new Signature(GitShellCommon.name, GitShellCommon.email, new(TimeDateTools.KernelDateTime, TimeZoneRenderers.ShowTimeZoneUtcOffsetLocal()));
            var newCommit = GitShellCommon.Repository.Commit(parameters.ArgumentsList[0], author, author);
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMIT_SUCCESS") + $":");
            TextWriterColor.Write($"  {newCommit.Sha[..7]}: {newCommit.MessageShort}", true, ThemeColorType.ListValue);
            return 0;
        }

    }
}
