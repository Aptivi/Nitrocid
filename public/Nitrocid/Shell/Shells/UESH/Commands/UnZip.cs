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

using System;
using System.IO;
using System.IO.Compression;
using Nitrocid.Files;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Switches;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Extracts a ZIP file
    /// </summary>
    /// <remarks>
    /// If you wanted to extract the contents of a ZIP file, you can use this command to gain access to the compressed files stored inside it.
    /// <br></br>
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
    class UnZipCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (parameters.ArgumentsList.Length == 1)
            {
                string ZipArchiveName = FilesystemTools.NeutralizePath(parameters.ArgumentsList[0]);
                ZipFile.ExtractToDirectory(ZipArchiveName, FilesystemTools.CurrentDir);
            }
            else if (parameters.ArgumentsList.Length > 1)
            {
                bool createDir = SwitchManager.ContainsSwitch(parameters.SwitchesList, "-createdir");
                string ZipArchiveName = FilesystemTools.NeutralizePath(parameters.ArgumentsList[0]);
                string Destination = createDir ? FilesystemTools.NeutralizePath(parameters.ArgumentsList[1]) : "";
                string target = createDir ? Path.GetFileNameWithoutExtension(ZipArchiveName) : FilesystemTools.NeutralizePath(Path.GetFileNameWithoutExtension(ZipArchiveName));
                if (createDir)
                {
                    Destination = $"{Destination}/{target}";
                    if (Convert.ToString(Destination[0]) == "/")
                        Destination = Destination[1..];
                }
                ZipFile.ExtractToDirectory(ZipArchiveName, Destination);
            }
            return 0;
        }

    }
}
