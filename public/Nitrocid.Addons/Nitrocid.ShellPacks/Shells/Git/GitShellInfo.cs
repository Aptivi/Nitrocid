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
            new CommandInfo("blame", LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_BLAME_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_ARGUMENT_PATH_DESC", "Nitrocid.ShellPacks")
                        }),
                        new CommandArgumentPart(false, "startLineNum", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_BLAME_ARGUMENT_STARTLINENUM_DESC", "Nitrocid.ShellPacks")
                        }),
                        new CommandArgumentPart(false, "endLineNum", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_BLAME_ARGUMENT_ENDLINENUM_DESC", "Nitrocid.ShellPacks")
                        }),
                    ])
                ], new BlameCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("checkout", LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_CHECKOUT_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "branch", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_CHECKOUT_ARGUMENT_BRANCH_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new CheckoutCommand()),

            new CommandInfo("commit", LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_COMMIT_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "summary", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_COMMIT_ARGUMENT_SUMMARY_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new CommitCommand()),

            new CommandInfo("describe", LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_DESCRIBE_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "commitsha", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_DESCRIBE_ARGUMENT_COMMITSHA_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new DescribeCommand()),

            new CommandInfo("diff", LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_DIFF_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new SwitchInfo("patch", LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_DIFF_SWITCH_PATCH_DESC", "Nitrocid.ShellPacks"), new()
                        {
                            ConflictsWith = ["tree", "all"]
                        }),
                        new SwitchInfo("tree", LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_DIFF_SWITCH_TREE_DESC", "Nitrocid.ShellPacks"), new()
                        {
                            ConflictsWith = ["patch", "all"]
                        }),
                        new SwitchInfo("all", LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_DIFF_SWITCH_ALL_DESC", "Nitrocid.ShellPacks"), new()
                        {
                            ConflictsWith = ["tree", "patch"]
                        }),
                    ])
                ], new DiffCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("fetch", LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_FETCH_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "remote", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_FETCH_ARGUMENT_REMOTE_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new FetchCommand()),

            new CommandInfo("filestatus", LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_FILESTATUS_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_ARGUMENT_PATH_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new FileStatusCommand()),

            new CommandInfo("info", LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_INFO_DESC", "Nitrocid.ShellPacks"), new InfoCommand()),

            new CommandInfo("lsbranches", LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_LSBRANCHES_DESC", "Nitrocid.ShellPacks"), new LsBranchesCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("lscommits", LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_LSCOMMITS_DESC", "Nitrocid.ShellPacks"), new LsCommitsCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("lsremotes", LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_LSREMOTES_DESC", "Nitrocid.ShellPacks"), new LsRemotesCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("lstags", LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_LSTAGS_DESC", "Nitrocid.ShellPacks"), new LsTagsCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("maketag", LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_MAKETAG_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "tagname", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_MAKETAG_ARGUMENT_TAGNAME_DESC", "Nitrocid.ShellPacks")
                        }),
                        new CommandArgumentPart(false, "message", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_MAKETAG_ARGUMENT_MESSAGE_DESC", "Nitrocid.ShellPacks")
                        }),
                    ])
                ], new MakeTagCommand()),

            new CommandInfo("pull", LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_PULL_DESC", "Nitrocid.ShellPacks"), new PullCommand()),

            new CommandInfo("push", LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_PUSH_DESC", "Nitrocid.ShellPacks"), new PushCommand()),

            new CommandInfo("reset", LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_RESET_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new SwitchInfo("soft", LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_RESET_SWITCH_SOFT_DESC", "Nitrocid.ShellPacks"), new SwitchOptions()
                        {
                            ConflictsWith = ["hard", "mixed"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("mixed", LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_RESET_SWITCH_MIXED_DESC", "Nitrocid.ShellPacks"), new SwitchOptions()
                        {
                            ConflictsWith = ["soft", "hard"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("hard", LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_RESET_SWITCH_HARD_DESC", "Nitrocid.ShellPacks"), new SwitchOptions()
                        {
                            ConflictsWith = ["soft", "mixed"],
                            AcceptsValues = false
                        }),
                    ])
                ], new ResetCommand()),

            new CommandInfo("setid", LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_SETID_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "email", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_SETID_ARGUMENT_EMAIL_DESC", "Nitrocid.ShellPacks")
                        }),
                        new CommandArgumentPart(true, "username", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_SETID_ARGUMENT_USERNAME_DESC", "Nitrocid.ShellPacks")
                        }),
                    ])
                ], new SetIdCommand()),

            new CommandInfo("stage", LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_STAGE_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "unstagedFile", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_STAGE_ARGUMENT_UNSTAGED_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new StageCommand()),

            new CommandInfo("stageall", LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_STAGEALL_DESC", "Nitrocid.ShellPacks"), new StageAllCommand()),

            new CommandInfo("status", LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_STATUS_DESC", "Nitrocid.ShellPacks"), new StatusCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported),

            new CommandInfo("unstage", LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_UNSTAGE_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "stagedFile", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_UNSTAGE_ARGUMENT_STAGED_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new UnstageCommand()),

            new CommandInfo("unstageall", LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_UNSTAGEALL_DESC", "Nitrocid.ShellPacks"), new UnstageAllCommand()),
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
