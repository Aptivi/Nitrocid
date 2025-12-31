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

using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Files;
using System.Reflection;

namespace Nitrocid.Base.Security.Signing
{
    /// <summary>
    /// Assembly signing tools
    /// </summary>
    public static class AssemblySigning
    {
        /// <summary>
        /// Gets a public key token from the loaded assembly
        /// </summary>
        /// <param name="assembly">Assembly to query</param>
        /// <returns>A byte array that holds the public key token</returns>
        public static byte[] PublicKeyToken(Assembly assembly)
        {
            // Check to see if the assembly is specified
            if (assembly is null)
                throw new KernelException(KernelExceptionType.Security, LanguageTools.GetLocalized("NKS_SECURITY_SIGNING_EXCEPTION_NEEDSASSEMBLY"));

            // Now, get the assembly name from it and return its public key token
            var asmName = new AssemblyName(assembly.FullName ?? "");
            return PublicKeyToken(asmName);
        }

        /// <summary>
        /// Gets a public key token from the loaded assembly
        /// </summary>
        /// <param name="assemblyPath">Path to an assembly file. It's neutralized by this function.</param>
        /// <returns>A byte array that holds the public key token</returns>
        public static byte[] PublicKeyToken(string assemblyPath)
        {
            // Check to see if the assembly path is specified
            if (string.IsNullOrEmpty(assemblyPath))
                throw new KernelException(KernelExceptionType.Security, LanguageTools.GetLocalized("NKS_SECURITY_SIGNING_EXCEPTION_NEEDSPATH"));

            // Now, get the assembly path, get its name, and return its public key token
            assemblyPath = FilesystemTools.NeutralizePath(assemblyPath);
            var asmName = AssemblyName.GetAssemblyName(assemblyPath);
            return PublicKeyToken(asmName);
        }

        /// <summary>
        /// Gets a public key token from the loaded assembly
        /// </summary>
        /// <param name="assemblyName">Assembly name instance.</param>
        /// <returns>A byte array that holds the public key token</returns>
        public static byte[] PublicKeyToken(AssemblyName assemblyName)
        {
            // Check to see if the assembly name is specified
            if (assemblyName is null)
                throw new KernelException(KernelExceptionType.Security, LanguageTools.GetLocalized("NKS_SECURITY_SIGNING_EXCEPTION_NEEDSNAME"));

            // Now, return the public key token
            var asmPublicKey = assemblyName.GetPublicKeyToken();
            if (asmPublicKey is null)
                return [];
            return asmPublicKey;
        }

        /// <summary>
        /// Checks to see if the given assembly is strongly signed
        /// </summary>
        /// <param name="assembly">Assembly to query</param>
        /// <returns>True if signed; false otherwise</returns>
        public static bool IsStronglySigned(Assembly assembly)
        {
            var token = PublicKeyToken(assembly);
            return !(token is null || token.Length == 0);
        }

        /// <summary>
        /// Checks to see if the given assembly is strongly signed
        /// </summary>
        /// <param name="assemblyPath">Assembly path to query</param>
        /// <returns>True if signed; false otherwise</returns>
        public static bool IsStronglySigned(string assemblyPath)
        {
            var token = PublicKeyToken(assemblyPath);
            return !(token is null || token.Length == 0);
        }

        /// <summary>
        /// Checks to see if the given assembly is strongly signed
        /// </summary>
        /// <param name="assemblyName">Assembly name instance</param>
        /// <returns>True if signed; false otherwise</returns>
        public static bool IsStronglySigned(AssemblyName assemblyName)
        {
            var token = PublicKeyToken(assemblyName);
            return !(token is null || token.Length == 0);
        }
    }
}
