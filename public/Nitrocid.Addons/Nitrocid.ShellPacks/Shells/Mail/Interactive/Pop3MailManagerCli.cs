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
    internal class Pop3MailManagerCli : BaseInteractiveTui<MimeMessage>, IInteractiveTui<MimeMessage>
    {
        internal bool refreshFirstPaneListing = true;
        internal MailShell mailShell;
        private List<MimeMessage> firstPaneListing = [];
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
        public override IEnumerable<MimeMessage> PrimaryDataSource
        {
            get
            {
                try
                {
                    if (refreshFirstPaneListing)
                    {
                        refreshFirstPaneListing = false;
                        firstPaneListing.Clear();
                        mailShell.PopulateMessages();
                        int MsgsLimitForPg = ShellsInit.ShellsConfig.MailMaxMessagesInPage;
                        int FirstIndex = MsgsLimitForPg * pageNum - 10;
                        int LastIndex = MsgsLimitForPg * pageNum - 1;
                        var messages = mailShell.POP3_Messages ?? [];
                        int MaxMessagesIndex = messages.Count() - 1;

                        for (int i = FirstIndex; i <= LastIndex; i++)
                        {
                            if (i <= MaxMessagesIndex)
                            {
                                // Getting information about the message is vital to display them.
                                DebugWriter.WriteDebug(DebugLevel.I, "Getting message {0}...", vars: [i]);
                                // TODO: NKS_SHELLPACKS_MAIL_EXCEPTION_POP3NOTCONNECTED -> POP3 server is not connected
                                if (mailShell.Pop3Client is null)
                                    throw new KernelException(KernelExceptionType.Mail, LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_EXCEPTION_POP3NOTCONNECTED"));
                                lock (mailShell.Pop3Client.SyncRoot)
                                {
                                    MimeMessage Msg = mailShell.Pop3Client.GetMessage(i, default, MailTools.Progress) ??
                                        throw new KernelException(KernelExceptionType.Mail, LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_EXCEPTION_OBTAINFAILED"));
                                    firstPaneListing.Add(Msg);
                                }
                            }
                            else
                                DebugWriter.WriteDebug(DebugLevel.W, "Reached max message limit. Message number {0}", vars: [i]);
                        }
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
        public override bool AcceptsEmptyData =>
            true;

        /// <inheritdoc/>
        public override string GetStatusFromItem(MimeMessage item)
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
        public override string GetEntryFromItem(MimeMessage item)
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

        internal void Open(MimeMessage? entry1, int messageNum)
        {
            try
            {
                // We are dealing with the message.
                var currentEntry = entry1;
                if (currentEntry is null)
                    return;

                // Open it in a separate infobox.
                var messageBuilder = MailTools.MailRenderMessage(null, mailShell.Pop3Client, "", messageNum + 1);
                InfoBoxModalColor.WriteInfoBoxModal(messageBuilder);
                refreshFirstPaneListing = true;
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_TUI_FOLDERMESSAGEOPENFAILED") + ": {0}".FormatString(ex.Message));
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
            }
        }

        internal void RemoveMessage(int msgIdx)
        {
            try
            {
                InfoBoxNonModalColor.WriteInfoBox(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_TUI_REMOVINGMESSAGE"), Settings.InfoBoxSettings);
                mailShell.MailRemoveMessage(msgIdx + 1);
                InteractiveTuiTools.SelectionMovement(this, SecondPaneCurrentSelection - 1);
                refreshFirstPaneListing = true;
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
                var addresses = firstPaneListing[msgIdx].From;
                foreach (var address in addresses)
                {
                    InfoBoxNonModalColor.WriteInfoBox(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_TUI_REMOVINGMESSAGEBYSENDER"), Settings.InfoBoxSettings, address.Name ?? "");
                    mailShell.MailRemoveAllBySender(address.Name ?? "");
                }
                InteractiveTuiTools.SelectionMovement(this, SecondPaneCurrentSelection - 1);
                refreshFirstPaneListing = true;
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_TUI_REMOVINGMESSAGEBYSENDERFAILED") + ": {0}".FormatString(ex.Message));
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
            }
        }

        public Pop3MailManagerCli(MailShell mailShell)
        {
            this.mailShell = mailShell;
        }
    }
}
