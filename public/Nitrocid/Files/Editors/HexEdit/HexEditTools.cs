
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

using System;
using System.IO;
using System.Linq;
using System.Threading;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Drivers;
using KS.Files.Operations.Printing;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Reflection;
using KS.Shell.Shells.Hex;

namespace KS.Files.Editors.HexEdit
{
    /// <summary>
    /// Hex editor tools module
    /// </summary>
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
                DebugWriter.WriteDebug(DebugLevel.I, "Trying to open file {0}...", File);
                HexEditShellCommon.HexEdit_FileStream = new FileStream(File, FileMode.Open);
                DebugWriter.WriteDebug(DebugLevel.I, "File {0} is open. Length: {1}, Pos: {2}", File, HexEditShellCommon.HexEdit_FileStream.Length, HexEditShellCommon.HexEdit_FileStream.Position);

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
                DebugWriter.WriteDebug(DebugLevel.E, "Open file {0} failed: {1}", File, ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
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
                DebugWriter.WriteDebug(DebugLevel.I, "Trying to close file...");
                HexEditShellCommon.HexEdit_FileStream.Close();
                HexEditShellCommon.HexEdit_FileStream = null;
                DebugWriter.WriteDebug(DebugLevel.I, "File is no longer open.");
                HexEditShellCommon.HexEdit_FileBytes = Array.Empty<byte>();
                HexEditShellCommon.HexEdit_FileBytesOrig = Array.Empty<byte>();
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Closing file failed: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
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
                DebugWriter.WriteDebug(DebugLevel.I, "Trying to save file...");
                HexEditShellCommon.HexEdit_FileStream.SetLength(0L);
                DebugWriter.WriteDebug(DebugLevel.I, "Length set to 0.");
                HexEditShellCommon.HexEdit_FileStream.Write(FileBytes, 0, FileBytes.Length);
                HexEditShellCommon.HexEdit_FileStream.Flush();
                DebugWriter.WriteDebug(DebugLevel.I, "File is saved.");
                HexEditShellCommon.HexEdit_FileBytesOrig = FileBytes;
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Saving file failed: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
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
                    DebugWriter.WriteDebugStackTrace(ex);
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
                Array.Resize(ref HexEditShellCommon.HexEdit_FileBytes, HexEditShellCommon.HexEdit_FileBytes.Length + 1);
                HexEditShellCommon.HexEdit_FileBytes[^1] = Content;
            }
            else
            {
                throw new KernelException(KernelExceptionType.HexEditor, Translate.DoTranslation("The hex editor hasn't opened a file stream yet."));
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
                    HexEdit_AddNewByte(ByteContent);
            }
            else
            {
                throw new KernelException(KernelExceptionType.HexEditor, Translate.DoTranslation("The hex editor hasn't opened a file stream yet."));
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
                if (ByteNumber < 1)
                    throw new KernelException(KernelExceptionType.HexEditor, Translate.DoTranslation("Byte number must start with 1."));
                var FileBytesList = HexEditShellCommon.HexEdit_FileBytes.ToList();
                long ByteIndex = ByteNumber - 1L;
                DebugWriter.WriteDebug(DebugLevel.I, "Byte index: {0}, number: {1}", ByteIndex, ByteNumber);
                DebugWriter.WriteDebug(DebugLevel.I, "File length: {0}", HexEditShellCommon.HexEdit_FileBytes.LongLength);

                // Actually remove a byte
                if (ByteNumber <= HexEditShellCommon.HexEdit_FileBytes.LongLength)
                {
                    FileBytesList.RemoveAt((int)ByteIndex);
                    DebugWriter.WriteDebug(DebugLevel.I, "Removed {0}. Result: {1}", ByteIndex, HexEditShellCommon.HexEdit_FileBytes.LongLength);
                    HexEditShellCommon.HexEdit_FileBytes = FileBytesList.ToArray();
                }
                else
                {
                    throw new KernelException(KernelExceptionType.HexEditor, Translate.DoTranslation("The specified byte number may not be larger than the file size."));
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.HexEditor, Translate.DoTranslation("The hex editor hasn't opened a file stream yet."));
            }
        }

        /// <summary>
        /// Deletes the bytes
        /// </summary>
        /// <param name="StartByteNumber">Start from the byte number</param>
        public static void HexEdit_DeleteBytes(long StartByteNumber) => HexEdit_DeleteBytes(StartByteNumber, HexEditShellCommon.HexEdit_FileBytes.LongLength);

        /// <summary>
        /// Deletes the bytes
        /// </summary>
        /// <param name="StartByteNumber">Start from the byte number</param>
        /// <param name="EndByteNumber">Ending byte number</param>
        public static void HexEdit_DeleteBytes(long StartByteNumber, long EndByteNumber)
        {
            if (HexEditShellCommon.HexEdit_FileStream is not null)
            {
                if (StartByteNumber < 1)
                    throw new KernelException(KernelExceptionType.HexEditor, Translate.DoTranslation("Byte number must start with 1."));
                StartByteNumber.SwapIfSourceLarger(ref EndByteNumber);
                long StartByteNumberIndex = StartByteNumber - 1L;
                long EndByteNumberIndex = EndByteNumber - 1L;
                var FileBytesList = HexEditShellCommon.HexEdit_FileBytes.ToList();
                DebugWriter.WriteDebug(DebugLevel.I, "Start byte number: {0}, end: {1}", StartByteNumber, EndByteNumber);
                DebugWriter.WriteDebug(DebugLevel.I, "Got start byte index: {0}", StartByteNumberIndex);
                DebugWriter.WriteDebug(DebugLevel.I, "Got end byte index: {0}", EndByteNumberIndex);
                DebugWriter.WriteDebug(DebugLevel.I, "File length: {0}", HexEditShellCommon.HexEdit_FileBytes.LongLength);

                // Actually remove the bytes
                if (StartByteNumber <= HexEditShellCommon.HexEdit_FileBytes.LongLength & EndByteNumber <= HexEditShellCommon.HexEdit_FileBytes.LongLength)
                {
                    for (long ByteNumber = EndByteNumber; ByteNumber >= StartByteNumber; ByteNumber -= 1)
                        FileBytesList.RemoveAt((int)(ByteNumber - 1L));
                    DebugWriter.WriteDebug(DebugLevel.I, "Removed {0} to {1}. New length: {2}", StartByteNumber, EndByteNumber, HexEditShellCommon.HexEdit_FileBytes.LongLength);
                    HexEditShellCommon.HexEdit_FileBytes = FileBytesList.ToArray();
                }
                else if (StartByteNumber > HexEditShellCommon.HexEdit_FileBytes.LongLength)
                {
                    throw new KernelException(KernelExceptionType.HexEditor, Translate.DoTranslation("The specified start byte number may not be larger than the file size."));
                }
                else if (EndByteNumber > HexEditShellCommon.HexEdit_FileBytes.LongLength)
                {
                    throw new KernelException(KernelExceptionType.HexEditor, Translate.DoTranslation("The specified end byte number may not be larger than the file size."));
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.HexEditor, Translate.DoTranslation("The hex editor hasn't opened a file stream yet."));
            }
        }

        /// <summary>
        /// Renders the file in hex
        /// </summary>
        public static void HexEdit_DisplayHex() =>
            HexEdit_DisplayHex(1L, HexEditShellCommon.HexEdit_FileBytes.LongLength);

        /// <summary>
        /// Renders the file in hex
        /// </summary>
        public static void HexEdit_DisplayHex(long Start) =>
            HexEdit_DisplayHex(Start, HexEditShellCommon.HexEdit_FileBytes.LongLength);

        /// <summary>
        /// Renders the file in hex
        /// </summary>
        public static void HexEdit_DisplayHex(long StartByte, long EndByte)
        {
            if (HexEditShellCommon.HexEdit_FileStream is not null)
            {
                if (StartByte < 1)
                    throw new KernelException(KernelExceptionType.HexEditor, Translate.DoTranslation("Byte number must start with 1."));
                FileContentPrinter.DisplayInHex(StartByte, EndByte, HexEditShellCommon.HexEdit_FileBytes);
            }
            else
                throw new KernelException(KernelExceptionType.HexEditor, Translate.DoTranslation("The hex editor hasn't opened a file stream yet."));
        }

        /// <summary>
        /// Queries the byte and displays the results
        /// </summary>
        public static void HexEdit_QueryByteAndDisplay(byte ByteContent) =>
            HexEdit_QueryByteAndDisplay(ByteContent, 1L, HexEditShellCommon.HexEdit_FileBytes.LongLength);

        /// <summary>
        /// Queries the byte and displays the results
        /// </summary>
        public static void HexEdit_QueryByteAndDisplay(byte ByteContent, long Start) =>
            HexEdit_QueryByteAndDisplay(ByteContent, Start, HexEditShellCommon.HexEdit_FileBytes.LongLength);

        /// <summary>
        /// Queries the byte and displays the results
        /// </summary>
        public static void HexEdit_QueryByteAndDisplay(byte ByteContent, long StartByte, long EndByte)
        {
            if (HexEditShellCommon.HexEdit_FileStream is not null)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "File Bytes: {0}", HexEditShellCommon.HexEdit_FileBytes.LongLength);
                if (StartByte < 1)
                    throw new KernelException(KernelExceptionType.HexEditor, Translate.DoTranslation("Byte number must start with 1."));
                if (StartByte <= HexEditShellCommon.HexEdit_FileBytes.LongLength & EndByte <= HexEditShellCommon.HexEdit_FileBytes.LongLength)
                {
                    DriverHandler.CurrentFilesystemDriverLocal.DisplayInHex(ByteContent, true, StartByte, EndByte, HexEditShellCommon.HexEdit_FileBytes);
                }
                else if (StartByte > HexEditShellCommon.HexEdit_FileBytes.LongLength)
                {
                    TextWriterColor.Write(Translate.DoTranslation("The specified start byte number may not be larger than the file size."), true, KernelColorType.Error);
                }
                else if (EndByte > HexEditShellCommon.HexEdit_FileBytes.LongLength)
                {
                    TextWriterColor.Write(Translate.DoTranslation("The specified end byte number may not be larger than the file size."), true, KernelColorType.Error);
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.HexEditor, Translate.DoTranslation("The hex editor hasn't opened a file stream yet."));
            }
        }

        /// <summary>
        /// Replaces every occurence of a byte with the replacement
        /// </summary>
        /// <param name="FromByte">Byte to be replaced</param>
        /// <param name="WithByte">Byte to replace with</param>
        public static void HexEdit_Replace(byte FromByte, byte WithByte) => HexEdit_Replace(FromByte, WithByte, 1L, HexEditShellCommon.HexEdit_FileBytes.LongLength);

        /// <summary>
        /// Replaces every occurence of a byte with the replacement
        /// </summary>
        /// <param name="FromByte">Byte to be replaced</param>
        /// <param name="WithByte">Byte to replace with</param>
        /// <param name="Start">Start byte number</param>
        public static void HexEdit_Replace(byte FromByte, byte WithByte, long Start) => HexEdit_Replace(FromByte, WithByte, Start, HexEditShellCommon.HexEdit_FileBytes.LongLength);

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
                if (StartByte < 1)
                    throw new KernelException(KernelExceptionType.HexEditor, Translate.DoTranslation("Byte number must start with 1."));
                DebugWriter.WriteDebug(DebugLevel.I, "Source: {0}, Target: {1}", FromByte, WithByte);
                DebugWriter.WriteDebug(DebugLevel.I, "File Bytes: {0}", HexEditShellCommon.HexEdit_FileBytes.LongLength);
                if (StartByte <= HexEditShellCommon.HexEdit_FileBytes.LongLength & EndByte <= HexEditShellCommon.HexEdit_FileBytes.LongLength)
                {
                    for (long ByteNumber = StartByte; ByteNumber <= EndByte; ByteNumber++)
                    {
                        if (HexEditShellCommon.HexEdit_FileBytes[(int)(ByteNumber - 1L)] == FromByte)
                        {
                            DebugWriter.WriteDebug(DebugLevel.I, "Replacing \"{0}\" with \"{1}\" in byte {2}", FromByte, WithByte, ByteNumber);
                            HexEditShellCommon.HexEdit_FileBytes[(int)(ByteNumber - 1L)] = WithByte;
                        }
                    }
                }
                else if (StartByte > HexEditShellCommon.HexEdit_FileBytes.LongLength)
                {
                    TextWriterColor.Write(Translate.DoTranslation("The specified start byte number may not be larger than the file size."), true, KernelColorType.Error);
                }
                else if (EndByte > HexEditShellCommon.HexEdit_FileBytes.LongLength)
                {
                    TextWriterColor.Write(Translate.DoTranslation("The specified end byte number may not be larger than the file size."), true, KernelColorType.Error);
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.HexEditor, Translate.DoTranslation("The hex editor hasn't opened a file stream yet."));
            }
        }

    }
}
