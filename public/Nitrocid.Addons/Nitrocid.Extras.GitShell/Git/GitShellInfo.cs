
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
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
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
            
            { "lsbranches",
                new CommandInfo("lsbranches", ShellType, /* Localizable */ "Lists all branches",
                    new[] {
                        new CommandArgumentInfo()
                    }, new Git_LsBranchesCommand())
            },

            { "status",
                new CommandInfo("status", ShellType, /* Localizable */ "Repository status",
                    new[] {
                        new CommandArgumentInfo()
                    }, new Git_StatusCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported)
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

        // TODO: This doesn't play nice with custom presets, so we need to do something about it.
        public override PromptPresetBase CurrentPreset => ShellPresets["Default"];

    }
}
