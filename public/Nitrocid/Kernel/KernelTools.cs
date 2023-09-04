
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
        internal static string bannerFigletFont = "Banner";
        private static readonly Version kernelVersion =
            Assembly.GetExecutingAssembly().GetName().Version;
        private static readonly Version kernelApiVersion =
            new(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion);

        // #ifdef'd variables...
        // Release specifiers (SPECIFIER: REL, RC, or DEV | MILESTONESPECIFIER: ALPHA, BETA, or NONE | None satisfied: Unsupported Release)
#if SPECIFIERREL
        internal readonly static string ReleaseSpecifier = $"Final";
#elif SPECIFIERRC
        internal readonly static string ReleaseSpecifier = $"Release Candidate";
#elif SPECIFIERDEV
#if MILESTONESPECIFIERALPHA
        internal readonly static string ReleaseSpecifier = $"Alpha 1";
#elif MILESTONESPECIFIERBETA
        internal readonly static string ReleaseSpecifier = $"Beta 2";
#else
        internal readonly static string ReleaseSpecifier = $"Developer Preview";
#endif
#else
        internal readonly static string ReleaseSpecifier = $"- UNSUPPORTED -";
#endif

        // Final console window title
#if SPECIFIERREL
        internal readonly static string ConsoleTitle = $"Nitrocid v{KernelVersion} (API v{KernelApiVersion})";
#else
        internal readonly static string ConsoleTitle = $"Nitrocid v{KernelVersion} {ReleaseSpecifier} (API v{KernelApiVersion})";
#endif

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
            if (Flags.KernelErrored)
            {
                Flags.KernelErrored = false;
                throw new KernelErrorException(Translate.DoTranslation("Kernel Error while booting: {0}"), KernelPanic.LastKernelErrorException, KernelPanic.LastKernelErrorException.Message);
            }
        }

    }
}
