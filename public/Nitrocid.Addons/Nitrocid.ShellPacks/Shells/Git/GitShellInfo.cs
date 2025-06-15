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
using Nitrocid.ShellPacks.Shells.Git.Commands;
using Nitrocid.ShellPacks.Shells.Git.Presets;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Shells;
using Nitrocid.Shell.Prompts;
using Nitrocid.Languages;

namespace Nitrocid.ShellPacks.Shells.Git
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
            new CommandInfo("blame", "NKS_SHELLPACKS_GIT_COMMAND_BLAME_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_GIT_COMMAND_ARGUMENT_PATH_DESC"
                        }),
                        new CommandArgumentPart(false, "startLineNum", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = "NKS_SHELLPACKS_GIT_COMMAND_BLAME_ARGUMENT_STARTLINENUM_DESC"
                        }),
                        new CommandArgumentPart(false, "endLineNum", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = "NKS_SHELLPACKS_GIT_COMMAND_BLAME_ARGUMENT_ENDLINENUM_DESC"
                        }),
                    ])
                ], new BlameCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("checkout", "NKS_SHELLPACKS_GIT_COMMAND_CHECKOUT_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "branch", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_GIT_COMMAND_CHECKOUT_ARGUMENT_BRANCH_DESC"
                        })
                    ])
                ], new CheckoutCommand()),

            new CommandInfo("commit", "NKS_SHELLPACKS_GIT_COMMAND_COMMIT_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "summary", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_GIT_COMMAND_COMMIT_ARGUMENT_SUMMARY_DESC"
                        })
                    ])
                ], new CommitCommand()),

            new CommandInfo("describe", "NKS_SHELLPACKS_GIT_COMMAND_DESCRIBE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "commitsha", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_GIT_COMMAND_DESCRIBE_ARGUMENT_COMMITSHA_DESC"
                        })
                    ])
                ], new DescribeCommand()),

            new CommandInfo("diff", "NKS_SHELLPACKS_GIT_COMMAND_DIFF_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new SwitchInfo("patch", "NKS_SHELLPACKS_GIT_COMMAND_DIFF_SWITCH_PATCH_DESC", new()
                        {
                            ConflictsWith = ["tree", "all"]
                        }),
                        new SwitchInfo("tree", "NKS_SHELLPACKS_GIT_COMMAND_DIFF_SWITCH_TREE_DESC", new()
                        {
                            ConflictsWith = ["patch", "all"]
                        }),
                        new SwitchInfo("all", "NKS_SHELLPACKS_GIT_COMMAND_DIFF_SWITCH_ALL_DESC", new()
                        {
                            ConflictsWith = ["tree", "patch"]
                        }),
                    ])
                ], new DiffCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("fetch", "NKS_SHELLPACKS_GIT_COMMAND_FETCH_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "remote", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_GIT_COMMAND_FETCH_ARGUMENT_REMOTE_DESC"
                        })
                    ])
                ], new FetchCommand()),

            new CommandInfo("filestatus", "NKS_SHELLPACKS_GIT_COMMAND_FILESTATUS_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_GIT_COMMAND_ARGUMENT_PATH_DESC"
                        })
                    ])
                ], new FileStatusCommand()),

            new CommandInfo("info", "NKS_SHELLPACKS_GIT_COMMAND_INFO_DESC", new InfoCommand()),

            new CommandInfo("lsbranches", "NKS_SHELLPACKS_GIT_COMMAND_LSBRANCHES_DESC", new LsBranchesCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("lscommits", "NKS_SHELLPACKS_GIT_COMMAND_LSCOMMITS_DESC", new LsCommitsCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("lsremotes", "NKS_SHELLPACKS_GIT_COMMAND_LSREMOTES_DESC", new LsRemotesCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("lstags", "NKS_SHELLPACKS_GIT_COMMAND_LSTAGS_DESC", new LsTagsCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("maketag", "NKS_SHELLPACKS_GIT_COMMAND_MAKETAG_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "tagname", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_GIT_COMMAND_MAKETAG_ARGUMENT_TAGNAME_DESC"
                        }),
                        new CommandArgumentPart(false, "message", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_GIT_COMMAND_MAKETAG_ARGUMENT_MESSAGE_DESC"
                        }),
                    ])
                ], new MakeTagCommand()),

            new CommandInfo("pull", "NKS_SHELLPACKS_GIT_COMMAND_PULL_DESC", new PullCommand()),

            new CommandInfo("push", "NKS_SHELLPACKS_GIT_COMMAND_PUSH_DESC", new PushCommand()),

            new CommandInfo("reset", "NKS_SHELLPACKS_GIT_COMMAND_RESET_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new SwitchInfo("soft", "NKS_SHELLPACKS_GIT_COMMAND_RESET_SWITCH_SOFT_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["hard", "mixed"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("mixed", "NKS_SHELLPACKS_GIT_COMMAND_RESET_SWITCH_MIXED_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["soft", "hard"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("hard", "NKS_SHELLPACKS_GIT_COMMAND_RESET_SWITCH_HARD_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["soft", "mixed"],
                            AcceptsValues = false
                        }),
                    ])
                ], new ResetCommand()),

            new CommandInfo("setid", "NKS_SHELLPACKS_GIT_COMMAND_SETID_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "email", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_GIT_COMMAND_SETID_ARGUMENT_EMAIL_DESC"
                        }),
                        new CommandArgumentPart(true, "username", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_GIT_COMMAND_SETID_ARGUMENT_USERNAME_DESC"
                        }),
                    ])
                ], new SetIdCommand()),

            new CommandInfo("stage", "NKS_SHELLPACKS_GIT_COMMAND_STAGE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "unstagedFile", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_GIT_COMMAND_STAGE_ARGUMENT_UNSTAGED_DESC"
                        })
                    ])
                ], new StageCommand()),

            new CommandInfo("stageall", "NKS_SHELLPACKS_GIT_COMMAND_STAGEALL_DESC", new StageAllCommand()),

            new CommandInfo("status", "NKS_SHELLPACKS_GIT_COMMAND_STATUS_DESC", new StatusCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported),

            new CommandInfo("unstage", "NKS_SHELLPACKS_GIT_COMMAND_UNSTAGE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "stagedFile", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_GIT_COMMAND_UNSTAGE_ARGUMENT_STAGED_DESC"
                        })
                    ])
                ], new UnstageCommand()),

            new CommandInfo("unstageall", "NKS_SHELLPACKS_GIT_COMMAND_UNSTAGEALL_DESC", new UnstageAllCommand()),
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
