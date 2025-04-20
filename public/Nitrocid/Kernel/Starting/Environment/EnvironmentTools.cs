﻿//
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

using Nitrocid.Kernel.Exceptions;
using Nitrocid.Kernel.Starting.Environment.Instances;
using Nitrocid.Languages;

namespace Nitrocid.Kernel.Starting.Environment
{
    /// <summary>
    /// Environment tools
    /// </summary>
    public static class EnvironmentTools
    {
        internal static bool resetEnvironment = false;
        internal static bool anotherEnvPending = false;
        internal static string[] kernelArguments = [];
        internal readonly static BaseEnvironment mainEnvironment = new NitrocidKS();
        internal static BaseEnvironment environment = mainEnvironment;

        /// <summary>
        /// Sets the environment
        /// </summary>
        /// <param name="baseEnvironment">Base environment instance</param>
        public static void SetEnvironment(BaseEnvironment? baseEnvironment)
        {
            if (baseEnvironment is null)
                throw new KernelException(KernelExceptionType.Environment, Translate.DoTranslation("Environment is not specified"));
            if (baseEnvironment != mainEnvironment)
                anotherEnvPending = true;
            environment = baseEnvironment;
        }

        /// <summary>
        /// Sets the environment
        /// </summary>
        /// <param name="baseEnvironment">Base environment instance</param>
        /// <param name="args">Arguments for the environment</param>
        public static void SetEnvironmentArgs(BaseEnvironment baseEnvironment, params string[] args)
        {
            if (baseEnvironment is null)
                throw new KernelException(KernelExceptionType.Environment, Translate.DoTranslation("Environment is not specified"));
            baseEnvironment.Arguments = args;
        }

        /// <summary>
        /// Resets the environment
        /// </summary>
        public static void ResetEnvironment()
        {
            SetEnvironment(mainEnvironment);
            SetEnvironmentArgs(mainEnvironment, kernelArguments);
        }

        internal static void ExecuteEnvironment() =>
            environment.EnvironmentEntry();
    }
}
