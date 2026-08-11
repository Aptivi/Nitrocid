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
using System.Threading;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Shells;

namespace Nitrocid.ShellPacks.Shells.Mail
{
    public partial class MailShell : BaseShell, IShell
    {

        private void IMAPKeepConnection()
        {
            try
            {
                // Every 30 seconds, send a ping to IMAP server
                // TODO: NKS_SHELLPACKS_MAIL_EXCEPTION_IMAPNOTCONNECTED -> IMAP server is not connected
                if (ImapClient is null)
                    throw new KernelException(KernelExceptionType.Mail, LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_EXCEPTION_IMAPNOTCONNECTED"));
                while (ImapClient.IsConnected)
                {
                    Thread.Sleep(ShellsInit.ShellsConfig.MailImapPingInterval);
                    if (ImapClient.IsConnected)
                    {
                        lock (ImapClient.SyncRoot)
                            ImapClient.NoOp();
                        PopulateMessages();
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

        private void POP3KeepConnection()
        {
            try
            {
                // Every 30 seconds, send a ping to POP3 server
                // TODO: NKS_SHELLPACKS_MAIL_EXCEPTION_POP3NOTCONNECTED -> POP3 server is not connected
                if (Pop3Client is null)
                    throw new KernelException(KernelExceptionType.Mail, LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_EXCEPTION_POP3NOTCONNECTED"));
                while (Pop3Client.IsConnected)
                {
                    Thread.Sleep(ShellsInit.ShellsConfig.MailPop3PingInterval);
                    if (Pop3Client.IsConnected)
                    {
                        lock (Pop3Client.SyncRoot)
                            Pop3Client.NoOp();
                        PopulateMessages();
                    }
                    else
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "Connection state is inconsistent. Stopping POP3KeepConnection()...");
                        Thread.CurrentThread.Interrupt();
                    }
                }
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to keep connection to POP3 server alive: {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
            }
        }

        private void SMTPKeepConnection()
        {
            try
            {
                // Every 30 seconds, send a ping to SMTP server
                // TODO: NKS_SHELLPACKS_MAIL_EXCEPTION_SMTPNOTCONNECTED -> SMTP server is not connected
                if (SmtpClient is null)
                    throw new KernelException(KernelExceptionType.Mail, LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_EXCEPTION_SMTPNOTCONNECTED"));
                while (SmtpClient.IsConnected)
                {
                    Thread.Sleep(ShellsInit.ShellsConfig.MailSmtpPingInterval);
                    if (SmtpClient.IsConnected)
                    {
                        lock (SmtpClient.SyncRoot)
                            SmtpClient.NoOp();
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
