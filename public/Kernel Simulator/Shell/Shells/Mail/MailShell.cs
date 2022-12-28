
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
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

using KS.Kernel.Debugging;
using KS.Misc.Threading;
using KS.Network.Mail;
using KS.Network.Mail.Transfer;
using KS.Shell.ShellBase.Shells;

namespace KS.Shell.Shells.Mail
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

        /// <inheritdoc/>
        public override void InitializeShell(params object[] ShellArgs)
        {
            // Send ping to keep the connection alive
            var IMAP_NoOp = new KernelThread("IMAP Keep Connection", false, MailPingers.IMAPKeepConnection);
            IMAP_NoOp.Start();
            DebugWriter.WriteDebug(DebugLevel.I, "Made new thread about IMAPKeepConnection()");
            var SMTP_NoOp = new KernelThread("SMTP Keep Connection", false, MailPingers.SMTPKeepConnection);
            SMTP_NoOp.Start();
            DebugWriter.WriteDebug(DebugLevel.I, "Made new thread about SMTPKeepConnection()");

            while (!Bail)
            {
                // Populate messages
                MailTransfer.PopulateMessages();
                if (MailShellCommon.Mail_NotifyNewMail)
                    MailHandlers.InitializeHandlers();

                // Prompt for the command
                Shell.GetLine();
            }

            // Disconnect the session
            MailShellCommon.IMAP_CurrentDirectory = "Inbox";
            if (MailShellCommon.KeepAlive)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Exit requested, but not disconnecting.");
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Exit requested. Disconnecting host...");
                if (MailShellCommon.Mail_NotifyNewMail)
                    MailHandlers.ReleaseHandlers();
                MailLogin.IMAP_Client.Disconnect(true);
                MailLogin.SMTP_Client.Disconnect(true);
            }
        }

    }
}
