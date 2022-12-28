
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

using KS.Network.Mail;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using KS.Shell.Shells.Mail;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Opens the mail shell
    /// </summary>
    /// <remarks>
    /// This command is an entry point to the mail shell that lets you view and list messages.
    /// <br></br>
    /// If no address is specified, it will prompt you for the address, password, and the mail server (IMAP) if the address is not found in the ISP database. Currently, it connects with necessary requirements to ensure successful connection.
    /// </remarks>
    class MailCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (MailShellCommon.KeepAlive)
            {
                ShellStart.StartShell(ShellType.MailShell);
            }
            else if (ListArgsOnly.Length == 0)
            {
                MailLogin.PromptUser();
            }
            else if (!string.IsNullOrEmpty(ListArgsOnly[0]))
            {
                MailLogin.PromptPassword(ListArgsOnly[0]);
            }
        }

    }
}