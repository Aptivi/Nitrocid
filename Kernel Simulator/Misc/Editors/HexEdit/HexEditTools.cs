
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
using System.Linq;
using System.Threading;
using Extensification.ArrayExts;
using Extensification.LongExts;
using KS.ConsoleBase.Colors;
using KS.Files;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Shell.Shells.Hex;

namespace KS.Misc.Editors.HexEdit
{
    public static class HexEditTools
    {

        /// <summary>
        /// Opens the binary file
        /// </summary>
        /// <param name="File">Target file. We recommend you to use <see cref="Filesystem.NeutralizePath(string, bool)"></see> to neutralize path.</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool HexEdit_OpenBinaryFile(string File)
        {
            try
            {
                DebugWriter.Wdbg(DebugLevel.I, "Trying to open file {0}...", File);
                HexEditShellCommon.HexEdit_FileStream = new FileStream(File, FileMode.Open);
                DebugWriter.Wdbg(DebugLevel.I, "File {0} is open. Length: {1}, Pos: {2}", File, HexEditShellCommon.HexEdit_FileStream.Length, HexEditShellCommon.HexEdit_FileStream.Position);

                // Read the file
                var FileBytes = new byte[(int)(HexEditShellCommon.HexEdit_FileStream.Length + 1)];
                HexEditShellCommon.HexEdit_FileStream.Read(FileBytes, 0, (int)HexEditShellCommon.HexEdit_FileStream.Length);
                HexEditShellCommon.HexEdit_FileStream.Seek(0L, SeekOrigin.Begin);

                // Add the information to the arrays
                HexEditShellCommon.HexEdit_FileBytes = FileBytes;
                HexEditShellCommon.HexEdit_FileBytesOrig = FileBytes;
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.Wdbg(DebugLevel.E, "Open file {0} failed: {1}", File, ex.Message);
                DebugWriter.WStkTrc(ex);
                return false;
            }
        }

        /// <summary>
        /// Closes binary file
        /// </summary>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool HexEdit_CloseBinaryFile()
        {
            try
            {
                DebugWriter.Wdbg(DebugLevel.I, "Trying to close file...");
                HexEditShellCommon.HexEdit_FileStream.Close();
                HexEditShellCommon.HexEdit_FileStream = null;
                DebugWriter.Wdbg(DebugLevel.I, "File is no longer open.");
                HexEditShellCommon.HexEdit_FileBytes = Array.Empty<byte>();
                HexEditShellCommon.HexEdit_FileBytesOrig = Array.Empty<byte>();
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.Wdbg(DebugLevel.E, "Closing file failed: {0}", ex.Message);
                DebugWriter.WStkTrc(ex);
                return false;
            }
        }

        /// <summary>
        /// Saves binary file
        /// </summary>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool HexEdit_SaveBinaryFile()
        {
            try
            {
                var FileBytes = HexEditShellCommon.HexEdit_FileBytes;
                DebugWriter.Wdbg(DebugLevel.I, "Trying to save file...");
                HexEditShellCommon.HexEdit_FileStream.SetLength(0L);
                DebugWriter.Wdbg(DebugLevel.I, "Length set to 0.");
                HexEditShellCommon.HexEdit_FileStream.Write(FileBytes, 0, FileBytes.Length);
                HexEditShellCommon.HexEdit_FileStream.Flush();
                DebugWriter.Wdbg(DebugLevel.I, "File is saved.");
                HexEditShellCommon.HexEdit_FileBytesOrig = FileBytes;
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.Wdbg(DebugLevel.E, "Saving file failed: {0}", ex.Message);
                DebugWriter.WStkTrc(ex);
                return false;
            }
        }

        /// <summary>
        /// Handles autosave
        /// </summary>
        public static void HexEdit_HandleAutoSaveBinaryFile()
        {
            if (HexEditShellCommon.HexEdit_AutoSaveFlag)
            {
                try
                {
                    Thread.Sleep(HexEditShellCommon.HexEdit_AutoSaveInterval * 1000);
                    if (HexEditShellCommon.HexEdit_FileStream is not null)
                    {
                        HexEdit_SaveBinaryFile();
                    }
                }
                catch (Exception ex)
                {
                    DebugWriter.WStkTrc(ex);
                }
            }
        }

        /// <summary>
        /// Was binary edited?
        /// </summary>
        public static bool HexEdit_WasHexEdited()
        {
            if (HexEditShellCommon.HexEdit_FileBytes is not null & HexEditShellCommon.HexEdit_FileBytesOrig is not null)
            {
                return !HexEditShellCommon.HexEdit_FileBytes.SequenceEqual(HexEditShellCommon.HexEdit_FileBytesOrig);
            }
            return false;
        }

        /// <summary>
        /// Adds a new byte to the current hex
        /// </summary>
        /// <param name="Content">New byte content</param>
        public static void HexEdit_AddNewByte(byte Content)
        {
            if (HexEditShellCommon.HexEdit_FileStream is not null)
            {
                HexEditShellCommon.HexEdit_FileBytes.Add(Content);
            }
            else
            {
                throw new InvalidOperationException(Translate.DoTranslation("The hex editor hasn't opened a file stream yet."));
            }
        }

        /// <summary>
        /// Adds the new bytes to the current hex
        /// </summary>
        /// <param name="Bytes">New bytes</param>
        public static void HexEdit_AddNewBytes(byte[] Bytes)
        {
            if (HexEditShellCommon.HexEdit_FileStream is not null)
            {
                foreach (byte ByteContent in Bytes)
                    HexEditShellCommon.HexEdit_FileBytes.Add(ByteContent);
            }
            else
            {
                throw new InvalidOperationException(Translate.DoTranslation("The hex editor hasn't opened a file stream yet."));
            }
        }

        /// <summary>
        /// Deletes a byte
        /// </summary>
        /// <param name="ByteNumber">The byte number</param>
        public static void HexEdit_DeleteByte(long ByteNumber)
        {
            if (HexEditShellCommon.HexEdit_FileStream is not null)
            {
                var FileBytesList = HexEditShellCommon.HexEdit_FileBytes.ToList();
                long ByteIndex = ByteNumber - 1L;
                DebugWriter.Wdbg(DebugLevel.I, "Byte index: {0}, number: {1}", ByteIndex, ByteNumber);
                DebugWriter.Wdbg(DebugLevel.I, "File length: {0}", HexEditShellCommon.HexEdit_FileBytes.LongLength);

                // Actually remove a byte
                if (ByteNumber <= HexEditShellCommon.HexEdit_FileBytes.LongLength)
                {
                    FileBytesList.RemoveAt((int)ByteIndex);
                    DebugWriter.Wdbg(DebugLevel.I, "Removed {0}. Result: {1}", ByteIndex, HexEditShellCommon.HexEdit_FileBytes.LongLength);
                    HexEditShellCommon.HexEdit_FileBytes = FileBytesList.ToArray();
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(ByteNumber), ByteNumber, Translate.DoTranslation("The specified byte number may not be larger than the file size."));
                }
            }
            else
            {
                throw new InvalidOperationException(Translate.DoTranslation("The hex editor hasn't opened a file stream yet."));
            }
        }

        /// <summary>
        /// Deletes the bytes
        /// </summary>
        /// <param name="StartByteNumber">Start from the byte number</param>
        public static void HexEdit_DeleteBytes(long StartByteNumber)
        {
            HexEdit_DeleteBytes(StartByteNumber, HexEditShellCommon.HexEdit_FileBytes.LongLength);
        }

        /// <summary>
        /// Deletes the bytes
        /// </summary>
        /// <param name="StartByteNumber">Start from the byte number</param>
        /// <param name="EndByteNumber">Ending byte number</param>
        public static void HexEdit_DeleteBytes(long StartByteNumber, long EndByteNumber)
        {
            if (HexEditShellCommon.HexEdit_FileStream is not null)
            {
                StartByteNumber.SwapIfSourceLarger(ref EndByteNumber);
                long StartByteNumberIndex = StartByteNumber - 1L;
                long EndByteNumberIndex = EndByteNumber - 1L;
                var FileBytesList = HexEditShellCommon.HexEdit_FileBytes.ToList();
                DebugWriter.Wdbg(DebugLevel.I, "Start byte number: {0}, end: {1}", StartByteNumber, EndByteNumber);
                DebugWriter.Wdbg(DebugLevel.I, "Got start byte index: {0}", StartByteNumberIndex);
                DebugWriter.Wdbg(DebugLevel.I, "Got end byte index: {0}", EndByteNumberIndex);
                DebugWriter.Wdbg(DebugLevel.I, "File length: {0}", HexEditShellCommon.HexEdit_FileBytes.LongLength);

                // Actually remove the bytes
                if (StartByteNumber <= HexEditShellCommon.HexEdit_FileBytes.LongLength & EndByteNumber <= HexEditShellCommon.HexEdit_FileBytes.LongLength)
                {
                    for (long ByteNumber = EndByteNumber, loopTo = StartByteNumber; ByteNumber >= loopTo; ByteNumber += -1)
                        FileBytesList.RemoveAt((int)(ByteNumber - 1L));
                    DebugWriter.Wdbg(DebugLevel.I, "Removed {0} to {1}. New length: {2}", StartByteNumber, EndByteNumber, HexEditShellCommon.HexEdit_FileBytes.LongLength);
                    HexEditShellCommon.HexEdit_FileBytes = FileBytesList.ToArray();
                }
                else if (StartByteNumber > HexEditShellCommon.HexEdit_FileBytes.LongLength)
                {
                    throw new ArgumentOutOfRangeException(nameof(StartByteNumber), StartByteNumber, Translate.DoTranslation("The specified start byte number may not be larger than the file size."));
                }
                else if (EndByteNumber > HexEditShellCommon.HexEdit_FileBytes.LongLength)
                {
                    throw new ArgumentOutOfRangeException(nameof(EndByteNumber), EndByteNumber, Translate.DoTranslation("The specified end byte number may not be larger than the file size."));
                }
            }
            else
            {
                throw new InvalidOperationException(Translate.DoTranslation("The hex editor hasn't opened a file stream yet."));
            }
        }

        /// <summary>
        /// Renders the file in hex
        /// </summary>
        public static void HexEdit_DisplayHex()
        {
            HexEdit_DisplayHex(1L, HexEditShellCommon.HexEdit_FileBytes.LongLength);
        }

        /// <summary>
        /// Renders the file in hex
        /// </summary>
        public static void HexEdit_DisplayHex(long Start)
        {
            HexEdit_DisplayHex(Start, HexEditShellCommon.HexEdit_FileBytes.LongLength);
        }

        /// <summary>
        /// Renders the file in hex
        /// </summary>
        public static void HexEdit_DisplayHex(long StartByte, long EndByte)
        {
            if (HexEditShellCommon.HexEdit_FileStream is not null)
            {
                DebugWriter.Wdbg(DebugLevel.I, "File Bytes: {0}", HexEditShellCommon.HexEdit_FileBytes.LongLength);
                StartByte.SwapIfSourceLarger(ref EndByte);
                if (StartByte <= HexEditShellCommon.HexEdit_FileBytes.LongLength & EndByte <= HexEditShellCommon.HexEdit_FileBytes.LongLength)
                {
                    // We need to know how to write the bytes and their contents in this shape:
                    // -> 0x00000010  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
                    // 0x00000020  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
                    // 0x00000030  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
                    // ... and so on.
                    TextWriterColor.Write($"0x{StartByte - 1L:X8}", false, ColorTools.ColTypes.ListEntry);
                    int ByteWritePositionX = Console.CursorLeft + 2;
                    int ByteCharWritePositionX = 61 + (ByteWritePositionX - 12);
                    int ByteNumberEachSixteen = 1;
                    for (long CurrentByteNumber = StartByte, loopTo = EndByte; CurrentByteNumber <= loopTo; CurrentByteNumber++)
                    {
                        // Write the byte and the contents
                        DebugWriter.Wdbg(DebugLevel.I, "Byte write position: {0}", ByteWritePositionX);
                        DebugWriter.Wdbg(DebugLevel.I, "Byte char write position: {0}", ByteCharWritePositionX);
                        DebugWriter.Wdbg(DebugLevel.I, "Byte number each sixteen: {0}", ByteNumberEachSixteen);
                        byte CurrentByte = HexEditShellCommon.HexEdit_FileBytes[(int)(CurrentByteNumber - 1L)];
                        DebugWriter.Wdbg(DebugLevel.I, "Byte: {0}", CurrentByte);
                        char ProjectedByteChar = Convert.ToChar(CurrentByte);
                        DebugWriter.Wdbg(DebugLevel.I, "Projected byte char: {0}", ProjectedByteChar);
                        char RenderedByteChar = '.';
                        if (!char.IsWhiteSpace(ProjectedByteChar) & !char.IsControl(ProjectedByteChar) & !char.IsHighSurrogate(ProjectedByteChar) & !char.IsLowSurrogate(ProjectedByteChar))
                        {
                            // The renderer will actually render the character, not as a dot.
                            DebugWriter.Wdbg(DebugLevel.I, "Char is not a whitespace.");
                            RenderedByteChar = ProjectedByteChar;
                        }
                        DebugWriter.Wdbg(DebugLevel.I, "Rendered byte char: {0}", ProjectedByteChar);
                        TextWriterWhereColor.WriteWhere($"{CurrentByte:X2}", ByteWritePositionX + 3 * (ByteNumberEachSixteen - 1), Console.CursorTop, false, ColorTools.ColTypes.ListValue);
                        TextWriterWhereColor.WriteWhere($"{RenderedByteChar}", ByteCharWritePositionX + (ByteNumberEachSixteen - 1), Console.CursorTop, false, ColorTools.ColTypes.ListValue);

                        // Increase the byte number
                        ByteNumberEachSixteen += 1;

                        // Check to see if we've exceeded 16 bytes
                        if (ByteNumberEachSixteen > 16)
                        {
                            // OK, let's increase the byte iteration and get the next line ready
                            TextWriterColor.Write(Kernel.Kernel.NewLine + $"0x{CurrentByteNumber:X8}", false, ColorTools.ColTypes.ListEntry);
                            ByteWritePositionX = Console.CursorLeft + 2;
                            ByteCharWritePositionX = 61 + (ByteWritePositionX - 12);
                            ByteNumberEachSixteen = 1;
                        }
                    }
                    TextWriterColor.Write("", true, ColorTools.ColTypes.Neutral);
                }
                else if (StartByte > HexEditShellCommon.HexEdit_FileBytes.LongLength)
                {
                    TextWriterColor.Write(Translate.DoTranslation("The specified start byte number may not be larger than the file size."), true, ColorTools.ColTypes.Error);
                }
                else if (EndByte > HexEditShellCommon.HexEdit_FileBytes.LongLength)
                {
                    TextWriterColor.Write(Translate.DoTranslation("The specified end byte number may not be larger than the file size."), true, ColorTools.ColTypes.Error);
                }
            }
            else
            {
                throw new InvalidOperationException(Translate.DoTranslation("The hex editor hasn't opened a file stream yet."));
            }
        }

        /// <summary>
        /// Queries the byte and displays the results
        /// </summary>
        public static void HexEdit_QueryByteAndDisplay(byte ByteContent)
        {
            HexEdit_QueryByteAndDisplay(ByteContent, 1L, HexEditShellCommon.HexEdit_FileBytes.LongLength);
        }

        /// <summary>
        /// Queries the byte and displays the results
        /// </summary>
        public static void HexEdit_QueryByteAndDisplay(byte ByteContent, long Start)
        {
            HexEdit_QueryByteAndDisplay(ByteContent, Start, HexEditShellCommon.HexEdit_FileBytes.LongLength);
        }

        /// <summary>
        /// Queries the byte and displays the results
        /// </summary>
        public static void HexEdit_QueryByteAndDisplay(byte ByteContent, long StartByte, long EndByte)
        {
            if (HexEditShellCommon.HexEdit_FileStream is not null)
            {
                DebugWriter.Wdbg(DebugLevel.I, "File Bytes: {0}", HexEditShellCommon.HexEdit_FileBytes.LongLength);
                if (StartByte <= HexEditShellCommon.HexEdit_FileBytes.LongLength & EndByte <= HexEditShellCommon.HexEdit_FileBytes.LongLength)
                {
                    for (long ByteNumber = StartByte, loopTo = EndByte; ByteNumber <= loopTo; ByteNumber++)
                    {
                        if (HexEditShellCommon.HexEdit_FileBytes[(int)(ByteNumber - 1L)] == ByteContent)
                        {
                            long ByteRenderStart = ByteNumber - 2L;
                            long ByteRenderEnd = ByteNumber + 2L;
                            TextWriterColor.Write($"- 0x{ByteNumber:X8}: ", false, ColorTools.ColTypes.ListEntry);
                            for (long ByteRenderNumber = ByteRenderStart, loopTo1 = ByteRenderEnd; ByteRenderNumber <= loopTo1; ByteRenderNumber++)
                            {
                                if (ByteRenderStart < 0L)
                                    ByteRenderStart = 1L;
                                if (ByteRenderEnd > HexEditShellCommon.HexEdit_FileBytes.LongLength)
                                    ByteRenderEnd = HexEditShellCommon.HexEdit_FileBytes.LongLength;
                                bool UseHighlight = HexEditShellCommon.HexEdit_FileBytes[(int)(ByteRenderNumber - 1L)] == ByteContent;
                                byte CurrentByte = HexEditShellCommon.HexEdit_FileBytes[(int)(ByteRenderNumber - 1L)];
                                DebugWriter.Wdbg(DebugLevel.I, "Byte: {0}", CurrentByte);
                                char ProjectedByteChar = Convert.ToChar(CurrentByte);
                                DebugWriter.Wdbg(DebugLevel.I, "Projected byte char: {0}", ProjectedByteChar);
                                char RenderedByteChar = '.';
                                if (!char.IsWhiteSpace(ProjectedByteChar))
                                {
                                    // The renderer will actually render the character, not as a dot.
                                    DebugWriter.Wdbg(DebugLevel.I, "Char is not a whitespace.");
                                    RenderedByteChar = ProjectedByteChar;
                                }
                                TextWriterColor.Write($"0x{ByteRenderNumber:X2}({RenderedByteChar}) ", false, UseHighlight ? ColorTools.ColTypes.Success : ColorTools.ColTypes.ListValue);
                            }
                            TextWriterColor.Write("", true, ColorTools.ColTypes.Neutral);
                        }
                    }
                }
                else if (StartByte > HexEditShellCommon.HexEdit_FileBytes.LongLength)
                {
                    TextWriterColor.Write(Translate.DoTranslation("The specified start byte number may not be larger than the file size."), true, ColorTools.ColTypes.Error);
                }
                else if (EndByte > HexEditShellCommon.HexEdit_FileBytes.LongLength)
                {
                    TextWriterColor.Write(Translate.DoTranslation("The specified end byte number may not be larger than the file size."), true, ColorTools.ColTypes.Error);
                }
            }
            else
            {
                throw new InvalidOperationException(Translate.DoTranslation("The hex editor hasn't opened a file stream yet."));
            }
        }

        /// <summary>
        /// Replaces every occurence of a byte with the replacement
        /// </summary>
        /// <param name="FromByte">Byte to be replaced</param>
        /// <param name="WithByte">Byte to replace with</param>
        public static void HexEdit_Replace(byte FromByte, byte WithByte)
        {
            HexEdit_Replace(FromByte, WithByte, 1L, HexEditShellCommon.HexEdit_FileBytes.LongLength);
        }

        /// <summary>
        /// Replaces every occurence of a byte with the replacement
        /// </summary>
        /// <param name="FromByte">Byte to be replaced</param>
        /// <param name="WithByte">Byte to replace with</param>
        /// <param name="Start">Start byte number</param>
        public static void HexEdit_Replace(byte FromByte, byte WithByte, long Start)
        {
            HexEdit_Replace(FromByte, WithByte, Start, HexEditShellCommon.HexEdit_FileBytes.LongLength);
        }

        /// <summary>
        /// Replaces every occurence of a byte with the replacement
        /// </summary>
        /// <param name="FromByte">Byte to be replaced</param>
        /// <param name="WithByte">Byte to replace with</param>
        /// <param name="StartByte">Start byte number</param>
        /// <param name="EndByte">End byte number</param>
        public static void HexEdit_Replace(byte FromByte, byte WithByte, long StartByte, long EndByte)
        {
            if (HexEditShellCommon.HexEdit_FileStream is not null)
            {
                DebugWriter.Wdbg(DebugLevel.I, "Source: {0}, Target: {1}", FromByte, WithByte);
                DebugWriter.Wdbg(DebugLevel.I, "File Bytes: {0}", HexEditShellCommon.HexEdit_FileBytes.LongLength);
                if (StartByte <= HexEditShellCommon.HexEdit_FileBytes.LongLength & EndByte <= HexEditShellCommon.HexEdit_FileBytes.LongLength)
                {
                    for (long ByteNumber = StartByte, loopTo = EndByte; ByteNumber <= loopTo; ByteNumber++)
                    {
                        if (HexEditShellCommon.HexEdit_FileBytes[(int)(ByteNumber - 1L)] == FromByte)
                        {
                            DebugWriter.Wdbg(DebugLevel.I, "Replacing \"{0}\" with \"{1}\" in byte {2}", FromByte, WithByte, ByteNumber);
                            HexEditShellCommon.HexEdit_FileBytes[(int)(ByteNumber - 1L)] = WithByte;
                        }
                    }
                }
                else if (StartByte > HexEditShellCommon.HexEdit_FileBytes.LongLength)
                {
                    TextWriterColor.Write(Translate.DoTranslation("The specified start byte number may not be larger than the file size."), true, ColorTools.ColTypes.Error);
                }
                else if (EndByte > HexEditShellCommon.HexEdit_FileBytes.LongLength)
                {
                    TextWriterColor.Write(Translate.DoTranslation("The specified end byte number may not be larger than the file size."), true, ColorTools.ColTypes.Error);
                }
            }
            else
            {
                throw new InvalidOperationException(Translate.DoTranslation("The hex editor hasn't opened a file stream yet."));
            }
        }

    }
}
