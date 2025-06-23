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

using GitCommand = LibGit2Sharp.Commands;
using LibGit2Sharp;
using Terminaux.Shell.Commands;
using Nitrocid.Languages;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Colors.Themes.Colors;
using Nitrocid.Kernel.Time;
using Nitrocid.Kernel.Time.Timezones;

namespace Nitrocid.ShellPacks.Shells.Git.Commands
{
    /// <summary>
    /// Pull all updates from the server
    /// </summary>
    /// <remarks>
    /// This command pulls all updates from the server.
    /// </remarks>
    class PullCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (!GitShellCommon.isIdentified)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_NEEDSIDENTIFICATION_1") + " 'setid' " + LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_NEEDSIDENTIFICATION_2"), true, ThemeColorType.Error);
                return 14;
            }
            var merger = new Signature(GitShellCommon.name, GitShellCommon.email, new(TimeDateTools.KernelDateTime, TimeZoneRenderers.ShowTimeZoneUtcOffsetLocal()));
            var pullOptions = new PullOptions();
            var pullResult = GitCommand.Pull(GitShellCommon.Repository, merger, pullOptions);
            switch (pullResult.Status)
            {
                case MergeStatus.UpToDate:
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_PULL_UPTODATE"));
                    break;
                case MergeStatus.FastForward:
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_PULL_FASTFORWARD") + $":");
                    TextWriterColor.Write($"  {pullResult.Commit.Sha[..7]}: {pullResult.Commit.MessageShort}", true, ThemeColorType.ListValue);
                    break;
                case MergeStatus.NonFastForward:
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_PULL_UPDATED") + $":");
                    TextWriterColor.Write($"  {pullResult.Commit.Sha[..7]}: {pullResult.Commit.MessageShort}", true, ThemeColorType.ListValue);
                    break;
                case MergeStatus.Conflicts:
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_PULL_MERGECONFLICTS"), true, ThemeColorType.Warning);
                    break;
            }
            return 0;
        }

    }
}
