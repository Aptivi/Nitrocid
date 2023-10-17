
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using KS.Shell.Prompts;
using KS.Shell.ShellBase.Arguments;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using KS.Shell.ShellBase.Switches;
using Nitrocid.Extras.GitShell.Git.Commands;
using Nitrocid.Extras.GitShell.Git.Presets;

namespace Nitrocid.Extras.GitShell.Git
{
    /// <summary>
    /// Common Git shell class
    /// </summary>
    internal class GitShellInfo : BaseShellInfo, IShellInfo
    {

        /// <summary>
        /// Git commands
        /// </summary>
        public override Dictionary<string, CommandInfo> Commands => new()
        {
            { "checkout",
                new CommandInfo("checkout", ShellType, /* Localizable */ "Checks out a branch",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "branch")
                        }, Array.Empty<SwitchInfo>())
                    }, new Git_CheckoutCommand())
            },

            { "commit",
                new CommandInfo("commit", ShellType, /* Localizable */ "Makes a commit",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "summary")
                        }, Array.Empty<SwitchInfo>())
                    }, new Git_CommitCommand())
            },

            { "fetch",
                new CommandInfo("fetch", ShellType, /* Localizable */ "Fetches all updates from a remote",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "remote")
                        }, Array.Empty<SwitchInfo>())
                    }, new Git_FetchCommand())
            },
            
            { "lsbranches",
                new CommandInfo("lsbranches", ShellType, /* Localizable */ "Lists all branches",
                    new[] {
                        new CommandArgumentInfo()
                    }, new Git_LsBranchesCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },
            
            { "lscommits",
                new CommandInfo("lscommits", ShellType, /* Localizable */ "Lists all commits",
                    new[] {
                        new CommandArgumentInfo()
                    }, new Git_LsCommitsCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },
            
            { "lsremotes",
                new CommandInfo("lsremotes", ShellType, /* Localizable */ "Lists all remotes",
                    new[] {
                        new CommandArgumentInfo()
                    }, new Git_LsRemotesCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },

            { "pull",
                new CommandInfo("pull", ShellType, /* Localizable */ "Pulls all updates from the server",
                    new[] {
                        new CommandArgumentInfo()
                    }, new Git_PullCommand())
            },

            { "push",
                new CommandInfo("push", ShellType, /* Localizable */ "Pushes all updates to the server",
                    new[] {
                        new CommandArgumentInfo()
                    }, new Git_PushCommand())
            },

            { "reset",
                new CommandInfo("reset", ShellType, /* Localizable */ "Resets the local repository",
                    new[] {
                        new CommandArgumentInfo(Array.Empty<CommandArgumentPart>(), new[]
                        {
                            new SwitchInfo("soft", /* Localizable */ "Does a soft reset", new SwitchOptions()
                            {
                                ConflictsWith = new[] { "hard", "mixed" },
                                AcceptsValues = false
                            }),
                            new SwitchInfo("mixed", /* Localizable */ "Does a mixed reset", new SwitchOptions()
                            {
                                ConflictsWith = new[] { "soft", "hard" },
                                AcceptsValues = false
                            }),
                            new SwitchInfo("hard", /* Localizable */ "Does a hard reset", new SwitchOptions()
                            {
                                ConflictsWith = new[] { "soft", "mixed" },
                                AcceptsValues = false
                            }),
                        })
                    }, new Git_ResetCommand())
            },

            { "setid",
                new CommandInfo("setid", ShellType, /* Localizable */ "Sets your identity up",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "email"),
                            new CommandArgumentPart(true, "username"),
                        }, Array.Empty<SwitchInfo>())
                    }, new Git_SetIdCommand())
            },

            { "stage",
                new CommandInfo("stage", ShellType, /* Localizable */ "Stages a change",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "unstagedFile")
                        }, Array.Empty<SwitchInfo>())
                    }, new Git_StageCommand())
            },
            
            { "stageall",
                new CommandInfo("stageall", ShellType, /* Localizable */ "Stages all changes",
                    new[] {
                        new CommandArgumentInfo()
                    }, new Git_StageAllCommand())
            },

            { "status",
                new CommandInfo("status", ShellType, /* Localizable */ "Repository status",
                    new[] {
                        new CommandArgumentInfo()
                    }, new Git_StatusCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported)
            },

            { "unstage",
                new CommandInfo("unstage", ShellType, /* Localizable */ "Unstages a change",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "stagedFile")
                        }, Array.Empty<SwitchInfo>())
                    }, new Git_UnstageCommand())
            },

            { "unstageall",
                new CommandInfo("unstageall", ShellType, /* Localizable */ "Unstages all changes",
                    new[] {
                        new CommandArgumentInfo()
                    }, new Git_UnstageAllCommand())
            },
        };

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

        public override BaseShell ShellBase => new GitShell();

        public override PromptPresetBase CurrentPreset =>
            PromptPresetManager.GetAllPresetsFromShell(ShellType)[PromptPresetManager.CurrentPresets[ShellType]];

    }
}
