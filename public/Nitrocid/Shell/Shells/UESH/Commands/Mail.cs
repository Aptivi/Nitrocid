
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

using KS.Network.Base.Connections;
using KS.Network.Mail;
using KS.Network.SpeedDial;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using Newtonsoft.Json.Linq;

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

        public override int Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly, ref string variableValue)
        {
            NetworkConnectionTools.OpenConnectionForShell(ShellType.MailShell, EstablishMailConnection, (_, connection) =>
            EstablishMailConnectionSpeedDial(connection), StringArgs);
            return 0;
        }

        private NetworkConnection EstablishMailConnection(string username) =>
            string.IsNullOrEmpty(username) ? MailLogin.PromptUser() : MailLogin.PromptPassword(username);

        private NetworkConnection EstablishMailConnectionSpeedDial(SpeedDialEntry connection) =>
            MailLogin.PromptPassword(connection.Options[0].ToString());

    }
}
