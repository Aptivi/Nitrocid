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
using Nitrocid.Shell.ShellBase.Commands;
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
            TextWriterColor.Write(Translate.DoTranslation("Status for branch {0}..."), GitShellCommon.BranchName);

            // Check to see if the repo has been modified
            if (!status.IsDirty)
            {
                TextWriters.Write(Translate.DoTranslation("No modifications are done."), true, KernelColorType.Success);
                return 0;
            }

            // Show all the statuses starting from untracked...
            TextWriters.Write(Translate.DoTranslation("Untracked files") + ":", true, KernelColorType.ListEntry);
            if (status.Untracked.Any())
            {
                foreach (var item in status.Untracked)
                    TextWriters.Write("  - {0}: {1}", true, KernelColorType.ListValue, item.FilePath, item.State.ToString());
            }
            else
                TextWriters.Write(Translate.DoTranslation("No untracked files."), true, KernelColorType.ListValue);
            TextWriterRaw.Write();

            // ...added...
            TextWriters.Write(Translate.DoTranslation("Added files") + ":", true, KernelColorType.ListEntry);
            if (status.Added.Any())
            {
                foreach (var item in status.Added)
                    TextWriters.Write("  - {0}: {1}", true, KernelColorType.ListValue, item.FilePath, item.State.ToString());
            }
            else
                TextWriters.Write(Translate.DoTranslation("No added files."), true, KernelColorType.ListValue);
            TextWriterRaw.Write();

            // ...modified...
            TextWriters.Write(Translate.DoTranslation("Modified files") + ":", true, KernelColorType.ListEntry);
            if (status.Modified.Any())
            {
                foreach (var item in status.Modified)
                    TextWriters.Write("  - {0}: {1}", true, KernelColorType.ListValue, item.FilePath, item.State.ToString());
            }
            else
                TextWriters.Write(Translate.DoTranslation("No modified files."), true, KernelColorType.ListValue);
            TextWriterRaw.Write();

            // ...removed...
            TextWriters.Write(Translate.DoTranslation("Removed files") + ":", true, KernelColorType.ListEntry);
            if (status.Removed.Any())
            {
                foreach (var item in status.Removed)
                    TextWriters.Write("  - {0}: {1}", true, KernelColorType.ListValue, item.FilePath, item.State.ToString());
            }
            else
                TextWriters.Write(Translate.DoTranslation("No removed files."), true, KernelColorType.ListValue);
            TextWriterRaw.Write();

            // ...staged...
            TextWriters.Write(Translate.DoTranslation("Staged files") + ":", true, KernelColorType.ListEntry);
            if (status.Staged.Any())
            {
                foreach (var item in status.Staged)
                    TextWriters.Write("  - {0}: {1}", true, KernelColorType.ListValue, item.FilePath, item.State.ToString());
            }
            else
                TextWriters.Write(Translate.DoTranslation("No staged files."), true, KernelColorType.ListValue);
            TextWriterRaw.Write();

            // ...renamed...
            TextWriters.Write(Translate.DoTranslation("Renamed staged files") + ":", true, KernelColorType.ListEntry);
            if (status.RenamedInIndex.Any())
            {
                foreach (var item in status.RenamedInIndex)
                    TextWriters.Write("  - {0}: {1}", true, KernelColorType.ListValue, item.FilePath, item.State.ToString());
            }
            else
                TextWriters.Write(Translate.DoTranslation("No renamed staged files."), true, KernelColorType.ListValue);
            TextWriterRaw.Write();

            // ...renamed unstaged...
            TextWriters.Write(Translate.DoTranslation("Renamed files") + ":", true, KernelColorType.ListEntry);
            if (status.RenamedInWorkDir.Any())
            {
                foreach (var item in status.RenamedInWorkDir)
                    TextWriters.Write("  - {0}: {1}", true, KernelColorType.ListValue, item.FilePath, item.State.ToString());
            }
            else
                TextWriters.Write(Translate.DoTranslation("No renamed files."), true, KernelColorType.ListValue);
            TextWriterRaw.Write();

            // ...and missing
            TextWriters.Write(Translate.DoTranslation("Missing files") + ":", true, KernelColorType.ListEntry);
            if (status.Missing.Any())
            {
                foreach (var item in status.Missing)
                    TextWriters.Write("  - {0}: {1}", true, KernelColorType.ListValue, item.FilePath, item.State.ToString());
            }
            else
                TextWriters.Write(Translate.DoTranslation("No missing files."), true, KernelColorType.ListValue);
            TextWriterRaw.Write();
            return 0;
        }

    }
}
