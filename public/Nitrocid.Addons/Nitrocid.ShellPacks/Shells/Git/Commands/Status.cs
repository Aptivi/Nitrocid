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
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
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
                TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_STATUS_NOCHANGES"), true, KernelColorType.Success);
                return 0;
            }

            // Show all the statuses starting from untracked...
            TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_STATUS_UNTRACKED") + ":", true, KernelColorType.ListEntry);
            if (status.Untracked.Any())
            {
                foreach (var item in status.Untracked)
                    TextWriters.Write("  - {0}: {1}", true, KernelColorType.ListValue, item.FilePath, item.State.ToString());
            }
            else
                TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_STATUS_NOUNTRACKED"), true, KernelColorType.ListValue);
            TextWriterRaw.Write();

            // ...added...
            TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_STATUS_ADDED") + ":", true, KernelColorType.ListEntry);
            if (status.Added.Any())
            {
                foreach (var item in status.Added)
                    TextWriters.Write("  - {0}: {1}", true, KernelColorType.ListValue, item.FilePath, item.State.ToString());
            }
            else
                TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_STATUS_NOADDED"), true, KernelColorType.ListValue);
            TextWriterRaw.Write();

            // ...modified...
            TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_STATUS_MODIFIED") + ":", true, KernelColorType.ListEntry);
            if (status.Modified.Any())
            {
                foreach (var item in status.Modified)
                    TextWriters.Write("  - {0}: {1}", true, KernelColorType.ListValue, item.FilePath, item.State.ToString());
            }
            else
                TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_STATUS_NOMODIFIED"), true, KernelColorType.ListValue);
            TextWriterRaw.Write();

            // ...removed...
            TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_STATUS_REMOVED") + ":", true, KernelColorType.ListEntry);
            if (status.Removed.Any())
            {
                foreach (var item in status.Removed)
                    TextWriters.Write("  - {0}: {1}", true, KernelColorType.ListValue, item.FilePath, item.State.ToString());
            }
            else
                TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_STATUS_NOREMOVED"), true, KernelColorType.ListValue);
            TextWriterRaw.Write();

            // ...staged...
            TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_STATUS_STAGED") + ":", true, KernelColorType.ListEntry);
            if (status.Staged.Any())
            {
                foreach (var item in status.Staged)
                    TextWriters.Write("  - {0}: {1}", true, KernelColorType.ListValue, item.FilePath, item.State.ToString());
            }
            else
                TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_STATUS_NOSTAGED"), true, KernelColorType.ListValue);
            TextWriterRaw.Write();

            // ...renamed...
            TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_STATUS_RENAMEDSTAGED") + ":", true, KernelColorType.ListEntry);
            if (status.RenamedInIndex.Any())
            {
                foreach (var item in status.RenamedInIndex)
                    TextWriters.Write("  - {0}: {1}", true, KernelColorType.ListValue, item.FilePath, item.State.ToString());
            }
            else
                TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_STATUS_NORENAMEDSTAGED"), true, KernelColorType.ListValue);
            TextWriterRaw.Write();

            // ...renamed unstaged...
            TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_STATUS_RENAMED") + ":", true, KernelColorType.ListEntry);
            if (status.RenamedInWorkDir.Any())
            {
                foreach (var item in status.RenamedInWorkDir)
                    TextWriters.Write("  - {0}: {1}", true, KernelColorType.ListValue, item.FilePath, item.State.ToString());
            }
            else
                TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_STATUS_NORENAMED"), true, KernelColorType.ListValue);
            TextWriterRaw.Write();

            // ...and missing
            TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_STATUS_MISSING") + ":", true, KernelColorType.ListEntry);
            if (status.Missing.Any())
            {
                foreach (var item in status.Missing)
                    TextWriters.Write("  - {0}: {1}", true, KernelColorType.ListValue, item.FilePath, item.State.ToString());
            }
            else
                TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_STATUS_NOMISSING"), true, KernelColorType.ListValue);
            TextWriterRaw.Write();
            return 0;
        }

    }
}
