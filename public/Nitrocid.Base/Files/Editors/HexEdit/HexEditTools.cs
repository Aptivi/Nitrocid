//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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

using System;
using System.IO;
using System.Linq;
using System.Threading;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Misc.Reflection;
using Nitrocid.Base.Drivers;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Shell.Shells.Hex;
using Nitrocid.Base.Kernel.Exceptions;

namespace Nitrocid.Base.Files.Editors.HexEdit
{
    /// <summary>
    /// Hex editor tools module
    /// </summary>
    public static class HexEditTools
    {

        /// <summary>
        /// Opens the binary file
        /// </summary>
        /// <param name="File">Target file. We recommend you to use <see cref="FilesystemTools.NeutralizePath(string, bool)"></see> to neutralize path.</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool OpenBinaryFile(string File)
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Trying to open file {0}...", vars: [File]);
                HexEditShellCommon.FileStream = new FileStream(File, FileMode.Open);
                DebugWriter.WriteDebug(DebugLevel.I, "File {0} is open. Length: {1}, Pos: {2}", vars: [File, HexEditShellCommon.FileStream.Length, HexEditShellCommon.FileStream.Position]);

                // Read the file
                var FileBytes = new byte[(int)(HexEditShellCommon.FileStream.Length + 1)];
                HexEditShellCommon.FileStream.Read(FileBytes, 0, (int)HexEditShellCommon.FileStream.Length);
                HexEditShellCommon.FileStream.Seek(0L, SeekOrigin.Begin);

                // Add the information to the arrays
                HexEditShellCommon.FileBytes = FileBytes;
                HexEditShellCommon.FileBytesOrig = FileBytes;
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Open file {0} failed: {1}", vars: [File, ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                return false;
            }
        }

        /// <summary>
        /// Closes binary file
        /// </summary>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool CloseBinaryFile()
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Trying to close file...");
                HexEditShellCommon.FileStream?.Close();
                HexEditShellCommon.FileStream = null;
                DebugWriter.WriteDebug(DebugLevel.I, "File is no longer open.");
                HexEditShellCommon.FileBytes = [];
                HexEditShellCommon.FileBytesOrig = [];
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Closing file failed: {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                return false;
            }
        }

        /// <summary>
        /// Saves binary file
        /// </summary>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool SaveBinaryFile()
        {
            try
            {
                if (HexEditShellCommon.FileStream is null)
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_NOTOPENYET"));
                var FileBytes = HexEditShellCommon.FileBytes ??
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_NOTOPENYET"));
                DebugWriter.WriteDebug(DebugLevel.I, "Trying to save file...");
                HexEditShellCommon.FileStream.SetLength(0L);
                DebugWriter.WriteDebug(DebugLevel.I, "Length set to 0.");
                HexEditShellCommon.FileStream.Write(FileBytes, 0, FileBytes.Length);
                HexEditShellCommon.FileStream.Flush();
                DebugWriter.WriteDebug(DebugLevel.I, "File is saved.");
                HexEditShellCommon.FileBytesOrig = FileBytes;
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Saving file failed: {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                return false;
            }
        }

        /// <summary>
        /// Handles autosave
        /// </summary>
        public static void HandleAutoSaveBinaryFile()
        {
            if (Config.MainConfig.HexEditAutoSaveFlag)
            {
                try
                {
                    Thread.Sleep(Config.MainConfig.HexEditAutoSaveInterval * 1000);
                    if (HexEditShellCommon.FileStream is not null)
                        SaveBinaryFile();
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
        public static bool WasHexEdited()
        {
            if (HexEditShellCommon.FileBytes is not null && HexEditShellCommon.FileBytesOrig is not null)
                return !HexEditShellCommon.FileBytes.SequenceEqual(HexEditShellCommon.FileBytesOrig);
            return false;
        }

        /// <summary>
        /// Adds a new byte to the current hex
        /// </summary>
        /// <param name="Content">New byte content</param>
        public static void AddNewByte(byte Content)
        {
            if (HexEditShellCommon.FileStream is not null && HexEditShellCommon.FileBytes is not null)
            {
                Array.Resize(ref HexEditShellCommon.FileBytes, HexEditShellCommon.FileBytes.Length + 1);
                HexEditShellCommon.FileBytes[^1] = Content;
            }
            else
                throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_FILESTREAMNOTOPENYET"));
        }

        /// <summary>
        /// Adds a new byte to the current hex
        /// </summary>
        /// <param name="Content">New byte content</param>
        /// <param name="pos">Position to insert a new byte to</param>
        public static void AddNewByte(byte Content, long pos)
        {
            if (HexEditShellCommon.FileStream is not null)
            {
                // Check the position
                if (HexEditShellCommon.FileBytes is null)
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_NOTOPENYET"));
                if (pos < 1 || pos > HexEditShellCommon.FileBytes.Length)
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_BYTENUMTOOLARGE"));

                var FileBytesList = HexEditShellCommon.FileBytes.ToList();
                long ByteIndex = pos - 1L;
                DebugWriter.WriteDebug(DebugLevel.I, "Byte index: {0}, number: {1}", vars: [ByteIndex, pos]);
                DebugWriter.WriteDebug(DebugLevel.I, "File length: {0}", vars: [HexEditShellCommon.FileBytes.LongLength]);

                // Actually remove a byte
                if (pos <= HexEditShellCommon.FileBytes.LongLength)
                {
                    FileBytesList.Insert((int)ByteIndex, Content);
                    DebugWriter.WriteDebug(DebugLevel.I, "Inserted {0}. Result: {1}", vars: [ByteIndex, HexEditShellCommon.FileBytes.LongLength]);
                    HexEditShellCommon.FileBytes = [.. FileBytesList];
                }
                else
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_BYTENUMTOOLARGE"));
            }
            else
                throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_FILESTREAMNOTOPENYET"));
        }

        /// <summary>
        /// Adds the new bytes to the current hex
        /// </summary>
        /// <param name="Bytes">New bytes</param>
        public static void AddNewBytes(byte[] Bytes)
        {
            if (HexEditShellCommon.FileStream is not null)
            {
                foreach (byte ByteContent in Bytes)
                    AddNewByte(ByteContent);
            }
            else
                throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_FILESTREAMNOTOPENYET"));
        }

        /// <summary>
        /// Deletes a byte
        /// </summary>
        /// <param name="ByteNumber">The byte number</param>
        public static void DeleteByte(long ByteNumber)
        {
            if (HexEditShellCommon.FileStream is not null)
            {
                if (HexEditShellCommon.FileBytes is null)
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_NOTOPENYET"));
                if (ByteNumber < 1)
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_BYTENUMISZERO"));
                var FileBytesList = HexEditShellCommon.FileBytes.ToList();
                long ByteIndex = ByteNumber - 1L;
                DebugWriter.WriteDebug(DebugLevel.I, "Byte index: {0}, number: {1}", vars: [ByteIndex, ByteNumber]);
                DebugWriter.WriteDebug(DebugLevel.I, "File length: {0}", vars: [HexEditShellCommon.FileBytes.LongLength]);

                // Actually remove a byte
                if (ByteNumber <= HexEditShellCommon.FileBytes.LongLength)
                {
                    FileBytesList.RemoveAt((int)ByteIndex);
                    DebugWriter.WriteDebug(DebugLevel.I, "Removed {0}. Result: {1}", vars: [ByteIndex, HexEditShellCommon.FileBytes.LongLength]);
                    HexEditShellCommon.FileBytes = [.. FileBytesList];
                }
                else
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_BYTENUMTOOLARGE"));
            }
            else
                throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_FILESTREAMNOTOPENYET"));
        }

        /// <summary>
        /// Deletes the bytes
        /// </summary>
        /// <param name="StartByteNumber">Start from the byte number</param>
        public static void DeleteBytes(long StartByteNumber)
        {
            if (HexEditShellCommon.FileBytes is null)
                throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_NOTOPENYET"));
            DeleteBytes(StartByteNumber, HexEditShellCommon.FileBytes.LongLength);
        }

        /// <summary>
        /// Deletes the bytes
        /// </summary>
        /// <param name="StartByteNumber">Start from the byte number</param>
        /// <param name="EndByteNumber">Ending byte number</param>
        public static void DeleteBytes(long StartByteNumber, long EndByteNumber)
        {
            if (HexEditShellCommon.FileStream is not null)
            {
                if (HexEditShellCommon.FileBytes is null)
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_NOTOPENYET"));
                if (StartByteNumber < 1)
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_BYTENUMISZERO"));
                StartByteNumber.SwapIfSourceLarger(ref EndByteNumber);
                long StartByteNumberIndex = StartByteNumber - 1L;
                long EndByteNumberIndex = EndByteNumber - 1L;
                var FileBytesList = HexEditShellCommon.FileBytes.ToList();
                DebugWriter.WriteDebug(DebugLevel.I, "Start byte number: {0}, end: {1}", vars: [StartByteNumber, EndByteNumber]);
                DebugWriter.WriteDebug(DebugLevel.I, "Got start byte index: {0}", vars: [StartByteNumberIndex]);
                DebugWriter.WriteDebug(DebugLevel.I, "Got end byte index: {0}", vars: [EndByteNumberIndex]);
                DebugWriter.WriteDebug(DebugLevel.I, "File length: {0}", vars: [HexEditShellCommon.FileBytes.LongLength]);

                // Actually remove the bytes
                if (StartByteNumber <= HexEditShellCommon.FileBytes.LongLength & EndByteNumber <= HexEditShellCommon.FileBytes.LongLength)
                {
                    for (long ByteNumber = EndByteNumber; ByteNumber >= StartByteNumber; ByteNumber -= 1)
                        FileBytesList.RemoveAt((int)(ByteNumber - 1L));
                    DebugWriter.WriteDebug(DebugLevel.I, "Removed {0} to {1}. New length: {2}", vars: [StartByteNumber, EndByteNumber, HexEditShellCommon.FileBytes.LongLength]);
                    HexEditShellCommon.FileBytes = [.. FileBytesList];
                }
                else if (StartByteNumber > HexEditShellCommon.FileBytes.LongLength)
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_STARTBYTENUMTOOLARGE"));
                else if (EndByteNumber > HexEditShellCommon.FileBytes.LongLength)
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_ENDBYTENUMTOOLARGE"));
            }
            else
                throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_FILESTREAMNOTOPENYET"));
        }

        /// <summary>
        /// Renders the file in hex
        /// </summary>
        public static void DisplayHex()
        {
            if (HexEditShellCommon.FileBytes is null)
                throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_NOTOPENYET"));
            DisplayHex(1L, HexEditShellCommon.FileBytes.LongLength);
        }

        /// <summary>
        /// Renders the file in hex
        /// </summary>
        public static void DisplayHex(long Start)
        {
            if (HexEditShellCommon.FileBytes is null)
                throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_NOTOPENYET"));
            DisplayHex(Start, HexEditShellCommon.FileBytes.LongLength);
        }

        /// <summary>
        /// Renders the file in hex
        /// </summary>
        public static void DisplayHex(long StartByte, long EndByte)
        {
            if (HexEditShellCommon.FileStream is not null)
            {
                if (HexEditShellCommon.FileBytes is null)
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_NOTOPENYET"));
                if (StartByte < 1)
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_BYTENUMISZERO"));
                FilesystemTools.DisplayInHex(StartByte, EndByte, HexEditShellCommon.FileBytes);
            }
            else
                throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_FILESTREAMNOTOPENYET"));
        }

        /// <summary>
        /// Queries the byte and displays the results
        /// </summary>
        public static void QueryByteAndDisplay(byte ByteContent)
        {
            if (HexEditShellCommon.FileBytes is null)
                throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_NOTOPENYET"));
            QueryByteAndDisplay(ByteContent, 1L, HexEditShellCommon.FileBytes.LongLength);
        }

        /// <summary>
        /// Queries the byte and displays the results
        /// </summary>
        public static void QueryByteAndDisplay(byte ByteContent, long Start)
        {
            if (HexEditShellCommon.FileBytes is null)
                throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_NOTOPENYET"));
            QueryByteAndDisplay(ByteContent, Start, HexEditShellCommon.FileBytes.LongLength);
        }

        /// <summary>
        /// Queries the byte and displays the results
        /// </summary>
        public static void QueryByteAndDisplay(byte ByteContent, long StartByte, long EndByte)
        {
            if (HexEditShellCommon.FileStream is not null)
            {
                if (HexEditShellCommon.FileBytes is null)
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_NOTOPENYET"));
                DebugWriter.WriteDebug(DebugLevel.I, "File Bytes: {0}", vars: [HexEditShellCommon.FileBytes.LongLength]);
                if (StartByte < 1)
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_BYTENUMISZERO"));
                if (StartByte <= HexEditShellCommon.FileBytes.LongLength & EndByte <= HexEditShellCommon.FileBytes.LongLength)
                {
                    DriverHandler.CurrentFilesystemDriverLocal.DisplayInHex(ByteContent, true, StartByte, EndByte, HexEditShellCommon.FileBytes);
                }
                else if (StartByte > HexEditShellCommon.FileBytes.LongLength)
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_STARTBYTENUMTOOLARGE"), true, ThemeColorType.Error);
                }
                else if (EndByte > HexEditShellCommon.FileBytes.LongLength)
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_ENDBYTENUMTOOLARGE"), true, ThemeColorType.Error);
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_FILESTREAMNOTOPENYET"));
            }
        }

        /// <summary>
        /// Replaces every occurrence of a byte with the replacement
        /// </summary>
        /// <param name="FromByte">Byte to be replaced</param>
        /// <param name="WithByte">Byte to replace with</param>
        public static void Replace(byte FromByte, byte WithByte)
        {
            if (HexEditShellCommon.FileBytes is null)
                throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_NOTOPENYET"));
            Replace(FromByte, WithByte, 1L, HexEditShellCommon.FileBytes.LongLength);
        }

        /// <summary>
        /// Replaces every occurrence of a byte with the replacement
        /// </summary>
        /// <param name="FromByte">Byte to be replaced</param>
        /// <param name="WithByte">Byte to replace with</param>
        /// <param name="Start">Start byte number</param>
        public static void Replace(byte FromByte, byte WithByte, long Start)
        {
            if (HexEditShellCommon.FileBytes is null)
                throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_NOTOPENYET"));
            Replace(FromByte, WithByte, Start, HexEditShellCommon.FileBytes.LongLength);
        }

        /// <summary>
        /// Replaces every occurrence of a byte with the replacement
        /// </summary>
        /// <param name="FromByte">Byte to be replaced</param>
        /// <param name="WithByte">Byte to replace with</param>
        /// <param name="StartByte">Start byte number</param>
        /// <param name="EndByte">End byte number</param>
        public static void Replace(byte FromByte, byte WithByte, long StartByte, long EndByte)
        {
            if (HexEditShellCommon.FileStream is not null)
            {
                if (HexEditShellCommon.FileBytes is null)
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_NOTOPENYET"));
                if (StartByte < 1)
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_BYTENUMISZERO"));
                DebugWriter.WriteDebug(DebugLevel.I, "Source: {0}, Target: {1}", vars: [FromByte, WithByte]);
                DebugWriter.WriteDebug(DebugLevel.I, "File Bytes: {0}", vars: [HexEditShellCommon.FileBytes.LongLength]);
                if (StartByte <= HexEditShellCommon.FileBytes.LongLength & EndByte <= HexEditShellCommon.FileBytes.LongLength)
                {
                    for (long ByteNumber = StartByte; ByteNumber <= EndByte; ByteNumber++)
                    {
                        if (HexEditShellCommon.FileBytes[(int)(ByteNumber - 1L)] == FromByte)
                        {
                            DebugWriter.WriteDebug(DebugLevel.I, "Replacing \"{0}\" with \"{1}\" in byte {2}", vars: [FromByte, WithByte, ByteNumber]);
                            HexEditShellCommon.FileBytes[(int)(ByteNumber - 1L)] = WithByte;
                        }
                    }
                }
                else if (StartByte > HexEditShellCommon.FileBytes.LongLength)
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_STARTBYTENUMTOOLARGE"), true, ThemeColorType.Error);
                else if (EndByte > HexEditShellCommon.FileBytes.LongLength)
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_ENDBYTENUMTOOLARGE"), true, ThemeColorType.Error);
            }
            else
                throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_FILESTREAMNOTOPENYET"));
        }

        /// <summary>
        /// Adds a new byte to the current hex
        /// </summary>
        /// <param name="bytes">Target byte array</param>
        /// <param name="Content">New byte content</param>
        public static byte[] AddNewByte(byte[] bytes, byte Content)
        {
            if (bytes is not null)
            {
                Array.Resize(ref bytes, bytes.Length + 1);
                bytes[^1] = Content;
            }
            else
                throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_NOARRAY"));
            return bytes;
        }

        /// <summary>
        /// Adds a new byte to the current hex
        /// </summary>
        /// <param name="bytes">Target byte array</param>
        /// <param name="Content">New byte content</param>
        /// <param name="pos">Position to insert a new byte to</param>
        public static byte[] AddNewByte(byte[] bytes, byte Content, long pos)
        {
            if (bytes is not null)
            {
                // Check the position
                if (pos < 1 || pos > bytes.Length)
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_BYTENUMTOOLARGE"));

                var FileBytesList = bytes.ToList();
                long ByteIndex = pos - 1L;
                DebugWriter.WriteDebug(DebugLevel.I, "Byte index: {0}, number: {1}", vars: [ByteIndex, pos]);
                DebugWriter.WriteDebug(DebugLevel.I, "File length: {0}", vars: [bytes.LongLength]);

                // Actually remove a byte
                if (pos <= bytes.LongLength)
                {
                    FileBytesList.Insert((int)ByteIndex, Content);
                    DebugWriter.WriteDebug(DebugLevel.I, "Inserted {0}. Result: {1}", vars: [ByteIndex, bytes.LongLength]);
                    bytes = [.. FileBytesList];
                }
                else
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_BYTENUMTOOLARGE"));
            }
            else
                throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_NOARRAY"));
            return bytes;
        }

        /// <summary>
        /// Adds the new bytes to the current hex
        /// </summary>
        /// <param name="bytes">Target byte array</param>
        /// <param name="Bytes">New bytes</param>
        public static byte[] AddNewBytes(byte[] bytes, byte[] Bytes)
        {
            if (bytes is not null)
            {
                foreach (byte ByteContent in Bytes)
                    bytes = AddNewByte(bytes, ByteContent);
            }
            else
                throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_NOARRAY"));
            return bytes;
        }

        /// <summary>
        /// Deletes a byte
        /// </summary>
        /// <param name="bytes">Target byte array</param>
        /// <param name="ByteNumber">The byte number</param>
        public static byte[] DeleteByte(byte[] bytes, long ByteNumber)
        {
            if (bytes is not null)
            {
                if (ByteNumber < 1)
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_BYTENUMISZERO"));
                var FileBytesList = bytes.ToList();
                long ByteIndex = ByteNumber - 1L;
                DebugWriter.WriteDebug(DebugLevel.I, "Byte index: {0}, number: {1}", vars: [ByteIndex, ByteNumber]);
                DebugWriter.WriteDebug(DebugLevel.I, "File length: {0}", vars: [bytes.LongLength]);

                // Actually remove a byte
                if (ByteNumber <= bytes.LongLength)
                {
                    FileBytesList.RemoveAt((int)ByteIndex);
                    DebugWriter.WriteDebug(DebugLevel.I, "Removed {0}. Result: {1}", vars: [ByteIndex, bytes.LongLength]);
                    bytes = [.. FileBytesList];
                }
                else
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_BYTENUMTOOLARGE"));
            }
            else
                throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_NOARRAY"));
            return bytes;
        }

        /// <summary>
        /// Deletes the bytes
        /// </summary>
        /// <param name="bytes">Target byte array</param>
        /// <param name="StartByteNumber">Start from the byte number</param>
        public static byte[] DeleteBytes(byte[] bytes, long StartByteNumber) =>
            DeleteBytes(bytes, StartByteNumber, bytes.LongLength);

        /// <summary>
        /// Deletes the bytes
        /// </summary>
        /// <param name="bytes">Target byte array</param>
        /// <param name="StartByteNumber">Start from the byte number</param>
        /// <param name="EndByteNumber">Ending byte number</param>
        public static byte[] DeleteBytes(byte[] bytes, long StartByteNumber, long EndByteNumber)
        {
            if (bytes is not null)
            {
                if (StartByteNumber < 1)
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_BYTENUMISZERO"));
                StartByteNumber.SwapIfSourceLarger(ref EndByteNumber);
                long StartByteNumberIndex = StartByteNumber - 1L;
                long EndByteNumberIndex = EndByteNumber - 1L;
                var FileBytesList = bytes.ToList();
                DebugWriter.WriteDebug(DebugLevel.I, "Start byte number: {0}, end: {1}", vars: [StartByteNumber, EndByteNumber]);
                DebugWriter.WriteDebug(DebugLevel.I, "Got start byte index: {0}", vars: [StartByteNumberIndex]);
                DebugWriter.WriteDebug(DebugLevel.I, "Got end byte index: {0}", vars: [EndByteNumberIndex]);
                DebugWriter.WriteDebug(DebugLevel.I, "File length: {0}", vars: [bytes.LongLength]);

                // Actually remove the bytes
                if (StartByteNumber <= bytes.LongLength & EndByteNumber <= bytes.LongLength)
                {
                    for (long ByteNumber = EndByteNumber; ByteNumber >= StartByteNumber; ByteNumber -= 1)
                        FileBytesList.RemoveAt((int)(ByteNumber - 1L));
                    DebugWriter.WriteDebug(DebugLevel.I, "Removed {0} to {1}. New length: {2}", vars: [StartByteNumber, EndByteNumber, bytes.LongLength]);
                    bytes = [.. FileBytesList];
                }
                else if (StartByteNumber > bytes.LongLength)
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_STARTBYTENUMTOOLARGE"));
                else if (EndByteNumber > bytes.LongLength)
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_ENDBYTENUMTOOLARGE"));
            }
            else
                throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_NOARRAY"));
            return bytes;
        }

        /// <summary>
        /// Renders the file in hex
        /// </summary>
        /// <param name="bytes">Target byte array</param>
        public static void DisplayHex(byte[] bytes) =>
            DisplayHex(bytes, 1L, bytes.LongLength);

        /// <summary>
        /// Renders the file in hex
        /// </summary>
        /// <param name="bytes">Target byte array</param>
        /// <param name="Start">Start byte number</param>
        public static void DisplayHex(byte[] bytes, long Start) =>
            DisplayHex(bytes, Start, bytes.LongLength);

        /// <summary>
        /// Renders the file in hex
        /// </summary>
        /// <param name="bytes">Target byte array</param>
        /// <param name="StartByte">Start byte number</param>
        /// <param name="EndByte">End byte number</param>
        public static void DisplayHex(byte[] bytes, long StartByte, long EndByte)
        {
            if (bytes is not null)
            {
                if (StartByte < 1)
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_BYTENUMISZERO"));
                FilesystemTools.DisplayInHex(StartByte, EndByte, bytes);
            }
            else
                throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_NOARRAY"));
        }

        /// <summary>
        /// Queries the byte and displays the results
        /// </summary>
        /// <param name="bytes">Target byte array</param>
        /// <param name="ByteContent">Byte to find</param>
        public static void QueryByteAndDisplay(byte[] bytes, byte ByteContent) =>
            QueryByteAndDisplay(bytes, ByteContent, 1L, bytes.LongLength);

        /// <summary>
        /// Queries the byte and displays the results
        /// </summary>
        /// <param name="bytes">Target byte array</param>
        /// <param name="ByteContent">Byte to find</param>
        /// <param name="Start">Start byte number</param>
        public static void QueryByteAndDisplay(byte[] bytes, byte ByteContent, long Start) =>
            QueryByteAndDisplay(bytes, ByteContent, Start, bytes.LongLength);

        /// <summary>
        /// Queries the byte and displays the results
        /// </summary>
        /// <param name="bytes">Target byte array</param>
        /// <param name="ByteContent">Byte to find</param>
        /// <param name="StartByte">Start byte number</param>
        /// <param name="EndByte">End byte number</param>
        public static void QueryByteAndDisplay(byte[] bytes, byte ByteContent, long StartByte, long EndByte)
        {
            if (bytes is not null)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "File Bytes: {0}", vars: [bytes.LongLength]);
                if (StartByte < 1)
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_BYTENUMISZERO"));
                if (StartByte <= bytes.LongLength & EndByte <= bytes.LongLength)
                    DriverHandler.CurrentFilesystemDriverLocal.DisplayInHex(ByteContent, true, StartByte, EndByte, bytes);
                else if (StartByte > bytes.LongLength)
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_STARTBYTENUMTOOLARGE"), true, ThemeColorType.Error);
                else if (EndByte > bytes.LongLength)
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_ENDBYTENUMTOOLARGE"), true, ThemeColorType.Error);
            }
            else
                throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_NOARRAY"));
        }

        /// <summary>
        /// Replaces every occurrence of a byte with the replacement
        /// </summary>
        /// <param name="bytes">Target byte array</param>
        /// <param name="FromByte">Byte to be replaced</param>
        /// <param name="WithByte">Byte to replace with</param>
        public static byte[] Replace(byte[] bytes, byte FromByte, byte WithByte) =>
            Replace(bytes, FromByte, WithByte, 1L, bytes.LongLength);

        /// <summary>
        /// Replaces every occurrence of a byte with the replacement
        /// </summary>
        /// <param name="bytes">Target byte array</param>
        /// <param name="FromByte">Byte to be replaced</param>
        /// <param name="WithByte">Byte to replace with</param>
        /// <param name="Start">Start byte number</param>
        public static byte[] Replace(byte[] bytes, byte FromByte, byte WithByte, long Start) =>
            Replace(bytes, FromByte, WithByte, Start, bytes.LongLength);

        /// <summary>
        /// Replaces every occurrence of a byte with the replacement
        /// </summary>
        /// <param name="bytes">Target byte array</param>
        /// <param name="FromByte">Byte to be replaced</param>
        /// <param name="WithByte">Byte to replace with</param>
        /// <param name="StartByte">Start byte number</param>
        /// <param name="EndByte">End byte number</param>
        public static byte[] Replace(byte[] bytes, byte FromByte, byte WithByte, long StartByte, long EndByte)
        {
            if (bytes is not null)
            {
                if (StartByte < 1)
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_BYTENUMISZERO"));
                DebugWriter.WriteDebug(DebugLevel.I, "Source: {0}, Target: {1}", vars: [FromByte, WithByte]);
                DebugWriter.WriteDebug(DebugLevel.I, "File Bytes: {0}", vars: [bytes.LongLength]);
                if (StartByte <= bytes.LongLength & EndByte <= bytes.LongLength)
                {
                    for (long ByteNumber = StartByte; ByteNumber <= EndByte; ByteNumber++)
                    {
                        if (bytes[(int)(ByteNumber - 1L)] == FromByte)
                        {
                            DebugWriter.WriteDebug(DebugLevel.I, "Replacing \"{0}\" with \"{1}\" in byte {2}", vars: [FromByte, WithByte, ByteNumber]);
                            bytes[(int)(ByteNumber - 1L)] = WithByte;
                        }
                    }
                }
                else if (StartByte > bytes.LongLength)
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_STARTBYTENUMTOOLARGE"), true, ThemeColorType.Error);
                else if (EndByte > bytes.LongLength)
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_ENDBYTENUMTOOLARGE"), true, ThemeColorType.Error);
            }
            else
                throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_NOARRAY"));
            return bytes;
        }

    }
}
