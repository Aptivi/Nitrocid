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

using Terminaux.Shell.Commands;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Languages;
using System;
using Terminaux.Colors.Themes.Colors;
using Nitrocid.Kernel.Exceptions;
using VisualCard.Parts.Implementations;
using VisualCard.Parts.Enums;

namespace Nitrocid.Extras.Contacts.Contacts.Commands
{
    class ContactInfoCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            try
            {
                // Initiate listing process
                var contacts = ContactsManager.GetContacts();
                if (contacts.Length == 0)
                {
                    TextWriters.Write(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_NOCONTACTS"), ThemeColorType.Error);
                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.Contacts);
                }
                if (!int.TryParse(parameters.ArgumentsList[0], out int contactNum))
                {
                    TextWriters.Write(LanguageTools.GetLocalized("NKS_CONTACTS_CONTACTINFO_NUMINVALID"), ThemeColorType.Error);
                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.Contacts);
                }
                int contactIdx = contactNum - 1;
                if (contactIdx < 0 || contactIdx >= contacts.Length)
                {
                    TextWriters.Write(LanguageTools.GetLocalized("NKS_CONTACTS_CONTACTINFO_NUMOUTOFRANGE"), ThemeColorType.Error);
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
                {
                    TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_CONTACTS_TUI_CONTACTNAME") + ": ", false, ThemeColorType.ListEntry);
                    TextWriters.Write(contact.GetString(CardStringsEnum.FullName)[0].Value ?? "", ThemeColorType.ListValue);
                }
                if (hasName)
                {
                    TextWriters.Write("  - " + LanguageTools.GetLocalized("NKS_CONTACTS_CONTACTINFO_FIRSTNAME") + ": ", false, ThemeColorType.ListEntry);
                    TextWriters.Write(contact.GetPartsArray<NameInfo>()[0].ContactFirstName ?? "", ThemeColorType.ListValue);
                    TextWriters.Write("  - " + LanguageTools.GetLocalized("NKS_CONTACTS_CONTACTINFO_LASTNAME") + ": ", false, ThemeColorType.ListEntry);
                    TextWriters.Write(contact.GetPartsArray<NameInfo>()[0].ContactLastName ?? "", ThemeColorType.ListValue);
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
                    TextWriters.Write("  - " + LanguageTools.GetLocalized("NKS_CONTACTS_TUI_ADDRESS") + ": ", false, ThemeColorType.ListEntry);
                    TextWriters.Write($"{street}, {postal}, {poBox}, {extended}, {locality}, {region}, {country}", ThemeColorType.ListValue);
                }
                if (hasMail)
                {
                    TextWriters.Write("  - " + LanguageTools.GetLocalized("NKS_CONTACTS_TUI_MAIL") + ": ", false, ThemeColorType.ListEntry);
                    TextWriters.Write(contact.GetString(CardStringsEnum.Mails)[0].Value ?? "", ThemeColorType.ListValue);
                }
                if (hasOrganization)
                {
                    var org = contact.GetPartsArray<OrganizationInfo>()[0];
                    string name = org.Name ?? "";
                    string unit = org.Unit ?? "";
                    string role = org.Role ?? "";
                    TextWriters.Write("  - " + LanguageTools.GetLocalized("NKS_CONTACTS_TUI_ORG") + ": ", false, ThemeColorType.ListEntry);
                    TextWriters.Write($"{name}, {unit}, {role}", ThemeColorType.ListValue);
                }
                if (hasTelephone)
                {
                    TextWriters.Write("  - " + LanguageTools.GetLocalized("NKS_CONTACTS_TUI_PHONE") + ": ", false, ThemeColorType.ListEntry);
                    TextWriters.Write(contact.GetString(CardStringsEnum.Telephones)[0].Value ?? "", ThemeColorType.ListValue);
                }
                if (hasURL)
                {
                    TextWriters.Write("  - " + LanguageTools.GetLocalized("NKS_CONTACTS_TUI_URL") + ": ", false, ThemeColorType.ListEntry);
                    TextWriters.Write(contact.GetString(CardStringsEnum.Url)[0].Value ?? "", ThemeColorType.ListValue);
                }
                if (hasGeo)
                {
                    TextWriters.Write("  - " + LanguageTools.GetLocalized("NKS_CONTACTS_CONTACTINFO_GEO") + ": ", false, ThemeColorType.ListEntry);
                    TextWriters.Write(contact.GetString(CardStringsEnum.Geo)[0].Value ?? "", ThemeColorType.ListValue);
                }
                if (hasImpp)
                {
                    TextWriters.Write("  - " + LanguageTools.GetLocalized("NKS_CONTACTS_TUI_IMPP") + ": ", false, ThemeColorType.ListEntry);
                    TextWriters.Write(contact.GetString(CardStringsEnum.Impps)[0].Value ?? "", ThemeColorType.ListValue);
                }
                if (hasNickname)
                {
                    TextWriters.Write("  - " + LanguageTools.GetLocalized("NKS_CONTACTS_TUI_NICK") + ": ", false, ThemeColorType.ListEntry);
                    TextWriters.Write(contact.GetString(CardStringsEnum.Nicknames)[0].Value ?? "", ThemeColorType.ListValue);
                }
                if (hasRoles)
                {
                    TextWriters.Write("  - " + LanguageTools.GetLocalized("NKS_CONTACTS_TUI_ROLE") + ": ", false, ThemeColorType.ListEntry);
                    TextWriters.Write(contact.GetString(CardStringsEnum.Roles)[0].Value ?? "", ThemeColorType.ListValue);
                }
                if (hasTitles)
                {
                    TextWriters.Write("  - " + LanguageTools.GetLocalized("NKS_CONTACTS_TUI_TITLE") + ": ", false, ThemeColorType.ListEntry);
                    TextWriters.Write(contact.GetString(CardStringsEnum.Titles)[0].Value ?? "", ThemeColorType.ListValue);
                }
                if (hasNotes)
                {
                    TextWriters.Write("  - " + LanguageTools.GetLocalized("NKS_CONTACTS_CONTACTINFO_NOTE") + ": ", false, ThemeColorType.ListEntry);
                    TextWriters.Write(contact.GetString(CardStringsEnum.Notes)[0].Value ?? "", ThemeColorType.ListValue);
                }
                return 0;
            }
            catch (Exception ex)
            {
                TextWriters.Write(LanguageTools.GetLocalized("NKS_CONTACTS_CONTACTINFO_CANTLISTSOME") + ex.Message, ThemeColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Contacts);
            }
        }
    }
}
