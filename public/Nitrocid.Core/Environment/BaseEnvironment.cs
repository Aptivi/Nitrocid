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

using Nitrocid.Core.Environment.Internal;
using Nitrocid.Core.Languages;
using System;
using System.IO;
using System.Reflection;

namespace Nitrocid.Core.Environment
{
    /// <summary>
    /// Base environment
    /// </summary>
    public abstract class BaseEnvironment : IEnvironment
    {
        private Assembly? baseAsm = null;

        /// <summary>
        /// Arguments to provide this environment
        /// </summary>
        public string[]? Arguments { get; internal set; }

        /// <inheritdoc/>
        public virtual string Name =>
            LanguageTools.GetLocalized("NKS_KERNEL_STARTING_ENVIRONMENT_BASENAME");

        /// <inheritdoc/>
        public Action EnvironmentInit =>
            () => baseAsm = Assembly.LoadFrom(Directory.GetParent(typeof(BaseEnvironment).Assembly.Location)?.FullName + "/Nitrocid.Base.dll");

        /// <inheritdoc/>
        public virtual Action EnvironmentEntry
        {
            get
            {
                var entryPointMethod = KernelEnvironment.GetEntryPoint(baseAsm);
                return new(() => entryPointMethod.Invoke(null, [Arguments]));
            }
        }

        /// <inheritdoc/>
        public virtual Action EnvironmentExit
        {
            get
            {
                var exitPointMethod = KernelEnvironment.GetExitPoint(baseAsm);
                return new(() => exitPointMethod?.Invoke(null, null));
            }
        }
    }
}
