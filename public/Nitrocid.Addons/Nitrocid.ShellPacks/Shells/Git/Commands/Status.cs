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
using Nitrocid.Languages;
using Terminaux.Shell.Commands;
using System.Linq;

namespace Nitrocid.ShellPacks.Shells.Git.Commands
{
    /// <summary>
    /// Git repository status
    /// </summary>
    /// <remarks>
    /// This command prints a Git repository status.
    /// </remarks>
    class StatusCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var status = GitShellCommon.Repository.RetrieveStatus();
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_STATUS_TITLE"), GitShellCommon.BranchName);

            // Check to see if the repo has been modified
            if (!status.IsDirty)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_STATUS_NOCHANGES"), true, ThemeColorType.Success);
                return 0;
            }

            // Show all the statuses starting from untracked...
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_STATUS_UNTRACKED") + ":", true, ThemeColorType.ListEntry);
            if (status.Untracked.Any())
            {
                foreach (var item in status.Untracked)
                    TextWriterColor.Write("  - {0}: {1}", true, ThemeColorType.ListValue, item.FilePath, item.State.ToString());
            }
            else
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_STATUS_NOUNTRACKED"), true, ThemeColorType.ListValue);
            TextWriterRaw.Write();

            // ...added...
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_STATUS_ADDED") + ":", true, ThemeColorType.ListEntry);
            if (status.Added.Any())
            {
                foreach (var item in status.Added)
                    TextWriterColor.Write("  - {0}: {1}", true, ThemeColorType.ListValue, item.FilePath, item.State.ToString());
            }
            else
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_STATUS_NOADDED"), true, ThemeColorType.ListValue);
            TextWriterRaw.Write();

            // ...modified...
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_STATUS_MODIFIED") + ":", true, ThemeColorType.ListEntry);
            if (status.Modified.Any())
            {
                foreach (var item in status.Modified)
                    TextWriterColor.Write("  - {0}: {1}", true, ThemeColorType.ListValue, item.FilePath, item.State.ToString());
            }
            else
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_STATUS_NOMODIFIED"), true, ThemeColorType.ListValue);
            TextWriterRaw.Write();

            // ...removed...
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_STATUS_REMOVED") + ":", true, ThemeColorType.ListEntry);
            if (status.Removed.Any())
            {
                foreach (var item in status.Removed)
                    TextWriterColor.Write("  - {0}: {1}", true, ThemeColorType.ListValue, item.FilePath, item.State.ToString());
            }
            else
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_STATUS_NOREMOVED"), true, ThemeColorType.ListValue);
            TextWriterRaw.Write();

            // ...staged...
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_STATUS_STAGED") + ":", true, ThemeColorType.ListEntry);
            if (status.Staged.Any())
            {
                foreach (var item in status.Staged)
                    TextWriterColor.Write("  - {0}: {1}", true, ThemeColorType.ListValue, item.FilePath, item.State.ToString());
            }
            else
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_STATUS_NOSTAGED"), true, ThemeColorType.ListValue);
            TextWriterRaw.Write();

            // ...renamed...
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_STATUS_RENAMEDSTAGED") + ":", true, ThemeColorType.ListEntry);
            if (status.RenamedInIndex.Any())
            {
                foreach (var item in status.RenamedInIndex)
                    TextWriterColor.Write("  - {0}: {1}", true, ThemeColorType.ListValue, item.FilePath, item.State.ToString());
            }
            else
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_STATUS_NORENAMEDSTAGED"), true, ThemeColorType.ListValue);
            TextWriterRaw.Write();

            // ...renamed unstaged...
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_STATUS_RENAMED") + ":", true, ThemeColorType.ListEntry);
            if (status.RenamedInWorkDir.Any())
            {
                foreach (var item in status.RenamedInWorkDir)
                    TextWriterColor.Write("  - {0}: {1}", true, ThemeColorType.ListValue, item.FilePath, item.State.ToString());
            }
            else
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_STATUS_NORENAMED"), true, ThemeColorType.ListValue);
            TextWriterRaw.Write();

            // ...and missing
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_STATUS_MISSING") + ":", true, ThemeColorType.ListEntry);
            if (status.Missing.Any())
            {
                foreach (var item in status.Missing)
                    TextWriterColor.Write("  - {0}: {1}", true, ThemeColorType.ListValue, item.FilePath, item.State.ToString());
            }
            else
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_STATUS_NOMISSING"), true, ThemeColorType.ListValue);
            TextWriterRaw.Write();
            return 0;
        }

    }
}
