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

using Nitrocid.Base.Drivers;
using System.IO;
using System.Text.RegularExpressions;

namespace Nitrocid.Base.Drivers.Encryption
{
    /// <summary>
    /// Base encryptor interface for an encryption algorithm
    /// </summary>
    public interface IEncryptionDriver : IDriver
    {
        /// <summary>
        /// Gets empty hash
        /// </summary>
        /// <returns>Empty hash</returns>
        abstract string EmptyHash { get; }

        /// <summary>
        /// The expected hash length
        /// </summary>
        abstract int HashLength { get; }

        /// <summary>
        /// Regular expression to match hashes
        /// </summary>
        abstract Regex HashRegex { get; }

        /// <summary>
        /// Encrypts a string
        /// </summary>
        /// <param name="str">Source string</param>
        /// <returns>Encrypted hash sum of a string</returns>
        abstract string GetEncryptedString(string str);

        /// <summary>
        /// Encrypts a file
        /// </summary>
        /// <param name="stream">Source stream</param>
        /// <returns>Encrypted hash sum of a file</returns>
        abstract string GetEncryptedFile(Stream stream);

        /// <summary>
        /// Encrypts a file
        /// </summary>
        /// <param name="Path">Relative path</param>
        /// <returns>Encrypted hash sum of a file</returns>
        abstract string GetEncryptedFile(string Path);

        /// <summary>
        /// Verifies the hash sum of a file from expected hash
        /// </summary>
        /// <param name="FileName">Target file</param>
        /// <param name="ExpectedHash">Expected hash of a target file</param>
        /// <param name="ActualHash">Actual hash calculated from hash tool</param>
        /// <returns>True if they match; else, false.</returns>
        abstract bool VerifyHashFromHash(string FileName, string ExpectedHash, string ActualHash);

        /// <summary>
        /// Verifies the hash sum of a file from hashes file
        /// </summary>
        /// <param name="FileName">Target file</param>
        /// <param name="HashesFile">Hashes file that contains the target file</param>
        /// <param name="ActualHash">Actual hash calculated from hash tool</param>
        /// <returns>True if they match; else, false.</returns>
        abstract bool VerifyHashFromHashesFile(string FileName, string HashesFile, string ActualHash);

        /// <summary>
        /// Verifies the hash sum of a file from expected hash once the file's hash is calculated.
        /// </summary>
        /// <param name="FileName">Target file</param>
        /// <param name="ExpectedHash">Expected hash of a target file</param>
        /// <returns>True if they match; else, false.</returns>
        abstract bool VerifyUncalculatedHashFromHash(string FileName, string ExpectedHash);

        /// <summary>
        /// Verifies the hash sum of a file from hashes file once the file's hash is calculated.
        /// </summary>
        /// <param name="FileName">Target file</param>
        /// <param name="HashesFile">Hashes file that contains the target file</param>
        /// <returns>True if they match; else, false.</returns>
        abstract bool VerifyUncalculatedHashFromHashesFile(string FileName, string HashesFile);
    }
}
