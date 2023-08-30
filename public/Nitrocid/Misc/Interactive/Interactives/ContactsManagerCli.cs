
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
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.Kernel.Debugging;
using KS.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using VisualCard.Parts;
using System.Text;
using KS.Misc.Probers.Regexp;
using KS.Files.Querying;
using System.Collections;
using KS.Misc.Text;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.Misc.Contacts;

namespace KS.Misc.Interactive.Interactives
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
            bool hasName = selectedContact.ContactNames.Any();
            bool hasAddress = selectedContact.ContactAddresses.Any();
            bool hasMail = selectedContact.ContactMails.Any();
            bool hasOrganization = selectedContact.ContactOrganizations.Any();
            bool hasTelephone = selectedContact.ContactTelephones.Any();
            bool hasURL = selectedContact.ContactURL.Any();

            // Generate the rendered text
            string finalRenderedContactName = hasName ?
                Translate.DoTranslation("Contact name") + $": {selectedContact.ContactFullName}" :
                Translate.DoTranslation("No contact name");
            string finalRenderedContactAddress = hasAddress ?
                Translate.DoTranslation("Contact address") + $": {selectedContact.ContactAddresses[0].StreetAddress}, {selectedContact.ContactAddresses[0].PostalCode}, {selectedContact.ContactAddresses[0].PostOfficeBox}, {selectedContact.ContactAddresses[0].ExtendedAddress}, {selectedContact.ContactAddresses[0].Locality}, {selectedContact.ContactAddresses[0].Region}, {selectedContact.ContactAddresses[0].Country}" :
                Translate.DoTranslation("No contact address");
            string finalRenderedContactMail = hasMail ?
                Translate.DoTranslation("Contact mail") + $": {selectedContact.ContactMails[0].ContactEmailAddress}" :
                Translate.DoTranslation("No contact mail");
            string finalRenderedContactOrganization = hasOrganization ?
                Translate.DoTranslation("Contact organization") + $": {selectedContact.ContactOrganizations[0].Name}, {selectedContact.ContactOrganizations[0].Unit}, {selectedContact.ContactOrganizations[0].Role}" :
                Translate.DoTranslation("No contact organization");
            string finalRenderedContactTelephone = hasTelephone ?
                Translate.DoTranslation("Contact telephone") + $": {selectedContact.ContactTelephones[0].ContactPhoneNumber}" :
                Translate.DoTranslation("No contact telephone");
            string finalRenderedContactURL = hasURL ?
                Translate.DoTranslation("Contact URL") + $": {selectedContact.ContactURL}" :
                Translate.DoTranslation("No contact URL");

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
            bool hasName = selectedContact.ContactNames.Any();

            // Generate the rendered text
            string finalRenderedContactName = hasName ?
                Translate.DoTranslation("Contact name") + $": {selectedContact.ContactFullName}" :
                Translate.DoTranslation("No contact name");

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

        private static void RemoveContact(int index)
        {
            InteractiveTuiTools.ForceRefreshSelection();
            ContactsManager.RemoveContact(index);
        }

        private static void RemoveContacts()
        {
            InteractiveTuiTools.ForceRefreshSelection();
            ContactsManager.RemoveContacts();
        }

        private static void ImportContacts()
        {
            try
            {
                // Initiate import process
                ContactsManager.ImportContacts();
            }
            catch (Exception ex)
            {
                InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Some of the contacts can't be imported.") + ex.Message, BoxForegroundColor, BoxBackgroundColor);
            }
            RedrawRequired = true;
        }

        private static void ImportContactsFrom()
        {
            // Now, render the search box
            string path = InfoBoxColor.WriteInfoBoxInput(Translate.DoTranslation("Enter path to a VCF file containing your contact. Android's contacts2.db file is also supported."), BoxForegroundColor, BoxBackgroundColor);
            if (Checking.FileExists(path))
            {
                try
                {
                    // Initiate installation
                    ContactsManager.InstallContacts(path);
                }
                catch
                {
                    InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Contact file is invalid."), BoxForegroundColor, BoxBackgroundColor);
                }
            }
            else
                InfoBoxColor.WriteInfoBox(Translate.DoTranslation("File doesn't exist. Make sure that you've written the correct path to a VCF file or to a contacts2.db file."), BoxForegroundColor, BoxBackgroundColor);
            RedrawRequired = true;
        }

        private static void ShowContactInfo(int index)
        {
            // Render the final information string
            var finalInfoRendered = new StringBuilder();
            var card = ContactsManager.GetContact(index);
            bool hasName = card.ContactNames.Any();
            bool hasAddress = card.ContactAddresses.Any();
            bool hasMail = card.ContactMails.Any();
            bool hasOrganization = card.ContactOrganizations.Any();
            bool hasTelephone = card.ContactTelephones.Any();
            bool hasURL = card.ContactURL.Any();
            bool hasGeo = card.ContactGeo.Any();
            bool hasImpps = card.ContactImpps.Any();
            bool hasNicknames = card.ContactNicknames.Any();
            bool hasRoles = card.ContactRoles.Any();
            bool hasTitles = card.ContactTitles.Any();
            bool hasNotes = string.IsNullOrEmpty(card.ContactNotes);

            string finalRenderedContactName =
                hasName ?
                Translate.DoTranslation("Contact name") + $": {card.ContactFullName}" :
                Translate.DoTranslation("No contact name");
            string finalRenderedContactAddress =
                hasAddress ?
                Translate.DoTranslation("Contact address") + $": {card.ContactAddresses[0].StreetAddress}, {card.ContactAddresses[0].PostalCode}, {card.ContactAddresses[0].PostOfficeBox}, {card.ContactAddresses[0].ExtendedAddress}, {card.ContactAddresses[0].Locality}, {card.ContactAddresses[0].Region}, {card.ContactAddresses[0].Country}" :
                Translate.DoTranslation("No contact address");
            string finalRenderedContactMail =
                hasMail ?
                Translate.DoTranslation("Contact mail") + $": {card.ContactMails[0].ContactEmailAddress}" :
                Translate.DoTranslation("No contact mail");
            string finalRenderedContactOrganization = hasOrganization ?
                Translate.DoTranslation("Contact organization") + $": {card.ContactOrganizations[0].Name}, {card.ContactOrganizations[0].Unit}, {card.ContactOrganizations[0].Role}" :
                Translate.DoTranslation("No contact organization");
            string finalRenderedContactTelephone = hasTelephone ?
                Translate.DoTranslation("Contact telephone") + $": {card.ContactTelephones[0].ContactPhoneNumber}" :
                Translate.DoTranslation("No contact telephone");
            string finalRenderedContactURL = hasURL ?
                Translate.DoTranslation("Contact URL") + $": {card.ContactURL}" :
                Translate.DoTranslation("No contact URL");
            string finalRenderedContactGeo = hasGeo ?
                Translate.DoTranslation("Contact geo") + $": {card.ContactURL}" :
                Translate.DoTranslation("No contact geo");
            string finalRenderedContactImpps = hasImpps ?
                Translate.DoTranslation("Contact IMPP") + $": {card.ContactURL}" :
                Translate.DoTranslation("No contact IMPP");
            string finalRenderedContactNicknames = hasNicknames ?
                Translate.DoTranslation("Contact nicknames") + $": {card.ContactURL}" :
                Translate.DoTranslation("No contact nicknames");
            string finalRenderedContactRoles = hasRoles ?
                Translate.DoTranslation("Contact roles") + $": {card.ContactURL}" :
                Translate.DoTranslation("No contact Roles");
            string finalRenderedContactTitles = hasTitles ?
                Translate.DoTranslation("Contact titles") + $": {card.ContactURL}" :
                Translate.DoTranslation("No contact titles");
            string finalRenderedContactNotes = hasNotes ?
                Translate.DoTranslation("Contact notes") + $": {card.ContactURL}" :
                Translate.DoTranslation("No contact notes");
            finalInfoRendered.AppendLine(finalRenderedContactName);
            finalInfoRendered.AppendLine(finalRenderedContactAddress);
            finalInfoRendered.AppendLine(finalRenderedContactMail);
            finalInfoRendered.AppendLine(finalRenderedContactOrganization);
            finalInfoRendered.AppendLine(finalRenderedContactTelephone);
            finalInfoRendered.AppendLine(finalRenderedContactURL);
            finalInfoRendered.AppendLine(finalRenderedContactGeo);
            finalInfoRendered.AppendLine(finalRenderedContactImpps);
            finalInfoRendered.AppendLine(finalRenderedContactNicknames);
            finalInfoRendered.AppendLine(finalRenderedContactRoles);
            finalInfoRendered.AppendLine(finalRenderedContactTitles);
            finalInfoRendered.AppendLine(finalRenderedContactNotes);
            finalInfoRendered.AppendLine("\n" + Translate.DoTranslation("Press any key to close this window."));

            // Now, render the info box
            InfoBoxColor.WriteInfoBox(finalInfoRendered.ToString(), BoxForegroundColor, BoxBackgroundColor);
            RedrawRequired = true;
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
            InfoBoxColor.WriteInfoBox(finalInfoRendered.ToString(), BoxForegroundColor, BoxBackgroundColor);
            RedrawRequired = true;
        }

        private static void SearchBox()
        {
            // Now, render the search box
            string exp = InfoBoxColor.WriteInfoBoxInput(Translate.DoTranslation("Enter regular expression to search the contacts."), BoxForegroundColor, BoxBackgroundColor);
            if (RegexpTools.IsValidRegex(exp))
            {
                // Initiate the search
                var foundCard = ContactsManager.SearchNext(exp);
                if (foundCard is null)
                    InfoBoxColor.WriteInfoBox(Translate.DoTranslation("There are no contacts that contains your requested expression."), BoxForegroundColor, BoxBackgroundColor);
                UpdateIndex(foundCard);
            }
            else
                InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Regular expression is invalid."), BoxForegroundColor, BoxBackgroundColor);
            RedrawRequired = true;
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
    }
}
