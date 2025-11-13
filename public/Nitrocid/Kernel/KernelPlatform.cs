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

using SpecProbe.Software.Platform;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Nitrocid.Kernel
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
            return entryAssemblyName == executingAssembly.FullName && executingAssembly.FullName == expectedAssembly.FullName;
        }

        /// <summary>
        /// Checks to see if the kernel is being run on the test host (checks against Nitrocid.Tests as entry)
        /// </summary>
        public static bool IsOnTestHost()
        {
            string entryAssemblyName = Assembly.GetEntryAssembly()?.FullName ?? "";
            return !IsOnUsualEnvironment() && entryAssemblyName.Contains("Nitrocid.Tests");
        }

        /// <summary>
        /// Is this system a Windows system?
        /// </summary>
        /// <returns>True if running on Windows (Windows 10, Windows 11, etc.). Otherwise, false.</returns>
        public static bool IsOnWindows() =>
            PlatformHelper.IsOnWindows();

        /// <summary>
        /// Is this system a Unix system? True for macOS, too!
        /// </summary>
        /// <returns>True if running on Unix (Linux, *nix, etc.). Otherwise, false.</returns>
        public static bool IsOnUnix() =>
            PlatformHelper.IsOnUnix();

        /// <summary>
        /// Is this system a Unix system that contains musl libc?
        /// </summary>
        /// <returns>True if running on Unix systems that use musl libc. Otherwise, false.</returns>
        public static bool IsOnUnixMusl() =>
            PlatformHelper.IsOnUnixMusl();

        /// <summary>
        /// Is this system a macOS system?
        /// </summary>
        /// <returns>True if running on macOS (MacBook, iMac, etc.). Otherwise, false.</returns>
        public static bool IsOnMacOS() =>
            PlatformHelper.IsOnMacOS();

        /// <summary>
        /// Is this system an Android system?
        /// </summary>
        /// <returns>True if running on Android phones using Termux. Otherwise, false.</returns>
        public static bool IsOnAndroid() =>
            PlatformHelper.IsOnAndroid();

        /// <summary>
        /// Polls $TERM_PROGRAM to get terminal emulator
        /// </summary>
        public static string GetTerminalEmulator() =>
            PlatformHelper.GetTerminalEmulator();

        /// <summary>
        /// Polls $TERM to get terminal type (vt100, dumb, ...)
        /// </summary>
        public static string GetTerminalType() =>
            PlatformHelper.GetTerminalType();

        /// <summary>
        /// Is Nitrocid KS running from TMUX?
        /// </summary>
        public static bool IsRunningFromTmux() =>
            PlatformHelper.IsRunningFromTmux();

        /// <summary>
        /// Is Nitrocid KS running from GNU Screen?
        /// </summary>
        public static bool IsRunningFromScreen() =>
            PlatformHelper.IsRunningFromScreen();

        /// <summary>
        /// Gets the current runtime identifier
        /// </summary>
        /// <returns>Returns a runtime identifier (win-x64 for example).</returns>
        public static string GetCurrentRid() =>
            RuntimeInformation.RuntimeIdentifier;

        /// <summary>
        /// Gets the current runtime identifier
        /// </summary>
        /// <returns>Returns a runtime identifier (win-x64 for example).</returns>
        public static string GetCurrentGenericRid() =>
            PlatformHelper.GetCurrentGenericRid();

    }
}
