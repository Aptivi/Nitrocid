
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

using KS.Network.Mail.Directory;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.Mail.Commands
{
    /// <summary>
    /// Makes a mail directory
    /// </summary>
    /// <remarks>
    /// If you want an additional mail folder to organize your messages there, you can use this command. Works with the combination of the mv or mvall command.
    /// </remarks>
    class Mail_MkdirCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly) => MailDirectory.CreateMailDirectory(ListArgsOnly[0]);

    }
}