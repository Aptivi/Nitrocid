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

using Terminaux.Shell.Commands;
using Terminaux.Shell.Arguments.Base.Help;
using Nitrocid.Base.Arguments;

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

        public override int Execute(CommandParameters parameters, ref string variableValue)
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
