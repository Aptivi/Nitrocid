
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

using System;
using System.Linq;
using KS.Files;
using KS.Misc.Archive;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.Archive.Commands
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
    class ArchiveShell_GetCommand : BaseCommand, ICommand
    {

        public override int Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly, ref string variableValue)
        {
            string Where = "";
            var Absolute = false;
            if (ListArgsOnly.Length > 1)
            {
                if (!(ListSwitchesOnly[0] == "-absolute"))
                    Where = Filesystem.NeutralizePath(ListArgsOnly[1]);
                if (ListSwitchesOnly.Contains("-absolute"))
                {
                    Absolute = true;
                }
            }
            ArchiveTools.ExtractFileEntry(ListArgsOnly[0], Where, Absolute);
            return 0;
        }

    }
}
