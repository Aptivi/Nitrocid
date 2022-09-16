
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

using Extensification.LongExts;
using KS.ConsoleBase.Colors;
using KS.Files.Folders;
using KS.Files.Querying;
using KS.Files.Read;
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using System;
using System.IO;

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
        /// <param name="ForcePlain">Forces binary files to be printed verbatim</param>
        public static void PrintContents(string filename, bool PrintLineNumbers, bool ForcePlain = false)
        {
            // Check the path
            Filesystem.ThrowOnInvalidPath(filename);
            filename = Filesystem.NeutralizePath(filename);

            // If interacting with the binary file, display it in hex. Otherwise, display it as if it is text. Except if forced to view binaries as texts.
            if (Parsing.IsBinaryFile(filename) && !ForcePlain)
            {
                byte[] bytes = File.ReadAllBytes(filename);
                DisplayInHex(1, bytes.LongLength, bytes);
            }
            else
                PrintContentsInternal(filename, PrintLineNumbers);
        }

        private static void PrintContentsInternal(string filename, bool PrintLineNumbers)
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

        /// <summary>
        /// Renders the file in hex
        /// </summary>
        /// <param name="StartByte">Start byte position</param>
        /// <param name="EndByte">End byte position</param>
        /// <param name="FileByte">File content in bytes</param>
        public static void DisplayInHex(long StartByte, long EndByte, byte[] FileByte)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "File Bytes: {0}", FileByte.LongLength);
            StartByte.SwapIfSourceLarger(ref EndByte);
            if (StartByte <= FileByte.LongLength & EndByte <= FileByte.LongLength)
            {
                // We need to know how to write the bytes and their contents in this shape:
                // -> 0x00000010  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
                //    0x00000020  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
                //    0x00000030  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
                // ... and so on.
                TextWriterColor.Write($"0x{StartByte - 1L:X8}", false, ColorTools.ColTypes.ListEntry);
                int ByteWritePositionX = $"0x{StartByte - 1L:X8}".Length + 2;
                int ByteCharWritePositionX = 61 + (ByteWritePositionX - 12);
                int ByteNumberEachSixteen = 1;
                for (long CurrentByteNumber = StartByte, loopTo = EndByte; CurrentByteNumber <= loopTo; CurrentByteNumber++)
                {
                    // Write the byte and the contents
                    DebugWriter.WriteDebug(DebugLevel.I, "Byte write position: {0}", ByteWritePositionX);
                    DebugWriter.WriteDebug(DebugLevel.I, "Byte char write position: {0}", ByteCharWritePositionX);
                    DebugWriter.WriteDebug(DebugLevel.I, "Byte number each sixteen: {0}", ByteNumberEachSixteen);
                    byte CurrentByte = FileByte[(int)(CurrentByteNumber - 1L)];
                    DebugWriter.WriteDebug(DebugLevel.I, "Byte: {0}", CurrentByte);
                    char ProjectedByteChar = Convert.ToChar(CurrentByte);
                    DebugWriter.WriteDebug(DebugLevel.I, "Projected byte char: {0}", ProjectedByteChar);
                    char RenderedByteChar = '.';
                    if (!char.IsWhiteSpace(ProjectedByteChar) & !char.IsControl(ProjectedByteChar) & !char.IsHighSurrogate(ProjectedByteChar) & !char.IsLowSurrogate(ProjectedByteChar))
                    {
                        // The renderer will actually render the character, not as a dot.
                        DebugWriter.WriteDebug(DebugLevel.I, "Char is not a whitespace.");
                        RenderedByteChar = ProjectedByteChar;
                    }
                    DebugWriter.WriteDebug(DebugLevel.I, "Rendered byte char: {0}", ProjectedByteChar);
                    TextWriterWhereColor.WriteWhere($"{CurrentByte:X2}", ByteWritePositionX + 3 * (ByteNumberEachSixteen - 1), ConsoleBase.ConsoleWrapper.CursorTop, false, ColorTools.ColTypes.ListValue);
                    TextWriterWhereColor.WriteWhere($"{RenderedByteChar}", ByteCharWritePositionX + (ByteNumberEachSixteen - 1), ConsoleBase.ConsoleWrapper.CursorTop, false, ColorTools.ColTypes.ListValue);

                    // Increase the byte number
                    ByteNumberEachSixteen += 1;

                    // Check to see if we've exceeded 16 bytes
                    if (ByteNumberEachSixteen > 16)
                    {
                        // OK, let's increase the byte iteration and get the next line ready
                        TextWriterColor.Write(Kernel.Kernel.NewLine + $"0x{CurrentByteNumber:X8}", false, ColorTools.ColTypes.ListEntry);
                        ByteWritePositionX = $"0x{CurrentByteNumber:X8}".Length + 2;
                        ByteCharWritePositionX = 61 + (ByteWritePositionX - 12);
                        ByteNumberEachSixteen = 1;
                    }
                }
                TextWriterColor.Write("", true, ColorTools.ColTypes.Neutral);
            }
            else if (StartByte > FileByte.LongLength)
            {
                TextWriterColor.Write(Translate.DoTranslation("The specified start byte number may not be larger than the file size."), true, ColorTools.ColTypes.Error);
            }
            else if (EndByte > FileByte.LongLength)
            {
                TextWriterColor.Write(Translate.DoTranslation("The specified end byte number may not be larger than the file size."), true, ColorTools.ColTypes.Error);
            }
        }
    }
}
