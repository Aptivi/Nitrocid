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

using Terminaux.Shell.Arguments;
using LibGit2Sharp;
using Nitrocid.Extras.GitShell.Git;
using Nitrocid.Extras.GitShell.Settings;
using Nitrocid.Kernel.Configuration;
using Terminaux.Shell.Commands;
using System.Collections.Generic;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Files.Paths;
using Terminaux.Shell.Shells;
using Nitrocid.Kernel;
using System.Linq;
using Nitrocid.Languages;

namespace Nitrocid.Extras.GitShell
{
    internal class GitShellInit : IAddon
    {
        private static bool nativeLibIsSet = false;
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("gitsh", LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_COMMAND_GITSH_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "repoPath", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_COMMAND_GITSH_ARGUMENT_REPOPATH_DESC")
                        })
                    ]),
                ], new GitCommandExec())
        ];

        public string AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasGitShell);

        public string AddonTranslatedName =>
            InterAddonTranslations.GetLocalizedAddonName(KnownAddons.ExtrasGitShell);

        internal static GitConfig GitConfig =>
            ConfigTools.IsCustomSettingBuiltin(nameof(GitConfig)) ? (GitConfig)Config.baseConfigurations[nameof(GitConfig)] : Config.GetFallbackKernelConfig<GitConfig>();

        public void FinalizeAddon()
        {
            LanguageTools.AddCustomAction(AddonName, new("Nitrocid.Extras.GitShell.Resources.Languages.Output.Localizations", typeof(GitShellInit).Assembly));
            var config = new GitConfig();
            ConfigTools.RegisterBaseSetting(config);
            ShellManager.RegisterShell("GitShell", new GitShellInfo());
            CommandManager.RegisterCustomCommands("Shell", [.. addonCommands]);
            if (!nativeLibIsSet)
            {
                nativeLibIsSet = true;
                GlobalSettings.NativeLibraryPath = PathsManagement.AddonsPath + "/Extras.GitShell/runtimes/" + KernelPlatform.GetCurrentGenericRid() + "/native/";
            }
        }

        public void StartAddon()
        { }

        public void StopAddon()
        {
            LanguageTools.RemoveCustomAction(AddonName);
            ShellManager.UnregisterShell("GitShell");
            CommandManager.UnregisterCustomCommands("Shell", [.. addonCommands.Select((ci) => ci.Command)]);
            ConfigTools.UnregisterBaseSetting(nameof(GitConfig));
        }
    }
}
