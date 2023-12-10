//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.Languages;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Switches;
using LibGit2Sharp;

namespace Nitrocid.Extras.GitShell.Git.Commands
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
                SeparatorWriterColor.WriteSeparatorKernelColor(Translate.DoTranslation("General changes in") + $" {GitShellCommon.RepoName}:", KernelColorType.ListTitle);
                foreach (var change in modified)
                    TextWriterColor.WriteKernelColor($"[M] * {change.Path}", KernelColorType.ListEntry);
                foreach (var change in added)
                    TextWriterColor.WriteKernelColor($"[A] + {change.Path}", KernelColorType.ListEntry);
                foreach (var change in deleted)
                    TextWriterColor.WriteKernelColor($"[D] - {change.Path}", KernelColorType.ListEntry);
                foreach (var change in conflicted)
                    TextWriterColor.WriteKernelColor($"[C] X {change.OldPath} vs. {change.Path}", KernelColorType.ListEntry);
                foreach (var change in renamed)
                    TextWriterColor.WriteKernelColor($"[R] / {change.OldPath} -> {change.Path}", KernelColorType.ListEntry);
            }
            TextWriterColor.Write();

            if (doPatch)
            {
                SeparatorWriterColor.WriteSeparatorKernelColor(Translate.DoTranslation("Content changes in") + $" {GitShellCommon.RepoName}:", KernelColorType.ListTitle);
                TextWriterColor.Write(patch.Content);
            }

            return 0;
        }

    }
}
