
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
using KS.Misc.Editors.JsonShell;
using KS.Misc.Threading;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using KS.Shell.Shells.Json.Commands;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KS.Shell.Shells.Json
{
    /// <summary>
    /// Common JSON shell module
    /// </summary>
    public static class JsonShellCommon
    {

        /// <summary>
        /// JSON shell commands
        /// </summary>
        public readonly static Dictionary<string, CommandInfo> JsonShell_Commands = new()
        {
            { "addproperty", new CommandInfo("addproperty", ShellType.JsonShell, "Adds a new property at the end of the JSON file", new CommandArgumentInfo(new[] { "<parentProperty> <propertyName> <propertyValue>" }, true, 3), new JsonShell_AddPropertyCommand()) },
            { "clear", new CommandInfo("clear", ShellType.JsonShell, "Clears the JSON file", new CommandArgumentInfo(), new JsonShell_ClearCommand()) },
            { "delproperty", new CommandInfo("delproperty", ShellType.JsonShell, "Removes a property from the JSON file", new CommandArgumentInfo(new[] { "<propertyName>" }, true, 1), new JsonShell_DelPropertyCommand()) },
            { "exitnosave", new CommandInfo("exitnosave", ShellType.JsonShell, "Exits the JSON shell without saving the changes", new CommandArgumentInfo(), new JsonShell_ExitNoSaveCommand()) },
            { "print", new CommandInfo("print", ShellType.JsonShell, "Prints the JSON file", new CommandArgumentInfo(new[] { "[property]" }, false, 0), new JsonShell_PrintCommand()) },
            { "save", new CommandInfo("save", ShellType.JsonShell, "Saves the JSON file", new CommandArgumentInfo(new[] { "[-b|-m]" }, false, 0), new JsonShell_SaveCommand()) }
        };
        /// <summary>
        /// JSON shell file stream
        /// </summary>
        public static FileStream JsonShell_FileStream;
        /// <summary>
        /// JSON shell file token
        /// </summary>
        public static JToken JsonShell_FileToken = JToken.Parse("{}");
        /// <summary>
        /// JSON shell auto save thread
        /// </summary>
        public static KernelThread JsonShell_AutoSave = new("JSON Shell Autosave Thread", false, JsonTools.JsonShell_HandleAutoSaveJsonFile);
        /// <summary>
        /// Auto save flag
        /// </summary>
        public static bool JsonShell_AutoSaveFlag = true;
        /// <summary>
        /// Auto save interval in seconds
        /// </summary>
        public static int JsonShell_AutoSaveInterval = 60;
        /// <summary>
        /// JSON formatting
        /// </summary>
        public static Formatting JsonShell_Formatting = Formatting.Indented;
        internal static JToken JsonShell_FileTokenOrig = JToken.Parse("{}");
        internal readonly static Dictionary<string, CommandInfo> JsonShell_ModCommands = new();

    }
}
