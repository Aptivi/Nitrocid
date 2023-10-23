
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

using KS.Files;
using KS.Kernel;
using KS.Kernel.Configuration;
using KS.Kernel.Extensions;
using KS.Shell.ShellBase.Arguments;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using LibGit2Sharp;
using Nitrocid.Extras.GitShell.Git;
using Nitrocid.Extras.GitShell.Settings;
using System.Collections.Generic;
using System.Linq;

namespace Nitrocid.Extras.GitShell
{
    internal class GitShellInit : IAddon
    {
        private static bool nativeLibIsSet = false;
        private readonly Dictionary<string, CommandInfo> addonCommands = new()
        {
            { "gitsh",
                new CommandInfo("gitsh", ShellType.Shell, /* Localizable */ "Git shell",
                    new[]
                    {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "repoPath")
                        }),
                    }, new GitCommandExec())
            },
        };

        string IAddon.AddonName => "Extras - Git Shell";

        AddonType IAddon.AddonType => AddonType.Optional;

        internal static GitConfig GitConfig =>
            (GitConfig)Config.baseConfigurations[nameof(GitConfig)];

        void IAddon.FinalizeAddon()
        {
            var config = new GitConfig();
            ConfigTools.RegisterBaseSetting(config);
            ShellManager.RegisterShell("GitShell", new GitShellInfo());
            CommandManager.RegisterAddonCommands(ShellType.Shell, addonCommands.Values.ToArray());
            if (!nativeLibIsSet)
            {
                nativeLibIsSet = true;
                GlobalSettings.NativeLibraryPath = Paths.AddonsPath + "/Extras.GitShell/runtimes/" + KernelPlatform.GetCurrentGenericRid() + "/native/";
            }
        }

        void IAddon.StartAddon()
        { }

        void IAddon.StopAddon()
        {
            ShellManager.UnregisterShell("GitShell");
            CommandManager.UnregisterAddonCommands(ShellType.Shell, addonCommands.Keys.ToArray());
            ConfigTools.UnregisterBaseSetting(nameof(GitConfig));
        }
    }
}
