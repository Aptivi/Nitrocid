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

using System.Collections.Generic;
using Nitrocid.Shell.ShellBase.Arguments;
using Nitrocid.Shell.ShellBase.Switches;
using Nitrocid.Extras.GitShell.Git.Commands;
using Nitrocid.Extras.GitShell.Git.Presets;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Shells;
using Nitrocid.Shell.Prompts;

namespace Nitrocid.Extras.GitShell.Git
{
    /// <summary>
    /// Common Git shell class
    /// </summary>
    internal class GitShellInfo : BaseShellInfo<GitShell>, IShellInfo
    {
        /// <summary>
        /// Git commands
        /// </summary>
        public override List<CommandInfo> Commands =>
        [
            new CommandInfo("blame", /* Localizable */ "Fetches the list of changes in a file line by line",
                [
                    new CommandArgumentInfo(new[]
                    {
                        new CommandArgumentPart(true, "file"),
                        new CommandArgumentPart(false, "startLineNum", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(false, "endLineNum", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    })
                ], new BlameCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("checkout", /* Localizable */ "Checks out a branch",
                [
                    new CommandArgumentInfo(new[]
                    {
                        new CommandArgumentPart(true, "branch")
                    })
                ], new CheckoutCommand()),

            new CommandInfo("commit", /* Localizable */ "Makes a commit",
                [
                    new CommandArgumentInfo(new[]
                    {
                        new CommandArgumentPart(true, "summary")
                    })
                ], new CommitCommand()),

            new CommandInfo("describe", /* Localizable */ "Describes a commit",
                [
                    new CommandArgumentInfo(new[]
                    {
                        new CommandArgumentPart(true, "commitsha")
                    })
                ], new DescribeCommand()),

            new CommandInfo("diff", /* Localizable */ "Shows a difference between the current commit and the local files",
                [
                    new CommandArgumentInfo(new[]
                    {
                        new SwitchInfo("patch", /* Localizable */ "Shows a difference between the current commit and the local files by their content in a patch hunk form", new()
                        {
                            ConflictsWith = ["tree", "all"]
                        }),
                        new SwitchInfo("tree", /* Localizable */ "Shows a difference between the current commit and the local files by their existence", new()
                        {
                            ConflictsWith = ["patch", "all"]
                        }),
                        new SwitchInfo("all", /* Localizable */ "Shows a difference between the current commit and the local files by their existence and by their content", new()
                        {
                            ConflictsWith = ["tree", "patch"]
                        }),
                    })
                ], new DiffCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("fetch", /* Localizable */ "Fetches all updates from a remote",
                [
                    new CommandArgumentInfo(new[]
                    {
                        new CommandArgumentPart(false, "remote")
                    })
                ], new FetchCommand()),

            new CommandInfo("filestatus", /* Localizable */ "Fetches the file status",
                [
                    new CommandArgumentInfo(new[]
                    {
                        new CommandArgumentPart(true, "file")
                    })
                ], new FileStatusCommand()),

            new CommandInfo("info", /* Localizable */ "Gets a simple repository information", new InfoCommand()),

            new CommandInfo("lsbranches", /* Localizable */ "Lists all branches", new LsBranchesCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("lscommits", /* Localizable */ "Lists all commits", new LsCommitsCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("lsremotes", /* Localizable */ "Lists all remotes", new LsRemotesCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("lstags", /* Localizable */ "Lists all tags", new LsTagsCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("maketag", /* Localizable */ "Makes a tag from the HEAD",
                [
                    new CommandArgumentInfo(new[]
                    {
                        new CommandArgumentPart(true, "tagname"),
                        new CommandArgumentPart(false, "message"),
                    })
                ], new MakeTagCommand()),

            new CommandInfo("pull", /* Localizable */ "Pulls all updates from the server", new PullCommand()),

            new CommandInfo("push", /* Localizable */ "Pushes all updates to the server", new PushCommand()),

            new CommandInfo("reset", /* Localizable */ "Resets the local repository",
                [
                    new CommandArgumentInfo(new[]
                    {
                        new SwitchInfo("soft", /* Localizable */ "Does a soft reset", new SwitchOptions()
                        {
                            ConflictsWith = ["hard", "mixed"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("mixed", /* Localizable */ "Does a mixed reset", new SwitchOptions()
                        {
                            ConflictsWith = ["soft", "hard"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("hard", /* Localizable */ "Does a hard reset", new SwitchOptions()
                        {
                            ConflictsWith = ["soft", "mixed"],
                            AcceptsValues = false
                        }),
                    })
                ], new ResetCommand()),

            new CommandInfo("setid", /* Localizable */ "Sets your identity up",
                [
                    new CommandArgumentInfo(new[]
                    {
                        new CommandArgumentPart(true, "email"),
                        new CommandArgumentPart(true, "username"),
                    })
                ], new SetIdCommand()),

            new CommandInfo("stage", /* Localizable */ "Stages a change",
                [
                    new CommandArgumentInfo(new[]
                    {
                        new CommandArgumentPart(true, "unstagedFile")
                    })
                ], new StageCommand()),

            new CommandInfo("stageall", /* Localizable */ "Stages all changes", new StageAllCommand()),

            new CommandInfo("status", /* Localizable */ "Repository status", new StatusCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported),

            new CommandInfo("unstage", /* Localizable */ "Unstages a change",
                [
                    new CommandArgumentInfo(new[]
                    {
                        new CommandArgumentPart(true, "stagedFile")
                    })
                ], new UnstageCommand()),

            new CommandInfo("unstageall", /* Localizable */ "Unstages all changes", new UnstageAllCommand()),
        ];

        public override Dictionary<string, PromptPresetBase> ShellPresets => new()
        {
            { "Default", new DefaultPreset() },
            { "PowerLine1", new PowerLine1Preset() },
            { "PowerLine2", new PowerLine2Preset() },
            { "PowerLine3", new PowerLine3Preset() },
            { "PowerLineBG1", new PowerLineBG1Preset() },
            { "PowerLineBG2", new PowerLineBG2Preset() },
            { "PowerLineBG3", new PowerLineBG3Preset() }
        };
    }
}
