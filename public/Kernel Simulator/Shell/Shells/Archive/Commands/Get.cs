
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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

using System;
using System.Linq;
using KS.ConsoleBase.Colors;
using KS.Files;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
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

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            string Where = "";
            var Absolute = default(bool);
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
        }

        public override void HelpHelper()
        {
            TextWriterColor.Write(Translate.DoTranslation("This command has the below switches that change how it works:"));
            TextWriterColor.Write("  -absolute: ", false, KernelColorType.ListEntry);
            TextWriterColor.Write(Translate.DoTranslation("Indicates that the target path is absolute"), true, KernelColorType.ListValue);
        }

    }
}
