//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
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
using Nitrocid.Base.Drivers;
using Nitrocid.Base.Files;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Reflection;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Base.Shell.Shells.Hex
{
    /// <summary>
    /// The hex editor class
    /// </summary>
    public partial class HexShell : BaseShell, IShell
    {
        /// <summary>
        /// Opens the binary file
        /// </summary>
        /// <param name="File">Target file. We recommend you to use <see cref="FilesystemTools.NeutralizePath(string, bool)"></see> to neutralize path.</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public bool OpenBinaryFile(string File)
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Trying to open file {0}...", vars: [File]);
                FileStream = new FileStream(File, FileMode.Open);
                DebugWriter.WriteDebug(DebugLevel.I, "File {0} is open. Length: {1}, Pos: {2}", vars: [File, FileStream.Length, FileStream.Position]);

                // Read the file
                var FileBytes = new byte[(int)(FileStream.Length + 1)];
                FileStream.ReadExactly(FileBytes, 0, (int)FileStream.Length);
                FileStream.Seek(0L, SeekOrigin.Begin);

                // Add the information to the arrays
                this.FileBytes = FileBytes;
                FileBytesOrig = FileBytes;
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
        public bool CloseBinaryFile()
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Trying to close file...");
                FileStream?.Close();
                FileStream = null;
                DebugWriter.WriteDebug(DebugLevel.I, "File is no longer open.");
                FileBytes = [];
                FileBytesOrig = [];
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
        public bool SaveBinaryFile()
        {
            try
            {
                if (FileStream is null)
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_NOTOPENYET"));
                var FileBytes = this.FileBytes ??
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_NOTOPENYET"));
                DebugWriter.WriteDebug(DebugLevel.I, "Trying to save file...");
                FileStream.SetLength(0L);
                DebugWriter.WriteDebug(DebugLevel.I, "Length set to 0.");
                FileStream.Write(FileBytes, 0, FileBytes.Length);
                FileStream.Flush();
                DebugWriter.WriteDebug(DebugLevel.I, "File is saved.");
                FileBytesOrig = FileBytes;
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
        /// Was binary edited?
        /// </summary>
        public bool WasHexEdited()
        {
            if (FileBytes is not null && FileBytesOrig is not null)
                return !FileBytes.SequenceEqual(FileBytesOrig);
            return false;
        }

        /// <summary>
        /// Adds a new byte to the current hex
        /// </summary>
        /// <param name="Content">New byte content</param>
        public void AddNewByte(byte Content)
        {
            if (FileStream is not null && FileBytes is not null)
            {
                Array.Resize(ref FileBytes, FileBytes.Length + 1);
                FileBytes[^1] = Content;
            }
            else
                throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_FILESTREAMNOTOPENYET"));
        }

        /// <summary>
        /// Adds a new byte to the current hex
        /// </summary>
        /// <param name="Content">New byte content</param>
        /// <param name="pos">Position to insert a new byte to</param>
        public void AddNewByte(byte Content, long pos)
        {
            if (FileStream is not null)
            {
                // Check the position
                if (FileBytes is null)
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_NOTOPENYET"));
                if (pos < 1 || pos > FileBytes.Length)
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_BYTENUMTOOLARGE"));

                var FileBytesList = FileBytes.ToList();
                long ByteIndex = pos - 1L;
                DebugWriter.WriteDebug(DebugLevel.I, "Byte index: {0}, number: {1}", vars: [ByteIndex, pos]);
                DebugWriter.WriteDebug(DebugLevel.I, "File length: {0}", vars: [FileBytes.LongLength]);

                // Actually remove a byte
                if (pos <= FileBytes.LongLength)
                {
                    FileBytesList.Insert((int)ByteIndex, Content);
                    DebugWriter.WriteDebug(DebugLevel.I, "Inserted {0}. Result: {1}", vars: [ByteIndex, FileBytes.LongLength]);
                    FileBytes = [.. FileBytesList];
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
        public void AddNewBytes(byte[] Bytes)
        {
            if (FileStream is not null)
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
        public void DeleteByte(long ByteNumber)
        {
            if (FileStream is not null)
            {
                if (FileBytes is null)
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_NOTOPENYET"));
                if (ByteNumber < 1)
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_BYTENUMISZERO"));
                var FileBytesList = FileBytes.ToList();
                long ByteIndex = ByteNumber - 1L;
                DebugWriter.WriteDebug(DebugLevel.I, "Byte index: {0}, number: {1}", vars: [ByteIndex, ByteNumber]);
                DebugWriter.WriteDebug(DebugLevel.I, "File length: {0}", vars: [FileBytes.LongLength]);

                // Actually remove a byte
                if (ByteNumber <= FileBytes.LongLength)
                {
                    FileBytesList.RemoveAt((int)ByteIndex);
                    DebugWriter.WriteDebug(DebugLevel.I, "Removed {0}. Result: {1}", vars: [ByteIndex, FileBytes.LongLength]);
                    FileBytes = [.. FileBytesList];
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
        public void DeleteBytes(long StartByteNumber)
        {
            if (FileBytes is null)
                throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_NOTOPENYET"));
            DeleteBytes(StartByteNumber, FileBytes.LongLength);
        }

        /// <summary>
        /// Deletes the bytes
        /// </summary>
        /// <param name="StartByteNumber">Start from the byte number</param>
        /// <param name="EndByteNumber">Ending byte number</param>
        public void DeleteBytes(long StartByteNumber, long EndByteNumber)
        {
            if (FileStream is not null)
            {
                if (FileBytes is null)
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_NOTOPENYET"));
                if (StartByteNumber < 1)
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_BYTENUMISZERO"));
                StartByteNumber.SwapIfSourceLarger(ref EndByteNumber);
                long StartByteNumberIndex = StartByteNumber - 1L;
                long EndByteNumberIndex = EndByteNumber - 1L;
                var FileBytesList = FileBytes.ToList();
                DebugWriter.WriteDebug(DebugLevel.I, "Start byte number: {0}, end: {1}", vars: [StartByteNumber, EndByteNumber]);
                DebugWriter.WriteDebug(DebugLevel.I, "Got start byte index: {0}", vars: [StartByteNumberIndex]);
                DebugWriter.WriteDebug(DebugLevel.I, "Got end byte index: {0}", vars: [EndByteNumberIndex]);
                DebugWriter.WriteDebug(DebugLevel.I, "File length: {0}", vars: [FileBytes.LongLength]);

                // Actually remove the bytes
                if (StartByteNumber <= FileBytes.LongLength & EndByteNumber <= FileBytes.LongLength)
                {
                    for (long ByteNumber = EndByteNumber; ByteNumber >= StartByteNumber; ByteNumber -= 1)
                        FileBytesList.RemoveAt((int)(ByteNumber - 1L));
                    DebugWriter.WriteDebug(DebugLevel.I, "Removed {0} to {1}. New length: {2}", vars: [StartByteNumber, EndByteNumber, FileBytes.LongLength]);
                    FileBytes = [.. FileBytesList];
                }
                else if (StartByteNumber > FileBytes.LongLength)
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_STARTBYTENUMTOOLARGE"));
                else if (EndByteNumber > FileBytes.LongLength)
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_ENDBYTENUMTOOLARGE"));
            }
            else
                throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_FILESTREAMNOTOPENYET"));
        }

        /// <summary>
        /// Renders the file in hex
        /// </summary>
        public void DisplayHex()
        {
            if (FileBytes is null)
                throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_NOTOPENYET"));
            DisplayHex(1L, FileBytes.LongLength);
        }

        /// <summary>
        /// Renders the file in hex
        /// </summary>
        public void DisplayHex(long Start)
        {
            if (FileBytes is null)
                throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_NOTOPENYET"));
            DisplayHex(Start, FileBytes.LongLength);
        }

        /// <summary>
        /// Renders the file in hex
        /// </summary>
        public void DisplayHex(long StartByte, long EndByte)
        {
            if (FileStream is not null)
            {
                if (FileBytes is null)
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_NOTOPENYET"));
                if (StartByte < 1)
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_BYTENUMISZERO"));
                FilesystemTools.DisplayInHex(StartByte, EndByte, FileBytes);
            }
            else
                throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_FILESTREAMNOTOPENYET"));
        }

        /// <summary>
        /// Queries the byte and displays the results
        /// </summary>
        public void QueryByteAndDisplay(byte ByteContent)
        {
            if (FileBytes is null)
                throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_NOTOPENYET"));
            QueryByteAndDisplay(ByteContent, 1L, FileBytes.LongLength);
        }

        /// <summary>
        /// Queries the byte and displays the results
        /// </summary>
        public void QueryByteAndDisplay(byte ByteContent, long Start)
        {
            if (FileBytes is null)
                throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_NOTOPENYET"));
            QueryByteAndDisplay(ByteContent, Start, FileBytes.LongLength);
        }

        /// <summary>
        /// Queries the byte and displays the results
        /// </summary>
        public void QueryByteAndDisplay(byte ByteContent, long StartByte, long EndByte)
        {
            if (FileStream is not null)
            {
                if (FileBytes is null)
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_NOTOPENYET"));
                DebugWriter.WriteDebug(DebugLevel.I, "File Bytes: {0}", vars: [FileBytes.LongLength]);
                if (StartByte < 1)
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_BYTENUMISZERO"));
                if (StartByte <= FileBytes.LongLength & EndByte <= FileBytes.LongLength)
                {
                    DriverHandler.CurrentFilesystemDriverLocal.DisplayInHex(ByteContent, true, StartByte, EndByte, FileBytes);
                }
                else if (StartByte > FileBytes.LongLength)
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_STARTBYTENUMTOOLARGE"), true, ThemeColorType.Error);
                }
                else if (EndByte > FileBytes.LongLength)
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
        public void Replace(byte FromByte, byte WithByte)
        {
            if (FileBytes is null)
                throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_NOTOPENYET"));
            Replace(FromByte, WithByte, 1L, FileBytes.LongLength);
        }

        /// <summary>
        /// Replaces every occurrence of a byte with the replacement
        /// </summary>
        /// <param name="FromByte">Byte to be replaced</param>
        /// <param name="WithByte">Byte to replace with</param>
        /// <param name="Start">Start byte number</param>
        public void Replace(byte FromByte, byte WithByte, long Start)
        {
            if (FileBytes is null)
                throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_NOTOPENYET"));
            Replace(FromByte, WithByte, Start, FileBytes.LongLength);
        }

        /// <summary>
        /// Replaces every occurrence of a byte with the replacement
        /// </summary>
        /// <param name="FromByte">Byte to be replaced</param>
        /// <param name="WithByte">Byte to replace with</param>
        /// <param name="StartByte">Start byte number</param>
        /// <param name="EndByte">End byte number</param>
        public void Replace(byte FromByte, byte WithByte, long StartByte, long EndByte)
        {
            if (FileStream is not null)
            {
                if (FileBytes is null)
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_NOTOPENYET"));
                if (StartByte < 1)
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_DRIVERS_FILESYSTEM_BASE_EXCEPTION_BYTENUMISZERO"));
                DebugWriter.WriteDebug(DebugLevel.I, "Source: {0}, Target: {1}", vars: [FromByte, WithByte]);
                DebugWriter.WriteDebug(DebugLevel.I, "File Bytes: {0}", vars: [FileBytes.LongLength]);
                if (StartByte <= FileBytes.LongLength & EndByte <= FileBytes.LongLength)
                {
                    for (long ByteNumber = StartByte; ByteNumber <= EndByte; ByteNumber++)
                    {
                        if (FileBytes[(int)(ByteNumber - 1L)] == FromByte)
                        {
                            DebugWriter.WriteDebug(DebugLevel.I, "Replacing \"{0}\" with \"{1}\" in byte {2}", vars: [FromByte, WithByte, ByteNumber]);
                            FileBytes[(int)(ByteNumber - 1L)] = WithByte;
                        }
                    }
                }
                else if (StartByte > FileBytes.LongLength)
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_STARTBYTENUMTOOLARGE"), true, ThemeColorType.Error);
                else if (EndByte > FileBytes.LongLength)
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_ENDBYTENUMTOOLARGE"), true, ThemeColorType.Error);
            }
            else
                throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_FILES_EDITORS_HEXEDITOR_EXCEPTION_FILESTREAMNOTOPENYET"));
        }

        private static void HandleAutoSaveBinaryFile(HexShell? shell)
        {
            if (shell is null)
                throw new KernelException(KernelExceptionType.Archive, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_LASTSHELLTYPEMISMATCH"));
            if (Config.MainConfig.HexEditAutoSaveFlag)
            {
                try
                {
                    Thread.Sleep(Config.MainConfig.HexEditAutoSaveInterval * 1000);
                    if (shell.FileStream is not null)
                        shell.SaveBinaryFile();
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                }
            }
        }
    }
}
