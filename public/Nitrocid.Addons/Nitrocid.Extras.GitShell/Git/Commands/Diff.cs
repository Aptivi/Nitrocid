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
using Nitrocid.Languages;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Switches;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

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
        public override string Command => 
            "diff";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_DIFF_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new SwitchInfo("patch", /* Localizable */ "NKS_SHELLPACKS_GIT_COMMAND_DIFF_SWITCH_PATCH_DESC", new()
                    {
                        ConflictsWith = ["tree", "all"]
                    }),
                    new SwitchInfo("tree", /* Localizable */ "NKS_SHELLPACKS_GIT_COMMAND_DIFF_SWITCH_TREE_DESC", new()
                    {
                        ConflictsWith = ["patch", "all"]
                    }),
                    new SwitchInfo("all", /* Localizable */ "NKS_SHELLPACKS_GIT_COMMAND_DIFF_SWITCH_ALL_DESC", new()
                    {
                        ConflictsWith = ["tree", "patch"]
                    }),
                ])
            ];

        public override CommandFlags Flags =>
            CommandFlags.RedirectionSupported | CommandFlags.Wrappable;

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            // Get the tree changes and the patch
            if (GitShellCommon.Repository is null)
                return 43;
            var diff = GitShellCommon.Repository.Diff;
            var tree = diff.Compare<TreeChanges>();
            var patch = diff.Compare<Patch>();

            // Determine what to show
            bool doTree =
                parameters.ContainsSwitch("-tree") ||
                parameters.ContainsSwitch("-all");
            bool doPatch =
                parameters.ContainsSwitch("-patch") ||
                parameters.ContainsSwitch("-all");
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
                SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_DIFF_GENERALCHANGES") + $" {GitShellCommon.RepoName}:", ThemeColorsTools.GetColor(ThemeColorType.ListTitle));
                foreach (var change in modified)
                    TextWriterColor.Write($"[M] * {change.Path}", ThemeColorType.ListEntry);
                foreach (var change in added)
                    TextWriterColor.Write($"[A] + {change.Path}", ThemeColorType.ListEntry);
                foreach (var change in deleted)
                    TextWriterColor.Write($"[D] - {change.Path}", ThemeColorType.ListEntry);
                foreach (var change in conflicted)
                    TextWriterColor.Write($"[C] X {change.OldPath} vs. {change.Path}", ThemeColorType.ListEntry);
                foreach (var change in renamed)
                    TextWriterColor.Write($"[R] / {change.OldPath} -> {change.Path}", ThemeColorType.ListEntry);
            }
            TextWriterRaw.Write();

            if (doPatch)
            {
                SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_DIFF_CONTENTCHANGES") + $" {GitShellCommon.RepoName}:", ThemeColorsTools.GetColor(ThemeColorType.ListTitle));
                TextWriterColor.Write(patch.Content);
            }

            return 0;
        }

    }
}
