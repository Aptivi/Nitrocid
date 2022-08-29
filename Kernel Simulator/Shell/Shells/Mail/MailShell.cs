
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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

using System;
using Extensification.StringExts;
using KS.ConsoleBase.Inputs;
using KS.Kernel.Debugging;
using KS.Misc.Threading;
using KS.Network.Mail;
using KS.Network.Mail.Transfer;
using KS.Shell.Prompts;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;

namespace KS.Shell.Shells.Mail
{
    public class MailShell : ShellExecutor, IShell
    {

        public override ShellType ShellType
        {
            get
            {
                return ShellType.MailShell;
            }
        }

        public override bool Bail { get; set; }

        public override void InitializeShell(params object[] ShellArgs)
        {
            // Send ping to keep the connection alive
            var IMAP_NoOp = new KernelThread("IMAP Keep Connection", false, MailPingers.IMAPKeepConnection);
            IMAP_NoOp.Start();
            DebugWriter.WriteDebug(DebugLevel.I, "Made new thread about IMAPKeepConnection()");
            var SMTP_NoOp = new KernelThread("SMTP Keep Connection", false, MailPingers.SMTPKeepConnection);
            SMTP_NoOp.Start();
            DebugWriter.WriteDebug(DebugLevel.I, "Made new thread about SMTPKeepConnection()");
            Kernel.Kernel.KernelEventManager.RaiseIMAPShellInitialized();

            while (!Bail)
            {
                // Populate messages
                MailTransfer.PopulateMessages();
                if (MailShellCommon.Mail_NotifyNewMail)
                    MailHandlers.InitializeHandlers();

                // See UESHShell.cs for more info
                lock (CancellationHandlers.GetCancelSyncLock(ShellType))
                {
                    // Initialize prompt
                    PromptPresetManager.WriteShellPrompt(ShellType);
                }

                // Listen for a command
                string cmd = Input.ReadLine();
                if ((string.IsNullOrEmpty(cmd) | (cmd?.StartsWithAnyOf(new[] { " ", "#" }))) == false)
                {
                    Kernel.Kernel.KernelEventManager.RaiseIMAPPreExecuteCommand(cmd);
                    Shell.GetLine(cmd, "", ShellType.MailShell);
                    Kernel.Kernel.KernelEventManager.RaiseIMAPPostExecuteCommand(cmd);
                }
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
