//
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
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

using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Nitrocid.Extras.RetroKS
{
    internal class RetroKSContext : AssemblyLoadContext
    {
        internal AssemblyDependencyResolver resolver;
        internal string bootPath = "";

        protected override Assembly Load(AssemblyName assemblyName)
        {
            // Check to see if we have this assembly in the default context
            var finalAsm = Default.Assemblies.FirstOrDefault((asm) => asm.FullName == assemblyName.FullName);
            if (finalAsm is not null)
                return finalAsm;

            // Now, try to resolve
            string assemblyPath = resolver.ResolveAssemblyToPath(assemblyName);
            if (assemblyPath != null)
                return LoadFromAssemblyPath(assemblyPath);
            return null;
        }

        protected override nint LoadUnmanagedDll(string unmanagedDllName)
        {
            string libraryPath = resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
            if (libraryPath != null)
                return LoadUnmanagedDllFromPath(libraryPath);
            return IntPtr.Zero;
        }
    }
}

