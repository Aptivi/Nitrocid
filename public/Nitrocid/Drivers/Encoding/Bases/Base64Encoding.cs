//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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
using Textify.General;
using TextEncoding = System.Text.Encoding;

namespace Nitrocid.Drivers.Encoding.Bases
{
    /// <summary>
    /// BASE64 encoder
    /// </summary>
    public class Base64Encoding : BaseEncodingDriver, IEncodingDriver
    {
        /// <inheritdoc/>
        public override string DriverName => "BASE64";

        /// <inheritdoc/>
        public override DriverTypes DriverType => DriverTypes.Encoding;

        /// <inheritdoc/>
        public override object Instance =>
            null;

        /// <inheritdoc/>
        public override bool IsSymmetric =>
            false;

        /// <inheritdoc/>
        public override byte[] Key =>
            throw new KernelException(KernelExceptionType.NotImplementedYet, Translate.DoTranslation("BASE64 doesn't support keys"));

        /// <inheritdoc/>
        public override byte[] Iv =>
            throw new KernelException(KernelExceptionType.NotImplementedYet, Translate.DoTranslation("BASE64 doesn't support initialization vectors"));

        /// <inheritdoc/>
        public override void Initialize() =>
            DebugWriter.WriteDebug(DebugLevel.I, "Base64 encoding.");

        /// <inheritdoc/>
        public override byte[] GetEncodedString(string text)
        {
            if (string.IsNullOrEmpty(text))
                throw new KernelException(KernelExceptionType.Encoding, Translate.DoTranslation("The text must not be empty."));

            // Try to get the encoded string
            string encoded = text.GetBase64Encoded();
            return TextEncoding.Default.GetBytes(encoded);
        }

        /// <inheritdoc/>
        public override string GetDecodedString(byte[] encoded)
        {
            if (encoded is null || encoded.Length <= 0)
                throw new KernelException(KernelExceptionType.Encoding, Translate.DoTranslation("The encoded text must not be empty."));

            // Try to get the decoded string
            string plaintext = TextEncoding.Default.GetString(encoded);
            return plaintext.GetBase64Decoded();
        }

        /// <inheritdoc/>
        public override byte[] GetEncodedString(string text, byte[] key, byte[] iv) =>
            GetEncodedString(text);

        /// <inheritdoc/>
        public override string GetDecodedString(byte[] encoded, byte[] key, byte[] iv) =>
            GetDecodedString(encoded);

        /// <inheritdoc/>
        public override void EncodeFile(string path)
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
        public override void DecodeFile(string path)
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
        public override void EncodeFile(string path, byte[] key, byte[] iv) =>
            EncodeFile(path);

        /// <inheritdoc/>
        public override void DecodeFile(string path, byte[] key, byte[] iv) =>
            DecodeFile(path);
    }
}
