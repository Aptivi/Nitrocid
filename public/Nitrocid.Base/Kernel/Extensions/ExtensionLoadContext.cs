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

using System;
using System.Reflection;
using System.Runtime.Loader;

namespace Nitrocid.Base.Kernel.Extensions
{
    internal class ExtensionLoadContext : AssemblyLoadContext
    {
        private readonly AssemblyDependencyResolver resolver;
        private readonly AssemblyDependencyResolver baseResolver;

        protected override Assembly? Load(AssemblyName assemblyName)
        {
            string? asmPathBase = baseResolver.ResolveAssemblyToPath(assemblyName);
            if (asmPathBase is not null)
                return null;
            string? asmPath = resolver.ResolveAssemblyToPath(assemblyName);
            if (asmPath is not null)
                return LoadFromAssemblyPath(asmPath);
            return null;
        }

        protected override nint LoadUnmanagedDll(string unmanagedDllName)
        {
            string? libPathBase = baseResolver.ResolveUnmanagedDllToPath(unmanagedDllName);
            if (libPathBase is not null)
                return IntPtr.Zero;
            string? libPath = resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
            if (libPath is not null)
                return LoadUnmanagedDllFromPath(libPath);
            return IntPtr.Zero;
        }

        public ExtensionLoadContext(string path) :
            base(path, isCollectible: true)
        {
            resolver = new AssemblyDependencyResolver(path);
            baseResolver = new AssemblyDependencyResolver(typeof(ExtensionLoadContext).Assembly.Location);
        }
    }
}
