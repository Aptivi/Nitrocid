
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
using KS.Shell.Shells.Rar.Commands;
using SharpCompress.Archives.Rar;

namespace KS.Shell.Shells.Rar
{
    static class RarShellCommon
    {

        // Variables
        public readonly static Dictionary<string, CommandInfo> RarShell_Commands = new Dictionary<string, CommandInfo>() { { "cdir", new CommandInfo("cdir", ShellType.RARShell, "Gets current local directory", new CommandArgumentInfo(), new RarShell_CDirCommand()) }, { "chdir", new CommandInfo("chdir", ShellType.RARShell, "Changes directory", new CommandArgumentInfo(new[] { "<directory>" }, true, 1), new RarShell_ChDirCommand()) }, { "chadir", new CommandInfo("chadir", ShellType.RARShell, "Changes archive directory", new CommandArgumentInfo(new[] { "<archivedirectory>" }, true, 1), new RarShell_ChADirCommand()) }, { "get", new CommandInfo("get", ShellType.RARShell, "Extracts a file to a specified directory or a current directory", new CommandArgumentInfo(new[] { "<entry> [where] [-absolute]" }, true, 1), new RarShell_GetCommand()) }, { "list", new CommandInfo("list", ShellType.RARShell, "Lists all files inside the archive", new CommandArgumentInfo(new[] { "[directory]" }, false, 0), new RarShell_ListCommand()) } };
        internal readonly static Dictionary<string, CommandInfo> RarShell_ModCommands = new Dictionary<string, CommandInfo>();
        public static FileStream RarShell_FileStream;
        public static RarArchive RarShell_RarArchive;
        public static string RarShell_CurrentDirectory;
        public static string RarShell_CurrentArchiveDirectory;

    }
}