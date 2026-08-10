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

using LibGit2Sharp;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Kernel.Time;
using Nitrocid.Base.Kernel.Time.Timezones;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

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
        public override string Command => 
            "commit";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_COMMIT_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "summary", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_GIT_COMMAND_COMMIT_ARGUMENT_SUMMARY_DESC"
                    })
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            var gitShell = (GitShell?)shell ??
                throw new KernelException(KernelExceptionType.Git, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_LASTSHELLTYPEMISMATCH"));
            if (!gitShell.isIdentified)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_NEEDSIDENTIFICATION_1") + " 'setid' " + LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_NEEDSIDENTIFICATION_2"), true, ThemeColorType.Error);
                return 15;
            }
            var author = new Signature(gitShell.name, gitShell.email, new(TimeDateTools.KernelDateTime, TimeZoneRenderers.ShowTimeZoneUtcOffsetLocal()));
            var newCommit = gitShell.Repository.Commit(parameters.ArgumentsList[0], author, author);
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMIT_SUCCESS") + $":");
            TextWriterColor.Write($"  {newCommit.Sha[..7]}: {newCommit.MessageShort}", true, ThemeColorType.ListValue);
            return 0;
        }

    }
}
