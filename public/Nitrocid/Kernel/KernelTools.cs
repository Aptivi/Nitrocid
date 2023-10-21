
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
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.Kernel.Exceptions;
using KS.Languages;
using System;
using System.Diagnostics;
using System.Reflection;

namespace KS.Kernel
{
    /// <summary>
    /// Kernel tools module
    /// </summary>
    public static class KernelTools
    {

        internal static Stopwatch StageTimer = new();
        private static readonly Version kernelVersion =
            Assembly.GetExecutingAssembly().GetName().Version;
        private static readonly Version kernelApiVersion =
            new(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion);

        /// <summary>
        /// Kernel version
        /// </summary>
        public static Version KernelVersion =>
            kernelVersion;
        /// <summary>
        /// Kernel API version
        /// </summary>
        // Refer to NitrocidModAPIVersion in the project file.
        public static Version KernelApiVersion =>
            kernelApiVersion;

        /// <summary>
        /// Check to see if KernelError has been called
        /// </summary>
        internal static void CheckErrored()
        {
            if (KernelPanic.KernelErrored)
            {
                KernelPanic.KernelErrored = false;
                var exception = KernelPanic.LastKernelErrorException;
                throw new KernelErrorException(Translate.DoTranslation("Kernel Error while booting: {0}"), exception, exception.Message);
            }
        }

    }
}
