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
using Nitrocid.Kernel.Configuration;
using Nitrocid.Shell.ShellBase.Commands;
using System.Collections.Generic;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Shell.ShellBase.Shells;
using System.Linq;
using Nitrocid.Extras.Ssh.Settings;
using Nitrocid.Extras.Ssh.Localized;
using Nitrocid.Extras.Ssh.Commands;
using Nitrocid.Languages;

namespace Nitrocid.Extras.Ssh
{
    internal class SshInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("sshell", "NKS_SSH_COMMAND_SSHELL_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "address:port", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SSH_COMMAND_ARGUMENT_ADDRESS_DESC"
                        }),
                        new CommandArgumentPart(true, "username", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SSH_COMMAND_ARGUMENT_USERNAME_DESC"
                        }),
                    ])
                ], new SshellCommand()),

            new CommandInfo("sshcmd", "NKS_SSH_COMMAND_SSHCMD_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "address:port", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SSH_COMMAND_ARGUMENT_ADDRESS_DESC"
                        }),
                        new CommandArgumentPart(true, "username", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SSH_COMMAND_ARGUMENT_USERNAME_DESC"
                        }),
                        new CommandArgumentPart(true, "command", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SSH_COMMAND_SSHCMD_ARGUMENT_COMMAND_DESC"
                        }),
                    ])
                ], new SshcmdCommand()),
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasSsh);

        ModLoadPriority IAddon.AddonType => ModLoadPriority.Optional;

        internal static SshConfig SshConfig =>
            (SshConfig)Config.baseConfigurations[nameof(SshConfig)];

        void IAddon.FinalizeAddon()
        {
            LanguageTools.AddCustomAction("Nitrocid.Extras.Ssh", new(() => LocalStrings.Languages, () => LocalStrings.Localizations, LocalStrings.Translate, LocalStrings.CheckCulture, LocalStrings.ListLanguagesCulture, LocalStrings.Exists));
            var config = new SshConfig();
            ConfigTools.RegisterBaseSetting(config);
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. addonCommands]);
        }

        void IAddon.StartAddon()
        { }

        void IAddon.StopAddon()
        {
            LanguageTools.RemoveCustomAction("Nitrocid.Extras.Ssh");
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. addonCommands.Select((ci) => ci.Command)]);
            ConfigTools.UnregisterBaseSetting(nameof(SshConfig));
        }
    }
}
