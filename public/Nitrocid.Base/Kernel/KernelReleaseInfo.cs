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

using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using System;
using Nitrocid.Base.Kernel.Time;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Kernel.Time.Renderers;
using System.Reflection;
using Textify.Versioning;
using System.Diagnostics;

namespace Nitrocid.Base.Kernel
{
    /// <summary>
    /// Kernel release info
    /// </summary>
    public static class KernelReleaseInfo
    {
        internal static readonly string rootNameSpace =
            (typeof(KernelReleaseInfo).Namespace?.Split('.')[0]) ?? "";
        private static readonly Version? kernelVersion =
            Assembly.GetExecutingAssembly().GetName().Version;
        private static readonly SemVer? kernelVersionFull =
            SemVer.ParseWithRev($"{kernelVersion}");

        // Refer to NitrocidModAPIVersion in the project file.
        private static readonly Version kernelApiVersion =
            new(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion ?? "0.0.0.0");

        // Release specifiers (SPECIFIER: REL, DEV, ALPHA, BETA, or RC | None satisfied: Unsupported Release)
        internal readonly static string ReleaseSpecifier = ""
#if !SPECIFIERREL
#if SPECIFIERALPHA
                                    + "Alpha"
#elif SPECIFIERBETA
                                    + "Beta"
#elif SPECIFIERRC
                                    + "Release Candidate"
#elif SPECIFIERDEV
                                    + "Developer Preview"
#else
                                    + "UNSUPPORTED"
#endif // SPECIFIERALPHA
#endif // !SPECIFIERREL
        ;

        // Final console window title
        internal readonly static string ConsoleTitle = $"Nitrocid v{VersionFullStr} (API v{ApiVersion})"
#if !SPECIFIERREL
                                    + $" - {ReleaseSpecifier}"
#endif
        ;

        // Release support window info
        internal readonly static DateTime supportWindow = new(2036, 2, 12);
        internal readonly static bool supportWindowPrimed =
#if SPECIFIERREL
            true;
#else
            false;
#endif

        /// <summary>
        /// Kernel version
        /// </summary>
        public static Version? Version =>
            kernelVersion;
        /// <summary>
        /// Kernel API version
        /// </summary>
        public static Version ApiVersion =>
            kernelApiVersion;
        /// <summary>
        /// Kernel version (full)
        /// </summary>
        public static SemVer? VersionFull =>
            kernelVersionFull;
        /// <summary>
        /// Kernel version (full)
        /// </summary>
        public static string VersionFullStr =>
            kernelVersionFull?.ToString() ?? "0.0.0.0";

        internal static void NotifyReleaseSupportWindow()
        {
            // Don't do anything if not primed yet
            if (!supportWindowPrimed)
                return;

            // Check to see if we're close to end of support window
            var currentDate = TimeDateTools.KernelDateTime.Date;
            var supportWindowWarn = supportWindow.Subtract(new TimeSpan(30, 0, 0, 0));
            if (currentDate >= supportWindowWarn && currentDate < supportWindow)
                TextWriterColor.Write("* " + LanguageTools.GetLocalized("NKS_KERNEL_RELEASEINFO_EOLWARNING") + $": {TimeDateRenderers.RenderDate(supportWindow)}. " + LanguageTools.GetLocalized("NKS_KERNEL_RELEASEINFO_EOLTIP"), ThemeColorType.Warning);
            else if (currentDate >= supportWindow)
                TextWriterColor.Write("* " + LanguageTools.GetLocalized("NKS_KERNEL_RELEASEINFO_EOL") + " " + LanguageTools.GetLocalized("NKS_KERNEL_RELEASEINFO_EOLTIP"), ThemeColorType.Warning);
        }
    }
}
