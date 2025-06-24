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

using System.IO;
using Nitrocid.Base.Kernel.Debugging;
using FS = Nitrocid.Base.Files.FilesystemTools;

namespace Nitrocid.Base.Drivers.Encryption
{
    /// <summary>
    /// Encryption and hashing module
    /// </summary>
    public static class Encryption
    {

        /// <summary>
        /// Translates the array of encrypted bytes to string
        /// </summary>
        /// <param name="encrypted">Array of encrypted bytes</param>
        /// <returns>A string representation of hash sum</returns>
        public static string GetArrayEnc(byte[] encrypted)
        {
            string hash = "";
            for (int i = 0; i <= encrypted.Length - 1; i++)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Appending {0} to hash", vars: [encrypted[i]]);
                hash += $"{encrypted[i]:X2}";
            }
            DebugWriter.WriteDebug(DebugLevel.I, "Final hash: {0}", vars: [hash]);
            return hash;
        }

        /// <summary>
        /// Encrypts a string
        /// </summary>
        /// <param name="str">Source string</param>
        /// <param name="algorithm">Algorithm</param>
        /// <returns>Encrypted hash sum</returns>
        public static string GetEncryptedString(string str, string algorithm)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Selected algorithm: {0}", vars: [algorithm]);
            DebugWriter.WriteDebug(DebugLevel.I, "String length: {0}", vars: [str.Length]);

            // Get the encryptor
            return DriverHandler.GetDriver<IEncryptionDriver>(algorithm).GetEncryptedString(str);
        }

        /// <summary>
        /// Encrypts a file
        /// </summary>
        /// <param name="str">Source stream</param>
        /// <param name="algorithm">Algorithm</param>
        /// <returns>Encrypted hash sum</returns>
        public static string GetEncryptedFile(Stream str, string algorithm)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Selected algorithm: {0}", vars: [algorithm]);
            DebugWriter.WriteDebug(DebugLevel.I, "Stream length: {0}", vars: [str.Length]);

            // Get the encryptor
            return DriverHandler.GetDriver<IEncryptionDriver>(algorithm).GetEncryptedFile(str);
        }

        /// <summary>
        /// Encrypts a file
        /// </summary>
        /// <param name="Path">Relative path</param>
        /// <param name="algorithm">Algorithm</param>
        /// <returns>Encrypted hash sum</returns>
        public static string GetEncryptedFile(string Path, string algorithm)
        {
            Path = FS.NeutralizePath(Path);
            var Str = new FileStream(Path, FileMode.Open);
            string Encrypted = GetEncryptedFile(Str, algorithm);
            Str.Close();
            return Encrypted;
        }

        /// <summary>
        /// Gets empty hash
        /// </summary>
        /// <param name="Algorithm">Algorithm</param>
        /// <returns>Empty hash</returns>
        public static string GetEmptyHash(string Algorithm) =>
            DriverHandler.GetDriver<IEncryptionDriver>(Algorithm).EmptyHash;

    }
}
