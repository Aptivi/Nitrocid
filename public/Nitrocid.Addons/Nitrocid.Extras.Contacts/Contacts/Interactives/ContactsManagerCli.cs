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
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Nitrocid.Base.Drivers;
using Nitrocid.Base.Drivers.Encoding;
using Nitrocid.Base.Drivers.EncodingAsymmetric;
using Nitrocid.Base.Drivers.EncodingAsymmetric.Bases;
using Nitrocid.Base.Files;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Reflection;
using Nitrocid.Base.Misc.Text.Probers.Regexp;
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Images;
using Terminaux.Inputs.Interactive;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Styles.Infobox;
using Textify.General;
using VisualCard.Parts;
using VisualCard.Parts.Enums;
using VisualCard.Parts.Implementations;

namespace Nitrocid.Extras.Contacts.Contacts.Interactives
{
    /// <summary>
    /// Contacts manager class
    /// </summary>
    public class ContactsManagerCli : BaseInteractiveTui<Card>, IInteractiveTui<Card>
    {
        /// <inheritdoc/>
        public override InteractiveTuiHelpPage[] HelpPages =>
        [
            new()
            {
                HelpTitle = /* Localizable */ "NKS_CONTACTS_TUI_HELP01_TITLE",
                HelpDescription = /* Localizable */ "NKS_CONTACTS_TUI_HELP01_DESC",
                HelpBody =
                    LanguageTools.GetLocalized("NKS_CONTACTS_TUI_HELP01_BODY") + "\n\n" +
#pragma warning disable NLOC0001
                    LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_COMMON_HELP_MOREINFO") + ": https://aptivi.gitbook.io/aptivi/nitrocid-ks-manual/fundamentals/simulated-kernel-features/extra-features/common-programs/contacts",
#pragma warning restore NLOC0001
            }
        ];

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
                    string addressString = PrintAddress(address);
                    finalInfoRendered.Append(addressString);
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
                    string orgStr = PrintOrganization(org);
                    finalInfoRendered.Append(orgStr);
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
                choiceInfos.Add(new($"{choiceInfos.Count + 1}", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_EXIT")));
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
                        case 1:
                            EditAddress(card);
                            break;
                        case 2:
                            EditMail(card);
                            break;
                        case 3:
                            EditOrganization(card);
                            break;
                        case 4:
                            EditTelephone(card);
                            break;
                        case 5:
                            EditUrl(card);
                            break;
                        case 6:
                            EditGeo(card);
                            break;
                        case 7:
                            EditImpp(card);
                            break;
                        case 8:
                            EditNickname(card);
                            break;
                        case 9:
                            EditRole(card);
                            break;
                        case 10:
                            EditTitle(card);
                            break;
                        case 11:
                            EditNote(card);
                            break;
                        case 12:
                            EditPicture(card);
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

        internal void EditAddress(Card? card)
        {
            if (card is null)
                return;

            // Now, open the infobox that lets you select an address, add a new one, and save all
            bool editing = true;
            while (editing)
            {
                var addresses = card.GetPartsArray<AddressInfo>();

                List<InputChoiceInfo> choiceInfos = [];
                for (int i = 0; i < addresses.Length; i++)
                {
                    AddressInfo address = addresses[i];
                    choiceInfos.Add(new($"{i + 1}", PrintAddress(address)));
                }
                choiceInfos.Add(new($"{choiceInfos.Count + 1}", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_KEYBINDING_ADD")));
                choiceInfos.Add(new($"{choiceInfos.Count + 1}", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_KEYBINDING_SAVE")));
                // TODO: NKS_CONTACTS_TUI_CONTACTADDRSEDITPROMPT -> "Editing the contact addresses of"
                int editIndex = InfoBoxSelectionColor.WriteInfoBoxSelection([.. choiceInfos], LanguageTools.GetLocalized("NKS_CONTACTS_TUI_CONTACTADDRSEDITPROMPT") + $" {GetContactNamesFinal(card)}", Settings.InfoBoxSettings);

                // Check to see if we pressed Exit or not
                if (editIndex == -1 || editIndex == choiceInfos.Count - 1)
                    editing = false;
                else if (editIndex == choiceInfos.Count - 2)
                    OpenAddressEditor(card, true);
                else
                    OpenAddressEditor(card, idx: editIndex);

                // Save all changes
                ContactsManager.SaveContacts();
            }
        }

        private void OpenAddressEditor(Card card, bool add = false, int idx = 0)
        {
            // Add the new address part, as necessary
            if (add)
            {
                card.AddPartToArray<AddressInfo>(";;;;;;");

                // Save all changes
                ContactsManager.SaveContacts();
            }

            // Determine the final index
            var addresses = card.GetPartsArray<AddressInfo>();
            int finalIdx = add ? addresses.Length - 1 : idx;
            var address = addresses[finalIdx];

            // Open the address editor to this specific address
            bool editing = true;
            while (editing)
            {
                List<InputChoiceInfo> choiceInfos =
                [
                    new("1", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_ADDRESSINFO_POBOX") + $": {address.PostOfficeBox}"),
                    new("2", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_ADDRESSINFO_EXTADDR") + $": {address.ExtendedAddress}"),
                    new("3", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_ADDRESSINFO_STRADDR") + $": {address.StreetAddress}"),
                    new("4", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_ADDRESSINFO_LOCALITY") + $": {address.Locality}"),
                    new("5", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_ADDRESSINFO_REGION") + $": {address.Region}"),
                    new("6", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_ADDRESSINFO_POSTALCODE") + $": {address.PostalCode}"),
                    new("7", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_ADDRESSINFO_COUNTRY") + $": {address.Country}"),
                ];
                choiceInfos.Add(new($"{choiceInfos.Count + 1}", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_KEYBINDING_DELETE")));
                choiceInfos.Add(new($"{choiceInfos.Count + 1}", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_KEYBINDING_SAVE")));
                // TODO: NKS_CONTACTS_TUI_CONTACTADDREDITPROMPT -> "Editing the contact address of"
                int editIndex = InfoBoxSelectionColor.WriteInfoBoxSelection([.. choiceInfos], LanguageTools.GetLocalized("NKS_CONTACTS_TUI_CONTACTADDREDITPROMPT") + $" {GetContactNamesFinal(card)}", Settings.InfoBoxSettings);

                // Check to see if we pressed Exit or not
                if (editIndex == -1 || editIndex == choiceInfos.Count - 1)
                    editing = false;
                else if (editIndex == choiceInfos.Count - 2)
                {
                    card.DeletePartsArray<AddressInfo>(finalIdx);
                    editing = false;
                }
                else
                {
                    // Select the editing cases based on the index number
                    switch (editIndex)
                    {
                        case 0:
                            string newPoBox = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_ADDRESSINFO_POBOXPROMPT"));
                            address.PostOfficeBox = newPoBox;
                            break;
                        case 1:
                            string newExtAddr = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_ADDRESSINFO_EXTADDRPROMPT"));
                            address.ExtendedAddress = newExtAddr;
                            break;
                        case 2:
                            string newStrAddr = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_ADDRESSINFO_STRADDRPROMPT"));
                            address.StreetAddress = newStrAddr;
                            break;
                        case 3:
                            string newLocality = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_ADDRESSINFO_LOCALITYPROMPT"));
                            address.Locality = newLocality;
                            break;
                        case 4:
                            string newRegion = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_ADDRESSINFO_REGIONPROMPT"));
                            address.Region = newRegion;
                            break;
                        case 5:
                            string newPostalCode = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_ADDRESSINFO_POSTALCODEPROMPT"));
                            address.PostalCode = newPostalCode;
                            break;
                        case 6:
                            string newCountry = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_ADDRESSINFO_COUNTRYPROMPT"));
                            address.Country = newCountry;
                            break;
                    }

                    // Save all changes
                    ContactsManager.SaveContacts();
                }
            }
        }

        private string PrintAddress(AddressInfo address)
        {
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
            return string.Join(", ", fullElements);
        }

        internal void EditMail(Card? card)
        {
            if (card is null)
                return;

            // Now, open the infobox that lets you select a mail, add a new one, and save all
            bool editing = true;
            while (editing)
            {
                var mails = card.GetString(CardStringsEnum.Mails);

                List<InputChoiceInfo> choiceInfos = [];
                for (int i = 0; i < mails.Length; i++)
                {
                    string mail = mails[i].Value;
                    choiceInfos.Add(new($"{i + 1}", mail));
                }
                choiceInfos.Add(new($"{choiceInfos.Count + 1}", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_KEYBINDING_ADD")));
                choiceInfos.Add(new($"{choiceInfos.Count + 1}", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_KEYBINDING_SAVE")));
                // TODO: NKS_CONTACTS_TUI_CONTACTMAILSEDITPROMPT -> "Editing the contact mails of"
                int editIndex = InfoBoxSelectionColor.WriteInfoBoxSelection([.. choiceInfos], LanguageTools.GetLocalized("NKS_CONTACTS_TUI_CONTACTMAILSEDITPROMPT") + $" {GetContactNamesFinal(card)}", Settings.InfoBoxSettings);

                // Check to see if we pressed Exit or not
                if (editIndex == -1 || editIndex == choiceInfos.Count - 1)
                    editing = false;
                else if (editIndex == choiceInfos.Count - 2)
                    OpenMailEditor(card, true);
                else
                    OpenMailEditor(card, idx: editIndex);

                // Save all changes
                ContactsManager.SaveContacts();
            }
        }

        private void OpenMailEditor(Card card, bool add = false, int idx = 0)
        {
            // Add the new mail part, as necessary
            if (add)
            {
                card.AddString(CardStringsEnum.Mails, "newmail@host.com");

                // Save all changes
                ContactsManager.SaveContacts();
            }

            // Determine the final index
            var mails = card.GetString(CardStringsEnum.Mails);
            int finalIdx = add ? mails.Length - 1 : idx;
            var mail = mails[finalIdx];

            // Open the mail editor to this specific mail
            bool editing = true;
            while (editing)
            {
                List<InputChoiceInfo> choiceInfos =
                [
                    new("1", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_MAILINFO_ADDRESS") + $": {mail.Value}"),
                ];
                choiceInfos.Add(new($"{choiceInfos.Count + 1}", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_KEYBINDING_DELETE")));
                choiceInfos.Add(new($"{choiceInfos.Count + 1}", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_KEYBINDING_SAVE")));
                // TODO: NKS_CONTACTS_TUI_CONTACTMAILEDITPROMPT -> "Editing the contact mail of"
                int editIndex = InfoBoxSelectionColor.WriteInfoBoxSelection([.. choiceInfos], LanguageTools.GetLocalized("NKS_CONTACTS_TUI_CONTACTMAILEDITPROMPT") + $" {GetContactNamesFinal(card)}", Settings.InfoBoxSettings);

                // Check to see if we pressed Exit or not
                if (editIndex == -1 || editIndex == choiceInfos.Count - 1)
                    editing = false;
                else if (editIndex == choiceInfos.Count - 2)
                {
                    card.DeleteString(CardStringsEnum.Mails, finalIdx);
                    editing = false;
                }
                else
                {
                    // Select the editing cases based on the index number
                    switch (editIndex)
                    {
                        case 0:
                            string newMail = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_MAILINFO_ADDRESSPROMPT"));
                            mail.Value = newMail;
                            break;
                    }

                    // Save all changes
                    ContactsManager.SaveContacts();
                }
            }
        }

        internal void EditOrganization(Card? card)
        {
            if (card is null)
                return;

            // Now, open the infobox that lets you select an organization, add a new one, and save all
            bool editing = true;
            while (editing)
            {
                var organizations = card.GetPartsArray<OrganizationInfo>();

                List<InputChoiceInfo> choiceInfos = [];
                for (int i = 0; i < organizations.Length; i++)
                {
                    OrganizationInfo organization = organizations[i];
                    choiceInfos.Add(new($"{i + 1}", PrintOrganization(organization)));
                }
                choiceInfos.Add(new($"{choiceInfos.Count + 1}", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_KEYBINDING_ADD")));
                choiceInfos.Add(new($"{choiceInfos.Count + 1}", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_KEYBINDING_SAVE")));
                // TODO: NKS_CONTACTS_TUI_CONTACTORGSEDITPROMPT -> "Editing the contact organizations of"
                int editIndex = InfoBoxSelectionColor.WriteInfoBoxSelection([.. choiceInfos], LanguageTools.GetLocalized("NKS_CONTACTS_TUI_CONTACTORGSEDITPROMPT") + $" {GetContactNamesFinal(card)}", Settings.InfoBoxSettings);

                // Check to see if we pressed Exit or not
                if (editIndex == -1 || editIndex == choiceInfos.Count - 1)
                    editing = false;
                else if (editIndex == choiceInfos.Count - 2)
                    OpenOrganizationEditor(card, true);
                else
                    OpenOrganizationEditor(card, idx: editIndex);

                // Save all changes
                ContactsManager.SaveContacts();
            }
        }

        private void OpenOrganizationEditor(Card card, bool add = false, int idx = 0)
        {
            // Add the new organization part, as necessary
            if (add)
            {
                card.AddPartToArray<OrganizationInfo>(";;");

                // Save all changes
                ContactsManager.SaveContacts();
            }

            // Determine the final index
            var organizations = card.GetPartsArray<OrganizationInfo>();
            int finalIdx = add ? organizations.Length - 1 : idx;
            var organization = organizations[finalIdx];

            // Open the organization editor to this specific organization
            bool editing = true;
            while (editing)
            {
                List<InputChoiceInfo> choiceInfos =
                [
                    new("1", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_ORGANIZATIONINFO_NAME") + $": {organization.Name}"),
                    new("2", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_ORGANIZATIONINFO_UNIT") + $": {organization.Unit}"),
                    new("3", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_ORGANIZATIONINFO_ROLE") + $": {organization.Role}"),
                ];
                choiceInfos.Add(new($"{choiceInfos.Count + 1}", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_KEYBINDING_DELETE")));
                choiceInfos.Add(new($"{choiceInfos.Count + 1}", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_KEYBINDING_SAVE")));
                // TODO: NKS_CONTACTS_TUI_CONTACTORGEDITPROMPT -> "Editing the contact organization of"
                int editIndex = InfoBoxSelectionColor.WriteInfoBoxSelection([.. choiceInfos], LanguageTools.GetLocalized("NKS_CONTACTS_TUI_CONTACTORGEDITPROMPT") + $" {GetContactNamesFinal(card)}", Settings.InfoBoxSettings);

                // Check to see if we pressed Exit or not
                if (editIndex == -1 || editIndex == choiceInfos.Count - 1)
                    editing = false;
                else if (editIndex == choiceInfos.Count - 2)
                {
                    card.DeletePartsArray<OrganizationInfo>(finalIdx);
                    editing = false;
                }
                else
                {
                    // Select the editing cases based on the index number
                    switch (editIndex)
                    {
                        case 0:
                            string newName = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_ORGANIZATIONINFO_NAMEPROMPT"));
                            organization.Name = newName;
                            break;
                        case 1:
                            string newUnit = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_ORGANIZATIONINFO_UNITPROMPT"));
                            organization.Unit = newUnit;
                            break;
                        case 2:
                            string newRole = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_ORGANIZATIONINFO_ROLEPROMPT"));
                            organization.Role = newRole;
                            break;
                    }

                    // Save all changes
                    ContactsManager.SaveContacts();
                }
            }
        }

        private string PrintOrganization(OrganizationInfo org)
        {
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
            return string.Join(", ", fullElements);
        }

        internal void EditTelephone(Card? card)
        {
            if (card is null)
                return;

            // Now, open the infobox that lets you select a telephone, add a new one, and save all
            bool editing = true;
            while (editing)
            {
                var telephones = card.GetString(CardStringsEnum.Telephones);

                List<InputChoiceInfo> choiceInfos = [];
                for (int i = 0; i < telephones.Length; i++)
                {
                    string telephone = telephones[i].Value;
                    choiceInfos.Add(new($"{i + 1}", telephone));
                }
                choiceInfos.Add(new($"{choiceInfos.Count + 1}", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_KEYBINDING_ADD")));
                choiceInfos.Add(new($"{choiceInfos.Count + 1}", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_KEYBINDING_SAVE")));
                // TODO: NKS_CONTACTS_TUI_CONTACTTELEPHONESEDITPROMPT -> "Editing the contact telephones of"
                int editIndex = InfoBoxSelectionColor.WriteInfoBoxSelection([.. choiceInfos], LanguageTools.GetLocalized("NKS_CONTACTS_TUI_CONTACTTELEPHONESEDITPROMPT") + $" {GetContactNamesFinal(card)}", Settings.InfoBoxSettings);

                // Check to see if we pressed Exit or not
                if (editIndex == -1 || editIndex == choiceInfos.Count - 1)
                    editing = false;
                else if (editIndex == choiceInfos.Count - 2)
                    OpenTelephoneEditor(card, true);
                else
                    OpenTelephoneEditor(card, idx: editIndex);

                // Save all changes
                ContactsManager.SaveContacts();
            }
        }

        private void OpenTelephoneEditor(Card card, bool add = false, int idx = 0)
        {
            // Add the new telephone part, as necessary
            if (add)
            {
                card.AddString(CardStringsEnum.Telephones, "0000000000");

                // Save all changes
                ContactsManager.SaveContacts();
            }

            // Determine the final index
            var telephones = card.GetString(CardStringsEnum.Telephones);
            int finalIdx = add ? telephones.Length - 1 : idx;
            var telephone = telephones[finalIdx];

            // Open the telephone editor to this specific telephone
            bool editing = true;
            while (editing)
            {
                List<InputChoiceInfo> choiceInfos =
                [
                    new("1", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_TELEPHONEINFO_NUMBER") + $": {telephone.Value}"),
                ];
                choiceInfos.Add(new($"{choiceInfos.Count + 1}", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_KEYBINDING_DELETE")));
                choiceInfos.Add(new($"{choiceInfos.Count + 1}", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_KEYBINDING_SAVE")));
                // TODO: NKS_CONTACTS_TUI_CONTACTTELEPHONEEDITPROMPT -> "Editing the contact telephone of"
                int editIndex = InfoBoxSelectionColor.WriteInfoBoxSelection([.. choiceInfos], LanguageTools.GetLocalized("NKS_CONTACTS_TUI_CONTACTTELEPHONEEDITPROMPT") + $" {GetContactNamesFinal(card)}", Settings.InfoBoxSettings);

                // Check to see if we pressed Exit or not
                if (editIndex == -1 || editIndex == choiceInfos.Count - 1)
                    editing = false;
                else if (editIndex == choiceInfos.Count - 2)
                {
                    card.DeleteString(CardStringsEnum.Telephones, finalIdx);
                    editing = false;
                }
                else
                {
                    // Select the editing cases based on the index number
                    switch (editIndex)
                    {
                        case 0:
                            string newTelephone = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_TELEPHONEINFO_NUMBERPROMPT"));
                            telephone.Value = newTelephone;
                            break;
                    }

                    // Save all changes
                    ContactsManager.SaveContacts();
                }
            }
        }

        internal void EditUrl(Card? card)
        {
            if (card is null)
                return;

            // Now, open the infobox that lets you select a url, add a new one, and save all
            bool editing = true;
            while (editing)
            {
                var urls = card.GetString(CardStringsEnum.Url);

                List<InputChoiceInfo> choiceInfos = [];
                for (int i = 0; i < urls.Length; i++)
                {
                    string url = urls[i].Value;
                    choiceInfos.Add(new($"{i + 1}", url));
                }
                choiceInfos.Add(new($"{choiceInfos.Count + 1}", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_KEYBINDING_ADD")));
                choiceInfos.Add(new($"{choiceInfos.Count + 1}", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_KEYBINDING_SAVE")));
                // TODO: NKS_CONTACTS_TUI_CONTACTURLSEDITPROMPT -> "Editing the contact urls of"
                int editIndex = InfoBoxSelectionColor.WriteInfoBoxSelection([.. choiceInfos], LanguageTools.GetLocalized("NKS_CONTACTS_TUI_CONTACTURLSEDITPROMPT") + $" {GetContactNamesFinal(card)}", Settings.InfoBoxSettings);

                // Check to see if we pressed Exit or not
                if (editIndex == -1 || editIndex == choiceInfos.Count - 1)
                    editing = false;
                else if (editIndex == choiceInfos.Count - 2)
                    OpenUrlEditor(card, true);
                else
                    OpenUrlEditor(card, idx: editIndex);

                // Save all changes
                ContactsManager.SaveContacts();
            }
        }

        private void OpenUrlEditor(Card card, bool add = false, int idx = 0)
        {
            // Add the new url part, as necessary
            if (add)
            {
                card.AddString(CardStringsEnum.Url, "https://example.com");

                // Save all changes
                ContactsManager.SaveContacts();
            }

            // Determine the final index
            var urls = card.GetString(CardStringsEnum.Url);
            int finalIdx = add ? urls.Length - 1 : idx;
            var url = urls[finalIdx];

            // Open the url editor to this specific url
            bool editing = true;
            while (editing)
            {
                List<InputChoiceInfo> choiceInfos =
                [
                    new("1", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_URLINFO_ADDRESS") + $": {url.Value}"),
                ];
                choiceInfos.Add(new($"{choiceInfos.Count + 1}", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_KEYBINDING_DELETE")));
                choiceInfos.Add(new($"{choiceInfos.Count + 1}", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_KEYBINDING_SAVE")));
                // TODO: NKS_CONTACTS_TUI_CONTACTURLEDITPROMPT -> "Editing the contact url of"
                int editIndex = InfoBoxSelectionColor.WriteInfoBoxSelection([.. choiceInfos], LanguageTools.GetLocalized("NKS_CONTACTS_TUI_CONTACTURLEDITPROMPT") + $" {GetContactNamesFinal(card)}", Settings.InfoBoxSettings);

                // Check to see if we pressed Exit or not
                if (editIndex == -1 || editIndex == choiceInfos.Count - 1)
                    editing = false;
                else if (editIndex == choiceInfos.Count - 2)
                {
                    card.DeleteString(CardStringsEnum.Url, finalIdx);
                    editing = false;
                }
                else
                {
                    // Select the editing cases based on the index number
                    switch (editIndex)
                    {
                        case 0:
                            string newUrl = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_URLINFO_ADDRESSPROMPT"));
                            url.Value = newUrl;
                            break;
                    }

                    // Save all changes
                    ContactsManager.SaveContacts();
                }
            }
        }

        internal void EditGeo(Card? card)
        {
            if (card is null)
                return;

            // Now, open the infobox that lets you select a geo, add a new one, and save all
            bool editing = true;
            while (editing)
            {
                var geos = card.GetString(CardStringsEnum.Geo);

                List<InputChoiceInfo> choiceInfos = [];
                for (int i = 0; i < geos.Length; i++)
                {
                    string geo = geos[i].Value;
                    choiceInfos.Add(new($"{i + 1}", geo));
                }
                choiceInfos.Add(new($"{choiceInfos.Count + 1}", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_KEYBINDING_ADD")));
                choiceInfos.Add(new($"{choiceInfos.Count + 1}", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_KEYBINDING_SAVE")));
                // TODO: NKS_CONTACTS_TUI_CONTACTGEOSEDITPROMPT -> "Editing the contact geos of"
                int editIndex = InfoBoxSelectionColor.WriteInfoBoxSelection([.. choiceInfos], LanguageTools.GetLocalized("NKS_CONTACTS_TUI_CONTACTGEOSEDITPROMPT") + $" {GetContactNamesFinal(card)}", Settings.InfoBoxSettings);

                // Check to see if we pressed Exit or not
                if (editIndex == -1 || editIndex == choiceInfos.Count - 1)
                    editing = false;
                else if (editIndex == choiceInfos.Count - 2)
                    OpenGeoEditor(card, true);
                else
                    OpenGeoEditor(card, idx: editIndex);

                // Save all changes
                ContactsManager.SaveContacts();
            }
        }

        private void OpenGeoEditor(Card card, bool add = false, int idx = 0)
        {
            // Add the new geo part, as necessary
            if (add)
            {
                card.AddString(CardStringsEnum.Geo, "0.00;0.00");

                // Save all changes
                ContactsManager.SaveContacts();
            }

            // Determine the final index
            var geos = card.GetString(CardStringsEnum.Geo);
            int finalIdx = add ? geos.Length - 1 : idx;
            var geo = geos[finalIdx];

            // Open the geo editor to this specific geo
            bool editing = true;
            while (editing)
            {
                List<InputChoiceInfo> choiceInfos =
                [
                    new("1", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_GEOINFO_COORDS") + $": {geo.Value}"),
                ];
                choiceInfos.Add(new($"{choiceInfos.Count + 1}", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_KEYBINDING_DELETE")));
                choiceInfos.Add(new($"{choiceInfos.Count + 1}", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_KEYBINDING_SAVE")));
                // TODO: NKS_CONTACTS_TUI_CONTACTGEOEDITPROMPT -> "Editing the contact geo of"
                int editIndex = InfoBoxSelectionColor.WriteInfoBoxSelection([.. choiceInfos], LanguageTools.GetLocalized("NKS_CONTACTS_TUI_CONTACTGEOEDITPROMPT") + $" {GetContactNamesFinal(card)}", Settings.InfoBoxSettings);

                // Check to see if we pressed Exit or not
                if (editIndex == -1 || editIndex == choiceInfos.Count - 1)
                    editing = false;
                else if (editIndex == choiceInfos.Count - 2)
                {
                    card.DeleteString(CardStringsEnum.Geo, finalIdx);
                    editing = false;
                }
                else
                {
                    // Select the editing cases based on the index number
                    switch (editIndex)
                    {
                        case 0:
                            string newGeo = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_GEOINFO_COORDSPROMPT"));
                            geo.Value = newGeo;
                            break;
                    }

                    // Save all changes
                    ContactsManager.SaveContacts();
                }
            }
        }

        internal void EditImpp(Card? card)
        {
            if (card is null)
                return;

            // Check for vCard version
            if (card.CardVersion.Major == 2)
                return;

            // Now, open the infobox that lets you select a impp, add a new one, and save all
            bool editing = true;
            while (editing)
            {
                var impps = card.GetString(CardStringsEnum.Impps);

                List<InputChoiceInfo> choiceInfos = [];
                for (int i = 0; i < impps.Length; i++)
                {
                    string impp = impps[i].Value;
                    choiceInfos.Add(new($"{i + 1}", impp));
                }
                choiceInfos.Add(new($"{choiceInfos.Count + 1}", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_KEYBINDING_ADD")));
                choiceInfos.Add(new($"{choiceInfos.Count + 1}", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_KEYBINDING_SAVE")));
                // TODO: NKS_CONTACTS_TUI_CONTACTIMPPSEDITPROMPT -> "Editing the contact impps of"
                int editIndex = InfoBoxSelectionColor.WriteInfoBoxSelection([.. choiceInfos], LanguageTools.GetLocalized("NKS_CONTACTS_TUI_CONTACTIMPPSEDITPROMPT") + $" {GetContactNamesFinal(card)}", Settings.InfoBoxSettings);

                // Check to see if we pressed Exit or not
                if (editIndex == -1 || editIndex == choiceInfos.Count - 1)
                    editing = false;
                else if (editIndex == choiceInfos.Count - 2)
                    OpenImppEditor(card, true);
                else
                    OpenImppEditor(card, idx: editIndex);

                // Save all changes
                ContactsManager.SaveContacts();
            }
        }

        private void OpenImppEditor(Card card, bool add = false, int idx = 0)
        {
            // Add the new impp part, as necessary
            if (add)
            {
                card.AddString(CardStringsEnum.Impps, "aim:IM");

                // Save all changes
                ContactsManager.SaveContacts();
            }

            // Determine the final index
            var impps = card.GetString(CardStringsEnum.Impps);
            int finalIdx = add ? impps.Length - 1 : idx;
            var impp = impps[finalIdx];

            // Open the impp editor to this specific impp
            bool editing = true;
            while (editing)
            {
                List<InputChoiceInfo> choiceInfos =
                [
                    new("1", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_IMPPINFO_INFO") + $": {impp.Value}"),
                ];
                choiceInfos.Add(new($"{choiceInfos.Count + 1}", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_KEYBINDING_DELETE")));
                choiceInfos.Add(new($"{choiceInfos.Count + 1}", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_KEYBINDING_SAVE")));
                // TODO: NKS_CONTACTS_TUI_CONTACTIMPPEDITPROMPT -> "Editing the contact impp of"
                int editIndex = InfoBoxSelectionColor.WriteInfoBoxSelection([.. choiceInfos], LanguageTools.GetLocalized("NKS_CONTACTS_TUI_CONTACTIMPPEDITPROMPT") + $" {GetContactNamesFinal(card)}", Settings.InfoBoxSettings);

                // Check to see if we pressed Exit or not
                if (editIndex == -1 || editIndex == choiceInfos.Count - 1)
                    editing = false;
                else if (editIndex == choiceInfos.Count - 2)
                {
                    card.DeleteString(CardStringsEnum.Impps, finalIdx);
                    editing = false;
                }
                else
                {
                    // Select the editing cases based on the index number
                    switch (editIndex)
                    {
                        case 0:
                            string newImpp = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_IMPPINFO_INFOPROMPT"));
                            impp.Value = newImpp;
                            break;
                    }

                    // Save all changes
                    ContactsManager.SaveContacts();
                }
            }
        }

        internal void EditNickname(Card? card)
        {
            if (card is null)
                return;

            // Check for vCard version
            if (card.CardVersion.Major == 2)
                return;

            // Now, open the infobox that lets you select a nickname, add a new one, and save all
            bool editing = true;
            while (editing)
            {
                var nicknames = card.GetString(CardStringsEnum.Nicknames);

                List<InputChoiceInfo> choiceInfos = [];
                for (int i = 0; i < nicknames.Length; i++)
                {
                    string nickname = nicknames[i].Value;
                    choiceInfos.Add(new($"{i + 1}", nickname));
                }
                choiceInfos.Add(new($"{choiceInfos.Count + 1}", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_KEYBINDING_ADD")));
                choiceInfos.Add(new($"{choiceInfos.Count + 1}", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_KEYBINDING_SAVE")));
                // TODO: NKS_CONTACTS_TUI_CONTACTNICKNAMESEDITPROMPT -> "Editing the contact nicknames of"
                int editIndex = InfoBoxSelectionColor.WriteInfoBoxSelection([.. choiceInfos], LanguageTools.GetLocalized("NKS_CONTACTS_TUI_CONTACTNICKNAMESEDITPROMPT") + $" {GetContactNamesFinal(card)}", Settings.InfoBoxSettings);

                // Check to see if we pressed Exit or not
                if (editIndex == -1 || editIndex == choiceInfos.Count - 1)
                    editing = false;
                else if (editIndex == choiceInfos.Count - 2)
                    OpenNicknameEditor(card, true);
                else
                    OpenNicknameEditor(card, idx: editIndex);

                // Save all changes
                ContactsManager.SaveContacts();
            }
        }

        private void OpenNicknameEditor(Card card, bool add = false, int idx = 0)
        {
            // Add the new nickname part, as necessary
            if (add)
            {
                card.AddString(CardStringsEnum.Nicknames, "Nickname");

                // Save all changes
                ContactsManager.SaveContacts();
            }

            // Determine the final index
            var nicknames = card.GetString(CardStringsEnum.Nicknames);
            int finalIdx = add ? nicknames.Length - 1 : idx;
            var nickname = nicknames[finalIdx];

            // Open the nickname editor to this specific nickname
            bool editing = true;
            while (editing)
            {
                List<InputChoiceInfo> choiceInfos =
                [
                    new("1", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_NICKNAMEINFO_NICK") + $": {nickname.Value}"),
                ];
                choiceInfos.Add(new($"{choiceInfos.Count + 1}", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_KEYBINDING_DELETE")));
                choiceInfos.Add(new($"{choiceInfos.Count + 1}", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_KEYBINDING_SAVE")));
                // TODO: NKS_CONTACTS_TUI_CONTACTNICKNAMEEDITPROMPT -> "Editing the contact nickname of"
                int editIndex = InfoBoxSelectionColor.WriteInfoBoxSelection([.. choiceInfos], LanguageTools.GetLocalized("NKS_CONTACTS_TUI_CONTACTNICKNAMEEDITPROMPT") + $" {GetContactNamesFinal(card)}", Settings.InfoBoxSettings);

                // Check to see if we pressed Exit or not
                if (editIndex == -1 || editIndex == choiceInfos.Count - 1)
                    editing = false;
                else if (editIndex == choiceInfos.Count - 2)
                {
                    card.DeleteString(CardStringsEnum.Nicknames, finalIdx);
                    editing = false;
                }
                else
                {
                    // Select the editing cases based on the index number
                    switch (editIndex)
                    {
                        case 0:
                            string newNickname = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_NICKNAMEINFO_NICKPROMPT"));
                            nickname.Value = newNickname;
                            break;
                    }

                    // Save all changes
                    ContactsManager.SaveContacts();
                }
            }
        }

        internal void EditRole(Card? card)
        {
            if (card is null)
                return;

            // Now, open the infobox that lets you select a role, add a new one, and save all
            bool editing = true;
            while (editing)
            {
                var roles = card.GetString(CardStringsEnum.Roles);

                List<InputChoiceInfo> choiceInfos = [];
                for (int i = 0; i < roles.Length; i++)
                {
                    string role = roles[i].Value;
                    choiceInfos.Add(new($"{i + 1}", role));
                }
                choiceInfos.Add(new($"{choiceInfos.Count + 1}", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_KEYBINDING_ADD")));
                choiceInfos.Add(new($"{choiceInfos.Count + 1}", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_KEYBINDING_SAVE")));
                // TODO: NKS_CONTACTS_TUI_CONTACTROLESEDITPROMPT -> "Editing the contact roles of"
                int editIndex = InfoBoxSelectionColor.WriteInfoBoxSelection([.. choiceInfos], LanguageTools.GetLocalized("NKS_CONTACTS_TUI_CONTACTROLESEDITPROMPT") + $" {GetContactNamesFinal(card)}", Settings.InfoBoxSettings);

                // Check to see if we pressed Exit or not
                if (editIndex == -1 || editIndex == choiceInfos.Count - 1)
                    editing = false;
                else if (editIndex == choiceInfos.Count - 2)
                    OpenRoleEditor(card, true);
                else
                    OpenRoleEditor(card, idx: editIndex);

                // Save all changes
                ContactsManager.SaveContacts();
            }
        }

        private void OpenRoleEditor(Card card, bool add = false, int idx = 0)
        {
            // Add the new role part, as necessary
            if (add)
            {
                card.AddString(CardStringsEnum.Roles, "Role");

                // Save all changes
                ContactsManager.SaveContacts();
            }

            // Determine the final index
            var roles = card.GetString(CardStringsEnum.Roles);
            int finalIdx = add ? roles.Length - 1 : idx;
            var role = roles[finalIdx];

            // Open the role editor to this specific role
            bool editing = true;
            while (editing)
            {
                List<InputChoiceInfo> choiceInfos =
                [
                    new("1", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_ROLEINFO_ROLE") + $": {role.Value}"),
                ];
                choiceInfos.Add(new($"{choiceInfos.Count + 1}", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_KEYBINDING_DELETE")));
                choiceInfos.Add(new($"{choiceInfos.Count + 1}", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_KEYBINDING_SAVE")));
                // TODO: NKS_CONTACTS_TUI_CONTACTROLEEDITPROMPT -> "Editing the contact role of"
                int editIndex = InfoBoxSelectionColor.WriteInfoBoxSelection([.. choiceInfos], LanguageTools.GetLocalized("NKS_CONTACTS_TUI_CONTACTROLEEDITPROMPT") + $" {GetContactNamesFinal(card)}", Settings.InfoBoxSettings);

                // Check to see if we pressed Exit or not
                if (editIndex == -1 || editIndex == choiceInfos.Count - 1)
                    editing = false;
                else if (editIndex == choiceInfos.Count - 2)
                {
                    card.DeleteString(CardStringsEnum.Roles, finalIdx);
                    editing = false;
                }
                else
                {
                    // Select the editing cases based on the index number
                    switch (editIndex)
                    {
                        case 0:
                            string newRole = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_ROLEINFO_ROLEPROMPT"));
                            role.Value = newRole;
                            break;
                    }

                    // Save all changes
                    ContactsManager.SaveContacts();
                }
            }
        }

        internal void EditTitle(Card? card)
        {
            if (card is null)
                return;

            // Now, open the infobox that lets you select a title, add a new one, and save all
            bool editing = true;
            while (editing)
            {
                var titles = card.GetString(CardStringsEnum.Titles);

                List<InputChoiceInfo> choiceInfos = [];
                for (int i = 0; i < titles.Length; i++)
                {
                    string title = titles[i].Value;
                    choiceInfos.Add(new($"{i + 1}", title));
                }
                choiceInfos.Add(new($"{choiceInfos.Count + 1}", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_KEYBINDING_ADD")));
                choiceInfos.Add(new($"{choiceInfos.Count + 1}", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_KEYBINDING_SAVE")));
                // TODO: NKS_CONTACTS_TUI_CONTACTTITLESEDITPROMPT -> "Editing the contact titles of"
                int editIndex = InfoBoxSelectionColor.WriteInfoBoxSelection([.. choiceInfos], LanguageTools.GetLocalized("NKS_CONTACTS_TUI_CONTACTTITLESEDITPROMPT") + $" {GetContactNamesFinal(card)}", Settings.InfoBoxSettings);

                // Check to see if we pressed Exit or not
                if (editIndex == -1 || editIndex == choiceInfos.Count - 1)
                    editing = false;
                else if (editIndex == choiceInfos.Count - 2)
                    OpenTitleEditor(card, true);
                else
                    OpenTitleEditor(card, idx: editIndex);

                // Save all changes
                ContactsManager.SaveContacts();
            }
        }

        private void OpenTitleEditor(Card card, bool add = false, int idx = 0)
        {
            // Add the new title part, as necessary
            if (add)
            {
                card.AddString(CardStringsEnum.Titles, "Job title");

                // Save all changes
                ContactsManager.SaveContacts();
            }

            // Determine the final index
            var titles = card.GetString(CardStringsEnum.Titles);
            int finalIdx = add ? titles.Length - 1 : idx;
            var title = titles[finalIdx];

            // Open the title editor to this specific title
            bool editing = true;
            while (editing)
            {
                List<InputChoiceInfo> choiceInfos =
                [
                    new("1", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_TITLEINFO_TITLE") + $": {title.Value}"),
                ];
                choiceInfos.Add(new($"{choiceInfos.Count + 1}", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_KEYBINDING_DELETE")));
                choiceInfos.Add(new($"{choiceInfos.Count + 1}", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_KEYBINDING_SAVE")));
                // TODO: NKS_CONTACTS_TUI_CONTACTTITLEEDITPROMPT -> "Editing the contact title of"
                int editIndex = InfoBoxSelectionColor.WriteInfoBoxSelection([.. choiceInfos], LanguageTools.GetLocalized("NKS_CONTACTS_TUI_CONTACTTITLEEDITPROMPT") + $" {GetContactNamesFinal(card)}", Settings.InfoBoxSettings);

                // Check to see if we pressed Exit or not
                if (editIndex == -1 || editIndex == choiceInfos.Count - 1)
                    editing = false;
                else if (editIndex == choiceInfos.Count - 2)
                {
                    card.DeleteString(CardStringsEnum.Titles, finalIdx);
                    editing = false;
                }
                else
                {
                    // Select the editing cases based on the index number
                    switch (editIndex)
                    {
                        case 0:
                            string newTitle = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_TITLEINFO_TITLEPROMPT"));
                            title.Value = newTitle;
                            break;
                    }

                    // Save all changes
                    ContactsManager.SaveContacts();
                }
            }
        }

        internal void EditNote(Card? card)
        {
            if (card is null)
                return;

            // Now, open the infobox that lets you select a note, add a new one, and save all
            bool editing = true;
            while (editing)
            {
                var notes = card.GetString(CardStringsEnum.Notes);

                List<InputChoiceInfo> choiceInfos = [];
                for (int i = 0; i < notes.Length; i++)
                {
                    string note = notes[i].Value;
                    choiceInfos.Add(new($"{i + 1}", note));
                }
                choiceInfos.Add(new($"{choiceInfos.Count + 1}", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_KEYBINDING_ADD")));
                choiceInfos.Add(new($"{choiceInfos.Count + 1}", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_KEYBINDING_SAVE")));
                // TODO: NKS_CONTACTS_TUI_CONTACTNOTESEDITPROMPT -> "Editing the contact notes of"
                int editIndex = InfoBoxSelectionColor.WriteInfoBoxSelection([.. choiceInfos], LanguageTools.GetLocalized("NKS_CONTACTS_TUI_CONTACTNOTESEDITPROMPT") + $" {GetContactNamesFinal(card)}", Settings.InfoBoxSettings);

                // Check to see if we pressed Exit or not
                if (editIndex == -1 || editIndex == choiceInfos.Count - 1)
                    editing = false;
                else if (editIndex == choiceInfos.Count - 2)
                    OpenNoteEditor(card, true);
                else
                    OpenNoteEditor(card, idx: editIndex);

                // Save all changes
                ContactsManager.SaveContacts();
            }
        }

        private void OpenNoteEditor(Card card, bool add = false, int idx = 0)
        {
            // Add the new note part, as necessary
            if (add)
            {
                card.AddString(CardStringsEnum.Notes, "Write your note");

                // Save all changes
                ContactsManager.SaveContacts();
            }

            // Determine the final index
            var notes = card.GetString(CardStringsEnum.Notes);
            int finalIdx = add ? notes.Length - 1 : idx;
            var note = notes[finalIdx];

            // Open the note editor to this specific note
            bool editing = true;
            while (editing)
            {
                List<InputChoiceInfo> choiceInfos =
                [
                    new("1", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_NOTEINFO_NOTE") + $": {note.Value}"),
                ];
                choiceInfos.Add(new($"{choiceInfos.Count + 1}", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_KEYBINDING_DELETE")));
                choiceInfos.Add(new($"{choiceInfos.Count + 1}", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_KEYBINDING_SAVE")));
                // TODO: NKS_CONTACTS_TUI_CONTACTNOTEEDITPROMPT -> "Editing the contact note of"
                int editIndex = InfoBoxSelectionColor.WriteInfoBoxSelection([.. choiceInfos], LanguageTools.GetLocalized("NKS_CONTACTS_TUI_CONTACTNOTEEDITPROMPT") + $" {GetContactNamesFinal(card)}", Settings.InfoBoxSettings);

                // Check to see if we pressed Exit or not
                if (editIndex == -1 || editIndex == choiceInfos.Count - 1)
                    editing = false;
                else if (editIndex == choiceInfos.Count - 2)
                {
                    card.DeleteString(CardStringsEnum.Notes, finalIdx);
                    editing = false;
                }
                else
                {
                    // Select the editing cases based on the index number
                    switch (editIndex)
                    {
                        case 0:
                            string newNote = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_CONTACTS_TUI_NOTEINFO_NOTEPROMPT"));
                            note.Value = newNote;
                            break;
                    }

                    // Save all changes
                    ContactsManager.SaveContacts();
                }
            }
        }

        internal void EditPicture(Card? card)
        {
            if (card is null)
                return;

            // Now, open the infobox that lets you select a picture, add a new one, and save all
            bool editing = true;
            while (editing)
            {
                var pictures = card.GetPartsArray<PhotoInfo>();

                List<InputChoiceInfo> choiceInfos = [];
                for (int i = 0; i < pictures.Length; i++)
                {
                    var picture = pictures[i];
                    choiceInfos.Add(new($"{i + 1}", IntegerTools.SizeString(picture.PhotoEncoded?.Length ?? 0)));
                }
                choiceInfos.Add(new($"{choiceInfos.Count + 1}", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_KEYBINDING_ADD")));
                choiceInfos.Add(new($"{choiceInfos.Count + 1}", LanguageTools.GetLocalized("NKS_CONTACTS_TUI_KEYBINDING_SAVE")));
                // TODO: NKS_CONTACTS_TUI_CONTACTPICTURESEDITPROMPT -> "Editing the contact pictures of"
                int editIndex = InfoBoxSelectionColor.WriteInfoBoxSelection([.. choiceInfos], LanguageTools.GetLocalized("NKS_CONTACTS_TUI_CONTACTPICTURESEDITPROMPT") + $" {GetContactNamesFinal(card)}", Settings.InfoBoxSettings);

                // Check to see if we pressed Exit or not
                if (editIndex == -1 || editIndex == choiceInfos.Count - 1)
                    editing = false;
                else if (editIndex == choiceInfos.Count - 2)
                    AddPicture(card);
                else
                    card.DeletePartsArray<PhotoInfo>(editIndex);

                // Save all changes
                ContactsManager.SaveContacts();
            }
        }

        private void AddPicture(Card card)
        {
            // Let the user select a picture
            string selectedPicture = FilesystemTools.SelectFile();

            // Check the picture and add it to the contact's picture list
            var defaultEncoding = DriverHandler.GetDriver<IEncodingAsymmetricDriver>("Default");
            defaultEncoding.Initialize();
            string pictureContent = FilesystemTools.ReadAllTextNoBlock(selectedPicture);
            var base64Content = defaultEncoding.GetEncodedString(pictureContent);
            string base64String = Encoding.Default.GetString(base64Content);
            card.AddPartToArray<PhotoInfo>("ENCODING=BASE64;PNG:" + base64String);

            // Save all changes
            ContactsManager.SaveContacts();
        }
    }
}
