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
using System.Text;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Time.Renderers;
using Nitrocid.Base.Languages;
using Terminaux.Inputs.Interactive;
using Terminaux.Inputs.Styles.Infobox;
using Textify.General;
using MailKit;
using MimeKit;
using Nitrocid.ShellPacks.Tools.Directory;
using Nitrocid.ShellPacks.Tools.Transfer;
using System.Linq;
using MailKit.Net.Imap;
using MimeKit.Cryptography;
using Terminaux.Inputs.Styles;
using MimeKit.Text;
using Terminaux.Inputs.Styles.Infobox.Tools;

namespace Nitrocid.ShellPacks.Shells.Mail.Interactive
{
    internal class MailManagerCli : BaseInteractiveTui<MailFolder, MimeMessage>, IInteractiveTui<MailFolder, MimeMessage>
    {
        internal bool refreshFirstPaneListing = true;
        internal bool refreshSecondPaneListing = true;
        private List<MailFolder> firstPaneListing = [];
        private List<MimeMessage> secondPaneListing = [];
        private int pageNum = 1;

        /// <inheritdoc/>
        public override InteractiveTuiHelpPage[] HelpPages =>
        [
            new()
            {
                HelpTitle = /* Localizable */ "NKS_SHELLPACKS_MAIL_TUI_HELP01_TITLE",
                HelpDescription = /* Localizable */ "NKS_SHELLPACKS_MAIL_TUI_HELP01_DESC",
                HelpBody =
                    LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_TUI_HELP01_BODY") + "\n\n" +
#pragma warning disable NLOC0001
                    LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_COMMON_HELP_MOREINFO") + ": https://aptivi.gitbook.io/aptivi/nitrocid-ks-manual/fundamentals/simulated-kernel-features/extra-features/more-networking/mail-client",
#pragma warning restore NLOC0001
            },
            new()
            {
                HelpTitle = /* Localizable */ "NKS_SHELLPACKS_MAIL_TUI_HELP02_TITLE",
                HelpDescription = /* Localizable */ "NKS_SHELLPACKS_MAIL_TUI_HELP02_DESC",
                HelpBody =
                    LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_TUI_HELP02_BODY") + "\n\n" +
#pragma warning disable NLOC0001
                    LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_COMMON_HELP_MOREINFO") + ": https://aptivi.gitbook.io/aptivi/nitrocid-ks-manual/fundamentals/simulated-kernel-features/extra-features/more-networking/mail-client",
#pragma warning restore NLOC0001
            }
        ];

        public override bool SecondPaneInteractable =>
            true;

        /// <inheritdoc/>
        public override IEnumerable<MailFolder> PrimaryDataSource
        {
            get
            {
                try
                {
                    if (refreshFirstPaneListing)
                    {
                        refreshFirstPaneListing = false;
                        firstPaneListing = [.. MailDirectory.MailListDirectories()];
                    }
                    return firstPaneListing;
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to get directory list: {0}", vars: [ex.Message]);
                    DebugWriter.WriteDebugStackTrace(ex);
                    return [];
                }
            }
        }

        /// <inheritdoc/>
        public override IEnumerable<MimeMessage> SecondaryDataSource
        {
            get
            {
                try
                {
                    if (refreshSecondPaneListing)
                    {
                        refreshSecondPaneListing = false;
                        secondPaneListing.Clear();
                        MailTransfer.PopulateMessages();
                        int MsgsLimitForPg = ShellsInit.ShellsConfig.MailMaxMessagesInPage;
                        int FirstIndex = MsgsLimitForPg * pageNum - 10;
                        int LastIndex = MsgsLimitForPg * pageNum - 1;
                        var messages = MailShellCommon.IMAP_Messages ?? [];
                        int MaxMessagesIndex = messages.Count() - 1;
                        var client = (ImapClient)((object[]?)MailShellCommon.Client?.ConnectionInstance ?? [])[0];

                        for (int i = FirstIndex; i <= LastIndex; i++)
                        {
                            if (i <= MaxMessagesIndex)
                            {
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
                                        Msg = client.Inbox.GetMessage(messages.ElementAtOrDefault(i), default, MailShellCommon.Progress);
                                    secondPaneListing.Add(Msg);
                                }
                            }
                            else
                                DebugWriter.WriteDebug(DebugLevel.W, "Reached max message limit. Message number {0}", vars: [i]);
                        }
                    }
                    return secondPaneListing;
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to get mail message list for the second pane [{0}]: {1}", vars: [MailShellCommon.IMAP_CurrentDirectory, ex.Message]);
                    DebugWriter.WriteDebugStackTrace(ex);
                    return [];
                }
            }
        }

        /// <inheritdoc/>
        public override bool AcceptsEmptyData =>
            true;

        /// <inheritdoc/>
        public override string GetStatusFromItem(MailFolder item)
        {
            try
            {
                return item.FullName;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <inheritdoc/>
        public override string GetEntryFromItem(MailFolder item)
        {
            try
            {
                return item.Name;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <inheritdoc/>
        public override string GetStatusFromItemSecondary(MimeMessage item)
        {
            try
            {
                string from = item.From.ToString();
                string subject = item.Subject;
                return $"[{from}] {subject}";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <inheritdoc/>
        public override string GetEntryFromItemSecondary(MimeMessage item)
        {
            try
            {
                string from = item.From.ToString();
                string subject = item.Subject;
                int replyOccurences = 0;
                while (subject.StartsWithNoCase("Re: "))
                    replyOccurences++;
                return $"{new string(' ', replyOccurences * 2)}[{from}] {subject}";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        internal void Open(MailFolder? entry1, MimeMessage? entry2)
        {
            try
            {
                // Don't do anything if we haven't been provided anything.
                if (entry1 is null && entry2 is null)
                    return;

                // Determine whether to deal with the message or with the folder
                if (CurrentPane == 2)
                {
                    // We are dealing with the remote side.
                    var currentEntry = entry2;
                    if (currentEntry is null)
                        return;

                    // We're dealing with a message. Open it in a separate infobox.
                    var messageBuilder = new StringBuilder();
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
                    DebugWriter.WriteDebug(DebugLevel.I, "Subject length: {0}, {1}", vars: [currentEntry.Subject.Length, currentEntry.Subject]);
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
                    if (currentEntry.Body is MultipartEncrypted)
                    {
                        DecryptedMessage = MailTransfer.DecryptMessage(currentEntry);
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
                        if (currentEntry.Body is MultipartEncrypted)
                        {
                            DebugWriter.WriteDebug(DebugLevel.I, "Parsing attachments...");
                            if (DecryptedMessage is null)
                                return;
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

                    InfoBoxModalColor.WriteInfoBoxModal(messageBuilder.ToString());
                    refreshSecondPaneListing = true;
                }
                else
                {
                    // We are dealing with the local side.
                    var currentEntry = entry1;
                    if (currentEntry is null || !currentEntry.Exists)
                        return;

                    // We're dealing with a folder. Open it in the selected pane.
                    MailDirectory.MailChangeDirectory(currentEntry.FullName);
                    InteractiveTuiTools.SelectionMovement(this, 1);
                    refreshFirstPaneListing = true;
                    refreshSecondPaneListing = true;
                }
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_TUI_FOLDERMESSAGEOPENFAILED") + ": {0}".FormatString(ex.Message));
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
            }
        }

        internal void MakeFolder()
        {
            try
            {
                // Determine whether to deal with the message or with the folder
                if (CurrentPane == 1)
                {
                    string directoryName = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_TUI_NEWDIRNAMEPROMPT"), Settings.InfoBoxSettings);
                    InfoBoxNonModalColor.WriteInfoBox(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_TUI_CREATINGDIR"), Settings.InfoBoxSettings);
                    MailDirectory.CreateMailDirectory(directoryName);
                    refreshFirstPaneListing = true;
                    refreshSecondPaneListing = true;
                }
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_TUI_CREATINGDIRFAILED") + ": {0}".FormatString(ex.Message));
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
            }
        }

        internal void MoveMessage(int messageIdx)
        {
            try
            {
                // Determine whether to deal with the message or with the folder
                if (CurrentPane == 2)
                {
                    InputChoiceInfo[] choices = firstPaneListing.Select((mf, idx) => new InputChoiceInfo($"{idx + 1}", mf.FullName)).ToArray();
                    int directoryIdx = InfoBoxSelectionColor.WriteInfoBoxSelection(choices, LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_TUI_NEWDIRTOMOVEPROMPT"), Settings.InfoBoxSettings);
                    if (directoryIdx < 0)
                        return;

                    // Move the message to a specified directory
                    InfoBoxNonModalColor.WriteInfoBox(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_TUI_MOVINGMESSAGE"), Settings.InfoBoxSettings);
                    MailManager.MailMoveMessage(messageIdx + 1, firstPaneListing[directoryIdx].Name);
                    refreshFirstPaneListing = true;
                    refreshSecondPaneListing = true;
                }
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_TUI_MOVINGMESSAGEFAILED") + ": {0}".FormatString(ex.Message));
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
            }
        }

        internal void MoveAllMessages(int messageIdx)
        {
            try
            {
                // Determine whether to deal with the message or with the folder
                if (CurrentPane == 2)
                {
                    InputChoiceInfo[] choices = firstPaneListing.Select((mf, idx) => new InputChoiceInfo($"{idx + 1}", mf.FullName)).ToArray();
                    int directoryIdx = InfoBoxSelectionColor.WriteInfoBoxSelection(choices, LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_TUI_NEWDIRTOMOVEBYSAMESENDERPROMPT"), Settings.InfoBoxSettings);
                    if (directoryIdx < 0)
                        return;

                    // Move the message to a specified directory
                    var addresses = secondPaneListing[messageIdx].From;
                    foreach (var address in addresses)
                    {
                        InfoBoxNonModalColor.WriteInfoBox(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_TUI_MOVINGMESSAGEBYSENDER"), Settings.InfoBoxSettings, address.Name);
                        MailManager.MailMoveAllBySender(address.Name, firstPaneListing[directoryIdx].Name);
                    }
                    refreshFirstPaneListing = true;
                    refreshSecondPaneListing = true;
                }
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_TUI_MOVINGMESSAGEBYSENDERFAILED") + ": {0}".FormatString(ex.Message));
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
            }
        }

        internal void RenameFolder(MailFolder? folder)
        {
            try
            {
                // Don't do anything if we haven't been provided anything.
                if (folder is null)
                    return;

                // Determine whether to deal with the message or with the folder
                if (CurrentPane == 1)
                {
                    string directoryName = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_TUI_NEWDIRNAMERENAMEPROMPT"), Settings.InfoBoxSettings, InfoBoxInputType.Text, folder.Name);
                    InfoBoxNonModalColor.WriteInfoBox(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_TUI_RENAMINGDIR"), Settings.InfoBoxSettings);
                    MailDirectory.RenameMailDirectory(folder.Name, directoryName);
                    refreshFirstPaneListing = true;
                    refreshSecondPaneListing = true;
                }
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_TUI_RENAMINGDIRFAILED") + ": {0}".FormatString(ex.Message));
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
            }
        }

        internal void RemoveFolder(MailFolder? folder)
        {
            try
            {
                // Don't do anything if we haven't been provided anything.
                if (folder is null)
                    return;

                // Determine whether to deal with the message or with the folder
                if (CurrentPane == 1)
                {
                    InfoBoxNonModalColor.WriteInfoBox(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_TUI_REMOVINGDIR"), Settings.InfoBoxSettings);
                    MailDirectory.DeleteMailDirectory(folder.Name);
                    InteractiveTuiTools.SelectionMovement(this, FirstPaneCurrentSelection - 1);
                    refreshFirstPaneListing = true;
                    refreshSecondPaneListing = true;
                }
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_TUI_REMOVINGDIRFAILED") + ": {0}".FormatString(ex.Message));
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
            }
        }

        internal void RemoveMessage(int msgIdx)
        {
            try
            {
                // Determine whether to deal with the message or with the folder
                if (CurrentPane == 2)
                {
                    InfoBoxNonModalColor.WriteInfoBox(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_TUI_REMOVINGMESSAGE"), Settings.InfoBoxSettings);
                    MailManager.MailRemoveMessage(msgIdx + 1);
                    InteractiveTuiTools.SelectionMovement(this, SecondPaneCurrentSelection - 1);
                    refreshFirstPaneListing = true;
                    refreshSecondPaneListing = true;
                }
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_TUI_REMOVINGMESSAGEFAILED") + ": {0}".FormatString(ex.Message));
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
            }
        }

        internal void RemoveAllMessages(int msgIdx)
        {
            try
            {
                // Determine whether to deal with the message or with the folder
                if (CurrentPane == 2)
                {
                    var addresses = secondPaneListing[msgIdx].From;
                    foreach (var address in addresses)
                    {
                        InfoBoxNonModalColor.WriteInfoBox(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_TUI_REMOVINGMESSAGEBYSENDER"), Settings.InfoBoxSettings, address.Name);
                        MailManager.MailRemoveAllBySender(address.Name);
                    }
                    InteractiveTuiTools.SelectionMovement(this, SecondPaneCurrentSelection - 1);
                    refreshFirstPaneListing = true;
                    refreshSecondPaneListing = true;
                }
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_TUI_REMOVINGMESSAGEBYSENDERFAILED") + ": {0}".FormatString(ex.Message));
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
            }
        }
    }
}
