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

using System;
using System.Collections.Generic;
using VisualCard.Parts;
using System.Text;
using Nitrocid.Kernel.Debugging;
using Terminaux.Inputs.Interactive;
using Terminaux.Inputs.Styles.Infobox;
using Nitrocid.Languages;
using Nitrocid.Misc.Text.Probers.Regexp;
using Textify.General;
using VisualCard.Parts.Implementations;
using Terminaux.Images;
using Terminaux.Colors;
using System.IO;
using Terminaux.Base;
using Nitrocid.ConsoleBase.Colors;
using VisualCard.Parts.Enums;
using Nitrocid.Files;
using System.Linq;

namespace Nitrocid.Extras.Contacts.Contacts.Interactives
{
    /// <summary>
    /// Contacts manager class
    /// </summary>
    public class ContactsManagerCli : BaseInteractiveTui<Card>, IInteractiveTui<Card>
    {
        /// <inheritdoc/>
        public override IEnumerable<Card> PrimaryDataSource =>
            ContactsManager.GetContacts();

        /// <inheritdoc/>
        public override bool AcceptsEmptyData =>
            true;

        /// <inheritdoc/>
        public override string GetInfoFromItem(Card item)
        {
            // Get some info from the contact
            Card selectedContact = item;
            if (selectedContact is null)
                return LanguageTools.GetLocalized("NKS_CONTACTS_TUI_NOCONTACT", "Nitrocid.Extras.Contacts");

            // Generate the rendered text
            string finalRenderedContactPicture =
                ContactsInit.ContactsConfig.ShowImages ?
                GetContactPictureFinal(selectedContact, (ConsoleWrapper.WindowWidth / 2) - 4, ConsoleWrapper.WindowHeight / 2, KernelColorTools.GetColor(KernelColorType.TuiBackground)) :
                "";
            string finalRenderedContactName = GetContactNameFinal(selectedContact);
            string finalRenderedContactAddress = GetContactAddressFinal(selectedContact);
            string finalRenderedContactMail = GetContactMailFinal(selectedContact);
            string finalRenderedContactOrganization = GetContactOrganizationFinal(selectedContact);
            string finalRenderedContactTelephone = GetContactTelephoneFinal(selectedContact);
            string finalRenderedContactURL = GetContactURLFinal(selectedContact);

            // Render them to the second pane
            return
                (!string.IsNullOrEmpty(finalRenderedContactPicture) ? finalRenderedContactPicture + CharManager.NewLine : "") +
                finalRenderedContactName + CharManager.NewLine +
                finalRenderedContactAddress + CharManager.NewLine +
                finalRenderedContactMail + CharManager.NewLine +
                finalRenderedContactOrganization + CharManager.NewLine +
                finalRenderedContactTelephone + CharManager.NewLine +
                finalRenderedContactURL
            ;
        }

        /// <inheritdoc/>
        public override string GetStatusFromItem(Card item)
        {
            // Get some info from the contact
            Card selectedContact = item;
            if (selectedContact is null)
                return "";

            // Generate the rendered text
            string finalRenderedContactName = GetContactNameFinal(selectedContact);

            // Render them to the status
            return finalRenderedContactName;
        }

        /// <inheritdoc/>
        public override string GetEntryFromItem(Card item)
        {
            Card contact = item;
            if (contact is null)
                return "";
            return contact.GetString(CardStringsEnum.FullName)[0].Value ?? "";
        }

        internal void RemoveContact(int index) =>
            ContactsManager.RemoveContact(index);

        internal void RemoveContacts() =>
            ContactsManager.RemoveContacts();

        internal void ImportContacts()
        {
            try
            {
                // Initiate import process
                ContactsManager.ImportContacts();
            }
            catch (Exception ex)
            {
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_CANTIMPORTCONTACTS", "Nitrocid.Extras.Contacts") + ex.Message, Settings.InfoBoxSettings);
            }
        }

        internal void ImportContactsFrom()
        {
            // Now, render the search box
            string path = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_PATHVCFPROMPT", "Nitrocid.Extras.Contacts"), Settings.InfoBoxSettings);
            if (FilesystemTools.FileExists(path))
            {
                try
                {
                    // Initiate installation
                    ContactsManager.InstallContacts(path);
                }
                catch
                {
                    InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_CONTACTS_CONTACTFILEINVALID_UNNAMED", "Nitrocid.Extras.Contacts"), Settings.InfoBoxSettings);
                }
            }
            else
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_FILENOTFOUND", "Nitrocid.Extras.Contacts"), Settings.InfoBoxSettings);
        }

        internal void ImportContactFromMeCard()
        {
            // Now, render the search box
            string meCard = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_MECARDPROMPT", "Nitrocid.Extras.Contacts"), Settings.InfoBoxSettings);
            if (!string.IsNullOrEmpty(meCard))
            {
                try
                {
                    // Initiate installation
                    ContactsManager.InstallContactFromMeCard(meCard);
                }
                catch
                {
                    InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_MECARDINVALID", "Nitrocid.Extras.Contacts"), Settings.InfoBoxSettings);
                }
            }
            else
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_MECARDEMPTY", "Nitrocid.Extras.Contacts"), Settings.InfoBoxSettings);
        }

        internal void ShowContactInfo(int index)
        {
            // Render the final information string
            var finalInfoRendered = new StringBuilder();
            string finalRenderedContactName = GetContactNameFinal(index);
            finalInfoRendered.AppendLine(finalRenderedContactName);
            string finalRenderedContactAddress = GetContactAddressFinal(index);
            finalInfoRendered.AppendLine(finalRenderedContactAddress);
            string finalRenderedContactMail = GetContactMailFinal(index);
            finalInfoRendered.AppendLine(finalRenderedContactMail);
            string finalRenderedContactOrganization = GetContactOrganizationFinal(index);
            finalInfoRendered.AppendLine(finalRenderedContactOrganization);
            string finalRenderedContactTelephone = GetContactTelephoneFinal(index);
            finalInfoRendered.AppendLine(finalRenderedContactTelephone);
            string finalRenderedContactURL = GetContactURLFinal(index);
            finalInfoRendered.AppendLine(finalRenderedContactURL);
            string finalRenderedContactGeo = GetContactGeoFinal(index);
            finalInfoRendered.AppendLine(finalRenderedContactGeo);
            string finalRenderedContactImpps = GetContactImppFinal(index);
            finalInfoRendered.AppendLine(finalRenderedContactImpps);
            string finalRenderedContactNicknames = GetContactNicknameFinal(index);
            finalInfoRendered.AppendLine(finalRenderedContactNicknames);
            string finalRenderedContactRoles = GetContactRoleFinal(index);
            finalInfoRendered.AppendLine(finalRenderedContactRoles);
            string finalRenderedContactTitles = GetContactTitleFinal(index);
            finalInfoRendered.AppendLine(finalRenderedContactTitles);
            string finalRenderedContactNotes = GetContactNotesFinal(index);
            finalInfoRendered.AppendLine(finalRenderedContactNotes);

            // If there is a profile picture and preview is enabled, print it
            string picture =
                ContactsInit.ContactsConfig.ShowImages ?
                GetContactPictureFinal(index, ConsoleWrapper.WindowWidth - 8, ConsoleWrapper.WindowHeight, KernelColorTools.GetColor(KernelColorType.TuiBoxBackground)) :
                "";
            if (!string.IsNullOrEmpty(picture))
            {
                finalInfoRendered.AppendLine("\n");
                finalInfoRendered.AppendLine(picture);
                finalInfoRendered.Append(ColorTools.RenderSetConsoleColor(KernelColorTools.GetColor(KernelColorType.TuiBoxBackground), true));
            }

            // Now, render the info box
            InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
        }

        internal void ShowContactRawInfo(int index)
        {
            // Render the final information string
            var finalInfoRendered = new StringBuilder();
            var card = ContactsManager.GetContact(index);

            string finalRenderedContactVcardInfo = card.SaveToString();
            finalInfoRendered.Append(finalRenderedContactVcardInfo);

            // Now, render the info box
            InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
        }

        internal void SearchBox()
        {
            // Now, render the search box
            string exp = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_SEARCHPROMPT", "Nitrocid.Extras.Contacts"), Settings.InfoBoxSettings);
            if (RegexpTools.IsValidRegex(exp))
            {
                // Initiate the search
                var foundCard = ContactsManager.SearchNext(exp);
                if (foundCard is null)
                    InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_NOCONTACTSSEARCH", "Nitrocid.Extras.Contacts"), Settings.InfoBoxSettings);
                UpdateIndex(foundCard);
            }
            else
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_CONTACTS_EXCEPTION_INVALIDREGEX", "Nitrocid.Extras.Contacts"), Settings.InfoBoxSettings);
        }

        internal void SearchNext()
        {
            // Initiate the search
            var foundCard = ContactsManager.SearchNext();
            UpdateIndex(foundCard);
        }

        internal void SearchPrevious()
        {
            // Initiate the search
            var foundCard = ContactsManager.SearchPrevious();
            UpdateIndex(foundCard);
        }

        internal void UpdateIndex(Card? foundCard)
        {
            var contacts = ContactsManager.GetContacts();
            if (foundCard is not null)
            {
                // Get the index from the instance
                int idx = Array.FindIndex(contacts, (card) => card == foundCard);
                DebugCheck.Assert(idx != -1, "contact index is -1!!!");
                InteractiveTuiTools.SelectionMovement(this, idx + 1);
            }
        }

        internal string GetContactNameFinal(int index)
        {
            // Render the final information string
            var card = ContactsManager.GetContact(index);
            return GetContactNameFinal(card);
        }

        internal string GetContactNameFinal(Card card)
        {
            // Render the final information string
            var finalInfoRendered = new StringBuilder();
            bool hasName = card.GetPartsArray<NameInfo>().Length != 0;

            if (hasName)
                finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_CONTACTNAME", "Nitrocid.Extras.Contacts") + $": {card.GetString(CardStringsEnum.FullName)[0].Value}");
            else
                finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_NOCONTACTNAME", "Nitrocid.Extras.Contacts"));

            // Now, return the value
            return finalInfoRendered.ToString();
        }

        internal string GetContactAddressFinal(int index)
        {
            // Render the final information string
            var card = ContactsManager.GetContact(index);
            return GetContactAddressFinal(card);
        }

        internal string GetContactAddressFinal(Card card)
        {
            // Render the final information string
            var finalInfoRendered = new StringBuilder();
            bool hasAddress = card.GetPartsArray<AddressInfo>().Length != 0;

            if (hasAddress)
            {
                finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_ADDRESS", "Nitrocid.Extras.Contacts") + ": ");

                var address = card.GetPartsArray<AddressInfo>()[0];
                List<string> fullElements = [];
                string street = address.StreetAddress ?? "";
                string postal = address.PostalCode ?? "";
                string poBox = address.PostOfficeBox ?? "";
                string extended = address.ExtendedAddress ?? "";
                string locality = address.Locality ?? "";
                string region = address.Region ?? "";
                string country = address.Country ?? "";
                if (!string.IsNullOrEmpty(street))
                    fullElements.Add(street);
                if (!string.IsNullOrEmpty(postal))
                    fullElements.Add(postal);
                if (!string.IsNullOrEmpty(poBox))
                    fullElements.Add(poBox);
                if (!string.IsNullOrEmpty(extended))
                    fullElements.Add(extended);
                if (!string.IsNullOrEmpty(locality))
                    fullElements.Add(locality);
                if (!string.IsNullOrEmpty(region))
                    fullElements.Add(region);
                if (!string.IsNullOrEmpty(country))
                    fullElements.Add(country);
                finalInfoRendered.Append(string.Join(", ", fullElements));
            }
            else
                finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_NOCONTACTNAME", "Nitrocid.Extras.Contacts"));

            // Now, return the value
            return finalInfoRendered.ToString();
        }

        internal string GetContactMailFinal(int index)
        {
            // Render the final information string
            var card = ContactsManager.GetContact(index);
            return GetContactMailFinal(card);
        }

        internal string GetContactMailFinal(Card card)
        {
            // Render the final information string
            var finalInfoRendered = new StringBuilder();
            bool hasMail = card.GetString(CardStringsEnum.Mails).Length != 0;

            if (hasMail)
                finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_MAIL", "Nitrocid.Extras.Contacts") + $": {card.GetString(CardStringsEnum.Mails)[0].Value}");
            else
                finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_NOMAIL", "Nitrocid.Extras.Contacts"));

            // Now, return the value
            return finalInfoRendered.ToString();
        }

        internal string GetContactOrganizationFinal(int index)
        {
            // Render the final information string
            var card = ContactsManager.GetContact(index);
            return GetContactOrganizationFinal(card);
        }

        internal string GetContactOrganizationFinal(Card card)
        {
            // Render the final information string
            var finalInfoRendered = new StringBuilder();
            bool hasOrganization = card.GetPartsArray<OrganizationInfo>().Length != 0;

            if (hasOrganization)
            {
                finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_ORG", "Nitrocid.Extras.Contacts") + ": ");

                var org = card.GetPartsArray<OrganizationInfo>()[0];
                List<string> fullElements = [];
                string name = org.Name ?? "";
                string unit = org.Unit ?? "";
                string role = org.Role ?? "";
                if (!string.IsNullOrEmpty(name))
                    fullElements.Add(name);
                if (!string.IsNullOrEmpty(unit))
                    fullElements.Add(unit);
                if (!string.IsNullOrEmpty(role))
                    fullElements.Add(role);
                finalInfoRendered.Append(string.Join(", ", fullElements));
            }
            else
                finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_NOORG", "Nitrocid.Extras.Contacts"));

            // Now, return the value
            return finalInfoRendered.ToString();
        }

        internal string GetContactTelephoneFinal(int index)
        {
            // Render the final information string
            var card = ContactsManager.GetContact(index);
            return GetContactTelephoneFinal(card);
        }

        internal string GetContactTelephoneFinal(Card card)
        {
            // Render the final information string
            var finalInfoRendered = new StringBuilder();
            bool hasTelephone = card.GetString(CardStringsEnum.Telephones).Length != 0;

            if (hasTelephone)
                finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_PHONE", "Nitrocid.Extras.Contacts") + $": {card.GetString(CardStringsEnum.Telephones)[0].Value}");
            else
                finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_NOPHONE", "Nitrocid.Extras.Contacts"));

            // Now, return the value
            return finalInfoRendered.ToString();
        }

        internal string GetContactURLFinal(int index)
        {
            // Render the final information string
            var card = ContactsManager.GetContact(index);
            return GetContactURLFinal(card);
        }

        internal string GetContactURLFinal(Card card)
        {
            // Render the final information string
            var finalInfoRendered = new StringBuilder();
            bool hasURL = card.GetString(CardStringsEnum.Url).Length != 0;

            if (hasURL)
                finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_URL", "Nitrocid.Extras.Contacts") + $": {card.GetString(CardStringsEnum.Url)[0].Value}");
            else
                finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_NOURL", "Nitrocid.Extras.Contacts"));

            // Now, return the value
            return finalInfoRendered.ToString();
        }

        internal string GetContactGeoFinal(int index)
        {
            // Render the final information string
            var card = ContactsManager.GetContact(index);
            return GetContactGeoFinal(card);
        }

        internal string GetContactGeoFinal(Card card)
        {
            // Render the final information string
            var finalInfoRendered = new StringBuilder();
            bool hasGeo = card.GetString(CardStringsEnum.Geo).Length != 0;

            if (hasGeo)
                finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_GEO", "Nitrocid.Extras.Contacts") + $": {card.GetString(CardStringsEnum.Geo)[0].Value}");
            else
                finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_NOGEO", "Nitrocid.Extras.Contacts"));

            // Now, return the value
            return finalInfoRendered.ToString();
        }

        internal string GetContactImppFinal(int index)
        {
            // Render the final information string
            var card = ContactsManager.GetContact(index);
            return GetContactImppFinal(card);
        }

        internal string GetContactImppFinal(Card card)
        {
            // Render the final information string
            var finalInfoRendered = new StringBuilder();
            bool hasImpp = card.GetString(CardStringsEnum.Impps).Length != 0;

            if (hasImpp)
                finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_IMPP", "Nitrocid.Extras.Contacts") + $": {card.GetString(CardStringsEnum.Impps)[0].Value}");
            else
                finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_NOIMPP", "Nitrocid.Extras.Contacts"));

            // Now, return the value
            return finalInfoRendered.ToString();
        }

        internal string GetContactNicknameFinal(int index)
        {
            // Render the final information string
            var card = ContactsManager.GetContact(index);
            return GetContactNicknameFinal(card);
        }

        internal string GetContactNicknameFinal(Card card)
        {
            // Render the final information string
            var finalInfoRendered = new StringBuilder();
            bool hasNickname = card.GetString(CardStringsEnum.Nicknames).Length != 0;

            if (hasNickname)
                finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_NICK", "Nitrocid.Extras.Contacts") + $": {card.GetString(CardStringsEnum.Nicknames)[0].Value}");
            else
                finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_NONICK", "Nitrocid.Extras.Contacts"));

            // Now, return the value
            return finalInfoRendered.ToString();
        }

        internal string GetContactRoleFinal(int index)
        {
            // Render the final information string
            var card = ContactsManager.GetContact(index);
            return GetContactRoleFinal(card);
        }

        internal string GetContactRoleFinal(Card card)
        {
            // Render the final information string
            var finalInfoRendered = new StringBuilder();
            bool hasRoles = card.GetString(CardStringsEnum.Roles).Length != 0;

            if (hasRoles)
                finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_ROLE", "Nitrocid.Extras.Contacts") + $": {card.GetString(CardStringsEnum.Roles)[0].Value}");
            else
                finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_NOROLE", "Nitrocid.Extras.Contacts"));

            // Now, return the value
            return finalInfoRendered.ToString();
        }

        internal string GetContactTitleFinal(int index)
        {
            // Render the final information string
            var card = ContactsManager.GetContact(index);
            return GetContactTitleFinal(card);
        }

        internal string GetContactTitleFinal(Card card)
        {
            // Render the final information string
            var finalInfoRendered = new StringBuilder();
            bool hasTitles = card.GetString(CardStringsEnum.Titles).Length != 0;

            if (hasTitles)
                finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_TITLE", "Nitrocid.Extras.Contacts") + $": {card.GetString(CardStringsEnum.Titles)[0].Value}");
            else
                finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_NOTITLE", "Nitrocid.Extras.Contacts"));

            // Now, return the value
            return finalInfoRendered.ToString();
        }

        internal string GetContactNotesFinal(int index)
        {
            // Render the final information string
            var card = ContactsManager.GetContact(index);
            return GetContactNotesFinal(card);
        }

        internal string GetContactNotesFinal(Card card)
        {
            // Render the final information string
            var finalInfoRendered = new StringBuilder();
            bool hasNotes = card.GetString(CardStringsEnum.Notes).Length > 0;

            if (hasNotes)
                finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_NOTES", "Nitrocid.Extras.Contacts") + $": {card.GetString(CardStringsEnum.Notes)[0].Value}");
            else
                finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_NONOTES", "Nitrocid.Extras.Contacts"));

            // Now, return the value
            return finalInfoRendered.ToString();
        }

        internal string GetContactPictureFinal(int index, int width, int height, Color? background = null)
        {
            // Render the final information string
            var card = ContactsManager.GetContact(index);
            return GetContactPictureFinal(card, width, height, background);
        }

        internal string GetContactPictureFinal(Card card, int width, int height, Color? background = null)
        {
            // Render the final information string
            var finalInfoRendered = new StringBuilder();
            bool hasPicture = card.GetPartsArray<PhotoInfo>().Length > 0;

            if (hasPicture)
            {
                Stream imageStream = card.GetPartsArray<PhotoInfo>()[0].GetStream();
                string finalPicture = ImageProcessor.RenderImage(imageStream, width, height, background);
                finalInfoRendered.Append(finalPicture);
            }

            // Now, return the value
            return finalInfoRendered.ToString();
        }

        internal void EditName(Card? card)
        {
            if (card is null)
                return;

            // Ask for the new name
            string newName = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_CONTACTNEWNAMEPROMPT", "Nitrocid.Extras.Contacts") + $": {GetContactNameFinal(card)}", Settings.InfoBoxSettings).Trim();
            if (string.IsNullOrWhiteSpace(newName))
                newName = "Unnamed contact";

            // Save the new name to the contact
            var splitName = newName.Split(' ');
            bool hasLastName = splitName.Length > 1;
            bool hasExtras = splitName.Length > 2;
            var partsName = card.GetPartsArray<NameInfo>();
            var stringsName = card.GetString(CardStringsEnum.FullName);
            if (partsName.Length > 0)
            {
                partsName[0].ContactFirstName = splitName[0];
                partsName[0].ContactLastName = hasLastName ? splitName[1] : "";
                partsName[0].AltNames = hasExtras ? [.. splitName.Skip(2)] : [];
            }
            else
                card.AddPartToArray<NameInfo>($"{(hasLastName ? splitName[1] : "")};{(splitName[0])};{(hasExtras ? splitName[2] : "")};;");
            if (stringsName.Length > 0)
                stringsName[0].Value = newName;
            else
                card.AddString(CardStringsEnum.FullName, newName);
        }
    }
}
