
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

using System.IO;
using KS.Misc.Editors.HexEdit;
using KS.Misc.Threading;

namespace KS.Shell.Shells.Hex
{
    /// <summary>
    /// Common hex editor shell module
    /// </summary>
    public static class HexEditShellCommon
    {

        /// <summary>
        /// File stream for the hex editor
        /// </summary>
        public static FileStream HexEdit_FileStream;
        /// <summary>
        /// File bytes for the hex editor
        /// </summary>
        public static byte[] HexEdit_FileBytes;
        /// <summary>
        /// Auto save thread for file
        /// </summary>
        public static KernelThread HexEdit_AutoSave = new("Hex Edit Autosave Thread", false, HexEditTools.HexEdit_HandleAutoSaveBinaryFile);
        /// <summary>
        /// Auto save flag
        /// </summary>
        public static bool HexEdit_AutoSaveFlag = true;
        /// <summary>
        /// Auto save interval in seconds
        /// </summary>
        public static int HexEdit_AutoSaveInterval = 60;
        internal static byte[] HexEdit_FileBytesOrig;

    }
}
