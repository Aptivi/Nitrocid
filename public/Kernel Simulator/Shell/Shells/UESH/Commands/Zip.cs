
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
using System.IO.Compression;
using System.Linq;
using KS.ConsoleBase.Colors;
using KS.Files;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Makes a ZIP file
    /// </summary>
    /// <remarks>
    /// If you wanted to make a ZIP file containing the contents you want to compress, you can use this command.
    /// <br></br>
    /// <list type="table">
    /// <listheader>
    /// <term>Switches</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>-fast</term>
    /// <description>Uses fast compression</description>
    /// </item>
    /// <item>
    /// <term>-nocomp</term>
    /// <description>No compression</description>
    /// </item>
    /// <item>
    /// <term>-nobasedir</term>
    /// <description>Don't create base directory on the archive</description>
    /// </item>
    /// </list>
    /// <br></br>
    /// </remarks>
    class ZipCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            string ZipArchiveName = Filesystem.NeutralizePath(ListArgsOnly[0]);
            string Destination = Filesystem.NeutralizePath(ListArgsOnly[1]);
            var ZipCompression = CompressionLevel.Optimal;
            bool ZipBaseDir = true;
            if (ListSwitchesOnly.Contains("-fast"))
            {
                ZipCompression = CompressionLevel.Fastest;
            }
            else if (ListSwitchesOnly.Contains("-nocomp"))
            {
                ZipCompression = CompressionLevel.NoCompression;
            }
            if (ListSwitchesOnly.Contains("-nobasedir"))
            {
                ZipBaseDir = false;
            }
            ZipFile.CreateFromDirectory(Destination, ZipArchiveName, ZipCompression, ZipBaseDir);
        }

        public override void HelpHelper()
        {
            TextWriterColor.Write(Translate.DoTranslation("This command has the below switches that change how it works:"));
            TextWriterColor.Write("  -fast: ", false, KernelColorType.ListEntry);
            TextWriterColor.Write(Translate.DoTranslation("Fast compression"), true, KernelColorType.ListValue);
            TextWriterColor.Write("  -nocomp: ", false, KernelColorType.ListEntry);
            TextWriterColor.Write(Translate.DoTranslation("No compression"), true, KernelColorType.ListValue);
            TextWriterColor.Write("  -nobasedir: ", false, KernelColorType.ListEntry);
            TextWriterColor.Write(Translate.DoTranslation("Don't create base directory in archive"), true, KernelColorType.ListValue);
        }

    }
}