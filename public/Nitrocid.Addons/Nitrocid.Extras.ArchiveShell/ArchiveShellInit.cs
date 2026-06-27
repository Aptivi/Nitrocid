//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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

using Terminaux.Shell.Arguments;
using Nitrocid.Extras.ArchiveShell.Archive.Commands;
using Nitrocid.Extras.ArchiveShell.Archive.Shell;
using Nitrocid.Extras.ArchiveShell.Settings;
using Nitrocid.Kernel.Configuration;
using Terminaux.Shell.Commands;
using System.Collections.Generic;
using Nitrocid.Kernel.Extensions;
using Terminaux.Shell.Shells;
using System.Linq;
using Nitrocid.Languages;

namespace Nitrocid.Extras.ArchiveShell
{
    internal class ArchiveShellInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("archive", LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_COMMAND_ARCHIVE_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "archivefile", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_COMMAND_ARCHIVE_ARGUMENT_ARCHIVEFILE_DESC")
                        }),
                    ])
                ], new ArchiveCommand())
        ];

        public string AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasArchiveShell);

        public string AddonTranslatedName =>
            InterAddonTranslations.GetLocalizedAddonName(KnownAddons.ExtrasArchiveShell);

        internal static ArchiveConfig ArchiveConfig =>
            ConfigTools.IsCustomSettingBuiltin(nameof(ArchiveConfig)) ? (ArchiveConfig)Config.baseConfigurations[nameof(ArchiveConfig)] : Config.GetFallbackKernelConfig<ArchiveConfig>();

        public void FinalizeAddon()
        {
            LanguageTools.AddCustomAction(AddonName, new("Nitrocid.Extras.ArchiveShell.Resources.Languages.Output.Localizations", typeof(ArchiveShellInit).Assembly));
            var config = new ArchiveConfig();
            ConfigTools.RegisterBaseSetting(config);
            ShellManager.RegisterShell("ArchiveShell", new ArchiveShellInfo());
        }

        public void StartAddon() =>
            CommandManager.RegisterCustomCommands("Shell", [.. addonCommands]);

        public void StopAddon()
        {
            LanguageTools.RemoveCustomAction(AddonName);
            ShellManager.UnregisterShell("ArchiveShell");
            CommandManager.UnregisterCustomCommands("Shell", [.. addonCommands.Select((ci) => ci.Command)]);
            ConfigTools.UnregisterBaseSetting(nameof(ArchiveConfig));
        }
    }
}
