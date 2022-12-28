
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

using System.Collections.Generic;
using System.IO;
using KS.Misc.Editors.TextEdit;
using KS.Misc.Threading;

namespace KS.Shell.Shells.Text
{
    /// <summary>
    /// Common text editor shell module
    /// </summary>
    public static class TextEditShellCommon
    {

        /// <summary>
        /// File lines for text editor
        /// </summary>
        public static List<string> TextEdit_FileLines;
        /// <summary>
        /// File stream for text editor
        /// </summary>
        public static FileStream TextEdit_FileStream;
        /// <summary>
        /// Auto save thread for the text editor
        /// </summary>
        public static KernelThread TextEdit_AutoSave = new("Text Edit Autosave Thread", false, TextEditTools.TextEdit_HandleAutoSaveTextFile);
        /// <summary>
        /// Auto save flag
        /// </summary>
        public static bool TextEdit_AutoSaveFlag = true;
        /// <summary>
        /// Auto save interval in seconds
        /// </summary>
        public static int TextEdit_AutoSaveInterval = 60;
        internal static List<string> TextEdit_FileLinesOrig;

    }
}
