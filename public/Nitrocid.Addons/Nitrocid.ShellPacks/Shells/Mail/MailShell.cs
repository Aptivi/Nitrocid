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

using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using System.Threading;
using System;
using Nitrocid.ShellPacks.Tools;
using Nitrocid.ShellPacks.Tools.Transfer;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Kernel.Threading;
using Nitrocid.Base.Network.SpeedDial;
using Nitrocid.Base.Network.Connections;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Colors.Themes.Colors;
using Nitrocid.Base.ConsoleBase.Inputs;

namespace Nitrocid.ShellPacks.Shells.Mail
{
    /// <summary>
    /// The mail shell
    /// </summary>
    public class MailShell : BaseShell, IShell
    {

        /// <inheritdoc/>
        public override string ShellType => "MailShell";

        /// <inheritdoc/>
        public override bool Bail { get; set; }

        internal bool detaching = false;

        /// <inheritdoc/>
        public override void InitializeShell(params object[] ShellArgs)
        {
            // Parse shell arguments
            NetworkConnection connection = (NetworkConnection)ShellArgs[0];
            ImapClient imapLink = (ImapClient)((object[]?)MailShellCommon.Client?.ConnectionInstance ?? [])[0];
            SmtpClient smtpLink = (SmtpClient)((object[]?)MailShellCommon.Client?.ConnectionInstance ?? [])[1];
            MailShellCommon.Client = connection;

            // Send ping to keep the connection alive
            var IMAP_NoOp = new KernelThread("IMAP Keep Connection", false, MailPingers.IMAPKeepConnection);
            IMAP_NoOp.Start();
            DebugWriter.WriteDebug(DebugLevel.I, "Made new thread about IMAPKeepConnection()");
            var SMTP_NoOp = new KernelThread("SMTP Keep Connection", false, MailPingers.SMTPKeepConnection);
            SMTP_NoOp.Start();
            DebugWriter.WriteDebug(DebugLevel.I, "Made new thread about SMTPKeepConnection()");

            // Write connection information to Speed Dial file if it doesn't exist there
            SpeedDialTools.TryAddEntryToSpeedDial(connection.ConnectionUri.AbsoluteUri, connection.ConnectionUri.Port, NetworkConnectionType.Mail, false, MailLogin.Authentication.UserName);

            while (!Bail)
            {
                try
                {
                    // Populate messages
                    MailTransfer.PopulateMessages();
                    if (ShellsInit.ShellsConfig.MailNotifyNewMail)
                        MailHandlers.InitializeHandlers();

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
                    InputTools.DetectKeypress();
                    Bail = true;
                }

                // Exiting, so reset the site
                if (Bail)
                {
                    MailShellCommon.IMAP_CurrentDirectory = "Inbox";
                    if (!detaching)
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "Exit requested. Disconnecting host...");
                        if (ShellsInit.ShellsConfig.MailNotifyNewMail)
                            MailHandlers.ReleaseHandlers();
                        IMAP_NoOp.Stop();
                        SMTP_NoOp.Stop();
                        imapLink.Disconnect(true);
                        smtpLink.Disconnect(true);
                        int connectionIndex = NetworkConnectionTools.GetConnectionIndex(MailShellCommon.Client);
                        NetworkConnectionTools.CloseConnection(connectionIndex);
                        MailShellCommon.Client = null;
                    }
                    detaching = false;
                }
            }
        }

    }
}
