//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using SpecProbe.Software.Platform;

namespace Nitrocid.Base.Kernel
{
    /// <summary>
    /// Kernel platform query
    /// </summary>
    public static class KernelPlatform
    {
        /// <summary>
        /// Checks to see if the kernel is running normally or from somewhere else
        /// </summary>
        public static bool IsOnUsualEnvironment()
        {
            string entryAssemblyName = Assembly.GetEntryAssembly()?.FullName ?? "";
            var executingAssembly = Assembly.GetExecutingAssembly();
            var expectedAssembly = typeof(KernelPlatform).Assembly;
            return entryAssemblyName != executingAssembly.FullName && executingAssembly.FullName == expectedAssembly.FullName;
        }

        /// <summary>
        /// Checks to see if the kernel is being run on the test host (checks against Nitrocid.Tests as entry)
        /// </summary>
        public static bool IsOnTestHost()
        {
            string entryAssemblyName = Assembly.GetEntryAssembly()?.FullName ?? "";
            return IsOnUsualEnvironment() && entryAssemblyName.Contains("Nitrocid.Tests");
        }

        /// <summary>
        /// Gets the current runtime identifier
        /// </summary>
        /// <returns>Returns a runtime identifier (win-x64 for example).</returns>
        public static string GetCurrentRid() =>
            RuntimeInformation.RuntimeIdentifier;

        /// <summary>
        /// Checks to see if the current Windows user is an administrator
        /// </summary>
        public static bool IsCurrentWindowsUserAdmin()
        {
            if (PlatformHelper.IsOnWindows() ||

                // This is a trick to avoid compiler warnings, since IsOnWindows() above doesn't seem to avoid compiler warnings.
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var identity = WindowsIdentity.GetCurrent();
                var principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            else
                // Assume that the user is admin for other systems.
                return true;
        }
    }
}
