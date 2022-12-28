
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
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

using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using KS.Shell.Shells.Archive.Commands;
using SharpCompress.Archives;
using System.Collections.Generic;
using System.IO;

namespace KS.Shell.Shells.Archive
{
    internal class ArchiveShellCommon
    {
        // Variables
        public readonly static Dictionary<string, CommandInfo> ArchiveShell_Commands = new()
        {
            { "cdir", new CommandInfo("cdir", ShellType.ArchiveShell, "Gets current local directory", new CommandArgumentInfo(), new ArchiveShell_CDirCommand()) },
            { "chdir", new CommandInfo("chdir", ShellType.ArchiveShell, "Changes directory", new CommandArgumentInfo(new[] { "<directory>" }, true, 1), new ArchiveShell_ChDirCommand()) },
            { "chadir", new CommandInfo("chadir", ShellType.ArchiveShell, "Changes archive directory", new CommandArgumentInfo(new[] { "<archivedirectory>" }, true, 1), new ArchiveShell_ChADirCommand()) },
            { "get", new CommandInfo("get", ShellType.ArchiveShell, "Extracts a file to a specified directory or a current directory", new CommandArgumentInfo(new[] { "<entry> [where] [-absolute]" }, true, 1), new ArchiveShell_GetCommand()) },
            { "list", new CommandInfo("list", ShellType.ArchiveShell, "Lists all files inside the archive", new CommandArgumentInfo(new[] { "[directory]" }, false, 0), new ArchiveShell_ListCommand()) },
            { "pack", new CommandInfo("pack", ShellType.ArchiveShell, "Packs a local file to the archive", new CommandArgumentInfo(new[] { "<localfile> [where]" }, true, 1), new ArchiveShell_PackCommand()) }
        };
        internal readonly static Dictionary<string, CommandInfo> ArchiveShell_ModCommands = new();
        public static FileStream ArchiveShell_FileStream;
        public static IArchive ArchiveShell_Archive;
        public static string ArchiveShell_CurrentDirectory;
        public static string ArchiveShell_CurrentArchiveDirectory;
    }
}
