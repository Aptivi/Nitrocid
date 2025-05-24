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

using System.Collections.Generic;
using SharpCompress.Archives;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Kernel.Debugging;
using Nitrocid.ConsoleBase.Writers;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Misc.Reflection;
using Nitrocid.ShellPacks.Tools;

namespace Nitrocid.ShellPacks.Shells.Archive.Commands
{
    /// <summary>
    /// Lists ZIP file entries
    /// </summary>
    /// <remarks>
    /// If you want to know what this ZIP file contains, you can use this command to list all the files and folders included in the archive.
    /// </remarks>
    class ListCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            List<IArchiveEntry> Entries;
            if (parameters.ArgumentsList.Length > 0)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Listing entries with {0} as target directory", vars: [parameters.ArgumentsList[0]]);
                Entries = ArchiveTools.ListArchiveEntries(parameters.ArgumentsList[0]);
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Listing entries with current directory as target directory");
                Entries = ArchiveTools.ListArchiveEntries(ArchiveShellCommon.CurrentArchiveDirectory ?? "");
            }
            foreach (IArchiveEntry Entry in Entries)
            {
                TextWriters.Write("- {0}: ", false, KernelColorType.ListEntry, Entry.Key ?? "");
                if (!Entry.IsDirectory) // Entry is a file
                {
                    TextWriters.Write("{0} ({1})", true, KernelColorType.ListValue, Entry.CompressedSize.SizeString(), Entry.Size.SizeString());
                }
                else
                {
                    TextWriterRaw.Write();
                }
            }
            return 0;
        }

    }
}
