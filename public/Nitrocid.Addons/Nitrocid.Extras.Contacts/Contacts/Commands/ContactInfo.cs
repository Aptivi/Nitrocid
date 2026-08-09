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

using System;
using System.Diagnostics.Metrics;
using System.Drawing;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using VisualCard.Parts.Enums;
using VisualCard.Parts.Implementations;

namespace Nitrocid.Extras.Contacts.Contacts.Commands
{
    class ContactInfoCommand : BaseCommand, ICommand
    {

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            try
            {
                // Initiate listing process
                var contacts = ContactsManager.GetContacts();
                if (contacts.Length == 0)
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_NOCONTACTS"), ThemeColorType.Error);
                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.Contacts);
                }
                if (!int.TryParse(parameters.ArgumentsList[0], out int contactNum))
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_CONTACTS_CONTACTINFO_NUMINVALID"), ThemeColorType.Error);
                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.Contacts);
                }
                int contactIdx = contactNum - 1;
                if (contactIdx < 0 || contactIdx >= contacts.Length)
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_CONTACTS_CONTACTINFO_NUMOUTOFRANGE"), ThemeColorType.Error);
                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.Contacts);
                }
                var contact = contacts[contactIdx];

                // Determine whether the contact has some parts
                bool hasName = contact.GetPartsArray<NameInfo>().Length != 0;
                bool hasFullName = contact.GetString(CardStringsEnum.FullName).Length != 0;
                bool hasAddress = contact.GetPartsArray<AddressInfo>().Length != 0;
                bool hasMail = contact.GetString(CardStringsEnum.Mails).Length != 0;
                bool hasOrganization = contact.GetPartsArray<OrganizationInfo>().Length != 0;
                bool hasTelephone = contact.GetString(CardStringsEnum.Telephones).Length != 0;
                bool hasURL = contact.GetString(CardStringsEnum.Url).Length != 0;
                bool hasGeo = contact.GetString(CardStringsEnum.Geo).Length != 0;
                bool hasImpp = contact.GetString(CardStringsEnum.Impps).Length != 0;
                bool hasNickname = contact.GetString(CardStringsEnum.Nicknames).Length != 0;
                bool hasRoles = contact.GetString(CardStringsEnum.Roles).Length != 0;
                bool hasTitles = contact.GetString(CardStringsEnum.Titles).Length != 0;
                bool hasNotes = contact.GetString(CardStringsEnum.Notes).Length > 0;

                // Print every detail
                if (hasFullName)
                    ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_CONTACTNAME"), contact.GetString(CardStringsEnum.FullName)[0].Value);
                if (hasName)
                {
                    ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_CONTACTS_CONTACTINFO_FIRSTNAME"), contact.GetPartsArray<NameInfo>()[0].ContactFirstName ?? "", indent: 1);
                    ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_CONTACTS_CONTACTINFO_LASTNAME"), contact.GetPartsArray<NameInfo>()[0].ContactLastName ?? "", indent: 1);
                }
                if (hasAddress)
                {
                    var address = contact.GetPartsArray<AddressInfo>()[0];
                    string street = address.StreetAddress ?? "";
                    string postal = address.PostalCode ?? "";
                    string poBox = address.PostOfficeBox ?? "";
                    string extended = address.ExtendedAddress ?? "";
                    string locality = address.Locality ?? "";
                    string region = address.Region ?? "";
                    string country = address.Country ?? "";
                    ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_ADDRESS"), $"{street}, {postal}, {poBox}, {extended}, {locality}, {region}, {country}", indent: 1);
                }
                if (hasMail)
                    ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_MAIL"), contact.GetString(CardStringsEnum.Mails)[0].Value ?? "", indent: 1);
                if (hasOrganization)
                {
                    var org = contact.GetPartsArray<OrganizationInfo>()[0];
                    string name = org.Name ?? "";
                    string unit = org.Unit ?? "";
                    string role = org.Role ?? "";
                    ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_ORG"), $"{name}, {unit}, {role}", indent: 1);
                }
                if (hasTelephone)
                    ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_PHONE"), contact.GetString(CardStringsEnum.Telephones)[0].Value ?? "", indent: 1);
                if (hasURL)
                    ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_URL"), contact.GetString(CardStringsEnum.Url)[0].Value ?? "", indent: 1);
                if (hasGeo)
                    ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_CONTACTS_CONTACTINFO_GEO"), contact.GetString(CardStringsEnum.Geo)[0].Value ?? "", indent: 1);
                if (hasImpp)
                    ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_IMPP"), contact.GetString(CardStringsEnum.Impps)[0].Value ?? "", indent: 1);
                if (hasNickname)
                    ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_NICK"), contact.GetString(CardStringsEnum.Nicknames)[0].Value ?? "", indent: 1);
                if (hasRoles)
                    ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_ROLE"), contact.GetString(CardStringsEnum.Roles)[0].Value ?? "", indent: 1);
                if (hasTitles)
                    ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_TITLE"), contact.GetString(CardStringsEnum.Titles)[0].Value ?? "", indent: 1);
                if (hasNotes)
                    ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_CONTACTS_CONTACTINFO_NOTE"), contact.GetString(CardStringsEnum.Notes)[0].Value ?? "", indent: 1);
                return 0;
            }
            catch (Exception ex)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_CONTACTS_CONTACTINFO_CANTLISTSOME") + ex.Message, ThemeColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Contacts);
            }
        }
    }
}
