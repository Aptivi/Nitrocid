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

using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Files;
using System.Security.Cryptography;
using TextEncoding = System.Text.Encoding;

namespace Nitrocid.Base.Drivers.EncodingAsymmetric.Bases
{
    /// <summary>
    /// RSA encoder
    /// </summary>
    public class RsaEncoding : BaseEncodingAsymmetricDriver, IEncodingAsymmetricDriver
    {
        private RSA? rsa;

        /// <inheritdoc/>
        public override string DriverName => "RSA";

        /// <inheritdoc/>
        public override DriverTypes DriverType => DriverTypes.EncodingAsymmetric;

        /// <inheritdoc/>
        public override object? Instance =>
            rsa;

        /// <inheritdoc/>
        public override void Initialize() =>
            rsa ??= RSA.Create();

        /// <inheritdoc/>
        public override byte[] GetEncodedString(string text)
        {
            if (string.IsNullOrEmpty(text))
                throw new KernelException(KernelExceptionType.Encoding, LanguageTools.GetLocalized("NKS_DRIVERS_ENCODING_BASE_NEEDSTEXT"));
            if (rsa is null)
                throw new KernelException(KernelExceptionType.Encoding, LanguageTools.GetLocalized("NKS_DRIVERS_ENCODING_BASE_EXCEPTION_NEEDSINIT"));

            // Try to get the encoded string
            byte[] textBytes = TextEncoding.Default.GetBytes(text);
            byte[] encrypted = rsa.Encrypt(textBytes, RSAEncryptionPadding.Pkcs1);
            return encrypted;
        }

        /// <inheritdoc/>
        public override string GetDecodedString(byte[] encoded)
        {
            if (encoded is null || encoded.Length <= 0)
                throw new KernelException(KernelExceptionType.Encoding, LanguageTools.GetLocalized("NKS_DRIVERS_ENCODING_BASE_NEEDSENCODEDTEXT"));
            if (rsa is null)
                throw new KernelException(KernelExceptionType.Encoding, LanguageTools.GetLocalized("NKS_DRIVERS_ENCODING_BASE_EXCEPTION_NEEDSINIT"));

            // Try to get the decoded string
            byte[] decrypted = rsa.Decrypt(encoded, RSAEncryptionPadding.Pkcs1);
            string plaintext = TextEncoding.Default.GetString(decrypted);
            return plaintext;
        }

        /// <inheritdoc/>
        public override void EncodeFile(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new KernelException(KernelExceptionType.Encoding, LanguageTools.GetLocalized("NKS_DRIVERS_ENCODING_BASE_NEEDSPATH"));
            if (!FilesystemTools.FileExists(path))
                throw new KernelException(KernelExceptionType.Encoding, LanguageTools.GetLocalized("NKS_DRIVERS_ENCODING_BASE_FILENOTFOUND"));
            if (rsa is null)
                throw new KernelException(KernelExceptionType.Encoding, LanguageTools.GetLocalized("NKS_DRIVERS_ENCODING_BASE_EXCEPTION_NEEDSINIT"));

            // Get the bytes of the file
            byte[] file = FilesystemTools.ReadAllBytes(path);
            byte[] encrypted = rsa.Encrypt(file, RSAEncryptionPadding.Pkcs1);

            // Write the array of bytes
            FilesystemTools.WriteAllBytes(path, encrypted);
        }

        /// <inheritdoc/>
        public override void DecodeFile(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new KernelException(KernelExceptionType.Encoding, LanguageTools.GetLocalized("NKS_DRIVERS_ENCODING_BASE_NEEDSPATH"));
            if (!FilesystemTools.FileExists(path))
                throw new KernelException(KernelExceptionType.Encoding, LanguageTools.GetLocalized("NKS_DRIVERS_ENCODING_BASE_FILENOTFOUND"));
            if (rsa is null)
                throw new KernelException(KernelExceptionType.Encoding, LanguageTools.GetLocalized("NKS_DRIVERS_ENCODING_BASE_EXCEPTION_NEEDSINIT"));

            // Get the bytes of the file
            byte[] encoded = FilesystemTools.ReadAllBytes(path);
            byte[] decrypted = rsa.Decrypt(encoded, RSAEncryptionPadding.Pkcs1);

            // Write the array of bytes
            FilesystemTools.WriteAllBytes(path, decrypted);
        }
    }
}
