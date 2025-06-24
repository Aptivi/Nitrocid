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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Textify.General;
using TextEncoding = System.Text.Encoding;

namespace Nitrocid.Base.Drivers.Encoding
{
    /// <summary>
    /// AES encoding
    /// </summary>
    public abstract class BaseEncodingDriver : IEncodingDriver
    {
        private Aes? aes;

        /// <inheritdoc/>
        public virtual string DriverName => "Default";

        /// <inheritdoc/>
        public virtual DriverTypes DriverType => DriverTypes.Encoding;

        /// <inheritdoc/>
        public virtual bool DriverInternal => false;

        /// <inheritdoc/>
        public virtual object? Instance =>
            aes;

        /// <inheritdoc/>
        public virtual byte[] Key =>
            aes?.Key ?? [];

        /// <inheritdoc/>
        public virtual byte[] Iv =>
            aes?.IV ?? [];

        /// <inheritdoc/>
        public virtual void Initialize() =>
            aes ??= Aes.Create();

        /// <inheritdoc/>
        public virtual byte[] GetEncodedString(string text) =>
            GetEncodedString(text, Key, Iv);

        /// <inheritdoc/>
        public virtual string GetDecodedString(byte[] encoded) =>
            GetDecodedString(encoded, Key, Iv);

        /// <inheritdoc/>
        public virtual byte[] GetEncodedString(string text, byte[] key, byte[] iv)
        {
            if (string.IsNullOrEmpty(text))
                throw new KernelException(KernelExceptionType.Encoding, LanguageTools.GetLocalized("NKS_DRIVERS_ENCODING_BASE_NEEDSTEXT"));

            // Try to get the encoded string
            byte[] encrypted;
            using (Aes aesEncryptor = Aes.Create())
            {
                // Populate the key and the initialization vector
                aesEncryptor.Key = key;
                aesEncryptor.IV = iv;

                // Now, make the encryptor
                var encryptor = aesEncryptor.CreateEncryptor(aesEncryptor.Key, aesEncryptor.IV);
                using MemoryStream msEncrypt = new();
                using CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write);
                using (StreamWriter swEncrypt = new(csEncrypt))
                {
                    // Write all data to the stream.
                    swEncrypt.Write(text);
                }
                encrypted = msEncrypt.ToArray();
            }
            return encrypted;
        }

        /// <inheritdoc/>
        public virtual string GetDecodedString(byte[] encoded, byte[] key, byte[] iv)
        {
            if (encoded is null || encoded.Length <= 0)
                throw new KernelException(KernelExceptionType.Encoding, LanguageTools.GetLocalized("NKS_DRIVERS_ENCODING_BASE_NEEDSENCODEDTEXT"));

            // Try to get the decoded string
            string? plaintext = null;
            using (Aes aesDecryptor = Aes.Create())
            {
                // Populate the key and the initialization vector
                aesDecryptor.Key = key;
                aesDecryptor.IV = iv;

                // Now, make the decryptor
                ICryptoTransform decryptor = aesDecryptor.CreateDecryptor(aesDecryptor.Key, aesDecryptor.IV);
                using MemoryStream msDecrypt = new(encoded);
                using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
                using StreamReader srDecrypt = new(csDecrypt);

                // Now, write the decrypted result.
                plaintext = srDecrypt.ReadToEnd();
            }
            return plaintext;
        }

        /// <inheritdoc/>
        public virtual byte[] ComposeBytesFromString(string encoded)
        {
            if (string.IsNullOrEmpty(encoded))
                throw new KernelException(KernelExceptionType.Encoding, LanguageTools.GetLocalized("NKS_DRIVERS_ENCODING_BASE_NEEDSENCODEDTEXT"));

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
                throw new KernelException(KernelExceptionType.Encoding, LanguageTools.GetLocalized("NKS_DRIVERS_ENCODING_BASE_NEEDSENCODEDTEXT"));

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

        /// <inheritdoc/>
        public virtual void EncodeFile(string path) =>
            EncodeFile(path, Key, Iv);

        /// <inheritdoc/>
        public virtual void DecodeFile(string path) =>
            DecodeFile(path, Key, Iv);

        /// <inheritdoc/>
        public virtual void EncodeFile(string path, byte[] key, byte[] iv)
        {
            if (string.IsNullOrEmpty(path))
                throw new KernelException(KernelExceptionType.Encoding, LanguageTools.GetLocalized("NKS_DRIVERS_ENCODING_BASE_NEEDSPATH"));
            if (!FilesystemTools.FileExists(path))
                throw new KernelException(KernelExceptionType.Encoding, LanguageTools.GetLocalized("NKS_DRIVERS_ENCODING_BASE_FILENOTFOUND"));

            // Get the bytes of the file
            byte[] file = FilesystemTools.ReadAllBytes(path);
            byte[] encrypted;
            using Aes aesEncryptor = Aes.Create();

            // Populate the key and the initialization vector
            aesEncryptor.Key = key;
            aesEncryptor.IV = iv;

            // Now, make the encryptor
            var encryptor = aesEncryptor.CreateEncryptor(aesEncryptor.Key, aesEncryptor.IV);
            using MemoryStream msEncrypt = new();
            using (CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write))
            {
                csEncrypt.Write(file, 0, file.Length);
            }
            encrypted = msEncrypt.ToArray();

            // Write the array of bytes
            string encodedPath = path + ".encoded";
            FilesystemTools.WriteAllBytes(encodedPath, encrypted);
        }

        /// <inheritdoc/>
        public virtual void DecodeFile(string path, byte[] key, byte[] iv)
        {
            if (string.IsNullOrEmpty(path))
                throw new KernelException(KernelExceptionType.Encoding, LanguageTools.GetLocalized("NKS_DRIVERS_ENCODING_BASE_NEEDSPATH"));
            if (!FilesystemTools.FileExists(path))
                throw new KernelException(KernelExceptionType.Encoding, LanguageTools.GetLocalized("NKS_DRIVERS_ENCODING_BASE_FILENOTFOUND"));

            // Get the bytes of the file
            byte[] encoded = FilesystemTools.ReadAllBytes(path);
            byte[] decrypted = new byte[encoded.Length];
            using Aes aesDecryptor = Aes.Create();

            // Populate the key and the initialization vector
            aesDecryptor.Key = key;
            aesDecryptor.IV = iv;

            // Now, make the decryptor
            ICryptoTransform decryptor = aesDecryptor.CreateDecryptor(aesDecryptor.Key, aesDecryptor.IV);
            using MemoryStream msDecrypt = new(encoded);
            using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
            int idx = 0;
            int result = csDecrypt.ReadByte();
            decrypted[idx] = (byte)result;
            while (true)
            {
                idx++;
                result = csDecrypt.ReadByte();
                if (result == -1)
                    break;
                decrypted[idx] = (byte)result;
            }
            int diffIdx = encoded.Length - idx;

            // Write the array of bytes
            string decodedPath = path.RemoveSuffix(".encoded");
            FilesystemTools.WriteAllBytes(decodedPath, decrypted.SkipLast(diffIdx).ToArray());
        }
    }
}
