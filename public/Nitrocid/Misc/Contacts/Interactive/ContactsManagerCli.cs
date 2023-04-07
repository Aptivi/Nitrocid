
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

using ColorSeq;
using Extensification.StringExts;
using KS.ConsoleBase;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.FancyWriters;
using System;
using System.Collections.Generic;
using System.Linq;
using KS.ConsoleBase.Inputs;
using ColorTools = KS.ConsoleBase.Colors.ColorTools;
using KS.Misc.Threading.Interactive;
using KS.Misc.Contacts;
using VisualCard.Parts;
using System.Text;
using KS.Misc.Probers.Regexp;
using KS.Files.Querying;
using System.IO;

namespace KS.Files.Interactive
{
    /// <summary>
    /// Contacts manager class
    /// </summary>
    public static class ContactsManagerCli
    {
        private static bool redrawRequired = true;
        private static bool isExiting = false;
        private static int paneCurrentSelection = 1;
        private static string status = "";
        private static readonly List<ContactsManagerBinding> ContactsManagerBindings = new()
        {
            // Operations
            new ContactsManagerBinding(/* Localizable */ "Delete",      ConsoleKey.F1, (index) => ContactsManager.RemoveContact(index), true),
            new ContactsManagerBinding(/* Localizable */ "Delete All",  ConsoleKey.F2, (_) => ContactsManager.RemoveContacts(), true),
            new ContactsManagerBinding(/* Localizable */ "Import",      ConsoleKey.F3, (_) => ImportContacts(), true),
            new ContactsManagerBinding(/* Localizable */ "Import From", ConsoleKey.F4, (_) => ImportContactsFrom(), true),
            new ContactsManagerBinding(/* Localizable */ "Info",        ConsoleKey.F5, ShowContactInfo, true),
            new ContactsManagerBinding(/* Localizable */ "Search",      ConsoleKey.F6, (_) => SearchBox(), true),
            new ContactsManagerBinding(/* Localizable */ "Search Next", ConsoleKey.F7, (_) => SearchNext(), true),
            new ContactsManagerBinding(/* Localizable */ "Search Back", ConsoleKey.F8, (_) => SearchPrevious(), true),

            // Misc bindings
            new ContactsManagerBinding(/* Localizable */ "Exit",       ConsoleKey.Escape, (_) => isExiting = true, true)
        };

        /// <summary>
        /// Contacts manager background color
        /// </summary>
        public static Color ContactsManagerBackgroundColor { get; set; } = new(Convert.ToInt32(ConsoleColors.DarkBlue));
        /// <summary>
        /// Contacts manager foreground color
        /// </summary>
        public static Color ContactsManagerForegroundColor { get; set; } = new(Convert.ToInt32(ConsoleColors.Yellow));
        /// <summary>
        /// Contacts manager pane background color
        /// </summary>
        public static Color ContactsManagerPaneBackgroundColor { get; set; } = new(Convert.ToInt32(ConsoleColors.Blue3));
        /// <summary>
        /// Contacts manager pane separator color
        /// </summary>
        public static Color ContactsManagerPaneSeparatorColor { get; set; } = new(Convert.ToInt32(ConsoleColors.DarkGreen_005f00));
        /// <summary>
        /// Contacts manager pane selected Contacts color (foreground)
        /// </summary>
        public static Color ContactsManagerPaneSelectedContactsForeColor { get; set; } = new(Convert.ToInt32(ConsoleColors.Yellow));
        /// <summary>
        /// Contacts manager pane selected Contacts color (background)
        /// </summary>
        public static Color ContactsManagerPaneSelectedContactsBackColor { get; set; } = new(Convert.ToInt32(ConsoleColors.DarkBlue));
        /// <summary>
        /// Contacts manager pane Contacts color (foreground)
        /// </summary>
        public static Color ContactsManagerPaneContactsForeColor { get; set; } = new(Convert.ToInt32(ConsoleColors.DarkYellow));
        /// <summary>
        /// Contacts manager pane Contacts color (background)
        /// </summary>
        public static Color ContactsManagerPaneContactsBackColor { get; set; } = new(Convert.ToInt32(ConsoleColors.Blue3));
        /// <summary>
        /// Contacts manager option background color
        /// </summary>
        public static Color ContactsManagerOptionBackgroundColor { get; set; } = new(Convert.ToInt32(ConsoleColors.DarkCyan));
        /// <summary>
        /// Contacts manager key binding in option color
        /// </summary>
        public static Color ContactsManagerKeyBindingOptionColor { get; set; } = new(Convert.ToInt32(ConsoleColors.Black));
        /// <summary>
        /// Contacts manager option foreground color
        /// </summary>
        public static Color ContactsManagerOptionForegroundColor { get; set; } = new(Convert.ToInt32(ConsoleColors.Cyan));
        /// <summary>
        /// Contacts manager box background color
        /// </summary>
        public static Color ContactsManagerBoxBackgroundColor { get; set; } = new(Convert.ToInt32(ConsoleColors.Red));
        /// <summary>
        /// Contacts manager box foreground color
        /// </summary>
        public static Color ContactsManagerBoxForegroundColor { get; set; } = new(Convert.ToInt32(ConsoleColors.White));

        /// <summary>
        /// Opens the contacts manager
        /// </summary>
        public static void OpenMain()
        {
            isExiting = false;
            redrawRequired = true;
            status = Translate.DoTranslation("Ready");

            while (!isExiting)
            {
                // Prepare the console
                ConsoleWrapper.CursorVisible = false;
                int SeparatorHalfConsoleWidth = ConsoleWrapper.WindowWidth / 2;
                int SeparatorHalfConsoleWidthInterior = (ConsoleWrapper.WindowWidth / 2) - 2;
                int SeparatorMinimumHeight = 1;
                int SeparatorMinimumHeightInterior = 2;
                int SeparatorMaximumHeight = ConsoleWrapper.WindowHeight - 2;
                int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;

                // Redraw the entire contacts manager screen
                if (redrawRequired)
                {
                    ColorTools.LoadBack(ContactsManagerBackgroundColor, true);

                    // Make a separator that separates the two panes to make it look like Total Commander or Midnight Commander. We need information in the upper and the
                    // lower part of the console, so we need to render the entire program to look like this: (just a concept mockup)
                    //
                    //       | vvvvvvvvvvvvvvvvvvvvvv (SeparatorHalfConsoleWidth)
                    //       |  vvvvvvvvvvvvvvvvvvvv  (SeparatorHalfConsoleWidthInterior)
                    // H: 0  |
                    // H: 1  | a--------------------|c---------------------| < ----> (SeparatorMinimumHeight)
                    // H: 2  | |b                   ||d                    | << ----> (SeparatorMinimumHeightInterior)
                    // H: 3  | |                    ||                     | <<
                    // H: 4  | |                    ||                     | <<
                    // H: 5  | |                    ||                     | <<
                    // H: 6  | |                    ||                     | <<
                    // H: 7  | |                    ||                     | <<
                    // H: 8  | |                    ||                     | << ----> (SeparatorMaximumHeightInterior)
                    // H: 9  | |--------------------||---------------------| < ----> (SeparatorMaximumHeight)
                    // H: 10 |
                    //       | where a is the dimension for the first pane upper left corner           (0, SeparatorMinimumHeight                                     (usually 1))
                    //       |   and b is the dimension for the first pane interior upper left corner  (1, SeparatorMinimumHeightInterior                             (usually 2))
                    //       |   and c is the dimension for the second pane upper left corner          (SeparatorHalfConsoleWidth, SeparatorMinimumHeight             (usually 1))
                    //       |   and d is the dimension for the second pane interior upper left corner (SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior (usually 2))

                    // First, the horizontal and vertical separators
                    BorderColor.WriteBorder(0, SeparatorMinimumHeight, SeparatorHalfConsoleWidthInterior, SeparatorMaximumHeightInterior, ContactsManagerPaneSeparatorColor, ContactsManagerPaneBackgroundColor);
                    BorderColor.WriteBorder(SeparatorHalfConsoleWidth, SeparatorMinimumHeight, SeparatorHalfConsoleWidthInterior, SeparatorMaximumHeightInterior, ContactsManagerPaneSeparatorColor, ContactsManagerPaneBackgroundColor);

                    // Render the key bindings
                    ConsoleWrapper.CursorLeft = 0;
                    foreach (ContactsManagerBinding binding in ContactsManagerBindings)
                    {
                        // First, check to see if the rendered binding info is going to exceed the console window width
                        if (!($" {binding.BindingKeyName} {binding.BindingName}  ".Length + ConsoleWrapper.CursorLeft >= ConsoleWrapper.WindowWidth))
                        {
                            TextWriterWhereColor.WriteWhere($" {binding.BindingKeyName} ", ConsoleWrapper.CursorLeft + 0, ConsoleWrapper.WindowHeight - 1, ContactsManagerKeyBindingOptionColor, ContactsManagerOptionBackgroundColor);
                            TextWriterWhereColor.WriteWhere($"{(binding._localizable ? Translate.DoTranslation(binding.BindingName) : binding.BindingName)}  ", ConsoleWrapper.CursorLeft + 1, ConsoleWrapper.WindowHeight - 1, ContactsManagerOptionForegroundColor, ContactsManagerBackgroundColor);
                        }
                    }

                    // Don't require redraw
                    redrawRequired = false;
                }

                // Render the contacts lists
                var contacts = ContactsManager.GetContacts();
                int contactsCount = contacts.Length;
                int pages = contactsCount / SeparatorMaximumHeightInterior;
                int answersPerPage = SeparatorMaximumHeightInterior - 1;
                int currentPage = (paneCurrentSelection - 1) / answersPerPage;
                int startIndex = answersPerPage * currentPage;
                int endIndex = answersPerPage * (currentPage + 1);
                for (int i = 0; i <= answersPerPage; i++)
                {
                    // Populate the first pane
                    string finalEntry = "";
                    int finalIndex = i + startIndex;
                    if (finalIndex <= contacts.Length - 1)
                    {
                        Card contact = contacts[finalIndex];
                        finalEntry = $"[{finalIndex + 1}] {contact.ContactFullName}".Truncate(SeparatorHalfConsoleWidthInterior - 4);
                    }

                    var finalForeColor = finalIndex == paneCurrentSelection - 1 ? ContactsManagerPaneSelectedContactsForeColor : ContactsManagerPaneContactsForeColor;
                    var finalBackColor = finalIndex == paneCurrentSelection - 1 ? ContactsManagerPaneSelectedContactsBackColor : ContactsManagerPaneContactsBackColor;
                    TextWriterWhereColor.WriteWhere(finalEntry + " ".Repeat(SeparatorHalfConsoleWidthInterior - finalEntry.Length), 1, SeparatorMinimumHeightInterior + finalIndex - startIndex, finalForeColor, finalBackColor);
                }
                ProgressBarVerticalColor.WriteVerticalProgress(100 * ((double)paneCurrentSelection / contactsCount), SeparatorHalfConsoleWidthInterior - 1, 1, 2, 2, false);

                // Write status and contacts info
                string finalInfoRendered = "";
                try
                {
                    if (contacts.Length > 0)
                    {
                        var selectedContact = contacts[paneCurrentSelection - 1];
                        bool hasName = selectedContact.ContactNames.Any();
                        bool hasAddress = selectedContact.ContactAddresses.Any();
                        bool hasMail = selectedContact.ContactMails.Any();
                        bool hasOrganization = selectedContact.ContactOrganizations.Any();
                        bool hasTelephone = selectedContact.ContactTelephones.Any();
                        bool hasURL = selectedContact.ContactURL.Any();

                        // Generate the rendered text
                        string finalRenderedContactName =
                            hasName ?
                            (Translate.DoTranslation("Contact name") + $": {selectedContact.ContactFullName}").Truncate(SeparatorHalfConsoleWidthInterior - 3) :
                            Translate.DoTranslation("No contact name");
                        string finalRenderedContactAddress =
                            hasAddress ?
                            (Translate.DoTranslation("Contact address") + $": {selectedContact.ContactAddresses[0].StreetAddress}, {selectedContact.ContactAddresses[0].PostalCode}, {selectedContact.ContactAddresses[0].PostOfficeBox}, {selectedContact.ContactAddresses[0].ExtendedAddress}, {selectedContact.ContactAddresses[0].Locality}, {selectedContact.ContactAddresses[0].Region}, {selectedContact.ContactAddresses[0].Country}").Truncate(SeparatorHalfConsoleWidthInterior - 3) :
                            Translate.DoTranslation("No contact address");
                        string finalRenderedContactMail =
                            hasMail ?
                            (Translate.DoTranslation("Contact mail") + $": {selectedContact.ContactMails[0].ContactEmailAddress}").Truncate(SeparatorHalfConsoleWidthInterior - 3) :
                            Translate.DoTranslation("No contact mail");
                        string finalRenderedContactOrganization = hasOrganization ?
                            (Translate.DoTranslation("Contact organization") + $": {selectedContact.ContactOrganizations[0].Name}, {selectedContact.ContactOrganizations[0].Unit}, {selectedContact.ContactOrganizations[0].Role}").Truncate(SeparatorHalfConsoleWidthInterior - 3) :
                            Translate.DoTranslation("No contact organization");
                        string finalRenderedContactTelephone = hasTelephone ?
                            (Translate.DoTranslation("Contact telephone") + $": {selectedContact.ContactTelephones[0].ContactPhoneNumber}").Truncate(SeparatorHalfConsoleWidthInterior - 3) :
                            Translate.DoTranslation("No contact telephone");
                        string finalRenderedContactURL = hasURL ?
                            (Translate.DoTranslation("Contact URL") + $": {selectedContact.ContactURL}").Truncate(SeparatorHalfConsoleWidthInterior - 3) :
                            Translate.DoTranslation("No contact URL");

                        // Render them to the second pane
                        TextWriterWhereColor.WriteWhere(finalRenderedContactName + " ".Repeat(SeparatorHalfConsoleWidthInterior - finalRenderedContactName.Length), SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior + 0, ContactsManagerForegroundColor, ContactsManagerPaneContactsBackColor);
                        TextWriterWhereColor.WriteWhere(finalRenderedContactAddress + " ".Repeat(SeparatorHalfConsoleWidthInterior - finalRenderedContactAddress.Length), SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior + 2, ContactsManagerForegroundColor, ContactsManagerPaneContactsBackColor);
                        TextWriterWhereColor.WriteWhere(finalRenderedContactMail + " ".Repeat(SeparatorHalfConsoleWidthInterior - finalRenderedContactMail.Length), SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior + 3, ContactsManagerForegroundColor, ContactsManagerPaneContactsBackColor);
                        TextWriterWhereColor.WriteWhere(finalRenderedContactOrganization + " ".Repeat(SeparatorHalfConsoleWidthInterior - finalRenderedContactOrganization.Length), SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior + 4, ContactsManagerForegroundColor, ContactsManagerPaneContactsBackColor);
                        TextWriterWhereColor.WriteWhere(finalRenderedContactTelephone + " ".Repeat(SeparatorHalfConsoleWidthInterior - finalRenderedContactTelephone.Length), SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior + 5, ContactsManagerForegroundColor, ContactsManagerPaneContactsBackColor);
                        TextWriterWhereColor.WriteWhere(finalRenderedContactURL + " ".Repeat(SeparatorHalfConsoleWidthInterior - finalRenderedContactTelephone.Length), SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior + 6, ContactsManagerForegroundColor, ContactsManagerPaneContactsBackColor);
                        TextWriterWhereColor.WriteWhere(" ".Repeat(SeparatorHalfConsoleWidthInterior - finalRenderedContactTelephone.Length), SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior + 7, ContactsManagerForegroundColor, ContactsManagerPaneContactsBackColor);
                        TextWriterWhereColor.WriteWhere(" ".Repeat(SeparatorHalfConsoleWidthInterior - finalRenderedContactTelephone.Length), SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior + 8, ContactsManagerForegroundColor, ContactsManagerPaneContactsBackColor);
                    
                        // Prepare the status
                        finalInfoRendered = $" {status}";
                        status = Translate.DoTranslation("Ready");
                    }
                    else
                        finalInfoRendered = Translate.DoTranslation("No contacts. Import your contacts using F3 or F4.");
                }
                catch (Exception ex)
                {
                    finalInfoRendered = Translate.DoTranslation("Failed to get contacts information.");
                    DebugWriter.WriteDebug(DebugLevel.E, "Error trying to get contacts information in contacts manager: {0}", ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                }
                TextWriterWhereColor.WriteWhere(finalInfoRendered.Truncate(ConsoleWrapper.WindowWidth - 3), 0, 0, ContactsManagerForegroundColor, ContactsManagerBackgroundColor);
                ConsoleExtensions.ClearLineToRight();

                // Wait for key
                ConsoleKey pressedKey = Input.DetectKeypress().Key;
                switch (pressedKey)
                {
                    case ConsoleKey.UpArrow:
                        paneCurrentSelection--;
                        if (paneCurrentSelection < 1)
                            paneCurrentSelection = contactsCount;
                        break;
                    case ConsoleKey.DownArrow:
                        paneCurrentSelection++;
                        if (paneCurrentSelection > contactsCount)
                            paneCurrentSelection = 1;
                        break;
                    case ConsoleKey.PageUp:
                        paneCurrentSelection = 1;
                        break;
                    case ConsoleKey.PageDown:
                        paneCurrentSelection = contactsCount;
                        break;
                    default:
                        var implementedBindings = ContactsManagerBindings.Where((binding) => binding.BindingKeyName == pressedKey);
                        foreach (var implementedBinding in implementedBindings)
                            implementedBinding.BindingAction.Invoke(paneCurrentSelection - 1);
                        break;
                }
            }

            // Clear the console to clean up
            ColorTools.LoadBack();
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
                InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Some of the contacts can't be imported.") + ex.Message, ContactsManagerBoxForegroundColor, ContactsManagerBoxBackgroundColor);
            }
            redrawRequired = true;
        }

        private static void ImportContactsFrom()
        {
            // Now, render the search box
            string path = InfoBoxColor.WriteInfoBoxInput(Translate.DoTranslation("Enter path to a VCF file containing your contact. Android's contacts2.db file is also supported."), ContactsManagerBoxForegroundColor, ContactsManagerBoxBackgroundColor);
            if (Checking.FileExists(path))
            {
                try
                {
                    // Initiate installation
                    ContactsManager.InstallContacts(path);
                }
                catch
                {
                    InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Contact file is invalid."), ContactsManagerBoxForegroundColor, ContactsManagerBoxBackgroundColor);
                }
            }
            else
                InfoBoxColor.WriteInfoBox(Translate.DoTranslation("File doesn't exist. Make sure that you've written the correct path to a VCF file or to a contacts2.db file."), ContactsManagerBoxForegroundColor, ContactsManagerBoxBackgroundColor);
            redrawRequired = true;
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
                (Translate.DoTranslation("Contact name") + $": {card.ContactFullName}") :
                Translate.DoTranslation("No contact name");
            string finalRenderedContactAddress =
                hasAddress ?
                (Translate.DoTranslation("Contact address") + $": {card.ContactAddresses[0].StreetAddress}, {card.ContactAddresses[0].PostalCode}, {card.ContactAddresses[0].PostOfficeBox}, {card.ContactAddresses[0].ExtendedAddress}, {card.ContactAddresses[0].Locality}, {card.ContactAddresses[0].Region}, {card.ContactAddresses[0].Country}") :
                Translate.DoTranslation("No contact address");
            string finalRenderedContactMail =
                hasMail ?
                (Translate.DoTranslation("Contact mail") + $": {card.ContactMails[0].ContactEmailAddress}") :
                Translate.DoTranslation("No contact mail");
            string finalRenderedContactOrganization = hasOrganization ?
                (Translate.DoTranslation("Contact organization") + $": {card.ContactOrganizations[0].Name}, {card.ContactOrganizations[0].Unit}, {card.ContactOrganizations[0].Role}") :
                Translate.DoTranslation("No contact organization");
            string finalRenderedContactTelephone = hasTelephone ?
                (Translate.DoTranslation("Contact telephone") + $": {card.ContactTelephones[0].ContactPhoneNumber}") :
                Translate.DoTranslation("No contact telephone");
            string finalRenderedContactURL = hasURL ?
                (Translate.DoTranslation("Contact URL") + $": {card.ContactURL}") :
                Translate.DoTranslation("No contact URL");
            string finalRenderedContactGeo = hasGeo ?
                (Translate.DoTranslation("Contact geo") + $": {card.ContactURL}") :
                Translate.DoTranslation("No contact geo");
            string finalRenderedContactImpps = hasImpps ?
                (Translate.DoTranslation("Contact IMPP") + $": {card.ContactURL}") :
                Translate.DoTranslation("No contact IMPP");
            string finalRenderedContactNicknames = hasNicknames ?
                (Translate.DoTranslation("Contact nicknames") + $": {card.ContactURL}") :
                Translate.DoTranslation("No contact nicknames");
            string finalRenderedContactRoles = hasRoles ?
                (Translate.DoTranslation("Contact roles") + $": {card.ContactURL}") :
                Translate.DoTranslation("No contact Roles");
            string finalRenderedContactTitles = hasTitles ?
                (Translate.DoTranslation("Contact titles") + $": {card.ContactURL}") :
                Translate.DoTranslation("No contact titles");
            string finalRenderedContactNotes = hasNotes ?
                (Translate.DoTranslation("Contact notes") + $": {card.ContactURL}") :
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
            InfoBoxColor.WriteInfoBox(finalInfoRendered.ToString(), ContactsManagerBoxForegroundColor, ContactsManagerBoxBackgroundColor);
            redrawRequired = true;
        }

        private static void SearchBox()
        {
            // Now, render the search box
            string exp = InfoBoxColor.WriteInfoBoxInput(Translate.DoTranslation("Enter regular expression to search the contacts."), ContactsManagerBoxForegroundColor, ContactsManagerBoxBackgroundColor);
            if (RegexpTools.IsValidRegex(exp))
            {
                // Initiate the search
                var foundCard = ContactsManager.SearchNext(exp);
                if (foundCard is null)
                    InfoBoxColor.WriteInfoBox(Translate.DoTranslation("There are no contacts that contains your requested expression."), ContactsManagerBoxForegroundColor, ContactsManagerBoxBackgroundColor);
                UpdateIndex(foundCard);
            }
            else
                InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Regular expression is invalid."), ContactsManagerBoxForegroundColor, ContactsManagerBoxBackgroundColor);
            redrawRequired = true;
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
                paneCurrentSelection = idx + 1;
            }
        }
    }
}
