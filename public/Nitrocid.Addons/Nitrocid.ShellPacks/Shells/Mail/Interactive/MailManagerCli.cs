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
using System.Text;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Time.Renderers;
using Nitrocid.Base.Languages;
using Terminaux.Inputs.Interactive;
using Terminaux.Inputs.Styles.Infobox;
using Textify.General;
using MailKit;
using MimeKit;
using System.Linq;
using MimeKit.Cryptography;
using Terminaux.Inputs.Styles;
using MimeKit.Text;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.ShellPacks.Shells.Mail.Tools;

namespace Nitrocid.ShellPacks.Shells.Mail.Interactive
{
    internal class MailManagerCli : BaseInteractiveTui<MailFolder, MimeMessage>, IInteractiveTui<MailFolder, MimeMessage>
    {
        internal bool refreshFirstPaneListing = true;
        internal bool refreshSecondPaneListing = true;
        internal MailShell mailShell;
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
                        firstPaneListing = [.. mailShell.MailListDirectories()];
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
                        mailShell.PopulateMessages();
                        int MsgsLimitForPg = ShellsInit.ShellsConfig.MailMaxMessagesInPage;
                        int FirstIndex = MsgsLimitForPg * pageNum - 10;
                        int LastIndex = MsgsLimitForPg * pageNum - 1;
                        var messages = mailShell.IMAP_Messages ?? [];
                        int MaxMessagesIndex = messages.Count() - 1;

                        for (int i = FirstIndex; i <= LastIndex; i++)
                        {
                            if (i <= MaxMessagesIndex)
                            {
                                // Getting information about the message is vital to display them.
                                DebugWriter.WriteDebug(DebugLevel.I, "Getting message {0}...", vars: [i]);
                                lock (mailShell.ImapClient.SyncRoot)
                                {
                                    MimeMessage Msg;
                                    if (!string.IsNullOrEmpty(mailShell.IMAP_CurrentDirectory) & !(mailShell.IMAP_CurrentDirectory == "Inbox"))
                                    {
                                        var Dir = mailShell.OpenFolder(mailShell.IMAP_CurrentDirectory);
                                        Msg = Dir.GetMessage(messages.ElementAtOrDefault(i), default, MailTools.Progress);
                                    }
                                    else
                                        Msg = mailShell.ImapClient.Inbox?.GetMessage(messages.ElementAtOrDefault(i), default, MailTools.Progress) ??
                                            throw new KernelException(KernelExceptionType.Mail, LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_EXCEPTION_OBTAINFAILED"));
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
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to get mail message list for the second pane [{0}]: {1}", vars: [mailShell.IMAP_CurrentDirectory, ex.Message]);
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
                string subject = item.Subject ?? "";
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
                string subject = item.Subject ?? "";
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

        internal void Open(MailFolder? entry1, MimeMessage? entry2, int messageNum)
        {
            try
            {
                // Don't do anything if we haven't been provided anything.
                if (entry1 is null && entry2 is null)
                    return;

                // Determine whether to deal with the message or with the folder
                if (CurrentPane == 2)
                {
                    // We are dealing with the message.
                    var currentEntry = entry2;
                    if (currentEntry is null)
                        return;

                    // Open it in a separate infobox.
                    var messageBuilder = MailTools.MailRenderMessage(mailShell.ImapClient, entry1?.FullName ?? "", messageNum + 1);
                    InfoBoxModalColor.WriteInfoBoxModal(messageBuilder);
                    refreshSecondPaneListing = true;
                }
                else
                {
                    // We are dealing with the folder.
                    var currentEntry = entry1;
                    if (currentEntry is null || !currentEntry.Exists)
                        return;

                    // Open it in the selected pane.
                    mailShell.MailChangeDirectory(currentEntry.FullName);
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
                    mailShell.CreateMailDirectory(directoryName);
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
                    mailShell.MailMoveMessage(messageIdx + 1, firstPaneListing[directoryIdx].Name);
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
                        InfoBoxNonModalColor.WriteInfoBox(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_TUI_MOVINGMESSAGEBYSENDER"), Settings.InfoBoxSettings, address.Name ?? "");
                        mailShell.MailMoveAllBySender(address.Name ?? "", firstPaneListing[directoryIdx].Name);
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
                    mailShell.RenameMailDirectory(folder.Name, directoryName);
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
                    mailShell.DeleteMailDirectory(folder.Name);
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
                    mailShell.MailRemoveMessage(msgIdx + 1);
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
                        InfoBoxNonModalColor.WriteInfoBox(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_TUI_REMOVINGMESSAGEBYSENDER"), Settings.InfoBoxSettings, address.Name ?? "");
                        mailShell.MailRemoveAllBySender(address.Name ?? "");
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

        public MailManagerCli(MailShell mailShell)
        {
            this.mailShell = mailShell;
        }
    }
}
