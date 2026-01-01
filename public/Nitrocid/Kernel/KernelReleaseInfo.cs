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

using Nitrocid.ConsoleBase.Colors;
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
        internal readonly static DateTime supportWindow = new(2025, 4, 23);
        internal readonly static bool supportWindowPrimed =
#if SPECIFIERREL
            true;
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
                TextWriters.Write("* " + Translate.DoTranslation("We'll no longer support this version of Nitrocid KS after this date") + $": {TimeDateRenderers.RenderDate(supportWindow)}. " + Translate.DoTranslation("Make sure that you upgrade to the supported version soon if you want to continue receiving support."), KernelColorType.Warning);
            else if (currentDate >= supportWindow)
                TextWriters.Write("* " + Translate.DoTranslation("This version of Nitrocid KS is no longer supported.") + " " + Translate.DoTranslation("Make sure that you upgrade to the supported version soon if you want to continue receiving support."), KernelColorType.Warning);
        }
    }
}
