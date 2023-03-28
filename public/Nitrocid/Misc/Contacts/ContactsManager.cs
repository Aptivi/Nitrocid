
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

using KS.Kernel.Exceptions;
using KS.Files.Querying;
using System.Collections.Generic;
using VisualCard.Parts;
using VisualCard;
using System;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Files;
using KS.Files.Folders;
using KS.Files.Operations;
using System.Linq;

namespace KS.Misc.Contacts
{
    /// <summary>
    /// Contacts management class
    /// </summary>
    public static class ContactsManager
    {
        private static readonly List<Card> cards = new();

        /// <summary>
        /// Gets all the available contacts from KSContacts directory
        /// </summary>
        /// <returns></returns>
        public static Card[] GetContacts() =>
            cards.ToArray();

        /// <summary>
        /// Installs the contacts to the manager
        /// </summary>
        public static void ImportContacts()
        {
            // Get the contact files
            string contactsImportPath = Paths.GetKernelPath(KernelPathType.ContactsImport);
            if (!Checking.FolderExists(contactsImportPath))
                Making.MakeDirectory(contactsImportPath);
            var contactFiles = Listing.GetFilesystemEntries(Paths.GetKernelPath(KernelPathType.ContactsImport) + "/*.vcf");

            // Now, enumerate through each contact file
            foreach (var contact in contactFiles)
                InstallContacts(contact);
        }

        /// <summary>
        /// Installs the contacts to the manager
        /// </summary>
        public static void InstallContacts()
        {
            // Get the contact files
            string contactsPath = Paths.GetKernelPath(KernelPathType.Contacts);
            if (!Checking.FolderExists(contactsPath))
                Making.MakeDirectory(contactsPath);
            var contactFiles = Listing.GetFilesystemEntries(Paths.GetKernelPath(KernelPathType.Contacts) + "/*.vcf");

            // Now, enumerate through each contact file
            foreach (var contact in contactFiles)
                InstallContacts(contact, false);
        }

        /// <summary>
        /// Installs the contacts to the manager
        /// </summary>
        /// <param name="pathToContactFile">Path to the contacts file</param>
        /// <param name="saveToPath">Saves the added contact to a VCF file in <see cref="KernelPathType.Contacts"/></param>
        public static void InstallContacts(string pathToContactFile, bool saveToPath = true)
        {
            try
            {
                // Check to see if we're dealing with the non-existent contacts file
                string contactsPath = Paths.GetKernelPath(KernelPathType.Contacts);
                if (!Checking.FileExists(pathToContactFile))
                    throw new KernelException(KernelExceptionType.Contacts, pathToContactFile);

                // Now, ensure that the parser is able to return the base parsers required to parse contacts
                var parsers = CardTools.GetCardParsers(pathToContactFile);
                DebugWriter.WriteDebug(DebugLevel.I, "Got {0} parsers from {1}.", parsers.Count, pathToContactFile);

                // Iterate through the contacts
                List<Card> addedCards = new();
                foreach (var parser in parsers)
                {
                    try
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Parser card version: {0}", parser.CardVersion);
                        DebugWriter.WriteDebug(DebugLevel.D, "Contents:");
                        DebugWriter.WriteDebug(DebugLevel.D, "Contents:", parser.CardContent);

                        // Now, parse the card
                        var card = parser.Parse();
                        if (!cards.Where((c) => c == card).Any())
                            cards.Add(card);
                        addedCards.Add(card);
                        DebugWriter.WriteDebug(DebugLevel.I, "Parser successfully processed contact {0}.", cards[^1].ContactFullName);
                        DebugWriter.WriteDebug(DebugLevel.I, "Cards: {0}", cards.Count);
                        DebugWriter.WriteDebug(DebugLevel.I, "Added cards: {0}", addedCards.Count);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WriteDebug(DebugLevel.E, "Failed to parse contact from {0}: {1}", pathToContactFile, ex.Message);
                        DebugWriter.WriteDebugStackTrace(ex);
                    }
                }

                // Check the added cards count and the parsers count
                if (addedCards.Count > 0)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Final added cards: {0}", addedCards.Count);
                    if (parsers.Count != addedCards.Count)
                    {
                        DebugWriter.WriteDebug(DebugLevel.E, "Added cards count {0} doesn't match expected contacts count {1}. Errors in the parser might tell you why.", addedCards.Count, parsers.Count);
                        throw new KernelException(KernelExceptionType.Contacts, Translate.DoTranslation("Some of the contacts can't be added. Only {0} out of {1} contacts were added."), addedCards.Count, parsers.Count);
                    }
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "There are no added cards. Marking contact file as invalid...");
                    throw new KernelException(KernelExceptionType.Contacts, Translate.DoTranslation("Either the provided contacts file doesn't have information about any contact or isn't valid."));
                }

                // Now, save the contacts to the contacts path if possible
                if (saveToPath)
                {
                    for (int i = 0; i < cards.Count; i++)
                    {
                        Card card = cards[i];
                        string path = contactsPath + $"/contact-{i}.vcf";
                        if (!Checking.FileExists(path))
                            card.SaveTo(path);
                    }
                }
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to parse contacts from {0}: {1}", pathToContactFile, ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                throw new KernelException(KernelExceptionType.Contacts, pathToContactFile, ex);
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
                string contactsPath = Paths.GetKernelPath(KernelPathType.Contacts);
                string contactPath = contactsPath + $"/contact-{contactIndex}.vcf";
                if (cards.Count <= 0)
                    throw new KernelException(KernelExceptionType.Contacts, Translate.DoTranslation("There are no contacts to remove."));
                if (contactIndex < 0 || contactIndex >= cards.Count)
                    throw new KernelException(KernelExceptionType.Contacts, Translate.DoTranslation("Contact index is out of range. Maximum index is {0} while provided index is {1}."), cards.Count - 1, contactIndex);

                // Now, remove the contact
                cards.RemoveAt(contactIndex);

                // Now, remove the contacts from the contacts path if possible
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
                string contactsPath = Paths.GetKernelPath(KernelPathType.Contacts);
                if (cards.Count <= 0)
                    throw new KernelException(KernelExceptionType.Contacts, Translate.DoTranslation("There are no contacts to remove."));

                // Now, remove the contacts
                cards.Clear();

                // Now, remove the contacts from the contacts path if possible
                if (removeFromPath)
                {
                    if (Checking.FolderExists(contactsPath))
                    {
                        var contactFiles = Listing.GetFilesystemEntries(Paths.GetKernelPath(KernelPathType.Contacts) + "/*.vcf");
                        foreach (var contactFile in contactFiles)
                            Removing.RemoveFile(contactFile);
                    }
                }
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to remove contacts {0}: {0}", ex.Message);
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
                    throw new KernelException(KernelExceptionType.Contacts, Translate.DoTranslation("There are no contacts to remove."));
                if (contactIndex < 0 || contactIndex >= cards.Count)
                    throw new KernelException(KernelExceptionType.Contacts, Translate.DoTranslation("Contact index is out of range. Maximum index is {0} while provided index is {1}."), cards.Count - 1, contactIndex);

                // Now, get the contact
                return cards[contactIndex];
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to remove contact {0}: {1}", contactIndex, ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                throw new KernelException(KernelExceptionType.Contacts, contactIndex.ToString(), ex);
            }
        }
    }
}
