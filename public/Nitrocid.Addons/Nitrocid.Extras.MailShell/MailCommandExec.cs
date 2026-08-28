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

using Nitrocid.Extras.MailShell.Tools;
using Nitrocid.Languages;
using Nitrocid.Network.Connections;
using Nitrocid.Network.SpeedDial;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;

namespace Nitrocid.Extras.MailShell
{
    internal class MailCommandExec : BaseCommand, ICommand
    {
        public override string Command =>
            "mail";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_COMMAND_MAIL_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(false, "emailAddress", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_COMMON_COMMAND_MAIL_ARGUMENT_ADDRESS_DESC"
                    }),
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            NetworkConnectionTools.OpenConnectionForShell("MailShell", EstablishMailConnection, (_, connection) =>
                EstablishMailConnectionSpeedDial(connection), parameters.ArgumentsText);
            return 0;
        }

        private NetworkConnection? EstablishMailConnection(string username) =>
            string.IsNullOrEmpty(username) ? MailLogin.PromptUser() : MailLogin.PromptPassword(username);

        private NetworkConnection? EstablishMailConnectionSpeedDial(SpeedDialEntry connection) =>
            MailLogin.PromptPassword(connection.Options[0]?.ToString() ?? "");

    }
}
