//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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

using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

// using Nitrocid.ShellPacks.Tools;
using Nitrocid.Base.Languages;
// using Nitrocid.Base.Network.Connections;
// using Nitrocid.Base.Network.SpeedDial;
using Terminaux.Shell.Commands;

namespace Nitrocid.ShellPacks.Commands
{
    internal class PopMailCommandExec : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Implement on 0.1.1.
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_POPMAIL_RETURNING"), ThemeColorType.Warning);
            // NetworkConnectionTools.OpenConnectionForShell("MailShell", EstablishMailConnection, (_, connection) =>
            // EstablishMailConnectionSpeedDial(connection), parameters.ArgumentsText);
            return 0;
        }

        // private NetworkConnection EstablishMailConnection(string username) =>
        //     string.IsNullOrEmpty(username) ? MailLogin.PromptUser() : MailLogin.PromptPassword(username);

        // private NetworkConnection EstablishMailConnectionSpeedDial(SpeedDialEntry connection) =>
        //     MailLogin.PromptPassword(connection.Options[0].ToString());

    }
}
