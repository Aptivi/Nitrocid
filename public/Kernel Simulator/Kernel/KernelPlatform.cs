
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using UnameNET;

namespace KS.Kernel
{
    /// <summary>
    /// Kernel platform query
    /// </summary>
    public static class KernelPlatform
    {

        /// <summary>
        /// Is this system a Windows system?
        /// </summary>
        public static bool IsOnWindows() => Environment.OSVersion.Platform == PlatformID.Win32NT;

        /// <summary>
        /// Is this system a Unix system? True for macOS, too!
        /// </summary>
        public static bool IsOnUnix() => Environment.OSVersion.Platform == PlatformID.Unix;

        /// <summary>
        /// Is this system a macOS system?
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "We already have IsOnUnix() to do the job, so it returns false when Windows is detected.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = ".NET Framework doesn't have CA1416.")]
        public static bool IsOnMacOS()
        {
            if (IsOnUnix())
            {
                string System = UnameManager.GetUname(UnameTypes.KernelName);
                return System.Contains("Darwin");
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Polls $TERM_PROGRAM to get terminal emulator
        /// </summary>
        public static string GetTerminalEmulator() => Environment.GetEnvironmentVariable("TERM_PROGRAM") ?? "";

        /// <summary>
        /// Polls $TERM to get terminal type (vt100, dumb, ...)
        /// </summary>
        public static string GetTerminalType() => Environment.GetEnvironmentVariable("TERM") ?? "";

        /// <summary>
        /// Is Kernel Simulator running from GRILO?
        /// </summary>
        public static bool IsRunningFromGrilo() => (System.Reflection.Assembly.GetEntryAssembly()?.GetName()?.Name?.StartsWith("GRILO")) ?? false;

        /// <summary>
        /// Is Kernel Simulator running from TMUX?
        /// </summary>
        public static bool IsRunningFromTmux() => Environment.GetEnvironmentVariable("TMUX") is not null;

    }
}
