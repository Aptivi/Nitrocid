//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
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
using System.Text;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Search;
using MimeKit;
using MimeKit.Cryptography;
using MimeKit.Text;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Kernel.Time.Renderers;
using Nitrocid.Base.Languages;
using Nitrocid.ShellPacks.Shells.Mail.Tools.PGP;
using Nitrocid.ShellPacks.Shells.Mail.Tools.Transfer;
using Terminaux.Base.Extensions;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Textify.General;

namespace Nitrocid.ShellPacks.Shells.Mail.Tools
{
    /// <summary>
    /// Mail tools
    /// </summary>
    public static class MailTools
    {
        private static readonly MailTransferProgress progress = new();

        /// <summary>
        /// The mail progress
        /// </summary>
        public static MailTransferProgress Progress =>
            progress;

        /// <summary>
        /// Creates mail folder
        /// </summary>
        /// <param name="client">IMAP mail client</param>
        /// <param name="Directory">Directory name</param>
        /// <param name="parentDirectory">Parent directory to create a directory within</param>
        public static void CreateMailDirectory(ImapClient client, string Directory, string parentDirectory)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Creating folder: {0}", vars: [Directory]);
            try
            {
                MailFolder MailFolder;
                lock (client.SyncRoot)
                {
                    MailFolder = OpenFolder(client, parentDirectory);
                    MailFolder.Create(Directory, true);
                }
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to create folder {0}: {1}", vars: [Directory, ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                throw new KernelException(KernelExceptionType.Mail, LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_MAILDIR_CREATEFAILED"), ex, Directory, ex.Message);
            }
        }

        /// <summary>
        /// Deletes mail folder
        /// </summary>
        /// <param name="client">IMAP mail client</param>
        /// <param name="Directory">Directory name</param>
        public static void DeleteMailDirectory(ImapClient client, string Directory)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Deleting folder: {0}", vars: [Directory]);
            try
            {
                MailFolder MailFolder;
                lock (client.SyncRoot)
                {
                    MailFolder = OpenFolder(client, Directory);
                    MailFolder.Delete();
                }
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to delete folder {0}: {1}", vars: [Directory, ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                throw new KernelException(KernelExceptionType.Mail, LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_MAILDIR_DELETEFAILED"), ex, Directory, ex.Message);
            }
        }

        /// <summary>
        /// Deletes mail folder
        /// </summary>
        /// <param name="client">IMAP mail client</param>
        /// <param name="Directory">Directory name</param>
        /// <param name="NewName">New mail directory name</param>
        public static void RenameMailDirectory(ImapClient client, string Directory, string NewName)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Renaming folder {0} to {1}", vars: [Directory, NewName]);
            try
            {
                MailFolder MailFolder;
                lock (client.SyncRoot)
                {
                    MailFolder = OpenFolder(client, Directory);
                    MailFolder.Rename(MailFolder.ParentFolder ?? MailFolder, NewName);
                }
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to delete folder {0}: {1}", vars: [Directory, ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                throw new KernelException(KernelExceptionType.Mail, LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_MAILDIR_DELETEFAILED"), ex, Directory, ex.Message);
            }
        }

        /// <summary>
        /// Changes current mail directory
        /// </summary>
        /// <param name="client">IMAP mail client</param>
        /// <param name="Directory">A mail directory</param>
        public static void MailChangeDirectory(ImapClient client, string Directory)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Opening folder: {0}", vars: [Directory]);
            try
            {
                lock (client.SyncRoot)
                    OpenFolder(client, Directory);
                DebugWriter.WriteDebug(DebugLevel.I, "Current directory changed.");
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to open folder {0}: {1}", vars: [Directory, ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                throw new KernelException(KernelExceptionType.Mail, LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_EXCEPTION_CANTOPENMAILFOLDER"), ex, Directory, ex.Message);
            }
        }

        /// <summary>
        /// Locates the normal (not special) folder and opens it.
        /// </summary>
        /// <param name="client">IMAP mail client</param>
        /// <param name="FolderString">A folder to open (not a path)</param>
        /// <param name="FolderMode">Folder mode</param>
        /// <returns>A folder</returns>
        public static MailFolder OpenFolder(ImapClient client, string FolderString, FolderAccess FolderMode = FolderAccess.ReadWrite)
        {
            var opened =
                TryOpenFolderFrom(client, FolderString, client.PersonalNamespaces, FolderMode) ??
                TryOpenFolderFrom(client, FolderString, client.SharedNamespaces, FolderMode) ??
                TryOpenFolderFrom(client, FolderString, client.OtherNamespaces, FolderMode) ??
                TryOpenFolder(client.Inbox) ??
                throw new KernelException(KernelExceptionType.NoSuchMailDirectory, LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_MAILDIR_DIRNOTFOUND"), FolderString);
            return opened;
        }

        /// <summary>
        /// Lists directories
        /// </summary>
        /// <returns>A list of mail folder instances</returns>
        public static MailFolder[] MailListDirectories(ImapClient client)
        {
            List<MailFolder> folders = [];
            folders.AddRange(ListFoldersFrom(client, client.PersonalNamespaces));
            folders.AddRange(ListFoldersFrom(client, client.SharedNamespaces));
            folders.AddRange(ListFoldersFrom(client, client.OtherNamespaces));
            return [.. folders];
        }

        /// <summary>
        /// Populates e-mail messages
        /// </summary>
        /// <param name="client">IMAP mail client</param>
        /// <param name="path">Path to a mail folder</param>
        public static IEnumerable<UniqueId> PopulateMessages(ImapClient client, string path)
        {
            IEnumerable<UniqueId> messages = [];
            if (client.IsConnected)
            {
                lock (client.SyncRoot)
                {
                    IMailFolder folder;
                    if (string.IsNullOrEmpty(path) || path == "Inbox")
                    {
                        folder = client.Inbox ??
                            throw new KernelException(KernelExceptionType.Mail, LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_EXCEPTION_INBOXOBTAINFAILED"));
                        folder.Open(FolderAccess.ReadWrite);
                    }
                    else
                        folder = OpenFolder(client, path);
                    DebugWriter.WriteDebug(DebugLevel.I, "Opened {0}", vars: [path]);
                    messages = folder.Search(SearchQuery.All).Reverse();
                    DebugWriter.WriteDebug(DebugLevel.I, "Messages count: {0} messages", vars: [messages.LongCount()]);
                }
            }
            return messages;
        }

        /// <summary>
        /// Lists messages
        /// </summary>
        /// <param name="client">IMAP mail client</param>
        /// <param name="directory">Directory to open</param>
        /// <param name="PageNum">Page number</param>
        /// <exception cref="ArgumentException"></exception>
        public static void MailListMessages(ImapClient client, string directory, int PageNum) =>
            MailListMessages(client, directory, PageNum, ShellsInit.ShellsConfig.MailMaxMessagesInPage);

        /// <summary>
        /// Lists messages
        /// </summary>
        /// <param name="client">IMAP mail client</param>
        /// <param name="directory">Directory to open</param>
        /// <param name="PageNum">Page number</param>
        /// <param name="MessagesInPage">Max messages in one page</param>
        /// <exception cref="ArgumentException"></exception>
        public static void MailListMessages(ImapClient client, string directory, int PageNum, int MessagesInPage)
        {
            // Sanity checks for the page number
            if (PageNum <= 0)
                PageNum = 1;
            DebugWriter.WriteDebug(DebugLevel.I, "Page number {0}", vars: [PageNum]);

            int FirstIndex = MessagesInPage * PageNum - 10;
            int LastIndex = MessagesInPage * PageNum - 1;
            // TODO: NKS_SHELLPACKS_MAIL_EXCEPTION_FOLDEROBTAINFAILED -> Obtaining folder {0} failed
            var folder = OpenFolder(client, directory) ??
                throw new KernelException(KernelExceptionType.Mail, LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_EXCEPTION_FOLDEROBTAINFAILED"), directory);
            var messages = PopulateMessages(client, directory);
            int MaxMessagesIndex = messages.Count() - 1;
            DebugWriter.WriteDebug(DebugLevel.I, "10 messages shown in each page. First message number in page {0} is {1} and last message number in page {0} is {2}", vars: [MessagesInPage, FirstIndex, LastIndex]);
            for (int i = FirstIndex; i <= LastIndex; i++)
            {
                if (i <= MaxMessagesIndex)
                {
                    string MsgFrom = "";
                    string MsgSubject = "";
                    string MsgPreview = "";

                    // Getting information about the message is vital to display them.
                    DebugWriter.WriteDebug(DebugLevel.I, "Getting message {0}...", vars: [i]);
                    lock (client.SyncRoot)
                    {
                        MimeMessage Msg = folder?.GetMessage(messages.ElementAtOrDefault(i), default, Progress) ??
                            throw new KernelException(KernelExceptionType.Mail, LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_EXCEPTION_OBTAINFAILED"));
                        MsgFrom = Msg.From.ToString();
                        MsgSubject = Msg.Subject ?? "";
                        MsgPreview = Msg.GetTextBody(MimeKit.Text.TextFormat.Text)?.Truncate(200) ?? "";
                    }
                    DebugWriter.WriteDebug(DebugLevel.I, "From {0}: {1}", vars: [MsgFrom, MsgSubject]);

                    // Display them now.
                    TextWriterColor.Write($"- [{i + 1}/{MaxMessagesIndex + 1}] {MsgFrom}: ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write(MsgSubject, true, ThemeColorType.ListValue);
                    if (ShellsInit.ShellsConfig.ShowPreview & !string.IsNullOrWhiteSpace(MsgPreview))
                    {
                        // For more efficient preview, use the PREVIEW extension as documented in RFC-8970 (https://tools.ietf.org/html/rfc8970). However,
                        // this is impossible at this time because no server and no client support this extension. It supports the LAZY modifier. It only
                        // displays 200 character long body.
                        //
                        // Concept: Msg.Preview(LazyMode:=True)
                        TextWriterColor.Write(MsgPreview, true, ThemeColorType.ListValue);
                    }
                }
                else
                    DebugWriter.WriteDebug(DebugLevel.W, "Reached max message limit. Message number {0}", vars: [i]);
            }
        }

        /// <summary>
        /// Removes a message
        /// </summary>
        /// <param name="client">IMAP mail client</param>
        /// <param name="directory">Directory to open</param>
        /// <param name="MsgNumber">Message number</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="ArgumentException"></exception>
        public static bool MailRemoveMessage(ImapClient client, string directory, int MsgNumber)
        {
            int Message = MsgNumber - 1;
            // TODO: NKS_SHELLPACKS_MAIL_EXCEPTION_FOLDEROBTAINFAILED -> Obtaining folder {0} failed
            var folder = OpenFolder(client, directory) ??
                throw new KernelException(KernelExceptionType.Mail, LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_EXCEPTION_FOLDEROBTAINFAILED"), directory);
            var messages = PopulateMessages(client, directory);
            int MaxMessagesIndex = messages.Count() - 1;
            DebugWriter.WriteDebug(DebugLevel.I, "Message number {0}", vars: [Message]);
            if (Message < 0)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Trying to remove message 0 or less than 0.");
                throw new KernelException(KernelExceptionType.Mail, LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_MESSAGENUMNOTZERO"));
            }
            else if (Message > MaxMessagesIndex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Message {0} not in list. It was larger than MaxMessagesIndex ({1})", vars: [Message, MaxMessagesIndex]);
                throw new KernelException(KernelExceptionType.Mail, LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_MESSAGENUMNOTFOUND"));
            }

            lock (client.SyncRoot)
            {
                folder.Store(messages.ElementAtOrDefault(Message), new StoreFlagsRequest(StoreAction.Add, MessageFlags.Deleted));
                DebugWriter.WriteDebug(DebugLevel.I, "Removed.");
                folder.Expunge();
            }
            return true;
        }

        /// <summary>
        /// Removes all mail that the specified sender has sent
        /// </summary>
        /// <param name="client">IMAP mail client</param>
        /// <param name="directory">Directory to open</param>
        /// <param name="Sender">The sender name</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool MailRemoveAllBySender(ImapClient client, string directory, string Sender)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "All mail by {0} will be removed.", vars: [Sender]);
            int DeletedMsgNumber = 1;
            int SteppedMsgNumber = 0;
            // TODO: NKS_SHELLPACKS_MAIL_EXCEPTION_FOLDEROBTAINFAILED -> Obtaining folder {0} failed
            var folder = OpenFolder(client, directory) ??
                throw new KernelException(KernelExceptionType.Mail, LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_EXCEPTION_FOLDEROBTAINFAILED"), directory);
            var messages = PopulateMessages(client, directory);
            for (int i = 0; i < messages.Count(); i++)
            {
                try
                {
                    lock (client.SyncRoot)
                    {
                        var MessageId = messages.ElementAtOrDefault(i);
                        MimeMessage Msg = folder.GetMessage(MessageId, default, Progress);
                        SteppedMsgNumber += 1;

                        foreach (var address in Msg.From)
                        {
                            if (address.Name == Sender)
                            {
                                DebugWriter.WriteDebug(DebugLevel.I, "Opened {0}. Removing {1}...", vars: [directory, Sender]);
                                folder.Store(MessageId, new StoreFlagsRequest(StoreAction.Add, MessageFlags.Deleted));
                                DebugWriter.WriteDebug(DebugLevel.I, "Removed.");
                                folder.Expunge();
                                DebugWriter.WriteDebug(DebugLevel.I, "Message {0} from {1} deleted from {2}. {3} messages remaining to parse.", vars: [DeletedMsgNumber, Sender, directory, messages.Count() - SteppedMsgNumber]);
                                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_RMALL_DELETEDNOTINBOX"), DeletedMsgNumber, Sender, directory, messages.Count() - SteppedMsgNumber);
                                DeletedMsgNumber += 1;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Moves a message
        /// </summary>
        /// <param name="client">IMAP mail client</param>
        /// <param name="directory">Directory to open</param>
        /// <param name="MsgNumber">Message number</param>
        /// <param name="TargetFolder">Target folder</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="ArgumentException"></exception>
        public static bool MailMoveMessage(ImapClient client, string directory, int MsgNumber, string TargetFolder)
        {
            int Message = MsgNumber - 1;
            // TODO: NKS_SHELLPACKS_MAIL_EXCEPTION_FOLDEROBTAINFAILED -> Obtaining folder {0} failed
            var folder = OpenFolder(client, directory) ??
                throw new KernelException(KernelExceptionType.Mail, LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_EXCEPTION_FOLDEROBTAINFAILED"), directory);
            var targetFolder = OpenFolder(client, TargetFolder) ??
                throw new KernelException(KernelExceptionType.Mail, LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_EXCEPTION_FOLDEROBTAINFAILED"), directory);
            var messages = PopulateMessages(client, directory);
            int MaxMessagesIndex = messages.Count() - 1;
            DebugWriter.WriteDebug(DebugLevel.I, "Message number {0}", vars: [Message]);
            if (Message < 0)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Trying to move message 0 or less than 0.");
                throw new KernelException(KernelExceptionType.Mail, LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_MESSAGENUMNOTZERO"));
            }
            else if (Message > MaxMessagesIndex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Message {0} not in list. It was larger than MaxMessagesIndex ({1})", vars: [Message, MaxMessagesIndex]);
                throw new KernelException(KernelExceptionType.Mail, LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_MESSAGENUMNOTFOUND"));
            }

            lock (client.SyncRoot)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Opened {0}. Moving {1}...", vars: [directory, MsgNumber]);
                folder.MoveTo(messages.ElementAtOrDefault(Message), targetFolder);
                DebugWriter.WriteDebug(DebugLevel.I, "Moved.");
            }
            return true;
        }

        /// <summary>
        /// Moves all mail that the specified sender has sent
        /// </summary>
        /// <param name="client">IMAP mail client</param>
        /// <param name="directory">Directory to open</param>
        /// <param name="Sender">The sender name</param>
        /// <param name="TargetFolder">Target folder</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool MailMoveAllBySender(ImapClient client, string directory, string Sender, string TargetFolder)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "All mail by {0} will be moved.", vars: [Sender]);
            int DeletedMsgNumber = 1;
            int SteppedMsgNumber = 0;
            // TODO: NKS_SHELLPACKS_MAIL_EXCEPTION_FOLDEROBTAINFAILED -> Obtaining folder {0} failed
            var folder = OpenFolder(client, directory) ??
                throw new KernelException(KernelExceptionType.Mail, LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_EXCEPTION_FOLDEROBTAINFAILED"), directory);
            var targetFolder = OpenFolder(client, TargetFolder) ??
                throw new KernelException(KernelExceptionType.Mail, LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_EXCEPTION_FOLDEROBTAINFAILED"), directory);
            var messages = PopulateMessages(client, directory);
            for (int i = 0; i < messages.Count(); i++)
            {
                try
                {
                    lock (client.SyncRoot)
                    {
                        var MessageId = messages.ElementAtOrDefault(i);
                        MimeMessage Msg = folder.GetMessage(MessageId, default, Progress);
                        SteppedMsgNumber += 1;

                        foreach (var address in Msg.From)
                        {
                            if (address.Name == Sender)
                            {
                                DebugWriter.WriteDebug(DebugLevel.I, "Opened {0}. Moving {1}...", vars: [directory, Sender]);
                                folder.MoveTo(MessageId, targetFolder);
                                DebugWriter.WriteDebug(DebugLevel.I, "Moved.");
                                DebugWriter.WriteDebug(DebugLevel.I, "Message {0} from {1} moved from {2}. {3} messages remaining to parse.", vars: [DeletedMsgNumber, Sender, directory, messages.Count() - SteppedMsgNumber]);
                                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_MVALL_DELETEDNOTINBOX"), DeletedMsgNumber, Sender, directory, messages.Count() - SteppedMsgNumber);
                                DeletedMsgNumber += 1;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Prints content of message to console
        /// </summary>
        /// <param name="client">IMAP mail client</param>
        /// <param name="directory">Directory to open</param>
        /// <param name="MessageNum">Message number</param>
        public static void MailPrintMessage(ImapClient client, string directory, int MessageNum) =>
            TextWriterColor.Write(MailRenderMessage(client, directory, MessageNum));

        /// <summary>
        /// Renders content of message to a string sequence
        /// </summary>
        /// <param name="client">IMAP mail client</param>
        /// <param name="directory">Directory to open</param>
        /// <param name="MessageNum">Message number</param>
        public static string MailRenderMessage(ImapClient client, string directory, int MessageNum)
        {
            var messageBuilder = new StringBuilder();
            int Message = MessageNum - 1;
            // TODO: NKS_SHELLPACKS_MAIL_EXCEPTION_FOLDEROBTAINFAILED -> Obtaining folder {0} failed
            var folder = OpenFolder(client, directory) ??
                throw new KernelException(KernelExceptionType.Mail, LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_EXCEPTION_FOLDEROBTAINFAILED"), directory);
            var messages = PopulateMessages(client, directory);
            int MaxMessagesIndex = messages.Count() - 1;
            MimeMessage currentEntry = folder.GetMessage(messages.ElementAtOrDefault(Message), default, Progress);
            DebugWriter.WriteDebug(DebugLevel.I, "Message number {0}", vars: [Message]);
            if (Message < 0)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Trying to access message 0 or less than 0.");
                throw new KernelException(KernelExceptionType.Mail, LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_MESSAGENUMNOTZERO"));
            }
            else if (Message > MaxMessagesIndex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Message {0} not in list. It was larger than MaxMessagesIndex ({1})", vars: [Message, MaxMessagesIndex]);
                throw new KernelException(KernelExceptionType.Mail, LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_MESSAGENUMNOTFOUND"));
            }

            lock (client.SyncRoot)
            {
                // Get message
                DebugWriter.WriteDebug(DebugLevel.I, "Getting message...");

                // Print all the addresses that sent the mail
                DebugWriter.WriteDebug(DebugLevel.I, "{0} senders.", vars: [currentEntry.From.Count]);
                foreach (InternetAddress Address in currentEntry.From)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Address: {0} ({1})", vars: [Address.Name, Address.Encoding.EncodingName]);
                    messageBuilder.AppendLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_MESSAGEVIEW_FROM").FormatString(Address.ToString()));
                }

                // Print all the addresses that received the mail
                DebugWriter.WriteDebug(DebugLevel.I, "{0} receivers.", vars: [currentEntry.To.Count]);
                foreach (InternetAddress Address in currentEntry.To)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Address: {0} ({1})", vars: [Address.Name, Address.Encoding.EncodingName]);
                    messageBuilder.AppendLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_MESSAGEVIEW_TO").FormatString(Address.ToString()));
                }

                // Print the date and time when the user received the mail
                DebugWriter.WriteDebug(DebugLevel.I, "Rendering time and date of {0}.", vars: [currentEntry.Date.DateTime.ToString()]);
                messageBuilder.AppendLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_MESSAGEVIEW_WHEN").FormatString(TimeDateRenderers.RenderTime(currentEntry.Date.DateTime), TimeDateRenderers.RenderDate(currentEntry.Date.DateTime)));

                // Prepare subject
                messageBuilder.AppendLine();
                DebugWriter.WriteDebug(DebugLevel.I, "Subject length: {0}, {1}", vars: [currentEntry.Subject?.Length, currentEntry.Subject]);
                messageBuilder.Append($"- {currentEntry.Subject}");

                // Write a sign after the subject if attachments are found
                DebugWriter.WriteDebug(DebugLevel.I, "Attachments count: {0}", vars: [currentEntry.Attachments.Count()]);
                if (currentEntry.Attachments.Any())
                    messageBuilder.AppendLine(" - [*]");
                else
                    messageBuilder.AppendLine();

                // Prepare body
                messageBuilder.AppendLine();
                DebugWriter.WriteDebug(DebugLevel.I, "Displaying body...");
                var DecryptedMessage = default(Dictionary<string, MimeEntity>);
                bool isEncrypted = currentEntry.Body is MultipartEncrypted;
                DebugWriter.WriteDebug(DebugLevel.I, "To decrypt: {0}", vars: [isEncrypted]);
                if (isEncrypted)
                {
                    DecryptedMessage = DecryptMessage(currentEntry);
                    DebugWriter.WriteDebug(DebugLevel.I, "Decrypted messages length: {0}", vars: [DecryptedMessage.Count]);
                    var DecryptedEntity = DecryptedMessage["Body"];
                    var DecryptedStream = new MemoryStream();
                    DebugWriter.WriteDebug(DebugLevel.I, $"Decrypted message type: {(DecryptedEntity is Multipart ? "Multipart" : "Singlepart")}");
                    if (DecryptedEntity is Multipart)
                    {
                        Multipart MultiEntity = (Multipart)DecryptedEntity;
                        DebugWriter.WriteDebug(DebugLevel.I, $"Decrypted message entity is {(MultiEntity is not null ? "multipart" : "nothing")}");
                        if (MultiEntity is not null)
                        {
                            for (int EntityNumber = 0; EntityNumber <= MultiEntity.Count - 1; EntityNumber++)
                            {
                                DebugWriter.WriteDebug(DebugLevel.I, $"Entity number {EntityNumber} is {(MultiEntity[EntityNumber].IsAttachment ? "an attachment" : "not an attachment")}");
                                if (!MultiEntity[EntityNumber].IsAttachment)
                                {
                                    MultiEntity[EntityNumber].WriteTo(DecryptedStream, true);
                                    DebugWriter.WriteDebug(DebugLevel.I, "Written {0} bytes to stream.", vars: [DecryptedStream.Length]);
                                    DecryptedStream.Position = 0L;
                                    var DecryptedByte = new byte[(int)(DecryptedStream.Length + 1)];
                                    DecryptedStream.Read(DecryptedByte, 0, (int)DecryptedStream.Length);
                                    DebugWriter.WriteDebug(DebugLevel.I, "Written {0} bytes to buffer.", vars: [DecryptedByte.Length]);
                                    messageBuilder.AppendLine(Encoding.Default.GetString(DecryptedByte));
                                }
                            }
                        }
                    }
                    else
                    {
                        DecryptedEntity.WriteTo(DecryptedStream, true);
                        DebugWriter.WriteDebug(DebugLevel.I, "Written {0} bytes to stream.", vars: [DecryptedStream.Length]);
                        DecryptedStream.Position = 0L;
                        var DecryptedByte = new byte[(int)(DecryptedStream.Length + 1)];
                        DecryptedStream.Read(DecryptedByte, 0, (int)DecryptedStream.Length);
                        DebugWriter.WriteDebug(DebugLevel.I, "Written {0} bytes to buffer.", vars: [DecryptedByte.Length]);
                        messageBuilder.AppendLine(Encoding.Default.GetString(DecryptedByte));
                    }
                }
                else
                    messageBuilder.AppendLine(currentEntry.GetTextBody((TextFormat)ShellsInit.ShellsConfig.MailTextFormat));
                messageBuilder.AppendLine();

                // Populate attachments
                if (currentEntry.Attachments.Any())
                {
                    messageBuilder.AppendLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_ATTACHMENTS"));
                    var AttachmentEntities = new List<MimeEntity>();
                    if (isEncrypted)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Parsing attachments...");
                        if (DecryptedMessage is not null)
                        {
                            for (int DecryptedEntityNumber = 0; DecryptedEntityNumber <= DecryptedMessage.Count - 1; DecryptedEntityNumber++)
                            {
                                var decryptedString = DecryptedMessage.Keys.ElementAtOrDefault(DecryptedEntityNumber);
                                var decryptedEntity = DecryptedMessage.Values.ElementAtOrDefault(DecryptedEntityNumber);
                                if (decryptedString is null)
                                    continue;
                                if (decryptedEntity is null)
                                    continue;
                                DebugWriter.WriteDebug(DebugLevel.I, "Is entity number {0} an attachment? {1}", vars: [DecryptedEntityNumber, decryptedString.Contains("Attachment")]);
                                DebugWriter.WriteDebug(DebugLevel.I, "Is entity number {0} a body that is a multipart? {1}", vars: [DecryptedEntityNumber, decryptedString == "Body" & DecryptedMessage["Body"] is Multipart]);
                                if (decryptedString.Contains("Attachment"))
                                {
                                    DebugWriter.WriteDebug(DebugLevel.I, "Adding entity {0} to attachment entities...", vars: [DecryptedEntityNumber]);
                                    AttachmentEntities.Add(decryptedEntity);
                                }
                                else if (decryptedString == "Body" & DecryptedMessage["Body"] is Multipart)
                                {
                                    Multipart MultiEntity = (Multipart)DecryptedMessage["Body"];
                                    DebugWriter.WriteDebug(DebugLevel.I, $"Decrypted message entity is {(MultiEntity is not null ? "multipart" : "nothing")}");
                                    if (MultiEntity is not null)
                                    {
                                        DebugWriter.WriteDebug(DebugLevel.I, "{0} entities found.", vars: [MultiEntity.Count]);
                                        for (int EntityNumber = 0; EntityNumber <= MultiEntity.Count - 1; EntityNumber++)
                                        {
                                            DebugWriter.WriteDebug(DebugLevel.I, $"Entity number {EntityNumber} is {(MultiEntity[EntityNumber].IsAttachment ? "an attachment" : "not an attachment")}");
                                            if (MultiEntity[EntityNumber].IsAttachment)
                                            {
                                                DebugWriter.WriteDebug(DebugLevel.I, "Adding entity {0} to attachment list...", vars: [EntityNumber]);
                                                AttachmentEntities.Add(MultiEntity[EntityNumber]);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                        AttachmentEntities = (List<MimeEntity>)currentEntry.Attachments;

                    foreach (MimeEntity Attachment in AttachmentEntities)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Attachment ID: {0}", vars: [Attachment.ContentId]);
                        if (Attachment is MessagePart)
                        {
                            DebugWriter.WriteDebug(DebugLevel.I, "Attachment is a message.");
                            messageBuilder.AppendLine($"- {Attachment.ContentDisposition?.FileName}");
                        }
                        else
                        {
                            DebugWriter.WriteDebug(DebugLevel.I, "Attachment is a file.");
                            MimePart AttachmentPart = (MimePart)Attachment;
                            messageBuilder.AppendLine($"- {AttachmentPart.FileName}");
                        }
                    }
                }
                return messageBuilder.ToString();
            }
        }

        /// <summary>
        /// Decrypts a message
        /// </summary>
        /// <param name="Text">Text part</param>
        /// <returns>A decrypted message, or null if unsuccessful.</returns>
        public static Dictionary<string, MimeEntity> DecryptMessage(MimeMessage Text)
        {
            var EncryptedDict = new Dictionary<string, MimeEntity>();
            DebugWriter.WriteDebug(DebugLevel.I, $"Encrypted message type: {(Text.Body is MultipartEncrypted ? "Multipart" : "Singlepart")}");
            if (Text.Body is MultipartEncrypted encrypted)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Message type: MultipartEncrypted");
                DebugWriter.WriteDebug(DebugLevel.I, "Decrypting...");
                EncryptedDict.Add("Body", encrypted.Decrypt(new PGPContext()));
            }
            else if (Text.Body is not null)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Trying to decrypt plain text. Returning body...");
                EncryptedDict.Add("Body", Text.Body);
            }
            int AttachmentNumber = 1;
            foreach (MimeEntity TextAttachment in Text.Attachments)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Attachment number {0}", vars: [AttachmentNumber]);
                DebugWriter.WriteDebug(DebugLevel.I, $"Encrypted attachment type: {(TextAttachment is MultipartEncrypted ? "Multipart" : "Singlepart")}");
                if (TextAttachment is MultipartEncrypted attachmentEncrypted)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Attachment type: MultipartEncrypted");
                    DebugWriter.WriteDebug(DebugLevel.I, "Decrypting...");
                    EncryptedDict.Add("Attachment " + AttachmentNumber, attachmentEncrypted.Decrypt(new PGPContext()));
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Trying to decrypt plain attachment. Returning body...");
                    EncryptedDict.Add("Attachment " + AttachmentNumber, TextAttachment);
                }
                AttachmentNumber += 1;
            }
            return EncryptedDict;
        }

        /// <summary>
        /// Sends a message
        /// </summary>
        /// <param name="client">IMAP mail client</param>
        /// <param name="sender">Sender name</param>
        /// <param name="Recipient">Recipient name</param>
        /// <param name="Subject">Subject</param>
        /// <param name="Body">Body (only text. See <see cref="MailSendMessage(SmtpClient, string, string, string, MimeEntity)"/> for more.)</param>
        /// <returns>True if successful; False if unsuccessful.</returns>
        public static bool MailSendMessage(SmtpClient client, string sender, string Recipient, string Subject, string Body)
        {
            // Construct a message
            var FinalMessage = new MimeMessage();
            FinalMessage.From.Add(MailboxAddress.Parse(sender));
            DebugWriter.WriteDebug(DebugLevel.I, "Added sender to FinalMessage.From.");
            FinalMessage.To.Add(MailboxAddress.Parse(Recipient));
            DebugWriter.WriteDebug(DebugLevel.I, "Added address to FinalMessage.To.");
            FinalMessage.Subject = Subject;
            DebugWriter.WriteDebug(DebugLevel.I, "Added subject to FinalMessage.Subject.");
            FinalMessage.Body = new TextPart(TextFormat.Plain) { Text = Body };
            DebugWriter.WriteDebug(DebugLevel.I, "Added body to FinalMessage.Body (plain text). Sending message...");

            // Send the message
            return SendMessageInternal(client, FinalMessage);
        }

        /// <summary>
        /// Sends a message with advanced features like attachments
        /// </summary>
        /// <param name="client">IMAP mail client</param>
        /// <param name="sender">Sender name</param>
        /// <param name="Recipient">Recipient name</param>
        /// <param name="Subject">Subject</param>
        /// <param name="Body">Body</param>
        /// <returns>True if successful; False if unsuccessful.</returns>
        public static bool MailSendMessage(SmtpClient client, string sender, string Recipient, string Subject, MimeEntity Body)
        {
            // Construct a message
            var FinalMessage = new MimeMessage();
            FinalMessage.From.Add(MailboxAddress.Parse(sender));
            DebugWriter.WriteDebug(DebugLevel.I, "Added sender to FinalMessage.From.");
            FinalMessage.To.Add(MailboxAddress.Parse(Recipient));
            DebugWriter.WriteDebug(DebugLevel.I, "Added address to FinalMessage.To.");
            FinalMessage.Subject = Subject;
            DebugWriter.WriteDebug(DebugLevel.I, "Added subject to FinalMessage.Subject.");
            FinalMessage.Body = Body;
            DebugWriter.WriteDebug(DebugLevel.I, "Added body to FinalMessage.Body (plain text). Sending message...");

            // Send the message
            return SendMessageInternal(client, FinalMessage);
        }

        /// <summary>
        /// Sends an encrypted message with advanced features like attachments
        /// </summary>
        /// <param name="client">IMAP mail client</param>
        /// <param name="sender">Sender name</param>
        /// <param name="Recipient">Recipient name</param>
        /// <param name="Subject">Subject</param>
        /// <param name="Body">Body</param>
        /// <returns>True if successful; False if unsuccessful.</returns>
        public static bool MailSendEncryptedMessage(SmtpClient client, string sender, string Recipient, string Subject, MimeEntity Body)
        {
            // Construct a message
            var FinalMessage = new MimeMessage();
            FinalMessage.From.Add(MailboxAddress.Parse(sender));
            DebugWriter.WriteDebug(DebugLevel.I, "Added sender to FinalMessage.From.");
            FinalMessage.To.Add(MailboxAddress.Parse(Recipient));
            DebugWriter.WriteDebug(DebugLevel.I, "Added address to FinalMessage.To.");
            FinalMessage.Subject = Subject;
            DebugWriter.WriteDebug(DebugLevel.I, "Added subject to FinalMessage.Subject.");
            FinalMessage.Body = MultipartEncrypted.Encrypt(new PGPContext(), FinalMessage.To.Mailboxes, Body);
            DebugWriter.WriteDebug(DebugLevel.I, "Added body to FinalMessage.Body (encrypted). Sending message...");

            // Send the message
            return SendMessageInternal(client, FinalMessage);
        }

        private static bool SendMessageInternal(SmtpClient client, MimeMessage message)
        {
            // Send the message
            lock (client.SyncRoot)
            {
                try
                {
                    client.Send(message, default, Progress);
                    return true;
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to send message: {0}", vars: [ex.Message]);
                    DebugWriter.WriteDebugStackTrace(ex);
                }
                return false;
            }
        }

        private static MailFolder? TryOpenFolderFrom(ImapClient client, string FolderString, FolderNamespaceCollection namespcs, FolderAccess FolderMode = FolderAccess.ReadWrite)
        {
            List<MailFolder> folders = ListFoldersFrom(client, namespcs);
            foreach (MailFolder dir in folders)
            {
                if (dir.Name.Equals(FolderString, StringComparison.OrdinalIgnoreCase))
                    return TryOpenFolder(dir, FolderMode);
            }
            return null;
        }

        private static MailFolder? TryOpenFolder(IMailFolder dir, FolderAccess FolderMode = FolderAccess.ReadWrite)
        {
            try
            {
                dir.Open(FolderMode);
                return (MailFolder?)dir;
            }
            catch
            {
                return null;
            }
        }

        private static List<MailFolder> ListFoldersFrom(ImapClient client, FolderNamespaceCollection namespcs)
        {
            List<MailFolder> folders = [];
            lock (client.SyncRoot)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Namespace collection parsing started.");
                foreach (FolderNamespace nmspc in namespcs)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Namespace: {0}", vars: [nmspc.Path]);
                    foreach (MailFolder dir in client.GetFolders(nmspc).Cast<MailFolder>())
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Folder: {0}", vars: [dir.Name]);
                        folders.Add(dir);
                    }
                }
            }
            return folders;
        }
    }
}
