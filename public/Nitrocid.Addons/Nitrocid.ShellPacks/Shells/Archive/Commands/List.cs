//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System.Collections.Generic;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Reflection;
using SharpCompress.Archives;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

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

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            var archiveShell = (ArchiveShell?)shell ??
                throw new KernelException(KernelExceptionType.Archive, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_LASTSHELLTYPEMISMATCH"));
            List<IArchiveEntry> Entries;
            if (parameters.ArgumentsList.Length > 0)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Listing entries with {0} as target directory", vars: [parameters.ArgumentsList[0]]);
                Entries = archiveShell.ListArchiveEntries(parameters.ArgumentsList[0]);
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Listing entries with current directory as target directory");
                Entries = archiveShell.ListArchiveEntries(archiveShell.CurrentArchiveDirectory ?? "");
            }
            foreach (IArchiveEntry Entry in Entries)
            {
                TextWriterColor.Write("- {0}: ", false, ThemeColorType.ListEntry, Entry.Key ?? "");
                if (!Entry.IsDirectory) // Entry is a file
                    TextWriterColor.Write("{0} ({1})", true, ThemeColorType.ListValue, Entry.CompressedSize.SizeString(), Entry.Size.SizeString());
                else
                    TextWriterRaw.Write();
            }
            return 0;
        }

    }
}
