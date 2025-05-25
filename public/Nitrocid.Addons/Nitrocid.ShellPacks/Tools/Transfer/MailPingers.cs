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
using System.Threading;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using Nitrocid.Kernel.Debugging;
using Nitrocid.ShellPacks.Shells.Mail;

namespace Nitrocid.ShellPacks.Tools.Transfer
{
    static class MailPingers
    {

        /// <summary>
        /// [IMAP] Tries to keep the connection going
        /// </summary>
        public static void IMAPKeepConnection()
        {
            try
            {
                // Every 30 seconds, send a ping to IMAP server
                var client = (ImapClient)((object[]?)MailShellCommon.Client?.ConnectionInstance ?? [])[0];
                while (client.IsConnected)
                {
                    Thread.Sleep(ShellsInit.ShellsConfig.MailImapPingInterval);
                    if (client.IsConnected)
                    {
                        lock (client.SyncRoot)
                            client.NoOp();
                        MailTransfer.PopulateMessages();
                    }
                    else
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "Connection state is inconsistent. Stopping IMAPKeepConnection()...");
                        Thread.CurrentThread.Interrupt();
                    }
                }
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to keep connection to IMAP server alive: {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
            }
        }

        /// <summary>
        /// [SMTP] Tries to keep the connection going
        /// </summary>
        public static void SMTPKeepConnection()
        {
            try
            {
                // Every 30 seconds, send a ping to SMTP server
                var client = (SmtpClient)((object[]?)MailShellCommon.Client?.ConnectionInstance ?? [])[1];
                while (client.IsConnected)
                {
                    Thread.Sleep(ShellsInit.ShellsConfig.MailSmtpPingInterval);
                    if (client.IsConnected)
                    {
                        lock (client.SyncRoot)
                            client.NoOp();
                    }
                    else
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "Connection state is inconsistent. Stopping SMTPKeepConnection()...");
                        Thread.CurrentThread.Interrupt();
                    }
                }
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to keep connection to SMTP server alive: {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
            }
        }

    }
}
