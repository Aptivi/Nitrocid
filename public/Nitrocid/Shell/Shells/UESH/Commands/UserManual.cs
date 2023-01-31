
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
using System.Diagnostics;
using System.Linq;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Opens the web browser to this wiki or to the KS API for mods.
    /// </summary>
    /// <remarks>
    /// <list type="table">
    /// <listheader>
    /// <term>Switches</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>-createdir</term>
    /// <description>Extracts the archive to the new directory that has the same name as the archive</description>
    /// </item>
    /// </list>
    /// <br></br>
    /// </remarks>
    class UserManualCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            bool ModDocumentation = ListSwitchesOnly.Contains("-modapi");
            if (ModDocumentation)
            {
                Process.Start("https://aptivi.github.io/NitrocidKS");
            }
            else
            {
                Process.Start("https://aptivi.gitbook.io/kernel-simulator-manual/");
            }
        }

        public override void HelpHelper()
        {
            TextWriterColor.Write(Translate.DoTranslation("This command has the below switches that change how it works:"));
            TextWriterColor.Write("  -modapi: ", false, KernelColorType.ListEntry);
            TextWriterColor.Write(Translate.DoTranslation("Opens the mod API documentation for the structure of the source code in its most current form"), true, KernelColorType.ListValue);
        }

    }
}
