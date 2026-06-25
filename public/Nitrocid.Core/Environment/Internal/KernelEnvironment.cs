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
using Nitrocid.Core.Languages;

namespace Nitrocid.Core.Environment.Internal
{
    internal static class KernelEnvironment
    {
        internal static MethodInfo GetEntryPoint(Assembly? baseAssembly)
        {
            var method = GetKernelEntryMethod("EntryPoint", baseAssembly) ??
                throw new Exception(LanguageTools.GetLocalized("NKS_KERNEL_STARTING_ENVIRONMENT_EXCEPTION_MISSINGENTRYPOINT"));
            return method;
        }
        
        internal static MethodInfo? GetExitPoint(Assembly? baseAssembly)
        {
            var method = GetKernelEntryMethod("ExitPoint", baseAssembly);
            return method;
        }

        internal static MethodInfo? GetKernelEntryMethod(string methodName, Assembly? baseAssembly)
        {
            string typeName = baseAssembly?.FullName ?? "";
            string asmName = baseAssembly?.GetName().Name ?? "";
            var method = Type.GetType($"{asmName}.Kernel.KernelEntry, {typeName}")?.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static);
            return method;
        }
    }
}
