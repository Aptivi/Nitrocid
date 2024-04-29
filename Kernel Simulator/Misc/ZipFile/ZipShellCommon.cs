﻿//
// Kernel Simulator  Copyright (C) 2018-2024  Aptivi
//
// This file is part of Kernel Simulator
//
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using System.IO;
using KS.Misc.ZipFile.Commands;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;

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

using SharpCompress.Archives.Zip;

namespace KS.Misc.ZipFile
{
    static class ZipShellCommon
    {

        // Variables
        public static readonly Dictionary<string, CommandInfo> ZipShell_Commands = new() { { "cdir", new CommandInfo("cdir", ShellType.ZIPShell, "Gets current local directory", new CommandArgumentInfo([], false, 0), new ZipShell_CDirCommand()) }, { "chdir", new CommandInfo("chdir", ShellType.ZIPShell, "Changes directory", new CommandArgumentInfo(["<directory>"], true, 1), new ZipShell_ChDirCommand()) }, { "chadir", new CommandInfo("chadir", ShellType.ZIPShell, "Changes archive directory", new CommandArgumentInfo(["<archivedirectory>"], true, 1), new ZipShell_ChADirCommand()) }, { "get", new CommandInfo("get", ShellType.ZIPShell, "Extracts a file to a specified directory or a current directory", new CommandArgumentInfo(["<entry> [where] [-absolute]"], true, 1), new ZipShell_GetCommand(), false, false, false, false, false) }, { "help", new CommandInfo("help", ShellType.ZIPShell, "Lists available commands", new CommandArgumentInfo(["[command]"], false, 0), new ZipShell_HelpCommand()) }, { "list", new CommandInfo("list", ShellType.ZIPShell, "Lists all files inside the archive", new CommandArgumentInfo(["[directory]"], false, 0), new ZipShell_ListCommand()) }, { "pack", new CommandInfo("pack", ShellType.ZIPShell, "Packs a local file to the archive", new CommandArgumentInfo(["<localfile> [where]"], true, 1), new ZipShell_PackCommand()) } };
        internal static readonly Dictionary<string, CommandInfo> ZipShell_ModCommands = [];
        public static FileStream ZipShell_FileStream;
        public static ZipArchive ZipShell_ZipArchive;
        public static string ZipShell_CurrentDirectory;
        public static string ZipShell_CurrentArchiveDirectory;

    }
}