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

using System.Collections.Generic;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Switches;
using Nitrocid.ShellPacks.Shells.Git.Commands;
using Nitrocid.ShellPacks.Shells.Git.Presets;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Prompts;

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
        public override BaseCommand[] Commands =>
        [
            new BlameCommand(),
            new CheckoutCommand(),
            new CommitCommand(),
            new DescribeCommand(),
            new DiffCommand(),
            new FetchCommand(),
            new FileStatusCommand(),
            new InfoCommand(),
            new LsBranchesCommand(),
            new LsCommitsCommand(),
            new LsRemotesCommand(),
            new LsTagsCommand(),
            new MakeTagCommand(),
            new PullCommand(),
            new PushCommand(),
            new ResetCommand(),
            new SetIdCommand(),
            new StageCommand(),
            new StageAllCommand(),
            new StatusCommand(),
            new UnstageCommand(),
            new UnstageAllCommand(),
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
