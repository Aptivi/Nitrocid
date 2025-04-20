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

using System.Collections.Generic;
using VisualCard.Parts;
using VisualCard;
using System;
using System.Linq;
using System.IO;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Files.Folders;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Files.Paths;
using Nitrocid.Files.Operations.Querying;
using Nitrocid.Files.Operations;
using Nitrocid.Languages;
using Nitrocid.Drivers.Encryption;
using Nitrocid.Misc.Text.Probers.Regexp;
using Terminaux.Inputs.Interactive;
using Nitrocid.Extras.Contacts.Contacts.Interactives;
using VisualCard.Extras.Converters;
using VisualCard.Parts.Enums;

namespace Nitrocid.Extras.Contacts.Contacts
{
    /// <summary>
    /// Contacts management class
    /// </summary>
    public static class ContactsManager
    {
        private static readonly List<Card> cards = [];
        private static int searchedIdx = -1;
        private static string cachedSearchExpression = "";

        /// <summary>
        /// Gets all the available contacts from KSContacts directory
        /// </summary>
        /// <returns></returns>
        public static Card[] GetContacts()
        {
            // Get the contact files
            string contactsPath = PathsManagement.GetKernelPath(KernelPathType.Contacts);
            if (!Checking.FolderExists(contactsPath))
                Making.MakeDirectory(contactsPath);
            var contactFiles = Listing.GetFilesystemEntries(PathsManagement.GetKernelPath(KernelPathType.Contacts) + "/*.vcf");
            DebugWriter.WriteDebug(DebugLevel.I, "Got {0} contacts.", contactFiles.Length);

            // Now, enumerate through each contact file
            foreach (var contact in contactFiles)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Installing contact {0}...", contact);
                InstallContacts(contact, false);
            }
            return [.. cards];
        }

        /// <summary>
        /// Installs the contacts to the manager
        /// </summary>
        public static void ImportContacts()
        {
            // Get the contact files
            string contactsImportPath = PathsManagement.GetKernelPath(KernelPathType.ContactsImport);
            if (!Checking.FolderExists(contactsImportPath))
                Making.MakeDirectory(contactsImportPath);
            var contactFiles = Listing.GetFilesystemEntries(PathsManagement.GetKernelPath(KernelPathType.ContactsImport) + "/*.vcf");
            var androidContactFiles = Listing.GetFilesystemEntries(PathsManagement.GetKernelPath(KernelPathType.ContactsImport) + "/*.db");
            DebugWriter.WriteDebug(DebugLevel.I, "Got {0} contacts and {1} Android databases.", contactFiles.Length, androidContactFiles.Length);

            // Now, enumerate through each contact file
            foreach (var contact in contactFiles)
            {
                try
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Installing contact {0}...", contact);
                    InstallContacts(contact);
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Contact installation {0} failed. {1}", contact, ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                }
            }

            // Now, enumerate through each Android contact database
            foreach (var contact in androidContactFiles)
            {
                try
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Installing contact from Android contacts database {0}...", contact);
                    InstallContacts(contact);
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Contact installation from Android contacts database {0} failed. {1}", contact, ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                }
            }
        }

        /// <summary>
        /// Installs the contacts to the manager
        /// </summary>
        /// <param name="pathToContactFile">Path to the contacts file</param>
        /// <param name="saveToPath">Saves the added contact to a VCF file in <see cref="KernelPathType.Contacts"/></param>
        public static void InstallContacts(string pathToContactFile, bool saveToPath = true)
        {
            // Check to see if we're dealing with the non-existent contacts file
            if (!Checking.FileExists(pathToContactFile))
                throw new KernelException(KernelExceptionType.Contacts, pathToContactFile);

            // Check to see if we're given the Android contacts2.db file
            bool isAndroidContactDb = Path.GetFileName(pathToContactFile) == "contacts2.db";
            DebugWriter.WriteDebug(DebugLevel.I, "Contact file came from Android's contact storage? {0}", isAndroidContactDb);

            // Now, ensure that the parser is able to return the base parsers required to parse contacts
            var parsers =
                isAndroidContactDb ?
                AndroidContactsDb.GetContactsFromDb(pathToContactFile) :
                CardTools.GetCards(pathToContactFile);
            InstallContacts(parsers, saveToPath);
        }

        /// <summary>
        /// Installs the contacts to the manager
        /// </summary>
        /// <param name="meCardSyntax">A valid MeCard syntax</param>
        /// <param name="saveToPath">Saves the added contact to a VCF file in <see cref="KernelPathType.Contacts"/></param>
        public static void InstallContactFromMeCard(string meCardSyntax, bool saveToPath = true)
        {
            // Check to see if we're dealing with the non-existent contacts file
            if (string.IsNullOrEmpty(meCardSyntax))
                throw new KernelException(KernelExceptionType.Contacts, Translate.DoTranslation("The MeCard syntax can't be empty."));

            // Now, ensure that the parser is able to return the base parsers required to parse contacts
            var parsers = MeCard.GetContactsFromMeCardString(meCardSyntax);
            InstallContacts(parsers, saveToPath);
        }

        /// <summary>
        /// Installs the contacts to the manager
        /// </summary>
        /// <param name="cards">How many cards are there?</param>
        /// <param name="saveToPath">Saves the added contact to a VCF file in <see cref="KernelPathType.Contacts"/></param>
        internal static void InstallContacts(Card[] cards, bool saveToPath = true)
        {
            try
            {
                string contactsPath = PathsManagement.GetKernelPath(KernelPathType.Contacts);
                if (!Checking.FolderExists(contactsPath))
                    Making.MakeDirectory(contactsPath);
                DebugWriter.WriteDebug(DebugLevel.I, "Got {0} cards.", cards.Length);
                if (cards is null || cards.Length == 0)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "There are no added cards. Marking contact file as invalid...");
                    throw new KernelException(KernelExceptionType.Contacts, Translate.DoTranslation("Either the provided contacts file doesn't have information about any contact or isn't valid."));
                }

                // Debug.
                foreach (var vcard in cards)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "VCard version: {0}", vcard.CardVersion);
                    DebugWriter.WriteDebug(DebugLevel.D, "Contents:");
                    DebugWriter.WriteDebugPrivacy(DebugLevel.D, "{0}", [0], vcard.ToString());
                    if (!ContactsManager.cards.Contains(vcard))
                        ContactsManager.cards.Add(vcard);
                    DebugWriter.WriteDebugPrivacy(DebugLevel.I, "Parser successfully processed contact {0}.", [0], vcard.GetString(CardStringsEnum.FullName)[0].Value);
                }
                DebugWriter.WriteDebug(DebugLevel.I, "Cards: {0}", cards.Length);

                // Save the contacts to the contacts path if possible
                if (saveToPath)
                {
                    for (int i = 0; i < cards.Length; i++)
                    {
                        Card card = cards[i];
                        string path = contactsPath + $"/contact-{Encryption.GetEncryptedString(card.SaveToString(), "SHA256")}.vcf";
                        DebugWriter.WriteDebug(DebugLevel.I, "Saving contact to {0}...", path);
                        if (!Checking.FileExists(path))
                            card.SaveTo(path);
                    }
                }
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to parse contacts: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                throw new KernelException(KernelExceptionType.Contacts, Translate.DoTranslation("Failed to parse contacts. See the error details for more information."), ex);
            }
        }

        /// <summary>
        /// Removes the contact from the contact store and, optionally, removes it from the file
        /// </summary>
        /// <param name="contactIndex">Target contact index</param>
        /// <param name="removeFromPath">Removes the contact from the path</param>
        public static void RemoveContact(int contactIndex, bool removeFromPath = true)
        {
            try
            {
                // Check to see if we're dealing with the non-existent index file
                string contactsPath = PathsManagement.GetKernelPath(KernelPathType.Contacts);
                if (!Checking.FolderExists(contactsPath))
                    Making.MakeDirectory(contactsPath);
                if (contactIndex < 0 || contactIndex >= cards.Count)
                    throw new KernelException(KernelExceptionType.Contacts, Translate.DoTranslation("Contact index is out of range. Maximum index is {0} while provided index is {1}."), cards.Count - 1, contactIndex);

                // Now, remove the contact
                DebugWriter.WriteDebug(DebugLevel.I, "Removing contact {0}... Cards: {1}", contactIndex, cards.Count);
                string contactPath = contactsPath + $"/contact-{Encryption.GetEncryptedString(cards[contactIndex].SaveToString(), "SHA256")}.vcf";
                cards.RemoveAt(contactIndex);

                // Now, remove the contacts from the contacts path if possible
                DebugWriter.WriteDebug(DebugLevel.I, "Removing contact {0} from filesystem since we've already removed contact {1} from the list, which caused the cards count to go to {2}... However, removeFromPath, {3}, judges whether to really remove this contact file or not.", contactPath, contactIndex, cards.Count, removeFromPath);
                if (removeFromPath)
                    if (Checking.FileExists(contactPath))
                        Removing.RemoveFile(contactPath);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to remove contact {0}: {1}", contactIndex, ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                throw new KernelException(KernelExceptionType.Contacts, contactIndex.ToString(), ex);
            }
        }

        /// <summary>
        /// Removes all the contacts from the contact store and, optionally, removes it from the file
        /// </summary>
        /// <param name="removeFromPath">Removes the contact from the path</param>
        public static void RemoveContacts(bool removeFromPath = true)
        {
            try
            {
                // Check to see if we're dealing with the non-existent index file
                string contactsPath = PathsManagement.GetKernelPath(KernelPathType.Contacts);
                if (!Checking.FolderExists(contactsPath))
                    Making.MakeDirectory(contactsPath);
                if (cards.Count <= 0)
                    return;

                // Now, remove the contacts
                DebugWriter.WriteDebug(DebugLevel.I, "Removing contacts... Cards: {0}", cards.Count);
                cards.Clear();

                // Now, remove the contacts from the contacts path if possible
                DebugWriter.WriteDebug(DebugLevel.I, "Removing contacts from filesystem since we've already removed contacts from the list, which caused the cards count to go to 0... However, removeFromPath, {0}, judges whether to really remove this contact file or not.", removeFromPath);
                if (removeFromPath)
                {
                    if (Checking.FolderExists(contactsPath))
                    {
                        var contactFiles = Listing.GetFilesystemEntries(PathsManagement.GetKernelPath(KernelPathType.Contacts) + "/*.vcf");
                        foreach (var contactFile in contactFiles)
                            Removing.RemoveFile(contactFile);
                    }
                }
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to remove contacts: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                throw new KernelException(KernelExceptionType.Contacts, ex);
            }
        }

        /// <summary>
        /// Gets the contact from the contact store
        /// </summary>
        /// <param name="contactIndex">Target contact index</param>
        public static Card GetContact(int contactIndex)
        {
            try
            {
                // Check to see if we're dealing with the non-existent index
                if (cards.Count <= 0)
                    throw new KernelException(KernelExceptionType.Contacts, Translate.DoTranslation("There are no contacts to get."));
                if (contactIndex < 0 || contactIndex >= cards.Count)
                    throw new KernelException(KernelExceptionType.Contacts, Translate.DoTranslation("Contact index is out of range. Maximum index is {0} while provided index is {1}."), cards.Count - 1, contactIndex);

                // Now, get the contact
                return cards[contactIndex];
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to get contact {0}: {1}", contactIndex, ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                throw new KernelException(KernelExceptionType.Contacts, contactIndex.ToString(), ex);
            }
        }

        /// <summary>
        /// Searches the contact database for the next card that the full name satisfies the cached expression.
        /// </summary>
        /// <returns>Next card that satisfies the cached expression</returns>
        public static Card? SearchNext() =>
            SearchNext(cachedSearchExpression);

        /// <summary>
        /// Searches the contact database for the next card that the full name satisfies the specified <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">Expression to search all cards</param>
        /// <returns>Next card that satisfies the expression</returns>
        public static Card? SearchNext(string expression)
        {
            try
            {
                // Check to see if we're dealing with the non-existent index
                if (cards.Count <= 0)
                    throw new KernelException(KernelExceptionType.Contacts, Translate.DoTranslation("There are no contacts to search for."));

                // Validate the expression
                if (!RegexpTools.IsValidRegex(expression))
                    throw new KernelException(KernelExceptionType.Contacts, Translate.DoTranslation("Regular expression is invalid."));

                // Compare between the cached expression and the given expression
                if (expression == cachedSearchExpression)
                    searchedIdx++;
                else
                    searchedIdx = 0;
                cachedSearchExpression = expression;

                // Get the list of cards satisfying the expression
                var satisfiedCards = cards.Where((card) => RegexpTools.IsMatch(card.GetString(CardStringsEnum.FullName)[0].Value ?? "", expression)).ToArray();

                // Return a card if the index is valid
                if (satisfiedCards.Length > 0)
                {
                    if (searchedIdx >= satisfiedCards.Length)
                        searchedIdx = 0;
                }
                else
                    return null;
                return satisfiedCards[searchedIdx];
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to search contact for {0}: {1}", expression, ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                throw new KernelException(KernelExceptionType.Contacts, expression, ex);
            }
        }

        /// <summary>
        /// Searches the contact database for the previous card that the full name satisfies the cached expression.
        /// </summary>
        /// <returns>Previous card that satisfies the cached expression</returns>
        public static Card? SearchPrevious() =>
            SearchPrevious(cachedSearchExpression);

        /// <summary>
        /// Searches the contact database for the previous card that the full name satisfies the specified <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">Expression to search all cards</param>
        /// <returns>Previous card that satisfies the expression</returns>
        public static Card? SearchPrevious(string expression)
        {
            try
            {
                // Check to see if we're dealing with the non-existent index
                if (cards.Count <= 0)
                    throw new KernelException(KernelExceptionType.Contacts, Translate.DoTranslation("There are no contacts to search for."));

                // Validate the expression
                if (!RegexpTools.IsValidRegex(expression))
                    throw new KernelException(KernelExceptionType.Contacts, Translate.DoTranslation("Regular expression is invalid."));

                // Get the list of cards satisfying the expression
                var satisfiedCards = cards.Where((card) => RegexpTools.IsMatch(card.GetString(CardStringsEnum.FullName)[0].Value ?? "", expression)).ToArray();

                // Compare between the cached expression and the given expression
                if (expression == cachedSearchExpression)
                    searchedIdx--;
                else
                    searchedIdx = satisfiedCards.Length - 1;
                cachedSearchExpression = expression;

                // Return a card if the index is valid
                if (satisfiedCards.Length > 0)
                {
                    if (searchedIdx < 0)
                        searchedIdx = satisfiedCards.Length - 1;
                }
                else
                    return null;
                return satisfiedCards[searchedIdx];
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to search contact for {0}: {1}", expression, ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                throw new KernelException(KernelExceptionType.Contacts, expression, ex);
            }
        }

        internal static void OpenContactsTui()
        {
            var tui = new ContactsManagerCli();
            tui.Bindings.Add(new InteractiveTuiBinding<Card>(Translate.DoTranslation("Delete"), ConsoleKey.F1, (_, index, _, _) => tui.RemoveContact(index)));
            tui.Bindings.Add(new InteractiveTuiBinding<Card>(Translate.DoTranslation("Delete All"), ConsoleKey.F2, (_, _, _, _) => tui.RemoveContacts()));
            tui.Bindings.Add(new InteractiveTuiBinding<Card>(Translate.DoTranslation("Import"), ConsoleKey.F3, (_, _, _, _) => tui.ImportContacts(), true));
            tui.Bindings.Add(new InteractiveTuiBinding<Card>(Translate.DoTranslation("Import From"), ConsoleKey.F4, (_, _, _, _) => tui.ImportContactsFrom(), true));
            tui.Bindings.Add(new InteractiveTuiBinding<Card>(Translate.DoTranslation("Info"), ConsoleKey.F5, (_, index, _, _) => tui.ShowContactInfo(index)));
            tui.Bindings.Add(new InteractiveTuiBinding<Card>(Translate.DoTranslation("Search"), ConsoleKey.F6, (_, _, _, _) => tui.SearchBox()));
            tui.Bindings.Add(new InteractiveTuiBinding<Card>(Translate.DoTranslation("Search Next"), ConsoleKey.F7, (_, _, _, _) => tui.SearchNext()));
            tui.Bindings.Add(new InteractiveTuiBinding<Card>(Translate.DoTranslation("Search Back"), ConsoleKey.F8, (_, _, _, _) => tui.SearchPrevious()));
            tui.Bindings.Add(new InteractiveTuiBinding<Card>(Translate.DoTranslation("Raw Info"), ConsoleKey.F9, (_, index, _, _) => tui.ShowContactRawInfo(index)));
            tui.Bindings.Add(new InteractiveTuiBinding<Card>(Translate.DoTranslation("Import From MeCard"), ConsoleKey.F10, (_, _, _, _) => tui.ImportContactFromMeCard(), true));
            InteractiveTuiTools.OpenInteractiveTui(tui);
        }
    }
}
