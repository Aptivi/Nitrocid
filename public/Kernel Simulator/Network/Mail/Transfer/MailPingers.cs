
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

using System.Threading;
using KS.Kernel.Debugging;
using KS.Shell.Shells.Mail;

namespace KS.Network.Mail.Transfer
{
    static class MailPingers
    {

        /// <summary>
        /// [IMAP] Tries to keep the connection going
        /// </summary>
        public static void IMAPKeepConnection()
        {
            // Every 30 seconds, send a ping to IMAP server
            while (MailLogin.IMAP_Client.IsConnected)
            {
                Thread.Sleep(MailShellCommon.Mail_ImapPingInterval);
                if (MailLogin.IMAP_Client.IsConnected)
                {
                    lock (MailLogin.IMAP_Client.SyncRoot)
                        MailLogin.IMAP_Client.NoOp();
                    MailTransfer.PopulateMessages();
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Connection state is inconsistent. Stopping IMAPKeepConnection()...");
                    Thread.CurrentThread.Interrupt();
                }
            }
        }

        /// <summary>
        /// [SMTP] Tries to keep the connection going
        /// </summary>
        public static void SMTPKeepConnection()
        {
            // Every 30 seconds, send a ping to SMTP server
            while (MailLogin.SMTP_Client.IsConnected)
            {
                Thread.Sleep(MailShellCommon.Mail_SmtpPingInterval);
                if (MailLogin.SMTP_Client.IsConnected)
                {
                    lock (MailLogin.SMTP_Client.SyncRoot)
                        MailLogin.SMTP_Client.NoOp();
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Connection state is inconsistent. Stopping SMTPKeepConnection()...");
                    Thread.CurrentThread.Interrupt();
                }
            }
        }

    }
}