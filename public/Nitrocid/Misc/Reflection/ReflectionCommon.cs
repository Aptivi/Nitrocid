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

using Nitrocid.Files;
using Nitrocid.Kernel;
using Nitrocid.Kernel.Exceptions;
using System;
using System.Linq;
using System.Reflection;

namespace Nitrocid.Misc.Reflection
{
    internal static class ReflectionCommon
    {
        internal static Type[] KernelTypes = Assembly.GetExecutingAssembly().GetTypes().Where((type) => type.FullName?.StartsWith($"{KernelMain.rootNameSpace}.") ?? false).ToArray();

        /// <summary>
        /// If the specified file is a .NET assembly
        /// </summary>
        /// <param name="path">Absolute path to the assembly file</param>
        /// <param name="asmName">Assembly name</param>
        /// <returns>True if it's a real assembly; false otherwise</returns>
        public static bool IsDotnetAssemblyFile(string path, out AssemblyName? asmName)
        {
            if (!FilesystemTools.FileExists(path))
                throw new KernelException(KernelExceptionType.Reflection);

            try
            {
                asmName = AssemblyName.GetAssemblyName(path);
                return true;
            }
            catch (BadImageFormatException)
            {
                asmName = null;
                return false;
            }
        }
    }
}
