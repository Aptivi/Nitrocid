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

using Terminaux.Shell.Arguments;
using Nitrocid.Extras.Contacts.Contacts;
using Nitrocid.Extras.Contacts.Contacts.Commands;
using Nitrocid.Files.Extensions;
using Nitrocid.Kernel.Debugging;
using Terminaux.Shell.Commands;
using System.Collections.Generic;
using Nitrocid.Kernel.Extensions;
using Terminaux.Shell.Shells;
using System.Linq;
using Terminaux.Shell.Switches;
using Nitrocid.Shell.Homepage;
using Nitrocid.Extras.Contacts.Settings;
using Nitrocid.Extras.Contacts.Localized;
using Nitrocid.Kernel.Configuration;
using VisualCard.Common.Diagnostics;
using Nitrocid.Kernel;
using Nitrocid.Languages;

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
            new CommandInfo("contacts", /* Localizable */ "NKS_CONTACTS_COMMAND_CONTACTS_DESC", new ContactsCommand()),
            new CommandInfo("listcontacts", /* Localizable */ "NKS_CONTACTS_COMMAND_LISTCONTACTS_DESC", new ListContactsCommand()),
            new CommandInfo("loadcontacts", /* Localizable */ "NKS_CONTACTS_COMMAND_LOADCONTACTS_DESC", new LoadContactsCommand()),
            new CommandInfo("importcontacts", /* Localizable */ "NKS_CONTACTS_COMMAND_IMPORTCONTACTS_DESC",
                [
                    new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "mecard/path", new CommandArgumentPartOptions()
                            {
                                ArgumentDescription = /* Localizable */ "NKS_CONTACTS_COMMAND_IMPORTCONTACTS_ARGUMENT_PATH_DESC"
                            })
                        ],
                        [
                            new SwitchInfo("mecard", /* Localizable */ "NKS_CONTACTS_COMMAND_IMPORTCONTACTS_SWITCH_MECARD_DESC", new(){
                                AcceptsValues = false,
                            }),
                        ]
                    )
                ], new ImportContactsCommand()),
            new CommandInfo("contactinfo", /* Localizable */ "NKS_CONTACTS_COMMAND_CONTACTINFO_DESC",
                [
                    new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "contactNum", new()
                            {
                                IsNumeric = true,
                                ArgumentDescription = /* Localizable */ "NKS_CONTACTS_COMMAND_CONTACTINFO_ARGUMENT_CONTACTNUM_DESC"
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
            HomepageTools.RegisterBuiltinAction("NKS_CONTACTS_HOMEPAGE_CONTACTS", ContactsManager.OpenContactsTui);
        }

        void IAddon.StartAddon()
        {
            LanguageTools.AddCustomAction("Nitrocid.Extras.Contacts", new(() => LocalStrings.Languages, () => LocalStrings.Localizations, LocalStrings.Translate, LocalStrings.CheckCulture, LocalStrings.ListLanguagesCulture, LocalStrings.Exists));
            var config = new ContactsConfig();
            ConfigTools.RegisterBaseSetting(config);
            CommandManager.RegisterCustomCommands("Shell", [.. addonCommands]);
            ExtensionHandlerTools.extensionHandlers.AddRange(handlers);

            // Enable logging if debugging is enabled
            LoggingTools.AbstractLogger = DebugWriter.debugLogger;
            LoggingTools.EnableLogging = KernelEntry.DebugMode;
        }

        void IAddon.StopAddon()
        {
            // Unload all contacts
            LanguageTools.RemoveCustomAction("Nitrocid.Extras.Contacts");
            ContactsManager.RemoveContacts(false);
            DebugWriter.WriteDebug(DebugLevel.I, "Unloaded all contacts");
            CommandManager.UnregisterCustomCommands("Shell", [.. addonCommands.Select((ci) => ci.Command)]);
            foreach (var handler in handlers)
                ExtensionHandlerTools.extensionHandlers.Remove(handler);
            HomepageTools.UnregisterBuiltinAction("NKS_CONTACTS_HOMEPAGE_CONTACTS");
            ConfigTools.UnregisterBaseSetting(nameof(ContactsConfig));
            LoggingTools.EnableLogging = false;
        }
    }
}
