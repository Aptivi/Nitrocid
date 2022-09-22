
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

using KS.Files;
using KS.Kernel.Debugging;
using System.IO;
using System.Text;
using Encryptor = System.Security.Cryptography.MD5;

namespace KS.Drivers.Encryption.Encryptors
{
    /// <summary>
    /// MD5 encryptor
    /// </summary>
    public class MD5 : IEncryptor
    {
        /// <summary>
        /// Empty hash
        /// </summary>
        public string EmptyHash => GetEncryptedString("");

        /// <inheritdoc/>
        public string GetEncryptedFile(Stream stream)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Stream length: {0}", stream.Length);
            var hashbyte = Encryptor.Create().ComputeHash(stream);
            return Encryption.GetArrayEnc(hashbyte);
        }

        /// <inheritdoc/>
        public string GetEncryptedFile(string Path)
        {
            Path = Filesystem.NeutralizePath(Path);
            var Str = new FileStream(Path, FileMode.Open);
            string Encrypted = GetEncryptedFile(Str);
            Str.Close();
            return Encrypted;
        }

        /// <inheritdoc/>
        public string GetEncryptedString(string str)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "String length: {0}", str.Length);
            var hashbyte = Encryptor.Create().ComputeHash(Encoding.UTF8.GetBytes(str));
            return Encryption.GetArrayEnc(hashbyte);
        }
    }
}
