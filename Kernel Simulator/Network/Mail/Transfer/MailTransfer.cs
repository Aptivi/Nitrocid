
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Extensification.StringExts;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Network.Mail.Directory;
using KS.Network.Mail.PGP;
using KS.Shell.Shells.Mail;
using KS.TimeDate;
using MailKit;
using MailKit.Search;
using MimeKit;
using MimeKit.Cryptography;
using MimeKit.Text;

namespace KS.Network.Mail.Transfer
{
    public static class MailTransfer
    {

        /// <summary>
        /// Prints content of message to console
        /// </summary>
        /// <param name="MessageNum">Message number</param>
        /// <param name="Decrypt">Whether to decrypt messages or not</param>
        public static void MailPrintMessage(int MessageNum, bool Decrypt = false)
        {
            int Message = MessageNum - 1;
            int MaxMessagesIndex = MailShellCommon.IMAP_Messages.Count() - 1;
            DebugWriter.Wdbg(DebugLevel.I, "Message number {0}", Message);
            if (Message < 0)
            {
                DebugWriter.Wdbg(DebugLevel.E, "Trying to access message 0 or less than 0.");
                TextWriterColor.Write(Translate.DoTranslation("Message number may not be negative or zero."), true, ColorTools.ColTypes.Error);
                return;
            }
            else if (Message > MaxMessagesIndex)
            {
                DebugWriter.Wdbg(DebugLevel.E, "Message {0} not in list. It was larger than MaxMessagesIndex ({1})", Message, MaxMessagesIndex);
                TextWriterColor.Write(Translate.DoTranslation("Message specified is not found."), true, ColorTools.ColTypes.Error);
                return;
            }

            lock (MailLogin.IMAP_Client.SyncRoot)
            {
                // Get message
                DebugWriter.Wdbg(DebugLevel.I, "Getting message...");
                MimeMessage Msg;
                if (!string.IsNullOrEmpty(MailShellCommon.IMAP_CurrentDirectory) & !(MailShellCommon.IMAP_CurrentDirectory == "Inbox"))
                {
                    var Dir = MailDirectory.OpenFolder(MailShellCommon.IMAP_CurrentDirectory);
                    Msg = Dir.GetMessage(MailShellCommon.IMAP_Messages.ElementAtOrDefault(Message), default, MailShellCommon.Mail_Progress);
                }
                else
                {
                    Msg = MailLogin.IMAP_Client.Inbox.GetMessage(MailShellCommon.IMAP_Messages.ElementAtOrDefault(Message), default, MailShellCommon.Mail_Progress);
                }

                // Prepare view
                Console.WriteLine();

                // Print all the addresses that sent the mail
                DebugWriter.Wdbg(DebugLevel.I, "{0} senders.", Msg.From.Count);
                foreach (InternetAddress Address in Msg.From)
                {
                    DebugWriter.Wdbg(DebugLevel.I, "Address: {0} ({1})", Address.Name, Address.Encoding.EncodingName);
                    TextWriterColor.Write(Translate.DoTranslation("- From {0}"), true, ColorTools.ColTypes.ListEntry, Address.ToString());
                }

                // Print all the addresses that received the mail
                DebugWriter.Wdbg(DebugLevel.I, "{0} receivers.", Msg.To.Count);
                foreach (InternetAddress Address in Msg.To)
                {
                    DebugWriter.Wdbg(DebugLevel.I, "Address: {0} ({1})", Address.Name, Address.Encoding.EncodingName);
                    TextWriterColor.Write(Translate.DoTranslation("- To {0}"), true, ColorTools.ColTypes.ListEntry, Address.ToString());
                }

                // Print the date and time when the user received the mail
                DebugWriter.Wdbg(DebugLevel.I, "Rendering time and date of {0}.", Msg.Date.DateTime.ToString());
                TextWriterColor.Write(Translate.DoTranslation("- Sent at {0} in {1}"), true, ColorTools.ColTypes.ListEntry, TimeDateRenderers.RenderTime(Msg.Date.DateTime), TimeDateRenderers.RenderDate(Msg.Date.DateTime));

                // Prepare subject
                Console.WriteLine();
                DebugWriter.Wdbg(DebugLevel.I, "Subject length: {0}, {1}", Msg.Subject.Length, Msg.Subject);
                TextWriterColor.Write($"- {Msg.Subject}", false, ColorTools.ColTypes.ListEntry);

                // Write a sign after the subject if attachments are found
                DebugWriter.Wdbg(DebugLevel.I, "Attachments count: {0}", Msg.Attachments.Count());
                if (Msg.Attachments.Count() > 0)
                {
                    TextWriterColor.Write(" - [*]", true, ColorTools.ColTypes.ListEntry);
                }
                else
                {
                    Console.WriteLine();
                }

                // Prepare body
                Console.WriteLine();
                DebugWriter.Wdbg(DebugLevel.I, "Displaying body...");
                var DecryptedMessage = default(Dictionary<string, MimeEntity>);
                DebugWriter.Wdbg(DebugLevel.I, "To decrypt: {0}", Decrypt);
                if (Decrypt)
                {
                    DecryptedMessage = DecryptMessage(Msg);
                    DebugWriter.Wdbg(DebugLevel.I, "Decrypted messages length: {0}", DecryptedMessage.Count);
                    var DecryptedEntity = DecryptedMessage["Body"];
                    var DecryptedStream = new MemoryStream();
                    DebugWriter.Wdbg(DebugLevel.I, $"Decrypted message type: {(DecryptedEntity is Multipart ? "Multipart" : "Singlepart")}");
                    if (DecryptedEntity is Multipart)
                    {
                        Multipart MultiEntity = (Multipart)DecryptedEntity;
                        DebugWriter.Wdbg(DebugLevel.I, $"Decrypted message entity is {(MultiEntity is not null ? "multipart" : "nothing")}");
                        if (MultiEntity is not null)
                        {
                            for (int EntityNumber = 0, loopTo = MultiEntity.Count - 1; EntityNumber <= loopTo; EntityNumber++)
                            {
                                DebugWriter.Wdbg(DebugLevel.I, $"Entity number {EntityNumber} is {(MultiEntity[EntityNumber].IsAttachment ? "an attachment" : "not an attachment")}");
                                if (!MultiEntity[EntityNumber].IsAttachment)
                                {
                                    MultiEntity[EntityNumber].WriteTo(DecryptedStream, true);
                                    DebugWriter.Wdbg(DebugLevel.I, "Written {0} bytes to stream.", DecryptedStream.Length);
                                    DecryptedStream.Position = 0L;
                                    var DecryptedByte = new byte[(int)(DecryptedStream.Length + 1)];
                                    DecryptedStream.Read(DecryptedByte, 0, (int)DecryptedStream.Length);
                                    DebugWriter.Wdbg(DebugLevel.I, "Written {0} bytes to buffer.", DecryptedByte.Length);
                                    TextWriterColor.Write(Encoding.Default.GetString(DecryptedByte), true, ColorTools.ColTypes.ListValue);
                                }
                            }
                        }
                    }
                    else
                    {
                        DecryptedEntity.WriteTo(DecryptedStream, true);
                        DebugWriter.Wdbg(DebugLevel.I, "Written {0} bytes to stream.", DecryptedStream.Length);
                        DecryptedStream.Position = 0L;
                        var DecryptedByte = new byte[(int)(DecryptedStream.Length + 1)];
                        DecryptedStream.Read(DecryptedByte, 0, (int)DecryptedStream.Length);
                        DebugWriter.Wdbg(DebugLevel.I, "Written {0} bytes to buffer.", DecryptedByte.Length);
                        TextWriterColor.Write(Encoding.Default.GetString(DecryptedByte), true, ColorTools.ColTypes.ListValue);
                    }
                }
                else
                {
                    TextWriterColor.Write(Msg.GetTextBody(MailShellCommon.Mail_TextFormat), true, ColorTools.ColTypes.ListValue);
                }
                Console.WriteLine();

                // Populate attachments
                if (Msg.Attachments.Count() > 0)
                {
                    TextWriterColor.Write(Translate.DoTranslation("Attachments:"), true, ColorTools.ColTypes.Neutral);
                    var AttachmentEntities = new List<MimeEntity>();
                    if (Decrypt)
                    {
                        DebugWriter.Wdbg(DebugLevel.I, "Parsing attachments...");
                        for (int DecryptedEntityNumber = 0, loopTo1 = DecryptedMessage.Count - 1; DecryptedEntityNumber <= loopTo1; DecryptedEntityNumber++)
                        {
                            DebugWriter.Wdbg(DebugLevel.I, "Is entity number {0} an attachment? {1}", DecryptedEntityNumber, DecryptedMessage.Keys.ElementAtOrDefault(DecryptedEntityNumber).Contains("Attachment"));
                            DebugWriter.Wdbg(DebugLevel.I, "Is entity number {0} a body that is a multipart? {1}", DecryptedEntityNumber, DecryptedMessage.Keys.ElementAtOrDefault(DecryptedEntityNumber) == "Body" & DecryptedMessage["Body"] is Multipart);
                            if (DecryptedMessage.Keys.ElementAtOrDefault(DecryptedEntityNumber).Contains("Attachment"))
                            {
                                DebugWriter.Wdbg(DebugLevel.I, "Adding entity {0} to attachment entities...", DecryptedEntityNumber);
                                AttachmentEntities.Add(DecryptedMessage.Values.ElementAtOrDefault(DecryptedEntityNumber));
                            }
                            else if (DecryptedMessage.Keys.ElementAtOrDefault(DecryptedEntityNumber) == "Body" & DecryptedMessage["Body"] is Multipart)
                            {
                                Multipart MultiEntity = (Multipart)DecryptedMessage["Body"];
                                DebugWriter.Wdbg(DebugLevel.I, $"Decrypted message entity is {(MultiEntity is not null ? "multipart" : "nothing")}");
                                if (MultiEntity is not null)
                                {
                                    DebugWriter.Wdbg(DebugLevel.I, "{0} entities found.", MultiEntity.Count);
                                    for (int EntityNumber = 0, loopTo2 = MultiEntity.Count - 1; EntityNumber <= loopTo2; EntityNumber++)
                                    {
                                        DebugWriter.Wdbg(DebugLevel.I, $"Entity number {EntityNumber} is {(MultiEntity[EntityNumber].IsAttachment ? "an attachment" : "not an attachment")}");
                                        if (MultiEntity[EntityNumber].IsAttachment)
                                        {
                                            DebugWriter.Wdbg(DebugLevel.I, "Adding entity {0} to attachment list...", EntityNumber);
                                            AttachmentEntities.Add(MultiEntity[EntityNumber]);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        AttachmentEntities = (List<MimeEntity>)Msg.Attachments;
                    }
                    foreach (MimeEntity Attachment in AttachmentEntities)
                    {
                        DebugWriter.Wdbg(DebugLevel.I, "Attachment ID: {0}", Attachment.ContentId);
                        if (Attachment is MessagePart)
                        {
                            DebugWriter.Wdbg(DebugLevel.I, "Attachment is a message.");
                            TextWriterColor.Write($"- {Attachment.ContentDisposition?.FileName}", true, ColorTools.ColTypes.Neutral);
                        }
                        else
                        {
                            DebugWriter.Wdbg(DebugLevel.I, "Attachment is a file.");
                            MimePart AttachmentPart = (MimePart)Attachment;
                            TextWriterColor.Write($"- {AttachmentPart.FileName}", true, ColorTools.ColTypes.Neutral);
                        }
                    }
                }
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
            DebugWriter.Wdbg(DebugLevel.I, $"Encrypted message type: {(Text.Body is MultipartEncrypted ? "Multipart" : "Singlepart")}");
            if (Text.Body is MultipartEncrypted)
            {
                MultipartEncrypted Encrypted = (MultipartEncrypted)Text.Body;
                DebugWriter.Wdbg(DebugLevel.I, $"Message type: {(Encrypted is not null ? "MultipartEncrypted" : "Nothing")}");
                DebugWriter.Wdbg(DebugLevel.I, "Decrypting...");
                EncryptedDict.Add("Body", Encrypted.Decrypt(new PGPContext()));
            }
            else
            {
                DebugWriter.Wdbg(DebugLevel.W, "Trying to decrypt plain text. Returning body...");
                EncryptedDict.Add("Body", Text.Body);
            }
            int AttachmentNumber = 1;
            foreach (MimeEntity TextAttachment in Text.Attachments)
            {
                DebugWriter.Wdbg(DebugLevel.I, "Attachment number {0}", AttachmentNumber);
                DebugWriter.Wdbg(DebugLevel.I, $"Encrypted attachment type: {(TextAttachment is MultipartEncrypted ? "Multipart" : "Singlepart")}");
                if (TextAttachment is MultipartEncrypted)
                {
                    MultipartEncrypted Encrypted = (MultipartEncrypted)TextAttachment;
                    DebugWriter.Wdbg(DebugLevel.I, $"Attachment type: {(Encrypted is not null ? "MultipartEncrypted" : "Nothing")}");
                    DebugWriter.Wdbg(DebugLevel.I, "Decrypting...");
                    EncryptedDict.Add("Attachment " + AttachmentNumber, Encrypted.Decrypt(new PGPContext()));
                }
                else
                {
                    DebugWriter.Wdbg(DebugLevel.W, "Trying to decrypt plain attachment. Returning body...");
                    EncryptedDict.Add("Attachment " + AttachmentNumber, TextAttachment);
                }
                AttachmentNumber += 1;
            }
            return EncryptedDict;
        }

        /// <summary>
        /// Sends a message
        /// </summary>
        /// <param name="Recipient">Recipient name</param>
        /// <param name="Subject">Subject</param>
        /// <param name="Body">Body (only text. See <see cref="MailSendMessage(string, string, MimeEntity)"/> for more.)</param>
        /// <returns>True if successful; False if unsuccessful.</returns>
        public static string MailSendMessage(string Recipient, string Subject, string Body)
        {
            // Construct a message
            var FinalMessage = new MimeMessage();
            FinalMessage.From.Add(MailboxAddress.Parse(MailLogin.Mail_Authentication.UserName));
            DebugWriter.Wdbg(DebugLevel.I, "Added sender to FinalMessage.From.");
            FinalMessage.To.Add(MailboxAddress.Parse(Recipient));
            DebugWriter.Wdbg(DebugLevel.I, "Added address to FinalMessage.To.");
            FinalMessage.Subject = Subject;
            DebugWriter.Wdbg(DebugLevel.I, "Added subject to FinalMessage.Subject.");
            FinalMessage.Body = new TextPart(TextFormat.Plain) { Text = Body.ToString() };
            DebugWriter.Wdbg(DebugLevel.I, "Added body to FinalMessage.Body (plain text). Sending message...");

            // Send the message
            lock (MailLogin.SMTP_Client.SyncRoot)
            {
                try
                {
                    MailLogin.SMTP_Client.Send(FinalMessage, default, MailShellCommon.Mail_Progress);
                    return Convert.ToString(true);
                }
                catch (Exception ex)
                {
                    DebugWriter.Wdbg(DebugLevel.E, "Failed to send message: {0}", ex.Message);
                    DebugWriter.WStkTrc(ex);
                }
                return Convert.ToString(false);
            }
        }

        /// <summary>
        /// Sends a message with advanced features like attachments
        /// </summary>
        /// <param name="Recipient">Recipient name</param>
        /// <param name="Subject">Subject</param>
        /// <param name="Body">Body</param>
        /// <returns>True if successful; False if unsuccessful.</returns>
        public static string MailSendMessage(string Recipient, string Subject, MimeEntity Body)
        {
            // Construct a message
            var FinalMessage = new MimeMessage();
            FinalMessage.From.Add(MailboxAddress.Parse(MailLogin.Mail_Authentication.UserName));
            DebugWriter.Wdbg(DebugLevel.I, "Added sender to FinalMessage.From.");
            FinalMessage.To.Add(MailboxAddress.Parse(Recipient));
            DebugWriter.Wdbg(DebugLevel.I, "Added address to FinalMessage.To.");
            FinalMessage.Subject = Subject;
            DebugWriter.Wdbg(DebugLevel.I, "Added subject to FinalMessage.Subject.");
            FinalMessage.Body = Body;
            DebugWriter.Wdbg(DebugLevel.I, "Added body to FinalMessage.Body (plain text). Sending message...");

            // Send the message
            lock (MailLogin.SMTP_Client.SyncRoot)
            {
                try
                {
                    MailLogin.SMTP_Client.Send(FinalMessage, default, MailShellCommon.Mail_Progress);
                    return Convert.ToString(true);
                }
                catch (Exception ex)
                {
                    DebugWriter.Wdbg(DebugLevel.E, "Failed to send message: {0}", ex.Message);
                    DebugWriter.WStkTrc(ex);
                }
                return Convert.ToString(false);
            }
        }

        /// <summary>
        /// Sends an encrypted message with advanced features like attachments
        /// </summary>
        /// <param name="Recipient">Recipient name</param>
        /// <param name="Subject">Subject</param>
        /// <param name="Body">Body</param>
        /// <returns>True if successful; False if unsuccessful.</returns>
        public static string MailSendEncryptedMessage(string Recipient, string Subject, MimeEntity Body)
        {
            // Construct a message
            var FinalMessage = new MimeMessage();
            FinalMessage.From.Add(MailboxAddress.Parse(MailLogin.Mail_Authentication.UserName));
            DebugWriter.Wdbg(DebugLevel.I, "Added sender to FinalMessage.From.");
            FinalMessage.To.Add(MailboxAddress.Parse(Recipient));
            DebugWriter.Wdbg(DebugLevel.I, "Added address to FinalMessage.To.");
            FinalMessage.Subject = Subject;
            DebugWriter.Wdbg(DebugLevel.I, "Added subject to FinalMessage.Subject.");
            FinalMessage.Body = MultipartEncrypted.Encrypt(new PGPContext(), FinalMessage.To.Mailboxes, Body);
            DebugWriter.Wdbg(DebugLevel.I, "Added body to FinalMessage.Body (plain text). Sending message...");

            // Send the message
            lock (MailLogin.SMTP_Client.SyncRoot)
            {
                try
                {
                    MailLogin.SMTP_Client.Send(FinalMessage, default, MailShellCommon.Mail_Progress);
                    return Convert.ToString(true);
                }
                catch (Exception ex)
                {
                    DebugWriter.Wdbg(DebugLevel.E, "Failed to send message: {0}", ex.Message);
                    DebugWriter.WStkTrc(ex);
                }
                return Convert.ToString(false);
            }
        }

        /// <summary>
        /// Populates e-mail messages
        /// </summary>
        public static void PopulateMessages()
        {
            if (MailLogin.IMAP_Client.IsConnected)
            {
                lock (MailLogin.IMAP_Client.SyncRoot)
                {
                    if (string.IsNullOrEmpty(MailShellCommon.IMAP_CurrentDirectory) | MailShellCommon.IMAP_CurrentDirectory == "Inbox")
                    {
                        MailLogin.IMAP_Client.Inbox.Open(FolderAccess.ReadWrite);
                        DebugWriter.Wdbg(DebugLevel.I, "Opened inbox");
                        MailShellCommon.IMAP_Messages = MailLogin.IMAP_Client.Inbox.Search(SearchQuery.All).Reverse();
                        DebugWriter.Wdbg(DebugLevel.I, "Messages count: {0} messages", MailShellCommon.IMAP_Messages.LongCount());
                    }
                    else
                    {
                        var Folder = MailDirectory.OpenFolder(MailShellCommon.IMAP_CurrentDirectory);
                        DebugWriter.Wdbg(DebugLevel.I, "Opened {0}", MailShellCommon.IMAP_CurrentDirectory);
                        MailShellCommon.IMAP_Messages = Folder.Search(SearchQuery.All).Reverse();
                        DebugWriter.Wdbg(DebugLevel.I, "Messages count: {0} messages", MailShellCommon.IMAP_Messages.LongCount());
                    }
                }
            }
        }

    }
}
