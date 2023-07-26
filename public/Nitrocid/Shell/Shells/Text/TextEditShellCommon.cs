
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

using System.Collections.Generic;
using System.IO;
using KS.Kernel.Configuration;
using KS.Kernel.Threading;
using KS.Misc.Editors.TextEdit;

namespace KS.Shell.Shells.Text
{
    /// <summary>
    /// Common text editor shell module
    /// </summary>
    public static class TextEditShellCommon
    {

        internal static List<string> fileLines = new();
        internal static FileStream fileStream;
        internal static int autoSaveInterval = 60;
        internal static List<string> TextEdit_FileLinesOrig;
        internal static KernelThread TextEdit_AutoSave = new("Text Edit Autosave Thread", false, TextEditTools.TextEdit_HandleAutoSaveTextFile);

        /// <summary>
        /// File lines for text editor
        /// </summary>
        public static List<string> TextEdit_FileLines =>
            fileLines;
        /// <summary>
        /// File stream for text editor
        /// </summary>
        public static FileStream TextEdit_FileStream =>
            fileStream;
        /// <summary>
        /// Auto save flag
        /// </summary>
        public static bool TextEdit_AutoSaveFlag =>
            Config.MainConfig.TextEdit_AutoSaveFlag;
        /// <summary>
        /// Auto save interval in seconds
        /// </summary>
        public static int TextEdit_AutoSaveInterval =>
            Config.MainConfig.TextEdit_AutoSaveInterval;

    }
}
