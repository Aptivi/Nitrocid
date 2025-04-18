﻿//
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

using Nitrocid.Extras.MailShell.Mail;
using Nitrocid.Extras.MailShell.Settings;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Shell.ShellBase.Arguments;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Shells;
using System.Collections.Generic;
using System.Linq;

namespace Nitrocid.Extras.MailShell
{
    internal class MailShellInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("mail", /* Localizable */ "Opens the IMAP mail client",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "emailAddress", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "E-mail address to login to"
                        }),
                    ])
                ], new MailCommandExec()),
            new CommandInfo("popmail", /* Localizable */ "Opens the POP3 mail client",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "emailAddress", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "E-mail address to login to"
                        }),
                    ])
                ], new PopMailCommandExec()),
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasMailShell);

        ModLoadPriority IAddon.AddonType => ModLoadPriority.Optional;

        internal static MailConfig MailConfig =>
            (MailConfig)Config.baseConfigurations[nameof(MailConfig)];

        void IAddon.FinalizeAddon()
        {
            var config = new MailConfig();
            ConfigTools.RegisterBaseSetting(config);
            ShellManager.RegisterAddonShell("MailShell", new MailShellInfo());
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. addonCommands]);
        }

        void IAddon.StartAddon()
        { }

        void IAddon.StopAddon()
        {
            ShellManager.UnregisterAddonShell("MailShell");
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. addonCommands.Select((ci) => ci.Command)]);
            ConfigTools.UnregisterBaseSetting(nameof(MailConfig));
        }
    }
}
