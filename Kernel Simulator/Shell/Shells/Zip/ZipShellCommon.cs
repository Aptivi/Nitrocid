
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

using System.Collections.Generic;
using System.IO;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using KS.Shell.Shells.Zip.Commands;
using SharpCompress.Archives.Zip;

namespace KS.Shell.Shells.Zip
{
    static class ZipShellCommon
    {

        // Variables
        public readonly static Dictionary<string, CommandInfo> ZipShell_Commands = new()
        {
            { "cdir", new CommandInfo("cdir", ShellType.ZIPShell, "Gets current local directory", new CommandArgumentInfo(), new ZipShell_CDirCommand()) },
            { "chdir", new CommandInfo("chdir", ShellType.ZIPShell, "Changes directory", new CommandArgumentInfo(new[] { "<directory>" }, true, 1), new ZipShell_ChDirCommand()) },
            { "chadir", new CommandInfo("chadir", ShellType.ZIPShell, "Changes archive directory", new CommandArgumentInfo(new[] { "<archivedirectory>" }, true, 1), new ZipShell_ChADirCommand()) },
            { "get", new CommandInfo("get", ShellType.ZIPShell, "Extracts a file to a specified directory or a current directory", new CommandArgumentInfo(new[] { "<entry> [where] [-absolute]" }, true, 1), new ZipShell_GetCommand()) },
            { "list", new CommandInfo("list", ShellType.ZIPShell, "Lists all files inside the archive", new CommandArgumentInfo(new[] { "[directory]" }, false, 0), new ZipShell_ListCommand()) },
            { "pack", new CommandInfo("pack", ShellType.ZIPShell, "Packs a local file to the archive", new CommandArgumentInfo(new[] { "<localfile> [where]" }, true, 1), new ZipShell_PackCommand()) }
        };
        internal readonly static Dictionary<string, CommandInfo> ZipShell_ModCommands = new();
        public static FileStream ZipShell_FileStream;
        public static ZipArchive ZipShell_ZipArchive;
        public static string ZipShell_CurrentDirectory;
        public static string ZipShell_CurrentArchiveDirectory;

    }
}
