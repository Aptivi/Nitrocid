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

using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Textify.General;

namespace Nitrocid.Misc.Reflection.Internal
{
    internal static class ResourcesManager
    {
        private static readonly Dictionary<Assembly, Dictionary<string, Stream>> assemblies = [];

        internal static bool DataExists(string resource, ResourcesType type, out Stream? data, Assembly? asm = null)
        {
            asm ??= Assembly.GetExecutingAssembly();
            data = null;
            InitializeData(asm);
            if (!assemblies.TryGetValue(asm, out Dictionary<string, Stream>? asmResources))
            {
                DebugWriter.WriteDebug(DebugLevel.E, $"{asm.FullName ?? "This unknown assembly"} doesn't have any resource.");
                return false;
            }

            // Get the stream
            RepopulateDisposed(resource, type, asm);
            bool result =
                type == ResourcesType.Misc ?
                asmResources.TryGetValue(resource, out data) :
                asmResources.TryGetValue($"{type}.{resource}", out data);
            DebugWriter.WriteDebug(DebugLevel.I, $"{asm.FullName ?? "This unknown assembly"} got resource {resource} with type {type} and got {(data is not null ? $"a data stream totalling {data.Length} bytes" : "nothing")}.");
            return result;
        }

        internal static Stream? GetData(string resource, ResourcesType type, Assembly? asm = null)
        {
            asm ??= Assembly.GetExecutingAssembly();
            InitializeData(asm);
            if (!DataExists(resource, type, out Stream? data, asm))
                throw new KernelException(KernelExceptionType.Reflection, $"Resource {resource} not found for type {type} in between {assemblies.Count} assemblies");
            return data;
        }

        internal static string[] GetResourceNames(Assembly? asm)
        {
            asm ??= Assembly.GetExecutingAssembly();
            InitializeData(asm);
            if (!assemblies.TryGetValue(asm, out Dictionary<string, Stream>? asmResources))
                return [];
            return asmResources.Select((kvp) => kvp.Key).ToArray();
        }

        internal static void InitializeData(Assembly? assembly)
        {
            if (assembly is null)
                return;
            if (assemblies.ContainsKey(assembly))
                return;
            DebugWriter.WriteDebug(DebugLevel.I, $"Initializing data for {assembly.FullName ?? "an unknown assembly"}...");
            var resourceFullNames = assembly.GetManifestResourceNames();
            var asmResources = new Dictionary<string, Stream>();
            foreach (string resourceFullName in resourceFullNames)
            {
                // Get the stream
                DebugWriter.WriteDebug(DebugLevel.I, $"Processing resource {resourceFullName}...");
                var contentStream = assembly.GetManifestResourceStream(resourceFullName);
                if (contentStream is null)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, $"Assembly doesn't have stream for {resourceFullName}. Ignoring...");
                    continue;
                }
                
                // Afterwards, add the resulting content to the resources dictionary to cache it
                string fileName = resourceFullName.RemovePrefix($"{assembly.GetName().Name}.Resources.");
                DebugWriter.WriteDebug(DebugLevel.I, $"Adding resource as {fileName}...");
                asmResources.Add(fileName, contentStream);
            }
            if (asmResources.Count > 0)
            {
                DebugWriter.WriteDebug(DebugLevel.I, $"Adding {assembly.FullName ?? "an unknown assembly"} with {asmResources.Count} resources...");
                assemblies.TryAdd(assembly, asmResources);
            }
        }

        internal static void RepopulateDisposed(string resource, ResourcesType type, Assembly asm)
        {
            // Bail if we don't have any stream
            if (!assemblies.TryGetValue(asm, out var asmResources))
                return;

            // Now, get the resource name and try to get it
            string resourceName = type == ResourcesType.Misc ? resource : $"{type}.{resource}";
            string resourceFullName = $"{asm.GetName().Name}.Resources.{resourceName}";
            if (!asmResources.TryGetValue(resourceName, out var data))
                return;

            // Check to see if this stream is disposed, and regenerate the stream if needed
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, $"{asm.FullName ?? "This unknown assembly"} got resource {resource} with type {type} and got {(data is not null ? $"a data stream totalling {data.Length} bytes" : "nothing")}.");
            }
            catch (ObjectDisposedException)
            {
                DebugWriter.WriteDebug(DebugLevel.W, $"{asm.FullName ?? "This unknown assembly"} got resource {resource} with type {type} and got data, but it's disposed. Regenerating...");
                var contentStream = asm.GetManifestResourceStream(resourceFullName);
                DebugWriter.WriteDebug(DebugLevel.I, $"While checking for disposed stream, {asm.FullName ?? "This unknown assembly"} got resource {resource} with type {type} and got {(contentStream is not null ? $"a data stream totalling {contentStream.Length} bytes" : "nothing")} to be added.");
                if (contentStream is null)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, $"Assembly doesn't have stream for {resourceFullName}. Ignoring...");
                    return;
                }
                assemblies[asm][resourceName] = contentStream;
            }
        }

        internal static string ConvertToString(Stream? contentStream)
        {
            if (contentStream is null)
                return "";
            using var contentStreamReader = new StreamReader(contentStream);
            string content = contentStreamReader.ReadToEnd();
            return content;
        }
    }
}
