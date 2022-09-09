
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

using KS.ConsoleBase.Colors;
using KS.Files.Folders;
using KS.Files.Read;
using KS.Kernel;
using KS.Misc.Writers.ConsoleWriters;

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
        public static void PrintContents(string filename) => PrintContents(filename, Flags.PrintLineNumbers);

        /// <summary>
        /// Prints the contents of a file to the console
        /// </summary>
        /// <param name="filename">Full path to file with wildcards supported</param>
        /// <param name="PrintLineNumbers">Whether to also print the line numbers or not</param>
        public static void PrintContents(string filename, bool PrintLineNumbers)
        {
            // Read the contents
            Filesystem.ThrowOnInvalidPath(filename);
            filename = Filesystem.NeutralizePath(filename);
            foreach (string FilePath in Listing.GetFilesystemEntries(filename, true))
            {
                var Contents = FileRead.ReadContents(FilePath);
                for (int ContentIndex = 0, loopTo = Contents.Length - 1; ContentIndex <= loopTo; ContentIndex++)
                {
                    if (PrintLineNumbers)
                    {
                        TextWriterColor.Write("{0,4}: ", false, ColorTools.ColTypes.ListEntry, ContentIndex + 1);
                    }
                    TextWriterColor.Write(Contents[ContentIndex], true, ColorTools.ColTypes.Neutral);
                }
            }
        }

    }
}
