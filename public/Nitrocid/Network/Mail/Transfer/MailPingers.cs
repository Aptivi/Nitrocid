
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System.Threading;
using KS.Kernel.Debugging;
using KS.Shell.Shells.Mail;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;

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
            while (((ImapClient)MailShellCommon.ClientImap.ConnectionInstance).IsConnected)
            {
                Thread.Sleep(MailShellCommon.Mail_ImapPingInterval);
                if (((ImapClient)MailShellCommon.ClientImap.ConnectionInstance).IsConnected)
                {
                    lock (((ImapClient)MailShellCommon.ClientImap.ConnectionInstance).SyncRoot)
                        ((ImapClient)MailShellCommon.ClientImap.ConnectionInstance).NoOp();
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
            while (((SmtpClient)MailShellCommon.ClientSmtp.ConnectionInstance).IsConnected)
            {
                Thread.Sleep(MailShellCommon.Mail_SmtpPingInterval);
                if (((SmtpClient)MailShellCommon.ClientSmtp.ConnectionInstance).IsConnected)
                {
                    lock (((SmtpClient)MailShellCommon.ClientSmtp.ConnectionInstance).SyncRoot)
                        ((SmtpClient)MailShellCommon.ClientSmtp.ConnectionInstance).NoOp();
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
