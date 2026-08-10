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

using Nitrocid.Base.Arguments;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Arguments.Base.Help;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;

namespace Nitrocid.Base.Shell.Shells.Admin.Commands
{
    /// <summary>
    /// Opens the help page
    /// </summary>
    /// <remarks>
    /// This command allows you to get help for any specific command, including its usage. If no command is specified, all commands are listed.
    /// </remarks>
    class ArgHelpCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "arghelp";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_ADMIN_COMMAND_ARGHELP_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(false, "argument", new CommandArgumentPartOptions()
                    {
                        AutoCompleter = (_) => [.. KernelArguments.AvailableCMDLineArgs.Keys],
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_ADMIN_COMMAND_ARGHELP_ARGUMENT_ARGUMENT_DESC"
                    })
                ])
            ];

        public override CommandFlags Flags =>
            CommandFlags.Wrappable | CommandFlags.RedirectionSupported;

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            // Now, show the help
            if (string.IsNullOrWhiteSpace(parameters.ArgumentsText))
                ArgumentHelpPrint.ShowArgsHelp(KernelArguments.AvailableCMDLineArgs);
            else
                ArgumentHelpPrint.ShowArgsHelp(parameters.ArgumentsList[0], KernelArguments.AvailableCMDLineArgs);
            return 0;
        }
    }
}
