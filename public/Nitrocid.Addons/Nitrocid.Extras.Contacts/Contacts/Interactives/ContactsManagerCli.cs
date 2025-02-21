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
                return Translate.DoTranslation("There is no contact. If you'd like to import contacts, please use the import options using the keystrokes defined at the bottom of the screen.");

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
                InfoBoxModalColor.WriteInfoBoxModalColorBack(Translate.DoTranslation("Some of the contacts can't be imported.") + ex.Message, KernelColorTools.GetColor(KernelColorType.TuiBoxForeground), KernelColorTools.GetColor(KernelColorType.TuiBoxBackground));
            }
        }

        internal void ImportContactsFrom()
        {
            // Now, render the search box
            string path = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Enter path to a VCF file containing your contact. Android's contacts2.db file is also supported."), KernelColorTools.GetColor(KernelColorType.TuiBoxForeground), KernelColorTools.GetColor(KernelColorType.TuiBoxBackground));
            if (FilesystemTools.FileExists(path))
            {
                try
                {
                    // Initiate installation
                    ContactsManager.InstallContacts(path);
                }
                catch
                {
                    InfoBoxModalColor.WriteInfoBoxModalColorBack(Translate.DoTranslation("Contact file is invalid."), KernelColorTools.GetColor(KernelColorType.TuiBoxForeground), KernelColorTools.GetColor(KernelColorType.TuiBoxBackground));
                }
            }
            else
                InfoBoxModalColor.WriteInfoBoxModalColorBack(Translate.DoTranslation("File doesn't exist. Make sure that you've written the correct path to a VCF file or to a contacts2.db file."), KernelColorTools.GetColor(KernelColorType.TuiBoxForeground), KernelColorTools.GetColor(KernelColorType.TuiBoxBackground));
        }

        internal void ImportContactFromMeCard()
        {
            // Now, render the search box
            string meCard = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Enter a valid MeCard representation of your contact."), KernelColorTools.GetColor(KernelColorType.TuiBoxForeground), KernelColorTools.GetColor(KernelColorType.TuiBoxBackground));
            if (!string.IsNullOrEmpty(meCard))
            {
                try
                {
                    // Initiate installation
                    ContactsManager.InstallContactFromMeCard(meCard);
                }
                catch
                {
                    InfoBoxModalColor.WriteInfoBoxModalColorBack(Translate.DoTranslation("Contact MeCard syntax is invalid."), KernelColorTools.GetColor(KernelColorType.TuiBoxForeground), KernelColorTools.GetColor(KernelColorType.TuiBoxBackground));
                }
            }
            else
                InfoBoxModalColor.WriteInfoBoxModalColorBack(Translate.DoTranslation("Contact MeCard syntax may not be empty"), KernelColorTools.GetColor(KernelColorType.TuiBoxForeground), KernelColorTools.GetColor(KernelColorType.TuiBoxBackground));
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
            InfoBoxModalColor.WriteInfoBoxModalColorBack(finalInfoRendered.ToString(), KernelColorTools.GetColor(KernelColorType.TuiBoxForeground), KernelColorTools.GetColor(KernelColorType.TuiBoxBackground));
        }

        internal void ShowContactRawInfo(int index)
        {
            // Render the final information string
            var finalInfoRendered = new StringBuilder();
            var card = ContactsManager.GetContact(index);

            string finalRenderedContactVcardInfo = card.SaveToString();
            finalInfoRendered.Append(finalRenderedContactVcardInfo);

            // Now, render the info box
            InfoBoxModalColor.WriteInfoBoxModalColorBack(finalInfoRendered.ToString(), KernelColorTools.GetColor(KernelColorType.TuiBoxForeground), KernelColorTools.GetColor(KernelColorType.TuiBoxBackground));
        }

        internal void SearchBox()
        {
            // Now, render the search box
            string exp = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Enter regular expression to search the contacts."), KernelColorTools.GetColor(KernelColorType.TuiBoxForeground), KernelColorTools.GetColor(KernelColorType.TuiBoxBackground));
            if (RegexpTools.IsValidRegex(exp))
            {
                // Initiate the search
                var foundCard = ContactsManager.SearchNext(exp);
                if (foundCard is null)
                    InfoBoxModalColor.WriteInfoBoxModalColorBack(Translate.DoTranslation("There are no contacts that contains your requested expression."), KernelColorTools.GetColor(KernelColorType.TuiBoxForeground), KernelColorTools.GetColor(KernelColorType.TuiBoxBackground));
                UpdateIndex(foundCard);
            }
            else
                InfoBoxModalColor.WriteInfoBoxModalColorBack(Translate.DoTranslation("Regular expression is invalid."), KernelColorTools.GetColor(KernelColorType.TuiBoxForeground), KernelColorTools.GetColor(KernelColorType.TuiBoxBackground));
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
                finalInfoRendered.Append(Translate.DoTranslation("Contact name") + $": {card.GetString(CardStringsEnum.FullName)[0].Value}");
            else
                finalInfoRendered.Append(Translate.DoTranslation("No contact name"));

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
                finalInfoRendered.Append(Translate.DoTranslation("Contact address") + ": ");

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
                finalInfoRendered.Append(Translate.DoTranslation("No contact name"));

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
                finalInfoRendered.Append(Translate.DoTranslation("Contact mail") + $": {card.GetString(CardStringsEnum.Mails)[0].Value}");
            else
                finalInfoRendered.Append(Translate.DoTranslation("No contact mail"));

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
                finalInfoRendered.Append(Translate.DoTranslation("Contact organization") + ": ");

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
                finalInfoRendered.Append(Translate.DoTranslation("No contact organization"));

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
                finalInfoRendered.Append(Translate.DoTranslation("Contact telephone") + $": {card.GetString(CardStringsEnum.Telephones)[0].Value}");
            else
                finalInfoRendered.Append(Translate.DoTranslation("No contact telephone"));

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
                finalInfoRendered.Append(Translate.DoTranslation("Contact URL") + $": {card.GetString(CardStringsEnum.Url)[0].Value}");
            else
                finalInfoRendered.Append(Translate.DoTranslation("No contact URL"));

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
                finalInfoRendered.Append(Translate.DoTranslation("Contact geo") + $": {card.GetString(CardStringsEnum.Geo)[0].Value}");
            else
                finalInfoRendered.Append(Translate.DoTranslation("No contact geo"));

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
                finalInfoRendered.Append(Translate.DoTranslation("Contact IMPP") + $": {card.GetString(CardStringsEnum.Impps)[0].Value}");
            else
                finalInfoRendered.Append(Translate.DoTranslation("No contact IMPP"));

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
                finalInfoRendered.Append(Translate.DoTranslation("Contact nickname") + $": {card.GetString(CardStringsEnum.Nicknames)[0].Value}");
            else
                finalInfoRendered.Append(Translate.DoTranslation("No contact nickname"));

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
                finalInfoRendered.Append(Translate.DoTranslation("Contact role") + $": {card.GetString(CardStringsEnum.Roles)[0].Value}");
            else
                finalInfoRendered.Append(Translate.DoTranslation("No contact role"));

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
                finalInfoRendered.Append(Translate.DoTranslation("Contact title") + $": {card.GetString(CardStringsEnum.Titles)[0].Value}");
            else
                finalInfoRendered.Append(Translate.DoTranslation("No contact title"));

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
                finalInfoRendered.Append(Translate.DoTranslation("Contact notes") + $": {card.GetString(CardStringsEnum.Notes)[0].Value}");
            else
                finalInfoRendered.Append(Translate.DoTranslation("No contact notes"));

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
            string newName = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Enter new name for this contact") + $": {GetContactNameFinal(card)}", KernelColorTools.GetColor(KernelColorType.TuiBoxForeground), KernelColorTools.GetColor(KernelColorType.TuiBoxBackground)).Trim();
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
