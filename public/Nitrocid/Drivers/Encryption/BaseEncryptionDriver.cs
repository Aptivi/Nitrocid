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

using Nitrocid.Kernel.Debugging;
using System.IO;
using System.Text.RegularExpressions;
using Encryptor = System.Security.Cryptography.SHA256;
using FS = Nitrocid.Files.FilesystemTools;
using TextEncoding = System.Text.Encoding;

namespace Nitrocid.Drivers.Encryption
{
    /// <summary>
    /// SHA256 encryptor
    /// </summary>
    public abstract class BaseEncryptionDriver : IEncryptionDriver
    {
        /// <inheritdoc/>
        public virtual string DriverName => "Default";

        /// <inheritdoc/>
        public virtual DriverTypes DriverType => DriverTypes.Encryption;

        /// <inheritdoc/>
        public virtual bool DriverInternal => false;

        /// <inheritdoc/>
        public virtual string EmptyHash => GetEncryptedString("");

        /// <inheritdoc/>
        public virtual int HashLength => 64;

        /// <inheritdoc/>
        public virtual Regex HashRegex => new("^([a-fA-F0-9]{64})$");

        /// <inheritdoc/>
        public virtual string GetEncryptedFile(Stream stream)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Stream length: {0}", vars: [stream.Length]);
            var hashbyte = Encryptor.Create().ComputeHash(stream);
            return Encryption.GetArrayEnc(hashbyte);
        }

        /// <inheritdoc/>
        public virtual string GetEncryptedFile(string Path)
        {
            Path = FS.NeutralizePath(Path);
            var Str = new FileStream(Path, FileMode.Open);
            string Encrypted = GetEncryptedFile(Str);
            Str.Close();
            return Encrypted;
        }

        /// <inheritdoc/>
        public virtual string GetEncryptedString(string str)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "String length: {0}", vars: [str.Length]);
            var hashbyte = Encryptor.HashData(TextEncoding.UTF8.GetBytes(str));
            return Encryption.GetArrayEnc(hashbyte);
        }

        /// <inheritdoc/>
        public virtual bool VerifyHashFromHash(string FileName, string ExpectedHash, string ActualHash) =>
            HashVerifier.VerifyHashFromHash(FileName, DriverName, ExpectedHash, ActualHash);

        /// <inheritdoc/>
        public virtual bool VerifyHashFromHashesFile(string FileName, string HashesFile, string ActualHash) =>
            HashVerifier.VerifyHashFromHashesFile(FileName, DriverName, HashesFile, ActualHash);

        /// <inheritdoc/>
        public virtual bool VerifyUncalculatedHashFromHash(string FileName, string ExpectedHash) =>
            HashVerifier.VerifyUncalculatedHashFromHash(FileName, DriverName, ExpectedHash);

        /// <inheritdoc/>
        public virtual bool VerifyUncalculatedHashFromHashesFile(string FileName, string HashesFile) =>
            HashVerifier.VerifyUncalculatedHashFromHashesFile(FileName, DriverName, HashesFile);
    }
}
