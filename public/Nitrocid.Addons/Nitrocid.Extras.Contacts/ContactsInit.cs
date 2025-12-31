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
using Nitrocid.Extras.Contacts.Contacts;
using Nitrocid.Extras.Contacts.Contacts.Commands;
using Nitrocid.Base.Files.Extensions;
using Nitrocid.Base.Kernel.Debugging;
using Terminaux.Shell.Commands;
using System.Collections.Generic;
using Nitrocid.Base.Kernel.Extensions;
using System.Linq;
using Terminaux.Shell.Switches;
using Nitrocid.Base.Shell.Homepage;
using Nitrocid.Extras.Contacts.Settings;
using Nitrocid.Base.Kernel.Configuration;
using VisualCard.Common.Diagnostics;
using Nitrocid.Base.Kernel;
using Nitrocid.Core.Languages;

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

        public string AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasContacts);

        public string AddonTranslatedName =>
            InterAddonTranslations.GetLocalizedAddonName(KnownAddons.ExtrasContacts);

        internal static ContactsConfig ContactsConfig =>
            ConfigTools.IsCustomSettingBuiltin(nameof(ContactsConfig)) ? (ContactsConfig)Config.baseConfigurations[nameof(ContactsConfig)] : Config.GetFallbackKernelConfig<ContactsConfig>();

        public void FinalizeAddon()
        {
            // Add homepage entries
            HomepageTools.RegisterBuiltinAction(/* Localizable */ "NKS_CONTACTS_HOMEPAGE_CONTACTS", ContactsManager.OpenContactsTui);
        }

        public void StartAddon()
        {
            LanguageTools.AddCustomAction(AddonName, new("Nitrocid.Extras.Contacts.Resources.Languages.Output.Localizations", typeof(ContactsInit).Assembly));
            var config = new ContactsConfig();
            ConfigTools.RegisterBaseSetting(config);
            CommandManager.RegisterCustomCommands("Shell", [.. addonCommands]);
            ExtensionHandlerTools.extensionHandlers.AddRange(handlers);

            // Enable logging if debugging is enabled
            LoggingTools.EnableLogging = KernelEntry.DebugMode;
            if (LoggingTools.EnableLogging)
                LoggingTools.AbstractLogger = DebugWriter.debugLogger;
        }

        public void StopAddon()
        {
            // Unload all contacts
            LanguageTools.RemoveCustomAction(AddonName);
            ContactsManager.RemoveContacts(false);
            DebugWriter.WriteDebug(DebugLevel.I, "Unloaded all contacts");
            CommandManager.UnregisterCustomCommands("Shell", [.. addonCommands.Select((ci) => ci.Command)]);
            foreach (var handler in handlers)
                ExtensionHandlerTools.extensionHandlers.Remove(handler);
            HomepageTools.UnregisterBuiltinAction(/* Localizable */ "NKS_CONTACTS_HOMEPAGE_CONTACTS");
            ConfigTools.UnregisterBaseSetting(nameof(ContactsConfig));
            LoggingTools.EnableLogging = false;
        }
    }
}
