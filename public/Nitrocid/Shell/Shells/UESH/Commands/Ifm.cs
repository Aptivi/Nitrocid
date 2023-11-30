//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.ConsoleBase.Interactive;
using KS.Files;
using KS.Files.Paths;
using KS.Misc.Interactives;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Interactive system host file manager
    /// </summary>
    /// <remarks>
    /// If you are planning to take a look at your filesystem in an interactive manner, use this command.
    /// </remarks>
    class IfmCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var tui = new FileManagerCli
            {
                firstPanePath = parameters.ArgumentsList.Length > 0 ? FilesystemTools.NeutralizePath(parameters.ArgumentsList[0]) : PathsManagement.HomePath,
                secondPanePath = parameters.ArgumentsList.Length > 1 ? FilesystemTools.NeutralizePath(parameters.ArgumentsList[1]) : PathsManagement.HomePath
            };
            InteractiveTuiTools.OpenInteractiveTui(tui);
            return 0;
        }
    }
}
