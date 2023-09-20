
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

using KS.Drivers;
using KS.Kernel.Configuration;

namespace KS.Files.Print
{
    /// <summary>
    /// File content printing module
    /// </summary>
    public static class FileContentPrinter
    {

        /// <summary>
        /// Prints the contents of a file to the console
        /// </summary>
        /// <param name="filename">Full path to file</param>
        public static void PrintContents(string filename) => 
            DriverHandler.CurrentFilesystemDriverLocal.PrintContents(filename, KernelFlags.PrintLineNumbers);

        /// <summary>
        /// Prints the contents of a file to the console
        /// </summary>
        /// <param name="filename">Full path to file with wildcards supported</param>
        /// <param name="PrintLineNumbers">Whether to also print the line numbers or not</param>
        /// <param name="ForcePlain">Forces binary files to be printed verbatim</param>
        public static void PrintContents(string filename, bool PrintLineNumbers, bool ForcePlain = false) =>
            DriverHandler.CurrentFilesystemDriverLocal.PrintContents(filename, PrintLineNumbers, ForcePlain);

        /// <summary>
        /// Renders the file in hex
        /// </summary>
        /// <param name="StartByte">Start byte position</param>
        /// <param name="EndByte">End byte position</param>
        /// <param name="FileByte">File content in bytes</param>
        public static void DisplayInHex(long StartByte, long EndByte, byte[] FileByte) =>
            DriverHandler.CurrentFilesystemDriverLocal.DisplayInHex(StartByte, EndByte, FileByte);
    }
}
