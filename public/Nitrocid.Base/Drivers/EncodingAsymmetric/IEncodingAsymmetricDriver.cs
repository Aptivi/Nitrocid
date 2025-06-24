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

namespace Nitrocid.Base.Drivers.EncodingAsymmetric
{
    /// <summary>
    /// Base encryptor interface for an asymmetric encoding algorithm
    /// </summary>
    public interface IEncodingAsymmetricDriver : IDriver
    {
        /// <summary>
        /// An instance of the encoding class to use for encoding operations. Should be populated by Initialize.
        /// </summary>
        object? Instance { get; }

        /// <summary>
        /// Initializes encoding. Must be called once for each driver.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Gets the encoded string in the array of bytes
        /// </summary>
        /// <param name="text">Text to encode</param>
        /// <returns>Array of bytes containing encoded data</returns>
        byte[] GetEncodedString(string text);

        /// <summary>
        /// Gets the decoded string from the array of bytes
        /// </summary>
        /// <param name="encoded">Encoded text to decode</param>
        /// <returns>Resulting decoded string</returns>
        string GetDecodedString(byte[] encoded);

        /// <summary>
        /// Composes an array of bytes from the string that satisfies the following format:
        /// This string must represent the group of byte number. This is usually generated from the decomposition function.
        /// </summary>
        /// <param name="encoded">Encoded string to compose</param>
        /// <returns>Encoded byte array</returns>
        byte[] ComposeBytesFromString(string encoded);

        /// <summary>
        /// Decomposes an array of bytes to the string of byte numbers.
        /// </summary>
        /// <param name="encoded">Encoded byte array to decompose</param>
        /// <returns>Encoded string</returns>
        string DecomposeBytesFromString(byte[] encoded);

        /// <summary>
        /// Tries to represent encoded text as a real string
        /// </summary>
        /// <param name="encoded">Encoded byte array to try to represent as text</param>
        /// <param name="strEncoded">Output text, or empty if the <paramref name="encoded"/> array contains binary characters</param>
        /// <returns>True if it can be represented; false otherwise.</returns>
        bool TryRepresentAsText(byte[] encoded, out string? strEncoded);

        /// <summary>
        /// Encodes a file
        /// </summary>
        /// <param name="path">Path to the file to encode (non-neutralized)</param>
        void EncodeFile(string path);

        /// <summary>
        /// Decodes a file
        /// </summary>
        /// <param name="path">Path to the file to decode (non-neutralized)</param>
        void DecodeFile(string path);
    }
}
