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
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.Kernel.Debugging;
using KS.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using VisualCard.Parts;
using System.Text;
using System.Collections;
using KS.Misc.Text;
using KS.ConsoleBase.Interactive;
using KS.Misc.Text.Probers.Regexp;
using KS.Files.Operations.Querying;
using KS.ConsoleBase.Inputs.Styles;

namespace Nitrocid.Extras.Contacts.Contacts.Interactives
{
    /// <summary>
    /// Contacts manager class
    /// </summary>
    public class ContactsManagerCli : BaseInteractiveTui, IInteractiveTui
    {
        /// <summary>
        /// Contact manager bindings
        /// </summary>
        public override List<InteractiveTuiBinding> Bindings { get; set; } = new()
        {
            // Operations
            new InteractiveTuiBinding(/* Localizable */ "Delete",      ConsoleKey.F1, (_, index) => RemoveContact(index), true),
            new InteractiveTuiBinding(/* Localizable */ "Delete All",  ConsoleKey.F2, (_, _) => RemoveContacts(), true),
            new InteractiveTuiBinding(/* Localizable */ "Import",      ConsoleKey.F3, (_, _) => ImportContacts(), true),
            new InteractiveTuiBinding(/* Localizable */ "Import From", ConsoleKey.F4, (_, _) => ImportContactsFrom(), true),
            new InteractiveTuiBinding(/* Localizable */ "Info",        ConsoleKey.F5, (_, index) => ShowContactInfo(index), true),
            new InteractiveTuiBinding(/* Localizable */ "Search",      ConsoleKey.F6, (_, _) => SearchBox(), true),
            new InteractiveTuiBinding(/* Localizable */ "Search Next", ConsoleKey.F7, (_, _) => SearchNext(), true),
            new InteractiveTuiBinding(/* Localizable */ "Search Back", ConsoleKey.F8, (_, _) => SearchPrevious(), true),
            new InteractiveTuiBinding(/* Localizable */ "Raw Info",    ConsoleKey.F9, (_, index) => ShowContactRawInfo(index), true),
        };

        /// <inheritdoc/>
        public override IEnumerable PrimaryDataSource =>
            ContactsManager.GetContacts();

        /// <inheritdoc/>
        public override bool AcceptsEmptyData =>
            true;

        /// <inheritdoc/>
        public override string GetInfoFromItem(object item)
        {
            // Get some info from the contact
            Card selectedContact = (Card)item;
            if (selectedContact is null)
                return Translate.DoTranslation("There is no contact. If you'd like to import contacts, please use the import options using the keystrokes defined at the bottom of the screen.");

            // Generate the rendered text
            string finalRenderedContactName = GetContactNameFinal(selectedContact);
            string finalRenderedContactAddress = GetContactAddressFinal(selectedContact);
            string finalRenderedContactMail = GetContactMailFinal(selectedContact);
            string finalRenderedContactOrganization = GetContactOrganizationFinal(selectedContact);
            string finalRenderedContactTelephone = GetContactTelephoneFinal(selectedContact);
            string finalRenderedContactURL = GetContactURLFinal(selectedContact);

            // Render them to the second pane
            return
                finalRenderedContactName + CharManager.NewLine +
                finalRenderedContactAddress + CharManager.NewLine +
                finalRenderedContactMail + CharManager.NewLine +
                finalRenderedContactOrganization + CharManager.NewLine +
                finalRenderedContactTelephone + CharManager.NewLine +
                finalRenderedContactURL
            ;
        }

        /// <inheritdoc/>
        public override void RenderStatus(object item)
        {
            // Get some info from the contact
            Card selectedContact = (Card)item;
            if (selectedContact is null)
                return;

            // Generate the rendered text
            string finalRenderedContactName = GetContactNameFinal(selectedContact);

            // Render them to the status
            Status = finalRenderedContactName;
        }

        /// <inheritdoc/>
        public override string GetEntryFromItem(object item)
        {
            Card contact = (Card)item;
            if (contact is null)
                return "";
            return contact.ContactFullName;
        }

        private static void RemoveContact(int index) =>
            ContactsManager.RemoveContact(index);

        private static void RemoveContacts() =>
            ContactsManager.RemoveContacts();

        private static void ImportContacts()
        {
            try
            {
                // Initiate import process
                ContactsManager.ImportContacts();
            }
            catch (Exception ex)
            {
                InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("Some of the contacts can't be imported.") + ex.Message, BoxForegroundColor, BoxBackgroundColor);
            }
        }

        private static void ImportContactsFrom()
        {
            // Now, render the search box
            string path = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Enter path to a VCF file containing your contact. Android's contacts2.db file is also supported."), BoxForegroundColor, BoxBackgroundColor);
            if (Checking.FileExists(path))
            {
                try
                {
                    // Initiate installation
                    ContactsManager.InstallContacts(path);
                }
                catch
                {
                    InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("Contact file is invalid."), BoxForegroundColor, BoxBackgroundColor);
                }
            }
            else
                InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("File doesn't exist. Make sure that you've written the correct path to a VCF file or to a contacts2.db file."), BoxForegroundColor, BoxBackgroundColor);
        }

        private static void ShowContactInfo(int index)
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
            finalInfoRendered.AppendLine("\n" + Translate.DoTranslation("Press any key to close this window."));

            // Now, render the info box
            InfoBoxColor.WriteInfoBoxColorBack(finalInfoRendered.ToString(), BoxForegroundColor, BoxBackgroundColor);
        }

        private static void ShowContactRawInfo(int index)
        {
            // Render the final information string
            var finalInfoRendered = new StringBuilder();
            var card = ContactsManager.GetContact(index);

            string finalRenderedContactVcardInfo = card.SaveToString();
            finalInfoRendered.AppendLine(finalRenderedContactVcardInfo);
            finalInfoRendered.Append(Translate.DoTranslation("Press any key to close this window."));

            // Now, render the info box
            InfoBoxColor.WriteInfoBoxColorBack(finalInfoRendered.ToString(), BoxForegroundColor, BoxBackgroundColor);
        }

        private static void SearchBox()
        {
            // Now, render the search box
            string exp = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Enter regular expression to search the contacts."), BoxForegroundColor, BoxBackgroundColor);
            if (RegexpTools.IsValidRegex(exp))
            {
                // Initiate the search
                var foundCard = ContactsManager.SearchNext(exp);
                if (foundCard is null)
                    InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("There are no contacts that contains your requested expression."), BoxForegroundColor, BoxBackgroundColor);
                UpdateIndex(foundCard);
            }
            else
                InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("Regular expression is invalid."), BoxForegroundColor, BoxBackgroundColor);
        }

        private static void SearchNext()
        {
            // Initiate the search
            var foundCard = ContactsManager.SearchNext();
            UpdateIndex(foundCard);
        }

        private static void SearchPrevious()
        {
            // Initiate the search
            var foundCard = ContactsManager.SearchPrevious();
            UpdateIndex(foundCard);
        }

        private static void UpdateIndex(Card foundCard)
        {
            var contacts = ContactsManager.GetContacts();
            if (foundCard is not null)
            {
                // Get the index from the instance
                int idx = Array.FindIndex(contacts, (card) => card == foundCard);
                DebugCheck.Assert(idx != -1, "contact index is -1!!!");
                FirstPaneCurrentSelection = idx + 1;
            }
        }

        private static string GetContactNameFinal(int index)
        {
            // Render the final information string
            var card = ContactsManager.GetContact(index);
            return GetContactNameFinal(card);
        }

        private static string GetContactNameFinal(Card card)
        {
            // Render the final information string
            var finalInfoRendered = new StringBuilder();
            bool hasName = card.ContactNames.Any();

            if (hasName)
                finalInfoRendered.Append(Translate.DoTranslation("Contact name") + $": {card.ContactFullName}");
            else
                finalInfoRendered.Append(Translate.DoTranslation("No contact name"));

            // Now, return the value
            return finalInfoRendered.ToString();
        }

        private static string GetContactAddressFinal(int index)
        {
            // Render the final information string
            var card = ContactsManager.GetContact(index);
            return GetContactAddressFinal(card);
        }

        private static string GetContactAddressFinal(Card card)
        {
            // Render the final information string
            var finalInfoRendered = new StringBuilder();
            bool hasAddress = card.ContactAddresses.Any();

            if (hasAddress)
            {
                finalInfoRendered.Append(Translate.DoTranslation("Contact address") + ": ");

                var address = card.ContactAddresses[0];
                List<string> fullElements = new();
                string street = address.StreetAddress;
                string postal = address.PostalCode;
                string poBox = address.PostOfficeBox;
                string extended = address.ExtendedAddress;
                string locality = address.Locality;
                string region = address.Region;
                string country = address.Country;
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

        private static string GetContactMailFinal(int index)
        {
            // Render the final information string
            var card = ContactsManager.GetContact(index);
            return GetContactMailFinal(card);
        }

        private static string GetContactMailFinal(Card card)
        {
            // Render the final information string
            var finalInfoRendered = new StringBuilder();
            bool hasMail = card.ContactMails.Any();

            if (hasMail)
                finalInfoRendered.Append(Translate.DoTranslation("Contact mail") + $": {card.ContactMails[0].ContactEmailAddress}");
            else
                finalInfoRendered.Append(Translate.DoTranslation("No contact mail"));

            // Now, return the value
            return finalInfoRendered.ToString();
        }

        private static string GetContactOrganizationFinal(int index)
        {
            // Render the final information string
            var card = ContactsManager.GetContact(index);
            return GetContactOrganizationFinal(card);
        }

        private static string GetContactOrganizationFinal(Card card)
        {
            // Render the final information string
            var finalInfoRendered = new StringBuilder();
            bool hasOrganization = card.ContactOrganizations.Any();

            if (hasOrganization)
            {
                finalInfoRendered.Append(Translate.DoTranslation("Contact organization") + ": ");

                var org = card.ContactOrganizations[0];
                List<string> fullElements = new();
                string name = org.Name;
                string unit = org.Unit;
                string role = org.Role;
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

        private static string GetContactTelephoneFinal(int index)
        {
            // Render the final information string
            var card = ContactsManager.GetContact(index);
            return GetContactTelephoneFinal(card);
        }

        private static string GetContactTelephoneFinal(Card card)
        {
            // Render the final information string
            var finalInfoRendered = new StringBuilder();
            bool hasTelephone = card.ContactTelephones.Any();

            if (hasTelephone)
                finalInfoRendered.Append(Translate.DoTranslation("Contact telephone") + $": {card.ContactTelephones[0].ContactPhoneNumber}");
            else
                finalInfoRendered.Append(Translate.DoTranslation("No contact telephone"));

            // Now, return the value
            return finalInfoRendered.ToString();
        }

        private static string GetContactURLFinal(int index)
        {
            // Render the final information string
            var card = ContactsManager.GetContact(index);
            return GetContactURLFinal(card);
        }

        private static string GetContactURLFinal(Card card)
        {
            // Render the final information string
            var finalInfoRendered = new StringBuilder();
            bool hasURL = !string.IsNullOrEmpty(card.ContactURL);

            if (hasURL)
                finalInfoRendered.Append(Translate.DoTranslation("Contact URL") + $": {card.ContactURL}");
            else
                finalInfoRendered.Append(Translate.DoTranslation("No contact URL"));

            // Now, return the value
            return finalInfoRendered.ToString();
        }

        private static string GetContactGeoFinal(int index)
        {
            // Render the final information string
            var card = ContactsManager.GetContact(index);
            return GetContactGeoFinal(card);
        }

        private static string GetContactGeoFinal(Card card)
        {
            // Render the final information string
            var finalInfoRendered = new StringBuilder();
            bool hasGeo = card.ContactGeo.Any();

            if (hasGeo)
                finalInfoRendered.Append(Translate.DoTranslation("Contact geo") + $": {card.ContactGeo[0].Geo}");
            else
                finalInfoRendered.Append(Translate.DoTranslation("No contact geo"));

            // Now, return the value
            return finalInfoRendered.ToString();
        }

        private static string GetContactImppFinal(int index)
        {
            // Render the final information string
            var card = ContactsManager.GetContact(index);
            return GetContactImppFinal(card);
        }

        private static string GetContactImppFinal(Card card)
        {
            // Render the final information string
            var finalInfoRendered = new StringBuilder();
            bool hasImpp = card.ContactImpps.Any();

            if (hasImpp)
                finalInfoRendered.Append(Translate.DoTranslation("Contact IMPP") + $": {card.ContactImpps[0].ContactIMPP}");
            else
                finalInfoRendered.Append(Translate.DoTranslation("No contact IMPP"));

            // Now, return the value
            return finalInfoRendered.ToString();
        }

        private static string GetContactNicknameFinal(int index)
        {
            // Render the final information string
            var card = ContactsManager.GetContact(index);
            return GetContactNicknameFinal(card);
        }

        private static string GetContactNicknameFinal(Card card)
        {
            // Render the final information string
            var finalInfoRendered = new StringBuilder();
            bool hasNickname = card.ContactNicknames.Any();

            if (hasNickname)
                finalInfoRendered.Append(Translate.DoTranslation("Contact nickname") + $": {card.ContactNicknames[0].ContactNickname}");
            else
                finalInfoRendered.Append(Translate.DoTranslation("No contact nickname"));

            // Now, return the value
            return finalInfoRendered.ToString();
        }

        private static string GetContactRoleFinal(int index)
        {
            // Render the final information string
            var card = ContactsManager.GetContact(index);
            return GetContactRoleFinal(card);
        }

        private static string GetContactRoleFinal(Card card)
        {
            // Render the final information string
            var finalInfoRendered = new StringBuilder();
            bool hasRoles = card.ContactRoles.Any();

            if (hasRoles)
                finalInfoRendered.Append(Translate.DoTranslation("Contact role") + $": {card.ContactRoles[0].ContactRole}");
            else
                finalInfoRendered.Append(Translate.DoTranslation("No contact role"));

            // Now, return the value
            return finalInfoRendered.ToString();
        }

        private static string GetContactTitleFinal(int index)
        {
            // Render the final information string
            var card = ContactsManager.GetContact(index);
            return GetContactTitleFinal(card);
        }

        private static string GetContactTitleFinal(Card card)
        {
            // Render the final information string
            var finalInfoRendered = new StringBuilder();
            bool hasTitles = card.ContactTitles.Any();

            if (hasTitles)
                finalInfoRendered.Append(Translate.DoTranslation("Contact title") + $": {card.ContactTitles[0].ContactTitle}");
            else
                finalInfoRendered.Append(Translate.DoTranslation("No contact title"));

            // Now, return the value
            return finalInfoRendered.ToString();
        }

        private static string GetContactNotesFinal(int index)
        {
            // Render the final information string
            var card = ContactsManager.GetContact(index);
            return GetContactNotesFinal(card);
        }

        private static string GetContactNotesFinal(Card card)
        {
            // Render the final information string
            var finalInfoRendered = new StringBuilder();
            bool hasNotes = !string.IsNullOrEmpty(card.ContactNotes);

            if (hasNotes)
                finalInfoRendered.Append(Translate.DoTranslation("Contact notes") + $": {card.ContactNotes}");
            else
                finalInfoRendered.Append(Translate.DoTranslation("No contact notes"));

            // Now, return the value
            return finalInfoRendered.ToString();
        }
    }
}
