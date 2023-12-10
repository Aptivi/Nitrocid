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

using System.IO;
using FS = KS.Files.FilesystemTools;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Files.Operations.Querying;

namespace KS.Drivers.Encryption
{
    /// <summary>
    /// Hash sum verification module
    /// </summary>
    public static class HashVerifier
    {

        /// <summary>
        /// Verifies the hash sum of a file from hashes file
        /// </summary>
        /// <param name="FileName">Target file</param>
        /// <param name="HashType">Hash algorithm</param>
        /// <param name="HashesFile">Hashes file that contains the target file</param>
        /// <param name="ActualHash">Actual hash calculated from hash tool</param>
        /// <returns>True if they match; else, false.</returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static bool VerifyHashFromHashesFile(string FileName, string HashType, string HashesFile, string ActualHash)
        {
            int ExpectedHashLength;
            string ExpectedHash = "";

            FileName = FS.NeutralizePath(FileName);
            HashesFile = FS.NeutralizePath(HashesFile);
            DebugWriter.WriteDebug(DebugLevel.I, "File name: {0}", FileName);
            DebugWriter.WriteDebug(DebugLevel.I, "Hashes file name: {0}", HashesFile);
            if (Checking.FileExists(FileName))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Hash type: {0} ({1})", HashType, HashType.ToString());
                ExpectedHashLength = GetExpectedHashLength(HashType);

                // Verify the hash
                if (Checking.FileExists(HashesFile))
                {
                    var HashStream = new StreamReader(HashesFile);
                    DebugWriter.WriteDebug(DebugLevel.I, "Stream length: {0}", HashStream.BaseStream.Length);
                    while (!HashStream.EndOfStream)
                    {
                        // Check if made from KS, and take it from before-last split space. If not, take it from the beginning
                        string StringLine = HashStream.ReadLine();
                        if (StringLine.StartsWith("- "))
                        {
                            DebugWriter.WriteDebug(DebugLevel.I, "Hashes file is of KS format");
                            if ((StringLine.StartsWith("- " + FileName) | StringLine.StartsWith("- " + Path.GetFileName(FileName))) & StringLine.EndsWith($"({HashType})"))
                            {
                                var HashSplit = StringLine.Split(' ');
                                ExpectedHash = HashSplit[^2].ToUpper();
                                ActualHash = ActualHash.ToUpper();
                            }
                        }
                        else
                        {
                            DebugWriter.WriteDebug(DebugLevel.I, "Hashes file is of standard format");
                            if (StringLine.EndsWith(Path.GetFileName(FileName)))
                            {
                                var HashSplit = StringLine.Split(' ');
                                ExpectedHash = HashSplit[0].ToUpper();
                                ActualHash = ActualHash.ToUpper();
                            }
                        }
                    }
                    HashStream.Close();
                }
                else
                {
                    throw new KernelException(KernelExceptionType.Encryption, "Hashes file {0} not found.", HashesFile);
                }

                if (ActualHash.Length == ExpectedHashLength & ExpectedHash.Length == ExpectedHashLength)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Hashes are consistent.");
                    DebugWriter.WriteDebug(DebugLevel.I, "Hashes {0} and {1}", ActualHash, ExpectedHash);
                    if (ActualHash == ExpectedHash)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Hashes match.");
                        return true;
                    }
                    else
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "Hashes don't match.");
                        return false;
                    }
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "{0} ({1}) or {2} ({3}) is malformed. Check the algorithm ({4}). Expected length: {5}", ActualHash, ActualHash.Length, ExpectedHash, ExpectedHash.Length, HashType, ExpectedHashLength);
                    throw new KernelException(KernelExceptionType.InvalidHash, "{0} ({1}) or {2} ({3}) is malformed. Check the algorithm ({4}). Expected length: {5}", ActualHash, ActualHash.Length, ExpectedHash, ExpectedHash.Length, HashType, ExpectedHashLength);
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.Encryption, "File {0} not found.", FileName);
            }
        }

        /// <summary>
        /// Verifies the hash sum of a file from expected hash
        /// </summary>
        /// <param name="FileName">Target file</param>
        /// <param name="HashType">Hash algorithm</param>
        /// <param name="ExpectedHash">Expected hash of a target file</param>
        /// <param name="ActualHash">Actual hash calculated from hash tool</param>
        /// <returns>True if they match; else, false.</returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static bool VerifyHashFromHash(string FileName, string HashType, string ExpectedHash, string ActualHash)
        {
            int ExpectedHashLength;

            FileName = FS.NeutralizePath(FileName);
            ExpectedHash = ExpectedHash.ToUpper();
            ActualHash = ActualHash.ToUpper();
            DebugWriter.WriteDebug(DebugLevel.I, "File name: {0}", FileName);
            if (Checking.FileExists(FileName))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Hash type: {0} ({1})", HashType, HashType.ToString());
                ExpectedHashLength = GetExpectedHashLength(HashType);

                // Verify the hash
                if (ActualHash.Length == ExpectedHashLength & ExpectedHash.Length == ExpectedHashLength)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Hashes are consistent.");
                    DebugWriter.WriteDebug(DebugLevel.I, "Hashes {0} and {1}", ActualHash, ExpectedHash);
                    if (ActualHash == ExpectedHash)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Hashes match.");
                        return true;
                    }
                    else
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "Hashes don't match.");
                        return false;
                    }
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "{0} ({1}) or {2} ({3}) is malformed. Check the algorithm ({4}). Expected length: {5}", ActualHash, ActualHash.Length, ExpectedHash, ExpectedHash.Length, HashType, ExpectedHashLength);
                    throw new KernelException(KernelExceptionType.InvalidHash, "{0} ({1}) or {2} ({3}) is malformed. Check the algorithm ({4}). Expected length: {5}", ActualHash, ActualHash.Length, ExpectedHash, ExpectedHash.Length, HashType, ExpectedHashLength);
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.Encryption, "File {0} not found.", FileName);
            }
        }

        /// <summary>
        /// Verifies the hash sum of a file from hashes file once the file's hash is calculated.
        /// </summary>
        /// <param name="FileName">Target file</param>
        /// <param name="HashType">Hash algorithm</param>
        /// <param name="HashesFile">Hashes file that contains the target file</param>
        /// <returns>True if they match; else, false.</returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static bool VerifyUncalculatedHashFromHashesFile(string FileName, string HashType, string HashesFile)
        {
            int ExpectedHashLength;
            string ExpectedHash = "";
            string ActualHash = "";

            FileName = FS.NeutralizePath(FileName);
            HashesFile = FS.NeutralizePath(HashesFile);
            DebugWriter.WriteDebug(DebugLevel.I, "File name: {0}", FileName);
            DebugWriter.WriteDebug(DebugLevel.I, "Hashes file name: {0}", HashesFile);
            if (Checking.FileExists(FileName))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Hash type: {0} ({1})", HashType, HashType.ToString());
                ExpectedHashLength = GetExpectedHashLength(HashType);

                // Verify the hash
                if (Checking.FileExists(HashesFile))
                {
                    var HashStream = new StreamReader(HashesFile);
                    DebugWriter.WriteDebug(DebugLevel.I, "Stream length: {0}", HashStream.BaseStream.Length);
                    while (!HashStream.EndOfStream)
                    {
                        // Check if made from KS, and take it from before-last split space. If not, take it from the beginning
                        string StringLine = HashStream.ReadLine();
                        if (StringLine.StartsWith("- "))
                        {
                            DebugWriter.WriteDebug(DebugLevel.I, "Hashes file is of KS format");
                            if ((StringLine.StartsWith("- " + FileName) | StringLine.StartsWith("- " + Path.GetFileName(FileName))) & StringLine.EndsWith($"({HashType})"))
                            {
                                var HashSplit = StringLine.Split(' ');
                                ExpectedHash = HashSplit[^2].ToUpper();
                                ActualHash = Encryption.GetEncryptedFile(FileName, HashType).ToUpper();
                            }
                        }
                        else
                        {
                            DebugWriter.WriteDebug(DebugLevel.I, "Hashes file is of standard format");
                            if (StringLine.EndsWith(Path.GetFileName(FileName)))
                            {
                                var HashSplit = StringLine.Split(' ');
                                ExpectedHash = HashSplit[0].ToUpper();
                                ActualHash = Encryption.GetEncryptedFile(FileName, HashType).ToUpper();
                            }
                        }
                    }
                    HashStream.Close();
                }
                else
                {
                    throw new KernelException(KernelExceptionType.Encryption, "Hashes file {0} not found.", HashesFile);
                }

                if (ActualHash.Length == ExpectedHashLength & ExpectedHash.Length == ExpectedHashLength)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Hashes are consistent.");
                    DebugWriter.WriteDebug(DebugLevel.I, "Hashes {0} and {1}", ActualHash, ExpectedHash);
                    if (ActualHash == ExpectedHash)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Hashes match.");
                        return true;
                    }
                    else
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "Hashes don't match.");
                        return false;
                    }
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "{0} ({1}) or {2} ({3}) is malformed. Check the algorithm ({4}). Expected length: {5}", ActualHash, ActualHash.Length, ExpectedHash, ExpectedHash.Length, HashType, ExpectedHashLength);
                    throw new KernelException(KernelExceptionType.InvalidHash, "{0} ({1}) or {2} ({3}) is malformed. Check the algorithm ({4}). Expected length: {5}", ActualHash, ActualHash.Length, ExpectedHash, ExpectedHash.Length, HashType, ExpectedHashLength);
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.Encryption, "File {0} not found.", FileName);
            }
        }

        /// <summary>
        /// Verifies the hash sum of a file from expected hash once the file's hash is calculated.
        /// </summary>
        /// <param name="FileName">Target file</param>
        /// <param name="HashType">Hash algorithm</param>
        /// <param name="ExpectedHash">Expected hash of a target file</param>
        /// <returns>True if they match; else, false.</returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static bool VerifyUncalculatedHashFromHash(string FileName, string HashType, string ExpectedHash)
        {
            int ExpectedHashLength;
            string ActualHash;
            FileName = FS.NeutralizePath(FileName);
            ExpectedHash = ExpectedHash.ToUpper();
            DebugWriter.WriteDebug(DebugLevel.I, "File name: {0}", FileName);
            if (Checking.FileExists(FileName))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Hash type: {0} ({1})", HashType, HashType.ToString());
                ExpectedHashLength = GetExpectedHashLength(HashType);

                // Calculate the file hash
                ActualHash = Encryption.GetEncryptedFile(FileName, HashType).ToUpper();

                // Verify the hash
                if (ActualHash.Length == ExpectedHashLength & ExpectedHash.Length == ExpectedHashLength)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Hashes are consistent.");
                    DebugWriter.WriteDebug(DebugLevel.I, "Hashes {0} and {1}", ActualHash, ExpectedHash);
                    if (ActualHash == ExpectedHash)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Hashes match.");
                        return true;
                    }
                    else
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "Hashes don't match.");
                        return false;
                    }
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "{0} ({1}) or {2} ({3}) is malformed. Check the algorithm ({4}). Expected length: {5}", ActualHash, ActualHash.Length, ExpectedHash, ExpectedHash.Length, HashType, ExpectedHashLength);
                    throw new KernelException(KernelExceptionType.InvalidHash, "{0} ({1}) or {2} ({3}) is malformed. Check the algorithm ({4}). Expected length: {5}", ActualHash, ActualHash.Length, ExpectedHash, ExpectedHash.Length, HashType, ExpectedHashLength);
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.Encryption, "File {0} not found.", FileName);
            }
        }

        /// <summary>
        /// Gets the expected hash length
        /// </summary>
        /// <param name="HashType">An encryption algorithm</param>
        /// <returns>The expected hash length</returns>
        public static int GetExpectedHashLength(string HashType) =>
            DriverHandler.GetDriver<IEncryptionDriver>(HashType).HashLength;

    }
}
