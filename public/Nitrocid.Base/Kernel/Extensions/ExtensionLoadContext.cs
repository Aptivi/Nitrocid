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
        // WARNING: It looks like that we have unloadability issues with addons that use Newtonsoft.Json that can't unload once
        // Newtonsoft.Json gets loaded, because Newtonsoft.Json caches properties and other objects in a way that causes
        // unloadability issues if we used JsonConvert. This makes load contexts useless in this regard when it comes to
        // unloading, which is screwed up.
        //
        // We can't fix this issue in late cycles of the development of 0.2.0 unless Newtonsoft.Json and other libraries that
        // cache their objects finally get fixed with a new update, as it involves a huge migration and careful studying of
        // possible breaking changes. Nitrocid's ThemePacks doesn't use serialization to parse the JSON files for themes for
        // Terminaux to process, which makes the matter worse.
        //
        // More information:
        //   - https://github.com/dotnet/runtime/issues/13283
        //   - https://github.com/JamesNK/Newtonsoft.Json/issues/2414
        //   - https://github.com/godotengine/godot-proposals/issues/11819
        //   - https://github.com/godotengine/godot/issues/78513
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
