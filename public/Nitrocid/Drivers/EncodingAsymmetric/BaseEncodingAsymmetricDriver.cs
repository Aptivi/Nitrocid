//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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

using Nitrocid.Files.Operations;
using Nitrocid.Files.Operations.Querying;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Textify.General;
using TextEncoding = System.Text.Encoding;

namespace Nitrocid.Drivers.EncodingAsymmetric
{
    /// <summary>
    /// BASE64 encoding
    /// </summary>
    [DataContract]
    public abstract class BaseEncodingAsymmetricDriver : IEncodingAsymmetricDriver
    {
        /// <inheritdoc/>
        public virtual string DriverName => "Default";

        /// <inheritdoc/>
        public virtual DriverTypes DriverType => DriverTypes.EncodingAsymmetric;

        /// <inheritdoc/>
        public virtual object? Instance =>
            null;

        /// <inheritdoc/>
        public bool DriverInternal =>
            false;

        /// <inheritdoc/>
        public virtual void Initialize() =>
            DebugWriter.WriteDebug(DebugLevel.I, "Base64 encoding.");

        /// <inheritdoc/>
        public virtual byte[] GetEncodedString(string text)
        {
            if (string.IsNullOrEmpty(text))
                throw new KernelException(KernelExceptionType.Encoding, Translate.DoTranslation("The text must not be empty."));

            // Try to get the encoded string
            string encoded = text.GetBase64Encoded();
            return TextEncoding.Default.GetBytes(encoded);
        }

        /// <inheritdoc/>
        public virtual string GetDecodedString(byte[] encoded)
        {
            if (encoded is null || encoded.Length <= 0)
                throw new KernelException(KernelExceptionType.Encoding, Translate.DoTranslation("The encoded text must not be empty."));

            // Try to get the decoded string
            string plaintext = TextEncoding.Default.GetString(encoded);
            return plaintext.GetBase64Decoded();
        }

        /// <inheritdoc/>
        public virtual void EncodeFile(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new KernelException(KernelExceptionType.Encoding, Translate.DoTranslation("The path must not be empty."));
            if (!Checking.FileExists(path))
                throw new KernelException(KernelExceptionType.Encoding, Translate.DoTranslation("File doesn't exist."));

            // Get the bytes of the file
            string file = Reading.ReadContentsText(path);
            byte[] encrypted = GetEncodedString(file);

            // Write the array of bytes
            Writing.WriteAllBytes(path, encrypted);
        }

        /// <inheritdoc/>
        public virtual void DecodeFile(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new KernelException(KernelExceptionType.Encoding, Translate.DoTranslation("The path must not be empty."));
            if (!Checking.FileExists(path))
                throw new KernelException(KernelExceptionType.Encoding, Translate.DoTranslation("File doesn't exist."));

            // Get the bytes of the file
            byte[] encoded = Reading.ReadAllBytes(path);
            string decryptedStr = GetDecodedString(encoded);
            byte[] decrypted = TextEncoding.Default.GetBytes(decryptedStr);

            // Write the array of bytes
            Writing.WriteAllBytes(path, decrypted);
        }

        /// <inheritdoc/>
        public virtual byte[] ComposeBytesFromString(string encoded)
        {
            if (string.IsNullOrEmpty(encoded))
                throw new KernelException(KernelExceptionType.Encoding, Translate.DoTranslation("The encoded text must not be empty."));

            // Get the wrapped bytes, assuming that all the byte numbers are padded to three digits.
            string[] encodedStrings = TextTools.GetWrappedSentences(encoded, 3);
            List<byte> bytes = [];
            foreach (string encodedString in encodedStrings)
                bytes.Add(byte.Parse(encodedString));
            return [.. bytes];
        }

        /// <inheritdoc/>
        public virtual string DecomposeBytesFromString(byte[] encoded)
        {
            if (encoded is null || encoded.Length <= 0)
                throw new KernelException(KernelExceptionType.Encoding, Translate.DoTranslation("The encoded text must not be empty."));

            // Pad the encoded byte numbers to three digits and return them as a single string
            StringBuilder encodedStringBuilder = new();
            foreach (var value in encoded)
                encodedStringBuilder.Append($"{value:000}");
            return encodedStringBuilder.ToString();
        }

        /// <inheritdoc/>
        public bool TryRepresentAsText(byte[] encoded, out string? strEncoded)
        {
            strEncoded = null;
            string text = TextEncoding.Default.GetString(encoded);
            for (int i = 0; i < text.Length; i++)
            {
                char textChar = text[i];
                if (char.IsControl(textChar))
                    return false;
            }
            strEncoded = text;
            return true;
        }
    }
}
