//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

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
            { "blame",
                new CommandInfo("blame", /* Localizable */ "Fetches the list of changes in a file line by line",
                    new[] {
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
                    }, new BlameCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },

            { "checkout",
                new CommandInfo("checkout", /* Localizable */ "Checks out a branch",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "branch")
                        })
                    }, new CheckoutCommand())
            },

            { "commit",
                new CommandInfo("commit", /* Localizable */ "Makes a commit",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "summary")
                        })
                    }, new CommitCommand())
            },

            { "describe",
                new CommandInfo("describe", /* Localizable */ "Describes a commit",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "commitsha")
                        })
                    }, new DescribeCommand())
            },

            { "fetch",
                new CommandInfo("fetch", /* Localizable */ "Fetches all updates from a remote",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "remote")
                        })
                    }, new FetchCommand())
            },

            { "filestatus",
                new CommandInfo("filestatus", /* Localizable */ "Fetches the file status",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "file")
                        })
                    }, new FileStatusCommand())
            },
            
            { "info",
                new CommandInfo("info", /* Localizable */ "Gets a simple repository information",
                    new[] {
                        new CommandArgumentInfo()
                    }, new InfoCommand())
            },
            
            { "lsbranches",
                new CommandInfo("lsbranches", /* Localizable */ "Lists all branches",
                    new[] {
                        new CommandArgumentInfo()
                    }, new LsBranchesCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },
            
            { "lscommits",
                new CommandInfo("lscommits", /* Localizable */ "Lists all commits",
                    new[] {
                        new CommandArgumentInfo()
                    }, new LsCommitsCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },
            
            { "lsremotes",
                new CommandInfo("lsremotes", /* Localizable */ "Lists all remotes",
                    new[] {
                        new CommandArgumentInfo()
                    }, new LsRemotesCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },
            
            { "lstags",
                new CommandInfo("lstags", /* Localizable */ "Lists all tags",
                    new[] {
                        new CommandArgumentInfo()
                    }, new LsTagsCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },
            
            { "maketag",
                new CommandInfo("maketag", /* Localizable */ "Makes a tag from the HEAD",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "tagname"),
                            new CommandArgumentPart(false, "message"),
                        })
                    }, new MakeTagCommand())
            },

            { "pull",
                new CommandInfo("pull", /* Localizable */ "Pulls all updates from the server",
                    new[] {
                        new CommandArgumentInfo()
                    }, new PullCommand())
            },

            { "push",
                new CommandInfo("push", /* Localizable */ "Pushes all updates to the server",
                    new[] {
                        new CommandArgumentInfo()
                    }, new PushCommand())
            },

            { "reset",
                new CommandInfo("reset", /* Localizable */ "Resets the local repository",
                    new[] {
                        new CommandArgumentInfo(new[]
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
                    }, new ResetCommand())
            },

            { "setid",
                new CommandInfo("setid", /* Localizable */ "Sets your identity up",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "email"),
                            new CommandArgumentPart(true, "username"),
                        })
                    }, new SetIdCommand())
            },

            { "stage",
                new CommandInfo("stage", /* Localizable */ "Stages a change",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "unstagedFile")
                        })
                    }, new StageCommand())
            },
            
            { "stageall",
                new CommandInfo("stageall", /* Localizable */ "Stages all changes",
                    new[] {
                        new CommandArgumentInfo()
                    }, new StageAllCommand())
            },

            { "status",
                new CommandInfo("status", /* Localizable */ "Repository status",
                    new[] {
                        new CommandArgumentInfo()
                    }, new StatusCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported)
            },

            { "unstage",
                new CommandInfo("unstage", /* Localizable */ "Unstages a change",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "stagedFile")
                        })
                    }, new UnstageCommand())
            },

            { "unstageall",
                new CommandInfo("unstageall", /* Localizable */ "Unstages all changes",
                    new[] {
                        new CommandArgumentInfo()
                    }, new UnstageAllCommand())
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
