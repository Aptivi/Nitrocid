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

using KS.Files.Paths;
using KS.Kernel;
using KS.Kernel.Configuration;
using KS.Kernel.Extensions;
using KS.Shell.Prompts;
using KS.Shell.ShellBase.Arguments;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using LibGit2Sharp;
using Nitrocid.Extras.GitShell.Git;
using Nitrocid.Extras.GitShell.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Nitrocid.Extras.GitShell
{
    internal class GitShellInit : IAddon
    {
        private static bool nativeLibIsSet = false;
        private readonly Dictionary<string, CommandInfo> addonCommands = new()
        {
            { "gitsh",
                new CommandInfo("gitsh", /* Localizable */ "Git shell",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "repoPath")
                        }),
                    ], new GitCommandExec())
            },
        };

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasGitShell);

        AddonType IAddon.AddonType => AddonType.Optional;

        internal static GitConfig GitConfig =>
            (GitConfig)Config.baseConfigurations[nameof(GitConfig)];

        ReadOnlyDictionary<string, Delegate> IAddon.PubliclyAvailableFunctions => null;

        ReadOnlyDictionary<string, PropertyInfo> IAddon.PubliclyAvailableProperties => null;

        ReadOnlyDictionary<string, FieldInfo> IAddon.PubliclyAvailableFields => null;

        void IAddon.FinalizeAddon()
        {
            var config = new GitConfig();
            ConfigTools.RegisterBaseSetting(config);
            ShellManager.reservedShells.Add("GitShell");
            ShellManager.RegisterShell("GitShell", new GitShellInfo());
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. addonCommands.Values]);
            if (!nativeLibIsSet)
            {
                nativeLibIsSet = true;
                GlobalSettings.NativeLibraryPath = PathsManagement.AddonsPath + "/Extras.GitShell/runtimes/" + KernelPlatform.GetCurrentGenericRid() + "/native/";
            }
        }

        void IAddon.StartAddon()
        { }

        void IAddon.StopAddon()
        {
            ShellManager.availableShells.Remove("GitShell");
            PromptPresetManager.CurrentPresets.Remove("GitShell");
            ShellManager.reservedShells.Remove("GitShell");
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. addonCommands.Keys]);
            ConfigTools.UnregisterBaseSetting(nameof(GitConfig));
        }
    }
}
