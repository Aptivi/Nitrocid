//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using Nitrocid.Base.Drivers;
using Nitrocid.Base.Kernel.Configuration;

namespace Nitrocid.Base.Files
{
    /// <summary>
    /// File content printing module
    /// </summary>
    public static partial class FilesystemTools
    {

        /// <summary>
        /// Prints the contents of a file to the console
        /// </summary>
        /// <param name="filename">Full path to file</param>
        /// <param name="colors">Whether to use colors or not</param>
        public static void PrintContents(string filename, bool colors = true) =>
            DriverHandler.CurrentFilesystemDriverLocal.PrintContents(filename, Config.MainConfig.PrintLineNumbers, colors);

        /// <summary>
        /// Prints the contents of a file to the console
        /// </summary>
        /// <param name="filename">Full path to file with wildcards supported</param>
        /// <param name="PrintLineNumbers">Whether to also print the line numbers or not</param>
        /// <param name="ForcePlain">Forces binary files to be printed verbatim</param>
        /// <param name="colors">Whether to use colors or not</param>
        public static void PrintContents(string filename, bool PrintLineNumbers, bool ForcePlain = false, bool colors = true) =>
            DriverHandler.CurrentFilesystemDriverLocal.PrintContents(filename, PrintLineNumbers, ForcePlain, colors);

        /// <summary>
        /// Renders the file in hex
        /// </summary>
        /// <param name="StartByte">Start byte position</param>
        /// <param name="EndByte">End byte position</param>
        /// <param name="FileByte">File content in bytes</param>
        /// <param name="colors">Whether to use colors or not</param>
        public static void DisplayInHex(long StartByte, long EndByte, byte[] FileByte, bool colors = true) =>
            DriverHandler.CurrentFilesystemDriverLocal.DisplayInHex(StartByte, EndByte, FileByte, colors);

        /// <summary>
        /// Renders the file in hex
        /// </summary>
        /// <param name="ByteContent">Content to highlight</param>
        /// <param name="HighlightResults">Whether to highlight the results. For querying.</param>
        /// <param name="StartByte">Start byte position</param>
        /// <param name="EndByte">End byte position</param>
        /// <param name="FileByte">File content in bytes</param>
        /// <param name="colors">Whether to use colors or not</param>
        public static void DisplayInHex(byte ByteContent, bool HighlightResults, long StartByte, long EndByte, byte[] FileByte, bool colors = true) =>
            DriverHandler.CurrentFilesystemDriverLocal.DisplayInHex(ByteContent, HighlightResults, StartByte, EndByte, FileByte, colors);

        /// <summary>
        /// Renders the contents of a file
        /// </summary>
        /// <param name="filename">Full path to file with wildcards supported</param>
        /// <param name="colors">Whether to use colors or not</param>
        public static string RenderContents(string filename, bool colors = true) =>
            DriverHandler.CurrentFilesystemDriverLocal.RenderContents(filename, Config.MainConfig.PrintLineNumbers, false, colors);

        /// <summary>
        /// Renders the contents of a file
        /// </summary>
        /// <param name="filename">Full path to file with wildcards supported</param>
        /// <param name="PrintLineNumbers">Whether to also print the line numbers or not</param>
        /// <param name="ForcePlain">Forces binary files to be printed verbatim</param>
        /// <param name="colors">Whether to use colors or not</param>
        public static string RenderContents(string filename, bool PrintLineNumbers, bool ForcePlain = false, bool colors = true) =>
            DriverHandler.CurrentFilesystemDriverLocal.RenderContents(filename, PrintLineNumbers, ForcePlain, colors);

        /// <summary>
        /// Renders the file in hex
        /// </summary>
        /// <param name="StartByte">Start byte position</param>
        /// <param name="EndByte">End byte position</param>
        /// <param name="FileByte">File content in bytes</param>
        /// <param name="colors">Whether to use colors or not</param>
        public static string RenderContentsInHex(long StartByte, long EndByte, byte[] FileByte, bool colors = true) =>
            DriverHandler.CurrentFilesystemDriverLocal.RenderContentsInHex(StartByte, EndByte, FileByte, colors);

        /// <summary>
        /// Renders the file in hex
        /// </summary>
        /// <param name="ByteContent">Content to highlight</param>
        /// <param name="HighlightResults">Whether to highlight the results. For querying.</param>
        /// <param name="StartByte">Start byte position</param>
        /// <param name="EndByte">End byte position</param>
        /// <param name="FileByte">File content in bytes</param>
        /// <param name="colors">Whether to use colors or not</param>
        public static string RenderContentsInHex(byte ByteContent, bool HighlightResults, long StartByte, long EndByte, byte[] FileByte, bool colors = true) =>
            DriverHandler.CurrentFilesystemDriverLocal.RenderContentsInHex(ByteContent, HighlightResults, StartByte, EndByte, FileByte, colors);

        /// <summary>
        /// Renders the file in hex
        /// </summary>
        /// <param name="ByteHighlight">Byte position to highlight</param>
        /// <param name="StartByte">Start byte position</param>
        /// <param name="EndByte">End byte position</param>
        /// <param name="FileByte">File content in bytes</param>
        /// <param name="colors">Whether to use colors or not</param>
        public static string RenderContentsInHex(long ByteHighlight, long StartByte, long EndByte, byte[] FileByte, bool colors = true) =>
            DriverHandler.CurrentFilesystemDriverLocal.RenderContentsInHex(ByteHighlight, StartByte, EndByte, FileByte, colors);

    }
}
