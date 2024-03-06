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
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using System.Security.Cryptography;
using TextEncoding = System.Text.Encoding;

namespace Nitrocid.Drivers.Encoding.Bases
{
    /// <summary>
    /// RSA encoder
    /// </summary>
    public class RsaEncoding : BaseEncodingDriver, IEncodingDriver
    {
        private RSA rsa;

        /// <inheritdoc/>
        public override string DriverName => "RSA";

        /// <inheritdoc/>
        public override DriverTypes DriverType => DriverTypes.Encoding;

        /// <inheritdoc/>
        public override object Instance =>
            rsa;

        /// <inheritdoc/>
        public override bool IsSymmetric =>
            false;

        /// <inheritdoc/>
        public override byte[] Key =>
            throw new KernelException(KernelExceptionType.NotImplementedYet, Translate.DoTranslation("RSA doesn't support keys"));

        /// <inheritdoc/>
        public override byte[] Iv =>
            throw new KernelException(KernelExceptionType.NotImplementedYet, Translate.DoTranslation("RSA doesn't support initialization vectors"));

        /// <inheritdoc/>
        public override void Initialize() =>
            rsa ??= RSA.Create();

        /// <inheritdoc/>
        public override byte[] GetEncodedString(string text)
        {
            if (string.IsNullOrEmpty(text))
                throw new KernelException(KernelExceptionType.Encoding, Translate.DoTranslation("The text must not be empty."));

            // Try to get the encoded string
            byte[] textBytes = TextEncoding.Default.GetBytes(text);
            byte[] encrypted = rsa.Encrypt(textBytes, RSAEncryptionPadding.Pkcs1);
            return encrypted;
        }

        /// <inheritdoc/>
        public override string GetDecodedString(byte[] encoded)
        {
            if (encoded is null || encoded.Length <= 0)
                throw new KernelException(KernelExceptionType.Encoding, Translate.DoTranslation("The encoded text must not be empty."));

            // Try to get the decoded string
            byte[] decrypted = rsa.Decrypt(encoded, RSAEncryptionPadding.Pkcs1);
            string plaintext = TextEncoding.Default.GetString(decrypted);
            return plaintext;
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
            byte[] file = Reading.ReadAllBytes(path);
            byte[] encrypted = rsa.Encrypt(file, RSAEncryptionPadding.Pkcs1);

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
            byte[] decrypted = rsa.Decrypt(encoded, RSAEncryptionPadding.Pkcs1);

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
