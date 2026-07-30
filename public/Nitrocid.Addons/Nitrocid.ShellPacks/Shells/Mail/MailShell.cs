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
using System.Net;
using System.Threading;
using FluentFTP;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Network.Connections;
using Terminaux.Inputs;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Threadify.Manager;

namespace Nitrocid.ShellPacks.Shells.Mail
{
    /// <summary>
    /// The mail shell
    /// </summary>
    public partial class MailShell : BaseShell, IShell
    {
        internal IEnumerable<UniqueId>? IMAP_Messages;
        internal NetworkInstanceConnection<object[]>? Client;

        /// <summary>
        /// IMAP current directory name
        /// </summary>
        public string IMAP_CurrentDirectory { get; set; } = "Inbox";

        /// <summary>
        /// IMAP client
        /// </summary>
        public ImapClient ImapClient =>
            (ImapClient)((Client?.ConnectionInstance) ?? [])[0];

        /// <summary>
        /// SMTP client
        /// </summary>
        public SmtpClient SmtpClient =>
            (SmtpClient)((Client?.ConnectionInstance) ?? [])[1];

        /// <summary>
        /// Network credentials
        /// </summary>
        public NetworkCredential NetworkCredential =>
            (NetworkCredential)((Client?.ConnectionInstance) ?? [])[3];

        /// <inheritdoc/>
        public override string ShellType => "MailShell";

        /// <inheritdoc/>
        public override bool Bail { get; set; }

        internal bool detaching = false;

        /// <inheritdoc/>
        public override void InitializeShell(params object[] ShellArgs)
        {
            // Parse shell arguments
            var connection = (NetworkInstanceConnection<object[]>)ShellArgs[0];
            Client = connection;

            // Send ping to keep the connection alive
            var IMAP_NoOp = new ThreadInstance("IMAP Keep Connection", false, IMAPKeepConnection);
            IMAP_NoOp.Start();
            DebugWriter.WriteDebug(DebugLevel.I, "Made new thread about IMAPKeepConnection()");
            var SMTP_NoOp = new ThreadInstance("SMTP Keep Connection", false, SMTPKeepConnection);
            SMTP_NoOp.Start();
            DebugWriter.WriteDebug(DebugLevel.I, "Made new thread about SMTPKeepConnection()");

            while (!Bail)
            {
                try
                {
                    // Populate messages
                    PopulateMessages();
                    if (ShellsInit.ShellsConfig.MailNotifyNewMail)
                        InitializeHandlers();

                    // Prompt for the command
                    ShellManager.GetLine();
                }
                catch (ThreadInterruptedException)
                {
                    CancellationHandlers.DismissRequest();
                    Bail = true;
                }
                catch (Exception ex)
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_SHELL_ERROR") + " {0}", true, ThemeColorType.Error, ex.Message);
                    DebugWriter.WriteDebug(DebugLevel.E, "Shell will have to exit: {0}", vars: [ex.Message]);
                    DebugWriter.WriteDebugStackTrace(ex);
                    Input.ReadKey();
                    Bail = true;
                }

                // Exiting, so reset the site
                if (Bail)
                {
                    IMAP_CurrentDirectory = "Inbox";
                    if (!detaching)
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "Exit requested. Disconnecting host...");
                        if (ShellsInit.ShellsConfig.MailNotifyNewMail)
                            ReleaseHandlers();
                        IMAP_NoOp.Stop();
                        SMTP_NoOp.Stop();
                        ImapClient.Disconnect(true);
                        SmtpClient.Disconnect(true);
                        int connectionIndex = NetworkConnectionTools.GetConnectionIndex(Client);
                        NetworkConnectionTools.CloseConnection(connectionIndex);
                        Client = null;
                    }
                    detaching = false;
                }
            }
        }

    }
}
