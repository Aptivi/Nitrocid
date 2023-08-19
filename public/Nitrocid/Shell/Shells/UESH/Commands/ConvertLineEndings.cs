
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

using KS.Files.LineEndings;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Converts the line endings
    /// </summary>
    /// <remarks>
    /// If you have a text file that needs a change for its line endings, you can use this command to convert the line endings to your platform's format, or the format of your choice by using these switches:
    /// <br></br>
    /// <list type="table">
    /// <listheader>
    /// <term>Switches</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>-w</term>
    /// <description>Converts the line endings to the Windows format (CR + LF)</description>
    /// </item>
    /// <item>
    /// <term>-u</term>
    /// <description>Converts the line endings to the Unix format (LF)</description>
    /// </item>
    /// <item>
    /// <term>-m</term>
    /// <description>Converts the line endings to the Mac OS 9 format (CR)</description>
    /// </item>
    /// </list>
    /// <br></br>
    /// </remarks>
    class ConvertLineEndingsCommand : BaseCommand, ICommand
    {

        public override int Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly, ref string variableValue)
        {
            string TargetTextFile = ListArgsOnly[0];
            var TargetLineEnding = LineEndingsTools.NewlineStyle;
            if (!(ListSwitchesOnly.Length == 0))
            {
                if (ListSwitchesOnly[0] == "-w")
                    TargetLineEnding = FilesystemNewlineStyle.CRLF;
                if (ListSwitchesOnly[0] == "-u")
                    TargetLineEnding = FilesystemNewlineStyle.LF;
                if (ListSwitchesOnly[0] == "-m")
                    TargetLineEnding = FilesystemNewlineStyle.CR;
            }

            // Convert the line endings
            LineEndingsConverter.ConvertLineEndings(TargetTextFile, TargetLineEnding);
            return 0;
        }

    }
}
