//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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
using System.Data;
using System.Linq;
using MailKit;
using MailKit.Net.Imap;
using MimeKit;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Extras.MailShell.Mail;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Terminaux.Base.Extensions;

namespace Nitrocid.Extras.MailShell.Tools.Directory
{
    /// <summary>
    /// Mail management module
    /// </summary>
    public static class MailManager
    {

        /// <summary>
        /// Whether to show the message preview or not
        /// </summary>
        public static bool ShowPreview =>
            MailShellInit.MailConfig.ShowPreview;

        /// <summary>
        /// Lists messages
        /// </summary>
        /// <param name="PageNum">Page number</param>
        /// <exception cref="ArgumentException"></exception>
        public static void MailListMessages(int PageNum) => MailListMessages(PageNum, MailShellCommon.MaxMessagesInPage);

        /// <summary>
        /// Lists messages
        /// </summary>
        /// <param name="PageNum">Page number</param>
        /// <param name="MessagesInPage">Max messages in one page</param>
        /// <exception cref="ArgumentException"></exception>
        public static void MailListMessages(int PageNum, int MessagesInPage)
        {
            // Sanity checks for the page number
            if (PageNum <= 0)
                PageNum = 1;
            DebugWriter.WriteDebug(DebugLevel.I, "Page number {0}", vars: [PageNum]);

            int MsgsLimitForPg = MessagesInPage;
            int FirstIndex = MsgsLimitForPg * PageNum - 10;
            int LastIndex = MsgsLimitForPg * PageNum - 1;
            var messages = MailShellCommon.IMAP_Messages ?? [];
            int MaxMessagesIndex = messages.Count() - 1;
            var client = (ImapClient)((object[]?)MailShellCommon.Client?.ConnectionInstance ?? [])[0];
            DebugWriter.WriteDebug(DebugLevel.I, "10 messages shown in each page. First message number in page {0} is {1} and last message number in page {0} is {2}", vars: [MsgsLimitForPg, FirstIndex, LastIndex]);
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
                        MimeMessage Msg;
                        if (!string.IsNullOrEmpty(MailShellCommon.IMAP_CurrentDirectory) & !(MailShellCommon.IMAP_CurrentDirectory == "Inbox"))
                        {
                            var Dir = MailDirectory.OpenFolder(MailShellCommon.IMAP_CurrentDirectory);
                            Msg = Dir.GetMessage(messages.ElementAtOrDefault(i), default, MailShellCommon.Progress);
                        }
                        else
                        {
                            Msg = client.Inbox.GetMessage(messages.ElementAtOrDefault(i), default, MailShellCommon.Progress);
                        }
                        MsgFrom = Msg.From.ToString();
                        MsgSubject = Msg.Subject;
                        MsgPreview = Msg.GetTextBody(MimeKit.Text.TextFormat.Text).Truncate(200);
                    }
                    DebugWriter.WriteDebug(DebugLevel.I, "From {0}: {1}", vars: [MsgFrom, MsgSubject]);

                    // Display them now.
                    TextWriters.Write($"- [{i + 1}/{MaxMessagesIndex + 1}] {MsgFrom}: ", false, KernelColorType.ListEntry);
                    TextWriters.Write(MsgSubject, true, KernelColorType.ListValue);
                    if (ShowPreview & !string.IsNullOrWhiteSpace(MsgPreview))
                    {
                        // For more efficient preview, use the PREVIEW extension as documented in RFC-8970 (https://tools.ietf.org/html/rfc8970). However,
                        // this is impossible at this time because no server and no client support this extension. It supports the LAZY modifier. It only
                        // displays 200 character long body.
                        //
                        // Concept: Msg.Preview(LazyMode:=True)
                        TextWriters.Write(MsgPreview, true, KernelColorType.ListValue);
                    }
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Reached max message limit. Message number {0}", vars: [i]);
                }
            }
        }

        /// <summary>
        /// Removes a message
        /// </summary>
        /// <param name="MsgNumber">Message number</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="ArgumentException"></exception>
        public static bool MailRemoveMessage(int MsgNumber)
        {
            int Message = MsgNumber - 1;
            var messages = MailShellCommon.IMAP_Messages ?? [];
            int MaxMessagesIndex = messages.Count() - 1;
            var client = (ImapClient)((object[]?)MailShellCommon.Client?.ConnectionInstance ?? [])[0];
            DebugWriter.WriteDebug(DebugLevel.I, "Message number {0}", vars: [Message]);
            if (Message < 0)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Trying to remove message 0 or less than 0.");
                throw new KernelException(KernelExceptionType.Mail, Translate.DoTranslation("Message number may not be negative or zero."));
            }
            else if (Message > MaxMessagesIndex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Message {0} not in list. It was larger than MaxMessagesIndex ({1})", vars: [Message, MaxMessagesIndex]);
                throw new KernelException(KernelExceptionType.Mail, Translate.DoTranslation("Message specified is not found."));
            }

            lock (client.SyncRoot)
            {
                if (!string.IsNullOrEmpty(MailShellCommon.IMAP_CurrentDirectory) & !(MailShellCommon.IMAP_CurrentDirectory == "Inbox"))
                {
                    // Remove message
                    var Dir = MailDirectory.OpenFolder(MailShellCommon.IMAP_CurrentDirectory);
                    DebugWriter.WriteDebug(DebugLevel.I, "Opened {0}. Removing {1}...", vars: [MailShellCommon.IMAP_CurrentDirectory, MsgNumber]);
                    Dir.Store(messages.ElementAtOrDefault(Message), new StoreFlagsRequest(StoreAction.Add, MessageFlags.Deleted));
                    DebugWriter.WriteDebug(DebugLevel.I, "Removed.");
                    Dir.Expunge();
                }
                else
                {
                    // Remove message
                    client.Inbox.Open(FolderAccess.ReadWrite);
                    DebugWriter.WriteDebug(DebugLevel.I, "Removing {0}...", vars: [MsgNumber]);
                    client.Inbox.Store(messages.ElementAtOrDefault(Message), new StoreFlagsRequest(StoreAction.Add, MessageFlags.Deleted));
                    DebugWriter.WriteDebug(DebugLevel.I, "Removed.");
                    client.Inbox.Expunge();
                }
            }
            return true;
        }

        /// <summary>
        /// Removes all mail that the specified sender has sent
        /// </summary>
        /// <param name="Sender">The sender name</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool MailRemoveAllBySender(string Sender)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "All mail by {0} will be removed.", vars: [Sender]);
            int DeletedMsgNumber = 1;
            int SteppedMsgNumber = 0;
            var messages = MailShellCommon.IMAP_Messages ?? [];
            var client = (ImapClient)((object[]?)MailShellCommon.Client?.ConnectionInstance ?? [])[0];
            for (int i = 0; i <= messages.Count(); i++)
            {
                try
                {
                    lock (client.SyncRoot)
                    {
                        var MessageId = messages.ElementAtOrDefault(i);
                        MimeMessage Msg;
                        if (!string.IsNullOrEmpty(MailShellCommon.IMAP_CurrentDirectory) & !(MailShellCommon.IMAP_CurrentDirectory == "Inbox"))
                        {
                            var Dir = MailDirectory.OpenFolder(MailShellCommon.IMAP_CurrentDirectory);
                            Msg = Dir.GetMessage(MessageId, default, MailShellCommon.Progress);
                        }
                        else
                        {
                            Msg = client.Inbox.GetMessage(MessageId, default, MailShellCommon.Progress);
                        }
                        SteppedMsgNumber += 1;

                        foreach (var address in Msg.From)
                        {
                            if (address.Name == Sender)
                            {
                                if (!string.IsNullOrEmpty(MailShellCommon.IMAP_CurrentDirectory) & !(MailShellCommon.IMAP_CurrentDirectory == "Inbox"))
                                {
                                    var Dir = MailDirectory.OpenFolder(MailShellCommon.IMAP_CurrentDirectory);

                                    // Remove message
                                    DebugWriter.WriteDebug(DebugLevel.I, "Opened {0}. Removing {1}...", vars: [MailShellCommon.IMAP_CurrentDirectory, Sender]);
                                    Dir.Store(MessageId, new StoreFlagsRequest(StoreAction.Add, MessageFlags.Deleted));
                                    DebugWriter.WriteDebug(DebugLevel.I, "Removed.");
                                    Dir.Expunge();
                                    DebugWriter.WriteDebug(DebugLevel.I, "Message {0} from {1} deleted from {2}. {3} messages remaining to parse.", vars: [DeletedMsgNumber, Sender, MailShellCommon.IMAP_CurrentDirectory, messages.Count() - SteppedMsgNumber]);
                                    TextWriterColor.Write(Translate.DoTranslation("Message {0} from {1} deleted from {2}. {3} messages remaining to parse."), DeletedMsgNumber, Sender, MailShellCommon.IMAP_CurrentDirectory, messages.Count() - SteppedMsgNumber);
                                }
                                else
                                {
                                    // Remove message
                                    client.Inbox.Open(FolderAccess.ReadWrite);
                                    DebugWriter.WriteDebug(DebugLevel.I, "Removing {0}...", vars: [Sender]);
                                    client.Inbox.Store(MessageId, new StoreFlagsRequest(StoreAction.Add, MessageFlags.Deleted));
                                    DebugWriter.WriteDebug(DebugLevel.I, "Removed.");
                                    client.Inbox.Expunge();
                                    DebugWriter.WriteDebug(DebugLevel.I, "Message {0} from {1} deleted from inbox. {2} messages remaining to parse.", vars: [DeletedMsgNumber, Sender, messages.Count() - SteppedMsgNumber]);
                                    TextWriterColor.Write(Translate.DoTranslation("Message {0} from {1} deleted from inbox. {2} messages remaining to parse."), DeletedMsgNumber, Sender, messages.Count() - SteppedMsgNumber);
                                }
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
        /// <param name="MsgNumber">Message number</param>
        /// <param name="TargetFolder">Target folder</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="ArgumentException"></exception>
        public static bool MailMoveMessage(int MsgNumber, string TargetFolder)
        {
            int Message = MsgNumber - 1;
            var messages = MailShellCommon.IMAP_Messages ?? [];
            int MaxMessagesIndex = messages.Count() - 1;
            var client = (ImapClient)((object[]?)MailShellCommon.Client?.ConnectionInstance ?? [])[0];
            DebugWriter.WriteDebug(DebugLevel.I, "Message number {0}", vars: [Message]);
            if (Message < 0)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Trying to move message 0 or less than 0.");
                throw new KernelException(KernelExceptionType.Mail, Translate.DoTranslation("Message number may not be negative or zero."));
            }
            else if (Message > MaxMessagesIndex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Message {0} not in list. It was larger than MaxMessagesIndex ({1})", vars: [Message, MaxMessagesIndex]);
                throw new KernelException(KernelExceptionType.Mail, Translate.DoTranslation("Message specified is not found."));
            }

            lock (client.SyncRoot)
            {
                if (!string.IsNullOrEmpty(MailShellCommon.IMAP_CurrentDirectory) & !(MailShellCommon.IMAP_CurrentDirectory == "Inbox"))
                {
                    // Move message
                    var Dir = MailDirectory.OpenFolder(MailShellCommon.IMAP_CurrentDirectory);
                    var TargetF = MailDirectory.OpenFolder(TargetFolder);
                    DebugWriter.WriteDebug(DebugLevel.I, "Opened {0}. Moving {1}...", vars: [MailShellCommon.IMAP_CurrentDirectory, MsgNumber]);
                    Dir.MoveTo(messages.ElementAtOrDefault(Message), TargetF);
                    DebugWriter.WriteDebug(DebugLevel.I, "Moved.");
                }
                else
                {
                    // Move message
                    var TargetF = MailDirectory.OpenFolder(TargetFolder);
                    DebugWriter.WriteDebug(DebugLevel.I, "Moving {0}...", vars: [MsgNumber]);
                    client.Inbox.Open(FolderAccess.ReadWrite);
                    client.Inbox.MoveTo(messages.ElementAtOrDefault(Message), TargetF);
                    DebugWriter.WriteDebug(DebugLevel.I, "Moved.");
                }
            }
            return true;
        }

        /// <summary>
        /// Moves all mail that the specified sender has sent
        /// </summary>
        /// <param name="Sender">The sender name</param>
        /// <param name="TargetFolder">Target folder</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool MailMoveAllBySender(string Sender, string TargetFolder)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "All mail by {0} will be moved.", vars: [Sender]);
            int DeletedMsgNumber = 1;
            int SteppedMsgNumber = 0;
            var client = (ImapClient)((object[]?)MailShellCommon.Client?.ConnectionInstance ?? [])[0];
            var messages = MailShellCommon.IMAP_Messages ?? [];
            for (int i = 0; i <= messages.Count(); i++)
            {
                try
                {
                    lock (client.SyncRoot)
                    {
                        var MessageId = messages.ElementAtOrDefault(i);
                        MimeMessage Msg;
                        if (!string.IsNullOrEmpty(MailShellCommon.IMAP_CurrentDirectory) & !(MailShellCommon.IMAP_CurrentDirectory == "Inbox"))
                        {
                            var Dir = MailDirectory.OpenFolder(MailShellCommon.IMAP_CurrentDirectory);
                            Msg = Dir.GetMessage(MessageId, default, MailShellCommon.Progress);
                        }
                        else
                        {
                            Msg = client.Inbox.GetMessage(MessageId, default, MailShellCommon.Progress);
                        }
                        SteppedMsgNumber += 1;

                        foreach (var address in Msg.From)
                        {
                            if (address.Name == Sender)
                            {
                                if (!string.IsNullOrEmpty(MailShellCommon.IMAP_CurrentDirectory) & !(MailShellCommon.IMAP_CurrentDirectory == "Inbox"))
                                {
                                    var Dir = MailDirectory.OpenFolder(MailShellCommon.IMAP_CurrentDirectory);
                                    var TargetF = MailDirectory.OpenFolder(TargetFolder);
                                    // Remove message
                                    DebugWriter.WriteDebug(DebugLevel.I, "Opened {0}. Moving {1}...", vars: [MailShellCommon.IMAP_CurrentDirectory, Sender]);
                                    Dir.MoveTo(MessageId, TargetF);
                                    DebugWriter.WriteDebug(DebugLevel.I, "Moved.");
                                    DebugWriter.WriteDebug(DebugLevel.I, "Message {0} from {1} moved from {2}. {3} messages remaining to parse.", vars: [DeletedMsgNumber, Sender, MailShellCommon.IMAP_CurrentDirectory, messages.Count() - SteppedMsgNumber]);
                                    TextWriterColor.Write(Translate.DoTranslation("Message {0} from {1} moved from {2}. {3} messages remaining to parse."), DeletedMsgNumber, Sender, MailShellCommon.IMAP_CurrentDirectory, messages.Count() - SteppedMsgNumber);
                                }
                                else
                                {
                                    // Remove message
                                    var TargetF = MailDirectory.OpenFolder(TargetFolder);
                                    DebugWriter.WriteDebug(DebugLevel.I, "Moving {0}...", vars: [Sender]);
                                    client.Inbox.Open(FolderAccess.ReadWrite);
                                    client.Inbox.MoveTo(MessageId, TargetF);
                                    DebugWriter.WriteDebug(DebugLevel.I, "Moved.");
                                    DebugWriter.WriteDebug(DebugLevel.I, "Message {0} from {1} moved. {2} messages remaining to parse.", vars: [DeletedMsgNumber, Sender, messages.Count() - SteppedMsgNumber]);
                                    TextWriterColor.Write(Translate.DoTranslation("Message {0} from {1} moved. {2} messages remaining to parse."), DeletedMsgNumber, Sender, messages.Count() - SteppedMsgNumber);
                                }
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

    }
}
