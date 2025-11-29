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

using Nitrocid.Core.Languages;
using System;
using System.Reflection;

namespace Nitrocid.Core.Environment
{
    /// <summary>
    /// Base environment
    /// </summary>
    public abstract class BaseEnvironment : IEnvironment
    {
        /// <summary>
        /// Arguments to provide this environment
        /// </summary>
        public string[]? Arguments { get; internal set; }

        /// <inheritdoc/>
        public virtual string Name =>
            LanguageTools.GetLocalized("NKS_KERNEL_STARTING_ENVIRONMENT_BASENAME");

        /// <inheritdoc/>
        public virtual Action EnvironmentEntry
        {
            get
            {
                string typeName = typeof(EnvironmentTools).Assembly?.FullName?.Replace(".Core", ".Base") ?? "";
                var entryPointMethod = Type.GetType($"Nitrocid.Base.Kernel.KernelEntry, {typeName}")?.GetMethod("EntryPoint", BindingFlags.NonPublic | BindingFlags.Static) ??
                    throw new Exception(LanguageTools.GetLocalized("NKS_KERNEL_STARTING_ENVIRONMENT_EXCEPTION_MISSINGENTRYPOINT"));
                return new(() => entryPointMethod.Invoke(null, [Arguments]));
            }
        }
    }
}
