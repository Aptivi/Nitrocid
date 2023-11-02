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
// along with this program.  If not, see <Mails://www.gnu.org/licenses/>.
//

using KS.Kernel.Configuration;
using KS.Kernel.Extensions;
using KS.Shell.ShellBase.Arguments;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using Nitrocid.Extras.MailShell.Mail;
using Nitrocid.Extras.MailShell.Settings;
using System.Collections.Generic;
using System.Linq;

namespace Nitrocid.Extras.MailShell
{
    internal class MailShellInit : IAddon
    {
        private readonly Dictionary<string, CommandInfo> addonCommands = new()
        {
            { "mail",
                new CommandInfo("mail", /* Localizable */ "Opens the mail client",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "emailAddress"),
                        })
                    }, new MailCommandExec())
            },
        };

        string IAddon.AddonName => "Extras - Mail Shell";

        AddonType IAddon.AddonType => AddonType.Optional;

        internal static MailConfig MailConfig =>
            (MailConfig)Config.baseConfigurations[nameof(MailConfig)];

        void IAddon.FinalizeAddon()
        {
            var config = new MailConfig();
            ConfigTools.RegisterBaseSetting(config);
            ShellManager.RegisterShell("MailShell", new MailShellInfo());
            CommandManager.RegisterAddonCommands(ShellType.Shell, addonCommands.Values.ToArray());
        }

        void IAddon.StartAddon()
        { }

        void IAddon.StopAddon()
        {
            ShellManager.UnregisterShell("MailShell");
            CommandManager.UnregisterAddonCommands(ShellType.Shell, addonCommands.Keys.ToArray());
            ConfigTools.UnregisterBaseSetting(nameof(MailConfig));
        }
    }
}
