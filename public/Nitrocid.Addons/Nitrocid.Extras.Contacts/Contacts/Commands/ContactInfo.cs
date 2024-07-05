//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Languages;
using System;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Kernel.Exceptions;
using VisualCard.Parts.Implementations;

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
                    TextWriters.Write(Translate.DoTranslation("No contacts."), KernelColorType.Error);
                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.Contacts);
                }
                if (!int.TryParse(parameters.ArgumentsList[0], out int contactNum))
                {
                    TextWriters.Write(Translate.DoTranslation("Contact number is invalid."), KernelColorType.Error);
                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.Contacts);
                }
                int contactIdx = contactNum - 1;
                if (contactIdx < 0 || contactIdx >= contacts.Length)
                {
                    TextWriters.Write(Translate.DoTranslation("Contact number is out of range."), KernelColorType.Error);
                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.Contacts);
                }
                var contact = contacts[contactIdx];

                // Determine whether the contact has some parts
                bool hasName = contact.GetPartsArray<NameInfo>().Length != 0;
                bool hasFullName = contact.GetPartsArray<FullNameInfo>().Length != 0;
                bool hasAddress = contact.GetPartsArray<AddressInfo>().Length != 0;
                bool hasMail = contact.GetPartsArray<EmailInfo>().Length != 0;
                bool hasOrganization = contact.GetPartsArray<OrganizationInfo>().Length != 0;
                bool hasTelephone = contact.GetPartsArray<TelephoneInfo>().Length != 0;
                bool hasURL = contact.GetPartsArray<UrlInfo>().Length != 0;
                bool hasGeo = contact.GetPartsArray<GeoInfo>().Length != 0;
                bool hasImpp = contact.GetPartsArray<ImppInfo>().Length != 0;
                bool hasNickname = contact.GetPartsArray<NicknameInfo>().Length != 0;
                bool hasRoles = contact.GetPartsArray<RoleInfo>().Length != 0;
                bool hasTitles = contact.GetPartsArray<TitleInfo>().Length != 0;
                bool hasNotes = contact.GetPartsArray<NoteInfo>().Length > 0;

                // Print every detail
                if (hasFullName)
                {
                    TextWriters.Write("- " + Translate.DoTranslation("Contact name") + ": ", false, KernelColorType.ListEntry);
                    TextWriters.Write(contact.GetPartsArray<FullNameInfo>()[0].FullName, KernelColorType.ListValue);
                }
                if (hasName)
                {
                    TextWriters.Write("  - " + Translate.DoTranslation("First name") + ": ", false, KernelColorType.ListEntry);
                    TextWriters.Write(contact.GetPartsArray<NameInfo>()[0].ContactFirstName, KernelColorType.ListValue);
                    TextWriters.Write("  - " + Translate.DoTranslation("Last name") + ": ", false, KernelColorType.ListEntry);
                    TextWriters.Write(contact.GetPartsArray<NameInfo>()[0].ContactLastName, KernelColorType.ListValue);
                }
                if (hasAddress)
                {
                    var address = contact.GetPartsArray<AddressInfo>()[0];
                    string street = address.StreetAddress;
                    string postal = address.PostalCode;
                    string poBox = address.PostOfficeBox;
                    string extended = address.ExtendedAddress;
                    string locality = address.Locality;
                    string region = address.Region;
                    string country = address.Country;
                    TextWriters.Write("  - " + Translate.DoTranslation("Contact address") + ": ", false, KernelColorType.ListEntry);
                    TextWriters.Write($"{street}, {postal}, {poBox}, {extended}, {locality}, {region}, {country}", KernelColorType.ListValue);
                }
                if (hasMail)
                {
                    TextWriters.Write("  - " + Translate.DoTranslation("Contact mail") + ": ", false, KernelColorType.ListEntry);
                    TextWriters.Write(contact.GetPartsArray<EmailInfo>()[0].ContactEmailAddress, KernelColorType.ListValue);
                }
                if (hasOrganization)
                {
                    var org = contact.GetPartsArray<OrganizationInfo>()[0];
                    string name = org.Name;
                    string unit = org.Unit;
                    string role = org.Role;
                    TextWriters.Write("  - " + Translate.DoTranslation("Contact organization") + ": ", false, KernelColorType.ListEntry);
                    TextWriters.Write($"{name}, {unit}, {role}", KernelColorType.ListValue);
                }
                if (hasTelephone)
                {
                    TextWriters.Write("  - " + Translate.DoTranslation("Contact telephone") + ": ", false, KernelColorType.ListEntry);
                    TextWriters.Write(contact.GetPartsArray<TelephoneInfo>()[0].ContactPhoneNumber, KernelColorType.ListValue);
                }
                if (hasURL)
                {
                    TextWriters.Write("  - " + Translate.DoTranslation("Contact URL") + ": ", false, KernelColorType.ListEntry);
                    TextWriters.Write(contact.GetPartsArray<UrlInfo>()[0].Url, KernelColorType.ListValue);
                }
                if (hasGeo)
                {
                    TextWriters.Write("  - " + Translate.DoTranslation("Contact Geo") + ": ", false, KernelColorType.ListEntry);
                    TextWriters.Write(contact.GetPartsArray<GeoInfo>()[0].Geo, KernelColorType.ListValue);
                }
                if (hasImpp)
                {
                    TextWriters.Write("  - " + Translate.DoTranslation("Contact IMPP") + ": ", false, KernelColorType.ListEntry);
                    TextWriters.Write(contact.GetPartsArray<ImppInfo>()[0].ContactIMPP, KernelColorType.ListValue);
                }
                if (hasNickname)
                {
                    TextWriters.Write("  - " + Translate.DoTranslation("Contact nickname") + ": ", false, KernelColorType.ListEntry);
                    TextWriters.Write(contact.GetPartsArray<NicknameInfo>()[0].ContactNickname, KernelColorType.ListValue);
                }
                if (hasRoles)
                {
                    TextWriters.Write("  - " + Translate.DoTranslation("Contact role") + ": ", false, KernelColorType.ListEntry);
                    TextWriters.Write(contact.GetPartsArray<RoleInfo>()[0].ContactRole, KernelColorType.ListValue);
                }
                if (hasTitles)
                {
                    TextWriters.Write("  - " + Translate.DoTranslation("Contact title") + ": ", false, KernelColorType.ListEntry);
                    TextWriters.Write(contact.GetPartsArray<TitleInfo>()[0].ContactTitle, KernelColorType.ListValue);
                }
                if (hasNotes)
                {
                    TextWriters.Write("  - " + Translate.DoTranslation("Contact note") + ": ", false, KernelColorType.ListEntry);
                    TextWriters.Write(contact.GetPartsArray<NoteInfo>()[0].Note, KernelColorType.ListValue);
                }
                return 0;
            }
            catch (Exception ex)
            {
                TextWriters.Write(Translate.DoTranslation("Some of the contacts can't be listed.") + ex.Message, KernelColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Contacts);
            }
        }
    }
}
