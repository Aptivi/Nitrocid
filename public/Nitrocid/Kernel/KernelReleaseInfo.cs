//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

    }
}
