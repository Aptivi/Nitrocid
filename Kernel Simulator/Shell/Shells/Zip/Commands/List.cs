
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
using System.Collections.Generic;
using FluentFTP.Helpers;
using KS.ConsoleBase.Colors;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Misc.ZipFile;
using KS.Shell.ShellBase.Commands;
using SharpCompress.Archives.Zip;

namespace KS.Shell.Shells.Zip.Commands
{
    /// <summary>
    /// Lists ZIP file entries
    /// </summary>
    /// <remarks>
    /// If you want to know what this ZIP file contains, you can use this command to list all the files and folders included in the archive.
    /// </remarks>
    class ZipShell_ListCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            List<ZipArchiveEntry> Entries;
            if (ListArgsOnly.Length > 0)
            {
                DebugWriter.Wdbg(DebugLevel.I, "Listing entries with {0} as target directory", ListArgsOnly[0]);
                Entries = ZipTools.ListZipEntries(ListArgsOnly[0]);
            }
            else
            {
                DebugWriter.Wdbg(DebugLevel.I, "Listing entries with current directory as target directory");
                Entries = ZipTools.ListZipEntries(ZipShellCommon.ZipShell_CurrentArchiveDirectory);
            }
            foreach (ZipArchiveEntry Entry in Entries)
            {
                TextWriterColor.Write("- {0}: ", false, ColorTools.ColTypes.ListEntry, Entry.Key);
                if (!Entry.IsDirectory) // Entry is a file
                {
                    TextWriterColor.Write("{0} ({1})", true, ColorTools.ColTypes.ListValue, Entry.CompressedSize.FileSizeToString(), Entry.Size.FileSizeToString());
                }
                else
                {
                    ConsoleBase.ConsoleWrapper.WriteLine();
                }
            }
        }

    }
}