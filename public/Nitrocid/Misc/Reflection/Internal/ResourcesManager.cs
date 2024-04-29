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

using Nitrocid.Kernel.Exceptions;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Textify.General;

namespace Nitrocid.Misc.Reflection.Internal
{
    internal static class ResourcesManager
    {
        private static readonly Dictionary<Assembly, Dictionary<string, string>> assemblies = [];

        internal static bool DataExists(string resource, ResourcesType type, out string data, Assembly asm = null)
        {
            asm ??= Assembly.GetExecutingAssembly();
            data = "";
            if (!assemblies.TryGetValue(asm, out Dictionary<string, string> asmResources))
                return false;
            return
                type == ResourcesType.Misc ?
                asmResources.TryGetValue(resource, out data) :
                asmResources.TryGetValue($"{type}.{resource}", out data);
        }

        internal static string GetData(string resource, ResourcesType type, Assembly asm = null)
        {
            asm ??= Assembly.GetExecutingAssembly();
            if (!DataExists(resource, type, out string data, asm))
                throw new KernelException(KernelExceptionType.Reflection, $"Resource {resource} not found for type {type} in between {assemblies.Count} assemblies");
            return data;
        }

        internal static void InitializeData(Assembly assembly)
        {
            var resourceFullNames = assembly.GetManifestResourceNames();
            var asmResources = new Dictionary<string, string>();
            foreach (string resourceFullName in resourceFullNames)
            {
                // Get the stream and parse its contents
                var contentStream = assembly.GetManifestResourceStream(resourceFullName);
                using var contentStreamReader = new StreamReader(contentStream);
                string content = contentStreamReader.ReadToEnd();
                string fileName = resourceFullName.RemovePrefix($"{assembly.GetName().Name}.Resources.");

                // Afterwards, add the resulting content to the resources dictionary to cache it
                asmResources.Add(fileName, content);
            }
            assemblies.TryAdd(assembly, asmResources);
        }

        static ResourcesManager() =>
            InitializeData(typeof(ResourcesManager).Assembly);
    }
}
