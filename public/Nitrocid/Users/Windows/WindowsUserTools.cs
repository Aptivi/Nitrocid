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

using Nitrocid.Kernel;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace Nitrocid.Users.Windows
{
    /// <summary>
    /// User tools for Windows
    /// </summary>
    public static class WindowsUserTools
    {
        /// <summary>
        /// Checks to see if the current user is an administrator
        /// </summary>
        public static bool IsAdministrator()
        {
            if (KernelPlatform.IsOnWindows() ||

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
