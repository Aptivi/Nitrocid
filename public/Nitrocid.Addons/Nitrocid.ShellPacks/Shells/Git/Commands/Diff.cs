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
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Shell.ShellBase.Switches;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Languages;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.ShellPacks.Shells.Git.Commands
{
    /// <summary>
    /// Shows a difference between the current commit and the local files
    /// </summary>
    /// <remarks>
    /// This command lets you see differences between the files in the current commit (HEAD) and the local files if any of them is modified.
    /// </remarks>
    class DiffCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Get the tree changes and the patch
            if (GitShellCommon.Repository is null)
                return 43;
            var diff = GitShellCommon.Repository.Diff;
            var tree = diff.Compare<TreeChanges>();
            var patch = diff.Compare<Patch>();

            // Determine what to show
            bool doTree =
                SwitchManager.ContainsSwitch(parameters.SwitchesList, "-tree") ||
                SwitchManager.ContainsSwitch(parameters.SwitchesList, "-all");
            bool doPatch =
                SwitchManager.ContainsSwitch(parameters.SwitchesList, "-patch") ||
                SwitchManager.ContainsSwitch(parameters.SwitchesList, "-all");
            if (!doTree && !doPatch)
                doTree = doPatch = true;

            // Now, show the tree difference
            if (doTree)
            {
                // Get these lists
                var modified = tree.Modified;
                var added = tree.Added;
                var deleted = tree.Deleted;
                var conflicted = tree.Conflicted;
                var renamed = tree.Renamed;

                // List the general changes
                SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_DIFF_GENERALCHANGES") + $" {GitShellCommon.RepoName}:", KernelColorTools.GetColor(KernelColorType.ListTitle));
                foreach (var change in modified)
                    TextWriters.Write($"[M] * {change.Path}", KernelColorType.ListEntry);
                foreach (var change in added)
                    TextWriters.Write($"[A] + {change.Path}", KernelColorType.ListEntry);
                foreach (var change in deleted)
                    TextWriters.Write($"[D] - {change.Path}", KernelColorType.ListEntry);
                foreach (var change in conflicted)
                    TextWriters.Write($"[C] X {change.OldPath} vs. {change.Path}", KernelColorType.ListEntry);
                foreach (var change in renamed)
                    TextWriters.Write($"[R] / {change.OldPath} -> {change.Path}", KernelColorType.ListEntry);
            }
            TextWriterRaw.Write();

            if (doPatch)
            {
                SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_DIFF_CONTENTCHANGES") + $" {GitShellCommon.RepoName}:", KernelColorTools.GetColor(KernelColorType.ListTitle));
                TextWriterColor.Write(patch.Content);
            }

            return 0;
        }

    }
}
