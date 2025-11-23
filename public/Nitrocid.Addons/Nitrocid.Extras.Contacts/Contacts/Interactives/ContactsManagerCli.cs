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
using Nitrocid.Base.Kernel.Debugging;
using Terminaux.Inputs.Interactive;
using Terminaux.Inputs.Styles.Infobox;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Text.Probers.Regexp;
using Textify.General;
using VisualCard.Parts.Implementations;
using Terminaux.Images;
using Terminaux.Colors;
using System.IO;
using Terminaux.Base;
using Terminaux.Colors.Themes.Colors;
using VisualCard.Parts.Enums;
using Nitrocid.Base.Files;
using System.Linq;
using Terminaux.Inputs.Styles;

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
                return LanguageTools.GetLocalized("NKS_CONTACTS_TUI_NOCONTACT");

            // Generate the rendered text
            string finalRenderedContactPicture =
                ContactsInit.ContactsConfig.ShowImages ?
                GetContactPictureFinal(selectedContact, (ConsoleWrapper.WindowWidth / 2) - 4, ConsoleWrapper.WindowHeight / 2, ThemeColorsTools.GetColor(ThemeColorType.TuiBackground)) :
                "";
            string finalRenderedContactName = GetContactNamesFinal(selectedContact, false);
            string finalRenderedContactAddress = GetContactAddressFinal(selectedContact, false);
            string finalRenderedContactMail = GetContactMailsFinal(selectedContact, false);
            string finalRenderedContactOrganization = GetContactOrganizationsFinal(selectedContact, false);
            string finalRenderedContactTelephone = GetContactTelephonesFinal(selectedContact, false);
            string finalRenderedContactURL = GetContactURLsFinal(selectedContact, false);

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
            string finalRenderedContactName = GetContactNamesFinal(selectedContact);

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
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_CANTIMPORTCONTACTS") + ex.Message, Settings.InfoBoxSettings);
            }
        }

        internal void ImportContactsFrom()
        {
            // Now, render the search box
            string path = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_PATHVCFPROMPT"), Settings.InfoBoxSettings);
            if (FilesystemTools.FileExists(path))
            {
                try
                {
                    // Initiate installation
                    ContactsManager.InstallContacts(path);
                }
                catch
                {
                    InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_CONTACTS_CONTACTFILEINVALID_UNNAMED"), Settings.InfoBoxSettings);
                }
            }
            else
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_FILENOTFOUND"), Settings.InfoBoxSettings);
        }

        internal void ImportContactFromMeCard()
        {
            // Now, render the search box
            string meCard = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_MECARDPROMPT"), Settings.InfoBoxSettings);
            if (!string.IsNullOrEmpty(meCard))
            {
                try
                {
                    // Initiate installation
                    ContactsManager.InstallContactFromMeCard(meCard);
                }
                catch
                {
                    InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_MECARDINVALID"), Settings.InfoBoxSettings);
                }
            }
            else
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_MECARDEMPTY"), Settings.InfoBoxSettings);
        }

        internal void ShowContactInfo(int index)
        {
            // Render the final information string
            var finalInfoRendered = new StringBuilder();
            string finalRenderedContactName = GetContactNamesFinal(index, false);
            finalInfoRendered.AppendLine(finalRenderedContactName);
            string finalRenderedContactAddress = GetContactAddressFinal(index, false);
            finalInfoRendered.AppendLine(finalRenderedContactAddress);
            string finalRenderedContactMail = GetContactMailsFinal(index, false);
            finalInfoRendered.AppendLine(finalRenderedContactMail);
            string finalRenderedContactOrganization = GetContactOrganizationsFinal(index, false);
            finalInfoRendered.AppendLine(finalRenderedContactOrganization);
            string finalRenderedContactTelephone = GetContactTelephonesFinal(index, false);
            finalInfoRendered.AppendLine(finalRenderedContactTelephone);
            string finalRenderedContactURL = GetContactURLsFinal(index, false);
            finalInfoRendered.AppendLine(finalRenderedContactURL);
            string finalRenderedContactGeo = GetContactGeosFinal(index, false);
            finalInfoRendered.AppendLine(finalRenderedContactGeo);
            string finalRenderedContactImpps = GetContactImppsFinal(index, false);
            finalInfoRendered.AppendLine(finalRenderedContactImpps);
            string finalRenderedContactNicknames = GetContactNicknamesFinal(index, false);
            finalInfoRendered.AppendLine(finalRenderedContactNicknames);
            string finalRenderedContactRoles = GetContactRolesFinal(index, false);
            finalInfoRendered.AppendLine(finalRenderedContactRoles);
            string finalRenderedContactTitles = GetContactTitlesFinal(index, false);
            finalInfoRendered.AppendLine(finalRenderedContactTitles);
            string finalRenderedContactNotes = GetContactNotesFinal(index, false);
            finalInfoRendered.AppendLine(finalRenderedContactNotes);

            // If there is a profile picture and preview is enabled, print it
            string picture =
                ContactsInit.ContactsConfig.ShowImages ?
                GetContactPictureFinal(index, ConsoleWrapper.WindowWidth - 8, ConsoleWrapper.WindowHeight, ThemeColorsTools.GetColor(ThemeColorType.TuiBoxBackground)) :
                "";
            if (!string.IsNullOrEmpty(picture))
            {
                finalInfoRendered.AppendLine("\n");
                finalInfoRendered.AppendLine(picture);
                finalInfoRendered.Append(ColorTools.RenderSetConsoleColor(ThemeColorsTools.GetColor(ThemeColorType.TuiBoxBackground), true));
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
            string exp = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_SEARCHPROMPT"), Settings.InfoBoxSettings);
            if (RegexpTools.IsValidRegex(exp))
            {
                // Initiate the search
                var foundCard = ContactsManager.SearchNext(exp);
                if (foundCard is null)
                    InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_NOCONTACTSSEARCH"), Settings.InfoBoxSettings);
                UpdateIndex(foundCard);
            }
            else
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_CONTACTS_EXCEPTION_INVALIDREGEX"), Settings.InfoBoxSettings);
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

        internal string GetContactNamesFinal(int index, bool single = true)
        {
            // Render the final information string
            var card = ContactsManager.GetContact(index);
            return GetContactNamesFinal(card, single);
        }

        internal string GetContactNamesFinal(Card card, bool single = true)
        {
            // Render the final information string
            var finalInfoRendered = new StringBuilder();
            bool hasName = card.GetPartsArray<NameInfo>().Length != 0;

            if (hasName)
            {
                if (single)
                    finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_CONTACTNAME") + $": {card.GetString(CardStringsEnum.FullName)[0].Value}");
                else
                {
                    finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_CONTACTNAME") + $": ");
                    var names = card.GetString(CardStringsEnum.FullName);
                    for (int i = 0; i < names.Length; i++)
                    {
                        var name = names[i];
                        finalInfoRendered.Append(name.Value);
                        if (i < names.Length - 1)
                            finalInfoRendered.Append(" | ");
                    }
                }
            }
            else
                finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_NOCONTACTNAME"));

            // Now, return the value
            return finalInfoRendered.ToString();
        }

        internal string GetContactAddressFinal(int index, bool single = true)
        {
            // Render the final information string
            var card = ContactsManager.GetContact(index);
            return GetContactAddressFinal(card, single);
        }

        internal string GetContactAddressFinal(Card card, bool single = true)
        {
            // Render the final information string
            var finalInfoRendered = new StringBuilder();
            bool hasAddress = card.GetPartsArray<AddressInfo>().Length != 0;

            if (hasAddress)
            {
                finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_ADDRESS") + ": ");

                var addresses = card.GetPartsArray<AddressInfo>();
                for (int i = 0; i < (single ? 1 : addresses.Length); i++)
                {
                    var address = card.GetPartsArray<AddressInfo>()[i];
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
                    if (i < addresses.Length - 1)
                        finalInfoRendered.Append(" | ");
                }
            }
            else
                // TODO: NKS_CONTACTS_TUI_NOADDRESS -> "No contact address"
                finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_NOADDRESS"));

            // Now, return the value
            return finalInfoRendered.ToString();
        }

        internal string GetContactMailsFinal(int index, bool single = true)
        {
            // Render the final information string
            var card = ContactsManager.GetContact(index);
            return GetContactMailsFinal(card, single);
        }

        internal string GetContactMailsFinal(Card card, bool single = true)
        {
            // Render the final information string
            var finalInfoRendered = new StringBuilder();
            bool hasMail = card.GetString(CardStringsEnum.Mails).Length != 0;

            if (hasMail)
            {
                if (single)
                    finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_MAIL") + $": {card.GetString(CardStringsEnum.Mails)[0].Value}");
                else
                {
                    finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_MAIL") + $": ");
                    var names = card.GetString(CardStringsEnum.Mails);
                    for (int i = 0; i < names.Length; i++)
                    {
                        var name = names[i];
                        finalInfoRendered.Append(name.Value);
                        if (i < names.Length - 1)
                            finalInfoRendered.Append(" | ");
                    }
                }
            }
            else
                finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_NOMAIL"));

            // Now, return the value
            return finalInfoRendered.ToString();
        }

        internal string GetContactOrganizationsFinal(int index, bool single = true)
        {
            // Render the final information string
            var card = ContactsManager.GetContact(index);
            return GetContactOrganizationsFinal(card, single);
        }

        internal string GetContactOrganizationsFinal(Card card, bool single = true)
        {
            // Render the final information string
            var finalInfoRendered = new StringBuilder();
            bool hasOrganization = card.GetPartsArray<OrganizationInfo>().Length != 0;

            if (hasOrganization)
            {
                finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_ORG") + ": ");

                var orgs = card.GetPartsArray<OrganizationInfo>();
                for (int i = 0; i < (single ? 1 : orgs.Length); i++)
                {
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
                    if (i < orgs.Length - 1)
                        finalInfoRendered.Append(" | ");
                }
            }
            else
                finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_NOORG"));

            // Now, return the value
            return finalInfoRendered.ToString();
        }

        internal string GetContactTelephonesFinal(int index, bool single = true)
        {
            // Render the final information string
            var card = ContactsManager.GetContact(index);
            return GetContactTelephonesFinal(card, single);
        }

        internal string GetContactTelephonesFinal(Card card, bool single = true)
        {
            // Render the final information string
            var finalInfoRendered = new StringBuilder();
            bool hasTelephone = card.GetString(CardStringsEnum.Telephones).Length != 0;

            if (hasTelephone)
            {
                if (single)
                    finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_PHONE") + $": {card.GetString(CardStringsEnum.Telephones)[0].Value}");
                else
                {
                    finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_PHONE") + $": ");
                    var names = card.GetString(CardStringsEnum.Telephones);
                    for (int i = 0; i < names.Length; i++)
                    {
                        var name = names[i];
                        finalInfoRendered.Append(name.Value);
                        if (i < names.Length - 1)
                            finalInfoRendered.Append(" | ");
                    }
                }
            }
            else
                finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_NOPHONE"));

            // Now, return the value
            return finalInfoRendered.ToString();
        }

        internal string GetContactURLsFinal(int index, bool single = true)
        {
            // Render the final information string
            var card = ContactsManager.GetContact(index);
            return GetContactURLsFinal(card, single);
        }

        internal string GetContactURLsFinal(Card card, bool single = true)
        {
            // Render the final information string
            var finalInfoRendered = new StringBuilder();
            bool hasURL = card.GetString(CardStringsEnum.Url).Length != 0;

            if (hasURL)
            {
                if (single)
                    finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_URL") + $": {card.GetString(CardStringsEnum.Url)[0].Value}");
                else
                {
                    finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_URL") + $": ");
                    var names = card.GetString(CardStringsEnum.Url);
                    for (int i = 0; i < names.Length; i++)
                    {
                        var name = names[i];
                        finalInfoRendered.Append(name.Value);
                        if (i < names.Length - 1)
                            finalInfoRendered.Append(" | ");
                    }
                }
            }
            else
                finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_NOURL"));

            // Now, return the value
            return finalInfoRendered.ToString();
        }

        internal string GetContactGeosFinal(int index, bool single = true)
        {
            // Render the final information string
            var card = ContactsManager.GetContact(index);
            return GetContactGeosFinal(card, single);
        }

        internal string GetContactGeosFinal(Card card, bool single = true)
        {
            // Render the final information string
            var finalInfoRendered = new StringBuilder();
            bool hasGeo = card.GetString(CardStringsEnum.Geo).Length != 0;

            if (hasGeo)
            {
                if (single)
                    finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_GEO") + $": {card.GetString(CardStringsEnum.Geo)[0].Value}");
                else
                {
                    finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_GEO") + $": ");
                    var names = card.GetString(CardStringsEnum.Geo);
                    for (int i = 0; i < names.Length; i++)
                    {
                        var name = names[i];
                        finalInfoRendered.Append(name.Value);
                        if (i < names.Length - 1)
                            finalInfoRendered.Append(" | ");
                    }
                }
            }
            else
                finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_NOGEO"));

            // Now, return the value
            return finalInfoRendered.ToString();
        }

        internal string GetContactImppsFinal(int index, bool single = true)
        {
            // Render the final information string
            var card = ContactsManager.GetContact(index);
            return GetContactImppsFinal(card, single);
        }

        internal string GetContactImppsFinal(Card card, bool single = true)
        {
            // Render the final information string
            var finalInfoRendered = new StringBuilder();
            bool hasImpp = card.GetString(CardStringsEnum.Impps).Length != 0;

            if (hasImpp)
            {
                if (single)
                    finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_IMPP") + $": {card.GetString(CardStringsEnum.Impps)[0].Value}");
                else
                {
                    finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_IMPP") + $": ");
                    var names = card.GetString(CardStringsEnum.Impps);
                    for (int i = 0; i < names.Length; i++)
                    {
                        var name = names[i];
                        finalInfoRendered.Append(name.Value);
                        if (i < names.Length - 1)
                            finalInfoRendered.Append(" | ");
                    }
                }
            }
            else
                finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_NOIMPP"));

            // Now, return the value
            return finalInfoRendered.ToString();
        }

        internal string GetContactNicknamesFinal(int index, bool single = true)
        {
            // Render the final information string
            var card = ContactsManager.GetContact(index);
            return GetContactNicknamesFinal(card, single);
        }

        internal string GetContactNicknamesFinal(Card card, bool single = true)
        {
            // Render the final information string
            var finalInfoRendered = new StringBuilder();
            bool hasNickname = card.GetString(CardStringsEnum.Nicknames).Length != 0;

            if (hasNickname)
            {
                if (single)
                    finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_NICK") + $": {card.GetString(CardStringsEnum.Nicknames)[0].Value}");
                else
                {
                    finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_NICK") + $": ");
                    var names = card.GetString(CardStringsEnum.Nicknames);
                    for (int i = 0; i < names.Length; i++)
                    {
                        var name = names[i];
                        finalInfoRendered.Append(name.Value);
                        if (i < names.Length - 1)
                            finalInfoRendered.Append(" | ");
                    }
                }
            }
            else
                finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_NONICK"));

            // Now, return the value
            return finalInfoRendered.ToString();
        }

        internal string GetContactRolesFinal(int index, bool single = true)
        {
            // Render the final information string
            var card = ContactsManager.GetContact(index);
            return GetContactRolesFinal(card, single);
        }

        internal string GetContactRolesFinal(Card card, bool single = true)
        {
            // Render the final information string
            var finalInfoRendered = new StringBuilder();
            bool hasRoles = card.GetString(CardStringsEnum.Roles).Length != 0;

            if (hasRoles)
            {
                if (single)
                    finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_ROLE") + $": {card.GetString(CardStringsEnum.Roles)[0].Value}");
                else
                {
                    finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_ROLE") + $": ");
                    var names = card.GetString(CardStringsEnum.Roles);
                    for (int i = 0; i < names.Length; i++)
                    {
                        var name = names[i];
                        finalInfoRendered.Append(name.Value);
                        if (i < names.Length - 1)
                            finalInfoRendered.Append(" | ");
                    }
                }
            }
            else
                finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_NOROLE"));

            // Now, return the value
            return finalInfoRendered.ToString();
        }

        internal string GetContactTitlesFinal(int index, bool single = true)
        {
            // Render the final information string
            var card = ContactsManager.GetContact(index);
            return GetContactTitlesFinal(card, single);
        }

        internal string GetContactTitlesFinal(Card card, bool single = true)
        {
            // Render the final information string
            var finalInfoRendered = new StringBuilder();
            bool hasTitles = card.GetString(CardStringsEnum.Titles).Length != 0;

            if (hasTitles)
            {
                if (single)
                    finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_TITLE") + $": {card.GetString(CardStringsEnum.Titles)[0].Value}");
                else
                {
                    finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_TITLE") + $": ");
                    var names = card.GetString(CardStringsEnum.Titles);
                    for (int i = 0; i < names.Length; i++)
                    {
                        var name = names[i];
                        finalInfoRendered.Append(name.Value);
                        if (i < names.Length - 1)
                            finalInfoRendered.Append(" | ");
                    }
                }
            }
            else
                finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_NOTITLE"));

            // Now, return the value
            return finalInfoRendered.ToString();
        }

        internal string GetContactNotesFinal(int index, bool single = true)
        {
            // Render the final information string
            var card = ContactsManager.GetContact(index);
            return GetContactNotesFinal(card, single);
        }

        internal string GetContactNotesFinal(Card card, bool single = true)
        {
            // Render the final information string
            var finalInfoRendered = new StringBuilder();
            bool hasNotes = card.GetString(CardStringsEnum.Notes).Length > 0;

            if (hasNotes)
            {
                if (single)
                    finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_NOTES") + $": {card.GetString(CardStringsEnum.Notes)[0].Value}");
                else
                {
                    finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_NOTES") + $": ");
                    var names = card.GetString(CardStringsEnum.Notes);
                    for (int i = 0; i < names.Length; i++)
                    {
                        var name = names[i];
                        finalInfoRendered.Append(name.Value);
                        if (i < names.Length - 1)
                            finalInfoRendered.Append(" | ");
                    }
                }
            }
            else
                finalInfoRendered.Append(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_NONOTES"));

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

        internal void AddContact()
        {
            // Add a placeholder contact for the editing TUI to open
            var newCard = new Card(new(2, 1));
            newCard.AddString(CardStringsEnum.FullName, "Unnamed contact");
            newCard.AddPartToArray<NameInfo>("contact;Unnamed;;;");
            ContactsManager.cards.Add(newCard);

            // Save all changes
            ContactsManager.SaveContacts();

            // Open this TUI
            EditContact(newCard);
        }

        internal void EditContact(Card? card)
        {
            if (card is null)
                return;

            // Ask what to edit
            bool editing = true;
            while (editing)
            {
                // Let the user choose what to edit
                List<InputChoiceInfo> choiceInfos =
                [
                    new("1", GetContactNamesFinal(card, false)),
                    new("2", GetContactAddressFinal(card, false)),
                    new("3", GetContactMailsFinal(card, false)),
                    new("4", GetContactOrganizationsFinal(card, false)),
                    new("5", GetContactTelephonesFinal(card, false)),
                    new("6", GetContactURLsFinal(card, false)),
                    new("7", GetContactGeosFinal(card, false)),
                    new("8", GetContactImppsFinal(card, false)),
                    new("9", GetContactNicknamesFinal(card, false)),
                    new("10", GetContactRolesFinal(card, false)),
                    new("11", GetContactTitlesFinal(card, false)),
                    new("12", GetContactNotesFinal(card, false)),
                    // TODO: NKS_CONTACTS_TUI_CONTACTHASPICTURE -> "Contains profile picture"
                    // TODO: NKS_CONTACTS_TUI_CONTACTHASNOPICTURE -> "Doesn't contain profile picture"
                    new("13", card.GetPartsArray<PhotoInfo>().Length > 0 ? LanguageTools.GetLocalized("NKS_CONTACTS_TUI_CONTACTHASPICTURE") : LanguageTools.GetLocalized("NKS_CONTACTS_TUI_CONTACTHASNOPICTURE")),
                ];
                choiceInfos.Add(new($"{choiceInfos.Count}", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_EXIT")));
                // TODO: NKS_CONTACTS_TUI_CONTACTEDITPROMPT -> "Editing the contact"
                int editIndex = InfoBoxSelectionColor.WriteInfoBoxSelection([.. choiceInfos], LanguageTools.GetLocalized("NKS_CONTACTS_TUI_CONTACTEDITPROMPT") + $": {GetContactNamesFinal(card)}", Settings.InfoBoxSettings);

                // Check to see if we pressed Exit or not
                if (editIndex == -1 || editIndex == choiceInfos.Count - 1)
                    editing = false;
                else
                {
                    // Select the editing cases based on the index number
                    switch (editIndex)
                    {
                        case 0:
                            EditName(card);
                            break;
                    }

                    // Save all changes
                    ContactsManager.SaveContacts();
                }
            }
        }

        internal void EditName(Card? card)
        {
            if (card is null)
                return;

            // Ask for the new name
            string newName = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_CONTACTNEWNAMEPROMPT") + $": {GetContactNamesFinal(card)}", Settings.InfoBoxSettings).Trim();
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
                card.AddPartToArray<NameInfo>($"{(hasLastName ? splitName[1] : "")};{splitName[0]};{(hasExtras ? splitName[2] : "")};;");
            if (stringsName.Length > 0)
                stringsName[0].Value = newName;
            else
                card.AddString(CardStringsEnum.FullName, newName);
        }
    }
}
