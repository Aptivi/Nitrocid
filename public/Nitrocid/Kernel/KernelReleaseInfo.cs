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

using Terminaux.Colors.Themes.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Kernel.Time;
using Nitrocid.Kernel.Time.Renderers;
using Nitrocid.Languages;
using System;

namespace Nitrocid.Kernel
{
    internal static class KernelReleaseInfo
    {
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
        internal readonly static string ConsoleTitle = $"Nitrocid v{KernelMain.VersionFullStr} (API v{KernelMain.ApiVersion})"
#if !SPECIFIERREL
                                    + $" - {ReleaseSpecifier}"
#endif
        ;

        // Release support window info
        internal readonly static DateTime supportWindow = new(2025, 11, 27);
        internal readonly static bool supportWindowPrimed =
#if SPECIFIERREL
            // This needs to stay false for now until we announce the next mod API version that 0.2.0 uses.
            false;
#else
            false;
#endif

        internal static void NotifyReleaseSupportWindow()
        {
            // Don't do anything if not primed yet
            if (!supportWindowPrimed)
                return;

            // Check to see if we're close to end of support window
            var currentDate = TimeDateTools.KernelDateTime.Date;
            var supportWindowWarn = supportWindow.Subtract(new TimeSpan(30, 0, 0, 0));
            if (currentDate >= supportWindowWarn && currentDate < supportWindow)
                TextWriters.Write("* " + LanguageTools.GetLocalized("NKS_KERNEL_RELEASEINFO_EOLWARNING") + $": {TimeDateRenderers.RenderDate(supportWindow)}. " + LanguageTools.GetLocalized("NKS_KERNEL_RELEASEINFO_EOLTIP"), ThemeColorType.Warning);
            else if (currentDate >= supportWindow)
                TextWriters.Write("* " + LanguageTools.GetLocalized("NKS_KERNEL_RELEASEINFO_EOL") + " " + LanguageTools.GetLocalized("NKS_KERNEL_RELEASEINFO_EOLTIP"), ThemeColorType.Warning);
        }
    }
}
