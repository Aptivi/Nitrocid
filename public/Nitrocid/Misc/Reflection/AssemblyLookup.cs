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
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using KS.Files;
using KS.Files.Operations.Querying;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;

namespace KS.Misc.Reflection
{
    /// <summary>
    /// Assembly lookup module
    /// </summary>
    public static class AssemblyLookup
    {

        private readonly static List<string> AssemblyLookupPaths = [];

        /// <summary>
        /// Adds the path pointing to the dependencies to the assembly search path
        /// </summary>
        /// <param name="Path">Path to the dependencies</param>
        public static void AddPathToAssemblySearchPath(string Path)
        {
            Path = FilesystemTools.NeutralizePath(Path);

            // Add the path to the search path
            if (!AssemblyLookupPaths.Contains(Path))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Adding path {0} to lookup paths...", Path);
                AssemblyLookupPaths.Add(Path);
            }
        }

        /// <summary>
        /// Removes the path pointing to the dependencies from the assembly search path
        /// </summary>
        /// <param name="Path">Path to the dependencies</param>
        public static void RemovePathFromAssemblySearchPath(string Path)
        {
            Path = FilesystemTools.NeutralizePath(Path);

            // Remove the path from the search path
            if (AssemblyLookupPaths.Contains(Path))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Removing path {0} from lookup paths...", Path);
                AssemblyLookupPaths.Remove(Path);
            }
        }

        /// <summary>
        /// Loads assembly from the search paths
        /// </summary>
        /// <returns>If successful, returns the assembly instance. Otherwise, null.</returns>
        internal static Assembly LoadFromAssemblySearchPaths(object sender, ResolveEventArgs args)
        {
            Assembly FinalAssembly = null;
            string DepAssemblyName = new AssemblyName(args.Name).Name;
            string ReqAssemblyName = args.RequestingAssembly is not null ? args.RequestingAssembly.GetName().Name : "An unknown assembly";
            DebugWriter.WriteDebug(DebugLevel.I, "{0} has requested to load {1}.", ReqAssemblyName, args.Name);

            if (DepAssemblyName == "Kernel Simulator")
                throw new KernelException(KernelExceptionType.OldModDetected);

            // Try to load assembly from lookup path
            foreach (string LookupPath in AssemblyLookupPaths)
            {
                string DepAssemblyFilePath = Path.Combine(LookupPath, DepAssemblyName + ".dll");
                try
                {
                    // Check to see if the dependency file exists
                    if (!Checking.FileExists(DepAssemblyFilePath))
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "Assembly {0} doesn't exist...", DepAssemblyFilePath);
                        continue;
                    }

                    // Try loading
                    DebugWriter.WriteDebug(DebugLevel.I, "Loading from {0}...", DepAssemblyFilePath);
                    FinalAssembly = Assembly.LoadFrom(DepAssemblyFilePath);
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to load {0} from {1}: {2}", args.Name, DepAssemblyFilePath, ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, "Trying another path...");
                }
            }

            // Get the final assembly
            return FinalAssembly;
        }

    }
}
