
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

using System;
using System.IO;
using KS.Kernel;
using KS.Kernel.Debugging;

namespace KS.Files.Querying
{
    /// <summary>
    /// File parsing module
    /// </summary>
    public static class Parsing
    {

        /// <summary>
        /// Gets all the invalid path characters
        /// </summary>
        public static char[] GetInvalidPathChars()
        {
            var FinalInvalidPathChars = Path.GetInvalidPathChars();
            var WindowsInvalidPathChars = new[] { '"', '<', '>' };
            if (Kernel.Kernel.KernelSimulatorMoniker == ".NET CoreCLR" & KernelPlatform.IsOnWindows())
            {
                // It's weird of .NET 6.0 to not consider the above three Windows invalid directory chars to be invalid,
                // so make them invalid as in .NET Framework.
                Array.Resize(ref FinalInvalidPathChars, 36);
                WindowsInvalidPathChars.CopyTo(FinalInvalidPathChars, FinalInvalidPathChars.Length - 3);
            }
            return FinalInvalidPathChars;
        }

        /// <summary>
        /// Tries to parse the path (For file names and only names, use <see cref="TryParseFileName(string)"/> instead.)
        /// </summary>
        /// <param name="Path">The path to be parsed</param>
        /// <returns>True if successful; false if unsuccessful</returns>
        public static bool TryParsePath(string Path)
        {
            try
            {
                Filesystem.ThrowOnInvalidPath(Path);
                return !(Path.IndexOfAny(GetInvalidPathChars()) >= 0);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to parse path {0}: {1}", Path, ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Tries to parse the file name (For full paths, use <see cref="TryParsePath(string)"/> instead.)
        /// </summary>
        /// <param name="Name">The file name to be parsed</param>
        /// <returns>True if successful; false if unsuccessful</returns>
        public static bool TryParseFileName(string Name)
        {
            try
            {
                Filesystem.ThrowOnInvalidPath(Name);
                return !(Name.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to parse file name {0}: {1}", Name, ex.Message);
            }
            return false;
        }

    }
}
