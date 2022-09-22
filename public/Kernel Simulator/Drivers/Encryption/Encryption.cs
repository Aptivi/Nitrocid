
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

using System;
using System.Collections.Generic;
using System.IO;
using KS.Drivers.Encryption.Encryptors;
using KS.Files;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;

namespace KS.Drivers.Encryption
{
    /// <summary>
    /// Encryption and hashing module
    /// </summary>
    public static class Encryption
    {

        internal static Dictionary<string, IEncryptor> encryptors = new()
        {
            { "CRC32", new CRC32() },
            { "MD5", new MD5() },
            { "SHA1", new SHA1() },
            { "SHA256", new SHA256() },
            { "SHA384", new SHA384() },
            { "SHA512", new SHA512() }
        };

        /// <summary>
        /// Encryption algorithms supported by KS
        /// </summary>
        public enum Algorithms
        {
            /// <summary>
            /// The MD5 Algorithm
            /// </summary>
            MD5,
            /// <summary>
            /// The SHA1 Algorithm
            /// </summary>
            SHA1,
            /// <summary>
            /// The SHA256 Algorithm
            /// </summary>
            SHA256,
            /// <summary>
            /// The SHA384 Algorithm
            /// </summary>
            SHA384,
            /// <summary>
            /// The SHA512 Algorithm
            /// </summary>
            SHA512,
            /// <summary>
            /// The CRC32 Algorithm
            /// </summary>
            CRC32
        }

        /// <summary>
        /// Registers the encryptor (Mods that implement IEncryptors should use this function to add their encryptors to the list)
        /// </summary>
        /// <param name="encryptor">Encryptor instance</param>
        public static void RegisterEncryptor(IEncryptor encryptor)
        {
            // Get the encryptor name according to the class name
            string encryptorName = encryptor.GetType().Name;

            // Check to see if the name is taken
            if (encryptors.ContainsKey(encryptorName))
                encryptorName += encryptors.Count;

            // Add it to the available encryptors
            encryptors.Add(encryptorName, encryptor);
        }

        /// <summary>
        /// Unregisters the encryptor (Mods that implement IEncryptors should use this function to remove their encryptors from the list when unloading the mod)
        /// </summary>
        /// <param name="encryptorName">Encryptor name</param>
        public static void UnregisterEncryptor(string encryptorName)
        {
            // Make sure that we don't remove algorithms implemented by Kernel Simulator
            if (Enum.IsDefined(typeof(Algorithms), encryptorName))
                throw new InvalidHashAlgorithmException(Translate.DoTranslation("Tried to remove an internal algorithm. This isn't possible."));

            // Remove it!
            encryptors.Remove(encryptorName);
        }

        /// <summary>
        /// Translates the array of encrypted bytes to string
        /// </summary>
        /// <param name="encrypted">Array of encrypted bytes</param>
        /// <returns>A string representation of hash sum</returns>
        public static string GetArrayEnc(byte[] encrypted)
        {
            string hash = "";
            for (int i = 0, loopTo = encrypted.Length - 1; i <= loopTo; i++)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Appending {0} to hash", encrypted[i]);
                hash += $"{encrypted[i]:X2}";
            }
            DebugWriter.WriteDebug(DebugLevel.I, "Final hash: {0}", hash);
            return hash;
        }

        /// <summary>
        /// Encrypts a string
        /// </summary>
        /// <param name="str">Source string</param>
        /// <param name="algorithm">Algorithm</param>
        /// <returns>Encrypted hash sum</returns>
        public static string GetEncryptedString(string str, Algorithms algorithm)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Selected algorithm: {0}", algorithm.ToString());
            DebugWriter.WriteDebug(DebugLevel.I, "String length: {0}", str.Length);

            // Get the encryptor
            return encryptors[algorithm.ToString()].GetEncryptedString(str);
        }

        /// <summary>
        /// Encrypts a file
        /// </summary>
        /// <param name="str">Source stream</param>
        /// <param name="algorithm">Algorithm</param>
        /// <returns>Encrypted hash sum</returns>
        public static string GetEncryptedFile(Stream str, Algorithms algorithm)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Selected algorithm: {0}", algorithm.ToString());
            DebugWriter.WriteDebug(DebugLevel.I, "Stream length: {0}", str.Length);

            // Get the encryptor
            return encryptors[algorithm.ToString()].GetEncryptedFile(str);
        }

        /// <summary>
        /// Encrypts a file
        /// </summary>
        /// <param name="Path">Relative path</param>
        /// <param name="algorithm">Algorithm</param>
        /// <returns>Encrypted hash sum</returns>
        public static string GetEncryptedFile(string Path, Algorithms algorithm)
        {
            Path = Filesystem.NeutralizePath(Path);
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
        public static string GetEmptyHash(Algorithms Algorithm) => encryptors[Algorithm.ToString()].EmptyHash;

    }
}
