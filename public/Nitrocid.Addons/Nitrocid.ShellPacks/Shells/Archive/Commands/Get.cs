//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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

using Nitrocid.Files;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Switches;
using Nitrocid.ShellPacks.Tools;

namespace Nitrocid.ShellPacks.Shells.Archive.Commands
{
    /// <summary>
    /// Extract a file from a ZIP archive
    /// </summary>
    /// <remarks>
    /// If you want to get a single file from the ZIP archive, you can use this command to extract such file to the current working directory, or a specified directory.
    /// <br></br>
    /// <list type="table">
    /// <listheader>
    /// <term>Switches</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>-absolute</term>
    /// <description>Uses the full target path</description>
    /// </item>
    /// </list>
    /// <br></br>
    /// </remarks>
    class GetCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string Where = "";
            var Absolute = false;
            if (parameters.ArgumentsList.Length > 1)
            {
                if (SwitchManager.ContainsSwitch(parameters.SwitchesList, "-absolute"))
                    Absolute = true;
                else
                    Where = FilesystemTools.NeutralizePath(parameters.ArgumentsList[1]);
            }
            ArchiveTools.ExtractFileEntry(parameters.ArgumentsList[0], Where, Absolute);
            return 0;
        }

    }
}
