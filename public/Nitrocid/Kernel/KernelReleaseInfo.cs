//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

namespace KS.Kernel
{
    internal static class KernelReleaseInfo
    {

        // Release specifiers (SPECIFIER: REL or DEV | MILESTONESPECIFIER: ALPHA, BETA, RC, or NONE | None satisfied: Unsupported Release)
        internal readonly static string ReleaseSpecifier = ""
#if SPECIFIERDEV
#if MILESTONESPECIFIERALPHA
                                    + "Alpha 1"
#elif MILESTONESPECIFIERBETA
                                    + "Beta 2"
#elif MILESTONESPECIFIERRC
                                    + "Release Candidate"
#else
                                    + "Developer Preview"
#endif // MILESTONESPECIFIERALPHA
#elif !SPECIFIERREL
                                    + "- UNSUPPORTED -"
#endif // SPECIFIERDEV
        ;

        // Final console window title
        internal readonly static string ConsoleTitle = $"Nitrocid v{KernelTools.KernelVersion} (API v{KernelTools.KernelApiVersion})"
#if !SPECIFIERREL
                                    + $" - {ReleaseSpecifier}"
#endif
        ;

    }
}
