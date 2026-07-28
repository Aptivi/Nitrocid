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
using System.Linq;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Misc.Reflection;
using Nitrocid.Base.Drivers;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Kernel.Exceptions;

namespace Nitrocid.Base.Files.Editors.HexEdit
{
    /// <summary>
    /// Hex editor tools module
    /// </summary>
    public static class HexEditTools
    {

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
