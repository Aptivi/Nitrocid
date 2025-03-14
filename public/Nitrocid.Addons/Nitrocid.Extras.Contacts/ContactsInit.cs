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

using Nitrocid.Shell.ShellBase.Arguments;
using Nitrocid.Extras.Contacts.Contacts;
using Nitrocid.Extras.Contacts.Contacts.Commands;
using Nitrocid.Files.Extensions;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Shell.ShellBase.Commands;
using System.Collections.Generic;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Shell.ShellBase.Shells;
using System.Linq;
using Nitrocid.Shell.ShellBase.Switches;
using Nitrocid.Shell.Homepage;
using Nitrocid.Extras.Contacts.Settings;
using Nitrocid.Kernel.Configuration;
using VisualCard.Common.Diagnostics;
using Nitrocid.Kernel;

namespace Nitrocid.Extras.Contacts
{
    internal class ContactsInit : IAddon
    {
        private readonly ExtensionHandler[] handlers = [
            new(".vcf", "Contacts", ContactsHandler.Handle, ContactsHandler.InfoHandle),
            new(".vcard", "Contacts", ContactsHandler.Handle, ContactsHandler.InfoHandle),
        ];
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("contacts", /* Localizable */ "Manages your contacts", new ContactsCommand()),
            new CommandInfo("listcontacts", /* Localizable */ "Lists your contacts", new ListContactsCommand()),
            new CommandInfo("loadcontacts", /* Localizable */ "Loads your contacts", new LoadContactsCommand()),
            new CommandInfo("importcontacts", /* Localizable */ "Imports your contacts",
                [
                    new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "mecard/path", new CommandArgumentPartOptions()
                            {
                                ArgumentDescription = /* Localizable */ "Either a path to a vCard contact file or a MeCard represnetation"
                            })
                        ],
                        [
                            new SwitchInfo("mecard", /* Localizable */ "Treats the required input as MeCard string", new(){
                                AcceptsValues = false,
                            }),
                        ]
                    )
                ], new ImportContactsCommand()),
            new CommandInfo("contactinfo", /* Localizable */ "Gets contact information",
                [
                    new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "contactNum", new()
                            {
                                IsNumeric = true,
                                ArgumentDescription = /* Localizable */ "Contact number"
                            })
                        ]
                    )
                ], new ContactInfoCommand()),
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasContacts);

        ModLoadPriority IAddon.AddonType => ModLoadPriority.Optional;

        internal static ContactsConfig ContactsConfig =>
            (ContactsConfig)Config.baseConfigurations[nameof(ContactsConfig)];

        void IAddon.FinalizeAddon()
        {
            // Add homepage entries
            HomepageTools.RegisterBuiltinAction(/* Localizable */ "Contacts", ContactsManager.OpenContactsTui);
        }

        void IAddon.StartAddon()
        {
            var config = new ContactsConfig();
            ConfigTools.RegisterBaseSetting(config);
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. addonCommands]);
            ExtensionHandlerTools.extensionHandlers.AddRange(handlers);

            // Enable logging if debugging is enabled
            LoggingTools.AbstractLogger = DebugWriter.debugLogger;
            LoggingTools.EnableLogging = KernelEntry.DebugMode;
        }

        void IAddon.StopAddon()
        {
            // Unload all contacts
            ContactsManager.RemoveContacts(false);
            DebugWriter.WriteDebug(DebugLevel.I, "Unloaded all contacts");
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. addonCommands.Select((ci) => ci.Command)]);
            foreach (var handler in handlers)
                ExtensionHandlerTools.extensionHandlers.Remove(handler);
            HomepageTools.UnregisterBuiltinAction("Contacts");
            ConfigTools.UnregisterBaseSetting(nameof(ContactsConfig));
            LoggingTools.EnableLogging = false;
        }
    }
}
