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

using MailKit;
using MimeKit;
using Nitrocid.Extras.MailShell.Mail;
using Nitrocid.Extras.MailShell.Settings;
using Nitrocid.Extras.MailShell.Tools.Directory;
using Nitrocid.Extras.MailShell.Tools.Transfer;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Languages;
using Nitrocid.Modifications;
using Nitrocid.Shell.ShellBase.Arguments;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Shells;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

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

        internal static MailConfig MailConfig =>
            (MailConfig)Config.baseConfigurations[nameof(MailConfig)];

        ReadOnlyDictionary<string, Delegate>? IAddon.PubliclyAvailableFunctions => new(new Dictionary<string, Delegate>()
        {
            { nameof(MailDirectory.CreateMailDirectory), new Action<string>(MailDirectory.CreateMailDirectory) },
            { nameof(MailDirectory.DeleteMailDirectory), new Action<string>(MailDirectory.DeleteMailDirectory) },
            { nameof(MailDirectory.RenameMailDirectory), new Action<string, string>(MailDirectory.RenameMailDirectory) },
            { nameof(MailDirectory.OpenFolder), new Func<string, FolderAccess, MailFolder>(MailDirectory.OpenFolder) },
            { nameof(MailDirectory.MailListDirectories), new Func<string>(MailDirectory.MailListDirectories) },
            { nameof(MailManager.MailListMessages), new Action<int>(MailManager.MailListMessages) },
            { nameof(MailManager.MailListMessages) + "2", new Action<int, int>(MailManager.MailListMessages) },
            { nameof(MailManager.MailRemoveMessage), new Func<int, bool>(MailManager.MailRemoveMessage) },
            { nameof(MailManager.MailRemoveAllBySender), new Func<string, bool>(MailManager.MailRemoveAllBySender) },
            { nameof(MailManager.MailMoveAllBySender), new Func<string, string, bool>(MailManager.MailMoveAllBySender) },
            { nameof(MailTransfer.DecryptMessage), new Func<MimeMessage, Dictionary<string, MimeEntity>>(MailTransfer.DecryptMessage) },
            { nameof(MailTransfer.MailSendMessage), new Func<string, string, string, bool>(MailTransfer.MailSendMessage) },
            { nameof(MailTransfer.MailSendMessage) + "2", new Func<string, string, MimeEntity, bool>(MailTransfer.MailSendMessage) },
            { nameof(MailTransfer.MailSendEncryptedMessage), new Func<string, string, MimeEntity, bool>(MailTransfer.MailSendEncryptedMessage) },
            { nameof(MailTransfer.PopulateMessages), new Action(MailTransfer.PopulateMessages) },
        });

        ReadOnlyDictionary<string, PropertyInfo>? IAddon.PubliclyAvailableProperties => new(new Dictionary<string, PropertyInfo>()
        {
            { nameof(MailManager.ShowPreview), typeof(MailManager).GetProperty(nameof(MailManager.ShowPreview)) ?? throw new Exception(Translate.DoTranslation("There is no property info for") + $" {nameof(MailManager.ShowPreview)}") },
        });

        ReadOnlyDictionary<string, FieldInfo>? IAddon.PubliclyAvailableFields => null;

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
