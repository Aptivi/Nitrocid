
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
using KS.Misc.Editors.HexEdit;
using KS.Misc.Threading;

namespace KS.Shell.Shells.Hex
{
    /// <summary>
    /// Common hex editor shell module
    /// </summary>
    public static class HexEditShellCommon
    {

        private static int autoSaveInterval = 60;
        internal static byte[] HexEdit_FileBytesOrig;
        internal static FileStream HexEdit_FileStream;
        internal static byte[] HexEdit_FileBytes;
        internal static KernelThread HexEdit_AutoSave = new("Hex Edit Autosave Thread", false, HexEditTools.HexEdit_HandleAutoSaveBinaryFile);

        /// <summary>
        /// Auto save flag
        /// </summary>
        public static bool HexEdit_AutoSaveFlag { get; set; } = true;
        /// <summary>
        /// Auto save interval in seconds
        /// </summary>
        public static int HexEdit_AutoSaveInterval
        {
            get => autoSaveInterval;
            set => autoSaveInterval = value < 0 ? 60 : value;
        }

    }
}
