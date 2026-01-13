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

using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using System.IO;
using System.Linq;
using System.Reflection;
using Textify.General;

namespace Nitrocid.Base.Misc.Reflection.Internal
{
    internal static class ResourcesManager
    {
        internal static bool DataExists(string resource, ResourcesType type, out Stream? data, Assembly? asm = null)
        {
            asm ??= Assembly.GetCallingAssembly();

            // Get the stream
            string resourceName = type == ResourcesType.Misc ? resource : $"{type}.{resource}";
            string resourceFullName = $"{asm.GetName().Name}.Resources.{resourceName}";
            data = asm.GetManifestResourceStream(resourceFullName);
            DebugWriter.WriteDebug(DebugLevel.I, $"{asm.FullName ?? "This unknown assembly"} got resource {resource} with type {type} and got {(data is not null ? $"a data stream totalling {data.Length} bytes" : "nothing")}.");
            return data is not null;
        }

        internal static Stream? GetData(string resource, ResourcesType type, Assembly? asm = null)
        {
            asm ??= Assembly.GetCallingAssembly();
            if (!DataExists(resource, type, out Stream? data, asm))
            {
                // TODO: NKS_MISC_REFLECTION_INTERNAL_EXCEPTION_RESNOTFOUND_KNOWNASM -> Resource {0} not found for type {1} in {2}
                // TODO: NKS_MISC_REFLECTION_INTERNAL_EXCEPTION_RESNOTFOUND_UNKNOWNASM -> Resource {0} not found for type {1} in this unknown assembly
                string message = asm.FullName is not null ?
                    /* Localizable */ "NKS_MISC_REFLECTION_INTERNAL_EXCEPTION_RESNOTFOUND_KNOWNASM".FormatString(resource, type, asm.FullName) :
                    /* Localizable */ "NKS_MISC_REFLECTION_INTERNAL_EXCEPTION_RESNOTFOUND_UNKNOWNASM".FormatString(resource, type);
                throw new KernelException(KernelExceptionType.Reflection, LanguageTools.GetLocalized(message));
            }
            return data;
        }

        internal static string[] GetResourceNames(Assembly? asm)
        {
            asm ??= Assembly.GetCallingAssembly();
            var resources = asm.GetManifestResourceNames().Select((resource) => resource.RemovePrefix($"{asm.GetName().Name}.Resources.")).ToArray();
            return resources;
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
