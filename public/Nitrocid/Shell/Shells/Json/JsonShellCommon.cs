
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System.IO;
using KS.Misc.Editors.JsonShell;
using KS.Misc.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KS.Shell.Shells.Json
{
    /// <summary>
    /// Common JSON shell module
    /// </summary>
    public static class JsonShellCommon
    {

        internal static JToken JsonShell_FileTokenOrig = JToken.Parse("{}");
        internal static FileStream JsonShell_FileStream;
        internal static KernelThread JsonShell_AutoSave = new("JSON Shell Autosave Thread", false, JsonTools.JsonShell_HandleAutoSaveJsonFile);
        private static int autoSaveInterval = 60;

        /// <summary>
        /// JSON shell file token
        /// </summary>
        public static JToken JsonShell_FileToken { get; set; } = JToken.Parse("{}");
        /// <summary>
        /// Auto save flag
        /// </summary>
        public static bool JsonShell_AutoSaveFlag { get; set; } = true;
        /// <summary>
        /// JSON formatting
        /// </summary>
        public static Formatting JsonShell_Formatting { get; set; } = Formatting.Indented;
        /// <summary>
        /// Auto save interval in seconds
        /// </summary>
        public static int JsonShell_AutoSaveInterval
        {
            get => autoSaveInterval;
            set => autoSaveInterval = value < 0 ? 60 : value;
        }

    }
}
