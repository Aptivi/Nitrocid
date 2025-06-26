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

using Nitrocid.Shell.ShellBase.Arguments;
using LibGit2Sharp;
using Nitrocid.Extras.GitShell.Git;
using Nitrocid.Extras.GitShell.Settings;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Shell.ShellBase.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Files.Paths;
using Nitrocid.Shell.ShellBase.Shells;
using Nitrocid.Kernel;
using Nitrocid.Modifications;
using System.Linq;

namespace Nitrocid.Extras.GitShell
{
    internal class GitShellInit : IAddon
    {
        private static bool nativeLibIsSet = false;
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("gitsh", /* Localizable */ "Git shell",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "repoPath", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to a directory with Git repository"
                        })
                    ]),
                ], new GitCommandExec())
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasGitShell);

        internal static GitConfig GitConfig =>
            ConfigTools.IsCustomSettingBuiltin(nameof(GitConfig)) ? (GitConfig)Config.baseConfigurations[nameof(GitConfig)] : new GitConfig();

        ReadOnlyDictionary<string, Delegate>? IAddon.PubliclyAvailableFunctions => null;

        ReadOnlyDictionary<string, PropertyInfo>? IAddon.PubliclyAvailableProperties => null;

        ReadOnlyDictionary<string, FieldInfo>? IAddon.PubliclyAvailableFields => null;

        void IAddon.FinalizeAddon()
        {
            var config = new GitConfig();
            ConfigTools.RegisterBaseSetting(config);
            ShellManager.RegisterAddonShell("GitShell", new GitShellInfo());
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. addonCommands]);
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
            ShellManager.UnregisterAddonShell("GitShell");
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. addonCommands.Select((ci) => ci.Command)]);
            ConfigTools.UnregisterBaseSetting(nameof(GitConfig));
        }
    }
}
