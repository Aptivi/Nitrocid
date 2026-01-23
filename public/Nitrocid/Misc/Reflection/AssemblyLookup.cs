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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Nitrocid.Files;
using Nitrocid.Files.Paths;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;

namespace Nitrocid.Misc.Reflection
{
    /// <summary>
    /// Assembly lookup module
    /// </summary>
    public static class AssemblyLookup
    {
        internal readonly static List<string> baseAssemblyLookupPaths = [];
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
                DebugWriter.WriteDebug(DebugLevel.I, "Adding path {0} to lookup paths...", vars: [Path]);
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
                DebugWriter.WriteDebug(DebugLevel.I, "Removing path {0} from lookup paths...", vars: [Path]);
                AssemblyLookupPaths.Remove(Path);
            }
        }

        /// <summary>
        /// Loads assembly from the search paths
        /// </summary>
        /// <returns>If successful, returns the assembly instance. Otherwise, null.</returns>
        internal static Assembly? LoadFromAssemblySearchPaths(object? sender, ResolveEventArgs args)
        {
            Assembly? FinalAssembly = null;
            string? DepAssemblyName = new AssemblyName(args.Name).Name;
            string? ReqAssemblyName = args.RequestingAssembly is not null ? args.RequestingAssembly.GetName().Name : "An unknown assembly";
            DebugWriter.WriteDebug(DebugLevel.I, "{0} has requested to load {1}.", vars: [ReqAssemblyName, args.Name]);

            // Guard against the old version of Nitrocid KS (0.1.0 Milestone X or lower)
            if (DepAssemblyName == "Kernel Simulator")
                throw new KernelException(KernelExceptionType.OldModDetected);
            if (args.Name.Contains(".resources"))
                return FinalAssembly;

            // Helper function
            void AssignAsm(string path)
            {
                try
                {
                    // Check to see if the dependency file exists
                    if (!FilesystemTools.FileExists(path))
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "Assembly {0} doesn't exist...", vars: [path]);
                        return;
                    }

                    // Try loading
                    DebugWriter.WriteDebug(DebugLevel.I, "Loading from {0}...", vars: [path]);
                    FinalAssembly = Assembly.LoadFrom(path);
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to load {0} from {1}: {2}", vars: [args.Name, path, ex.Message]);
                    DebugWriter.WriteDebugStackTrace(ex);
                }
            }

            // Try to load assembly from lookup path
            var paths = baseAssemblyLookupPaths.Union(AssemblyLookupPaths).ToArray();
            foreach (string LookupPath in paths)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Lookup Path: {0}", vars: [LookupPath]);
                string DepAssemblyFilePath = Path.Combine(LookupPath, DepAssemblyName + ".dll");
                AssignAsm(DepAssemblyFilePath);
            }
            if (FinalAssembly is null)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Using Nitrocid execuable path {0} for lookup", vars: [PathsManagement.ExecPath]);
                string DepAssemblyFilePath = Path.Combine(PathsManagement.ExecPath, DepAssemblyName + ".dll");
                AssignAsm(DepAssemblyFilePath);
            }

            // Get the final assembly
            return FinalAssembly;
        }
    }
}
