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

using Nitrocid.Extras.MailShell.Tools.Directory;
using Nitrocid.Languages;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;

namespace Nitrocid.Extras.MailShell.Mail.Commands
{
    /// <summary>
    /// Renames a directory
    /// </summary>
    /// <remarks>
    /// If you want to change the name of a directory to something else, you can use this command.
    /// </remarks>
    class RenCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "ren";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_COMMAND_REN_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "oldFolderName", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_MAIL_COMMAND_REN_ARGUMENT_OLDFOLDERNAME_DESC"
                    }),
                    new CommandArgumentPart(true, "newFolderName", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_MAIL_COMMAND_REN_ARGUMENT_NEWFOLDERNAME_DESC"
                    })
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            MailDirectory.RenameMailDirectory(parameters.ArgumentsList[0], parameters.ArgumentsList[1]);
            return 0;
        }
    }
}
